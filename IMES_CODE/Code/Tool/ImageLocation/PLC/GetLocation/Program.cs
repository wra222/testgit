using System;
using System.IO;
using System.Net.Sockets;

namespace GetLocation
{
    class Program
    {
        static TcpClient client = null;
        static StreamReader receiver =null;
        static StreamWriter sender= null;

        static int Main(string[] args)
        {
            try
            {
                bool ret = false;
                char[] sendBuffer = new char[]
	                                        {
		                                        (char)0x50, (char)0x00, (char)0x00, (char)0xFF, (char)0xFF, (char)0x03, (char)0x00,
		                                        (char)0x0C,										// Packet Data Length Low
		                                        (char)0x00,										// Packet Data Length Hi
		                                        (char)0x10, (char)0x00,
		                                        (char)0x01, (char)0x04,									// Sub Command
                                                (char)0x00, (char)0x00,
                                                (char)0xE8, (char)0x03, (char)0x00,							// D0003E8(D1000):EntryShelfNo
                                                (char)0xA8,										// D reg
                                                (char)0x01,										// Length Low
		                                       (char) 0x00										// Length Hi
	                                        };



                // ソケット接続
                ret = SocketConnect("192.168.0.10", 8192);
                if (!ret)
                {
                    return -1;
                }

                // ソケット?信
                ret = SocketSend(sendBuffer, 21);
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

                char lowdata = receiveBuffer[11];
                char hidata = receiveBuffer[12];
                ushort wShelfNo = (ushort)(hidata << 8);
                wShelfNo = (ushort)(wShelfNo & 0xFF00);
                wShelfNo = (ushort)(wShelfNo | lowdata);

                return wShelfNo;
            }
            catch (Exception)
            {
                return -4;
            }
        }

        static  bool SocketConnect( string ip , int portNo )
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
            catch (Exception )
            {
                return false;
            }
         }

        static bool SocketSend(char[] buffer, int size )
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
