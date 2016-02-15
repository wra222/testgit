// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  获取SessionBOM，包括MB/Product的BOM，并将其放入Session变量
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-06   YangWeihua                      create
using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.ComponentModel;

namespace IMES.Activity
{
    /// <summary>
    /// 获取SessionBOM，包括MB/Product的BOM，并将其放入Session变量
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于FA各个站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.通过IBOMRepository获取指定mo的指定站的BOM, 并加入Session中;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///             
    /// </para> 
    /// <para>    
    /// 输入：
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.SessionBOM
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         BOM
    /// </para> 
    /// </remarks>
    public partial class GetBOM : BaseActivity
    {
        /// <summary>
        /// constructor
        /// </summary>
        public GetBOM()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get Bom and put it into Session.SessionKeys.SessionBom
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //获取当前站要刷的BOM，此时BOM是空的
            var sessionBOM = this.GetSessionBOM();
            //var owner = GetPartOwner();
            //if (owner != null)
            //{
            //    IList<IProductPart> parts = owner.GetProductPartByStation(this.Station);
            //    {
            //        ////获取当前站已刷的Part并加入BOM中
            //        foreach (var part in parts)
            //        {
            //            var matchedPart = sessionBOM.Match(part.Value, Station);
            //            if (matchedPart != null)
            //            {
            //                sessionBOM.AddCheckedPart(matchedPart);
            //            }
            //        }
            //    }
            //}

            CurrentSession.AddValue(Session.SessionKeys.SessionBom, sessionBOM);
            CurrentSession.AddValue("ClearBom", sessionBOM);

            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// Get Bom
        /// </summary>
        /// <returns></returns>
        private IFlatBOM GetSessionBOM()
        {
            IFlatBOM sessionBOM = null;
            var bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            string family = null;
            string model = "";
            switch (ModelFrom)
            {
                case ModelFromEnum.ModelName:
                    model = CurrentSession.GetValue(Session.SessionKeys.ModelName) as string;
                    IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
                    Model modelObj = modelRepository.Find(model);
                    family = modelObj == null ? "" : modelObj.FamilyName;
                    break;
                case ModelFromEnum.MB:
                    IMB currentMB = CurrentSession.GetValue(Session.SessionKeys.MB) as IMB;
                    model = currentMB == null ? "" : currentMB.Model;
                    family = currentMB == null ? "" : currentMB.Family;
                    break;
                case ModelFromEnum.Product:
                    IProduct currentProduct = CurrentSession.GetValue(Session.SessionKeys.Product) as IProduct;
                    model = currentProduct == null ? "" : currentProduct.Model;
                    family = currentProduct == null ? "" : currentProduct.Family;
                    break;
                default:
                    IProduct defaultProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
                    if (defaultProduct != null)
                    {
                        model = defaultProduct.Model;
                        family = defaultProduct.Family;
                    }
                    break;
            }


            object mainObj = CurrentSession.GetValue(Session.SessionKeys.Product);
            if (MainObjFrom == MainObjFromEnum.Session)
            {
                mainObj = CurrentSession;
            }


            return sessionBOM = bomRepository.GetModelFlatBOMByStationModel(this.Customer, this.Station, this.Line, family, model, mainObj);
        }


        /// <summary>
        /// 获取BOM时用的Model来自于，共有三种，MB,Product，ModelName
        /// </summary>
        public static DependencyProperty ModelFromProperty = DependencyProperty.Register("ModelFrom", typeof(ModelFromEnum), typeof(GetBOM));

        /// <summary>
        /// 获取BOM时用的Model来自于，共有三种，MB,Product，ModelName
        /// </summary>
        [DescriptionAttribute("ModelFrom")]
        [CategoryAttribute("InArguments Of GetBOM")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public ModelFromEnum ModelFrom
        {
            get
            {
                return ((ModelFromEnum)(base.GetValue(GetBOM.ModelFromProperty)));
            }
            set
            {
                base.SetValue(GetBOM.ModelFromProperty, value);
            }
        }

        /// <summary>
        /// 获取BOM时用的MainObj来自于，共有三种，Product，Session
        /// </summary>
        public static DependencyProperty MainObjFromProperty = DependencyProperty.Register("MainObjFrom", typeof(MainObjFromEnum), typeof(GetBOM), new PropertyMetadata(MainObjFromEnum.Product));

        /// <summary>
        /// 获取BOM时用的Model来自于，共有三种，MB,Product，ModelName
        /// </summary>
        [DescriptionAttribute("MainObjFrom")]
        [CategoryAttribute("InArguments Of GetBOM")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public MainObjFromEnum MainObjFrom
        {
            get
            {
                return ((MainObjFromEnum)(base.GetValue(GetBOM.MainObjFromProperty)));
            }
            set
            {
                base.SetValue(GetBOM.MainObjFromProperty, value);
            }
        }

        /// <summary>
        /// 获取BOM时用的Model来自于，共有三种，MB,Product，ModelName
        /// </summary>
        public enum ModelFromEnum
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
            /// Product
            /// </summary>
            ModelName = 4
        }

        /// <summary>
        /// 获取BOM时用的MainObj来自于，共有三种，Product，Session
        /// </summary>
        public enum MainObjFromEnum
        {
            /// <summary>
            /// Product
            /// </summary>
            Product = 1,
            /// <summary>
            /// Session
            /// </summary>
            Session = 2,

        }
    }
}
