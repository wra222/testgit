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
    public partial class GetMaterialBox : BaseActivity
    {
        ///<summary>
        ///</summary>
        public GetMaterialBox()
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
           

            var materialBoxRep = RepositoryFactory.GetInstance().GetRepository<IMaterialBoxRepository>();
            if (IsSingle)
            {
                 string materialBoxId =(string)CurrentSession.GetValue(Session.SessionKeys.MaterialBoxId);
                if (materialBoxId==null)
                {
                    throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.MaterialBoxId });
                }
                MaterialBox materialBox = materialBoxRep.Find(materialBoxId);
                if (materialBox == null)
                {
                    throw new FisException("CQCHK0011", new string[] { materialBoxId });
                }
                CurrentSession.AddValue(Session.SessionKeys.MaterialBox, materialBox);
            }
            else
            {
                List<string> materialBoxIdList = (List<string>)CurrentSession.GetValue(Session.SessionKeys.MaterialBoxIdList);
                if (materialBoxIdList == null || materialBoxIdList.Count == 0)
                {
                    throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.MaterialBoxIdList });
                }
                IList<MaterialBox> materialBoxList = materialBoxRep.GetMaterialBoxbyMultiBoxId(materialBoxIdList);
                if (materialBoxList == null || materialBoxList.Count == 0)
                {
                    throw new FisException("CQCHK0011", new string[] { string.Join(",", materialBoxIdList.ToArray()) });
                }

                CurrentSession.AddValue(Session.SessionKeys.MaterialBoxIdList, materialBoxList);
            }
          

           
            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// 输入序号找不到MB报错信息的ErroCode,不填则默认报错SFC002
        /// </summary>
        public static DependencyProperty NotExistExceptionProperty = DependencyProperty.Register("NotExistException", typeof(bool), typeof(GetMaterialBox), new PropertyMetadata(true));

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
                return ((bool)(base.GetValue(GetMaterialBox.NotExistExceptionProperty)));
            }
            set
            {
                base.SetValue(GetMaterialBox.NotExistExceptionProperty, value);
            }
        }

        /// <summary>
        /// 输入的类型:ProductID,CustSN,ProductIDOrCustSN
        /// </summary>
        public static DependencyProperty IsSingleProperty = DependencyProperty.Register("IsSingle", typeof(bool), typeof(GetMaterialBox), new PropertyMetadata(true));

        /// <summary>
        /// 输入的类型:ProductID,CustSN,ProductIDOrCustSN
        /// </summary>
        [DescriptionAttribute("IsSingle")]
        [CategoryAttribute("InArugment Of GetMaterial")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsSingle
        {
            get
            {
                return ((bool)(base.GetValue(GetMaterialBox.IsSingleProperty)));
            }
            set
            {
                base.SetValue(GetMaterialBox.IsSingleProperty, value);
            }
        }

       
    }
}
