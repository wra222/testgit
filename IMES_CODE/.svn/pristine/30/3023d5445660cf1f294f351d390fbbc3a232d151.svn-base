// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  
// CI-MES12-SPEC-PAK-Combine DN & Pallet for BT.docx                     
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-5-7   207003         create
// Known issues:
using System;
using System.Data;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;
using System.Collections.Generic;
using System.ComponentModel;

namespace IMES.Activity
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         Delivery 分配
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
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
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              productId
    /// </para> 
    /// </remarks>
    public partial class UpdateBTLoc : BaseActivity
    {
        /// <summary>
        /// constructor
        /// </summary>
        public UpdateBTLoc()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 单条还是成批插入
        /// </summary>
        public static DependencyProperty IsSingleProperty = DependencyProperty.Register("IsSingle", typeof(bool), typeof(UpdateBTLoc), new PropertyMetadata(true));

        /// <summary>
        /// 单条还是成批插入,Session.SessionKeys.ProdList
        /// </summary>
        [DescriptionAttribute("IsSingle")]
        [CategoryAttribute("IsSingle Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsSingle
        {
            get
            {
                return ((bool)(base.GetValue(IsSingleProperty)));
            }
            set
            {
                base.SetValue(IsSingleProperty, value);
            }
        }

        /// <summary>
        /// Delivery 分配原则
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            IPalletRepository CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
             IList<IProduct> prodList =null;
             if (IsSingle)
             {
                 Product curProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
                 prodList = new List<IProduct>();
                 prodList.Add(curProduct);
             }
             else
             {
                prodList = CurrentSession.GetValue(Session.SessionKeys.ProdList) as IList<IProduct>;
             }

             foreach (IProduct prod in prodList)
             {
                 updatePakBtLocMas(CurrentRepository, prod.ProId);
             }

            #region vincent disable code
            //SnoDetBtLocInfo condition = new SnoDetBtLocInfo();
            //condition.snoId = curProduct.ProId;
            //IList<string> listSno = CurrentRepository.GetSnoListFromSnoDetBtLoc(condition);


            //SnoDetBtLocInfo setValue = new SnoDetBtLocInfo();
            //setValue.status = "Out";
            //setValue.editor = this.Editor;
            //setValue.udt = DateTime.Now;
            //SnoDetBtLocInfo setCondition = new SnoDetBtLocInfo();
            //setCondition.snoId = curProduct.ProId;
            ////CurrentRepository.UpdateSnoDetBtLoc(setValue, setCondition);
            //CurrentRepository.UpdateSnoDetBtLocDefered(CurrentSession.UnitOfWork, setValue, setCondition);
            
            //foreach (string tmp in listSno)
            //{

            //    SnoDetBtLocInfo conditionTemp = new SnoDetBtLocInfo();
            //    conditionTemp.sno = tmp;
            //    conditionTemp.status = "In";

            //    int cmbqty = CurrentRepository.GetCountOfSnoIdFromSnoDetBtLoc(conditionTemp);


            //    PakBtLocMasInfo setPakValue2 = new PakBtLocMasInfo();
            //    PakBtLocMasInfo setPakCondition2 = new PakBtLocMasInfo();
            //    setPakValue2.cmbQty = cmbqty;
            //    if (cmbqty == 0)
            //    {
            //        setPakValue2.status = "Open"; //release location
            //        setPakValue2.model = "Other";
            //    }
            //    setPakCondition2.snoId = tmp;
            //    //CurrentRepository.UpdatePakBtLocMas(setPakValue2, setPakCondition2);
            //      CurrentRepository.UpdatePakBtLocMasDefered(CurrentSession.UnitOfWork, setPakValue2, setPakCondition2);
            //    //Vincent 2014-10-17 disable code
            //    //if (cmbqty == 0)
            //    //{
            //    //    PakBtLocMasInfo setPakValue = new PakBtLocMasInfo();
            //    //    PakBtLocMasInfo setPakCondition = new PakBtLocMasInfo();
            //    //    setPakValue.status = "Open"; //release location
            //    //    setPakValue.model = "Other";
            //    //    setPakCondition.snoId = tmp;
            //    //    //CurrentRepository.UpdatePakBtLocMas(setPakValue, setPakCondition);
            //    //    CurrentRepository.UpdatePakBtLocMasDefered(CurrentSession.UnitOfWork, setPakValue, setPakCondition);
            //    //}

            //}
            #endregion
            return base.DoExecute(executionContext);
        }


        private void updatePakBtLocMas(IPalletRepository palletRep, string productId)
        {
           

            SnoDetBtLocInfo condition = new SnoDetBtLocInfo();
            condition.snoId = productId;
            IList<string> listSno = palletRep.GetSnoListFromSnoDetBtLoc(condition);
            if (listSno != null && listSno.Count > 0)
            {
                SnoDetBtLocInfo setValue = new SnoDetBtLocInfo();
                setValue.status = "Out";
                setValue.editor = this.Editor;
                setValue.udt = DateTime.Now;
                SnoDetBtLocInfo setCondition = new SnoDetBtLocInfo();
                setCondition.snoId = productId;
                palletRep.UpdateSnoDetBtLocDefered(CurrentSession.UnitOfWork, setValue, setCondition);

                foreach (string tmp in listSno)
                {

                    SnoDetBtLocInfo conditionTemp = new SnoDetBtLocInfo();
                    conditionTemp.sno = tmp;
                    conditionTemp.status = "In";

                    int cmbqty = palletRep.GetCountOfSnoIdFromSnoDetBtLoc(conditionTemp);


                    PakBtLocMasInfo setPakValue2 = new PakBtLocMasInfo();
                    PakBtLocMasInfo setPakCondition2 = new PakBtLocMasInfo();
                    setPakValue2.cmbQty = cmbqty;
                    if (cmbqty == 0)
                    {
                        setPakValue2.status = "Open"; //release location
                        setPakValue2.model = "Other";
                    }
                    setPakCondition2.snoId = tmp;
                    palletRep.UpdatePakBtLocMasDefered(CurrentSession.UnitOfWork, setPakValue2, setPakCondition2);
                }
            }
        }


    }

}
