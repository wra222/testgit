using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting;

namespace IMES.BartenderPrinter
{
    public class RegisterRemoting
    {
        private static IChannel channel =null;
        public static void RegistRemotingObj(string channelName,  
                                                                int port, 
                                                                string typeName, 
                                                                string assembly,
                                                                 string url)
        {
            //Port
            IChannel chnl = ChannelServices.GetChannel(channelName);
            if (channel == null || chnl == null)
            {
                if (channel != null)
                    ChannelServices.UnregisterChannel(channel);
                if (chnl != null)
                    ChannelServices.UnregisterChannel(chnl);
                channel = new TcpServerChannel(channelName, port);
                ChannelServices.RegisterChannel(channel, false);
            }

            //Object Regist
            WellKnownServiceTypeEntry registerType = new WellKnownServiceTypeEntry(typeName, assembly, url, WellKnownObjectMode.Singleton);
            RemotingConfiguration.RegisterWellKnownServiceType(registerType);
            RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
            RemotingConfiguration.CustomErrorsEnabled(false);
            
        }
    }
}
