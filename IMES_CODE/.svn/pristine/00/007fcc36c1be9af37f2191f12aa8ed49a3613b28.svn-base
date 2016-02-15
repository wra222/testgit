using System;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Extend;

namespace IMES.Activity
{
    /// <summary>
    /// 用于维修站, 解绑旧Part绑定记录, 如果只输入了Faulty Part Sno，则需要删除[PCB_Part]/[Product_Part] 中的相关记录，如果Faulty Part Sno 不存在，则报告错误：“该Part （@FaultyPartSno）不存在!!”
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      当前只应用于BN的FA Repair站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.检查是否Faulty Part Sno有输入;
    ///         2.删除Part绑定数据，下列类型需特殊处理：
    ///             MB: 清空Product表中对应MB的各项属性；
    ///             LCM1, LCM2: 若解绑其中一种类型，则另一种类型也需要一同解绑
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///             如果Faulty Part Sno 不存在，则报告错误：“该Part （@FaultyPartSno）不存在!!” 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.PartID
    ///         Session.CurrentReapirDefectInfo
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
    ///         Product_Parts, Product(only for MB)
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    /// </para> 
    /// </remarks>
    public partial class RemovePart : BaseActivity
	{
        private const string FaRepairStation = "45";

        /// <summary>
        /// 构造函数
        /// </summary>
        public RemovePart()
		{
			InitializeComponent();
		}

