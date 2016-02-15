// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 当Unit,PizzaID，Carton称重时，根据Model获取标准重量和误差
//              没有标准误差报错，没有标准重量，将当前的实际重量保存为标准重量
// 将获取的StandardWeight，Tolerance放到session中                      
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-26   Yuan XiaoWei                 create
// 2011-1-7     Chen Xu                      Modify: UnitWeight称重时： ModelWeight.UnitWeight 如果没有取到，则标准重量StandardWeight为NULL (这台机器后面就不需要进行标准重量的比较了)
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
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.StandardWeight;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.Extend;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
namespace IMES.Activity
{
    /// <summary>
    /// 获取Model的标准重量
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于UnitWeight,PizzaWeight,CartonWeight
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.根据Product的Model获取ModelWeight;
    ///         2.如果有误差，但是没有标准重量，将当前的实际重量作为标准重量保存到ModelWeight;
    ///         3.UnitWeigh称重时 ModelWeight.UnitWeight 如果没有取到，则标准重量StandardWeight为NULL (这台机器后面就不需要进行标准重量的比较了)
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.Tolerance
    ///         Session.StandardWeight
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         没有标准重量时，更新标准重量ModelWeight
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IModelWeightRepository
    ///         ModelWeight
    ///         Product
    /// </para> 
    /// </remarks>
    public partial class GetModelWeight : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetModelWeight()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 标准重量的类型，共有两种Unit，Carton
        /// </summary>
        public static DependencyProperty WeightTypeProperty = DependencyProperty.Register("WeightType", typeof(WeightTypeEnum), typeof(GetModelWeight));

