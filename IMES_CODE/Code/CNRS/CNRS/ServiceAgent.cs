using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace UTL.Agent
{
    public class ServiceAgent
    {
        private static string CHANNELNAME = "TAPChanelClient";
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
        public T GetObjectByName<T>(string remoteSrvAddress, string remoteSrvPort, string objectUri)
        {
            if (!imesBllDic.ContainsKey(typeof(T).Name + objectUri))
            {
                //string stationService = System.Configuration.ConfigurationManager.AppSettings[objectUri];
               // string address = System.Configuration.ConfigurationManager.AppSettings["Address"];
               // string port = System.Configuration.ConfigurationManager.AppSettings["Port"];
                string url = "tcp://" + remoteSrvAddress + ":" + remoteSrvPort + "/";

                try
                {
                    lock (imesBllSyncObject)
                    {
                        channel = ChannelServices.GetChannel(CHANNELNAME);
                        if ((channel == null))
                        {
                            TcpClientChannel chnl = new TcpClientChannel(CHANNELNAME, null);
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

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return (T)imesBllDic[typeof(T).Name + objectUri];
        }

        public T GetObjectByName<T>(string objectUri)
        {
            if (!imesBllDic.ContainsKey(typeof(T).Name + objectUri))
            {
                string stageService = System.Configuration.ConfigurationManager.AppSettings[objectUri];
                string stageAddressName = stageService + "Address";
                string stagePortName = stageService + "Port";
                string address = System.Configuration.ConfigurationManager.AppSettings[stageAddressName];
                string port = System.Configuration.ConfigurationManager.AppSettings[stagePortName];
                string url = "tcp://" + address + ":" + port + "/";

                try
                {
                    lock (imesBllSyncObject)
                    {
                        channel = ChannelServices.GetChannel(CHANNELNAME);
                        if ((channel == null))
                        {
                            TcpClientChannel chnl = new TcpClientChannel(CHANNELNAME, null);
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

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return (T)imesBllDic[typeof(T).Name + objectUri];
        }

        private static Dictionary<string, object> givenServiceBllDic = new Dictionary<string, object>();
        private static object givenServiceBllSyncObject = new object();

      
    }
}
