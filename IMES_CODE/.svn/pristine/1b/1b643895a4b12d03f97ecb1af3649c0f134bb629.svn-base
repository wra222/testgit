using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;


namespace GetIMESSessionInfo
{
    class Program
    {
        static string IMESUrl = "IMESService.Common";
        static string quit = "";
        static ServiceAgent serviceAgent = ServiceAgent.getInstance();
        static void Main(string[] args)
        {
            try
            {
                string address = "";
                string port = "";
                while (quit != "q")
                {
                    if (quit != "r")
                    {
                        Console.Write("Please Input Refresh Part Match Assembly Service Address:");
                        address = Console.ReadLine();
                        //Console.WriteLine();
                        Console.Write("Please Input Refresh Part Match Assembly Service Port:");
                        port = Console.ReadLine();
                        //Console.WriteLine();
                        //Console.Write("Please Input SessionType(C:Common  M:MB P:Product):");
                        //string inSessionType = Console.ReadLine().ToUpper();
                        //Console.WriteLine();

                        //sessionType = inSessionType.StartsWith("C") ? SessionType.Common : inSessionType.StartsWith("M") ? SessionType.MB : SessionType.Product;

                    }

                    ICache cache = serviceAgent.GetObjectByName<ICache>(address, port, IMESUrl);

                    cache.RefreshPartMatchAssembly();
                    Console.WriteLine("========================Check Server log File=======================");


                    Console.Write("Please input Q/q:Quit and Continue Any Key .... : ");
                    quit = Console.ReadLine().ToLower();
                    Console.WriteLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
            }
            finally
            {
               
            }
        }
        
    }
}
