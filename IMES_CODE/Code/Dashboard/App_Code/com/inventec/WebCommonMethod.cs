/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:  iMES web common method
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2009-10-20  Zhao Meili(EB)        Create 
 * 2009-10-28  Li.Ming-Jun(eB1)      Add: 1.serialize; 2.deserialize
 * 2009-12-16  Chen Xu (EB1-4)       Add printItemEntityForPrint

 * Known issues:
 */

using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Web.Services.Protocols;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using com.inventec.RBPC.Net.intf;
using com.inventec.RBPC.Net.datamodel.intf;




namespace com.inventec.iMESWEB
{
    /// <summary>
    /// Summary description for iMESCommonMethod
    /// </summary>
    public class WebCommonMethod
    {
        static ISecurityManager securityMgr = null;
        public WebCommonMethod()
        {
        }

        public static string getConfiguration(string key)
        {
            string configValue = System.Configuration.ConfigurationManager.AppSettings[key];
            if (configValue == null)
            {
                configValue = string.Empty;
            }
            return configValue.Trim();

        }

        public static int isTestWeight()
        {
            string setConfigValue=getConfiguration("SetWeightForTest");
            if (string.Compare(setConfigValue, "TRUE", true) == 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }

        public static object deserialize(string str)
        {
            byte[] bts = Convert.FromBase64String(str);
            BinaryFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(new MemoryStream(bts));
        }

    }

    //add by itc-208014:printItemEntityForPrint
    public class printItemEntityForPrint
    {
        private string com_TemplateName = string.Empty;
        private string com_LabelType = string.Empty;
        private IList<object> com_Keys = new List<object>();
        private string com_Piece = string.Empty;

        public string TemplateName
        {
            get { return com_TemplateName; }
            set { com_TemplateName = value; }
        }

        public string LabelType
        {
            get { return com_LabelType; }
            set { com_LabelType = value; }
        }
        public IList<object> Keys
        {

            get { return com_Keys; }
            set { com_Keys = value; }
        }

        public string Piece
        {
            get { return com_Piece; }
            set { com_Piece = value; }
        }
    }
}



    




  