// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据productId,station获取UnitWeightProductLog对象
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-16   Chen Xu  itc208014           create
// Known issues:
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
using IMES.FisObject.Common.PartSn;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject.Common.MO;
using IMES.FisObject.FA.Product;
using System.Collections.Generic;
using IMES.FisObject.Common.Station;
using IMES.FisObject.PAK.DN;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using IMES.Common;

namespace IMES.Activity
{
    /// <summary>
    ///   根据productId,station获取UnitWeightProductLog对象
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于检查是否有UnitWeight过站log
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.参考UC;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProductRepository
    ///         IProduct
    /// </para> 
    /// </remarks>
    public partial class CheckUnitWeightPrintLabel : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckUnitWeightPrintLabel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 根据productId,station获取UnitWeightProductLog对象
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            FisException ex;
            List<string> erpara = new List<string>();

            IProductRepository iproductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            CurrentSession.AddValue(Session.SessionKeys.isPassStationLog, false);


            // Print =====如果为BT 产品，当满足如下条件时，需要列印Config Label=====
            
            CurrentSession.AddValue(Session.SessionKeys.labelBranch, "");
            
            IDeliveryRepository iDeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            var delievery = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
            
            //Revision: 9810:	修改列印Config Label 的条件为非BT
            if (!currentProduct.IsBT)
            {
                string BTRegId = iDeliveryRepository.GetDeliveryInfoValue(delievery, "RegId");
                if (BTRegId != null && BTRegId.Length == 3)
                { BTRegId = BTRegId.Substring(1, 2); }
                else
                { BTRegId = ""; }
                string BTShipTp = iDeliveryRepository.GetDeliveryInfoValue(delievery, "ShipTp");
                string BTCountry = iDeliveryRepository.GetDeliveryInfoValue(delievery, "Country");

                //ITC-1360-1517: 取非
                if (!string.IsNullOrEmpty(BTRegId) && !string.IsNullOrEmpty(BTShipTp) && !string.IsNullOrEmpty(BTCountry))
                {
                   // if ((BTRegId == "SCN" || BTRegId == "SAF" || BTRegId == "SNE" || BTRegId == "SCE") && (BTShipTp == "CTO") && (BTCountry != "JAPAN"))
                   // if ((BTRegId == "CN" || BTRegId == "AF" || BTRegId == "NE" || BTRegId == "CE") && (BTShipTp == "CTO") && (BTCountry != "JAPAN"))
                    if ((ActivityCommonImpl.Instance.CheckDomesticDN(BTRegId) || BTRegId == "AF" || BTRegId == "NE" || BTRegId == "CE") && (BTShipTp == "CTO") && (BTCountry != "JAPAN"))
                   {
                        CurrentSession.AddValue(Session.SessionKeys.labelBranch, "C");
                    }    
                }
            }

            //  Print =====如果IMES_PAK..PODLabelPart中有维护PartNo等于Model的前几位字符的记录 and (Model的第7位不是数字)，则需要列印POD Label====
            IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<PODLabelPartDef> podLabelPartLst = new List<PODLabelPartDef>();

            //ITC-1360-1530: 
            //var palletno = (string)CurrentSession.GetValue(Session.SessionKeys.PalletNo);
            /*
            ******** Marked bt Benson at 20130522
             podLabelPartLst = ipartRepository.GetPODLabelPartListByPartNo(currentProduct.Model);
             if (podLabelPartLst.Count > 0)
             {
                 string number = "0123456789";
                 string modelbit = currentProduct.Model.Substring(6, 1);
                 if (!number.Contains(modelbit))
                 {
                     if (CurrentSession.GetValue(Session.SessionKeys.labelBranch).ToString() == "C")
                     {
                         CurrentSession.AddValue(Session.SessionKeys.labelBranch, "CP");
                     }
                     else
                     {
                         CurrentSession.AddValue(Session.SessionKeys.labelBranch, "P");
                     }
                 }
             }
             ******** Marked bt Benson at 20130522
             */
            if (CheckPOD())
            {
                if (CurrentSession.GetValue(Session.SessionKeys.labelBranch).ToString() == "C")
                {
                    CurrentSession.AddValue(Session.SessionKeys.labelBranch, "CP");
                }
                else
                {
                    CurrentSession.AddValue(Session.SessionKeys.labelBranch, "P");
                }
            }

            if (CurrentSession.GetValue(Session.SessionKeys.labelBranch).ToString()=="")
            {
                 erpara.Add(currentProduct.CUSTSN);
                 ex = new FisException("SFC017", erpara);    //该CustomerSN不需要列印Config Label 和POD Label!
                 throw ex;
            }


            return base.DoExecute(executionContext);
        }



        private bool CheckPOD()
        {
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            string site =(string)CurrentSession.GetValue("Site");
            string model = currentProduct.Model.Trim();
            string number = "0123456789";
            string modelbit = model.Substring(6, 1);
            string delievery = currentProduct.DeliveryNo.Trim();
            IList<PODLabelPartDef> podLabelPartLst = new List<PODLabelPartDef>();
            podLabelPartLst = PartRepository.GetPODLabelPartListByPartNo(model);
            bool isPOD = false;
            if (site == "ICC")
            {

                //如果IMES_PAK..PODLabelPart中有维护PartNo等于Model的前几位字符的记录 and (Model的第7位不是数字)，则需要列印POD Label====
               IList<string> valueList =
                       PartRepository.GetConstValueTypeList("ExceptPODModel").Select(x => x.value).Where(y => y != "").Distinct().ToList();
                if (!number.Contains(modelbit) && podLabelPartLst.Count > 0 && !valueList.Contains(model))
                { isPOD = true; }

            }
            else
            {
                if (podLabelPartLst.Count > 0 && !number.Contains(modelbit))
                {
                    isPOD = true; 
                }

            }
            return isPOD;
        
        
        }

    }
}
