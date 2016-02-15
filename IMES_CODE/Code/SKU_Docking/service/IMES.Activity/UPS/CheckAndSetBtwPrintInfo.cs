// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Change PO Product Condition
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2015-01-21 Vincent             create
// Known issues:
using System;
using System.Linq;

using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.UPS;
using IMES.UPS;
using IMES.UPS.WS;
using IMES.FisObject.PAK.DN;
using IMES.Common;

namespace IMES.Activity
{
    /// <summary>
    /// CheckAndSetBtwPrintInfo Product
    /// </summary>
    public partial class CheckAndSetBtwPrintInfo : BaseActivity
    {
        ///<summary>
        ///</summary>
        public CheckAndSetBtwPrintInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// GetAndSetBtwPrintInfo
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;

            var currenProduct = utl.IsNull<IProduct>(session, Session.SessionKeys.Product);
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
            IUPSRepository upsRep = RepositoryFactory.GetInstance().GetRepository<IUPSRepository>(); 

            IList<AstDefineInfo> needCombineAstList = utl.IsNull<IList<AstDefineInfo>>(session, Session.SessionKeys.NeedCombineAstDefineList);
            IList<IPart> astPartList = utl.IsNull<IList<IPart>>(session, Session.SessionKeys.NeedCombineAstPartList);
            if (needCombineAstList.Count == 0 ||
               !needCombineAstList.Any(x=>x.NeedPrint=="Y"))
            {
                return base.DoExecute(executionContext);  // 此机器不需要print asset Label !
            }
           
            if (!utl.UPS.checkUPSDeviceInCombineStation(currenProduct, needCombineAstList, this.Station))
            {
                 return base.DoExecute(executionContext);  // none UPS device 
            } 

            DateTime now = DateTime.Now;
            UPSCombinePO  combinePO = (UPSCombinePO)session.GetValue(Session.SessionKeys.UPSCombinePO);
            if (combinePO == null)
            {
                combinePO = upsRep.Find(currenProduct.ProId);
            }

            if (combinePO == null)
            {
                //throw no assign UPS Asset number
                throw new FisException("CQCHK1120", new string[] { currenProduct.CUSTSN, currenProduct.Model });
            }

            //CDSI 機器不走UPS Print 
            bool isCDSIDevice=false;
            if (currenProduct.IsCDSI && !currenProduct.IsCNRS)
            {
                isCDSIDevice=true;
                //return base.DoExecute(executionContext);
            }

            var upsAvPart = combinePO.UPSAVPart;
            if (upsAvPart == null || upsAvPart.Count==0)
            {
                //throw error no UPS AV PartNo
                throw new FisException("CQCHK1115", new string[] { currenProduct.CUSTSN, currenProduct.Model });
            }

            if (string.IsNullOrEmpty(currenProduct.DeliveryNo))
            {
                //throw no assign delivery No
                throw new FisException("CQCHK1114", new string[] { currenProduct.CUSTSN, currenProduct.Model });
            }
            //Check ModelInfo 是否有SID
            string sid = currenProduct.ModelObj.Attributes.Where(x => x.Name == "SID").Select(y => y.Value).FirstOrDefault();
            string productName = currenProduct.ModelObj.Attributes.Where(x => x.Name == "MN1").Select(y => y.Value).FirstOrDefault();
            if (string.IsNullOrEmpty(sid) && string.IsNullOrEmpty(productName))
            {
                    throw new FisException("PAK085", new string[] { currenProduct.Model, "SID and MN1" });
            }

            string uuid = currenProduct.ProductInfoes.Where(x => x.InfoType == "UUID").Select(y => y.InfoValue).FirstOrDefault();

            UPSWS ws = UPSWS.Instance;
            string sn = currenProduct.CUSTSN;
            string hppo = combinePO.HPPO;
            string mac = currenProduct.MAC;
            string model = currenProduct.Model;
            string mac2 = ((string)currenProduct.GetExtendedProperty("WM"));
            if (!string.IsNullOrEmpty(mac2))
            {
                mac2 = mac2.Replace(":", "");
            }
            string newHPPo = null;

