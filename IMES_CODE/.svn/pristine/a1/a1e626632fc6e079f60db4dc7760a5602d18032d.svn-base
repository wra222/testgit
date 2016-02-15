using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.UPS.WS;

namespace IMES.UPS
{
    public class UPSWS
    {
         #region singlton 
        private UPSWS()
        {
        }
        private static readonly UPSWS _instance =new UPSWS();
        private static readonly ATRPSoapClient _soapClient = new ATRPSoapClient("UPS");
        /// <summary>
        /// Instance used for singleton
        /// </summary>
        public static UPSWS Instance
        {
            get
            {
                //if (_instance == null)
                //    _instance = new UPSWS();
                //if(_soapClient==null)
                //{
                //    _soapClient = new ATRPSoapClient("UPS");
                //}
                return _instance;
            }
        }

        #endregion
        public ATRPSoapClient UPSWSClient
        {
            get { return _soapClient;}
        }
        public ATMStruct GetATM(string sn, string avPartNo, string hppo)
        {  
            return _soapClient.UPSGetATM(sn, avPartNo, avPartNo,hppo,null);            
        }

        public  USIStruct GetUSI(string sn, string avPartNo, string hppo, string hpSKU)
        {
            return _soapClient.UPSGetUSI(sn, avPartNo, hppo,hpSKU);
        }

        public ATMStruct UpdatePrintInfoOld(string sn, string avPartNo, string hppo, 
                                                           string mac, string mac2,
                                                            string systemID, string placement, string tagData)
        {
            return _soapClient.UPSUpdatePrintInfoOld(sn, avPartNo, avPartNo, hppo,
                                                                      mac,mac2,systemID,placement,tagData);          
        }

        public ATMStruct UpdatePrintInfo(string sn, string avPartNo, string hppo,
                                                         string mac, string mac2, string systemID, 
                                                          string uuid, string productName, 
                                                          string placement, string tagData)
        {
            return _soapClient.UPSUpdatePrintInfo(sn, avPartNo, avPartNo, hppo,
                                                                      mac, mac2, systemID,uuid, productName, placement, tagData);
        }

        public ATMStruct UpdateShellPrintInfo(string sn, string avPartNo, string hppo,
                                                        string mac, string systemID,
                                                         string placement)
        {
            return _soapClient.UPSUpdateShellPrintInfo(sn,  avPartNo, hppo,
                                                                              mac,systemID,placement);
        }


        public TagOrderStruct GetTagOrder(string zwar)
        {
            return _soapClient.UPSGetTagOrder(zwar);
        }

        public ResetStruct UpdateHPPO(string sn, string avPartNo,  string oldHPPO, string newHPPO)
        {
            return _soapClient.UPSUpdateHPPO(sn, avPartNo, avPartNo, oldHPPO, newHPPO);
        }

        public ATRPStruct GetATRP(string sn, string placement)
        {
            return _soapClient.UPSGetATRP(sn,placement);
        }

        public ResetStruct ResetSN(string sn)
        {
            return _soapClient.UPSResetSN(sn);
        }
    }
}
