using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace FOPWrap
{
    public class FOP
    {
        public static void GeneratePDF(
            string FOPPath,
            string xmlPath,
            string xslPath,
            string pdfPath)
        {
            if (!File.Exists(FOPPath))
                throw new ArgumentException("FOP path incorrect.");
            if (!File.Exists(xmlPath))
                throw new ArgumentException("XML path incorrect.");
            if (!File.Exists(xslPath))
                throw new ArgumentException("XSL path incorrect.");

            //get ready to start the process
            ProcessStartInfo startInfo = new ProcessStartInfo(FOPPath);
            startInfo.Arguments = "-xml \"" + xmlPath + "\" -xsl \"" + xslPath + "\" -pdf \"" + pdfPath+"\"";
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            FileInfo FOPPathInfo = new FileInfo(FOPPath);
            startInfo.WorkingDirectory = FOPPathInfo.DirectoryName;
            Process fopProcess = Process.Start(startInfo);

            //wait until finish
            fopProcess.WaitForExit();
        }
        public static void GenerateFo(
            string FOPPath,
            string xmlPath,
            string xslPath,
            string foPath)
        {
            if (!File.Exists(FOPPath))
                throw new ArgumentException("FOP path incorrect.");
            if (!File.Exists(xmlPath))
                throw new ArgumentException("XML path incorrect.");
            if (!File.Exists(xslPath))
                throw new ArgumentException("XSL path incorrect.");

            //get ready to start the process
            ProcessStartInfo startInfo = new ProcessStartInfo(FOPPath);
            startInfo.Arguments = "-xml \"" + xmlPath + "\" -xsl \"" + xslPath + "\" -foout \"" + foPath + "\"";
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            FileInfo FOPPathInfo = new FileInfo(FOPPath);
            startInfo.WorkingDirectory = FOPPathInfo.DirectoryName;
            Process fopProcess = Process.Start(startInfo);

            //wait until finish
            fopProcess.WaitForExit();
        }
    }
}
