using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using log4net;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace UTL.Protocol
{
    public class WebHttp
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static string PostResponse(string url, string xmlMsg, string user, string password, int timeout, out HttpStatusCode statusCode)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                logger.DebugFormat("URL:{0}", url);
                logger.DebugFormat("User:{0}", user);
                logger.DebugFormat("Password:{0}", password);
                logger.DebugFormat("TimeOut:{0}", timeout);
                logger.DebugFormat("xmlMsg:{0}", xmlMsg);

                string ret = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = timeout * 1000;
                request.Method = "POST";
                request.PreAuthenticate = true;

                setAuthorization(request, user, password);
                ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                request.ContentType = "application/xml;charset=utf-8";
                //post xml
                byte[] byteXml = Encoding.UTF8.GetBytes(xmlMsg);
                request.ContentLength = byteXml.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteXml, 0, byteXml.Length);
                dataStream.Close();
                logger.DebugFormat("Post Data Completed!!");
                //waiting response xml
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                ret = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                statusCode = response.StatusCode;
                response.Close();
                if (statusCode == HttpStatusCode.OK && ret.Contains("<ClientError") && ret.Contains("</ClientError>"))
                {
                    statusCode = HttpStatusCode.BadRequest;
                }
                logger.DebugFormat("response xmlMsg:{0}", ret);
                return ret;
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public static string GetResponse(string url, string user, string password, int timeout, out HttpStatusCode statusCode)
        {
            statusCode = HttpStatusCode.NotFound;
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                logger.DebugFormat("URL:{0}", url);
                logger.DebugFormat("User:{0}", user);
                logger.DebugFormat("Password:{0}", password);
                logger.DebugFormat("TimeOut:{0}", timeout);

                string ret = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = timeout * 1000;
                request.Method = "Get";
                request.PreAuthenticate = true;

                setAuthorization(request, user, password);
                ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

                //waiting response xml
                //WebResponse response = request.GetResponse();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                //foreach (string name in response.Headers.AllKeys)
                //{
                //    logger.DebugFormat("response.Headers {0} : {1}", name, response.Headers[name]); 
                //}
                //logger.DebugFormat("response.StatusDescription : {0}" , response.StatusDescription);
                statusCode = response.StatusCode;
                StreamReader reader = new StreamReader(dataStream);
                ret = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();

                response.Close();

                if (statusCode == HttpStatusCode.OK && ret.Contains("<ClientError") && ret.Contains("</ClientError>"))
                {
                    statusCode = HttpStatusCode.BadRequest;
                }
                logger.DebugFormat("response xmlMsg:{0}", ret);
                return ret;

            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        private static void setAuthorization(HttpWebRequest http, string user, string password)
        {

            String encoded = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(user + ":" + password));
            http.Headers.Add("Authorization", "Basic " + encoded);
        }
    }
}
