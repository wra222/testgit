// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 根据MONO获取MO对象
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-01-20   Yuan XiaoWei                 create
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
using IMES.FisObject.Common.BOM;
using IMES.FisObject.Common.Part;
using System.Collections.Generic;
using System.Data;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.FA.Product;
namespace IMES.Activity
{
    /// <summary>
    ///  根据MO获取MO对象
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于013TravelCardPrint,014ProIdPrint
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.参考UC;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.业务异常：CHK037
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MONO
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.ProdMO
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMORepository
    ///         MO
    /// </para> 
    /// </remarks>
    public partial class GetTravelcardParam : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetTravelcardParam()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 根据MONO获取MO对象并放到Session中
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {



            string mono = CurrentSession.GetValue(Session.SessionKeys.MONO).ToString();
            IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository, BOM>();
            BOM bom = bomRepository.GetBOM(mono);
            bom.GetAllPart();
            string[] param = new string[] { "", ""}; // 0: BATTERY  1:LCM 
            IList<IBOMPart> allPart = bom.GetAllPart();
            var query1 =
               from item in allPart
               where (item.Type == "LCM" || item.Type == "BATTERY")
               orderby item.Cdt ascending
               select item;

            foreach (var itemInfo in query1)
            {
                if (itemInfo.Type == "BATTERY")
                { param[0] = itemInfo.Descr; }
                if (itemInfo.Type == "LCM")
                { param[1] = itemInfo.Descr; }
            }

            CurrentSession.AddValue("CardParam", param);
            return base.DoExecute(executionContext);
               






            /*****************************************************************************************************************
              IBOMRepository bomRep2 = RepositoryFactory.GetInstance().GetRepository<IBOMRepository, BOM>();
              BOM modelBom = bomRep2.GetModelBOM("TCXJY0000005");
              IList<IBOMPart> allPart = modelBom.GetAllPart();
              var query1 =
                 from item in allPart
                 where (item.Type == "LCM" || item.Customer == "BATTERY")
                 orderby item.Cdt ascending
                 select item;

              foreach (var itemInfo in query1)
              {
                  if (itemInfo.Type == "LCM")
                  { parm[0] = itemInfo.Descr; }
                  if (itemInfo.Type == "BATTERY")
                  {   parm[2] =itemInfo.Descr;}
              }
           ********************************************* Get By Model BOM   *********************************************/

          
        }
    }
}
