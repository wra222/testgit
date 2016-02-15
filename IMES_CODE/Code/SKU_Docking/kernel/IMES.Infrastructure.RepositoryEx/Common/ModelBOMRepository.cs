﻿//For maintain BOM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Util;
using System.Data;
using IMES.Infrastructure.UnitOfWork;
using System.Reflection;
using System.Data.SqlClient;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.Utility;
using IMES.DataModel;
using System.Configuration;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.FisBOM;
using IMES.Infrastructure.Repository;
using IMES.FisObject.Common.ModelBOM;


namespace IMES.Infrastructure.Repository.Common
{
    ///<summary>
    ///数据访问与持久化类: BOM相关
    ///</summary>
    public class ModelBOMRepository : BaseRepository<BOMNodeData>, IModelBOMRepository
    {
        private static readonly IPartRepositoryEx partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepositoryEx>();

        #region Overrides of BaseRepository<BOMNodeData>

        protected override void PersistNewItem(BOMNodeData item)
        {
            throw new NotImplementedException("Normal");
        }

        protected override void PersistUpdatedItem(BOMNodeData item)
        {
            throw new NotImplementedException("Normal");
        }

        protected override void PersistDeletedItem(BOMNodeData item)
        {
            throw new NotImplementedException("Normal");
        }

        #endregion

        #region Implementation of IRepository<BOMNodeData>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override BOMNodeData Find(object key)
        {
            try
            {

                BOMNodeData ret = null;



                //SQL statement
                string sqlStr = @"select ID, Material, Plant, Component, Quantity, 
                                                        Alternative_item_group, Priority, Flag, Editor, Cdt, 
                                                        Udt
                                            from ModelBOM
                                            where Material=@Material and
                                                        Flag=1";
                SqlParameter Para = new SqlParameter("@Material", SqlDbType.VarChar);
                Para.Direction = ParameterDirection.Input;
                Para.Value = key.ToString();
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                                                                CommandType.Text,
                                                                                                                                sqlStr,
                                                                                                                               Para))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new BOMNodeData();

                        ret.ID = GetValue_Int32(sqlR, 0);
                        ret.Material = GetValue_Str(sqlR, 1);
                        ret.Plant = GetValue_Str(sqlR, 2);
                        ret.Component = GetValue_Str(sqlR, 3);
                        ret.Quantity = GetValue_Str(sqlR, 4);
                        ret.Alternative_item_group = GetValue_Str(sqlR, 5);
                        ret.Priority = GetValue_Str(sqlR, 6);
                        ret.Flag = GetValue_Int32(sqlR, 7);
                         ret.Editor = GetValue_Str(sqlR, 8);
                        ret.Cdt = GetValue_DateTime(sqlR, 9);
                        ret.Udt = GetValue_DateTime(sqlR, 10);
                       

                        ret.Tracker.Clear();
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取所有对象列表
        /// </summary>
        /// <returns>所有对象列表</returns>
        public override IList<BOMNodeData> FindAll()
        {
            throw new NotImplementedException("Normal");
        }

