/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:GetQC activity for FA TestStation Page
 *                 
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-06-14  Kaisheng             Create
 * Known issues:
 *      (none)
 * Modify history:
*/
using System;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.FisBOM;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Common;

namespace IMES.Activity
{
    /// <summary>
    /// 抽樣
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-FA-UC FA Test Station.docx
    /// </para>
    /// <para>
    /// 实现逻辑：详见UC
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.Product
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.SessionKeys.RandomInspectionStation
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProduct
    ///         IFamilyRepository
    ///         
    /// </para> 
    /// </remarks>
    public partial class GetProductQCForFATest : BaseActivity
    {
        ///<summary>
        ///</summary>
        public GetProductQCForFATest()
        {
            InitializeComponent();
        }

        private bool IsStationPIA(string model, string station)
        {
            IList<ConstValueInfo> constValueList = ActivityCommonImpl.Instance.GetConstValueListByType("PIASampling", "");
            if (constValueList == null)
                return false;
            foreach (ConstValueInfo c in constValueList)
            {
                if (ActivityCommonImpl.Instance.CheckModelByProcReg(model, c.name) && station.Equals(c.value))
                    return true;
            }
            return false;
        }

         /// <summary>
        /// QC,抽樣
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProduct CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            
            
            bool isPIA =false;
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            //d.	当前站为60，且机器为良品，则执行下面操作
            //if (this.Station != "60")
            if (!IsStationPIA(CurrentProduct.Model, this.Station))
            {
                CurrentSession.AddValue("isStationPIA", false);
                CurrentSession.AddValue("isPIA", false);
                return base.DoExecute(executionContext);
            }
            CurrentSession.AddValue("isStationPIA", true);

            //若Product存在ProductLog（Condition: Station=71 and Status=1），则该Product直接判定为PIA，不进行抽检操作
            IList<ProductLog> logList71 = productRepository.GetProductLogsOrderByCdtDesc(CurrentProduct.ProId,"71",1);
            if ((logList71 != null) && (logList71.Count >0))
            {
                isPIA = true;//"PIA";
            }
            else
            {
                //UC Update 	获取Product数量@Count，取消抽样时间 @QCStartTime之后的条件：
                //DateTime maxcdt = new DateTime(1900, 1, 1);//System.DateTime.Now.AddDays(-365); 
                //---------------------------------------------------------------------------
                //DateTime t = new DateTime(2012,1,1,12,0,0);
                //DateTime qcStartTime = productRepository.GetQCStartTime(t);
                DateTime qcStartTime = new DateTime(1900, 1, 1);
                //----------------------------------------------------------------------------
                //	获取抽检率：@QCRatio = QCRatio.QCRatio (Condtion: Family=Left(@Line,1))
                IFamilyRepository CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository, Family>();
                QCRatio currentQCRatio = CurrentRepository.GetQCRatio(this.Line.Substring(0,1));
                //若@QCRatio为空或者为Null，则报错：“请维护当前Line的QC抽检率”
                //if ((currentQCRatio == null)||(currentQCRatio.OQCRatio == null)||(currentQCRatio.OQCRatio == 0))
                //UC Update 2012/07/19 
                // 若@QCRatio为空或者为Null，则报错：“请维护当前Line的QC抽检率”
                // 若抽检率为0，则不进行如下抽检
                //--------------------------------------------------------------------
                //if ((currentQCRatio == null) || (currentQCRatio.OQCRatio == 0))
                if (currentQCRatio == null)
                {
                    List<string> errpara = new List<string>();
                    throw new FisException("CHK040", errpara);//change Error msg
                }
                if (currentQCRatio.OQCRatio == 0)
                {
                    CurrentSession.AddValue("isPIA", false);
                    return base.DoExecute(executionContext);
                }
                //---------------------------------------------------------------------
                int  iQCRadio = currentQCRatio.OQCRatio;
                //	获取Product数量@Count：
                int iQcCount = productRepository.GetSampleCount("PIA1",qcStartTime,this.Line);
                if ((iQcCount == 0)||(iQcCount % iQCRadio == 0))
                {
                    isPIA = true;//"PIA";
                }
                else
                {
                    isPIA = false;//"Exemption";
                }

            }
            //(bool)this.CurrentFlowSession.GetValue("isPIA") == True && (bool)this.CurrentFlowSession.GetValue("isStation60") == True
            //(bool)this.CurrentFlowSession.GetValue("isPIA") == False && (bool)this.CurrentFlowSession.GetValue("isStation60") == True
            //CurrentSession.AddValue("isStation60", this.Station);

            CurrentSession.AddValue("isPIA", isPIA);
            return base.DoExecute(executionContext);
        }
    }
}
