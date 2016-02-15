using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Transactions;
using System.Xml;
using System.Data;
using System.IO;
using System.Threading;//
//using ;
using ArchiveInterface;
using System.Reflection;
namespace ArchiveDB
{
    class DBAcess
    {
        private const int _retryCount = 6;
        private int _checkMinDiffCount = 500;
        private SqlConnection _copySqlCon;
        private SqlConnection _arcSqlCon;
        private SqlConnection _deleteSqlCon;
        private bool _isHaveError;
        private Log _infoLog;
        private Log _errorLog;
        private int _archiveDay;
        private int _cdtDay;
        private int _logDay;
        private int _deleteRows;
        private string _selepTime;
        private int _deleteTimeOut;
        private bool _isDeletSrcData;
        private string _xmlPath;
        private string _timeStamp;
        private string _dbName;
        private DataTable tableRowCount;
        private DataTable _dtArchiveMainTable;
        private DataTable _dtArchiveTableList;
        private DataTable _dtArchiveFKTableList;
        private Mail _mail;
        private int _continuteDeleteDay;


        private DataSet dsArchiveID; //For Export CSV using
        private string _mailErrTitle = "";
        private IArchiveInterface _objDBSetting;
        public DBAcess(string srcConnString, string destConnString, int Day, Log infoLog, Log errorLog, bool isDeleteSrcData)
        {

            _copySqlCon = new SqlConnection(srcConnString);
            _arcSqlCon = new SqlConnection(destConnString);
            _archiveDay = Day;
            _infoLog = infoLog;
            _errorLog = errorLog;
            _isDeletSrcData = isDeleteSrcData;
            dsArchiveID = new DataSet();

        }
        public DBAcess()
        {

            IniConfigSetting();
            IniSettingTable();
            InsertArchiveLog("Begin Archive", "", LogType.Event, _timeStamp);
            _isHaveError = false;

        }
        private void IniConfigSetting()
        {
            Config config = new Config();
            dsArchiveID = new DataSet();
            string arcConnString = config.GetConnectionString("ArcConnectionString");
            string copyConnString = config.GetConnectionString("CopyConnectionString");
            string deleteConnString = config.GetConnectionString("DeleteConnectionString");
            bool isTest = config.GetValue("IsTest") == "Y" ? true : false;
            if (isTest)
            {
                arcConnString = config.GetConnectionString("TestArcConnectionString");
                copyConnString = config.GetConnectionString("TestCopyConnectionString");
                deleteConnString = config.GetConnectionString("TestDeleteConnectionString");

            }



            _arcSqlCon = new SqlConnection(arcConnString);
            _copySqlCon = new SqlConnection(copyConnString);
            _deleteSqlCon = new SqlConnection(deleteConnString);



            _archiveDay = int.Parse(config.GetValue("Day"));
            _cdtDay = int.Parse(config.GetValue("CdtDay"));
            _logDay = int.Parse(config.GetValue("KeepLogDay"));
            _deleteRows = int.Parse(config.GetValue("DeleteRows"));
            _selepTime = config.GetValue("SelepTime");
            _deleteTimeOut = int.Parse(config.GetValue("DeleteTimeOut"));
            //    _checkMinDiffCount = int.Parse(config.GetValue("CheckMinDiffCount"));
            string logPath = config.GetValue("LogPath");
            _xmlPath = config.GetValue("XMLPath");
            _continuteDeleteDay = int.Parse(config.GetValue("ContinueDeleteDay"));
            _infoLog = new Log(logPath, LogType.Info);
            _errorLog = new Log(logPath, LogType.Error);
            _timeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
            //   _isNeedExportXML = config.GetValue("IsNeedExportToXML") == "Y" ? true : false;
            _isDeletSrcData = config.GetValue("IsDeleteSrcData") == "Y" ? true : false;
            _mail = new Mail();
            _dbName = config.GetValue("DBName");
            _mailErrTitle = "Archive " + _dbName + " Error :";

            string ddlPath = System.AppDomain.CurrentDomain.BaseDirectory + _dbName + ".dll";
            Assembly asmb = Assembly.LoadFrom(ddlPath);
            _objDBSetting = (IArchiveInterface)asmb.CreateInstance("DBSetting." + _dbName);
        }
        public IArchiveInterface GetDBSettingObj()
        {
            return _objDBSetting;
        }
        private SqlConnection OpenCopyCon()
        {
            if (_copySqlCon.State != ConnectionState.Open) _copySqlCon.Open();
            return _copySqlCon;
        }
        private SqlConnection OpenArcCon()
        {
            if (_arcSqlCon.State != ConnectionState.Open) _arcSqlCon.Open();
            return _arcSqlCon;
        }
        private SqlConnection OpenDeleteCon()
        {
            if (_deleteSqlCon.State != ConnectionState.Open) _deleteSqlCon.Open();
            return _deleteSqlCon;
        }

        public void ClosAllCon()
        {
            if (_deleteSqlCon.State == ConnectionState.Open) _deleteSqlCon.Close();
            if (_arcSqlCon.State == ConnectionState.Open) _arcSqlCon.Close();
            if (_copySqlCon.State == ConnectionState.Open) _copySqlCon.Close();

        }
        private void IniSettingTable()
        {
            //       _copySqlCon.Open();
            string sql = @" select * from ArchiveMainTable where Enable='Y' order by  DeleteOrder 
                                   select * from ArchiveTableList  order by  DeleteOrder
                                   select * from ArchiveFKTableList ";

            DataSet ds = SQLTool.GetDataByDataSet(OpenDeleteCon(), sql);
            _dtArchiveMainTable = ds.Tables[0];
            _dtArchiveTableList = ds.Tables[1];
            _dtArchiveFKTableList = ds.Tables[2];

        }




