using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace RemotingService
{
    public class ServiceAgent
    {
        private static ServiceAgent _serviceAgent = new ServiceAgent();


        private IChannel channel = null;

        //private LabelManager labelManager = null;
        // private LabelManager labelManagerForPrintingRoom = null;
        //private const String LABELMANAGER_SERVICEOBJECT = "LabelManager";
        //private const String CHANNELNAME = "iMesLabelManagerChanelClient";

        private ServiceAgent()
        {
        }


        public static ServiceAgent getInstance()
        {
            return _serviceAgent;
        }

        private static Dictionary<string, object> imesBllDic = new Dictionary<string, object>();
        private static object imesBllSyncObject = new object();

        /// <summary>
        /// 获取BLL Service对象
        /// </summary>
        /// <typeparam name="T">你所需要的BLL对象对应的接口类型</typeparam>
        /// <param name="objectUri">你所需要的BLL对象在Service的配置文件中配置的objectUri，也就是WebConstant类中对应的service path Region中配置的字串</param>
        /// <returns></returns>
        public T GetObjectByName<T>(string objectUri)
        {
            lock (imesBllSyncObject)
            {
                if (!imesBllDic.ContainsKey(typeof(T).Name + objectUri))
                {
                    string stationService = System.Configuration.ConfigurationManager.AppSettings[objectUri];
                    string address = System.Configuration.ConfigurationManager.AppSettings[stationService + "Address"];
                    string port = System.Configuration.ConfigurationManager.AppSettings[stationService + "Port"];
                    string url = "tcp://" + address + ":" + port + "/";

                    try
                    {
                        channel = ChannelServices.GetChannel(WebConstant.CHANNELNAME);
                        if ((channel == null))
                        {
                            TcpClientChannel chnl = new TcpClientChannel(WebConstant.CHANNELNAME, null);
                            ChannelServices.RegisterChannel(chnl, false);
                        }

                        url = url + objectUri;
                        T currentBllManager = (T)Activator.GetObject(typeof(T), url);

                        if (!imesBllDic.ContainsKey(typeof(T).Name + objectUri))
                        {
                            imesBllDic.Add(typeof(T).Name + objectUri, currentBllManager);
                            return currentBllManager;
                        }

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

            return (T)imesBllDic[typeof(T).Name + objectUri];
        }

        private static Dictionary<string, object> givenServiceBllDic = new Dictionary<string, object>();
        private static object givenServiceBllSyncObject = new object();

        /// <summary>
        /// 获取指定Service上的BLL Service对象
        /// </summary>
        /// <typeparam name="T">你所需要的BLL对象对应的接口类型</typeparam>
        /// <param name="objectUri">你所需要的BLL对象在Service的配置文件中配置的objectUri，也就是WebConstant类中对应的service path Region中配置的字串</param>
        /// <param name="service">指定Service的名字，可以是SA/FA/PAK其中的一个</param>
        /// <returns></returns>
        public T GetObjectByName<T>(string objectUri, string service)
        {
            lock (imesBllSyncObject)
            {
                if (!givenServiceBllDic.ContainsKey(service + typeof(T).Name + objectUri))
                {
                    string address = System.Configuration.ConfigurationManager.AppSettings[service + "Address"];
                    string port = System.Configuration.ConfigurationManager.AppSettings[service + "Port"];
                    string url = "tcp://" + address + ":" + port + "/";

                    try
                    {
                        channel = ChannelServices.GetChannel(WebConstant.CHANNELNAME);
                        if ((channel == null))
                        {
                            TcpClientChannel chnl = new TcpClientChannel(WebConstant.CHANNELNAME, null);
                            ChannelServices.RegisterChannel(chnl, false);
                        }

                        url = url + objectUri;
                        T currentBllManager = (T)Activator.GetObject(typeof(T), url);
                        //lock (givenServiceBllSyncObject)
                        //{
                        if (!givenServiceBllDic.ContainsKey(service + typeof(T).Name + objectUri))
                        {
                            givenServiceBllDic.Add(service + typeof(T).Name + objectUri, currentBllManager);
                            return currentBllManager;
                        }

                        //}

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

            return (T)givenServiceBllDic[service + typeof(T).Name + objectUri];
        }

        private static Dictionary<string, object> bllManagerDic = new Dictionary<string, object>();
        private static object syncObject = new object();

        public T GetMaintainObjectByName<T>(string objectUri)
        {
            lock (imesBllSyncObject)
            {
                if (!bllManagerDic.ContainsKey(typeof(T).Name + objectUri))
                {
                    string address = System.Configuration.ConfigurationManager.AppSettings["ServiceMaintainAddress"];
                    string port = System.Configuration.ConfigurationManager.AppSettings["ServiceMaintainPort"];
                    string url = "tcp://" + address + ":" + port + "/";

                    try
                    {
                        channel = ChannelServices.GetChannel(WebConstant.CHANNELNAME);
                        if ((channel == null))
                        {
                            TcpClientChannel chnl = new TcpClientChannel(WebConstant.CHANNELNAME, null);
                            ChannelServices.RegisterChannel(chnl, false);
                        }

                        url = url + objectUri;
                        T currentBllManager = (T)Activator.GetObject(typeof(T), url);
                        //lock (syncObject)
                        //{
                        if (!bllManagerDic.ContainsKey(typeof(T).Name + objectUri))
                        {
                            bllManagerDic.Add(typeof(T).Name + objectUri, currentBllManager);
                            return currentBllManager;
                        }
                        //}
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return (T)bllManagerDic[typeof(T).Name + objectUri];
        }        

    }


    public partial class WebConstant
    {
        public WebConstant()
        {
        }       

        #region other
        public const string CHANNELNAME = "IMESChanelClient";
        #endregion

        public const string ISAPService = "IMESService.ISAPService";

       

       
    }

}