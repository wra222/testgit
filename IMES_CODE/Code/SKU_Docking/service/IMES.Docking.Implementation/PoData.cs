/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:IMES service implement for PoData (for docking) Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI PoData to IMES for Docking.docx
 * UC:CI-MES12-SPEC-PAK-UC PoData to IMES for Docking.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-06-06  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;
using System.Globalization; 
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Docking.Interface.DockingIntf;
using log4net;
using IMES.FisObject.Common.Part;
namespace IMES.Docking.Implementation
{
    /// <summary>
    /// IMES service for PoData.
    /// </summary>
    public class _PoData : MarshalByRefObject, IPoData
    {
        private struct S_DNLine
        {
            public string DeliveryNo;
            public DateTime ShipDate;
            public string PoNo;
            public string Model;
            public string ShipmentNo;
            public string strQty;
            public int Qty;
            public Hashtable HDeliveryInfo;
        };
        
        private struct S_PNLine
        {
            public string DeliveryNo;
            public string PalletNo;
            public short DeliveryQty;
            public string PalletType;
            public string UCC;
            public int DeviceQty;
        };

        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        private IDeliveryRepository deliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
        private IPalletRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
        private IPizzaRepository pizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
        private IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();

        #region IPoData Members
        private int fixQty(string str)
        {
            if (str == null)
            {
                return 0;
            }

            string input = str.Trim();
            if (input.Length <= 0)
            {
                return 0;
            }

            bool bHasDot = false;
            for (int j = 0; j < input.Length; j++)
            {
                if ((input[j] < '0' || input[j] > '9') && input[j] != '.')
                {
                    return 0;
                }

                if (input[j] == '.')
                {
                    if (bHasDot) return 0;
                    bHasDot = true;
                }
            }

            if (bHasDot)
            {
                return int.Parse(input.Substring(0, input.IndexOf('.')));
            }
            else
            {
                return int.Parse(input);
            }
        }

