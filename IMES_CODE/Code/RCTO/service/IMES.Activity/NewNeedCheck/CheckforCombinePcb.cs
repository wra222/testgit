// INVENTEC corporation (c)2012 all rights reserved. 
// Description: CombinePCBinLot Check Fuction
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-07-10   Kaisheng                     create
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
    /// CombinePCBinLot 检查
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于CombinePCBinLot
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         Check 
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
    public partial class CheckforCombinePcb : BaseActivity
	{

        /// <summary>
        /// 构造函数
        /// </summary>
		public CheckforCombinePcb()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Combine Pcb in Lot 检查异常情况
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IMB currentMB = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
            IMBRepository currentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();

            CurrentSession.AddValue("NewTerstLogremark", "");

            //2.28.	MBCode:若第6码为’M’，则取MBSN前3码为MBCode，若第5码为’M’，则取前2码
            //CheckCode:若MBSN的第5码为’M’，则取MBSN的第6码，否则取第7码
            string strMBCode = "";
            string strCheckCode = "";

            if (currentMB.Sn.Substring(5, 1) == "M")
            {
                strMBCode = currentMB.Sn.Substring(0, 3);
                strCheckCode = currentMB.Sn.Substring(6, 1);
            }
            else if (currentMB.Sn.Substring(4, 1) == "M")
            {
                strMBCode = currentMB.Sn.Substring(0, 2);
                strCheckCode = currentMB.Sn.Substring(5, 1);

            }
            else
            {
                strMBCode = currentMB.Sn.Substring(0, 2);
                strCheckCode = currentMB.Sn.Substring(5, 1);
            }
            //--------------------------------------------------
            //--------------------------------------BEGIN-------
            //1.是否存在成功过ICT Input的Log（PCBLog.Station=10 and Status=1），若不存在，则报错：“请先刷入ICT Input”
            IList<MBLog> resultLogList = currentMBRepository.GetMBLog(currentMB.Sn, "10", 1);
            if (resultLogList == null || resultLogList.Count == 0)
            {
                List<string> errpara2 = new List<string>();
                //errpara2.Add(currentMB.Sn);
                errpara2.Add(currentMB.Sn);
                throw new FisException("PAK078", errpara2);
            }
            //2.	是否在修护区若存在记录，则报错：“请先将MBSno刷出修护区”参考方法：
            //      select * from PCBRepair nolock where PCBNo = @MBSno and Status = '0'
            Repair repairreturn = currentMB.GetCurrentRepair();
            if (repairreturn != null)
            {
                List<string> errpara3 = new List<string>();
                errpara3.Add(currentMB.Sn);
                throw new FisException("CHK220", errpara3);
            }
            //3.	是否结合主机若存在记录，则报错：“此MB已经结合了主机”参考方法：
            //select * from IMES2012_FA..Product nolock where PCBID = @MBSno
            IProductRepository ProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            if (ProductRepository.IfBindPCB(currentMB.Sn))
            {
                List<string> errpara4 = new List<string>();
                errpara4.Add(currentMB.Sn);
                throw new FisException("CHK221", errpara4);
            }
            //4.	若MB_Test存在维护记录（MB_Test.Code=MBCode and Type=0），则进行如下操作。
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
                   if ((currentMB.Family == null) || (currentMB.Family == ""))
                    {
                        List<string> errpara6 = new List<string>();
                        errpara6.Add(currentMB.Sn);
                        throw new FisException("CHK223", errpara6);
                    }
                  //c.	获取@ImageRemark(rTrim(MB_Test.Remark) Condition：Code=MBCode and Type=0)
                   string ImageRemark = "";
                   IList<MBTestDef> MbTestRemarkLst = currentMBRepository.GetMBTestList_NotCut(strMBCode, false);
                   if ((MbTestRemarkLst != null) && (MbTestRemarkLst.Count != 0))
                   {
                       ImageRemark = MbTestRemarkLst[0].remark.Trim();
                   }
                  //d.	板子为RCTO板子（MBSno的CheckCode为R），检查MBSN是否已经上传全测版测试Log
                  //（select * from PCBTestLog nolock where PCBNo = @MBSno and Type = ‘M/B’ 
                  //and Station=当前站 and Cdt>PCBLog中当前MB的最大过站时间），
                  //若不存在测试Log，则报错：“此机器未进行全测版测试，请去SA1 Test刷不良！”
                   DateTime maxpcblogcdt = currentMBRepository.GetMaxCdtFromPCBLog(currentMB.Sn);
                   if (maxpcblogcdt == System.DateTime.MinValue)
                       maxpcblogcdt = new DateTime(1900, 1, 1);//System.DateTime.Now.AddDays(-365); 

                  //-----------------------------d.--------------------------------------------
                    if (strCheckCode== "R")
                    {
                        //检查MBSN是否已经上传全测版测试Log（select * from PCBTestLog nolock where PCBNo = @MBSno and Type = ‘M/B’ and Station=当前站 and Cdt>PCBLog中当前MB的最大过站时间），若不存在测试Log，则提示（不是报错）：“此机器未进行全测版测试，只能刷不良！”，不执行step 6，等待刷入不良代码；若存在测试Log，则继续执行Step 6。
                        string[] param1 = { "M/B" };
                        var lstalltestlog = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, param1, this.Station, maxpcblogcdt);
                        if ((lstalltestlog == null) || (lstalltestlog.Count == 0))
                        {
                             throw new FisException("CHK243", new List<string>());//此机器未进行全测版测试，请去SA1 Test刷不良  --改
                        }
                    }
                  //---------------------------------------------------------------------------
                  // e.	若板子存在修护记录（Condition：PCBRepair.PCBNo=@MBSno and Status = 1 and Station<>’19’）
                  //    或PCBTestLog不存在Station=08的Fail记录（PCBTestLog.PCBNo=@MBSno and Station=’08’ and Status=’0’），
                  //    则进行如下检查：
                    var bRepairRecord = false;
                    var bFailtestlog = false;
                    foreach (var repair in currentMB.Repairs)
                    {
                        if ((repair.Status == Repair.RepairStatus.Finished) && (repair.Station.Trim() != "19"))
                        {
                            bRepairRecord = true;
                            break;
                        }
                    }
                    DateTime tmpCdt = new DateTime(1900, 1, 1);
                    var Failtestloglst = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, 0, "08", tmpCdt);
                    if ((Failtestloglst != null) && (Failtestloglst.Count != 0))
                    {
                        bFailtestlog = true;
                    }
                    //若@ImageRemark=’R’，检查MBSN是否已经上传全测版测试Log（select * from PCBTestLog nolock where PCBNo = @MBSno and Type = ‘M/B’ and Station=当前站 and Cdt>PCBLog中当前MB的最大过站时间），若不存在测试Log，则报错：“此机器未进行全测版测试， 请去SA1 Test 刷不良！”
                    //若@ImageRemark<>’R’，检查MBSN是否已经上传测试Log（select * from PCBTestLog nolock where PCBNo = @MBSno and Station=当前站 and Cdt>PCBLog中当前MB的最大过站时间），若不存在，则报错：“该机器未进行测试，请去SA1 Test 刷不良！”
                    if ((bRepairRecord == true) || (bFailtestlog == true))
                    {
                        if (ImageRemark == "R")
                        {
                            string[] param1 = { "M/B" };
                            var lstalltestlog = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, param1, this.Station, maxpcblogcdt);
                            if ((lstalltestlog == null) || (lstalltestlog.Count == 0))
                            {
                                throw new FisException("CHK243", new List<string>());
                            }
                        }
                        else
                        {
                            var lstalltestlog = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, this.Station, maxpcblogcdt);
                            if ((lstalltestlog == null) || (lstalltestlog.Count == 0))
                            {
                                throw new FisException("CHK244", new List<string>());
                            }
                        }
                    }
                    //f.	若板子不存在修护记录（Condition：PCBRepair.PCBNo=@MBSno and Status = 1 and Station<>’19’）
                    //和 PCBTestLog存在Station=08的Fail记录（PCBTestLog.PCBNo=@MBSno and Station=’08’ and Status=’0’），
                    //则进行如下检查：
                    //若CheckCode等于C或者数字，@ImageRemark<>’R’， 检查MBSN是否已经上传测试Log（select * from PCBTestLog nolock where PCBNo = @MBSno and Station=当前站 and Cdt>PCBLog中当前MB的最大过站时间），若不存在，则报错：“该机器未进行测试，请去SA1 Test刷不良！”
                    if ((bRepairRecord == false) && (bFailtestlog == false)) //Modify 2012/05/29
                    {
                       if ((strCheckCode == "C") || (Char.IsNumber(strCheckCode, 0) == true))
                        {
                            if (ImageRemark != "R")
                            {
                                var lstalltestlog = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, this.Station, maxpcblogcdt);
                                if ((lstalltestlog == null) || (lstalltestlog.Count == 0))
                                {
                                   throw new FisException("CHK244", new List<string>());
                                }
                            }
                        }
                    }
                    //g.	若MB没有最新的测试Log，则不执行MBCT的比对。获取该MB的MBCT(PCBInfo.InfoValue, Condition: InfoType = ‘MBCT’ and PCBNo=@MBSno)，
                    //检查最新（测试Log上传时间> PCBLog中当前MB的最大过站时间）上传当前检测站的测试Log的Remark栏位中MBCT的值是否与
                    //MBCT(PCBInfo.InfoValue, Condition: InfoType = ‘MBCT’ and PCBNo=@MBSno)一致，
                    //若不一致，则报错：“BIOS燒錄CT錯誤，請重新燒錄！
                    var lstPCBtestlogbyPCB = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, this.Station, maxpcblogcdt);
                    if ((lstPCBtestlogbyPCB != null) && (lstPCBtestlogbyPCB.Count != 0))
                    {
                        string strMBCT = "";
                        strMBCT = currentMB.GetExtendedProperty("MBCT") == null ? "": (string) currentMB.GetExtendedProperty("MBCT");
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
                            if (strMBCT != strMBCTremark)
                            {
                                ////Debug Mantis1225
                                //------------------------------------------------
                                var iFindMBCTPos = strMBCTremark.IndexOf(strMBCT);
                                if (iFindMBCTPos == -1)
                                {
                                    throw new FisException("CHK246", new List<string>());//改号:BIOS燒錄CT錯誤，請重新燒錄
                                }
                            }

                        }
                        else if (strMBCT != "")
                        {
                            throw new FisException("CHK246", new List<string>());//改号:BIOS燒錄CT錯誤，請重新燒錄
                        }
                    }
                  //h.	板子为非RCTO(MBSno的CheckCode为不为R)
                  //if (strCheckCode != "R")
                  //{
                      //若板子为成退板子（成退WC=33，PCBRepair存在成退的维修信息）
                      var brepairFinished = false;
                      foreach (var repair in currentMB.Repairs)
                      {
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
                          var lstItcNDcheck = currentMBRepository.GetITCNDDefectChecks_NotCut(strMBCode, "M/B");//(MB_SNo.Substring(0, 2), "M/B");

                          string[] param1 = { "M/B" };
                          var lstItcNDcheckstd = currentMBRepository.GetITCNDDefectChecks_NotCut(strMBCode, "MB");//(MB_SNo.Substring(0, 2), "MB");
                          string[] param2 = { "MB" };
                          var lsttestlogforall = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, param1, 1, this.Station, maxcdt);
                          var lsttestlogforstd = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, param2, 1, this.Station, maxcdt);
                          if (strCheckCode != "R")
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
                          //i.	若板子为成退板子（成退WC=33，PCBRepair存在成退的维修信息），且ITCNDefect Check Maintain (Table: Maintain_ITCNDefect_Check) 中有设置，
                          //则检查PCBTestLog Remark栏位的格式
                          var lstItcNDcheckMark = currentMBRepository.GetITCNDDefectChecks_NotCut(strMBCode);//.Substring(0, 2));
                          var lsttestlogforMark = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, 1, this.Station, maxcdt);
                          if (lstItcNDcheckMark.Count > 0)
                          {
                              if ((lsttestlogforMark == null) || (lsttestlogforMark.Count == 0))
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
                  //}                    
              }
              //5.	根据PCATest_Check表中设置信息进行测试Log最新(测试Log上传时间> PCBLog中当前MB的最大过站时间)上传当前检测站的Remark栏位信息卡站，详细卡站如下：
              //1)	若PCATest_Check中存在Code= MBCode的数据，则执行下面程序;若不存在，则执行Step 2）。
              var lstPcatestCheckinfo = currentMBRepository.GetPcaTestCheckInfoListByCode_NotCut(strMBCode);
              DateTime maxcdt5 = currentMBRepository.GetMaxCdtFromPCBLog(currentMB.Sn);
              if (maxcdt5 == System.DateTime.MinValue)
                  maxcdt5 = new DateTime(1900, 1, 1);//System.DateTime.Now.AddDays(-7); 
              var strgetremark = "";
              var lstPCBtestlogbyPCB5 = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, this.Station, maxcdt5);

              if ((lstPCBtestlogbyPCB5 == null) || (lstPCBtestlogbyPCB5.Count == 0))
                  strgetremark = "";
              else
                  strgetremark = lstPCBtestlogbyPCB5[0].Remark;

              if ((lstPcatestCheckinfo != null) && (lstPcatestCheckinfo.Count != 0))
              {
                  
                  //i.	若PCATest_Check.MAC = ‘Y’若Remark栏位获取的MAC值，与PCB中绑定的MAC(PCB.MAC)信息不一致，则报错：“MAC比对错误”
                  if (lstPcatestCheckinfo[0].mac.Trim() == "Y")
                  {
                      string strMAC = currentMB.MAC == null ? "" : currentMB.MAC.Trim();
                      var ilocMAC = strgetremark.IndexOf("MAC:");
                      if (ilocMAC != -1)
                      {
                          ilocMAC = ilocMAC + 4;
                          string strremarkMAC = strgetremark.Substring(ilocMAC);
                          var ilenMAC = strremarkMAC.IndexOf("~");
                          var strMACremark = "";
                          if (ilenMAC == -1)
                              strMACremark = strremarkMAC;
                          else
                              strMACremark = strremarkMAC.Substring(0, ilenMAC);
                          if (strMAC != strMACremark)
                          {
                              //“MAC比对错误”
                              //Debug Mantis1225
                              //------------------------------------------------
                              var iFindMacPos = strMACremark.IndexOf(strMAC);
                              if (iFindMacPos ==-1)
                              {
                                  List<string> errparaNewB = new List<string>();
                                  //errparaF.Add(MB_SNo);
                                  throw new FisException("CHK248", errparaNewB);
                              }
                          }
                      }
                      else if (strMAC != "")
                      {
                          //“MAC比对错误”
                          List<string> errparaNewB = new List<string>();
                          //errparaF.Add(MB_SNo);
                          throw new FisException("CHK248", errparaNewB);
                      }
                  }
                  //ii.	若PCATest_Check.MBCT = ‘Y’若Remark栏位获取的MBCT值，与PCB绑定的MBCT（PCBInfo.InfoValue, Condition: InfoType=’MBCT’）信息不一致，则报错：“MBCT比对错误”
                  if (lstPcatestCheckinfo[0].mbct.Trim() == "Y")
                  {
                      var strMBCT = (string)currentMB.GetExtendedProperty("MBCT");
                      if (strMBCT == null)
                          strMBCT = "";
                      var ilocMBCT = strgetremark.IndexOf("MBCT:");
                      if (ilocMBCT != -1)
                      {
                          ilocMBCT = ilocMBCT + 5;
                          string strremarkMBCT = strgetremark.Substring(ilocMBCT);
                          var ilenMBCT = strremarkMBCT.IndexOf("~");
                          var strMBCTremark = "";
                          if (ilenMBCT == -1)
                              strMBCTremark = strremarkMBCT;
                          else
                              strMBCTremark = strremarkMBCT.Substring(0, ilenMBCT);
                          //if (strMBCT.IndexOf(strMBCTremark) == -1)
                          if (strMBCT != strMBCTremark)
                          {
                              //MBCT比对错误
                              //Debug Mantis1225
                              //------------------------------------------------
                              var iFindMBCTPos = strMBCTremark.IndexOf(strMBCT);
                              if (iFindMBCTPos ==-1)
                              {
                                  List<string> errparaNewC = new List<string>();
                                  //errparaF.Add(MB_SNo);
                                  throw new FisException("CHK249", errparaNewC);
                              }
                          }
                      }
                      else if (strMBCT != "")
                      {
                          //MBCT比对错误
                          List<string> errparaNewC = new List<string>();
                          //errparaF.Add(MB_SNo);
                          throw new FisException("CHK249", errparaNewC);
                      }
                  }
                  //iii.	@HDDV=PCATest_Check.HDDV，@HDDV不等于空(‘’)且不等于’N’，若Remark栏位获取的HDDV值与@HDDV的信息不一致，则报错：“V比对错误”
                  string[] remarklst = strgetremark.Split('~');
                  string strBIOSinRemark = "BIOS:";
                  string strHDDVinRemark = "V:";
                  foreach (string splitItem in remarklst)
                  {
                      if ((splitItem != null) && (splitItem != ""))
                      {
                          if (splitItem.Substring(0, 2) == "V:")
                          {
                              strHDDVinRemark = splitItem.ToUpper();
                          }
                          else if (splitItem.Substring(0, 5) == "BIOS:")
                          {
                              strBIOSinRemark = splitItem.ToUpper();
                          }
                      }
                  }

                  if ((lstPcatestCheckinfo[0].hddv != "") && (lstPcatestCheckinfo[0].hddv != "N"))
                  {
                      string strHDDVremark = "V:" + lstPcatestCheckinfo[0].hddv.Trim().ToUpper();
                      //var ilocHDDV = strgetremark.IndexOf(strHDDVremark);
                      //if (ilocHDDV == -1)
                      if (strHDDVremark != strHDDVinRemark)
                      {
                          //V比对错误
                          //Debug Mantis1225
                          //------------------------------------------------
                          var iFindHDDVPos = strHDDVinRemark.IndexOf(strHDDVremark);
                          if (iFindHDDVPos == -1)
                          {
                              List<string> errparaNewD = new List<string>();
                              //errparaF.Add(MB_SNo);
                              throw new FisException("CHK250", errparaNewD);
                          }
                      }
                  }
                  //iv.	@BIOS=PCATest_Check.BIOS，@ BIOS不等于空(‘’)且不等于’N’，若Remark栏位获取的BIOS值与@BIOS的信息不一致，则报错：“BIOS比对错误”
                  if ((lstPcatestCheckinfo[0].bios != "") && (lstPcatestCheckinfo[0].bios != "N"))
                  {
                      string strBIOSremark = "BIOS:" + lstPcatestCheckinfo[0].bios.Trim().ToUpper();
                      if (strBIOSremark != strBIOSinRemark)
                      {
                          //BIOS比对错误
                          //Debug Mantis1225
                            //------------------------------------------------
                           var iFindBIOSPos = strBIOSinRemark.IndexOf(strBIOSremark);
                           if (iFindBIOSPos == -1)
                           {
                               List<string> errparaNewE = new List<string>();
                               //errparaF.Add(MB_SNo);
                               throw new FisException("CHK251", errparaNewE);
                           }
                      }
                  }

              }
              else  //2)	PCATest_Check中存在Code=@family的数据
              {
                    PcaTestCheckInfo testCheckinfo = new PcaTestCheckInfo();
                    testCheckinfo.code = currentMB.Family;
                    var lstPcatestCheckinfobyFAM = currentMBRepository.GetPcaTestCheckInfoListByCode(testCheckinfo);
                    if ((lstPcatestCheckinfobyFAM != null) && (lstPcatestCheckinfobyFAM.Count != 0))
                    {
                        //i.	若PCATest_Check.MAC = ‘Y’若Remark栏位获取的MAC值与PCB绑定的MAC信息不一致，则报错：“MAC比对错误”
                        if (lstPcatestCheckinfobyFAM[0].mac.Trim() == "Y")
                        {
                            string strMAC = currentMB.MAC == null ? "" : currentMB.MAC.Trim();
                            var ilocMAC = strgetremark.IndexOf("MAC:");
                            if (ilocMAC != -1)
                            {
                                ilocMAC = ilocMAC + 4;
                                string strremarkMAC = strgetremark.Substring(ilocMAC);
                                var ilenMAC = strremarkMAC.IndexOf("~");
                                var strMACremark = "";
                                if (ilenMAC == -1)
                                    strMACremark = strremarkMAC;
                                else
                                    strMACremark = strremarkMAC.Substring(0, ilenMAC);
                                if (strMAC != strMACremark)
                                {
                                    //“MAC比对错误”
                                    var iFindMacPos = strMACremark.IndexOf(strMAC);
                                    if (iFindMacPos == -1)
                                    {
                                        List<string> errparaNewF = new List<string>();
                                        //errparaF.Add(MB_SNo);
                                        throw new FisException("CHK248", errparaNewF);
                                    }
                                }
                            }
                            else if (strMAC != "")
                            {
                                //“MAC比对错误”
                                List<string> errparaNewB = new List<string>();
                                //errparaF.Add(MB_SNo);
                                throw new FisException("CHK248", errparaNewB);
                            }
                        }
                        //ii.	若PCATest_Check.MBCT = ‘Y’若Remark栏位获取的MBCT值与PCB绑定的MBCT信息不一致，则报错：“MBCT比对错误”
                        if (lstPcatestCheckinfobyFAM[0].mbct.Trim() == "Y")
                        {
                            var strMBCT = (string)currentMB.GetExtendedProperty("MBCT");
                            if (strMBCT == null)
                                strMBCT = "";
                            strMBCT = strMBCT.Trim();
                            var ilocMBCT = strgetremark.IndexOf("MBCT:");
                            if (ilocMBCT != -1)
                            {
                                ilocMBCT = ilocMBCT + 5;
                                string strremarkMBCT = strgetremark.Substring(ilocMBCT);
                                var ilenMBCT = strremarkMBCT.IndexOf("~");
                                var strMBCTremark = "";
                                if (ilenMBCT == -1)
                                    strMBCTremark = strremarkMBCT;
                                else
                                    strMBCTremark = strremarkMBCT.Substring(0, ilenMBCT);
                                if (strMBCT != strMBCTremark)
                                {
                                    //MBCT比对错误
                                    //Debug Mantis1225
                                    //---------------------------------------------
                                    var iFindMBCTPos = strMBCTremark.IndexOf(strMBCT);
                                    if (iFindMBCTPos == -1)
                                    {
                                        List<string> errparaNewG = new List<string>();
                                        //errparaF.Add(MB_SNo);
                                        throw new FisException("CHK249", errparaNewG);
                                    }
                                }
                            }
                            else if (strMBCT != "")
                            {
                                //MBCT比对错误
                                List<string> errparaNewG = new List<string>();
                                //errparaF.Add(MB_SNo);
                                throw new FisException("CHK249", errparaNewG);
                            }
                        }
                    }
              }
              //6.	测试log卡站已设置(Table MB_Test存在记录，Conditon: MB_Test.Code = MBCode and MB_Test.Type=0)，则进行以下检查：
              bool bCheckFollowStep = true;
              if ((lstmbtestrec != null) && (lstmbtestrec.Count != 0))
              {
                  //1)	若板子不存在修护记录（Condition：PCBRepair.PCBNo=@MBSno and Status = 1 and Station<>’19’）和PCBTestLog不存在Station=08的Fail记录（PCBTestLog.PCBNo=@MBSno and Station=’08’ and Status=’0’），CheckCode等于C或者数字，@ImageRemark=’R’，则不进行下面的操作
                  var bRepairRecord = false;
                  var bFailtestlog = false;
                  foreach (var repair in currentMB.Repairs)
                  {
                      if ((repair.Status == Repair.RepairStatus.Finished) && (repair.Station.Trim() != "19"))
                      {
                          bRepairRecord = true;
                          break;
                      }
                  }
                  DateTime tmpCdt = new DateTime(1900, 1, 1);
                  var Failtestloglst = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, 0, "08", tmpCdt);
                  if ((Failtestloglst != null) && (Failtestloglst.Count != 0))
                  {
                      bFailtestlog = true;
                  }
                  if ((bRepairRecord == false) && (bFailtestlog == false))  //Modify 2012/05/29 
                  {
                      if ((strCheckCode == "C") || (Char.IsNumber(strCheckCode, 0) == true))
                      {
                          string ImageRemark = "";
                          IList<MBTestDef> MbTestRemarkLst = currentMBRepository.GetMBTestList_NotCut(strMBCode, false);
                          if ((MbTestRemarkLst != null) && (MbTestRemarkLst.Count != 0))
                          {
                              ImageRemark = MbTestRemarkLst[0].remark.Trim();
                          }
                          if (ImageRemark == "R")
                          {
                              bCheckFollowStep = false;
                          }
                      }
                  }
              }
              //2)	系统设置匹配测试Log。若本次（MB测试的时间大于最新过站的时间）测试Log的Status=1，则通过；
              //  若Status=0报告错误：“Function Test Is Failure,Can Not Input As OK!”；若测试记录不存在，则报告错误：“Please Goto SA Funtion Test!”
              if (bCheckFollowStep)
              {
                  string[] param = { "MB", "M/B", "RCTO" };
                  var lsttestlog = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, param, 0, this.Station, maxcdt5);
                  var lsttestlognoexist = currentMBRepository.GetPCBTestLogListFromPCBTestLog(currentMB.Sn, param, 1, this.Station, maxcdt5);
                  if (lstmbtestrec.Count > 0)
                  {
                      if (lsttestlog.Count > 0)
                      {
                          //'Function Test Is Failure,Can Not Input As OK!'
                          List<string> errparaD = new List<string>();
                          //errparaD.Add(MB_SNo);
                          throw new FisException("CHK230", errparaD);
                      }
                      else if ((lsttestlognoexist == null) || (lsttestlognoexist.Count == 0))
                      {
                          //若测试记录不存在，则报告错误：“Please Goto SA Funtion Test!”
                          //'Please Goto SA Funtion Test!'
                          List<string> errparaE = new List<string>();
                          //errparaE.Add(MB_SNo);
                          throw new FisException("CHK229", errparaE);
                      }
                  }
              }
            //-------------------------------------- END--------------------------------------------------------------
           return base.DoExecute(executionContext);
        }
	}
}
