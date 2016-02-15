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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
//using IMES.Station.Interface.CommonIntf;
//using IMES.Station.Interface.StationIntf;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
//using com.inventec.template;
//using com.inventec.template.structure;


namespace com.inventec.iMESWEB
{
    /// <summary>
    /// Summary description for SeviceAgent
    /// </summary>
    public class ServiceAgent
    {
        private static ServiceAgent _serviceAgent = new ServiceAgent();

        //private object Print1397LabelManager = null;
        //private object Print1397Label_IFamilyManager = null;
        //private object Print1397Label_I1397Manager = null;
        //private object PCARepairInputManager = null;
        //private object PCARepairOutputManager = null;
        //private object PCARepairManager = null;
        //private object FARepairManager = null;
        //private object OQCInputManager = null;
        //private object OQCOutputManager = null;
        //private object OQCRepairManager = null;
        //private object OfflinePrintCTManager = null;
        //private object GenerateCustomerSNManager = null;
        //private object PCAShippingManager = null;
        //private object Common_ISessionManager = null;
        //private object Common_IMBManager = null;
        //private object Common_IProductManager = null;
        //private object Common_I1397Manager = null;
        //private object Common_IDefectManager = null;
        //private object Common_ISMTMOManager = null;
        //private object Common_IFamilyManager = null;
        //private object Common_ITestStationManager = null;
        //private object Common_IDocTypeManager = null;
        //private object Common_I4MManager = null;
        //private object Common_IResponsibilityManager = null;
        //private object Common_ISubDefectManager = null;
        //private object Common_IPartNoManager = null;
        //private object Common_IPalletManager = null;
        //private object Common_IBolNoManager = null;
        //private object Common_IRepairManager = null;
        //private object Common_IComponetManager = null;
        //private object Common_IMBCODEManager = null;
        //private object Common_I111LevelManager = null;
        //private object Common_IMOManager = null;
        //private object Common_IObligationManager = null;
        //private object Common_IMarkManager = null;
        //private object Common_IQCStatisticManager = null;
        //private object ImageDownloadCheckManager = null;
        //private object MBLabelPrintManager = null;
        //private object PCATestStationManager = null;
        //private object ReprintMACLabelManager = null;
        //private object BoardInputManager = null;
        //private object FATestStationManager = null;
        //private object KittingInputManager = null;
        //private object Common_ICauseManager = null;
        //private object Common_ICoverManager = null;
        //private object Common_IUncoverManager = null;
        //private object Common_ITrackingStatusManager = null;
        //private object Common_IPdLineManager = null;
        //private object Common_IPRINTTEMPLATEManager = null;
        //private object Common_IProdIdRangeManager = null;
        //private object Common_IMajorPartManager = null;
        //private object CombineCPUManager = null;
        //private object GenerateSMTMOManager = null;
        //private object ICTInputManager = null;
        //private object TravelCardManager = null;
        //private object InitialPartsCollectionManager = null;
        //private object PrintComMBManager = null;
        //private object ProIdPrintManager = null;
        //private object ProIdPrint_ModelManager = null;
        //private object ProIdPrint_MoManager = null;
        //private object TravelCardPrint_ModelManager = null;
        //private object TravelCardPrint_MoManager = null;
        //private object Common_ICollectionData = null;
        //private object MBLabelPrint_IMoManger = null;
        //private object MBPrintDismantleManger = null;
        //private object IMAGEOutputManger = null;

        private IChannel channel = null;