        //<UC:2.1>文件中各个数据项需要删除掉字串尾部的空格[trim各字段实现]
        private S_DNLine ParseDNInfo(string line)
        {
            S_DNLine ret = new S_DNLine();
            ret.HDeliveryInfo = new Hashtable();
            //<UC:2.13>将Delivery属性值'&'替换成'^&'
            string[] fields = line.Replace("&", "^&").Split('~');
            if (fields.Length < 75)
            {
                throw new FisException("CHK502", new string[] { line.Substring(0, 30) + "..." });
            }
            ret.DeliveryNo = fields[0].Trim();

            //<UC:2.9>Delivery.ShipDate: 原文件中该数据格式是YYYYMMDD
            string shipdate = fields[1].Trim();
            ret.ShipDate = DateTime.ParseExact(shipdate, "yyyyMMdd", DateTimeFormatInfo.InvariantInfo); 
            ret.PoNo = fields[2].Trim();
            ret.HDeliveryInfo.Add("Customer", fields[3].Trim());
            ret.HDeliveryInfo.Add("Name1", fields[4].Trim());
            ret.HDeliveryInfo.Add("Name2", fields[5].Trim());
            ret.HDeliveryInfo.Add("ShiptoId", fields[6].Trim());
            ret.HDeliveryInfo.Add("Adr1", fields[7].Trim());
            ret.HDeliveryInfo.Add("Adr2", fields[8].Trim());
            ret.HDeliveryInfo.Add("Adr3", fields[9].Trim());
            ret.HDeliveryInfo.Add("Adr4", fields[10].Trim());
            ret.HDeliveryInfo.Add("City", fields[11].Trim());
            ret.HDeliveryInfo.Add("State", fields[12].Trim());
            ret.HDeliveryInfo.Add("Postal", fields[13].Trim());
            ret.HDeliveryInfo.Add("Country", fields[14].Trim());
            ret.HDeliveryInfo.Add("Carrier", fields[15].Trim());
            ret.HDeliveryInfo.Add("SO", fields[16].Trim());
            ret.HDeliveryInfo.Add("CustPo", fields[17].Trim());
            ret.Model = fields[18].Trim();
            ret.HDeliveryInfo.Add("BOL", fields[19].Trim());
            ret.HDeliveryInfo.Add("Invoice", fields[20].Trim());
            ret.HDeliveryInfo.Add("RetCode", fields[21].Trim());
            ret.HDeliveryInfo.Add("ProdNo", fields[22].Trim());
            ret.HDeliveryInfo.Add("IECSo", fields[23].Trim());
            ret.HDeliveryInfo.Add("IECSoItem", fields[24].Trim());
            ret.HDeliveryInfo.Add("PoItem", fields[25].Trim());
            ret.HDeliveryInfo.Add("CustSo", fields[26].Trim());
            ret.HDeliveryInfo.Add("ShipFrom", fields[27].Trim());
            ret.HDeliveryInfo.Add("ShipingMark", fields[28].Trim());
            ret.HDeliveryInfo.Add("Flag", fields[29].Trim());
            ret.HDeliveryInfo.Add("RegId", fields[30].Trim());
            ret.HDeliveryInfo.Add("BoxSort", fields[31].Trim());
            ret.HDeliveryInfo.Add("Duty", fields[32].Trim());
            ret.HDeliveryInfo.Add("RegCarrier", fields[33].Trim());
            ret.HDeliveryInfo.Add("Destination", fields[34].Trim());
            ret.HDeliveryInfo.Add("Department", fields[35].Trim());
            ret.HDeliveryInfo.Add("OrdRefrence", fields[36].Trim());
            ret.HDeliveryInfo.Add("DeliverTo", fields[37].Trim());
            ret.HDeliveryInfo.Add("Tel", fields[38].Trim());
            ret.HDeliveryInfo.Add("WH", fields[39].Trim());
            ret.ShipmentNo = fields[40].Trim();
            ret.HDeliveryInfo.Add("Consolidated", ret.ShipmentNo);
            if (ret.ShipmentNo == "")
            {
                //throw new FisException("CHK524", new string[] { ret.DeliveryNo });
                ret.ShipmentNo = ret.DeliveryNo;
            }
            else if (ret.ShipmentNo.Length >= 10)
            {
                ret.ShipmentNo = ret.ShipmentNo.Substring(0, 10);
            }
            ret.HDeliveryInfo.Add("SKU", fields[41].Trim());
            ret.HDeliveryInfo.Add("Deport", fields[42].Trim());
            ret.strQty = fields[43].Trim();
            ret.HDeliveryInfo.Add("CQty", fields[44].Trim());
            ret.HDeliveryInfo.Add("EmeaCarrier", fields[45].Trim());
            ret.HDeliveryInfo.Add("Plant", fields[46].Trim());
            ret.HDeliveryInfo.Add("ShipTp", fields[47].Trim());
            ret.HDeliveryInfo.Add("ConfigID", fields[48].Trim());
            ret.HDeliveryInfo.Add("HYML", fields[49].Trim());
            ret.HDeliveryInfo.Add("CustName", fields[50].Trim());
            ret.HDeliveryInfo.Add("ShipHold", fields[51].Trim());
            ret.HDeliveryInfo.Add("CTO-DS", fields[52].Trim());
            ret.HDeliveryInfo.Add("PackType", fields[53].Trim());
            ret.HDeliveryInfo.Add("PltType", fields[54].Trim());
            ret.HDeliveryInfo.Add("ShipWay", fields[55].Trim());
            ret.HDeliveryInfo.Add("Dept", fields[56].Trim());
            ret.HDeliveryInfo.Add("MISC", fields[57].Trim());
            ret.HDeliveryInfo.Add("ShipFromNme", fields[58].Trim());
            ret.HDeliveryInfo.Add("ShipFromAr1", fields[59].Trim());
            ret.HDeliveryInfo.Add("ShipFromCty", fields[60].Trim());
            ret.HDeliveryInfo.Add("ShipFromTl", fields[61].Trim());
            ret.HDeliveryInfo.Add("DnItem", fields[62].Trim());
            ret.HDeliveryInfo.Add("Price", fields[63].Trim());
            ret.HDeliveryInfo.Add("BoxReg", fields[64].Trim());
            ret.HDeliveryInfo.Add("850DAY", fields[65].Trim());
            ret.HDeliveryInfo.Add("PackingType", fields[66].Trim());
            ret.HDeliveryInfo.Add("PackingLV", fields[67].Trim());
            ret.HDeliveryInfo.Add("CnQty", fields[68].Trim());
            ret.HDeliveryInfo.Add("PalletQty", fields[69].Trim());
            ret.HDeliveryInfo.Add("UCC", fields[70].Trim());
            ret.HDeliveryInfo.Add("LabelFlg", fields[71].Trim());
            ret.HDeliveryInfo.Add("PAKType", fields[72].Trim());
            ret.HDeliveryInfo.Add("InvoiceNum", fields[73].Trim());

            #region Vincent add get item number from ConstValue table
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<ConstValueInfo> valueList = partRepository.GetConstValueListByType("SAPPOHeader");
            foreach (ConstValueInfo item in valueList)
            {
                int itemNum = 0;
                bool result = int.TryParse(item.name, out itemNum);
                if (result && !string.IsNullOrEmpty(item.value))
                {
                    itemNum = itemNum - 1;
                    if (fields.Length > itemNum)
                        ret.HDeliveryInfo.Add(item.value.Trim(), fields[itemNum].Trim());

                }
            }
            #endregion

            return ret;
        }

