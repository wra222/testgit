using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using IMES_Reports.App_Code;
using System.Diagnostics;
namespace IMES_Reports
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            int count=0;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Input_Output());
            Application.Run(new Fly_Form());
           // Application.Run(new Main_From());
            foreach (Process p in Process.GetProcesses())
            {
                if (p.ProcessName == "IMES_Reports")
                {
                    count++;

                }
            }
            if (count < 2)
            {
                AppHelper AH = new AppHelper();
                AH.WriteConfig("IsOpen", "NO");
            }
        }
    }
}
