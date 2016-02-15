/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Activity/ITCNDCheckProduct
 * UI:CI-MES12-SPEC-FA-UI ITCND Check.docx –2011/10/10 
 * UC:CI-MES12-SPEC-FA-UC ITCND Check.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-11   zhanghe               (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* 2)检查机型是否被QC Hold住
* 检查Model.Status=0 提示“此机型被QC Hold”
* 3)检查机器是否被QC Hold住
* 检查Product.State=’H’ 提示“此机器被QC Hold”
* 4）if exists (select * from ProductInfo where a.ProductID=@PrdId InfoType='Exp') and InfoValue<>’’)
* 提示“測試機器，請勿出貨”
* 注：原来逻辑的QASpcList和LocalMaintain（Tp='Exp'）未使用
* 5)if exists(select Model from TSModel  ( nolock )  where Model=@Model and Mark='0')
* select '0','該機型不需要做ICTnD Check.'
* 5）ProductLog 表latest record WC=66, IsPass=0
* 提示“ITCND Fail, 請重新測試”
* 6）if not exist(select Model from TSModel  ( nolock )  where Model=@Model and Mark='1')
* ProductLog 表latest record WC=66, IsPass=1的记录的Udt后存在EPIA或SLAM记录WC in(73,74)记录，则提示“EPIA後,請重新DownLoad Image”
* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
*/
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.MO;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.Net;
//using System.IO;
using System.Xml;
using System.Text;