        private S_PNLine ParsePNInfo(string line)
        {
            S_PNLine ret = new S_PNLine();
            string[] fields = line.Split('~');
            if (fields.Length < 6)
            {
                throw new FisException("CHK503", new string[] { line.Substring(0, 30) + "..." });
            }
            ret.DeliveryNo = fields[0].Trim();
            ret.PalletNo = fields[1].Trim();
            ret.DeliveryQty = (short)fixQty(fields[2].Trim());
            ret.PalletType = fields[3].Trim();
            ret.UCC = fields[4].Trim();
            ret.DeviceQty = 0;
            if (fields.Length > 6)
            {
                ret.DeviceQty = fixQty(fields[6].Trim());
            }
            return ret;
        }

        private string GetUCC(S_DNLine dn)
        {
            string prefix = "";
            string regId = (string)dn.HDeliveryInfo["RegId"];
            string deport = (string)dn.HDeliveryInfo["Deport"];
            IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<string> valueList = PartRepository.GetValueFromSysSettingByName("Site");
            if (valueList.Count == 0)
            {
                throw new FisException("PAK095", new string[] { "Site" });
            }

            if (valueList[0] == "ICC")//重慶
            {
                IList<ConstValueInfo> boxIdPrefixList = PartRepository.GetConstValueListByType("BoxIdPreFixRule");
                var prefixList = (from p in boxIdPrefixList
                                  where p.name == regId
                                  select p.value).ToList();
                if (prefixList != null && prefixList.Count > 0)
                {
                    prefix = prefixList[0].Trim();
                }

                if (string.IsNullOrEmpty(prefix))
                {
                    throw new FisException("CQCHK0049", new string[] { "BoxIdPreFixRule", regId });
                }
            }
            else
            {
                if (regId == "SNA") prefix = "H410-Y";
                else if (regId == "SNL") prefix = "LA" + deport + "Y";
                else if (regId == "SNU") prefix = "D7" + deport + "Y";
                else if (regId == "SNE") prefix = "63D7-QY";
                else if (regId == "SAF") prefix = "H4FN-0C";
                else return "";
            }
            //Get BoxId
            //参考方法：
            //SELECT @BoxId=RTRIM(BoxId) 
            //	FROM SnoCtrl_BoxId 
            //		WHERE Cust=@title and  valid='1'
            SnoCtrlBoxIdInfo info = deliveryRepository.GetSnoCtrlBoxIdInfoByLikeCustAndValid(prefix, "1");
            if (info == null)
            {
                return "";
            }

            //Delete SnoCtrl_BoxId
            //参考方法：
            //IF @BoxId IS NOT NULL
            //	DELETE FROM SnoCtrl_BoxId WHERE BoxId=@BoxId
            deliveryRepository.DeleteSnoCtrlBoxIdInfo(info.id);

            return info.boxId;
        }