        private string SetCopyScript(DataRow dr, string mainPK, string mainTable)
        {
            //mainPK : ProductID,DeliveryNo,PalletNo....
            string pKName2 = dr["PKName2"].ToString().Trim();
            string tableName = dr["TableName"].ToString().Trim();
            string fkName = dr["FKName"].ToString().Trim();
            string item = dr["Item"].ToString().Trim();
            string result;
            //     string fkResult = "";
            if (string.IsNullOrEmpty(pKName2))
            {
                result = Tool.SetCopySQL(0);
                result = string.Format(result, tableName, mainPK, item);

            }
            else
            {
                result = Tool.SetCopySQL(1);
                result = string.Format(result, tableName, pKName2, mainTable, mainPK, item);

            }
            //  result = fkResult + result;
            return result;
        }
        private void MoveFKTable(string mainTbName, string parentfkName, string sql)
        {
            string fkResultSQL;
            bool isXML;
            bool isBulkCopySucc = false;
            bool isDeleteSucc = false;

            foreach (DataRow drFK in _dtArchiveFKTableList.Select("ParentTableName='" + mainTbName + "'"))
            {
                isXML = drFK["IsNeedXML"].ToString().Trim().Equals("Y");
                fkResultSQL = "";
                fkResultSQL = Tool.SetCopyFKTableSQL(sql, parentfkName, drFK["FKName"].ToString(), drFK["TableName"].ToString());
                DataTable dt = SQLTool.GetDataByDataTable(OpenCopyCon(), fkResultSQL);
                dt.TableName = drFK["TableName"].ToString();
                isBulkCopySucc = BulkCopy(dt, fkResultSQL, drFK["ViewName"].ToString());
                if (isBulkCopySucc)
                { DeleteDataRetry(fkResultSQL, drFK["TableName"].ToString()); }
                // { isDeleteSucc = DeleteData(fkResultSQL, drFK["TableName"].ToString()); }
                if (isXML && dt.Rows.Count > 0 && isBulkCopySucc)
                { ExportXML(dt); }
            }
            if (isBulkCopySucc && isDeleteSucc) DeleteDataRetry(sql, mainTbName);


        }
        private string GetDeleteSql(string selSql, string tbName)
        {
            string delSql = "";
            delSql = _objDBSetting.GetDeleteSql(tbName, _deleteRows.ToString(), _selepTime);
            if (string.IsNullOrEmpty(delSql))
            { delSql = Tool.SetDeleteSQLfromCopy(selSql, _deleteRows.ToString(), _selepTime); }
            return delSql;
        }


        private void DeleteDataRetry(string sql, string tbName)
        {
            if (!_isDeletSrcData) { return; }
            string deleteSql = GetDeleteSql(sql, tbName);
            int i = 0;
            string errMsg = "";
            DateTime startTime = DateTime.Now;
            TimeSpan copyTime;
            int cost;
            while (i < _retryCount)
            {
                try
                {
                    Console.WriteLine("Begin to delete " + tbName + "  data..");
                    DeleteData(deleteSql);
                    copyTime = DateTime.Now - startTime;
                    cost = copyTime.Minutes * 60 + copyTime.Seconds;
                    Console.WriteLine("Finish to delete " + tbName + " data, it takes " + cost.ToString() + " sec");
                    UpdateArchiveTableLog(tbName, 0, "Y", "", false, 0, cost, deleteSql);
                    return;

                }
                catch (Exception exp)
                {
                    errMsg = errMsg + "Retry :" + i.ToString() + " ; Error Message :" + exp.Message + "<BR>";
                    _mail.Send(_mailErrTitle + "[DeleteDataRetry]  Delete data " + tbName + " Error:",
                        "Delete data retry :" + i.ToString() + " <BR>Error Message:<BR>" + errMsg + "<BR> Delete SQL :<BR>" + deleteSql);

                }

                Thread.Sleep(5000);
                i++;

            }

            _errorLog.WriteLog("[DeleteDataRetry]  Delete data " + tbName + " Error :" + errMsg);
            UpdateArchiveTableLog(tbName, 0, "N", errMsg + " ;SQL : " + deleteSql, false, 0, 0, deleteSql);

            try
            { _mail.Send(_mailErrTitle + "[DeleteDataRetry]  Delete data " + tbName + " Error:", "Error Message:<BR>" + errMsg + "<BR>" + " Delete SQL :" + "<BR>" + deleteSql); }
            catch { }
            _isHaveError = true;
            throw new Exception("DeleteDataRetry have error, AP stop");
            //  return false;
        }



        private void DeleteData(string deleteSql)
        {
            try
            {
                //     string delSql = GetDeleteSqlScript.GetSQL(tbName, _deleteRows.ToString());
                //     if (string.IsNullOrEmpty(delSql))
                //     { delSql = Tool.SetDeleteSQLfromCopy(selSql, _deleteRows.ToString(), _selepTime); }
                //  sqlCmd.CommandTimeout = 900;
                SqlCommand sqlCmd = new SqlCommand(deleteSql, OpenDeleteCon());
                if (_deleteTimeOut < 600)
                { _deleteTimeOut = 600; }
                sqlCmd.CommandTimeout = _deleteTimeOut;
                sqlCmd.ExecuteNonQuery();

            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                OpenDeleteCon().Close();
            }
        }
        //private bool DeleteData(string sql, string tbName)
        //{
        //    bool isSuc = false;
        //    if (!_isDeletSrcData) { return true; }
        //    try
        //    {
        //        Console.WriteLine("Begin to delete " + tbName + "  data..");
        //        DateTime startTime = DateTime.Now;
        //        TimeSpan copyTime;

        //        sql = Tool.SetDeleteSQLfromCopy(sql);
        //        SqlCommand sqlCmd = new SqlCommand(sql, OpenDeleteCon());
        //        sqlCmd.CommandTimeout = 1200;
        //        sqlCmd.ExecuteNonQuery();
        //        copyTime = DateTime.Now - startTime;
        //        int cost = copyTime.Minutes * 60 + copyTime.Seconds;
        //        Console.WriteLine("Finish to delete " + tbName + " data, it takes " + cost.ToString() + " sec");

