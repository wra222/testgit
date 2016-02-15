// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 根据Session中保存的MB生成子板序号
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-12-31   Yuan XiaoWei                 create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

using IMES.Infrastructure.Utility.Generates;
using IMES.Infrastructure.Utility.Generates.intf;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure.Utility.Generates.impl;
using IMES.FisObject.PCA.MBMO;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Model;
using IMES.DataModel;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    /// 根据Session中保存的MB生成子板序号
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于ICT INPUT,PrintComMb
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.获取母版MB序号
    ///         2.获取产生子板数量;
    ///         3.产生子板序号(母版序号和子板序号仅第6位不同。母版第六位为8，子板第六位为1，3，5，7);
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MB
    ///         Session.Qty
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.MBNOList
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBRepository
    /// </para> 
    /// </remarks>
    public partial class GetFamilyInfo : BaseActivity
    {
        /// <summary>
        /// 当前流程的类型，共有两种，MB,Product
        /// </summary>
        public static DependencyProperty InputTypeProperty = DependencyProperty.Register("InputType", typeof(InputTypeEnum), typeof(GetFamilyInfo)
            , new PropertyMetadata(InputTypeEnum.MB));

        /// <summary>
        /// 当前流程的类型，共有两种，MB,Product
        /// </summary>
        [DescriptionAttribute("InputType")]
        [CategoryAttribute("InArguments Of CheckIsGenMAC")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public InputTypeEnum InputType
        {
            get
            {
                return ((InputTypeEnum)(base.GetValue(GetFamilyInfo.InputTypeProperty)));
            }
            set
            {
                base.SetValue(GetFamilyInfo.InputTypeProperty, value);
            }
        }

        /// <summary>
        ///  FamilyInfoName
        /// </summary>
        public static DependencyProperty InfoNameProperty = DependencyProperty.Register("InfoName", typeof(string), typeof(GetFamilyInfo)
            , new PropertyMetadata("IsAssignMac"));
        
        /// <summary>
        /// FamilyInfoName
        /// </summary>
        [DescriptionAttribute("InfoName")]
        [CategoryAttribute("InArguments Of CheckIsGenMAC")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        //[DefaultValue("")]
        public string InfoName
        {
            get
            {
                return ((string)(base.GetValue(GetFamilyInfo.InfoNameProperty)));
            }
            set
            {
                base.SetValue(GetFamilyInfo.InfoNameProperty, value);
            }
        }
        
        /// <summary>
        ///  FamilyDefaultValue
        /// </summary>
        public static DependencyProperty InfoDefaultValueProperty = DependencyProperty.Register("InfoDefaultValue", typeof(string), typeof(GetFamilyInfo)
            , new PropertyMetadata("Y"));

        /// <summary>
        /// FamilyInfoName
        /// </summary>
        [DescriptionAttribute("InfoDefaultValue")]
        [CategoryAttribute("InArguments Of CheckIsGenMAC")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string InfoDefaultValue
        {
            get
            {
                return ((string)(base.GetValue(GetFamilyInfo.InfoDefaultValueProperty)));
            }
            set
            {
                base.SetValue(GetFamilyInfo.InfoDefaultValueProperty, value);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public GetFamilyInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 根据母版号码和数量生成子板号码
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //string ss = this.InfoName;
            string FamilyInfoName = InfoName;
            string FamilyInfoValue = InfoDefaultValue;
            string currentFamily = "";
            //bool IsGenMAC = true;
            try
            {
                switch (InputType)
                {
                    case InputTypeEnum.MB:
                        //IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                        MB currentMB = (MB)CurrentSession.GetValue(Session.SessionKeys.MB);
                        if (currentMB == null)
                        {
                            throw new FisException("SFC001", new string[] { "MB" });
                        }
                        //string PCBNo = currentMB.PCBModelID.ToString();
                        //IPart currentPart = iPartRepository.GetPartByPartNo(PCBNo);
                        //currentFamily = currentPart.Descr.ToString();
                        currentFamily = currentMB.Family;
                        if (string.IsNullOrEmpty(currentFamily))
                        {
                            throw new FisException("SFC001", new string[] { "Family" });
                        }
                        break;
                    case InputTypeEnum.Product:
                        //IModelRepository iModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
                        Product currentProduct = ((Product)CurrentSession.GetValue(Session.SessionKeys.Product));
                        if (currentProduct == null)
                        {
                            throw new FisException("SFC002", new string[] { "Product" });
                        }
                        //string currentModel = currentProduct.Model;
                        //Model Model = iModelRepository.Find(currentModel);
                        //currentFamily = Model.FamilyName.ToString();
                        currentFamily = currentProduct.Family;
                        if (string.IsNullOrEmpty(currentFamily))
                        {
                            throw new FisException("SFC002", new string[] { "Family" });
                        }
                        break;
                    default:
                        break;
                }
                IFamilyRepository iFamilyRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository, Family>();
                FamilyInfoDef condition = new FamilyInfoDef();
                condition.name = FamilyInfoName;
                condition.family = currentFamily;

                IList<FamilyInfoDef> FamilyInfoLst = iFamilyRepository.GetExistFamilyInfo(condition);
                if (FamilyInfoLst == null || FamilyInfoLst.Count == 0)
                {
                    throw new FisException("CHK1024", new string[] { });
                }
                foreach (FamilyInfoDef item in FamilyInfoLst)
                {
                    FamilyInfoValue = item.value;
                }
                
                CurrentSession.AddValue(FamilyInfoName, FamilyInfoValue);
                //IFamilyRepository iFamilyRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
                //IList<FamilyInfoDef> FamilyInfoLst  = iFamilyRepository.GeFamilyInfoByName(currentFamily, FamilyInfoName);
                

            }
            catch (FisException fe)
            {
                throw fe;
            }
            
            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// 当前流程的类型，共有两种，MB,Product
        /// </summary>
        public enum InputTypeEnum
        {
            /// <summary>
            /// MB
            /// </summary>
            MB = 1,
            /// <summary>
            /// Product
            /// </summary>
            Product = 2
        }
    }
}