        //private LabelManager labelManager = null;
        //private LabelManager labelManagerForPrintingRoom = null;
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
                    channel = ChannelServices.GetChannel(WebConstant.CHANNELNAME);
                    if ((channel == null))
                    {
                        TcpClientChannel chnl = new TcpClientChannel(WebConstant.CHANNELNAME, null);
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

        public T GetMaintainObjectByName<T>(string objectUri)
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
                    lock (syncObject)
                    {
                        if (!bllManagerDic.ContainsKey(typeof(T).Name + objectUri))
                        {
                            bllManagerDic.Add(typeof(T).Name + objectUri, currentBllManager);
                            return currentBllManager;
                        }

                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return (T)bllManagerDic[typeof(T).Name + objectUri];
        }

        //public object GetObjectByName(string className, Type interfaceType)
        //{

        //    string address = System.Configuration.ConfigurationManager.AppSettings["ServiceAddress"];
        //    string port = System.Configuration.ConfigurationManager.AppSettings["ServicePort"];
        //    string url = "tcp://" + address + ":" + port + "/";

        //    try
        //    {
        //        channel = ChannelServices.GetChannel(WebConstant.CHANNELNAME);
        //        if ((channel == null))
        //        {
        //            TcpClientChannel chnl = new TcpClientChannel(WebConstant.CHANNELNAME, null);
        //            ChannelServices.RegisterChannel(chnl, false);
        //        }

        //        switch (className)
        //        {
        //            case (WebConstant.ProIdPrint):
        //                {
        //                    switch (interfaceType.Name)
        //                    {
        //                        case (WebConstant.IModel_SERVICE):
        //                            {
        //                                if (ProIdPrint_ModelManager == null)
        //                                {
        //                                    url = url + WebConstant.ProIdPrintObject;
        //                                    ProIdPrint_ModelManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return ProIdPrint_ModelManager;
        //                            }
        //                        case (WebConstant.IProIdPrint_SERVICE):
        //                            {
        //                                if (ProIdPrintManager == null)
        //                                {
        //                                    url = url + WebConstant.ProIdPrintObject;
        //                                    ProIdPrintManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return ProIdPrintManager;
        //                            }
        //                        case (WebConstant.IMO_SERVICE):
        //                            {
        //                                if (ProIdPrint_MoManager == null)
        //                                {
        //                                    url = url + WebConstant.ProIdPrintObject;
        //                                    ProIdPrint_MoManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return ProIdPrint_MoManager;
        //                            }
        //                        default:
        //                            {
        //                                return null;
        //                            }
        //                    }
        //                }
        //            case (WebConstant.IMAGEDownloadCheck):
        //                {
        //                    if (ImageDownloadCheckManager == null)
        //                    {
        //                        url = url + WebConstant.IMAGEDownloadCheckObject;
        //                        ImageDownloadCheckManager = Activator.GetObject(interfaceType, url);
        //                    }
        //                    return ImageDownloadCheckManager;
        //                }
        //            case (WebConstant.Print1397Label):
        //                {
        //                    switch (interfaceType.Name)
        //                    {
        //                        case (WebConstant.IFAMILY_SERVICE):
        //                            {
        //                                if (Print1397Label_IFamilyManager == null)
        //                                {
        //                                    url = url + WebConstant.Print1397LabelObject;
        //                                    Print1397Label_IFamilyManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Print1397Label_IFamilyManager;
        //                            }
        //                        case (WebConstant.I1397LEVEL_SERVICE):
        //                            {
        //                                if (Print1397Label_I1397Manager == null)
        //                                {
        //                                    url = url + WebConstant.Print1397LabelObject;
        //                                    Print1397Label_I1397Manager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Print1397Label_I1397Manager;
        //                            }
        //                        case (WebConstant.IPrint1397Label_SERVICE):
        //                            {
        //                                if (Print1397LabelManager == null)
        //                                {
        //                                    url = url + WebConstant.Print1397LabelObject;
        //                                    Print1397LabelManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Print1397LabelManager;
        //                            }
        //                        default:
        //                            {
        //                                return null;
        //                            }
        //                    }
        //                }
        //            case (WebConstant.PCARepairInput):
        //                {

        //                    if (PCARepairInputManager == null)
        //                    {
        //                        url = url + WebConstant.PCARepairInputObject;
        //                        //url = url + "FisService.Shipping";
        //                        PCARepairInputManager = Activator.GetObject(interfaceType, url);
        //                    }
        //                    return PCARepairInputManager;

        //                }
        //            case (WebConstant.PCARepairOutput):
        //                {
        //                    if (PCARepairOutputManager == null)
        //                    {
        //                        url = url + WebConstant.PCARepairOutputObject;
        //                        //url = url + "FisService.Shipping";
        //                        PCARepairOutputManager = Activator.GetObject(interfaceType, url);
        //                    }
        //                    return PCARepairOutputManager;

        //                }
        //            case (WebConstant.PCARepair):
        //                {
        //                    if (PCARepairManager == null)
        //                    {
        //                        url = url + WebConstant.PCARepairObject;
        //                        //url = url + "FisService.Shipping";
        //                        PCARepairManager = Activator.GetObject(interfaceType, url);
        //                    }
        //                    return PCARepairManager;

        //                }
        //            case (WebConstant.FARepair):
        //                {
        //                    if (FARepairManager == null)
        //                    {
        //                        url = url + WebConstant.FARepairObject;
        //                        //url = url + "FisService.Shipping";
        //                        FARepairManager = Activator.GetObject(interfaceType, url);
        //                    }
        //                    return FARepairManager;

        //                }
        //            case (WebConstant.OQCRepairInput):
        //                {
        //                    if (OQCInputManager == null)
        //                    {
        //                        url = url + WebConstant.OQCRepairInputObject;
        //                        //url = url + "FisService.Shipping";
        //                        OQCInputManager = Activator.GetObject(interfaceType, url);
        //                    }
        //                    return OQCInputManager;

        //                }
        //            case (WebConstant.OQCRepairOutput):
        //                {
        //                    if (OQCOutputManager == null)
        //                    {
        //                        url = url + WebConstant.OQCRepairOutputObject;
        //                        //url = url + "FisService.Shipping";
        //                        OQCOutputManager = Activator.GetObject(interfaceType, url);
        //                    }
        //                    return OQCOutputManager;

        //                }
        //            case (WebConstant.OQCRepair):
        //                {
        //                    if (OQCRepairManager == null)
        //                    {
        //                        url = url + WebConstant.OQCRepairObject;
        //                        //url = url + "FisService.Shipping";
        //                        OQCRepairManager = Activator.GetObject(interfaceType, url);
        //                    }
        //                    return OQCRepairManager;

        //                }
        //            case (WebConstant.OfflinePrintCT):
        //                {
        //                    if (OfflinePrintCTManager == null)
        //                    {
        //                        url = url + WebConstant.OfflinePrintCTObject;
        //                        //url = url + "FisService.Shipping";
        //                        OfflinePrintCTManager = Activator.GetObject(interfaceType, url);
        //                    }
        //                    return OfflinePrintCTManager;

        //                }
        //            case (WebConstant.GenerateCustomerSN):
        //                {
        //                    if (GenerateCustomerSNManager == null)
        //                    {
        //                        url = url + WebConstant.GenerateCustomerSNObject;
        //                        //url = url + "FisService.Shipping";
        //                        GenerateCustomerSNManager = Activator.GetObject(interfaceType, url);
        //                    }
        //                    return GenerateCustomerSNManager;

        //                }
        //            case (WebConstant.PCAShippingLabel):
        //                {

        //                    if (PCAShippingManager == null)
        //                    {
        //                        url = url + WebConstant.PCAShippingLabeObject;
        //                        PCAShippingManager = Activator.GetObject(interfaceType, url);
        //                    }
        //                    return PCAShippingManager;

        //                }
        //            case (WebConstant.MBLabelPrint):
        //                {

        //                    switch (interfaceType.Name)
        //                    {
        //                        case (WebConstant.IMBLabelPrint_SERVICE):
        //                            {
        //                                if (MBLabelPrintManager == null)
        //                                {
        //                                    url = url + WebConstant.MBLabelPrintObject;
        //                                    //url = url + "FisService.Shipping";
        //                                    MBLabelPrintManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return MBLabelPrintManager;
        //                            }
        //                        case (WebConstant.ISMTMO_SERVICE):
        //                            {
        //                                if (MBLabelPrint_IMoManger == null)
        //                                {
        //                                    url = url + WebConstant.MBLabelPrintObject;
        //                                    MBLabelPrint_IMoManger = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return MBLabelPrint_IMoManger;
        //                            }
        //                        default:
        //                            {
        //                                return null;
        //                            }
        //                    }

        //                }

        //            case (WebConstant.PCATestStation):
        //                {

        //                    if (PCATestStationManager == null)
        //                    {
        //                        url = url + WebConstant.PCATestStationObject;
        //                        //url = url + "FisService.Shipping";
        //                        PCATestStationManager = Activator.GetObject(interfaceType, url);
        //                    }
        //                    return PCATestStationManager;

        //                }

        //            case (WebConstant.MACReprint):
        //                {

        //                    if (ReprintMACLabelManager == null)
        //                    {
        //                        url = url + WebConstant.MACReprintObject;
        //                        //url = url + "FisService.Shipping";
        //                        ReprintMACLabelManager = Activator.GetObject(interfaceType, url);
        //                    }
        //                    return ReprintMACLabelManager;

        //                }

        //            case (WebConstant.BoardInput):
        //                {

        //                    if (BoardInputManager == null)
        //                    {
        //                        url = url + WebConstant.BoardInputObject;
        //                        //url = url + "FisService.Shipping";
        //                        BoardInputManager = Activator.GetObject(interfaceType, url);
        //                    }
        //                    return BoardInputManager;

        //                }

        //            case (WebConstant.FATestStation):
        //                {

        //                    if (FATestStationManager == null)
        //                    {
        //                        url = url + WebConstant.FATestStationObject;
        //                        //url = url + "FisService.Shipping";
        //                        FATestStationManager = Activator.GetObject(interfaceType, url);
        //                    }
        //                    return FATestStationManager;

        //                }
        //            case (WebConstant.KittingInput):
        //                {

        //                    if (KittingInputManager == null)
        //                    {
        //                        url = url + WebConstant.KittingInputObject;
        //                        //url = url + "FisService.Shipping";
        //                        KittingInputManager = Activator.GetObject(interfaceType, url);
        //                    }
        //                    return KittingInputManager;

        //                }
        //            case (WebConstant.GenerateSMTMO):
        //                {

        //                    if (GenerateSMTMOManager == null)
        //                    {
        //                        url = url + WebConstant.GenerateSMTMOObject;
        //                        //url = url + "FisService.Shipping";
        //                        GenerateSMTMOManager = Activator.GetObject(interfaceType, url);
        //                    }
        //                    return GenerateSMTMOManager;

        //                }
        //            case (WebConstant.ICTInput):
        //                {

        //                    if (ICTInputManager == null)
        //                    {
        //                        url = url + WebConstant.ICTInputObject;
        //                        //url = url + "FisService.Shipping";
        //                        ICTInputManager = Activator.GetObject(interfaceType, url);
        //                    }
        //                    return ICTInputManager;

        //                }
        //            case (WebConstant.TravelCardPrint):
        //                {

        //                    switch (interfaceType.Name)
        //                    {
        //                        case (WebConstant.ITravelCard_SERVICE):
        //                            {
        //                                if (TravelCardManager == null)
        //                                {
        //                                    url = url + WebConstant.TravelCardPrintObject;
        //                                    TravelCardManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return TravelCardManager;
        //                            }
        //                        case (WebConstant.IMO_SERVICE):
        //                            {
        //                                if (TravelCardPrint_MoManager == null)
        //                                {
        //                                    url = url + WebConstant.TravelCardPrintObject;
        //                                    TravelCardPrint_MoManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return TravelCardPrint_MoManager;
        //                            }
        //                        case (WebConstant.IModel_SERVICE):
        //                            {
        //                                if (TravelCardPrint_ModelManager == null)
        //                                {
        //                                    url = url + WebConstant.TravelCardPrintObject;
        //                                    TravelCardPrint_ModelManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return TravelCardPrint_ModelManager;
        //                            }
        //                        default:
        //                            return null;
        //                    }

        //                }

        //            case (WebConstant.InitialPartsCollection):
        //                {

        //                    if (InitialPartsCollectionManager == null)
        //                    {
        //                        url = url + WebConstant.InitialPartsCollectionObject;
        //                        //url = url + "FisService.Shipping";
        //                        InitialPartsCollectionManager = Activator.GetObject(interfaceType, url);
        //                    }
        //                    return InitialPartsCollectionManager;

        //                }

        //            case (WebConstant.CombineCPU):
        //                {

        //                    if (CombineCPUManager == null)
        //                    {
        //                        url = url + WebConstant.CombineCPUObject;
        //                        CombineCPUManager = Activator.GetObject(interfaceType, url);
        //                    }
        //                    return CombineCPUManager;

        //                }


        //            case (WebConstant.PrintComMB):
        //                {

        //                    if (PrintComMBManager == null)
        //                    {
        //                        url = url + WebConstant.PrintComMBObject;
        //                        PrintComMBManager = Activator.GetObject(interfaceType, url);
        //                    }
        //                    return PrintComMBManager;

        //                }

        //            case (WebConstant.MBPrintDismantle):
        //                {
        //                    if (MBPrintDismantleManger == null)
        //                    {
        //                        url = url + WebConstant.MBPrintDismantleObject;
        //                        MBPrintDismantleManger = Activator.GetObject(interfaceType, url);
        //                    }
        //                    return MBPrintDismantleManger;
        //                }

        //          case (WebConstant.IMAGEoutput):
        //                {
        //                    if (MBPrintDismantleManger == null)
        //                    {
        //                        url = url + WebConstant.MBPrintDismantleObject;
        //                        MBPrintDismantleManger = Activator.GetObject(interfaceType, url);
        //                    }
        //                    return MBPrintDismantleManger;
        //                }

        //            case (WebConstant.Common):
        //                {
        //                    //if (CommonManager == null)
        //                    //{
        //                    //    url = url + WebConstant.CommonObject;
        //                    //    CommonManager = Activator.GetObject(interfaceType, url);
        //                    //}
        //                    //return CommonManager;

        //                    switch (interfaceType.Name)
        //                    {
        //                        case (WebConstant.IQCStatistic_SERVICE):
        //                            {
        //                                if (Common_IQCStatisticManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_IQCStatisticManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_IQCStatisticManager;
        //                            }
        //                        case (WebConstant.IMark_SERVICE):
        //                            {
        //                                if (Common_IMarkManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_IMarkManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_IMarkManager;
        //                            }
        //                        case (WebConstant.IObligation_SERVICE):
        //                            {
        //                                if (Common_IObligationManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_IObligationManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_IObligationManager;
        //                            }
        //                        case (WebConstant.I111LEVEL_SERVICE):
        //                            {
        //                                if (Common_I111LevelManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_I111LevelManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_I111LevelManager;
        //                            }
        //                        case (WebConstant.IMB_CODE_SERVICE):
        //                            {
        //                                if (Common_IMBCODEManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_IMBCODEManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_IMBCODEManager;
        //                            }
        //                        case (WebConstant.IFAMILY_SERVICE):
        //                            {
        //                                if (Common_IFamilyManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_IFamilyManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_IFamilyManager;
        //                            }
        //                        case (WebConstant.IDefect_SERVICE):
        //                            {
        //                                if (Common_IDefectManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_IDefectManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_IDefectManager;
        //                            }
        //                        case (WebConstant.IMB_SERVICE):
        //                            {
        //                                if (Common_IMBManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_IMBManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_IMBManager;
        //                            }
        //                        case (WebConstant.ISession_SERVICE):
        //                            {
        //                                if (Common_ISessionManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_ISessionManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_ISessionManager;
        //                            }
        //                        case (WebConstant.I1397LEVEL_SERVICE):
        //                            {
        //                                if (Common_I1397Manager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_I1397Manager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_I1397Manager;
        //                            }

        //                        case (WebConstant.ISMTMO_SERVICE):
        //                            {
        //                                if (Common_ISMTMOManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_ISMTMOManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_ISMTMOManager;
        //                            }
        //                        case (WebConstant.IProduct_SERVICE):
        //                            {
        //                                if (Common_IProductManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_IProductManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_IProductManager;
        //                            }
        //                        case (WebConstant.ITESTSTATION_SERVICE):
        //                            {
        //                                if (Common_ITestStationManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_ITestStationManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_ITestStationManager;
        //                            }
        //                        case (WebConstant.IDOCTYPE_SERVICE):
        //                            {
        //                                if (Common_IDocTypeManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_IDocTypeManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_IDocTypeManager;
        //                            }

        //                        case (WebConstant.I4M_SERVICE):
        //                            {
        //                                if (Common_I4MManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_I4MManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_I4MManager;
        //                            }
        //                        case (WebConstant.IResponsibility_SERVICE):
        //                            {
        //                                if (Common_IResponsibilityManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_IResponsibilityManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_IResponsibilityManager;
        //                            }
        //                        case (WebConstant.ISubDefect_SERVICE):
        //                            {
        //                                if (Common_ISubDefectManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_ISubDefectManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_ISubDefectManager;
        //                            }
        //                        case (WebConstant.IPartNo_SERVICE):
        //                            {
        //                                if (Common_IPartNoManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_IPartNoManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_IPartNoManager;
        //                            }
        //                        case (WebConstant.IPallet_SERVICE):
        //                            {
        //                                if (Common_IPalletManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_IPalletManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_IPalletManager;
        //                            }
        //                        case (WebConstant.IBOLNo_SERVICE):
        //                            {
        //                                if (Common_IBolNoManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_IBolNoManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_IBolNoManager;
        //                            }


        //                        case (WebConstant.ICause_SERVICE):
        //                            {
        //                                if (Common_ICauseManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_ICauseManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_ICauseManager;
        //                            }
        //                        case (WebConstant.ICover_SERVICE):
        //                            {
        //                                if (Common_ICoverManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_ICoverManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_ICoverManager;
        //                            }

        //                        case (WebConstant.IUncover_SERVICE):
        //                            {
        //                                if (Common_IUncoverManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_IUncoverManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_IUncoverManager;
        //                            }
        //                        case (WebConstant.ITrackingStatus_SERVICE):
        //                            {
        //                                if (Common_ITrackingStatusManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_ITrackingStatusManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_ITrackingStatusManager;
        //                            }

        //                        case (WebConstant.IPDLINE_SERVICE):
        //                            {
        //                                if (Common_IPdLineManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_IPdLineManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_IPdLineManager;
        //                            }


        //                        case (WebConstant.IPRINTTEMPLATE_SERVICE):
        //                            {
        //                                if (Common_IPRINTTEMPLATEManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_IPRINTTEMPLATEManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_IPRINTTEMPLATEManager;
        //                            }

        //                        case (WebConstant.IRepair_SERVICE):
        //                            {
        //                                if (Common_IRepairManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_IRepairManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_IRepairManager;
        //                            }

        //                        case (WebConstant.IComponent_SERVICE):
        //                            {
        //                                if (Common_IComponetManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_IComponetManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_IComponetManager;
        //                            }

        //                        case (WebConstant.IProdIdRange_SERVICE):
        //                            {
        //                                if (Common_IProdIdRangeManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_IProdIdRangeManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_IProdIdRangeManager;

        //                            }

        //                        case (WebConstant.IMajorPart_SERVICE):
        //                            {
        //                                if (Common_IMajorPartManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_IMajorPartManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_IMajorPartManager;

        //                            }
        //                        case (WebConstant.ICollectionData_SERVICE):
        //                            {
        //                                if (Common_ICollectionData == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_ICollectionData = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_ICollectionData;

        //                            }
        //                        case (WebConstant.IMO_SERVICE):
        //                            {
        //                                if (Common_IMOManager == null)
        //                                {
        //                                    url = url + WebConstant.CommonObject;
        //                                    Common_IMOManager = Activator.GetObject(interfaceType, url);
        //                                }
        //                                return Common_IMOManager;

        //                            }

        //                        default:
        //                            {
        //                                return null;
        //                            }

        //                    }
        //                }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    //}
        //    return null;
        //}

        //add by itc208014: LabelManager: getPrintTemplateObject
        //public LabelManager getPrintTemplateObject(Boolean isPrintRoom)
        //{

        //    String address = System.Configuration.ConfigurationManager.AppSettings.Get("TemplateServiceAddress").ToString();
        //    String port = System.Configuration.ConfigurationManager.AppSettings.Get("TemplateServicePort").ToString();
        //    String printPort = System.Configuration.ConfigurationManager.AppSettings.Get("TSPrintingRoomPort").ToString();
        //    String url = "tcp://" + address + ":" + port + "/";
        //    String printUrl = "tcp://" + address + ":" + printPort + "/";

        //    try
        //    {

        //        channel = ChannelServices.GetChannel(CHANNELNAME);
        //        if (null == channel)
        //        {
        //            TcpClientChannel chnl = new TcpClientChannel(CHANNELNAME, new BinaryClientFormatterSinkProvider());
        //            ChannelServices.RegisterChannel(chnl, false);
        //        }

        //        if (isPrintRoom)
        //        {
        //            if (null == this.labelManagerForPrintingRoom)
        //            {
        //                printUrl = printUrl + LABELMANAGER_SERVICEOBJECT;
        //                labelManagerForPrintingRoom = (LabelManager)Activator.GetObject(typeof(LabelManager), printUrl);
        //            }
        //            return labelManagerForPrintingRoom;
        //        }
        //        else
        //        {
        //            if (null == this.labelManager)
        //            {
        //                url = url + LABELMANAGER_SERVICEOBJECT;
        //                labelManager = (LabelManager)Activator.GetObject(typeof(LabelManager), url);
        //            }
        //            return labelManager;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

    }


}