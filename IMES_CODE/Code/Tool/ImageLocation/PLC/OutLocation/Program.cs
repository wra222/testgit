using System;
using System.IO;
using System.Net.Sockets;

namespace OutLocation
{
    class Program
    {
        static TcpClient client = null;
        static StreamReader receiver = null;
        static StreamWriter sender = null;

        static int Main(string[] args)
        {
            try
            {
                if (args.Length < 2)
                {
                    return -5;
                }
                ushort shelfNo = ushort.Parse(args[0]);
                ushort result = ushort.Parse(args[1]);

                bool ret = false;
                char[] sendBuffer = new char[]
	                                        {
		                                       (char)0x50, (char)0x00, (char)0x00, (char)0xFF, (char)0xFF, (char)0x03, (char)0x00,
		                                        (char)0x12,										// Packet Data Length Low
		                                        (char)0x00,										// Packet Data Length Hi
		                                        (char)0x10, (char)0x00,
		                                        (char)0x01, (char)0x14,									// Sub Command
		                                        (char)0x00, (char)0x00,
		                                        (char)0x4C, (char)0x04, (char)0x00,							// D00044C(D1100):ShelfNo of ExitWork
													                                                                            // D00044E(D1102):Result of ExitWork
		                                        (char)0xA8,										                        // D reg
		                                        (char)0x03,										                        // Length Low
		                                        (char)0x00,										                        // Length Hi
		                                        (char)0x00, (char)0x00,									// Change Data D1100
		                                        (char)0x00, (char)0x00,									// Change Data D1101
		                                        (char)0x00, (char)0x00									// Change Data D1102
	                                        };



                // ソケット接続
                ret = SocketConnect("192.168.0.10", 8192);
                if (!ret)
                {
                    return -1;
                }

                char hidata = (char)(shelfNo >> 8);
                char lowdata = (char)shelfNo;
                sendBuffer[21] = lowdata;
                sendBuffer[22] = hidata;

                ushort work_result;
                if (result == 1)
                {				// OK	
                    work_result = 0x05FE;
                }
                else
                {							// NG
                    work_result = 0x05FF;
                }
                hidata = (char)(work_result >> 8);
                lowdata = (char)work_result;
                sendBuffer[25] = (char)lowdata;
                sendBuffer[26] = (char)hidata;

                // ソケット?信
                ret = SocketSend(sendBuffer, 27);
                if (!ret)
                {
                    return -2;
                }

                // ソケット受信

                char[] receiveBuffer = new char[1024];
                for (int i = 0; i < 1024; ++i)
                {
                    receiveBuffer[i] = (char)0x00;
                }

                int receiveSize;
                ret = SocketRecv(receiveBuffer, 1024, out receiveSize);
                if (!ret)
                {
                    return -3;
                }

                // ソケット切断
                client.Close();                

                return 0;
            }
            catch (Exception)
            {
                return -4;
            }
        }

        static bool SocketConnect(string ip, int portNo)
        {
            try
            {
                client = new TcpClient(ip, portNo);
                client.NoDelay = true;
                Stream stream = client.GetStream();
                receiver = new StreamReader(stream);
                sender = new StreamWriter(stream);
                sender.AutoFlush = true;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        static bool SocketSend(char[] buffer, int size)
        {
            try
            {
                sender.Write(buffer, 0, size);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        static bool SocketRecv(char[] buffer, int size, out int receivedSize)
        {
            try
            {
                receivedSize = receiver.Read(buffer, 0, size);
                receiver.DiscardBufferedData();
                return true;
            }
            catch (Exception)
            {
                receivedSize = 0;
                return false;
            }
        }
    }
}
