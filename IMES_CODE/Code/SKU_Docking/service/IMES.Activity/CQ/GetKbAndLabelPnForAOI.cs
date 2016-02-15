
using System;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Line;
using IMES.FisObject.Common.Part;
using System.Linq;
using IMES.FisObject.Common.FisBOM;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Repository._Schema;
using IMES.Common;
namespace IMES.Activity
{
    /// <summary>
    ///CheckIsAOILine
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
    ///         1.
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         this.Key
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.Product
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              Product
    /// </para> 
    /// </remarks>
    public partial class GetKbAndLabelPnForAOI : BaseActivity
	{
		///<summary>
		///</summary>
        public GetKbAndLabelPnForAOI()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 
        /// </summary>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IHierarchicalBOM curBom = bomRep.GetHierarchicalBOMByModel(product.Model);
            IList<IBOMNode> PLBomNodeList = curBom.GetFirstLevelNodesByNodeType("P1");
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            var query = from x in PLBomNodeList
                        where x.Part.Descr.Substring(0, 1) == "K"
                        select x.Part.PN;
            string kbPn = query.ToList().Count > 0 ? query.ToList()[0].Trim().Replace("-","") : "";

            List<string> erpara = new List<string>();
            string labelLightSetting = "LabelLightGuidType";
            string labelLight = "PAK Label";
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<string> values = partRep.GetValueFromSysSettingByName(labelLightSetting);
            if (values != null && values.Count > 0)
            {
                labelLight = values[0];
            }
            else
            {
                erpara.Add(labelLightSetting);
                FisException ex = new FisException("CQCHK0020", erpara);
                throw ex;
            }

            //Get Label Pn
            string labelList = "";
            //mantis 0001435: [PAK] ESOP 需要增加了 2面拍照
            if (this.Station == "8E")
            {
                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                //            string strSQL = @"SELECT DISTINCT a.Material,b.Descr,c.PartNo FROM dbo.ModelBOM a,dbo.Part  b,dbo.WipBuffer c 
                //                                                     WHERE a.Material=@model AND a.Component=b.PartNo 
                //                                                          AND (a.Component LIKE '6%' OR a.Component LIKE '2TG%') 
                //                                                          AND b.BomNodeType NOT IN('BM','P1') 
                //                                                          AND b.PartNo=c.PartNo AND c.KittingType ='FA Label' ";
//                string strSQL = @"SELECT DISTINCT a.Material,b.Descr,c.PartNo FROM dbo.ModelBOM a,dbo.Part  b,dbo.WipBuffer c 
//                                                     WHERE a.Material=@model AND a.Component=b.PartNo 
//                                                          AND (a.Component LIKE '6%' OR a.Component LIKE '2TG%') 
//                                                          AND b.BomNodeType NOT IN('BM','P1') 
//                                                          AND b.PartNo=c.PartNo AND c.Code ='ESOP Label' 
//                                                          AND c.PartNo not in (select Value from ConstValueType where Type='BTDLLabel') ";
                //     strSQL += "'" + labelLight + "' ";
                string strSQL = @"  exec  IMES_GetESOPLBPartNo @prdid";

                SqlParameter paraName = new SqlParameter("@prdid", SqlDbType.VarChar, 32);
                paraName.Direction = ParameterDirection.Input;
                paraName.Value = product.ProId;
                SqlDataReader dr = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_GetData, System.Data.CommandType.Text, strSQL, paraName);
                List<string> lst = new List<string>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lst.Add(dr["PartNo"].ToString().Trim());
                    }
                    labelList = string.Join(",", lst.ToArray());
                }
                dr.Close();
            }
            else//A  D
            {
                IList<string> imageUrlList = new List<string>();
                IList<string> PNList = new List<string>();
                IList<string> NeedCheckCCDPartList = new List<string>();

                string imageUr = "";
                IList<string> esopastlist = utl.ConstValueType("CCDASTPartNo", "").Select(x => x.value).ToList();
                for (int i = 0; i < curBom.FirstLevelNodes.Count; i++)
                {
                    
                    IPart part = ((BOMNode)curBom.FirstLevelNodes.ElementAt(i)).Part;
                    NeedCheckCCDPartList.Add(part.PN);
                    if ( esopastlist.Any(y => y == part.Descr))
                    {
                        PNList.Add(part.PN);
                    }
                   
                }	
                bool hasMN2 = true;  //0=no error; 1=MN2错误; 2=No AST           
                string strMN2 = product.GetModelProperty("MN2") as string;
                if (string.IsNullOrEmpty(strMN2))
                {
                    hasMN2 = false;
                }

                if (hasMN2)
                {
                    var pnList = PNList.Distinct().OrderBy(y => y).ToList();
                     imageUr = strMN2;
                    foreach (string pn in pnList)
                    {
                        imageUr = imageUr + pn;
                    }
                    NeedCheckCCDPartList.Add(imageUr);
                }
                IList<ConstValueTypeInfo> ccdinfotype = utl.ConstValueType("CCD A/DModel", "").Where(x=> NeedCheckCCDPartList.Any(y=> y==x.value)).ToList();
                 List<string> lst = new List<string>();
                foreach (ConstValueTypeInfo cddpartno in ccdinfotype)
                {
                    lst.Add(cddpartno.value.Trim() + "_" + cddpartno.description.Trim());
                }
                labelList = string.Join(",", lst.ToArray());
            }
            CurrentSession.AddValue("AOIKBPn", kbPn);
            CurrentSession.AddValue("AOILabelList", labelList);
            return base.DoExecute(executionContext);
        }
	}
}
