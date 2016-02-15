using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.FisObject.Common.Part;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Text.RegularExpressions;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.Common.Misc;
using Metas = IMES.Infrastructure.Repository._Metas;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Repository._Schema;
namespace IMES.Maintain.Implementation
{
    public class AstDefine : MarshalByRefObject, IAstDefine
    {
        #region IAstDefine 成员

        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
        IMiscRepository miscRep = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
        public IList<AstDefineInfo> GetAllAstDefineInfo()
        {
         
            try 
            {
                IList<AstDefineInfo> astDefineInfoList = miscRep.GetData<Metas.AstDefine, AstDefineInfo>(null);
                return astDefineInfoList;
              
            }
            catch(Exception ee)
            {
                logger.Error(ee.Message);
                throw;
            }
    
        }

        public List<string> GetAllAstCode(string astType)
        {
            List<string> res = new List<string>();
            res.Add("");
            string strSQL = @"select distinct Descr from Part where BomNodeType=@AstType order by Descr";

            SqlParameter paraName = new SqlParameter("@AstType", SqlDbType.VarChar, 10);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = astType;

            DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraName);
            foreach (DataRow dr in dt.Rows)
            {
                res.Add(dr[0].ToString().Trim());
            }
            return res;
        
        }
        public void AddAstDefineInfo(AstDefineInfo item)
        {
            CheckDuplicateData(item.AstType, item.AstCode);
            miscRep.InsertData<IMES.Infrastructure.Repository._Metas.AstDefine, AstDefineInfo>(item);
        }
        public void DelAstDefineInfo(string astType, string astCode)
        {
            AstDefineInfo item = new AstDefineInfo { AstType = astType, AstCode = astCode };
            miscRep.DeleteData<IMES.Infrastructure.Repository._Metas.AstDefine, AstDefineInfo>(item);
        }
        public void UpdateAstDefineInfo(AstDefineInfo item, string astType, string astCode)
        {
            AstDefineInfo condition = new AstDefineInfo { AstType = astType, AstCode = astCode };
            if (item.AstCode != astCode)
            {
                CheckDuplicateData(item.AstType, item.AstCode);
            }
            miscRep.UpdateData<IMES.Infrastructure.Repository._Metas.AstDefine, AstDefineInfo>(condition,item);
        }

        public void CheckDuplicateData(string astType, string astCode)
        {
            AstDefineInfo item = new AstDefineInfo { AstCode = astCode };
            IList<AstDefineInfo> astDefineInfoList = miscRep.GetData<Metas.AstDefine, AstDefineInfo>(item);
            if (astDefineInfoList.Count > 0)
            {
                throw new FisException("Duplicate AstCode!");
            }
        }

        public List<string> GetAssignAstStation()
        {
            List<string> ret = itemRepository.GetConstValueTypeList("AssignAstStationList").Select(x => x.value).Distinct().ToList();
            return ret;
        }
        public List<string> GetCombineASTStation()
        {
            List<string> ret = itemRepository.GetConstValueTypeList("CombineASTStationList").Select(x => x.value).Distinct().ToList();
            return ret;
        }

        #endregion
    }
}
