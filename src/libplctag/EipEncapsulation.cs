/*###################################################################################################*/
/*#                                                                                                 #*/
/*# Class-Name   : EipEncapsulation                                                                 #*/
/*#                                                                                                 #*/
/*# Derived from : none                                                                             #*/
/*#                                                                                                 #*/
/*# File-Name    : EipEncapsulation.cs                                                              #*/
/*#                                                                                                 #*/
/*# Author       : originally by Rossmann Engineering and modified by Jochen Haar                   #*/
/*#                                                                                                 #*/
/*# Date         : 14.03.2021 with respect to the modifications - origin date is unknown            #*/
/*#                                                                                                 #*/
/*# Tool/Compiler: Visual Studio 2019                                                               #*/
/*#                                                                                                 #*/
/*# Exceptions   : none                                                                             #*/
/*#                                                                                                 #*/
/*#=================================================================================================#*/
/*#                                                                                                 #*/
/*# Introduction : Class to create the Encapsulation header and the Common Packet Format for        #*/
/*#                Ethernet/IP - Common Industrial Protocol (CIP) - message frames                  #*/
/*#                                                                                                 #*/
/*#=================================================================================================#*/
/*# Change report                                                                                   #*/
/*#----------+-------------------+------------------------------------------------------------------#*/
/*# Date     | Name              | description of the reason for changes                            #*/
/*#----------+-------------------+------------------------------------------------------------------#*/
/*#          |                   |                                                                  #*/
/*#----------+-------------------+------------------------------------------------------------------#*/
/*###################################################################################################*/

using System;
using System.Collections.Generic;