namespace IMES.Activity
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于ITCND Check
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    /// </para> 
    /// </remarks>
    public partial class ITCNDCheckProduct : BaseActivity
    {
        /// <summary> 
        /// </summary>
        public ITCNDCheckProduct()
        {
            InitializeComponent();
        }

        /// <summary> 
        /// </summary>        
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //TODO 1
            ProductLog testLog = new ProductLog();
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            var product = CurrentSession.GetValue(Session.SessionKeys.Product) as IProduct;
            if (product == null)
            {
                throw new NullReferenceException("Product in session is null");
            }
            Model model = product.ModelObj;
            if (model == null)
            {
                List<string> errpara = new List<string>();
                errpara.Add(product.ProId);
                throw new FisException("CHK804", errpara);
            }
            if (model.Status == "0")
            {
                List<string> errpara = new List<string>();
                throw new FisException("CHK802", errpara);
            }
            if(product.State == "H")
            {
                List<string> errpara = new List<string>();
                throw new FisException("CHK802", errpara);
            }
            string value = (string)product.GetExtendedProperty("Exp");
            if (!String.IsNullOrEmpty(value))
            {
                List<string> errpara = new List<string>();                
                throw new FisException("CHK803", errpara);
            }

            #region mark by Vincent
            //IList<TsModelInfo> tsInfo = null;

            //TsModelInfo cond = new TsModelInfo();
            //cond.model = product.Model;
            //cond.mark = "0";
            //tsInfo = productRepository.GetTsModelList(cond);
            //if(tsInfo != null && tsInfo.Count != 0)
            //{
            //    List<string> errpara = new List<string>();                
            //    throw new FisException("CHK805", errpara);           
            //}
            ////
            
            //IList<ProductLog> allLogs = new List<ProductLog>();


            //cond.model = product.Model;
            //cond.mark = "1";
            //tsInfo = productRepository.GetTsModelList(cond);
            //if(tsInfo == null || tsInfo.Count == 0)
            //{
            //    allLogs = product.ProductLogs;
            //    IList<ProductLog> logs = new List<ProductLog>();
            //    ProductLog maxLog = new ProductLog();
            //    bool bExist = false;
            //    if(allLogs != null && allLogs.Count != 0)
            //    {
            //        foreach(ProductLog temp in allLogs)
            //        {
            //            if(temp.Station == "66" && temp.Status == StationStatus.Pass)
            //            {
            //                bExist = true;
            //                if(temp.Cdt.CompareTo(maxLog.Cdt) > 0)
            //                {
            //                    maxLog = temp;
            //                }
            //            }
            //            //else if(temp.Station == "73" || temp.Station == "74")
            //            else if(temp.Station == "6A")
            //            {
            //                logs.Add(temp);
            //            }
            //        }
            //        if (bExist == true)
            //        {
            //            foreach (ProductLog temp in logs)
            //            {
            //                if (temp.Cdt.CompareTo(maxLog.Cdt) > 0)
            //                {
            //                    List<string> errpara = new List<string>();
            //                    throw new FisException("CHK807", errpara);
            //                }
            //            }
            //        }
            //    }                
            //}

            ////UC 7
            //allLogs = product.ProductLogs;
            //if (allLogs != null && allLogs.Count > 0)
            //{
            //    ProductLog maxLog = new ProductLog();
            //    foreach (ProductLog temp in allLogs)
            //    {
            //        if (temp.Cdt.CompareTo(maxLog.Cdt) > 0)
            //        {
            //            maxLog = temp;
            //        }
            //    }
            //    if (maxLog.Station == "66" && maxLog.Status == StationStatus.Fail)
            //    {
            //        List<string> errpara = new List<string>();
            //        throw new FisException("CHK806", errpara);
            //    }

            //    if (!(maxLog.Station == "66" && maxLog.Status == StationStatus.Pass))
            //    {
            //        List<string> errpara = new List<string>();
            //        throw new FisException("CHK845", errpara);
            //    }
            //}
            #endregion

            //UC 8
            //ITC-1360-1787
            string[] code = new string[2];
            code[0] = product.Model;
            code[1] = product.CUSTSN;
            bool isHold = false;
            isHold = productRepository.CheckExistItcndCheckQcHold("1", code);
            if (isHold == true)
            {
                List<string> errpara = new List<string>();
                throw new FisException("CHK847", errpara);
            }

            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

            // mantis 485
            bool needCheck6R = true;
            IList<ConstValueTypeInfo> lstConstValueType = partRepository.GetConstValueTypeList("NoCheckRunInTimeSN");
            if (null != lstConstValueType)
                foreach (ConstValueTypeInfo vi in lstConstValueType)
                {
                    if (null != product.CUSTSN && product.CUSTSN.Equals(vi.value))
                    {
                        needCheck6R = false;
                        break;
                    }
                }

            if (needCheck6R)
            {
                lstConstValueType = partRepository.GetConstValueTypeList("NoCheckRunInTimeModel");
                if (null != lstConstValueType)
                {
                    foreach (ConstValueTypeInfo vi in lstConstValueType)
                    {
                        if (null != product.Model && product.Model.Equals(vi.value))
                        {
                            needCheck6R = false;
                            break;
                        }
                    }

                    if (needCheck6R)
                    {
                        foreach (ConstValueTypeInfo vi in lstConstValueType)
                        {
                            if (null != product.Family && product.Family.Equals(vi.value))
                            {
                                needCheck6R = false;
                                break;
                            }
                        }
                    }
                }
            }


            //UC 9
            IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            //Marked by Benson at 20130517
          //  IList<MoBOMInfo> mbinfo = new List<MoBOMInfo>();
            //mbinfo = bomRepository.GetPnListByModelAndBomNodeTypeAndDescr(product.Model, "P1", "ECOA");
            //Marked by Benson at 20130517
            IList<MoBOMInfo> mbinfo = new List<MoBOMInfo>();
            mbinfo = GetWin8Bom(product.Model);
            if (mbinfo != null && mbinfo.Count > 0)
           {
               if (NeedCheckOA3Time(product))
               {
                   DateTime win8Time;
                   DateTime logTime;
                   //('P/N','KEY','HASH')
                   string[] infotypes = new string[3];
                   infotypes[0] = "P/N";
                   infotypes[1] = "KEY";
                   infotypes[2] = "HASH";
                   win8Time = productRepository.GetNewestCdtFromProductInfo(product.ProId, infotypes);
                   logTime = productRepository.GetNewestCdtFromProductLog(product.ProId, "66", 1);

                   if (win8Time == DateTime.MinValue || logTime == DateTime.MinValue)
                   {
                       List<string> errpara = new List<string>();
                       //throw new FisException("CHK900", errpara);
                       errpara.Add("");
                       // ImageDownload 失败 %1
                       throw new FisException("CQCHK0034", errpara);
                   }

                   // mantis 430, FA ITCNDTS 站卡win8 key上传时间 需要维护
                   int win8TimeoutSecond = 60;
                   IList<string> valueList = partRepository.GetValueFromSysSettingByName("Win8KeyUploadTimeOut");
                   if (valueList != null && valueList.Count > 0)
                   {
                       win8TimeoutSecond = Convert.ToInt16(valueList[0]);
                   }
                   IList<ConstValueInfo> constlist = partRepository.GetConstValueListByType("Win8KeyUploadTimeOut").Where(x=>x.name==product.Family).ToList();
                   if (constlist != null && constlist.Count > 0)
                   {
                       win8TimeoutSecond = Convert.ToInt16(constlist[0].value);
                   }
                   if (DateTime.Compare(win8Time, logTime.AddSeconds(win8TimeoutSecond)) > 0 ||
                       DateTime.Compare(logTime, win8Time.AddSeconds(win8TimeoutSecond)) > 0)
                   {
                       List<string> errpara = new List<string>();
                       throw new FisException("CHK901", errpara);
                   }
               }

                //UC10
                //最新的P/N：@ImgPN 
                //（ProductInfo.InfoValue Condtion: Uppder(ProductInfo.InfoType) = ‘P/N’ and ProductID=[ProductID] order by Udt desc）
                IList<string> infoTypes = new List<string>();
                infoTypes.Add("P/N");
                IList<IMES.FisObject.FA.Product.ProductInfo> proInfos = new List<IMES.FisObject.FA.Product.ProductInfo>();
                proInfos = productRepository.GetProductInfoListUpperCaseItemType(product.ProId, infoTypes);

                //获取BOM的ECOA的PN：@BOMPN （select b.PartNo from ModelBOM a (nolock), Part b (nolock) 
                //where a.Material = @Model
                //and a.Component = b.PartNo
                //and b.BomNodeType = 'P1'
                //and b.Descr LIKE 'ECOA%'）

                //若@ImgPN与@BOMPN不相等，则报错：“ImageDownload 失败，ECOA PN 错误
                bool bCompare = false;
                if (proInfos != null && proInfos.Count > 0)
                {
                    foreach (MoBOMInfo temp in mbinfo)
                    {
                        if (temp.component == proInfos[0].InfoValue)
                        {
                            bCompare = true;
                            break;
                        }
                    }
                }
                if (bCompare == false)
                {
                    List<string> errpara = new List<string>();
                    throw new FisException("CHK927", errpara);
                }
            }
            
            

            // UC11 OA3
            var site = CurrentSession.GetValue("Site") as string;
            if (!"ICC".Equals(site))
            {
                //UC11(win8)
                //前提条件：Select Upper(Value)  from ConstValue nolock
                //where Type = 'CheckOA3State'
                //and Name = left(ProductStatus.Line,1)
                //等于“N”，则不进行下面的操作；未维护数据或者不等于“N”，则继续执行下面的操作
                //请参考《CI-MES12-SPEC-PAK-UC Upload Shipment Data to SAP.docx》2.1.5 Step 5 Check OA3 状态

                IList<ConstValueInfo> cvInfo = new List<ConstValueInfo>();

                ConstValueInfo cvCond = new ConstValueInfo();
                cvCond.type = "CheckOA3State";
                cvCond.name = product.Status.Line.Substring(0, 1);
                cvInfo = partRepository.GetConstValueInfoList(cvCond);
                if (cvInfo == null || cvInfo.Count == 0)
                {
                    //do
                    CheckOA3(product);
                }
                else
                {
                    bool findN = false;
                    foreach (ConstValueInfo tmp in cvInfo)
                    {
                        if (tmp.value.ToUpper() == "N")
                        {
                            findN = true;
                            break;
                        }
                    }
                    if (findN == false)
                    {
                        //do
                        CheckOA3(product);
                    }
                }
            }

            // UC12 ICC run in time
            if ("ICC".Equals(site))
            {
                ProductLog log65 = productRepository.GetLatestLogByWcAndStatus(product.ProId, "65", 1);
                if (null == log65)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add("65");
                    throw new FisException("CHK1008", errpara); // 無記錄
                }
                DateTime time65 = log65.Cdt;
                //6R 不會上傳fail
                //ProductLog log6RFail = productRepository.GetLatestLogByWcAndStatus(product.ProId, "6R", 0);
                //if (log6RFail != null && log65.Cdt < log6RFail.Cdt)
                //{
                //    time65 = log6RFail.Cdt;
                //}

                

                ConstValueInfo cvCond = null;
                IList<ConstValueInfo> cvInfo = null;
                int allowedTime = -1;

                if (needCheck6R)
                {
                    // UC.a
                    ProductLog log6R = productRepository.GetLatestLogByWcAndStatus(product.ProId, "6R", 1);
                    //if (null == log6R || time65 > log6R.Cdt)
                    if (log6R == null)
                    {
                        List<string> errpara = new List<string>();
                        errpara.Add("6R");
                        throw new FisException("CHK1008", errpara); // 無記錄
                    }
                    DateTime time6R = log6R.Cdt;

                    cvCond = new ConstValueInfo();
                    cvCond.type = "ImageRuninTime";
                    cvCond.name = product.Family;
                    cvInfo = partRepository.GetConstValueInfoList(cvCond);
                    if (cvInfo == null || cvInfo.Count == 0 || string.IsNullOrEmpty(cvInfo[0].value) || "0".Equals(cvInfo[0].value))
                    {
                        List<string> errpara = new List<string>();
                        errpara.Add("Image run in time");
                        throw new FisException("CHK1009", errpara); // Image run in time 未設定時間差，請聯繫IE設定
                    }

                    allowedTime = Convert.ToInt16(cvInfo[0].value);
                    if (allowedTime > Math.Abs((time6R - time65).TotalMinutes))
                    {
                        List<string> errpara = new List<string>();
                        errpara.Add("Image run in test");
                        throw new FisException("CHK1010", errpara); // Image run in test不足
                    }

                    // UC.b
                    ProductLog log66 = productRepository.GetLatestLogByWcAndStatus(product.ProId, "66", 1);
                    if (null == log66 || log65.Cdt > log66.Cdt)
                    {
                        List<string> errpara = new List<string>();
                        errpara.Add("66");
                        throw new FisException("CHK1008", errpara); // 無記錄
                    }
                    DateTime time66 = log66.Cdt;

                    cvCond = new ConstValueInfo();
                    cvCond.type = "PostTestTime";
                    cvCond.name = product.Family;
                    cvInfo = partRepository.GetConstValueInfoList(cvCond);
                    if (cvInfo == null || cvInfo.Count == 0 || string.IsNullOrEmpty(cvInfo[0].value) || "0".Equals(cvInfo[0].value))
                    {
                        List<string> errpara = new List<string>();
                        errpara.Add("Post Test time");
                        throw new FisException("CHK1009", errpara); // Post Test time 未設定時間差，請聯繫IE設定
                    }
                    allowedTime = Convert.ToInt16(cvInfo[0].value);
                    if (allowedTime < (DateTime.Now - time66).TotalMinutes)
                    {
                        List<string> errpara = new List<string>();
                        errpara.Add("Post test");
                        throw new FisException("CQCHK1004", errpara); // Post test 超時
                    }
                }
                
            }

            // UC ICC bios
            if ("ICC".Equals(site))
            {
                ConstValueInfo cvCond = new ConstValueInfo();
                cvCond.type = "BIOSVer";
                cvCond.name = product.Model;
                IList<ConstValueInfo> cvInfo = partRepository.GetConstValueInfoList(cvCond);
                if (cvInfo == null || cvInfo.Count == 0)
                {
                    cvCond.name = product.Model.Substring(0, 6);
                    cvInfo = partRepository.GetConstValueInfoList(cvCond);
                }
                if (cvInfo != null && cvInfo.Count > 0)
                {
                    string[] BIOSVerList = cvInfo[0].value.Split('~');

                    List<string> infoTypes = new List<string>();
                    infoTypes.Add("BIOS");
                    IList<IMES.FisObject.FA.Product.ProductInfo> proInfos = productRepository.GetProductInfoListUpperCaseItemType(product.ProId, infoTypes);
                    if (proInfos != null && proInfos.Count > 0)
                    {
                        foreach (IMES.FisObject.FA.Product.ProductInfo pi in proInfos)
                        {
                            if (string.IsNullOrEmpty(pi.InfoValue))
                            {
                                List<string> errpara = new List<string>();
                                errpara.Add("BIOS");
                                throw new FisException("CHK1011", errpara); // 相关信息还未上传
                            }
                            bool bCompareBios = false;
                            for (int i = 0; i < BIOSVerList.Length; i++)
                            {
                                if (pi.InfoValue.Equals(BIOSVerList[i]))
                                {
                                    bCompareBios = true;
                                    break;
                                }
                            }
                            if (!bCompareBios)
                            {
                                List<string> errpara = new List<string>();
                                errpara.Add("BIOS");
                                throw new FisException("CHK1012", errpara); // BIOS 版本錯誤
                            }
                        }
                    }
                }
                else   // throw error 
                {
                    throw new FisException("CHK1070", new string[] { product.Model });
                }
            }

            return base.DoExecute(executionContext);
        }
        private IList<MoBOMInfo> GetWin8Bom(string model)
        {
            IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<string> valueList = PartRepository.GetValueFromSysSettingByName("Site");
            if (valueList.Count == 0)
            { throw new FisException("PAK095", new string[] { "Site" }); }
            string site = valueList[0];
            IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IList<MoBOMInfo> mbinfo = new List<MoBOMInfo>();
            if (site == "IPC")
            {
                mbinfo = bomRepository.GetPnListByModelAndBomNodeTypeAndDescr(model, "P1", "ECOA");
           }
           else
            {
                IList<MoBOMInfo> mbinfoTmp = new List<MoBOMInfo>();
                mbinfoTmp = bomRepository.GetPnListByModelAndBomNodeTypeAndDescr(model, "P1", "%");
                foreach (MoBOMInfo m in mbinfoTmp)
                {
                    IPart p = PartRepository.Find(m.component);
                    if (p.Descr.ToUpper().Contains("COA (WIN8)"))
                    {
                        mbinfo.Add(m);
                    }
                }
            }
            return mbinfo;

        }
        private void CheckOA3(IProduct p)
        {
            bool bNeedCheckEOA = false;
            string fkiPath = "";
            CredentialCache FKICredentialCache = new CredentialCache();

            try
            {
                //currentSession.AddValue("FKIPATH", fkiPath);
                //currentSession.AddValue("FKIUSER", fkiUser);
                //currentSession.AddValue("FKIPWD", fkiPwd);

                fkiPath = (string)CurrentSession.GetValue("FKIPATH");
                //fkiPath = System.Configuration.ConfigurationManager.AppSettings["FKIServicePath"].Trim();
                if (!String.IsNullOrEmpty(fkiPath))
                {
                    try
                    {
                        //string fkiUser = System.Configuration.ConfigurationManager.AppSettings["FKIAuthUser"].Trim();
                        //string fkiPwd = System.Configuration.ConfigurationManager.AppSettings["FKIAuthPassword"].Trim();
                        string fkiUser = (string)CurrentSession.GetValue("FKIUSER");
                        string fkiPwd = (string)CurrentSession.GetValue("FKIPWD");
                        if (!String.IsNullOrEmpty(fkiUser))
                        {
                            if (fkiUser.Contains("\\"))
                            {
                                string user = fkiUser.Substring(fkiUser.IndexOf('\\') + 1);
                                string domain = fkiUser.Substring(0, fkiUser.IndexOf('\\'));
                                FKICredentialCache.Add(new System.Uri(fkiPath), "NTLM", new System.Net.NetworkCredential(user, fkiPwd, domain));
                            }
                            else
                            {
                                FKICredentialCache.Add(new System.Uri(fkiPath), "NTLM", new System.Net.NetworkCredential(fkiUser, fkiPwd));
                            }
                        }
                    }
                    catch { }
                    bNeedCheckEOA = true;
                }
            }
            catch { }


            try
            {
                IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                IHierarchicalBOM bom = bomRepository.GetHierarchicalBOMByModel(p.Model);

                string dn = p.DeliveryNo.Trim();

                if (bNeedCheckEOA && !CheckOA3Key(bom, p, fkiPath, FKICredentialCache))
                {
                    throw new FisException("PAK158", new string[] { p.CUSTSN, dn });
                }
            }
            catch (FisException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return;
        }



        private bool CheckOA3Key(IHierarchicalBOM bom, IProduct p, string fkiPath, CredentialCache FKICredentialCache)
        {
            //bool bWIN8 = false;
            //IList<IBOMNode> P1BomNodeList = bom.GetFirstLevelNodesByNodeType("P1");
            //foreach (IBOMNode bomNode in P1BomNodeList)
            //{
            //    if (bomNode.Part.Descr.StartsWith("ECOA"))
            //    {
            //        bWIN8 = true;
            //        break;
            //    }
            //}

            //if (!bWIN8) return true;
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            if (fkiPath == "") return true; //Switch off checking by FKIServer.
            string thisURI = "";
            if (fkiPath.EndsWith("/"))
            {
                thisURI = fkiPath + "UnitStatus";
            }
            else
            {
                thisURI = fkiPath + "/UnitStatus";
            }

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(thisURI);
            req.AllowAutoRedirect = true;
            req.CookieContainer = new CookieContainer();
            req.ContentType = "application/plain; charset=utf-8";
            req.Accept = "*/*";
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            req.KeepAlive = true;
            req.Method = "POST";
            req.Credentials = FKICredentialCache.GetCredential(new Uri(fkiPath), "NTLM");

            string XMLInputData = "<?xml version='1.0' encoding='utf-8' ?>";
            XMLInputData += "<UnitStatusRequest xmlns='http://HP.ITTS.OA30/digitaldistribution/2011/08'>";
            XMLInputData += "<HPSerialNumber>";
            XMLInputData += p.CUSTSN;
            XMLInputData += "</HPSerialNumber>";
            XMLInputData += "<ProductKeyID>";
            XMLInputData += "";
            XMLInputData += "</ProductKeyID>";
            XMLInputData += "</UnitStatusRequest>";
            Encoding encoding = Encoding.Default;
            byte[] buffer = encoding.GetBytes(XMLInputData);
            req.ContentLength = buffer.Length;
            req.GetRequestStream().Write(buffer, 0, buffer.Length);

            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            XmlTextReader xmlr = new XmlTextReader(res.GetResponseStream());
            string status = "";
            string rc = "";
            bool bError = false;
            bool bFound = false;
            StringBuilder str = new StringBuilder("Formatted Response:\n");

            while (xmlr.Read())
            {
                switch (xmlr.NodeType)
                {
                    case XmlNodeType.Element:
                        if (xmlr.IsEmptyElement)
                            str.AppendFormat("<{0}/>", xmlr.Name);
                        else
                            str.AppendFormat("<{0}>", xmlr.Name);
                        break;
                    case XmlNodeType.Text:
                        str.Append(xmlr.Value);
                        break;
                    case XmlNodeType.CDATA:
                        str.AppendFormat("<![CDATA[{0}]]>", xmlr.Value);
                        break;
                    case XmlNodeType.ProcessingInstruction:
                        str.AppendFormat("<?{0} {1}?>", xmlr.Name, xmlr.Value);
                        break;
                    case XmlNodeType.Comment:
                        str.AppendFormat("<!--{0}-->", xmlr.Value);
                        break;
                    case XmlNodeType.XmlDeclaration:
                        str.AppendFormat("<?xml version='1.0'?>");
                        break;
                    case XmlNodeType.DocumentType:
                        str.AppendFormat("<!DOCTYPE{0} [{1}]>", xmlr.Name, xmlr.Value);
                        break;
                    case XmlNodeType.EntityReference:
                        str.Append(xmlr.Name);
                        break;
                    case XmlNodeType.EndElement:
                        str.AppendFormat("</{0}>", xmlr.Name);
                        break;
                    case XmlNodeType.Whitespace:
                        str.Append("\n");
                        break;
                }

                if (xmlr.NodeType == System.Xml.XmlNodeType.Element && xmlr.LocalName.Equals("ReturnCode"))
                {
                    xmlr.Read();
                    str.Append(xmlr.Value);
                    rc = xmlr.Value.Trim();
                    if (rc != "000") bError = true;
                    continue;
                }

                if (xmlr.NodeType == System.Xml.XmlNodeType.Element && xmlr.LocalName.Equals("ReturnMessage"))
                {
                    if (bError)
                    {
                        xmlr.Read();
                        str.Append(xmlr.Value);
                        string msg = xmlr.Value;
                        xmlr.Close();
                        throw new Exception("[" + rc + "]:" + msg);
                    }
                }

                if (xmlr.NodeType == System.Xml.XmlNodeType.Element && xmlr.LocalName.Equals("ProductKeyStateName"))
                {
                    xmlr.Read();
                    str.Append(xmlr.Value);
                    status = xmlr.Value;

                    // Marked this by Benson at 2013/3/8
                    //  if (status == "NotifiedBound")
                    if (status == "NotifiedBound" || status == "FKIErrorBound")  
                    {
                        //只能有一行状态为"Bound"/"NotifiedBound"/"PendingBound" --OLD
                        //只能有一行状态为"Bound"/"NotifiedBound"/"PendingBound/FKIErrorBound" --NEW add by Benson at 2013/3/8
                        if (bFound)
                        {
                            xmlr.Close();
                            return false;
                        }
                        bFound = true;
                        continue;
                    }

                    if (status == "Bound" || status == "PendingBound")
                    {
                        //只能有一行状态为"Bound"/"NotifiedBound"/"PendingBound"
                        if (bFound)
                        {
                            xmlr.Close();
                            return false;
                        }
                        bFound = true;

                        //对于状态为Bound/PendingBound的Product需要记录到ProductInfo中
                        IMES.FisObject.FA.Product.ProductInfo item = new IMES.FisObject.FA.Product.ProductInfo();
                        item.ProductID = p.ProId;
                        item.InfoType = "Win8KeyState";
                        item.InfoValue = status;
                        item.Editor = this.Editor;
                        if (productRepository.CheckExistProductInfo(p.ProId, "Win8KeyState"))
                        {
                            IMES.FisObject.FA.Product.ProductInfo cond = new IMES.FisObject.FA.Product.ProductInfo();
                            cond.ProductID = p.ProId;
                            cond.InfoType = "Win8KeyState";
                            productRepository.UpdateProductInfoDefered(CurrentSession.UnitOfWork, item, cond);
                        }
                        else
                        {
                            productRepository.InsertProductInfoDefered(CurrentSession.UnitOfWork, item);
                        }

                        continue;
                    }
                    //Returned 或PendingReturn或NotifiedReturned或FKIErrorReturn --Mantis0001691
                    // Marked this by Benson at 2013/3/8
                //    if (status != "Returned" && status != "NotifiedReturned" && status != "PendingReturn")
                    if (status != "Returned" && status != "NotifiedReturned" && status != "PendingReturn" && status != "FKIErrorReturn")
                    {
                        xmlr.Close();
                        return false;
                    }
                }
            }
            xmlr.Close();
            Console.Write(str.ToString());
            return bFound;
        }

        private bool NeedCheckOA3Time(IProduct product)
        {
            bool needCheck6R = true;
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<ConstValueTypeInfo> lstConstValueType = partRepository.GetConstValueTypeList("NoCheckOA3TimeSN");
            if (null != lstConstValueType)
                foreach (ConstValueTypeInfo vi in lstConstValueType)
                {
                    if (null != product.CUSTSN && product.CUSTSN.Equals(vi.value))
                    {
                        needCheck6R = false;
                        break;
                    }
                }

            if (needCheck6R)
            {
                lstConstValueType = partRepository.GetConstValueTypeList("NoCheckOA3TimeModel");
                if (null != lstConstValueType)
                {
                    foreach (ConstValueTypeInfo vi in lstConstValueType)
                    {
                        if (null != product.Model && product.Model.Equals(vi.value))
                        {
                            needCheck6R = false;
                            break;
                        }
                    }

                    if (needCheck6R)
                    {
                        foreach (ConstValueTypeInfo vi in lstConstValueType)
                        {
                            if (null != product.Family && product.Family.Equals(vi.value))
                            {
                                needCheck6R = false;
                                break;
                            }
                        }
                    }
                }
            }
            return needCheck6R;
        }
    }
}