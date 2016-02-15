// INVENTEC corporation (c)2012 all rights reserved. 
// Description: UpdateLotNoForPCATEST
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-05-18   Kaisheng                     create
// Known issues:

using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Utility.Generates.impl;
using IMES.Infrastructure.Utility.Generates.intf;
using System.ComponentModel;

namespace IMES.Activity
{
    /// <summary>
    /// Create LOT NO
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于
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
    ///         SessionKeys
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         LotNo
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         更新NumControl
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         
    /// </para> 
    /// </remarks>
    public partial class UpdateLotNoForPCATEST : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public UpdateLotNoForPCATEST()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            MB currenMB = CurrentSession.GetValue(Session.SessionKeys.MB) as MB;
            IMBRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
            INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
            //Modify UC Update 	前提条件:Station=15 ITC-1414-0024
            if (this.Station != "15")
            {
                return base.DoExecute(executionContext);
            }
    
            //-------------------------------------------------------------
            //2.28.	MBCode:若第6码为’M’，则取MBSN前3码为MBCode，若第5码为’M’，则取前2码
            //CheckCode:若MBSN的第5码为’M’，则取MBSN的第6码，否则取第7码
            string strMBCode ="";
            string strCheckCode = "";
            if (currenMB.Sn.Substring(5, 1) == "M" || currenMB.Sn.Substring(5, 1) == "B")
            {
                strMBCode = currenMB.Sn.Substring(0, 3);
                strCheckCode = currenMB.Sn.Substring(6, 1);
            }
            else if (currenMB.Sn.Substring(4, 1) == "M" || currenMB.Sn.Substring(4, 1) == "B")
            {
                strMBCode = currenMB.Sn.Substring(0, 2);
                strCheckCode = currenMB.Sn.Substring(5, 1);
            }
            else
            {
                strCheckCode = currenMB.Sn.Substring(6, 1);
            }
            //-------------------------------------------------------------
            //2.29.	MBSN子板/RCTO的判定:CheckCode为数字，则为子板，为’R’，则为RCTO
            if (strCheckCode == "R")
            {
                //为’R’，则为RCTO
            }
            else if (Char.IsNumber(strCheckCode, 0) == true)
            {
                //CheckCode为数字，则为子板
            }

