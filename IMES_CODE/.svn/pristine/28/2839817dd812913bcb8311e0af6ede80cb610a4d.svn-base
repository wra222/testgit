using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Runtime.Remoting;

namespace IMES.Maintain.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionStringSettings ConnectionString = ConfigurationManager.ConnectionStrings["OnlineDBServer"];
            Console.WriteLine(ConnectionString.ConnectionString);
            ConnectionString = ConfigurationManager.ConnectionStrings["HistoryDBServer"];
            Console.WriteLine(ConnectionString.ConnectionString);
            RemotingConfiguration.Configure("IMES.Query.ConsoleHost.exe.config", false);
            RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
            RemotingConfiguration.CustomErrorsEnabled(false);
            Console.ReadLine();
        }
    }
}
