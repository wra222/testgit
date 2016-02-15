using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Infrastructure.Repository.Common
{
    public class FamilyRepositoryEx : FamilyRepository,IFamilyRepositoryEx
    {
        private static readonly IPartRepositoryEx partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepositoryEx>();
       
        public IList<FamilyInfoNameEx> GetFamilyInfoName()
        {
            try
            {
                IList<FamilyInfoNameEx> ret = new List<FamilyInfoNameEx>();

                string SQLStatement = "Select Name, Description, Editor, Cdt, Udt from FamilyInfoName NOLOCK";
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text, SQLStatement))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        FamilyInfoNameEx item = new FamilyInfoNameEx();
                        item.Name = GetValue_Str(sqlR, 0);
                        item.Description = GetValue_Str(sqlR, 1);
                        item.Editor =  GetValue_Str(sqlR, 2);
                        item.Cdt =GetValue_DateTime(sqlR,3);
                         item.Udt =GetValue_DateTime(sqlR,4);
                        ret.Add(item);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void AddFamilyInfoName(FamilyInfoNameEx item)
        {
             try
            {
               
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @"insert into FamilyInfoName(Name, Description, Editor, Cdt, Udt)
                                                            values(@Name, @Descr, @Editor,@Now,@Now)";

                        sqlCtx.Params.Add("Name", new SqlParameter("@Name", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Descr", new SqlParameter("@Descr", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Now", new SqlParameter("@Now", SqlDbType.DateTime));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }

                sqlCtx.Params["Name"].Value = item.Name;
                sqlCtx.Params["Descr"].Value = item.Description;
                sqlCtx.Params["Editor"].Value = item.Editor;
                sqlCtx.Params["Now"].Value =_Schema.SqlHelper.GetDateTime();
                  
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                CommandType.Text,
                                                                                 sqlCtx.Sentence, 
                                                                                sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateFamilyInfoName(FamilyInfoNameEx item, string nameKey)
        {
            try
            {

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @"update FamilyInfoName
                                                            set Name =@Name,
                                                                Description = @Descr,
                                                                Editor = @Editor,
                                                                Udt =@Now
                                                            where Name=@NameKey";

                        sqlCtx.Params.Add("Name", new SqlParameter("@Name", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Descr", new SqlParameter("@Descr", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Now", new SqlParameter("@Now", SqlDbType.DateTime));
                        sqlCtx.Params.Add("NameKey", new SqlParameter("@NameKey", SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }

                sqlCtx.Params["Name"].Value = item.Name;
                sqlCtx.Params["Descr"].Value = item.Description;
                sqlCtx.Params["Editor"].Value = item.Editor;
                sqlCtx.Params["Now"].Value = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params["NameKey"].Value = nameKey;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DeleteFamilyInfoName(string nameKey)
        {
            try
            {

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @"delete from FamilyInfoName                                                                                                     
                                                            where Name=@NameKey";
                       
                        sqlCtx.Params.Add("NameKey", new SqlParameter("@NameKey", SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
               
                sqlCtx.Params["NameKey"].Value = nameKey;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<FamilyInfoDef> GetFamilyInfoDefList(string strFamilyName)
        {
            try
            {
                IList<FamilyInfoDef> ret = new List<FamilyInfoDef>();

                SqlParameter[] paramsArray = new SqlParameter[1];
                paramsArray[0] = new SqlParameter("@Value", SqlDbType.VarChar);
                paramsArray[0].Value = strFamilyName;

                string SQLStatement = @"select A.ID as ID, B.Name as Name, isNull(A.Value, '') as Value, B.Description as Description,
                    A.Editor as Editor, A.Cdt as Cdt, A.Udt as Udt
                    From (select Family, Name, Value, Editor, Cdt, Udt, ID from FamilyInfo with(NOLOCK) where Family = @Value) as A 
                    Right Outer Join FamilyInfoName as B with(NOLOCK)
                    On A.Name = B.Name";
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text, SQLStatement, paramsArray))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        FamilyInfoDef item = new FamilyInfoDef();
                        item.id = GetValue_Int32(sqlR, 0);
                        item.family = strFamilyName;
                        item.name = GetValue_Str(sqlR, 1);
                        item.value = GetValue_Str(sqlR, 2);
                        item.descr = GetValue_Str(sqlR, 3);
                        item.editor = GetValue_Str(sqlR, 4);
                        item.cdt = GetValue_DateTime(sqlR, 5);
                        item.udt = GetValue_DateTime(sqlR, 6);
                        ret.Add(item);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public FamilyInfoDef GetFamilyInfoDef(string strFamilyInfoId)
        {
            try
            {
                FamilyInfoDef item = new FamilyInfoDef();

                SqlParameter[] paramsArray = new SqlParameter[1];
                paramsArray[0] = new SqlParameter("@Value", SqlDbType.Int);
                paramsArray[0].Value = Convert.ToInt32( strFamilyInfoId);

                string SQLStatement = "Select ID, Name, Value, Descr, Editor, Cdt, Udt, Family from FamilyInfo with(NOLOCK) where ID=@Value";
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text, SQLStatement, paramsArray))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        item.id = GetValue_Int32(sqlR, 0);
                        item.name = GetValue_Str(sqlR, 1);
                        item.value = GetValue_Str(sqlR, 2);
                        item.descr = GetValue_Str(sqlR, 3);
                        item.editor = GetValue_Str(sqlR, 4);
                        item.cdt = GetValue_DateTime(sqlR, 5);
                        item.udt = GetValue_DateTime(sqlR, 6);
                        item.family = GetValue_Str(sqlR, 7);
                    }
                }
                return item;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteFamilyInfo(FamilyInfoDef model)
        {
            try
            {

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @"delete from FamilyInfo where ID=@NameKey";

                        sqlCtx.Params.Add("NameKey", new SqlParameter("@NameKey", SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }

                sqlCtx.Params["NameKey"].Value = model.id;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }

            partRep.InsertCacheUpdate(CacheTypeEnum.Family, model.family);
        }

        public IList<string> GetFamilyByCustomer(string customer)
        {
            try
            {
                IList<string> ret = new List<string>();

                //SQL statement
                string sqlStr = @"Select Family from Family NOLOCK where CustomerID=@CustomerID";
                SqlParameter Para = new SqlParameter("@CustomerID", SqlDbType.VarChar);
                Para.Direction = ParameterDirection.Input;
                Para.Value = customer;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                                CommandType.Text,
                                                                                                                                sqlStr,
                                                                                                                               Para))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = GetValue_Str(sqlR, 0);
                        ret.Add(item);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteFamilyEx(string family, string customer)
        {
            try
            {

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @"delete from FamilyInfo where Family=@Family; delete from Family where Family=@Family and CustomerID=@CustomerID;";

                        sqlCtx.Params.Add("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        sqlCtx.Params.Add("CustomerID", new SqlParameter("@CustomerID", SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }

                sqlCtx.Params["Family"].Value = family;
                sqlCtx.Params["CustomerID"].Value = customer;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
            partRep.InsertCacheUpdate(CacheTypeEnum.Family, family);
        }

        public void DeleteFamilyInfoDefered(IUnitOfWork uow, FamilyInfoDef model)
        {
            Action deferAction = () => { DeleteFamilyInfo(model); };
            AddOneInvokeBody(uow, deferAction);
        }
        public void DeleteFamilyExDefered(IUnitOfWork uow, string family, string customer)
        {
            Action deferAction = () => { DeleteFamilyEx(family, customer); };
            AddOneInvokeBody(uow, deferAction);
        }
    }
}
