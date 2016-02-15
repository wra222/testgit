using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Configuration.Assemblies;
//using System.Configuration.ConfigurationFileMap;
//using System.Configuration.ExeConfigurationFileMap;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
namespace IMES_Reports.App_Code
{
   public class AppHelper
    {
	    SoftReg softReg = new SoftReg();
        public string ReadConfig(string section, string setting)
        {
            
            List<string> list = new List<string>();
            ExeConfigurationFileMap    file = new ExeConfigurationFileMap();
            file.ExeConfigFilename = System.Windows.Forms.Application.ExecutablePath + ".config";
            Configuration config = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(file, ConfigurationUserLevel.None);

            //var myApp = (AppSettingsSection)config.GetSection("appSettings");
            var myApp = (AppSettingsSection)config.GetSection(section);
            //return myApp.Settings["connectionstring"].Value;
           
            return myApp.Settings[setting].Value;
        }
        public string WriteConfig(string section, string setting)
        {
            ExeConfigurationFileMap file = new ExeConfigurationFileMap();
            file.ExeConfigFilename = System.Windows.Forms.Application.ExecutablePath + ".config";
            Configuration config = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(file, ConfigurationUserLevel.None);

            var myApp = (AppSettingsSection)config.GetSection("appSettings");

            myApp.Settings[section].Value = setting;
            config.Save();
            return setting;

        }
        public void SetAutoRun(string fileName, bool isAutoRun)
        {
            RegistryKey reg = null;
            try
            {
                if (!System.IO.File.Exists(fileName))
                    throw new Exception("该文件不存在!");
                //String name = fileName.Substring(fileName.LastIndexOf(@"/") + 1);
                String name = fileName.Substring(fileName.LastIndexOf(@"\") + 1);
                reg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (reg == null)
                    reg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE/Microsoft/Windows/CurrentVersion/Run");
                if (isAutoRun)
                    reg.SetValue(name, fileName);
                else
                    reg.SetValue(name, false);
                //lbl_autorunerr.Visible = false;
            }
            catch(Exception e)
           {

                //lbl_autorunerr.Visible = true;
                //throw new Exception(ex.ToString());
            }
            finally
            {
                if (reg != null)
                    reg.Close();
            }
        }
        public bool IsActive_Check()
        {
            bool Result;
            try
            {
                RegistryKey retkey = Registry.CurrentUser.OpenSubKey("Software", true).OpenSubKey("Inventec").OpenSubKey("IMES.INI").OpenSubKey("IsActive");
                string A = retkey.GetValue("UserName").ToString();
                if (ReadConfig("appSettings", "IsActive") == "YES" && A != "")
                {
				   if(A==softReg.GetRNum())
				   {
                    Result = true;
                    return Result;
					}
					else
					{
					Result = false;
                    return Result;
					}
                }
                else
                {
                    Result = false;
                    return Result;
                }
            }
            catch (Exception e)
            {
                RegistryKey retkey = Registry.CurrentUser.OpenSubKey("Software", true).CreateSubKey("Inventec").CreateSubKey("IMES.INI").CreateSubKey("IsActive");
                retkey.SetValue("UserName", "");
                Result = false;
                return Result;
            }
        }
        public bool IsTryOut()
        {
            bool Resullt = false;
            try
            {
                RegistryKey UseTime_retkey = Registry.CurrentUser.OpenSubKey("Software", true).OpenSubKey("Inventec").OpenSubKey("IMES.INI").OpenSubKey("Tryout");

                string A1 = UseTime_retkey.GetValue("Tryout Times").ToString();
                if (A1 == "Run Out")
                {
                    Resullt = true;
                }
            }
            catch
            {
                RegistryKey retkey = Registry.CurrentUser.OpenSubKey("Software", true).CreateSubKey("Inventec").CreateSubKey("IMES.INI").CreateSubKey("Tryout");
                retkey.SetValue("Tryout Times","");
            }
            return Resullt;
        }
        public bool UseTime_Check()
        {
            bool Result;
            if (ReadConfig("appSettings", "IsActive") == "YES"  )
            {
                Result = true;
                return Result;
            }
            else
            {
                Result = false;
                return Result;
            }
        }
        public bool IsAuto_Check()
        {
            bool Result;
            if (ReadConfig("appSettings", "IsAuto") == "YES" &&ReadTryMessage("IsAuto","IsAuto")=="YES" )
            {
                Result = true;
                return Result;
            }
            else
            {
                Result = false;
                return Result;
            }
        }
        public bool Buttun_Check(string Type)
        {
            bool Result;
            if (ReadConfig("appSettings", Type) == "YES"&&ReadTryMessage(Type,"Btn_Check")=="YES")
            {
                Result = true;
                return Result;
            }
            else
            {
                Result = false;
                return Result;
            }
        }
		public string ReadTryMessage(string setting,string subkey)
		{
		  RegistryKey reg = null;
		  string Result="";
		  try
		  {
		    reg = Registry.CurrentUser.OpenSubKey("Software", true).OpenSubKey("Inventec").OpenSubKey("IMES.INI").OpenSubKey(subkey);
            Result = reg.GetValue(setting).ToString();
		  }
		  catch
		  {
		  	reg = Registry.CurrentUser.OpenSubKey("Software", true).CreateSubKey("Inventec").CreateSubKey("IMES.INI").CreateSubKey(subkey);
            reg.SetValue(setting, "");
			Result="";
		  }
		  return Result;
		}
		public void WriteTryMessage(string setting,string message,string subkey)
		{
		 RegistryKey reg = null;
				  try
		  {
		    reg = Registry.CurrentUser.OpenSubKey("Software", true).OpenSubKey("Inventec").OpenSubKey("IMES.INI").OpenSubKey(subkey);
            reg.SetValue(setting, message);
		  }
		  catch
		  {
		  	reg = Registry.CurrentUser.OpenSubKey("Software", true).CreateSubKey("Inventec").CreateSubKey("IMES.INI").CreateSubKey(subkey);
            reg.SetValue(setting, message);
		  }
		}
    }
}