        //        UpdateArchiveTableLog(tbName, 0, "Y", "", false, 0, cost);
        //        isSuc = true;
        //    }
        //    catch (Exception exp)
        //    {
        //        _errorLog.WriteLog("[DeleteData]  Delete data " + tbName + " Error :" + exp.Message);
        //        UpdateArchiveTableLog(tbName, 0, "N", exp.Message + " ;SQL : " + sql, false, 0, 0);

        //        // "Archive HB DB Error :"
        //        try
        //        {
        //            _mail.Send(_mailErrTitle + "[DeleteData]  Delete data " + tbName + " Error:", "Error Message:" + exp.Message + "\r\t" + "SQL :" + sql);

        //        }
        //        catch { }


        //        _isHaveError = true;
        //    }
        //    return isSuc;

        //}
        public void MoveCdtData()
        {
            try
            {
                DateTime startTime = DateTime.Now;
                string sql = "";
                string tbName = "";
                bool isXML;


                DataRow[] drArry = _dtArchiveTableList.Select("Item='Cdt' and Enable='Y'");
                DataTable dt;
                foreach (DataRow dr in drArry)
                {
                    isXML = dr["IsNeedXML"].ToString().Trim().Trim().Equals("Y");
                    try
                    {
                        tbName = dr["TableName"].ToString().Trim();
                        sql = Tool.SetCopyCdtSQL(tbName, _cdtDay);
                        dt = SQLTool.GetDataByDataTable(OpenCopyCon(), sql);
                        dt.TableName = tbName;
                        BulkCopy(dt, sql, dr["ViewName"].ToString().Trim());

                        DeleteDataRetry(sql, tbName);
                        if (isXML && dt.Rows.Count > 0)
                        { ExportXML(dt); }
                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine(exp.Message);
                        _errorLog.WriteLog("[ExecuteMoveData]  move table " + tbName + " Error :" + exp.Message);
                        UpdateArchiveTableLog(tbName, 0, "N", exp.Message + " ;SQL : " + sql, true, 0, 0, "");
                        _isHaveError = true;
                    }


                }
                TimeSpan copyTime = DateTime.Now - startTime;
                int cost = copyTime.Minutes * 60 + copyTime.Seconds;
                InsertArchiveLog("MoveCdtData", cost.ToString(), LogType.Cost, _timeStamp);
            }
            catch (Exception exp)
            {
                InsertArchiveLog("MoveCdtData", exp.Message, LogType.Error, _timeStamp);
                _mail.Send(_mailErrTitle + "MoveCdtData", "Error " + exp.Message);
                _isHaveError = true;
                throw new Exception("MoveCdtData error message :" + exp.Message);

            }
        }
        public void MoveOtherData()
        {
            try
            {
                DateTime startTime = DateTime.Now;
                string sql = "";
                //   string pkName;
                string tbName;
                string fkName;

                DataRow[] drArry = _dtArchiveTableList.Select("Item='Other' and Enable='Y'", "DeleteOrder");
                bool isXML;
                foreach (DataRow dr in drArry)
                {
                    isXML = dr["IsNeedXML"].ToString().Trim().Trim().Equals("Y");
                    sql = dr["SQLScript"].ToString().Trim();
                    tbName = dr["TableName"].ToString().Trim();
                    DataTable dt = SQLTool.GetDataByDataTable(OpenCopyCon(), sql);
                    dt.TableName = tbName;
                    if (BulkCopy(dt, sql, dr["ViewName"].ToString().Trim()))
                    {
                        if (isXML && dt.Rows.Count > 0)
                        { ExportXML(dt); }
                        fkName = dr["FKName"].ToString().Trim();
                        if (!string.IsNullOrEmpty(fkName))
                        { MoveFKTable(dr["TableName"].ToString().Trim(), fkName, sql); }
                        DeleteDataRetry(sql, tbName);
                        //else
                        //{ DeleteDataRetry(sql, tbName); }
                    }


                }

                TimeSpan copyTime = DateTime.Now - startTime;
                int cost = copyTime.Minutes * 60 + copyTime.Seconds;
                InsertArchiveLog("MoveOtherData", cost.ToString(), LogType.Cost, _timeStamp);
            }

            catch (Exception exp)
            {
                InsertArchiveLog("MoveOtherData", exp.Message, LogType.Error, _timeStamp);
                _mail.Send(_mailErrTitle + "MoveOtherData", "Error " + exp.Message);
                _isHaveError = true;
                throw new Exception("MoveOtherData error message :" + exp.Message);

            }

        }
        public void TestBulkCopy()
        {
            string sql = @"select * from ProductLog where ProductID in
                                (
                                Select ProductID from Product where DeliveryNo in    (select DeliveryNo from  
                                dbo.Delivery where ShipDate < DateAdd(d,-180, GETDATE()))
                                )
                                ";
            DataTable dt = SQLTool.GetDataByDataTable(OpenCopyCon(), sql);

            SqlBulkCopy bulkCopy = new SqlBulkCopy(OpenArcCon());
            bulkCopy.BulkCopyTimeout = 300;
            bulkCopy.DestinationTableName = "ProductLog";
            bulkCopy.WriteToServer(dt);

        }
        public void MoveMainData()
        {
            try
            {
                DateTime startTime = DateTime.Now;
                string sql = "";
                string pkName;
                string tbName;
                string item = "";
                bool isXML;
                DataRow[] drArry = _dtArchiveMainTable.Select(" Enable='Y'");

                foreach (DataRow dr in drArry)
                {
                    isXML = dr["IsNeedXML"].ToString().Trim().Trim().Equals("Y");
                    pkName = dr["PKName"].ToString().Trim();
                    tbName = dr["TableName"].ToString().Trim();
                    item = dr["Item"].ToString().Trim();
                    if (!GetSubTableMoveResult(item)) continue;
                    sql = @"select * from {0} where {1} in
                                 (
                                  select PK_ID from ArchiveIDList where Item='{2}' 
                                 )";
                    sql = string.Format(sql, tbName, pkName, item);
                    DataTable dt = SQLTool.GetDataByDataTable(OpenCopyCon(), sql);
                    dt.TableName = tbName;
                    if (BulkCopy(dt, sql, dr["ViewName"].ToString().Trim()))
                    {
                        if (isXML && dt.Rows.Count > 0)
                        { ExportXML(dt); }
                        DeleteDataRetry(sql, tbName);
                        string sql2 = "Update ArchiveIDList set Status='OK' where Item='{0}'";
                        sql2 = string.Format(sql2, item);
                        SQLTool.ExcuteSQL(OpenDeleteCon(), sql2);

                        //if (DeleteDataRetry(sql, tbName))
                        //{
                        //    string sql2 = "Update ArchiveIDList set Status='OK' where Item='{0}'";
                        //    sql2 = string.Format(sql2, item);
                        //    SQLTool.ExcuteSQL(OpenDeleteCon(), sql2);

                        //}

                    }


                }
                TimeSpan copyTime = DateTime.Now - startTime;
                int cost = copyTime.Minutes * 60 + copyTime.Seconds;
                InsertArchiveLog("MoveMainData", cost.ToString(), LogType.Cost, _timeStamp);
            }
            catch (Exception exp)
            {
                InsertArchiveLog("MoveMainData", exp.Message, LogType.Error, _timeStamp);
                _mail.Send(_mailErrTitle + "MoveMainData", "Error " + exp.Message);
                _isHaveError = true;
                throw new Exception("MoveMainData error, message :" + exp.Message);
            }

        }
        private bool GetSubTableMoveResult(string item)
        {
            bool isAllcc = false;
            string cmd = @" select COUNT(*) from ArchiveTableLog where 
		                         Item='{0}' and  (ISNULL(IsDeleteSuccess,'')<>'Y' or ISNULL(IsCopySuccess,'')<>'Y')
		                         and TimeStamp='{1}'";

            cmd = string.Format(cmd, item, _timeStamp);
            SqlCommand sqlCmd = new SqlCommand(cmd, OpenDeleteCon());
            if (sqlCmd.ExecuteScalar().ToString() == "0") isAllcc = true;
            return isAllcc;
        }


