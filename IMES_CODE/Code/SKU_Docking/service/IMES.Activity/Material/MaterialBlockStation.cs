// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据输入的MaterialCT,获取Material对象，并放到Session中
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2014-03-01  Vincent
// Known issues:
using System;
using System.Linq;
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
    public partial class MaterialBlockStation : BaseActivity
    {
        ///<summary>
        ///</summary>
        public MaterialBlockStation()
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

                Material material = (Material)CurrentSession.GetValue(Session.SessionKeys.Material);
                if (material == null)
                {
                    //throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.Material });
                    FisException fe = new FisException("CQCHK0006", new string[] { Session.SessionKeys.Material });
                    if (IsStopWF == IsStopWFEnum.NotStop)
                    {
                        fe.stopWF = false;
                    }
                    throw fe;
                }
                if (!material.CheckMaterailStatus(this.Station))
                {

                    FisException fe = new FisException("CQCHK0006", new string[] { Session.SessionKeys.Material });
                    if (IsStopWF == IsStopWFEnum.NotStop)
                    {
                        fe.stopWF = false;
                    }
                    throw fe;
                }
                CurrentSession.AddValue(Session.SessionKeys.Material, material);
            }
            else
            {
                IList<Material> materialList = (IList<Material>)CurrentSession.GetValue(Session.SessionKeys.MaterialList);
                if (materialList == null || materialList.Count == 0)
                {
                    //throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.MaterialList });
                    FisException fe = new FisException("CQCHK0006", new string[] { Session.SessionKeys.Material });
                    if (IsStopWF == IsStopWFEnum.NotStop)
                    {
                        fe.stopWF = false;
                    }
                    throw fe;
                }

                   var blockStationFailList = (from p in materialList
                                                     where !p.CheckMaterailStatus(this.Station)
                                                     select p.MaterialCT
                                                     ).ToList();
                   if (blockStationFailList != null && blockStationFailList.Count > 0)
                   {
                       //throw new FisException("CQCHK0012", new string[] { string.Join(",",blockStationFailList.ToArray()), this.Station });
                       FisException fe = new FisException("CQCHK0006", new string[] { Session.SessionKeys.Material });
                       if (IsStopWF == IsStopWFEnum.NotStop)
                       {
                           fe.stopWF = false;
                       }
                       throw fe;
                   }

                CurrentSession.AddValue(Session.SessionKeys.MaterialList, materialList);
            }
          

           
            return base.DoExecute(executionContext);
        }

        

        /// <summary>
        /// 输入多筆資料
        /// </summary>
        public static DependencyProperty IsSingleCTProperty = DependencyProperty.Register("IsSingleCT", typeof(bool), typeof(MaterialBlockStation), new PropertyMetadata(true));

        /// <summary>
        /// 多筆資料
        /// </summary>
        [DescriptionAttribute("IsSingleCT")]
        [CategoryAttribute("InArugment Of GetMaterial")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsSingleCT
        {
            get
            {
                return ((bool)(base.GetValue(MaterialBlockStation.IsSingleCTProperty)));
            }
            set
            {
                base.SetValue(MaterialBlockStation.IsSingleCTProperty, value);
            }
        }

        /// <summary>
        ///  遇到Fis异常时是否停止工作流，共有两种，Stop，NotStop
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(IsStopWFEnum), typeof(MaterialBlockStation));

        /// <summary>
        ///  遇到Fis异常时是否停止工作流，共有两种，Stop，NotStop
        /// </summary>
        [DescriptionAttribute("IsStopWF")]
        [CategoryAttribute("InArguments Of SFC")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue(IsStopWFEnum.Stop)]
        public IsStopWFEnum IsStopWF
        {
            get
            {
                return ((IsStopWFEnum)(base.GetValue(MaterialBlockStation.IsStopWFProperty)));
            }
            set
            {
                base.SetValue(MaterialBlockStation.IsStopWFProperty, value);
            }
        }

       
    }
}
