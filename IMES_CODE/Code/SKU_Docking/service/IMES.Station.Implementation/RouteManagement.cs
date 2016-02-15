/*
 * Description: Route Management Implementation
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-03-19   205033            Create
 * Known issues:
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
using IMES.Infrastructure.Repository._Schema;
using IMES.Infrastructure.UnitOfWork;
using System.Configuration;

namespace IMES.Route
{
    class RouteManagementImpl : MarshalByRefObject, IRouteManagement
    {
    	private string _workflowPathKey = "ModelWorkflowDirectory";
        private string _workflowPath {
        	get {
        		return ConfigurationManager.AppSettings.Get(_workflowPathKey);
        	}
		}
        private string _connString = SqlHelper.ConnectionString_GetData;
        private string _sqlGetRouteIdList = "SELECT Process FROM Process WHERE Type = @RouteType";
        private string _sqlGetRoute = "SELECT Process AS 'Name', Type AS 'Type', Descr AS 'Description' FROM Process WHERE Process = @RouteId";
        private string _sqlGetRouteSteps = "SELECT Status AS 'Condition', PreStation AS 'FromStation', Station AS 'ToStation' FROM Process_Station WHERE Process = @RouteId";
        private string _sqlDeleteRoute = "DELETE FROM ProcessAttr WHERE Process = @RouteId;DELETE FROM Process_Station WHERE Process = @RouteId;DELETE FROM Process WHERE Process = @RouteId";
        private string _sqlDeleteRouteSteps = "DELETE FROM Process_Station WHERE Process = @RouteId";
        private string _sqlUpdateRoute = "UPDATE Process SET Process = @Name, Type = @Type, Descr = @Description, Editor = @Editor, Udt = GETDATE() WHERE Process = @RouteId";
        private string _sqlInsertRoute = "INSERT INTO Process(Process, Type, Descr, Editor, Cdt, Udt) VALUES(@Name, @Type, @Description, @Editor, GETDATE(), GETDATE()); SELECT @Name AS 'RouteId'";
        private string _sqlInsertRouteSteps = "INSERT INTO Process_Station(Process, Status, PreStation, Station, Editor, Cdt, Udt) VALUES(@RouteId, @Condition, @FromStation, @ToStation, @Editor, GETDATE(), GETDATE())";

        private string _sqlReadRouteAttr = "SELECT AttrValue FROM ProcessAttr WHERE Process = @RouteId AND AttrName = @AttrName";
        private string _sqlInsertRouteAttr = "INSERT ProcessAttr(Process, AttrName, AttrValue, Descr, Editor, Cdt, Udt) VALUES(@RouteId, @AttrName, @AttrValue, '', @Editor, GETDATE(), GETDATE())";
        private string _sqlUpdateRouteAttr = "UPDATE ProcessAttr SET AttrValue = @AttrValue, Editor = @Editor, Udt = GETDATE() WHERE Process = @RouteId AND AttrName = @AttrName";
        private string _sqlDeleteRouteAttr = "DELETE FROM ProcessAttr WHERE Process = @RouteId AND AttrName = @AttrName";

        private string _sqlStationNameToId = "SELECT Station AS 'StationId' FROM Station WHERE Name = @Name";
        private string _sqlStationIdToName = "SELECT Name FROM Station WHERE Station = @StationId";

        private string _sqlReadStationAttr = "SELECT AttrValue FROM StationAttr WHERE Station = @StationId AND AttrName = @AttrName";
        private string _sqlInsertStationAttr = "INSERT StationAttr(Station, AttrName, AttrValue, Descr, Editor, Cdt, Udt) VALUES(@StationId, @AttrName, @AttrValue, '', @Editor, GETDATE(), GETDATE())";
        private string _sqlUpdateStationAttr = "UPDATE StationAttr SET AttrValue = @AttrValue, Editor = @Editor, Udt = GETDATE() WHERE Station = @StationId AND AttrName = @AttrName";
        private string _sqlDeleteStationAttr = "DELETE FROM StationAttr WHERE Station = @StationId AND AttrName = @AttrName";

        private string _sqlGetStationNameList = "SELECT DISTINCT Name AS 'StationName' FROM Station";

        /// <summary>
        /// SqlTransactionManager Status Backup
        /// </summary>
        private object _inScopeTag = null;
        private object _conn = null;
        private object _trans = null;
        private object _dbCataLog = null;
        private object _embeded = null;
        private object _errOccured = null;

        private bool _inTransaction = false;

        protected object ConvertStationNameToId(string name)
        {
            if (name == null || name == "")
                return DBNull.Value;

            object obj = (string)SqlHelper.ExecuteScalar(_connString,
                CommandType.Text, _sqlStationNameToId,
                new SqlParameter[] { new SqlParameter("@Name", name) });

            if (obj == null)
                throw new Exception("Invalid station name: " + name);

            return obj;
        }

        internal string ConvertStationIdToName(string stationId)
        {
            if (stationId == null || stationId == "")
                throw new Exception("Invalid station ID: null");

            string obj = (string)SqlHelper.ExecuteScalar(_connString,
                CommandType.Text, _sqlStationIdToName,
                new SqlParameter[] { new SqlParameter("@StationId", stationId) });

            if (obj == null)
                throw new Exception("Invalid station ID: " + stationId);

            return obj;
        }

        #region IRouteManagement 成员

        public IList<string> GetRouteIdList(string routeType)
        {
        	_workflowPathKey = routeType + "WorkflowDirectory";

            try
            {
                RestoreSqlConnectionManager();

                DataTable dt = SqlHelper.ExecuteDataFill(_connString,
                    CommandType.Text, _sqlGetRouteIdList,
                    new SqlParameter[] {
                        new SqlParameter("@RouteType", routeType)
                    });
                List<string> list = new List<string>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add((string)dr[0]);
                }

                return list;
            }
            finally
            {
                BackupSqlConnectionManager();
            }
        }

        public RouteInfo GetRoute(string routeId)
        {
            try
            {
                RestoreSqlConnectionManager();

                RouteInfo ri = new RouteInfo();
                SqlParameter[] param = new SqlParameter[] {
                    new SqlParameter("@RouteId", routeId)
                };

                // 1. Get route steps
                DataTable dt = SqlHelper.ExecuteDataFill(_connString,
                    CommandType.Text, _sqlGetRouteSteps, param);
                ri.Steps = new List<StepInfo>(dt.Rows.Count);
                foreach (DataRow r in dt.Rows)
                {
                    StepInfo si = new StepInfo();
                    si.Condition = (int)r["Condition"] == 1 ? "PASS" : "FAIL";
                    si.FromStation = r["FromStation"] as string;
                    si.ToStation = r["ToStation"] as string;

                    ri.Steps.Add(si);
                }

                // 2. Get route info
                dt = SqlHelper.ExecuteDataFill(_connString,
                    CommandType.Text, _sqlGetRoute, param);
                DataRow dr = dt.Rows[0];
                ri.RouteId = routeId;
                ri.Name = dr["Name"] as string;
                ri.Type = dr["Type"] as string;
                ri.Description = dr["Description"] as string;

                return ri;
            }
            finally
            {
                BackupSqlConnectionManager();
            }
        }

        public void DeleteRoute(string routeId, string editor)
        {
            try
            {
                RestoreSqlConnectionManager();

                SqlParameter[] param = new SqlParameter[] {
                    new SqlParameter("@RouteId", routeId)
                };
                
                SqlHelper.ExecuteNonQuery(_connString,
                    CommandType.Text, _sqlDeleteRoute, param);

                return;
            }
            finally
            {
                BackupSqlConnectionManager();
            }
        }

        public string SaveRoute(RouteInfo ri, string editor)
        {
            try
            {
                RestoreSqlConnectionManager();

                if (!_inTransaction)
                    SqlTransactionManager.Begin();

                // 1. Save route info
                if (ri.RouteId == null || ri.RouteId == "")
                {
                    // Insert a new route
                    ri.RouteId = (string)SqlHelper.ExecuteScalar(_connString,
                        CommandType.Text, _sqlInsertRoute,
                        new SqlParameter[] {
                            new SqlParameter("@Name", ri.Name),
                            new SqlParameter("@Type", ri.Type),
                            new SqlParameter("@Description", ri.Description),
                            new SqlParameter("@Editor", editor)
                        });
                }
                else
                {
                    // Update an existing route

                    // Delete route steps
                    SqlHelper.ExecuteNonQuery(_connString, CommandType.Text, _sqlDeleteRouteSteps,
                        new SqlParameter[] {
                            new SqlParameter("@RouteId", ri.RouteId)
                        });

                    // Update route info
                    SqlHelper.ExecuteNonQuery(_connString, CommandType.Text, _sqlUpdateRoute,
                        new SqlParameter[] {
                            new SqlParameter("@Name", ri.Name),
                            new SqlParameter("@Type", ri.Type),
                            new SqlParameter("@Description", ri.Description),
                            new SqlParameter("@Editor", editor),
                            new SqlParameter("@RouteId", ri.RouteId)
                        });
                }

                // 2. Save route steps
                foreach (var si in ri.Steps)
                {
                    SqlHelper.ExecuteNonQuery(_connString, CommandType.Text, _sqlInsertRouteSteps,
                        new SqlParameter[] {
                            new SqlParameter("@RouteId", ri.RouteId),
                            new SqlParameter("@Condition", si.Condition.ToUpper() == "PASS" ? 1 : 0),
                            new SqlParameter("@FromStation", ConvertStationNameToId(si.FromStation)),
                            new SqlParameter("@ToStation", ConvertStationNameToId(si.ToStation)),
                            new SqlParameter("@Editor", editor)
                        });
                }

                if (!_inTransaction)
                    SqlTransactionManager.Commit();

                return ri.RouteId;
            }
            catch (Exception ex)
            {
                if (!_inTransaction)
                    SqlTransactionManager.Rollback();

                throw ex;
            }
            finally
            {
                if (!_inTransaction)
                {
                    SqlTransactionManager.Dispose();
                    SqlTransactionManager.End();
                }

                BackupSqlConnectionManager();
            }
        }

        public string ReadRouteAttr(string routeId, string attrName)
        {
            try
            {
                RestoreSqlConnectionManager();

                return SqlHelper.ExecuteScalar(_connString,
                    CommandType.Text, _sqlReadRouteAttr,
                    new SqlParameter[] {
                        new SqlParameter("@RouteId", routeId),
                        new SqlParameter("@AttrName", attrName)
                    }) as string;
            }
            finally
            {
                BackupSqlConnectionManager();
            }
        }

        public void WriteRouteAttr(string routeId, string attrName, string attrValue, string editor)
        {
            try
            {
                RestoreSqlConnectionManager();

                if (!_inTransaction)
                    SqlTransactionManager.Begin();

                SqlParameter[] param = new SqlParameter[] {
                    new SqlParameter("@RouteId", routeId),
                    new SqlParameter("@AttrName", attrName),
                    new SqlParameter("@AttrValue", attrValue),
                    new SqlParameter("@Editor", editor)
                };

                int count = SqlHelper.ExecuteNonQuery(_connString,
                    CommandType.Text, _sqlUpdateRouteAttr, param);
                if (count <= 0)
                    count = SqlHelper.ExecuteNonQuery(_connString,
                        CommandType.Text, _sqlInsertRouteAttr, param);

                if (!_inTransaction)
                    SqlTransactionManager.Commit();
            }
            catch (Exception ex)
            {
                if (!_inTransaction)
                    SqlTransactionManager.Rollback();

                throw ex;
            }
            finally
            {
                if (!_inTransaction)
                {
                    SqlTransactionManager.Dispose();
                    SqlTransactionManager.End();
                }

                BackupSqlConnectionManager();
            }
        }

        public void DeleteRouteAttr(string routeId, string attrName, string editor)
        {
            try
            {
                RestoreSqlConnectionManager();

                SqlHelper.ExecuteNonQuery(_connString,
                    CommandType.Text, _sqlDeleteRouteAttr,
                    new SqlParameter[] {
                        new SqlParameter("@RouteId", routeId),
                        new SqlParameter("@AttrName", attrName)
                    });
            }
            finally
            {
                BackupSqlConnectionManager();
            }
        }

        public string ReadStationAttr(string stationId, string attrName)
        {
            try
            {
                RestoreSqlConnectionManager();

                return SqlHelper.ExecuteScalar(_connString,
                    CommandType.Text, _sqlReadStationAttr,
                    new SqlParameter[] {
                        new SqlParameter("@StationId", ConvertStationNameToId(stationId)),
                        new SqlParameter("@AttrName", attrName)
                    }) as string;
            }
            finally
            {
                BackupSqlConnectionManager();
            }
        }

        public void WriteStationAttr(string stationId, string attrName, string attrValue, string editor)
        {
            try
            {
                RestoreSqlConnectionManager();

                if (!_inTransaction)
                    SqlTransactionManager.Begin();

                SqlParameter[] param = new SqlParameter[] {
                    new SqlParameter("@StationId", ConvertStationNameToId(stationId)),
                    new SqlParameter("@AttrName", attrName),
                    new SqlParameter("@AttrValue", attrValue),
                    new SqlParameter("@Editor", editor)
                };

                int count = SqlHelper.ExecuteNonQuery(_connString,
                    CommandType.Text, _sqlUpdateStationAttr, param);
                if (count <= 0)
                    count = SqlHelper.ExecuteNonQuery(_connString,
                        CommandType.Text, _sqlInsertStationAttr, param);

                if (!_inTransaction)
                    SqlTransactionManager.Commit();
            }
            catch (Exception ex)
            {
                if (!_inTransaction)
                    SqlTransactionManager.Rollback();

                throw ex;
            }
            finally
            {
                if (!_inTransaction)
                {
                    SqlTransactionManager.Dispose();
                    SqlTransactionManager.End();
                }

                BackupSqlConnectionManager();
            }
        }

        public void DeleteStationAttr(string stationId, string attrName, string editor)
        {
            try
            {
                RestoreSqlConnectionManager();

                SqlHelper.ExecuteNonQuery(_connString,
                    CommandType.Text, _sqlDeleteStationAttr,
                    new SqlParameter[] {
                        new SqlParameter("@StationId", ConvertStationNameToId(stationId)),
                        new SqlParameter("@AttrName", attrName)
                    });
            }
            finally
            {
                BackupSqlConnectionManager();
            }
        }

        public IList<string> GetWorkflowIdList()
        {
            try
            {
                RestoreSqlConnectionManager();
                DirectoryInfo wp = new DirectoryInfo(_workflowPath);

                IList<string> list = new List<string>();
                foreach (FileInfo f in wp.GetFiles("*.xoml"))
                {
                	string[] names = f.Name.Split(new char[] {'.'});
                	
                    list.Add(string.Join(".", names, 0, names.Length-1));
				}
				
                return list;
            }
            finally
            {
                BackupSqlConnectionManager();
            }
        }

        public string ReadWorkflow(string workflowId, out string rulesContent)
        {
            try
            {
                RestoreSqlConnectionManager();
                DirectoryInfo di = new DirectoryInfo(_workflowPath);

                rulesContent = "";
                try
                {
                    rulesContent = File.ReadAllText(di.GetFiles(workflowId+".rules")[0].FullName);
                }
                catch (Exception)
                {
                    // ignore if workflow doesn't have a rule
                }

                try
                {
                    return File.ReadAllText(di.GetFiles(workflowId+".xoml")[0].FullName);
                }
                catch (Exception)
                {
                    throw new Exception("Failed to open workflow file.");
                }
            }
            finally
            {
                BackupSqlConnectionManager();
            }
        }

        public void WriteWorkflow(string workflowId, string workflowContent, string rulesContent)
        {
            try
            {
                RestoreSqlConnectionManager();
                DirectoryInfo di = new DirectoryInfo(_workflowPath);

                File.WriteAllText(di.FullName + "\\" + workflowId+".rules",
                    rulesContent, Encoding.Unicode);

                File.WriteAllText(di.FullName + "\\" + workflowId+".xoml",
                    workflowContent, Encoding.Unicode);
            }
            finally
            {
                BackupSqlConnectionManager();
            }
        }

        public void DeleteWorkflow(string workflowId)
        {
            try
            {
                RestoreSqlConnectionManager();
                DirectoryInfo di = new DirectoryInfo(_workflowPath);

                di.GetFiles(workflowId+".rules")[0].Delete();
                di.GetFiles(workflowId+".xoml")[0].Delete();
            }
            catch(Exception)
            {
            }
            finally
            {
                BackupSqlConnectionManager();
            }
        }
        #endregion

        private void CleanupLocalStatus()
        {
            _inScopeTag = null;
            _conn = null;
            _trans = null;
            _dbCataLog = null;
            _embeded = null;
            _errOccured = null;
        }
        private void CleanupRemoteStatus()
        {
            typeof(SqlTransactionManager).GetField("_inScopeTag",
                BindingFlags.Static | BindingFlags.NonPublic).SetValue(
                null, false);
            typeof(SqlTransactionManager).GetField("_conn",
                BindingFlags.Static | BindingFlags.NonPublic).SetValue(
                null, null);
            typeof(SqlTransactionManager).GetField("_trans",
                BindingFlags.Static | BindingFlags.NonPublic).SetValue(
                null, null);
            typeof(SqlTransactionManager).GetField("_dbCataLog",
                BindingFlags.Static | BindingFlags.NonPublic).SetValue(
                null, null);
            typeof(SqlTransactionManager).GetField("_embeded",
                BindingFlags.Static | BindingFlags.NonPublic).SetValue(
                null, 0);
            typeof(SqlTransactionManager).GetField("_errOccured",
                BindingFlags.Static | BindingFlags.NonPublic).SetValue(
                null, false);
        }
        private void BackupSqlConnectionManager()
        {
            if (!_inTransaction)
                return;

            _inScopeTag = typeof(SqlTransactionManager).GetField("_inScopeTag",
                BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            _conn = typeof(SqlTransactionManager).GetField("_conn",
                BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            _trans = typeof(SqlTransactionManager).GetField("_trans",
                BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            _dbCataLog = typeof(SqlTransactionManager).GetField("_dbCataLog",
                BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            _embeded = typeof(SqlTransactionManager).GetField("_embeded",
                BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            _errOccured = typeof(SqlTransactionManager).GetField("_errOccured",
                BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);

            CleanupRemoteStatus();
        }

        private void RestoreSqlConnectionManager()
        {
            if (!_inTransaction)
                return;

            typeof(SqlTransactionManager).GetField("_inScopeTag",
                BindingFlags.Static | BindingFlags.NonPublic).SetValue(
                null, _inScopeTag);
            typeof(SqlTransactionManager).GetField("_conn",
                BindingFlags.Static | BindingFlags.NonPublic).SetValue(
                null, _conn);
            typeof(SqlTransactionManager).GetField("_trans",
                BindingFlags.Static | BindingFlags.NonPublic).SetValue(
                null, _trans);
            typeof(SqlTransactionManager).GetField("_dbCataLog",
                BindingFlags.Static | BindingFlags.NonPublic).SetValue(
                null, _dbCataLog);
            typeof(SqlTransactionManager).GetField("_embeded",
                BindingFlags.Static | BindingFlags.NonPublic).SetValue(
                null, _embeded);
            typeof(SqlTransactionManager).GetField("_errOccured",
                BindingFlags.Static | BindingFlags.NonPublic).SetValue(
                null, _errOccured);

            CleanupLocalStatus();
        }

        #region IRouteManagement Transaction Members


        public void Begin()
        {
            if (_inTransaction)
                return;

            try
            {
                SqlTransactionManager.Begin();
                _inTransaction = true;
            }
            finally
            {
                BackupSqlConnectionManager();
            }
        }

        public void Commit()
        {
            if (!_inTransaction)
                return;

            try
            {
                RestoreSqlConnectionManager();
                SqlTransactionManager.Commit();
            }
            finally
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
                _inTransaction = false;
            }
        }

        public void Rollback()
        {
            if (!_inTransaction)
                return;
            try
            {
                RestoreSqlConnectionManager();
                SqlTransactionManager.Rollback();
            }
            finally
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
                _inTransaction = false;
            }
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        #endregion

        public bool IsActive()
        {
            return true;
        }

        #region IRouteManagement Members


        public void DeleteRuleSet(string site, string station, string name)
        {
            throw new NotImplementedException();
        }

        public string ReadRuleSet(string site, string station, string name)
        {
            string rulesContent = "";
            try
            {
                RestoreSqlConnectionManager();

                DirectoryInfo wp = new DirectoryInfo(_workflowPath);
                DirectoryInfo di = wp.GetDirectories("rules")[0];

                try
                {
                    rulesContent = File.ReadAllText(di.GetFiles(name + ".rules")[0].FullName);
                }
                catch (Exception)
                {
                    // ignore if workflow doesn't have a rule
                }
            }
            finally
            {
                BackupSqlConnectionManager();
            }

            return rulesContent;
        }

        public void WriteRuleSet(string site, string station, string name, string content)
        {
            try
            {
                RestoreSqlConnectionManager();

                DirectoryInfo wp = new DirectoryInfo(_workflowPath);
                DirectoryInfo di = wp.GetDirectories("rules")[0];

                File.WriteAllText(di.FullName + "\\" + name + ".rules",
                    content, Encoding.Unicode);
            }
            finally
            {
                BackupSqlConnectionManager();
            }
        }

        #endregion

        #region IRouteManagement Members


        public IList<string> GetStationNameList()
        {
            try
            {
                RestoreSqlConnectionManager();

                DataTable dt = SqlHelper.ExecuteDataFill(_connString,
                    CommandType.Text, _sqlGetStationNameList, null);
                List<string> list = new List<string>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add((string)dr[0]);
                }

                return list;
            }
            finally
            {
                BackupSqlConnectionManager();
            }
        }

        #endregion
    }

    class RouteManagementServiceImpl : MarshalByRefObject, IRouteManagementService
    {
        #region IRouteManagementService Members

        public IRouteManagement RouteManager
        {
            get
            {
                return new RouteManagementImpl();
            }
        }

        #endregion
    }

    class RouteManagementUtils
    {
        public static void GetWorkflow(string stationId,
            string defWorkflow, string defRules,
            out string workflow, out string rules)
        {
            string attr = null;

            try
            {
                RouteManagementImpl impl = new RouteManagementImpl();
                attr = impl.ReadStationAttr(impl.ConvertStationIdToName(stationId), "Workflow");
            }
            catch (Exception)
            {
            }
            if (attr == null)
            {
                workflow = defWorkflow;
                rules = defRules;

                return;
            }

            workflow = attr + ".xoml";
            rules = null;
            if (defRules != null && defRules.Trim() != "")
                rules = attr + ".rules";
        }
    }
}