        public void MoveSubData()
        {
            try
            {

                DateTime startTime = DateTime.Now;
                string sql = "";
                string pkName;
                string tbName;
                string fkName;
                string mainTbName;
                string item = "";
                bool isXML;
                foreach (DataRow dr in _dtArchiveMainTable.Rows)
                {
                    DataRow[] drArry = _dtArchiveTableList.Select("Item='" + dr["Item"].ToString().Trim() + "' and Enable='Y' ", "DeleteOrder");
                    pkName = dr["PKName"].ToString().Trim();
                    mainTbName = dr["TableName"].ToString().Trim();
                    item = dr["Item"].ToString().Trim();
                    foreach (DataRow dr2 in drArry)
                    {
                        isXML = dr2["IsNeedXML"].ToString().Trim().Trim().Equals("Y");
                        if (!string.IsNullOrEmpty(dr2["SQLScript"].ToString().Trim()))
                        {
                            sql = dr2["SQLScript"].ToString().Trim();
                        }
                        else
                        {
                            sql = SetCopyScript(dr2, pkName, mainTbName);
                        }
                        tbName = dr2["TableName"].ToString().Trim();
                        DataTable dt = SQLTool.GetDataByDataTable(OpenCopyCon(), sql);
                        dt.TableName = tbName;
                        if (BulkCopy(dt, sql, dr2["ViewName"].ToString().Trim()))
                        {

                            if (isXML && dt.Rows.Count > 0)
                            { ExportXML(dt); }
                            fkName = dr2["FKName"].ToString().Trim();
                            if (!string.IsNullOrEmpty(fkName))
                            { 
                                MoveFKTable(dr2["TableName"].ToString().Trim(), fkName, sql);
                             //   DeleteDataRetry(sql, tbName); 
                            }
                            DeleteDataRetry(sql, tbName);
                            //else
                            //{ DeleteDataRetry(sql, tbName); }
                        }
                        else
                        {
                            return;
                        }

                    }
                }
                TimeSpan copyTime = DateTime.Now - startTime;
                int cost = copyTime.Minutes * 60 + copyTime.Seconds;
                InsertArchiveLog("MoveSubData", cost.ToString(), LogType.Cost, _timeStamp);

            }
            catch (Exception exp)
            {
                InsertArchiveLog("MoveSubData", exp.Message, LogType.Error, _timeStamp);
                _mail.Send(_mailErrTitle + "MoveSubData", "Error " + exp.Message);
                _isHaveError = true;
                throw new Exception("MoveSubData error message :" + exp.Message);

            }

        }
        private void CheckDiffCount(int delCount, string tbName, string selSql)
        {
            if (_checkMinDiffCount == 0 || delCount == 0) { return; }
            string cmd = "  SELECT BeforeCount from ArchiveTableLog where TimeStamp='{0}'   and TableName='{1}'";
            cmd = string.Format(cmd, _timeStamp, tbName);
            int selCount = int.Parse(SQLTool.ExecuteScalar(OpenDeleteCon(), cmd).ToString());
            string body = "";
            if ((selCount - delCount) < _checkMinDiffCount)
            {
                body = @"The script " + selSql + " <BR> will delete " + delCount.ToString() + " rows ," + "  the count rows of source table  is " +
                                selCount.ToString();
                _mail.Send("Warning!! Archive " + _dbName + " DB stop!", body);
                throw new Exception(body);

            }


        }

