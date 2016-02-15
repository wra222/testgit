using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using System.Linq;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Extend;
using System.Data.SqlClient;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using IMES.Common;
namespace IMES.Activity
{
    /// <summary>
    /// Return OA3 KEY
    /// </summary>
    public partial class DismantleOA3Key : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public DismantleOA3Key()
		{
			InitializeComponent();
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            var currentProduct = CurrentSession.GetValue(Session.SessionKeys.Product) as IProduct;
            if (currentProduct == null)
            {
                var ex1 = new FisException("SFC002", new string[] { "" });
                throw ex1;
            }
             IList<string> lstUrl;
             IList<string> lstUser;
             IList<string> lstPwd;
             IList<string> lstreturncode;
            if (ActivityCommonImpl.Instance.CheckModelByProcReg(currentProduct.Model, "ThinClient"))
            {
                 lstUrl = partRepository.GetValueFromSysSettingByName("TCRETUENOA3KEYURL");
                 lstUser = partRepository.GetValueFromSysSettingByName("TCRETUENOA3KEYUser");
                 lstPwd = partRepository.GetValueFromSysSettingByName("TCRETUENOA3KEYPassword");
                 lstreturncode = partRepository.GetValueFromSysSettingByName("TCRETUENOA3KEYCODE");
            }
            else
            {
               lstUrl = partRepository.GetValueFromSysSettingByName("RETUENOA3KEYURL");
               lstUser = partRepository.GetValueFromSysSettingByName("RETUENOA3KEYUser");
               lstPwd = partRepository.GetValueFromSysSettingByName("RETUENOA3KEYPassword");
               lstreturncode = partRepository.GetValueFromSysSettingByName("RETUENOA3KEYCODE");
            }
           string  url = lstUrl.Count == 0 ? "" : lstUrl[0];
           string user = lstUser.Count == 0 ? "" : lstUser[0];
           string password = lstPwd.Count == 0 ? "" : lstPwd[0];
           string returncode = lstreturncode.Count == 0 ? "" : lstreturncode[0];
           if ("" == url || "" == user || "" == password || ""==returncode)
            { throw new FisException("Wrong ReturnOA3Key setting"); }
           IList<IMES.FisObject.FA.Product.ProductInfo> pis = currentProduct.ProductInfoes;
           bool haswin8key = false;
           if (pis != null)
           {
               foreach (IMES.FisObject.FA.Product.ProductInfo pi in pis)
               {
                   if ("COA".Equals(pi.InfoType))
                   {
                       haswin8key = true;
                   }
               }
           }

           if (currentProduct.CUSTSN != "" && haswin8key)
           {
               string fkimessage = "";
               if (!CALL_FKIWEBSERVICE(url, user, password, currentProduct.CUSTSN, returncode, DateTime.Now.ToString(), out fkimessage))
               {
                   throw new FisException(currentProduct.CUSTSN+"  ReturnOA3Key Error,FKI Return Message:" + fkimessage);
               }
           }

            return base.DoExecute(executionContext);

        }
        private bool CALL_FKIWEBSERVICE(string thisURI, string username, string password, string HPSerialNumber, string returncode, string ReturnDate, out string strfkistatus)
        {
             strfkistatus = "";
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                  

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(thisURI);
                req.ContentType = "application/plain; charset=utf-8";
                req.Accept = "*/*";
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                req.Method = "POST";
                CredentialCache FKICredentialCache = new CredentialCache();
                FKICredentialCache.Add(new System.Uri(thisURI), "NTLM", new System.Net.NetworkCredential(username, password));
                req.Credentials = FKICredentialCache;
                string XMLInputData = "<?xml version='1.0' encoding='utf-8' ?>";
                XMLInputData += "<ReturnUsedKeysRequest xmlns='http://HP.ITTS.OA30/digitaldistribution/2011/08'>";
                XMLInputData += "<SNs>";
                XMLInputData += "<SN>";
                XMLInputData += "<HPSerialNumber>";
                XMLInputData += HPSerialNumber;
                XMLInputData += "</HPSerialNumber>";
                XMLInputData += "<ConfirmHPSerialNumber>";
                XMLInputData += HPSerialNumber;
                XMLInputData += "</ConfirmHPSerialNumber>";
                XMLInputData += "<ReturnCode>";
                XMLInputData += returncode;
                XMLInputData += "</ReturnCode>";
                XMLInputData += "<ReturnDate>";
                XMLInputData += ReturnDate;
                XMLInputData += "</ReturnDate>";
                XMLInputData += "</SN>";
                XMLInputData += "</SNs>";
                XMLInputData += "</ReturnUsedKeysRequest>";
                Encoding encoding = Encoding.Default;
                byte[] buffer = encoding.GetBytes(XMLInputData);
                req.ContentLength = buffer.Length;
                req.GetRequestStream().Write(buffer, 0, buffer.Length);
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                XmlTextReader xmlr = new XmlTextReader(res.GetResponseStream());
                StringBuilder str = new StringBuilder();
                while (xmlr.Read())
                {

                    if (xmlr.NodeType == System.Xml.XmlNodeType.Element && xmlr.LocalName.Equals("ReturnMessage"))
                    {
                        xmlr.Read();
                        strfkistatus = xmlr.Value.Trim();
                        if (strfkistatus != "" && strfkistatus != null)
                        {
                            return false;
                        }
                    }
                    if (xmlr.NodeType == System.Xml.XmlNodeType.Element && xmlr.LocalName.Equals("ReturnCode"))
                    {
                        xmlr.Read();
                        strfkistatus = xmlr.Value.Trim();
                    }
                }
                xmlr.Close();
                //return "SUCCESS" + strfkistatus;
                return true;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }


        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="certificate"></param>
       /// <param name="chain"></param>
       /// <param name="errors"></param>
       /// <returns></returns>
        public  bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {   // 軞岆諉忳    
            return true;

        }
	}
}
