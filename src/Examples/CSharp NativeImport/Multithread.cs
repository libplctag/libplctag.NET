using libplctag.NativeImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NativeImport_Examples
{
    class Multithread
    {
        const string TAG_PATH = "protocol=ab_eip&gateway=192.168.0.10&path=1,0&plc=LGX&elem_size=4&elem_count=1&name=MY_DINT";
        const int ELEM_COUNT = 1;
        const int ELEM_SIZE = 4;
        const int DATA_TIMEOUT = 2000;
        const int MAX_THREADS = 300;

        static int tag;
        static bool done = false;

        static void thread_func(object data)
        {
            {
                int tid = (int)data;
                int rc;
                int value;

                while (!done)
                {
                    DateTime start;
                    DateTime end;

                    /* capture the starting time */
                    start = DateTime.Now;

                    /* use do/while to allow easy exit without return */
                    do
                    {
                        Console.WriteLine($"Locking in {tid}");
                        rc = plctag.plc_tag_lock(tag);

                        if (rc != (int)STATUS_CODES.PLCTAG_STATUS_OK)
                        {
                            value = 1000;
                            break; /* punt, no lock */
                        }

                        Console.WriteLine($"Reading in {tid}");
                        rc = plctag.plc_tag_read(tag, DATA_TIMEOUT);

                        if (rc != (int)STATUS_CODES.PLCTAG_STATUS_OK)
                        {
                            value = 1001;
                        }
                        else
                        {
                            value = (int)plctag.plc_tag_get_int32(tag, 0);

                            /* increment the value */
                            value = (value > 500 ? 0 : value + 1);

                            /* yes, we should be checking this return value too... */
                            plctag.plc_tag_set_int32(tag, 0, (int)value);

                            /* write the value */
                            rc = plctag.plc_tag_write(tag, DATA_TIMEOUT);
                        }

                        Console.WriteLine($"Unlocking in {tid}");
                        /* yes, we should look at the return value */
                        plctag.plc_tag_unlock(tag);

                    } while (false);

                    end = DateTime.Now;

                    Console.WriteLine($"Thread {tid} got result {value} with return code {plctag.plc_tag_decode_error(rc)} in {end-start}");

                    Thread.Sleep(1);
                }

                return;
            }
        }

        static public int main()
        {
            int rc = (int)STATUS_CODES.PLCTAG_STATUS_OK;
            Thread[] thread = new Thread[MAX_THREADS];
            int num_threads;
            int thread_id = 0;

            /* check the library version. */
            if (plctag.plc_tag_check_lib_version(2,1,0) != (int)STATUS_CODES.PLCTAG_STATUS_OK)
            {
                Console.WriteLine("Required compatible library version 2.1.0 not available!");
                return 1;
            }

            //if (argc != 2)
            //{
            //    fprintf(stderr, "ERROR: Must provide number of threads to run (between 1 and 300) argc=%d!\n", argc);
            //    return 0;
            //}

            num_threads = 10;
            //num_threads = (int)strtol(argv[1], NULL, 10);

            //if (num_threads < 1 || num_threads > MAX_THREADS)
            //{
            //    fprintf(stderr, "ERROR: %d (%s) is not a valid number. Must provide number of threads to run (between 1 and 300)!\n", num_threads, argv[1]);
            //    return 0;
            //}

            /* create the tag */
            tag = plctag.plc_tag_create(TAG_PATH, DATA_TIMEOUT);

            /* everything OK? */
            if (tag < 0)
            {
                Console.WriteLine($"ERROR {plctag.plc_tag_decode_error(tag)}: Could not create tag!");
                return 0;
            }

            if ((rc = plctag.plc_tag_status(tag)) != (int)STATUS_CODES.PLCTAG_STATUS_OK)
            {
                Console.WriteLine($"Error setting up tag internal state. {plctag.plc_tag_decode_error(rc)}");
                plctag.plc_tag_destroy(tag);
                return 0;
            }

            /* create the read threads */

            Console.WriteLine($"Creating {num_threads} threads.");
            for (thread_id = 0; thread_id < num_threads; thread_id++)
            {
                thread[thread_id] = new Thread(new ParameterizedThreadStart(thread_func));
                thread[thread_id].Start(thread_id);
            }

            /* wait until ^C */
            while (true)
            {
                Thread.Sleep(100);
            }

            done = true;

            for (thread_id = 0; thread_id < num_threads; thread_id++)
            {
                thread[thread_id].Join();
            }

            plctag.plc_tag_destroy(tag);

            return 0;
        }
    }
}
