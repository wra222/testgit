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
        /// Delivery 分配原则
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            IPalletRepository CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            Product curProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            SnoDetBtLocInfo condition = new SnoDetBtLocInfo();
            condition.snoId = curProduct.ProId;
            IList<string> listSno = CurrentRepository.GetSnoListFromSnoDetBtLoc(condition);


            SnoDetBtLocInfo setValue = new SnoDetBtLocInfo();
            setValue.status = "Out";
            setValue.editor = this.Editor;
            setValue.udt = DateTime.Now;
            SnoDetBtLocInfo setCondition = new SnoDetBtLocInfo();
            setCondition.snoId = curProduct.ProId;
            //CurrentRepository.UpdateSnoDetBtLoc(setValue, setCondition);
            CurrentRepository.UpdateSnoDetBtLocDefered(CurrentSession.UnitOfWork, setValue, setCondition);
            
            foreach (string tmp in listSno)
            {

                SnoDetBtLocInfo conditionTemp = new SnoDetBtLocInfo();
                conditionTemp.sno = tmp;
                conditionTemp.status = "In";

                int cmbqty = CurrentRepository.GetCountOfSnoIdFromSnoDetBtLoc(conditionTemp);


                PakBtLocMasInfo setPakValue2 = new PakBtLocMasInfo();
                PakBtLocMasInfo setPakCondition2 = new PakBtLocMasInfo();
                setPakValue2.cmbQty = cmbqty;
                setPakCondition2.snoId = tmp;
                //CurrentRepository.UpdatePakBtLocMas(setPakValue2, setPakCondition2);
                  CurrentRepository.UpdatePakBtLocMasDefered(CurrentSession.UnitOfWork, setPakValue2, setPakCondition2);
                if (cmbqty == 0)
                {
                    PakBtLocMasInfo setPakValue = new PakBtLocMasInfo();
                    PakBtLocMasInfo setPakCondition = new PakBtLocMasInfo();
                    setPakValue.status = "OPEN";
                    setPakValue.model = "Other";
                    setPakCondition.snoId = tmp;
                    //CurrentRepository.UpdatePakBtLocMas(setPakValue, setPakCondition);
                    CurrentRepository.UpdatePakBtLocMasDefered(CurrentSession.UnitOfWork, setPakValue, setPakCondition);
                }

            }
            return base.DoExecute(executionContext);
        }
    }

}
