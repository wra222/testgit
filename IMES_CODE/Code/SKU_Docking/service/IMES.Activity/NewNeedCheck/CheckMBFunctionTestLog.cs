﻿using System;
using System.Collections.Generic;
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
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.TestLog;
using IMES.DataModel;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    /// CheckMBData
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于PCA Test Station 
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         Check MB Sno
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MB
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
    /// 相关FisObject:
    ///         IMBRepository
    ///         IMB
    /// </para> 
    /// </remarks>
    public partial class CheckMBFunctionTestLog : BaseActivity
	{

        private const string skuMBConstValueType = "SkuMBFunTestType";
        private const string rctoMBConstValueType = "RCTOMBFunTestType";
        private const string faReturnMBConstValueType = "FAReturnMBFunTestType";
        private const string fruMBConstValueType = "FRUMBFunTestType";
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckMBFunctionTestLog()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Check MB Function TestLog
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IMB currentMB = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
            IMBRepository currentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();

            string strMBCode = "";
            string mbLastFuncTestType = "";
            if (currentMB.Sn.Length == 11)
            {
                strMBCode = currentMB.Sn.Substring(0, 3);
            }
            else
            {
                strMBCode = currentMB.Sn.Substring(0, 2);
            }
            //strMBCode = currentMB.Sn.Substring(0, 2);

            bool isRCTOMB = (currentMB.Sn.ToUpper().Substring(5, 1) == "R");
            bool isFRUStation = (this.Station == this.FruTestStation);

            #region get last PCBTestLog
            //1.檢查 MB_Test是否有設置By MBCode and Type=false, 有設置MB_Test則Pass不檢查
            //2.Fru Station  不檢查MB_Test，都需要Check 
            if (!isFRUStation)
            {
                IList<MBTestDef> mbTestList = currentMBRepository.GetMBTestList_NotCut(strMBCode, false);
                if (mbTestList != null && mbTestList.Count > 0)
                {
                    return base.DoExecute(executionContext);
                }
            }

            //Get latest process tiime
            DateTime lastProcessTime= currentMB.MBStatus.Udt;
            //檢查上傳Function test log不須檢查Station
            //IList<TestLog> pcbTestLogList = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, this.Station, lastProcessTime);
            IList<TestLog> pcbTestLogList = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, lastProcessTime);
            if (pcbTestLogList==null || pcbTestLogList.Count==0)
            {
                throw new FisException("CHK1061", new string[] { });
            }
            TestLog lastTestLog = pcbTestLogList[0];

            if (lastTestLog.Status == TestLog.TestLogStatus.Fail)
            {
                throw new FisException("CHK245", new string[] { });
            }

            mbLastFuncTestType = lastTestLog.Type.Trim();
            #endregion

            #region 檢查 PCBTestLog.Type 是否為 "M/B"、"MB"、"MBR"

           
            // first check Fru Station MB
             if (isFRUStation)
            {
                IList<ConstValueTypeInfo> fruTestTypeList = partRep.GetConstValueTypeList(fruMBConstValueType);
                var allowTestTypeList = (from p in fruTestTypeList
                                         where p.value.Trim() == mbLastFuncTestType
                                         select p).ToList();
                if (allowTestTypeList.Count == 0)
                {
                    string allowTestType = string.Join(",", fruTestTypeList.Select(x => x.value).ToArray());
                    throw new FisException("CHK1063", new string[] { "FRU", allowTestType, mbLastFuncTestType });
                }
            }             
             else // Not FRU Station case
             {
                 //RCTO MB
                 if (isRCTOMB)
                 {
                     IList<ConstValueTypeInfo> rctoTestTypeList = partRep.GetConstValueTypeList(rctoMBConstValueType);

                     var allowTestTypeList = (from p in rctoTestTypeList
                                              where p.value.Trim() == mbLastFuncTestType
                                              select p).ToList();
                     if (allowTestTypeList.Count == 0)
                     {
                         string allowTestType = string.Join(",", rctoTestTypeList.Select(x => x.value).ToArray());
                         throw new FisException("CHK1063", new string[] { "RCTO", allowTestType, mbLastFuncTestType });
                     }
                 }
                 else  // FA Retrun MB & Sku MB Check
                 {

                     //檢查成退MB
                     bool bFAReturnMB = false;
                     foreach (var repair in currentMB.Repairs)
                     {
                         if (repair.Station == "33")
                         {
                             bFAReturnMB = true;
                             break;
                         }
                     }

                     //成退MB
                     if (bFAReturnMB)
                     {
                         IList<ConstValueTypeInfo> faReturnMBTestTypeList = partRep.GetConstValueTypeList(faReturnMBConstValueType);
                         var allowTestTypeList = (from p in faReturnMBTestTypeList
                                                  where p.value.Trim() == mbLastFuncTestType
                                                  select p).ToList();
                         if (allowTestTypeList.Count == 0)
                         {
                             string allowTestType = string.Join(",", faReturnMBTestTypeList.Select(x => x.value).ToArray());
                             throw new FisException("CHK1063", new string[] { "FA Return", allowTestType, mbLastFuncTestType  });
                         }
                     }
                     else //檢查 SKU MB            
                     {
                         IList<ConstValueTypeInfo> skuMBTestTypeList = partRep.GetConstValueTypeList(skuMBConstValueType);
                         var allowTestTypeList = (from p in skuMBTestTypeList
                                                  where p.value.Trim() == mbLastFuncTestType
                                                  select p).ToList();
                         if (allowTestTypeList.Count == 0)
                         {
                             string allowTestType = string.Join(",", skuMBTestTypeList.Select(x => x.value).ToArray());
                             throw new FisException("CHK1063", new string[] { "SKU", allowTestType, mbLastFuncTestType });
                         }

                         IList<ITCNDDefectCheckDef> itcndDefectCheckList = currentMBRepository.GetITCNDDefectChecks_NotCut(strMBCode);
                         if (itcndDefectCheckList != null && itcndDefectCheckList.Count > 0)
                         {
                             if (itcndDefectCheckList[0].type.Trim() != mbLastFuncTestType)
                             {
                                 //error sku 與itcnt上傳檢測方式不一致
                                 throw new FisException("CHK1063", new string[] { "SKU ITCND ", itcndDefectCheckList[0].type.Trim(), mbLastFuncTestType });
                             }
                         }
                     }
                 }
            }
            #endregion

            #region 檢查remark內容

            #region 檢查MAC
            string testLogRemark = lastTestLog.Remark;
            string[] testRemarkList = testLogRemark.Split('~');

            string strMAC = currentMB.MAC == null ? "" : currentMB.MAC.Trim();

            string[] remarkMACList = (from q in testRemarkList
                                      where q.StartsWith("MAC")
                                      select q.Substring(4).Trim()).ToArray();


            if (remarkMACList != null && remarkMACList.Count() > 0)
            {
                if (strMAC != remarkMACList[0])
                {
                    throw new FisException("CHK248", new string[] { });
                }
            }
            else if (!string.IsNullOrEmpty(strMAC))
            {
                throw new FisException("CHK248", new string[] { });
            }
            #endregion


            #region 檢查MBCT 根據PCATestCheck Table設置
            IList<PcaTestCheckInfo> pcaTestCheckList = currentMBRepository.GetPcaTestCheckInfoListByCode_NotCut(strMBCode);

            if ((pcaTestCheckList != null) && (pcaTestCheckList.Count != 0))
            {
                if (pcaTestCheckList[0].mbct.Trim() == "Y")
                {
                    string strMBCT = "";
                    if (currentMB.GetExtendedProperty("MBCT") == null)
                    {
                        strMBCT = "";
                    }
                    else
                    {
                        strMBCT = (string)currentMB.GetExtendedProperty("MBCT");
                    }
                    strMBCT = strMBCT.Trim();
                    string[] remarkMBCTList = (from q in testRemarkList
                                               where q.StartsWith("MBCT")
                                               select q.Substring(5).Trim()).ToArray();

                    if (remarkMBCTList != null && remarkMBCTList.Count() > 0)
                    {
                        if (strMBCT != remarkMBCTList[0])
                        {
                            throw new FisException("CHK246", new List<string>()); //改号:BIOS燒錄CT錯誤，請重新燒錄
                        }
                    }
                    else if (!string.IsNullOrEmpty(strMBCT))
                    {
                        throw new FisException("CHK246", new List<string>());//改号:BIOS燒錄CT錯誤，請重新燒錄
                    }
                }
                //CHECK BIOS
                if (pcaTestCheckList[0].bios.Trim() == "Y")
                {
                    IList<ConstValueInfo> valueList = partRep.GetConstValueListByType("BIOSVer_SA");//MBCODE--->MB Family mantis"0001548
                   var bioslist = (from p in valueList
                                      where p.name ==strMBCode
                                      select p.value).ToList();
                      if (bioslist == null || bioslist.Count == 0)
                      {
                          bioslist = (from p in valueList
                                      where p.name == currentMB.Family
                                      select p.value).ToList();
                          if (bioslist == null || bioslist.Count == 0)
                          {
                              throw new FisException("CQCHK0026", new string[] { strMBCode + ":BIOSVer" });
                          }
                      }
                    string[] remarkbios = (from q in testRemarkList
                                           where q.StartsWith("BIOS")
                                           select q.Substring(5).Trim()).ToArray();
                    if (remarkbios != null && remarkbios.Count()>0)
                    {
                        bool bCompareBios = false;
                        string[] BIOSVerList = bioslist[0].Split('~');
                        for (int i = 0; i < BIOSVerList.Length; i++)
                        {
                            if (remarkbios[0].IndexOf(BIOSVerList[i].ToString()) !=-1)
                            {
                                bCompareBios = true;
                                break;
                            }
                        }

                        if (!bCompareBios)
                        {
                            // BIOS 匹配不對
                            throw new FisException("CQCHK0023", new string[] { });
                        }
                    }
                    else
                    {
                        throw new FisException("CQCHK1080", new string[] { currentMB.Sn });
                    }

                }
                //check hdd 
                if (pcaTestCheckList[0].hddv == "Y")
                {
                    IList<ConstValueInfo> valueList = partRep.GetConstValueListByType("HDDVer");
                    var hddlist = (from p in valueList
                                    where p.name == strMBCode
                                    select p.value).ToList();
                    if (hddlist == null || hddlist.Count == 0)
                    {
                        hddlist = (from p in valueList
                                   where p.name == currentMB.Family
                                   select p.value).ToList();
                        if (hddlist == null || hddlist.Count == 0)
                        {
                            // ConstValue 未设定 %1，请联系IE设定
                            throw new FisException("CQCHK0026", new string[] { strMBCode + ":HDDVer" });
                        }
                    }
                    string[] remarkhdd = (from q in testRemarkList
                                           where q.StartsWith("HDD")
                                           select q.Substring(4).Trim()).ToArray();
                    if (remarkhdd != null && remarkhdd.Count()>0)
                    {
                        bool bCompareBios = false;
                        string[] HDDVerlist = hddlist[0].Split('~');
                        for (int i = 0; i < HDDVerlist.Length; i++)
                        {
                            if (remarkhdd[0].IndexOf(HDDVerlist[i].ToString()) != -1)
                            {
                                bCompareBios = true;
                                break;
                            }
                        }

                        if (!bCompareBios)
                        {
                            // HDD 匹配不對
                            throw new FisException("CQCHK1079", new string[] { hddlist[0] });
                        }
                    }
                    else
                    {
                        throw new FisException("CQCHK1081", new string[] { currentMB.Sn });
                    }


                }
            }

            #endregion
            
            #endregion
            
            try
            {
                if (currentMB.IsVB)
                {
                    CurrentSession.AddValue("IsMBOKVGA", true);

                }
                else
                {
                    CurrentSession.AddValue("IsMBOKVGA", false);
                }
            }
            catch (FisException)
            {
                CurrentSession.AddValue("IsMBOKVGA", false);
            }
            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty FruTestStationProperty = DependencyProperty.Register("FruTestStation", typeof(string), typeof(CheckMBFunctionTestLog), new PropertyMetadata("1A"));

        /// <summary>
        /// CheckLogStation
        /// </summary>
        [DescriptionAttribute("FruTestStation")]
        [CategoryAttribute("InArguments Of CheckMBFunctionTestLog")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string FruTestStation
        {
            get
            {
                return ((string)(base.GetValue(CheckMBFunctionTestLog.FruTestStationProperty)));
            }
            set
            {
                base.SetValue(CheckMBFunctionTestLog.FruTestStationProperty, value);
            }
        }
	}
}