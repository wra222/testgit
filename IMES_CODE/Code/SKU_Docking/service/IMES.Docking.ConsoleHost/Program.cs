using System;
using System.Configuration;
using System.Runtime.Remoting;
using IMES.Infrastructure.Utility.Cache;
using IMES.Infrastructure.Utility.RuleSets;

namespace IMES.ConsoleHost
{
    public class Program
    {
        static void Main(string[] args)
        {

            ConnectionStringSettings ConnectionString = ConfigurationManager.ConnectionStrings["DBServer"];
            Console.WriteLine(ConnectionString.ConnectionString);
            System.Console.WriteLine("Load proactive cache now... All clients must wait for it...");
            //LoadProactiveCache();
            System.Console.WriteLine("Load all done!");
            RemotingConfiguration.Configure("IMES.Docking.ConsoleHost.exe.config", false);
            RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
            RemotingConfiguration.CustomErrorsEnabled(false);
            Console.ReadLine();
        }

        private static void LoadProactiveCache()
        {
            DeserializedRuleSetsManager.getInstance.LoadAll();
            DataChangeMediator.Start();
        }
    }
}
