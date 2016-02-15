/*
 * INVENTEC corporation: 2012 all rights reserved. 
 * Description: PCA OQC Input
 * UI:CI-MES12-SPEC-SA-UI PCA OQC Input.docx 
 * UC:CI-MES12-SPEC-SA-UC PCA OQC Input.docx          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-05-30   Kaisheng               Create
 * Known issues:
 * TODO：
 * UC 具体业务：  1.	刷入Lot相关数据，做Lot的Lock/Undo/Pass操作
 *               
 * UC Revision:  
 */
using System;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using MBInfo = IMES.DataModel.MBInfo;
using System.Collections;
using IMES.FisObject.Common.PrintLog;
using IMES.FisObject.Common.Line;
using IMES.Infrastructure.UnitOfWork;
using System.Data;
using System.Linq;

namespace IMES.Station.Implementation
{
    using System.Linq;


    /// <summary>
    /// PCA OQC Input
    /// </summary> 
    /// 


    public class PCAOQCInput : MarshalByRefObject, IPCAOQCInput
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        #region IPCAOQCInput Members

        /// <summary>
        /// 刷mbsno，调用该方法启动工作流，根据输入mbsno获取Lot
        /// 返回MNSB LIST 
        /// </summary>
        /// <param name="mbsno">mbsno</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="curMBInfo">curMBInfo</param>
        /// <returns>model</returns>
        public ArrayList inputMBSnoORLotNo(string InputStr, string InputType, string editor, string station, string customer)
        {
            logger.Debug("(PCAOQCInputImpl)Input MBSN or LotNo start:" + InputStr + "editor:" + editor + "station:" + station + "customer:" + customer);

            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList retLst = new ArrayList();
            IMBRepository iMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            string LotNO = "";
            ArrayList lstRet = new ArrayList();
           try
            {
                //若刷入数据为MBSN，获取MBSN的所在LotNo（PCBLot.LotNo where PCBNo=@MBSN and Status=1），
                // 则不存在记录，则报错：“该MBSN未组Lot”
                if (InputType == "MBSN")
                {
                    PcblotInfo conPcblot = new PcblotInfo();
                    conPcblot.pcbno = InputStr;
                    conPcblot.status = "1";
                    IList<PcblotInfo> pcblotLstA = new List<PcblotInfo>();
                    pcblotLstA = iMBRepository.GetPcblotInfoList(conPcblot);
                    if ((pcblotLstA == null) || (pcblotLstA.Count == 0))
                    {
                        erpara.Add(InputStr);
                        ex = new FisException("CHK312", erpara); //该MBSN未组Lot
                        throw ex;
                    }
                    if (pcblotLstA.Count==1)
                    {
                        LotNO = pcblotLstA[0].lotNo;
                    }
                    else
                    {
                        var pcblotLst =
                            from item in pcblotLstA
                            orderby item.cdt descending
                            select item;
                        foreach (PcblotInfo tmpNode in pcblotLst)
                        {
                            LotNO = tmpNode.lotNo;
                            break;
                        }
                    }
                  
                }
                else
                {
                    LotNO = InputStr;
                }
                
                string sessionKey = LotNO;

                //2、	获取LotNo的详细信息(Lot.* where Lot.LotNo=@LotNo)；若Lot信息不存在，则报错：“该Lot不存在”
                LotInfo conLotInfo = new LotInfo();
                conLotInfo.lotNo = LotNO;
                IList<LotInfo> getLotInfo = iMBRepository.GetlotInfoList(conLotInfo);
                if ((getLotInfo == null) || (getLotInfo.Count == 0))
                {
                    erpara.Add(LotNO);
                    ex = new FisException("CHK313", erpara); //该Lot不存在
                    throw ex;
                }
                //若Lot.Status=’0’，则报错：“Lot没有组合完成”；
                if (getLotInfo[0].status == "0")
                {
                    erpara.Add(LotNO);
                    ex = new FisException("CHK314", erpara); //Lot没有组合完成
                    throw ex;
                }
                //若Lot.Status=’4’，则报错：“该Lot已解散”；
                if (getLotInfo[0].status == "4")
                {
                    erpara.Add(LotNO);
                    ex = new FisException("CHK315", erpara); //该Lot已解散
                    throw ex;
                }
                //若Lot.Status=’9’，则报错：“该Lot已经通过OQC”
                if (getLotInfo[0].status == "9")
                {
                    erpara.Add(LotNO);
                    ex = new FisException("CHK316", erpara); //该Lot已经通过OQC
                    throw ex;
                }
                //4、	Update Lot Status
                //若Lot.Status=1，则Update Lot.Status=2；
                //Update PCBStatus(where PCBNo in PCBLot.PCBNo and Status=1 and LotNo=@LotNo)；
                UnitOfWork uow = new UnitOfWork(); 
                if (getLotInfo[0].status.Trim() == "1")
                {
                    LotInfo setLotInfo = new LotInfo();
                    conLotInfo = new LotInfo();
                    conLotInfo.lotNo = getLotInfo[0].lotNo;
                    //conLotInfo.type = strType;
                    //setLotInfo.qty = retlot[0].qty + 1;//1;//setValue.Qty赋1，其他按需要赋值即可
                    setLotInfo.status = "2";
                    setLotInfo.editor = editor;
                    setLotInfo.udt = DateTime.Now;
                    //itemRepository.UpdateLotInfoDefered(CurrentSession.UnitOfWork, setLotInfo, conLotInfo);
                    iMBRepository.UpdateLotInfoDefered(uow, setLotInfo, conLotInfo);
                //}
                    //DEBUG Mantis 1009(itc-1414-0224)
                    //update PCBStatus,PCBLog  ->条件也要： Lot.Status=1
                    
                   //Update PCBStatus：Station=’31A’Status=’1’
                    //Insert PCBLog
                    //Insert PCBLog：Station=’31A’Status=’1’
                    PcblotInfo conPcblot4 = new PcblotInfo();
                    //conPcblot = new PcblotInfo();
                    conPcblot4.lotNo= LotNO;
                    conPcblot4.status = "1";
                    IList<PcblotInfo>pcblotLst4 = new List<PcblotInfo>();
                    pcblotLst4 = iMBRepository.GetPcblotInfoList(conPcblot4);
                    if ((pcblotLst4 == null) || (pcblotLst4.Count == 0))
                    {

                    }
                    else
                    {
                        for (int i = 0; i < pcblotLst4.Count; i++)
                        {
                            IMB mb = iMBRepository.Find(pcblotLst4[i].pcbno);
                            if (mb != null)
                            {
                                mb.MBStatus.Station = "31A";
                                mb.MBStatus.Status = MBStatusEnum.Pass;
                                mb.MBStatus.Editor = editor;
                                mb.MBStatus.Udt = DateTime.Now;
                                //记录MB Log
                                MBLog mb_log = new MBLog(0, mb.Sn, mb.Model, "31A", (int) MBStatusEnum.Pass,
                                                         getLotInfo[0].line, editor, new DateTime());
                                mb.AddLog(mb_log);
                                iMBRepository.Update(mb, uow);
                            }
                        }
                    }
                }
                uow.Commit();

                //5、重新获取Lot信息，并显示LotNo Line[Line.Descr] Type PCS = Qty Status[2：OQC In；3：Locked]
                conLotInfo = new LotInfo();
                conLotInfo.lotNo = LotNO;
                IList<LotInfo> ReturnLotInfo = iMBRepository.GetlotInfoList(conLotInfo);
                ILineRepository lineRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();
                Line lineInfo = lineRepository.Find(ReturnLotInfo[0].line);
                if (lineInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK317", erpara); //该PCB %1 没有PdLine，请确认！
                    throw ex;
                }
                string lotLineinfo = ReturnLotInfo[0].line.Trim() + "[" +lineInfo.Descr.Trim() + "]";
                string strStatus="";
                if (ReturnLotInfo[0].status == "2")
                {
                    strStatus = "OQC In";
                }
                else if (ReturnLotInfo[0].status == "3")
                {
                    strStatus = "Locked";
                }
                //Old--获取PCBLot信息，并显示PCBLot.PCBNo where LotNo=@LotNo and Status=1
                //PcblotInfo RtnconPcblot = new PcblotInfo();
                //RtnconPcblot.lotNo = ReturnLotInfo[0].lotNo;
                //RtnconPcblot.status = "1";
                //IList<PcblotInfo> rtnpcblotLst = new List<PcblotInfo>();
                //rtnpcblotLst = iMBRepository.GetPcblotInfoList(RtnconPcblot);
                //IList<string> mbsnList = new List<string>();
                //foreach (PcblotInfo pcblotnode in rtnpcblotLst)
                //{
                //    mbsnList.Add(pcblotnode.pcbno.Trim());
                //}
               //UC Update 
               //	获取LotSetting.CheckQty(先用PdLine检索，若不存在，再用’ALL’检索，若不存在，则报错：“请IE维护LotSetting”)，
               //若[Checked Qty]>=LotSetting.CheckQty，---则执行[4 Save Data] PASS分支:界面初始时不自动处理
                var checkQtyforLine = 0;
                LotSettingInfo conlotSetting = new LotSettingInfo();
                conlotSetting.line = ReturnLotInfo[0].line;
                //add type 
                conlotSetting.type = ReturnLotInfo[0].type;
                IList<LotSettingInfo> LotSettinglst = iMBRepository.GetLotSettingInfoList(conlotSetting);
                if ((LotSettinglst == null) || (LotSettinglst.Count == 0))
                {
                    conlotSetting = new LotSettingInfo();
                    conlotSetting.line = "ALL";
                    conlotSetting.type = ReturnLotInfo[0].type;
                    LotSettinglst = iMBRepository.GetLotSettingInfoList(conlotSetting);
                    if ((LotSettinglst == null) || (LotSettinglst.Count == 0))
                    {
                        //报错：“请与IE联系，维护 Lot 相关设置”
                        List<string> errpara = new List<string>();
                        //errpara.Add(currenMB.Sn);
                        FisException ex1 = new FisException("CHK278", errpara);
                        throw ex1;
                    }
                    else
                    {
                        checkQtyforLine = LotSettinglst[0].checkQty;
                    }
                }
                else
                {
                    checkQtyforLine = LotSettinglst[0].checkQty;
                }
               
               //======================================================================================
                //UC Update 2012/07/03 
                //6、	获取PCBLot信息，并显示在[MBSN List]和[Checked Qty]( [Checked Qty]: Sum(Checked))
                //参考方法：
                //select a.PCBNo, ISNULL(b.Status,'0') as Checked from PCBLot a
                //      left Join PCBLotCheck b
                //      on a..PCBNo where LotNo = b.=@LotNo
                //      and a.PCBNo = b.PCBNo
                //   where a.LotNo=@LotNo and a.Status=1
                //   order by a.PCBNo
                IList<string> mbsnList = new List<string>();
                IList<string> checkedList = new List<string>();
                DataTable PcbLotCheckedTable = iMBRepository.GetPcbNoAndCheckStatusList(ReturnLotInfo[0].lotNo, "1");
                if (PcbLotCheckedTable == null)
                {
                    //throw new FisException("CHM001", new string[] { model1 });
                }
                else
                {
                    var pcbLotCheckedCount = PcbLotCheckedTable.Rows.Count;
                    for (int i = 0; i < pcbLotCheckedCount; i++)
                    {
                        mbsnList.Add(PcbLotCheckedTable.Rows[i][0] as string);
                        checkedList.Add(PcbLotCheckedTable.Rows[i][1] as string);
                    }
                }
                var pcblotcheck = new PcblotcheckInfo();
                pcblotcheck.lotNo = LotNO;
                pcblotcheck.status = "1";
                var iCheckqty = iMBRepository.GetCountOfPcblotCheck(pcblotcheck);
                var havePromptstr = "";
                if (ReturnLotInfo[0].status !="2")
                {
                    try
                    {
                        throw new FisException("CHK401", new List<string>());

                    }
                    catch (FisException ex1)
                    {
                        havePromptstr = ex1.mErrmsg;
                    }
                }
             
                lstRet.Add(ReturnLotInfo[0].lotNo);      //0
                lstRet.Add(lotLineinfo);                 //1 
                lstRet.Add(ReturnLotInfo[0].type);       //2
                lstRet.Add(ReturnLotInfo[0].qty);        //3 
                lstRet.Add(strStatus);                   //4
                lstRet.Add(mbsnList);                    //5
                lstRet.Add(ReturnLotInfo[0].line);       //6
                lstRet.Add(checkedList);                 //7
                lstRet.Add(checkQtyforLine);             //8
                lstRet.Add(iCheckqty);                   //9
                lstRet.Add(havePromptstr);              //10
                return lstRet;
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
                logger.Debug("(PCAOQCInputImpl)Input MBSN or LotNo end  Input:" + InputStr + "editor:" + editor + "station:" + station + "customer:" + customer);
            }
        }
        /// <summary>
        /// insertPcbLotCheck
        /// </summary>
        /// <param name="lotNo"></param>
        /// <param name="mbSno"></param>
        /// <param name="editor"></param>
        /// <param name="line"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ArrayList InsertPcbLotCheck(string lotNo, String mbSno, string editor, string line, string customer)
        {
            logger.Debug("(PCAOQCInputImpl)insertPcbLotCheck:" + mbSno + "editor:" + editor + "line:" + line + "customer:" + customer);
            try
            {
                //FisException ex;
                List<string> erpara = new List<string>();
                ArrayList retLst = new ArrayList();
                IMBRepository iMbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                //	Insert PCBLotCheck Set LotNo = @LotNo , PCBNo = @PCBNo, Status = ‘1
                var pcblotcheck = new PcblotcheckInfo();
                pcblotcheck.lotNo = lotNo;
                pcblotcheck.pcbno = mbSno;
                pcblotcheck.status = "1";
                pcblotcheck.editor = editor;
                pcblotcheck.cdt = DateTime.Now;
                iMbRepository.InsertPcblotcheckInfo(pcblotcheck);
                //Get MBSN List -	重新获取[MBSN List]
                IList<string> mbsnList = new List<string>();
                IList<string> checkedList = new List<string>();
                DataTable PcbLotCheckedTable = iMbRepository.GetPcbNoAndCheckStatusList(lotNo, "1");
                if (PcbLotCheckedTable == null)
                {
                    //throw new FisException("CHM001", new string[] { model1 });
                }
                else
                {
                    var pcbLotCheckedCount = PcbLotCheckedTable.Rows.Count;
                    for (int i = 0; i < pcbLotCheckedCount; i++)
                    {
                        mbsnList.Add(PcbLotCheckedTable.Rows[i][0] as string);
                        checkedList.Add(PcbLotCheckedTable.Rows[i][1] as string);
                    }
                }
                
                //select count(*) from Pcblotcheck where lotNo=@lotNo and status ='1'
                var conpcblotcheck = new PcblotcheckInfo();
                conpcblotcheck.lotNo = lotNo;
                conpcblotcheck.status = "1";
                var iCheckqty = iMbRepository.GetCountOfPcblotCheck(conpcblotcheck);

                retLst.Add(mbsnList);
                retLst.Add(checkedList);
                retLst.Add(iCheckqty);

                return retLst;
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
                logger.Debug("(PCAOQCInputImpl)insertPcbLotCheck end  Input:" + mbSno + "editor:" + editor + "line:" +
                             line + "customer:" + customer);
            }
        }