        /// <summary>
        /// 添加一个对象
        /// </summary>
        /// <param name="item">新添加的对象</param>
        public override void Add(BOMNodeData item, IUnitOfWork work)
        {
            base.Add(item, work);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        public override void Remove(BOMNodeData item, IUnitOfWork work)
        {
            base.Remove(item, work);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="work"></param>
        public override void Update(BOMNodeData item, IUnitOfWork work)
        {
            base.Update(item, work);
        }

        #endregion


        #region Implement of IModelBOMRepository

        public DataTable GetTreeTable(string model, int limitCount, ref int getStatus)
        {
            ///  DataTable column Name: Id, Material, Component, Descr, IsPart, IsModel, level
            try
            {
                DataTable ret = null;

                //SQL statement
                string sqlStr = @"select ID as Id,
                                               Material,
                                               Component,
                                               Descr,
                                               (case MBomNodeType 
                                               when null then
                                                    0
                                               else
                                                    1
                                               end) as IsPart,
                                                (case MBomNodeType 
                                               when null then
                                                    1
                                               else
                                                    0
                                               end) as IsModel,         
                                               [Level] as level,
                                            BomNodeType, PartType
                                        from dbo.fn_ExpandBom_ModelBOM(@model)";

                SqlParameter[] paramsArray = new SqlParameter[1];
                paramsArray[0] = new SqlParameter("@model", SqlDbType.VarChar);
                paramsArray[0].Value = model;
             
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                    CommandType.Text, 
                                                                                    sqlStr, 
                                                                                    paramsArray);
                getStatus = 0;
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<BOMNodeData> findModelBomByMaterialAndComponent(string parentCode, string oldCode)
        {
            //Select * from ModelBom where Material='" + parentCode + "' and Component='" + oldCode + "'"
            try
            {
                IList<BOMNodeData> ret = new List<BOMNodeData>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {

                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @"select ID, Material, Plant, Component, Quantity, 
                                                                        Alternative_item_group, Priority, Flag, Editor, Cdt, 
                                                                        Udt
                                                        from ModelBOM with(NOLOCK)
                                                        where Material=@Material and
                                                                   Component =@Component and 
                                                                    Flag=1 ";

                        sqlCtx.Params.Add("Material", new SqlParameter("@Material", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Component", new SqlParameter("@Component", SqlDbType.VarChar));
                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);

                       
                    }
                }

                sqlCtx.Params["Material"].Value = parentCode;
                sqlCtx.Params["Component"].Value = oldCode;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            BOMNodeData  item = new BOMNodeData();

                            item.ID = GetValue_Int32(sqlR, 0);
                            item.Material = GetValue_Str(sqlR, 1);
                            item.Plant = GetValue_Str(sqlR, 2);
                            item.Component = GetValue_Str(sqlR, 3);
                            item.Quantity = GetValue_Str(sqlR, 4);
                            item.Alternative_item_group = GetValue_Str(sqlR, 5);
                            item.Priority = GetValue_Str(sqlR, 6);
                            item.Flag = GetValue_Int32(sqlR, 7);
                            item.Editor = GetValue_Str(sqlR, 8);
                            item.Cdt = GetValue_DateTime(sqlR, 9);
                            item.Udt = GetValue_DateTime(sqlR, 10);


                            item.Tracker.Clear();
                            ret.Add(item);
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


        public DataTable GetExistPartNo(string code)
        {
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @"select PartNo, Descr, BomNodeType, PartType, CustPartNo,
                                                                        AutoDL, Remark, Flag, Editor, Cdt, Udt
                                                            from Part with(NOLOCK)
                                                            where PartNo=@PartNo and
                                                                  Flag=1 ";

                        sqlCtx.Params.Add("PartNo", new SqlParameter("@PartNo", SqlDbType.VarChar));
                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);

                    }
                }
                sqlCtx.Params["PartNo"].Value = code;
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, 
                                                                                      CommandType.Text, 
                                                                                      sqlCtx.Sentence,
                                                                                      sqlCtx.Params.Values.ToArray<SqlParameter>());
             
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public DataTable GetPartTypeInfoByCode(string code)
        {
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();

                    
                        sqlCtx.Sentence = @"SELECT PartNo, Descr, BomNodeType, PartType, CustPartNo,
                                                                            AutoDL, Remark, Flag, Editor, Cdt, Udt 
                                                        FROM Part with(NOLOCK)
                                                        WHERE PartNo=@PartNo and 
                                                                        Flag=1 ";



                        sqlCtx.Params.Add("PartNo", new SqlParameter("@PartNo" , SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
                sqlCtx.Params["PartNo"].Value = code;
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, 
                                                                                     CommandType.Text, 
                                                                                     sqlCtx.Sentence, 
                                                                                     sqlCtx.Params.Values.ToArray<SqlParameter>());
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public DataTable GetMaterialInfo(string pn)
        {
            //SELECT [Model] AS PartNo,'' AS Descr,
            //       0 AS WHERETYPE
            //      ,[BOMApproveDate] AS [BOMApproveDate]
            //      ,[Editor]
            //      ,[Cdt]
            //      ,[Udt]
            //  FROM [IMES_GetData_Datamaintain].[dbo].[Model]
            //WHERE [Model]= 'pn'
            //union 
            //SELECT [PartNo] AS PartNo,Descr AS Descr,
            //       1 AS WHERETYPE,
            //       GetDate() AS [BOMApproveDate]
            //      ,[Editor]
            //      ,[Cdt]
            //      ,[Udt]
            //  FROM [IMES_GetData_Datamaintain].[dbo].[Part]
            //WHERE [PartNo]='pn' AND Flag=1
            //order by WHERETYPE
            try
            {
                DataTable ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();

                        //sqlCtx.Sentence =       "SELECT {2} AS {7},'' AS {12} " + 
                        //                            ",0 AS WHERETYPE " + 
                        //                            ",{3} AS {3} " + 
                        //                            ",{4} " + 
                        //                            ",{5} " + 
                        //                            ",{6} " + 
                        //                        "FROM {0} " + 
                        //                        "WHERE {2}=@{7} " + 
                        //                    "UNION " +
                        //                        "SELECT {7} AS {7},{12} AS {12} " +
                        //                            ",1 AS WHERETYPE " + 
                        //                            ",GetDate() AS {3} " + 
                        //                            ",{8} " + 
                        //                            ",{9} " + 
                        //                            ",{10} " + 
                        //                        "FROM {1} " +
                        //                        "WHERE {7}=@{7} AND {11}=1 " + 
                        //                    "ORDER BY WHERETYPE ";

                        //sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.Model).Name,
                        //                                                typeof(_Schema.Part).Name,
                        //                                                _Schema.Model.fn_model,
                        //                                                _Schema.Model.fn_BOMApproveDate,
                        //                                                _Schema.Model.fn_Editor,
                        //                                                _Schema.Model.fn_Cdt,
                        //                                                _Schema.Model.fn_Udt,
                        //                                                _Schema.Part.fn_PartNo,
                        //                                                _Schema.Part.fn_Editor,
                        //                                                _Schema.Part.fn_Cdt,
                        //                                                _Schema.Part.fn_Udt,
                        //                                                _Schema.Part.fn_Flag,
                        //                                                _Schema.Part.fn_Descr);

                        sqlCtx.Sentence = @"SELECT Model AS PartNo,
                                                                 Family AS Descr ,                  
                                                                 0 AS WHERETYPE ,
                                                                 BOMApproveDate AS BOMApproveDate ,
                                                                 Editor ,
                                                                 Cdt ,
                                                                 Udt,
                                                                 'PC' as BomNodeType,
                                                                 'Model' as PartType  
                                                          FROM Model with(NOLOCK)
                                                          WHERE Model=@PartNo 
                                                          UNION 
                                                          SELECT PartNo AS PartNo,
                                                                 Descr AS Descr ,
                                                                 1 AS WHERETYPE ,
                                                                 GetDate() AS BOMApproveDate ,
                                                                 Editor ,
                                                                 Cdt ,
                                                                 Udt ,
                                                                 BomNodeType,
                                                                 PartType
                                                           FROM Part with(NOLOCK)
                                                           WHERE PartNo=@PartNo
                                                                 AND Flag=1 
                                                           ORDER BY WHERETYPE ";
                      

                        sqlCtx.Params.Add("PartNo", new SqlParameter("@PartNo" , SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
                sqlCtx.Params["PartNo"].Value = pn;
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, 
                                                                                    CommandType.Text, 
                                                                                    sqlCtx.Sentence, 
                                                                                    sqlCtx.Params.Values.ToArray<SqlParameter>());
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public BOMNodeData GetModelBOM(string model)
        {
            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
            try
            {

                return this.Find(model);
            }
            catch (Exception)
            {
                LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
                throw;
            }
            finally
            {
                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
            }
        }



        public BOMNodeData GetModelBOM(int modelBOMId)
        {
            try
            {

                BOMNodeData ret = null;



                //SQL statement
                string sqlStr = @"select ID, Material, Plant, Component, Quantity, 
                                                        Alternative_item_group, Priority, Flag, Editor, Cdt, 
                                                        Udt
                                            from ModelBOM with(NOLOCK)
                                            where ID=@ID and
                                                        Flag=1";
                SqlParameter Para = new SqlParameter("@ID", SqlDbType.Int);
                Para.Direction = ParameterDirection.Input;
                Para.Value = modelBOMId;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                                                                CommandType.Text,
                                                                                                                                sqlStr,
                                                                                                                               Para))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new BOMNodeData();

                        ret.ID = GetValue_Int32(sqlR, 0);
                        ret.Material = GetValue_Str(sqlR, 1);
                        ret.Plant = GetValue_Str(sqlR, 2);
                        ret.Component = GetValue_Str(sqlR, 3);
                        ret.Quantity = GetValue_Str(sqlR, 4);
                        ret.Alternative_item_group = GetValue_Str(sqlR, 5);
                        ret.Priority = GetValue_Str(sqlR, 6);
                        ret.Flag = GetValue_Int32(sqlR, 7);
                        ret.Editor = GetValue_Str(sqlR, 8);
                        ret.Cdt = GetValue_DateTime(sqlR, 9);
                        ret.Udt = GetValue_DateTime(sqlR, 10);


                        ret.Tracker.Clear();
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        

        public DataTable GetComponentByMaterial(string code)
        {
            //SELECT distinct Component from ModelBOM where Material=@code

            try
            {

                DataTable ret = null;

                //SQL statement
                string sqlStr = @"select Distinct Component 
                                            from ModelBOM with(NOLOCK)
                                            where Material=@Material and
                                                        Flag=1";
                SqlParameter Para = new SqlParameter("@Material", SqlDbType.VarChar);
                Para.Direction = ParameterDirection.Input;
                Para.Value = code;

                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, 
                                                                                         CommandType.Text,
                                                                                         sqlStr,
                                                                                         Para);
                return ret;
               
            }
            catch (Exception)
            {
                throw;
            }           
        }


        public DataTable GetComponentByMaterial(IList<string> codeList)
        {
            //SELECT distinct Component from ModelBOM where Material in @code

            try
            {

                DataTable ret = null;

                //SQL statement
                string sqlStr = @"select Distinct Component 
                                            from ModelBOM with(NOLOCK)
                                            where Material in ('{0}') and
                                                        Flag=1";

                sqlStr = string.Format(sqlStr,string.Join ("','", codeList.ToArray() ) );
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                         CommandType.Text,
                                                                                         sqlStr);
                return ret;

            }
            catch (Exception)
            {
                throw;
            }           
        }

        public DataTable GetParentInfo(IList<string> currentComponents)
        {
            try
            {
                DataTable ret = null;

                if (currentComponents != null && currentComponents.Count > 0)
                {
                    _Schema.SQLContext sqlCtx = null;
                        lock (MethodBase.GetCurrentMethod())
                        {
                            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                            {
                                sqlCtx = new _Schema.SQLContext();


                                sqlCtx.Sentence = @"SELECT DISTINCT a.Material, b.Descr, c.Flag, c.ApproveDate FROM ModelBOM AS a with(NOLOCK)
LEFT OUTER JOIN 
(SELECT Descr, PartNo FROM Part with(NOLOCK) WHERE Flag=1) AS b 
ON a.Material=b.PartNo 
LEFT OUTER JOIN 
(SELECT 'Model' AS Flag, BOMApproveDate AS ApproveDate, Model FROM Model with(NOLOCK) ) AS c 
ON a.Material=c.Model 
WHERE a.Component in ('{0}') AND a.Flag=1 ";                       

                                _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                            }
                        }
                        //sqlCtx.Sentence=string.Format(sqlCtx.Sentence, string.Join("' , '",currentComponents.ToArray()));
                        string SQLStatement = string.Format(sqlCtx.Sentence, string.Join("' , '", currentComponents.ToArray()));     
                        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, 
                                                                                     CommandType.Text,
                                                                                     SQLStatement);                   
                }
               
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public DataTable GetSubModelBOMByCode(IList<int> idList)
        {
            try
            {

                DataTable ret = null;

                if (idList != null && idList.Count > 0)
                {
                    _Schema.SQLContext sqlCtx = null;
                        lock (MethodBase.GetCurrentMethod())
                        {
                            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                            {
                                sqlCtx = new _Schema.SQLContext();


                                sqlCtx.Sentence = @"SELECT a.Component AS Material, c.BomNodeType, c.PartType, c.Descr, a.Quantity,
a.Alternative_item_group, a.Flag
,a.Editor, a.Cdt, a.Udt, a.ID, a.Priority  FROM ModelBOM AS a with(NOLOCK)
LEFT OUTER JOIN 
(
SELECT DISTINCT Material AS Code, '' AS PartType, 0 AS dataFromType FROM ModelBOM with(NOLOCK) --WHERE Flag=1
) AS b ON b.Code=a.Component 
LEFT OUTER JOIN 
(
SELECT PartNo AS Code, BomNodeType, PartType, Descr 
FROM Part with(NOLOCK) --WHERE Flag=1
) AS c ON c.Code=a.Component 
WHERE  a.ID IN ({0}) 
ORDER BY a.Alternative_item_group, a.Priority";                       

                                _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                            }
                        }

                        string inSQLStr = string.Join(",", idList.Select(i => i.ToString()).ToArray());
                        //sqlCtx.Sentence = string.Format(sqlCtx.Sentence, inSQLStr);
                        string SQLStatement = string.Format(sqlCtx.Sentence, inSQLStr);
                        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, 
                                                                                     CommandType.Text,
                                                                                    SQLStatement);        

             
                }
               
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void SaveModelBOMAs(string oldCode, string newCode, string editor)
        {            
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();

//                        sqlCtx.Sentence = @"--标志删除[ModelBOM]
//                                                            UPDATE [ModelBOM]
//                                                               SET [Flag] = 0
//                                                                  ,[Editor] = @Editor
//                                                                  ,[Cdt] = GetDate()
//                                                                  ,[Udt] = GetDate()
//                                                             WHERE Material=@newCode
//                                                            
//                                                            --真正删除过去标志删除了的目前新加的值对的记录
//                                                            DELETE FROM [ModelBOM] WHERE Material=@newCode AND Component IN (
//                                                            select Component FROM ModelBOM where Material=@oldCode AND Flag=0 )
//
//                                                            INSERT ModelBOM (Material, Plant, Component, Quantity, Alternative_item_group, 
//                                                                             Priority, Flag, Editor, Cdt, Udt)
//                                                            select @newCode, Plant, Component, Quantity, Alternative_item_group, 
//                                                                   Priority, Flag, @Editor, GETDATE(), GETDATE()
//                                                            FROM ModelBOM with(NOLOCK)
//                                                            where Material=@oldCode AND 
//                                                                  Flag=1 ";

                        sqlCtx.Sentence = @"INSERT ModelBOM (Material, Plant, Component, Quantity, Alternative_item_group, 
                                                                                        Priority, Flag, Editor, Cdt, Udt)
                                                            select @newCode, Plant, Component, Quantity, Alternative_item_group, 
                                                                                           Priority, Flag, @Editor, GETDATE(), GETDATE()
                                                                                    FROM ModelBOM with(NOLOCK)
                                                                                    where Material=@oldCode";

                        sqlCtx.Params.Add("oldCode", new SqlParameter("@oldCode", SqlDbType.VarChar));
                        sqlCtx.Params.Add("newCode", new SqlParameter("@newCode", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
                sqlCtx.Params["oldCode"].Value = oldCode;
                sqlCtx.Params["newCode"].Value = newCode;
                sqlCtx.Params["Editor"].Value = editor;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                  CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params.Values.ToArray<SqlParameter>());
               
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void SaveModelBOMAsDefered(IUnitOfWork uow, string oldCode, string newCode, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), oldCode, newCode, editor);
        }

        public DataTable GetMaterialById(string code)
        {
            //SELECT Material,Material_group FROM ModelBOM where Material=@code
            try
            {
                DataTable ret = null;     

               
                //SQL statement
                string sqlStr = @"select ID, Material, Plant, Component, Quantity, 
                                                        Alternative_item_group, Priority, Flag, Editor, Cdt, 
                                                        Udt
                                            from ModelBOM with(NOLOCK)
                                            where Material=@Material and
                                                        Flag=1";

                SqlParameter Para= new SqlParameter("@Material", SqlDbType.VarChar);
                 Para.Direction = ParameterDirection.Input;
                 Para.Value = code;
               
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                                                                CommandType.Text,
                                                                                                                                sqlStr,
                                                                                                                                Para);
                
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteRefreshModelByModel(string model, string editor)
        {
            //Delete From RefreshModel where Model='model'AND Editor='editor'
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();

                        sqlCtx.Sentence = @" Delete from RefreshModel where Model=@model AND Editor=@Editor  ";



                        sqlCtx.Params.Add("model", new SqlParameter("@model", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                    
                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
                sqlCtx.Params["model"].Value = model;
                sqlCtx.Params["Editor"].Value = editor;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, 
                                                                                 CommandType.Text, 
                                                                                 sqlCtx.Sentence, 
                                                                                 sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteRefreshModelByModelDefered(IUnitOfWork uow, string model, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), model, editor);
        }

        public void AddRefreshModel(string model, string editor)
        {
            //INSERT INTO RefreshModel (Model, Editor) VALUES ('model','editor')
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();

                        sqlCtx.Sentence = @" insert RefreshModel(Model, Editor)  values(@model, @Editor)  ";



                        sqlCtx.Params.Add("model", new SqlParameter("@model", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
                sqlCtx.Params["model"].Value = model;
                sqlCtx.Params["Editor"].Value = editor;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                 CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
            partRep.InsertCacheUpdate(CacheTypeEnum.BOM, model);
        }

        public void AddRefreshModelDefered(IUnitOfWork uow, string model, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), model, editor);
        }

        public void AddModelBOM(BOMNodeData item)
        {
            try
            {
                //GYB: 新增记录的时候，需要按照Material/Component/Flag='0' 检查是否存在删除记录,如果有则Update,否则Insert
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();

                        sqlCtx.Sentence = @"If exists(select * from ModelBOM with(NOLOCK) where Material=@Material and Component=@Component and Flag=0)
                                                            begin    
                                                                UPDATE [ModelBOM]
                                                                   SET Material=@Material,
                                                                            Plant=@Plant,
                                                                            Component=@Component,
                                                                            Quantity =@Quantity, 
                                                                            Alternative_item_group=@Alternative_item_group,
                                                                            Priority = @Priority,
                                                                            Flag=1,   
                                                                            [Editor] = @Editor,
                                                                            [Udt] = GetDate()
                                                                 WHERE ID in ( select top 1 ID
                                                                                          from [ModelBOM] with(NOLOCK)
                                                                                          where Material=@Material and 
                                                                                                     Component=@Component and 
                                                                                                      Flag=0
                                                                                          order by Udt  
                                                                                          )                                                           
                                                           
                                                            end
                                                            else
                                                            begin
                                                                INSERT ModelBOM (Material, Plant, Component, Quantity, Alternative_item_group, 
                                                                                 Priority, Flag, Editor, Cdt, Udt)
                                                                values (@Material, @Plant, @Component, @Quantity, @Alternative_item_group, 
                                                                            @Priority, 1, @Editor, GETDATE(), GETDATE())
                                                               
                                                           end";



                        sqlCtx.Params.Add("Material", new SqlParameter("@Material", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Plant", new SqlParameter("@Plant", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Component", new SqlParameter("@Component", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Quantity", new SqlParameter("@Quantity", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Alternative_item_group", new SqlParameter("@Alternative_item_group", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Priority", new SqlParameter("@Priority", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
                sqlCtx.Params["Material"].Value = item.Material;
                sqlCtx.Params["Plant"].Value = item.Plant;
                sqlCtx.Params["Component"].Value = item.Component;
                sqlCtx.Params["Quantity"].Value = item.Quantity;
                sqlCtx.Params["Alternative_item_group"].Value = item.Alternative_item_group;
                sqlCtx.Params["Priority"].Value = item.Priority;
                sqlCtx.Params["Editor"].Value = item.Editor;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                  CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params.Values.ToArray<SqlParameter>());            
            }
            catch (Exception)
            {
              
                throw;
            }
            partRep.InsertCacheUpdate(CacheTypeEnum.BOM, item.Material);
        }

        public void AddModelBOMDefered(IUnitOfWork uow, BOMNodeData item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeleteModelBomByMaterialAndComponent(string parentCode, string oldCode)
        {
            //DELETE FROM [IMES_GetData_Datamaintain].[dbo].[ModelBOM]
            //where Material='" + parentCode + "' and Component='" + oldCode + "' AND Flag=1
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();

                        sqlCtx.Sentence = @"Delete from ModelBOM
                                                            where Material = @Material and
                                                                       Component = @Component and
                                                                       Flag=0";


                        sqlCtx.Params.Add("Material", new SqlParameter("@Material", SqlDbType.VarChar));                        
                        sqlCtx.Params.Add("Component", new SqlParameter("@Component", SqlDbType.VarChar));
                        

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
                sqlCtx.Params["Material"].Value = parentCode;
                sqlCtx.Params["Component"].Value = oldCode;
              
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                  CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params.Values.ToArray<SqlParameter>());         
            }
            catch (Exception)
            {
                throw;
            }

            partRep.InsertCacheUpdate(CacheTypeEnum.BOM, parentCode);
        }

        public void DeleteModelBomByMaterialAndComponentDefered(IUnitOfWork uow, string parentCode, string oldCode)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), parentCode, oldCode);
        }
              

        public void UpdateModelBOM(BOMNodeData item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();

                        sqlCtx.Sentence = @"  UPDATE [ModelBOM]
                                                                   SET Material=@Material,
                                                                            Plant=@Plant,
                                                                            Component=@Component,
                                                                            Quantity =@Quantity, 
                                                                            Alternative_item_group=@Alternative_item_group,
                                                                            Priority = @Priority,
                                                                            Flag=@Flag,   
                                                                            [Editor] = @Editor,
                                                                            [Udt] = GetDate()
                                                                 WHERE ID =@ID";                                                         
                                                         



                        sqlCtx.Params.Add("Material", new SqlParameter("@Material", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Plant", new SqlParameter("@Plant", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Component", new SqlParameter("@Component", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Quantity", new SqlParameter("@Quantity", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Alternative_item_group", new SqlParameter("@Alternative_item_group", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Priority", new SqlParameter("@Priority", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Flag", new SqlParameter("@Flag", SqlDbType.Int));
                        sqlCtx.Params.Add("ID", new SqlParameter("@ID", SqlDbType.Int));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
                sqlCtx.Params["Material"].Value = item.Material;
                sqlCtx.Params["Plant"].Value = item.Plant;
                sqlCtx.Params["Component"].Value = item.Component;
                sqlCtx.Params["Quantity"].Value = item.Quantity;
                sqlCtx.Params["Alternative_item_group"].Value = item.Alternative_item_group;
                sqlCtx.Params["Priority"].Value = item.Priority;
                sqlCtx.Params["Editor"].Value = item.Editor;
                sqlCtx.Params["ID"].Value = item.ID;
                sqlCtx.Params["Flag"].Value = item.Flag;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                  CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params.Values.ToArray<SqlParameter>());            
            }
            catch (Exception)
            {
                throw;
            }

            partRep.InsertCacheUpdate(CacheTypeEnum.BOM, item.Material);
        }

        public void UpdateModelBOMDefered(IUnitOfWork uow, BOMNodeData item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateGroupQuantity(string qty, string groupNo, string editor)
        {
            //UPDATE [IMES_GetData_Datamaintain].[dbo].[ModelBOM]
            //SET  [Quantity] = 'qty'
            //,[Editor] = 'editor'
            //,[Udt] = getdate()
            //WHERE [Alternative_item_group]='groupNo' And [Quantity]<> 'qty' and Flag=1
            try
            {

                  _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();

                        sqlCtx.Sentence = @"  UPDATE [ModelBOM]
                                                                   SET Material=@Material,
                                                                            Plant=@Plant,
                                                                            Component=@Component,
                                                                            Quantity =@Quantity, 
                                                                            Alternative_item_group=@Alternative_item_group,
                                                                            Priority = @Priority,
                                                                            Flag=@Flag,   
                                                                            [Editor] = @Editor,
                                                                            [Udt] = GetDate()
                                                                 WHERE Alternative_item_group =@Alternative_item_group and
                                                                                Quantity<>@Quantity and
                                                                                Flag=@Flag";                                                         
                                                         



                        //sqlCtx.Params.Add("Material", new SqlParameter("@Material", SqlDbType.VarChar));
                        //sqlCtx.Params.Add("Plant", new SqlParameter("@Plant", SqlDbType.VarChar));
                        //sqlCtx.Params.Add("Component", new SqlParameter("@Component", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Quantity", new SqlParameter("@Quantity", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Alternative_item_group", new SqlParameter("@Alternative_item_group", SqlDbType.VarChar));
                        //sqlCtx.Params.Add("Priority", new SqlParameter("@Priority", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Flag", new SqlParameter("@Flag", SqlDbType.Int));
                        //sqlCtx.Params.Add("ID", new SqlParameter("@ID", SqlDbType.Int));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
                //sqlCtx.Params["Material"].Value = item.Material;
                //sqlCtx.Params["Plant"].Value = item.Plant;
                //sqlCtx.Params["Component"].Value = item.Component;

                sqlCtx.Params["Quantity"].Value = qty;
                sqlCtx.Params["Alternative_item_group"].Value = groupNo;
                sqlCtx.Params["Editor"].Value = editor;


                //sqlCtx.Params["ID"].Value = item.ID;
                sqlCtx.Params["Flag"].Value = 1;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                  CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params.Values.ToArray<SqlParameter>());   



               
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void UpdateGroupQuantityDefered(IUnitOfWork uow, string qty, string groupNo, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), qty, groupNo, editor);
        }


        public void RemoveModelBOMById(int id, string editor)
        {
            try
            {

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();

                        sqlCtx.Sentence = @"  UPDATE [ModelBOM]
                                                                   SET  Flag=@Flag,   
                                                                            [Editor] = @Editor,
                                                                            [Udt] = GetDate()
                                                                 WHERE ID =@ID and
                                                                                Flag=1";
                        //sqlCtx.Params.Add("Material", new SqlParameter("@Material", SqlDbType.VarChar));
                        //sqlCtx.Params.Add("Plant", new SqlParameter("@Plant", SqlDbType.VarChar));
                        //sqlCtx.Params.Add("Component", new SqlParameter("@Component", SqlDbType.VarChar));
                        //sqlCtx.Params.Add("Quantity", new SqlParameter("@Quantity", SqlDbType.VarChar));
                        //sqlCtx.Params.Add("Alternative_item_group", new SqlParameter("@Alternative_item_group", SqlDbType.VarChar));
                        //sqlCtx.Params.Add("Priority", new SqlParameter("@Priority", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Flag", new SqlParameter("@Flag", SqlDbType.Int));
                        sqlCtx.Params.Add("ID", new SqlParameter("@ID", SqlDbType.Int));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }
                //sqlCtx.Params["Material"].Value = item.Material;
                //sqlCtx.Params["Plant"].Value = item.Plant;
                //sqlCtx.Params["Component"].Value = item.Component;
                //sqlCtx.Params["Quantity"].Value = item.Quantity;
                //sqlCtx.Params["Alternative_item_group"].Value = item.Alternative_item_group;
                sqlCtx.Params["Editor"].Value = editor;
                sqlCtx.Params["ID"].Value = id;
                sqlCtx.Params["Flag"].Value =0;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                  CommandType.Text,
                                                                                 sqlCtx.Sentence,
                                                                                 sqlCtx.Params.Values.ToArray<SqlParameter>()); 

              
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void RemoveModelBOMByIdDefered(IUnitOfWork uow, int id, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), id, editor);
        }


        public void SetNewAlternativeGroup(IList<int> ids, string editor)
        {
            //UPDATE ModelBOM SET Alternative_item_group=newid() where ID in(idlist) AND Flag=1
            try
            {
                if (ids != null && ids.Count > 0)
                {
                    Guid newGuid = Guid.NewGuid();
                    _Schema.SQLContext sqlCtx = null;
                    lock (MethodBase.GetCurrentMethod())
                    {
                        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                        {
                            sqlCtx = new _Schema.SQLContext();

                            sqlCtx.Sentence = @"  UPDATE [ModelBOM]
                                                                   SET    Alternative_item_group=@Alternative_item_group,
                                                                             [Editor] = @Editor,
                                                                             [Udt] = GetDate()
                                                                 WHERE ID in ( {0} )";

                            sqlCtx.Params.Add("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                            sqlCtx.Params.Add("Alternative_item_group", new SqlParameter("@Alternative_item_group", SqlDbType.VarChar));
                            _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                        }
                    }
                   

                     string inSQLStr = string.Join(",", ids.Select(i => i.ToString()).ToArray());

                     //sqlCtx.Sentence = string.Format(sqlCtx.Sentence, inSQLStr);
                     string SQLStatement = string.Format(sqlCtx.Sentence, inSQLStr);
                     sqlCtx.Params["Alternative_item_group"].Value = newGuid.ToString();
                    sqlCtx.Params["Editor"].Value = editor;
                  

                    _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                      CommandType.Text,
                                                                                     SQLStatement,
                                                                                     sqlCtx.Params.Values.ToArray<SqlParameter>()); 
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void SetNewAlternativeGroupDefered(IUnitOfWork uow, IList<int> ids, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), ids, editor);
        }

        // CacheUpdate
        public void CacheUpdate_ForBOM(string component)
        {
            try
            {
                //SqlParameter[] paramsArray = new SqlParameter[1];
                //paramsArray[0] = new SqlParameter("@Component", SqlDbType.VarChar);
                //paramsArray[0].Value = component;

                //_Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM,
                //    CommandType.StoredProcedure, "IMES_CacheUpdate_ForBOM", paramsArray);

                partRep.InsertCacheUpdate(CacheTypeEnum.BOM,component);
             
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CacheUpdate_ForBOMDefered(IUnitOfWork uow, string component)
        {
            Action deferAction = () => { CacheUpdate_ForBOM(component); };
            AddOneInvokeBody(uow, deferAction);
        }
        // BOMNodeRelation

        // may contain parents which were not included in used roottype
        public IList<BOMNodeRelation> FindBOMNodeRelationParents(string ChildBOMNodeType)
        {
            try
            {
                IList<BOMNodeRelation> ret = new List<BOMNodeRelation>();

                SqlParameter[] paramsArray = new SqlParameter[1];
                paramsArray[0] = new SqlParameter("@ChildBOMNodeType", SqlDbType.VarChar);
                paramsArray[0].Value = ChildBOMNodeType;
                string SQLStatement = @"Select ID, BOMNodeType, ChildBOMNodeType, Descr, Editor, Cdt, Udt from BOMNodeRelation NOLOCK where 
                                        ChildBOMNodeType=@ChildBOMNodeType order by ID";
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                            CommandType.Text, SQLStatement, paramsArray))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        BOMNodeRelation item = new BOMNodeRelation();
                        item.ID = GetValue_Int32(sqlR, 0);
                        item.BOMNodeType = GetValue_Str(sqlR, 1);
                        item.ChildBOMNodeType = GetValue_Str(sqlR, 2);
                        item.Descr = GetValue_Str(sqlR, 3);
                        item.Editor = GetValue_Str(sqlR, 4);
                        item.Cdt = GetValue_DateTime(sqlR, 5);
                        item.Udt = GetValue_DateTime(sqlR, 6);
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

        public IList<BOMNodeRelation> FindBOMNodeRelationChild(string ParentType)
        {
            try
            {
                IList<BOMNodeRelation> ret = new List<BOMNodeRelation>();

                SqlParameter[] paramsArray = new SqlParameter[1];
                paramsArray[0] = new SqlParameter("@ParentType", SqlDbType.VarChar);
                paramsArray[0].Value = ParentType;
                string SQLStatement = @"Select ID, BOMNodeType, ChildBOMNodeType, Descr, Editor, Cdt, Udt from BOMNodeRelation NOLOCK where 
                                        BOMNodeType=@ParentType order by ID";
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                            CommandType.Text, SQLStatement, paramsArray))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        BOMNodeRelation item = new BOMNodeRelation();
                        item.ID = GetValue_Int32(sqlR, 0);
                        item.BOMNodeType = GetValue_Str(sqlR, 1);
                        item.ChildBOMNodeType = GetValue_Str(sqlR, 2);
                        item.Descr = GetValue_Str(sqlR, 3);
                        item.Editor = GetValue_Str(sqlR, 4);
                        item.Cdt = GetValue_DateTime(sqlR, 5);
                        item.Udt = GetValue_DateTime(sqlR, 6);
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

        public IList<BOMNodeRelation> FindBOMNodeRelationByPair(string BOMNodeType, string ChildBOMNodeType)
        {
            try
            {
                IList<BOMNodeRelation> ret = new List<BOMNodeRelation>();

                SqlParameter[] paramsArray = new SqlParameter[2];
                paramsArray[0] = new SqlParameter("@BOMNodeType", SqlDbType.VarChar);
                paramsArray[0].Value = BOMNodeType;
                paramsArray[1] = new SqlParameter("@ChildBOMNodeType", SqlDbType.VarChar);
                paramsArray[1].Value = ChildBOMNodeType;
                string SQLStatement = @"Select ID, BOMNodeType, ChildBOMNodeType, Descr, Editor, Cdt, Udt from BOMNodeRelation NOLOCK where 
                                        BOMNodeType=@BOMNodeType and ChildBOMNodeType=@ChildBOMNodeType order by ID";
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                            CommandType.Text, SQLStatement, paramsArray))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        BOMNodeRelation item = new BOMNodeRelation();
                        item.ID = GetValue_Int32(sqlR, 0);
                        item.BOMNodeType = GetValue_Str(sqlR, 1);
                        item.ChildBOMNodeType = GetValue_Str(sqlR, 2);
                        item.Descr = GetValue_Str(sqlR, 3);
                        item.Editor = GetValue_Str(sqlR, 4);
                        item.Cdt = GetValue_DateTime(sqlR, 5);
                        item.Udt = GetValue_DateTime(sqlR, 6);
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

        public DataTable FindBOMNodeRelationByRoot(string TYPE)
        {
            try
            {
                DataTable ret = null;

                //SQL statement
                string sqlStr = @"select ID,
                                           BOMNodeType,
                                           ChildBOMNodeType,
                                           Descr,
                                           [Level] as level,
                                           Editor
                                        from dbo.fn_ExpandBomRelation(@TYPE)";

                SqlParameter[] paramsArray = new SqlParameter[1];
                paramsArray[0] = new SqlParameter("@TYPE", SqlDbType.VarChar);
                paramsArray[0].Value = TYPE;

                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                    CommandType.Text,
                                                                                    sqlStr,
                                                                                    paramsArray);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable FindBOMNodeRelationParentsByRoot(string TYPE, string ChildBOMNodeType)
        {
            try
            {
                DataTable ret = null;

                //SQL statement
                string sqlStr = @"select Tree, ID,
                                           BOMNodeType,
                                           ChildBOMNodeType,
                                           Descr,
                                           [Level] as level,
                                           Editor
                                        from dbo.fn_ExpandBomRelation(@TYPE) where ChildBOMNodeType=@ChildBOMNodeType";

                SqlParameter[] paramsArray = new SqlParameter[2];
                paramsArray[0] = new SqlParameter("@TYPE", SqlDbType.VarChar);
                paramsArray[0].Value = TYPE;
                paramsArray[1] = new SqlParameter("@ChildBOMNodeType", SqlDbType.VarChar);
                paramsArray[1].Value = ChildBOMNodeType;

                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                    CommandType.Text,
                                                                                    sqlStr,
                                                                                    paramsArray);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<BOMNodeRelation> GetBOMNodeRelation(){
            try
            {
                IList<BOMNodeRelation> ret = new List<BOMNodeRelation>();

                string SQLStatement = "Select ID, BOMNodeType, ChildBOMNodeType, Descr, Editor, Cdt, Udt from BOMNodeRelation NOLOCK order by ID";
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text, SQLStatement))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        BOMNodeRelation item = new BOMNodeRelation();
                        item.ID = GetValue_Int32(sqlR, 0);
                        item.BOMNodeType = GetValue_Str(sqlR, 1);
                        item.ChildBOMNodeType = GetValue_Str(sqlR, 2);
                        item.Descr = GetValue_Str(sqlR, 3);
                        item.Editor = GetValue_Str(sqlR, 4);
                        item.Cdt = GetValue_DateTime(sqlR, 5);
                        item.Udt = GetValue_DateTime(sqlR, 6);
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


        public string AddBOMNodeRelation(BOMNodeRelation item){
            try
            {
                /*
                //GetAquireIdInsert
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(BOMNodeRelation));
                        sqlCtx.Params.Add("BOMNodeType", new SqlParameter("@BOMNodeType", SqlDbType.VarChar));
                        sqlCtx.Params.Add("ChildBOMNodeType", new SqlParameter("@ChildBOMNodeType", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Descr", new SqlParameter("@Descr", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Cdt", new SqlParameter("@Cdt", SqlDbType.DateTime));
                        sqlCtx.Params.Add("Udt", new SqlParameter("@Udt", SqlDbType.DateTime));
                        sqlCtx.Params.Add("ID", new SqlParameter("@ID", SqlDbType.Int));
                    }
                }

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params["BOMNodeType"].Value = item.BOMNodeType;
                sqlCtx.Params["ChildBOMNodeType"].Value = item.ChildBOMNodeType;
                sqlCtx.Params["Descr"].Value = item.Descr;
                sqlCtx.Params["Editor"].Value = item.Editor;
                sqlCtx.Params["Cdt"].Value = cmDt;
                sqlCtx.Params["Udt"].Value = cmDt;
                string ID = Convert.ToString(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
                return ID;*/

                SqlParameter[] paramsArray = new SqlParameter[5];
                paramsArray[0] = new SqlParameter("@BOMNodeType", SqlDbType.VarChar);
                paramsArray[0].Value = item.BOMNodeType;
                paramsArray[1] = new SqlParameter("@ChildBOMNodeType", SqlDbType.VarChar);
                paramsArray[1].Value = item.ChildBOMNodeType;
                paramsArray[2] = new SqlParameter("@Descr", SqlDbType.VarChar);
                paramsArray[2].Value = item.Descr;
                paramsArray[3] = new SqlParameter("@Editor", SqlDbType.VarChar);
                paramsArray[3].Value = item.Editor;
                paramsArray[4] = new SqlParameter("@Now", SqlDbType.DateTime);
                paramsArray[4].Value = _Schema.SqlHelper.GetDateTime();

                string SQLStatement = @"insert into BOMNodeRelation(BOMNodeType, ChildBOMNodeType, Descr, Editor, Cdt, Udt)
                                        values(@BOMNodeType, @ChildBOMNodeType, @Descr, @Editor,@Now,@Now);
                                        SELECT @@IDENTITY;";
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM,
                                                CommandType.Text, SQLStatement, paramsArray))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        string ID = Convert.ToString(sqlR[0]);
                        return ID;
                    }
                }
                return "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateBOMNodeRelation(BOMNodeRelation item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @"update BOMNodeRelation
                                                            set Descr = @Descr,
                                                                Editor = @Editor,
                                                                Udt =@Now
                                                            where BOMNodeType=@BOMNodeType and ChildBOMNodeType=@ChildBOMNodeType";

                        sqlCtx.Params.Add("BOMNodeType", new SqlParameter("@BOMNodeType", SqlDbType.VarChar));
                        sqlCtx.Params.Add("ChildBOMNodeType", new SqlParameter("@ChildBOMNodeType", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Descr", new SqlParameter("@Descr", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Now", new SqlParameter("@Now", SqlDbType.DateTime));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }

                sqlCtx.Params["BOMNodeType"].Value = item.BOMNodeType;
                sqlCtx.Params["ChildBOMNodeType"].Value = item.ChildBOMNodeType;
                sqlCtx.Params["Descr"].Value = item.Descr;
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

        public void UpdateBOMNodeRelation(BOMNodeRelation item, int ID)
        {
            try
            {

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @"update BOMNodeRelation
                                                            set BOMNodeType =@BOMNodeType,
                                                                ChildBOMNodeType =@ChildBOMNodeType,
                                                                Descr = @Descr,
                                                                Editor = @Editor,
                                                                Udt =@Now
                                                            where ID=@ID";

                        sqlCtx.Params.Add("BOMNodeType", new SqlParameter("@BOMNodeType", SqlDbType.VarChar));
                        sqlCtx.Params.Add("ChildBOMNodeType", new SqlParameter("@ChildBOMNodeType", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Descr", new SqlParameter("@Descr", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        sqlCtx.Params.Add("Now", new SqlParameter("@Now", SqlDbType.DateTime));
                        sqlCtx.Params.Add("ID", new SqlParameter("@ID", SqlDbType.Int));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }

                sqlCtx.Params["BOMNodeType"].Value = item.BOMNodeType;
                sqlCtx.Params["ChildBOMNodeType"].Value = item.ChildBOMNodeType;
                sqlCtx.Params["Descr"].Value = item.Descr;
                sqlCtx.Params["Editor"].Value = item.Editor;
                sqlCtx.Params["Now"].Value = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params["ID"].Value = ID;

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

        public void DeleteBOMNodeRelation(string BOMNodeType, string ChildBOMNodeType)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @"delete from BOMNodeRelation where BOMNodeType=@BOMNodeType and ChildBOMNodeType=@ChildBOMNodeType";

                        sqlCtx.Params.Add("BOMNodeType", new SqlParameter("@BOMNodeType", SqlDbType.VarChar));
                        sqlCtx.Params.Add("ChildBOMNodeType", new SqlParameter("@ChildBOMNodeType", SqlDbType.VarChar));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }

                sqlCtx.Params["BOMNodeType"].Value = BOMNodeType;
                sqlCtx.Params["ChildBOMNodeType"].Value = ChildBOMNodeType;

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

        public void DeleteBOMNodeRelation(int ID)
        {
            try
            {

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = new _Schema.SQLContext();
                        sqlCtx.Sentence = @"delete from BOMNodeRelation where ID=@ID";

                        sqlCtx.Params.Add("ID", new SqlParameter("@ID", SqlDbType.Int));

                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
                    }
                }

                sqlCtx.Params["ID"].Value = ID;

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

        public DataTable GetTreeTableByID(string ID)
        {
            ///  DataTable column Name: Id, Material, Component, Descr, IsPart, IsModel, level
            try
            {
                DataTable ret = null;

                //SQL statement
                string sqlStr = @"select ID,
                                           BOMNodeType,
                                           ChildBOMNodeType,
                                           Descr,
                                           Editor
                                        from BOMNodeRelation where ID=@ID";

                SqlParameter[] paramsArray = new SqlParameter[1];
                paramsArray[0] = new SqlParameter("@ID", SqlDbType.VarChar);
                paramsArray[0].Value = ID;

                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                    CommandType.Text,
                                                                                    sqlStr,
                                                                                    paramsArray);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public DataTable GetBomNodeType(string ParentPartNo, string ChildPartNo)
        {
            try
            {
                DataTable ret = null;

                //SQL statement
                string sqlStr = @"select PartNo, BomNodeType from Part where PartNo=@ID1 or PartNo=@ID2";
                SqlParameter[] paramsArray = new SqlParameter[2];
                paramsArray[0] = new SqlParameter("@ID1", SqlDbType.VarChar);
                paramsArray[0].Value = ParentPartNo;
                paramsArray[1] = new SqlParameter("@ID2", SqlDbType.VarChar);
                paramsArray[1].Value = ChildPartNo;
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                    CommandType.Text,
                                                                                    sqlStr,
                                                                                    paramsArray);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetChildBOMTypes(string ParentType)
        {
            try
            {
                DataTable ret = null;

                //SQL statement
                string sqlStr = @"select ChildBOMNodeType from BOMNodeRelation where BOMNodeType=@TYPE";
                SqlParameter[] paramsArray = new SqlParameter[1];
                paramsArray[0] = new SqlParameter("@TYPE", SqlDbType.VarChar);
                paramsArray[0].Value = ParentType;
                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                    CommandType.Text,
                                                                                    sqlStr,
                                                                                    paramsArray);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IList<string> GetModelsFromModelBOM(string modelNo, int rowCount)
        {
            try
            {
                IList<string> ret = new List<string>();
                string SQLStatement = @"select top {1} ID,Material,Plant,Component,Quantity,Alternative_item_group,Priority from ModelBOM NOLOCK where Material ='{0}' and Flag=1 ";

                SQLStatement = string.Format(SQLStatement, modelNo, rowCount.ToString());

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, SQLStatement))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            /*BOMNodeData item = new BOMNodeData();

                            item.ID = GetValue_Int32(sqlR, 0);
                            item.Material = GetValue_Str(sqlR, 1);
                            item.Plant = GetValue_Str(sqlR, 2);
                            item.Component = GetValue_Str(sqlR, 3);
                            item.Quantity = GetValue_Str(sqlR, 4);
                            item.Alternative_item_group = GetValue_Str(sqlR, 5);
                            item.Priority = GetValue_Str(sqlR, 6);
                            */
                            ret.Add(GetValue_Str(sqlR, 3));
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

        public IList<string> GetPartsFromModelBOM(string partNo, int rowCount)
        {
            try
            {
                IList<string> ret = new List<string>();
                string SQLStatement = @"select top {1} ID,Material,Plant,Component,Quantity,Alternative_item_group,Priority from ModelBOM NOLOCK where Component ='{0}' and Flag=1 ";

                SQLStatement = string.Format(SQLStatement, partNo, rowCount.ToString());

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, SQLStatement))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            /*BOMNodeData item = new BOMNodeData();

                            item.ID = GetValue_Int32(sqlR, 0);
                            item.Material = GetValue_Str(sqlR, 1);
                            item.Plant = GetValue_Str(sqlR, 2);
                            item.Component = GetValue_Str(sqlR, 3);
                            item.Quantity = GetValue_Str(sqlR, 4);
                            item.Alternative_item_group = GetValue_Str(sqlR, 5);
                            item.Priority = GetValue_Str(sqlR, 6);
                            */
                            ret.Add(GetValue_Str(sqlR, 1));
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

        //        #region . Implementation of IModelBOMRepository

        //        public BOM GetBOM(string mo)
        //        {
        //            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //            try
        //            {
        //                LoggingInfoFormat("GetBOM->MO: {0}", mo);

        //                if (!IsCached())
        //                    return GetBOM_DB(mo);

        //                BOM bom = GetBOM_Cache(mo);
        //                if (bom == null || bom.Items == null || bom.Items.Count < 1)
        //                {
        //                    bom = GetBOM_DB(mo);

        //                    if (bom != null && bom.Items != null && bom.Items.Count > 0)
        //                    {
        //                        lock (_syncObj_cache)
        //                        {
        //                            if (!_cache.Contains(mo))
        //                                AddToCache(mo, bom);
        //                        }
        //                    }
        //                }
        //                //拷貝
        //                return CopyBOM(bom);
        //            }
        //            catch (Exception)
        //            {
        //                LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //                throw;
        //            }
        //            finally
        //            {
        //                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //            }
        //        }

        //        public BOM GetBOMByStation(string mo, string station)
        //        {
        //            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //            try
        //            {
        //                LoggingInfoFormat("GetBOMByStation->MO: {0}, Staion: {1}.", mo, station);

        //                if (!IsCached())
        //                    return BOMFilterStrategyManager.FilterBOM(CopyBOMWithFilter(GetBOM_DB(mo), station));

        //                BOM bom = GetBOM_Cache(mo);
        //                if (bom == null || bom.Items == null || bom.Items.Count < 1)
        //                {
        //                    bom = GetBOM_DB(mo);

        //                    if (bom != null && bom.Items != null && bom.Items.Count > 0)
        //                    {
        //                        lock (_syncObj_cache)
        //                        {
        //                            if (!_cache.Contains(mo))
        //                                AddToCache(mo, bom);
        //                        }
        //                    }
        //                }
        //                //拷貝兼過濾
        //                return BOMFilterStrategyManager.FilterBOM(CopyBOMWithFilter(bom, station));
        //            }
        //            catch(Exception)
        //            {
        //                LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //                throw;
        //            }
        //            finally
        //            {
        //                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //            }
        //        }

               
        //        public BOM GetModelBOM(string model, string station)
        //        {
        //            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //            try
        //            {
        //                LoggingInfoFormat("GetModelBOM->Model: {0}, Staion: {1}.", model, station);

        //                BOM bom = this.GetModelBOM_DB(model, 18);
        //                //拷貝兼過濾
        //                return CopyBOMWithFilter(bom, station);
        //            }
        //            catch (Exception)
        //            {
        //                LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //                throw;
        //            }
        //            finally
        //            {
        //                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //            }
        //        }

        //        /// <summary>
        //        /// 获取指定MOBOM中指定类型的且Part.Descr包含指定字符串的Pn列表
        //        /// select MoBOM.PartNo from MoBOM inner join Part On MoBOM.PartNo = Part.PartNo where MO=? and PartType=? and Descr like '%?%'
        //        /// </summary>
        //        /// <param name="mo">mo</param>
        //        /// <param name="partType">partType</param>
        //        /// <param name="descrCondition">Part.Descr包含的指定字符串</param>
        //        /// <returns></returns>
        //        public IList<string> GetPnFromMoBOMByTypeAndDescrCondition(string mo, string partType, string descrCondition)
        //        {
        //            try
        //            {
        //                IList<string> ret = new List<string>();

        //                _Schema.SQLContext sqlCtx = null;
        //                _Schema.TableAndFields tf1 = null;
        //                _Schema.TableAndFields tf2 = null;
        //                _Schema.TableAndFields[] tblAndFldsesArray = null;

        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
        //                    {
        //                        tf1 = new _Schema.TableAndFields();
        //                        tf1.Table = typeof(_Schema.MoBOM);
        //                        _Schema.MoBOM cond = new _Schema.MoBOM();
        //                        cond.MO = mo;
        //                        tf1.equalcond = cond;
        //                        tf1.ToGetFieldNames.Add(_Schema.MoBOM.fn_PartNo);

        //                        tf2 = new _Schema.TableAndFields();
        //                        tf2.Table = typeof(_Schema.Part);
        //                        _Schema.Part pCond = new _Schema.Part();
        //                        pCond.PartType = partType;
        //                        pCond.Flag = 1;     //Part表加Flag
        //                        tf2.equalcond = pCond;
        //                        _Schema.Part likeCond = new _Schema.Part();
        //                        likeCond.Descr = "%" + descrCondition + "%";
        //                        tf2.likecond = likeCond;
        //                        tf2.ToGetFieldNames = null;

        //                        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
        //                        _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.MoBOM.fn_PartNo, tf2, _Schema.Part.fn_PartNo);
        //                        tblCnntIs.Add(tc1);

        //                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

        //                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };
        //                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);

        //                        sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.Part.fn_Flag)].Value = pCond.Flag;    //Part表加Flag
        //                    }
        //                }
        //                tf1 = tblAndFldsesArray[0];
        //                tf2 = tblAndFldsesArray[1];

        //                sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.MoBOM.fn_MO)].Value = mo;
        //                sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.Part.fn_PartType)].Value = partType;
        //                sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.Part.fn_Descr)].Value = "%" + descrCondition  + "%";

        //                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //                {
        //                    if (sqlR != null)
        //                    {
        //                        while (sqlR.Read())
        //                        {
        //                            string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.MoBOM.fn_PartNo)]);
        //                            ret.Add(item);
        //                        }
        //                    }
        //                }
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public string Get1397NO(string _111PN)
        //        {
        //            try
        //            {
        //                string ret = null;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                        cond.Component = _111PN;
        //                        cond.Flag = 1;//ModelBOM表加Flag
        //                        _Schema.ModelBOM likeCond = new _Schema.ModelBOM();
        //                        likeCond.Material = "1397%";
        //                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), null, new List<string>() { _Schema.ModelBOM.fn_Material }, cond, likeCond, null, null, null, null, null, null);
        //                        sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = likeCond.Material;
        //                        sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag;//ModelBOM表加Flag
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = _111PN;
        //                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //                {
        //                    if (sqlR != null && sqlR.Read())
        //                    {
        //                        ret = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Material]);
        //                    }
        //                }
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public IList<string> Get1397NOList(string _111PN)
        //        {
        //            try
        //            {
        //                IList<string> ret = new List<string>();

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                        cond.Component = _111PN;
        //                        cond.Flag = 1;//ModelBOM表加Flag
        //                        _Schema.ModelBOM likeCond = new _Schema.ModelBOM();
        //                        likeCond.Material = "1397%";
        //                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "DISTINCT", new List<string>() { _Schema.ModelBOM.fn_Material }, cond, likeCond, null, null, null, null, null, null);
        //                        sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = likeCond.Material;
        //                        sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag;//ModelBOM表加Flag
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = _111PN;
        //                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //                {
        //                    while (sqlR != null && sqlR.Read())
        //                    {
        //                        string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Material]);
        //                        ret.Add(item);
        //                    }
        //                }
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public Family GetFirstFamilyViaMoBOM(string partNum)
        //        {
        //            try
        //            {
        //                Family ret = null;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        sqlCtx = new _Schema.SQLContext();
        //                        sqlCtx.Sentence = "SELECT {0} FROM {1} WHERE {2}=(SELECT {3} FROM {4} WHERE {5}=(SELECT TOP 1 {6} FROM {7} WHERE {8}=@{8} AND Deviation=1))";

        //                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, _Schema.Model.fn_Family,
        //                                                                        typeof(_Schema.Model).Name,
        //                                                                        _Schema.Model.fn_model,
        //                                                                        _Schema.MO.fn_Model,
        //                                                                        typeof(_Schema.MO).Name,
        //                                                                        _Schema.MO.fn_Mo,
        //                                                                        _Schema.MoBOM.fn_MO,
        //                                                                        typeof(_Schema.MoBOM).Name,
        //                                                                        _Schema.MoBOM.fn_PartNo);

        //                        sqlCtx.Params.Add(_Schema.MoBOM.fn_PartNo, new SqlParameter("@" + _Schema.MoBOM.fn_PartNo, SqlDbType.VarChar));

        //                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.MoBOM.fn_PartNo].Value = partNum;

        //                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //                {
        //                    if (sqlR != null)
        //                    {
        //                        if (sqlR.Read())
        //                        {
        //                            string family = GetValue_Str(sqlR, 0);
        //                            if (!string.IsNullOrEmpty(family))
        //                                ret = FmlRepository.Find(family);
        //                        }
        //                    }
        //                }

        //                #region OLD
        //                //_Schema.SQLContext sqlCtx = null;
        //                //_Schema.TableAndFields tf1 = null;
        //                //_Schema.TableAndFields tf2 = null;
        //                //_Schema.TableAndFields tf3 = null;
        //                //_Schema.TableAndFields tf4 = null;
        //                //_Schema.TableAndFields tf5 = null;
        //                //_Schema.TableAndFields[] tblAndFldsesArray = null;
        //                //lock (MethodBase.GetCurrentMethod())
        //                //{
        //                //    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
        //                //    {
        //                //        tf1 = new _Schema.TableAndFields();
        //                //        tf1.Table = typeof(_Schema.MoBOM);
        //                //        _Schema.MoBOM cond = new _Schema.MoBOM();
        //                //        cond.PartNo = partNum;
        //                //        cond.Deviation = true;
        //                //        tf1.equalcond = cond;
        //                //        tf1.ToGetFieldNames = null;

        //                //        tf2 = new _Schema.TableAndFields();
        //                //        tf2.Table = typeof(_Schema.MO);
        //                //        tf2.ToGetFieldNames = null;

        //                //        tf3 = new _Schema.TableAndFields();
        //                //        tf3.Table = typeof(_Schema.Model);
        //                //        tf3.ToGetFieldNames = null;

        //                //        tf4 = new _Schema.TableAndFields();
        //                //        tf4.Table = typeof(_Schema.Family);

        //                //        tf5 = new _Schema.TableAndFields();
        //                //        tf5.Table = typeof(_Schema.Part);
        //                //        tf5.ToGetFieldNames = null;

        //                //        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
        //                //        _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.MoBOM.fn_MO, tf2, _Schema.MO.fn_Mo);
        //                //        tblCnntIs.Add(tc1);
        //                //        _Schema.TableConnectionItem tc2 = new _Schema.TableConnectionItem(tf2, _Schema.MO.fn_Model, tf3, _Schema.Model.fn_model);
        //                //        tblCnntIs.Add(tc2);
        //                //        _Schema.TableConnectionItem tc3 = new _Schema.TableConnectionItem(tf3, _Schema.Model.fn_Family, tf4, _Schema.Family.fn_family);
        //                //        tblCnntIs.Add(tc3);
        //                //        _Schema.TableConnectionItem tc4 = new _Schema.TableConnectionItem(tf1, _Schema.MoBOM.fn_PartNo, tf5, _Schema.Part.fn_PartNo);
        //                //        tblCnntIs.Add(tc4);

        //                //        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

        //                //        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2, tf3, tf4, tf5 };
        //                //        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "TOP 1", ref tblAndFldsesArray, tblCnnts);

        //                //        sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.MoBOM.fn_Deviation)].Value = cond.Deviation;
        //                //    }
        //                //}
        //                //tf1 = tblAndFldsesArray[0];
        //                //tf2 = tblAndFldsesArray[1];
        //                //tf3 = tblAndFldsesArray[2];
        //                //tf4 = tblAndFldsesArray[3];
        //                //tf5 = tblAndFldsesArray[4];

        //                //sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.MoBOM.fn_PartNo)].Value = partNum;

        //                //using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //                //{
        //                //    if (sqlR != null)
        //                //    {
        //                //        if (sqlR.Read())
        //                //        {
        //                //            ret = new Family(GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf4.alias, _Schema.Family.fn_family)]),
        //                //                             GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf4.alias, _Schema.Family.fn_Descr)]),
        //                //                             GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf4.alias, _Schema.Family.fn_CustomerID)]));
        //                //            ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf4.alias, _Schema.Family.fn_Editor)]);
        //                //            ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf4.alias, _Schema.Family.fn_Cdt)]);
        //                //            ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf4.alias, _Schema.Family.fn_Udt)]);
        //                //            ret.Tracker.Clear();
        //                //        }
        //                //    }
        //                //}
        //                #endregion

        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public IList<string> GetPnFromModelBOMByType(string model, string partType)
        //        {
        //            try
        //            {
        //                IList<string> ret = new List<string>();

        //                BOM bom = this.GetModelBOM_DB(model, 18);

        //                bom = FilterBOMAndCopy(bom, partType);
        //                if (bom != null)
        //                {
        //                    foreach (BOMItem bomi in bom.Items)
        //                    {
        //                        if (bomi.AlterParts != null)
        //                        {
        //                            foreach (IBOMPart bompt in bomi.AlterParts)
        //                            {
        //                                if (!ret.Contains(bompt.PN))
        //                                    ret.Add(bompt.PN);
        //                            }
        //                        }
        //                    }
        //                }
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public IList<string> GetPnFromMoBOMByType(string mo, string partType)
        //        {
        //            try
        //            {
        //                IList<string> ret = new List<string>();

        //                BOM bom = null;

        //                if (!IsCached())
        //                {
        //                    bom = GetBOM_DB(mo);
        //                }
        //                else
        //                {
        //                    bom = GetBOM_Cache(mo);
        //                    if (bom == null || bom.Items == null || bom.Items.Count < 1)
        //                    {
        //                        bom = GetBOM_DB(mo);

        //                        if (bom != null)
        //                        {
        //                            lock (_syncObj_cache)
        //                            {
        //                                if (!_cache.Contains(mo))
        //                                    AddToCache(mo, bom);
        //                            }
        //                        }
        //                    }
        //                }
        //                bom = FilterBOMAndCopy(bom, partType);
        //                if (bom != null && bom.Items != null && bom.Items.Count > 0)
        //                {
        //                    foreach(BOMItem bomi in bom.Items)
        //                    {
        //                        if (bomi.AlterParts != null)
        //                        {
        //                            foreach (IBOMPart bompt in bomi.AlterParts)
        //                            {
        //                                if (!ret.Contains(bompt.PN))
        //                                    ret.Add(bompt.PN);
        //                            }
        //                        }
        //                    }
        //                }
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        //#region . Filter  .

        //        //public List<BOMPartCheckSetting> GetPartCheckSetting(string customer, string model)
        //        //{
        //        //    
        //        //}

        //        //public List<BOMPartCheckSetting> GetPartCheckSetting(string customer, string model, string wc)
        //        //{
        //        //    
        //        //}

        //        //#endregion

        //        public DataTable GetPartsViaModelBOM(string material)
        //        {
        //            //select Descr,PartNo from IMES_GetData..Part where PartNo IN (select Component from IMES_GetData..ModelBOM where Material=''and Flag=1) order by PartNo,Descr
        //            try
        //            {
        //                DataTable ret = null;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.Part cond = new _Schema.Part();
        //                        cond.Flag = 1;//Part表加Flag
        //                        _Schema.Part insetCond = new _Schema.Part();
        //                        insetCond.PartNo = "INSET";
        //                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Part), null, new List<string>() { _Schema.Part.fn_Descr, _Schema.Part.fn_PartNo }, cond, null, null, null, null, null, null, insetCond);

        //                        sqlCtx.Params[_Schema.Part.fn_Flag].Value = cond.Flag;//Part表加Flag;

        //                        _Schema.SQLContext sqlCtx_sub = ComposeSubSQLForGetComponentByMaterial(material);

        //                        foreach (KeyValuePair<string, SqlParameter> value in sqlCtx_sub.Params)
        //                        {
        //                            sqlCtx.Params.Add("s_" + value.Key, new SqlParameter("@s_" + value.Value.ParameterName.Substring(1), value.Value.SqlDbType));
        //                        }
        //                        sqlCtx.Sentence = sqlCtx.Sentence.Replace(_Schema.Func.DecInSet(_Schema.Part.fn_PartNo), sqlCtx_sub.Sentence.Replace("@" + _Schema.ModelBOM.fn_Material, "@s_" + _Schema.ModelBOM.fn_Material).Replace("@" + _Schema.ModelBOM.fn_Flag, "@s_" + _Schema.ModelBOM.fn_Flag));//ModelBOM表加Flag

        //                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.Part.fn_Descr, _Schema.Part.fn_PartNo })); 
        //                    }
        //                }

        //                sqlCtx.Params["s_" + _Schema.ModelBOM.fn_Material].Value = material;
        //                sqlCtx.Params["s_" + _Schema.ModelBOM.fn_Flag].Value = 1;//ModelBOM表加Flag
        //                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //                ret = _Schema.Func.SortColumns(ret, new int[] { sqlCtx.Indexes[_Schema.Part.fn_Descr],
        //                                                                sqlCtx.Indexes[_Schema.Part.fn_PartNo]
        //                                                                });
        //                return ret;
        //            }
        //            catch(Exception)
        //            {
        //                throw;
        //            }
        //        }

              

        //        #endregion

        //        #region . Inners .

        //        private BOM GetBOM_Cache(string mo)
        //        {
        //            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //            try
        //            {
        //                LoggingInfoFormat("GetBOM_Cache->MO: {0}", mo);

        //                BOM bom = null;
        //                lock (_syncObj_cache)
        //                {
        //                    if (_cache.Contains(mo))
        //                        bom = (BOM)_cache[mo];
        //                }
        //                LoggingInfoFormat("GetBOM_Cache->BOM[ {0} ]", bom != null ? bom.ToString() : "<NULL>" );
        //                return bom;
        //            }
        //            catch (Exception)
        //            {
        //                LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //                throw;
        //            }
        //            finally
        //            {
        //                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //            }
        //        }

        //        private BOM GetBOM_DB(string mo)
        //        {
        //            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //            try
        //            {
        //                LoggingInfoFormat("GetBOM_DB->MO: {0}", mo);

        //                List<BOMItem> content = new List<BOMItem>();
        //                BOM ret = new BOM(content);

        //                _Schema.SQLContext sqlCtx = null;
        //                _Schema.TableAndFields tf1 = null;
        //                _Schema.TableAndFields tf2 = null;
        //                _Schema.TableAndFields tf3 = null;
        //                _Schema.TableAndFields tf4 = null;
        //                _Schema.TableAndFields tf5 = null;
        //                _Schema.TableAndFields[] tblAndFldsesArray = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
        //                    {
        //                        tf1 = new _Schema.TableAndFields();
        //                        tf1.Table = typeof(_Schema.MoBOM);
        //                        _Schema.MoBOM cond = new _Schema.MoBOM();
        //                        cond.MO = mo;
        //                        cond.Deviation = true;
        //                        tf1.equalcond = cond;
        //                        tf1.ToGetFieldNames.Add(_Schema.MoBOM.fn_Group);
        //                        tf1.ToGetFieldNames.Add(_Schema.MoBOM.fn_PartNo);
        //                        tf1.ToGetFieldNames.Add(_Schema.MoBOM.fn_Qty);
        //                        //tf1.ToGetFieldNames.Add(_Schema.MoBOM.fn_Action);
        //                        //tf1.ToGetFieldNames.Add(_Schema.MoBOM.fn_AssemblyCode);

        //                        tf2 = new _Schema.TableAndFields();
        //                        tf2.Table = typeof(_Schema.MO);
        //                        tf2.ToGetFieldNames = null;

        //                        tf3 = new _Schema.TableAndFields();
        //                        tf3.Table = typeof(_Schema.Model);
        //                        tf3.ToGetFieldNames.Add(_Schema.Model.fn_model);

        //                        tf4 = new _Schema.TableAndFields();
        //                        tf4.Table = typeof(_Schema.Family);
        //                        tf4.ToGetFieldNames.Add(_Schema.Family.fn_CustomerID);

        //                        tf5 = new _Schema.TableAndFields();
        //                        tf5.Table = typeof(_Schema.Part);
        //                        //Part表加Flag
        //                        _Schema.Part pCond = new _Schema.Part();
        //                        pCond.Flag = 1;
        //                        tf5.equalcond = pCond;

        //                        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
        //                        _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.MoBOM.fn_MO, tf2, _Schema.MO.fn_Mo);
        //                        tblCnntIs.Add(tc1);
        //                        _Schema.TableConnectionItem tc2 = new _Schema.TableConnectionItem(tf2, _Schema.MO.fn_Model, tf3, _Schema.Model.fn_model);
        //                        tblCnntIs.Add(tc2);
        //                        _Schema.TableConnectionItem tc3 = new _Schema.TableConnectionItem(tf3, _Schema.Model.fn_Family, tf4, _Schema.Family.fn_family);
        //                        tblCnntIs.Add(tc3);
        //                        _Schema.TableConnectionItem tc4 = new _Schema.TableConnectionItem(tf1, _Schema.MoBOM.fn_PartNo, tf5, _Schema.Part.fn_PartNo);
        //                        tblCnntIs.Add(tc4);

        //                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

        //                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2, tf3, tf4, tf5 };
        //                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, null, ref tblAndFldsesArray, tblCnnts);

        //                        sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.MoBOM.fn_Deviation)].Value = cond.Deviation;

        //                        sqlCtx.Params[_Schema.Func.DecAlias(tf5.alias, _Schema.Part.fn_Flag)].Value = pCond.Flag;//Part表加Flag

        //                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Func.DecAliasInner(tf1.alias, _Schema.MoBOM.fn_Group));
        //                    }
        //                }
        //                tf1 = tblAndFldsesArray[0];
        //                tf2 = tblAndFldsesArray[1];
        //                tf3 = tblAndFldsesArray[2];
        //                tf4 = tblAndFldsesArray[3];
        //                tf5 = tblAndFldsesArray[4];

        //                sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.MoBOM.fn_MO)].Value = mo;

        //                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //                {
        //                    if (sqlR != null)
        //                    {
        //                        FieldInfo fi_qty = typeof(BOMItem).GetField("_qty", BindingFlags.Instance | BindingFlags.NonPublic);
        //                        FieldInfo fi_alterParts = typeof(BOMItem).GetField("_alterParts", BindingFlags.Instance | BindingFlags.NonPublic);
        //                        FieldInfo fi_model = typeof(BOMItem).GetField("_model", BindingFlags.Instance | BindingFlags.NonPublic);
        //                        FieldInfo fi_customerId = typeof(BOMItem).GetField("_customerId", BindingFlags.Instance | BindingFlags.NonPublic);

        //                        BOMItem bomItem = null;
        //                        IList<IBOMPart> alterParts = null;
        //                        int thisGroup = -1;

        //                        while (sqlR.Read())
        //                        {
        //                            int nowGroup = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.MoBOM.fn_Group)]);
        //                            string customerID = string.Empty;
        //                            if (thisGroup == -1 || thisGroup != nowGroup) //開始新組
        //                            {
        //                                thisGroup = nowGroup;
        //                                bomItem = new BOMItem();
        //                                content.Add(bomItem);

        //                                int qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.MoBOM.fn_Qty)]);
        //                                fi_qty.SetValue(bomItem, qty);

        //                                alterParts = new List<IBOMPart>();
        //                                fi_alterParts.SetValue(bomItem, alterParts);

        //                                string model = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf3.alias, _Schema.Model.fn_model)]);
        //                                fi_model.SetValue(bomItem, model);

        //                                customerID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf4.alias, _Schema.Family.fn_CustomerID)]);
        //                                fi_customerId.SetValue(bomItem, customerID);
        //                            }

        //                            IPart iprt = new Part(
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias,_Schema.Part.fn_PartNo)]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias, IMES.Infrastructure.Repository._Metas.Part_NEW.fn_bomNodeType)]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias,_Schema.Part.fn_PartType)]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias,_Schema.Part.fn_CustPartNo)]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias,_Schema.Part.fn_Descr)]),
        //                                        //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias,_Schema.Part.fn_FruNo)]),
        //                                        //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias, _Schema.Part.fn_Vendor)]),
        //                                        //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias,_Schema.Part.fn_IECVersion)]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias,_Schema.Part.fn_Remark)]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias, _Schema.Part.fn_AutoDL)]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias,_Schema.Part.fn_Editor)]),
        //                                        GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias,_Schema.Part.fn_Cdt)]),
        //                                        GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias,_Schema.Part.fn_Udt)]),
        //                                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias,_Schema.Part.fn_Descr2)]));
                                    
        //                            ((Part)iprt).Tracker.Clear();
                                    
        //                            //string assbCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.MoBOM.fn_AssemblyCode)]);
        //                            //IList<string> assbCodes = new List<string>(assbCode.Split(new string[] { "~" }, StringSplitOptions.RemoveEmptyEntries));//2010-02-04 Liu Dong(eB1-4)         Modify ITC-1103-0170 
        //                            IBOMPart bompart = new BOMPart(iprt, /*assbCodes,*/ bomItem.CustomerId, bomItem.Model);

        //                            ((Part)iprt).Tracker = ((BOMPart)bompart).Tracker.Merge(((Part)iprt).Tracker);

        //                            alterParts.Add(bompart);
        //                        }
        //                    }
        //                }
        //                LoggingInfoFormat("GetBOM_DB->BOM[ {0} ]", ret != null ? ret.ToString() : "<NULL>");
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //                throw;
        //            }
        //            finally
        //            {
        //                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //            }
        //        }

        //        private BOM GetModelBOM_DB(string model, int limitCount)
        //        {
        //            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //            try
        //            {
        //                LoggingInfoFormat("GetModelBOM_DB->Model: {0}", model);

        //                List<BOMItem> content = new List<BOMItem>();
        //                BOM ret = new BOM(content);

        //                Model mdl = MdlRepository.Find(model);
        //                string customer = string.Empty;
        //                if (mdl != null)
        //                {
        //                    customer = mdl.Family.Customer;
        //                }

        //                SqlParameter[] paramsArray = new SqlParameter[2];

        //                paramsArray[0] = new SqlParameter("@model", SqlDbType.VarChar);
        //                paramsArray[0].Value = model;
        //                paramsArray[1] = new SqlParameter("@limitCount", SqlDbType.Int);
        //                paramsArray[1].Value = limitCount;

        //                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.StoredProcedure, "GetModelBOM", paramsArray))
        //                {
        //                    if (sqlR != null)
        //                    {
        //                        FieldInfo fi_qty = typeof(BOMItem).GetField("_qty", BindingFlags.Instance | BindingFlags.NonPublic);
        //                        FieldInfo fi_alterParts = typeof(BOMItem).GetField("_alterParts", BindingFlags.Instance | BindingFlags.NonPublic);
        //                        FieldInfo fi_model = typeof(BOMItem).GetField("_model", BindingFlags.Instance | BindingFlags.NonPublic);
        //                        FieldInfo fi_customerId = typeof(BOMItem).GetField("_customerId", BindingFlags.Instance | BindingFlags.NonPublic);

        //                        BOMItem bomItem = null;
        //                        IList<IBOMPart> alterParts = null;
        //                        string thisGroup = null;

        //                        while (sqlR.Read())
        //                        {
        //                            string pn = GetValue_Str(sqlR, 0);
        //                            string nowGroup = GetValue_Str(sqlR, 1);
        //                            int qty = Convert.ToInt32(GetValue_Str(sqlR, 2));
        //                            //string assbCode = GetValue_Str(sqlR, 3);
        //                            string customerID = string.Empty;
        //                            if (thisGroup == null || thisGroup != nowGroup) //開始新組
        //                            {
        //                                thisGroup = nowGroup;
        //                                bomItem = new BOMItem();
        //                                content.Add(bomItem);

        //                                fi_qty.SetValue(bomItem, qty);

        //                                alterParts = new List<IBOMPart>();
        //                                fi_alterParts.SetValue(bomItem, alterParts);

        //                                fi_model.SetValue(bomItem, model);

        //                                customerID = customer;
        //                                fi_customerId.SetValue(bomItem, customerID);
        //                            }

        //                            IPart iprt = PrtRepository.Find(pn);
        //                            if (iprt != null)
        //                            {
        //                                //IList<string> assbCodes = new List<string>(assbCode.Split(new string[] { "~" }, StringSplitOptions.RemoveEmptyEntries));//2010-02-04 Liu Dong(eB1-4)         Modify ITC-1103-0170 
        //                                IBOMPart bompart = new BOMPart(iprt, /*assbCodes,*/ bomItem.CustomerId, bomItem.Model);

        //                                ((Part)iprt).Tracker = ((BOMPart)bompart).Tracker.Merge(((Part)iprt).Tracker);

        //                                alterParts.Add(bompart);
        //                            }
        //                        }
        //                    }
        //                }
        //                LoggingInfoFormat("GetModelBOM_DB->BOM[ {0} ]", ret != null ? ret.ToString() : "<NULL>");
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //                throw;
        //            }
        //            finally
        //            {
        //                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //            }
        //        }

        //        private void LoadAllPartCheckSettingFromDB()
        //        {
        //            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //            try
        //            {
        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting));
        //                    }
        //                }
        //                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
        //                {
        //                    if (sqlR != null)
        //                    {
        //                        while (sqlR.Read())
        //                        {
        //                             BOMPartCheckSetting item = new BOMPartCheckSetting(
        //                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Customer]),
        //                                GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Model]),
        //                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_WC]),
        //                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Tp]),
        //                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_ValueType])
        //                                );
        //                            string key = item.GetHashCode().ToString();
        //                            if (!_filtercache.ContainsKey(key))
        //                            {
        //                                IList<BOMPartCheckSetting> newLst = new List<BOMPartCheckSetting>();
        //                                _filtercache.Add(key, newLst);
        //                                newLst.Add(item);
        //                            }
        //                            else
        //                            {
        //                                _filtercache[key].Add(item);
        //                            }

        //                            LoggingInfoFormat("Add One PartCheckSetting: {0}", item.ToString());

        //                            //BOMPartCheckSetting itemWithoutCust = new BOMPartCheckSetting(
        //                            //    string.Empty, 
        //                            //    item.Model,
        //                            //    item.Wc, 
        //                            //    item.PartType
        //                            //    );

        //                            //string keyWithoutCust = itemWithoutCust.GetHashCode().ToString();
        //                            //if (!_filtercache.ContainsKey(keyWithoutCust))
        //                            //    _filtercache.Add(keyWithoutCust, itemWithoutCust);
        //                        }
        //                    }
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //                throw;
        //            }
        //            finally
        //            {
        //                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //            }
        //        }

        //        private BOM CopyBOMWithFilter(BOM originBOM, string station)
        //        {
        //            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //            try
        //            {
        //                if (!this.IsCached())
        //                {
        //                    lock (_syncObj_filtercache)
        //                    {
        //                        _filtercache.Clear();//每次清了从库里拿新的,即不是Cache
        //                        this.LoadAllPartCheckSettingFromDB();
        //                        BOM ret = CopyBOMWithFilter_Inner(originBOM, station);
        //                        _filtercache.Clear();//原则上用完了也要清
        //                        LoggingInfoFormat("CopyBOMWithFilter->BOM[ {0} ]", ret != null ? ret.ToString() : "<NULL>");
        //                        return ret;
        //                    }
        //                }
        //                else
        //                {
        //                    lock (_syncObj_filtercache)
        //                    {
        //                        if (_filtercache.Count < 1)
        //                        {
        //                            this.LoadAllPartCheckSettingFromDB();
        //                        }
        //                        return CopyBOMWithFilter_Inner(originBOM, station);
        //                    }
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //                throw;
        //            }
        //            finally
        //            {
        //                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //            }
        //        }

        //        #region Copy Methods

        //        private BOM CopyBOM(BOM originBOM)
        //        {
        //            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //            try
        //            {
        //                if (originBOM == null)
        //                    return originBOM;

        //                List<BOMItem> content = new List<BOMItem>();
        //                BOM ret = new BOM(content);

        //                IEnumerator<BOMItem> enmrt = originBOM.Items.GetEnumerator();
        //                while (enmrt.MoveNext())
        //                {
        //                    BOMItem newBomItem = new BOMItem();
        //                    newBomItem.GetType().GetField("_customerId", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newBomItem, enmrt.Current.CustomerId);
        //                    newBomItem.GetType().GetField("_model", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newBomItem, enmrt.Current.Model);
        //                    newBomItem.GetType().GetField("_qty", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newBomItem, enmrt.Current.Qty);

        //                    IList<IBOMPart> alterParts = null;
        //                    content.Add(newBomItem);

        //                    IEnumerator<IBOMPart> enmrt_i = enmrt.Current.AlterParts.GetEnumerator();
        //                    while (enmrt_i.MoveNext())
        //                    {
        //                        if (newBomItem.AlterParts == null)
        //                        {
        //                            alterParts = new List<IBOMPart>();
        //                            newBomItem.GetType().GetField("_alterParts", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newBomItem, alterParts);
        //                        }
        //                        IBOMPart obj = (IBOMPart)CloneTool.DoDeepCopyObj(enmrt_i.Current);
        //                        alterParts.Add(obj);
        //                    }

        //                    if (newBomItem.AlterParts == null || newBomItem.AlterParts.Count < 1)
        //                        content.Remove(newBomItem);
        //                }
        //                LoggingInfoFormat("CopyBOM->BOM[ {0} ]", ret != null ? ret.ToString() : "<NULL>");
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //                throw;
        //            }
        //            finally
        //            {
        //                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //            }
        //        }

        //        private BOM FilterBOMAndCopy(BOM originBOM, string partType)
        //        {
        //            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //            try
        //            {
        //                if (originBOM == null)
        //                    return originBOM;

        //                List<BOMItem> content = new List<BOMItem>();
        //                BOM ret = new BOM(content);

        //                IEnumerator<BOMItem> enmrt = originBOM.Items.GetEnumerator();
        //                while (enmrt.MoveNext())
        //                {
        //                    BOMItem newBomItem = new BOMItem();
        //                    newBomItem.GetType().GetField("_customerId", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newBomItem, enmrt.Current.CustomerId);
        //                    newBomItem.GetType().GetField("_model", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newBomItem, enmrt.Current.Model);
        //                    newBomItem.GetType().GetField("_qty", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newBomItem, enmrt.Current.Qty);

        //                    IList<IBOMPart> alterParts = null;
        //                    content.Add(newBomItem);

        //                    IEnumerator<IBOMPart> enmrt_i = enmrt.Current.AlterParts.GetEnumerator();
        //                    while (enmrt_i.MoveNext())
        //                    {
        //                        if (enmrt_i.Current.Type == partType)
        //                        {
        //                            if (newBomItem.AlterParts == null)
        //                            {
        //                                alterParts = new List<IBOMPart>();
        //                                newBomItem.GetType().GetField("_alterParts", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newBomItem, alterParts);
        //                            }
        //                            IBOMPart obj = (IBOMPart)CloneTool.DoDeepCopyObj(enmrt_i.Current);
        //                            alterParts.Add(obj);
        //                        }
        //                    }

        //                    if (newBomItem.AlterParts == null || newBomItem.AlterParts.Count < 1)
        //                        content.Remove(newBomItem);
        //                }
        //                LoggingInfoFormat("FilterBOMAndCopy->BOM[ {0} ]", ret != null ? ret.ToString() : "<NULL>");
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //                throw;
        //            }
        //            finally
        //            {
        //                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //            }
        //        }

        //        private BOM CopyBOMWithFilter_Inner(BOM originBOM, string station)
        //        {
        //            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //            try
        //            {
        //                if (originBOM == null)
        //                    return originBOM;

        //                List<BOMItem> content = new List<BOMItem>();
        //                BOM ret = new BOM(content);

        //                IEnumerator<BOMItem> enmrt = originBOM.Items.GetEnumerator();
        //                while (enmrt.MoveNext())
        //                {
        //                    IList<string> distinctValueTypes = new List<string>();

        //                    BOMItem newBomItem = new BOMItem();
        //                    newBomItem.GetType().GetField("_customerId", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newBomItem, enmrt.Current.CustomerId);
        //                    newBomItem.GetType().GetField("_model", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newBomItem, enmrt.Current.Model);
        //                    newBomItem.GetType().GetField("_qty", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newBomItem, enmrt.Current.Qty);

        //                    IList<IBOMPart> alterParts = null;
        //                    content.Add(newBomItem);

        //                    IEnumerator<IBOMPart> enmrt_i = enmrt.Current.AlterParts.GetEnumerator();
        //                    while (enmrt_i.MoveNext())
        //                    {
        //                        BOMPartCheckSetting cmper = new BOMPartCheckSetting(enmrt.Current.CustomerId,
        //                            //2010-04-14 Liu Dong(eB1-4)         Modify YWH:BOMPartCheckSetting的Model元素以SQL的通配符LIKE来实现过滤BOM动作.
        //                                                                            null,//enmrt.Current.Model,
        //                            //2010-04-14 Liu Dong(eB1-4)         Modify YWH:BOMPartCheckSetting的Model元素以SQL的通配符LIKE来实现过滤BOM动作.
        //                                                                            station,
        //                                                                            enmrt_i.Current.Type,
        //                                                                            null);
        //                        string key = cmper.GetHashCode().ToString();
        //                        lock (_syncObj_filtercache)
        //                        {
        //                            bool bingle = _filtercache.ContainsKey(key);

        //                            //2010-04-14 Liu Dong(eB1-4)         Modify YWH:BOMPartCheckSetting的Model元素以SQL的通配符LIKE来实现过滤BOM动作.
        //                            //if (bingle == false)
        //                            //{
        //                            //    bingle = _filtercache.ContainsKey(new BOMPartCheckSetting(enmrt.Current.CustomerId,
        //                            //                                                null,
        //                            //                                                station,
        //                            //                                                enmrt_i.Current.Type).GetHashCode().ToString());
        //                            //}
        //                            if (bingle)
        //                            {
        //                                bool match = false;
        //                                foreach (BOMPartCheckSetting bompchkst in _filtercache[key])
        //                                {
        //                                    bool match_i = false;

        //                                    string wildcardModel = bompchkst.Model;
        //                                    if (!string.IsNullOrEmpty(wildcardModel))
        //                                        match_i = ToStringTool.IsLike(wildcardModel, enmrt.Current.Model);
        //                                    else
        //                                        match_i = true;

        //                                    if (match_i)
        //                                    {
        //                                        if (match == false)
        //                                            match = true;

        //                                        if (!distinctValueTypes.Contains(bompchkst.ValueType))
        //                                            distinctValueTypes.Add(bompchkst.ValueType);
        //                                    }
        //                                }
        //                                //string wildcardModel = _filtercache[key].Model;
        //                                bingle = match;
        //                            }
        //                            //2010-04-14 Liu Dong(eB1-4)         Modify YWH:BOMPartCheckSetting的Model元素以SQL的通配符LIKE来实现过滤BOM动作.

        //                            if (bingle)
        //                            {
        //                                if (newBomItem.AlterParts == null)
        //                                {
        //                                    alterParts = new List<IBOMPart>();
        //                                    newBomItem.GetType().GetField("_alterParts", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newBomItem, alterParts);
        //                                }
        //                                IBOMPart obj = (IBOMPart)CloneTool.DoDeepCopyObj(enmrt_i.Current); //这一級不用拷貝用原對象也應該沒有問題吧? YWH at 2010-01-28 said: NO.
        //                                alterParts.Add(obj);
        //                            }
        //                        }
        //                    }

        //                    if (newBomItem.AlterParts == null || newBomItem.AlterParts.Count < 1)
        //                    {
        //                        content.Remove(newBomItem);
        //                    }
        //                    else
        //                    {
        //                        int k = 0;
        //                        foreach (string valueType in distinctValueTypes)
        //                        {
        //                            BOMItem curr = null;
        //                            if (k == 0)
        //                            {
        //                                curr = newBomItem;
        //                            }
        //                            else
        //                            {
        //                                curr = (BOMItem)CloneTool.DoDeepCopyObj(newBomItem);
        //                                content.Add(curr);
        //                            }
        //                            curr.ValueType = valueType;
        //                            k++;
        //                        }
        //                    }
        //                }
        //                LoggingInfoFormat("CopyBOMWithFilter_Inner->BOM[ {0} ]", ret != null ? ret.ToString() : "<NULL>");
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //                throw;
        //            }
        //            finally
        //            {
        //                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //            }
        //        }

        //        #endregion

        //        private IList<MOBOM> GetMOBOM(string mo, string deviation, string action)
        //        {
        //            try
        //            {
        //                IList<MOBOM> ret = new List<MOBOM>();

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.MoBOM eqCond = new _Schema.MoBOM();
        //                        eqCond.MO = mo;
        //                        eqCond.Deviation = true;//deviation == "1" ? true : false;
        //                        eqCond.Action = action;

        //                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM), eqCond, null, null);
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.MoBOM.fn_MO].Value = mo;
        //                sqlCtx.Params[_Schema.MoBOM.fn_Deviation].Value = deviation == "1" ? true : false;
        //                sqlCtx.Params[_Schema.MoBOM.fn_Action].Value = action;
        //                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //                {
        //                    if (sqlR != null)
        //                    {
        //                        while (sqlR.Read())
        //                        {
        //                            MOBOM item = new MOBOM();
        //                            item.Action = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Action]);
        //                            item.AssemblyCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_AssemblyCode]);
        //                            item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Cdt]);
        //                            item.Deviation = GetValue_Bit(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Deviation]);
        //                            item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Editor]);
        //                            item.Group = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Group]);
        //                            item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_ID]);
        //                            item.MO = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_MO]);
        //                            item.PartNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_PartNo]);
        //                            item.Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Qty]);
        //                            item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Udt]);
        //                            item.Tracker.Clear();
        //                            ret.Add(item);
        //                        }
        //                    }
        //                }
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        private IList<MOBOM> GetMOBOM(string mo, string deviation)
        //        {
        //            try
        //            {
        //                IList<MOBOM> ret = new List<MOBOM>();

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.MoBOM eqCond = new _Schema.MoBOM();
        //                        eqCond.MO = mo;
        //                        eqCond.Deviation = true;// deviation == "1" ? true : false;

