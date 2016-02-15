/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description:  iMES service agent for web use
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2009-10-20  Zhao Meili(EB)        Create 
 * 2009-12-20  Chen Xu (EB1-4)       Add LabelManager: getPrintTemplateObject
 * 2010-01-11  YuanXiaoWei           Add public T GetObjectByName<T>(string objectUri)
 * Known issues:
 */

using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;



namespace IMES.Infrastructure.Extend.Utility
{
    /// <summary>
    /// Summary description for SeviceAgent
    /// </summary>
    public class ServiceAgent
    {
        private static ServiceAgent _serviceAgent = new ServiceAgent();

        private IChannel channel = null;
              

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
            if (!imesBllDic.ContainsKey(typeof(T).Name + objectUri))
            {
                string stationService = System.Configuration.ConfigurationManager.AppSettings[objectUri];
                string address = System.Configuration.ConfigurationManager.AppSettings[stationService + "Address"];
                string port = System.Configuration.ConfigurationManager.AppSettings[stationService + "Port"];
                string url = "tcp://" + address + ":" + port + "/";

                try
                {
                    channel = ChannelServices.GetChannel(ServiceConstant.CHANNELNAME);
                    if ((channel == null))
                    {
                        TcpClientChannel chnl = new TcpClientChannel(ServiceConstant.CHANNELNAME, null);
                        ChannelServices.RegisterChannel(chnl, false);
                    }

                    url = url + objectUri;
                    T currentBllManager = (T)Activator.GetObject(typeof(T), url);
                    lock (imesBllSyncObject)
                    {
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

        /// <summary>
        /// 获取指定Service上的BLL Service对象
        /// </summary>
        /// <typeparam name="T">你所需要的BLL对象对应的接口类型</typeparam>
        /// <param name="objectUri">你所需要的BLL对象在Service的配置文件中配置的objectUri，也就是WebConstant类中对应的service path Region中配置的字串</param>
        /// <param name="service">指定Service的名字，可以是SA/FA/PAK其中的一个</param>
        /// <returns></returns>
        public T GetObjectByName<T>(string objectUri, string service)
        {
            if (!givenServiceBllDic.ContainsKey(service + typeof(T).Name + objectUri))
            {
                string address = System.Configuration.ConfigurationManager.AppSettings[service + "Address"];
                string port = System.Configuration.ConfigurationManager.AppSettings[service + "Port"];
                string url = "tcp://" + address + ":" + port + "/";

                try
                {
                    channel = ChannelServices.GetChannel(ServiceConstant.CHANNELNAME);
                    if ((channel == null))
                    {
                        TcpClientChannel chnl = new TcpClientChannel(ServiceConstant.CHANNELNAME, null);
                        ChannelServices.RegisterChannel(chnl, false);
                    }

                    url = url + objectUri;
                    T currentBllManager = (T)Activator.GetObject(typeof(T), url);
                    lock (givenServiceBllSyncObject)
                    {
                        if (!givenServiceBllDic.ContainsKey(service + typeof(T).Name + objectUri))
                        {
                            givenServiceBllDic.Add(service + typeof(T).Name + objectUri, currentBllManager);
                            return currentBllManager;
                        }

                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return (T)givenServiceBllDic[service + typeof(T).Name + objectUri];
        }

        private static Dictionary<string, object> bllManagerDic = new Dictionary<string, object>();
        private static object syncObject = new object();

            

    }


}