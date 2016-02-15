// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据输入的MaterialCT,获取Material对象，并放到Session中
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2014-03-01  Vincent
// Known issues:
using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.ComponentModel;
using IMES.FisObject.Common.Material;
namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class GetMaterial : BaseActivity
    {
        ///<summary>
        ///</summary>
        public GetMaterial()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get Product Object and Put it into Session.SessionKeys.Product
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
           

            var materialRep = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository>();
            if (IsSingleCT)
            {
                 string materialCT =(string)CurrentSession.GetValue(Session.SessionKeys.MaterialCT);
                if (materialCT==null)
                {
                    throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.MaterialCT });
                }
                Material material = materialRep.Find(materialCT);
                if (material == null)
                {
                    throw new FisException("CQCHK0011", new string[] { materialCT });
                }
                CurrentSession.AddValue(Session.SessionKeys.Material, material);
            }
            else
            {
                List<string> materialCTList = (List<string>)CurrentSession.GetValue(Session.SessionKeys.MaterialCTList);
                if (materialCTList == null || materialCTList.Count == 0)
                {
                    throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.MaterialCTList });
                }
                IList<Material> materialList = materialRep.GetMaterialByMultiCT(materialCTList);
                if (materialList == null || materialList.Count==0)
                {
                    throw new FisException("CQCHK0011", new string[] { string.Join(",", materialCTList.ToArray())});
                }

                CurrentSession.AddValue(Session.SessionKeys.MaterialList, materialList);
            }
          

           
            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// 输入序号找不到MB报错信息的ErroCode,不填则默认报错SFC002
        /// </summary>
        public static DependencyProperty NotExistExceptionProperty = DependencyProperty.Register("NotExistException", typeof(bool), typeof(GetMaterial),new PropertyMetadata(true));

        /// <summary>
        /// 输入序号找不到MB报错信息的ErroCode,不填则默认报错SFC002
        /// </summary>
        [DescriptionAttribute("NotExistException")]
        [CategoryAttribute("InArguments Of GetMaterial")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool NotExistException
        {
            get
            {
                return ((bool)(base.GetValue(GetMaterial.NotExistExceptionProperty)));
            }
            set
            {
                base.SetValue(GetMaterial.NotExistExceptionProperty, value);
            }
        }

        /// <summary>
        /// 输入的类型:ProductID,CustSN,ProductIDOrCustSN
        /// </summary>
        public static DependencyProperty IsSingleCTProperty = DependencyProperty.Register("IsSingleCT", typeof(bool), typeof(GetMaterial),new PropertyMetadata(true));

        /// <summary>
        /// 输入的类型:ProductID,CustSN,ProductIDOrCustSN
        /// </summary>
        [DescriptionAttribute("IsSingleCT")]
        [CategoryAttribute("InArugment Of GetMaterial")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsSingleCT
        {
            get
            {
                return ((bool)(base.GetValue(GetMaterial.IsSingleCTProperty)));
            }
            set
            {
                base.SetValue(GetMaterial.IsSingleCTProperty, value);
            }
        }

       
    }
}