        private bool BulkCopy(DataTable dt, string script, string viewName)
        {
            CheckDiffCount(dt.Rows.Count, dt.TableName, script);
            Console.WriteLine("Begin to copy " + dt.TableName + "  data..");
            bool isSucc = true;
            Tool.BeginCountdown();
            int cost;
            int rowCount = 0;
            // DataTable dt = SQLTool.GetDataByDataTable(OpenSrcCon(), sql);
            // dt.TableName = tbName;
            rowCount = dt.Rows.Count;

            try
            {
                // SqlBulkCopy bulkCopy = new SqlBulkCopy(OpenArcCon());
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(OpenArcCon()))
                {
                    bulkCopy.BulkCopyTimeout = 1800;
                    if (String.IsNullOrEmpty(viewName))
                    { bulkCopy.DestinationTableName = dt.TableName.Replace("[", "").Replace("]", ""); }
                    else
                    { bulkCopy.DestinationTableName = viewName; }

                    bulkCopy.WriteToServer(dt);
                    int y = dt.Rows.Count;
                    /*
                      bulkCopy.DestinationTableName = "[dbo].[PAKPaltno]";
                      dt.TableName = dt.TableName.Replace("[", "").Replace("]", "");
                      dt.TableName = "dbo.[PAK.PAKPaltno]";
                      bulkCopy.DestinationTableName = dt.TableName;
                      bulkCopy.DestinationTableName = "[PAK.PAKPaltno]";
                      bulkCopy.DestinationTableName = "[HPEDI_TEST]..[PAK.PAKPaltno]";
                      bulkCopy.DestinationTableName = "[10.99.183.65].[HPEDI_TEST].dbo.[PAK.PAKPaltno]";
                      bulkCopy.DestinationTableName = "dbo.[PAK.PAKPaltno]";
                      bulkCopy.DestinationTableName = "[HPEDI_TEST].[dbo].[PAK.PAKPaltno]";
                     */


                }

                cost = Tool.EndCountdown();
                Console.WriteLine("Finish to copy " + rowCount.ToString() + " rows " + dt.TableName + " data, it takes " + cost.ToString() + " sec");
                _infoLog.WriteLog("It takes " + cost + " to move " + rowCount.ToString() + " rows " + dt.TableName + " data ");
                UpdateArchiveTableLog(dt.TableName, dt.Rows.Count, "Y", "", true, cost, 0, ""); //AAA


            }
            catch (Exception e)
            {
                isSucc = false;
                _isHaveError = true;
                string t = " [BulkCopy] Error,Table: " + dt.TableName + " Time Stamp :" + _timeStamp;
                _errorLog.WriteLog(t + "\r\n" + "Error Message:" + e.Message);

                _mail.Send(_mailErrTitle + t, "Error Message:" + e.Message);

                Console.WriteLine(t + "\r\n" + "Error Message:" + e.Message);
                //_infoLog.WriteLog("It takes " + strTime + " to move " + rowCount.ToString() + " rows " + dt.TableName + " data ");
                UpdateArchiveTableLog(dt.TableName, dt.Rows.Count, "N", e.Message, true, 0, 0, "");

            }
            return isSucc;
        }

        private void ExportXML(DataTable dt)
        {
            if (!Directory.Exists(_xmlPath))
            { Directory.CreateDirectory(_xmlPath); }
            if (!Directory.Exists(_xmlPath + _timeStamp))
            { Directory.CreateDirectory(_xmlPath + _timeStamp); }
            string path = _xmlPath + _timeStamp + "\\";
            string xmlFileName = path + dt.TableName + ".xml";
            //    Tool.BeginCountdown();
            dt.WriteXml(xmlFileName);
            //  InsertArchiveLog("Cost-ExportXML("+dt.TableName+")", Tool.EndCountdown().ToString(), _timeStamp);

        }

        private void ExportTxt(DataTable dt)
        {
            if (!Directory.Exists(_xmlPath))
            { Directory.CreateDirectory(_xmlPath); }
            if (!Directory.Exists(_xmlPath + _timeStamp))
            { Directory.CreateDirectory(_xmlPath + _timeStamp); }
            string filePath = _xmlPath + _timeStamp + "\\" + dt.TableName + ".txt";
            StreamWriter sw;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            string txt = "";

            sw = File.CreateText(filePath);
            foreach (DataRow row in dt.Rows)
            {
                txt = txt + row["PK_ID"].ToString() + "\r\n";
            }
            sw.Write(txt);
            sw.Close();
        }
        public bool SetNewstTimeStamp(int interval)
        {
            string sql = @" select top 1 * from ArchiveLog
                                 where LogName='Prepare Archive' and LogValue='Y'  and Cdt> DATEAdd(MI,-{0},GETDATE())
                                 order by Cdt desc";
            sql = string.Format(sql, interval);
            DataTable dt = SQLTool.GetDataByDataTable(OpenDeleteCon(), sql);
            if (dt.Rows.Count == 0 || dt.Rows[0]["LogValue"].ToString() != "Y")
            { return false; }
            else
            { _timeStamp = dt.Rows[0]["TimeStamp"].ToString(); return true; }


        }

