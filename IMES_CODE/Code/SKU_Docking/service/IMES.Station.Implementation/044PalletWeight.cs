/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Content & Warranty Print
* UI:CI-MES12-SPEC-PAK-UI Pallet Weight.docx –2011/11/04 
* UC:CI-MES12-SPEC-PAK-UC Pallet Weight.docx –2011/11/04            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-11-04   Du.Xuan               Create   
* Known issues:
* ITC-1360-0653 修改错误编码PAK064，增加session处理
* ITC-1360-0647 增加数据不全时候的异常提示
* ITC-1360-0649 增加PAK092文字资源
* ITC-1360-1162 PAKComn无数据不报错
* ITC-1360-1609 允许重复称重 合并提示信息
* ITC-1360-1644 为支持预称重，Save 时不再要求更新ProductStatus 和记录ProductLog
*               为支持预称重， 不再通过检查Product 信息确定是否可以重印，而是修改为检查Pallet.Weight 来实现
* ITC-1360-1713 增加unit weight检查  
* ITC-1360-1807 修改pallettype算法，同pdpa2
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using log4net;
using IMES.Station.Interface.StationIntf;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.PrintLog;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.WorkflowRuntime;
using System.Workflow.Runtime;
using IMES.Route;
using IMES.Infrastructure.Repository._Schema;
using IMES.Infrastructure.Utility.Common;
using IMES.FisObject.PAK.StandardWeight;
using IMES.Common;

namespace IMES.Station.Implementation
{

    public class PalletWeight : MarshalByRefObject, IPalletWeight
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType currentSessionType = Session.SessionType.Common;


        #region PalletWeight