            Delivery delivery = dnRep.Find(currenProduct.DeliveryNo);
            if (combinePO.IECPO != delivery.PoNo)
            {
                UPSIECPOInfo iecPoInfo = upsRep.GetIECPO(delivery.PoNo, model);
                if (iecPoInfo != null)
                {
                    newHPPo = iecPoInfo.HPPO;                   
                }
            }            

            //update printInfo -> update hp po -> get bartender  
            foreach (AstDefineInfo define in needCombineAstList)
            {
                if (define.HasUPSAst != "Y" || 
                    define.NeedPrint != "Y" || 
                    define.HasCDSIAst=="Y")  //非UPS AST don't case or Need Print Asstag
                {
                    continue;
                }
                bool hasUPSPartNo = false;
                var matchPartList = astPartList.Where(x => x.BOMNodeType == define.AstType && x.Descr == define.AstCode).ToList();
                 foreach (IPart part in matchPartList)
                 {
                     #region Check UPSAVPart and UPSUpdatePrintInfo web service
                     string avPartNo = part.Attributes.Where(x => x.InfoType == "AV" && !string.IsNullOrEmpty(x.InfoValue)).Select(x => x.InfoValue).FirstOrDefault();
                    if (string.IsNullOrEmpty(avPartNo))
                    {
                        //throw error 2TGXXXX part no setup AV part no
                        throw new FisException("CQCHK1116", new string[] { currenProduct.CUSTSN, currenProduct.Model, part.PN });
                    }

                    if (!upsAvPart.Any(x => x.AVPartNo == avPartNo))
                    {
                        //throw error AVPart is not UPS AV Part number 
                        //throw new FisException("CQCHK1117", new string[] { currenProduct.CUSTSN, currenProduct.Model, part.PN, avPartNo });
                        //考慮考虑机型1个是UPS ，另1个不是UPS case
                        continue;
                    }
                    hasUPSPartNo = true;
                    #region UPSGetTagOrder for Normal case
                    string tagData = string.Empty;
                    //IList<ConstValueInfo> upsTagDataList = null;
                    //ConstValueInfo upsTagData = null;
                    //if (utl.TryConstValue("UPSTagData", avPartNo, out upsTagDataList, out upsTagData))
                    //{
                    //    tagData = upsTagData.value;
                    //}
                    if (!isCDSIDevice)
                    {
                        string zwarPartNo=part.Attributes.Where(x => x.InfoType == "ZWAR").Select(x => x.InfoValue).LastOrDefault();
                        if (string.IsNullOrEmpty(zwarPartNo))
                        {
                            //throw error missing ZWAR PartNo in AV , IEC PartNo
                            throw new FisException("CQCHK1123", new string[]{currenProduct.CUSTSN, currenProduct.Model, 
                                                                                                            string.Empty,part.PN, avPartNo});
                        }
                        TagOrderStruct tagOrder = ws.GetTagOrder(zwarPartNo);
                        if (tagOrder == null ||
                            tagOrder.retcode != 0 ||
                            string.IsNullOrEmpty(tagOrder.datastring))
                        {
                            //throw error error code and Error message
                            throw new FisException("CQCHK1118", new string[] { "UPSGetTagOrder", currenProduct.CUSTSN, 
                                                                                                            hppo, avPartNo, 
                                                                                                            "RetCode:" + tagOrder.retcode.ToString() +" " + tagOrder.message ?? "" });
                        }

                        Dictionary<string, string> orderList = getTagOrderValue(tagOrder.datastring);
                        if (orderList.ContainsKey("Template"))
                        {
                            string templateName = orderList["Template"];
                            if (templateName != avPartNo)
                            {
                                throw new FisException("CQCHK1123", new string[]{currenProduct.CUSTSN, currenProduct.Model, 
                                                                                                            zwarPartNo+"/" +templateName ,
                                                                                                            part.PN, 
                                                                                                            avPartNo});
                            }
                        }

                        if (define.AstLocation == utl.AstShippingLocation)
                        {
                            if (orderList.ContainsKey("Box"))
                            {
                               tagData = orderList["Box"];                                
                            }
                        }
                        else
                        {
                            if (orderList.ContainsKey("Chassis"))
                            {
                                tagData = orderList["Chassis"];
                            }
                        }

                        if (string.IsNullOrEmpty(tagData))
                        {
                            throw new FisException("CQCHK1124", new string[]{currenProduct.CUSTSN, currenProduct.Model, 
                                                                                                            zwarPartNo,tagOrder.datastring, avPartNo});

                        }

                        //write product Attributes
                        currenProduct.SetAttributeValue("UPSTagData", tagData, this.Editor, this.Station, tagOrder.datastring);
                    }
                    #endregion
                    if (!string.IsNullOrEmpty(newHPPo))
                    {
                        ResetStruct rs = ws.UpdateHPPO(sn, avPartNo, hppo, newHPPo);
                        if (rs.retcode < 0)
                        {
                            //throw new Exception(ret.message);
                            throw new FisException("CQCHK1118", new string[] { "UPSUpdateHPPO", currenProduct.CUSTSN, hppo, avPartNo, rs.message });
                        }
                        hppo = newHPPo;// 用最的HPPO 执行UpdatePrintInfo
                    }

                    #region UPSUpdatePrintInfo for normal and shell case
                    if (isCDSIDevice)
                    {
                        ATMStruct ret = ws.UpdateShellPrintInfo(sn, avPartNo, hppo, mac, sid, define.AstLocation);
                        if (ret.retcode < 0)
                        {
                            ret = ws.UpdateShellPrintInfo(sn, avPartNo, hppo, mac, productName, define.AstLocation);
                            if (ret.retcode < 0)
                            {
                                //throw new Exception(ret.message);
                                throw new FisException("CQCHK1118", new string[] { "UPSUpdateShellPrintInfo", currenProduct.CUSTSN, hppo, avPartNo, ret.message });
                            }
                        }
                    }
                    else
                    {
                        ATMStruct ret = ws.UpdatePrintInfo(sn, avPartNo, hppo, mac, mac2, sid, uuid, productName, define.AstLocation, tagData);
                        if (ret.retcode < 0)
                        {
                            ret = ws.UpdatePrintInfo(sn, avPartNo, hppo, mac, mac2, productName, uuid, productName, define.AstLocation, tagData);
                            if (ret.retcode < 0)
                            {
                                //throw new Exception(ret.message);
                                throw new FisException("CQCHK1118", new string[] { "UPSUpdatePrintInfo", currenProduct.CUSTSN, hppo, avPartNo, ret.message });
                            }
                        }
                    }
                    #endregion

                    //if (!string.IsNullOrEmpty(newHPPo))
                    //{
                    //    ResetStruct rs=ws.UpdateHPPO(sn, avPartNo, hppo, newHPPo);
                    //    if (rs.retcode <0)
                    //    {
                    //        //throw new Exception(ret.message);
                    //        throw new FisException("CQCHK1118", new string[] { "UPSUpdateHPPO", currenProduct.CUSTSN, hppo, avPartNo, rs.message });
                    //    }
                    //}
                    #endregion
                }
                 if (!hasUPSPartNo)  //都沒UPS AV PartNo throw error
                 {
                     throw new FisException("CQCHK1117", new string[] { currenProduct.CUSTSN, currenProduct.Model,  
                         string.Join(",",matchPartList.Select(x=>x.PN).ToArray()) ,
                         string.Join(",", matchPartList.Select(x=>x.Attributes.Where(y=>y.InfoType=="AV" && !string.IsNullOrEmpty(y.InfoValue))
                                                                                        .Select(z=>z.InfoValue).FirstOrDefault()??"").ToArray()) });
                 }
            } 

            return base.DoExecute(executionContext);
        }

        private Dictionary<string, string> getTagOrderValue(string tagOrderData)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            string[] order = tagOrderData.Split(new char[] { ';' });
            foreach (string data in order)
            {
                int index = data.IndexOf('=');
                string name = data.Substring(0,index).Trim();
                string value = data.Substring(index+1).Trim();
                ret.Add(name,value);
            }
            return ret;
        }
    }


    
}
