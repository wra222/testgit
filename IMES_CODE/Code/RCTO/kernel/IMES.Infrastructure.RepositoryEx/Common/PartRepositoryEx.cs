﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.Part;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using IMES.Infrastructure.UnitOfWork;
using IMES.DataModel;

namespace IMES.Infrastructure.Repository.Common
{
    public class PartRepositoryEx:PartRepository,IPartRepositoryEx
    {
       
        #region new Part method
        private string ObjToString(Object obj)
        {
          if(obj==null)
          {return string.Empty;}
          else
          {return obj.ToString().Trim();}
        }

        public void DeletePartInfoByID(int id)
        {
            try
            {               
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @" delete from PartInfo where ID=@id";                        
                        sqlCtx.Params.Add("id", new SqlParameter("@id", SqlDbType.Int));
                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }

                sqlCtx.Params["id"].Value = id;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                sqlCtx.Params.Values.ToArray<SqlParameter>());

              
            }
            catch (Exception)
            {
             
                throw;
            }
            finally
            {
               
            }
        }

        public void DeletePartInfoByIDDefered(IUnitOfWork uow, int id)
        {
            Action deferAction = () => { DeletePartInfoByID(id); };
            AddOneInvokeBody(uow, deferAction);
        }

        public void DeletePartInfoByID(int id, string partNo)
        {
            DeletePartInfoByID(id);
            InsertCacheUpdate(CacheTypeEnum.Part, partNo);
        }

        public void DeletePartInfoByIDDefered(IUnitOfWork uow, int id, string partNo)
        {
            Action deferAction = () => { DeletePartInfoByID(id, partNo); };
            AddOneInvokeBody(uow, deferAction);
        }

        public IList<PartInfoMaintainInfo> GetPartInfoListByPartNo(string partNo)
        {
            try
            {
                _Schema.SQLContext sqlCtx = new _Schema.SQLContext();
                IList<PartInfoMaintainInfo> ret = new List<PartInfoMaintainInfo>();
                //string pn, string bomNodeType, string type, string custpn, string descr, string remark, string autodl, string editor, DateTime cdt, DateTime udt, string descr2  
                string SQLStatement = @"select ID,PartNo,InfoType,InfoValue,Editor,Cdt,Udt from PartInfo with(NOLOCK) where PartNo=@PartNo
                                                            Union
                                                            (
                                                                 select '',@PartNo,Code,'','','','' from PartTypeAttribute with(NOLOCK) where PartType 
                                                                  in(  select PartType from Part where PartNo=@PartNo)
                                                                 and Code not in 
                                                                 (
                                                                  select InfoType from PartInfo with(NOLOCK) where PartNo=@PartNo
                                                                 ) 
                                                             );";

               
                sqlCtx.Params.Add("PartNo", new SqlParameter("@PartNo", SqlDbType.VarChar));
                sqlCtx.Params["PartNo"].Value = partNo;
                        
                using (DataTable dt = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text, SQLStatement,
                                                                                                                             sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    string colName;
                    object value;
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            PartInfoMaintainInfo partInfo = new PartInfoMaintainInfo();
                                                
                                 foreach (FieldInfo f in partInfo.GetType().GetFields())
                                    {
                                        
                                         colName=f.Name;
                                         value=dr[colName];
                                         partInfo.GetType().GetField(colName).SetValue(partInfo, value);

                                    }

                            //

                                 ret.Add(partInfo);
                        }


                    }


                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        
        
        
        }
        public IList<PartDef> GetPartListByPartialPartNo(string partNo, int rowCount)
        { 
            try
            {
                IList<PartDef> ret = new List<PartDef>();
              //string pn, string bomNodeType, string type, string custpn, string descr, string remark, string autodl, string editor, DateTime cdt, DateTime udt, string descr2  
                string SQLStatement = @"select top {1} PartNo, BomNodeType,Descr, PartType, CustPartNo, AutoDL, Remark,
                                                           Flag, Editor, Cdt, Udt  from Part with(NOLOCK) where PartNo like '%{0}%' order by PartNo";

               SQLStatement=string.Format(SQLStatement,partNo,rowCount.ToString());

                using (DataTable dt = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text, SQLStatement))
                {
                    if(dt.Rows.Count>0)
                    {
                        foreach(DataRow dr in dt.Rows)
                        {
                            PartDef part=new PartDef();
                            part.partNo=ObjToString(dr["PartNo"]);
                            part.bomNodeType=ObjToString(dr["BomNodeType"]);
                            part.partType=ObjToString(dr["PartType"]);
                            part.custPartNo= ObjToString(dr["CustPartNo"]);
                            part.descr=ObjToString(dr["Descr"]);
                            part.remark=ObjToString(dr["Remark"]);
                            part.autoDL= ObjToString(dr["AutoDL"]);
                            part.flag = int.Parse(dr["Flag"].ToString());
                            part.editor=ObjToString(dr["Editor"]);
                            part.cdt=DateTime.Parse(dr["Cdt"].ToString());
                            part.udt=  DateTime.Parse(dr["Udt"].ToString());
                            ret.Add(part);
                        }
                     
                    
                    }
                    
                    
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        
        
        }
         #endregion
        #region override PartType method
        public new  IList<PartType> GetPartTypeObjList()
        {
            try
            {
                IList<PartType> ret = new List<PartType>();
                
                string SQLStatement = "select PartType, PartTypeGroup, Editor, Cdt, Udt, ID from PartTypeEx ";
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text, SQLStatement))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PartType item = new PartType(GetValue_Int32(sqlR,5),
                                                                        GetValue_Str(sqlR, 0),
                                                                         GetValue_Str(sqlR, 1),
                                                                         GetValue_Str(sqlR, 2),
                                                                         GetValue_DateTime(sqlR, 3),
                                                                         GetValue_DateTime(sqlR, 4));
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

        public void  AddPartTypeDefered(IUnitOfWork uow, PartType item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public new void SavePartTypeDefered(IUnitOfWork uow,PartType item, string strOldPartType)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item, strOldPartType);
        }

        public new void DeletePartTypeByPartTypeDefered(IUnitOfWork uow, string partType)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), partType);
        }


        public new void AddPartType(PartType item)
        {
            try
            {


                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @"insert into PartTypeEx (PartType, PartTypeGroup, Editor, Cdt, Udt)
                                                            values(@PartType,@PartTypeGroup,@Editor,@Now,@Now)";

                        sqlCtx.Params.Add("PartType", new SqlParameter("@PartType", SqlDbType.VarChar));
                        sqlCtx.Params.Add("PartTypeGroup", new SqlParameter("@PartTypeGroup", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Now", new SqlParameter("@Now", SqlDbType.DateTime));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }

                sqlCtx.Params["PartType"].Value = item.PartTypeName;
                sqlCtx.Params["PartTypeGroup"].Value = item.PartTypeGroup;
                sqlCtx.Params["Editor"].Value = item.Editor;
                sqlCtx.Params["Now"].Value = _Schema.SqlHelper.GetDateTime();

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

        public new void SavePartType(PartType item, string strOldPartType)
        {
            try
            {


                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @"update PartTypeEx
                                                            set PartType =@PartType,
                                                                PartTypeGroup = @PartTypeGroup,
                                                                Editor = @Editor,
                                                                Udt =@Now
                                                            where PartType=@NameKey";

                        sqlCtx.Params.Add("PartType", new SqlParameter("@PartType", SqlDbType.VarChar));
                        sqlCtx.Params.Add("PartTypeGroup", new SqlParameter("@PartTypeGroup", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Now", new SqlParameter("@Now", SqlDbType.DateTime));
                        sqlCtx.Params.Add("NameKey", new SqlParameter("@NameKey", SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }

                sqlCtx.Params["PartType"].Value = item.PartTypeName;
                sqlCtx.Params["PartTypeGroup"].Value = item.PartTypeGroup;
                sqlCtx.Params["Editor"].Value = item.Editor;
                sqlCtx.Params["Now"].Value = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params["NameKey"].Value = strOldPartType;

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

        public new void DeletePartTypeByPartType(string partType)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @" delete from PartTypeAttribute 
                                                            where PartType = @PartType

                                                            delete from PartTypeDescription 
                                                            where PartType = @PartType

                                                            delete from PartTypeEx                                                                                                     
                                                            where PartType=@PartType";

                        sqlCtx.Params.Add("PartType", new SqlParameter("@PartType", SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }

                sqlCtx.Params["PartType"].Value = partType;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                sqlCtx.Params.Values.ToArray<SqlParameter>());
               
            }
            catch (Exception)
            {                
                throw;
            }
            finally
            {               
            }
            
        }
        #endregion


        #region IPartRepositoryEx
        public void SavePartEx(PartDef newPart, string oldPartNo)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @" UPDATE Part
                                                           SET [Descr] = @Descr
                                                                  ,[BomNodeType] = @BomNodeType
                                                                  ,[PartType] =@PartType
                                                                  ,[CustPartNo] = @CustPartNo
                                                                  ,[AutoDL] = @AutoDL
                                                                  ,[Remark] = @Remark
                                                                  ,[Editor] = @Editor
                                                                  ,[Udt] = @Udt
                                                               WHERE PartNo=@oldPartNo ";

                        sqlCtx.Params.Add("Descr", new SqlParameter("@Descr", SqlDbType.VarChar));
                        sqlCtx.Params.Add("BomNodeType", new SqlParameter("@BomNodeType", SqlDbType.VarChar));
                        sqlCtx.Params.Add("PartType", new SqlParameter("@PartType", SqlDbType.VarChar));
                        sqlCtx.Params.Add("CustPartNo", new SqlParameter("@CustPartNo", SqlDbType.VarChar));
                        sqlCtx.Params.Add("AutoDL", new SqlParameter("@AutoDL", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Remark", new SqlParameter("@Remark", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Udt", new SqlParameter("@Udt", SqlDbType.DateTime));
                        sqlCtx.Params.Add("oldPartNo", new SqlParameter("@oldPartNo", SqlDbType.VarChar));
                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }

                sqlCtx.Params["Descr"].Value = newPart.descr;
                sqlCtx.Params["BomNodeType"].Value = newPart.bomNodeType;
                sqlCtx.Params["PartType"].Value = newPart.partType;
                sqlCtx.Params["CustPartNo"].Value = newPart.custPartNo;
                sqlCtx.Params["AutoDL"].Value = newPart.autoDL;
                sqlCtx.Params["Remark"].Value = newPart.remark;
                sqlCtx.Params["Editor"].Value = newPart.editor;
                sqlCtx.Params["Udt"].Value = newPart.udt;
                sqlCtx.Params["oldPartNo"].Value = oldPartNo;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                sqlCtx.Params.Values.ToArray<SqlParameter>());


            }
            catch (Exception)
            {

                throw;
            }        

            InsertCacheUpdate(CacheTypeEnum.Part, oldPartNo);
        }

        public void SavePartExDefered(IUnitOfWork uow, PartDef newPart, string oldPartNo)
        {
          Action deferAction = () => { SavePartEx(newPart, oldPartNo); };
          AddOneInvokeBody(uow, deferAction);
        }

        public IList<string> GetBomNodeTypeList()
        {
            try
            {
                IList<string> ret = new List<string>();

                string SQLStatement = @"select * from
                                                            (
	                                                            select distinct BomNodeType from Part 
	                                                            union
	                                                            select distinct PartTypeGroup from PartTypeEx
                                                            ) t
                                                             order by t.BomNodeType ";
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text, SQLStatement))
                {
                    while (sqlR != null && sqlR.Read())
                    {                       
                        ret.Add(GetValue_Str(sqlR, 0));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<string> GetPartTypeList(string bomNodeType)
        {
            try
            {
                IList<string> ret = new List<string>();

                 
                 _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @"select distinct PartType from PartTypeEx 
                                                            where PartTypeGroup = @Group ";

                        sqlCtx.Params.Add("Group", new SqlParameter("@Group", SqlDbType.VarChar));
                       _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }

                 sqlCtx.Params["Group"].Value =bomNodeType;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text,
                                                                                                                              sqlCtx.Sentence,
                                                                                                                              sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ret.Add(GetValue_Str(sqlR, 0));
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IList<PartDef> GetPartListByPartType(string partType, int rowCount)
        {
            try
            {
                IList<PartDef> ret = new List<PartDef>();
                string SQLStatement = @"select top {1} PartNo, BomNodeType,Descr, PartType, CustPartNo, AutoDL, Remark,
                                                           Flag, Editor, Cdt, Udt  from Part with(NOLOCK) where PartType = '{0}' order by PartNo";

                SQLStatement = string.Format(SQLStatement, partType, rowCount.ToString());

                using (DataTable dt = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text, SQLStatement))
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            PartDef part = new PartDef();
                            part.partNo = ObjToString(dr["PartNo"]);
                            part.bomNodeType = ObjToString(dr["BomNodeType"]);
                            part.partType = ObjToString(dr["PartType"]);
                            part.custPartNo = ObjToString(dr["CustPartNo"]);
                            part.descr = ObjToString(dr["Descr"]);
                            part.remark = ObjToString(dr["Remark"]);
                            part.autoDL = ObjToString(dr["AutoDL"]);
                            part.editor = ObjToString(dr["Editor"]);
                            part.cdt = DateTime.Parse(dr["Cdt"].ToString());
                            part.udt = DateTime.Parse(dr["Udt"].ToString());
                            ret.Add(part);
                        }
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IList<string> GetProductsFromProduct_Part(string partNo, int rowCount)
        {
            try
            {
                IList<string> ret = new List<string>();
                string SQLStatement = @"select top {1} ProductID from Product_Part where PartNo = '{0}' ";

                SQLStatement = string.Format(SQLStatement, partNo, rowCount.ToString());

                using (DataTable dt = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text, SQLStatement))
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string ProductID = ObjToString(dr["ProductID"]);
                            ret.Add(ProductID);
                        }
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePartEx(string partNo)
        {
            try
            {
                string SQLStatement = @"delete from PartInfo where PartNo='{0}'; delete from Part where PartNo='{0}';";
                SQLStatement = string.Format(SQLStatement, partNo);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                    CommandType.Text,
                    SQLStatement);
            }
            catch (Exception)
            {
                throw;
            }
            InsertCacheUpdate(CacheTypeEnum.Part, partNo);
        }

        public void DeletePartExDefered(IUnitOfWork uow, string partNo)
        {
            Action deferAction = () => { DeletePartEx(partNo); };
            AddOneInvokeBody(uow, deferAction);
        }

        public void InsertCacheUpdate(CacheTypeEnum cacheType, string item)
        {            
            try
            {
                IList<string> ret = new List<string>();
                MethodBase mb = MethodBase.GetCurrentMethod();

                _Schema.SQLContext sqlCtx = null;
                lock (mb)
                {
                    if (!_Schema.Func.PeerTheSQL(mb.MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @"merge CacheUpdate as T
                                                        using (select @Type,@Item,0, a.ServerIP,a.AppName
                                                               from  CacheUpdateServer a) as S([Type],Item,Updated,CacheServerIP,AppName)
                                                               on T.[Type]=S.[Type] and T.Item= S.Item and 
                                                                    T.CacheServerIP =S.CacheServerIP and T.AppName=S.AppName
                                                         WHEN MATCHED and T.Updated=1 THEN
                                                            UPDATE SET Updated = 0,
                                                                       Udt=getdate()	      
                                                         WHEN NOT MATCHED THEN
                                                            INSERT ([Type], Item, Updated, CacheServerIP, AppName, Cdt, Udt)
                                                            VALUES (S.[Type], S.Item, S.Updated, S.CacheServerIP, S.AppName, getdate(), getdate());";

                        sqlCtx.Params.Add("Type", new SqlParameter("@Type", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Item", new SqlParameter("@Item", SqlDbType.VarChar));
                        _Schema.Func.InsertIntoCache(mb.MetadataToken, sqlCtx);
                    }
                }

                sqlCtx.Params["Type"].Value = cacheType.ToString();
                sqlCtx.Params["Item"].Value = item;

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

        public void InsertCacheUpdateDefered(IUnitOfWork uow, CacheTypeEnum cacheType, string item)
        {
            Action deferAction = () => { InsertCacheUpdate(cacheType, item); };
            AddOneInvokeBody(uow, deferAction);
        }
        #endregion
    }
}
