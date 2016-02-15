/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Activity/GenerateProductRecord
 * UI:CI-MES12-SPEC-FA-UI Travel Card Print.docx –2011/10/10 
 * UC:CI-MES12-SPEC-FA-UC Travel Card Print.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-10-15   zhanghe               (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* 1.//以下属性尚未提供
*   Exception= Exception#,
*   IsBT=@IsBT, 
*   ShipType=Model. ShipType（PC（整机）=0,FRU=1， RCTO（BaseModel，组装过的组件）=2，Option（组件）=3；从Model表得到）,
*   ModelType= ModelType# (  ModelType  CTO=1 BTO=2；从Model表机型12码第七码如为数字则为CTO,字母为BTO) ,
*   STAG=Model.STAG(STAG=S Master label上打印SN STAG=T就只打印Master label STAG is NULL 此机型不打印Master Label)
*   记录Product的Exception,Remark和BT Remark，在ProductInfo插入记录，其中InfoType=Remark; InfoType=BTRemark

* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
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
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;

namespace IMES.Activity
{
    /// <summary>
    /// 保存产生的product
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于Travel Card Print
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///             1.创建product对象
    ///             2.保存product对象
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.ProdMO
    ///         Session.ProdNoList
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
    ///         insert Product
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProductRepository
    /// </para> 
    /// </remarks>
    public partial class GenerateProductRecord : BaseActivity
    {
        /// <summary> 
        /// </summary>
        public GenerateProductRecord()
        {
            InitializeComponent();
        }

        /// <summary> 
        /// </summary>        
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var prodMo = (MO)CurrentSession.GetValue(Session.SessionKeys.ProdMO);
            var prodNoList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.ProdNoList);            
            var shipDate = CurrentSession.GetValue(IMES.Infrastructure.Extend.ExtendSession.SessionKeys.DeliveryDate).ToString();
            string bomremark = (string)CurrentSession.GetValue("BomRemark");
            string remark = (string)CurrentSession.GetValue("Remark");
            string exception = (string)CurrentSession.GetValue("Exception");
            string inFAI = (string)CurrentSession.GetValue("inFAI");
            var subProdList = new List<IProduct>();
            CurrentSession.AddValue(Session.SessionKeys.ProdList, subProdList);

            IProductRepository prodRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            //将所有的ProdId插入Product表，ID=ProdId#, MOID=mo#, ModelID=model#
            
            foreach (string item in prodNoList)
            {

                Product prod = new Product(item);

                prod.MO = prodMo.Key.ToString();
                prod.Model = prodMo.Model;
                ProductStatus prodStatus = new ProductStatus();
                prodStatus.Editor = this.Editor;
                prodStatus.Line = this.Line;
                prodStatus.StationId = this.Station;
                prodStatus.ProId = item;
                prodStatus.Status = StationStatus.Pass;
                prodStatus.ReworkCode = string.Empty;
                prod.Status = prodStatus;
                //ITC-1360-1337 ITC-1360-1300
                prod.SetExtendedProperty("ShipDate", shipDate, this.Editor);
                if(!String.IsNullOrEmpty(bomremark))
                    prod.SetExtendedProperty("BomRemark", bomremark.ToUpper(), this.Editor);
                if(!String.IsNullOrEmpty(remark))
                    prod.SetExtendedProperty("Remark", remark.ToUpper(), this.Editor);
                if(!String.IsNullOrEmpty(exception))
                    prod.SetExtendedProperty("Exception", exception, this.Editor);
                if (inFAI == "Y")
                    prod.SetExtendedProperty("FAIinFA", "Y", this.Editor);
                prod.SetExtendedProperty("PdLine", this.Line, this.Editor);
                //TODO 1.
                prodRepository.Add(prod, CurrentSession.UnitOfWork);
                subProdList.Add(prod);
            }
            IList<string> range = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.ProdNoList);
            string mo = (string)CurrentSession.GetValue(Session.SessionKeys.MONO);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "PrdId");
            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, range[0]);            
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, range[range.Count-1]);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, mo);
                          
            return base.DoExecute(executionContext);
        }
    }
}