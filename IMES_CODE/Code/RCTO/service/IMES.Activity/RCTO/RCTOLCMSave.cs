// INVENTEC corporation (c)2011 all rights reserved. 
// Description:保存各种类型的PrintLog 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-03   Kerwin                       create
// Known issues:
using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.PrintLog;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Line;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.FA.Product;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// LCM Defect Input Save
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于各个批量产生序号的站, 记录产生序号的Log
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.产生序号的Activity,记录Session.PrintLogDescr,Session.PrintLogBegNo,Session.PrintLogEndNo保存在Session变量中
    ///         2.WritePrintLog将保存在Session中间变量中的log内容存入数据库;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.PrintLogDescr
    ///         Session.PrintLogBegNo
    ///         Session.PrintLogEndNo
    ///         Session.PrintLogName
    ///         
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///        无 
    /// </para> 
    ///<para> 
    /// 输出：
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         insert PrintLog
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IPrintLogRepository
    /// </para> 
    /// </remarks>
    public partial class RCTOLCMSave : BaseActivity
    {

        /// <summary>
        /// constructor
        /// </summary>
        public RCTOLCMSave()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            IProduct curProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            string ctno = (string)CurrentSession.GetValue("CTNO");

            IList<string> defectList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.DefectList);

            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            
            //Insert UnpackProduct_Part select *, @editor, @Udt from Product_Part 
            //where ProductID=@ProductID and CheckItemType<>’LCM’
            ProductPart cond = new ProductPart();
            cond.PartID = curProduct.ProId;

            ProductPart ncond = new ProductPart();
            ncond.CheckItemType = "LCM";

            productRep.BackUpProductPartByDn(Editor, cond, ncond);
            //Delete Product_Part where ProductID=@ProductID and CheckItemType<>’LCM’
            productRep.DeleteProductPart(cond, ncond);
            //Insert UnpackProductInfo select *, @editor, @Udt from ProductInfo where ProductID=@ProductID
            IMES.FisObject.FA.Product.ProductInfo infoconf = new IMES.FisObject.FA.Product.ProductInfo();
            infoconf.ProductID = curProduct.ProId; ;
            productRep.BackUpProductInfo(Editor, infoconf, null);
            //Delete ProductInfo where ProductID=@ProductID 
            productRep.DeleteProductInfo(infoconf, null);

            //if exists(select * from IqcCause1(nolock) where CtLabel=@CTNo and MpDefect=@Defect)
            //update IqcCause1 set Udt = GETDATE() where CtLabel=@CTNo and MpDefect=@Defect
            //else
            //insert into IqcCause1(CtLabel,MpDefect) values(@CTNo,@Defect)

            IDefectRepository defectRep = RepositoryFactory.GetInstance().GetRepository<IDefectRepository, Defect>();
            foreach (string code in defectList)
            {
                IqcCause1Info qconf = new IqcCause1Info();
                qconf.ctLabel = ctno;
                qconf.mpDefect = code;
                IList<IqcCause1Info> infoList = defectRep.GetIqcCause1InfoList(qconf);

                if (infoList.Count == 0)
                {
                    defectRep.AddIqcCause(qconf);
                }
                else
                {
                    IqcCause1Info item = new IqcCause1Info();
                    item.udt = DateTime.Now;
                    defectRep.UpdateUDTofIqcCause(item, qconf);
                }
            }

            return base.DoExecute(executionContext);
        }
    }
}