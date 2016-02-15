using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.PrintItem;
using IMES.DataModel;
using IMES.Infrastructure.Util;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Repository._Metas;
using fons = IMES.FisObject.Common.PrintItem;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.Infrastructure.Repository.Common
{
    /// <summary>
    /// 数据访问与持久化类: LabelType相关
    /// </summary>
    public class LabelTypeRepository : BaseRepository<LabelType>, ILabelTypeRepository
    {
        private static GetValueClass g = new GetValueClass();

        #region Overrides of BaseRepository<LabelType>

        protected override void PersistNewItem(LabelType item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    throw new NotImplementedException("Normal");
                    //this.PersistInsertLabelType(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(LabelType item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    throw new NotImplementedException("Normal");
                    //this.PersistModifyLabelType(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(LabelType item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    throw new NotImplementedException("Normal");
                    //this.PersistDeleteLabelType(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }
        #endregion

        #region Implementation of IRepository<LabelType>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override LabelType Find(object key)
        {
            try
            {
                LabelType ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.LabelType cond = new _Schema.LabelType();
                        cond.labelType = (string)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.LabelType), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.LabelType.fn_labelType].Value = (string)key;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new LabelType();
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.LabelType.fn_Cdt]);
                        ret.Description = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.LabelType.fn_Description]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.LabelType.fn_Editor]);
                        ret.LblType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.LabelType.fn_labelType]);
                        ret.PrintMode = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.LabelType.fn_PrintMode]);
                        ret.RuleMode = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.LabelType.fn_RuleMode]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.LabelType.fn_Udt]);
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
        public override IList<LabelType> FindAll()
        {
            try
            {
                IList<LabelType> ret = new List<LabelType>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.LabelType));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        LabelType item = new LabelType();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.LabelType.fn_Cdt]);
                        item.Description = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.LabelType.fn_Description]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.LabelType.fn_Editor]);
                        item.LblType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.LabelType.fn_labelType]);
                        item.PrintMode = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.LabelType.fn_PrintMode]);
                        item.RuleMode = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.LabelType.fn_RuleMode]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.LabelType.fn_Udt]);
                        item.Tracker.Clear();
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

        /// <summary>
        /// 添加一个对象
        /// </summary>
        /// <param name="item">新添加的对象</param>
        public override void Add(LabelType item, IUnitOfWork work)
        {
            throw new NotImplementedException("Normal");
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        public override void Remove(LabelType item, IUnitOfWork work)
        {
            throw new NotImplementedException("Normal");
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="uow"></param>
        public override void Update(LabelType item, IUnitOfWork work)
        {
            throw new NotImplementedException("Normal");
        }

        #endregion

        #region ILabelTypeRepository Members

        //public IList<PrintTemplateInfo> GetPrintTemplateList(string stationId, string labelType)
        //{
        //    try
        //    {
        //        IList<PrintTemplateInfo> ret = new List<PrintTemplateInfo>();

        //        DateTime dt = _Schema.SqlHelper.GetDateTime();

        //        _Schema.SQLContext sqlCtx = null;
        //        _Schema.TableAndFields tf1 = null;
        //        _Schema.TableAndFields tf2 = null;
        //        _Schema.TableAndFields[] tblAndFldsesArray = null;
                //lock (MethodBase.GetCurrentMethod())
                //{
        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
        //        {
        //            tf1 = new _Schema.TableAndFields();
        //            tf1.Table = typeof(_Schema.PrintTemplate);
        //            tf1.ToGetFieldNames.Add(_Schema.PrintTemplate.fn_Description);
        //            tf1.ToGetFieldNames.Add(_Schema.PrintTemplate.fn_TemplateName);

        //            tf2 = new _Schema.TableAndFields();
        //            tf2.Table = typeof(_Schema.Station_LabelType);
        //            _Schema.Station_LabelType eqCond = new _Schema.Station_LabelType();
        //            eqCond.LabelType = labelType;
        //            eqCond.Station = stationId;
        //            tf2.equalcond = eqCond;
        //            tf2.ToGetFieldNames = null;

        //            List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
        //            _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.PrintTemplate.fn_LabelType, tf2, _Schema.Station_LabelType.fn_LabelType);
        //            tblCnntIs.Add(tc1);

        //            _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

        //            tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };
        //            sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);
        //        }
    //}
        //        tf1 = tblAndFldsesArray[0];
        //        tf2 = tblAndFldsesArray[1];

        //        sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.Station_LabelType.fn_LabelType)].Value = labelType;
        //        sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.Station_LabelType.fn_Station)].Value = stationId;

        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            if (sqlR != null)
        //            {
        //                while (sqlR.Read())
        //                {
        //                    PrintTemplateInfo pti = new PrintTemplateInfo();
        //                    pti.friendlyName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.PrintTemplate.fn_Description)]);
        //                    pti.id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.PrintTemplate.fn_TemplateName)]);
        //                    ret.Add(pti);
        //                }
        //            }
        //        }
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public IList<PrintTemplateInfo> GetPrintTemplateList(string labelType)
        {
            try
            {
                IList<PrintTemplateInfo> ret = new List<PrintTemplateInfo>();

                DateTime dt = _Schema.SqlHelper.GetDateTime();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PrintTemplate cond = new _Schema.PrintTemplate();
                        cond.LabelType = labelType;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PrintTemplate), null, new List<string>() { _Schema.PrintTemplate.fn_Description, _Schema.PrintTemplate.fn_TemplateName }, cond, null, null, null, null, null, null, null);
                    }
                }

                sqlCtx.Params[_Schema.PrintTemplate.fn_LabelType].Value = labelType;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            PrintTemplateInfo pti = new PrintTemplateInfo();
                            pti.friendlyName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_Description]);
                            pti.id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_TemplateName]);
                            ret.Add(pti);
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

        //public int GetPrintMode(string stationId, string labelType)
        //{
        //    try
        //    {
        //        int ret = -1;

        //        _Schema.SQLContext sqlCtx = null;
        //        _Schema.TableAndFields tf1 = null;
        //        _Schema.TableAndFields tf2 = null;
        //        _Schema.TableAndFields[] tblAndFldsesArray = null;
                //lock (MethodBase.GetCurrentMethod())
                //{
        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
        //        {
        //            tf1 = new _Schema.TableAndFields();
        //            tf1.Table = typeof(_Schema.LabelType);
        //            tf1.ToGetFieldNames.Add(_Schema.LabelType.fn_PrintMode);

        //            tf2 = new _Schema.TableAndFields();
        //            tf2.Table = typeof(_Schema.Station_LabelType);
        //            _Schema.Station_LabelType eqCond = new _Schema.Station_LabelType();
        //            eqCond.Station = stationId;
        //            eqCond.LabelType = labelType;
        //            tf2.equalcond = eqCond;
        //            tf2.ToGetFieldNames = null;

        //            List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
        //            _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.LabelType.fn_labelType, tf2, _Schema.Station_LabelType.fn_LabelType);
        //            tblCnntIs.Add(tc1);

        //            _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

        //            tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };

        //            sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, null, ref tblAndFldsesArray, tblCnnts);
        //        }
    //}
        //        tf1 = tblAndFldsesArray[0];
        //        tf2 = tblAndFldsesArray[1];

        //        sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.Station_LabelType.fn_Station)].Value = stationId;
        //        sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.Station_LabelType.fn_LabelType)].Value = labelType;

        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            if (sqlR != null)
        //            {
        //                if (sqlR.Read())
        //                {
        //                    ret = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.LabelType.fn_PrintMode)]);
        //                }
        //            }
        //        }
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public int GetPrintMode(string labelType)
        {
            try
            {
                int ret = -1;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.LabelType cond = new _Schema.LabelType();
                        cond.labelType = labelType;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.LabelType), null, new List<string>() { _Schema.LabelType.fn_PrintMode }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.LabelType.fn_labelType].Value = labelType;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        if (sqlR.Read())
                        {
                            ret = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.LabelType.fn_PrintMode]);
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

        //public int GetRuleMode(string stationId, string labelType)
        //{
        //    try
        //    {
        //        int ret = -1;

        //        _Schema.SQLContext sqlCtx = null;
        //        _Schema.TableAndFields tf1 = null;
        //        _Schema.TableAndFields tf2 = null;
        //        _Schema.TableAndFields[] tblAndFldsesArray = null;
                //lock (MethodBase.GetCurrentMethod())
                //{
        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
        //        {
        //            tf1 = new _Schema.TableAndFields();
        //            tf1.Table = typeof(_Schema.LabelType);
        //            tf1.ToGetFieldNames.Add(_Schema.LabelType.fn_RuleMode);

        //            tf2 = new _Schema.TableAndFields();
        //            tf2.Table = typeof(_Schema.Station_LabelType);
        //            _Schema.Station_LabelType eqCond = new _Schema.Station_LabelType();
        //            eqCond.Station = stationId;
        //            eqCond.LabelType = labelType;
        //            tf2.equalcond = eqCond;
        //            tf2.ToGetFieldNames = null;

        //            List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
        //            _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.LabelType.fn_labelType, tf2, _Schema.Station_LabelType.fn_LabelType);
        //            tblCnntIs.Add(tc1);

        //            _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

        //            tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };

        //            sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, null, ref tblAndFldsesArray, tblCnnts);
        //        }
    //}
        //        tf1 = tblAndFldsesArray[0];
        //        tf2 = tblAndFldsesArray[1];

        //        sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.Station_LabelType.fn_Station)].Value = stationId;
        //        sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.Station_LabelType.fn_LabelType)].Value = labelType;

        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            if (sqlR != null)
        //            {
        //                if (sqlR.Read())
        //                {
        //                    ret = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.LabelType.fn_RuleMode)]);
        //                }
        //            }
        //        }
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public int GetRuleMode(string labelType)
        {
            try
            {
                int ret = -1;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.LabelType cond = new _Schema.LabelType();
                        cond.labelType = labelType;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.LabelType), null, new List<string>() { _Schema.LabelType.fn_RuleMode }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.LabelType.fn_labelType].Value = labelType;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        if (sqlR.Read())
                        {
                            ret = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.LabelType.fn_RuleMode]);
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

        //public IList<string> GetPrintLabelTypeList(string station)
        //{
        //    try
        //    {
        //        IList<string> ret = new List<string>();

        //        _Schema.SQLContext sqlCtx = null;
                //lock (MethodBase.GetCurrentMethod())
                //{
        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        {
        //            _Schema.Station_LabelType cond = new _Schema.Station_LabelType();
        //            cond.Station = station;
        //            sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Station_LabelType), cond, null, null);
        //        }
    //}
        //        sqlCtx.Params[_Schema.Station_LabelType.fn_Station].Value = station;
        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            while (sqlR != null && sqlR.Read())
        //            {
        //                string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Station_LabelType.fn_LabelType]);
        //                ret.Add(item);
        //            }
        //        }
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public IList<string> GetPrintLabelTypeList(string pcode)
        {
            try
            {
                IList<string> ret = new List<string>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PCode_LabelType cond = new _Schema.PCode_LabelType();
                        cond.PCode = pcode;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PCode_LabelType), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PCode_LabelType.fn_PCode].Value = pcode;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PCode_LabelType.fn_LabelType]);
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

        public LabelType FillTemplates(LabelType lblType)
        {
            try
            {
                IList<fons.PrintTemplate> newFieldVal = new List<fons.PrintTemplate>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PrintTemplate cond = new _Schema.PrintTemplate();
                        cond.LabelType = lblType.LblType;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PrintTemplate), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PrintTemplate.fn_LabelType].Value = lblType;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        fons.PrintTemplate item = new fons.PrintTemplate();
                        item.Description = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_Description]);
                        item.LblType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_LabelType]);
                        item.Piece = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_Piece]);
                        item.SpName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_SpName]);
                        item.TemplateName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_TemplateName]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_Editor]);
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_Cdt]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_Udt]);
                        item.Layout = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_layout]);
                        item.Tracker.Clear();
                        item.Tracker = lblType.Tracker;
                        newFieldVal.Add(item);
                    }
                }

                lblType.GetType().GetField("_templates", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(lblType, newFieldVal);

                return lblType;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public IList<LabelType> GetLabelTypesByStation(string station)
        //{
        //    try
        //    {
        //        IList<LabelType> ret = new List<LabelType>();

        //        _Schema.SQLContext sqlCtx = null;
        //        _Schema.TableAndFields tf1 = null;
        //        _Schema.TableAndFields tf2 = null;
        //        _Schema.TableAndFields[] tblAndFldsesArray = null;
                //lock (MethodBase.GetCurrentMethod())
                //{
        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
        //        {
        //            tf1 = new _Schema.TableAndFields();
        //            tf1.Table = typeof(_Schema.LabelType);

        //            tf2 = new _Schema.TableAndFields();
        //            tf2.Table = typeof(_Schema.Station_LabelType);
        //            _Schema.Station_LabelType eqCond = new _Schema.Station_LabelType();
        //            eqCond.Station = station;
        //            tf2.equalcond = eqCond;
        //            tf2.ToGetFieldNames = null;

        //            List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
        //            _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.LabelType.fn_labelType, tf2, _Schema.Station_LabelType.fn_LabelType);
        //            tblCnntIs.Add(tc1);

        //            _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

        //            tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };

        //            sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, null, ref tblAndFldsesArray, tblCnnts);
        //        }
    //}
        //        tf1 = tblAndFldsesArray[0];
        //        tf2 = tblAndFldsesArray[1];
        //        sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.Station_LabelType.fn_Station)].Value = station;
        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            if (sqlR != null)
        //            {
        //                while (sqlR.Read())
        //                {
        //                    LabelType item = new LabelType();
        //                    item.Description = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.LabelType.fn_Description)]);
        //                    item.LblType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.LabelType.fn_labelType)]);
        //                    item.PrintMode = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.LabelType.fn_PrintMode)]);
        //                    item.RuleMode = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.LabelType.fn_RuleMode)]);
        //                    item.Tracker.Clear();
        //                    ret.Add(item);
        //                }
        //            }
        //        }
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public IList<LabelType> GetLabelTypesByStation(string pcode)
        {
            try
            {
                IList<LabelType> ret = new List<LabelType>();

                _Schema.SQLContext sqlCtx = null;
                _Schema.TableAndFields tf1 = null;
                _Schema.TableAndFields tf2 = null;
                _Schema.TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new _Schema.TableAndFields();
                        tf1.Table = typeof(_Schema.LabelType);

                        tf2 = new _Schema.TableAndFields();
                        tf2.Table = typeof(_Schema.PCode_LabelType);
                        _Schema.PCode_LabelType eqCond = new _Schema.PCode_LabelType();
                        eqCond.PCode = pcode;
                        tf2.equalcond = eqCond;
                        tf2.ToGetFieldNames = null;

                        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
                        _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.LabelType.fn_labelType, tf2, _Schema.PCode_LabelType.fn_LabelType);
                        tblCnntIs.Add(tc1);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };

                        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, null, ref tblAndFldsesArray, tblCnnts);
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];
                sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.PCode_LabelType.fn_PCode)].Value = pcode;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            LabelType item = new LabelType();
                            item.Description = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.LabelType.fn_Description)]);
                            item.LblType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.LabelType.fn_labelType)]);
                            item.PrintMode = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.LabelType.fn_PrintMode)]);
                            item.RuleMode = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.LabelType.fn_RuleMode)]);
                            item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.LabelType.fn_Cdt)]);
                            item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.LabelType.fn_Udt)]);
                            item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.LabelType.fn_Editor)]);
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

        /// <summary>
        /// For Both Runtime And Maintain
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public fons.PrintTemplate GetPrintTemplate(string templateName)
        {
            try
            {
                fons.PrintTemplate ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PrintTemplate cond = new _Schema.PrintTemplate();
                        cond.TemplateName = templateName;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PrintTemplate), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PrintTemplate.fn_TemplateName].Value = templateName;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new fons.PrintTemplate();
                        ret.Description = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_Description]);
                        ret.LblType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_LabelType]);
                        ret.Piece = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_Piece]);
                        ret.SpName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_SpName]);
                        ret.TemplateName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_TemplateName]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_Editor]);
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_Cdt]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_Udt]);
                        ret.Layout = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_layout]);
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

        public fons.PrintTemplate GetPrintTemplate(string labelType, string templateName)
        {
            try
            {
                fons.PrintTemplate ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PrintTemplate cond = new _Schema.PrintTemplate();
                        cond.TemplateName = templateName;
                        cond.LabelType = labelType;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PrintTemplate), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PrintTemplate.fn_TemplateName].Value = templateName;
                sqlCtx.Params[_Schema.PrintTemplate.fn_LabelType].Value = labelType;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new fons.PrintTemplate();
                        ret.Description = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_Description]);
                        ret.LblType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_LabelType]);
                        ret.Piece = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_Piece]);
                        ret.SpName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_SpName]);
                        ret.TemplateName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_TemplateName]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_Editor]);
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_Cdt]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_Udt]);
                        ret.Layout = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_layout]);
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

        public fons.PrintTemplate GetPrintTemplate(string labelType, string model, string mo, string dn, string partno, int rulemode, string customer)
        {
            SqlParameter[] paramsArray = new SqlParameter[7];

            paramsArray[0] = new SqlParameter("@LabelType", SqlDbType.VarChar);
            paramsArray[0].Value = string.IsNullOrEmpty(labelType) ? "" : labelType;
            paramsArray[1] = new SqlParameter("@RuleMode", SqlDbType.Int);
            paramsArray[1].Value = rulemode;
            paramsArray[2] = new SqlParameter("@MO", SqlDbType.VarChar);
            paramsArray[2].Value = string.IsNullOrEmpty(mo) ? "" : mo;
            paramsArray[3] = new SqlParameter("@DeliveryNo", SqlDbType.VarChar);
            paramsArray[3].Value = string.IsNullOrEmpty(dn) ? "" : dn;
            paramsArray[4] = new SqlParameter("@PartNo", SqlDbType.VarChar);
            paramsArray[4].Value = string.IsNullOrEmpty(partno) ? "" : partno;
            paramsArray[5] = new SqlParameter("@Model", SqlDbType.VarChar);
            paramsArray[5].Value = model;
            paramsArray[6] = new SqlParameter("@Customer", SqlDbType.VarChar);
            paramsArray[6].Value = customer;

            using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.StoredProcedure, "IMES_GetTemplate", paramsArray))
            {
                if (sqlR != null && sqlR.Read())
                {
                    fons.PrintTemplate newTemplate = new fons.PrintTemplate();
                    newTemplate.TemplateName = GetValue_Str(sqlR, 0);
                    newTemplate.Piece = GetValue_Int32(sqlR, 1);
                    newTemplate.Layout = GetValue_Int32(sqlR, 2);
                    return newTemplate;
                }
            }
            return null;

        }

        public string GetMainBat(string currentSPName, List<string> parameterKeys, List<List<string>> parameterValues)
        {
            if (parameterKeys != null && parameterValues != null)
            {
                int keyCount = parameterKeys.Count;
                if (keyCount > parameterValues.Count)
                {
                    keyCount = parameterValues.Count;
                }
                SqlParameter[] paramsArray = new SqlParameter[keyCount];
                for (int i = 0; i < keyCount; i++)
                {
                    paramsArray[i] = new SqlParameter(parameterKeys[i], SqlDbType.VarChar);
                    paramsArray[i].Value = parameterValues[i][0];
                }

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.StoredProcedure, currentSPName, paramsArray))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        return GetValue_Str(sqlR, 0);
                    }
                }
            }
            return null;

        }

        public string GetMainBat(string currentSPName, List<string> parameterKeys, List<string> parameterValues)
        {
            if (parameterKeys != null && parameterValues != null)
            {
                int keyCount = parameterKeys.Count;
                if (keyCount > parameterValues.Count)
                {
                    keyCount = parameterValues.Count;
                }
                SqlParameter[] paramsArray = new SqlParameter[keyCount];
                for (int i = 0; i < keyCount; i++)
                {
                    paramsArray[i] = new SqlParameter(parameterKeys[i], SqlDbType.VarChar);
                    paramsArray[i].Value = parameterValues[i];
                }

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.StoredProcedure, currentSPName, paramsArray))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        return GetValue_Str(sqlR, 0);
                    }
                }
            }
            return null;
        }
        #endregion

        #region For Maintain

        public IList<LabelType> GetLabelTypeList()
        {
            try
            {
                IList<LabelType> ret = new List<LabelType>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.LabelType));
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.LabelType.fn_labelType);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        LabelType item = new LabelType();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.LabelType.fn_Cdt]);
                        item.Description = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.LabelType.fn_Description]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.LabelType.fn_Editor]);
                        item.LblType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.LabelType.fn_labelType]);
                        item.PrintMode = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.LabelType.fn_PrintMode]);
                        item.RuleMode = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.LabelType.fn_RuleMode]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.LabelType.fn_Udt]);
                        item.Tracker.Clear();
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

        public int CheckExistedLabelType(string strLabelType)
        {
            // select count(*) from LabelType where LabelType=?
            try
            {
                int ret = 0;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.LabelType cond = new _Schema.LabelType();
                        cond.labelType = strLabelType;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.LabelType), "COUNT", new List<string>() { _Schema.LabelType.fn_labelType }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.LabelType.fn_labelType].Value = strLabelType;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = GetValue_Int32(sqlR, sqlCtx.Indexes["COUNT"]);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddLabelType(LabelType item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.LabelType));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.LabelType.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.LabelType.fn_Description].Value = item.Description;
                sqlCtx.Params[_Schema.LabelType.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.LabelType.fn_labelType].Value = item.LblType;
                sqlCtx.Params[_Schema.LabelType.fn_PrintMode].Value = item.PrintMode;
                sqlCtx.Params[_Schema.LabelType.fn_RuleMode].Value = item.RuleMode;
                sqlCtx.Params[_Schema.LabelType.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveLabelType(LabelType item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.LabelType));
                    }
                }
                sqlCtx.Params[_Schema.LabelType.fn_labelType].Value = item.LblType;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.LabelType.fn_Description].Value = item.Description;
                sqlCtx.Params[_Schema.LabelType.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.LabelType.fn_PrintMode].Value = item.PrintMode;
                sqlCtx.Params[_Schema.LabelType.fn_RuleMode].Value = item.RuleMode;
                sqlCtx.Params[_Schema.LabelType.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteLabelType(LabelType item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.LabelType));
                    }
                }
                sqlCtx.Params[_Schema.LabelType.fn_labelType].Value = item.LblType;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<fons.PrintTemplate> GetPrintTemplateListByLabelType(string strLabelType)
        {
            try
            {
                IList<fons.PrintTemplate> ret = new List<fons.PrintTemplate>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PrintTemplate cond = new _Schema.PrintTemplate();
                        cond.LabelType = strLabelType;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PrintTemplate), null, null, cond, null, null, null, null, null, null, null);
                        //Vincent Change order by Udt
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderByDesc, _Schema.PrintTemplate.fn_Udt);
                    }
                }
                sqlCtx.Params[_Schema.PrintTemplate.fn_LabelType].Value = strLabelType;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        fons.PrintTemplate item = new fons.PrintTemplate();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_Cdt]);
                        item.Description = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_Description]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_Editor]);
                        item.LblType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_LabelType]);
                        item.Piece = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_Piece]);
                        item.SpName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_SpName]);
                        item.TemplateName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_TemplateName]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_Udt]);
                        item.Layout = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PrintTemplate.fn_layout]);
                        item.Tracker.Clear();
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

        public int CheckExistedTemplateName(string strTemplateName)
        {
            // select count(*) from PrintTemplate where TemplateName=?
            try
            {
                int ret = 0;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PrintTemplate cond = new _Schema.PrintTemplate();
                        cond.TemplateName = strTemplateName;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PrintTemplate), "COUNT", new List<string>() { _Schema.PrintTemplate.fn_TemplateName }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PrintTemplate.fn_TemplateName].Value = strTemplateName;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = GetValue_Int32(sqlR, sqlCtx.Indexes["COUNT"]);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddPrintTemplate(fons.PrintTemplate item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PrintTemplate));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PrintTemplate.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.PrintTemplate.fn_Description].Value = item.Description;
                sqlCtx.Params[_Schema.PrintTemplate.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.PrintTemplate.fn_LabelType].Value = item.LblType;
                sqlCtx.Params[_Schema.PrintTemplate.fn_Piece].Value = item.Piece;
                sqlCtx.Params[_Schema.PrintTemplate.fn_SpName].Value = item.SpName;
                sqlCtx.Params[_Schema.PrintTemplate.fn_TemplateName].Value = item.TemplateName;
                sqlCtx.Params[_Schema.PrintTemplate.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.PrintTemplate.fn_layout].Value = item.Layout;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SavePrintTemplate(fons.PrintTemplate item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PrintTemplate));
                    }
                }
                sqlCtx.Params[_Schema.PrintTemplate.fn_TemplateName].Value = item.TemplateName;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.PrintTemplate.fn_Description].Value = item.Description;
                sqlCtx.Params[_Schema.PrintTemplate.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.PrintTemplate.fn_LabelType].Value = item.LblType;
                sqlCtx.Params[_Schema.PrintTemplate.fn_Piece].Value = item.Piece;
                sqlCtx.Params[_Schema.PrintTemplate.fn_SpName].Value = item.SpName;
                sqlCtx.Params[_Schema.PrintTemplate.fn_Udt].Value = cmDt;
                sqlCtx.Params[_Schema.PrintTemplate.fn_layout].Value = item.Layout;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePrintTemplate(fons.PrintTemplate item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PrintTemplate));
                    }
                }
                sqlCtx.Params[_Schema.PrintTemplate.fn_TemplateName].Value = item.TemplateName;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PCodeLabelType> GetPCodeByLabelType(string strLabelType)
        {
            // select PCode from PCode_LabelType where LabelType=?
            try
            {
                IList<PCodeLabelType> ret = new List<PCodeLabelType>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PCode_LabelType cond = new _Schema.PCode_LabelType();
                        cond.LabelType = strLabelType;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PCode_LabelType), null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PCode_LabelType.fn_LabelType].Value = strLabelType;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        PCodeLabelType item = new PCodeLabelType();
                        item.LabelType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PCode_LabelType.fn_LabelType]);
                        item.PCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PCode_LabelType.fn_PCode]);
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

        public void SavePCode(PCodeLabelType item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PCode_LabelType));
                    }
                }
                sqlCtx.Params[_Schema.PCode_LabelType.fn_LabelType].Value = item.LabelType;
                sqlCtx.Params[_Schema.PCode_LabelType.fn_PCode].Value = item.PCode;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<LabelRule> GetLabelRuleByTemplateName(string strTemplateName)
        {
            // select * from LabelRule where TemplateName=?
            try
            {
                IList<LabelRule> ret = new List<LabelRule>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.LabelRule cond = new _Schema.LabelRule();
                        cond.TemplateName = strTemplateName;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.LabelRule), null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.LabelRule.fn_TemplateName].Value = strTemplateName;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        LabelRule item = new LabelRule();
                        item.RuleID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.LabelRule.fn_RuleID]);
                        item.TemplateName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.LabelRule.fn_TemplateName]);
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

        public void AddLabelRule(LabelRule item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.LabelRule));
                    }
                }
                sqlCtx.Params[_Schema.LabelRule.fn_TemplateName].Value = item.TemplateName;
                item.RuleID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteLabelRule(LabelRule item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.LabelRule));
                    }
                }
                sqlCtx.Params[_Schema.LabelRule.fn_RuleID].Value = item.RuleID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<LabelRuleSet> GetLabelRuleSetByRuleID(int RuleID)
        {
            // select * from LabelRuleSet where RuleID=?
            try
            {
                IList<LabelRuleSet> ret = new List<LabelRuleSet>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.LabelRuleSet cond = new _Schema.LabelRuleSet();
                        cond.RuleID = RuleID;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.LabelRuleSet), null, null, cond, null, null, null, null, null, null, null);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.LabelRuleSet.fn_Mode);
                    }
                }
                sqlCtx.Params[_Schema.LabelRuleSet.fn_RuleID].Value = RuleID;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        LabelRuleSet item = new LabelRuleSet();
                        item.AttributeName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.LabelRuleSet.fn_AttributeName]);
                        item.AttributeValue = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.LabelRuleSet.fn_AttributeValue]);
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.LabelRuleSet.fn_Cdt]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.LabelRuleSet.fn_Editor]);
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.LabelRuleSet.fn_ID]);
                        item.Mode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.LabelRuleSet.fn_Mode]);
                        item.RuleID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.LabelRuleSet.fn_RuleID]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.LabelRuleSet.fn_Udt]);
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

        public int CheckExistedAttributeName(int ruleId, string mode, string attributeName)
        {
            // select count(*) from LabelRuleSet where RuleID = ? and Mode = ? and AttributeName=?
            try
            {
                int ret = 0;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.LabelRuleSet cond = new _Schema.LabelRuleSet();
                        cond.RuleID = ruleId;
                        cond.Mode = mode;
                        cond.AttributeName = attributeName;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.LabelRuleSet), "COUNT", new List<string>() { _Schema.LabelRuleSet.fn_ID }, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.LabelRuleSet.fn_RuleID].Value = ruleId;
                sqlCtx.Params[_Schema.LabelRuleSet.fn_Mode].Value = mode;
                sqlCtx.Params[_Schema.LabelRuleSet.fn_AttributeName].Value = attributeName;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = GetValue_Int32(sqlR, sqlCtx.Indexes["COUNT"]);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddLabelRuleSet(LabelRuleSet item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.LabelRuleSet));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.LabelRuleSet.fn_AttributeName].Value = item.AttributeName;
                sqlCtx.Params[_Schema.LabelRuleSet.fn_AttributeValue].Value = item.AttributeValue;
                sqlCtx.Params[_Schema.LabelRuleSet.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.LabelRuleSet.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.LabelRuleSet.fn_Mode].Value = item.Mode;
                sqlCtx.Params[_Schema.LabelRuleSet.fn_RuleID].Value = item.RuleID;
                sqlCtx.Params[_Schema.LabelRuleSet.fn_Udt].Value = cmDt;
                item.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveLabelRuleSet(LabelRuleSet item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.LabelRuleSet));
                    }
                }
                sqlCtx.Params[_Schema.LabelRuleSet.fn_ID].Value = item.ID;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.LabelRuleSet.fn_AttributeName].Value = item.AttributeName;
                sqlCtx.Params[_Schema.LabelRuleSet.fn_AttributeValue].Value = item.AttributeValue;
                sqlCtx.Params[_Schema.LabelRuleSet.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.LabelRuleSet.fn_Mode].Value = item.Mode;
                sqlCtx.Params[_Schema.LabelRuleSet.fn_RuleID].Value = item.RuleID;
                sqlCtx.Params[_Schema.LabelRuleSet.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteLabelRuleSet(LabelRuleSet item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.LabelRuleSet));
                    }
                }
                sqlCtx.Params[_Schema.LabelRuleSet.fn_ID].Value = item.ID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeletePCodeByLabelType(string strLabelType)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PCode_LabelType cond = new _Schema.PCode_LabelType();
                        cond.LabelType = strLabelType;
                        sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PCode_LabelType), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.PCode_LabelType.fn_LabelType].Value = strLabelType;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public LabelRule GetLabelRuleByRuleID(int ruleId)
        {
            try
            {
                LabelRule ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.LabelRule cond = new _Schema.LabelRule();
                        cond.RuleID = ruleId;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.LabelRule), null, null, cond, null, null, null, null, null, null, null);

                    }
                }
                sqlCtx.Params[_Schema.LabelRule.fn_RuleID].Value = ruleId;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new LabelRule();
                        ret.RuleID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.LabelRule.fn_RuleID]);
                        ret.TemplateName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.LabelRule.fn_TemplateName]);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public LabelRuleSet GetLabelRuleSetByID(int ruleSetId)
        {
            try
            {
                LabelRuleSet ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.LabelRuleSet cond = new _Schema.LabelRuleSet();
                        cond.ID = ruleSetId;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.LabelRuleSet), null, null, cond, null, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.LabelRuleSet.fn_ID].Value = ruleSetId;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new LabelRuleSet();
                        ret.AttributeName = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.LabelRuleSet.fn_AttributeName]);
                        ret.AttributeValue = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.LabelRuleSet.fn_AttributeValue]);
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.LabelRuleSet.fn_Cdt]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.LabelRuleSet.fn_Editor]);
                        ret.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.LabelRuleSet.fn_ID]);
                        ret.Mode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.LabelRuleSet.fn_Mode]);
                        ret.RuleID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.LabelRuleSet.fn_RuleID]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.LabelRuleSet.fn_Udt]);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Defered

        public void AddLabelTypeDefered(IMES.Infrastructure.UnitOfWork.IUnitOfWork uow, LabelType item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void SaveLabelTypeDefered(IMES.Infrastructure.UnitOfWork.IUnitOfWork uow, LabelType item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeleteLabelTypeDefered(IMES.Infrastructure.UnitOfWork.IUnitOfWork uow, LabelType item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void AddPrintTemplateDefered(IMES.Infrastructure.UnitOfWork.IUnitOfWork uow, fons.PrintTemplate item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void SavePrintTemplateDefered(IMES.Infrastructure.UnitOfWork.IUnitOfWork uow, fons.PrintTemplate item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeletePrintTemplateDefered(IMES.Infrastructure.UnitOfWork.IUnitOfWork uow, fons.PrintTemplate item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void SavePCodeDefered(IMES.Infrastructure.UnitOfWork.IUnitOfWork uow, PCodeLabelType item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void AddLabelRuleDefered(IMES.Infrastructure.UnitOfWork.IUnitOfWork uow, LabelRule item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeleteLabelRuleDefered(IMES.Infrastructure.UnitOfWork.IUnitOfWork uow, LabelRule item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void AddLabelRuleSetDefered(IMES.Infrastructure.UnitOfWork.IUnitOfWork uow, LabelRuleSet item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void SaveLabelRuleSetDefered(IMES.Infrastructure.UnitOfWork.IUnitOfWork uow, LabelRuleSet item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeleteLabelRuleSetDefered(IMES.Infrastructure.UnitOfWork.IUnitOfWork uow, LabelRuleSet item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeletePCodeByLabelTypeDefered(IMES.Infrastructure.UnitOfWork.IUnitOfWork uow, string strLabelType)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), strLabelType);
        }

        #endregion

        #endregion

        public IList<OfflineLableSettingDef> GetAllOfflineLabelSetting()
        {
            try
            {
                IList<OfflineLableSettingDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetCommonSelect<OfflineLabelSetting>(tk, OfflineLabelSetting.fn_fileName);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<OfflineLabelSetting, OfflineLableSettingDef, OfflineLableSettingDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<OfflineLableSettingDef> GetOfflineLabelSetting(string fileName)
        {
            try
            {
                IList<OfflineLableSettingDef> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        OfflineLabelSetting cond = new OfflineLabelSetting();
                        cond.fileName = fileName;
                        sqlCtx = FuncNew.GetConditionedSelect<OfflineLabelSetting>(tk, null, null, new ConditionCollection<OfflineLabelSetting>(new EqualCondition<OfflineLabelSetting>(cond)), OfflineLabelSetting.fn_id);
                    }
                }
                sqlCtx.Param(OfflineLabelSetting.fn_fileName).Value = fileName;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<OfflineLabelSetting, OfflineLableSettingDef, OfflineLableSettingDef>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddOfflineLabelSetting(OfflineLableSettingDef obj)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetAquireIdInsert<OfflineLabelSetting>(tk);
                    }
                }

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<OfflineLabelSetting, OfflineLableSettingDef>(sqlCtx, obj);

                sqlCtx.Param(OfflineLabelSetting.fn_cdt).Value = cmDt;
                sqlCtx.Param(OfflineLabelSetting.fn_udt).Value = cmDt;

                obj.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateOfflineLabelSetting(OfflineLableSettingDef obj, string oldFileName)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        OfflineLabelSetting cond = new OfflineLabelSetting();
                        cond.fileName = oldFileName;
                        OfflineLabelSetting setv = FuncNew.SetColumnFromField<OfflineLabelSetting, OfflineLableSettingDef>(obj);
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<OfflineLabelSetting>(tk, new SetValueCollection<OfflineLabelSetting>(new CommonSetValue<OfflineLabelSetting>(setv)), new ConditionCollection<OfflineLabelSetting>(new EqualCondition<OfflineLabelSetting>(cond)));
                    }
                }
                sqlCtx.Param(OfflineLabelSetting.fn_fileName).Value = oldFileName;

                sqlCtx = FuncNew.SetColumnFromField<OfflineLabelSetting, OfflineLableSettingDef>(sqlCtx, obj, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(OfflineLabelSetting.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteOfflineLabelSetting(OfflineLableSettingDef obj)
        {
            try
            {
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        OfflineLabelSetting cond = new OfflineLabelSetting();
                        cond.id = obj.id;
                        sqlCtx = FuncNew.GetConditionedDelete<OfflineLabelSetting>(tk, new ConditionCollection<OfflineLabelSetting>(new EqualCondition<OfflineLabelSetting>(cond)));
                    }
                }
                sqlCtx.Param(OfflineLabelSetting.fn_id).Value = obj.id;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<PrintTemplateEntity> GetPrintTemplateListByLabelType(PrintTemplateEntity condition)
        {
            try
            {
                IList<PrintTemplateEntity> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::PrintTemplate cond = mtns::FuncNew.SetColumnFromField<mtns::PrintTemplate, PrintTemplateEntity>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::PrintTemplate>(null, null, new mtns::ConditionCollection<mtns::PrintTemplate>(new mtns::EqualCondition<mtns::PrintTemplate>(cond)), mtns::PrintTemplate.fn_templateName);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::PrintTemplate, PrintTemplateEntity>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::PrintTemplate, PrintTemplateEntity, PrintTemplateEntity>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePrintTemplate(PrintTemplateEntity setValue, PrintTemplateEntity condition)
        {
            try
            {
                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::PrintTemplate cond = mtns::FuncNew.SetColumnFromField<mtns::PrintTemplate, PrintTemplateEntity>(condition);
                mtns::PrintTemplate setv = mtns::FuncNew.SetColumnFromField<mtns::PrintTemplate, PrintTemplateEntity>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = mtns::FuncNew.GetConditionedUpdate<mtns::PrintTemplate>(new mtns::SetValueCollection<mtns::PrintTemplate>(new mtns::CommonSetValue<mtns::PrintTemplate>(setv)), new mtns::ConditionCollection<mtns::PrintTemplate>(new mtns::EqualCondition<mtns::PrintTemplate>(cond)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::PrintTemplate, PrintTemplateEntity>(sqlCtx, condition);
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::PrintTemplate, PrintTemplateEntity>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::PrintTemplate.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region . Defered  .

        public void AddOfflineLabelSettingDefered(IUnitOfWork uow, OfflineLableSettingDef obj)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), obj);
        }

        public void UpdateOfflineLabelSettingDefered(IUnitOfWork uow, OfflineLableSettingDef obj, string oldFileName)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), obj, oldFileName);
        }

        public void DeleteOfflineLabelSettingDefered(IUnitOfWork uow, OfflineLableSettingDef obj)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), obj);
        }

        public void UpdatePrintTemplateDefered(IUnitOfWork uow, PrintTemplateEntity setValue, PrintTemplateEntity condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        #endregion

        #region for LabelTypeRule Table
        public IList<LabelTypeRuleDef> GetLabeTypeRuleByPCode(string pCode)
        {
            try
            {
                IList<LabelTypeRuleDef> ret = new List<LabelTypeRuleDef>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select a.LabelType, isnull(b.Station,'') as Station, isnull(b.Family,'') as Family, isnull(b.Model,'') as Model, 
                                                                       isnull(b.ModelConstValue,'') as ModelConstValue,  isnull(b.DeliveryConstValue,'') as DeliveryConstValue,
                                                                       isnull(b.BomLevel,-1) as BomLevel,isnull(b.PartNo,'') as PartNo, isnull(b.BomNodeType,'') as BomNodeType, 
                                                                       isnull(b.PartDescr,'') as PartDescr , isnull(b.PartType,'') as PartType , isnull(b.PartConstValue,'') as PartConstValue, 
                                                                       isnull(b.Remark,'') as Remark ,ISNULL(b.Editor,'') as Editor, 
                                                                       isnull(b.Cdt,getdate()) as Cdt, isnull(b.Udt,getdate()) as Udt
                                                                from PCode_LabelType  a
                                                                left join LabelTypeRule b on a.LabelType = b.LabelType 
                                                                where  a.PCode =@PCode  ";
                        sqlCtx.AddParam("PCode", new SqlParameter("@PCode", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("PCode").Value = pCode;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        LabelTypeRuleDef item = IMES.Infrastructure.Repository._Schema.SQLData.ToObject<LabelTypeRuleDef>(sqlR);
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
        public LabelTypeRuleDef GetLabeTypeRuleByLabelType(string labelType)
        {
            try
            {
                LabelTypeRuleDef ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select LabelType, Station, Family, Model, ModelConstValue,DeliveryConstValue,
                                                                       BomLevel,PartNo, BomNodeType, PartDescr, PartType, PartConstValue, 
                                                                       Remark, Editor, Cdt, Udt
                                                                from LabelTypeRule
                                                                where LabelType=@LabelType ";
                        sqlCtx.AddParam("LabelType", new SqlParameter("@LabelType", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("LabelType").Value = labelType;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                         ret = IMES.Infrastructure.Repository._Schema.SQLData.ToObject<LabelTypeRuleDef>(sqlR);                      

                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<LabelTypeRuleDef> GetLabeTypeRuleByLabelType(IList<string> labelTypeList)
        {
            try
            {
                IList<LabelTypeRuleDef> ret = new List<LabelTypeRuleDef>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select a.LabelType, a.Station, a.Family, a.Model, a.ModelConstValue,a.DeliveryConstValue,
                                                                        a.BomLevel,a.PartNo, a.BomNodeType, a.PartDescr, a.PartType, a.PartConstValue, 
                                                                        a.Remark, a.Editor, a.Cdt, a.Udt
                                                                from LabelTypeRule a
                                                                inner join @LabelTypeList b on a.LabelType = b.data ";

                        SqlParameter para = new SqlParameter("@LabelTypeList", SqlDbType.Structured);
                        para.TypeName = "TbStringList";                        
                        sqlCtx.AddParam("LabelTypeList", para);

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("LabelTypeList").Value = IMES.Infrastructure.Repository._Schema.SQLData.ToDataTable(labelTypeList);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        LabelTypeRuleDef item = IMES.Infrastructure.Repository._Schema.SQLData.ToObject<LabelTypeRuleDef>(sqlR);
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

        public void UpdateAndInsertLabeTypeRule(LabelTypeRuleDef item)
        {
            try
            {

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"merge LabelTypeRule as T
                                                            using (select @LabelType, @Station, @Family, @Model,@ModelConstValue, @DeliveryConstValue,
                                                                                   @BomLevel,@PartNo, @BomNodeType, @PartDescr, @PartType, @PartConstValue, 
			                                                                      @Remark, @Editor)
                                                                   as S(LabelType, Station, Family, Model, ModelConstValue,DeliveryConstValue,
                                                                            BomLevel, PartNo, BomNodeType, PartDescr, PartType, PartConstValue, 
                                                                            Remark, Editor)
                                                            on (T.LabelType = S.LabelType)
                                                            WHEN MATCHED THEN
                                                                update 
                                                                set Station=S.Station, 
                                                                    Family=S.Family, 
                                                                    Model=S.Model,
                                                                    ModelConstValue=S.ModelConstValue,  
                                                                    DeliveryConstValue = S.DeliveryConstValue,                                                                  
                                                                    BomLevel=S.BomLevel, 
                                                                    PartNo=S.PartNo, 
                                                                    BomNodeType=S.BomNodeType, 
                                                                    PartDescr=S.PartDescr, 
                                                                    PartType=S.PartType, 
                                                                    PartConstValue=S.PartConstValue,                                                                   
                                                                    Remark=S.Remark, 
                                                                    Editor=S.Editor,
                                                                    Udt=GetDate()
                                                            WHEN NOT MATCHED THEN
                                                              insert (LabelType, Station, Family, Model, ModelConstValue,DeliveryConstValue,  
                                                                        BomLevel, PartNo, BomNodeType, PartDescr, PartType, PartConstValue, 
                                                                         Remark, Editor,Cdt, Udt)
                                                              Values (S.LabelType, S.Station, S.Family, S.Model, S.ModelConstValue, S.DeliveryConstValue,
                                                                           S.BomLevel, S.PartNo, S.BomNodeType, S.PartDescr, S.PartType, S.PartConstValue, 
                                                                            S.Remark, S.Editor,getdate(), getdate()); ";

                        sqlCtx.AddParam("LabelType", new SqlParameter("@LabelType", SqlDbType.VarChar));
                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        sqlCtx.AddParam("Model", new SqlParameter("@Model", SqlDbType.VarChar));
                        sqlCtx.AddParam("ModelConstValue", new SqlParameter("@ModelConstValue", SqlDbType.VarChar));
                        sqlCtx.AddParam("DeliveryConstValue", new SqlParameter("@DeliveryConstValue", SqlDbType.VarChar));

                        sqlCtx.AddParam("BomLevel", new SqlParameter("@BomLevel", SqlDbType.Int));

                        sqlCtx.AddParam("PartNo", new SqlParameter("@PartNo", SqlDbType.VarChar));
                        sqlCtx.AddParam("BomNodeType", new SqlParameter("@BomNodeType", SqlDbType.VarChar));
                        sqlCtx.AddParam("PartDescr", new SqlParameter("@PartDescr", SqlDbType.VarChar));
                        sqlCtx.AddParam("PartType", new SqlParameter("@PartType", SqlDbType.VarChar));
                        sqlCtx.AddParam("PartConstValue", new SqlParameter("@PartConstValue", SqlDbType.VarChar));

                        
                        sqlCtx.AddParam("Remark", new SqlParameter("@Remark", SqlDbType.VarChar));
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("LabelType").Value = item.LabelType;
                sqlCtx.Param("Station").Value = item.Station??"";
                sqlCtx.Param("Family").Value = item.Family?? "";
                sqlCtx.Param("Model").Value = item.Model?? "";
                sqlCtx.Param("ModelConstValue").Value = item.ModelConstValue ?? "";
                sqlCtx.Param("DeliveryConstValue").Value = item.DeliveryConstValue ?? "";

                sqlCtx.Param("BomLevel").Value = item.BomLevel;

                sqlCtx.Param("PartNo").Value = item.PartNo?? "";
                sqlCtx.Param("BomNodeType").Value = item.BomNodeType?? "";
                sqlCtx.Param("PartDescr").Value = item.PartDescr?? "";
                sqlCtx.Param("PartType").Value = item.PartType?? "";
                sqlCtx.Param("PartConstValue").Value = item.PartConstValue ?? "";
              
                sqlCtx.Param("Remark").Value = item.Remark?? "";
                sqlCtx.Param("Editor").Value = item.Editor;


                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                  CommandType.Text,
                                                                                sqlCtx.Sentence,
                                                                                sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }

        }
        public void DeleteLabelTypeRule(string labelType)
        {
            try
            {

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"delete from LabelTypeRule
                                                            where LabelType = @LabelType ";

                        sqlCtx.AddParam("LabelType", new SqlParameter("@LabelType", SqlDbType.VarChar));
                        
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("LabelType").Value = labelType;                

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                  CommandType.Text,
                                                                                sqlCtx.Sentence,
                                                                                sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateAndInsertModelConstValue(string labelType, string modelConstValue, string editor)
        {
            try
            {

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"merge LabelTypeRule as T
                                                            using (select @LabelType, @ModelConstValue, @Editor)
                                                                   as S(LabelType, ModelConstValue, Editor)
                                                            on (T.LabelType = S.LabelType)
                                                            WHEN MATCHED THEN
                                                                update 
                                                                set ModelConstValue=S.ModelConstValue,                                                                    
                                                                      Editor=S.Editor,
                                                                      Udt=GetDate()
                                                            WHEN NOT MATCHED THEN
                                                              insert (LabelType, ModelConstValue, BomLevel, 
                                                                         Editor,Cdt, Udt)
                                                              Values (S.LabelType, S.ModelConstValue, 0,
                                                                           S.Editor,getdate(), getdate()); ";

                        sqlCtx.AddParam("LabelType", new SqlParameter("@LabelType", SqlDbType.VarChar));
                        sqlCtx.AddParam("ModelConstValue", new SqlParameter("@ModelConstValue", SqlDbType.VarChar));
                                              
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("LabelType").Value = labelType;               
                sqlCtx.Param("ModelConstValue").Value = modelConstValue;              
                sqlCtx.Param("Editor").Value = editor;


                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                  CommandType.Text,
                                                                                sqlCtx.Sentence,
                                                                                sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateAndInsertDeliveryConstValue(string labelType, string deliveryConstValue, string editor)
        {
            try
            {

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"merge LabelTypeRule as T
                                                            using (select @LabelType, @DeliveryConstValue, @Editor)
                                                                   as S(LabelType, DeliveryConstValue, Editor)
                                                            on (T.LabelType = S.LabelType)
                                                            WHEN MATCHED THEN
                                                                update 
                                                                set DeliveryConstValue=S.DeliveryConstValue,                                                                    
                                                                      Editor=S.Editor,
                                                                      Udt=GetDate()
                                                            WHEN NOT MATCHED THEN
                                                              insert (LabelType, DeliveryConstValue, BomLevel, 
                                                                         Editor,Cdt, Udt)
                                                              Values (S.LabelType, S.DeliveryConstValue, 0,
                                                                           S.Editor,getdate(), getdate()); ";

                        sqlCtx.AddParam("LabelType", new SqlParameter("@LabelType", SqlDbType.VarChar));
                        sqlCtx.AddParam("DeliveryConstValue", new SqlParameter("@DeliveryConstValue", SqlDbType.VarChar));

                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("LabelType").Value = labelType;
                sqlCtx.Param("DeliveryConstValue").Value = deliveryConstValue;
                sqlCtx.Param("Editor").Value = editor;


                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                  CommandType.Text,
                                                                                sqlCtx.Sentence,
                                                                                sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateAndInsertPartConstValue(string labelType, int bomLevel, string partConstValue, string editor)
        {
            try
            {

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"merge LabelTypeRule as T
                                                            using (select @LabelType,@BomLevel, @PartConstValue, @Editor)
                                                                   as S(LabelType,BomLevel,  PartConstValue, Editor)
                                                            on (T.LabelType = S.LabelType)
                                                            WHEN MATCHED THEN
                                                                update 
                                                                set  BomLevel=S.BomLevel, 
                                                                       PartConstValue=S.PartConstValue,
                                                                       Editor=S.Editor,
                                                                       Udt=GetDate()
                                                            WHEN NOT MATCHED THEN
                                                              insert (LabelType, BomLevel, PartConstValue, 
                                                                         Editor,Cdt, Udt)
                                                              Values (S.LabelType, S.BomLevel, S.PartConstValue, 
                                                                           S.Editor,getdate(), getdate()); ";

                        sqlCtx.AddParam("LabelType", new SqlParameter("@LabelType", SqlDbType.VarChar));                      
                        sqlCtx.AddParam("BomLevel", new SqlParameter("@BomLevel", SqlDbType.Int));                       
                        sqlCtx.AddParam("PartConstValue", new SqlParameter("@PartConstValue", SqlDbType.VarChar));                      
                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("LabelType").Value = labelType;
                sqlCtx.Param("BomLevel").Value = bomLevel;                
                sqlCtx.Param("PartConstValue").Value = partConstValue;              
                sqlCtx.Param("Editor").Value = editor;


                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                  CommandType.Text,
                                                                                sqlCtx.Sentence,
                                                                                sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateAndInsertLabeTypeRuleDefered(IUnitOfWork uow, LabelTypeRuleDef item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(),item);
        }
        public void DeleteLabelTypeRuleDefered(IUnitOfWork uow, string labelType)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), labelType);
        }
        public void UpdateAndInsertModelConstValueDefered(IUnitOfWork uow, string labelType, string modelConstValue, string editor) 
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), labelType, modelConstValue, editor);
        }
        public void UpdateAndInsertDeliveryConstValue(IUnitOfWork uow, string labelType, string deliveryConstValue, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), labelType, deliveryConstValue, editor);
        }
        public void UpdateAndInsertPartConstValueDefered(IUnitOfWork uow, string labelType, int bomLevel, string partConstValue, string editor)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), labelType, bomLevel, partConstValue,editor);
        }

        #endregion

        #region for MO_LabelType PO_LabelType and PrintTemplate
        public IMES.FisObject.Common.PrintItem.PrintTemplate GetPrintTemplateByMo(string moNo,string labelType)
        {
            try
            {
                IMES.FisObject.Common.PrintItem.PrintTemplate ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select b.TemplateName, b.LabelType as LblType, b.Piece, b.SpName, b.Description, 
		                                                            b.Editor, b.Cdt, b.Udt, b.Layout
                                                             from MO_Label a, PrintTemplate b
                                                             where a.LabelType= b.LabelType and
                                                                   a.TemplateName = b.TemplateName and
                                                                   a.MO=@MO and
                                                                   a.LabelType=@LabelType";

                        sqlCtx.AddParam("MO", new SqlParameter("@MO", SqlDbType.VarChar));
                        sqlCtx.AddParam("LabelType", new SqlParameter("@LabelType", SqlDbType.VarChar)); 
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("MO").Value = moNo;
                sqlCtx.Param("LabelType").Value = labelType;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = IMES.Infrastructure.Repository._Schema.SQLData.ToObject<IMES.FisObject.Common.PrintItem.PrintTemplate>(sqlR);
                      
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IMES.FisObject.Common.PrintItem.PrintTemplate GetPrintTemplateByPo(string po, string labelType)
        {
            try
            {
                IMES.FisObject.Common.PrintItem.PrintTemplate ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select b.TemplateName, b.LabelType as LblType, b.Piece, b.SpName, b.Description, 
		                                                            b.Editor, b.Cdt, b.Udt, b.Layout
                                                             from PO_Label a, PrintTemplate b
                                                             where a.LabelType= b.LabelType and
                                                                   a.TemplateName = b.TemplateName and
                                                                   a.PO=@PO and
                                                                   a.LabelType=@LabelType";

                        sqlCtx.AddParam("PO", new SqlParameter("@PO", SqlDbType.VarChar));
                        sqlCtx.AddParam("LabelType", new SqlParameter("@LabelType", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("PO").Value = po;
                sqlCtx.Param("LabelType").Value = labelType;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = IMES.Infrastructure.Repository._Schema.SQLData.ToObject<IMES.FisObject.Common.PrintItem.PrintTemplate>(sqlR);

                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

//        public IMES.FisObject.Common.PrintItem.PrintTemplate GetPrintTemplateByName(string templateName)
//        {
//            try
//            {
//                IMES.FisObject.Common.PrintItem.PrintTemplate ret = null;

//                MethodBase mthObj = MethodBase.GetCurrentMethod();
//                int tk = mthObj.MetadataToken;
//                SQLContextNew sqlCtx = null;
//                lock (mthObj)
//                {
//                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
//                    {


//                        sqlCtx = new SQLContextNew();
//                        sqlCtx.Sentence = @"select b.TemplateName, b.LabelType as LblType, b.Piece, b.SpName, b.Description, 
//		                                                            b.Editor, b.Cdt, b.Udt, b.Layout
//                                                             from PrintTemplate b
//                                                             where b.TemplateName =@TemplateName";

//                        sqlCtx.AddParam("TemplateName", new SqlParameter("@TemplateName", SqlDbType.VarChar));
//                        SQLCache.InsertIntoCache(tk, sqlCtx);
//                    }
//                }

//                sqlCtx.Param("TemplateName").Value = templateName;

//                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
//                                                                                                                            CommandType.Text,
//                                                                                                                            sqlCtx.Sentence,
//                                                                                                                            sqlCtx.Params))
//                {
//                    if (sqlR != null && sqlR.Read())
//                    {
//                        ret = IMES.Infrastructure.Repository._Schema.SQLData.ToObject<IMES.FisObject.Common.PrintItem.PrintTemplate>(sqlR);

//                    }
//                }
//                return ret;
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }
        public void InsertMOLabel(string moNo, string labelType, string templateName)
        {
            try
            {

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"insert MO_Label(LabelType, MO, TemplateName)
                                                        Values(@LabelType, @MO, @TemplateName) ";

                        sqlCtx.AddParam("LabelType", new SqlParameter("@LabelType", SqlDbType.VarChar));
                        sqlCtx.AddParam("MO", new SqlParameter("@MO", SqlDbType.VarChar));
                        sqlCtx.AddParam("TemplateName", new SqlParameter("@TemplateName", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("LabelType").Value = labelType;
                sqlCtx.Param("MO").Value = moNo;
                sqlCtx.Param("TemplateName").Value = templateName;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                  CommandType.Text,
                                                                                sqlCtx.Sentence,
                                                                                sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public void InsertPOLabel(string po, string labelType, string templateName)
        {
            try
            {

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"insert PO_Label(LabelType, PO, TemplateName)
                                                        Values(@LabelType, @PO, @TemplateName) ";

                        sqlCtx.AddParam("LabelType", new SqlParameter("@LabelType", SqlDbType.VarChar));
                        sqlCtx.AddParam("PO", new SqlParameter("@PO", SqlDbType.VarChar));
                        sqlCtx.AddParam("TemplateName", new SqlParameter("@TemplateName", SqlDbType.VarChar));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("LabelType").Value = labelType;
                sqlCtx.Param("PO").Value = po;
                sqlCtx.Param("TemplateName").Value = templateName;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                  CommandType.Text,
                                                                                sqlCtx.Sentence,
                                                                                sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<LabelTemplateRuleDef> GetLabelTemplateRule(string labelType)
        {
            try
            {
                IList<LabelTemplateRuleDef> ret = new List<LabelTemplateRuleDef>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {


                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select a.LabelType, a.TemplateName,c.Mode, c.AttributeName, c.AttributeValue, 
                                                                   'N' as IsChecked, a.Udt
                                                            from PrintTemplate a, 
                                                                 LabelRule b, 
                                                                 LabelRuleSet c
                                                            where b.RuleID = c.RuleID and
                                                                  a.TemplateName =b.TemplateName and
                                                                  a.LabelType=@LabelType
                                                             order by a.TemplateName, b.RuleID";

                        sqlCtx.AddParam("LabelType", new SqlParameter("@LabelType", SqlDbType.VarChar));
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("LabelType").Value = labelType;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ret.Add( IMES.Infrastructure.Repository._Schema.SQLData.ToObject<LabelTemplateRuleDef>(sqlR));
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

        #region for Bartender Label
        /// <summary>
        /// Bartendar Label call sp return Name/Value List
        /// </summary>
        /// <param name="currentSPName">sp name</param>
        /// <param name="parameterKeys">sp parameter name</param>
        /// <param name="parameterValues">sp parameter value</param>
        /// <returns></returns>
        public IList<NameValueDataTypeInfo> GetBartendarNameValueInfo(string currentSPName, List<string> parameterKeys, List<List<string>> parameterValues)
        {
            IList<NameValueDataTypeInfo> ret = new List<NameValueDataTypeInfo>();
            if (parameterKeys != null && parameterValues != null)
            {
                int keyCount = parameterKeys.Count;
                if (keyCount > parameterValues.Count)
                {
                    keyCount = parameterValues.Count;
                }
                SqlParameter[] paramsArray = new SqlParameter[keyCount];
                for (int i = 0; i < keyCount; i++)
                {
                    paramsArray[i] = new SqlParameter(parameterKeys[i], SqlDbType.VarChar);
                    paramsArray[i].Value = parameterValues[i][0];
                }

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.StoredProcedure, currentSPName, paramsArray))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            ret.Add(new NameValueDataTypeInfo
                            {
                                Name = GetValue_Str(sqlR, 0),
                                Value = GetValue_Str(sqlR, 1),
                                DataType = GetValue_Str(sqlR, 2)
                            });
                        }
                    }
                }
            }
            return ret;
        }
        #endregion
    }
}
