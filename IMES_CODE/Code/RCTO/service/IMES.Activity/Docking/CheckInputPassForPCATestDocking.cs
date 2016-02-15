// INVENTEC corporation (c)2009 all rights reserved. 
// Description: PCA Test Station For Docking
// 保存前检查，
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-07-03   Kaisheng                     create
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
    /// PCA Test Station 当前检查站为SMD_A或SMD_B，则检查是否已经通过此站，若通过此战，则报告错误,不良品,良品保存前数据库校验
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于PCA Test Station For Docking
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         不良品,良品保存前数据库校验
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
    public partial class CheckInputPassForPCATestDocking : BaseActivity
	{

        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckInputPassForPCATestDocking()
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

            string strMBCode = "";
            if (currentMB.Sn.Substring(5, 1) == "M")
            {
                strMBCode = currentMB.Sn.Substring(0, 3);
            }
            else if (currentMB.Sn.Substring(4, 1) == "M")
            {
                strMBCode = currentMB.Sn.Substring(0, 2);
            }
            
            //12. Check Input Pass
            //若当前检查站为SMD_A或SMD_B，则检查是否已经通过此站，若通过此战，则报告错误：“SMD A/B Duplicate Input!”
            //IList<MBLog> mblogs = currentMB.MBLogs;
            var strStation = this.Station;
            var strcurstation = this.Station;
            //if (strStation.Length >= 2)
            //{
            //    if ((strStation.Substring(0, 2) == "05") || (strStation.Substring(0, 2) == "06"))
            //    {
            //        foreach (var mblogcheck in currentMB.MBLogs)
            //        {
            //            if ((mblogcheck.StationID.Substring(0, 2) == "05") || (mblogcheck.StationID.Substring(0, 2) == "06"))
            //            {
            //                /*
            //                if (Session != null)
            //                {
            //                    SessionManager.GetInstance.RemoveSession(Session);
            //                }
            //                 */
            //                //SMD A/B Duplicate Input!重复输入
            //                List<string> errparaA = new List<string>();
            //                //errparaA.Add(MB_SNo);
            //                throw new FisException("CHK227", errparaA);
            //            }
            //        }
            //    }
            //}
            
          //var lstmbtestrec = currentMBRepository.GetMBTestList(currentMB.Sn.Trim(), false);
            var lstmbtestrec = currentMBRepository.GetMBTestList_NotCut(strMBCode, false);
            DateTime maxcdt = currentMBRepository.GetMaxCdtFromPCBLog(currentMB.Sn);
            //Session.AddValue(Session.SessionKeys.DefectList, defectList);
            IList<string> defectList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.DefectList);

            if (maxcdt == System.DateTime.MinValue)
                maxcdt = new DateTime(1900, 1, 1);//System.DateTime.Now.AddDays(-365); 
            if (!(defectList == null || defectList.Count == 0))
            {
                //Delete :2012/03/09 :UC Update,删除不良品相关的检查
                //-----------------------------------------------------------------------------------------------------------------
                /*
                //不良品
                var lsttestfaillog1 = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, 1, this.Station, maxcdt);
                var lsttestfaillog0 = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, 0, this.Station, maxcdt);
                //不良品，且进行测试log卡站设置(Table MB_Test存在记录，Conditon: MB_Test.Code = MBCode and MB_Test.Type=0)，则进行以下检查
                //if (lstmbtestrec.Count > 0)
                //Modify Kaisheng,UC Changed  不良品，检测站为“15：SA1 Test；16：SA2 Test；17：SA3 Test”，且进行测试log卡站设置(Table MB_Test存在记录，Conditon: MB_Test.Code = MBCode and MB_Test.Type=0)，则进行以下检查
                if ((lstmbtestrec.Count > 0) && ((strcurstation == "15") || (strcurstation == "16") || (strcurstation == "17")))
                {
                    if (lsttestfaillog1.Count > 0)
                    {
                        /
                        //if (Session != null)
                        //{
                        //    SessionManager.GetInstance.RemoveSession(Session);
                        //}
                         /
                        //Function Test Is OK,Can Not Input As Failure!'
                        List<string> errparaB = new List<string>();
                        //errparaB.Add(MB_SNo);
                        throw new FisException("CHK228", errparaB);
                    }
                    if ((lsttestfaillog0 == null) || (lsttestfaillog0.Count == 0))
                    {
                        //if (Session != null)
                        //{
                        //    SessionManager.GetInstance.RemoveSession(Session);
                        //}
                 
                        //Please Goto SA Funtion Test!
                        List<string> errparaC = new List<string>();
                        //errparaC.Add(MB_SNo);
                        throw new FisException("CHK229", errparaC);
                    }
                }
                */
                //------------------------------------------------------------------------------------------------------------------
            }
            else
            {
                //良品

                ////Add Kaisheng: 需求变更
                ////2.	若MB未刷入Defect，且系统已经提示：“不良机器，需输入Defect Code!”或“BIOS燒錄CT錯誤，請重新燒錄！”，则报错：“不良机器，需输入Defect Code!”
                //string strcheckPromptinfo = (string)CurrentSession.GetValue("HavePromptinfo");
                //if (strcheckPromptinfo != "NO!") 
                //{
                //    //“不良机器，需输入Defect Code!”
                //    List<string> errparaNewA = new List<string>();
                //    //errparaF.Add(MB_SNo);
                //    throw new FisException("CHK247", errparaNewA);
                //}
                //---------------------------------------------------------------------------------------------------
                //Add Kaisheng: 需求变更
                //3.	MB为良品，检测站为“15：SA1 Test；16：SA2 Test；17：SA3 Test”，根据PCATest_Check表中设置信息进行测试Log最新上传的Remark栏位信息卡站
                //GetPcaTestCheckInfoListByCode
                
                //if ((strcurstation == "15") || (strcurstation == "16") || (strcurstation == "17"))
                //{
                //    var strgetremark ="";
                //    //Modify Kaisheng UC Update Add Station Check
                //    //var lstPCBtestlogbyPCB = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn);
                //    var lstPCBtestlogbyPCB = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, this.Station, maxcdt);

                //    if ((lstPCBtestlogbyPCB == null) || (lstPCBtestlogbyPCB.Count == 0))
                //        strgetremark = "";
                //    else
                //        strgetremark = lstPCBtestlogbyPCB[0].Remark; 

                //    //var strgetremark = (string)CurrentSession.GetValue("NewTerstLogremark");
                //  //var lstPcatestCheckinfo=currentMBRepository.GetPcaTestCheckInfoListByCode(currentMB.Sn);
                //    var lstPcatestCheckinfo = currentMBRepository.GetPcaTestCheckInfoListByCode_NotCut(strMBCode);
                //    if ((lstPcatestCheckinfo != null) && (lstPcatestCheckinfo.Count != 0))
                //    {
                //        //i.	若PCATest_Check.MAC = ‘Y’
                //        //若Remark栏位获取的MAC值，与PCB中绑定的MAC(PCB.MAC)信息不一致，则报错：“MAC比对错误”
                        
                //        if (lstPcatestCheckinfo[0].mac.Trim() == "Y")
                //        {
                //            //CurrentSession.AddValue("NewTerstLogremark", lstPCBtestlogbyPCB[0].Remark);
                //            string strMAC = currentMB.MAC == null ? "" : currentMB.MAC.Trim();
                //            var ilocMAC = strgetremark.IndexOf("MAC:");
                //            if (ilocMAC != -1)
                //            {
                //                ilocMAC = ilocMAC + 4;
                //                string strremarkMAC = strgetremark.Substring(ilocMAC);
                //                var ilenMAC = strremarkMAC.IndexOf("~");
                //                var strMACremark = "";
                //                if (ilenMAC == -1)
                //                    strMACremark = strremarkMAC;
                //                else
                //                    strMACremark = strremarkMAC.Substring(0, ilenMAC);
                //                if (strMAC != strMACremark)
                //                {
                //                    //“MAC比对错误”
                //                    List<string> errparaNewB = new List<string>();
                //                    //errparaF.Add(MB_SNo);
                //                    throw new FisException("CHK248", errparaNewB);
                //                }
                //            }
                //            else if (strMAC != "")
                //            {
                //                //“MAC比对错误”
                //                List<string> errparaNewB = new List<string>();
                //                //errparaF.Add(MB_SNo);
                //                throw new FisException("CHK248", errparaNewB);
                //            }
                //        }
                //        //ii.	若PCATest_Check.MBCT = ‘Y’
                //        //若Remark栏位获取的MBCT值，与PCB绑定的MBCT（PCBInfo.InfoValue, Condition: InfoType=’MBCT’）信息不一致，则报错：“MBCT比对错误”
                //        if (lstPcatestCheckinfo[0].mbct.Trim() == "Y")
                //        {
                //            var ilocMBCT = strgetremark.IndexOf("MBCT:");
                //            var strMBCT = (string)currentMB.GetExtendedProperty("MBCT");
                //            if (strMBCT == null)
                //                strMBCT = "";
                //            if (ilocMBCT != -1)
                //            {
                //                ilocMBCT = ilocMBCT + 5;
                //                string strremarkMBCT = strgetremark.Substring(ilocMBCT);
                //                var ilenMBCT = strremarkMBCT.IndexOf("~");
                //                var strMBCTremark="";
                //                if (ilenMBCT == -1)
                //                    strMBCTremark = strremarkMBCT;
                //                else
                //                    strMBCTremark = strremarkMBCT.Substring(0, ilenMBCT);
                //                //if (strMBCT.IndexOf(strMBCTremark) == -1)

                //                if (strMBCT != strMBCTremark)
                //                {
                //                    //MBCT比对错误
                //                    List<string> errparaNewC = new List<string>();
                //                    //errparaF.Add(MB_SNo);
                //                    throw new FisException("CHK249", errparaNewC);
                //                }

                //            }
                //            else if (strMBCT != "")
                //            {
                //                //MBCT比对错误
                //                List<string> errparaNewC = new List<string>();
                //                //errparaF.Add(MB_SNo);
                //                throw new FisException("CHK249", errparaNewC);
                //            }
                //        }
                //        //iii.	@HDDV=PCATest_Check.HDDV，@HDDV不等于空(‘’)且不等于’N’，若Remark栏位不包含@HDDV的信息，则报错：“HDDV比对错误”
                //        //Kaisheng 03/01 UC变更 
                //        //比对时，将HDDV和BIOS取出值转换成大写，且将Remark栏位的字符串转换成大写，再比对
                //        string[] remarklst = strgetremark.Split('~');
                //        string strBIOSinRemark = "BIOS:";
                //        string strHDDVinRemark = "V:";
                //        foreach (string splitItem in remarklst)
                //        {
                //            if ((splitItem != null) && (splitItem != ""))
                //            {
                //                if (splitItem.Substring(0, 2) == "V:")
                //                {
                //                    strHDDVinRemark = splitItem.ToUpper();
                //                }
                //                else if (splitItem.Substring(0, 5) == "BIOS:")
                //                {
                //                    strBIOSinRemark = splitItem.ToUpper();
                //                }
                //            }

                //        }

                //        if ((lstPcatestCheckinfo[0].hddv != "") && (lstPcatestCheckinfo[0].hddv != "N"))
                //        {
                //            string strHDDVremark = "V:" + lstPcatestCheckinfo[0].hddv.Trim().ToUpper();
                //            //var ilocHDDV = strgetremark.IndexOf(strHDDVremark);
                //            //if (ilocHDDV == -1)
                //            if (strHDDVremark != strHDDVinRemark)
                //            {
                //                //V比对错误
                //                List<string> errparaNewD = new List<string>();
                //                //errparaF.Add(MB_SNo);
                //                throw new FisException("CHK250", errparaNewD);
                //            }
                //        }
                //        //iv.	@BIOS=PCATest_Check.BIOS，@ BIOS不等于空(‘’)且不等于’N’，若Remark栏位不包含@BIOS的信息，则报错：“BIOS比对错误”
                //        if ((lstPcatestCheckinfo[0].bios != "") && (lstPcatestCheckinfo[0].bios != "N"))
                //        {
                //            string strBIOSremark = "BIOS:" + lstPcatestCheckinfo[0].bios.Trim().ToUpper();
                //            //if (strgetremark.IndexOf(strBIOSremark) == -1)
                //            if (strBIOSremark != strBIOSinRemark)
                //            {
                //                //BIOS比对错误
                //                List<string> errparaNewE = new List<string>();
                //                //errparaF.Add(MB_SNo);
                //                throw new FisException("CHK251", errparaNewE);
                //            }
                //        }

                //    }
                //    else
                //    {
                //        PcaTestCheckInfo testCheckinfo = new PcaTestCheckInfo();
                //        testCheckinfo.code = currentMB.Family;
                //        var lstPcatestCheckinfobyFAM = currentMBRepository.GetPcaTestCheckInfoListByCode(testCheckinfo);
                //        if ((lstPcatestCheckinfobyFAM != null) && (lstPcatestCheckinfobyFAM.Count != 0))
                //        {
                //            //i.	若PCATest_Check.MAC = ‘Y’若Remark栏位获取的MAC值与PCB绑定的MAC信息不一致，则报错：“MAC比对错误”
                //            if (lstPcatestCheckinfobyFAM[0].mac.Trim() == "Y")
                //            {
                //                /*
                //                string strMAC = currentMB.MAC == null ? "" : currentMB.MAC.Trim();
                //                string strMACforremark ="";
                //                strMACforremark = "MAC:" + strMAC;
                //                if (strgetremark.IndexOf(strMACforremark) == -1)
                //                {
                                    
                //                    //MAC比对错误
                //                    List<string> errparaNewF = new List<string>();
                //                    //errparaF.Add(MB_SNo);
                //                    throw new FisException("CHK248", errparaNewF);
                //                }
                //                else if (strMAC == "")
                //                {
                //                    //MAC比对错误
                //                    List<string> errparaNewF = new List<string>();
                //                    //errparaF.Add(MB_SNo);
                //                    throw new FisException("CHK248", errparaNewF);
                //                }*/

                //                string strMAC = currentMB.MAC == null ? "" : currentMB.MAC.Trim();
                //                var ilocMAC = strgetremark.IndexOf("MAC:");
                //                if (ilocMAC != -1)
                //                {
                //                    ilocMAC = ilocMAC + 4;
                //                    string strremarkMAC = strgetremark.Substring(ilocMAC);
                //                    var ilenMAC = strremarkMAC.IndexOf("~");
                //                    var strMACremark = "";
                //                    if (ilenMAC == -1)
                //                        strMACremark = strremarkMAC;
                //                    else
                //                        strMACremark = strremarkMAC.Substring(0, ilenMAC);
                //                    if (strMAC != strMACremark)
                //                    {
                //                        //“MAC比对错误”
                //                        List<string> errparaNewF = new List<string>();
                //                        //errparaF.Add(MB_SNo);
                //                        throw new FisException("CHK248", errparaNewF);
                //                    }
                //                }
                //                else if (strMAC != "")
                //                {
                //                    //“MAC比对错误”
                //                    List<string> errparaNewB = new List<string>();
                //                    //errparaF.Add(MB_SNo);
                //                    throw new FisException("CHK248", errparaNewB);
                //                }
                //            }
                //            //ii.	若PCATest_Check.MBCT = ‘Y’若Remark栏位获取的MBCT值与PCB绑定的MBCT信息不一致，则报错：“MBCT比对错误”
                //            if (lstPcatestCheckinfobyFAM[0].mbct.Trim() == "Y")
                //            {
                //                /*
                //                var strMBCT = (string)currentMB.GetExtendedProperty("MBCT");
                //                if (strMBCT == null)
                //                    strMBCT = "";
                //                string strMBCTforremark = "MBCT:" + strMBCT.Trim();

                //                if (strgetremark.IndexOf(strMBCTforremark) == -1)
                //                {
                //                    //MBCT比对错误
                //                    List<string> errparaNewG = new List<string>();
                //                    //errparaF.Add(MB_SNo);
                //                    throw new FisException("CHK249", errparaNewG);
                //                }
                //                else if (strMBCT.Trim() == "")
                //                {
                //                    //MBCT比对错误
                //                    List<string> errparaNewG = new List<string>();
                //                    //errparaF.Add(MB_SNo);
                //                    throw new FisException("CHK249", errparaNewG);
                //                }
                //                 */
                //                var strMBCT = (string)currentMB.GetExtendedProperty("MBCT");
                //                if (strMBCT == null)
                //                    strMBCT = "";
                //                strMBCT = strMBCT.Trim();
                //                var ilocMBCT = strgetremark.IndexOf("MBCT:");
                //                if (ilocMBCT != -1)
                //                {
                //                    ilocMBCT = ilocMBCT + 5;
                //                    string strremarkMBCT = strgetremark.Substring(ilocMBCT);
                //                    var ilenMBCT = strremarkMBCT.IndexOf("~");
                //                    var strMBCTremark = "";
                //                    if (ilenMBCT == -1)
                //                        strMBCTremark = strremarkMBCT;
                //                    else
                //                        strMBCTremark = strremarkMBCT.Substring(0, ilenMBCT);
                //                    if (strMBCT != strMBCTremark)
                //                    {
                //                        //MBCT比对错误
                //                        List<string> errparaNewG = new List<string>();
                //                        //errparaF.Add(MB_SNo);
                //                        throw new FisException("CHK249", errparaNewG);
                //                    }

                //                }
                //                else if (strMBCT != "")
                //                {
                //                    //MBCT比对错误
                //                    List<string> errparaNewG = new List<string>();
                //                    //errparaF.Add(MB_SNo);
                //                    throw new FisException("CHK249", errparaNewG);
                //                }
                //            }

                //        }

                        
                //    }
                //}


                //Add By Kaisheng 2012/05/22 UC Update
                //1)若板子不存在修护记录（Condition：PCBRepair.PCBNo=@MBSno and Status = 1 and Station<>’19’）
                //  和
                //  PCBTestLog存在Station=08的Fail记录（PCBTestLog.PCBNo=@MBSno and Station=’08’ and Status=’0’），
                //  CheckCode等于C或者数字，@ImageRemark=’R’，则不进行下面的操作
                
                //Note:
                //@ImageRemark: rtrim(MB_Test.Remark)
                //-------2012/05/22---------------BEGIN------------------------------
                bool bCheckFollowStep = true;
                //if ((lstmbtestrec.Count > 0) && ((strcurstation == "15") || (strcurstation == "16") || (strcurstation == "17")))
                //{
                //    var bRepairRecord = false;
                //    var bFailtestlog = false;
                //    foreach (var repair in currentMB.Repairs)
                //    {
                //        if ((repair.Status == Repair.RepairStatus.Finished) && (repair.Station.Trim() != "19"))
                //        {
                //            bRepairRecord = true;
                //            break;
                //        }
                //    }
                //    // IList<TestLog> GetPCBTestLogListFromPCBTestLog(string pcbNo, int status, string station, DateTime beginCdt);
                    
                //    DateTime tmpCdt = new DateTime(1900, 1, 1);
                //    var Failtestloglst = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, 0, "08", tmpCdt);
                //    if ((Failtestloglst != null) && (Failtestloglst.Count != 0))
                //    {
                //        bFailtestlog = true;
                //    }
                //    //if ((bRepairRecord == false) && (bFailtestlog == true))
                //    if ((bRepairRecord == false) && (bFailtestlog == false))  //Modify 2012/05/29 
                //    {
                //        if ((currentMB.Sn.ToUpper().Substring(5, 1) == "C") || (Char.IsNumber(currentMB.Sn, 5) == true))
                //        {
                //            string ImageRemark = "";
                //          //IList<MBTestDef> MbTestRemarkLst = currentMBRepository.GetMBTestList(currentMB.Sn.Trim(), false);
                //            IList<MBTestDef> MbTestRemarkLst = currentMBRepository.GetMBTestList_NotCut(strMBCode, false);
                //            if ((MbTestRemarkLst != null) && (MbTestRemarkLst.Count != 0))
                //            {
                //                ImageRemark = MbTestRemarkLst[0].remark.Trim();
                //            }
                //            if (ImageRemark == "R")
                //            {
                //                bCheckFollowStep = false;
                //            }
                //        }
                //    }
                //}
                //-------------------------------- END-------------------------------
                if (bCheckFollowStep)
                {
                    //系统设置匹配测试Log

                    string[] param = { "MB", "M/B", "RCTO" };
                    var lsttestlog = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, param, 0, this.Station, maxcdt);
                    var lsttestlognoexist = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, param, 1, this.Station, maxcdt);

                    //进行测试log卡站设置(Table MB_Test存在记录，Conditon: MB_Test.Code = MBCode and MB_Test.Type=0)，则进行以下检查
                    //Modify Kaisheng:UC变更
                    //MB为良品，检测站为“15：SA1 Test；16：SA2 Test；17：SA3 Test”，，且进行测试log卡站设置(Table MB_Test存在记录，Conditon: MB_Test.Code = MBCode and MB_Test.Type=0)，则进行以下检查
                    //if (lstmbtestrec.Count > 0)
                    if ((lstmbtestrec.Count > 0) && ((strcurstation == "15") || (strcurstation == "16") || (strcurstation == "17")))
                    {
                        if (lsttestlog.Count > 0)
                        {
                            /*
                            if (Session != null)
                            {
                                SessionManager.GetInstance.RemoveSession(Session);
                            }*/
                            //'Function Test Is Failure,Can Not Input As OK!'
                            List<string> errparaD = new List<string>();
                            //errparaD.Add(MB_SNo);
                            throw new FisException("CHK230", errparaD);
                        }
                        else if ((lsttestlognoexist == null) || (lsttestlognoexist.Count == 0))
                        {
                            /*
                            if (Session != null)
                            {
                                SessionManager.GetInstance.RemoveSession(Session);
                            }
                             */
                            //若测试记录不存在，则报告错误：“Please Goto SA Funtion Test!”
                            //'Please Goto SA Funtion Test!'
                            List<string> errparaE = new List<string>();
                            //errparaE.Add(MB_SNo);
                            throw new FisException("CHK229", errparaE);
                        }
                    }
                }
                ////Modify Kaisheng:UC变更 , 取消--检查Test Log Remark栏位已设置
                //---------------------------------------------------------------------------------------------------------------
                //检查Test Log Remark栏位已设置
                /*
                var lstITCNDefect = currentMBRepository.GetITCNDDefectChecks(currentMB.Sn);
                lsttestlog = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, 1, this.Station, maxcdt);
                //DEBUG ITC-1360-0286 -- 且进行测试log卡站设置(Table MB_Test存在记录，Conditon: MB_Test.Code = MBCode and MB_Test.Type=0)
                if (lstmbtestrec.Count > 0)
                {

                    var btestlogExist = false;
                    if (lsttestlog.Count > 0)
                    {
                        foreach (var testlogcheck in lsttestlog)
                        {
                            if (testlogcheck.Remark.ToUpper().IndexOf(currentMB.MAC.ToUpper()) != -1)
                            {
                                btestlogExist = true;
                                break;
                            }
                        }
                    }
                    if (lstITCNDefect.Count > 0)
                    {
                        if ((currentMB.Sn.Trim().ToUpper().Substring(0, 2) != "5K") && (currentMB.Sn.Trim().ToUpper().Substring(0, 2) != "G3"))
                        {
                            if (((lsttestlog == null) || (lsttestlog.Count == 0)) || (btestlogExist == false))
                            //if (((lsttestlog == null) || (lsttestlog.Count == 0)) &&(btestlogExist)) 
                            {
                                
                                //if (Session != null)
                                //{
                                //    SessionManager.GetInstance.RemoveSession(Session);
                                //}
                                 
                                //'測試資料上傳不正確,請確認'
                                List<string> errparaF = new List<string>();
                                //errparaF.Add(MB_SNo);
                                throw new FisException("CHK231", errparaF);
                            }
                        }
                    }
                }
                */
                //------------------------------------------------------------------------------------------------------------------

                ////currentMB.IsVB 未实现
                ////-----------------------------------------
                //try
                //{
                //    if (currentMB.IsVB)
                //    {
                //        CurrentSession.AddValue("IsMBOKVGA", true);
                        
                //    }
                //    else
                //    {
                //        CurrentSession.AddValue("IsMBOKVGA", false);
                //    }
                //}
                //catch (FisException)
                //{
                //    CurrentSession.AddValue("IsMBOKVGA", false);
                //}
            }
                      //--------------------------------------------------
            return base.DoExecute(executionContext);
        }
	}
}
