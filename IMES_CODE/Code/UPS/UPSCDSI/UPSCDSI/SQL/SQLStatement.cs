using System;
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
using UTL;
using UTL.MetaData;
using UPS.UTL.SQL;


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

        #region UPS
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
        #endregion

        #region CDSI
        static public List<string> GetCDSISNList(SqlConnection connect,                                                                              
                                                                               string snoId,
                                                                               string tp)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                List<string> SNList = new List<string>();

                SqlCommand dbCmd = connect.CreateCommand();
                dbCmd.CommandType = CommandType.StoredProcedure;
                dbCmd.CommandText = "op_CDSIDataUpdate";
                SQLHelper.createInputSqlParameter(dbCmd, "@SnoId", 10, snoId);
                SQLHelper.createInputSqlParameter(dbCmd, "@tp", 2, tp);

                logger.DebugFormat("SQL {0}", dbCmd.CommandText);
                logger.DebugFormat("SQL {0} {1}", "SQL", "@SnoId", snoId);
                logger.DebugFormat("SQL {0} {1}", "SQL", "@tp", tp);


                SqlDataReader sdr = dbCmd.ExecuteReader();
                while (sdr.Read())
                {
                    SNList.Add(sdr.GetString(0).Trim());
                }
                sdr.Close();
                return SNList;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
           
        }

        static public List<string> GetCDSISNList(SqlConnection connect,                                                                          
                                                                            int offsetDay)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                List<string> SNList = new List<string>();

                SqlCommand dbCmd = connect.CreateCommand();
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = @"select distinct a.SnoId
                                                            from Special_Det a
                                                            left join ProductAttr b on (a.SnoId = b.ProductID and b.AttrName='CDSIState')
                                                            where a.Tp='CDSI' 
	                                                            and a.Udt>=dateadd(dd,@day,getdate())
	                                                            and b.AttrValue is null";

                SQLHelper.createInputSqlParameter(dbCmd, "@day", offsetDay);

                logger.DebugFormat("SQL {0}", dbCmd.CommandText);
                          
                logger.DebugFormat("SQL {0} {1}", "@day", offsetDay.ToString());



                SqlDataReader sdr = dbCmd.ExecuteReader();

                while (sdr.Read())
                {
                    SNList.Add(sdr.GetString(0).Trim());
                }
                sdr.Close();
                return SNList;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
           
        }

        static public string GetSnoPoMo(SqlConnection connect,
                                                           string productId)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string ret = null;

                SqlCommand dbCmd = connect.CreateCommand();
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = "select top 1 PO from dbo.SnoDet_PoMo (nolock) where SnoId=@SnoId";
                SQLHelper.createInputSqlParameter(dbCmd, "@SnoId", 10, productId);

                logger.DebugFormat("SQL {0}", dbCmd.CommandText);                
                logger.DebugFormat("SQL {0} {1}", "@SnoId", productId);



                SqlDataReader sdr = dbCmd.ExecuteReader();
                while (sdr.Read())
                {
                    ret = sdr.GetString(0).Trim();
                }
                sdr.Close();
                return ret;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
           
        }

        static public void ReadCDSIXML(SqlConnection connect,
                                                        string snoId)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                List<string> SNList = new List<string>();

                SqlCommand dbCmd = connect.CreateCommand();
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = "op_ReadCDSIXML";
                SQLHelper.createInputSqlParameter(dbCmd, "@snoid", 10, snoId);

                logger.DebugFormat("SQL {0}", dbCmd.CommandText);                
                logger.DebugFormat("SQL {0} {1}", "@snoid", snoId);

                dbCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
            

        }

        static public ProductInfo GetProductInfo(SqlConnection connect,                                                                     
                                                                            string prodID)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                ProductInfo Prod = null;

                SqlCommand dbCmd = connect.CreateCommand();
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = @"select a.CUSTSN, 
                                                                   a.Model, 
                                                                   a.MAC, 
                                                                   isnull(b.Value,'') as ATSNAV , 
                                                                   isnull(c.Value,'') as MN1 , 
                                                                   isnull(d.Value,'') as SYSID,
                                                                    a.MO
                                                             from Product a
                                                             left join ModelInfo b on a.Model =b.Model  and b.Name='ATSNAV'
                                                             left join ModelInfo c on a.Model =c.Model  and c.Name='MN1'
                                                             left join ModelInfo d  on a.Model =d.Model  and  d.Name='SYSID'
                                                             where ProductID=@ProductID";
                SQLHelper.createInputSqlParameter(dbCmd, "@ProductID", 15, prodID);

                logger.DebugFormat("SQL {0}", dbCmd.CommandText);
                logger.DebugFormat("SQL {0} {1}", "@ProductID", prodID);


                SqlDataReader sdr = dbCmd.ExecuteReader();
                while (sdr.Read())
                {
                    Prod = new ProductInfo();
                    Prod.ProductID = prodID;
                    Prod.CUSTSN = sdr.GetString(0).Trim();
                    Prod.Model = sdr.GetString(1).Trim();
                    Prod.MAC = sdr.GetString(2).Trim();
                    Prod.ATSNAV = sdr.GetString(3).Trim();
                    Prod.MN1 = sdr.GetString(4).Trim();
                    Prod.SYSID = sdr.GetString(5).Trim();
                    Prod.MOId = sdr.GetString(6).Trim();
                    Prod.PO = "";

                }
                sdr.Close();
                return Prod;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
            
        }


        static public void WriteProductAttr(SqlConnection connect,                                                              
                                                                 string prodID,
                                                                 string name,
                                                                 string value,
                                                                 string editor,
                                                                 DateTime cdt)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                SqlCommand dbCmd = connect.CreateCommand();
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = @"if exists(select * from ProductAttr where ProductID =@ProductId and AttrName=@name)
                                                            begin
	                                                               update ProductAttr
	                                                               set   AttrValue =@value,
			                                                             Editor =@editor,
			                                                             Udt =@cdt 
	                                                               where ProductID =@ProductId and 
			                                                             AttrName=@name 
                                                            end
                                                            else  
                                                            begin
	                                                            insert ProductAttr (AttrName, ProductID, AttrValue, Editor, Cdt, Udt)
	                                                            values(@name,@ProductId,@value,@editor,@cdt,@cdt)
                                                            end";
                SQLHelper.createInputSqlParameter(dbCmd, "@ProductId", 15, prodID);
                SQLHelper.createInputSqlParameter(dbCmd, "@name", 32, name);
                SQLHelper.createInputSqlParameter(dbCmd, "@value", 255, value);
                SQLHelper.createInputSqlParameter(dbCmd, "@editor", 32, editor);
                SQLHelper.createInputSqlParameter(dbCmd, "@cdt", cdt);

                logger.DebugFormat("SQL {0}", dbCmd.CommandText);


                logger.DebugFormat("SQL {0} {1}", "@ProductId", prodID);
                logger.DebugFormat("SQL {0} {1}", "@name", name);
                logger.DebugFormat("SQL {0} {1}", "@value", value);
                logger.DebugFormat("SQL {0} {1}", "@editor", editor);
                logger.DebugFormat("SQL {0} {1}", "@cdt", cdt.ToString());

                dbCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
            

        }

        static public void WriteProductAttr(SqlConnection connect,
                                                            SqlTransaction transaction,
                                                                 string prodID,
                                                                 string name,
                                                                 string value,
                                                                 string editor,
                                                                 DateTime cdt)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                SqlCommand dbCmd = connect.CreateCommand();
                dbCmd.Transaction = transaction;
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = @"if exists(select * from ProductAttr where ProductID =@ProductId and AttrName=@name)
                                                            begin
	                                                               update ProductAttr
	                                                               set   AttrValue =@value,
			                                                             Editor =@editor,
			                                                             Udt =@cdt 
	                                                               where ProductID =@ProductId and 
			                                                             AttrName=@name 
                                                            end
                                                            else  
                                                            begin
	                                                            insert ProductAttr (AttrName, ProductID, AttrValue, Editor, Cdt, Udt)
	                                                            values(@name,@ProductId,@value,@editor,@cdt,@cdt)
                                                            end";
                SQLHelper.createInputSqlParameter(dbCmd, "@ProductId", 15, prodID);
                SQLHelper.createInputSqlParameter(dbCmd, "@name", 32, name);
                SQLHelper.createInputSqlParameter(dbCmd, "@value", 255, value);
                SQLHelper.createInputSqlParameter(dbCmd, "@editor", 32, editor);
                SQLHelper.createInputSqlParameter(dbCmd, "@cdt", cdt);

                logger.DebugFormat("SQL {0}", dbCmd.CommandText);


                logger.DebugFormat("SQL {0} {1}", "@ProductId", prodID);
                logger.DebugFormat("SQL {0} {1}", "@name", name);
                logger.DebugFormat("SQL {0} {1}", "@value", value);
                logger.DebugFormat("SQL {0} {1}", "@editor", editor);
                logger.DebugFormat("SQL {0} {1}", "@cdt", cdt.ToString());

                dbCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }


        }

        static public void WriteCDSIAST(SqlConnection connect,                                                             
                                                                 string prodID,
                                                                 string tp,
                                                                 string value)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                SqlCommand dbCmd = connect.CreateCommand();
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = @"insert CDSIAST(SnoId, Tp, Sno)
                                                            values(@productId,@tp,@value)";
                SQLHelper.createInputSqlParameter(dbCmd, "@productId", 15, prodID);
                SQLHelper.createInputSqlParameter(dbCmd, "@tp", 32, tp);
                SQLHelper.createInputSqlParameter(dbCmd, "@value", 255, value);


                logger.DebugFormat("SQL {0}", dbCmd.CommandText);
                logger.DebugFormat("SQL {0} {1}", "@productId", prodID);
                logger.DebugFormat("SQL {0} {1}", "@tp", tp);
                logger.DebugFormat("SQL {0} {1}", "@value", value);

                dbCmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
          
        }

        static public void WriteCDSIAST(SqlConnection connect,
                                                         SqlTransaction transaction,   
                                                                 string prodID,
                                                                 string tp,
                                                                 string value)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                SqlCommand dbCmd = connect.CreateCommand();
                dbCmd.Transaction = transaction;
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = @"insert CDSIAST(SnoId, Tp, Sno)
                                                            values(@productId,@tp,@value)";
                SQLHelper.createInputSqlParameter(dbCmd, "@productId", 15, prodID);
                SQLHelper.createInputSqlParameter(dbCmd, "@tp", 32, tp);
                SQLHelper.createInputSqlParameter(dbCmd, "@value", 255, value);


                logger.DebugFormat("SQL {0}", dbCmd.CommandText);
                logger.DebugFormat("SQL {0} {1}", "@productId", prodID);
                logger.DebugFormat("SQL {0} {1}", "@tp", tp);
                logger.DebugFormat("SQL {0} {1}", "@value", value);

                dbCmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }

        }

        static public void DeleteCDSIAST(SqlConnection connect,                                                         
                                                                string prodID)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                SqlCommand dbCmd = connect.CreateCommand();
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = @"delete  from CDSIAST where SnoId=@productId";

                SQLHelper.createInputSqlParameter(dbCmd, "@productId", 15, prodID);

                logger.DebugFormat("SQL {0}", dbCmd.CommandText);
                logger.DebugFormat("SQL {0} {1}", "@productId", prodID);


                dbCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
           

        }

        static public void DeleteCDSIAST(SqlConnection connect,
                                                          SqlTransaction transaction,     
                                                               string prodID)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                SqlCommand dbCmd = connect.CreateCommand();
                dbCmd.Transaction = transaction;
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = @"delete  from CDSIAST where SnoId=@productId";

                SQLHelper.createInputSqlParameter(dbCmd, "@productId", 15, prodID);

                logger.DebugFormat("SQL {0}", dbCmd.CommandText);
                logger.DebugFormat("SQL {0} {1}", "@productId", prodID);


                dbCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }


        }

        static public string GetModelInfo(SqlConnection connect,                                                                    
                                                                       string model,
                                                                       string name)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                string ret = null;
                SqlCommand dbCmd = connect.CreateCommand();
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = "select Value from ModelInfo where Model=@model and Name=@Name";
                SQLHelper.createInputSqlParameter(dbCmd, "@model", 15, model);
                SQLHelper.createInputSqlParameter(dbCmd, "@Name", 15, name);

                logger.DebugFormat("SQL {0}", dbCmd.CommandText);              
                logger.DebugFormat("SQL {0} {1}", "@model", model);
                logger.DebugFormat("SQL {0} {1}", "@Name", name);

                SqlDataReader sdr = dbCmd.ExecuteReader();
                while (sdr.Read())
                {

                    ret = sdr.GetString(0).Trim();

                }
                sdr.Close();
                return ret;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
            
        }

        static public void ReadXMLCDSIAST(SqlConnection connect,                                                                
                                                                  string prodID,
                                                                   string xmlFilePath)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                AssetTag tag = GetXMLAssetTag(xmlFilePath);
                DeleteCDSIAST(connect,  prodID);
                WriteCDSIAST(connect,  prodID, "DID", tag.DID);
                WriteCDSIAST(connect,  prodID, "ASSET_TAG", tag.ASSET_TAG);
                WriteCDSIAST(connect,  prodID, "HPOrder", tag.HPOrder);
                WriteCDSIAST(connect,  prodID, "PurchaseOrder", tag.PurchaseOrder);
                WriteCDSIAST(connect,  prodID, "FactoryPO", tag.FactoryPO);
                WriteProductAttr(connect,  prodID, "CDSIState", "OK", "CDSI", DateTime.Now);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
            


        }

        static public AssetTag GetXMLAssetTag(string xmlFilePath)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                AssetTag assetTag = new AssetTag();
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlFilePath);
                assetTag.DID = doc.SelectSingleNode("//INFO/DID") == null ? "" : doc.SelectSingleNode("//INFO/DID").InnerText;
                assetTag.HPOrder = doc.SelectSingleNode("//INFO/HPOrder") == null ? "" : doc.SelectSingleNode("//INFO/HPOrder").InnerText;
                assetTag.PurchaseOrder = doc.SelectSingleNode("//INFO/PurchaseOrder") == null ? "" : doc.SelectSingleNode("//INFO/PurchaseOrder").InnerText;
                assetTag.FactoryPO = doc.SelectSingleNode("//INFO/FactoryPO") == null ? "" : doc.SelectSingleNode("//INFO/FactoryPO").InnerText;
                assetTag.ASSET_TAG = doc.SelectSingleNode("//RESULT/DATA[@id='1']/VALUE") == null ? "" : doc.SelectSingleNode("//RESULT/DATA[@id='1']/VALUE").InnerText;
                return assetTag;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
           
        }

        static public List<MO> GetAvailableMO(SqlConnection connect,                                                                 
                                                                  ProductInfo prodInfo,
                                                                   int startDateoffset,
                                                                   int udtOffset)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                List<MO> ret = new List<MO>();

                DateTime now = DateTime.Now;
                DateTime startDate = now.AddDays(startDateoffset);
                DateTime udtDate = now.AddDays(udtOffset);

                SqlCommand dbCmd = connect.CreateCommand();
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = @"select MO, (Qty-Print_Qty) as qty  from MO 
                                                          where Model=@model and 
                                                                     StartDate>=@StartDate and
                                                                     Qty>Print_Qty and
                                                                      Status ='H'  and  
                                                                     Udt>=@UpdateTime
                                                         order by StartDate, Udt";
                SQLHelper.createInputSqlParameter(dbCmd, "@model", 15, prodInfo.Model);
                SQLHelper.createInputSqlParameter(dbCmd, "@StartDate", 1, startDate);
                SQLHelper.createInputSqlParameter(dbCmd, "@UpdateTime", udtDate);

                logger.DebugFormat("SQL {0}", dbCmd.CommandText);
                            
                logger.DebugFormat("SQL {0} {1}", "@model", prodInfo.Model);
                logger.DebugFormat("SQL {0} {1}", "@StartDate", startDate.ToString());
                logger.DebugFormat("SQL {0} {1}", "@UpdateTime", udtDate.ToString());

                SqlDataReader sdr = dbCmd.ExecuteReader();
                while (sdr.Read())
                {
                    MO mo = new MO();
                    mo.MOId = sdr.GetString(0).Trim();
                    mo.Qty = sdr.GetInt32(1);

                    ret.Add(mo);
                }
                sdr.Close();
                return ret;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
           
        }

        static public int GetAssignMOQty(SqlConnection connect,                                                                 
                                                                  string moId)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                int ret = 0;

                SqlCommand dbCmd = connect.CreateCommand();
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = @"select count(*)
                                                            from SnoDet_PoMo 
                                                          where Mo=@Mo ";
                SQLHelper.createInputSqlParameter(dbCmd, "@Mo", 32, moId);

                logger.DebugFormat("SQL {0}", dbCmd.CommandText);
                logger.DebugFormat("SQL {0} {1}", "@Mo", moId);



                SqlDataReader sdr = dbCmd.ExecuteReader();
                while (sdr.Read())
                {
                    ret = sdr.GetInt32(0);
                }
                sdr.Close();
                return ret;

            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
          
        }


        static public List<Delivery> GetAvailableDelivery(SqlConnection connect,
                                                                                     ProductInfo prodInfo,
                                                                                       int shipDateoffset)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                List<Delivery> ret = new List<Delivery>();

                DateTime now = DateTime.Now;
                DateTime shipDate = new DateTime(now.Year, now.Month, now.Day);  //now.AddDays(shipDateoffset);
                DateTime shipDateEnd = shipDate.AddDays(shipDateoffset);

                SqlCommand dbCmd = connect.CreateCommand();
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = @"select DeliveryNo, PoNo,Qty from Delivery 
                                                          where Model=@model and 
                                                                     ShipDate between @ShipDate and @ShipDateEnd and
                                                                     Qty> 0 and
                                                                     Status ='00'  
                                                         order by ShipDate";
                SQLHelper.createInputSqlParameter(dbCmd, "@model", 15, prodInfo.Model);
                SQLHelper.createInputSqlParameter(dbCmd, "@ShipDate", shipDate);
                SQLHelper.createInputSqlParameter(dbCmd, "@ShipDateEnd", shipDateEnd);

                logger.DebugFormat("SQL {0}", dbCmd.CommandText);
                
                logger.DebugFormat("SQL {0} {1}", "@model", prodInfo.Model);
                logger.DebugFormat("SQL {0} {1}", "@ShipDate", shipDate.ToString());
                logger.DebugFormat("SQL {0} {1}", "@ShipDateEnd", shipDateEnd.ToString());



                SqlDataReader sdr = dbCmd.ExecuteReader();
                while (sdr.Read())
                {
                    Delivery delivery = new Delivery();
                    delivery.DeliveryNo = sdr.GetString(0).Trim();
                    delivery.PO = sdr.GetString(1).Trim();
                    delivery.Qty = sdr.GetInt32(2);
                    ret.Add(delivery);
                }
                sdr.Close();
                return ret;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
           
        }

        static public int GetAssignPOQty(SqlConnection connect,                                                               
                                                                 string DeliveryNo,
                                                                 string PoNo)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                int ret = 0;

                SqlCommand dbCmd = connect.CreateCommand();
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = @"select count(*)
                                                            from SnoDet_PoMo 
                                                          where PO=@PO or
                                                                     Delivery=@DeliveryNo";
                SQLHelper.createInputSqlParameter(dbCmd, "@PO", 32, PoNo);
                SQLHelper.createInputSqlParameter(dbCmd, "@DeliveryNo", 32, DeliveryNo);

                logger.DebugFormat("SQL {0}", dbCmd.CommandText);             

                logger.DebugFormat("SQL {0} {1}", "@PO", PoNo);
                logger.DebugFormat("SQL {0} {1}", "@DeliveryNo", DeliveryNo);


                SqlDataReader sdr = dbCmd.ExecuteReader();
                while (sdr.Read())
                {
                    ret = sdr.GetInt32(0);
                }
                sdr.Close();
                return ret;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
            

        }


        static public void InsertCDSIPoMo(SqlConnection connect,                                                               
                                                                CDSIPO poInfo)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                //SqlCommand dbCmd = new SqlCommand();
                SqlCommand dbCmd = connect.CreateCommand();
                //dbCmd.Connection = connect;
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = @"insert dbo.SnoDet_PoMo(SnoId, Mo, PO, POItem, Delivery, PLT, BoxId, Remark, Editor, Cdt, Udt)
                                                            values(@SnoId, @Mo, @PO, '00001', @Delivery, '', '', '', 'CDSI', GETDATE(), GETDATE())";

                SQLHelper.createInputSqlParameter(dbCmd, "@SnoId", 15, poInfo.ProductID);
                SQLHelper.createInputSqlParameter(dbCmd, "@Mo", 32, poInfo.MOId);
                SQLHelper.createInputSqlParameter(dbCmd, "@PO", 32, poInfo.PO);
                SQLHelper.createInputSqlParameter(dbCmd, "@Delivery", 32, poInfo.DeliveryNo);

                logger.DebugFormat("SQL {0}", dbCmd.CommandText);              
                logger.DebugFormat("SQL {0} {1}", "@SnoId", poInfo.ProductID);
                logger.DebugFormat("SQL {0} {1}", "@Mo", poInfo.MOId);
                logger.DebugFormat("SQL {0} {1}", "@PO", poInfo.PO);
                logger.DebugFormat("SQL {0} {1}", "@Delivery", poInfo.DeliveryNo);
                dbCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }  
        }

        static public void InsertCDSIPoMo(SqlConnection connect,
                                                            SqlTransaction transaction,    
                                                                CDSIPO poInfo)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                SqlCommand dbCmd = new SqlCommand();
                //SqlCommand dbCmd = connect.CreateCommand();
                dbCmd.Connection = connect;
                dbCmd.Transaction = transaction;
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = @"insert dbo.SnoDet_PoMo(SnoId, Mo, PO, POItem, Delivery, PLT, BoxId, Remark, Editor, Cdt, Udt)
                                                            values(@SnoId, @Mo, @PO, '00001', @Delivery, '', '', '', 'CDSI', GETDATE(), GETDATE())";

                SQLHelper.createInputSqlParameter(dbCmd, "@SnoId", 15, poInfo.ProductID);
                SQLHelper.createInputSqlParameter(dbCmd, "@Mo", 32, poInfo.MOId);
                SQLHelper.createInputSqlParameter(dbCmd, "@PO", 32, poInfo.PO);
                SQLHelper.createInputSqlParameter(dbCmd, "@Delivery", 32, poInfo.DeliveryNo);

                logger.DebugFormat("SQL {0}", dbCmd.CommandText);
                logger.DebugFormat("SQL {0} {1}", "@SnoId", poInfo.ProductID);
                logger.DebugFormat("SQL {0} {1}", "@Mo", poInfo.MOId);
                logger.DebugFormat("SQL {0} {1}", "@PO", poInfo.PO);
                logger.DebugFormat("SQL {0} {1}", "@Delivery", poInfo.DeliveryNo);
                dbCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
        }


        static public bool AssignMO(SqlConnection connect,                                                    
                                                        AppConfig config,
                                                        ProductInfo prodInfo,
                                                       CDSIPO poInfo)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                bool selectedMo = false;

                List<MO> moList = GetAvailableMO(connect,                                                                         
                                                                               prodInfo,
                                                                                config.MOStartDateOffSetDay,
                                                                                config.MOUdtOffSetDay);
                if (moList.Count == 0) return selectedMo;

                int availableQty = 0;

                foreach (MO mo in moList)
                {
                    availableQty = mo.Qty - GetAssignMOQty(connect,  mo.MOId);
                    if (availableQty > 0)
                    {

                        poInfo.MOId = mo.MOId;
                        selectedMo = true;
                        break;
                    }
                }

                return selectedMo;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }
           

        }

        static public bool AssignPO(SqlConnection connect,                                                     
                                                        AppConfig config,
                                                        ProductInfo prodInfo,
                                                      CDSIPO poInfo)
        {

            bool selectedPo = false;

            List<Delivery> poList = GetAvailableDelivery(connect,
                                                                                   prodInfo,
                                                                                      config.ShipDateOffSetDay);
            if (poList.Count == 0) return selectedPo;

            int availableQty = 0;

            foreach (Delivery po in poList)
            {
                availableQty = po.Qty - GetAssignPOQty(connect,  po.DeliveryNo, po.PO);
                if (availableQty > 0)
                {
                    poInfo.PO = po.PO;
                    poInfo.DeliveryNo = po.DeliveryNo;
                    prodInfo.PO = po.PO;                    
                    selectedPo = true;
                    break;
                }
            }

            return selectedPo;
        }


        static public int CheckCDSIFactoryPO(SqlConnection connect,                                                                          
                                                                          string po)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}()", methodName);
            try
            {
                int selectedPo = 0;

                SqlCommand dbCmd = connect.CreateCommand();
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = @"select  FactoryPO, 1 as Category 
                                                            from [CDSI_SHELL].dbo.Units 
                                                            where FactoryPO  = @PO
                                                            union
                                                            select  FactoryPO, 2 as Category
                                                            from [CDSI2].dbo.Units
                                                            where FactoryPO =@PO ";
                SQLHelper.createInputSqlParameter(dbCmd, "@PO", 32, po);

                logger.DebugFormat( "SQL {0}" ,dbCmd.CommandText);
                logger.DebugFormat("SQL {0} {1}", "@PO", po);

                SqlDataReader sdr = dbCmd.ExecuteReader();
                while (sdr.Read())
                {
                    selectedPo = sdr.GetInt32(1);
                }

                sdr.Close();
                return selectedPo;
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}()", methodName);
            }            
        }
        #endregion
                          
    }

    

    public class ModelBom
    {
        public string Model;
        public string AVPartNo;
        public string IECPartNo;
        public string IECPartType;
    }


    public class ProductInfo
    {
        public string ProductID;
        public string Model;
        public string CUSTSN;
        public string MAC;
        public string ATSNAV;
        public string MN1;
        public string SYSID;
        public string MOId;
        public string PO;
    }

    public class CDSIPO
    {
        public string ProductID;
        public string MOId;
        public string PO;
        //public string POItem;
        public string DeliveryNo;
        //public string PalletNo;
        //public string BoxId;
        //public string Remark;
        //public string Editor;
        //public DateTime Cdt;
        //public DateTime Udt;
    }

    public class Delivery
    {
        public string DeliveryNo;
        public string PO;
        public int Qty;
    }

    public class MO
    {
        public string MOId;
        public int Qty;
    }

    public class AssetTag
    {
        public string DID;
        public string HPOrder;
        public string PurchaseOrder;
        public string FactoryPO;
        public string ASSET_TAG;
    }

}