        /// <summary>
        /// 上传PO Data
        /// </summary>
        public IList<DNForUI> UploadData(IList<string> dnLines, IList<string> pnLines, string editor, out long sum)
        {
            logger.Debug("(_PoData)UploadData starts");

            if (dnLines.Count == 0 && pnLines.Count == 0)
            {
                throw new FisException("CHK257", new string[] { });
            }

            try
            {
                Hashtable hDnList = new Hashtable();
                IList<S_PNLine> pnList = new List<S_PNLine>();
                IList<string> toAddBAList = new List<string>();

                foreach (string tmp in dnLines)
                {
                    S_DNLine ele = ParseDNInfo(tmp);

                    //若DN文件中包含多个相同DN记录，报文件错误
                    if (hDnList.Contains(ele.DeliveryNo))
                    {
                        throw new FisException("CHK504", new string[] { ele.DeliveryNo });
                    }

                    //mantis 2282: Docking 内销船务资料 （PoData to IMES ）页面上传不上去报错‘没有成功上传的记录！’
                    //<UC:2.15>若Delivery中包含属性RegId='SCN'，不导入
                    //if ((string)ele.HDeliveryInfo["RegId"] == "SCN")
                    //{
                    //    continue;
                    //}

                    //<UC:2.3>若Delivery属性RegId= 'SUC'，则设置为RegId= 'SNU'
                    string regId= (string)ele.HDeliveryInfo["RegId"];
                    string postRegId="";
                    if (!string.IsNullOrEmpty(regId) && regId.Length>2 )
                    {
                        postRegId = regId.Substring(1, 2);
                    }

                    if (postRegId == "UC")
                    {
                        ele.HDeliveryInfo["RegId"] = regId.Substring(0, 1) + "NU";
                    }

                    //if ((string)ele.HDeliveryInfo["RegId"] == "SUC")
                    //{
                    //    ele.HDeliveryInfo["RegId"] = "SNU";
                    //}

                    //<UC:2.10>将Delivery.ShipmentNo作为属性Consolidated保存在DeliveryInfo表中，若属性Flag='O' 或 'C'时，属性Consolidated改为RedShipment;Delivery.ShipmentNo只取数据的前10位保存
                    //2012-9-25:删除：若属性Flag=O 或 C时，属性Consolidated改为RedShipment
                    //if ((string)ele.HDeliveryInfo["Flag"] == "O" || (string)ele.HDeliveryInfo["Flag"] == "C")
                    //{
                        //ele.HDeliveryInfo.Add("RedShipment", ele.ShipmentNo);
                    //}
                    //else
                    //{
                    //    ele.HDeliveryInfo.Add("Consolidated", ele.ShipmentNo);
                    //}

                    //<UC:2.5>将Delivery.Model对应的原文件的值作为属性PartNo保存在DeliveryInfo里
                    ele.HDeliveryInfo.Add("PartNo", ele.Model);

                    //<UC:2.6>原文件的Delivery.Model若包含'/'，则只取 '/' 前的字串；否则是全部字串保存到Delivery.Model
                    string tmpModel = ele.Model;
                    if (tmpModel.Contains('/'))
                    {
                        ele.Model = tmpModel.Substring(0, tmpModel.IndexOf('/'));
                    }

                    //<UC:2.7>如果Delivery.Model 不为空，需要使用按照如下方法重新确定Delivery.Model(查ModelInfo表获取新的Model值)
                    if (ele.Model != "")
                    {
                        IList<string> newModelList = modelRepository.GetModelByNameAndValue("PN", ele.Model);
                        if (newModelList.Count > 0)
                        {
                            ele.Model = newModelList[0];
                        }
                    }

                    hDnList.Add(ele.DeliveryNo, ele);

                    if ((tmp.Contains("~BNAF~") || tmp.Contains("~DZNA~")) && 
                        (tmp.Contains("~SNA~") || tmp.Contains("~SAF~") || tmp.Contains("~QNA~") || tmp.Contains("~QAF~")))
                    {
                        toAddBAList.Add(ele.DeliveryNo);
                    }
                }

                Hashtable hDnQty = new Hashtable();
                foreach (string tmp in pnLines)
                {
                    S_PNLine ele = ParsePNInfo(tmp);

                    if (!hDnList.Contains(ele.DeliveryNo))
                    {
                        continue;
                    }

                    //<UC:2.2>对于Pallet当PalletType前两码不是ZP(散装出货)时，如果其对应的Delivery对应的文本文件的属性串中包含（'~BNAF~'或'~DZNA~'）并且同时包含（'~SNA~'或'~SAF~'）字串时，PalletNo前加上'BA'；否则PalletNo前加上'BA'
                    if (!ele.PalletType.StartsWith("ZP"))
                    {
                        if (toAddBAList.Contains(ele.DeliveryNo))
                        {
                            ele.PalletNo = "BA" + ele.PalletNo;
                        }
                        else
                        {
                            ele.PalletNo = "NA" + ele.PalletNo;
                        }
                    }

                    if (hDnQty.Contains(ele.DeliveryNo))
                    {
                        hDnQty[ele.DeliveryNo] = fixQty(hDnQty[ele.DeliveryNo].ToString()) + (int)ele.DeliveryQty;
                    }
                    else
                    {
                        hDnQty.Add(ele.DeliveryNo, (int)ele.DeliveryQty);
                    }

                    //<UC:2.8>若Pallet对应的Delivery不存在于Delivery 表，且(Delivery的RegId属性值等于SNU，and UCC='' and Pallet前2位不等于NA)时，需要重新获取Pallte的UCC，若没有得到，则连同其对应的Delivery都不导入
                    Delivery tmpDlv = deliveryRepository.Find(ele.DeliveryNo);
                    if (tmpDlv == null && ele.UCC == "" && !ele.PalletNo.StartsWith("NA"))
                    {
                        S_DNLine tmpDN = (S_DNLine)hDnList[ele.DeliveryNo];
                        string tmpRegId = (string)tmpDN.HDeliveryInfo["RegId"];
                        if (!string.IsNullOrEmpty(tmpRegId) &&
                             tmpRegId.Length>2                  &&
                            tmpRegId.Substring(1,2) == "NU")
                        {
                            string ucc = GetUCC(tmpDN);
                            if (ucc == "")
                            {
                                /*
                                 * Answer to: ITC-1414-0215
                                 * Description: [新需求]在获取Box Id 失败的时候，需要报错并终止流程， 整机和docking都需要修改
                                 */
                                throw new FisException("PAK098", new string[] { tmpDN.DeliveryNo });
                                //hDnList.Remove(ele.DeliveryNo);
                                //continue;
                            }
                            ele.UCC = ucc;
                        }
                    }

                    pnList.Add(ele);
                }

                Hashtable tmpHT = hDnList.Clone() as Hashtable;
                hDnList.Clear();

                foreach (DictionaryEntry de in tmpHT)
                {
                    S_DNLine ele = (S_DNLine)de.Value;

                    //<UC:2.4>若Delivery.Qty=''时，数量等于SUM(Delivery_Pallet.DeliveryQty) GROUP BY DeliveryNo；否则参考以下语句作转换：SELECT CONVERT(int,CONVERT(float (18),CONVERT(decimal(15,3),RTRIM(Delivery.Qty))))[对第四位小数四舍五入，然后向下取整]
                    if (ele.strQty == "")
                    {
                        ele.Qty = fixQty(hDnQty[ele.DeliveryNo].ToString());
                    }
                    else
                    {
                        ele.Qty = fixQty(ele.strQty);
                    }

                    //<UC:2.17>如果Delivery.Qty > 150，且Delivery.Model 以'PC'开头，且Delivery 结合的Pallet 以NA 开头，则不导入此记录
                    if (ele.Qty > 150 && ele.Model.StartsWith("PC"))
                    {
                        bool bSkip = false;
                        foreach (S_PNLine tmp in pnList)
                        {
                            if (tmp.DeliveryNo == ele.DeliveryNo && tmp.PalletNo.StartsWith("NA"))
                            {
                                bSkip = true;
                                break;
                            }
                        }
                        if (bSkip) continue;
                    }

                    //<UC:2.14>若Delivery对应的(PartNo(Model)前两位='PC'或PO)且PartNo NOT LIKE RTRIM(Model)+ '/%'时，不导入此记录
                    if ((ele.Model.StartsWith("PC") || ele.Model.StartsWith("PO")) && !(((string)ele.HDeliveryInfo["PartNo"]).StartsWith(ele.Model + "/")))
                    {
                        continue;
                    }

                    hDnList.Add(de.Key, ele);
                }

                //上传DN
                IList<Delivery> deliveryList = new List<Delivery>();
                //IList<string> updateList = new List<string>();
                foreach (DictionaryEntry de in hDnList)
                {
                    S_DNLine ele = (S_DNLine)de.Value;

                    //<UC:2.12>若Delivery已存在则不再导入，但DB中的Udt还需要Update
                    //仅Update的也放在Activity中处理（2012/10/09）
                    /*if (null != deliveryRepository.Find(ele.DeliveryNo))
                    {
                        updateList.Add(ele.DeliveryNo);
                        continue;
                    }
                    */
                    Delivery toSave = new Delivery();
                    toSave.DeliveryNo = ele.DeliveryNo;
                    toSave.ShipDate = ele.ShipDate;
                    toSave.PoNo = ele.PoNo;
                    toSave.ModelName = ele.Model;
                    toSave.ShipmentNo = ele.ShipmentNo;
                    toSave.Qty = ele.Qty;
                    //<UC:2.11>(1)Delivery.Status=00
                    toSave.Status = "00";
                    toSave.Editor = editor;
                    foreach (DictionaryEntry eleInfo in ele.HDeliveryInfo)
                    {
                        //<UC:2.16>RegId 如果解析上传文件得到空值，需要将该属性按照空串记录到DeliveryInfo 表中
                        if (((string)eleInfo.Value) == "" && ((string)eleInfo.Key) != "RegId") continue;
                        toSave.SetExtendedProperty((string)eleInfo.Key, eleInfo.Value, editor);
                    }
                    deliveryList.Add(toSave);
                }

                //上传PN
                IList<Pallet> palletList = new List<Pallet>();
                IList<DeliveryPalletInfo> dpList = new List<DeliveryPalletInfo>();
                IList<string> savePnList = new List<string>();
                foreach (S_PNLine ele in pnList)
                {
                    //<UC:2.19>当Delivery不导入时，Pallet也不需要导入
                    if (!hDnList.Contains(ele.DeliveryNo))
                    {
                        continue;
                    }
                    
                    if (!savePnList.Contains(ele.PalletNo))
                    {
                        Pallet toSave = new Pallet();
                        toSave.PalletNo = ele.PalletNo;
                        toSave.UCC = ele.UCC;
                        //<UC:2.11>(2)Pallet.Station=00
                        toSave.Station = "00";
                        toSave.Editor = editor;
                        palletList.Add(toSave);
                        savePnList.Add(ele.PalletNo);
                    }

                    DeliveryPalletInfo toSave1 = new DeliveryPalletInfo();
                    toSave1.deliveryNo = ele.DeliveryNo;
                    toSave1.palletNo = ele.PalletNo;
                    toSave1.shipmentNo = ((S_DNLine)hDnList[ele.DeliveryNo]).ShipmentNo;
                    toSave1.deliveryQty = ele.DeliveryQty;
                    //<UC:2.11>(3)Delivery_Pallet.Status=0
                    toSave1.status = "0";
                    toSave1.editor = editor;

                    toSave1.deviceQty = ele.DeviceQty;
                    toSave1.palletType = ele.PalletType.Substring(0, 2);

                    dpList.Add(toSave1);
                }
                
                FisException ex;
                List<string> erpara = new List<string>();
                Session.SessionType TheType = Session.SessionType.Common;
                string sessionKey = "UPLOAD_PO_DATA_DOCKING";

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, "", "", "");

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", "");
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", "");
                    wfArguments.Add("SessionType", TheType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("UploadPoDataForDocking.xoml", "", wfArguments);

                    currentSession.SetInstance(instance);
                    currentSession.AddValue("DeliveryList", deliveryList);
                    currentSession.AddValue("PalletList", palletList);
                    currentSession.AddValue("DeliveryPalletList", dpList);
                    //currentSession.AddValue("UpdateUDTDNList", updateList);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK192", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK193", erpara);
                    throw ex;
                }


                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }

