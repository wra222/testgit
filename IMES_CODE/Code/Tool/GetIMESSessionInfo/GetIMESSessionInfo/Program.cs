using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace GetIMESSessionInfo
{
    class Program
    {
        static string IMESUrl = "IMESService.Common";
        static string quit = "";
        static ServiceAgent serviceAgent = ServiceAgent.getInstance();
        static void Main(string[] args)
        {
            string address = "";
            string port = "";
            SessionType sessionType = SessionType.Product;
            while (quit != "q")
            {
                if (quit != "r")
                {
                    Console.Write("Please Input Service Address:");
                    address = Console.ReadLine();
                    //Console.WriteLine();
                    Console.Write("Please Input Service Port:");
                    port = Console.ReadLine();
                    //Console.WriteLine();
                    Console.Write("Please Input SessionType(C:Common  M:MB P:Product):");
                    string inSessionType = Console.ReadLine().ToUpper();
                    //Console.WriteLine();

                    sessionType = inSessionType.StartsWith("C") ? SessionType.Common : inSessionType.StartsWith("M") ? SessionType.MB : SessionType.Product;
                  
                }

                ISession session = serviceAgent.GetObjectByName<ISession>(address, port, IMESUrl);
                IList<SessionInfo> sessionInfoList = session.GetSessionByType(sessionType);
                
                Console.WriteLine(string.Format("========================SessionInfo:{0}=======================",sessionInfoList.Count.ToString() ));
                var p = sessionInfoList.OrderBy(x => x.Cdt);
                foreach (SessionInfo item in p)
                {
                    
                    Console.WriteLine(string.Format("Date:{0} Operator:{1} PDLine:{2} SessionKey:{3} SessionType:{4} StationId:{5}",
                                                 item.Cdt.ToString("yyyyMMdd HH:mm:ss.fff"), item.Operator, item.PdLine,item.SessionKey, item.sessiontype.ToString(), item.StationId));
                   
                }
                Console.WriteLine("========================================================");

                Console.Write("Q/q:Quit R/r:Refresh  C: Continue Any Key .... : ");
                quit = Console.ReadLine().ToLower();
                Console.WriteLine();
            }


        }
    }
}
