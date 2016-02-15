using System;
using System.Collections.Generic;
using IMES.Maintain.Interface.MaintainIntf;
using System.Data;
using IMES.DataModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using System.Linq;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;
using log4net;

namespace IMES.Maintain.Implementation
{
    public class AssetRuleManager : MarshalByRefObject, IAssetRule
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetAssetRuleList()
        {
            DataTable result = new DataTable();

            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                result = itemRepository.GetAssetCheckRuleList();
               
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public IList<AstRuleInfo> GetAsSetRuleList()
        {
            IList<AstRuleInfo> result = new List<AstRuleInfo>();

            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                result = itemRepository.GetAstRule();

            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public void DeleteAssetRule(AstRuleInfo item)
        {
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                AstRuleInfo astRule = new AstRuleInfo();

                //astRule.tp = "AT";
                //astRule.code = item.code;
                //astRule.checkTp = "";//item.checkTp;
                //astRule.checkItem = item.checkItem;
                //astRule.custName = item.custName;
                //astRule.station = item.station;
                //astRule.editor = item.editor;
                astRule.id = item.id;


                itemRepository.DeleteAstRuleRule(astRule);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteAssetRule(int id)
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                //AstRuleInfo astRule = new AstRuleInfo();
                //astRule.id = item.id;
                itemRepository.DeleteAstRule(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string AddAssetRule(AstRuleInfo item)
        {
            String result = "";
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                AstRuleInfo astRule = new AstRuleInfo();

                astRule.tp = item.tp;
                astRule.code = item.code;
                astRule.checkTp = item.checkTp;
                astRule.checkItem = item.checkItem;
                astRule.custName = item.custName;
                astRule.station = item.station;
                

                DataTable exists = itemRepository.ExistAstRule(astRule);
                if (exists != null && exists.Rows.Count > 0)
                {
                    //已经存在具有相同AstType、CheckItem、CheckType、Station和CustName的AssetRule记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT071", erpara);
                    throw ex;

                }
                astRule.editor = item.editor;
                astRule.remark = item.remark;
                astRule.cdt = DateTime.Now;
                astRule.udt = DateTime.Now;

                itemRepository.AddAstRuleInfo(astRule);
                result = astRule.id.ToString();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public void AddAsSetRule(AstRuleInfo item)
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                IList<AstRuleInfo> exists = itemRepository.GetAstRuleByCondition(item);
                if (exists != null && exists.Count > 0)
                {
                    //已经存在具有相同AstType、CheckItem、CheckType、Station和CustName的AssetRule记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT071", erpara);
                    throw ex;

                }
                item.cdt = DateTime.Now;
                item.udt = DateTime.Now;
                itemRepository.AddAstRule(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectInfoDef> GetAstTypeList()
        {
            List<SelectInfoDef> result = new List<SelectInfoDef>();
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                string nodeType = "AT";
                IList<string> getData = itemRepository.GetAstTypes(nodeType);
                for (int i = 0; i < getData.Count; i++)
                {
                    SelectInfoDef item = new SelectInfoDef();
                    string getValue = getData[i].Trim();
                    item.Text = getValue;
                    item.Value = getValue;
                    result.Add(item);
                }

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public IList<ConstValueInfo> GetCheckItemValue(string SelectTypes,string name)
        {
            IList<ConstValueInfo> dataLst = new List<ConstValueInfo>();
            IList<ConstValueInfo> templist = new List<ConstValueInfo>();
            ConstValueMaintain ConstValue = new ConstValueMaintain();
            
            try
            {
                templist = ConstValue.GetConstValueListByType(SelectTypes);
                dataLst = (from q in templist
                           where q.name == name
                           select q).ToList();

                return dataLst;
            }
            catch (Exception ee)
            {
                //logger.Error(ee.Message);
                throw ee;
            }
        }

        public IList<string> GetAVPartNoValue(string ASTTypes, string Tp)
        {
            //IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
            //try
            //{
            //    IList<string> lst = (from parts in itemRepository.getpart
            //                         join partinfos in itemRepository.GetPartInfoList(new PartInfo { InfoType = "AV" }) on parts.PN equals partinfos.PN
            //                         where parts.BOMNodeType == Tp &&
            //                         parts.Descr == ASTTypes
            //                         select partinfos.InfoValue).ToList();

            //    return lst;
            //}
            //catch (Exception ee)
            //{
            //    //logger.Error(ee.Message);
            //    throw ee;
            //}
            logger.Debug("(AssetRuleManager)GetAVPartNoValue starts");
            try
            {
                string strSQL = @"select distinct b.InfoValue
                                     from Part a,
                                          PartInfo b
                                    where a.PartNo = b.PartNo and
                                          a.BomNodeType =@Tp and
                                          a.Descr = @AstType and
                                          b.InfoType = 'AV'";
                SqlParameter paraNameType = new SqlParameter("@Tp", SqlDbType.VarChar, 3);
                SqlParameter paraNameType1 = new SqlParameter("@AstType", SqlDbType.VarChar, 80);
                paraNameType.Direction = ParameterDirection.Input;
                paraNameType.Value = Tp;
                paraNameType1.Direction = ParameterDirection.Input;
                paraNameType1.Value = ASTTypes;
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameType, paraNameType1);
                List<string> list = new List<string>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                {
                    string item = dr[0].ToString().Trim();
                    list.Add(item);
                }
                return list;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(AssetRuleManager)GetAVPartNoValue end");
            }
        }

    }
}
