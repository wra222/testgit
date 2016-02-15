// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  
// UI:CI-MES12-SPEC-PAK-UI PD PA Label 1.docx
// UC:CI-MES12-SPEC-PAK-UC PD PA Label 1.docx                           
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2013/01/08 Benson          create
// Known issues:
using System;
using System.Data;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;
namespace IMES.Activity
{
    /// <summary>
    /// Energy Label 的列印条件及方法
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///     
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         
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
    ///         
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              productId
    /// </para> 
    /// </remarks>
    public partial class CheckEnergyLabelPrint : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public CheckEnergyLabelPrint()
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
            Product curProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            IList<IBOMNode> bomNodeList = (IList<IBOMNode>)CurrentSession.GetValue(Session.SessionKeys.SessionBom);
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IHierarchicalBOM curBom = bomRep.GetHierarchicalBOMByModel(curProduct.Model);
            IList<IBOMNode> bomList = curBom.FirstLevelNodes;
            IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<string> valueList =
                       PartRepository.GetConstValueTypeList("EnergyLabelPartNo").Select(x => x.value).Where(y => y != "").Distinct().ToList();
            CurrentSession.AddValue("EnergyLabel", "");
            if(valueList.Count>0)
            {
              foreach (IBOMNode node in bomList)
             {
                 if (valueList.Contains(node.Part.PN))
                 {
                     CheckEnergyLabelSQL(curProduct.Model);
                     CurrentSession.AddValue("EnergyLabel", "EnergyLabel");
                 }
             }
            }
           return base.DoExecute(executionContext);
        }

        private void CheckEnergyLabelSQL(string model)
        { 
           IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
           Model modelObj= modelRep.Find(model);
           string family = modelObj.FamilyName.Trim();
          //AVMaintain table: 檢查資料是否存在
          string strSQL =  @" SELECT b.ChinaLevel 
                                        FROM HPIMES.dbo.ModelBOM a, 
                                              GetData.dbo.AVMaintain b 
                                        where a.Material=@model AND 
                                            substring (a.Component,3,12)=b.IECAV AND 
                                            LEFT(b.IECAV,3)='2BS' ";
      

            SqlParameter paraName = new SqlParameter("@model", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = model;
            SqlDataReader dr = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text, strSQL, paraName);
            if (!dr.HasRows)
            { throw new FisException("CQCHK1059", new string[] { "AVMaintain" }); }
            string AV = "";
            while (dr.Read())
            { AV = dr[0].ToString(); }
            //Engery Label: 檢查資料是否存在 
            strSQL = @"SELECT top 1 * FROM HPIMES.dbo.EnergyLabel WHERE Family=@Family AND ChinaLevel=@AV";
            SqlParameter paraName2 = new SqlParameter("@Family", SqlDbType.VarChar, 64);
            paraName2.Direction = ParameterDirection.Input;
            paraName2.Value = family;
            SqlParameter paraNameAV = new SqlParameter("@AV", SqlDbType.VarChar, 64);
            paraNameAV.Direction = ParameterDirection.Input;
            paraNameAV.Value = AV;
            SqlDataReader dr2 = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text, strSQL, paraName2,paraNameAV);
            if (!dr2.HasRows)
            { throw new FisException("CQCHK1059", new string[] { "EnergyLabel" }); }

            //CEP:檢查資料是否存在: 
            strSQL = @" if exists(select top 1* from ModelBOM where Material=@model and Component='ZM2BL0B0021601')
                                             begin
                                              if exists( 
                                                select top 1*
                                                  from ModelInfo
                                                  where Model =@model and
                                                      Name='CECP' 
                                                     )
                                                    select 'Y'
                                                   else select 'N'  
                                             end 
                                           else
                                             Select 'Y'  ";
            SqlParameter paraName3 = new SqlParameter("@model", SqlDbType.VarChar, 32);
            paraName3.Direction = ParameterDirection.Input;
            paraName3.Value = model;
            SqlDataReader dr3 = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text, strSQL, paraName3);
            while (dr3.Read())
            { 
               if(dr3[0].ToString()=="N")
                {
                    throw new FisException("CQCHK1059", new string[] { "CECP" });
                }
            }
        

        
        }
	}
}
