using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using IMES_Reports.App_Code;

namespace IMES_Reports
{
   public class SoftReg
    {
        ///<summary>
        /// 获取硬盘卷标号
        ///</summary>
        ///<returns></returns>
        public string GetDiskVolumeSerialNumber()
        {
            ManagementClass mc = new ManagementClass("win32_NetworkAdapterConfiguration");
            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
        }

        ///<summary>
        /// 获取CPU序列号
        ///</summary>
        ///<returns></returns>
        public string GetCpu()
        {
            string strCpu = null;
            ManagementClass myCpu = new ManagementClass("win32_Processor");
            ManagementObjectCollection myCpuCollection = myCpu.GetInstances();
            foreach (ManagementObject myObject in myCpuCollection)
            {
                strCpu = myObject.Properties["Processorid"].Value.ToString();
            }
            return strCpu;
        }

        ///<summary>
        /// 生成机器码
        ///</summary>
        ///<returns></returns>
        public string GetMNum()
        {
            string strNum = GetCpu() + GetDiskVolumeSerialNumber();
            string strMNum = strNum.Substring(0, 24);    //截取前24位作为机器码
            return strMNum;
        }

        public int[] intCode = new int[127];    //存储密钥
        public char[] charCode = new char[25];  //存储ASCII码
        public int[] intNumber = new int[25];   //存储ASCII码值
        public int[] CheckNumber = new int[9];
        public char[] charCheck = new char[9];
        //初始化密钥
        public void SetIntCode()
        {
            for (int i = 1; i < intCode.Length; i++)
            {
                intCode[i] = i % 9;
            }
        }

        ///<summary>
        /// 生成注册码
        ///</summary>
        ///<returns></returns>
        public string GetRNum()
        {
            SetIntCode();
            string strMNum = GetMNum();
            for (int i = 1; i < charCode.Length; i++)   //存储机器码
            {
                charCode[i] = Convert.ToChar(strMNum.Substring(i - 1, 1));
            }
            for (int j = 1; j < intNumber.Length; j++)  //改变ASCII码值
            {
                intNumber[j] = Convert.ToInt32(charCode[j]) + intCode[Convert.ToInt32(charCode[j])];
            }
            string strAsciiName = "";   //注册码
            for (int k = 1; k < intNumber.Length; k++)  //生成注册码
            {

                if ((intNumber[k] >= 48 && intNumber[k] <= 57) || (intNumber[k] >= 65 && intNumber[k]
                    <= 90) || (intNumber[k] >= 97 && intNumber[k] <= 122))  //判断如果在0-9、A-Z、a-z之间
                {
                    strAsciiName += Convert.ToChar(intNumber[k]).ToString();
                }
                else if (intNumber[k] > 122)  //判断如果大于z
                {
                    strAsciiName += Convert.ToChar(intNumber[k] - 10).ToString();
                }
                else
                {
                    strAsciiName += Convert.ToChar(intNumber[k] - 9).ToString();
                }
            }
            return strAsciiName;
        }
        //public string GetRNum(string Input)
        //{
        //    string Check="";
        //    if (Input.Length==0) {
        //        Check = "";
        //    }
        //    if (Code_Check(Input))
        //    {
        //        Check = Input;
        //    }
        //     return Check;
        //}
        //public string Code_Check()
        //{ 
        //    bool IsExist=false;
        //    AppHelper app=new AppHelper();
        //    app.ReadConfig("appSettings", "connectionstring");
        //    Data_Factory df = new Data_Factory();
            
        //    return IsExist;
        //}
        public bool Code_Check()
        {
            bool IsExist = false;
            AppHelper app = new AppHelper();
            app.ReadConfig("appSettings", "connectionstring");
            Data_Factory df = new Data_Factory();

            return IsExist;
        }
        public string GetCheckCode(string RNum)
        {
            string Result = "";
            if(RNum.Length==33)
            {
                string Temp = RNum.Substring(24, 9);
                for (int i = 1; i < charCheck.Length; i++)   //存储机器码
                {
                    charCheck[i] = Convert.ToChar(Temp.Substring(i - 1, 1));
                }
                for (int j = 1; j < CheckNumber.Length; j++)  //改变ASCII码值
                {
                    CheckNumber[j] = Convert.ToInt32(charCode[j]);//+ intCode[Convert.ToInt32(charCode[j])];
                }
                for (int k = 1; k < CheckNumber.Length; k++)  //生成注册码
                {

                    if ((CheckNumber[k] >= 48 && CheckNumber[k] <= 57) || (intNumber[k] >= 65 && CheckNumber[k]
                        <= 90) || (CheckNumber[k] >= 97 && CheckNumber[k] <= 122))  //判断如果在0-9、A-Z、a-z之间
                    {
                        Result += Convert.ToChar(CheckNumber[k]).ToString();
                    }
                    else if (CheckNumber[k] > 122)  //判断如果大于z
                    {
                        Result += Convert.ToChar(CheckNumber[k] - 10).ToString();
                    }
                    else
                    {
                        Result += Convert.ToChar(CheckNumber[k] - 9).ToString();
                    }
                }
            }
            return Result;
        }
    }

}