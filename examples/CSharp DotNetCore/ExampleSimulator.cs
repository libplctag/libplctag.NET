using libplctag;
using libplctag.DataTypes;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace CSharp_DotNetCore
{
    internal class ExampleSimulator
    {
        public static void RunSimulation()
        {

            // This shows how to provide two separate implementations for communication with a PLC.
            //    1. The "real" implementation, that implements communication with the PLC.
            //    2. A simulation, that provides the same interface, but does not communicate with the PLC.


            // Your architecture may differ from this. For example, your application may be closer to a SCADA
            // application, and exposes the concept of a PLC Tag to the end user. In that case, create an 
            // interface for a Tag, and expose that instead.

            
            // The example here shows a Turnstile gate that is used to count the number of people that
            // attend a FIFA World Cup game.
            //
            // You would see Turnstile gates at Sports Stadiums, Train Stations, and other public places
            // where monitoring the flow of people is needed.
            // 
            // This turnstile gate is remotely monitored, and has a PLC that records the counter
            // information and can send it across the network to a HMI.
            // 
            // At the end of the match, the administration staff will reset
            // the counter, ready for the next match.


            ITurnstileGate turnstileGate = new Simulation();
            //ITurnstileGate turnstileGate = new RealImplementation();  // When running against the real device, we simply change to the RealImplementation.



            // The feedback thread: twice a second it prints the latest count to screen.
            var feedbackThread = Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(500);
                    Console.WriteLine($"{turnstileGate.Counter} people have passed through the gate");
                }
                
            });

            // The user input thread waits for the user to press "Enter", and then makes
            // a request to recieve the latest data from the PLC.
            var userInputThread = Task.Run(async () =>
            {
                Console.WriteLine("Press Enter to refresh the data from the remote device\n");

                while (true)
                {
                    Console.ReadLine();
                    await turnstileGate.ReceiveData();
                }
            });

            Task.WaitAll(feedbackThread, userInputThread);
        }


        private interface ITurnstileGate
        {
            /// <summary>
            /// Get the data from the PLC into HMI memory.
            /// </summary>
            Task ReceiveData();

            /// <summary>
            /// The most recent data recieved from the Turnstile Gate PLC regarding the counter.
            /// </summary>
            /// <remarks>
            /// Note: This value will _not_ be automatically updated, instead we must call <see cref="ReceiveData"/> to update it.
            /// </remarks>
            int Counter { get; }

            /// <summary>
            /// Immediately sends a message to the Turnstile Gate PLC to reset the counter.
            /// </summary>
            Task ResetCounter();
        }




        /// <summary>
        /// Provides a real implementation. Would be used when accessing the real device.
        /// </summary>
        private class RealImplementation : ITurnstileGate
        {
            private readonly Tag<DintPlcMapper, int> CounterTag;
            private readonly Tag<BoolPlcMapper, bool> ResetTag;
            public RealImplementation()
            {
                CounterTag = new Tag<DintPlcMapper, int>()
                {
                    PlcType = PlcType.ControlLogix,
                    Path = "0,1",
                    Name = "PEOPLE_COUNTER",
                    Gateway = "10.10.10.10",
                    Protocol = Protocol.ab_eip,
                };

                ResetTag = new Tag<BoolPlcMapper, bool>()
                {
                    PlcType = PlcType.ControlLogix,
                    Path = "0,1",
                    Name = "PEOPLE_COUNTER_RESET_FLAG",
                    Gateway = "10.10.10.10",
                    Protocol = Protocol.ab_eip
                };
            }

            public int Counter { get; internal set; }

            public async Task ResetCounter()
            {
                // The PLC is expected to have some logic that sets the ResetFlag from true back to false after resetting the counter
                await ResetTag.WriteAsync(true);
                await ReceiveData();
            }

            public async Task ReceiveData()
            {
                Counter = await CounterTag.ReadAsync();
            }
        }






        /// <summary>
        /// This class provides an in-memory simulation of the TurnstileGate 
        /// </summary>
        /// <remarks>
        /// Note that access to liveCounter is not thread-safe - but maybe it's good enough for a simulation??
        /// </remarks>
        private class Simulation : ITurnstileGate
        {
            private readonly Timer plcScan;
            private int liveCounter;

            public Simulation()
            {
                // Every 1000ms it will randomly increment the counter by 0, 1 or 2
                plcScan = new Timer()
                {
                    AutoReset = true,
                    Interval = 1000
                };
                plcScan.Elapsed += (s, e) =>
                {
                    liveCounter += Random.Shared.Next(0, 2);
                };
                plcScan.Start();
            }

            public int Counter { get; internal set; }

            public async Task ResetCounter()
            {
                await SimulateNetworkRequest();
                liveCounter = 0;
                Counter = liveCounter;
            }

            public async Task ReceiveData()
            {
                await SimulateNetworkRequest();
                Counter = liveCounter;
            }

            private async Task SimulateNetworkRequest()
            {
                // A random delay between 200ms and 1000ms
                await Task.Delay(Random.Shared.Next(200, 1000));
            }
        }
    }
}
