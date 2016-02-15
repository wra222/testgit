// INVENTEC corporation (c)2009 all rights reserved. 
// Description: PCA Test Station 检查MBSNO，处理15种异常情况
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-13   Kaisheng                     create
// Known issues:
using System;
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
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// PCA Test Station 检查MBSNO，处理15种异常情况
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
    public partial class CheckMBSnoforPCATest : BaseActivity
	{

        /// <summary>
        /// 构造函数
        /// </summary>
		public CheckMBSnoforPCATest()
		{
			InitializeComponent();
		}

        /// <summary>
        /// PCA Test Station 检查MBSNO，处理15种异常情况
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IMB currentMB = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
            IMBRepository currentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            CurrentSession.AddValue("HavePromptinfo", "NO!");
            CurrentSession.AddValue("NewTerstLogremark", "");

            //2.28.	MBCode:若第6码为’M’，则取MBSN前3码为MBCode，若第5码为’M’，则取前2码
            //CheckCode:若MBSN的第5码为’M’，则取MBSN的第6码，否则取第7码
            string strMBCode = "";
            if (currentMB.Sn.Substring(5, 1) == "M" || currentMB.Sn.Substring(5, 1) == "B")
            {
                strMBCode = currentMB.Sn.Substring(0, 3);
            }
            else if (currentMB.Sn.Substring(4, 1) == "M" || currentMB.Sn.Substring(4, 1) == "B")
            {
                strMBCode = currentMB.Sn.Substring(0, 2);
            }
            //--------------------------------------------------
            //1.是否存在重复的MB Sno 		
            //select @Count = COUNT(*) from PCB nolock where PCBNo = @MBSno 若@Count >1，则报错：“MBSno 重复”	
            if (currentMBRepository.GetCountOfPCB(currentMB.Sn) > 1)
            {
                List<string> errpara1 = new List<string>();
                errpara1.Add(currentMB.Sn);
                throw new FisException("CHK159", errpara1);
            }
            /*
            if ((currentMB.MAC == null) || (currentMB.MAC.Trim() == ""))
            {
                List<string> errparamacnull = new List<string>();
                errparamacnull.Add(currentMB.Sn);
                throw new FisException("CHK033", errparamacnull);
            }
            */
            //Modify 2012/2/22 Kaisheng： 
            //Reason：需求变更  7.1 去除MAC的Check
            //-------------------------------------------------------------------------------------------------------
            //是否存在重复的MAC，需要报告错误：“Duplicate MAC Address!”
            //select @Count = COUNT(*) from PCB nolock where MAC in (select MAC from PCB nolock where PCBNo=@MBSno)
            /*
            IList<IMB> maclist = currentMBRepository.GetMBListByMAC(currentMB.MAC);
            //if (currentMBRepository.GetMBListByMAC(currentMB.MAC).Count > 1)
            if ((maclist != null) && (maclist.Count > 1))
            {
                List<string> errpara2 = new List<string>();
                //errpara2.Add(currentMB.Sn);
                errpara2.Add(currentMB.MAC);
                throw new FisException("CHK032", errpara2);
            }
            */
            //--------------------------------------BEGIN-------------------------------------------------------------
            //Modify 2012/04/11 Kaisheng:UC update
            // 2.	若检测站为‘15,16,17’，检查是否有ICT Input的成功过站记录
            // select * from PCBLog nolock where Station = '10' and Status = '1' and PCBNo = @MBSno
            // 若不存在Station=10,Status=1的过站记录，则报错：“请先刷ICT Input” --PAK078(该MB需先去做ICT测试，刷ICT Input！)
            if ((this.Station == "15") || (this.Station == "16") || (this.Station == "17"))
            {
                IList<MBLog> resultLogList = currentMBRepository.GetMBLog(currentMB.Sn, "10", 1);
                if (resultLogList == null || resultLogList.Count == 0)
                {
                    List<string> errpara2 = new List<string>();
                    //errpara2.Add(currentMB.Sn);
                    errpara2.Add(currentMB.Sn);
                    throw new FisException("PAK078", errpara2);
                }

            }

            //-------------------------------------- END--------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------
            //3.	是否在修护区   若存在记录，则报错：“请先将MBSno刷出修护区”
            //select * from PCBRepair nolock where PCBNo = @MBSno and Status = '0'
            Repair repairreturn = currentMB.GetCurrentRepair();
            if (repairreturn != null)
            {
                List<string> errpara3 = new List<string>();
                errpara3.Add(currentMB.Sn);
                throw new FisException("CHK220", errpara3);
            }
            //4.	是否结合主机,若存在记录，则报错：“此MB已经结合了主机”
            //select * from IMES2012_FA..Product nolock where PCBID = @MBSno
            IProductRepository ProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            if (ProductRepository.IfBindPCB(currentMB.Sn))
            {
                List<string> errpara4 = new List<string>();
                errpara4.Add(currentMB.Sn);
                throw new FisException("CHK221", errpara4);
            }
            //Add 2012/04/06 UC Update:
            //----------------------------------------------BEGIN--------------------------------------------
            //5.	若检测站为“15：SA1 Test；16：SA2 Test；17：SA3 Test”，且板子（非VGA）存在修护记录（Condition：PCBRepair.PCBNo=@MBSno and Status = 1），则进行如下检查：
            //Move code --getmaxcdt 
            //ADD 2012/2/22 Kaisheng：
            //Reason：需求变更：
            //c.	检查MBSN是否已经上传测试Log（select * from PCBTestLog nolock where PCBNo = @MBSno），若不存在测试Log，则提示（不是报错）：“此机器SA测试失败，只能刷不良！”，不执行step d、e和f；若存在测试Log，则执行Step d、e和f。
            //----------------------------------------------------------------------------------------------
            DateTime maxpcblogcdt = currentMBRepository.GetMaxCdtFromPCBLog(currentMB.Sn);
            if (maxpcblogcdt == System.DateTime.MinValue)
                maxpcblogcdt = new DateTime(1900, 1, 1);//System.DateTime.Now.AddDays(-365); 

            //UC Update 2012/4/27 应产线要求，去除修护主板必须走全测的逻辑检查
            //kaisheng 
            //--------------------------------------------------------------------------------------------------
            //if ((this.Station == "15") || (this.Station == "16") || (this.Station == "17"))
            //{
            //    if (currentMB.IsVB == false)
            //    {
            //        if (currentMBRepository.ExistPCBRepair(currentMB.Sn, 1))
            //        {
            //            // 检查MBSN是否已经上传全测版测试Log（select * from PCBTestLog nolock where PCBNo = @MBSno and Type = ‘M/B’ and Station=当前站 and Cdt>PCBLog中当前MB的最大过站时间），若不存在测试Log，则提示（不是报错）：“此机器未进行全测版测试，只能刷不良！”，不执行step 6，等待刷入不良代码；若存在测试Log，则继续执行Step 6。
            //            string[] param1 = { "M/B" };
            //            var lstalltestlog = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, param1, this.Station, maxpcblogcdt);
            //            if ((lstalltestlog == null) || (lstalltestlog.Count == 0))
            //            {
            //                try
            //                {
            //                    throw new FisException("CHK243", new List<string>());//改此机器未进行全测版测试，只能刷不良！
            //                }
            //                catch (FisException ex)
            //                {

            //                    CurrentSession.AddValue("HavePromptinfo", ex.mErrmsg);
            //                }
            //                return base.DoExecute(executionContext); 
            //            }
            //        }

            //    }
            //}
            //-----------------------------------------------------------------------------------------------------
            
            //----------------------------------------------END--------------------------------------------
            

            //5 板子为非RCTO(MBSno第6为不为R)，若检测站为15：SA1 Test，则进行如下检查，不通过则退出  
            //string strmssno= currentMB.Sn.Substring(5, 1);
            //if ((currentMB.Sn.ToUpper().Substring(5, 1) != "R") && (this.Station == "15"))
            
            //Modify 2012/2/22 Kaisheng：
            //Reason：需求变更：7.1 Test Station 增加 16和17的Check,“15：SA1 Test；16：SA2 Test；17：SA3 Test”，去掉条件：板子为非RCTO(MBSno第6为不为R)
            //-----------------------------------------------------------------------------------------------
            //if ((currentMB.Sn.ToUpper().Substring(5, 1) != "R") && (this.Station == "15")) //for DEBUG
            //-----------------------------------------------------------------------------------------------
            if ((this.Station == "15") || (this.Station == "16") || (this.Station == "17"))
            {
                //UC Update:2012/03/08:前提条件：若MB_Test未维护记录（MB_Test.Code=MBCode and Type=0），则不进行如下操作
                //MBCODE 2->3
              //var lstmbtestrec = currentMBRepository.GetMBTestList(currentMB.Sn.Trim(), false);
                var lstmbtestrec = currentMBRepository.GetMBTestList_NotCut(strMBCode, false);
                if  ((lstmbtestrec!=null)&&(lstmbtestrec.Count!=0))
                {
                    //a.	Model是否存在，若存在获取@Model（PCB.PCBModelID, Condition: PCB.PCBNo=@MBSno）
                    if ((currentMB.Model == null) || (currentMB.Model == ""))
                    {
                        List<string> errpara5 = new List<string>();
                        errpara5.Add(currentMB.Sn);
                        throw new FisException("CHK222", errpara5);
                    }
                    //b.	Family是否存在，若存在获取@family(GetData..Part.Descr, Contidion: GetData..Part.PartNo=@Model)
                    //IMES.FisObject.PCA.MB.MB.get_Family() 位置 G:\imes-sa\kernel\IMES.FisObject\PCA\MB\MB.cs:行号 160
                    try
                    {
                        if ((currentMB.Family == null) || (currentMB.Family == ""))
                        {
                            List<string> errpara6 = new List<string>();
                            errpara6.Add(currentMB.Sn);
                            throw new FisException("CHK223", errpara6);
                        }
                    }
                    catch (FisException e)
                    {
                        List<string> errparaFam = new List<string>();
                        errparaFam.Add(currentMB.Sn);
                        errparaFam.Add(e.mErrcode);
                        throw new FisException("CHK223", errparaFam);
                    }

                    ////Kaisheng update 2012/05/23  UC Update ---Delete .c Check
                    ////---------------------------------------- BEGIN--------------------------------------------------
                    //////ADD 2012/2/22 Kaisheng：
                    //////Reason：需求变更：
                    //////c.	检查MBSN是否已经上传测试Log（select * from PCBTestLog nolock where PCBNo = @MBSno），若不存在测试Log，则提示（不是报错）：“此机器SA测试失败，只能刷不良！”，不执行step d、e和f；若存在测试Log，则执行Step d、e和f。
                    //////----------------------------------------------------------------------------------------------
                    //////New Add UC Update Add Station
                    ////DateTime maxpcblogcdt = currentMBRepository.GetMaxCdtFromPCBLog(currentMB.Sn);
                    ////if (maxpcblogcdt == System.DateTime.MinValue)
                    ////      maxpcblogcdt = new DateTime(1900, 1, 1);//System.DateTime.Now.AddDays(-365); 
                    //// old:var lstPCBtestlogbyPCB = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn);
                    //var lstPCBtestlogbyPCB = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, this.Station, maxpcblogcdt);

                    //if ((lstPCBtestlogbyPCB == null) || (lstPCBtestlogbyPCB.Count == 0))
                    //{
                    //    try
                    //    {
                    //        throw new FisException("CHK245", new List<string>());//改号:此机器SA测试失败，只能刷不良！
                    //    }
                    //    catch (FisException ex)
                    //    {

                    //        //CurrentSession.AddValue("NewTerstLogremark", "");
                    //        CurrentSession.AddValue("HavePromptinfo", ex.mErrmsg);
                    //    }
                    //    return base.DoExecute(executionContext);
                    //}
                    //CurrentSession.AddValue("NewTerstLogremark", lstPCBtestlogbyPCB[0].Remark);
                    //-----------------------------------------  END---------------------------------------------------
                    //CurrentSession.AddValue("HavePromptinfo", "YES");

                    ////Kaisheng ,2012/05/23  --uc:d.	若MB没有最新的测试Log，则不执行MBCT的比对
                    ////-------------------------------------------------------------------------------
                    ////d.	获取该MB的MBCT(PCBInfo.InfoValue, Condition: InfoType = ‘MBCT’ and PCBNo=@MBSno)，检查最新上传的测试Log的Remark栏位中MBCT的值是否与MBCT(PCBInfo.InfoValue, Condition: InfoType = ‘MBCT’ and PCBNo=@MBSno)一致，若不一致，则提示：“BIOS燒錄CT錯誤，請重新燒錄！”；若一致，继续执行step e
                    ////--------------------ADD 05/23 ----------------------------------------
                    //var lstPCBtestlogbyPCB = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, this.Station, maxpcblogcdt);
                    //if ((lstPCBtestlogbyPCB != null) && (lstPCBtestlogbyPCB.Count != 0))
                    //{
                    ////-------------------- end
                    //    string strMBCT ="";
                    //    if (currentMB.GetExtendedProperty("MBCT") == null)
                    //        strMBCT = "";
                    //    else
                    //        strMBCT = (string)currentMB.GetExtendedProperty("MBCT");
                    //    strMBCT = strMBCT.Trim();
                    //    string remarkfortestlog = lstPCBtestlogbyPCB[0].Remark;
                    //    var ilocMBCT = remarkfortestlog.IndexOf("MBCT:");
                    //    if (ilocMBCT != -1) 
                    //    {
                    //        ilocMBCT = ilocMBCT + 5;
                    //        string strremarkMBCT = remarkfortestlog.Substring(ilocMBCT);
                    //        var ilenMBCT = strremarkMBCT.IndexOf("~");
                    //        var strMBCTremark = "";
                    //        if (ilenMBCT == -1)
                    //        {
                    //            strMBCTremark = strremarkMBCT;
                    //        }
                    //        else
                    //        {
                    //            strMBCTremark = strremarkMBCT.Substring(0, ilenMBCT);
                    //        }
                    //        //if (strMBCT.IndexOf(strMBCTremark) == -1)
                    //        if (strMBCT != strMBCTremark)
                    //        {
                    //            try
                    //            {
                    //                throw new FisException("CHK246", new List<string>());//改号:BIOS燒錄CT錯誤，請重新燒錄
                    //            }
                    //            catch (FisException ex)
                    //            {

                    //                CurrentSession.AddValue("HavePromptinfo", ex.mErrmsg);
                    //            }
                    //            return base.DoExecute(executionContext); 
                    //        }

                    //    }
                    //    else if (strMBCT !="")
                    //    {
                    //        try
                    //        {
                    //            throw new FisException("CHK246", new List<string>());//改号:BIOS燒錄CT錯誤，請重新燒錄
                    //        }
                    //        catch (FisException ex)
                    //        {

                    //            CurrentSession.AddValue("HavePromptinfo", ex.mErrmsg);
                    //        }
                    //        return base.DoExecute(executionContext); 
                    //    }
                    ////--------------------ADD 05/23 ----------------------------------------
                    //}
                    ////----------------------end
                    //Add By Kaisheng 2012/05/21 UC Update
                    //----------------2012/05/21------------BEGIN--------------------------------------------
                    //e.获取@ImageRemark(rTrim(MB_Test.Remark) Condition：Code=MBCode and Type=0)
                    string ImageRemark = "";
                  //IList<MBTestDef> MbTestRemarkLst = currentMBRepository.GetMBTestList(currentMB.Sn.Trim(), false);
                    IList<MBTestDef> MbTestRemarkLst = currentMBRepository.GetMBTestList_NotCut(strMBCode, false);
                    if ((MbTestRemarkLst != null) && (MbTestRemarkLst.Count != 0))
                    {
                        ImageRemark = MbTestRemarkLst[0].remark.Trim();
                    }
                    //f.板子为RCTO板子（MBSno的CheckCode为R），检查MBSN是否已经上传全测版测试Log
                    // （select * from PCBTestLog nolock where PCBNo = @MBSno and Type = ‘M/B’and Station=当前站
                    //     and Cdt>PCBLog中当前MB的最大过站时间），
                    // 若不存在测试Log，则提示（不是报错）：“此机器未进行全测版测试，只能刷不良！”，
                    // 等待刷入Defect，不进行下面的操作
                    // 整机部分，暂时先不修改MBCode
                    if (currentMB.Sn.ToUpper().Substring(5, 1) == "R")
                    {
                        //检查MBSN是否已经上传全测版测试Log（select * from PCBTestLog nolock where PCBNo = @MBSno and Type = ‘M/B’ and Station=当前站 and Cdt>PCBLog中当前MB的最大过站时间），若不存在测试Log，则提示（不是报错）：“此机器未进行全测版测试，只能刷不良！”，不执行step 6，等待刷入不良代码；若存在测试Log，则继续执行Step 6。
                        string[] param1 = { "M/B" };
                        var lstalltestlog = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, param1, this.Station, maxpcblogcdt);
                        if ((lstalltestlog == null) || (lstalltestlog.Count == 0))
                        {
                            try
                            {
                                throw new FisException("CHK243", new List<string>());
                            }
                            catch (FisException ex)
                            {

                                CurrentSession.AddValue("HavePromptinfo", ex.mErrmsg);
                            }
                            return base.DoExecute(executionContext); 
                        }
                    }
                    //g.若板子存在修护记录（Condition：PCBRepair.PCBNo=@MBSno and Status = 1 and Station<>’19’）
                    // 或PCBTestLog存在Station=08的Fail记录（PCBTestLog.PCBNo=@MBSno and Station=’08’ and Status=’0’），
                    // 则进行如下检查：
                    // 若@ImageRemark=’R’，检查MBSN是否已经上传全测版测试Log
                    // （select * from PCBTestLog nolock where PCBNo = @MBSno and Type = ‘M/B’ and Station=当前站 and Cdt>PCBLog中当前MB的最大过站时间），
                    // 若不存在测试Log，则提示（不是报错）：“此机器未进行全测版测试，只能刷不良！”，等待刷入Defect，不进行下面的操作；
                    
                    //若@ImageRemark<>’R’，检查MBSN是否已经上传测试Log
                    // （select * from PCBTestLog nolock where PCBNo = @MBSno and Station=当前站 and Cdt>PCBLog中当前MB的最大过站时间），
                    // 若不存在，则提示（不是报错）：“该机器未进行测试，只能刷不良！”，等待刷入Defect，不进行下面的操作
                    //UC Update  2012/09/29
                    //修改判断条件：
                    //---------------------------------------------------------------------------------------------
                    //var bRepairRecord = false;
                    //foreach (var repair in currentMB.Repairs)
                    //{
                    //    if ((repair.Status == Repair.RepairStatus.Finished) && (repair.Station.Trim() != "19"))
                    //    {
                    //        bRepairRecord = true;
                    //        break;
                    //    }
                    //}
                    //---------------------------------------------------------------------------------------------
                    //select PCBNo from PCBRepair 
                    //     where PCBNo = @PCBNo and Station<>'10' and Station <>'19' and Status = 1
                    //union
                    //select PCBNo from PCBRepair a, PCBRepair_DefectInfo b 
                    //where a.PCBNo = @PCBNo and a.Station = '10' and a.Status  = 1
                    //and a.ID = b.PCARepairID
                    //     and UPPER(b.Remark) like '%YNB%'
                    //IMBRepository::IList<string> GetPcbNoFromPcbRepairAndPcbRepairDefectInfo(string pcbNo, string remarkLike);
                    var bRepairRecord = false;
                    IList<string> pcblistbyRepair = currentMBRepository.GetPcbNoFromPcbRepairAndPcbRepairDefectInfo(currentMB.Sn, "YNB");
                    if ((pcblistbyRepair == null) || (pcblistbyRepair.Count == 0))
                    {
                        bRepairRecord = false;
                    }
                    else
                    {
                        bRepairRecord = true;
                    }
                    //---------------------------------------------------------------------------------------------
                    var bFailtestlog = false;
                    // IList<TestLog> GetPCBTestLogListFromPCBTestLog(string pcbNo, int status, string station, DateTime beginCdt);
                    DateTime tmpCdt = new DateTime(1900, 1, 1);
                    var Failtestloglst = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, 0, "08", tmpCdt);
                    if ((Failtestloglst != null) && (Failtestloglst.Count != 0))
                    {
                        bFailtestlog = true;
                    }
                    if ((bRepairRecord == true) || (bFailtestlog == true))
                    {
                        if (ImageRemark == "R")
                        {
                            //检查MBSN是否已经上传全测版测试Log（select * from PCBTestLog nolock where PCBNo = @MBSno and Type = ‘M/B’ and Station=当前站 and Cdt>PCBLog中当前MB的最大过站时间），若不存在测试Log，则提示（不是报错）：“此机器未进行全测版测试，只能刷不良！”，不执行step 6，等待刷入不良代码；若存在测试Log，则继续执行Step 6。
                            string[] param1 = { "M/B" };
                            var lstalltestlog = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, param1, this.Station, maxpcblogcdt);
                            if ((lstalltestlog == null) || (lstalltestlog.Count == 0))
                            {
                                try
                                {
                                    throw new FisException("CHK243", new List<string>());
                                }
                                catch (FisException ex)
                                {

                                    CurrentSession.AddValue("HavePromptinfo", ex.mErrmsg);
                                }
                                return base.DoExecute(executionContext);
                            }
                        }
                        else
                        {
                            var lstalltestlog = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, this.Station, maxpcblogcdt);
                            if ((lstalltestlog == null) || (lstalltestlog.Count == 0))
                            {
                                try
                                {
                                    throw new FisException("CHK244", new List<string>());
                                }
                                catch (FisException ex)
                                {

                                    CurrentSession.AddValue("HavePromptinfo", ex.mErrmsg);
                                }
                                return base.DoExecute(executionContext);
                            }
                        }
                    }
                    //h.若板子不存在修护记录（Condition：PCBRepair.PCBNo=@MBSno and Status = 1 and Station<>’19’）
                    //  和
                    //  PCBTestLog--不--存在Station=08的Fail记录（PCBTestLog.PCBNo=@MBSno and Station=’08’ and Status=’0’），
                    //  则进行如下检查：
                    //if ((bRepairRecord == false) && (bFailtestlog == true))
                    if ((bRepairRecord == false) && (bFailtestlog == false)) //Modify 2012/05/29
                    {
                        //若CheckCode等于C或者数字，@ImageRemark<>’R’， 检查MBSN是否已经上传测试Log
                        //（select * from PCBTestLog nolock where PCBNo = @MBSno and Station=当前站 and Cdt>PCBLog中当前MB的最大过站时间），
                        //若不存在，则提示（不是报错）：“该机器未进行测试，只能刷不良！”，等待刷入Defect，不进行下面的操作
                        
                        // 整机部分，暂时先不修改MBCode
                        if ((currentMB.Sn.ToUpper().Substring(5, 1) == "C") || (Char.IsNumber(currentMB.Sn, 5) == true))
                        {
                            if (ImageRemark != "R")
                            {
                                var lstalltestlog = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, this.Station, maxpcblogcdt);
                                if ((lstalltestlog == null) || (lstalltestlog.Count == 0))
                                {
                                    try
                                    {
                                        throw new FisException("CHK244", new List<string>());
                                    }
                                    catch (FisException ex)
                                    {

                                        CurrentSession.AddValue("HavePromptinfo", ex.mErrmsg);
                                    }
                                    return base.DoExecute(executionContext);
                                }
                            }
                        }
                         
                    }
                    //----------------2012/05/21------------ END---------------------------------------------

                    //Move Code From D->H
                    //Kaisheng ,2012/05/23  --uc:d.	若MB没有最新的测试Log，则不执行MBCT的比对
                    //-------------------------------------------------------------------------------
                    //d.	获取该MB的MBCT(PCBInfo.InfoValue, Condition: InfoType = ‘MBCT’ and PCBNo=@MBSno)，检查最新上传的测试Log的Remark栏位中MBCT的值是否与MBCT(PCBInfo.InfoValue, Condition: InfoType = ‘MBCT’ and PCBNo=@MBSno)一致，若不一致，则提示：“BIOS燒錄CT錯誤，請重新燒錄！”；若一致，继续执行step e
                    //--------------------ADD 05/23 ----------------------------------------
                    var lstPCBtestlogbyPCB = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, this.Station, maxpcblogcdt);
                    if ((lstPCBtestlogbyPCB != null) && (lstPCBtestlogbyPCB.Count != 0))
                    {
                        //-------------------- end
                        string strMBCT = "";
                        if (currentMB.GetExtendedProperty("MBCT") == null)
                            strMBCT = "";
                        else
                            strMBCT = (string)currentMB.GetExtendedProperty("MBCT");
                        strMBCT = strMBCT.Trim();
                        string remarkfortestlog = lstPCBtestlogbyPCB[0].Remark;
                        var ilocMBCT = remarkfortestlog.IndexOf("MBCT:");
                        if (ilocMBCT != -1)
                        {
                            ilocMBCT = ilocMBCT + 5;
                            string strremarkMBCT = remarkfortestlog.Substring(ilocMBCT);
                            var ilenMBCT = strremarkMBCT.IndexOf("~");
                            var strMBCTremark = "";
                            if (ilenMBCT == -1)
                            {
                                strMBCTremark = strremarkMBCT;
                            }
                            else
                            {
                                strMBCTremark = strremarkMBCT.Substring(0, ilenMBCT);
                            }
                            //if (strMBCT.IndexOf(strMBCTremark) == -1)
                            if (strMBCT != strMBCTremark)
                            {
                                //Debug Mantis1225
                                //---------------------------------------------
                                var iFindMBCTPos = strMBCTremark.IndexOf(strMBCT);
                                if (iFindMBCTPos == -1)
                                {
                                    try
                                    {
                                        throw new FisException("CHK246", new List<string>()); //改号:BIOS燒錄CT錯誤，請重新燒錄
                                    }
                                    catch (FisException ex)
                                    {

                                        CurrentSession.AddValue("HavePromptinfo", ex.mErrmsg);
                                    }
                                    return base.DoExecute(executionContext);
                                }
                            }

                        }
                        else if (strMBCT != "")
                        {
                            try
                            {
                                throw new FisException("CHK246", new List<string>());//改号:BIOS燒錄CT錯誤，請重新燒錄
                            }
                            catch (FisException ex)
                            {

                                CurrentSession.AddValue("HavePromptinfo", ex.mErrmsg);
                            }
                            return base.DoExecute(executionContext);
                        }
                        //--------------------ADD 05/23 ----------------------------------------
                    }
                    //----------------------end

                    //----------------------------------------------------------------------------------------------
                    //CurrentSession.AddValue("HavePromptinfo", "NO!");
                    
                    //成退板子,并刚做过修护
                    //if exists (select * from PCBRepair nolock where PCBNo=@MBSno and Station='33') and  exists (select * from PCBStatus nolock where PCBNo=@MBSno and Station='24' )
                    var brepairFinished = false;
                    // IList<Repair> mbRepairlogs = currentMB.Repairs;
                    // var properlog = (from p in mbRepairlogs where p.Station == "33" select p).ToList();
                    foreach (var repair in currentMB.Repairs)
                    {
                        //Modify kaisheng:UC变更
                        //去掉刚做过修护（PCBStatus.Station=24【PCA Repair Output】
                        //if ((repair.Station == "33") && (currentMB.MBStatus.Station == "24"))
                        if (repair.Station == "33")
                        {

                            brepairFinished = true;
                            break;
                        }
                    }
                    if (brepairFinished)
                    {
                        DateTime maxcdt = currentMBRepository.GetMaxCdtFromPCBRepair(currentMB.Sn, "33");
                        if (maxcdt == System.DateTime.MinValue)
                            maxcdt = new DateTime(1900, 1, 1);//System.DateTime.Now.AddDays(-7); 
                        //MBCode 2->3
                      //var lstItcNDcheck = currentMBRepository.GetITCNDDefectChecks(currentMB.Sn, "M/B");//(MB_SNo.Substring(0, 2), "M/B");
                        var lstItcNDcheck = currentMBRepository.GetITCNDDefectChecks_NotCut(strMBCode, "M/B");//(MB_SNo.Substring(0, 2), "M/B");
                        
                        string[] param1 = { "M/B" };
                      //var lstItcNDcheckstd = currentMBRepository.GetITCNDDefectChecks(currentMB.Sn, "MB");//(MB_SNo.Substring(0, 2), "MB");
                        var lstItcNDcheckstd = currentMBRepository.GetITCNDDefectChecks_NotCut(strMBCode, "MB");//(MB_SNo.Substring(0, 2), "MB");
                        string[] param2 = { "MB" };
                        var lsttestlogforall = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, param1, 1, this.Station, maxcdt);
                        var lsttestlogforstd = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, param2, 1, this.Station, maxcdt);
                        if (currentMB.Sn.ToUpper().Substring(5, 1) != "R")
                        {
                            if (lstItcNDcheck.Count > 0)
                            {
                                if ((lsttestlogforall == null) || (lsttestlogforall.Count == 0))
                                {
                                    //'該板子需要走全測版測試!'
                                    List<string> errpara7 = new List<string>();
                                    errpara7.Add(currentMB.Sn);
                                    throw new FisException("CHK224", errpara7);
                                }
                                else if (lsttestlogforall[0].Remark.Trim() == "")
                                {
                                    //'該板子需要走全測版測試!'
                                    List<string> errpara7 = new List<string>();
                                    errpara7.Add(currentMB.Sn);
                                    throw new FisException("CHK224", errpara7);
                                }

                            }
                            else if (lstItcNDcheckstd.Count > 0)
                            {
                                if ((lsttestlogforstd == null) || (lsttestlogforstd.Count == 0))
                                {
                                    //'該板子需要走標準測版測試!'
                                    List<string> errpara8 = new List<string>();
                                    errpara8.Add(currentMB.Sn);
                                    throw new FisException("CHK225", errpara8);
                                }
                                else if (lsttestlogforstd[0].Remark.Trim() == "")
                                {
                                    //'該板子需要走標準測版測試!'
                                    List<string> errpara8 = new List<string>();
                                    errpara8.Add(currentMB.Sn);
                                    throw new FisException("CHK225", errpara8);
                                }
                            }
                            else
                            {
                                if ((lsttestlogforall == null) || (lsttestlogforall.Count == 0))
                                {
                                    //'該板子需要走全測版測試!'
                                    List<string> errpara9 = new List<string>();
                                    errpara9.Add(currentMB.Sn);
                                    throw new FisException("CHK224", errpara9);
                                }
                                else if (lsttestlogforall[0].Remark.Trim() == "")
                                {
                                    //'該板子需要走全測版測試!'
                                    List<string> errpara9 = new List<string>();
                                    errpara9.Add(currentMB.Sn);
                                    throw new FisException("CHK224", errpara9);
                                }
                            }
                        }
                        //检查PCBTestLog Remark栏位的格式
                        //var lstItcNDcheckMark = currentMBRepository.GetITCNDDefectChecks(currentMB.Sn);//.Substring(0, 2));
                        var lstItcNDcheckMark = currentMBRepository.GetITCNDDefectChecks_NotCut(strMBCode);//.Substring(0, 2));
                        
                        var lsttestlogforMark = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, 1, this.Station, maxcdt);
                      

                        if (lstItcNDcheckMark.Count > 0)
                        {
                            if ((lsttestlogforMark == null) || (lsttestlogforMark.Count==0))
                            {
                                //測試上傳的Remark欄位格式有誤,請聯繫IE檢查!'
                                List<string> errpararemark10 = new List<string>();
                                //errpara10.Add(currentMB.Sn);
                                throw new FisException("CHK226", errpararemark10);
                            }
                            string logremark = lsttestlogforMark[0].Remark.Trim().ToUpper();
                            //需求变更，去掉remark 50位长度限制
                            //if (!((logremark.Substring(0, 4) == "MAC:") && (logremark.Substring(16, 6) == "~MBCT:") && (logremark.Substring(36, 3) == "~V:") && (logremark.Length == 50)))
                            //为保证检查remark的特征位，须确保其长度〉39
                            if (logremark.Length < 39)
                            {
                                //測試上傳的Remark欄位格式有誤,請聯繫IE檢查!'
                                List<string> errpara10 = new List<string>();
                                //errpara10.Add(currentMB.Sn);
                                throw new FisException("CHK226", errpara10);
                            }
                            if (!((logremark.Substring(0, 4) == "MAC:") && (logremark.Substring(16, 6) == "~MBCT:") && (logremark.Substring(36, 3) == "~V:")))
                            {
                                //測試上傳的Remark欄位格式有誤,請聯繫IE檢查!'
                                List<string> errpara10 = new List<string>();
                                //errpara10.Add(currentMB.Sn);
                                throw new FisException("CHK226", errpara10);
                            }

                        }

                    }
                }
            }
            //------------------------------------------------------------------------------------------------
            //currentMBRepository.Update(currentMB, CurrentSession.UnitOfWork);
            return base.DoExecute(executionContext);
        }
	}
}