        /// <summary>
        /// 刷pallletNo，启动工作流，检查输入的pallletNo
        /// 将pallletNo放到Session.PalletNo中
        /// </summary>
        /// <param name="inputID"></param>
        /// <param name="acturalWeight"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ArrayList InputPallet(string inputID, decimal acturalWeight, string type,
                            string line, string editor, string station, string customer)
        {
            logger.Debug("(PalletWeight)InputPallet Start,"
                + " [pallletNo/UCCID]:" + inputID
                + " [line]:" + line
                + " [editor]:" + editor
                + " [station]:" + station
                + " [customer]:" + customer);


            //当20 位长的时候,视为UCC ID,否则,视为PalletNo 
            try
            {
                string plt = "";
                ArrayList retList = new ArrayList();
                List<string> erpara = new List<string>();

                if (inputID.Length == 20)
                {
                    string strSQL = "select rtrim(PalletNo) from Pallet where UCC=@palletId";
                    SqlParameter paraName = new SqlParameter("@palletId", SqlDbType.VarChar, 32);
                    paraName.Direction = ParameterDirection.Input;
                    paraName.Value = inputID;

                    object obj = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text, strSQL, paraName);

                    if (obj == null)
                    {
                        FisException e;
                        erpara.Add(inputID);
                        e = new FisException("PAK092", erpara);
                        throw e;
                    }

                    plt = obj.ToString();
                }
                else
                {
                    plt = inputID;
                }

                string sessionKey = plt;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, currentSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "044PalletWeight.xoml", "044PalletWeight.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    currentSession.AddValue(Session.SessionKeys.PalletNo, plt);
                    //currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.ActuralWeight, acturalWeight);
                    //currentSession.AddValue("PalletType", type);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        FisException ex;
                        //List<string> erpara = new List<string>();
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    FisException ex;
                    //List<string> erpara = new List<string>();
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
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
                //=============================================================
                Pallet curPallet = (Pallet)currentSession.GetValue(Session.SessionKeys.Pallet);
                IPalletRepository palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                IPizzaRepository pizzaRep = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

                string strRepeat = "";
                string strCheck = "";
                //1.	重复称重提示
                //当输入的Pallet 已经称重(ISNULL（IMES_PAK..Pallet.Weight, ''） <> '') 时，
                //使用对话框提示用户：“此栈板为重复称重.”
                if (curPallet.Weight != 0)
                {
                    /*SessionManager.GetInstance.RemoveSession(currentSession);
                    FisException ex;
                    erpara.Add(sessionKey);
                    ex = new FisException("PAK064", erpara);//此栈板为重复称重
                    throw ex;
                    */
                    FisException ex;
                    erpara.Add(sessionKey);
                    ex = new FisException("PAK064", erpara);//此栈板为重复称重
                    strRepeat = ex.mErrmsg;
                }

                //3.	2D Barcode 检查 – IMEI 检查？
                //使用Pallet No 查询IMES_PAK..Delivery_Pallet 表，得到Shipment，然后按照如下方法确定是否需要提示用户Scan 2D Barcode
                IList<DeliveryPallet> dpList = palletRep.GetDeliveryPallet(curPallet.PalletNo);
                if (dpList.Count == 0)
                {
                    SessionManager.GetInstance.RemoveSession(currentSession);
                    FisException ex;
                    erpara.Add(sessionKey);
                    ex = new FisException("PAK090", erpara);//无法找到相应Delivery
                    throw ex;
                }

                String shipment = dpList[0].ShipmentID;
                Boolean chkFlag = false;
                //SELECT @setnum=DOC_SET_NUMBER FROM HP_EDI..[PAK.PAKComn] (NOLOCK) WHERE InternalID=@shipment 
                IList<string> numList = pizzaRep.GetDocSetNumListFromPakDashPakComnByInternalID(shipment);

                if (numList.Count > 0)
                {
                    /*SessionManager.GetInstance.RemoveSession(currentSession);
                    FisException ex;
                    erpara.Add(sessionKey);
                    ex = new FisException("PAK091", erpara);//PAKComn没有相应数据
                    throw ex;*/
                    //IF EXISTS (SELECT * FROM HP_EDI..[PAK.PAKRT] 
                    //WHERE DOC_SET_NUMBER=@setnum AND XSL_TEMPLATE_NAME like '%Verizon2D%' 
                    //AND DOC_CAT='Pallet Ship Label- Pack ID Single' )

                    //如果上面的SQL 走到执行Select '1'	 语句的分支时，
                    //则提示用户：“Please scan 2D barcode!”，并在后面要求刷入IMEI
                    chkFlag = palletRep.CheckExistFromPakDotPakRtByDocCatAndDocSetNumer("Pallet Ship Label- Pack ID Single",
                    numList[0], "Verizon2D");
                    if (chkFlag)
                    {
                        FisException ex;
                        erpara.Add(sessionKey);
                        ex = new FisException("PAK118", erpara);//Please scan 2D barcode!
                        strCheck = ex.mErrmsg;
                    }
                }

                //Pallet Type
                //Get Pallet Type by Pallet No:
                //2.	参考CI-MES12-SPEC-PAK-UC PD PA Label 2.docx 文档中Page 15 页中g. 小节的描述，
                //确定Pallet Type 及相关提示信息 – RTRIM(@pclor) 为Pallet Type；
                //@emeastr 为提示信息（在UI 上[Pallet No] 右侧，显示对应的提示信息）
                
                // g.下文描述的是针对不同的情况，如何获取需要提示用户的信息 
                //a)	取Delivery 的PalletQty (IMES_PAK..DeliveryInfo.InfoValue，Condition: InfoType = 'PalletQty')属性值，
                //并取其整数部分保存到@paletqty 变量中；如果该属性不存在，则令@paletqty = 60
                Delivery curDelivery = deliveryRep.Find(dpList[0].DeliveryID);
                string regId = (string)curDelivery.GetExtendedProperty("RegId");
                if (regId != null && regId.Length == 3)
                { regId = regId.Substring(1, 2); }
                else
                { regId = ""; }
                string shipWay = (string)curDelivery.GetExtendedProperty("ShipWay");

                string paletqty = (string)curDelivery.GetExtendedProperty("PalletQty");

                if (string.IsNullOrEmpty(paletqty))
                {
                    paletqty = "60";
                }
                //b)	使用如下方法，取得变量@pqty
                //SELECT @pqty = sum(DeliveryQty) FROM Delivery_Pallet NOLOCK WHERE PalletNo = @Plt
                int pqty = deliveryRep.GetSumDeliveryQtyOfACertainPallet(curPallet.PalletNo);

                //c)	使用如下方法，取得变量@pqty2
                //SELECT @pqty2=TierQty FROM PalletStandard WHERE FullQty=@paletqty
                int pqty2 = palletRep.GetTierQtyFromPalletQtyInfo(paletqty);

                //d)	如果@pqty>=@pqty2，则令@emeastr='海运，满一层请使用大的木头栈板'
                string emeastr = string.Empty;
                string pclor = string.Empty;

                if (pqty > pqty2)
                {
                    emeastr = "海运，满一层请使用大的木头栈板";
                }
                //e)	如果(@Region='SNE' or @Region='SCE' ) and @shipway<>'T002'时
                //i.	如果@pqty>=@pqty2 and @pqty2>0，则令@emeastr='满一层请使用chep栈板'；@pclor=' H'；
                //否则令@emeastr=' '；@pclor=''
                if ((regId == "NE" || regId == "CE") && (shipWay != "T002"))
                {
                    if (pqty > pqty2 && pqty2 > 0)
                    {
                        emeastr = "满一层请使用chep栈板";
                        pclor = " H";
                    }
                    else
                    {
                        emeastr = "";
                        pclor = "";
                    }
                }
                else if ((regId == "NL") && (shipWay != "T002"))
                {   //f)	不满足上一步的条件时，如果@Region='SNL' and @shipway<>'T002' 时
                    //i.	如果@pqty>=@pqty2 and @pqty2>0，则令@emeastr='请使用绿色塑料栈板'；@pclor=' P'；
                    //否则令@emeastr=' '；@pclor=''
                    if (pqty > pqty2 && pqty2 > 0)
                    {
                        emeastr = "请使用蓝色塑料栈板";
                        pclor = " A";
                    }
                    else
                    {
                        emeastr = "";
                        pclor = "";
                    }

                }
                else if ((regId == "NU" || regId == "CU") && (shipWay != "T002"))
                {    //g)	不满足前面的条件时，如果(@Region='SNU' or @Region='SCU' ) and @shipway<>'T002' 时
                    //i.	如果@pqty>=@pqty2 and @pqty2>0，则令@emeastr='请使用蓝色塑料栈板'；@pclor=' A'；
                    //否则令@emeastr=' '；@pclor=''
                    if (pqty > pqty2 && pqty2 > 0)
                    {
                        emeastr = "请使用蓝色塑料栈板";
                        pclor = " A";
                    }
                    else
                    {
                        emeastr = "";
                        pclor = "";
                    }
                }
                else if ((regId == "NE" || regId == "CE") && (shipWay == "T002"))
                {
                    //h)	不满足前面的条件时，如果(@Region='SNE' or @Region='SCE' ) and @shipway='T002' 时，
                    //      则令@emeastr='EMEA海运,请使用E1栈板'；@pclor=' K'
                    emeastr = "EMEA海运,请使用E1栈板";
                    pclor = " K";
                }
                else if ((regId == "AF") && (shipWay == "T001"))
                {
                    //i)	不满足前面的条件时，如果@shipway='T001' and @Region='SAF' 时，
                    //i.	如果@pqty>=@pqty2 and @pqty2>0，则令@emeastr='請使用綠色塑料棧板'；@pclor=' P'；
                    //否则令@emeastr=' '；@pclor=''
                    if (pqty > pqty2 && pqty2 > 0)
                    {
                        emeastr = "请使用绿色塑料栈板";
                        pclor = " P";
                    }
                    else
                    {
                        emeastr = "";
                        pclor = "";
                    }

                }                   
                //else if ((regId == "CN") && (shipWay == "T001"))
                else if ((ActivityCommonImpl.Instance.CheckDomesticDN(regId)) && (shipWay == "T001"))
                {
                    // j)	不满足前面的条件时，如果@shipway='T001' and @Region='SCN' 时，
                    //i.	如果@pqty>=@pqty2 and @pqty2>0，则令@emeastr='满一层请使用大的木头栈板'；@pclor=' '；
                    //否则令@emeastr=' '；@pclor=''
                    if (pqty > pqty2 && pqty2 > 0)
                    {
                        emeastr = "满一层请使用大的木头栈板";
                        pclor = " ";
                    }
                    else
                    {
                        emeastr = "";
                        pclor = "";
                    }

                }
                else
                {
                    emeastr = "";
                    pclor = "";
                }


                //3.按照下表确定Pallet Type
                // Pallet Type	Command
                //  'H'	        '666666'
                //  'P'	        '110119'
                //  'A'	        '666666'
                //  'E'         '666666'
                //  Else		N/A
                //上表中Command 列，表示在特定的Pallet Type 时，需要刷入那个对应的命令码完成操作
                String command = "";
                switch (pclor.Trim())
                {
                    case "H":
                        command = "666666";
                        break;
                    case "P":
                        command = "110119";
                        break;
                    case "A":
                        command = "666666";
                        break;
                    case "E":
                        command = "666666";
                        break;
                }
                currentSession.AddValue("PalletColor", pclor);

                decimal standWeight = 0;
                decimal palletWeight = 0;
                decimal productWeight = 0;
                IPalletWeightRepository pltWeightRepository = RepositoryFactory.GetInstance().GetRepository<IPalletWeightRepository, IMES.FisObject.PAK.Pallet.PalletWeight>();
                IList<PalletWeightInfo> weightList = null;

                productWeight = (decimal)currentSession.GetValue("ProductWeight");

                PalletWeightInfo conf = new PalletWeightInfo();
                conf.palletType = pclor.Trim();
                weightList = pltWeightRepository.GetPltWeightByCondition(conf);
                if ((weightList != null) && (weightList.Count > 0))
                {
                    palletWeight = weightList[0].palletWeight;
                }

                //SELECT @PalletStandardWeight = @ProductsWeight + @PalletWeight
                standWeight = productWeight + palletWeight;
                currentSession.AddValue(Session.SessionKeys.StandardWeight, standWeight);

               // 如果空运出货EMEA 地区时，没有选择[Pallet Type]，则报告错误：“请选择Pallet Type!”
                //空运出货的判定方法：
                //取结合当前Pallet 的任一Delivery（Delivery_Pallet.DeliveryNo，Condition: PalletNo = @PalletNo），
                //然后取该Delivery的ShipWay 属性（DeliveryInfo.InfoValue，Condition:DeliveryInfo.InfoType = ‘ShipWay’），
                //如果ShipWay 属性为T001，则为空运出货

                //出货EMEA的判定方法：
                //取结合当前Pallet 的任一Delivery（Delivery_Pallet.DeliveryNo，Condition: PalletNo = @PalletNo），
                //然后，取该Delivery的RegId 属性（DeliveryInfo.InfoValue，Condition:DeliveryInfo.InfoType = ‘RegId’），
                //如果RegId 属性为SNE 或者SCE，则为出货EMEA 地区
                bool checkType = false;
                if (shipWay == "T001" && (regId == "NE" || regId == "CE"))
                {
                    checkType = true;
                }

                 //为了支持小栈板，当上文得到的Pallet Type 为空的时候，
                //如果空运出货EMEA 地区时，@emeastr 设置为“请选择栈板类型” 
                //（在UI 上[Pallet No] 右侧，显示对应的提示信息）
                if (command == "" && emeastr =="" && checkType)
                {
                     emeastr = "请选择栈板类型";
                }

                //为了支持海运小栈板，当上文得到的Pallet Type 为空的时候，
                //如果海运出货时，@emeastr 设置为“海运，请使用木头栈板” 
                //T002，则为海运出货
                if (shipWay == "T002" && command == "" && emeastr == "")
                {
                    emeastr = "海运，请使用木头栈板";
                }

                //Product Info – 放在栈板上的Product Model以及数量
                //参考方法：
                //SELECT Model, Count(*) as PCs FROM IMES_FA..Product nolock 
                //WHERE PalletNo = @PalletNo
                //GROUP BY Model
                //ORDER BY Model
                IList<ModelStatistics> modelList = palletRep.GetByModelStatisticsForSinglePallet(curPallet.PalletNo);

                decimal standardWeight = (decimal)currentSession.GetValue(Session.SessionKeys.StandardWeight);
                decimal tolerance = (decimal)currentSession.GetValue(Session.SessionKeys.Tolerance);

                string strNode = "";
                retList.Add(chkFlag);
                retList.Add(curPallet.PalletNo);
                retList.Add(emeastr);
                retList.Add(command);
                retList.Add(modelList);
                retList.Add(standardWeight);
                retList.Add(tolerance);               
                if (chkFlag)
                {
                    strNode = strCheck + System.Environment.NewLine + strRepeat;
                }
                else
                {
                    strNode = strRepeat;
                }
                retList.Add(strNode);
                retList.Add(checkType);
                //=============================================================
                return retList;
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
                logger.Debug("(PalletWeight)InputPallet End,"
                   + " [pallletNo]:" + inputID
                   + " [line]:" + line
                   + " [editor]:" + editor
                   + " [station]:" + station
                   + " [customer]:" + customer);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="palletNo"></param>
        public String InputCustSN(string custSN, string palletNo, string type)
        {
            logger.Debug("(PalletWeight)Cancel Start,"
               + " [pallletNo]:" + palletNo
               + " [CustSN]:" + custSN);

            List<string> erpara = new List<string>();
            FisException ex;
            try
            {

                Session currentSession = SessionManager.GetInstance.GetSession(palletNo, currentSessionType);

                if (currentSession == null)
                {
                    erpara.Add(palletNo);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    currentSession.AddValue("PalletType", type);

                    var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);
                    //1.	Check Pallet No
                    //如果Customer S/N 绑定的Pallet No (IMES_FA..Product.PalletNo)与UI 上已经录入的Pallet No 不同
                    //则报告错误：“此栈板序号与Customer S/N 序号比对错误！”
                    if (currentProduct.PalletNo != palletNo)
                    {
                        erpara.Add(palletNo);
                        ex = new FisException("PAK065", erpara);//此栈板序号与Customer S/N 序号比对错误！
                        throw ex;
                    }

                    //2.	RFID 检查
                    //如果该Customer S/N 对应的Product Model，在ModelBOM 中，
                    //Model的直接下阶数据中存在Descr = 'RFID Label' 的Part，则需要弹出对话框提示用户：“请到RFID 称重页面！”
                    IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                    string pno = currentProduct.Model.Trim();
                    IHierarchicalBOM curBom = bomRep.GetHierarchicalBOMByModel(pno);
                    IList<IBOMNode> bomNodeList = curBom.FirstLevelNodes;
                    foreach (IBOMNode bomNode in bomNodeList)
                    {
                        if (bomNode.Part.Descr == "RFID Label")
                        {
                            erpara.Add(palletNo);
                            ex = new FisException("PAK066", erpara);//“请到RFID 称重页面！”
                            throw ex;
                        }
                    }

                    //3.	Unit Weight 检查
                    //如果该Customer S/N 对应的ISNULL(Product.UnitWeight, 0.0) = 0.0，
                    //则报告错误：“此Product 尚未进行Unit Weight！”
                    if (currentProduct.UnitWeight.Equals(0))
                    {
                        erpara.Add(currentProduct.ProId);
                        ex = new FisException("PAK122", erpara);//“此Product 尚未进行Unit Weight！”
                        throw ex;
                    }

                    return custSN;

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
                logger.Debug("(PalletWeight)InputCustSN End,"
                + " [pallletNo]:" + palletNo);
            }

        }

        public String CheckIMEI(string palletNo, string imei)
        {
            logger.Debug("(PalletWeight)CheckIMEI Start,"
               + " [pallletNo]:" + palletNo
               + " [imei]:" + imei);

            List<string> erpara = new List<string>();
            FisException ex;
            try
            {

                Session currentSession = SessionManager.GetInstance.GetSession(palletNo, currentSessionType);

                if (currentSession == null)
                {
                    erpara.Add(palletNo);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    //根据UI 上的Pallet No(下文中的@plt)及用户刷入的IMEI (下文中的@imei)来Check
                    //IF NOT EXISTS (SELECT * FROM HP_EDI..MEID_Log WHERE PALLET_ID=rtrim(@plt) AND StringIDValue=@imei)
                    //BEGIN
                    //SELECT '0'
                    //RETURN
                    //END
                    //ELSE
                    //BEGIN
                    //UPDATE HP_EDI..MEID_Log SET IsPass='1' WHERE PALLET_ID=rtrim(@plt) AND StringIDValue=@imei
                    //END
                    //IF NOT EXISTS ( SELECT * FROM HP_EDI..MEID_Log WHERE PALLET_ID=rtrim(@plt) AND IsPass='0')
                    //BEGIN
                    //SELECT '1'
                    //END
                    //ELSE
                    //BEGIN
                    //SELECT '2'
                    //END
                    //当前面得到的结果为'0'，则报告错误：“2D Barcode 与DB 不符!”
                    //当前面得到的结果为'2'，则报告错误：“请刷2D Barcode!”
                    //当前面得到的结果为'1'，则检查通过

                    IPizzaRepository pizzaRep = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                    Boolean chkflag = false;

                    chkflag = pizzaRep.CheckExistMeidLogByPltAndStringIdValue(palletNo, imei);
                    if (!chkflag)
                    {
                        erpara.Add(palletNo);
                        ex = new FisException("PAK067", erpara);//“2D Barcode 与DB 不符!”
                        throw ex;
                    }

                    pizzaRep.UpdateMeidLogIsPassByPalletIdAndStringIDValue(1, palletNo, imei);

                    chkflag = pizzaRep.CheckExistMeidLogByPltAndIsPass(palletNo, 0);
                    if (chkflag)
                    {
                        erpara.Add(palletNo);
                        ex = new FisException("PAK068", erpara);//“请刷2D Barcode!”
                        throw ex;
                    }
                    return imei;
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
                logger.Debug("(PalletWeight)InputCustSN End,"
                + " [pallletNo]:" + palletNo);
            }

        }
        /// <summary>
        /// 将ActuralWeight添加到Session.ActuralWeight中
        /// 将custSn放到Session.CustSN中
        /// 结束工作流
        /// </summary>
        /// <param name="pallletNo"></param>
        /// <param name="custSn"></param>
        /// <param name="acturalWeight"></param>
        /// <param name="currentStandardWeight"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        public IList<PrintItem> Save(string pallletNo, string custSn, decimal acturalWeight, IList<PrintItem> printItems)
        {
            logger.Debug("(PalletWeight)Save Start,"
                + " [pallletNo]:" + pallletNo
                + " [custSn]:" + custSn
                + " [acturalWeight]:" + acturalWeight.ToString());
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(pallletNo, currentSessionType);

                if (currentSession == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(pallletNo);
                    ex = new FisException("CHK021", erpara);
                    logger.Error(ex.Message, ex);
                    throw ex;
                }
                else
                {
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.CustSN, custSn);

                    currentSession.AddValue(Session.SessionKeys.ActuralWeight, acturalWeight);
                    //currentSession.AddValue(Session.SessionKeys.StandardWeight, acturalWeight);

                    currentSession.AddValue(Session.SessionKeys.PrintLogName, pallletNo);
                    //currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, "");
                    //currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, "");
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "PalletWeight");

                    currentSession.Exception = null;
                    currentSession.SwitchToWorkFlow();
                }

                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }

                return (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);

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
                logger.Debug("(PalletWeight)Save End,"
                    + " [pallletNo]:" + pallletNo
                    + " [custSn]:" + custSn
                    + " [acturalWeight]:" + acturalWeight.ToString());
            }
        }
        public ArrayList InputPallet_CQ(string inputID, decimal acturalWeight, string type,
                        string line, string editor, string station, string customer)
        {
            logger.Debug("(PalletWeight)InputPallet Start,"
                + " [pallletNo/UCCID]:" + inputID
                + " [line]:" + line
                + " [editor]:" + editor
                + " [station]:" + station
                + " [customer]:" + customer);


            //当20 位长的时候,视为UCC ID,否则,视为PalletNo 
            try
            {
                string plt = "";
                ArrayList retList = new ArrayList();
                List<string> erpara = new List<string>();

                if (inputID.Length == 20)
                {
                    string strSQL = "select rtrim(PalletNo) from Pallet where UCC=@palletId";
                    SqlParameter paraName = new SqlParameter("@palletId", SqlDbType.VarChar, 32);
                    paraName.Direction = ParameterDirection.Input;
                    paraName.Value = inputID;

                    object obj = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text, strSQL, paraName);

                    if (obj == null)
                    {
                        FisException e;
                        erpara.Add(inputID);
                        e = new FisException("PAK092", erpara);
                        throw e;
                    }

                    plt = obj.ToString();
                }
                else
                {
                    plt = inputID;
                }

                string sessionKey = plt;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, currentSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "044PalletWeight.xoml", "044PalletWeight.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    currentSession.AddValue(Session.SessionKeys.PalletNo, plt);
                    //currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.ActuralWeight, acturalWeight);
                    //currentSession.AddValue("PalletType", type);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        FisException ex;
                        //List<string> erpara = new List<string>();
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    FisException ex;
                    //List<string> erpara = new List<string>();
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
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
                //=============================================================
                Pallet curPallet = (Pallet)currentSession.GetValue(Session.SessionKeys.Pallet);
                IPalletRepository palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                IPizzaRepository pizzaRep = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

                string strRepeat = "";
                string strCheck = "";
                //1.	重复称重提示
                //当输入的Pallet 已经称重(ISNULL（IMES_PAK..Pallet.Weight, ''） <> '') 时，
                //使用对话框提示用户：“此栈板为重复称重.”
                if (curPallet.Weight != 0)
                {
                   FisException ex;
                    erpara.Add(sessionKey);
                    ex = new FisException("PAK064", erpara);//此栈板为重复称重
                    strRepeat = ex.mErrmsg;
                }

                //3.	2D Barcode 检查 – IMEI 检查？
                //使用Pallet No 查询IMES_PAK..Delivery_Pallet 表，得到Shipment，然后按照如下方法确定是否需要提示用户Scan 2D Barcode
                IList<DeliveryPallet> dpList = palletRep.GetDeliveryPallet(curPallet.PalletNo);
                if (dpList.Count == 0)
                {
                    SessionManager.GetInstance.RemoveSession(currentSession);
                    FisException ex;
                    erpara.Add(sessionKey);
                    ex = new FisException("PAK090", erpara);//无法找到相应Delivery
                    throw ex;
                }

                String shipment = dpList[0].ShipmentID;
                Boolean chkFlag = false;
                //SELECT @setnum=DOC_SET_NUMBER FROM HP_EDI..[PAK.PAKComn] (NOLOCK) WHERE InternalID=@shipment 
                IList<string> numList = pizzaRep.GetDocSetNumListFromPakDashPakComnByInternalID(shipment);

                if (numList.Count > 0)
                {
                    /*SessionManager.GetInstance.RemoveSession(currentSession);
                    FisException ex;
                    erpara.Add(sessionKey);
                    ex = new FisException("PAK091", erpara);//PAKComn没有相应数据
                    throw ex;*/
                    //IF EXISTS (SELECT * FROM HP_EDI..[PAK.PAKRT] 
                    //WHERE DOC_SET_NUMBER=@setnum AND XSL_TEMPLATE_NAME like '%Verizon2D%' 
                    //AND DOC_CAT='Pallet Ship Label- Pack ID Single' )

                    //如果上面的SQL 走到执行Select '1'	 语句的分支时，
                    //则提示用户：“Please scan 2D barcode!”，并在后面要求刷入IMEI
                    chkFlag = palletRep.CheckExistFromPakDotPakRtByDocCatAndDocSetNumer("Pallet Ship Label- Pack ID Single",
                    numList[0], "Verizon2D");
                    if (chkFlag)
                    {
                        FisException ex;
                        erpara.Add(sessionKey);
                        ex = new FisException("PAK118", erpara);//Please scan 2D barcode!
                        strCheck = ex.mErrmsg;
                    }
                }

                // ***********Begin GET Pallet Type FOR CQ *****************
                currentSession.AddValue("Site", CommonUti.GetSite());
                Delivery curDelivery = deliveryRep.Find(dpList[0].DeliveryID);
                IList<PalletType> lstPalletType= CommonUti.GetPalletType(curPallet.PalletNo, curDelivery.DeliveryNo);
                if (lstPalletType.Count == 0)
                {
                    throw new Exception("No match Pallet Type");
                }
                PalletType palletTypeObj = lstPalletType[0];
                string palletKind = palletTypeObj.Type;
                string command = palletTypeObj.CheckCode;
                if (palletTypeObj.MinusPltWeight == "0")
                { currentSession.AddValue(Session.SessionKeys.PalletWeight, "0"); }
                else
                { currentSession.AddValue(Session.SessionKeys.PalletWeight, palletTypeObj.PltWeight.ToString()); }

                // ***********End GET Pallet Type FOR CQ *****************

                bool checkType = false;
                if (palletTypeObj.ShipWay == "T001" && (palletTypeObj.RegId == "SNE" || palletTypeObj.RegId == "SCE"))
                {
                    checkType = true;
                }


                // *************Begin Calc Standard Weight FOR CQ *****************
                decimal standardWeight = 0;
                decimal palletWeight = 0;
                decimal productWeight = 0;
                IPalletWeightRepository pltWeightRepository = RepositoryFactory.GetInstance().GetRepository<IPalletWeightRepository, IMES.FisObject.PAK.Pallet.PalletWeight>();
             
                productWeight = (decimal)currentSession.GetValue("ProductWeight");
                palletWeight=palletTypeObj.PltWeight;
                standardWeight = productWeight + palletWeight;
                currentSession.AddValue(Session.SessionKeys.StandardWeight, standardWeight);



                // *************End Calc Standard Weight FOR CQ *****************
            
                IList<ModelStatistics> modelList = palletRep.GetByModelStatisticsForSinglePallet(curPallet.PalletNo);
                decimal tolerance = (decimal)currentSession.GetValue(Session.SessionKeys.Tolerance);
                string strNode = "";
                retList.Add(chkFlag);
                retList.Add(curPallet.PalletNo);
                retList.Add(palletKind); //  emeastr = "海运，满一层请使用大的木头栈板";
                retList.Add(command); //  case "H":           command = "666666";
                retList.Add(modelList);
                retList.Add(standardWeight);
                retList.Add(tolerance);
                if (chkFlag)
                {
                    strNode = strCheck + System.Environment.NewLine + strRepeat;
                }
                else
                {
                    strNode = strRepeat;
                }
                retList.Add(strNode);
                retList.Add(checkType);
                //=============================================================
                return retList;
            }
            catch (FisException e)
            {
                Cancel(inputID);
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                Cancel(inputID);
                logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(PalletWeight)InputPallet End,"
                   + " [pallletNo]:" + inputID
                   + " [line]:" + line
                   + " [editor]:" + editor
                   + " [station]:" + station
                   + " [customer]:" + customer);
            }

        }



        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="productID"></param>
        public void Cancel(string pallletNo)
        {
            logger.Debug("(PalletWeight)Cancel Start,"
               + " [pallletNo]:" + pallletNo);
            try
            {

                Session currentSession = SessionManager.GetInstance.GetSession(pallletNo, currentSessionType);

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
                logger.Debug("(PalletWeight)Cancel End,"
                + " [pallletNo]:" + pallletNo);
            }

        }
        #endregion


        #region "methods do not interact with the running workflow"


        /// <summary>
        /// 重印标签
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="reason"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        public ArrayList ReprintPalletWeightLabel(string custSN, string reason, string line, string editor,
                                    string station, string customer, IList<PrintItem> printItems)
        {
            logger.Debug("(PalletWeight)ReprintLabel Start,"
                            + " [custSN]:" + custSN
                            + " [line]:" + line
                            + " [editor]:" + editor
                            + " [station]:" + station
                            + " [customer]:" + customer);
            string plt = "";
            ArrayList retList = new ArrayList();
            try
            {
                var currentProduct = CommonImpl.GetProductByInput(custSN, CommonImpl.InputTypeEnum.CustSN);


                /*string strSQL = "select rtrim(PalletNo) from Pallet where UCC=@palletId";
                SqlParameter paraName = new SqlParameter("@palletId", SqlDbType.VarChar, 32);
                paraName.Direction = ParameterDirection.Input;
                paraName.Value = currentProduct.PalletNo;

                object obj = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text, strSQL, paraName);

                if (obj == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(custSN);
                    ex = new FisException("CHK106", erpara);
                    throw ex;
                 }
                */
                plt = currentProduct.PalletNo;//obj.ToString();
                string sessionKey = currentProduct.ProId;


                IPalletRepository PalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                Pallet CurrentPallet = PalletRepository.Find(plt);
                if (CurrentPallet == null)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(custSN);
                    ex = new FisException("CHK106", erpara);
                    throw ex;

                }
                if (CurrentPallet.Weight.Equals(0))
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(custSN);
                    ex = new FisException("CHK860", erpara);//此Product没有打印过，无需重印
                    throw ex;
                }

                /*var repository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();
                bool printFlag = false;
                printFlag = repository.CheckExistPrintLogByLabelNameAndDescr(plt, plt);
                */

                var repository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.FA.Product.IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                /*IList<ProductLog> logList = repository.GetProductLogs(currentProduct.ProId,station);
                
                if (logList.Count==0)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(plt);                  
                    ex = new FisException("CHK860", erpara);//此Product没有打印过，无需重印
                    throw ex;
                } */
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, currentSessionType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, currentSessionType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", currentSessionType);

                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("044PalletWeightReprint.xoml", "", wfArguments);
                    currentSession.AddValue(Session.SessionKeys.Pallet, CurrentPallet);

                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currentProduct.ProId);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currentProduct.ProId);
                    currentSession.AddValue(Session.SessionKeys.PrintLogName, currentProduct.ProId);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr, "PalletWeight");
                    currentSession.AddValue(Session.SessionKeys.Reason, reason);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.SetInstance(instance);


                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
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

                IList<PrintItem> printList = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);

                retList.Add(printList);
                retList.Add(CurrentPallet.PalletNo);
                retList.Add(CurrentPallet.Weight);

                return retList;

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
                logger.Debug("(PalletWeight)ReprintLabel End,"
                                + " [custSN]:" + custSN
                                + " [line]:" + line
                                + " [editor]:" + editor
                                + " [station]:" + station
                                + " [customer]:" + customer);
            }
        }

        /// <summary>
        /// 获取当天已经完成称重的Pallet数量
        /// </summary>
        /// <returns></returns>
        public int getQtyOfPalletToday()
        {
            logger.Debug("(PalletWeight)getQtyOfPalletToday Start");

            try
            {
                int todayWeight;
                IPalletRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                todayWeight = palletRepository.GetQtyOfPackedPalletToday();
                return todayWeight;
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
                logger.Debug("(PalletWeight)getQtyOfPalletToday End");
            }
        }
        #endregion

    }
}