        public bool InitiaIDListTable()
        {
            bool isSucess = true;
            string sql = "";
            try
            {
                _infoLog.WriteLog("[InitiaIDListTable]  Begin to Initial ID List Table data ");
                Tool.BeginCountdown();
                //  SqlCommand sqlCmd = new SqlCommand("TRUNCATE  table ArchiveIDList", OpenDeleteCon());
                //   SqlCommand sqlCmd = new SqlCommand("DELETE ArchiveIDList", OpenDeleteCon());
                SqlCommand sqlCmd = new SqlCommand("DELETE ArchiveIDList   where Status='OK'", OpenDeleteCon());

                sqlCmd.ExecuteNonQuery();
                DateTime starttime = DateTime.Now;
                Console.WriteLine("Begin to Initial ID List Table data...");
                int arcD = GetArcDay();
                foreach (DataRow dr in _dtArchiveMainTable.Rows)
                {
                    GetIDList(dr["SQLScript"].ToString(), dr["PKName"].ToString().Trim(), dr["Item"].ToString().Trim(), arcD, out sql);
                }

                string cost = Tool.EndCountdown().ToString();
                //Check DN ship date
                //                string cmd2 = @"    select DATEDIFF(DAY,Max(ShipDate),GETDATE())
                //                                           from Delivery a,ArchiveIDList b where
                //                                           a.DeliveryNo=b.PK_ID
                //                                           and b.Item='Delivery' ";
                //   DataTable dt2 = SQLTool.GetDataByDataTable(OpenDeleteCon(), cmd2);

                string err = _objDBSetting.CheckDaySetting(OpenDeleteCon());
                if (err != "")
                {
                    _errorLog.WriteLog("[InitiaIDListTable Erro in " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + err);
                    InsertArchiveLog("InitiaIDListTable", err, LogType.Error, _timeStamp);
                    InsertArchiveLog("End Archive", "Fail", LogType.Error, _timeStamp);
                    _mail.Send("Warning!! Archive HB DB fail, Time Stamp:" + _timeStamp, "InitiaIDListTable fail, error" + "\r\n" + err);
                    return false;
                }
                Console.WriteLine("Finish to Initial ID List Table data, it takes " + cost);
                _infoLog.WriteLog("[InitiaIDListTable]  Finsih to Initial ID List Table, it takes " + cost);
                InsertArchiveLog("InitiaIDListTable", cost, LogType.Cost, _timeStamp);
                InsertArchiveLog("Prepare Archive", "Y", LogType.Event, _timeStamp);
            }
            catch (Exception exp)
            {
                isSucess = false;
                Console.WriteLine(exp.Message);
                _errorLog.WriteLog("[InitiaIDListTable Erro in " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + exp.Message);
                _errorLog.WriteLog(" SQL : " + "\r\n" + sql);
                InsertArchiveLog("InitiaIDListTable", exp.Message + ", SQL :" + sql, LogType.Error, _timeStamp);
                InsertArchiveLog("End Archive", "Fail", LogType.Error, _timeStamp);
                _mail.Send("Warning!! Archive HB DB fail, Time Stamp:" + _timeStamp, "InitiaIDListTable fail, error" + "\r\n" + exp.Message);

            }
            return isSucess;
        }

        public void EndArchive(string cost)
        {

            InsertArchiveLog("Total Archive time", cost, LogType.Cost, _timeStamp);
            if (_isHaveError)
            { InsertArchiveLog("End Archive", "Have Error", LogType.Event, _timeStamp); }
            else
            { InsertArchiveLog("End Archive", "Success", LogType.Event, _timeStamp); }
            DeleteLog();
            ClosAllCon();
        }
        private int GetArcDay()
        {
            int arDay = _archiveDay;
            if (_continuteDeleteDay > 0)
            {
                int d = _objDBSetting.GetDayDiffFromMinShipDay(OpenDeleteCon());
                if (d > _archiveDay && (d - _archiveDay) > _continuteDeleteDay)
                { arDay = d - _continuteDeleteDay; }
            }
            return arDay;
        }
        private void GetIDList(string sql, string pkName, string item, int arcDay, out string convertSql)
        {

            string cmd = Tool.CovertInsertMainItemSQL(sql, item, _timeStamp);
            convertSql = cmd;
            Console.WriteLine("Begin bulkCopy " + pkName);

            SqlCommand sqlCmd = new SqlCommand(cmd, OpenDeleteCon());
            SqlParameter paraDay = new SqlParameter("@day", SqlDbType.Int, 32);
            paraDay.Direction = ParameterDirection.Input;
            // paraDay.Value = _archiveDay * -1;
            paraDay.Value = arcDay * -1;

            sqlCmd.Parameters.Add(paraDay);
            SqlDataAdapter sqlDap = new SqlDataAdapter(sqlCmd);
            DataSet ds = new DataSet();
            DataTable dt;
            sqlDap.Fill(ds);
            dt = ds.Tables[0];
            dt.TableName = "ArchiveIDList";

            DataTable dtNew = dt.Copy();
            dtNew.TableName = item;

            dsArchiveID.Tables.Add(dtNew);

            SqlBulkCopy bulkCopy = new SqlBulkCopy(OpenDeleteCon());
            bulkCopy.BulkCopyTimeout = 0;
            bulkCopy.DestinationTableName = dt.TableName;
            bulkCopy.ColumnMappings.Add("PK_ID", "PK_ID");
            bulkCopy.ColumnMappings.Add("TimeStamp", "TimeStamp");
            bulkCopy.ColumnMappings.Add("Item", "Item");
            bulkCopy.ColumnMappings.Add("Cdt", "Cdt");
            bulkCopy.SqlRowsCopied += new SqlRowsCopiedEventHandler(OnRowsCopied);
            bulkCopy.NotifyAfter = dt.Rows.Count;
            bulkCopy.WriteToServer(dt);
            Console.WriteLine("End bulkCopy " + pkName);

        }

        private void CheckSqlStatus()
        {
            if (_copySqlCon.State == ConnectionState.Open)
            { _copySqlCon.Close(); }
            if (_arcSqlCon.State == ConnectionState.Open)
            { _arcSqlCon.Close(); }
        }