        /// <summary>
        /// 标准重量的类型，共有两种Unit，Carton
        /// </summary>
        [DescriptionAttribute("WeightType")]
        [CategoryAttribute("WeightType Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public WeightTypeEnum WeightType
        {
            get
            {
                return ((WeightTypeEnum)(base.GetValue(GetModelWeight.WeightTypeProperty)));
            }
            set
            {
                base.SetValue(GetModelWeight.WeightTypeProperty, value);
            }
        }

        /// <summary>
        /// 执行逻辑
        /// 标准误差如果Model没有，再按Custormer找
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            string defaultTolerance = (string)CurrentSession.GetValue(Session.SessionKeys.Tolerance);
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            IModelWeightRepository currentModelWeightRepository = RepositoryFactory.GetInstance().GetRepository<IModelWeightRepository, ModelWeight>();
            ModelWeight currentModelWeight = currentModelWeightRepository.Find(currentProduct.Model);
            IFamilyRepository famliyRep = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository, Family>();

            IModelToleranceRepository currentModelToleranceRepository = RepositoryFactory.GetInstance().GetRepository<IModelToleranceRepository, ModelTolerance>();
            ModelTolerance currentModelTolerance = currentModelToleranceRepository.Find(currentProduct.Model);
            if (currentModelTolerance == null)
            {
                currentModelTolerance = currentModelToleranceRepository.Find(Customer);
            }

            if (WeightType == WeightTypeEnum.Unit)
            {
                //if (currentModelTolerance == null || currentModelTolerance.UnitTolerance == null)
                //{
                //    List<string> errpara = new List<string>();

                //    errpara.Add(currentProduct.Model);

                //    throw new FisException("CHK004", errpara);
                //}

                //UC Revision:6912:修改误差可以配置( UnitWeight称重 新方案： 缺省是2)
                if (currentModelTolerance == null || currentModelTolerance.UnitTolerance == null)
                {
                    currentModelTolerance = new ModelTolerance();
                  //  currentModelTolerance.UnitTolerance = "2";

                    //ITC-1360-1204: 改由sysSeting配置
                    IList<string> ToleranceLst = new List<string>();
                    ToleranceLst = ipartRepository.GetValueFromSysSettingByName("UnitWeightTolerance");
                    //For BSAM
              //       BSamUnitWeightTolerance
                    if( (string)CurrentSession.GetValue(ExtendSession.SessionKeys.IsBSamModel)=="Y")
                    { ToleranceLst = ipartRepository.GetValueFromSysSettingByName("BSamUnitWeightTolerance"); }
                    //For BSAM

                   //For Tablet

                    FamilyInfoDef fcond = new FamilyInfoDef();
                    fcond.family = currentProduct.Family;
                    fcond.name = "Category";
                    IList<FamilyInfoDef> famValList = famliyRep.GetExistFamilyInfo(fcond);
                    if (famValList.Count > 0)
                    {
                        string sysName = famValList[0].value.Trim() + "UnitWeightTolerance";
                        ToleranceLst = ipartRepository.GetValueFromSysSettingByName(sysName);
                        if (ToleranceLst.Count == 0)
                        {
                            throw new FisException("PAK095", new string[] { sysName });
                        }
                    }

                   //For Tablet
                    
                    if (ToleranceLst != null && ToleranceLst.Count > 0)
                    {
                        // ITC-1360-1465 : 2%
                        if (!ToleranceLst[0].ToString().Contains("%"))
                        {
                            string sysSettingUnitTolerance = ToleranceLst[0].ToString() + "%";
                            currentModelTolerance.UnitTolerance = sysSettingUnitTolerance;
                        }
                        else
                        {
                            currentModelTolerance.UnitTolerance = ToleranceLst[0].ToString();
                        }
                    }
                    else
                    {
                        //List<string> errpara = new List<string>();
                        //errpara.Add("UnitWeightTolerance");
                        //throw new FisException("PAK095", errpara);

                        // ITC-1360-1465 : 2%
                        /*
                         * Answer to: ITC-1414-0105
                         * Description: Set default tolerance properly.
                         */
                        if (defaultTolerance != null && defaultTolerance != "")
                        {
                            currentModelTolerance.UnitTolerance = defaultTolerance;
                        }
                        else
                        {
                            currentModelTolerance.UnitTolerance = "2%";
                        }

                    }
                }



                //// UnitWeight称重 新方案：	Standard Weight – IMES_GetData..ModelWeight.UnitWeight（需转换为decimal(10,2)） – 如果没有取到，则标准重量为NULL (这台机器后面就不需要进行标准重量的比较了)
                if (currentModelWeight == null)
                {
                    //currentModelWeight = new ModelWeight();
                    //currentModelWeight.Model = currentProduct.Model;
                    //currentModelWeight.UnitWeight = (decimal)CurrentSession.GetValue(Session.SessionKeys.ActuralWeight);
                    //currentModelWeight.Editor = Editor;
                    //currentModelWeight.Udt = DateTime.Now;

                    //currentModelWeightRepository.Add(currentModelWeight, CurrentSession.UnitOfWork);


                    CurrentSession.AddValue(Session.SessionKeys.StandardWeight, null);


                }
                else if (currentModelWeight.UnitWeight.Equals(0))
                {
                    //currentModelWeight.UnitWeight = (decimal)CurrentSession.GetValue(Session.SessionKeys.ActuralWeight);
                    //currentModelWeight.Editor = Editor;
                    //currentModelWeight.Udt = DateTime.Now;

                    //currentModelWeightRepository.Update(currentModelWeight, CurrentSession.UnitOfWork);

                    //5.	如果称取的重量为0，则报告错误：“Unit weight should not be 0!”
                    List<string> errpara = new List<string>();
                    errpara.Add(currentProduct.Model);
                    throw new FisException("PAK072", errpara);
                   
                }
                else
                {
                    CurrentSession.AddValue(Session.SessionKeys.StandardWeight, currentModelWeight.UnitWeight);
                }

                CurrentSession.AddValue(Session.SessionKeys.Tolerance, currentModelTolerance.UnitTolerance);

            }
            else
            {
                if (currentModelTolerance == null || currentModelTolerance.CartonTolerance == null)
                {
                    List<string> errpara = new List<string>();

                    errpara.Add(currentProduct.Model);

                    throw new FisException("CHK005", errpara);
                }
                if (currentModelWeight == null )
                {
                    currentModelWeight = new ModelWeight();
                    currentModelWeight.Model = currentProduct.Model;
                    currentModelWeight.CartonWeight = (decimal)CurrentSession.GetValue(Session.SessionKeys.ActuralWeight);
                    currentModelWeight.SendStatus = "";
                    currentModelWeight.Remark = "New";
                    currentModelWeight.Editor = Editor;
                    currentModelWeight.Udt = DateTime.Now;

                    currentModelWeightRepository.Add(currentModelWeight, CurrentSession.UnitOfWork);
                }
                else if (currentModelWeight.CartonWeight.Equals(0))
                {
                    currentModelWeight.CartonWeight = (decimal)CurrentSession.GetValue(Session.SessionKeys.ActuralWeight);
                    currentModelWeight.SendStatus = "";
                    currentModelWeight.Remark = "0";
                    currentModelWeight.Editor = Editor;
                    currentModelWeight.Udt = DateTime.Now;

                    currentModelWeightRepository.Update(currentModelWeight, CurrentSession.UnitOfWork);
                }

                CurrentSession.AddValue(Session.SessionKeys.Tolerance, currentModelTolerance.CartonTolerance);
                CurrentSession.AddValue(Session.SessionKeys.StandardWeight, currentModelWeight.CartonWeight);
            }

            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// 标准重量的类型，共有两种Unit，Carton
        /// </summary>
        public enum WeightTypeEnum
        {
            /// <summary>
            /// 获取Unit标准重量和误差
            /// </summary>
            Unit = 1,

            /// <summary>
            /// 获取Carton标准重量和误差
            /// </summary>
            Carton = 2
        }
    }
}
