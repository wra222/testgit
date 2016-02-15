// INVENTEC corporation (c)2012 all rights reserved. 
// Description:
//      1.检查[Model1]是否存在未进包装（不存在69的成功过站Log）且已经产生CUSTSN（Product.CUSTSN<>''）的Product记录，若不存在记录，则报错：“Model：XXX不存在可以转换的Product
//      2.获取[Current Station]，Station倒序排列
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-06-18   Kerwin                       create
// Known issues:
using System.Collections.Generic;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Model;

namespace IMES.Activity
{

    /// <summary>
    /// 检查Model1,Model2是否满足ChangeModel条件
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-FA-UC Change Model
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///     1.检查[Model1]是否存在未进包装（不存在69的成功过站Log）且已经产生CUSTSN（Product.CUSTSN!=''）的Product记录，若不存在记录，则报错：“Model：XXX不存在可以转换的Product
    ///     2.获取[Current Station]，Station倒序排列
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     CHK155
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
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         MB
    ///         
    /// </para> 
    /// </remarks>
    public partial class VGAMBCheck : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public VGAMBCheck()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查Model1,Model2是否满足ChangeModel条件
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string[] ChangeModelCheckItem = CurrentSession.GetValue(Session.SessionKeys.ChangeModelCheckItem) as string[];
            string model1 = CurrentSession.GetValue(Session.SessionKeys.Model1) as string;
            string model2 = CurrentSession.GetValue(Session.SessionKeys.Model2) as string;
            string SelectStation = CurrentSession.GetValue(Session.SessionKeys.SelectStation) as string;
            int changeQty = (int)CurrentSession.GetValue(Session.SessionKeys.Qty);
            IProductRepository MyProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
            IList<string> ProductIDList = MyProductRepository.GetProductIdsByModelAndStation(model1, SelectStation, changeQty);

            if (ProductIDList != null && ProductIDList.Count == changeQty)
            {
                string productIDStr = "";
                foreach (string temp in ProductIDList)
                {
                    productIDStr = productIDStr + temp + "-";
                }
                if (productIDStr.EndsWith("-"))
                {
                    productIDStr = productIDStr.Substring(0, productIDStr.Length - 1);
                }
                CurrentSession.AddValue(Session.SessionKeys.ProductIDListStr, productIDStr);
            }

            if (ChangeModelCheckItem != null && (ChangeModelCheckItem.Contains("VGA") || ChangeModelCheckItem.Contains("MB")))
            {

                if (ProductIDList == null || ProductIDList.Count != changeQty)
                {
                    throw new FisException("CHM006", new string[] { model1, SelectStation, changeQty.ToString() });
                }

                IBOMRepository MyBomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();

                if (ChangeModelCheckItem.Contains("VGA"))
                {
                    IList<string> VGAMBCodeList = MyProductRepository.GetInfoValuePrefixFromProductInfo(ProductIDList.ToArray<string>(), "VGA");
                    IList<string> Model2MBCodeList = MyBomRepository.GetPartInfoValueListByModelAndBomNodeTypeAndInfoTypes(model2, "MB", "VGA", "SV", "MB");
                    
                    foreach (string mbcode in VGAMBCodeList)
                    {
                        if (!Model2MBCodeList.Contains(mbcode))
                        {
                            throw new FisException("CHM004", new string[] { });
                        }
                    }

                }

                if (ChangeModelCheckItem.Contains("MB"))
                {
                    IList<string> MBMBCodeList = MyProductRepository.GetPartSnPrefixFromProductPart(ProductIDList.ToArray<string>(), "MB");

                    IList<string> Model2MBMBCodeList = MyBomRepository.GetPartInfoValueListByModelAndBomNodeTypeAndInfoType(model2, "MB", "MB");
                    if (Model2MBMBCodeList == null || Model2MBMBCodeList.Count == 0)
                    {
                        Model2MBMBCodeList = new List<string>();
                        var repository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, IMES.FisObject.Common.Model.Model>();
                        Model model2Obj = repository.Find(model2);
                        Model2MBMBCodeList.Add(model2Obj.GetAttribute("MB"));
                        Model2MBMBCodeList.Add(model2Obj.GetAttribute("MB1"));
                        Model2MBMBCodeList.Add(model2Obj.GetAttribute("MB2"));
                        Model2MBMBCodeList.Add(model2Obj.GetAttribute("MB3"));
                    }
                    foreach (string mbcode in MBMBCodeList)
                    {
                        if (!Model2MBMBCodeList.Contains(mbcode))
                        {
                            throw new FisException("CHM005", new string[] { });
                        }
                    }
                }


            }

            return base.DoExecute(executionContext);
        }

    }
}