        /// <summary>
        /// 扫描9999，结束工作流
        /// 如果没有Defect，即defectCodeList为null或cout为0
        /// 将Session.AddValue(Session.SessionKeys.HasDefect,false)
        /// 否则Session.AddValue(Session.SessionKeys.HasDefect,true)
        /// </summary>
        /// <param name="mbsno">mbsno</param>
        /// <param name="defectCodeList">defectCodeList</param>
        public string save(string LotNo, String strCMD, string editor, string line, string customer)
        {
            logger.Debug("(PCAOQCInputImpl)save start,"
                + " LotNo: " + LotNo
                + " KeyCode:" + strCMD);
            FisException ex;
            List<string> erpara = new List<string>();
            IMBRepository iMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            string returnstring = "OK";
            try
            {
                if (strCMD == "LOCK") //Update Lot.Status=3
                {
                    LotInfo setLotInfo = new LotInfo();
                    LotInfo conLotInfo = new LotInfo();
                    conLotInfo.lotNo = LotNo;
                    setLotInfo.status = "3";
                    setLotInfo.editor = editor;
                    setLotInfo.udt = DateTime.Now;
                    iMBRepository.UpdateLotInfo(setLotInfo, conLotInfo);
                }
                else if (strCMD == "UNLOCK") //Update Lot.Status=2
                {
                    LotInfo setLotInfo = new LotInfo();
                    LotInfo conLotInfo = new LotInfo();
                    conLotInfo.lotNo = LotNo;
                    setLotInfo.status = "2";
                    setLotInfo.editor = editor;
                    setLotInfo.udt = DateTime.Now;
                    iMBRepository.UpdateLotInfo(setLotInfo, conLotInfo);
                }
                else if (strCMD == "UNDO")
                {
                    UnitOfWork uow = new UnitOfWork();
                    //	Update Lot.Status=4
                    LotInfo setLotInfo = new LotInfo();
                    LotInfo conLotInfo = new LotInfo();
                    conLotInfo.lotNo = LotNo;
                    setLotInfo.status = "4";
                    setLotInfo.editor = editor;
                    setLotInfo.udt = DateTime.Now;
                    iMBRepository.UpdateLotInfoDefered(uow,setLotInfo, conLotInfo);
                    //Update PCBStatus where PCBNo in PCBLot.PCBNo and PCBLot.Status=1 and LotNo=#LotNo Station=10 Status=1
                    PcblotInfo conupPcblot = new PcblotInfo();
                    conupPcblot.lotNo= LotNo;
                    conupPcblot.status = "1";
                    IList<PcblotInfo> upPcblotLst = new List<PcblotInfo>();
                    upPcblotLst = iMBRepository.GetPcblotInfoList(conupPcblot);
                    if ((upPcblotLst == null) || (upPcblotLst.Count == 0))
                    {

                    }
                    else
                    {
                        for (int i = 0; i < upPcblotLst.Count; i++)
                        {
                            IMB mb = iMBRepository.Find(upPcblotLst[i].pcbno);
                            if (mb != null)
                            {
                                mb.MBStatus.Station = "10";
                                mb.MBStatus.Status = MBStatusEnum.Pass;
                                mb.MBStatus.Editor = editor;
                                mb.MBStatus.Udt = DateTime.Now;
                                //记录MB Log 	Insert PCBLog  Station=’31A’ Status=’0’ PdLine=‘UNDO
                                MBLog mb_log = new MBLog(0, mb.Sn, mb.Model, "31A", (int)MBStatusEnum.Fail, "UNDO", editor, new DateTime());
                                mb.AddLog(mb_log);
                                iMBRepository.Update(mb, uow);
                            }
                        }
                    }
                    //	Update PCBLot.Status=0
                    PcblotInfo conPcblot = new PcblotInfo();
                    PcblotInfo setPcblot = new PcblotInfo();
                    conPcblot.lotNo = LotNo;
                    setPcblot.status = "0";
                    iMBRepository.UpdatePCBLotInfoDefered(uow, setPcblot, conPcblot);
                    uow.Commit();
                }
                else if (strCMD == "PASS")
                {
                    //	若当前Lot.Status=’3’，则报错：“该Lot已锁定，请解锁后再做放行”
                    LotInfo conLotInfo = new LotInfo();
                    conLotInfo.lotNo = LotNo;
                    IList<LotInfo> getLotInfo = iMBRepository.GetlotInfoList(conLotInfo);
                    if (getLotInfo[0].status == "3")
                    {
                        erpara.Add(LotNo);
                        ex = new FisException("CHK319", erpara); //该Lot已锁定，请解锁后再做放行！
                        throw ex;
                    }
                    //	获取LotSetting.FailQty(先用PdLine检索，若不存在，再用’ALL’检索，若不存在，则报错：“请IE维护LotSetting”)
                    LotSettingInfo conlotSetting = new LotSettingInfo();
                    int failQtyforLine = 0;
                    conlotSetting.line = getLotInfo[0].line;
                    conlotSetting.type = getLotInfo[0].type;
                    IList<LotSettingInfo> LotSettinglst = iMBRepository.GetLotSettingInfoList(conlotSetting);
                    if ((LotSettinglst == null) || (LotSettinglst.Count == 0))
                    {
                        conlotSetting = new LotSettingInfo();
                        conlotSetting.line = "ALL";
                        conlotSetting.type = getLotInfo[0].type;
                        LotSettinglst = iMBRepository.GetLotSettingInfoList(conlotSetting);
                        if ((LotSettinglst == null) || (LotSettinglst.Count == 0))
                        {
                            //报错：“请与IE联系，维护 Lot 相关设置”
                            List<string> errpara = new List<string>();
                            errpara.Add(LotNo);
                            ex = new FisException("CHK278", errpara);
                            throw ex;
                        }
                        else
                        {
                            failQtyforLine = LotSettinglst[0].failQty;
                        }
                    }
                    else
                    {
                        failQtyforLine = LotSettinglst[0].failQty;
                    }
                    //	获取当前Lot的PCBLot.Status=0的数量@CNT，
                    //若@CNT大于或者等于LotSetting.FailQty，则Update Lot.Status=3，并提示：“该Lot抽检失败的数量过多，已被锁定”；
                    PcblotInfo conpassPcblot = new PcblotInfo();
                    conpassPcblot.lotNo = LotNo;
                    conpassPcblot.status = "0";
                    IList<PcblotInfo> passPcblotLst = new List<PcblotInfo>();
                    int pcbfailCount = 0;
                    passPcblotLst = iMBRepository.GetPcblotInfoList(conpassPcblot);
                    if ((passPcblotLst == null) || (passPcblotLst.Count == 0))
                    {
                        pcbfailCount = 0;
                    }
                    else
                    {
                        pcbfailCount = passPcblotLst.Count;
                    }
                    //int pcbfailCount = passPcblotLst.Count;
                    if (pcbfailCount >= failQtyforLine)
                    {
                        LotInfo setfailLotInfo = new LotInfo();
                        LotInfo confailLotInfo = new LotInfo();
                        confailLotInfo.lotNo = LotNo;
                        setfailLotInfo.status = "3";
                        setfailLotInfo.editor = editor;
                        setfailLotInfo.udt = DateTime.Now;
                        iMBRepository.UpdateLotInfo(setfailLotInfo, confailLotInfo);
                        List<string> errpara = new List<string>();
                        errpara.Add(LotNo);
                        ex = new FisException("CHK318", errpara);//该Lot抽检失败的数量过多，已被锁定
                        throw ex;
                    }
                    else
                    {
                        //Update Lot.Status=9；
                        UnitOfWork uof = new UnitOfWork(); 
                        LotInfo setfailLotInfo = new LotInfo();
                        LotInfo confailLotInfo = new LotInfo();
                        confailLotInfo.lotNo = LotNo;
                        setfailLotInfo.status = "9";
                        setfailLotInfo.editor = editor;
                        setfailLotInfo.udt = DateTime.Now;
                        iMBRepository.UpdateLotInfoDefered(uof,setfailLotInfo, confailLotInfo);
                        // Update PCBStatus where PCBNo in PCBLot.PCBNo and PCBLot.Status=1 (Station=31 and Status=1)；
                        // Insert PCBLog (Station=31 and Status=1)
                        //UpdatePCBStatus(PCBStatusInfo setValue, PCBStatusInfo condition)
                        PcblotInfo conupPcblot = new PcblotInfo();
                        conupPcblot.lotNo = LotNo;
                        conupPcblot.status = "1";
                        IList<PcblotInfo> upPcblotLst = new List<PcblotInfo>();
                        upPcblotLst = iMBRepository.GetPcblotInfoList(conupPcblot);
                        if ((upPcblotLst == null) || (upPcblotLst.Count == 0))
                        {

                        }
                        else
                        {
                             for (int i = 0; i < upPcblotLst.Count; i++)
                            {
                                IMB mb = iMBRepository.Find(upPcblotLst[i].pcbno);
                                if (mb != null)
                                {
                                    //UC Update  
                                    //Update PCBStatus where PCBNo in PCBLot.PCBNo and PCBLot.Status=1 and  
                                    //Station=31A and Status=1【2012-6-20】 (Station=31 and Status=1)；
                                    if ((mb.MBStatus.Station == "31A") && (mb.MBStatus.Status == MBStatusEnum.Pass))
                                    {
                                        mb.MBStatus.Station = "31";
                                        mb.MBStatus.Status = MBStatusEnum.Pass;
                                        mb.MBStatus.Editor = editor;
                                        mb.MBStatus.Udt = DateTime.Now;
                                        //记录MB Log 	Insert PCBLog (Station=31 and Status=1)
                                        MBLog mb_log = new MBLog(0, mb.Sn, mb.Model, "31", (int)MBStatusEnum.Pass, line, editor, new DateTime());
                                        mb.AddLog(mb_log);
                                        iMBRepository.Update(mb, uof);
                                    }
                                }
                            }
                        }
                        uof.Commit();
                    }

                }
                return returnstring;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(PCAOQCInputImpl)save end,"
                   + " LotNo: " + LotNo
                   + " KeyCode:" + strCMD);
            }
        }

        public string saveMB(string Inputstring, string editor, string station, string customer)
        {
            logger.Debug("(PCAOQCInputImpl)saveMB start Input:" + Inputstring + "editor:" + editor + "station:" + station + "customer:" + customer);

            //FisException ex;
            List<string> erpara = new List<string>();
            ArrayList retLst = new ArrayList();
            IMBRepository iMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            ArrayList lstRet = new ArrayList();
            UnitOfWork uow = new UnitOfWork();
            string returnstring = "OK";
            try
            {
                IMB mb = iMBRepository.Find(Inputstring);
                if (mb != null)
                {
                    string preStation = mb.MBStatus.Station;
                    string status = (string)mb.MBStatus.Status.ToString();
                    string line = mb.MBStatus.Line;
                    if (preStation != "15")
                    {
                        string[] param = { Inputstring, status, preStation };
                        throw new FisException("SFC009", param);
                    }
                    mb.MBStatus.Station = "31A";
                    mb.MBStatus.Status = MBStatusEnum.Pass;
                    mb.MBStatus.Editor = editor;
                    mb.MBStatus.Udt = DateTime.Now;
                    //记录MB Log
                    MBLog mb_log = new MBLog(0, mb.Sn, mb.Model, "31A", (int)MBStatusEnum.Pass, line, editor, new DateTime());
                    mb.AddLog(mb_log);
                    iMBRepository.Update(mb, uow);
                }
                uow.Commit();
                return returnstring;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(PCAOQCInputImpl)saveMB End Input:" + Inputstring + "editor:" + editor + "station:" + station + "customer:" + customer);
            }
        }


        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="uutSn">mbsno</param>
        public void Cancel(string LotNo)
        {
            logger.Debug("(PCAOQCInputImpl)Cancel Start," + "LotNo:" + LotNo);
            try
            {
                string sessionKey = LotNo;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.MB);

                if (currentSession != null)
                {
                    SessionManager.GetInstance.RemoveSession(currentSession);
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(PCAOQCInputImpl)Cancel End," + "LotNo:" + LotNo);
            }

        }


        #endregion


    }

    
}
