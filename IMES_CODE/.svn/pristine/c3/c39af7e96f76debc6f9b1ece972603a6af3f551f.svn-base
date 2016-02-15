// INVENTEC corporation (c)2014 all rights reserved. 
// Description: 產生IMEI Code 15碼
//                    
// Update: 
// Date                        Name                         Reason 
// ==========   =======================      ============
//2014-04-27   Vincent  for generate IMEI Code
// Known issues:
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.Infrastructure;
using IMES.Infrastructure.Utility.Generates.intf;
using IMES.FisObject.Common.NumControl;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Utility.Generates.impl;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.UnitOfWork;
using System;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Model;
using System.Text.RegularExpressions;
using IMES.DataModel;
using System.Collections.Generic;
using System.ComponentModel;
using IMES.Common;

namespace IMES.Activity
{
   /// <summary>
   /// Get IMEI
   /// </summary>
    public partial class AcquireIMEI : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public AcquireIMEI()
        {
            InitializeComponent();
        }
        /// <summary>
        ///  Store Product InfoType
        /// </summary>
        public static DependencyProperty InfoTypeProperty = DependencyProperty.Register("InfoType", typeof(string), typeof(AcquireIMEI), new PropertyMetadata("IMEI~IMEI_1"));

        /// <summary>
        /// 
        /// </summary>
        [DescriptionAttribute("Store Store Product InfoType")]
        [CategoryAttribute("InArguments Of AcquireIMEI")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string InfoType
        {
            get
            {
                return ((string)(base.GetValue(InfoTypeProperty)));
            }
            set
            {
                base.SetValue(InfoTypeProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            IProduct curProd = session.GetValue(Session.SessionKeys.Product) as IProduct;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();


            Model model = (Model)session.GetValue("NewModel");
            if (model == null)
            {
                model=modelRep.Find(curProd.Model);
            }

            if (model == null)
            {
                throw new FisException("CHK928", new string[] { curProd.Model });
            }

            session.AddValue("NewModel", model);

            //1.有ProductInfo 有IMEI 的值，不再產生新的IMEI
            IList<string> infoTypes = this.InfoType.Split(new char[] { '~' }).ToList();
            IList<string> infoValues = new List<string>();
            
            foreach (string infoType in infoTypes)
            {
                infoValues.Add((string)curProd.GetExtendedProperty(infoType));
            }
            //篩選 InfoType=IMEI_XXX
            var infoTypeValueList = curProd.ProductInfoes.Where(x => x.InfoType.StartsWith("IMEI_") && 
                                                                                                          !infoTypes.Contains(x.InfoType)).
                                                        OrderBy(y=>y.InfoType).ToList();
            foreach (IMES.FisObject.FA.Product.ProductInfo item in infoTypeValueList)
            {
                infoTypes.Add(item.InfoType);
                infoValues.Add(item.InfoValue);
            }

            //2.檢查ModelInfo IMEI
             string preFixIMEICode = model.GetAttribute("IMEI");
             string digitPattern = @"^\d{8}$";

             int qty = int.Parse(model.GetAttribute("IMEIQTY")??"1");

             //3..檢查ModelInfo IMEICode
             string imeiCode = model.GetAttribute("IMEICode");

            if (!string.IsNullOrEmpty(preFixIMEICode) &&                  
                Regex.IsMatch(preFixIMEICode.Trim(), digitPattern))
            {
                if (string.IsNullOrEmpty(imeiCode))
                {
                    throw new FisException("CHK1106", new string[] { curProd.Model, "IMEICode" });
                }
                if (qty == 1) //一筆Case
                {
                    string imeiInfo = infoValues[0];
                    string name = infoTypes[0];
                    if (string.IsNullOrEmpty(imeiInfo))
                    {
                        #region 分配新的IMEI
                        genOneMEI(session, curProd, name, imeiInfo, preFixIMEICode, imeiCode);                       
                        #endregion
                    }
                    else
                    {
                        //檢查PrefixCode  及IMEICode 是否相同 
                        //Check PrefixedCode is same or not
                        bool isSamePrex = imeiInfo.StartsWith(preFixIMEICode);
                        if (!isSamePrex)
                        {
                            genOneMEI(session, curProd, name, imeiInfo, preFixIMEICode, imeiCode);
                        }
                        else
                        {
                            bool isSameCode = checkSamCode(session, curProd, infoTypes, infoValues, imeiCode);
                            if (!isSameCode)
                            {
                                genOneMEI(session, curProd, name, imeiInfo, preFixIMEICode, imeiCode);
                            }
                        }                       
                    }                  

                }
                else //不找回收序號for多筆
                {
                   bool hasNoIMEI = infoValues.Any(x => string.IsNullOrEmpty(x));
                    if (hasNoIMEI)
                    {
                        #region generate new IMEI
                        genMultiIMEI(session, curProd, infoTypes, infoValues, preFixIMEICode, imeiCode, qty);
                      
                        #endregion
                    }
                    else
                    {
                        bool isSamePrefixCode = infoValues.All(x => x.StartsWith(preFixIMEICode));
                        if (!isSamePrefixCode)
                        {
                            genMultiIMEI(session, curProd, infoTypes, infoValues, preFixIMEICode, imeiCode, qty);
                        }
                        else
                        {
                            bool isSameCode = checkSamCode(session, curProd, infoTypes, infoValues, imeiCode);  
                            if (!isSameCode)
                            {
                                genMultiIMEI(session, curProd, infoTypes, infoValues, preFixIMEICode, imeiCode, qty);
                            }
                        }
                    }                   
                }

                //刪除多餘的ProductInfo
                int count =infoTypes.Count;
                for (int i = qty; i < count; ++i)
                {
                    if (infoValues[i] != null)
                    {
                        prodRep.DeleteProductInfoDefered(session.UnitOfWork, new IMES.FisObject.FA.Product.ProductInfo {  ProductID=curProd.ProId, InfoType = infoTypes[i] }, null);
                    }
                }

            }
            else if (!string.IsNullOrEmpty(preFixIMEICode))
            {
                throw new FisException("CHK1085", new string[] { curProd.Model, preFixIMEICode });
            }

            return base.DoExecute(executionContext);
        }


        private static object _syncRoot_GetSeq = new object();

        private bool checkSamCode(Session session, IProduct curProd, 
                                                IList<string> infoTypes, IList<string> infoValues,
                                                string imeiCode)
        {
            bool isSameCode = true;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            int count = infoTypes.Count;
            for (int i = 0; i < count; ++i)
            {
                string name = infoTypes[i];
                string imeiInfo = infoValues[i];
                IList<CombinedAstNumberInfo> combinedAstNumberInfoList = utl.AstNum.GetCombinedAstCode(curProd.ProId, "IMEI", imeiInfo);
                if (combinedAstNumberInfoList != null && combinedAstNumberInfoList.Count > 0)
                {
                    string astCode = combinedAstNumberInfoList[0].Code;
                    if (imeiCode != astCode)
                    {
                        isSameCode = false;
                        break;
                    }
                }
                else
                {
                    isSameCode = false;
                    break;
                }
            }
            return isSameCode;
        }

        private void genMultiIMEI(Session session, IProduct curProd, 
                                                IList<string> infoTypes, IList<string> infoValues, 
                                                string preFixIMEICode, string imeiCode, int qty)
        {
            IList<string> seqList = getIMEISeq(curProd.Model, imeiCode.Trim(), this.Customer, qty);
            for (int i = 0; i < qty; i++)
            {
                string name = "";
                if (i < infoTypes.Count)
                {
                    name = infoTypes[i];
                    //seqList[i]= code + getIMEICheckCode(code);                            
                }
                else
                {
                    name = "IMEI_" + i.ToString("D1");
                    //infoTypes.Add(name);
                    //infoValues.Add(curProd.ProductInfoes.Where(x => x.InfoType == name).Select(y => y.InfoValue).FirstOrDefault());
                }
                string code = preFixIMEICode.Trim() + seqList[i].Substring(6, 6);
                string IMEIcode = code + getIMEICheckCode(code);
                curProd.SetExtendedProperty(name, IMEIcode, this.Editor);
                ActivityCommonImpl.Instance.AstNum.InsertCombinedAstNumber(session, curProd.ProId, imeiCode, "IMEI", IMEIcode, this.Station, this.Editor);
                string imeiInfo = infoValues[i];
                if (!string.IsNullOrEmpty(imeiInfo))
                {
                    //Release old IMEI
                    ActivityCommonImpl.Instance.AstNum.ReleaseCombinedAstNumber(session, curProd.ProId, "IMEI", imeiInfo, this.Station, this.Editor);
                }
            }
        }


        private void genOneMEI(Session session, IProduct curProd,
                                               string name, string imeiInfo,
                                               string preFixIMEICode, string imeiCode)
        {
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            //找取回收IMEI
            string IMEI = utl.AstNum.CheckAndSetReleaseAstNumber(curProd.ProId, imeiCode, "IMEI", this.Station, this.Editor);
            if (string.IsNullOrEmpty(IMEI) || !IMEI.StartsWith(preFixIMEICode))
            {
                string seq = getIMEISeq(curProd.Model, imeiCode.Trim(), this.Customer);
                if (string.IsNullOrEmpty(seq))
                {
                    throw new FisException("CHK1087", new string[] { curProd.Model });
                }

                string code = preFixIMEICode.Trim() + seq.Substring(6, 6);
                IMEI = code + getIMEICheckCode(code);
                ActivityCommonImpl.Instance.AstNum.InsertCombinedAstNumber(session, curProd.ProId, imeiCode, "IMEI", IMEI, this.Station, this.Editor);
            }

            curProd.SetExtendedProperty(name, IMEI, this.Editor);

            //1.有ProductInfo 有IMEI 的值,release old IMEI
            if (!string.IsNullOrEmpty(imeiInfo))
            {
                //Release old IMEI
                ActivityCommonImpl.Instance.AstNum.ReleaseCombinedAstNumber(session, curProd.ProId, "IMEI", imeiInfo, this.Station, this.Editor);
            }  
        }

        private string getIMEISeq(string model,string preFixCode,string custom)
        {
            string numType ="IMEI";
            try
            {
                SqlTransactionManager.Begin();
                lock (_syncRoot_GetSeq)
                {
                    INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

                    MACRange currentRange = numCtrlRepository.GetMACRange(preFixCode, new string[] { "R", "A" });
                    if (currentRange == null)
                    {
                        throw new FisException("ICT014", new string[] {});
                    }
                    else
                    {
                        if (!validateIMEISettingRange(currentRange.BegNo, currentRange.EndNo))
                        {
                            throw new FisException("CHK1086", new string[] { currentRange.BegNo + "~" + currentRange.EndNo });
                        }
                        NumControl currentMaxNum = numCtrlRepository.GetMaxValue(numType, preFixCode);
                        if (currentMaxNum == null)
                        {
                            currentMaxNum = new NumControl();
                            currentMaxNum.NOName = preFixCode;
                            currentMaxNum.NOType = numType;
                            currentMaxNum.Value = currentRange.BegNo;
                            currentMaxNum.Customer = custom;

                            IUnitOfWork uof = new UnitOfWork();
                            numCtrlRepository.InsertNumControlDefered(uof, currentMaxNum);
                            if (currentMaxNum.Value == currentRange.EndNo)
                            {
                                numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Closed);
                            }
                            else
                            {
                                numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Active);
                            }
                            uof.Commit();
                            SqlTransactionManager.Commit();
                            return currentMaxNum.Value;

                        }
                        else
                        {
                            if (currentMaxNum.Value == currentRange.EndNo)
                            {
                                numCtrlRepository.SetMACRangeStatus(currentRange.ID, MACRange.MACRangeStatus.Closed);
                                currentRange = numCtrlRepository.GetMACRange(preFixCode, new string[] { "R" });
                                if (!validateIMEISettingRange(currentRange.BegNo, currentRange.EndNo))
                                {
                                    throw new FisException("CHK1086", new string[] { currentRange.BegNo + "~" + currentRange.EndNo });
                                }

                                if (currentMaxNum.Value == currentRange.BegNo || currentMaxNum.Value == currentRange.EndNo)
                                {
                                    throw new FisException("ICT018", new string[] { currentMaxNum.Value });
                                }
                            }

                            int curNextNum = int.Parse(currentMaxNum.Value)+1;
                            currentMaxNum.Value = curNextNum.ToString("D12");                          

                            IUnitOfWork uof = new UnitOfWork();
                            numCtrlRepository.Update(currentMaxNum, uof);
                            if (currentMaxNum.Value == currentRange.EndNo)
                            {
                                numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Closed);
                            }
                            else
                            {
                                numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Active);
                            }
                            uof.Commit();
                            SqlTransactionManager.Commit();
                            return currentMaxNum.Value;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SqlTransactionManager.Rollback();
                throw e;
            }
            finally
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }


        }

        private IList<string> getIMEISeq(string model, string preFixCode, string custom, int qty)
        {
            string numType = "IMEI";
            try
            {
                IList<string> ret = new List<string>();
                if (qty == 1)
                {
                    ret.Add(getIMEISeq(model, preFixCode, custom));
                    return ret;
                }

                SqlTransactionManager.Begin();
                lock (_syncRoot_GetSeq)
                {
                    INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

                    MACRange currentRange = numCtrlRepository.GetMACRange(preFixCode, new string[] { "R", "A" });
                    if (currentRange == null)
                    {
                        throw new FisException("ICT014", new string[] { });
                    }
                    else
                    {
                        if (!validateIMEISettingRange(currentRange.BegNo, currentRange.EndNo))
                        {
                            throw new FisException("CHK1086", new string[] { currentRange.BegNo + "~" + currentRange.EndNo });
                        }
                        NumControl currentMaxNum = numCtrlRepository.GetMaxValue(numType, preFixCode);
                        if (currentMaxNum == null)
                        {
                            currentMaxNum = new NumControl();
                            currentMaxNum.NOName = preFixCode;
                            currentMaxNum.NOType = numType;
                            currentMaxNum.Value = currentRange.BegNo;
                            currentMaxNum.Customer = custom;
                            ret.Add(currentMaxNum.Value);
                            qty--;

                            IUnitOfWork uof = new UnitOfWork();
                            if (qty >0 && currentMaxNum.Value == currentRange.EndNo) //check Last Range
                            {
                                numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Closed);
                                currentRange = numCtrlRepository.GetMACRange(preFixCode, new string[] { "R", "A" });
                                if (currentRange == null)
                                {
                                    throw new FisException("ICT014", new string[] { });
                                }
                            }                         

                            int remainingCount = qty;
                           
                            for (int j = 0; j < qty; j++)
                            {
                                remainingCount--;
                                int curNum = int.Parse(currentMaxNum.Value) + 1;
                                currentMaxNum.Value = curNum.ToString("D12");
                                if (remainingCount >0  && currentMaxNum.Value == currentRange.EndNo) //check Last Range
                                {
                                    numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Closed);
                                    currentRange = numCtrlRepository.GetMACRange(preFixCode, new string[] { "R", "A" });
                                    if (currentRange == null)
                                    {
                                        throw new FisException("ICT014", new string[] { });
                                    }

                                    if (!validateIMEISettingRange(currentRange.BegNo, currentRange.EndNo))
                                    {
                                        throw new FisException("CHK1086", new string[] { currentRange.BegNo + "~" + currentRange.EndNo });
                                    }

                                    if (currentMaxNum.Value == currentRange.BegNo || currentMaxNum.Value == currentRange.EndNo)
                                    {
                                        throw new FisException("ICT018", new string[] { currentMaxNum.Value });
                                    }
                                }
                                ret.Add(currentMaxNum.Value);
                            }

                            if (int.Parse(currentMaxNum.Value) > int.Parse(currentRange.EndNo))
                            {
                                throw new FisException("GEN022", new string[] { currentMaxNum.Value + ">" + currentRange.EndNo });
                            }

                            if (currentMaxNum.Value == currentRange.EndNo)
                            {
                                numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Closed);
                            }
                            else
                            {
                                numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Active);
                            }
                            numCtrlRepository.InsertNumControlDefered(uof, currentMaxNum);
                            uof.Commit();
                            SqlTransactionManager.Commit();
                            return ret;

                        }
                        else
                        {
                            if (currentMaxNum.Value == currentRange.EndNo)
                            {
                                numCtrlRepository.SetMACRangeStatus(currentRange.ID, MACRange.MACRangeStatus.Closed);
                                currentRange = numCtrlRepository.GetMACRange(preFixCode, new string[] { "R","A" });
                                if (currentRange == null)
                                {
                                    throw new FisException("ICT014", new string[] { });
                                }

                                if (!validateIMEISettingRange(currentRange.BegNo, currentRange.EndNo))
                                {
                                    throw new FisException("CHK1086", new string[] { currentRange.BegNo + "~" + currentRange.EndNo });
                                }

                                if (currentMaxNum.Value == currentRange.BegNo || currentMaxNum.Value == currentRange.EndNo)
                                {
                                    throw new FisException("ICT018", new string[] { currentMaxNum.Value });
                                }
                            }

                            IUnitOfWork uof = new UnitOfWork();

                            int remainingCount = qty;
                            for (int j = 0; j < qty; j++)
                            {
                                remainingCount--;
                                int curNextNum = int.Parse(currentMaxNum.Value) + 1;
                                currentMaxNum.Value = curNextNum.ToString("D12");
                                ret.Add(currentMaxNum.Value);
                                if (remainingCount > 0 && currentMaxNum.Value == currentRange.EndNo) //check Last Range
                                {
                                    numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Closed);
                                    currentRange = numCtrlRepository.GetMACRange(preFixCode, new string[] { "R", "A" });
                                    if (currentRange == null)
                                    {
                                        throw new FisException("ICT014", new string[] { });
                                    }

                                    if (!validateIMEISettingRange(currentRange.BegNo, currentRange.EndNo))
                                    {
                                        throw new FisException("CHK1086", new string[] { currentRange.BegNo + "~" + currentRange.EndNo });
                                    }

                                    if (currentMaxNum.Value == currentRange.BegNo || currentMaxNum.Value == currentRange.EndNo)
                                    {
                                        throw new FisException("ICT018", new string[] { currentMaxNum.Value });
                                    }
                                }                               
                            }

                            if (int.Parse(currentMaxNum.Value) > int.Parse(currentRange.EndNo))
                            {
                                throw new FisException("GEN022", new string[] { currentMaxNum.Value + ">" + currentRange.EndNo });
                            }

                          
                            numCtrlRepository.Update(currentMaxNum, uof);
                            if (currentMaxNum.Value == currentRange.EndNo)
                            {
                                numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Closed);
                            }
                            else
                            {
                                numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Active);
                            }
                            uof.Commit();
                            SqlTransactionManager.Commit();
                            return ret;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SqlTransactionManager.Rollback();
                throw e;
            }
            finally
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }


        }


       

        private string getIMEICheckCode(string code)
        {
            int total=0;
            for (int i = 0; i < code.Length; i++)
            {
                int digit = int.Parse(code.Substring(i,1));
                if (i % 2 == 0) //奇數位 => (2).将奇数位数字相加，再加上上一步算得的值
                {
                    total= total +digit;
                }
                else //偶數位 =>(1).将偶数位数字分别乘以2，分别计算个位数和十位数之和 
                {
                    digit = digit * 2;
                    total = total + (digit/10)+ (digit % 10) ;
                }
            }

            //取個位數 =>(3).如果得出的数个位是0则校验位为0，否则为10减去个位数
            total = (total % 10);
            if (total == 0)
            {
                return "0";
            }
            else
            {
                total = 10 - total;
                return total.ToString();
            }

        }


        private bool validateIMEISettingRange(string beginNum, string endNum)
        {

            string digitPattern = @"^[0]{6}\d{6}$";
            if (Regex.IsMatch(beginNum, digitPattern) &&
               Regex.IsMatch(endNum, digitPattern))
            {
                return true;
            }
            else
            {
                return false;
            }
        }       

    }
}