            //Session.AddValue("IsDocking", true);
            // Docking  @Type=’PC’
            var isDocking = false;
            try
            {
                isDocking = (bool)CurrentSession.GetValue("IsDocking");
            }
            catch (Exception ex)
            {
                var strerr = ex.Message;
                isDocking = false;
            }
            //-------------------------------------------------------------
            //Type:
            //1、	若@MBSN的CheckCode为‘R’，则@Type=’RCTO’
            //2、	若IsFru Checked，则@Type=’Fru’
            //3、	其他，@Type=’PC’
            string strType = "";
            if (isDocking == false)
            {
                if (strCheckCode == "R")
                {
                    strType = "RCTO";
                }
                else if ((bool)CurrentSession.GetValue("bFruChecked")==true)
                {
                    strType = "Fru";
                }
                else
                {
                    strType = "PC";
                }
            }
            else
            {
                strType = "PC";
            }
            //-------------------------------------------------------------
            //[LotList]中是否存在@MBCode和@Type记录 +和@Line）
            //UC UPDATE 2012/06/20 LotList]中不存在（@MBCode、@Type、和@Line 和 Lot.Status=0）记录
            IMES.DataModel.LotInfo Conlot = new IMES.DataModel.LotInfo();
            Conlot.mbcode = strMBCode;
            Conlot.type = strType;
            Conlot.line = this.Line;
            Conlot.status = "0";//Add by kaisheng 2012/06/20
            IList<LotInfo> retlot = itemRepository.GetlotInfoList(Conlot);
            
                        
            if ((retlot == null) || (retlot.Count == 0))
            {
                //不存在@MBCode和@Type记录, 则执行‘15.1 LotNo生成规则’，产生该MBCode和Type的LotNo，并Insert Lot/PCBLot
                //LotNo规则：YYYYMMDDXXXX，XXXX代表流水码,10进制,自增，幅度为1
                // --------------------------------------------------------
                //1. LotNo生成
                string sublotNo = "";
                //string Watercode = "";
                sublotNo = DateTime.Today.Year.ToString("0000") + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00");
                try
                {

                    SqlTransactionManager.Begin();
                    lock (_syncRoot_GetSeq)
                    {
                        NumControl currentMaxNum = numCtrlRepository.GetMaxValue("LotNo", "LotNo");
                        if (currentMaxNum == null)
                        {
                            currentMaxNum = new NumControl();
                            currentMaxNum.NOType = "LotNo";
                            currentMaxNum.NOName = "LotNo";
                            currentMaxNum.Value = sublotNo + beginNO;
                            currentMaxNum.Customer = "";
                            numCtrlRepository.InsertNumControl(currentMaxNum);
                            SqlTransactionManager.Commit();
                            CurrentSession.AddValue(Session.SessionKeys.LotNo, sublotNo + beginNO);
                        }
                        else if (currentMaxNum.Value.Substring(0, 8) != sublotNo)
                        {
                            currentMaxNum.Value = sublotNo + beginNO;
                            IUnitOfWork uof = new UnitOfWork();
                            numCtrlRepository.Update(currentMaxNum, uof);
                            uof.Commit();
                            SqlTransactionManager.Commit();
                            CurrentSession.AddValue(Session.SessionKeys.LotNo, sublotNo + beginNO);
                        }
                        else
                        {
                            if (currentMaxNum.Value.EndsWith("9999"))
                            {
                                //SqlTransactionManager.Commit();
                                throw new FisException("CHK867", new string[] { }); //CHK, The Quantity of LotNo is full today, please contact IE!
                            }
                            else
                            {
                                ISequenceConverter seqCvt = new SequenceConverterNormal("0123456789", 4, "9999", beginNO, '0');
                                string sequenceNumber = currentMaxNum.Value.Substring(8, 4);
                                sequenceNumber = seqCvt.NumberRule.IncreaseToNumber(sequenceNumber, 1);
                                currentMaxNum.Value = sublotNo + sequenceNumber;//currentMaxNum.Value.Substring(0, 8) + sequenceNumber;
                                IUnitOfWork uof = new UnitOfWork();
                                numCtrlRepository.Update(currentMaxNum, uof);
                                uof.Commit();
                                SqlTransactionManager.Commit();
                                CurrentSession.AddValue(Session.SessionKeys.LotNo, sublotNo + sequenceNumber);
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
                
                //Insert Lot/PCBLot 
                //Insert Lot：LotNo=新产生No, Line=当前Line，Type=@Type, Qty=1, Status=0
                //Insert PCBLot：LotNo=@LotNo, PCBNo=@MBSN, Status=1
                LotInfo conLotInfo = new LotInfo();
                conLotInfo.lotNo = (string)CurrentSession.GetValue(Session.SessionKeys.LotNo);
                conLotInfo.line = this.Line;
                conLotInfo.mbcode = strMBCode;
                conLotInfo.type = strType;
                conLotInfo.qty = 1;
                conLotInfo.status = "0";
                conLotInfo.editor = this.Editor;
                itemRepository.InsertLotInfoDefered(CurrentSession.UnitOfWork, conLotInfo);
                PcblotInfo conPcblotInfo = new PcblotInfo();
                conPcblotInfo.lotNo = (string)CurrentSession.GetValue(Session.SessionKeys.LotNo);
                conPcblotInfo.pcbno = currenMB.Sn;
                conPcblotInfo.status = "1";
                conPcblotInfo.editor = this.Editor;
                itemRepository.InsertPCBLotInfoDefered(CurrentSession.UnitOfWork, conPcblotInfo);
                CurrentSession.AddValue("LotNoQty", 1);

            }
            else
            {
                LotInfo conLotInfo = new LotInfo();
                //Get Qty

                //conLotInfo.lotNo = retlot[0].lotNo;
                ////conLotInfo.type = strType;
                //IList<LotInfo> getLotInfo = itemRepository.GetlotInfoList(conLotInfo);
                //CurrentSession.AddValue("LotNoQty", getLotInfo[0].qty + 1);
                
                CurrentSession.AddValue("LotNoQty",retlot[0].qty + 1);
                
                    //存在@MBCode和@Type记录，则Update Lot，Insert PCBLot
                //Update Lot：Qty=Qty+1, Editor, Udt where LotNo=@LotNo
                //Insert PCBLot：LotNo=@LotNo, PCBNo=@MBSN, Status=1
                LotInfo setLotInfo = new LotInfo();
                conLotInfo = new LotInfo();
                conLotInfo.lotNo = retlot[0].lotNo;
                //conLotInfo.type = strType;
                //setLotInfo.qty = retlot[0].qty + 1;//1;//setValue.Qty赋1，其他按需要赋值即可
                setLotInfo.qty = 1;
                setLotInfo.editor = this.Editor;
                setLotInfo.udt = DateTime.Now;
                //itemRepository.UpdateLotInfoDefered(CurrentSession.UnitOfWork, setLotInfo, conLotInfo);
                itemRepository.UpdateLotInfoForIncQtyDefered(CurrentSession.UnitOfWork, setLotInfo, conLotInfo);
                PcblotInfo conPcblotInfo = new PcblotInfo();
                conPcblotInfo.lotNo = retlot[0].lotNo;
                conPcblotInfo.pcbno = currenMB.Sn;
                conPcblotInfo.status = "1";
                conPcblotInfo.editor = this.Editor;
                itemRepository.InsertPCBLotInfoDefered(CurrentSession.UnitOfWork, conPcblotInfo);

                
            }
            //GetPassQtyfromlotSetting -Activity
            // Defered方法， +1 可能没立即保存--如何确保Qty值 ？？ --
            int passQtyforLine = (int)CurrentSession.GetValue("PassQtyinlotSetting");



            //2、	Check @MBCode和@Type对应的Qty，若Qty>=PassQty，则Update Lot
            //Update Lot：Status=1 where LotNo=@LotNo
            int currentQty = (int)CurrentSession.GetValue("LotNoQty");
            if (currentQty >= passQtyforLine)
            {

                LotInfo setLotInfo = new LotInfo();
                LotInfo conLotInfo = new LotInfo();
                //DEBUG Mantis 1005
                if ((retlot == null) || (retlot.Count == 0))
                    conLotInfo.lotNo = (string)CurrentSession.GetValue(Session.SessionKeys.LotNo);
                else
                    conLotInfo.lotNo = retlot[0].lotNo;
                setLotInfo.status = "1";
                setLotInfo.udt = DateTime.Now;
                itemRepository.UpdateLotInfoDefered(CurrentSession.UnitOfWork, setLotInfo, conLotInfo);
            }
            return base.DoExecute(executionContext);
        }

        private static object _syncRoot_GetSeq = new object();

        private readonly string beginNO = "0000";
    }
}
