using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using com.inventec.RBPC.Net.datamodel.intf;
using com.inventec.RBPC.Net.intf;
using com.inventec.RBPC.Net.entity;

namespace com.inventec.portal.dashboard
{

    /// <summary>
    /// Summary description for RBPCAgent
    /// </summary>
    public class RBPCAgent
    {
        public RBPCAgent()
        {
        }

        private static string url = ConfigurationManager.AppSettings["RBPCServiceAddress"].ToString();
        private static string port = ConfigurationManager.AppSettings["RBPCServicePort"].ToString();
        private static string tcpChannel = "tcp://" + url + ":" + port + "/";

        public static T getRBPCManager<T>()
        {
            IChannel channel = ChannelServices.GetChannel("RBPCClient");
            if (channel == null)
            {
                System.Runtime.Remoting.Channels.IChannel chnl = new System.Runtime.Remoting.Channels.Tcp.TcpClientChannel("RBPCClient", null);
                ChannelServices.RegisterChannel(chnl, false);
            }

            return (T)Activator.GetObject(typeof(T), tcpChannel + typeof(T).Name);
        }


    }

}

