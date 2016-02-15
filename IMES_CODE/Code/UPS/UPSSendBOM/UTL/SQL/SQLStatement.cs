﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using UTL.SQL;
using UTL;
using log4net;
using System.Reflection;
using System.Collections;
using UTL.Config;
using UTL.MetaData;
using UPS.UTL.SQL;
using UPS;

namespace UTL.SQL
{
    public class SQLStatement
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //private static string TimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

        static public DataTable executeSP(string connectionString,
                                                     string SPName,
                                                    params SqlParameter[] cmdParms)
        {
            DataTable Ret = new DataTable();
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                return SQLHelper.ExecuteDataFill(connectionString, CommandType.StoredProcedure, SPName, cmdParms);

            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                return Ret;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }

        }


        static public List<ModelBom> GetModelAVBom(string strConnect, 
                                                                                List<string> modelList)
        {
            List<ModelBom> ret = new List<ModelBom>();
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                using (SqlConnection conn = new SqlConnection(strConnect))
                {
                    conn.Open();
                    string strSQL = @"select distinct a.Material as Model,
                                                           b.InfoValue as AVPartNo,
                                                           d.PartNo as IECPartNo,
                                                           d.Descr  as IECPartType 
                                                    from ModelBOM a,
                                                            PartInfo b,
                                                            Part  d,
                                                         @ModelList c
                                                    where a.Material = c.data and
                                                          a.Component = b.PartNo and
                                                          a.Component = d.PartNo and  
                                                          b.InfoType='AV'        and
                                                          a.Component like '2TG%'";

                    SqlParameter para = new SqlParameter("@ModelList ", SqlDbType.Structured);
                    para.TypeName = "TbStringList";
                    para.Direction = ParameterDirection.Input;
                    para.Value = SQLReader.ToDataTable(modelList);
                    logger.InfoFormat("Model List:{0}", string.Join(",", modelList.ToArray()));

                  using( SqlDataReader  sdr= SQLHelper.ExecuteReader(conn, CommandType.Text, strSQL,para))
                  {
                      if(sdr!=null)
                      {
                          while(sdr.Read())
                          {
                              ModelBom item=SQLReader.ToObjectByField<ModelBom>(sdr);
                              ret.Add(item);
                          }
                      }
                  } 
              }
                return ret;             
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }


        static public void  InsertUPSSupportAV(string strConnect,
                                                                   List<string> avPartNoList)
        {            
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                  string strSQL = @"delete from UPSAVPart;
                                                 insert UPSAVPart(PartNo, Cdt)
                                                 select data,
                                                            getdate() 
                                                    from @AVPartNoList ";

                    SqlParameter para = new SqlParameter("@AVPartNoList ", SqlDbType.Structured);
                    para.TypeName = "TbStringList";
                    para.Direction = ParameterDirection.Input;
                    para.Value = SQLReader.ToDataTable(avPartNoList);
                    logger.InfoFormat("AV PartNo List:{0}", string.Join(",", avPartNoList.ToArray()));

                    SQLHelper.ExecuteNonQuery(strConnect, CommandType.Text, strSQL, para);  
               
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }
        static public void DeleteUPSHPPO(string strConnect, UPSHPPO upshppo)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string strSQL = @"delete from UPSHPPO WHERE HPPO=@HPPO ";
                SQLHelper.ExecuteNonQuery(strConnect, CommandType.Text, strSQL,  SQLHelper.createInputSqlParameter("@HPPO",upshppo.HPPO));

            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        static public void InsertAssetRange(string strConnect,
                                                             string code,
                                                             string begin,
                                                             string end,
                                                             string remark,
                                                            string editor)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string strSQL = @"insert AssetRange(Code, [Begin], [End], Remark, Editor, Cdt, Udt, Status)
                                            Values(@Code, @Begin, @End, @Remark, @Editor, GETDATE(),GETDATE(),'R') ";                

                SQLHelper.ExecuteNonQuery(strConnect, CommandType.Text, strSQL, 
                                                                SQLHelper.createInputSqlParameter("@Code",code),
                                                                SQLHelper.createInputSqlParameter("@Begin", begin),
                                                                 SQLHelper.createInputSqlParameter("@End", end),
                                                                  SQLHelper.createInputSqlParameter("@Remark", remark),
                                                                   SQLHelper.createInputSqlParameter("@Editor", editor));

            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }


        static public void InsertAssetRangePostFixed(string strConnect,
                                                                           string avPartNo,
                                                                           string postFix,
                                                                           string remark,
                                                                           string editor)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string strSQL = @"if exists(select Name from ConstValue where Type ='AST' and Name =@AvPartNo)
                                            begin
                                               update ConstValue
                                               set Value =@PostFix,
                                                   Udt =GETDATE(),
                                                   Editor =@Editor
                                               where Type='AST' and
                                                     Name =@AvPartNo     
                                            end
                                            else
                                            begin
                                               insert ConstValue(Name, Type, Value, Description, Editor, Cdt, Udt)
                                               Values(@AvPartNo,'AST',@PostFix,@Remark,@Editor, GETDATE(),GETDATE())
                                            end  ";

                SQLHelper.ExecuteNonQuery(strConnect, CommandType.Text, strSQL,
                                                                SQLHelper.createInputSqlParameter("@AvPartNo", avPartNo),
                                                                SQLHelper.createInputSqlParameter("@PostFix", postFix),
                                                                SQLHelper.createInputSqlParameter("@Remark", remark),
                                                                SQLHelper.createInputSqlParameter("@Editor", editor));

            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }



        static public bool CheckAssetRangeByHpPo(string strConnect,
                                                                                          string hpPo)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                using (SqlConnection conn = new SqlConnection(strConnect))
                {
                    conn.Open();
                    string strSQL = @"select a.Code 
                                                    from AssetRange a
                                                    where a.Remark like @Remark";



                    using (SqlDataReader sdr = SQLHelper.ExecuteReader(conn, CommandType.Text, strSQL,
                        SQLHelper.createInputSqlParameter("@Remark", "PO:" + hpPo +"%")))
                    {
                        if (sdr != null)
                        {
                            return sdr.HasRows;                          
                        }
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        public static IList<UPSHPPO> GetUPSHPPO(AppConfig config,List<string> poList)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.InfoFormat("BEGIN: {0}()", methodName);
            try
            {
                UPSDatabase db = new UPSDatabase(config.DBConnectStr);

                return (from po in db.UPSHPPOEntity
                                           join d in poList on po.HPPO equals d
                                           select po).ToList();

            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {
                logger.InfoFormat("END: {0}()", methodName);
            }
        }

        public static List<ConstValueTypeInfo> GetConstValueType(string strConnect, string type)
        {

            List<ConstValueTypeInfo> ret = new List<ConstValueTypeInfo>();
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                using (SqlConnection conn = new SqlConnection(strConnect))
                {
                    conn.Open();
                    string strSQL = @"SELECT ID,Type,Value,Description,Editor FROM dbo.ConstValueType WHERE Type=@type";
                    using (SqlDataReader sdr = SQLHelper.ExecuteReader(conn, CommandType.Text, strSQL, SQLHelper.createInputSqlParameter("@type", type)))
                    {
                        if (sdr != null)
                        {
                            while (sdr.Read())
                            {
                                ConstValueTypeInfo item = SQLReader.ToObjectByField<ConstValueTypeInfo>(sdr);
                                ret.Add(item);
                            }
                        }
                    }
                }
                return ret;
            }
            catch (Exception e)
            {
                logger.Error(methodName, e);
                throw e;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }
                          
    }

    

    public class ModelBom
    {
        public string Model;
        public string AVPartNo;
        public string IECPartNo;
        public string IECPartType;
    }
   
     public class ConstValueTypeInfo
    {
        public Int32 ID ;
        public String Type ;
        public String Value ;
        public String Description ;
        public String Editor ;
    }

}
