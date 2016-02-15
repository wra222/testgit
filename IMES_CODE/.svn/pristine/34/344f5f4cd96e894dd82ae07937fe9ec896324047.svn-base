// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据BegNo,EndNo,Name获取PrintLog对象
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-13   Chen Xu  itc208014           create
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
using IMES.FisObject.PAK.Pallet;
using System.Collections.Generic;
using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.PrintLog;

namespace IMES.Activity
{
    /// <summary>
    ///  根据BegNo,EndNo,Name获取PrintLog对象
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于检查是否有过站log
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
    ///         
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
    public partial class GetPrintLog : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetPrintLog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 当前BegNo描述 BegNo
        /// </summary>
        public static DependencyProperty BegNoProperty = DependencyProperty.Register("BegNo", typeof(String), typeof(GetPrintLog));

        /// <summary>
        /// BegNo
        /// </summary>
        [DescriptionAttribute("BegNo")]
        [CategoryAttribute("BegNo Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public String BegNo
        {
            get
            {
                return ((String)(base.GetValue(GetPrintLog.BegNoProperty)));
            }
            set
            {
                base.SetValue(GetPrintLog.BegNoProperty, value);
            }
        }

        /// <summary>
        /// 当前EndNo描述 EndNo
        /// </summary>
        public static DependencyProperty EndNoProperty = DependencyProperty.Register("EndNo", typeof(String), typeof(GetPrintLog));

        /// <summary>
        /// EndNo
        /// </summary>
        [DescriptionAttribute("EndNo")]
        [CategoryAttribute("EndNo Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public String EndNo
        {
            get
            {
                return ((String)(base.GetValue(GetPrintLog.EndNoProperty)));
            }
            set
            {
                base.SetValue(GetPrintLog.EndNoProperty, value);
            }
        }

        /// <summary>
        /// 当前Name描述 Name
        /// </summary>
        public static DependencyProperty PrintLogNameProperty = DependencyProperty.Register("PrintLogName", typeof(String), typeof(GetPrintLog));

        /// <summary>
        /// PrintLogName
        /// </summary>
        [DescriptionAttribute("PrintLogName")]
        [CategoryAttribute("PrintLogName Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public String PrintLogName
        {
            get
            {
                return ((String)(base.GetValue(GetPrintLog.PrintLogNameProperty)));
            }
            set
            {
                base.SetValue(GetPrintLog.PrintLogNameProperty, value);
            }
        }


        /// <summary>
        /// 当前序号类型描述 PrintNoType
        /// </summary>
        public static DependencyProperty PrintNoTypeProperty = DependencyProperty.Register("PrintNoType", typeof(TypeEnum), typeof(GetPrintLog));

        /// <summary>
        /// PrintNoType
        /// </summary>
        [DescriptionAttribute("PrintNoType")]
        [CategoryAttribute("PrintNoType Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public TypeEnum PrintNoType
        {
            get
            {
                return ((TypeEnum)(base.GetValue(GetPrintLog.PrintNoTypeProperty)));
            }
            set
            {
                base.SetValue(GetPrintLog.PrintNoTypeProperty, value);
            }
        }


        /// <summary>
        /// 当前PrintNoType的类型，共有两种，MB,Product,CustSN, Pallet
        /// </summary>
        public enum TypeEnum
        {
            /// <summary>
            /// MB
            /// </summary>
            MB = 1,
            
            /// <summary>
            /// Product
            /// </summary>
            Product = 2,

            /// <summary>
            /// CustSN
            /// </summary>
            CustSN = 3,


            /// <summary>
            /// Pallet
            /// </summary>
            Pallet = 4,


        }

        /// <summary>
        /// 根据BegNo,EndNo,Name获取PrintLog对象
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            FisException ex;
            List<string> erpara = new List<string>();
            
            CurrentSession.AddValue(Session.SessionKeys.isPassStationLog,false);

            var iPrintRepository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();
            IList<PrintLog> PrintLogList = new List<PrintLog>();

            if (string.IsNullOrEmpty(this.PrintLogName))
            {
                FisException fe = new FisException("PAK031", new string[] { });  //请输入PrintLogName！
                throw fe;
            }

            string key = "";
            switch (PrintNoType)
            {
                case TypeEnum.MB:
                    key = (string)CurrentSession.GetValue(Session.SessionKeys.MBSN);
                    break;
                case TypeEnum.Product:
                    Product currentProduct = ((Product)CurrentSession.GetValue(Session.SessionKeys.Product));
                    key = currentProduct.ProId;
                    break;
                case TypeEnum.CustSN:
                    key = (string)CurrentSession.GetValue(Session.SessionKeys.CustSN);
                    break;
                case TypeEnum.Pallet:
                    if (((Pallet)CurrentSession.GetValue(Session.SessionKeys.Pallet)) != null)
                    {
                        Pallet currentPallet = ((Pallet)CurrentSession.GetValue(Session.SessionKeys.Pallet));
                        key = currentPallet.PalletNo;
                    }
                    else key = (string)CurrentSession.GetValue(Session.SessionKeys.PalletNo);
                    break;
                default:
                    break;
            }

            Boolean exitFlag = false;
            if (!string.IsNullOrEmpty(this.PrintLogName) && !string.IsNullOrEmpty(this.BegNo) && !string.IsNullOrEmpty(this.EndNo))
            {
                PrintLog condition = new PrintLog();
                condition.BeginNo = this.BegNo;
                condition.EndNo = this.EndNo;
                condition.Name = this.PrintLogName;

                PrintLogList = iPrintRepository.GetPrintLogListByCondition(condition);
               
                if (PrintLogList.Count > 0)
                {
                    exitFlag = true;
                }
            }

            else if (!string.IsNullOrEmpty(key))
            {
                PrintLogList = iPrintRepository.GetPrintLogListByRange(key);
                if (PrintLogList.Count > 0)
                {
                    foreach (PrintLog iprlog in PrintLogList)
                    {
                        if (iprlog.Name == this.PrintLogName)
                        {
                            exitFlag = true;
                            break;
                        }
                    }
                }
            }

            if (!exitFlag)
            {
                 erpara.Add(this.PrintLogName);
                 erpara.Add(key);
                 ex = new FisException("SFC019", erpara);    //没有该%1 的打印记录，请确认该序号%2 是否正确！
                 throw ex;
            }

            
            CurrentSession.AddValue(Session.SessionKeys.isPassStationLog, true);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogList, PrintLogList);
            
            return base.DoExecute(executionContext);
        }

        
    }
}
