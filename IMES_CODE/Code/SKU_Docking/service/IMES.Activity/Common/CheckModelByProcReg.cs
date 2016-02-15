// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2013-02-07   Benson          create
//2013-03-13    Vincent           release
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
using IMES.FisObject.Common;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.BSam;
using IMES.Infrastructure.Extend;
using IMES.FisObject.PAK.DN;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using System.Text.RegularExpressions;

namespace IMES.Activity
{


    /// <summary>
    /// Check the Model by Regular Expression 
    /// </summary>
  
    public partial class CheckModelByProcReg : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
        public CheckModelByProcReg()
		{
			InitializeComponent();
		}
        //--
        /// <summary>
        /// ProcReg Name
        /// </summary>
        public static DependencyProperty ProcRegNameProperty = DependencyProperty.Register("ProcRegName", typeof(string), typeof(CheckModelByProcReg), new PropertyMetadata());
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
               return ((string)(base.GetValue(CheckModelByProcReg.ProcRegNameProperty)));
            }
            set
            {
                base.SetValue(CheckModelByProcReg.ProcRegNameProperty, value);
            }
        }

        //--

        /// <summary>
        /// 禁用時要停止workflow
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(bool), typeof(CheckModelByProcReg), new PropertyMetadata(true));
        /// <summary>
        ///  禁用時要停止workflow
        /// </summary>
        [DescriptionAttribute("IsStopWF")]
        [CategoryAttribute("IsStopWF")]
        [BrowsableAttribute(true)]       
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsStopWF
        {
            get
            {
                return ((bool)(base.GetValue(CheckModelByProcReg.IsStopWFProperty)));
            }
            set
            {
                base.SetValue(CheckModelByProcReg.IsStopWFProperty, value);
            }
        }
        /// <summary>
        /// throw exception or not
        /// </summary>
        public static DependencyProperty IsThrowExceptionProperty = DependencyProperty.Register("IsThrowException", typeof(bool), typeof(CheckModelByProcReg), new PropertyMetadata(true));
        /// <summary>
        ///  禁用時要停止workflow
        /// </summary>
        [DescriptionAttribute("IsThrowException")]
        [CategoryAttribute("IsThrowException")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsThrowException
        {
            get
            {
                return ((bool)(base.GetValue(CheckModelByProcReg.IsThrowExceptionProperty)));
            }
            set
            {
                base.SetValue(CheckModelByProcReg.IsThrowExceptionProperty, value);
            }
        }
        
        /// <summary>
        /// Match 報錯
        /// </summary>
        public static DependencyProperty MatchExceptionProperty = DependencyProperty.Register("MatchException", typeof(bool), typeof(CheckModelByProcReg), new PropertyMetadata(false));

        /// <summary>
        /// Match 報錯
        /// </summary>
        [DescriptionAttribute("MatchException")]
        [CategoryAttribute("MatchException")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool MatchException
        {
            get
            {
                return ((bool)(base.GetValue(CheckModelByProcReg.MatchExceptionProperty)));
            }
            set
            {
                base.SetValue(CheckModelByProcReg.MatchExceptionProperty, value);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            string model = currentProduct.Model.Trim();
            bool isPass = false;
            IList<ConstValueInfo> lstConst = GetConstValueListByType("ProcReg", "Name").Where(x => x.value.Trim() != "").ToList(); ;
            CurrentSession.AddValue("IsPassCheckModel", true);
            if (ProcRegName != "")
            {
                string[] regArr = ProcRegName.Split(',');
                foreach (String regName in regArr)
                {
                    var data = (from p in lstConst
                                where p.name.Trim() == regName
                                select p.value.Trim()).ToList();

                    if (data.Count == 0 )
                    {
                        FisException e = new FisException("CHK990", new string[] { "ConstValue ", "ProcReg", "Name:" + regName });
                        e.stopWF = this.IsStopWF;
                        throw e;
                    }
                    foreach (string regValue in data)
                    {
                        Regex regex = new Regex(regValue);
                        if (regex.IsMatch(model))
                        {
                            if (MatchException)
                            {
                                CurrentSession.AddValue("IsPassCheckModel", false);
                                if (IsThrowException)
                                {
                                    FisException e = new FisException("PAK131", new string[] { });
                                     e.stopWF = this.IsStopWF;
                                     throw e;
                                }
                             
                            }
                            else
                            {
                                isPass = true;
                            }
                        }
                 
                    }

                }
            
            
            }
            if (!isPass)
            {
                //FisException e = new FisException("PAK131", new string[] { });
                //e.stopWF = this.IsStopWF;
                //throw e;
                CurrentSession.AddValue("IsPassCheckModel", false);
                if (IsThrowException)
                {
                    FisException e = new FisException("PAK131", new string[] { });
                    e.stopWF = this.IsStopWF;
                    throw e;
                }
            }
            return base.DoExecute(executionContext);
          
        }

        private IList<ConstValueInfo> GetConstValueListByType(string type, string orderby)
        {
            IList<ConstValueInfo> retLst = new List<ConstValueInfo>();
            try
            {
                if (!String.IsNullOrEmpty(type))
                {
                    IPartRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                    var resultLst = palletRepository.GetConstValueListByType(type);
                    if (!string.IsNullOrEmpty(orderby))
                    {
                        var tmpLst = from item in resultLst orderby item.id select item;
                        retLst = tmpLst.ToList();
                    }
                    else
                    {
                        retLst = resultLst;
                    }
                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }

        }
	}
}