        //                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM), eqCond, null, null);
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.MoBOM.fn_MO].Value = mo;
        //                sqlCtx.Params[_Schema.MoBOM.fn_Deviation].Value = deviation == "1" ? true : false;
        //                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //                {
        //                    if (sqlR != null)
        //                    {
        //                        while (sqlR.Read())
        //                        {
        //                            MOBOM item = new MOBOM();
        //                            item.Action = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Action]);
        //                            item.AssemblyCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_AssemblyCode]);
        //                            item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Cdt]);
        //                            item.Deviation = GetValue_Bit(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Deviation]);
        //                            item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Editor]);
        //                            item.Group = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Group]);
        //                            item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_ID]);
        //                            item.MO = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_MO]);
        //                            item.PartNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_PartNo]);
        //                            item.Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Qty]);
        //                            item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Udt]);
        //                            item.Tracker.Clear();
        //                            ret.Add(item);
        //                        }
        //                    }
        //                }
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        #endregion

        //        #region ICache Members

        //        public bool IsCached()
        //        {
        //            return DataChangeMediator.CheckCacheSwitchOpen(DataChangeMediator.CacheSwitchType.BOM);
        //        }

        //        public void ProcessItem(CacheUpdateInfo item)
        //        {
        //            if (item.Type == IMES.DataModel.CacheType.MOBOM)
        //                LoadOneCache(item.Item);
        //            else if (item.Type == IMES.DataModel.CacheType.PartCheckSetting)
        //                LoadAllPartCheckSetting();
        //        }

