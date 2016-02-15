// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Check if product model is RCTO model
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-08-21   itc202017                      create
// Known issues:
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.PAK.DN;
using System.ComponentModel;
using IMES.DataModel;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using IMES.Common;
using IMES.Infrastructure.Extend.Dictionary;
namespace IMES.Activity
{
    /// <summary>
    /// Check if product model is RCTO model
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      OQC output
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         	若Model的前3码非’146’或’173’则抛出异常
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Product
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
    ///         none
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              
    /// </para> 
    /// </remarks>
    public partial class BlockForRCTOModel : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BlockForRCTOModel()
        {
            InitializeComponent();
        }
        /// <summary>
        /// ProcReg Name
        /// </summary>
        public static DependencyProperty ProcRegNameProperty = DependencyProperty.Register("ProcRegName", typeof(string), typeof(BlockForRCTOModel), new PropertyMetadata("RCTO"));
        /// <summary>
        ///  ProcReg Name
        /// </summary>
        [DescriptionAttribute("ProcRegName")]
        [CategoryAttribute("ProcRegName")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string ProcRegName
        {
            get
            {
                return ((string)(base.GetValue(BlockForRCTOModel.ProcRegNameProperty)));
            }
            set
            {
                base.SetValue(BlockForRCTOModel.ProcRegNameProperty, value);
            }
        }

        /// <summary>
        /// Check if product model is RCTO model
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            IList<ConstValueInfo> lstConst =  ActivityCommonImpl.Instance.GetConstValueListByType(ConstName.ConstValue.ProcReg, ConstName.Name);
            string model = currentProduct.Model.Trim();
            bool isPass = false;
            if (ProcRegName != string.Empty)
            {
                string[] regArr = ProcRegName.Split(',');
                foreach (String regName in regArr)
                {
                    var data = (from p in lstConst
                                where p.name.Trim() == regName
                                select p.value.Trim()).ToList();

                    if (data.Count == 0)
                    {
                        FisException e = new FisException("CHK990", new string[] { "ConstValue ", "ProcReg", "Name:" + regName });
                        throw e;
                    }
                    foreach (string regValue in data)
                    {
                        Regex regex = new Regex(regValue);
                        if (regex.IsMatch(model))
                        {
                                isPass = true;
                                break;
                        }

                    }

                }
            
            }
            if (!isPass)
            {
                throw new FisException("CHK528", new string[] {});
            }

            return base.DoExecute(executionContext);
        }
    }
}
