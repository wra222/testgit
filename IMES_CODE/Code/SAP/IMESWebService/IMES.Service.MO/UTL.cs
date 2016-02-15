using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Reflection;
using System.Transactions;
using IMES.Query.DB;
using System.Configuration;
using System.Data;


namespace IMES.Service.Common
{
    public enum EnumMsgCategory
    {
        Send,
        Receive
    }
    public enum EnumMsgState
    {
        Sending,
        Received,
        Success,
        Fail,
        Resend
    }

    public enum EnumProductMOState
    {
        Start,
        Run,
        Complete,
        Confirm,
        Close,
        Rework,
        Dismantle
    }

    public enum EnumMOState
    {
        Release,
        Start,
        Run,       
        Close      
    }

    public class SAPMOStatus
    {
        public static readonly string Release = "REL";
        public static readonly string PartialConfirmed = "PCNF";
        public static readonly string Confirmed = "CNF";
        public static readonly string Close = "TECO";        
        
    }

    public class UTL
    {
        

        public static TransactionScope CreateDbTxn()
        {
            int dbTimeOut = int.Parse(ConfigurationManager.AppSettings["DBTxnTimeOut"].ToString());
            return SQLTxn.CreateDbTxnScope(dbTimeOut);
        }

        public static void SendMail(string subject,
                                                string body)
        {
            IMES.Query.DB.SendMail.Send(ConfigurationManager.AppSettings["FromAddress"].ToString(),
                                                               ConfigurationManager.AppSettings["ToAddress"].ToString().Split(new char[] { ';', ',' }),
                                                               ConfigurationManager.AppSettings["CcAddress"].ToString().Split(new char[] { ';', ',' }),
                                                               subject,
                                                               body,
                                                                ConfigurationManager.AppSettings["MailServer"].ToString());
        }

        public static string GenTxnId()
        {
            return string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now);
        }


        public static string GetDtString(DataRow dr, string name)
        {
            return dr[name].ToString().Trim();
        }

        public static string GetDtString(DataRow dr, int index)
        {
            return dr[index].ToString().Trim();
        }

        public static int GetDtInt(DataRow dr, string name)
        {
            return (int)dr[name];
        }

        public static int GetDtInt(DataRow dr, int index)
        {
            return (int)dr[index];
        }
    }


    public static class ObjectTool
    {
        public static void CopyObject(Object OriginalObj, Object NewObj)
        {
            object value;
            string name;
            foreach (FieldInfo f in OriginalObj.GetType().GetFields())
            {
                value = f.GetValue(OriginalObj);
                name = f.Name;
                NewObj.GetType().GetField(name).SetValue(NewObj, (object)value);

            }

        }
        private static string GetValueByType(string type, object dataObj)
        {

            FieldInfo fi = dataObj.GetType().GetField(type);
            if (fi == null)
                return "";
            else
                if (fi.GetValue(dataObj) == null)
                { return ""; }
                else
                { return fi.GetValue(dataObj).ToString(); }

        }
        public static void CheckNullData(List<string> checkIem, object dataObj)
        {
            string className = dataObj.GetType().BaseType.Name;
            if (className == "Object")
            { className = dataObj.GetType().Name; }
            string title = "These columns of " + className + " are null or no data : ";
            string error = "";
            foreach (string item in checkIem)
            {
                if (string.IsNullOrEmpty(GetValueByType(item, dataObj).Trim()))
                { error = error + item + ","; }
            }
            if (!string.IsNullOrEmpty(error))
            { throw new Exception(title + error); }
        }
        public static string ObjectTostring(object dataObj)
        {
            object value;
            string name;
            string result = "";
            foreach (FieldInfo f in dataObj.GetType().GetFields())
            {
                name = f.Name;
                value = GetValueByType(name, dataObj);

                result = result + name + ":" + value + "\r\n";

            }
            return result;
        }
        public static string GenTxnId()
        {
            return string.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now);
        }

        public static DateTime String2Date(string strDate)
        {
            return DateTime.ParseExact(strDate, "yyyyMMdd", null);
        }

        public static string ObjectPropertyToString(object dataObj)
        {
            object value;
            string name;
            string result = "";
            foreach (PropertyInfo f in dataObj.GetType().GetProperties())
            {
                name = f.Name;

                value = GetPropertyValueByType(f, dataObj);

                result = result + name + ":" + value + "\r\n";

            }
            return result;
        }

        private static string GetPropertyValueByType(PropertyInfo type, object dataObj)
        {

            return type.GetValue(dataObj, null) == null ? "" : type.GetValue(dataObj, null).ToString();

        }
    }
}