        private void OnRowsCopied(object sender, SqlRowsCopiedEventArgs args)
        {

            //TimeSpan copyTime = DateTime.Now - startTime;
            //Console.WriteLine("Move data successfully. It takes " + copyTime.Seconds.ToString() + "." + copyTime.Milliseconds.ToString() + " seconds" + " to move data");
        }
        public void AfterArchiveTableRowCount(string filepath)
        {

            //       infoLog.WriteLog("[AfterArchiveTableRowCount]  Begin to count rows in all table after archive ");
            DateTime startTime = DateTime.Now;

            string cmd = @"CREATE TABLE #temp (TableName VARCHAR (255), RowCnt INT) 
                                    EXEC sp_MSforeachtable 'INSERT INTO #temp SELECT ''?'', COUNT(*) FROM ?' 
                                    SELECT Replace(Replace(TableName,'[dbo].[',''),']','') as 
                                    TableName, RowCnt FROM #temp ORDER BY RowCnt desc 
                                   DROP TABLE #temp ";
            _copySqlCon.Open();
            SqlCommand sqlCmd = new SqlCommand(cmd, OpenCopyCon());

            SqlDataAdapter sqlDap = new SqlDataAdapter(sqlCmd);
            DataSet ds = new DataSet();
            DataTable dtSrc;
            sqlDap.Fill(ds);
            dtSrc = ds.Tables[0];
            foreach (DataRow dr in tableRowCount.Rows)
            {
                DataRow[] drFind = dtSrc.Select("TableName='" + dr[0].ToString() + "'");
                foreach (DataRow dr2 in drFind)
                { dr[2] = dr2[1].ToString(); }

            }

            string aLine, aParagraph = null;
            foreach (DataRow dr in tableRowCount.Rows)
            {

                aLine = dr[0].ToString() + "," + dr[1].ToString() + "," + dr[2].ToString() + "\r\n";
                aParagraph = aParagraph + aLine;
            }
            _copySqlCon.Close();
            if (File.Exists(filepath))
                File.Delete(filepath);
            StreamWriter sw;
            sw = File.CreateText(filepath);
            sw.WriteLine(aParagraph);
            sw.Close();

            //infoLog.WriteLog("[AfterArchiveTableRowCount]  Finish to count rows in all table after archive");
            //TimeSpan copyTime = DateTime.Now - startTime;
            //string strTime = copyTime.Minutes.ToString() + "." + copyTime.Seconds.ToString() + "." + copyTime.Milliseconds.ToString();
            //infoLog.WriteLog("[AfterArchiveTableRowCount]  Finish to count rows in all table after archive, it takes " + strTime);
        }
        public void InsertArchiveLog(string logName, string logValue, LogType logType, string timeStamp)
        {
            if (timeStamp == null) timeStamp = _timeStamp;
            string cmdTxt;
            cmdTxt = @"Insert ArchiveLog(LogName,LogValue,LogType,TimeStamp,Cdt) 
                                    Values(@logName,@logValue,@logType,@timeStamp,GETDATE())";

            SqlParameter p1 = new SqlParameter("@logName", logName);
            SqlParameter p2 = new SqlParameter("@logValue", logValue);
            SqlParameter p3 = new SqlParameter("@logType", logType.ToString());
            SqlParameter p4 = new SqlParameter("@timeStamp", timeStamp);
            SqlCommand sqlCmd = new SqlCommand(cmdTxt, OpenDeleteCon());
            sqlCmd.Parameters.Add(p1);
            sqlCmd.Parameters.Add(p2);
            sqlCmd.Parameters.Add(p3);
            sqlCmd.Parameters.Add(p4);
            sqlCmd.ExecuteNonQuery();
        }


        private void UpdateArchiveTableLog(string tableName, int count, string isSuccess, string msg, bool isCopyFlag, int copyTime, int deleteTime, string deleteSQL)
        {
            string cmdTxt;
            if (isCopyFlag)
            { cmdTxt = "Update ArchiveTableLog set ArchiveCount=@count,IsCopySuccess=@isSuccess,Msg=@msg,CopyTime=@copyTime , Udt=GETDATE() where TimeStamp=@dateStamp and TableName=@tableName   "; }
            else
            {
                cmdTxt = @"Update ArchiveTableLog set IsDeleteSuccess=@isSuccess,Msg=@msg,DeleteTime=@deleteTime,Udt=GETDATE() ,DeleteSQL=@deleteSQL
                                 where TimeStamp=@dateStamp and TableName=@tableName   ";
            }

            SqlParameter p1 = new SqlParameter("@count", count);
            SqlParameter p2 = new SqlParameter("@isSuccess", isSuccess);
            SqlParameter p3 = new SqlParameter("@msg", msg);
            SqlParameter p4 = new SqlParameter("@dateStamp", _timeStamp);
            SqlParameter p5 = new SqlParameter("@tableName", tableName);
            SqlParameter p6 = new SqlParameter("@copyTime", copyTime);
            SqlParameter p7 = new SqlParameter("@deleteTime", deleteTime);
            SqlParameter p8 = new SqlParameter("@deleteSQL", deleteSQL);



            SqlCommand sqlCmd = new SqlCommand(cmdTxt, OpenDeleteCon());
            sqlCmd.Parameters.Add(p1);
            sqlCmd.Parameters.Add(p2);
            sqlCmd.Parameters.Add(p3);
            sqlCmd.Parameters.Add(p4);
            sqlCmd.Parameters.Add(p5);
            sqlCmd.Parameters.Add(p6);
            sqlCmd.Parameters.Add(p7);
            sqlCmd.Parameters.Add(p8);

            sqlCmd.ExecuteNonQuery();
        }





