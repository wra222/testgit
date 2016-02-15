using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Net.Sockets;

namespace ImageConsole
{

	class Program
	{
		static TcpClient client = null;
		static StreamReader receiver = null;
		static StreamWriter sender = null;
		static int waittingtime = 2000;

		static NetworkStream networkstream;						//　★★★変更★★★

		/// <summary>
		/// 1=functionname:EntryShelfNo; CheckEntryShelfNo(★追加); SetEntryShelfNo(★追加); ClearEntryShelfNo(★追加); 
		///               :ExitWork; CheckExitWork(★追加); ClearExitWork(★追加); 
		///               :All_ShelfStatus
		/// 2=shelfno
		/// 3=downloadresult
		/// 4=shelfnostatus:0x101：Free.（無産品）;0x201：During.（投入中）;0x301：Downloading.（Download中;0x5FE：OK Complete.（OK完畢）;0x5FF：NG Complete.（NG完畢）;0x401：Exiting.（排出中）
		/// 5=Socketip, 
		/// 6=SocketportNo
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			try
			{
				if (args.Length < 6)
				{
					Console.WriteLine("Fail;Need param,Count={0} ", args.Length);
					Thread.Sleep(waittingtime);
					return;
				}

				string functionname = args[0];
				ushort shelfno = ushort.Parse(args[1]);
				string downloadresult = args[2];
				string shelfnostatus = args[3];
				string Socketip = args[4];
				int SocketportNo = Convert.ToInt16(args[5]);

				if (functionname.Trim() == "EntryShelfNo")
				{
					EntryShelfNo(Socketip, SocketportNo);
				}
				else if (functionname.Trim() == "CheckEntryShelfNo")
				{
					CheckEntryShelfNo(Socketip, SocketportNo);
				}
				else if (functionname.Trim() == "SetEntryShelfNo")
				{
                    SetEntryShelfNo(shelfno, Socketip, SocketportNo);
				}
				else if (functionname.Trim() == "ClearEntryShelfNo")
				{
					ClearEntryShelfNo(Socketip, SocketportNo);
				}
				else if (functionname.Trim() == "ExitWork")
				{
					ExitWork(shelfno, downloadresult, Socketip, SocketportNo);
				}
				else if (functionname.Trim() == "CheckExitWork")
				{
					CheckExitWork(Socketip, SocketportNo);
				}
				else if (functionname.Trim() == "ClearExitWork")
				{
					ClearExitWork(Socketip, SocketportNo);
				}
				else if (functionname.Trim() == "All_ShelfStatus")
				{
					ushort work_shelfStatus = new ushort();
					if (shelfnostatus == "Free")
					{
						work_shelfStatus = 0x101;
					}
					else if (shelfnostatus == "During")
					{
						work_shelfStatus = 0x201;
					}
					else if (shelfnostatus == "Downloading")
					{
						work_shelfStatus = 0x301;
					}
					else if (shelfnostatus == "OK Complete")
					{
						work_shelfStatus = 0x5FE;
					}
					else if (shelfnostatus == "NG Complete")
					{
						work_shelfStatus = 0x5FF;
					}
					else if (shelfnostatus == "Exiting")
					{
						work_shelfStatus = 0x401;
					}
					All_ShelfStatus(work_shelfStatus, Socketip, SocketportNo);
				}
				else
				{
					Console.WriteLine("Wrong Function Name ,System Support:EntryShelfNo;ExitWork;All_ShelfStatus");
					Thread.Sleep(waittingtime);
				}
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		// ソケット接続
		static bool SocketConnect(string ip, int portNo)
		{
			try
			{
				client = new TcpClient(ip, portNo);
				client.NoDelay = true;
//				Stream stream = client.GetStream();
				networkstream = client.GetStream();				//　★★★変更★★★

//				receiver = new StreamReader(stream);
//				sender = new StreamWriter(stream);
//				sender.AutoFlush = true;

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		// ソケット送信
		static bool SocketSend(byte[] buffer, int size)
		{
			try
			{
				networkstream.Write(buffer, 0, size);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		// ソケット受信
		static bool SocketRecv(byte[] buffer, int size, out int receivedSize)
		{
			try
			{
				receivedSize = networkstream.Read(buffer, 0, size);
//				receiver.DiscardBufferedData();
				return true;
			}
			catch (Exception)
			{
				receivedSize = 0;
				return false;
			}
		}

		// EntryShelfNoコマンド
		static void EntryShelfNo(string Socketip, int SocketportNo)
		{
			bool ret = false;
			byte[] sendBuffer = new byte[]
			{
				(byte)0x50, (byte)0x00, (byte)0x00, (byte)0xFF, (byte)0xFF, (byte)0x03, (byte)0x00,
				(byte)0x0C,										// Packet Data Length Low
				(byte)0x00,										// Packet Data Length Hi
				(byte)0x10, (byte)0x00,
				(byte)0x01, (byte)0x04,							// Sub Command
				(byte)0x00, (byte)0x00,
				(byte)0x28, (byte)0xA0, (byte)0x00,				// D00A028(D41000):EntryShelfNo
				(byte)0xA8,										// D reg
				(byte)0x01,										// Length Low
				(byte)0x00										// Length Hi
			};

			// ソケット接続
			ret = SocketConnect(Socketip, SocketportNo);
			if (!ret)
			{
				Console.WriteLine("Fail;SocketConnect Fail ");
				Thread.Sleep(waittingtime);
				return;
			}

			// ソケット送信
			ret = SocketSend(sendBuffer, 21);
			if (!ret)
			{
				Console.WriteLine("Fail;Socket SendData Fail ");
				Thread.Sleep(waittingtime);
				return;
			}

			// ソケット受信
			byte[] receiveBuffer = new byte[1024];
			for (int i = 0; i < 1024; ++i)
			{
				receiveBuffer[i] = (byte)0x00;
			}

			int receiveSize;
			ret = SocketRecv(receiveBuffer, 1024, out receiveSize);
			if (!ret)
			{
				Console.WriteLine("Fail;Socket receive data Fail ");
				Thread.Sleep(waittingtime);
				return;
			}

			// ソケット切断
			client.Close();

			byte lowdata = receiveBuffer[11];
			byte hidata = receiveBuffer[12];
			ushort wShelfNo = (ushort)(hidata << 8);
			wShelfNo = (ushort)(wShelfNo & 0xFF00);
			wShelfNo = (ushort)(wShelfNo | lowdata);
			Console.WriteLine("Pass;" + wShelfNo);
			return;
		}

		// CheckEntryShelfNoコマンド(★追加)
		static void CheckEntryShelfNo(string Socketip, int SocketportNo)
		{
			bool ret = false;
			byte[] sendBuffer = new byte[]
			{
				(byte)0x50, (byte)0x00, (byte)0x00, (byte)0xFF, (byte)0xFF, (byte)0x03, (byte)0x00,
				(byte)0x0C,										// Packet Data Length Low
				(byte)0x00,										// Packet Data Length Hi
				(byte)0x10, (byte)0x00,
				(byte)0x01, (byte)0x04,							// Sub Command
				(byte)0x00, (byte)0x00,
				(byte)0x2C, (byte)0xA0, (byte)0x00,				// D00A02C(D41004):EntryShelfNo
				(byte)0xA8,										// D reg
				(byte)0x01,										// Length Low
				(byte)0x00										// Length Hi
			};

			// ソケット接続
			ret = SocketConnect(Socketip, SocketportNo);
			if (!ret)
			{
				Console.WriteLine("Fail;SocketConnect Fail ");
				Thread.Sleep(waittingtime);
				return;
			}

			// ソケット送信
			ret = SocketSend(sendBuffer, 21);
			if (!ret)
			{
				Console.WriteLine("Fail;Socket SendData Fail ");
				Thread.Sleep(waittingtime);
				return;
			}

			// ソケット受信
			byte[] receiveBuffer = new byte[1024];
			for (int i = 0; i < 1024; ++i)
			{
				receiveBuffer[i] = (byte)0x00;
			}

			int receiveSize;
			ret = SocketRecv(receiveBuffer, 1024, out receiveSize);
			if (!ret)
			{
				Console.WriteLine("Fail;Socket receive data Fail ");
				Thread.Sleep(waittingtime);
				return;
			}

			// ソケット切断
			client.Close();

			byte lowdata = receiveBuffer[11];
			byte hidata = receiveBuffer[12];
			ushort wPrepara = (ushort)(hidata << 8);
			wPrepara = (ushort)(wPrepara & 0xFF00);
			wPrepara = (ushort)(wPrepara | lowdata);
			Console.WriteLine("Pass;" + wPrepara);
			return;
		}

		// SetEntryShelfNoコマンド(★追加)
		static void SetEntryShelfNo(string Socketip, int SocketportNo)
		{
			bool ret = false;
			byte[] sendBuffer = new byte[]
			{
				(byte)0x50, (byte)0x00, (byte)0x00, (byte)0xFF, (byte)0xFF, (byte)0x03, (byte)0x00,
				(byte)0x0E,										// Packet Data Length Low
				(byte)0x00,										// Packet Data Length Hi
				(byte)0x10, (byte)0x00,
				(byte)0x01, (byte)0x14,							// Sub Command
				(byte)0x00, (byte)0x00,
				(byte)0x32, (byte)0xA0, (byte)0x00,				// D00A032(D41010):ShelfNo of ExitWork
				(byte)0xA8,										// D reg
				(byte)0x01,										// Length Low
				(byte)0x00,										// Length Hi
				(byte)0x00, (byte)0x00,							// Change Data D41010
			};

			// ソケット接続
			ret = SocketConnect(Socketip, SocketportNo);
			if (!ret)
			{
				Console.WriteLine("Fail;SocketConnect Fail ");
				Thread.Sleep(waittingtime);
				return;
			}

			byte hidata = (byte)(0x0001 >> 8);
			byte lowdata = (byte)0x0001;
			sendBuffer[21] = lowdata;
			sendBuffer[22] = hidata;

			// ソケット送信
			ret = SocketSend(sendBuffer, 23);
			if (!ret)
			{
				Console.WriteLine("Fail;Socket SendData Fail ");
				Thread.Sleep(waittingtime);
				return;
			}

			// ソケット受信
			byte[] receiveBuffer = new byte[1024];
			for (int i = 0; i < 1024; ++i)
			{
				receiveBuffer[i] = (byte)0x00;
			}

			int receiveSize;
			ret = SocketRecv(receiveBuffer, 1024, out receiveSize);
			if (!ret)
			{
				Console.WriteLine("Fail;Socket receive data Fail ");
				Thread.Sleep(waittingtime);
				return;
			}

			// ソケット切断
			client.Close();
			Console.Write("Pass;SetEntryShelfNo OK ");
			Thread.Sleep(waittingtime);
			return;
		}

		// ClearEntryShelfNoコマンド(★追加)
		static void ClearEntryShelfNo(string Socketip, int SocketportNo)
		{
			bool ret = false;
			byte[] sendBuffer = new byte[]
			{
				(byte)0x50, (byte)0x00, (byte)0x00, (byte)0xFF, (byte)0xFF, (byte)0x03, (byte)0x00,
				(byte)0x0E,										// Packet Data Length Low
				(byte)0x00,										// Packet Data Length Hi
				(byte)0x10, (byte)0x00,
				(byte)0x01, (byte)0x14,							// Sub Command
				(byte)0x00, (byte)0x00,
				(byte)0x32, (byte)0xA0, (byte)0x00,				// D00A032(D41010):ShelfNo of ExitWork
				(byte)0xA8,										// D reg
				(byte)0x01,										// Length Low
				(byte)0x00,										// Length Hi
				(byte)0x00, (byte)0x00,							// Change Data D41010
			};

			// ソケット接続
			ret = SocketConnect(Socketip, SocketportNo);
			if (!ret)
			{
				Console.WriteLine("Fail;SocketConnect Fail ");
				Thread.Sleep(waittingtime);
				return;
			}

			byte hidata = (byte)(0x0000 >> 8);
			byte lowdata = (byte)0x0000;
			sendBuffer[21] = lowdata;
			sendBuffer[22] = hidata;

			// ソケット送信
			ret = SocketSend(sendBuffer, 23);
			if (!ret)
			{
				Console.WriteLine("Fail;Socket SendData Fail ");
				Thread.Sleep(waittingtime);
				return;
			}

			// ソケット受信
			byte[] receiveBuffer = new byte[1024];
			for (int i = 0; i < 1024; ++i)
			{
				receiveBuffer[i] = (byte)0x00;
			}

			int receiveSize;
			ret = SocketRecv(receiveBuffer, 1024, out receiveSize);
			if (!ret)
			{
				Console.WriteLine("Fail;Socket receive data Fail ");
				Thread.Sleep(waittingtime);
				return;
			}

			// ソケット切断
			client.Close();
			Console.Write("Pass;ClearEntryShelfNo OK ");
			Thread.Sleep(waittingtime);
			return;
		}

		// ExitWorkコマンド
		static void ExitWork(ushort shelfno, string downloadresult, string Socketip, int SocketportNo)
		{
			bool ret = false;
			byte[] sendBuffer = new byte[]
			{
				(byte)0x50, (byte)0x00, (byte)0x00, (byte)0xFF, (byte)0xFF, (byte)0x03, (byte)0x00,
				(byte)0x16,										// Packet Data Length Low
				(byte)0x00,										// Packet Data Length Hi
				(byte)0x10, (byte)0x00,
				(byte)0x01, (byte)0x14,							// Sub Command
				(byte)0x00, (byte)0x00,
				(byte)0x8C, (byte)0xA0, (byte)0x00,				// D00A08C(D41100):ShelfNo of ExitWork
																// D00A08E(D41102):Result of ExitWork
				(byte)0xA8,										// D reg
				(byte)0x05,										// Length Low
				(byte)0x00,										// Length Hi
				(byte)0x00, (byte)0x00,							// Change Data D41100
				(byte)0x00, (byte)0x00,							// Change Data D41101
				(byte)0x00, (byte)0x00,							// Change Data D41102
				(byte)0x00, (byte)0x00,							// Change Data D41103
				(byte)0x00, (byte)0x00							// Change Data D41104
			};

			// ソケット接続
			ret = SocketConnect(Socketip, SocketportNo);
			if (!ret)
			{
				Console.WriteLine("Fail;SocketConnect Fail ");
				Thread.Sleep(waittingtime);
				return;
			}

			byte hidata = (byte)(shelfno >> 8);
			byte lowdata = (byte)shelfno;
			sendBuffer[21] = lowdata;
			sendBuffer[22] = hidata;

			ushort work_result;
			if (downloadresult == "OK")
			{													// OK	
				work_result = 0x05FE;
			}
			else
			{													// NG
				work_result = 0x05FF;
			}
			hidata = (byte)(work_result >> 8);
			lowdata = (byte)work_result;
			sendBuffer[25] = (byte)lowdata;
			sendBuffer[26] = (byte)hidata;

			hidata = (byte)(0x0001 >> 8);
			lowdata = (byte)0x0001;
			sendBuffer[29] = (byte)lowdata;
			sendBuffer[30] = (byte)hidata;

			// ソケット送信
			ret = SocketSend(sendBuffer, 31);
			if (!ret)
			{
				Console.WriteLine("Fail;Socket SendData Fail ");
				Thread.Sleep(waittingtime);
				return;
			}

			// ソケット受信
			byte[] receiveBuffer = new byte[1024];
			for (int i = 0; i < 1024; ++i)
			{
				receiveBuffer[i] = (byte)0x00;
			}

			int receiveSize;
			ret = SocketRecv(receiveBuffer, 1024, out receiveSize);
			if (!ret)
			{
				Console.WriteLine("Fail;Socket receive data Fail ");
				Thread.Sleep(waittingtime);
				return;
			}

			// ソケット切断
			client.Close();
			Console.Write("Pass;ExitWork OK ");
			Thread.Sleep(waittingtime);
			return;
		}

		// CheckExitWorkコマンド(★追加)
		static void CheckExitWork(string Socketip, int SocketportNo)
		{
			bool ret = false;
			byte[] sendBuffer = new byte[]
			{
				(byte)0x50, (byte)0x00, (byte)0x00, (byte)0xFF, (byte)0xFF, (byte)0x03, (byte)0x00,
				(byte)0x0C,										// Packet Data Length Low
				(byte)0x00,										// Packet Data Length Hi
				(byte)0x10, (byte)0x00,
				(byte)0x01, (byte)0x04,							// Sub Command
				(byte)0x00, (byte)0x00,
				(byte)0x96, (byte)0xA0, (byte)0x00,				// D00A096(D41110):EntryShelfNo
				(byte)0xA8,										// D reg
				(byte)0x01,										// Length Low
				(byte)0x00										// Length Hi
			};

			// ソケット接続
			ret = SocketConnect(Socketip, SocketportNo);
			if (!ret)
			{
				Console.WriteLine("Fail;SocketConnect Fail ");
				Thread.Sleep(waittingtime);
				return;
			}

			// ソケット送信
			ret = SocketSend(sendBuffer, 21);
			if (!ret)
			{
				Console.WriteLine("Fail;Socket SendData Fail ");
				Thread.Sleep(waittingtime);
				return;
			}

			// ソケット受信
			byte[] receiveBuffer = new byte[1024];
			for (int i = 0; i < 1024; ++i)
			{
				receiveBuffer[i] = (byte)0x00;
			}

			int receiveSize;
			ret = SocketRecv(receiveBuffer, 1024, out receiveSize);
			if (!ret)
			{
				Console.WriteLine("Fail;Socket receive data Fail ");
				Thread.Sleep(waittingtime);
				return;
			}

			// ソケット切断
			client.Close();

			byte lowdata = receiveBuffer[11];
			byte hidata = receiveBuffer[12];
			ushort wExitEnd = (ushort)(hidata << 8);
			wExitEnd = (ushort)(wExitEnd & 0xFF00);
			wExitEnd = (ushort)(wExitEnd | lowdata);
			Console.WriteLine("Pass;" + wExitEnd);
			return;
		}

		// ClearExitWorkコマンド(★追加)
		static void ClearExitWork(string Socketip, int SocketportNo)
		{
			bool ret = false;
			byte[] sendBuffer = new byte[]
			{
				(byte)0x50, (byte)0x00, (byte)0x00, (byte)0xFF, (byte)0xFF, (byte)0x03, (byte)0x00,
				(byte)0x0E,										// Packet Data Length Low
				(byte)0x00,										// Packet Data Length Hi
				(byte)0x10, (byte)0x00,
				(byte)0x01, (byte)0x14,							// Sub Command
				(byte)0x00, (byte)0x00,
				(byte)0x90, (byte)0xA0, (byte)0x00,				// D00A090(D41104):ShelfNo of ExitWork
				(byte)0xA8,										// D reg
				(byte)0x01,										// Length Low
				(byte)0x00,										// Length Hi
				(byte)0x00, (byte)0x00,							// Change Data D41010
			};

			// ソケット接続
			ret = SocketConnect(Socketip, SocketportNo);
			if (!ret)
			{
				Console.WriteLine("Fail;SocketConnect Fail ");
				Thread.Sleep(waittingtime);
				return;
			}

			byte hidata = (byte)(0x0000 >> 8);
			byte lowdata = (byte)0x0000;
			sendBuffer[21] = lowdata;
			sendBuffer[22] = hidata;

			// ソケット送信
			ret = SocketSend(sendBuffer, 23);
			if (!ret)
			{
				Console.WriteLine("Fail;Socket SendData Fail ");
				Thread.Sleep(waittingtime);
				return;
			}

			// ソケット受信
			byte[] receiveBuffer = new byte[1024];
			for (int i = 0; i < 1024; ++i)
			{
				receiveBuffer[i] = (byte)0x00;
			}

			int receiveSize;
			ret = SocketRecv(receiveBuffer, 1024, out receiveSize);
			if (!ret)
			{
				Console.WriteLine("Fail;Socket receive data Fail ");
				Thread.Sleep(waittingtime);
				return;
			}

			// ソケット切断
			client.Close();
			Console.Write("Pass;ClearExitWork OK ");
			Thread.Sleep(waittingtime);
			return;
		}

		// All_ShelfStatusコマンド
		static void All_ShelfStatus(ushort ShelfStatus, string Socketip, int SocketportNo)
		{
			bool ret = false;
			byte[] SendBuffer = new byte[]
			{
				(byte)0x50, (byte)0x00, (byte)0x00, (byte)0xFF, (byte)0xFF, (byte)0x03, (byte)0x00,
				(byte)0x0C,										// Packet Data Length Low
				(byte)0x00,										// Packet Data Length Hi
				(byte) 0x10, (byte)0x00,
				(byte)0x01, (byte)0x04,							// Sub Command
				(byte)0x00, (byte)0x00,
				(byte) 0x1C, (byte)0xA2, (byte)0x00,			// D00A21C(D41500):EntryShelfNo
				(byte) 0xA8,									// D reg
				(byte) 0xFA,									// Length Low
				(byte) 0x00										// Length Hi
			};

			// ソケット接続
			ret = SocketConnect(Socketip, SocketportNo);
			if (!ret)
			{
				Console.WriteLine("Fail;SocketConnect Fail ");
				Thread.Sleep(waittingtime);
				return;
			}

			// ソケット送信
			ret = SocketSend(SendBuffer, 21);
			if (!ret)
			{
				Console.WriteLine("Fail;Socket SendData Fail ");
				Thread.Sleep(waittingtime);
				return;
			}

			// ソケット受信
			byte[] receiveBuffer = new byte[1024];
			for (int i = 0; i < 1024; ++i)
			{
				receiveBuffer[i] = (byte)0x00;
			}
			int ReceiveSize;
			ret = SocketRecv(receiveBuffer, 1024, out ReceiveSize);
			if (!ret)
			{
				Console.WriteLine("Fail;Socket receive data Fail ");
				Thread.Sleep(waittingtime);
				return;
			}

			// ソケット切断
			client.Close();

			// 受信データ解析
			byte lowdata;
			byte hidata;
			//  ushort ShelfStatus ;
			for (int i = 0; i < 250; i++)
			{
				lowdata = (byte)receiveBuffer[11 + i * 2];
				hidata = (byte)receiveBuffer[12 + i * 2];
				ShelfStatus = (ushort)(hidata << 8);
				ShelfStatus = (ushort)(ShelfStatus & 0xFF00);
				ShelfStatus = (ushort)(ShelfStatus | lowdata);
				ShelfStatus++;
			}

			Console.WriteLine("Pass;");
			Thread.Sleep(waittingtime);
			return;
		}

        static void SetEntryShelfNo(ushort shelfno, string Socketip, int SocketportNo)
        {
            bool ret = false;
            byte[] sendBuffer = new byte[]
			{
				(byte)0x50, (byte)0x00, (byte)0x00, (byte)0xFF, (byte)0xFF, (byte)0x03, (byte)0x00,
				(byte)0x0E,										// Packet Data Length Low
				(byte)0x00,										// Packet Data Length Hi
				(byte)0x10, (byte)0x00,
				(byte)0x01, (byte)0x14,							// Sub Command
				(byte)0x00, (byte)0x00,
				(byte)0x32, (byte)0xA0, (byte)0x00,				// D00A032(D41010):ShelfNo of ExitWork
				(byte)0xA8,										// D reg
				(byte)0x01,										// Length Low
				(byte)0x00,										// Length Hi
				(byte)0x00, (byte)0x00,							// Change Data D41010
			};

            // ソケット接続
            ret = SocketConnect(Socketip, SocketportNo);
            if (!ret)
            {
                Console.WriteLine("Fail;SocketConnect Fail ");
                Thread.Sleep(waittingtime);
                return;
            }

            byte hidata = (byte)(shelfno >> 8);
            byte lowdata = (byte)shelfno;
            sendBuffer[21] = lowdata;
            sendBuffer[22] = hidata;

            // ソケット送信
            ret = SocketSend(sendBuffer, 23);
            if (!ret)
            {
                Console.WriteLine("Fail;Socket SendData Fail ");
                Thread.Sleep(waittingtime);
                return;
            }

            // ソケット受信
            byte[] receiveBuffer = new byte[1024];
            for (int i = 0; i < 1024; ++i)
            {
                receiveBuffer[i] = (byte)0x00;
            }

            int receiveSize;
            ret = SocketRecv(receiveBuffer, 1024, out receiveSize);
            if (!ret)
            {
                Console.WriteLine("Fail;Socket receive data Fail ");
                Thread.Sleep(waittingtime);
                return;
            }

            // ソケット切断
            client.Close();
            Console.Write("Pass;SetEntryShelfNo OK ");
            Thread.Sleep(waittingtime);
            return;
        }
	}


}
