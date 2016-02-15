/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Combine Po in Carton
* UI:CI-MES12-SPEC-PAK-UI Combine Po in Carton.docx –2012/05/21 
* UC:CI-MES12-SPEC-PAK-UC Combine Po in Carton.docx –2012/05/21          
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-05-21   Du.Xuan               Create   
* ITC-1414-0065  接口参数传入错误
* ITC-1414-0119  flag不等于N时未记录status和log
* ITC-1414-0122  Flag=N and boxReg='' 不再执行UPDATE SnoCtrl_BoxId
* Known issues:
* TODO：
* 
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
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.Utility;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.MO;
using System.Collections.Generic;
using carton = IMES.FisObject.PAK.CartonSSCC;
using IMES.DataModel;
using IMES.Infrastructure.Util;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    /// 產生BOX ID號相关逻辑
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Carton NO
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         更新Product.CUSTSN
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         无
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
    ///         Product
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProduct
    ///         IProductRepository
    /// </para> 
    /// </remarks>
    public partial class GenerateBoxID : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GenerateBoxID()
        {
            InitializeComponent();
        }

        private static object _syncRoot_GetSeq = new object();

        /// <summary>
        /// 產生BOX ID號號相关逻辑
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override System.Workflow.ComponentModel.ActivityExecutionStatus DoExecute(System.Workflow.ComponentModel.ActivityExecutionContext executionContext)
        {
            IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IPalletRepository palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            carton.ICartonSSCCRepository cartRep = RepositoryFactory.GetInstance().GetRepository<carton.ICartonSSCCRepository, IMES.FisObject.PAK.CartonSSCC.CartonSSCC>();

            try
            {

                //仅当Delivery 的Flag 属性为N 的时候，需要Generate Ship to Carton Label Pdf Document:
                //1.	Generate Box Id
                //2.	Generate and Record Data for Ship to Carton Label   
                //3.	Generate Ship to Carton Label Pdf Document
                string boxID = "";
                Delivery curDev = deliveryRep.Find(product.DeliveryNo);
                //curDev.Editor = this.Editor;
                CurrentSession.AddValue(Session.SessionKeys.Delivery, curDev);

                string flagstr = (string)curDev.GetExtendedProperty("Flag");
                string setSPValue = "";
                bool uccremoveflag = false;
                if (flagstr == "N")
                {
                    // 1.	解除Carton 结合的旧的Box Id
                    //参考方法：
                    //IF EXISTS (SELECT * FROM CartonInfo (NOLOCK ) WHERE CartonNo=@cn AND InfoType='BoxId')
                    //BEGIN
                    //DELETE CartonInfo WHERE artonNo=@cn AND InfoType='BoxId'
                    //END
                    CartonInfoInfo infoCond = new CartonInfoInfo();
                    infoCond.cartonNo = product.CartonSN;
                    infoCond.infoType = "BoxId";
                    //IList<CartonInfoInfo> infoList = null;// cartRep.GetCartonInfoInfo(infoCond);

                    cartRep.DeleteCartonInfoDefered(CurrentSession.UnitOfWork, infoCond);
                    //IF EXISTS (SELECT * FROM CartonInfo (NOLOCK ) WHERE CartonNo=@cn AND InfoType='UCC')
                    //BEGIN
	                //DELETE CartonInfo WHERE artonNo=@cn AND InfoType='UCC'
                    //END
                    CartonInfoInfo uccCond = new CartonInfoInfo();
                    uccCond.cartonNo = product.CartonSN;
                    uccCond.infoType = "UCC";

                    cartRep.DeleteCartonInfoDefered(CurrentSession.UnitOfWork, uccCond);

                    uccremoveflag = true;

                    //2.	取当前日期/ Pallet UCC
                    //参考方法：
                    //SELECT @dt=GETDATE(),@ucc=''
                    //SELECT @ucc = UCC FROM Pallet (NOLOCK) WHERE PalletNo = @plt
                    string ucc = "";

                    Pallet curPallet = palletRep.Find(product.PalletNo);
                    ucc = curPallet.UCC;
                    int ucclength = ucc.TrimEnd().Length;
                    int index = ucc.TrimEnd().IndexOf(",");

                    //3.	取Delivery 相关属性
                    //参考方法:
                    //SELECT @po = RTRIM(PoNo), @model = Model FROM Delivery NOLOCK WHERE DeliveryNo = @dn
                    //SELECT @Flag = InfoValue FROM DeliveryInfo NOLOCK WHERE DeliveryNo = @dn AND InfoType = 'Flag'
                    //SELECT @BoxId_Tag = InfoValue FROM DeliveryInfo NOLOCK WHERE DeliveryNo = @dn AND InfoType = 'BoxId'
                    //SET @BoxId_Tag = LEFT(@BoxId_Tag, 10)
                    //SELECT @Region = InfoValue FROM DeliveryInfo NOLOCK WHERE DeliveryNo = @dn AND InfoType = 'RegId'
                    //SELECT @Deport = InfoValue FROM DeliveryInfo NOLOCK WHERE DeliveryNo = @dn AND InfoType = 'Deport'
                    //SELECT @boxReg = InfoValue FROM DeliveryInfo NOLOCK WHERE DeliveryNo = @dn AND InfoType = 'BoxReg'

                    string po = "";
                    string model = "";
                    //string descr = "";
                    string flag = "";
                    string boxId_Tag = "";
                    string region = "";
                    string deport = "";
                    string boxReg = "";

                    po = curDev.PoNo.TrimEnd();
                    model = curDev.ModelName;
                    flag = (string)curDev.GetExtendedProperty("Flag");
                    boxId_Tag = (string)curDev.GetExtendedProperty("BoxId");
                    if (!string.IsNullOrEmpty(boxId_Tag) && boxId_Tag.Length >= 10)
                    {
                        boxId_Tag = boxId_Tag.Substring(0, 10);
                    }
                    else
                    {
                        boxId_Tag = "";
                    }
                    region = (string)curDev.GetExtendedProperty("RegId");
                    deport = (string)curDev.GetExtendedProperty("Deport");
                    boxReg = (string)curDev.GetExtendedProperty("BoxReg");
                    string uccFlag = (string)curDev.GetExtendedProperty("UCC");

                    
                    //4.	如果@boxReg<>''，则按照如下规则产生Box Id，并将产生的Box Id 存到变量@BoxId 中
                    if (uccFlag == "X")
                    {
                        if (!curPallet.PalletNo.StartsWith("NA"))
                        {
                            ucc = getUCCIDList();
                            ucclength = ucc.TrimEnd().Length;
                            index = -1;
                        }
                        else
                        {
                            ucc = curPallet.UCC;
                            ucclength = ucc.TrimEnd().Length;
                            index = -1;
                        }
                    }
                    else
                    {
                        #region none UCC case
                        if (!string.IsNullOrEmpty(boxReg))
                        {                         
                            boxID = getBoxId(boxReg);  
                        }
                        else
                        {
                            #region none boxId Prefix Code
                            //5.	如果@boxReg=''，则按照如下规则产生Box Id，并将产生的Box Id 存到变量@BoxId 中
                            //a)	当满足(LEN(RTRIM(@ucc))<>20 or (LEN(RTRIM(@ucc))=20 AND CHARINDEX(',',RTRIM(@ucc))>0)) AND RTRIM(@BoxId_Tag) ='' AND @Flag='N'条件时，按照如下规则产生Box Id
                            IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                            IList<string> valueList = PartRepository.GetValueFromSysSettingByName("Site");
                            if (valueList.Count == 0)
                            {
                                throw new FisException("PAK095", new string[] { "Site" });
                            }

                           // if ((ucclength != 20) || (ucclength == 20 && index >= 0 && string.IsNullOrEmpty(boxId_Tag.TrimEnd()) && flag == "N"))
                           // {
                            string alarm = "";
                            if (valueList[0] == "ICC")//重慶
                            {
                                //Vincent used ConstValue setting
                                IList<ConstValueInfo> boxIdPrefixList = PartRepository.GetConstValueListByType("BoxIdPreFixRule");
                                var prefixList = (from p in boxIdPrefixList
                                                  where p.name == region
                                                  select p.value).ToList();
                                if (prefixList != null && prefixList.Count > 0)
                                {
                                    alarm = prefixList[0].Trim();
                                }

                                if (string.IsNullOrEmpty(alarm))
                                {
                                    throw new FisException("CQCHK0049", new string[] { "BoxIdPreFixRule", region });
                                }
                            }
                            else
                            {
                                switch (region)
                                {
                                    case "SNA":
                                        alarm = "H410-Y";
                                        break;
                                    case "SNL":
                                        alarm = "LA" + deport + "Y";
                                        break;
                                    case "SNU":
                                        alarm = "D7" + deport + "Y";
                                        break;
                                    case "SNE":
                                        alarm = "63D7-QY";
                                        break;
                                    case "SAF":
                                        alarm = "H4FN-0C";
                                        break;
                                }

                            }


                            //SET ROWCOUNT 1
                            //SELECT @BoxId=RTRIM(BoxId) FROM SnoCtrl_BoxId WHERE Cust=@alarm AND valid=@editor
                            //IF @BoxId is not null
                            //DELETE FROM SnoCtrl_BoxId WHERE BoxId=@BoxId
                            //SET ROWCOUNT 0
                            //如果@BoxId IS NULL or @BoxId=''，则报告错误：'BoxId已用完.'+@alarm
                            SnoCtrlBoxIdInfo snoConf = new SnoCtrlBoxIdInfo();
                            SnoCtrlBoxIdInfo snoSet = new SnoCtrlBoxIdInfo();
                            snoConf.cust = alarm;
                            snoConf.valid = "1";
                            IList<string> boxList = deliveryRep.GetBoxIdListFromSnoCtrlBoxId(snoConf);

                            snoSet.valid = Editor;
                            //deliveryRep.UpdateSnoCtrlBoxIdInfoDefered(CurrentSession.UnitOfWork, snoSet, snoConf);

                            if (boxList != null && boxList.Count > 0 && !string.IsNullOrEmpty(boxList[0]))
                            {
                                SnoCtrlBoxIdInfo dsnoConf = new SnoCtrlBoxIdInfo();
                                boxID = boxList[0].TrimEnd();
                                dsnoConf.boxId = boxID;
                                deliveryRep.DeleteSnoCtrlBoxIdInfoDefered(CurrentSession.UnitOfWork, dsnoConf);
                            }
                            else
                            {
                                List<string> erpara = new List<string>();
                                erpara.Add(alarm);
                                FisException fe = new FisException("PAK135", erpara);   //BoxId已用完.'+@alarm
                                throw fe;
                            }

                            // }
                            #endregion
                        }

                        #endregion
                    }
                    //6.	如果LEN(RTRIM(@ucc))=20 AND CHARINDEX(',',@ucc)=0，则UCC 保存到CartonInfo 中；
                    //否则，将Box Id 保存到CartonInfo

                    //Record UCC
                    //IF EXISTS(SELECT * FROM CartonInfo NOLOCK WHERE CartonNo = @cn AND InfoType = 'UCC')
                    //UPDATE CartonInfo SET InfoValue = @ucc, Editor = @editor, Udt = GETDATE()
                    //WHERE CartonNo = @cn AND InfoType = 'UCC'
                    //ELSE
                    //INSERT INTO CartonInfo([CartonNo],[InfoType],[InfoValue],[Editor],[Cdt],[Udt])
                    //VALUES(@cn, 'UCC', @ucc, @editor, GETDATE(), GETDATE())
                    //Record Box Id
                    //IF EXISTS(SELECT * FROM CartonInfo NOLOCK WHERE CartonNo = @cn AND InfoType = 'BoxId')
                    //UPDATE CartonInfo SET InfoValue = @BoxId, Editor = @editor, Udt = GETDATE()
                    //WHERE CartonNo = @cn AND InfoType = 'BoxId'
                    //ELSE
                    //INSERT INTO CartonInfo([CartonNo],[InfoType],[InfoValue],[Editor],[Cdt],[Udt])
                    //VALUES(@cn, 'BoxId', @BoxId, @editor, GETDATE(), GETDATE())
                    string namestr = "";
                    string valuestr = "";

                    //if ((ucclength == 20) && (index < 0))
                    if (uccFlag == "X")
                    {
                        namestr = "UCC";
                        valuestr = ucc;
                    }
                    else
                    {
                        namestr = "BoxId";
                        valuestr = boxID;
                    }
                    setSPValue = valuestr;

                    infoCond.infoType = namestr;

                    //070031 2012.10.24 Unite the Ucc with BoxId Logic.
                    //070031 2012.09.27
                    if (namestr == "BoxId")
                    {
                        InvokeBody ivkBdy = null;

                        CartonInfoInfo setCarton = new CartonInfoInfo();
                        setCarton.infoType = namestr;
                        setCarton.infoValue = valuestr;
                        setCarton.cartonNo = product.CartonSN;
                        setCarton.editor = Editor;

                        var cond = new CartonInfoInfo();
                        cond.cartonNo = infoCond.cartonNo;
                        cond.infoType = namestr;
                        cond.infoValue = valuestr;
                        ivkBdy = cartRep.CheckTheBoxIdAndInsertOrUpdateDefered(CurrentSession.UnitOfWork, setCarton, cond, namestr);

                        var ivkBdy2 = cartRep.CheckTheBoxIdAndInsertOrUpdateDefered(CurrentSession.UnitOfWork, setCarton, cond, namestr);
                        ivkBdy2.DependencyIvkbdy = ivkBdy;
                        ivkBdy.ExpectRetVal = true;
                    }
                    else//Ucc
                    {
                        IList<CartonInfoInfo>  infoList = cartRep.GetCartonInfoInfo(infoCond);

                        CartonInfoInfo setCarton = new CartonInfoInfo();
                        setCarton.infoValue = valuestr;
                        setCarton.editor = Editor;
                        setCarton.udt = DateTime.Now;

                        if (infoList.Count > 0 && uccremoveflag==false)
                        {
                            
                            cartRep.UpdateCartonInfoDefered(CurrentSession.UnitOfWork, setCarton, infoCond);
                        }
                        else
                        {
                            setCarton.cartonNo = product.CartonSN;
                            setCarton.infoType = namestr;
                            cartRep.InsertCartonInfoDefered(CurrentSession.UnitOfWork, setCarton);
                        }
                    }
                    //070031 2012.09.27
                    //070031 2012.10.24 Unite the Ucc with BoxId Logic.
                }

                //Generate and Record Data for Ship to Carton Label 
                //调用HP_EDI.dbo.op_PackingData @BoxId,@dn,@plt,@cn,@cdtEDI实现
                //Parameters:@BoxId – Box Id @dn – Delivery No @plt – Pallet No @cn – Carton No @cdtEDI - GETDATE()
                if (flagstr == "N")
                {
                    palletRep.CallOp_PackingDataDefered(CurrentSession.UnitOfWork, setSPValue, product.DeliveryNo, product.PalletNo, product.CartonSN, DateTime.Now);
                }
                
            }
            catch (Exception)
            {              
                throw;
            }
          

            return base.DoExecute(executionContext);
        }


        private string getUCCIDList()
        {
            try
            {
                SqlTransactionManager.Begin();
                lock (_syncRoot_GetSeq)
                {
                    IList<string> ret = new List<string>();
                    INumControlRepository numControlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

                    long curMax = 0;
                    bool bInsert = false;
                    string curMaxStr = numControlRepository.GetMaxUCCIDNumber("UCCID");
                    if (curMaxStr == null || curMaxStr == "")
                    {
                        bInsert = true;
                        curMax = 2000000;
                    }
                    else
                    {
                        curMax = long.Parse(curMaxStr);
                    }
                 
                   
                    string s = "000008869889" + curMax.ToString("0000000");
                    int dig20 = 0;
                    int sum = 0;
                    for (int j = 0; j < 19; j++)
                    {
                        /*
                         * Answer to: ITC-1360-1155
                         * Description: Wrong processing while getting dig20.
                         */
                        if (j % 2 == 0) sum += (s[j] - '0') * 3;
                        else sum += s[j] - '0';
                    }
                    /*
                     * Answer to: ITC-1360-1153
                     * Description: Wrong processing while getting dig20.
                     */
                    dig20 = (10 - (sum % 10)) % 10;
                    s += dig20.ToString();                  
                    curMax++;
                    if (curMax == 6000000)
                    {
                        curMax = 2000000;
                    }                    

                    NumControl nc = new NumControl(-1, "UCCID", "", curMax.ToString(), "HP");
                    numControlRepository.SaveMaxNumber(nc, bInsert, "{0}");
                    SqlTransactionManager.Commit();

                    return s;
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


        private string getBoxId(string boxReg)
        {
            string boxID = "";
            //New Format of Box Id
            //RRRR-SSSSS 
            //Remark:
            //RRRR – @boxReg
            //- – 固定字符’-‘
            //SSSSS – 流水号，31进制，基字符'0123456789BCDFGHJKLMNPQRSTVWXYZ'；起始值为R0000；最大值为VZZZZ

            string maxnum = "";
            string prestr = "";

            boxID = boxReg + "-";
            prestr = boxID;
            // 自己管理事务开始
            try
            {
                SqlTransactionManager.Begin();
               // IUnitOfWork uof = new UnitOfWork();//使用自己的UnitOfWork

                //ITC-1414-0065
                //从NumControl中获取流水号
                //GetMaxNumber
                INumControlRepository numControl = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

                bool addflag = false;

                maxnum = numControl.GetMaxNumber("BOXID", prestr + "{0}");
                if (string.IsNullOrEmpty(maxnum))
                {
                    maxnum = "R0000";
                    addflag = true;
                }
                else
                {
                    MathSequenceWithCarryNumberRule marc = new MathSequenceWithCarryNumberRule(5, "0123456789BCDFGHJKLMNPQRSTVWXYZ");

                    //maxnum="CNU248000Y";                   
                    string numstr = maxnum.Substring(maxnum.Length - 5);
                    if (numstr.ToUpper() == "VZZZZ")
                    {
                        FisException fe = new FisException("CHK867", new string[] { });   //流水号已满!
                        throw fe;
                    }
                    numstr = marc.IncreaseToNumber(numstr, 1);
                    maxnum = numstr;

                }

                boxID = boxID + maxnum.ToUpper();

                //setSPValue = boxID;

                NumControl item = new NumControl();
                item.NOType = "BOXID";
                item.Value = boxID;
                item.NOName = "";
                item.Customer = "HP";

                numControl.SaveMaxNumber(item, addflag, prestr + "{0}");

                //uof.Commit();  //立即提交UnitOfWork更新NumControl里面的最大值

                SqlTransactionManager.Commit();//提交事物，释放行级更新锁
                return boxID;
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

    }
}
