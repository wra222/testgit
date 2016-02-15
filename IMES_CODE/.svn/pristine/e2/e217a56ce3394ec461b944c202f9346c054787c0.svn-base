using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Reflection;
using System.Transactions;
using System.Web.Configuration;
using System.Web.Services;
using IMES.Query.DB;
using IMES;

namespace IMES.WS.Common
{
    public enum EnumMsgCategory
    {
        Send,
        Receive,
        Response
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

    public class WSConstant
    {
        public static string SAPRawMaterial = "ZROH";
        public static string SAPFinishedGood = "ZFRT";
        public static string SAPModulesForCTO = "ZMOD";
        public static string SAPSemiProduct = "HALB";

    }

    public class SysHoldCode
    {
        public static readonly string MOCheckModelFail = "SYS100";
        public static readonly string MOCheckMaterialFail = "SYS101";
    }

    public class UTL
    {
        
        public static TransactionScope CreateDbTxn()
        {
         
           // int dbTimeOut = int.Parse(WebConfigurationManager.ConnectionStrings["DBTxnTimeOut"].ToString());
             int dbTimeOut = int.Parse(WebConfigurationManager.AppSettings["DBTxnTimeOut"]);
         
            return SQLTxn.CreateDbTxnScope(dbTimeOut);
        }

        public static void SendMail(string subject,
                                                string body)
        {
            try
            {
                IMES.Query.DB.SendMail.Send(WebConfigurationManager.AppSettings["FromAddress"].ToString(),
                                                           WebConfigurationManager.AppSettings["ToAddress"].ToString().Split(new char[] { ';', ',' }),
                                                           WebConfigurationManager.AppSettings["CcAddress"].ToString().Split(new char[] { ';', ',' }),
                                                           subject,
                                                           body,
                                                            WebConfigurationManager.AppSettings["MailServer"].ToString());
            }
            catch 
            {
            }
        }

        public static void SendMail(string preFixNameAddress,
                                               string subject,
                                               string body)
        {
            try
            {
                IMES.Query.DB.SendMail.Send(WebConfigurationManager.AppSettings["FromAddress"].ToString(),
                                                           WebConfigurationManager.AppSettings[preFixNameAddress+"ToAddress"].ToString().Split(new char[] { ';', ',' }),
                                                           WebConfigurationManager.AppSettings[preFixNameAddress + "CcAddress"].ToString().Split(new char[] { ';', ',' }),
                                                           subject,
                                                           body,
                                                            WebConfigurationManager.AppSettings["MailServer"].ToString());
            }
            catch
            {
            }
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
                { error = error + item + ",";}
            }
            if (!string.IsNullOrEmpty(error))
            { throw new Exception(title + error);}
        }
        public static string ObjectTostring(object dataObj)
        {
            object value;            
            string name;
            string result = "";
            foreach (FieldInfo f in dataObj.GetType().GetFields())
            {
                name = f.Name;
                value = GetValueByType(name,dataObj);

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
            return DateTime.ParseExact(strDate, "yyyyMMdd",null);
        }

        public static string ObjectPropertyToString(object dataObj)
        {
            object value;
            string name;
            string result = "";
            foreach (PropertyInfo property in dataObj.GetType().GetProperties())
            {
                name = property.Name;
                value = GetPropertyValueByType(property, dataObj);               
                result = result + name + ":" + value + "\r\n";

            }
            return result;
        }

        private static string GetPropertyValueByType(PropertyInfo property, object dataObj)
        {

            return property.GetValue(dataObj,null) == null ? "" : property.GetValue(dataObj, null).ToString();
                       
        }
    }


}