        //        public void ClearCache()
        //        {
        //            lock (_syncObj_cache)
        //            {
        //                _cache.Flush();
        //            }
        //        }

        //        private void LoadAllPartCheckSetting()
        //        {
        //            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //            try
        //            {
        //                lock (_syncObj_filtercache)
        //                {
        //                    _filtercache.Clear();
        //                    this.LoadAllPartCheckSettingFromDB();
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //                throw;
        //            }
        //            finally
        //            {
        //                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //            }
        //        }

        //        private void LoadOneCache(string pk) //mo
        //        {
        //            LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //            try
        //            {
        //                lock (_syncObj_cache)
        //                {
        //                    LoggingInfoFormat("LoadOneCache->MO: {0}", pk);

        //                    if (_cache.Contains(pk))
        //                    {
        //                        _cache.Remove(pk);
        //                    }
        //                    BOM bom = GetBOM_DB(pk);
        //                    if (bom != null)
        //                    {
        //                        LoggingInfoFormat("LoadOneCache->BOM[ {0} ]", bom != null ? bom.ToString() : "<NULL>");
        //                        AddToCache(pk, bom);
        //                    }
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //                throw;
        //            }
        //            finally
        //            {
        //                LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //            }
        //        }

        //        private IMES.DataModel.CacheUpdateInfo GetACacheSignal(string pk, string type)
        //        {
        //            IMES.DataModel.CacheUpdateInfo ret = new IMES.DataModel.CacheUpdateInfo();
        //            ret.Cdt = ret.Udt = _Schema.SqlHelper.GetDateTime();
        //            ret.Updated = false;
        //            ret.Type = type;
        //            ret.Item = pk;
        //            return ret;
        //        }

        //        private void AddToCache(string key, object obj)
        //        {
        //            _cache.Add(key, obj, CacheItemPriority.Normal, new MOBOMRefreshAction(), new SlidingTime(TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["TOSC_MOBOMCache"].ToString()))));
        //        }

        //        [Serializable]
        //        private class MOBOMRefreshAction : ICacheItemRefreshAction
        //        {
        //            public void Refresh(string key, object expiredValue, CacheItemRemovedReason removalReason)
        //            {
        //            }
        //        }

        //        #endregion

        //        #region For Maintain

        //        public IList<MOBOM> GetMOBOMList(string mo)
        //        {
        //            //来自于
        //            //1.MoBOM数据表中MO栏位等于MO,Devilation等于1的所有记录
        //            //2.MO栏位等于MO、Devilation等于0且Action栏位等于“DELETE”的所有记录的合集
        //            //3.按Group列的字符序排序

        //            //参考sql如下：
        //            //select id, MO, PartNo, AssemblyCode, Qty, Group, Deviation, Action, Editor, Cdt, Udt from
        //            //(select id, MO, PartNo, AssemblyCode, Qty, Group, Deviation, Action, Editor, Cdt, Udt from MoBOM where MO='MO' and Deviation = '1'
        //            //union
        //            //select id, MO, PartNo, AssemblyCode, Qty, Group, Deviation, Action, Editor, Cdt, Udt from MoBOM where MO='MO' and Deviation = '0' and Action = 'DELETE') as A
        //            //order by A.Group
        //            try
        //            {
        //                IList<MOBOM> ret = null;

        //                IList<MOBOM> set1 = this.GetMOBOM(mo, "1");
        //                IList<MOBOM> set2 = this.GetMOBOM(mo, "0", "DELETE");

        //                ret = new List<MOBOM>(set1.Union(set2, new EqualityComparer4MOBOM()));

        //                return (from item in ret orderby item.Group select item).ToList();
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public IList<MOBOM> GetMOBOMListByGroup(string mo, bool deviation, int group, int exceptMOBOMId)
        //        {
        //            try
        //            {
        //                IList<MOBOM> ret = new List<MOBOM>();

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.MoBOM eqCond = new _Schema.MoBOM();
        //                        eqCond.MO = mo;
        //                        eqCond.Deviation = true;
        //                        eqCond.Group = group;

        //                        _Schema.MoBOM neqCond = new _Schema.MoBOM();
        //                        neqCond.ID = exceptMOBOMId;

