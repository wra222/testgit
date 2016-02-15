// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 判断当前Product的ProductStatus.Station<81 且不等于68
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-10-26   Kerwin                       create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
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
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Part;
using IMES.DataModel;
using IMES.Common;


namespace IMES.Activity
{

    /// <summary>
    /// 判断当前Product的Status的WC属性是否合法  
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC Asset Tag Label Print
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.判断当前Product的ProductStatus.Station small than 81 且不等于68,如果否则抛异常错误流程
    ///
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     PAK006
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
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         Product
    ///         IPartRepository
    ///         
    /// </para> 
    /// </remarks>
    public partial class CheckStationForAssetTagPrint : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckStationForAssetTagPrint()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 判断当前Product的ProductStatus.Station small than 81 且不等于68,如果否则抛异常错误流程
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            IList<AstDefineInfo> astDefineInfoList =utl.IsNull<IList<AstDefineInfo>>(session ,Session.SessionKeys.NeedCombineAstDefineList);
            IList<IPart> astPartList = utl.IsNull<IList<IPart>> (session, Session.SessionKeys.NeedCombineAstPartList);
            bool needCheckWC=false;
            bool allShippingAst = false;
           
            //IList<AstDefineInfo> removeAstDefineInfoList = null;
            //从Session里取得Product对象
            Product currentProduct = (Product)session.GetValue(Session.SessionKeys.Product);
            if (astDefineInfoList.Count>0 && 
                utl.HasShippingAstTag(astDefineInfoList))
            {
                needCheckWC = true;
                allShippingAst = utl.AllShippingAstTag(astDefineInfoList);
                //hasAstDefine=true;
                //removeAstDefineInfoList = astDefineInfoList.Where(x => utl.IsShippingAstTag(x)).ToList();
            }

            if (needCheckWC)
            {
                string currentWC = currentProduct.Status.StationId;

                /* 2012-7-16, Jessica Liu,新需求：增加站号，以方便流程
                if (string.Compare(currentWC, "81") < 0 && currentWC != "68")
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(currentProduct.ProId);
                    ex = new FisException("PAK006", erpara);
                    throw ex;
                }
                */
                IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IList<string> retlst = ipartRepository.GetValueFromSysSettingByName("PAKATWC");
                bool inStations = false;
                if (retlst != null && retlst.Count != 0)
                {
                    string[] cmpStations = retlst[0].Split(',');

                    inStations= cmpStations.Any(x=>x == currentWC);
                    //foreach (string tempStation in cmpStations)
                    //{
                    //    if (string.Compare(tempStation.Trim(), currentWC.Trim()) == 0)
                    //    {
                    //        inStations = true;
                    //        break;
                    //    }
                    //}

                    if (!inStations)
                    {
                        if (allShippingAst)
                        {
                            throw new FisException("PAK006", new List<string> { currentProduct.ProId });
                        }
                        else   //剔除Shipping這的打印
                        {
                            astDefineInfoList = astDefineInfoList.Where(x => !utl.IsShippingAstTag(x)).ToList();
                            if (astDefineInfoList.Count == 0)
                            {
                                throw new FisException("PAK006", new List<string> { currentProduct.ProId });
                            }
                            astPartList = astPartList.Where(x => astDefineInfoList.Any(y => y.AstCode == x.Descr)).ToList();
                            session.AddValue(Session.SessionKeys.NeedCombineAstDefineList, astDefineInfoList);
                            session.AddValue(Session.SessionKeys.NeedCombineAstPartList, astPartList);
                            //session.AddValue("NeedShippingAstDefineList", removeAstDefineInfoList);
                        }
                    }

                }
                else
                {
                    //FisException ex;
                    //List<string> erpara = new List<string>();
                    //erpara.Add(currentProduct.ProId);
                    //ex = new FisException("CHK916", erpara);
                    throw new FisException("CHK916", new List<string> { currentProduct.ProId });
                }
            }
            return base.DoExecute(executionContext);
        }

    }
}