//-----------------------------------------------------------------------------------------------------
namespace IIoT_EipDeviceScanner
//-----------------------------------------------------------------------------------------------------
{
    //-------------------------------------------------------------------------------------------------
    public class EipEncapsulation
    //-------------------------------------------------------------------------------------------------
    {
        public CommandsEnum Command { get; set; }
        public UInt16 Length { get; set; }
        public UInt32 SessionHandle { get; set; }
        public StatusEnum Status { get; }
        private byte[] SenderContext = new byte[8];
        private UInt32 Options = 0;
        public List<byte> CommandSpecificData = new List<byte>();

        /// <summary>
        /// Table 2-3.3 Error Codes
        /// </summary>
        //---------------------------------------------------------------------------------------------
        public enum StatusEnum : UInt32
        //---------------------------------------------------------------------------------------------
        {
            Success = 0x0000,
            InvalidCommand = 0x0001,
            InsufficientMemory = 0x0002,
            IncorrectData = 0x0003,
            InvalidSessionHandle = 0x0064,
            InvalidLength = 0x0065,
            UnsupportedEncapsulationProtocol = 0x0069
        }

        /// <summary>
        /// Table 2-3.2 Encapsulation Commands
        /// </summary>
        //---------------------------------------------------------------------------------------------
        public enum CommandsEnum : UInt16
        //---------------------------------------------------------------------------------------------
        {
            NOP = 0x0000,
            ListServices = 0x0004,
            ListIdentity = 0x0063,
            ListInterfaces = 0x0064,
            RegisterSession = 0x0065,
            UnregisterSession = 0x0066,
            SendRRData = 0x006F,
            SendUnitData = 0x0070,
            IndicateStatus = 0x0072,
            Cancel = 0x0073
        }

        //---------------------------------------------------------------------------------------------
        public byte[] ToBytes()
        //---------------------------------------------------------------------------------------------
        {
            byte[] returnValue = new byte[24 + CommandSpecificData.Count];

            returnValue[0] = (byte)this.Command;
            returnValue[1] = (byte)((UInt16)this.Command >> 8);
            returnValue[2] = (byte)this.Length;
            returnValue[3] = (byte)((UInt16)this.Length >> 8);
            returnValue[4] = (byte)this.SessionHandle;
            returnValue[5] = (byte)((UInt32)this.SessionHandle >> 8);
            returnValue[6] = (byte)((UInt32)this.SessionHandle >> 16);
            returnValue[7] = (byte)((UInt32)this.SessionHandle >> 24);
            returnValue[8] = (byte)this.Status;
            returnValue[9] = (byte)((UInt16)this.Status >> 8);
            returnValue[10] = (byte)((UInt16)this.Status >> 16);
            returnValue[11] = (byte)((UInt16)this.Status >> 24);
            returnValue[12] = SenderContext[0];
            returnValue[13] = SenderContext[1];
            returnValue[14] = SenderContext[2];
            returnValue[15] = SenderContext[3];
            returnValue[16] = SenderContext[4];
            returnValue[17] = SenderContext[5];
            returnValue[18] = SenderContext[6];
            returnValue[19] = SenderContext[7];
            returnValue[20] = (byte)this.Options;
            returnValue[21] = (byte)((UInt16)this.Options >> 8);
            returnValue[22] = (byte)((UInt16)this.Options >> 16);
            returnValue[23] = (byte)((UInt16)this.Options >> 24);

            for (int i = 0; i < CommandSpecificData.Count; i++) returnValue[24 + i] = CommandSpecificData[i];

            return returnValue;
        }

        /// <summary>
        /// Socket Address - see section 2-6.3.2
        /// </summary>
        //---------------------------------------------------------------------------------------------
        public class SocketAddress
        //---------------------------------------------------------------------------------------------
        {
            public UInt16 SIN_family;
            public UInt16 SIN_port;
            public UInt32 SIN_Address;
            public byte[] SIN_Zero = new byte[8];
        }

        //---------------------------------------------------------------------------------------------
        public class CommonPacketFormat
        //---------------------------------------------------------------------------------------------
        {
            public UInt16 ItemCount = 2;
            public UInt16 AddressItem = 0x0000;
            public UInt16 AddressLength = 0;
            public UInt16 DataItem = 0xB2;  // 0xB2 = Unconnected Data Item
            public UInt16 DataLength = 8;
            public List<byte> Data = new List<byte>();
            public UInt16 SockaddrInfoItem_O_T = 0x8001;  // 8000 for O->T and 8001 for T->O - Volume 2 Table 2-6.9
            public UInt16 SockaddrInfoLength = 16;
            public SocketAddress SocketaddrInfo_O_T = null;

            //-----------------------------------------------------------------------------------------
            public byte[] ToBytes()
            //-----------------------------------------------------------------------------------------
            {
                if (SocketaddrInfo_O_T != null) ItemCount = 3;
                byte[] returnValue = new byte[10 + Data.Count + (SocketaddrInfo_O_T == null ? 0 : 20)];

                returnValue[0] = (byte)this.ItemCount;
                returnValue[1] = (byte)((UInt16)this.ItemCount >> 8);
                returnValue[2] = (byte)this.AddressItem;
                returnValue[3] = (byte)((UInt16)this.AddressItem >> 8);
                returnValue[4] = (byte)this.AddressLength;
                returnValue[5] = (byte)((UInt16)this.AddressLength >> 8);
                returnValue[6] = (byte)this.DataItem;
                returnValue[7] = (byte)((UInt16)this.DataItem >> 8);
                returnValue[8] = (byte)this.DataLength;
                returnValue[9] = (byte)((UInt16)this.DataLength >> 8);

                for (int i = 0; i < Data.Count; i++) returnValue[10 + i] = Data[i];

                // add Socket Address Info Item if requested
                if (SocketaddrInfo_O_T != null)
                {
                    returnValue[10 + Data.Count + 0] = (byte)this.SockaddrInfoItem_O_T;
                    returnValue[10 + Data.Count + 1] = (byte)((UInt16)this.SockaddrInfoItem_O_T >> 8);
                    returnValue[10 + Data.Count + 2] = (byte)this.SockaddrInfoLength;
                    returnValue[10 + Data.Count + 3] = (byte)((UInt16)this.SockaddrInfoLength >> 8);
                    returnValue[10 + Data.Count + 4] = (byte)((UInt16)this.SocketaddrInfo_O_T.SIN_family >> 8);
                    returnValue[10 + Data.Count + 5] = (byte)this.SocketaddrInfo_O_T.SIN_family;
                    returnValue[10 + Data.Count + 6] = (byte)((UInt16)this.SocketaddrInfo_O_T.SIN_port >> 8);
                    returnValue[10 + Data.Count + 7] = (byte)this.SocketaddrInfo_O_T.SIN_port;
                    returnValue[10 + Data.Count + 8] = (byte)((UInt32)this.SocketaddrInfo_O_T.SIN_Address >> 24);
                    returnValue[10 + Data.Count + 9] = (byte)((UInt32)this.SocketaddrInfo_O_T.SIN_Address >> 16);
                    returnValue[10 + Data.Count + 10] = (byte)((UInt32)this.SocketaddrInfo_O_T.SIN_Address >> 8);
                    returnValue[10 + Data.Count + 11] = (byte)this.SocketaddrInfo_O_T.SIN_Address;
                    returnValue[10 + Data.Count + 12] = this.SocketaddrInfo_O_T.SIN_Zero[0];
                    returnValue[10 + Data.Count + 13] = this.SocketaddrInfo_O_T.SIN_Zero[1];
                    returnValue[10 + Data.Count + 14] = this.SocketaddrInfo_O_T.SIN_Zero[2];
                    returnValue[10 + Data.Count + 15] = this.SocketaddrInfo_O_T.SIN_Zero[3];
                    returnValue[10 + Data.Count + 16] = this.SocketaddrInfo_O_T.SIN_Zero[4];
                    returnValue[10 + Data.Count + 17] = this.SocketaddrInfo_O_T.SIN_Zero[5];
                    returnValue[10 + Data.Count + 18] = this.SocketaddrInfo_O_T.SIN_Zero[6];
                    returnValue[10 + Data.Count + 19] = this.SocketaddrInfo_O_T.SIN_Zero[7];
                }

                return returnValue;
            }
        }
    }
}