        //                        sqlCtx = _Schema.Func.GetConditionedFuncSelectWith3NotConds(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM), null, null, eqCond, null, null, null, null, null, null, null, neqCond, null, null);
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.MoBOM.fn_MO].Value = mo;
        //                sqlCtx.Params[_Schema.MoBOM.fn_Deviation].Value = deviation;
        //                sqlCtx.Params[_Schema.MoBOM.fn_Group].Value = group;
        //                sqlCtx.Params[_Schema.MoBOM.fn_ID].Value = exceptMOBOMId;
        //                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //                {
        //                    if (sqlR != null)
        //                    {
        //                        while (sqlR.Read())
        //                        {
        //                            MOBOM item = new MOBOM();
        //                            item.Action = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Action]);
        //                            item.AssemblyCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_AssemblyCode]);
        //                            item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Cdt]);
        //                            item.Deviation = GetValue_Bit(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Deviation]);
        //                            item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Editor]);
        //                            item.Group = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Group]);
        //                            item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_ID]);
        //                            item.MO = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_MO]);
        //                            item.PartNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_PartNo]);
        //                            item.Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Qty]);
        //                            item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Udt]);
        //                            item.Tracker.Clear();
        //                            ret.Add(item);
        //                        }
        //                    }
        //                }
        //                return ret;
        //            }
        //            catch(Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public MOBOM GetMOBOM(int id)
        //        {
        //            try
        //            {
        //                MOBOM ret = null;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.MoBOM eqCond = new _Schema.MoBOM();
        //                        eqCond.ID = id;

        //                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM), eqCond, null, null);
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.MoBOM.fn_ID].Value = id;
        //                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //                {
        //                    if (sqlR != null)
        //                    {
        //                        if (sqlR.Read())
        //                        {
        //                            ret = new MOBOM();
        //                            ret.Action = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Action]);
        //                            ret.AssemblyCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_AssemblyCode]);
        //                            ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Cdt]);
        //                            ret.Deviation = GetValue_Bit(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Deviation]);
        //                            ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Editor]);
        //                            ret.Group = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Group]);
        //                            ret.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_ID]);
        //                            ret.MO = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_MO]);
        //                            ret.PartNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_PartNo]);
        //                            ret.Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Qty]);
        //                            ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Udt]);
        //                            ret.Tracker.Clear();
        //                        }
        //                    }
        //                }
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public int CheckMOBOMQty(string mo, bool deviation, string partNo)
        //        {
        //            try
        //            {
        //                int ret = 0;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.MoBOM eqCond = new _Schema.MoBOM();
        //                        eqCond.MO = mo;
        //                        eqCond.Deviation = true;
        //                        eqCond.PartNo = partNo;

        //                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM), "COUNT", new List<string>() { _Schema.MoBOM.fn_ID }, eqCond, null, null, null, null, null, null, null);
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.MoBOM.fn_MO].Value = mo;
        //                sqlCtx.Params[_Schema.MoBOM.fn_Deviation].Value = deviation;
        //                sqlCtx.Params[_Schema.MoBOM.fn_PartNo].Value = partNo;
        //                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //                {
        //                    if (sqlR != null)
        //                    {
        //                        if (sqlR.Read())
        //                        {
        //                            ret = GetValue_Int32(sqlR, sqlCtx.Indexes["COUNT"]);
        //                        }
        //                    }
        //                }
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public int CheckMOBOMQty(string mo, bool deviation)
        //        {
        //            try
        //            {
        //                int ret = 0;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.MoBOM eqCond = new _Schema.MoBOM();
        //                        eqCond.MO = mo;
        //                        eqCond.Deviation = true;

        //                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM), "COUNT", new List<string>() { _Schema.MoBOM.fn_ID }, eqCond, null, null, null, null, null, null, null);
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.MoBOM.fn_MO].Value = mo;
        //                sqlCtx.Params[_Schema.MoBOM.fn_Deviation].Value = deviation;
        //                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //                {
        //                    if (sqlR != null)
        //                    {
        //                        if (sqlR.Read())
        //                        {
        //                            ret = GetValue_Int32(sqlR, sqlCtx.Indexes["COUNT"]);
        //                        }
        //                    }
        //                }
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public int AddMOBOM(MOBOM item)
        //        {
        //            try
        //            {
        //                SqlTransactionManager.Begin();

        //                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(item.MO, CacheType.MOBOM));

        //                //int ret = 0;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM));
        //                    }
        //                }
        //                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //                sqlCtx.Params[_Schema.MoBOM.fn_Action].Value = item.Action;
        //                sqlCtx.Params[_Schema.MoBOM.fn_AssemblyCode].Value = item.AssemblyCode;
        //                sqlCtx.Params[_Schema.MoBOM.fn_Cdt].Value = cmDt;
        //                sqlCtx.Params[_Schema.MoBOM.fn_Deviation].Value = item.Deviation;
        //                sqlCtx.Params[_Schema.MoBOM.fn_Editor].Value = item.Editor;
        //                sqlCtx.Params[_Schema.MoBOM.fn_Group].Value = item.Group;
        //                //sqlCtx.Params[_Schema.MoBOM.fn_ID].Value = item.ID;
        //                sqlCtx.Params[_Schema.MoBOM.fn_MO].Value = item.MO;
        //                sqlCtx.Params[_Schema.MoBOM.fn_PartNo].Value = item.PartNo;
        //                sqlCtx.Params[_Schema.MoBOM.fn_Qty].Value = item.Qty;
        //                sqlCtx.Params[_Schema.MoBOM.fn_Udt].Value = cmDt;
        //                item.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
        //                item.Tracker.Clear();

        //                SqlTransactionManager.Commit();

        //                return item.ID;
        //            }
        //            catch (Exception)
        //            {
        //                SqlTransactionManager.Rollback();
        //                throw;
        //            }
        //            finally
        //            {
        //                SqlTransactionManager.Dispose();
        //                SqlTransactionManager.End();
        //            }
        //        }

        //        public void UpdateMOBOM(MOBOM item, string oldMo)
        //        {
        //            try
        //            {
        //                SqlTransactionManager.Begin();

        //                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(oldMo, CacheType.MOBOM));
        //                if (oldMo != item.MO)
        //                    DataChangeMediator.AddChangeDemand(this.GetACacheSignal(item.MO, CacheType.MOBOM));

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM));
        //                    }
        //                }
        //                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //                sqlCtx.Params[_Schema.MoBOM.fn_Action].Value = item.Action;
        //                sqlCtx.Params[_Schema.MoBOM.fn_AssemblyCode].Value = item.AssemblyCode;
        //                sqlCtx.Params[_Schema.MoBOM.fn_Deviation].Value = item.Deviation;
        //                sqlCtx.Params[_Schema.MoBOM.fn_Editor].Value = item.Editor;
        //                sqlCtx.Params[_Schema.MoBOM.fn_Group].Value = item.Group;
        //                sqlCtx.Params[_Schema.MoBOM.fn_ID].Value = item.ID;
        //                sqlCtx.Params[_Schema.MoBOM.fn_MO].Value = item.MO;
        //                sqlCtx.Params[_Schema.MoBOM.fn_PartNo].Value = item.PartNo;
        //                sqlCtx.Params[_Schema.MoBOM.fn_Qty].Value = item.Qty;
        //                sqlCtx.Params[_Schema.MoBOM.fn_Udt].Value = cmDt;
        //                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //                item.Udt = cmDt;
        //                item.Tracker.Clear();

        //                SqlTransactionManager.Commit();
        //            }
        //            catch (Exception)
        //            {
        //                SqlTransactionManager.Rollback();
        //                throw;
        //            }
        //            finally
        //            {
        //                SqlTransactionManager.Dispose();
        //                SqlTransactionManager.End();
        //            }
        //        }

        //        public void DeleteMOBOM(MOBOM item)
        //        {
        //            try
        //            {
        //                SqlTransactionManager.Begin();

        //                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(item.MO, CacheType.MOBOM));

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM));
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.MoBOM.fn_ID].Value = item.ID;
        //                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

        //                SqlTransactionManager.Commit();
        //            }
        //            catch (Exception)
        //            {
        //                SqlTransactionManager.Rollback();
        //                throw;
        //            }
        //            finally
        //            {
        //                SqlTransactionManager.Dispose();
        //                SqlTransactionManager.End();
        //            }
        //        }

        //        public void DeleteMOBOMByMo(string mo)
        //        {
        //            try
        //            {
        //                SqlTransactionManager.Begin();

        //                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(mo, CacheType.MOBOM));

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.MoBOM cond = new _Schema.MoBOM();
        //                        cond.MO = mo;
        //                        sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM), cond, null, null);
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.MoBOM.fn_MO].Value = mo;
        //                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

        //                SqlTransactionManager.Commit();
        //            }
        //            catch (Exception)
        //            {
        //                SqlTransactionManager.Rollback();
        //                throw;
        //            }
        //            finally
        //            {
        //                SqlTransactionManager.Dispose();
        //                SqlTransactionManager.End();
        //            }
        //        }

        //        public void CopyMOBOM(string mo)
        //        {
        //            //复制当前MO的所有记录并将它们的Devilation栏位设置为0
        //            try
        //            {
        //                SqlTransactionManager.Begin();

        //                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(mo, CacheType.MOBOM));

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        sqlCtx = new _Schema.SQLContext();
        //                        sqlCtx.Sentence = "INSERT INTO {0} " +
        //                                        "({1},{2},{3},{4},{5},{6},{7},{8},{9},{10}) " +
        //                                        "(SELECT {1},{2},{3},{4},{5},0,{7},{8},{9},{10} FROM {0} " +
        //                                        "WHERE {1}=@{1} AND {6}=1) ";

        //                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.MoBOM).Name,
        //                                                                        _Schema.MoBOM.fn_MO,
        //                                                                        _Schema.MoBOM.fn_PartNo,
        //                                                                        _Schema.MoBOM.fn_AssemblyCode,
        //                                                                        _Schema.MoBOM.fn_Qty,
        //                                                                        _Schema.MoBOM.fn_Group,
        //                                                                        _Schema.MoBOM.fn_Deviation,
        //                                                                        _Schema.MoBOM.fn_Action,
        //                                                                        _Schema.MoBOM.fn_Editor,
        //                                                                        _Schema.MoBOM.fn_Cdt,
        //                                                                        _Schema.MoBOM.fn_Udt);

        //                        sqlCtx.Params.Add(_Schema.MoBOM.fn_MO, new SqlParameter("@" + _Schema.MoBOM.fn_MO, SqlDbType.VarChar));

        //                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.MoBOM.fn_MO].Value = mo;
        //                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

        //                SqlTransactionManager.Commit();
        //            }
        //            catch (Exception)
        //            {
        //                SqlTransactionManager.Rollback();
        //                throw;
        //            }
        //            finally
        //            {
        //                SqlTransactionManager.Dispose();
        //                SqlTransactionManager.End();
        //            }
        //        }

        //        public void UpdateMOBOMForDeleteAction(string mo, string partNo, bool deviation)
        //        {
        //            //将当前被选记录的对应Devilation等于0的记录的Action栏位设置为"DELETE"
        //            try
        //            {
        //                SqlTransactionManager.Begin();

        //                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(mo, CacheType.MOBOM));

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.MoBOM eqCond = new _Schema.MoBOM();
        //                        eqCond.MO = mo;
        //                        eqCond.PartNo = partNo;
        //                        eqCond.Deviation = true;

        //                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM), new List<string>() { _Schema.MoBOM.fn_Action }, null, null, null, eqCond, null, null, null, null, null, null, null);
        //                    }
        //                }
        //                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //                sqlCtx.Params[_Schema.MoBOM.fn_MO].Value = mo;
        //                sqlCtx.Params[_Schema.MoBOM.fn_PartNo].Value = partNo;
        //                sqlCtx.Params[_Schema.MoBOM.fn_Deviation].Value = deviation;
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.MoBOM.fn_Action)].Value = "DELETE";
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.MoBOM.fn_Udt)].Value = cmDt;
        //                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

        //                SqlTransactionManager.Commit();
        //            }
        //            catch (Exception)
        //            {
        //                SqlTransactionManager.Rollback();
        //                throw;
        //            }
        //            finally
        //            {
        //                SqlTransactionManager.Dispose();
        //                SqlTransactionManager.End();
        //            }
        //        }

        //        public ModelBOM GetModelBOM(int modelBOMId)
        //        {
        //            try
        //            {
        //                ModelBOM ret = null;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                        cond.ID = modelBOMId;
        //                        cond.Flag = 1;//ModelBOM表加Flag
        //                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), cond, null, null);
        //                        sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag; //ModelBOM表加Flag
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.ModelBOM.fn_ID].Value = modelBOMId;
        //                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //                {
        //                    if (sqlR != null)
        //                    {
        //                        if (sqlR.Read())
        //                        {
        //                            ret = new ModelBOM();
        //                            ret.Alternative_item_group = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Alternative_item_group]);
        //                            //ret.Base_qty = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Base_qty]);
        //                            //ret.Bom = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Bom]);
        //                            ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Cdt]);
        //                            //ret.Change_number = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Change_number]);
        //                            ret.Component = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Component]);
        //                            ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Editor]);
        //                            ret.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_ID]);
        //                            //ret.Item_number = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Item_number]);
        //                            //ret.Item_text = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Item_text]);
        //                            ret.Material = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Material]);
        //                            //ret.Material_group = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Material_group]);
        //                            ret.Plant = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Plant]);
        //                            ret.Priority = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Priority]);
        //                            ret.Quantity = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Quantity]);
        //                            //ret.Sub_items = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Sub_items]);
        //                            ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Udt]);
        //                            //ret.UOM = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_UOM]);
        //                            //ret.Usage_probability = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Usage_probability]);
        //                            //ret.Valid_from_date = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Valid_from_date]);
        //                            //ret.Valid_to_date = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Valid_to_date]);
        //                            ret.Tracker.Clear();
        //                        }
        //                    }
        //                }
        //                return ret;
        //            }
        //            catch(Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public void UpdateModelBOM(ModelBOM item)
        //        {
        //            try
        //            {
        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                        cond.ID = item.ID;
        //                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), null, new List<string>() { _Schema.ModelBOM.fn_ID, _Schema.ModelBOM.fn_Flag }, null, null, cond, null, null, null, null, null, null, null);
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.ModelBOM.fn_ID].Value = item.ID;

        //                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Alternative_item_group)].Value = item.Alternative_item_group;
        //                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Base_qty)].Value = item.Base_qty;
        //                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Bom)].Value = item.Bom;
        //                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Change_number)].Value = item.Change_number;
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Component)].Value = item.Component;
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Editor)].Value = item.Editor;
        //                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Item_number)].Value = item.Item_number;
        //                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Item_text)].Value = item.Item_text;
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Material)].Value = item.Material;
        //                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Material_group)].Value = item.Material_group;
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Plant)].Value = item.Plant;
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Priority)].Value = item.Priority;
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Quantity)].Value = item.Quantity;
        //                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Sub_items)].Value = item.Sub_items;
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Udt)].Value = cmDt;
        //                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_UOM)].Value = item.UOM;
        //                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Usage_probability)].Value = item.Usage_probability;
        //                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Valid_from_date)].Value = item.Valid_from_date;
        //                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Valid_to_date)].Value = item.Valid_to_date;
        //                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        //public void ChangeModelBOM(ModelBOM item, int oldId)
        //        //{
        //        //    try
        //        //    {
        //        //        _Schema.SQLContext sqlCtx = null;
        //        //        lock (MethodBase.GetCurrentMethod())
        //        //        {
        //        //              if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        //              {
        //        //                  _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //        //                  cond.ID = oldId;
        //        //                  sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), null, null, null, null, cond, null, null, null, null, null, null, null);
        //        //              }
        //        //        }
        //        //        sqlCtx.Params[_Schema.ModelBOM.fn_ID].Value = oldId;
        //        //        DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Alternative_item_group)].Value = item.Alternative_item_group;
        //        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Base_qty)].Value = item.Base_qty;
        //        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Bom)].Value = item.Bom;
        //        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Cdt)].Value = cmDt;
        //        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Change_number)].Value = item.Change_number;
        //        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Component)].Value = item.Component;
        //        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Editor)].Value = item.Editor;
        //        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_ID)].Value = item.ID;
        //        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Item_number)].Value = item.Item_number;
        //        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Item_text)].Value = item.Item_text;
        //        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Material)].Value = item.Material;
        //        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Material_group)].Value = item.Material_group;
        //        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Plant)].Value = item.Plant;
        //        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Priority)].Value = item.Priority;
        //        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Quantity)].Value = item.Quantity;
        //        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Sub_items)].Value = item.Sub_items;
        //        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Udt)].Value = cmDt;
        //        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_UOM)].Value = item.UOM;
        //        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Usage_probability)].Value = item.Usage_probability;
        //        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Valid_from_date)].Value = item.Valid_from_date;
        //        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Valid_to_date)].Value = item.Valid_to_date;
        //        //        _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        //    }
        //        //    catch (Exception)
        //        //    {
        //        //        throw;
        //        //    }
        //        //}

        //        public void IncludeItemToAlternativeItemGroup(string value, string parent, string code)
        //        {
        //            //实现的SQL   UPDATE ModelBOM SET Alternative_item_group=”’+value+”’ where Material='" + parent + "' and Component='" + code + "'";
        //            try
        //            {
        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                        cond.Material = parent;
        //                        cond.Component = code;
        //                        cond.Flag = 1;//ModelBOM表加Flag
        //                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), new List<string>() { _Schema.ModelBOM.fn_Alternative_item_group }, null, null, null, cond, null, null, null, null, null, null, null);
        //                        sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag; //ModelBOM表加Flag
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = parent;
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = code;
        //                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Alternative_item_group)].Value = value;
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Udt)].Value = cmDt;
        //                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

                

        //        private bool PeekDeletedModelBOM(ModelBOM item)
        //        {
        //            SqlDataReader sqlR = null;
        //            try
        //            {
        //                bool ret = false;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                        cond.Material = item.Material;
        //                        cond.Component = item.Component;
        //                        cond.Flag = 1;
        //                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), null, new List<string>() { _Schema.ModelBOM.fn_ID }, cond, null, null, null, null, null, null, null);

        //                        sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (UPDLOCK) WHERE");

        //                        sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = 0;
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = item.Material;
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = item.Component;
        //                sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //                if (sqlR != null && sqlR.Read())
        //                {
        //                    item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_ID]);
        //                    ret = true;
        //                }
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //            finally
        //            {
        //                if (sqlR != null)
        //                {
        //                    sqlR.Close();
        //                }
        //            }
        //        }

        //        private void RecoverPartModelBOM(ModelBOM item)
        //        {
        //            try
        //            {
        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                        cond.Flag = 1;
        //                        cond.ID = item.ID;
        //                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), null, new List<string>() { _Schema.ModelBOM.fn_ID, _Schema.ModelBOM.fn_Component, _Schema.ModelBOM.fn_Material }, null, null, cond, null, null, null, null, null, null, null);

        //                        sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = 0;
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.ModelBOM.fn_ID].Value = item.ID;

        //                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Alternative_item_group)].Value = item.Alternative_item_group;
        //                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Base_qty)].Value = item.Base_qty;
        //                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Bom)].Value = item.Bom;
        //                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Change_number)].Value = item.Change_number;
        //                //sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = item.Component;
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Editor)].Value = item.Editor;
        //                //sqlCtx.Params[_Schema.ModelBOM.fn_ID].Value = item.ID;
        //                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Item_number)].Value = item.Item_number;
        //                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Item_text)].Value = item.Item_text;
        //                //sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = item.Material;
        //                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Material_group)].Value = item.Material_group;
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Plant)].Value = item.Plant;
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Priority)].Value = item.Priority;
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Quantity)].Value = item.Quantity;
        //                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Sub_items)].Value = item.Sub_items;
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Udt)].Value = cmDt;
        //                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_UOM)].Value = item.UOM;
        //                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Usage_probability)].Value = item.Usage_probability;
        //                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Valid_from_date)].Value = item.Valid_from_date;
        //                //sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Valid_to_date)].Value = item.Valid_to_date;
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Flag)].Value = 1;
        //                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        private void InsertModelBOM(ModelBOM item)
        //        {
        //            try
        //            {
        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM));
        //                    }
        //                }
        //                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Alternative_item_group].Value = item.Alternative_item_group;
        //                //sqlCtx.Params[_Schema.ModelBOM.fn_Base_qty].Value = item.Base_qty;
        //                //sqlCtx.Params[_Schema.ModelBOM.fn_Bom].Value = item.Bom;
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Cdt].Value = cmDt;
        //                //sqlCtx.Params[_Schema.ModelBOM.fn_Change_number].Value = item.Change_number;
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = item.Component;
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Editor].Value = item.Editor;
        //                //sqlCtx.Params[_Schema.ModelBOM.fn_ID].Value = item.ID;
        //                //sqlCtx.Params[_Schema.ModelBOM.fn_Item_number].Value = item.Item_number;
        //                //sqlCtx.Params[_Schema.ModelBOM.fn_Item_text].Value = item.Item_text;
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = item.Material;
        //                //sqlCtx.Params[_Schema.ModelBOM.fn_Material_group].Value = item.Material_group;
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Plant].Value = item.Plant;
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Priority].Value = item.Priority;
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Quantity].Value = item.Quantity;
        //                //sqlCtx.Params[_Schema.ModelBOM.fn_Sub_items].Value = item.Sub_items;
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Udt].Value = cmDt;
        //                //sqlCtx.Params[_Schema.ModelBOM.fn_UOM].Value = item.UOM;
        //                //sqlCtx.Params[_Schema.ModelBOM.fn_Usage_probability].Value = item.Usage_probability;
        //                //sqlCtx.Params[_Schema.ModelBOM.fn_Valid_from_date].Value = item.Valid_from_date;
        //                //sqlCtx.Params[_Schema.ModelBOM.fn_Valid_to_date].Value = item.Valid_to_date;
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = 1;//ModelBOM表加Flag
        //                item.ID = Convert.ToInt32( _Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public void DeleteModelBOMByCode(string parentCode, string code)
        //        {
        //            //"DELETE FROM ModelBOM where Material='" + parentCode + "' and Component='"    + code + "'"
        //            try
        //            {
        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.ModelBOM eqCond = new _Schema.ModelBOM();
        //                        eqCond.Material = parentCode;
        //                        eqCond.Component = code;
        //                        eqCond.Flag = 1;

        //                        //sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), eqCond, null, null);
        //                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), new List<string>() { _Schema.ModelBOM.fn_Flag }, null, null, null, eqCond, null, null, null, null, null, null, null);

        //                        sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = eqCond.Flag;
        //                        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Flag)].Value = 0;
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = parentCode;
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = code;
        //                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Udt)].Value = cmDt;
        //                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public IList<string> GetModelBOMByMaterialAndAlternativeItemGroup(string code, string itemGroup)
        //        {
        //            //SELECT Component from ModelBOM where Material='code' and 
        //            //Alternative_item_group = 'itemGroup'
        //            try
        //            {
        //                IList<string> ret = new List<string>();

        //                _Schema.SQLContext sqlCtx = ComposeSubSQLForGetModelBOMByMaterialAndAlternativeItemGroup(code, itemGroup);
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = code;
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Alternative_item_group].Value = itemGroup;
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = 1;//ModelBOM表加Flag
        //                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //                {
        //                    while (sqlR != null && sqlR.Read())
        //                    {
        //                        string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Component]);
        //                        ret.Add(item);
        //                    }
        //                }
        //                return ret;
        //            }
        //            catch(Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public void IncludeAllItemToAlternativeItemGroup(string code1, string code2, string itemGroup1, string itemGroup2)
        //        {
        //            //UPDATE ModelBOM SET Alternative_item_group='itemGroup1' where Material='code1' and Component in 
        //            //(SELECT Component from ModelBOM where Material='code2' and 
        //            //Alternative_item_group = 'itemGroup2')
        //            try
        //            {
        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                        cond.Material = code1;

        //                        _Schema.ModelBOM insetCond = new _Schema.ModelBOM();
        //                        insetCond.Component = "INSET";

        //                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), new List<string>() { _Schema.ModelBOM.fn_Alternative_item_group }, null, null, null, cond, null, null, null, null, null, null, insetCond);

        //                        _Schema.SQLContext sqlCtx_sub = ComposeSubSQLForGetModelBOMByMaterialAndAlternativeItemGroup(code2, itemGroup2);

        //                        foreach (KeyValuePair<string, SqlParameter> value in sqlCtx_sub.Params)
        //                        {
        //                            sqlCtx.Params.Add("s_" + value.Key, new SqlParameter("@s_" + value.Value.ParameterName.Substring(1), value.Value.SqlDbType));
        //                        }
        //                        sqlCtx.Sentence = sqlCtx.Sentence.Replace(_Schema.Func.DecInSet(_Schema.ModelBOM.fn_Component), sqlCtx_sub.Sentence.Replace("@" + _Schema.ModelBOM.fn_Material, "@s_" + _Schema.ModelBOM.fn_Material).Replace("@" + _Schema.ModelBOM.fn_Alternative_item_group, "@s_" + _Schema.ModelBOM.fn_Alternative_item_group).Replace("@" + _Schema.ModelBOM.fn_Flag, "@s_" + _Schema.ModelBOM.fn_Flag));//ModelBOM表加Flag
        //                    }
        //                }
        //                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = code1;
        //                sqlCtx.Params["s_" + _Schema.ModelBOM.fn_Material].Value = code2;
        //                sqlCtx.Params["s_" + _Schema.ModelBOM.fn_Alternative_item_group].Value = itemGroup2;
        //                sqlCtx.Params["s_" + _Schema.ModelBOM.fn_Flag].Value = 1;//ModelBOM表加Flag
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Alternative_item_group)].Value = itemGroup1;
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Udt)].Value = cmDt;
        //                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        private _Schema.SQLContext ComposeSubSQLForGetModelBOMByMaterialAndAlternativeItemGroup(string material, string alternativeItemGroup)
        //        {
        //            _Schema.SQLContext sqlCtx = null;
        //            lock (MethodBase.GetCurrentMethod())
        //            {
        //                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                {
        //                    _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                    cond.Material = material;
        //                    cond.Alternative_item_group = alternativeItemGroup;
        //                    cond.Flag = 1;//ModelBOM表加Flag
        //                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "DISTINCT", new List<string>() { _Schema.ModelBOM.fn_Component }, cond, null, null, null, null, null, null, null);
        //                }
        //            }
        //            return sqlCtx;
        //        }

        //        public string ExcludeAlternativeItem(string parent, string code)
        //        {
        //            ////UPDATE ModelBOM SET Alternative_item_group=(SELECT max(CONVERT(int,Alternative_item_group)) + 1 from ModelBOM) where Material='" + parent + "' and Component='" + code + "
        //            //UPDATE ModelBOM SET Alternative_item_group=newid() where Material='" + parent + "' and Component='" + code + "
        //            try
        //            {
        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                        cond.Material = parent;
        //                        cond.Component = code;
        //                        cond.Flag = 1;//ModelBOM表加Flag

        //                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), new List<string>() { _Schema.ModelBOM.fn_Alternative_item_group }, null, null, null, cond, null, null, null, null, null, null, null);

        //                        //_Schema.SQLContext sqlCtx_sub = ComposeSubSQLForExcludeAlternativeItem();
        //                        //sqlCtx.Sentence = sqlCtx.Sentence.Replace(sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Alternative_item_group)].ParameterName,string.Format("({0})+1",sqlCtx_sub.Sentence));
        //                        //sqlCtx.Params.Remove(_Schema.Func.DecSV(_Schema.ModelBOM.fn_Alternative_item_group));

        //                        sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag;//ModelBOM表加Flag
        //                    }
        //                }
        //                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //                Guid newGuid = Guid.NewGuid();
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = parent;
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = code;
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Alternative_item_group)].Value = newGuid.ToString();
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Udt)].Value = cmDt;
        //                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //                return newGuid.ToString();
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        //private _Schema.SQLContext ComposeSubSQLForExcludeAlternativeItem()
        //        //{
        //        //    //SELECT max(CONVERT(int,Alternative_item_group)) + 1 from ModelBOM
        //        //    _Schema.SQLContext sqlCtx = null;
        //                //lock (MethodBase.GetCurrentMethod())
        //                //{
        //        //    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        //    {
        //        //        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "MAX", new List<string>() { _Schema.ModelBOM.fn_Alternative_item_group }, null, null, null, null, null, null, null, null);
        //        //    }
        //        //}
        //        //    return sqlCtx;
        //        //}

        //        public void DeleteSubModelByCode(string code)
        //        {
        //            try
        //            {
        //                //"DELETE FROM ModelBOM where Material='" + code + "'";
        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        //ModelBOM表加Flag
        //                        _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                        cond.Material = code;
        //                        cond.Flag = 1;
        //                        //sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), cond, null, null);
        //                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), new List<string>() { _Schema.ModelBOM.fn_Flag }, null, null, null, cond, null, null, null, null, null, null, null);

        //                        sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag;              //ModelBOM表加Flag
        //                        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Flag)].Value = 0;  //ModelBOM表加Flag
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = code;
        //                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Udt)].Value = cmDt;
        //                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public DataTable GetModelBOMTypes()
        //        {
        //            //SELECT distinct Material_group, flag=1 FROM ModelBOM  
        //            //union 
        //            //SELECT DISTINCT PartType , flag=0 
        //            //FROM Part a 
        //            //left join (SELECT DISTINCT Material_group FROM ModelBOM) as b 
        //            //on a.PartType=b.Material_group
        //            //WHERE b.Material_group is null
        //            //order by Material_group
        //            try
        //            {
        //                //_Schema.SQLContext sqlCtx1 = ComposeSubSQLForGetModelBOMTypes_FromModelBOM();
        //                //DataTable ret1 = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx1.Sentence, sqlCtx1.Params.Values.ToArray<SqlParameter>());

        //                _Schema.SQLContext sqlCtx2 = ComposeSubSQLForGetModelBOMTypes_Part();
        //                DataTable ret2 = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx2.Sentence, sqlCtx2.Params.Values.ToArray<SqlParameter>());
                       
        //                //var set1 = (from item in DataTableExtensions.AsEnumerable(ret1) select item).ToList();
        //                var set2 = (from item in DataTableExtensions.AsEnumerable(ret2) select item).ToList();

        //               // var setU = (from item in set1.Union(set2) orderby item.Field<string>(0) select item).ToList();

        //               // if (setU != null && setU.Count > 0)
        //               //     return DataTableExtensions.CopyToDataTable<DataRow>(setU);

        //                if (set2 != null && set2.Count > 0)
        //                    return DataTableExtensions.CopyToDataTable<DataRow>(set2);
        //                else
        //                    return new DataTable();
        //            }
        //            catch(Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        //private _Schema.SQLContext ComposeSubSQLForGetModelBOMTypes_FromModelBOM()
        //        //{
        //        //    _Schema.SQLContext sqlCtx = null;
        //        //    lock (MethodBase.GetCurrentMethod())
        //        //    {
        //        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        //        {
        //        //            _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //        //            cond.Flag = 1;//ModelBOM表加Flag
        //        //            sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "DISTINCT", new List<string>() { _Schema.ModelBOM.fn_Material_group }, cond, null, null, null, null, null, null, null);
        //        //            sqlCtx.Sentence = sqlCtx.Sentence.Replace(" FROM ", ", 1 AS flag FROM ");
        //        //            sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag;//ModelBOM表加Flag
        //        //        }
        //        //    }
        //        //    return sqlCtx;
        //        //}

        //        private _Schema.SQLContext ComposeSubSQLForGetModelBOMTypes_Part()
        //        {
        //            //SELECT DISTINCT PartType, flag=0 
        //            //FROM Part A 
        //            //LEFT JOIN (SELECT DISTINCT Material_group FROM ModelBOM) AS B 
        //            //ON A.PartType=B.Material_group
        //            //WHERE B.Material_group IS NULL
        //            _Schema.SQLContext sqlCtx = null;
        //            lock (MethodBase.GetCurrentMethod())
        //            {
        //                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                {
        //                    sqlCtx = new _Schema.SQLContext();
        //                    sqlCtx.Sentence = "Select distinct BomNodeType as PartType, 0 as flag from Part";


        //                    //sqlCtx.Sentence = "SELECT DISTINCT {2}, flag=0 " +
        //                    //                   "FROM {0} A ";

        //                    //sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.Part).Name,
        //                    //                                                 typeof(_Schema.ModelBOM).Name,
        //                    //                                                 _Schema.Part.fn_PartType);

        //                    //sqlCtx.Sentence =   "SELECT DISTINCT {2}, flag=0 " +
        //                    //                    "FROM {0} A " +
        //                    //                    "LEFT JOIN (SELECT DISTINCT {3} FROM {1} WHERE {5}=1) AS B " +
        //                    //                    "ON A.{2}=B.{3} " +
        //                    //                    "WHERE B.{3} IS NULL AND A.{4}=1 ";//Part表加Flag

        //                    //sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.Part).Name,
        //                    //                                                 typeof(_Schema.ModelBOM).Name,
        //                    //                                                 _Schema.Part.fn_PartType,
        //                    //                                                 _Schema.ModelBOM.fn_Material_group,
        //                    //                                                 _Schema.Part.fn_Flag,//Part表加Flag
        //                    //                                                 _Schema.ModelBOM.fn_Flag);//ModelBOM表加Flag

        //                    _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);

        //                    #region OLD
        //                    //sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Part), "DISTINCT", new List<string>() { _Schema.Part.fn_PartType }, null, null, null, null, null, null, null, null);
        //                    //sqlCtx.Sentence = sqlCtx.Sentence.Replace(" FROM ", ", 0 AS flag FROM ");
        //                    //sqlCtx.Sentence = sqlCtx.Sentence + string.Format(" AND {0} NOT IN (SELECT DISTINCT {1} FROM ModelBOM) ", _Schema.Part.fn_PartType, _Schema.ModelBOM.fn_Material_group);
        //                    #endregion
        //                }
        //            }
        //            return sqlCtx;
        //        }

        //        public DataTable GetModelBOMCodes(string parentCode, string match)
        //        {
        //            //SELECT distinct Material, BOMApproveDate 
        //            //FROM ModelBOM mb left JOIN Model on (mb.Material = Model.Model) 
        //            //where Material_group=@parentCode
        //            //if (match != "")
        //            //{
        //            //   and Material like '%" + match + "%'";
        //            //}
        //            //order by Material
        //            try
        //            {
        //                DataTable ret = null;

        //                _Schema.SQLContext sqlCtx = null;
        //                _Schema.TableAndFields tf1 = null;
        //                _Schema.TableAndFields tf2 = null;
        //                _Schema.TableAndFields[] tblAndFldsesArray = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
        //                    {
        //                        Console.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId);
        //                        tf1 = new _Schema.TableAndFields();
        //                        tf1.Table = typeof(_Schema.ModelBOM);
        //                        _Schema.ModelBOM eqCond = new _Schema.ModelBOM();
        //                        //eqCond.Material_group = parentCode;
        //                        eqCond.Flag = 1;//ModelBOM表加Flag
        //                        tf1.equalcond = eqCond;
        //                        _Schema.ModelBOM likeCond = new _Schema.ModelBOM();
        //                        likeCond.Material = "%match%";
        //                        tf1.likecond = likeCond;
        //                        tf1.ToGetFieldNames.Add(_Schema.ModelBOM.fn_Material);

        //                        tf2 = new _Schema.TableAndFields();
        //                        tf2.Table = typeof(_Schema.Model);
        //                        tf2.ToGetFieldNames.Add(_Schema.Model.fn_BOMApproveDate);
        //                        tf2.ToGetFieldNames.Add(_Schema.Model.fn_Editor);

        //                        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
        //                        _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.ModelBOM.fn_Material, tf2, _Schema.Model.fn_model);
        //                        tblCnntIs.Add(tc1);

        //                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

        //                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };

        //                        _Schema.TableBiJoinedLogic tblBiJndLgc = new _Schema.TableBiJoinedLogic();
        //                        tblBiJndLgc.Add(tf1);
        //                        tblBiJndLgc.Add(_Schema.Func.LEFTJOIN);
        //                        tblBiJndLgc.Add(tf2);
        //                        tblBiJndLgc.Add(tc1);

        //                        sqlCtx = _Schema.Func.GetConditionedComprehensiveJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts, tblBiJndLgc);

        //                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Func.DecAliasInner(tf1.alias, _Schema.ModelBOM.fn_Material));

        //                        sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.ModelBOM.fn_Flag)].Value = eqCond.Flag;//ModelBOM表加Flag
        //                    }
        //                }
        //                tf1 = tblAndFldsesArray[0];
        //                tf2 = tblAndFldsesArray[1];

        //                _Schema.SQLContext sqlCtx_final = new _Schema.SQLContext(sqlCtx);
        //                //sqlCtx_final.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.ModelBOM.fn_Material_group)].Value = parentCode;
        //                if (string.IsNullOrEmpty(match))
        //                {
        //                    sqlCtx_final.Sentence = sqlCtx_final.Sentence.Replace(_Schema.Func.DecAliasInner(tf1.alias, _Schema.ModelBOM.fn_Material) + " LIKE " + sqlCtx_final.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.ModelBOM.fn_Material)].ParameterName, "1=1");
        //                    sqlCtx_final.Params.Remove(_Schema.Func.DecAlias(tf1.alias, _Schema.ModelBOM.fn_Material));
        //                }
        //                else
        //                {
        //                    sqlCtx_final.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.ModelBOM.fn_Material)].Value = "%" + match  + "%";
        //                }
        //                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx_final.Sentence, sqlCtx_final.Params.Values.ToArray<SqlParameter>());
        //                ret = _Schema.Func.SortColumns(ret, new int[] { sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias,_Schema.ModelBOM.fn_Material)],
        //                                                                sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias,_Schema.Model.fn_BOMApproveDate)],
        //                                                                sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias,_Schema.Model.fn_Editor)]
        //                                                                });
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public DataTable GetParentModelBOMByCode(string code)
        //        {
        //            //SELECT distinct Material, Material_group, Component FROM ModelBOM where Component=@code
        //            try
        //            {
        //                DataTable ret = null;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                        cond.Component = code;
        //                        cond.Flag = 1;//ModelBOM表加Flag
        //                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "DISTINCT", new List<string>() { _Schema.ModelBOM.fn_Material,  _Schema.ModelBOM.fn_Component }, cond, null, null, null, null, null, null, null);
        //                        sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag;//ModelBOM表加Flag
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = code;
        //                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //                ret = _Schema.Func.SortColumns(ret, new int[] { sqlCtx.Indexes[_Schema.ModelBOM.fn_Material],
        //                                                                //sqlCtx.Indexes[_Schema.ModelBOM.fn_Material_group],
        //                                                                sqlCtx.Indexes[_Schema.ModelBOM.fn_Component]
        //                                                                });    
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public DataTable GetSubModelBOMByCode(string code)
        //        {
        //            //SELECT A.Component AS Material, B.Material_group, A.Quantity, A.Alternative_item_group FROM ModelBOM AS A
        //            //INNER JOIN (
        //            //SELECT DISTINCT Material, Material_group FROM ModelBOM 
        //            //UNION 
        //            //SELECT DISTINCT PartNo AS Material, PartType AS Material_group FROM Part
        //            //LEFT OUTER JOIN (SELECT DISTINCT Material FROM ModelBOM) AS C
        //            //ON Part.PartNo=C.Material WHERE C.Material IS NULL
        //            //) AS B ON A.Component=B.Material WHERE A.Material='code' ORDER BY A.Material
        //            try
        //            {
        //                DataTable ret = null;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        sqlCtx = new _Schema.SQLContext();

        //                        sqlCtx.Sentence = "SELECT A.{6} AS {2}, A.{7}, A.{8} FROM {0} AS A " +
        //                                            "INNER JOIN (" +
        //                                            "SELECT DISTINCT {2} FROM {0} WHERE {10}=1 " + //ModelBOM表加Flag
        //                                            "UNION " +
        //                                            "SELECT DISTINCT {4} AS {2} FROM {1} " +
        //                                            "LEFT OUTER JOIN (SELECT DISTINCT {2} FROM {0} WHERE {10}=1 ) AS C " +
        //                                            "ON {1}.{4}=C.{2} WHERE C.{2} IS NULL AND {1}.{9}=1 " + //Part表加Flag
        //                                            ") AS B ON A.{6}=B.{2} WHERE A.{2}=@{2} AND A.{10}=1 ORDER BY A.{2}";
        //                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.ModelBOM).Name,
        //                                                                        typeof(_Schema.Part).Name,
        //                                                                        _Schema.ModelBOM.fn_Material,
        //                                                                       "",
        //                                                                        _Schema.Part.fn_PartNo,
        //                                                                        _Schema.Part.fn_PartType,
        //                                                                        _Schema.ModelBOM.fn_Component,
        //                                                                        _Schema.ModelBOM.fn_Quantity,
        //                                                                        _Schema.ModelBOM.fn_Alternative_item_group,
        //                                                                        _Schema.Part.fn_Flag,//Part表加Flag
        //                                                                        _Schema.ModelBOM.fn_Flag);//ModelBOM表加Flag


        //                        //sqlCtx.Sentence =   "SELECT A.{6} AS {2}, B.{3}, A.{7}, A.{8} FROM {0} AS A " + 
        //                        //                    "INNER JOIN (" +
        //                        //                    "SELECT DISTINCT {2}, {3} FROM {0} WHERE {10}=1 " + //ModelBOM表加Flag
        //                        //                    "UNION " + 
        //                        //                    "SELECT DISTINCT {4} AS {2}, {5} AS {3} FROM {1} " +
        //                        //                    "LEFT OUTER JOIN (SELECT DISTINCT {2} FROM {0} WHERE {10}=1 ) AS C " +
        //                        //                    "ON {1}.{4}=C.{2} WHERE C.{2} IS NULL AND {1}.{9}=1 " + //Part表加Flag
        //                        //                    ") AS B ON A.{6}=B.{2} WHERE A.{2}=@{2} AND A.{10}=1 ORDER BY A.{2}";
        //                        //sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.ModelBOM).Name,
        //                        //                                                typeof(_Schema.Part).Name,
        //                        //                                                _Schema.ModelBOM.fn_Material,
        //                        //                                                _Schema.ModelBOM.fn_Material_group,
        //                        //                                                _Schema.Part.fn_PartNo,
        //                        //                                                _Schema.Part.fn_PartType,
        //                        //                                                _Schema.ModelBOM.fn_Component,
        //                        //                                                _Schema.ModelBOM.fn_Quantity,
        //                        //                                                _Schema.ModelBOM.fn_Alternative_item_group,
        //                        //                                                _Schema.Part.fn_Flag,//Part表加Flag
        //                        //                                                _Schema.ModelBOM.fn_Flag);//ModelBOM表加Flag

        //                        #region OLD
        //                        //sqlCtx.Sentence = "SELECT a.{2} AS {3}, b.{4}, a.{7}, a.{8} " + 
        //                        //                  "FROM {0} AS a INNER JOIN " +
        //                        //                  "(SELECT DISTINCT {3}, {4} FROM {0} " + 
        //                        //                  "UNION " + 
        //                        //                  "SELECT DISTINCT {5} AS {3}, {6} AS {4} FROM {1} " +
        //                        //                  "WHERE {5} NOT IN (SELECT {3} FROM {0})) AS b " +
        //                        //                  "ON a.{2}=b.{3} AND a.{3}=@{3} ORDER BY a.{3} ";
            
        //                        // sqlCtx.Sentence = string.Format(sqlCtx.Sentence,typeof(_Schema.ModelBOM).Name,
        //                        //                                                typeof(_Schema.Part).Name,
        //                        //                                                _Schema.ModelBOM.fn_Component,
        //                        //                                                _Schema.ModelBOM.fn_Material,
        //                        //                                                _Schema.ModelBOM.fn_Material_group,
        //                        //                                                _Schema.Part.fn_PartNo,
        //                        //                                                _Schema.Part.fn_PartType,
        //                        //                                                _Schema.ModelBOM.fn_Quantity,
        //                        //                                                _Schema.ModelBOM.fn_Alternative_item_group);
        //                        #endregion

        //                        sqlCtx.Params.Add(_Schema.ModelBOM.fn_Material, new SqlParameter("@" + _Schema.ModelBOM.fn_Material, SqlDbType.VarChar));

        //                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = code;
        //                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

        //                #region OLD
        //                //SELECT distinct Component as Material, Material_group, Quantity, Alternative_item_group FROM ModelBOM where Material=@code
        //                //_Schema.SQLContext sqlCtx = null;
        //                //lock (MethodBase.GetCurrentMethod())
        //                //{
        //                //    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                //    {
        //                //        _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                //        cond.Material = code;
        //                //        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "DISTINCT", new List<string>() { _Schema.ModelBOM.fn_Component, _Schema.ModelBOM.fn_Material_group, _Schema.ModelBOM.fn_Quantity, _Schema.ModelBOM.fn_Alternative_item_group }, cond, null, null, null, null, null, null, null);
        //                //    }
        //                //}
        //                //sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = code;
        //                //ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //                //ret = _Schema.Func.SortColumns(ret, new int[] { sqlCtx.Indexes[_Schema.ModelBOM.fn_Component],
        //                //                                    sqlCtx.Indexes[_Schema.ModelBOM.fn_Material_group],
        //                //                                    sqlCtx.Indexes[_Schema.ModelBOM.fn_Quantity],
        //                //                                    sqlCtx.Indexes[_Schema.ModelBOM.fn_Alternative_item_group]});
        //                #endregion

        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public DataTable GetAlternativeItems(string code, string alternativeItemGroup)
        //        {
        //            //SELECT distinct Component as Material from ModelBOM where Material=@code and Alternative_item_group=@alternativeItemGroup
        //            try
        //            {
        //                DataTable ret = null;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                        cond.Material = code;
        //                        cond.Alternative_item_group = alternativeItemGroup;
        //                        cond.Flag = 1;//ModelBOM表加Flag
        //                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "DISTINCT", new List<string>() { _Schema.ModelBOM.fn_Component }, cond, null, null, null, null, null, null, null);
        //                        sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag;//ModelBOM表加Flag 
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = code;
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Alternative_item_group].Value = alternativeItemGroup;
        //                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public DataTable GetOffspringModelBOM(string code)
        //        {
        //            //SELECT distinct Material, AssemblyCode 
        //            //FROM ModelBOM left join MO on ModelBOM.Material=MO.Model left join MoBOM on MO.MO=MoBOM.MO 
        //            //where Material in (select Component from ModelBOM where Material=@code)
        //            try
        //            {
        //                DataTable ret = null;

        //                _Schema.SQLContext sqlCtx = null;
        //                _Schema.TableAndFields tf1 = null;
        //                _Schema.TableAndFields tf2 = null;
        //                _Schema.TableAndFields tf3 = null;
        //                _Schema.TableAndFields[] tblAndFldsesArray = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
        //                    {
        //                        tf1 = new _Schema.TableAndFields();
        //                        tf1.Table = typeof(_Schema.ModelBOM);
        //                        _Schema.ModelBOM eqCond = new _Schema.ModelBOM();
        //                        eqCond.Flag = 1;//ModelBOM表加Flag
        //                        tf1.equalcond = eqCond;
        //                        _Schema.ModelBOM insetCond = new _Schema.ModelBOM();
        //                        insetCond.Material = "INSET";
        //                        tf1.inSetcond = insetCond;
        //                        tf1.ToGetFieldNames.Add(_Schema.ModelBOM.fn_Material);

        //                        tf2 = new _Schema.TableAndFields();
        //                        tf2.Table = typeof(_Schema.MO);
        //                        tf2.ToGetFieldNames = null;

        //                        tf3 = new _Schema.TableAndFields();
        //                        tf3.Table = typeof(_Schema.MoBOM);
        //                        tf3.ToGetFieldNames.Add(_Schema.MoBOM.fn_AssemblyCode);

        //                        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
        //                        _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.ModelBOM.fn_Material, tf2, _Schema.MO.fn_Model);
        //                        tblCnntIs.Add(tc1);
        //                        _Schema.TableConnectionItem tc2 = new _Schema.TableConnectionItem(tf2, _Schema.MO.fn_Mo, tf3, _Schema.MoBOM.fn_MO);
        //                        tblCnntIs.Add(tc2);

        //                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

        //                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2, tf3 };

        //                        _Schema.TableBiJoinedLogic tblBiJndLgc = new _Schema.TableBiJoinedLogic();
        //                        tblBiJndLgc.Add(tf1);
        //                        tblBiJndLgc.Add(_Schema.Func.LEFTJOIN);
        //                        tblBiJndLgc.Add(tf2);
        //                        tblBiJndLgc.Add(tc1);
        //                        tblBiJndLgc.Add(_Schema.Func.LEFTJOIN);
        //                        tblBiJndLgc.Add(tf3);
        //                        tblBiJndLgc.Add(tc2);

        //                        sqlCtx = _Schema.Func.GetConditionedComprehensiveJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts, tblBiJndLgc);

        //                        sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.ModelBOM.fn_Flag)].Value = eqCond.Flag;//ModelBOM表加Flag;

        //                        _Schema.SQLContext sqlCtx_sub = ComposeSubSQLForGetComponentByMaterial(code);

        //                        foreach (KeyValuePair<string, SqlParameter> value in sqlCtx_sub.Params)
        //                        {
        //                            sqlCtx.Params.Add("s_" + value.Key, new SqlParameter("@s_" + value.Value.ParameterName.Substring(1), value.Value.SqlDbType));
        //                        }
        //                        sqlCtx.Sentence = sqlCtx.Sentence.Replace(_Schema.Func.DecAlias(tf1.alias, _Schema.Func.DecInSet(_Schema.ModelBOM.fn_Material)), sqlCtx_sub.Sentence.Replace("@" + _Schema.ModelBOM.fn_Material, "@s_" + _Schema.ModelBOM.fn_Material).Replace("@" + _Schema.ModelBOM.fn_Flag, "@s_" + _Schema.ModelBOM.fn_Flag));//ModelBOM表加Flag
        //                    }
        //                }
        //                tf1 = tblAndFldsesArray[0];
        //                tf2 = tblAndFldsesArray[1];
        //                tf3 = tblAndFldsesArray[2];

        //                sqlCtx.Params["s_" + _Schema.ModelBOM.fn_Material].Value = code;
        //                sqlCtx.Params["s_" + _Schema.ModelBOM.fn_Flag].Value = 1;//ModelBOM表加Flag
        //                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //                ret = _Schema.Func.SortColumns(ret, new int[] { sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.ModelBOM.fn_Material)],
        //                                                                sqlCtx.Indexes[_Schema.Func.DecAlias(tf3.alias, _Schema.MoBOM.fn_AssemblyCode)]
        //                                                                });                
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        //public DataTable GetMoBOMByModel(string model)
        //        //{
        //        //    //SELECT distinct MO, Qty, PrintQty as StartQty, Udt FROM MO where Model=@model
        //        //    try
        //        //    {
        //        //        DataTable ret = null;

        //        //        _Schema.SQLContext sqlCtx = null;
        //        //        lock (MethodBase.GetCurrentMethod())
        //        //        {
        //        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        //            {
        //        //                _Schema.MO cond = new _Schema.MO();
        //        //                cond.Model = model;
        //        //                sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MO), "DISTINCT", new List<string>() { _Schema.MO.fn_Mo, _Schema.MO.fn_Qty, _Schema.MO.fn_Print_Qty, _Schema.MO.fn_Udt }, cond, null, null, null, null, null, null, null);
        //        //            }
        //        //        }
        //        //        sqlCtx.Params[_Schema.MO.fn_Model].Value = model;
        //        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        //        ret = _Schema.Func.SortColumns(ret, new int[] { sqlCtx.Indexes[_Schema.MO.fn_Mo],
        //        //                                                        sqlCtx.Indexes[_Schema.MO.fn_Qty],
        //        //                                                        sqlCtx.Indexes[_Schema.MO.fn_Print_Qty],
        //        //                                                        sqlCtx.Indexes[_Schema.MO.fn_Udt]
        //        //                                                        });
        //        //        return ret;
        //        //    }
        //        //    catch (Exception)
        //        //    {
        //        //        throw;
        //        //    }
        //        //}

        //        public DataTable GetMaterialsByCode(string code)
        //        {
        //            //SELECT Material from ModelBOM where Material =@code
        //            //UNION 
        //            //SELECT PartNo as Material from Part where PartNo=@code
        //            try
        //            {
        //                _Schema.SQLContext sqlCtx1 = ComposeSubSQLForGetMaterialsByCode_FromModelBOM(code);
        //                sqlCtx1.Params[_Schema.ModelBOM.fn_Material].Value = code;
        //                sqlCtx1.Params[_Schema.ModelBOM.fn_Flag].Value = 1;//ModelBOM表加Flag
        //                DataTable ret1 = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx1.Sentence, sqlCtx1.Params.Values.ToArray<SqlParameter>());

        //                _Schema.SQLContext sqlCtx2 = ComposeSubSQLForGetMaterialsByCode_FromPart(code);
        //                sqlCtx2.Params[_Schema.Part.fn_PartNo].Value = code;
        //                sqlCtx2.Params[_Schema.Part.fn_Flag].Value = 1;//Part表加Flag
        //                DataTable ret2 = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx2.Sentence, sqlCtx2.Params.Values.ToArray<SqlParameter>());

        //                var set1 = (from item in DataTableExtensions.AsEnumerable(ret1) select item).ToList();
        //                var set2 = (from item in DataTableExtensions.AsEnumerable(ret2) select item).ToList();

        //                var setU = (from item in set1.Union(set2) select item).ToList();

        //                if (setU != null && setU.Count > 0)
        //                    return DataTableExtensions.CopyToDataTable<DataRow>(setU);
        //                else
        //                    return new DataTable();
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        private _Schema.SQLContext ComposeSubSQLForGetMaterialsByCode_FromModelBOM(string code)
        //        {
        //            _Schema.SQLContext sqlCtx = null;
        //            lock (MethodBase.GetCurrentMethod())
        //            {
        //                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                {
        //                    _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                    cond.Material = code;
        //                    cond.Flag = 1;//ModelBOM表加Flag
        //                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "DISTINCT", new List<string>() { _Schema.ModelBOM.fn_Material }, cond, null, null, null, null, null, null, null);
        //                }
        //            }
        //            return sqlCtx;
        //        }

        //        private _Schema.SQLContext ComposeSubSQLForGetMaterialsByCode_FromPart(string code)
        //        {
        //            _Schema.SQLContext sqlCtx = null;
        //            lock (MethodBase.GetCurrentMethod())
        //            {
        //                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                {
        //                    _Schema.Part cond = new _Schema.Part();
        //                    cond.PartNo = code;
        //                    cond.Flag = 1;//Part表加Flag
        //                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Part), "DISTINCT", new List<string>() { _Schema.Part.fn_PartNo }, cond, null, null, null, null, null, null, null);
        //                }
        //            }
        //            return sqlCtx;
        //        }

        //        //public DataTable GetMaterialByModel(string code)
        //        //{
        //        //    //SELECT Material FROM Model where Model=@code
        //        //}

                

        //        public DataTable GetModelById(string code)
        //        {
        //            //SELECT Model FROM Model where Model=@code
        //            try
        //            {
        //                DataTable ret = null;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.Model cond = new _Schema.Model();
        //                        cond.model = code;
        //                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Model), null, new List<string>() { _Schema.Model.fn_model }, cond, null, null, null, null, null, null, null);
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.Model.fn_model].Value = code;
        //                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public DataTable GetComponentQuantityByMaterial(string code)
        //        {
        //            //SELECT distinct Component, Quantity FROM ModelBOM where Material=@model
        //            try
        //            {
        //                DataTable ret = null;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                        cond.Material = code;
        //                        cond.Flag = 1;//ModelBOM表加Flag
        //                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "DISTINCT", new List<string>() { _Schema.ModelBOM.fn_Component, _Schema.ModelBOM.fn_Quantity }, cond, null, null, null, null, null, null, null);
        //                        sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag; //ModelBOM表加Flag
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = code;
        //                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //                ret = _Schema.Func.SortColumns(ret, new int[] { sqlCtx.Indexes[_Schema.ModelBOM.fn_Component],
        //                                                                sqlCtx.Indexes[_Schema.ModelBOM.fn_Quantity]
        //                                                                }); 
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public DataTable GetTypeOfCode(string code)
        //        {
        //            //SELECT Material_group FROM  ModelBOM WHERE Material = 'code' 
        //            //UNION
        //            //SELECT PartType FROM  Part  WHERE PartNo not in (SELECT Material FROM ModelBOM ) AND  PartNo ='code'
        //            try
        //            {
        //                DataTable ret = null;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        sqlCtx = new _Schema.SQLContext();

        //                        //sqlCtx.Sentence = "SELECT {5} FROM {0} WHERE {2}=@Code AND {7}=1 " + //ModelBOM表加Flag
        //                        //                    "UNION " + 
        //                        //                    "SELECT {4} FROM {1} " + 
        //                        //                    "LEFT OUTER JOIN {0} ON {1}.{3}={0}.{2} " +
        //                        //                    "WHERE {0}.{2} IS NULL AND {3}=@Code AND {1}.{6}=1 AND {0}.{7}=1 ";//Part表加Flag//ModelBOM表加Flag


        //                        //sqlCtx.Sentence = 
        //                        //                    "SELECT DISTINCT {4} AS PartType FROM {1} " +
        //                        //                    "LEFT OUTER JOIN {0} ON {1}.{3}={0}.{2} " +
        //                        //                    "WHERE {0}.{2} IS NULL AND {3}=@Code AND {1}.{6}=1 AND {0}.{7}=1 ";//Part表加Flag//ModelBOM表加Flag

        //                        //sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.ModelBOM).Name,
        //                        //                                                 typeof(_Schema.Part).Name,
        //                        //                                                 _Schema.ModelBOM.fn_Material,
        //                        //                                                 _Schema.Part.fn_PartNo,
        //                        //                                                 _Schema.Part.fn_PartType,
        //                        //                                                // _Schema.ModelBOM.fn_Material_group,
        //                        //                                                "",
        //                        //                                                 _Schema.Part.fn_Flag,//Part表加Flag
        //                        //                                                 _Schema.ModelBOM.fn_Flag//ModelBOM表加Flag
        //                        //                                                 );


        //                        sqlCtx.Sentence = @" select BomNodeType, PartType
        //                                                              from Part
        //                                                             where PartNo=@Code and
        //                                                                    Flag=1  ";                                                                                                       

                                

        //                        #region OLD
        //                        //sqlCtx.Sentence = "SELECT {0} FROM {1} WHERE {2}=@Code " + 
        //                        //                  "UNION " +
        //                        //                  "SELECT {3} FROM {4} WHERE {5} NOT IN (SELECT {2} FROM {1}) AND {5}=@Code ";

        //                        //sqlCtx.Sentence = string.Format(sqlCtx.Sentence, _Schema.ModelBOM.fn_Material_group,
        //                        //                                                typeof(_Schema.ModelBOM).Name,
        //                        //                                                _Schema.ModelBOM.fn_Material,
        //                        //                                                _Schema.Part.fn_PartType,
        //                        //                                                typeof(_Schema.Part).Name,
        //                        //                                                _Schema.Part.fn_PartNo);
        //                        #endregion

        //                        sqlCtx.Params.Add("Code", new SqlParameter("@Code", SqlDbType.VarChar));

        //                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
        //                    }
        //                }
        //                sqlCtx.Params["Code"].Value = code;
        //                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

        //                #region OLD
        //                //_Schema.SQLContext sqlCtx = null;
        //                //lock (MethodBase.GetCurrentMethod())
        //                //{
        //                //    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                //    {
        //                //        _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                //        cond.Material = code;
        //                //        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "DISTINCT", new List<string>() { _Schema.ModelBOM.fn_Material_group }, cond, null, null, null, null, null, null, null);
        //                //    }
        //                //}
        //                //sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = code;
        //                //ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //                #endregion

        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public DataTable GetAlternativeItemGroupBySpecial(string parent, string code)
        //        {
        //            //SELECT distinct Alternative_item_group from ModelBOM where Material=@parent and Component=@code
        //            try
        //            {
        //                DataTable ret = null;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                        cond.Material = parent;
        //                        cond.Component = code;
        //                        cond.Flag = 1;//ModelBOM表加Flag
        //                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "DISTINCT", new List<string>() { _Schema.ModelBOM.fn_Alternative_item_group }, cond, null, null, null, null, null, null, null);
        //                        sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag;//ModelBOM表加Flag
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = parent;
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = code;
        //                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public DataTable GetComponentByMaterial(string code)
        //        {
        //            //SELECT distinct Component from ModelBOM where Material=@code
        //            try
        //            {
        //                DataTable ret = null;

        //                _Schema.SQLContext sqlCtx = ComposeSubSQLForGetComponentByMaterial(code);
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = code;
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = 1;//ModelBOM表加Flag
        //                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //                return ret;
        //            }
        //            catch(Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public DataTable GetPartById(string code)
        //        {
        //            //SELECT [PartNo] FROM [Part] where [PartNo]='code'
        //            try
        //            {
        //                DataTable ret = null;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.Part cond = new _Schema.Part();
        //                        cond.PartNo = code;
        //                        cond.Flag = 1;//Part表加Flag
        //                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Part), null, new List<string>() { _Schema.Part.fn_PartNo }, cond, null, null, null, null, null, null, null);

        //                        sqlCtx.Params[_Schema.Part.fn_Flag].Value = cond.Flag;//Part表加Flag
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.Part.fn_PartNo].Value = code;
        //                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public DataTable GetPartNoByType(string type, string match)
        //        {
        //            //SELECT [PartNo] FROM [Part] where [PartType] ='type' and PartNo like '%" + match + "%' order by [PartNo] ";
        //            try
        //            {
        //                DataTable ret = null;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.Part cond = new _Schema.Part();
        //                        cond.PartType = type;
        //                        cond.Flag = 1;//Part表加Flag

        //                        _Schema.Part likeCond = new _Schema.Part();
        //                        likeCond.PartNo = "%" + match + "%";

        //                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Part), null, new List<string>() { _Schema.Part.fn_PartNo }, cond, likeCond, null, null, null, null, null, null);

        //                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Part.fn_PartNo);

        //                        sqlCtx.Params[_Schema.Part.fn_Flag].Value = cond.Flag;//Part表加Flag
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.Part.fn_PartType].Value = type;
        //                sqlCtx.Params[_Schema.Part.fn_PartNo].Value = "%" + match + "%";
        //                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        private _Schema.SQLContext ComposeSubSQLForGetComponentByMaterial(string code)
        //        {
        //            _Schema.SQLContext sqlCtx = null;
        //            lock (MethodBase.GetCurrentMethod())
        //            {
        //                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                {
        //                    _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                    cond.Material = code;
        //                    cond.Flag = 1;//ModelBOM表加Flag
        //                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "DISTINCT", new List<string>() { _Schema.ModelBOM.fn_Component }, cond, null, null, null, null, null, null, null);
        //                }
        //            }
        //            return sqlCtx;
        //        }

        //        public int getMaxGroupNo()
        //        {
        //            try
        //            {
        //                int ret = -1;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM), "MAX", new List<string>() { _Schema.MoBOM.fn_Group }, null, null, null, null, null, null, null, null);
        //                    }
        //                }
        //                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //                {
        //                    if (sqlR != null && sqlR.Read())
        //                    {
        //                        ret = GetValue_Int32(sqlR, sqlCtx.Indexes["MAX"]);
        //                    }
        //                }
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public void saveGroupNo(int mobomId, string mo, int group)
        //        {
        //            try
        //            {
        //                SqlTransactionManager.Begin();

        //                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(mo, CacheType.MOBOM));

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.MoBOM cond = new _Schema.MoBOM();
        //                        cond.ID = mobomId;
        //                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM), new List<string>() { _Schema.MoBOM.fn_Group }, null, null, null, cond, null, null, null, null, null, null, null);
        //                    }
        //                }
        //                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //                sqlCtx.Params[_Schema.MoBOM.fn_ID].Value = mobomId;
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.MoBOM.fn_Group)].Value = group;
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.MoBOM.fn_Udt)].Value = cmDt;
        //                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

        //                SqlTransactionManager.Commit();
        //            }
        //            catch (Exception)
        //            {
        //                SqlTransactionManager.Rollback();
        //                throw;
        //            }
        //            finally
        //            {
        //                SqlTransactionManager.Dispose();
        //                SqlTransactionManager.End();
        //            }
        //        }

        //        public DateTime getMaxUdt(string mo)
        //        {
        //            try
        //            {
        //                DateTime ret = DateTime.MinValue;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.MoBOM eqCond = new _Schema.MoBOM();
        //                        eqCond.MO = mo;
        //                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM), "MAX", new List<string>() { _Schema.MoBOM.fn_Udt }, eqCond, null, null, null, null, null, null, null);
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.MoBOM.fn_MO].Value = mo;
        //                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //                {
        //                    if (sqlR != null)
        //                    {
        //                        if (sqlR.Read())
        //                        {
        //                            ret = GetValue_DateTime(sqlR, sqlCtx.Indexes["MAX"]);
        //                        }
        //                    }
        //                }
        //                return ret;
        //            }
        //            catch(Exception)
        //            {
        //                throw;
        //            }
        //        }

              

        //        public int ExcludeAlternativeItemToNull(string parent, string code)
        //        {
        //            //UPDATE ModelBOM SET Alternative_item_group=NULL where Material='" + parent + "' and Component='" + code + "'
        //            try
        //            {
        //                int ret = 0;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                        cond.Material = parent;
        //                        cond.Component = code;
        //                        cond.Flag = 1;//ModelBOM表加Flag
        //                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), new List<string>() { _Schema.ModelBOM.fn_Alternative_item_group }, null, null, null, cond, null, null, null, null, null, null, null);
        //                        sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag;//ModelBOM表加Flag
        //                    }
        //                }
        //                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = parent;
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = code;
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Alternative_item_group)].Value = null;
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Udt)].Value = cmDt;
        //                ret = _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public DataTable GetPartCheckSettingList(string customer, string model)
        //        {
        //            //如果model=="", [Model]='model'条件变成[Model]='model' OR [Model] IS NULL两个条件
        //            //SELECT [Station],[PartType],[ID],[Customer],[Model]
        //            //  FROM [IMES_GetData_Datamaintain].[dbo].[PartCheckSetting]
        //            // where [Customer]='customer' AND [Model]='model'
        //            //order by [Station],[PartType]
        //            try
        //            {
        //                DataTable ret = null;

        //                _Schema.SQLContext sqlCtx = null;

        //                if (string.IsNullOrEmpty(model))
        //                    sqlCtx = ComposeForGetPartCheckSettingList(customer);
        //                else
        //                    sqlCtx = ComposeForGetPartCheckSettingList(customer, model);
        //                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //                ret = _Schema.Func.SortColumns(ret, new int[] { sqlCtx.Indexes[_Schema.PartCheckSetting.fn_WC],
        //                                                                sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Tp],
        //                                                                sqlCtx.Indexes[_Schema.PartCheckSetting.fn_ID],
        //                                                                sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Customer],
        //                                                                sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Model]});   
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        private _Schema.SQLContext ComposeForGetPartCheckSettingList(string customer, string model)
        //        {
        //            _Schema.SQLContext sqlCtx = null;
        //            lock (MethodBase.GetCurrentMethod())
        //            {
        //                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                {
        //                    _Schema.PartCheckSetting cond = new _Schema.PartCheckSetting();
        //                    cond.Customer = customer;
        //                    cond.Model = model;
        //                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), null, new List<string>() { _Schema.PartCheckSetting.fn_WC, _Schema.PartCheckSetting.fn_Tp, _Schema.PartCheckSetting.fn_ID, _Schema.PartCheckSetting.fn_Customer, _Schema.PartCheckSetting.fn_Model }, cond, null, null, null, null, null, null, null);
        //                    sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.PartCheckSetting.fn_WC, _Schema.PartCheckSetting.fn_Tp })); 
        //                }
        //            }
        //            sqlCtx.Params[_Schema.PartCheckSetting.fn_Customer].Value = customer;
        //            sqlCtx.Params[_Schema.PartCheckSetting.fn_Model].Value = model;
        //            return sqlCtx;
        //        }

        //        private _Schema.SQLContext ComposeForGetPartCheckSettingList(string customer)
        //        {
        //            _Schema.SQLContext sqlCtx = null;
        //            lock (MethodBase.GetCurrentMethod())
        //            {
        //                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                {
        //                    _Schema.PartCheckSetting cond = new _Schema.PartCheckSetting();
        //                    cond.Customer = customer;
        //                    _Schema.PartCheckSetting noecond = new _Schema.PartCheckSetting();
        //                    noecond.Model = string.Empty;
        //                    sqlCtx = _Schema.Func.GetConditionedFuncSelectExt(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), null, new List<string>() { _Schema.PartCheckSetting.fn_WC, _Schema.PartCheckSetting.fn_Tp, _Schema.PartCheckSetting.fn_ID, _Schema.PartCheckSetting.fn_Customer, _Schema.PartCheckSetting.fn_Model }, cond, null, null, null, null, null, null, null, noecond);
        //                    sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.PartCheckSetting.fn_WC, _Schema.PartCheckSetting.fn_Tp }));
        //                    sqlCtx.Params[_Schema.PartCheckSetting.fn_Model].Value = noecond.Model; 
        //                }
        //            }
        //            sqlCtx.Params[_Schema.PartCheckSetting.fn_Customer].Value = customer;
        //            return sqlCtx;
        //        }

        //        public void AddPartCheckSetting(PartCheckSetting partCheckSetting)
        //        {
        //            try
        //            {
        //                SqlTransactionManager.Begin();

        //                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(string.Empty, CacheType.PartCheckSetting));

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting));
        //                    }
        //                }
        //                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //                sqlCtx.Params[_Schema.PartCheckSetting.fn_Cdt].Value = cmDt;
        //                sqlCtx.Params[_Schema.PartCheckSetting.fn_Customer].Value = partCheckSetting.Customer;
        //                sqlCtx.Params[_Schema.PartCheckSetting.fn_Editor].Value = partCheckSetting.Editor;
        //                //sqlCtx.Params[_Schema.PartCheckSetting.fn_ID].Value = partCheckSetting.ID;
        //                sqlCtx.Params[_Schema.PartCheckSetting.fn_Model].Value = partCheckSetting.Model;
        //                sqlCtx.Params[_Schema.PartCheckSetting.fn_Tp].Value = partCheckSetting.Tp;
        //                sqlCtx.Params[_Schema.PartCheckSetting.fn_Udt].Value = cmDt;
        //                sqlCtx.Params[_Schema.PartCheckSetting.fn_WC].Value = partCheckSetting.WC;
        //                sqlCtx.Params[_Schema.PartCheckSetting.fn_ValueType].Value = partCheckSetting.ValueType; 
        //                partCheckSetting.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));

        //                SqlTransactionManager.Commit();
        //            }
        //            catch (Exception)
        //            {
        //                SqlTransactionManager.Rollback();
        //                throw;
        //            }
        //            finally
        //            {
        //                SqlTransactionManager.Dispose();
        //                SqlTransactionManager.End();
        //            }
        //        }

        //        public void SavePartCheckSetting(PartCheckSetting partCheckSetting)
        //        {
        //            try
        //            {
        //                SqlTransactionManager.Begin();

        //                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(string.Empty, CacheType.PartCheckSetting));

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting));
        //                    }
        //                }
        //                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //                sqlCtx.Params[_Schema.PartCheckSetting.fn_Customer].Value = partCheckSetting.Customer;
        //                sqlCtx.Params[_Schema.PartCheckSetting.fn_Editor].Value = partCheckSetting.Editor;
        //                sqlCtx.Params[_Schema.PartCheckSetting.fn_ID].Value = partCheckSetting.ID;
        //                sqlCtx.Params[_Schema.PartCheckSetting.fn_Model].Value = partCheckSetting.Model;
        //                sqlCtx.Params[_Schema.PartCheckSetting.fn_Tp].Value = partCheckSetting.Tp;
        //                sqlCtx.Params[_Schema.PartCheckSetting.fn_Udt].Value = cmDt;
        //                sqlCtx.Params[_Schema.PartCheckSetting.fn_WC].Value = partCheckSetting.WC;
        //                sqlCtx.Params[_Schema.PartCheckSetting.fn_ValueType].Value = partCheckSetting.ValueType; 
        //                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

        //                SqlTransactionManager.Commit();
        //            }
        //            catch (Exception)
        //            {
        //                SqlTransactionManager.Rollback();
        //                throw;
        //            }
        //            finally
        //            {
        //                SqlTransactionManager.Dispose();
        //                SqlTransactionManager.End();
        //            }
        //        }

        //        public void DeletePartCheckSetting(PartCheckSetting partCheckSetting)
        //        {
        //            try
        //            {
        //                SqlTransactionManager.Begin();

        //                DataChangeMediator.AddChangeDemand(this.GetACacheSignal(string.Empty, CacheType.PartCheckSetting));

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting));
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.PartCheckSetting.fn_ID].Value = partCheckSetting.ID;
        //                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

        //                SqlTransactionManager.Commit();
        //            }
        //            catch (Exception)
        //            {
        //                SqlTransactionManager.Rollback();
        //                throw;
        //            }
        //            finally
        //            {
        //                SqlTransactionManager.Dispose();
        //                SqlTransactionManager.End();
        //            }
        //        }

        //        public DataTable GetExistPartCheckSetting(string customer, string model, string station, string partType, string valueType, int id)
        //        {
        //            //判断Station、Part Type的值对已经被Part Check List中其他Part Type使用
        //            //如果model=="", [Model]='model'条件变为[Model]='model' OR  [Model] IS NULL两个条件
        //            //如果station=="", [Station]='station' 条件变为[Station]='station'  OR  [Station] IS NULL两个条件
        //            //如果partType=="", [PartType]='partType'条件变为[PartType]='partType' OR  [PartType] IS NULL两个条件
        //            //如果valueType=="", [ValueType]='valueType'条件变为[ValueType]='valueType' OR  [ValueType] IS NULL两个条件
        //            //SELECT [ID] FROM [IMES_GetData].[dbo].[PartCheckSetting]
        //            //WHERE [Customer]='customer' AND [Model]='model'
        //            //AND [Station]='station' AND [PartType]='partType' AND ValueType='valueType'
        //            //AND ID<>'id'
        //            try
        //            {
        //                DataTable ret = null;

        //                _Schema.SQLContextCollection sqlSet = new _Schema.SQLContextCollection();
        //                int i = 0;
        //                sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_customer(customer));
        //                sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_id(id));
        //                if (string.IsNullOrEmpty(model))
        //                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_model());
        //                else
        //                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_model(model));
        //                if (string.IsNullOrEmpty(station))
        //                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_station());
        //                else
        //                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_station(station));
        //                if (string.IsNullOrEmpty(partType))
        //                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_partType());
        //                else
        //                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_partType(partType));
        //                if (string.IsNullOrEmpty(valueType))
        //                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_valueType());
        //                else
        //                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_valueType(valueType));

        //                _Schema.SQLContext sqlCtx = sqlSet.MergeToOneAndQuery();
        //                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                 throw;
        //            }
        //        }

        //        public DataTable GetExistPartCheckSetting(string customer, string model, string station, string partType, string valueType)
        //        {
        //            //判断Station、Part Type的值对已经被Part Check List中其他Part Type使用
        //            //如果model=="", [Model]='model'条件变为[Model]='model' OR  [Model] IS NULL两个条件
        //            //如果station=="", [Station]='station' 条件变为[Station]='station'  OR  [Station] IS NULL两个条件
        //            //如果partType=="", [PartType]='partType'条件变为[PartType]='partType' OR  [PartType] IS NULL两个条件
        //            //如果valueType=="", [ValueType]='valueType'条件变为[ValueType]='valueType' OR  [ValueType] IS NULL两个条件
        //            //SELECT [ID] FROM [IMES_GetData].[dbo].[PartCheckSetting]
        //            //WHERE [Customer]='customer' AND [Model]='model'
        //            //AND [Station]='station' AND [PartType]='partType' AND ValueType='valueType'
        //            try
        //            {
        //                DataTable ret = null;

        //                _Schema.SQLContextCollection sqlSet = new _Schema.SQLContextCollection();
        //                int i = 0;
        //                sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_customer(customer));
        //                if (string.IsNullOrEmpty(model))
        //                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_model());
        //                else
        //                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_model(model));
        //                if (string.IsNullOrEmpty(station))
        //                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_station());
        //                else
        //                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_station(station));
        //                if (string.IsNullOrEmpty(partType))
        //                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_partType());
        //                else
        //                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_partType(partType));
        //                if (string.IsNullOrEmpty(valueType))
        //                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_valueType());
        //                else
        //                    sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_valueType(valueType));

        //                _Schema.SQLContext sqlCtx = sqlSet.MergeToOneAndQuery(); 
        //                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        private _Schema.SQLContext ComposeForGetExistPartCheckSetting_customer(string customer)
        //        {
        //            _Schema.SQLContext sqlCtx = null;
        //            lock (MethodBase.GetCurrentMethod())
        //            {
        //                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                {
        //                    _Schema.PartCheckSetting cond = new _Schema.PartCheckSetting();
        //                    cond.Customer = customer;
        //                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), null, new List<string>() { _Schema.PartCheckSetting.fn_ID }, cond, null, null, null, null, null, null, null);
        //                }
        //            }
        //            sqlCtx.Params[_Schema.PartCheckSetting.fn_Customer].Value = customer;
        //            return sqlCtx;
        //        }

        //        private _Schema.SQLContext ComposeForGetExistPartCheckSetting_id(int id)
        //        {
        //            _Schema.SQLContext sqlCtx = null;
        //            lock (MethodBase.GetCurrentMethod())
        //            {
        //                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                {
        //                    _Schema.PartCheckSetting ncond = new _Schema.PartCheckSetting();
        //                    ncond.ID = id;
        //                    sqlCtx = _Schema.Func.GetConditionedFuncSelectWith3NotConds(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), null, new List<string>() { _Schema.PartCheckSetting.fn_ID }, null, null, null, null, null, null, null, null, ncond, null, null);
        //                }
        //            }
        //            sqlCtx.Params[_Schema.PartCheckSetting.fn_ID].Value = id;
        //            return sqlCtx;
        //        }

        //        private _Schema.SQLContext ComposeForGetExistPartCheckSetting_model(string model)
        //        {
        //            _Schema.SQLContext sqlCtx = null;
        //            lock (MethodBase.GetCurrentMethod())
        //            {
        //                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                {
        //                    _Schema.PartCheckSetting cond = new _Schema.PartCheckSetting();
        //                    cond.Model = model;
        //                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), null, new List<string>() { _Schema.PartCheckSetting.fn_ID }, cond, null, null, null, null, null, null, null);
        //                }
        //            }
        //            sqlCtx.Params[_Schema.PartCheckSetting.fn_Model].Value = model;
        //            return sqlCtx;
        //        }

        //        private _Schema.SQLContext ComposeForGetExistPartCheckSetting_model()
        //        {
        //            _Schema.SQLContext sqlCtx = null;
        //            lock (MethodBase.GetCurrentMethod())
        //            {
        //                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                {
        //                    _Schema.PartCheckSetting noecond = new _Schema.PartCheckSetting();
        //                    noecond.Model = string.Empty;
        //                    sqlCtx = _Schema.Func.GetConditionedFuncSelectExt(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), null, new List<string>() { _Schema.PartCheckSetting.fn_ID }, null, null, null, null, null, null, null, null, noecond);
        //                }
        //            }
        //            sqlCtx.Params[_Schema.PartCheckSetting.fn_Model].Value = string.Empty;
        //            return sqlCtx;
        //        }

        //        private _Schema.SQLContext ComposeForGetExistPartCheckSetting_station(string station)
        //        {
        //            _Schema.SQLContext sqlCtx = null;
        //            lock (MethodBase.GetCurrentMethod())
        //            {
        //                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                {
        //                    _Schema.PartCheckSetting cond = new _Schema.PartCheckSetting();
        //                    cond.WC = station;
        //                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), null, new List<string>() { _Schema.PartCheckSetting.fn_ID }, cond, null, null, null, null, null, null, null);
        //                }
        //            }
        //            sqlCtx.Params[_Schema.PartCheckSetting.fn_WC].Value = station;
        //            return sqlCtx;
        //        }

        //        private _Schema.SQLContext ComposeForGetExistPartCheckSetting_station()
        //        {
        //            _Schema.SQLContext sqlCtx = null;
        //            lock (MethodBase.GetCurrentMethod())
        //            {
        //                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                {
        //                    _Schema.PartCheckSetting noecond = new _Schema.PartCheckSetting();
        //                    noecond.WC = string.Empty;
        //                    sqlCtx = _Schema.Func.GetConditionedFuncSelectExt(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), null, new List<string>() { _Schema.PartCheckSetting.fn_ID }, null, null, null, null, null, null, null, null, noecond);
        //                }
        //            }
        //            sqlCtx.Params[_Schema.PartCheckSetting.fn_WC].Value = string.Empty;
        //            return sqlCtx;
        //        }

        //        private _Schema.SQLContext ComposeForGetExistPartCheckSetting_partType(string partType)
        //        {
        //            _Schema.SQLContext sqlCtx = null;
        //            lock (MethodBase.GetCurrentMethod())
        //            {
        //                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                {
        //                    _Schema.PartCheckSetting cond = new _Schema.PartCheckSetting();
        //                    cond.Tp = partType;
        //                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), null, new List<string>() { _Schema.PartCheckSetting.fn_ID }, cond, null, null, null, null, null, null, null);
        //                }
        //            }
        //            sqlCtx.Params[_Schema.PartCheckSetting.fn_Tp].Value = partType;
        //            return sqlCtx;
        //        }

        //        private _Schema.SQLContext ComposeForGetExistPartCheckSetting_partType()
        //        {
        //            _Schema.SQLContext sqlCtx = null;
        //            lock (MethodBase.GetCurrentMethod())
        //            {
        //                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                {
        //                    _Schema.PartCheckSetting noecond = new _Schema.PartCheckSetting();
        //                    noecond.Tp = string.Empty;
        //                    sqlCtx = _Schema.Func.GetConditionedFuncSelectExt(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), null, new List<string>() { _Schema.PartCheckSetting.fn_ID }, null, null, null, null, null, null, null, null, noecond);
        //                }
        //            }
        //            sqlCtx.Params[_Schema.PartCheckSetting.fn_Tp].Value = string.Empty;
        //            return sqlCtx;
        //        }

        //        private _Schema.SQLContext ComposeForGetExistPartCheckSetting_valueType(string valueType)
        //        {
        //            _Schema.SQLContext sqlCtx = null;
        //            lock (MethodBase.GetCurrentMethod())
        //            {
        //                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                {
        //                    _Schema.PartCheckSetting cond = new _Schema.PartCheckSetting();
        //                    cond.ValueType = valueType;
        //                    sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), null, new List<string>() { _Schema.PartCheckSetting.fn_ID }, cond, null, null, null, null, null, null, null);
        //                }
        //            }
        //            sqlCtx.Params[_Schema.PartCheckSetting.fn_ValueType].Value = valueType;
        //            return sqlCtx;
        //        }

        //        private _Schema.SQLContext ComposeForGetExistPartCheckSetting_valueType()
        //        {
        //            _Schema.SQLContext sqlCtx = null;
        //            lock (MethodBase.GetCurrentMethod())
        //            {
        //                if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                {
        //                    _Schema.PartCheckSetting noecond = new _Schema.PartCheckSetting();
        //                    noecond.ValueType = string.Empty;
        //                    sqlCtx = _Schema.Func.GetConditionedFuncSelectExt(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), null, new List<string>() { _Schema.PartCheckSetting.fn_ID }, null, null, null, null, null, null, null, null, noecond);
        //                }
        //            }
        //            sqlCtx.Params[_Schema.PartCheckSetting.fn_ValueType].Value = string.Empty;
        //            return sqlCtx;
        //        }

        //        public PartCheckSetting FindPartCheckSettingById(int id)
        //        {
        //            try
        //            {
        //                PartCheckSetting ret = null;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.PartCheckSetting cond = new _Schema.PartCheckSetting();
        //                        cond.ID = id;
        //                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), cond, null, null);
        //                    }
        //                }

        //                sqlCtx.Params[_Schema.PartCheckSetting.fn_ID].Value = id;

        //                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //                {
        //                    if (sqlR != null)
        //                    {
        //                        if (sqlR.Read())
        //                        {
        //                            ret = new PartCheckSetting();
        //                            ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Cdt]);
        //                            ret.Customer = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Customer]);
        //                            ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Editor]);
        //                            ret.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_ID]);
        //                            ret.Model = GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Model]);
        //                            ret.Tp = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Tp]);
        //                            ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Udt]);
        //                            ret.ValueType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_ValueType]);
        //                            ret.WC = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_WC]);
        //                        }
        //                    }
        //                }
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

                

        //        public DataTable GetParentInfo(string current)
        //        {
        //            //SELECT distinct a.Material, b.Descr, c.Flag, c.ApproveDate FROM ModelBOM  AS a 
        //            //left outer join 
        //            //(SELECT Descr, PartNo
        //            //  FROM [IMES_GetData_Datamaintain].[dbo].[Part]
        //            //WHERE Flag=1) AS b
        //            //ON a.Material=b.PartNo
        //            //left outer join 
        //            //(Select 'Model' AS Flag, BOMApproveDate AS ApproveDate, Model from Model) AS c
        //            //on a.Material=c.Model 
        //            //where a.Component='current' and a.Flag=1
        //            try
        //            {
        //                DataTable ret = null;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        sqlCtx = new _Schema.SQLContext();

        //                        string strApproveDate = "ApproveDate";

        //                        //sqlCtx.Sentence =   "SELECT DISTINCT a.{3}, b.{5}, c.{4}, c.{11} FROM {0} AS a " + 
        //                        //                    "LEFT OUTER JOIN " + 
        //                        //                    "(SELECT {5}, {6} FROM {1} WHERE {7}=1) AS b " + 
        //                        //                    "ON a.{3}=b.{6} " +
        //                        //                    "LEFT OUTER JOIN " +
        //                        //                    "(SELECT '{8}' AS {4}, {10} AS {11}, {8} FROM {2}) AS c " +
        //                        //                    "ON a.{3}=c.{8} " +
        //                        //                    "WHERE a.{9}=@{9} AND a.{4}=1 ";

        //                        sqlCtx.Sentence = @"SELECT DISTINCT a.Material, b.Descr, 
        //                                                                     case when c.Model IS NULL then null else 'Model' end   as Flag ,
        //                                                                     c.BOMApproveDate  as {1}  
        //                                                              FROM ModelBOM AS a 
        //                                                              LEFT OUTER JOIN  Part b ON a.Material=b.PartNo and b.Flag=1 
        //                                                              LEFT OUTER JOIN  Model c --(SELECT 'Model' AS Flag, BOMApproveDate AS ApproveDate, Model FROM Model) AS c 
        //                                                              ON a.Material=c.Model 
        //                                                              WHERE a.Component=@{0} AND a.Flag=1";

        //                        //sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.ModelBOM).Name,
        //                        //                                                typeof(_Schema.Part).Name,
        //                        //                                                typeof(_Schema.Model).Name,
        //                        //                                                _Schema.ModelBOM.fn_Material,
        //                        //                                                _Schema.ModelBOM.fn_Flag,
        //                        //                                                _Schema.Part.fn_Descr,
        //                        //                                                _Schema.Part.fn_PartNo,
        //                        //                                                _Schema.Part.fn_Flag,
        //                        //                                                _Schema.Model.fn_model,
        //                        //                                                _Schema.ModelBOM.fn_Component,
        //                        //                                                _Schema.Model.fn_BOMApproveDate,
        //                        //                                                strApproveDate);
        //                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence,
        //                                                                        _Schema.ModelBOM.fn_Component,                                                                      
        //                                                                        strApproveDate);

        //                        sqlCtx.Params.Add(_Schema.ModelBOM.fn_Component, new SqlParameter("@" + _Schema.ModelBOM.fn_Component, SqlDbType.VarChar));

        //                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = current;
        //                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

               

                

                

        //        private void SetNewAlternativeGroupBatch_Inner(IList<int> ids, string editor, Guid newGuid)
        //        {
        //            try
        //            {
        //                if (ids != null && ids.Count > 0)
        //                {
        //                    #region
        //                    //_Schema.SQLContextCollection sqlCtxSet = new _Schema.SQLContextCollection();
        //                    //int i = 0;
        //                    //foreach (int entry in ids)
        //                    //{
        //                    //    _Schema.SQLContext sqlCtx = ComposeForSetNewAlternativeGroup(entry);
        //                    //    sqlCtxSet.AddOne(i, sqlCtx);
        //                    //    i++;
        //                    //}
        //                    //_Schema.SQLContext sqlCtxBatch = sqlCtxSet.MergeToOneNonQuery();
        //                    //_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtxBatch.Sentence, sqlCtxBatch.Params.Values.ToArray<SqlParameter>());
        //                    #endregion

        //                    _Schema.SQLContext sqlCtx = null;
        //                    sqlCtx = ComposeForSetNewAlternativeGroup(ids, editor, newGuid);
        //                    _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        private _Schema.SQLContext ComposeForSetNewAlternativeGroup(IList<int> ids, string editor, Guid newGuid)
        //        {
        //            try
        //            {
        //                _Schema.SQLContext ret = null;
        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.ModelBOM insetCond = new _Schema.ModelBOM();
        //                        insetCond.ID = 1;//"INSET";
        //                        _Schema.ModelBOM eqCond = new _Schema.ModelBOM();
        //                        eqCond.Flag = 1;
        //                        sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), new List<string>() { _Schema.ModelBOM.fn_Alternative_item_group, _Schema.ModelBOM.fn_Editor }, null, null, null, eqCond, null, null, null, null, null, null, insetCond);

        //                        sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = eqCond.Flag;
        //                        //sqlCtx.Sentence = sqlCtx.Sentence.Replace(sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Alternative_item_group)].ParameterName, "LOWER(CONVERT(VARCHAR(255),NEWID()))");
        //                        //sqlCtx.Params.Remove(_Schema.Func.DecSV(_Schema.ModelBOM.fn_Alternative_item_group));
        //                    }
        //                }
        //                ret = new _Schema.SQLContext(sqlCtx);
        //                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //                ret.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Editor)].Value = editor;
        //                ret.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Alternative_item_group)].Value = newGuid.ToString();
        //                ret.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Udt)].Value = cmDt;
        //                ret.Sentence = ret.Sentence.Replace(_Schema.Func.DecInSet(_Schema.ModelBOM.fn_ID), _Schema.Func.ConvertInSet(ids));
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

                
        //        public DataTable GetExistRefreshModelByModel(string model, string editor)
        //        {
        //            //SELECT Model FROM RefreshModel where Model='model'AND Editor='editor'
        //            try
        //            {
        //                DataTable ret = null;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.RefreshModel cond = new _Schema.RefreshModel();
        //                        cond.model = model;
        //                        cond.Editor = editor;
        //                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.RefreshModel), null, new List<string>() { _Schema.RefreshModel.fn_model }, cond, null, null, null, null, null, null, null);
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.RefreshModel.fn_model].Value = model;
        //                sqlCtx.Params[_Schema.RefreshModel.fn_Editor].Value = editor;
        //                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

               

        //        public DataTable GetRefreshModelList(string editor)
        //        {
        //            //SELECT Model FROM RefreshModel WHERE Editor='editor'
        //            try
        //            {
        //                DataTable ret = null;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.RefreshModel cond = new _Schema.RefreshModel();
        //                        cond.Editor = editor;
        //                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.RefreshModel), null, new List<string>() { _Schema.RefreshModel.fn_model }, cond, null, null, null, null, null, null, null);
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.RefreshModel.fn_Editor].Value = editor;
        //                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        //public ModelBOM FindModelBOMByID(int id)
        //        //{
        //        //    try
        //        //    {
        //        //        ModelBOM ret = null;

        //        //        ret = this.GetModelBOM(id);

        //        //        return ret;
        //        //    }
        //        //    catch (Exception)
        //        //    {
        //        //        throw;
        //        //    }
        //        //}

               

                

        //        public DataTable GetSubModelBOMByCode(IList<int> idList)
        //        {
        //            try
        //            {
        //                DataTable ret = null;

        //                if (idList != null && idList.Count > 0)
        //                {
        //                    IList<int> batch = new List<int>();
        //                    int i = 0;
        //                    foreach (int it in idList)
        //                    {
        //                        batch.Add(it);
        //                        if ((i + 1) % batchSQLCnt == 0 || i == idList.Count - 1)
        //                        {
        //                            DataTable dt = GetSubModelBOMByCode_Inner(batch);
        //                            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
        //                            {
        //                                if (ret == null)
        //                                {
        //                                    ret = dt;
        //                                }
        //                                else
        //                                {
        //                                    foreach (DataRow dr in dt.Rows)
        //                                    {
        //                                        ret.Rows.Add(dr.ItemArray);
        //                                    }
        //                                }
        //                            }
        //                            batch.Clear();
        //                        }
        //                        i++;
        //                    }
        //                }
        //                //2010-04-22 Liu Dong(eB1-4)         Modify ITC-1136-0133
        //                if (ret != null && ret.Rows != null && ret.Rows.Count > 0)
        //                {
        //                    DataView dv = ret.DefaultView;
        //                    dv.Sort = string.Format("{0},{1}", _Schema.ModelBOM.fn_Alternative_item_group, _Schema.ModelBOM.fn_Priority);
        //                    ret = dv.ToTable();
        //                }
        //                //2010-04-22 Liu Dong(eB1-4)         Modify ITC-1136-0133
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        private DataTable GetSubModelBOMByCode_Inner(IList<int> idList)
        //        {
        //            try
        //            {
        //                DataTable ret = null;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        sqlCtx = new _Schema.SQLContext();

        //                        //string strCode = "Code";
        //                        //string strDataFromType = "dataFromType";

        //                        //sqlCtx.Sentence = "SELECT a.{3} AS {11}, CASE WHEN b.{17} IS NULL THEN b.{14} ELSE c.{14} END AS {14}, a.{4}," +
        //                        //                    "a.{5}, a.{6} " +
        //                        //                    ",a.{7}, a.{8}, a.{9}, a.{2} FROM {0} AS a " +
        //                        //                    "LEFT OUTER JOIN " +
        //                        //                    "(" +
        //                        //                      "SELECT DISTINCT {11} AS {16}, {12} AS {14}, 0 AS {17} FROM {0} WHERE {10}=1" +
        //                        //                    ") AS b ON b.{16}=a.{3} " +
        //                        //                    "LEFT OUTER JOIN " +
        //                        //                    "(" +
        //                        //                      "SELECT {13} AS {16}, {14} AS {14} " +
        //                        //                      "FROM {1} WHERE {15}=1" +
        //                        //                    ") AS c ON c.{16}=a.{3} " +
        //                        //                    "WHERE a.{10}=1 AND a.{2} IN (INSET[{2}]) " +
        //                        //                    "ORDER BY a.{5}, a.{6}";

                

        //                        //sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.ModelBOM).Name,
        //                        //                                                typeof(_Schema.Part).Name, 
        //                        //                                                _Schema.ModelBOM.fn_ID, 
        //                        //                                                _Schema.ModelBOM.fn_Component,
        //                        //                                                _Schema.ModelBOM.fn_Quantity,
        //                        //                                                _Schema.ModelBOM.fn_Alternative_item_group,
        //                        //                                                _Schema.ModelBOM.fn_Priority,
        //                        //                                                _Schema.ModelBOM.fn_Editor,
        //                        //                                                _Schema.ModelBOM.fn_Cdt,
        //                        //                                                _Schema.ModelBOM.fn_Udt,
        //                        //                                                _Schema.ModelBOM.fn_Flag,
        //                        //                                                _Schema.ModelBOM.fn_Material,
        //                        //                                                _Schema.ModelBOM.fn_Material_group,
        //                        //                                                _Schema.Part.fn_PartNo,
        //                        //                                                _Schema.Part.fn_PartType,
        //                        //                                                _Schema.Part.fn_Flag,
        //                        //                                                strCode,
        //                        //                                                strDataFromType);

        //                        sqlCtx.Sentence = @"SELECT a.Component AS Material, 
        //                                                           c.BomNodeType AS PartType, 
        //                                                           a.Quantity,
        //                                                           a.Alternative_item_group, 
        //                                                           a.Priority ,
        //                                                           a.Editor, 
        //                                                           a.Cdt, 
        //                                                           a.Udt, 
        //                                                           a.ID 
        //                                                     FROM ModelBOM AS a 
        //                                                     LEFT OUTER JOIN  Part as c 
        //                                                         ON c.PartNo=a.Component and
        //                                                            c.Flag=1  
        //                                                     WHERE a.Flag=1 AND a.ID IN ({0}) 
        //                                                     ORDER BY a.Alternative_item_group, a.Priority";
                                

                               

        //                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
        //                    }
        //                }
        //                //string Sentence = sqlCtx.Sentence.Replace(_Schema.Func.DecInSet(_Schema.ModelBOM.fn_ID), _Schema.Func.ConvertInSet(idList));
        //                string inSQLStr = string.Join(",", idList.Select(i => i.ToString()).ToArray());
        //                string Sentence = string.Format(sqlCtx.Sentence, inSQLStr);
        //                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, Sentence, null);
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

                

        //        private DataTable GetParantInfoe_Inner(IList<string> currentComponent)
        //        {
        //            try
        //            {
        //                DataTable ret = null;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        sqlCtx = new _Schema.SQLContext();

        //                        //string strCode = "Code";
        //                        //string strDataFromType = "dataFromType";

        //                        sqlCtx.Sentence =   "SELECT DISTINCT a.{3}, b.{6}, c.{4}, c.ApproveDate FROM {0} AS a " +
        //                                            "LEFT OUTER JOIN " +
        //                                            "(SELECT {6}, {7} FROM {1} WHERE {8}=1) AS b " +
        //                                            "ON a.{3}=b.{7} " +
        //                                            "LEFT OUTER JOIN " +
        //                                            "(SELECT 'Model' AS {4}, {9} AS ApproveDate, {10} FROM {2}) AS c " +
        //                                            "ON a.{3}=c.{10} " +
        //                                            "WHERE a.{5} IN (INSET[{5}]) AND a.{4}=1 ";

        //                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.ModelBOM).Name,
        //                                                                         typeof(_Schema.Part).Name,
        //                                                                         typeof(_Schema.Model).Name,
        //                                                                         _Schema.ModelBOM.fn_Material,
        //                                                                         _Schema.ModelBOM.fn_Flag,
        //                                                                         _Schema.ModelBOM.fn_Component,
        //                                                                         _Schema.Part.fn_Descr,
        //                                                                         _Schema.Part.fn_PartNo,
        //                                                                         _Schema.Part.fn_Flag,
        //                                                                         _Schema.Model.fn_BOMApproveDate,
        //                                                                         _Schema.Model.fn_model);

        //                        _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
        //                    }
        //                }
        //                string Sentence = sqlCtx.Sentence.Replace(_Schema.Func.DecInSet(_Schema.ModelBOM.fn_Component), _Schema.Func.ConvertInSet(currentComponent));
        //                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, Sentence, null);
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public DataTable GetComponentByMaterial(IList<string> codeList)
        //        {
        //            try
        //            {
        //                DataTable ret = null;

        //                if (codeList != null && codeList.Count > 0)
        //                {
        //                    IList<string> batch = new List<string>();
        //                    int i = 0;
        //                    foreach (string it in codeList)
        //                    {
        //                        batch.Add(it);
        //                        if ((i + 1) % batchSQLCnt == 0 || i == codeList.Count - 1)
        //                        {
        //                            DataTable dt = GetComponentByMaterial_Inner(batch);
        //                            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
        //                            {
        //                                if (ret == null)
        //                                {
        //                                    ret = dt;
        //                                }
        //                                else
        //                                {
        //                                    foreach (DataRow dr in dt.Rows)
        //                                    {
        //                                        ret.Rows.Add(dr.ItemArray);
        //                                    }
        //                                }
        //                            }
        //                            batch.Clear();
        //                        }
        //                        i++;
        //                    }
        //                }
        //                if (ret != null && ret.Rows != null && ret.Rows.Count > 0)
        //                {
        //                    DataView dv = ret.DefaultView;
        //                    ret = dv.ToTable(true);
        //                }
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        private DataTable GetComponentByMaterial_Inner(IList<string> codeList)
        //        {
        //            //SELECT distinct Component from ModelBOM where Material in 'codeList' And Flag=1;
        //            try
        //            {
        //                DataTable ret = null;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                        cond.Flag = 1;
        //                        _Schema.ModelBOM insetCond = new _Schema.ModelBOM();
        //                        insetCond.Material = "INSET";
        //                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "DISTINCT", null, cond, null, null, null, null, null, null, insetCond);

        //                        sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag;
        //                    }
        //                }
        //                string Sentence = sqlCtx.Sentence.Replace(_Schema.Func.DecInSet(_Schema.ModelBOM.fn_Material), _Schema.Func.ConvertInSet(codeList));
        //                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

               

        //        public IList<PartCheckSetting> GetPartCheckSettingList()
        //        {
        //            //SELECT [Station],[Customer],[Model],[PartType],[ValueType],[Editor],[Cdt],[Udt],[ID]
        //            //  FROM [PartCheckSetting]
        //            //order by [Station],[Customer],[Model],[PartType],[ValueType]
        //            try
        //            {
        //                IList<PartCheckSetting> ret = new List<PartCheckSetting>();

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting));
        //                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.PartCheckSetting.fn_WC, _Schema.PartCheckSetting.fn_Customer, _Schema.PartCheckSetting.fn_Model, _Schema.PartCheckSetting.fn_Tp, _Schema.PartCheckSetting.fn_ValueType}));
        //                    }
        //                }
        //                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
        //                {
        //                    if (sqlR != null)
        //                    {
        //                        while (sqlR.Read())
        //                        {
        //                            PartCheckSetting item = new PartCheckSetting();
        //                            item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Cdt]);
        //                            item.Customer = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Customer]);
        //                            item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Editor]);
        //                            item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_ID]);
        //                            item.Model = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Model]);
        //                            item.Tp = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Tp]);
        //                            item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Udt]);
        //                            item.ValueType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_ValueType]);
        //                            item.WC = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_WC]);
        //                            ret.Add(item);
        //                        }
        //                    }
        //                }
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        public DataTable GetValueTypeListByCustomerAndPartType(string customer, string partType)
        //        {

        //            //SELECT distinct [ValueType]   
        //            //  FROM [IMES_GetData_Datamaintain].[dbo].[PartCheck]
        //            //  where Customer='customer' AND PartType='partType' 
        //            //order by [ValueType]
        //            try
        //            {
        //                DataTable ret = null;

        //                _Schema.SQLContext sqlCtx = null;
        //                lock (MethodBase.GetCurrentMethod())
        //                {
        //                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //                    {
        //                        _Schema.PartCheck eqcond = new _Schema.PartCheck();
        //                        eqcond.Customer = customer;
        //                        eqcond.PartType = partType;
        //                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheck), "DISTINCT", new List<string>() { _Schema.PartCheck.fn_ValueType }, eqcond, null, null, null, null, null, null, null);
        //                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.PartCheck.fn_ValueType);
        //                    }
        //                }
        //                sqlCtx.Params[_Schema.PartCheck.fn_Customer].Value = customer;
        //                sqlCtx.Params[_Schema.PartCheck.fn_PartType].Value = partType;
        //                ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //                return ret;
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }

        //        #region Defered

        //        public void AddMOBOMDefered(IUnitOfWork uow, MOBOM item)
        //        {
        //            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        //        }

        //        public void UpdateMOBOMDefered(IUnitOfWork uow, MOBOM item, string oldMo)
        //        {
        //            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item, oldMo);
        //        }

        //        public void DeleteMOBOMDefered(IUnitOfWork uow, MOBOM item)
        //        {
        //            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        //        }

        //        public void CopyMOBOMDefered(IUnitOfWork uow, string mo)
        //        {
        //            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), mo);
        //        }

        //        public void UpdateMOBOMForDeleteActionDefered(IUnitOfWork uow, string mo, string partNo, bool deviation)
        //        {
        //            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), mo, partNo, deviation);
        //        }

                

        //        //public void ChangeModelBOMDefered(IUnitOfWork uow, ModelBOM item, int oldId)
        //        //{
        //        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod().Name, item, oldId);
        //        //}

        //        public void IncludeItemToAlternativeItemGroupDefered(IUnitOfWork uow, string value, string parent, string code)
        //        {
        //            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), value, parent, code);
        //        }

             

        //        public void DeleteModelBOMByCodeDefered(IUnitOfWork uow, string parentCode, string code)
        //        {
        //            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), parentCode, code);
        //        }

        //        public void IncludeAllItemToAlternativeItemGroupDefered(IUnitOfWork uow, string parent, string code1, string code2)
        //        {
        //            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), parent, code1, code2);
        //        }

        //        public void ExcludeAlternativeItemDefered(IUnitOfWork uow, string parent, string code)
        //        {
        //            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), parent, code);
        //        }

        //        public void DeleteSubModelByCodeDefered(IUnitOfWork uow, string code)
        //        {
        //            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), code);
        //        }

        //        public void saveGroupNoDefered(IUnitOfWork uow, int mobomId, string mo, int group)
        //        {
        //            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), mobomId, mo, group);
        //        }

        //        public void DeleteMOBOMByMoDefered(IUnitOfWork uow, string mo)
        //        {
        //            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), mo);
        //        }

        //        public void ExcludeAlternativeItemToNullDefered(IUnitOfWork uow, string parent, string code)
        //        {
        //            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), parent, code);
        //        }

        //        public void AddPartCheckSettingDefered(IUnitOfWork uow, PartCheckSetting partCheckSetting)
        //        {
        //            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), partCheckSetting);
        //        }

        //        public void SavePartCheckSettingDefered(IUnitOfWork uow, PartCheckSetting partCheckSetting)
        //        {
        //            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), partCheckSetting);
        //        }

        //        public void DeletePartCheckSettingDefered(IUnitOfWork uow, PartCheckSetting partCheckSetting)
        //        {
        //            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), partCheckSetting);
        //        }

               

               

               

              

                

              

        //        #endregion

        //        #endregion
    }
}