        public void BeforeArchiveTableRowCount()
        {
            Console.WriteLine("Begin to count rows in all table before archive..");
            Tool.BeginCountdown();
            try
            {
                //  EXEC sp_MSforeachtable 'INSERT INTO #temp SELECT ''?'', COUNT(*) FROM ?'  collate Chinese_Taiwan_Stroke_BIN
                string cmd = @"CREATE TABLE #temp (TableName VARCHAR (255)  collate Chinese_Taiwan_Stroke_BIN , RowCnt INT) 
                                     INSERT INTO #temp
                                     select  b.name as tablename ,   a.rowcnt as datacount  from    sysindexes a ,  
                                    sysobjects b where   a.id = b.id   and a.indid < 2   and objectproperty(b.id, 'IsMSShipped') = 0   
                                    and  b.name in(
                                     Select TableName from ArchiveTableList
                                     union
                                     Select TableName from ArchiveFKTableList
                                    union
                                     Select TableName from ArchiveMainTable)
                                    Insert into ArchiveTableLog(TableName,BeforeCount,TimeStamp,Cdt)    
                                    SELECT Replace(Replace(TableName,'[dbo].[',''),']','') as TableName, RowCnt,'{0}',GETDATE() FROM #temp 
                                     WHERE Replace(Replace(TableName,'[dbo].[',''),']','') in (
                                    Select TableName from ArchiveTableList
                                     union
                                     Select TableName from ArchiveFKTableList
                                    union
                                     Select TableName from ArchiveMainTable
                                    )
                                    ORDER BY RowCnt desc 
                                    DROP TABLE #temp      ; ";

                if (_dbName == "HPEDI")
                {
                    cmd = @"CREATE TABLE #temp (TableName VARCHAR (255) , RowCnt INT) 
                                    EXEC sp_MSforeachtable 'INSERT INTO #temp SELECT ''?'', COUNT(*) FROM ?' 
                                    Insert into ArchiveTableLog(TableName,BeforeCount,TimeStamp,Cdt)    
                                    SELECT Replace(TableName,'[dbo].','') as TableName, RowCnt,'{0}',GETDATE() FROM #temp 
                                     WHERE Replace(TableName,'[dbo].','') in (
                                    Select TableName from ArchiveTableList
                                     union
                                     Select TableName from ArchiveFKTableList
                                    union
                                     Select TableName from ArchiveMainTable
                                    )
                                    ORDER BY RowCnt desc 
                                    DROP TABLE #temp      ; ";

                }

                cmd = "Delete ArchiveTableLog where  TimeStamp='{0}' ; " + cmd;
                string tmp = @"  update ArchiveTableLog set Item=b.Item from   ArchiveTableLog a
                      inner join ArchiveTableList b on a.TableName=b.TableName  
                      where  TimeStamp='{0}' ";
                cmd = cmd + tmp;

                cmd = string.Format(cmd, _timeStamp);
                SqlCommand sqlCmd = new SqlCommand(cmd, OpenDeleteCon());
                sqlCmd.CommandTimeout = 30000;
                sqlCmd.ExecuteNonQuery();
                Console.WriteLine("Finish to count rows in all table before archive, it takes " + Tool.EndCountdown().ToString());
                //  int cost=copyTime*60+
                InsertArchiveLog("BeforeArchiveTableRowCount", Tool.EndCountdown().ToString(), LogType.Cost, _timeStamp);
            }
            catch (Exception exp)
            {
                //       InsertArchiveLog("BeforeArchiveTableRowCount", exp.Message, LogType.Error, _timeStamp);
                //_mail.Send(_mailErrTitle + "BeforeArchiveTableRowCount", "Error :\r\n" + exp.Message);
                _isHaveError = true;
                ClosAllCon();
                throw new Exception("BeforeArchiveTableRowCount error:" + " \r\n" + exp.Message);
            }
        }

        private void InitTableRowCount()
        {

            tableRowCount = new DataTable();
            tableRowCount.Columns.Add("TableName", Type.GetType("System.String"));
            tableRowCount.Columns.Add("RowCnt_Before", Type.GetType("System.String"));
            tableRowCount.Columns.Add("RowCnt_After", Type.GetType("System.String"));
            string allTable = Product.TableList + "," + PCB.TableList + "," + DN.TableList + "," + Other.TableList + "," + Relation.TableList;
            string[] tableArray = allTable.Split(',');

            foreach (string tb in tableArray)
            {
                DataRow dr;
                dr = tableRowCount.NewRow();
                dr[0] = tb;
                tableRowCount.Rows.Add(dr);
            }

        }
        public bool IsHaveError
        {
            set { _isHaveError = value; }
            get { return _isHaveError; }
        }
        public void IniArchiveAllFKConstraint()
        {
            string cmd = @"DELETE ArchiveAllFKConstraint
                                Insert into ArchiveAllFKConstraint
                                SELECT f.name AS ForeignKey,
                                OBJECT_NAME(f.parent_object_id) AS TableName,
                                COL_NAME(fc.parent_object_id,
                                fc.parent_column_id) AS ColumnName,
                                OBJECT_NAME (f.referenced_object_id) AS ReferenceTableName,
                                COL_NAME(fc.referenced_object_id,
                                fc.referenced_column_id) AS ReferenceColumnName 
                                FROM sys.foreign_keys AS f
                                INNER JOIN sys.foreign_key_columns AS fc
                                ON f.OBJECT_ID = fc.constraint_object_id ";

            SqlCommand sqlCmd = new SqlCommand(cmd, OpenDeleteCon());
            sqlCmd.CommandTimeout = 300;
            sqlCmd.ExecuteNonQuery();
        }

        private void DeleteLog()
        {
            string cmd = @"select distinct TimeStamp into #ts from ArchiveTableLog 
                                     WHERE Cdt<DateAdd(d,@day, GETDATE())
                                    DELETE ArchiveTableLog where TimeStamp in
                                    (
                                    select  TimeStamp from #ts
                                    )
                                    DELETE ArchiveLog where TimeStamp in
                                    (
                                    select  TimeStamp from #ts
                                    )
                                    Drop TABLE #ts ";
            SqlCommand sqlCmd = new SqlCommand(cmd, OpenDeleteCon());
            SqlParameter paraDay = new SqlParameter("@day", SqlDbType.Int, 32);
            paraDay.Direction = ParameterDirection.Input;
            paraDay.Value = _logDay * -1;
            sqlCmd.Parameters.Add(paraDay);
            sqlCmd.ExecuteNonQuery();
        }
    }
}