        #region Overrides of BaseActivity

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            try
            {
                IRepairTarget repairTarget = GetRepairTarget();
                RepairDefect defect;
                defect = (RepairDefect)CurrentSession.GetValue(Session.SessionKeys.CurrentRepairdefect);
                IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
                          
                //bool bSearch = false;
                bool bLCMType = false;
                
                //IList<IProductPart> productParts = product.ProductParts;
                IList<IProductPart> productParts = (IList<IProductPart>)CurrentSession.GetValue(ExtendSession.SessionKeys.ProductPart);
                bool isReleaseMB = (bool)CurrentSession.GetValue(ExtendSession.SessionKeys.ReleaseMB);
                bool isReleasePart = (bool)CurrentSession.GetValue(ExtendSession.SessionKeys.ReleasePart);
                CurrentSession.AddValue(ExtendSession.SessionKeys.PartType, "");
                if (isReleaseMB) //MB
                {
                    CurrentSession.AddValue(Session.SessionKeys.OldMB, product.PCBID);

                    product.PCBID = "";
                    defect.PartType = "MB";
                    product.PCBModel = null;
                    product.MAC = null;
                    product.UUID = null;
                    product.MBECR = null;
                    product.CVSN = null;
                    //解綁MB要同時解綁 BATT
                    //20110615 只要有换MB/LCM/Battery三者中任意一个，都需要把机器原来结合的MB/LCM/Battery解掉，然后重BoardInput(40)站重流
                    foreach (ProductPart part in productParts)
                    {
                        //if (part.Value.Trim() == defect.OldPartSno.Trim())
                       // {
                            //CurrentSession.AddValue(ExtendSession.SessionKeys.PartType, part.PartType);
                            /*if (part.PartType.Trim() == "BATT")
                            {
                                product.RemovePart(part.Value, part.PartID);
                                break;
                            }*/
                        //}
                        product.RemovePart(part.Value, part.PartID); // Dean 20110715 解綁MB要同時解綁 BATT/LCM1/LCM2
                    }
                    
                    CurrentSession.AddValue(ExtendSession.SessionKeys.PartType, "MB");
                }
                else if (isReleasePart)
                {
                    // Dean 20110715 解綁LCM1/LCM2/BATT 要同時解綁MB，並且還是更新PCBStatus=30                    
                    var mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                    var mb = mbRepository.Find(product.PCBID);

                    if (mb == null)
                    {
                        var ex = new FisException("SFC001", new string[] { mb.Sn });
                        throw ex;
                    }

                    var PCBStatus = new MBStatus(mb.Sn, "30", MBStatusEnum.Pass, this.Editor, mb.MBStatus.Line, mb.MBStatus.Cdt, DateTime.Now);
                    mb.MBStatus = PCBStatus;
                    mbRepository.Update(mb, CurrentSession.UnitOfWork);
                    
                    var mbLog = new MBLog(
                        0,
                        mb.Sn,
                        mb.Model,
                        "30",
                        1,
                        mb.MBStatus.Line,
                        this.Editor,
                        DateTime.Now);

                    mb.AddLog(mbLog);
                    mbRepository.Update(mb, CurrentSession.UnitOfWork);

                    product.PCBID = "";                    
                    product.PCBModel = null;
                    product.MAC = null;
                    product.UUID = null;
                    product.MBECR = null;
                    product.CVSN = null;
                    
                    foreach (ProductPart part in productParts)
                    {
                      if (part.Value.Trim() == defect.OldPartSno.Trim())
                      {
                          CurrentSession.AddValue(ExtendSession.SessionKeys.PartType, part.PartType);                          
                          //if (part.PartType != "MB" && part.PartType != "")
                          /*if (part.PartType.Trim() == "LCM1" || part.PartType.Trim() == "LCM2")
                          {
                             bLCMType = true; 
                          }
                          else if (part.PartType.Trim() == "BATT")//先解到BATT本身的
                          {
                              product.RemovePart(part.Value, part.PartID);
                          }
                          break;*/
                          foreach (ProductPart part1 in productParts)
                          {
                              product.RemovePart(part1.Value, part1.PartID);// Dean 20110715 解綁LCM1/LCM2要同時解綁 BATT/LCM1/LCM2
                          }
                          break;
                      }
                    }

                    //remove LCM1 & LCM2
                    /*if (bLCMType)
                    {
                        foreach (ProductPart part in productParts)
                        {
                            //if (part.PartType === "LCM1" || part.PartType == "LCM2")//若是LCM1/LCM2/BATT，要同時解綁
                            //解綁LCM1或LCM2要同時解綁LCM1/LCM2/BATT(若是LCM1/LCM2/BATT，要同時解綁)
                            if (part.PartType.Trim() != "MB" && part.PartType != "")
                            {
                                product.RemovePart(part.Value, part.PartID);
                            }                           
                        }
                    }*/
                }

                #region original 20110527
                //if (defect.OldPartSno.Trim() != "") //MB/LCM1/LCM2
                //{                    
                //    //MB 
                //    if (defect.OldPart.Trim() != "" && product.PCBModel.Trim() == defect.OldPart.Trim() ||
                //         defect.OldPartSno.Trim() != "" && product.PCBID.Trim() == defect.OldPartSno.Trim())
                //    {
                //        //Dean 20110516 即當Faulty Part NO ,Faulty Part Sn 和Product_Part中的數據不匹配
                //        if (defect.OldPart.Trim() != "" && defect.OldPartSno.Trim() != "")
                //        {
                //            if (product.PCBModel == defect.OldPart.Trim() &&
                //                    product.PCBID.Trim() == defect.OldPartSno.Trim())
                //            {
                //                CurrentSession.AddValue(Session.SessionKeys.OldMB, product.PCBID);

                //                product.PCBID = "";
                //                defect.PartType = "MB";
                //                product.PCBModel = null;
                //                product.MAC = null;
                //                product.UUID = null;
                //                product.MBECR = null;
                //                product.CVSN = null;
                //                CurrentSession.AddValue(ExtendSession.SessionKeys.PartType, "MB");                                
                //                //bSearch = true;
                //            }
                //            else//兩個都有欄，但不匹配的ErrorMessage
                //            {
                //                List<string> errpara = new List<string>();

                //                errpara.Add(defect.OldPartSno);
                //                FisException e = new FisException("MAT010", errpara);
                //                e.stopWF = false;
                //                throw e;                                
                //            }
                //        }
                //        else //only sn check 
                //        {
                //            CurrentSession.AddValue(Session.SessionKeys.OldMB, product.PCBID);

                //            product.PCBID = "";
                //            defect.PartType = "MB";
                //            product.PCBModel = null;
                //            product.MAC = null;
                //            product.UUID = null;
                //            product.MBECR = null;
                //            product.CVSN = null;
                //            CurrentSession.AddValue(ExtendSession.SessionKeys.PartType, "MB");
                //            //bSearch = true;
                //        }
                //    }                    
                //    else//LCM1/LCM2
                //    {                                                
                //        foreach (ProductPart part in productParts)
                //        {
                //            if (defect.OldPart.Trim() != "" && part.PartID.Trim() == defect.OldPart.Trim() ||
                //                defect.OldPartSno.Trim() != "" && part.Value.Trim() == defect.OldPartSno.Trim())
                //            {
                //                //Dean 20110516 即當Faulty Part NO ,Faulty Part Sn 和Product_Part中的數據不匹配
                //                if (defect.OldPart.Trim() != "" && defect.OldPartSno.Trim() != "")
                //                {                                    
                //                    if (part.PartID == defect.OldPart.Trim() &&
                //                        part.Value.Trim() == defect.OldPartSno.Trim())
                //                    {                                                                                                                    
                //                        //product.RemovePart(part.Value, part.PartID);
                //                        defect.PartType = part.PartType;
                //                        CurrentSession.AddValue(ExtendSession.SessionKeys.PartType, part.PartType);

                //                        if (part.PartType == "LCM1" || part.PartType=="LCM2")
                //                        {
                //                            bLCMType = true; 
                //                        }
                //                        else
                //                        {
                //                            product.RemovePart(part.Value, part.PartID);
                //                        }
                //                        //if (!RemoveLCM1 && part.PartType == "LCM1") RemoveLCM1 = true;
                //                        //if (!RemoveLCM2 && part.PartType == "LCM2") RemoveLCM2 = true; 
                //                        bSearch = true;
                //                        break;
                //                    }
                //                    else //兩個都有欄，但不匹配的ErrorMessage
                //                    {
                //                        List<string> errpara = new List<string>();

                //                        errpara.Add(defect.OldPartSno);

                //                        FisException e = new FisException("MAT010", errpara);
                //                        e.stopWF = false;
                //                        throw e;
                //                    }
                //                }
                //                else //only sn check 
                //                {                                    
                //                    //product.RemovePart(part.Value, part.PartID);
                //                    defect.PartType = part.PartType;
                //                    CurrentSession.AddValue(ExtendSession.SessionKeys.PartType, part.PartType);
                //                    if (part.PartType == "LCM1" || part.PartType == "LCM2")
                //                    {
                //                        bLCMType = true;
                //                    }
                //                    else
                //                    {
                //                        product.RemovePart(part.Value, part.PartID);
                //                    }

                //                    bSearch = true;
                //                    //if (!RemoveLCM1 && part.PartType == "LCM1") RemoveLCM1 = true;
                //                    //if (!RemoveLCM2 && part.PartType == "LCM2") RemoveLCM2 = true; 
                //                    break;
                //                }
                //            }
                //        }

                //        if (!bSearch)
                //        {
                //            List<string> errpara = new List<string>();

                //            errpara.Add(defect.OldPart);


                //            FisException e = new FisException("CHK027", errpara);
                //            e.stopWF = false;
                //            throw e;
                //            //throw new FisException("CHK027", errpara);
                //        }
                //        else if (bLCMType)
                //        {
                //            foreach (ProductPart part in productParts)
                //            {                                
                //                if (part.PartType == "LCM1" || part.PartType == "LCM2")
                //                {
                //                    product.RemovePart(part.Value, part.PartID);
                //                }
                //                /*if (RemoveLCM1 || RemoveLCM2)
                //                {
                //                    if ((RemoveLCM1  && part.PartType == "LCM2") ||
                //                        (RemoveLCM2 & part.PartType == "LCM1"))
                //                    {
                //                        product.RemovePart(part.Value, part.PartID);
                //                    }
                //                }*/
                //            }
                //        }


                //    }
                //    #region Remark
                //    /*if (!bSearch)
                //    {
                //        List<string> errpara = new List<string>();

                //        errpara.Add(defect.OldPart);

                //        FisException e = new FisException("CHK027", errpara);
                //        e.stopWF = false;
                //        throw e;
                //        //throw new FisException("CHK027", errpara);
                //    }
                //   else
                //    {
                //        foreach (ProductPart part in productParts)
                //        {
                //            if (part.PartType == "LCM1" || part.PartType == "LCM2")
                //            {
                //                product.RemovePart(part.Value, part.PartID);
                //            }
                //        }
                //    }*/
                //#endregion
                //}
               
                //oldPart=partRep.GetPartByPartNo(defect.OldPartSno);                                                                         

                //if (oldPart == null)//Dean 20110427
                //{
                //    List<string> errpara = new List<string>();

                //    errpara.Add(defect.OldPart);

                //    throw new FisException("CHK027", errpara);

                //}

                //if (string.Compare(FaRepairStation, Station, true) == 0)
                //{
                //    if (string.Compare(oldPart.Type, "MB", true) == 0)
                //    {
                //        IMBRepository imr = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                //        IProduct tempProduct = ((IProduct)repairTarget);

                //        //update Product table
                //        tempProduct.PCBID = null;
                //        //PCBModel, MAC, UUID, MBECR, CVSN 2010-03
                //        tempProduct.PCBModel = null;
                //        tempProduct.MAC = null;
                //        tempProduct.UUID = null;
                //        tempProduct.MBECR = null;
                //        tempProduct.CVSN = null;
                //    }
                //    else if (string.Compare(oldPart.Type, "LCM1", true) == 0 || string.Compare(oldPart.Type, "LCM2", true) == 0)
                //    {
                //        List<string> partTypeList = new List<string> {"LCM1", "LCM2"};
                //        productRep.DeleteProductPartByPartTypeDefered(CurrentSession.UnitOfWork, ((IProduct)repairTarget).ProId, partTypeList);
                //    }
                //}
                #endregion original 20110527
                if (isReleaseMB ||isReleasePart)
                    productRep.Update(product, CurrentSession.UnitOfWork);
                return base.DoExecute(executionContext);
            }
            catch (FisException ex)
            {
                throw;
            }
        }

        #endregion
    }
}