                //获取界面需要显示的本次上传DN列表
                return deliveryRepository.GetDNListByDNList(new List<string>(hDnList.Keys.Cast<string>()), out sum);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(_PoData)UploadData end");
            }
        }
        
        /// <summary>
        /// 获取符合输入条件的DN列表
        /// </summary>
        public IList<DNForUI> QueryData(DNQueryCondition cond, out int realCount, out long sum)
        {
            logger.Debug("(_PoData)QueryData start");
            try
            {
                return deliveryRepository.GetDNListByCondition(cond, out realCount, out sum);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(_PoData)QueryData end");
            }
        }

        public class IcpDNInfoForUI : IComparer<DNInfoForUI>
        {
            public int Compare(DNInfoForUI x, DNInfoForUI y)
            {
                return x.InfoType.CompareTo(y.InfoType);
            }
        }

        /// <summary>
        /// 获取DN属性列表
        /// </summary>
        public IList<DNInfoForUI> GetDNInfoList(string dn)
        {
            logger.Debug("(_PoData)GetDNInfoList start, DeliveryNo:" + dn);
            try
            {
                return deliveryRepository.GetDNInfoList(dn);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(_PoData)GetDNInfoList end, DeliveryNo:" + dn);
            }
        }

        /// <summary>
        /// 获取DN对应的Pallet列表
        /// </summary>
        public IList<DNPalletQty> GetDNPalletList(string dn)
        {
            logger.Debug("(_PoData)GetDNPalletList start, DeliveryNo:" + dn);
            try
            {
                return deliveryRepository.GetPalletList(dn);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(_PoData)GetDNPalletList end, DeliveryNo:" + dn);
            }
        }

        /// <summary>
        /// 获取Shipment对应的PalletCapacity列表
        /// </summary>
        public IList<PalletCapacityInfo> GetDNPalletCapacityList(string ship)
        {
            logger.Debug("(_PoData)GetDNPalletCapacityList start, ShipmentNo.:" + ship);
            try
            {
                return palletRepository.GetPalletCapacityInfoList(ship);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(_PoData)GetDNPalletCapacityList end, ShipmentNo.:" + ship);
            }
        }

        /// <summary>
        /// 删除DN
        /// </summary>
        public int DeleteDN(string dn, string editor)
        {
            logger.Debug("(_PoData)DeleteDN start, DeliveryNo:" + dn);

            try
            {
                //DN已与unit绑定
                if (productRepository.GetCombinedQtyByDN(dn) != 0)
                {
                    return -1;
                }

                //DN为SAP分配栈板
                if (deliveryRepository.Find(dn).ShipmentNo.Length == 16 && deliveryRepository.GetDeliveryInfoValue(dn, "PAKType") == "A")
                {
                    return -2;
                }

                FisException ex;
                List<string> erpara = new List<string>();
                Session.SessionType TheType = Session.SessionType.Common;
                string sessionKey = dn;

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, "", "", "");

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", "");
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", "");
                    wfArguments.Add("SessionType", TheType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("PoDataDeleteDNForDocking.xoml", "", wfArguments);

                    currentSession.SetInstance(instance);
                    currentSession.AddValue(Session.SessionKeys.DeliveryNo, dn);
                    currentSession.AddValue("Project", "Docking");

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK192", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK193", erpara);
                    throw ex;
                }


                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }

                return 0;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(_PoData)DeleteDN end, DeliveryNo:" + dn);
            }
        }

        /// <summary>
        /// 删除Shipment
        /// </summary>
        public int DeleteShipment(string ship, string editor)
        {
            logger.Debug("(_PoData)DeleteShipment start, ShipmentNo:" + ship);

            try
            {
                //DN已与unit绑定
                if (productRepository.GetCombinedQtyByShipmentNo(ship) != 0)
                {
                    return -1;
                }

                FisException ex;
                List<string> erpara = new List<string>();
                Session.SessionType TheType = Session.SessionType.Common;
                string sessionKey = ship;

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, "", "", "");

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", "");
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", "");
                    wfArguments.Add("SessionType", TheType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("PoDataDeleteDNForDocking.xoml", "", wfArguments);

                    currentSession.SetInstance(instance);
                    currentSession.AddValue("ShipmentNo", ship);
                    currentSession.AddValue("Project", "Docking");

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK192", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK193", erpara);
                    throw ex;
                }


                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }

                return 0;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(_PoData)DeleteShipment end, ShipmentNo:" + ship);
            }
        }

        public IList<PrintItem> GetPrintTemplate(string customer, IList<PrintItem> printItems)
        {
            logger.Debug("(_PoData)GetPrintTemplate start");

            try
            {
                FisException ex;
                List<string> erpara = new List<string>();
                Session.SessionType TheType = Session.SessionType.Common;
                string sessionKey = Guid.NewGuid().ToString();

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, "", "", "", "");

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", "");
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", "");
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", TheType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("PrintShipToLabelFRUForDocking.xoml", "", wfArguments);

                    currentSession.SetInstance(instance);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK192", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK193", erpara);
                    throw ex;
                }


                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }

                object printObject = currentSession.GetValue(Session.SessionKeys.PrintItems);
                if (printObject == null)
                {
                    return null;
                }

                return (IList<PrintItem>)printObject;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(_PoData)GetPrintTemplate end");
            }
        }

        #endregion
    }
}
