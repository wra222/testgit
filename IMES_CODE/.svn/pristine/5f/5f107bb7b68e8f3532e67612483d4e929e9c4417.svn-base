/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description:GetProductQC activity for AFTMVS Page
 *                 
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-28  itc202017             Create
 * 2011-12-13  Kerwin                implement
 * Known issues:
 *      (none)
 * Modify history:
 * Answer to: ITC-1360-0551
 * Description: Wrong argument line input, so get value qcCnt will always be 0,
 *              and the result of the process will always be "toEPIA".
*/
using System;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.FisBOM;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Repository._Schema;

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
    ///      CI-MES12-SPEC-FA-UC AFT MVS.docx
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
    ///         IModelRepository
    /// </para> 
    /// </remarks>
    public partial class GetProductQC : BaseActivity
    {
        ///<summary>
        ///</summary>
        public GetProductQC()
        {
            InitializeComponent();
        }

        /*
        /// <summary>
        /// Get model type (BTO/CTO).
        /// </summary>
        /// 机型12码第七码是字母的是BTO，第七码是数字的是CTO,Y为特殊类型
        /// 此处BTO和Special机型处理一致，故认为Special也是BTO机型
        private int getModelType(string model)
        {
            if (model.Length < 7) return -1;
            char c = model[6];
            if (c >= '0' && c <= '9') return 0; //CTO
            if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')) return 1; //BTO
            return -1;
        }
        */
        /// <summary>
        /// QC,EOQC抽樣
        /// 2012-09-11 改为调用SP
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProduct CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            DataTable dt = new DataTable();

            SqlParameter[] paramsArray = new SqlParameter[1];
            paramsArray[0] = new SqlParameter("ProductID", CurrentProduct.ProId);
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            dt = productRepository.ExecSpForQuery(SqlHelper.ConnectionString_FA, "op_MVS_Get_Product_CheckType", paramsArray);

            if (dt.Rows.Count != 1)
            {
                throw new Exception("SP return value is invalid![row cnt:" + dt.Rows.Count + "]");
            }

            DataRow row = dt.Rows[0];
            if (row[0].ToString().Trim() == "ERROR")
            {
                throw new Exception(row[1].ToString().Trim());
            }

            string InspectionStation = "";
            if (row[0].ToString().Trim() == "SUCCESS")
            {
                if (row[1].ToString().Trim() == "PASS")
                {
                    InspectionStation = "79";
                }
                else if (row[1].ToString().Trim() == "EPIA")
                {
                    InspectionStation = "73";
                }
                else
                {
                    throw new Exception("SP return value is invalid![Unknown QC method:" + row[1].ToString().Trim() + "]");
                }
            }
            else
            {
                throw new Exception("SP return value is invalid![First column of SP result:" + row[0].ToString().Trim() + "]");
            }

            CurrentSession.AddValue(Session.SessionKeys.RandomInspectionStation, InspectionStation);

            return base.DoExecute(executionContext);
            /*            
            string InspectionStation = "";
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            if (productRepository.IsRealEPIAReflow(CurrentProduct.ProId, "PIA", "1"))   //Real EPIA Reflow
            {
                InspectionStation = "73";
            }
            else
            {
                IFamilyRepository CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository, Family>();
                QCRatio currentQCRatio = CurrentRepository.GetQCRatio(CurrentProduct.Model);
                if (currentQCRatio == null)
                {
                    currentQCRatio = CurrentRepository.GetQCRatio(CurrentProduct.Family);
                }
                if (currentQCRatio == null)
                {
                    currentQCRatio = CurrentRepository.GetQCRatio(CurrentProduct.Customer);
                }

                if (currentQCRatio == null)
                {
                    List<string> errpara = new List<string>();
                    throw new FisException("CHK040", errpara);
                }

                int qcCnt = 0;
                DateTime t = new DateTime(2012,1,1,12,0,0);
                DateTime qcStartTime = productRepository.GetQCStartTime(t);

                if (productRepository.IsSpecialCTO(CurrentProduct.Model))   //SpecialCTO, treated as BTO
                {
                    qcCnt = productRepository.GetBTOSampleCount("PIA", qcStartTime, CurrentProduct.Model);
                }
                else
                {
                    int moType = getModelType(CurrentProduct.Model);
                    if (moType == 0)    //CTO
                    {
                        qcCnt = productRepository.GetCTOSampleCount("PIA", qcStartTime, CurrentProduct.Model, CurrentProduct.Family);
                    }
                    else if (moType == 1)   //BTO
                    {
                        qcCnt = productRepository.GetBTOSampleCount("PIA", qcStartTime, CurrentProduct.Model);
                    }
                }
                
                if (qcCnt % currentQCRatio.EOQCRatio == 0) //Real EPIA
                {
                    InspectionStation = "73";
                }
                else    //QC Pass
                {
                    InspectionStation = "79";
                }
            }

            CurrentSession.AddValue(Session.SessionKeys.RandomInspectionStation, InspectionStation);

            return base.DoExecute(executionContext);
            */
        }
    }
}
