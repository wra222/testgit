﻿//2010-02-04 Liu Dong(eB1-4)         Modify ITC-1103-0170 
//2010-04-14 Liu Dong(eB1-4)         Modify YWH:BOMPartCheckSetting的Model元素以SQL的通配符LIKE来实现过滤BOM动作.
//2010-04-22 Liu Dong(eB1-4)         Modify ITC-1136-0133
//2010-06-18 Liu Dong(eB1-4)         Modify ITC-1155-0196, ITC-1155-0199

using System;
using System.Linq;
using IMES.FisObject.Common.FisBOM;
//using IMES.Infrastructure.Repository._Schema;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Utility;
using IMES.DataModel;
using IBOMRepository = IMES.FisObject.Common.FisBOM.IBOMRepository;
using System.Reflection;
using System.Collections.Generic;
using System.Data;
using IMES.Infrastructure.Repository._Metas;
using System.Data.SqlClient;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Util;
using IMES.FisObject.Common.Part.PartPolicy;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using IMES.Infrastructure.Utility.Cache;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using System.Configuration;
using IMES.FisObject.Common.Model;
using mtns = IMES.Infrastructure.Repository._Metas;
using System.Text.RegularExpressions;
using IMES.FisObject.Common.Line;

namespace IMES.Infrastructure.Repository.Common
{
    ///<summary>
    ///数据访问与持久化类: BOM相关
    ///</summary>
    public class BOMRepository : /*BaseRepository<IFlatBOM>, */IBOMRepository, ICache //, IMES.FisObject.Common.BOM.IBOMRepository
    {
        private static GetValueClass g = new GetValueClass();

        #region Cache
        private static CacheManager _cache_real = null;
        private static CacheManager _cache
        {
            get
            {
                if (_cache_real == null)
                    _cache_real = CacheFactory.GetCacheManager("MOBOMCache");
                return _cache_real;
            }
        }
        private static object _syncObj_cache = new object();
        //private static IDictionary<string, IList<BOMPartCheckSetting>> _filtercache = new Dictionary<string, IList<BOMPartCheckSetting>>();
        //private static object _syncObj_filtercache = new object();
        #endregion

        #region Link To Other
        private static IMES.FisObject.Common.Part.IPartRepository _prtRepository = null;
        private static IMES.FisObject.Common.Part.IPartRepository PrtRepository
        {
            get
            {
                if (_prtRepository == null)
                    _prtRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Part.IPartRepository, IMES.FisObject.Common.Part.IPart>();
                return _prtRepository;
            }
        }

        private static IMES.FisObject.Common.Part.PartPolicy.IPartPolicyRepository _prtPcyRepository = null;
        private static IMES.FisObject.Common.Part.PartPolicy.IPartPolicyRepository PrtPcyRepository
        {
            get
            {
                if (_prtPcyRepository == null)
                    _prtPcyRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Part.PartPolicy.IPartPolicyRepository>();
                return _prtPcyRepository;
            }
        }

        private static IModelRepository _mdlRepository = null;
        private static IModelRepository MdlRepository
        {
            get
            {
                if (_mdlRepository == null)
                    _mdlRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, IMES.FisObject.Common.Model.Model>();
                return _mdlRepository;
            }
        }

        private static IMES.FisObject.Common.Station.IStationRepository _sttRepository = null;
        private static IMES.FisObject.Common.Station.IStationRepository SttRepository
        {
            get
            {
                if (_sttRepository == null)
                    _sttRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Station.IStationRepository, IMES.FisObject.Common.Station.IStation>();
                return _sttRepository;
            }
        }

        //private static IFamilyRepository _fmlRepository = null;
        //private static IFamilyRepository FmlRepository
        //{
        //    get
        //    {
        //        if (_fmlRepository == null)
        //            _fmlRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository, fons::Family>();
        //        return _fmlRepository;
        //    }
        //}

        #endregion

        //#region Overrides of BaseRepository<BOM>

        //protected override void PersistNewItem(BOM item)
        //{
        //    throw new NotImplementedException("Normal");
        //}

        //protected override void PersistUpdatedItem(BOM item)
        //{
        //    throw new NotImplementedException("Normal");
        //}

        //protected override void PersistDeletedItem(BOM item)
        //{
        //    throw new NotImplementedException("Normal");
        //}

        //#endregion

        //#region Implementation of IRepository<BOM>

        ///// <summary>
        ///// 根据对象key获取对象
        ///// </summary>
        ///// <param name="key">对象的key</param>
        ///// <returns>对象实例</returns>
        //public override BOM Find(object key)
        //{
        //    throw new NotImplementedException("Normal");
        //}

        ///// <summary>
        ///// 获取所有对象列表
        ///// </summary>
        ///// <returns>所有对象列表</returns>
        //public override IList<BOM> FindAll()
        //{
        //    throw new NotImplementedException("Normal");
        //}

        ///// <summary>
        ///// 添加一个对象
        ///// </summary>
        ///// <param name="item">新添加的对象</param>
        //public override void Add(BOM item, IUnitOfWork work)
        //{
        //    base.Add(item, work);
        //}

        ///// <summary>
        ///// 删除指定对象
        ///// </summary>
        ///// <param name="item">需删除的对象</param>
        //public override void Remove(BOM item, IUnitOfWork work)
        //{
        //    base.Remove(item, work);
        //}

        ///// <summary>
        ///// 更新指定对象
        ///// </summary>
        ///// <param name="item">需更新的对象</param>
        ///// <param name="work"></param>
        //public override void Update(BOM item, IUnitOfWork work)
        //{
        //    base.Update(item, work);
        //}

        //#endregion

        //#region . Implementation of IBOMRepository  .

        //public BOM GetBOM(string mo)
        //{
        //    LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //    try
        //    {
        //        LoggingInfoFormat("GetBOM->MO: {0}", mo);

        //        if (!IsCached())
        //            return GetBOM_DB(mo);

        //        BOM bom = GetBOM_Cache(mo);
        //        if (bom == null || bom.Items == null || bom.Items.Count < 1)
        //        {
        //            bom = GetBOM_DB(mo);

        //            if (bom != null && bom.Items != null && bom.Items.Count > 0)
        //            {
        //                lock (_syncObj_cache)
        //                {
        //                    if (!_cache.Contains(mo))
        //                        AddToCache(mo, bom);
        //                }
        //            }
        //        }
        //        //拷貝
        //        return CopyBOM(bom);
        //    }
        //    catch (Exception)
        //    {
        //        LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //        throw;
        //    }
        //    finally
        //    {
        //        LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //    }
        //}

        //public BOM GetBOMByStation(string mo, string station)
        //{
        //    LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //    try
        //    {
        //        LoggingInfoFormat("GetBOMByStation->MO: {0}, Staion: {1}.", mo, station);

        //        if (!IsCached())
        //            return BOMFilterStrategyManager.FilterBOM(CopyBOMWithFilter(GetBOM_DB(mo), station));

        //        BOM bom = GetBOM_Cache(mo);
        //        if (bom == null || bom.Items == null || bom.Items.Count < 1)
        //        {
        //            bom = GetBOM_DB(mo);

        //            if (bom != null && bom.Items != null && bom.Items.Count > 0)
        //            {
        //                lock (_syncObj_cache)
        //                {
        //                    if (!_cache.Contains(mo))
        //                        AddToCache(mo, bom);
        //                }
        //            }
        //        }
        //        //拷貝兼過濾
        //        return BOMFilterStrategyManager.FilterBOM(CopyBOMWithFilter(bom, station));
        //    }
        //    catch(Exception)
        //    {
        //        LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //        throw;
        //    }
        //    finally
        //    {
        //        LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //    }
        //}

        //public BOM GetModelBOM(string model)
        //{
        //    LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //    try
        //    {
        //        LoggingInfoFormat("GetModelBOM->Model: {0}", model);

        //        BOM bom = this.GetModelBOM_DB(model, 18);
        //        LoggingInfoFormat("GetModelBOM->BOM[ {0} ]", bom != null ? bom.ToString() : "<NULL>");
        //        return bom;
        //    }
        //    catch (Exception)
        //    {
        //        LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //        throw;
        //    }
        //    finally
        //    {
        //        LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //    }
        //}

        //public BOM GetModelBOM(string model, string station)
        //{
        //    LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //    try
        //    {
        //        LoggingInfoFormat("GetModelBOM->Model: {0}, Staion: {1}.", model, station);

        //        BOM bom = this.GetModelBOM_DB(model, 18);
        //        //拷貝兼過濾
        //        return CopyBOMWithFilter(bom, station);
        //    }
        //    catch (Exception)
        //    {
        //        LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //        throw;
        //    }
        //    finally
        //    {
        //        LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //    }
        //}

        ///// <summary>
        ///// 获取指定MOBOM中指定类型的且Part.Descr包含指定字符串的Pn列表
        ///// select MoBOM.PartNo from MoBOM inner join Part On MoBOM.PartNo = Part.PartNo where MO=? and PartType=? and Descr like '%?%'
        ///// </summary>
        ///// <param name="mo">mo</param>
        ///// <param name="partType">partType</param>
        ///// <param name="descrCondition">Part.Descr包含的指定字符串</param>
        ///// <returns></returns>
        //public IList<string> GetPnFromMoBOMByTypeAndDescrCondition(string mo, string partType, string descrCondition)
        //{
        //    try
        //    {
        //        IList<string> ret = new List<string>();

        //        _Schema.SQLContext sqlCtx = null;
        //        _Schema.TableAndFields tf1 = null;
        //        _Schema.TableAndFields tf2 = null;
        //        _Schema.TableAndFields[] tblAndFldsesArray = null;

        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
        //            {
        //                tf1 = new _Schema.TableAndFields();
        //                tf1.Table = typeof(_Schema.MoBOM);
        //                _Schema.MoBOM cond = new _Schema.MoBOM();
        //                cond.MO = mo;
        //                tf1.equalcond = cond;
        //                tf1.ToGetFieldNames.Add(_Schema.MoBOM.fn_PartNo);

        //                tf2 = new _Schema.TableAndFields();
        //                tf2.Table = typeof(_Schema.Part);
        //                _Schema.Part pCond = new _Schema.Part();
        //                pCond.PartType = partType;
        //                pCond.Flag = 1;     //Part表加Flag
        //                tf2.equalcond = pCond;
        //                _Schema.Part likeCond = new _Schema.Part();
        //                likeCond.Descr = "%" + descrCondition + "%";
        //                tf2.likecond = likeCond;
        //                tf2.ToGetFieldNames = null;

        //                List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
        //                _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.MoBOM.fn_PartNo, tf2, _Schema.Part.fn_PartNo);
        //                tblCnntIs.Add(tc1);

        //                _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

        //                tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };
        //                sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);

        //                sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.Part.fn_Flag)].Value = pCond.Flag;    //Part表加Flag
        //            }
        //        }
        //        tf1 = tblAndFldsesArray[0];
        //        tf2 = tblAndFldsesArray[1];

        //        sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.MoBOM.fn_MO)].Value = mo;
        //        sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.Part.fn_PartType)].Value = partType;
        //        sqlCtx.Params[_Schema.Func.DecAlias(tf2.alias, _Schema.Part.fn_Descr)].Value = "%" + descrCondition  + "%";

        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            if (sqlR != null)
        //            {
        //                while (sqlR.Read())
        //                {
        //                    string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.MoBOM.fn_PartNo)]);
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

        //public string Get1397NO(string _111PN)
        //{
        //    try
        //    {
        //        string ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                cond.Component = _111PN;
        //                cond.Flag = 1;//ModelBOM表加Flag
        //                _Schema.ModelBOM likeCond = new _Schema.ModelBOM();
        //                likeCond.Material = "1397%";
        //                sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), null, new List<string>() { _Schema.ModelBOM.fn_Material }, cond, likeCond, null, null, null, null, null, null);
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = likeCond.Material;
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag;//ModelBOM表加Flag
        //            }
        //        }
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = _111PN;
        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            if (sqlR != null && sqlR.Read())
        //            {
        //                ret = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Material]);
        //            }
        //        }
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public IList<string> Get1397NOList(string _111PN)
        //{
        //    try
        //    {
        //        IList<string> ret = new List<string>();

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                cond.Component = _111PN;
        //                cond.Flag = 1;//ModelBOM表加Flag
        //                _Schema.ModelBOM likeCond = new _Schema.ModelBOM();
        //                likeCond.Material = "1397%";
        //                sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "DISTINCT", new List<string>() { _Schema.ModelBOM.fn_Material }, cond, likeCond, null, null, null, null, null, null);
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = likeCond.Material;
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag;//ModelBOM表加Flag
        //            }
        //        }
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = _111PN;
        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            while (sqlR != null && sqlR.Read())
        //            {
        //                string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Material]);
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

        //public fons::Family GetFirstFamilyViaMoBOM(string partNum)
        //{
        //    try
        //    {
        //        fons::Family ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                sqlCtx = new _Schema.SQLContext();
        //                sqlCtx.Sentence = "SELECT {0} FROM {1} WHERE {2}=(SELECT {3} FROM {4} WHERE {5}=(SELECT TOP 1 {6} FROM {7} WHERE {8}=@{8} AND Deviation=1))";

        //                sqlCtx.Sentence = string.Format(sqlCtx.Sentence, _Schema.Model.fn_Family,
        //                                                                typeof(_Schema.Model).Name,
        //                                                                _Schema.Model.fn_model,
        //                                                                _Schema.MO.fn_Model,
        //                                                                typeof(_Schema.MO).Name,
        //                                                                _Schema.MO.fn_Mo,
        //                                                                _Schema.MoBOM.fn_MO,
        //                                                                typeof(_Schema.MoBOM).Name,
        //                                                                _Schema.MoBOM.fn_PartNo);

        //                sqlCtx.Params.Add(_Schema.MoBOM.fn_PartNo, new SqlParameter("@" + _Schema.MoBOM.fn_PartNo, SqlDbType.VarChar));

        //                _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
        //            }
        //        }
        //        sqlCtx.Params[_Schema.MoBOM.fn_PartNo].Value = partNum;

        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            if (sqlR != null)
        //            {
        //                if (sqlR.Read())
        //                {
        //                    string family = GetValue_Str(sqlR, 0);
        //                    if (!string.IsNullOrEmpty(family))
        //                        ret = FmlRepository.Find(family);
        //                }
        //            }
        //        }

        //        #region OLD
        //        //_Schema.SQLContext sqlCtx = null;
        //        //_Schema.TableAndFields tf1 = null;
        //        //_Schema.TableAndFields tf2 = null;
        //        //_Schema.TableAndFields tf3 = null;
        //        //_Schema.TableAndFields tf4 = null;
        //        //_Schema.TableAndFields tf5 = null;
        //        //_Schema.TableAndFields[] tblAndFldsesArray = null;
        //        //lock (MethodBase.GetCurrentMethod())
        //        //{
        //        //    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
        //        //    {
        //        //        tf1 = new _Schema.TableAndFields();
        //        //        tf1.Table = typeof(_Schema.MoBOM);
        //        //        _Schema.MoBOM cond = new _Schema.MoBOM();
        //        //        cond.PartNo = partNum;
        //        //        cond.Deviation = true;
        //        //        tf1.equalcond = cond;
        //        //        tf1.ToGetFieldNames = null;

        //        //        tf2 = new _Schema.TableAndFields();
        //        //        tf2.Table = typeof(_Schema.MO);
        //        //        tf2.ToGetFieldNames = null;

        //        //        tf3 = new _Schema.TableAndFields();
        //        //        tf3.Table = typeof(_Schema.Model);
        //        //        tf3.ToGetFieldNames = null;

        //        //        tf4 = new _Schema.TableAndFields();
        //        //        tf4.Table = typeof(_Schema.Family);

        //        //        tf5 = new _Schema.TableAndFields();
        //        //        tf5.Table = typeof(_Schema.Part);
        //        //        tf5.ToGetFieldNames = null;

        //        //        List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
        //        //        _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.MoBOM.fn_MO, tf2, _Schema.MO.fn_Mo);
        //        //        tblCnntIs.Add(tc1);
        //        //        _Schema.TableConnectionItem tc2 = new _Schema.TableConnectionItem(tf2, _Schema.MO.fn_Model, tf3, _Schema.Model.fn_model);
        //        //        tblCnntIs.Add(tc2);
        //        //        _Schema.TableConnectionItem tc3 = new _Schema.TableConnectionItem(tf3, _Schema.Model.fn_Family, tf4, _Schema.Family.fn_family);
        //        //        tblCnntIs.Add(tc3);
        //        //        _Schema.TableConnectionItem tc4 = new _Schema.TableConnectionItem(tf1, _Schema.MoBOM.fn_PartNo, tf5, _Schema.Part.fn_PartNo);
        //        //        tblCnntIs.Add(tc4);

        //        //        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

        //        //        tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2, tf3, tf4, tf5 };
        //        //        sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "TOP 1", ref tblAndFldsesArray, tblCnnts);

        //        //        sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.MoBOM.fn_Deviation)].Value = cond.Deviation;
        //        //    }
        //        //}
        //        //tf1 = tblAndFldsesArray[0];
        //        //tf2 = tblAndFldsesArray[1];
        //        //tf3 = tblAndFldsesArray[2];
        //        //tf4 = tblAndFldsesArray[3];
        //        //tf5 = tblAndFldsesArray[4];

        //        //sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.MoBOM.fn_PartNo)].Value = partNum;

        //        //using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        //{
        //        //    if (sqlR != null)
        //        //    {
        //        //        if (sqlR.Read())
        //        //        {
        //        //            ret = new Family(GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf4.alias, _Schema.Family.fn_family)]),
        //        //                             GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf4.alias, _Schema.Family.fn_Descr)]),
        //        //                             GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf4.alias, _Schema.Family.fn_CustomerID)]));
        //        //            ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf4.alias, _Schema.Family.fn_Editor)]);
        //        //            ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf4.alias, _Schema.Family.fn_Cdt)]);
        //        //            ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf4.alias, _Schema.Family.fn_Udt)]);
        //        //            ret.Tracker.Clear();
        //        //        }
        //        //    }
        //        //}
        //        #endregion

        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public IList<string> GetPnFromModelBOMByType(string model, string partType)
        //{
        //    try
        //    {
        //        IList<string> ret = new List<string>();

        //        BOM bom = this.GetModelBOM_DB(model, 18);

        //        bom = FilterBOMAndCopy(bom, partType);
        //        if (bom != null)
        //        {
        //            foreach (BOMItem bomi in bom.Items)
        //            {
        //                if (bomi.AlterParts != null)
        //                {
        //                    foreach (IBOMPart bompt in bomi.AlterParts)
        //                    {
        //                        if (!ret.Contains(bompt.PN))
        //                            ret.Add(bompt.PN);
        //                    }
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

        //public IList<string> GetPnFromMoBOMByType(string mo, string partType)
        //{
        //    try
        //    {
        //        IList<string> ret = new List<string>();

        //        BOM bom = null;

        //        if (!IsCached())
        //        {
        //            bom = GetBOM_DB(mo);
        //        }
        //        else
        //        {
        //            bom = GetBOM_Cache(mo);
        //            if (bom == null || bom.Items == null || bom.Items.Count < 1)
        //            {
        //                bom = GetBOM_DB(mo);

        //                if (bom != null)
        //                {
        //                    lock (_syncObj_cache)
        //                    {
        //                        if (!_cache.Contains(mo))
        //                            AddToCache(mo, bom);
        //                    }
        //                }
        //            }
        //        }
        //        bom = FilterBOMAndCopy(bom, partType);
        //        if (bom != null && bom.Items != null && bom.Items.Count > 0)
        //        {
        //            foreach(BOMItem bomi in bom.Items)
        //            {
        //                if (bomi.AlterParts != null)
        //                {
        //                    foreach (IBOMPart bompt in bomi.AlterParts)
        //                    {
        //                        if (!ret.Contains(bompt.PN))
        //                            ret.Add(bompt.PN);
        //                    }
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

        ////#region . Filter  .

        ////public List<BOMPartCheckSetting> GetPartCheckSetting(string customer, string model)
        ////{
        ////    
        ////}

        ////public List<BOMPartCheckSetting> GetPartCheckSetting(string customer, string model, string wc)
        ////{
        ////    
        ////}

        ////#endregion

        //public DataTable GetPartsViaModelBOM(string material)
        //{
        //    //select Descr,PartNo from IMES_GetData..Part where PartNo IN (select Component from IMES_GetData..ModelBOM where Material=''and Flag=1) order by PartNo,Descr
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.Part cond = new _Schema.Part();
        //                cond.Flag = 1;//Part表加Flag
        //                _Schema.Part insetCond = new _Schema.Part();
        //                insetCond.PartNo = "INSET";
        //                sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Part), null, new List<string>() { _Schema.Part.fn_Descr, _Schema.Part.fn_PartNo }, cond, null, null, null, null, null, null, insetCond);

        //                sqlCtx.Params[_Schema.Part.fn_Flag].Value = cond.Flag;//Part表加Flag;

        //                _Schema.SQLContext sqlCtx_sub = ComposeSubSQLForGetComponentByMaterial(material);

        //                foreach (KeyValuePair<string, SqlParameter> value in sqlCtx_sub.Params)
        //                {
        //                    sqlCtx.Params.Add("s_" + value.Key, new SqlParameter("@s_" + value.Value.ParameterName.Substring(1), value.Value.SqlDbType));
        //                }
        //                sqlCtx.Sentence = sqlCtx.Sentence.Replace(_Schema.Func.DecInSet(_Schema.Part.fn_PartNo), sqlCtx_sub.Sentence.Replace("@" + _Schema.ModelBOM.fn_Material, "@s_" + _Schema.ModelBOM.fn_Material).Replace("@" + _Schema.ModelBOM.fn_Flag, "@s_" + _Schema.ModelBOM.fn_Flag));//ModelBOM表加Flag

        //                sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.Part.fn_Descr, _Schema.Part.fn_PartNo })); 
        //            }
        //        }

        //        sqlCtx.Params["s_" + _Schema.ModelBOM.fn_Material].Value = material;
        //        sqlCtx.Params["s_" + _Schema.ModelBOM.fn_Flag].Value = 1;//ModelBOM表加Flag
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        ret = _Schema.Func.SortColumns(ret, new int[] { sqlCtx.Indexes[_Schema.Part.fn_Descr],
        //                                                        sqlCtx.Indexes[_Schema.Part.fn_PartNo]
        //                                                        });
        //        return ret;
        //    }
        //    catch(Exception)
        //    {
        //        throw;
        //    }
        //}

        //public DataTable GetExistPartNo(string code)
        //{
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.Part cond = new _Schema.Part();
        //                cond.PartNo = code;
        //                cond.Flag = 1;
        //                cond.AutoDL = "Y";
        //                sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Part), null, new List<string>() { _Schema.Part.fn_PartNo, _Schema.Part.fn_Descr }, cond, null, null, null, null, null, null, null);

        //                sqlCtx.Params[_Schema.Part.fn_Flag].Value = cond.Flag;
        //                sqlCtx.Params[_Schema.Part.fn_AutoDL].Value = cond.AutoDL;
        //            }
        //        }
        //        sqlCtx.Params[_Schema.Part.fn_PartNo].Value = code;
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        ret = _Schema.Func.SortColumns(ret, new int[] { sqlCtx.Indexes[_Schema.Part.fn_PartNo],
        //                                                        sqlCtx.Indexes[_Schema.Part.fn_Descr]
        //                                                        });
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //#endregion

        //#region . Inners .

        //private BOM GetBOM_Cache(string mo)
        //{
        //    LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //    try
        //    {
        //        LoggingInfoFormat("GetBOM_Cache->MO: {0}", mo);

        //        BOM bom = null;
        //        lock (_syncObj_cache)
        //        {
        //            if (_cache.Contains(mo))
        //                bom = (BOM)_cache[mo];
        //        }
        //        LoggingInfoFormat("GetBOM_Cache->BOM[ {0} ]", bom != null ? bom.ToString() : "<NULL>" );
        //        return bom;
        //    }
        //    catch (Exception)
        //    {
        //        LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //        throw;
        //    }
        //    finally
        //    {
        //        LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //    }
        //}

        //private BOM GetBOM_DB(string mo)
        //{
        //    LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //    try
        //    {
        //        LoggingInfoFormat("GetBOM_DB->MO: {0}", mo);

        //        List<BOMItem> content = new List<BOMItem>();
        //        BOM ret = new BOM(content);

        //        _Schema.SQLContext sqlCtx = null;
        //        _Schema.TableAndFields tf1 = null;
        //        _Schema.TableAndFields tf2 = null;
        //        _Schema.TableAndFields tf3 = null;
        //        _Schema.TableAndFields tf4 = null;
        //        _Schema.TableAndFields tf5 = null;
        //        _Schema.TableAndFields[] tblAndFldsesArray = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
        //            {
        //                tf1 = new _Schema.TableAndFields();
        //                tf1.Table = typeof(_Schema.MoBOM);
        //                _Schema.MoBOM cond = new _Schema.MoBOM();
        //                cond.MO = mo;
        //                cond.Deviation = true;
        //                tf1.equalcond = cond;
        //                tf1.ToGetFieldNames.Add(_Schema.MoBOM.fn_Group);
        //                tf1.ToGetFieldNames.Add(_Schema.MoBOM.fn_PartNo);
        //                tf1.ToGetFieldNames.Add(_Schema.MoBOM.fn_Qty);
        //                //tf1.ToGetFieldNames.Add(_Schema.MoBOM.fn_Action);
        //                //tf1.ToGetFieldNames.Add(_Schema.MoBOM.fn_AssemblyCode);

        //                tf2 = new _Schema.TableAndFields();
        //                tf2.Table = typeof(_Schema.MO);
        //                tf2.ToGetFieldNames = null;

        //                tf3 = new _Schema.TableAndFields();
        //                tf3.Table = typeof(_Schema.Model);
        //                tf3.ToGetFieldNames.Add(_Schema.Model.fn_model);

        //                tf4 = new _Schema.TableAndFields();
        //                tf4.Table = typeof(_Schema.Family);
        //                tf4.ToGetFieldNames.Add(_Schema.Family.fn_CustomerID);

        //                tf5 = new _Schema.TableAndFields();
        //                tf5.Table = typeof(_Schema.Part);
        //                //Part表加Flag
        //                _Schema.Part pCond = new _Schema.Part();
        //                pCond.Flag = 1;
        //                tf5.equalcond = pCond;

        //                List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
        //                _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.MoBOM.fn_MO, tf2, _Schema.MO.fn_Mo);
        //                tblCnntIs.Add(tc1);
        //                _Schema.TableConnectionItem tc2 = new _Schema.TableConnectionItem(tf2, _Schema.MO.fn_Model, tf3, _Schema.Model.fn_model);
        //                tblCnntIs.Add(tc2);
        //                _Schema.TableConnectionItem tc3 = new _Schema.TableConnectionItem(tf3, _Schema.Model.fn_Family, tf4, _Schema.Family.fn_family);
        //                tblCnntIs.Add(tc3);
        //                _Schema.TableConnectionItem tc4 = new _Schema.TableConnectionItem(tf1, _Schema.MoBOM.fn_PartNo, tf5, _Schema.Part.fn_PartNo);
        //                tblCnntIs.Add(tc4);

        //                _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

        //                tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2, tf3, tf4, tf5 };
        //                sqlCtx = _Schema.Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, null, ref tblAndFldsesArray, tblCnnts);

        //                sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.MoBOM.fn_Deviation)].Value = cond.Deviation;

        //                sqlCtx.Params[_Schema.Func.DecAlias(tf5.alias, _Schema.Part.fn_Flag)].Value = pCond.Flag;//Part表加Flag

        //                sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Func.DecAliasInner(tf1.alias, _Schema.MoBOM.fn_Group));
        //            }
        //        }
        //        tf1 = tblAndFldsesArray[0];
        //        tf2 = tblAndFldsesArray[1];
        //        tf3 = tblAndFldsesArray[2];
        //        tf4 = tblAndFldsesArray[3];
        //        tf5 = tblAndFldsesArray[4];

        //        sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.MoBOM.fn_MO)].Value = mo;

        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            if (sqlR != null)
        //            {
        //                FieldInfo fi_qty = typeof(BOMItem).GetField("_qty", BindingFlags.Instance | BindingFlags.NonPublic);
        //                FieldInfo fi_alterParts = typeof(BOMItem).GetField("_alterParts", BindingFlags.Instance | BindingFlags.NonPublic);
        //                FieldInfo fi_model = typeof(BOMItem).GetField("_model", BindingFlags.Instance | BindingFlags.NonPublic);
        //                FieldInfo fi_customerId = typeof(BOMItem).GetField("_customerId", BindingFlags.Instance | BindingFlags.NonPublic);

        //                BOMItem bomItem = null;
        //                IList<IBOMPart> alterParts = null;
        //                int thisGroup = -1;

        //                while (sqlR.Read())
        //                {
        //                    int nowGroup = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.MoBOM.fn_Group)]);
        //                    string customerID = string.Empty;
        //                    if (thisGroup == -1 || thisGroup != nowGroup) //開始新組
        //                    {
        //                        thisGroup = nowGroup;
        //                        bomItem = new BOMItem();
        //                        content.Add(bomItem);

        //                        int qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.MoBOM.fn_Qty)]);
        //                        fi_qty.SetValue(bomItem, qty);

        //                        alterParts = new List<IBOMPart>();
        //                        fi_alterParts.SetValue(bomItem, alterParts);

        //                        string model = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf3.alias, _Schema.Model.fn_model)]);
        //                        fi_model.SetValue(bomItem, model);

        //                        customerID = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf4.alias, _Schema.Family.fn_CustomerID)]);
        //                        fi_customerId.SetValue(bomItem, customerID);
        //                    }

        //                    IPart iprt = new IMES.FisObject.Common.Part.Part(
        //                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias,_Schema.Part.fn_PartNo)]),
        //                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias,_Schema.Part.fn_PartType)]),
        //                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias,_Schema.Part.fn_CustPartNo)]),
        //                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias,_Schema.Part.fn_Descr)]),
        //                                //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias,_Schema.Part.fn_FruNo)]),
        //                                //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias, _Schema.Part.fn_Vendor)]),
        //                                //GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias,_Schema.Part.fn_IECVersion)]),
        //                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias,_Schema.Part.fn_Remark)]),
        //                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias, _Schema.Part.fn_AutoDL)]),
        //                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias,_Schema.Part.fn_Editor)]),
        //                                GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias,_Schema.Part.fn_Cdt)]),
        //                                GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias,_Schema.Part.fn_Udt)]),
        //                                GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf5.alias,_Schema.Part.fn_Descr2)]));

        //                    ((IMES.FisObject.Common.Part.Part)iprt).Tracker.Clear();
                            
        //                    //string assbCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.MoBOM.fn_AssemblyCode)]);
        //                    //IList<string> assbCodes = new List<string>(assbCode.Split(new string[] { "~" }, StringSplitOptions.RemoveEmptyEntries));//2010-02-04 Liu Dong(eB1-4)         Modify ITC-1103-0170 
        //                    IBOMPart bompart = new BOMPart(iprt, /*assbCodes,*/ bomItem.CustomerId, bomItem.Model);

        //                    ((IMES.FisObject.Common.Part.Part)iprt).Tracker = ((BOMPart)bompart).Tracker.Merge(((IMES.FisObject.Common.Part.Part)iprt).Tracker);

        //                    alterParts.Add(bompart);
        //                }
        //            }
        //        }
        //        LoggingInfoFormat("GetBOM_DB->BOM[ {0} ]", ret != null ? ret.ToString() : "<NULL>");
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //        throw;
        //    }
        //    finally
        //    {
        //        LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //    }
        //}

        //private BOM GetModelBOM_DB(string model, int limitCount)
        //{
        //    LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //    try
        //    {
        //        LoggingInfoFormat("GetModelBOM_DB->Model: {0}", model);

        //        List<BOMItem> content = new List<BOMItem>();
        //        BOM ret = new BOM(content);

        //        IMES.FisObject.Common.Model.Model mdl = MdlRepository.Find(model);
        //        string customer = string.Empty;
        //        if (mdl != null)
        //        {
        //            customer = mdl.Family.Customer;
        //        }

        //        SqlParameter[] paramsArray = new SqlParameter[2];

        //        paramsArray[0] = new SqlParameter("@model", SqlDbType.VarChar);
        //        paramsArray[0].Value = model;
        //        paramsArray[1] = new SqlParameter("@limitCount", SqlDbType.Int);
        //        paramsArray[1].Value = limitCount;

        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.StoredProcedure, "GetModelBOM", paramsArray))
        //        {
        //            if (sqlR != null)
        //            {
        //                FieldInfo fi_qty = typeof(BOMItem).GetField("_qty", BindingFlags.Instance | BindingFlags.NonPublic);
        //                FieldInfo fi_alterParts = typeof(BOMItem).GetField("_alterParts", BindingFlags.Instance | BindingFlags.NonPublic);
        //                FieldInfo fi_model = typeof(BOMItem).GetField("_model", BindingFlags.Instance | BindingFlags.NonPublic);
        //                FieldInfo fi_customerId = typeof(BOMItem).GetField("_customerId", BindingFlags.Instance | BindingFlags.NonPublic);

        //                BOMItem bomItem = null;
        //                IList<IBOMPart> alterParts = null;
        //                string thisGroup = null;

        //                while (sqlR.Read())
        //                {
        //                    string pn = GetValue_Str(sqlR, 0);
        //                    string nowGroup = GetValue_Str(sqlR, 1);
        //                    int qty = Convert.ToInt32(GetValue_Str(sqlR, 2));
        //                    //string assbCode = GetValue_Str(sqlR, 3);
        //                    string customerID = string.Empty;
        //                    if (thisGroup == null || thisGroup != nowGroup) //開始新組
        //                    {
        //                        thisGroup = nowGroup;
        //                        bomItem = new BOMItem();
        //                        content.Add(bomItem);

        //                        fi_qty.SetValue(bomItem, qty);

        //                        alterParts = new List<IBOMPart>();
        //                        fi_alterParts.SetValue(bomItem, alterParts);

        //                        fi_model.SetValue(bomItem, model);

        //                        customerID = customer;
        //                        fi_customerId.SetValue(bomItem, customerID);
        //                    }

        //                    IPart iprt = PrtRepository.Find(pn);
        //                    if (iprt != null)
        //                    {
        //                        //IList<string> assbCodes = new List<string>(assbCode.Split(new string[] { "~" }, StringSplitOptions.RemoveEmptyEntries));//2010-02-04 Liu Dong(eB1-4)         Modify ITC-1103-0170 
        //                        IBOMPart bompart = new BOMPart(iprt, /*assbCodes,*/ bomItem.CustomerId, bomItem.Model);

        //                        ((IMES.FisObject.Common.Part.Part)iprt).Tracker = ((BOMPart)bompart).Tracker.Merge(((IMES.FisObject.Common.Part.Part)iprt).Tracker);

        //                        alterParts.Add(bompart);
        //                    }
        //                }
        //            }
        //        }
        //        LoggingInfoFormat("GetModelBOM_DB->BOM[ {0} ]", ret != null ? ret.ToString() : "<NULL>");
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //        throw;
        //    }
        //    finally
        //    {
        //        LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //    }
        //}

        //private void LoadAllPartCheckSettingFromDB()
        //{
        //    LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //    try
        //    {
        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting));
        //            }
        //        }
        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
        //        {
        //            if (sqlR != null)
        //            {
        //                while (sqlR.Read())
        //                {
        //                     BOMPartCheckSetting item = new BOMPartCheckSetting(
        //                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Customer]),
        //                        GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Model]),
        //                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_WC]),
        //                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Tp]),
        //                        GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_ValueType])
        //                        );
        //                    string key = item.GetHashCode().ToString();
        //                    if (!_filtercache.ContainsKey(key))
        //                    {
        //                        IList<BOMPartCheckSetting> newLst = new List<BOMPartCheckSetting>();
        //                        _filtercache.Add(key, newLst);
        //                        newLst.Add(item);
        //                    }
        //                    else
        //                    {
        //                        _filtercache[key].Add(item);
        //                    }

        //                    LoggingInfoFormat("Add One PartCheckSetting: {0}", item.ToString());

        //                    //BOMPartCheckSetting itemWithoutCust = new BOMPartCheckSetting(
        //                    //    string.Empty, 
        //                    //    item.Model,
        //                    //    item.Wc, 
        //                    //    item.PartType
        //                    //    );

        //                    //string keyWithoutCust = itemWithoutCust.GetHashCode().ToString();
        //                    //if (!_filtercache.ContainsKey(keyWithoutCust))
        //                    //    _filtercache.Add(keyWithoutCust, itemWithoutCust);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //        throw;
        //    }
        //    finally
        //    {
        //        LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //    }
        //}

        //private BOM CopyBOMWithFilter(BOM originBOM, string station)
        //{
        //    LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //    try
        //    {
        //        if (!this.IsCached())
        //        {
        //            lock (_syncObj_filtercache)
        //            {
        //                _filtercache.Clear();//每次清了从库里拿新的,即不是Cache
        //                this.LoadAllPartCheckSettingFromDB();
        //                BOM ret = CopyBOMWithFilter_Inner(originBOM, station);
        //                _filtercache.Clear();//原则上用完了也要清
        //                LoggingInfoFormat("CopyBOMWithFilter->BOM[ {0} ]", ret != null ? ret.ToString() : "<NULL>");
        //                return ret;
        //            }
        //        }
        //        else
        //        {
        //            lock (_syncObj_filtercache)
        //            {
        //                if (_filtercache.Count < 1)
        //                {
        //                    this.LoadAllPartCheckSettingFromDB();
        //                }
        //                return CopyBOMWithFilter_Inner(originBOM, station);
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //        throw;
        //    }
        //    finally
        //    {
        //        LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //    }
        //}

        //#region Copy Methods

        //private BOM CopyBOM(BOM originBOM)
        //{
        //    LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //    try
        //    {
        //        if (originBOM == null)
        //            return originBOM;

        //        List<BOMItem> content = new List<BOMItem>();
        //        BOM ret = new BOM(content);

        //        IEnumerator<BOMItem> enmrt = originBOM.Items.GetEnumerator();
        //        while (enmrt.MoveNext())
        //        {
        //            BOMItem newBomItem = new BOMItem();
        //            newBomItem.GetType().GetField("_customerId", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newBomItem, enmrt.Current.CustomerId);
        //            newBomItem.GetType().GetField("_model", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newBomItem, enmrt.Current.Model);
        //            newBomItem.GetType().GetField("_qty", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newBomItem, enmrt.Current.Qty);

        //            IList<IBOMPart> alterParts = null;
        //            content.Add(newBomItem);

        //            IEnumerator<IBOMPart> enmrt_i = enmrt.Current.AlterParts.GetEnumerator();
        //            while (enmrt_i.MoveNext())
        //            {
        //                if (newBomItem.AlterParts == null)
        //                {
        //                    alterParts = new List<IBOMPart>();
        //                    newBomItem.GetType().GetField("_alterParts", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newBomItem, alterParts);
        //                }
        //                IBOMPart obj = (IBOMPart)CloneTool.DoDeepCopyObj(enmrt_i.Current);
        //                alterParts.Add(obj);
        //            }

        //            if (newBomItem.AlterParts == null || newBomItem.AlterParts.Count < 1)
        //                content.Remove(newBomItem);
        //        }
        //        LoggingInfoFormat("CopyBOM->BOM[ {0} ]", ret != null ? ret.ToString() : "<NULL>");
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //        throw;
        //    }
        //    finally
        //    {
        //        LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //    }
        //}

        //private BOM FilterBOMAndCopy(BOM originBOM, string partType)
        //{
        //    LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //    try
        //    {
        //        if (originBOM == null)
        //            return originBOM;

        //        List<BOMItem> content = new List<BOMItem>();
        //        BOM ret = new BOM(content);

        //        IEnumerator<BOMItem> enmrt = originBOM.Items.GetEnumerator();
        //        while (enmrt.MoveNext())
        //        {
        //            BOMItem newBomItem = new BOMItem();
        //            newBomItem.GetType().GetField("_customerId", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newBomItem, enmrt.Current.CustomerId);
        //            newBomItem.GetType().GetField("_model", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newBomItem, enmrt.Current.Model);
        //            newBomItem.GetType().GetField("_qty", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newBomItem, enmrt.Current.Qty);

        //            IList<IBOMPart> alterParts = null;
        //            content.Add(newBomItem);

        //            IEnumerator<IBOMPart> enmrt_i = enmrt.Current.AlterParts.GetEnumerator();
        //            while (enmrt_i.MoveNext())
        //            {
        //                if (enmrt_i.Current.Type == partType)
        //                {
        //                    if (newBomItem.AlterParts == null)
        //                    {
        //                        alterParts = new List<IBOMPart>();
        //                        newBomItem.GetType().GetField("_alterParts", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newBomItem, alterParts);
        //                    }
        //                    IBOMPart obj = (IBOMPart)CloneTool.DoDeepCopyObj(enmrt_i.Current);
        //                    alterParts.Add(obj);
        //                }
        //            }

        //            if (newBomItem.AlterParts == null || newBomItem.AlterParts.Count < 1)
        //                content.Remove(newBomItem);
        //        }
        //        LoggingInfoFormat("FilterBOMAndCopy->BOM[ {0} ]", ret != null ? ret.ToString() : "<NULL>");
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //        throw;
        //    }
        //    finally
        //    {
        //        LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //    }
        //}

        //private BOM CopyBOMWithFilter_Inner(BOM originBOM, string station)
        //{
        //    LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //    try
        //    {
        //        if (originBOM == null)
        //            return originBOM;

        //        List<BOMItem> content = new List<BOMItem>();
        //        BOM ret = new BOM(content);

        //        IEnumerator<BOMItem> enmrt = originBOM.Items.GetEnumerator();
        //        while (enmrt.MoveNext())
        //        {
        //            IList<string> distinctValueTypes = new List<string>();

        //            BOMItem newBomItem = new BOMItem();
        //            newBomItem.GetType().GetField("_customerId", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newBomItem, enmrt.Current.CustomerId);
        //            newBomItem.GetType().GetField("_model", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newBomItem, enmrt.Current.Model);
        //            newBomItem.GetType().GetField("_qty", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newBomItem, enmrt.Current.Qty);

        //            IList<IBOMPart> alterParts = null;
        //            content.Add(newBomItem);

        //            IEnumerator<IBOMPart> enmrt_i = enmrt.Current.AlterParts.GetEnumerator();
        //            while (enmrt_i.MoveNext())
        //            {
        //                BOMPartCheckSetting cmper = new BOMPartCheckSetting(enmrt.Current.CustomerId,
        //                    //2010-04-14 Liu Dong(eB1-4)         Modify YWH:BOMPartCheckSetting的Model元素以SQL的通配符LIKE来实现过滤BOM动作.
        //                                                                    null,//enmrt.Current.Model,
        //                    //2010-04-14 Liu Dong(eB1-4)         Modify YWH:BOMPartCheckSetting的Model元素以SQL的通配符LIKE来实现过滤BOM动作.
        //                                                                    station,
        //                                                                    enmrt_i.Current.Type,
        //                                                                    null);
        //                string key = cmper.GetHashCode().ToString();
        //                lock (_syncObj_filtercache)
        //                {
        //                    bool bingle = _filtercache.ContainsKey(key);

        //                    //2010-04-14 Liu Dong(eB1-4)         Modify YWH:BOMPartCheckSetting的Model元素以SQL的通配符LIKE来实现过滤BOM动作.
        //                    //if (bingle == false)
        //                    //{
        //                    //    bingle = _filtercache.ContainsKey(new BOMPartCheckSetting(enmrt.Current.CustomerId,
        //                    //                                                null,
        //                    //                                                station,
        //                    //                                                enmrt_i.Current.Type).GetHashCode().ToString());
        //                    //}
        //                    if (bingle)
        //                    {
        //                        bool match = false;
        //                        foreach (BOMPartCheckSetting bompchkst in _filtercache[key])
        //                        {
        //                            bool match_i = false;

        //                            string wildcardModel = bompchkst.Model;
        //                            if (!string.IsNullOrEmpty(wildcardModel))
        //                                match_i = ToStringTool.IsLike(wildcardModel, enmrt.Current.Model);
        //                            else
        //                                match_i = true;

        //                            if (match_i)
        //                            {
        //                                if (match == false)
        //                                    match = true;

        //                                if (!distinctValueTypes.Contains(bompchkst.ValueType))
        //                                    distinctValueTypes.Add(bompchkst.ValueType);
        //                            }
        //                        }
        //                        //string wildcardModel = _filtercache[key].Model;
        //                        bingle = match;
        //                    }
        //                    //2010-04-14 Liu Dong(eB1-4)         Modify YWH:BOMPartCheckSetting的Model元素以SQL的通配符LIKE来实现过滤BOM动作.

        //                    if (bingle)
        //                    {
        //                        if (newBomItem.AlterParts == null)
        //                        {
        //                            alterParts = new List<IBOMPart>();
        //                            newBomItem.GetType().GetField("_alterParts", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(newBomItem, alterParts);
        //                        }
        //                        IBOMPart obj = (IBOMPart)CloneTool.DoDeepCopyObj(enmrt_i.Current); //这一級不用拷貝用原對象也應該沒有問題吧? YWH at 2010-01-28 said: NO.
        //                        alterParts.Add(obj);
        //                    }
        //                }
        //            }

        //            if (newBomItem.AlterParts == null || newBomItem.AlterParts.Count < 1)
        //            {
        //                content.Remove(newBomItem);
        //            }
        //            else
        //            {
        //                int k = 0;
        //                foreach (string valueType in distinctValueTypes)
        //                {
        //                    BOMItem curr = null;
        //                    if (k == 0)
        //                    {
        //                        curr = newBomItem;
        //                    }
        //                    else
        //                    {
        //                        curr = (BOMItem)CloneTool.DoDeepCopyObj(newBomItem);
        //                        content.Add(curr);
        //                    }
        //                    curr.ValueType = valueType;
        //                    k++;
        //                }
        //            }
        //        }
        //        LoggingInfoFormat("CopyBOMWithFilter_Inner->BOM[ {0} ]", ret != null ? ret.ToString() : "<NULL>");
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //        throw;
        //    }
        //    finally
        //    {
        //        LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //    }
        //}

        //#endregion

        //private IList<MOBOM> GetMOBOM(string mo, string deviation, string action)
        //{
        //    try
        //    {
        //        IList<MOBOM> ret = new List<MOBOM>();

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.MoBOM eqCond = new _Schema.MoBOM();
        //                eqCond.MO = mo;
        //                eqCond.Deviation = true;//deviation == "1" ? true : false;
        //                eqCond.Action = action;

        //                sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM), eqCond, null, null);
        //            }
        //        }
        //        sqlCtx.Params[_Schema.MoBOM.fn_MO].Value = mo;
        //        sqlCtx.Params[_Schema.MoBOM.fn_Deviation].Value = deviation == "1" ? true : false;
        //        sqlCtx.Params[_Schema.MoBOM.fn_Action].Value = action;
        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            if (sqlR != null)
        //            {
        //                while (sqlR.Read())
        //                {
        //                    MOBOM item = new MOBOM();
        //                    item.Action = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Action]);
        //                    item.AssemblyCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_AssemblyCode]);
        //                    item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Cdt]);
        //                    item.Deviation = GetValue_Bit(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Deviation]);
        //                    item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Editor]);
        //                    item.Group = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Group]);
        //                    item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_ID]);
        //                    item.MO = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_MO]);
        //                    item.PartNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_PartNo]);
        //                    item.Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Qty]);
        //                    item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Udt]);
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

        //private IList<MOBOM> GetMOBOM(string mo, string deviation)
        //{
        //    try
        //    {
        //        IList<MOBOM> ret = new List<MOBOM>();

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.MoBOM eqCond = new _Schema.MoBOM();
        //                eqCond.MO = mo;
        //                eqCond.Deviation = true;// deviation == "1" ? true : false;

        //                sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM), eqCond, null, null);
        //            }
        //        }
        //        sqlCtx.Params[_Schema.MoBOM.fn_MO].Value = mo;
        //        sqlCtx.Params[_Schema.MoBOM.fn_Deviation].Value = deviation == "1" ? true : false;
        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            if (sqlR != null)
        //            {
        //                while (sqlR.Read())
        //                {
        //                    MOBOM item = new MOBOM();
        //                    item.Action = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Action]);
        //                    item.AssemblyCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_AssemblyCode]);
        //                    item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Cdt]);
        //                    item.Deviation = GetValue_Bit(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Deviation]);
        //                    item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Editor]);
        //                    item.Group = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Group]);
        //                    item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_ID]);
        //                    item.MO = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_MO]);
        //                    item.PartNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_PartNo]);
        //                    item.Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Qty]);
        //                    item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Udt]);
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

        //#endregion

        //#region ICache Members

        //public bool IsCached()
        //{
        //    return DataChangeMediator.CheckCacheSwitchOpen(DataChangeMediator.CacheSwitchType.BOM);
        //}

        //public void ProcessItem(CacheUpdateInfo item)
        //{
        //    if (item.Type == IMES.DataModel.CacheType.MOBOM)
        //        LoadOneCache(item.Item);
        //    else if (item.Type == IMES.DataModel.CacheType.PartCheckSetting)
        //        LoadAllPartCheckSetting();
        //}

        //public void ClearCache()
        //{
        //    lock (_syncObj_cache)
        //    {
        //        _cache.Flush();
        //    }
        //}

        //private void LoadAllPartCheckSetting()
        //{
        //    LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //    try
        //    {
        //        lock (_syncObj_filtercache)
        //        {
        //            _filtercache.Clear();
        //            this.LoadAllPartCheckSettingFromDB();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //        throw;
        //    }
        //    finally
        //    {
        //        LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //    }
        //}

        //private void LoadOneCache(string pk) //mo
        //{
        //    LoggingBegin(this.GetType(), MethodBase.GetCurrentMethod());
        //    try
        //    {
        //        lock (_syncObj_cache)
        //        {
        //            LoggingInfoFormat("LoadOneCache->MO: {0}", pk);

        //            if (_cache.Contains(pk))
        //            {
        //                _cache.Remove(pk);
        //            }
        //            BOM bom = GetBOM_DB(pk);
        //            if (bom != null)
        //            {
        //                LoggingInfoFormat("LoadOneCache->BOM[ {0} ]", bom != null ? bom.ToString() : "<NULL>");
        //                AddToCache(pk, bom);
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
        //        throw;
        //    }
        //    finally
        //    {
        //        LoggingEnd(this.GetType(), MethodBase.GetCurrentMethod());
        //    }
        //}

        //private IMES.DataModel.CacheUpdateInfo GetACacheSignal(string pk, string type)
        //{
        //    IMES.DataModel.CacheUpdateInfo ret = new IMES.DataModel.CacheUpdateInfo();
        //    ret.Cdt = ret.Udt = _Schema.SqlHelper.GetDateTime();
        //    ret.Updated = false;
        //    ret.Type = type;
        //    ret.Item = pk;
        //    return ret;
        //}

        //private void AddToCache(string key, object obj)
        //{
        //    _cache.Add(key, obj, CacheItemPriority.Normal, new MOBOMRefreshAction(), new SlidingTime(TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["TOSC_MOBOMCache"].ToString()))));
        //}

        //[Serializable]
        //private class MOBOMRefreshAction : ICacheItemRefreshAction
        //{
        //    public void Refresh(string key, object expiredValue, CacheItemRemovedReason removalReason)
        //    {
        //    }
        //}

        //#endregion

        //#region For Maintain

        //public IList<MOBOM> GetMOBOMList(string mo)
        //{
        //    //来自于
        //    //1.MoBOM数据表中MO栏位等于MO,Devilation等于1的所有记录
        //    //2.MO栏位等于MO、Devilation等于0且Action栏位等于“DELETE”的所有记录的合集
        //    //3.按Group列的字符序排序

        //    //参考sql如下：
        //    //select id, MO, PartNo, AssemblyCode, Qty, Group, Deviation, Action, Editor, Cdt, Udt from
        //    //(select id, MO, PartNo, AssemblyCode, Qty, Group, Deviation, Action, Editor, Cdt, Udt from MoBOM where MO='MO' and Deviation = '1'
        //    //union
        //    //select id, MO, PartNo, AssemblyCode, Qty, Group, Deviation, Action, Editor, Cdt, Udt from MoBOM where MO='MO' and Deviation = '0' and Action = 'DELETE') as A
        //    //order by A.Group
        //    try
        //    {
        //        IList<MOBOM> ret = null;

        //        IList<MOBOM> set1 = this.GetMOBOM(mo, "1");
        //        IList<MOBOM> set2 = this.GetMOBOM(mo, "0", "DELETE");

        //        ret = new List<MOBOM>(set1.Union(set2, new EqualityComparer4MOBOM()));

        //        return (from item in ret orderby item.Group select item).ToList();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public IList<MOBOM> GetMOBOMListByGroup(string mo, bool deviation, int group, int exceptMOBOMId)
        //{
        //    try
        //    {
        //        IList<MOBOM> ret = new List<MOBOM>();

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.MoBOM eqCond = new _Schema.MoBOM();
        //                eqCond.MO = mo;
        //                eqCond.Deviation = true;
        //                eqCond.Group = group;

        //                _Schema.MoBOM neqCond = new _Schema.MoBOM();
        //                neqCond.ID = exceptMOBOMId;

        //                sqlCtx = _Schema.Func.GetConditionedFuncSelectWith3NotConds(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM), null, null, eqCond, null, null, null, null, null, null, null, neqCond, null, null);
        //            }
        //        }
        //        sqlCtx.Params[_Schema.MoBOM.fn_MO].Value = mo;
        //        sqlCtx.Params[_Schema.MoBOM.fn_Deviation].Value = deviation;
        //        sqlCtx.Params[_Schema.MoBOM.fn_Group].Value = group;
        //        sqlCtx.Params[_Schema.MoBOM.fn_ID].Value = exceptMOBOMId;
        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            if (sqlR != null)
        //            {
        //                while (sqlR.Read())
        //                {
        //                    MOBOM item = new MOBOM();
        //                    item.Action = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Action]);
        //                    item.AssemblyCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_AssemblyCode]);
        //                    item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Cdt]);
        //                    item.Deviation = GetValue_Bit(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Deviation]);
        //                    item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Editor]);
        //                    item.Group = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Group]);
        //                    item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_ID]);
        //                    item.MO = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_MO]);
        //                    item.PartNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_PartNo]);
        //                    item.Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Qty]);
        //                    item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Udt]);
        //                    item.Tracker.Clear();
        //                    ret.Add(item);
        //                }
        //            }
        //        }
        //        return ret;
        //    }
        //    catch(Exception)
        //    {
        //        throw;
        //    }
        //}

        //public MOBOM GetMOBOM(int id)
        //{
        //    try
        //    {
        //        MOBOM ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.MoBOM eqCond = new _Schema.MoBOM();
        //                eqCond.ID = id;

        //                sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM), eqCond, null, null);
        //            }
        //        }
        //        sqlCtx.Params[_Schema.MoBOM.fn_ID].Value = id;
        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            if (sqlR != null)
        //            {
        //                if (sqlR.Read())
        //                {
        //                    ret = new MOBOM();
        //                    ret.Action = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Action]);
        //                    ret.AssemblyCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_AssemblyCode]);
        //                    ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Cdt]);
        //                    ret.Deviation = GetValue_Bit(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Deviation]);
        //                    ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Editor]);
        //                    ret.Group = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Group]);
        //                    ret.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_ID]);
        //                    ret.MO = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_MO]);
        //                    ret.PartNo = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_PartNo]);
        //                    ret.Qty = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Qty]);
        //                    ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MoBOM.fn_Udt]);
        //                    ret.Tracker.Clear();
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

        //public int CheckMOBOMQty(string mo, bool deviation, string partNo)
        //{
        //    try
        //    {
        //        int ret = 0;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.MoBOM eqCond = new _Schema.MoBOM();
        //                eqCond.MO = mo;
        //                eqCond.Deviation = true;
        //                eqCond.PartNo = partNo;

        //                sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM), "COUNT", new List<string>() { _Schema.MoBOM.fn_ID }, eqCond, null, null, null, null, null, null, null);
        //            }
        //        }
        //        sqlCtx.Params[_Schema.MoBOM.fn_MO].Value = mo;
        //        sqlCtx.Params[_Schema.MoBOM.fn_Deviation].Value = deviation;
        //        sqlCtx.Params[_Schema.MoBOM.fn_PartNo].Value = partNo;
        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            if (sqlR != null)
        //            {
        //                if (sqlR.Read())
        //                {
        //                    ret = GetValue_Int32(sqlR, sqlCtx.Indexes["COUNT"]);
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

        //public int CheckMOBOMQty(string mo, bool deviation)
        //{
        //    try
        //    {
        //        int ret = 0;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.MoBOM eqCond = new _Schema.MoBOM();
        //                eqCond.MO = mo;
        //                eqCond.Deviation = true;

        //                sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM), "COUNT", new List<string>() { _Schema.MoBOM.fn_ID }, eqCond, null, null, null, null, null, null, null);
        //            }
        //        }
        //        sqlCtx.Params[_Schema.MoBOM.fn_MO].Value = mo;
        //        sqlCtx.Params[_Schema.MoBOM.fn_Deviation].Value = deviation;
        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            if (sqlR != null)
        //            {
        //                if (sqlR.Read())
        //                {
        //                    ret = GetValue_Int32(sqlR, sqlCtx.Indexes["COUNT"]);
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

        //public int AddMOBOM(MOBOM item)
        //{
        //    try
        //    {
        //        SqlTransactionManager.Begin();

        //        DataChangeMediator.AddChangeDemand(this.GetACacheSignal(item.MO, CacheType.MOBOM));

        //        //int ret = 0;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM));
        //            }
        //        }
        //        DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //        sqlCtx.Params[_Schema.MoBOM.fn_Action].Value = item.Action;
        //        sqlCtx.Params[_Schema.MoBOM.fn_AssemblyCode].Value = item.AssemblyCode;
        //        sqlCtx.Params[_Schema.MoBOM.fn_Cdt].Value = cmDt;
        //        sqlCtx.Params[_Schema.MoBOM.fn_Deviation].Value = item.Deviation;
        //        sqlCtx.Params[_Schema.MoBOM.fn_Editor].Value = item.Editor;
        //        sqlCtx.Params[_Schema.MoBOM.fn_Group].Value = item.Group;
        //        //sqlCtx.Params[_Schema.MoBOM.fn_ID].Value = item.ID;
        //        sqlCtx.Params[_Schema.MoBOM.fn_MO].Value = item.MO;
        //        sqlCtx.Params[_Schema.MoBOM.fn_PartNo].Value = item.PartNo;
        //        sqlCtx.Params[_Schema.MoBOM.fn_Qty].Value = item.Qty;
        //        sqlCtx.Params[_Schema.MoBOM.fn_Udt].Value = cmDt;
        //        item.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
        //        item.Tracker.Clear();

        //        SqlTransactionManager.Commit();

        //        return item.ID;
        //    }
        //    catch (Exception)
        //    {
        //        SqlTransactionManager.Rollback();
        //        throw;
        //    }
        //    finally
        //    {
        //        SqlTransactionManager.Dispose();
        //        SqlTransactionManager.End();
        //    }
        //}

        //public void UpdateMOBOM(MOBOM item, string oldMo)
        //{
        //    try
        //    {
        //        SqlTransactionManager.Begin();

        //        DataChangeMediator.AddChangeDemand(this.GetACacheSignal(oldMo, CacheType.MOBOM));
        //        if (oldMo != item.MO)
        //            DataChangeMediator.AddChangeDemand(this.GetACacheSignal(item.MO, CacheType.MOBOM));

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM));
        //            }
        //        }
        //        DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //        sqlCtx.Params[_Schema.MoBOM.fn_Action].Value = item.Action;
        //        sqlCtx.Params[_Schema.MoBOM.fn_AssemblyCode].Value = item.AssemblyCode;
        //        sqlCtx.Params[_Schema.MoBOM.fn_Deviation].Value = item.Deviation;
        //        sqlCtx.Params[_Schema.MoBOM.fn_Editor].Value = item.Editor;
        //        sqlCtx.Params[_Schema.MoBOM.fn_Group].Value = item.Group;
        //        sqlCtx.Params[_Schema.MoBOM.fn_ID].Value = item.ID;
        //        sqlCtx.Params[_Schema.MoBOM.fn_MO].Value = item.MO;
        //        sqlCtx.Params[_Schema.MoBOM.fn_PartNo].Value = item.PartNo;
        //        sqlCtx.Params[_Schema.MoBOM.fn_Qty].Value = item.Qty;
        //        sqlCtx.Params[_Schema.MoBOM.fn_Udt].Value = cmDt;
        //        _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        item.Udt = cmDt;
        //        item.Tracker.Clear();

        //        SqlTransactionManager.Commit();
        //    }
        //    catch (Exception)
        //    {
        //        SqlTransactionManager.Rollback();
        //        throw;
        //    }
        //    finally
        //    {
        //        SqlTransactionManager.Dispose();
        //        SqlTransactionManager.End();
        //    }
        //}

        //public void DeleteMOBOM(MOBOM item)
        //{
        //    try
        //    {
        //        SqlTransactionManager.Begin();

        //        DataChangeMediator.AddChangeDemand(this.GetACacheSignal(item.MO, CacheType.MOBOM));

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM));
        //            }
        //        }
        //        sqlCtx.Params[_Schema.MoBOM.fn_ID].Value = item.ID;
        //        _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

        //        SqlTransactionManager.Commit();
        //    }
        //    catch (Exception)
        //    {
        //        SqlTransactionManager.Rollback();
        //        throw;
        //    }
        //    finally
        //    {
        //        SqlTransactionManager.Dispose();
        //        SqlTransactionManager.End();
        //    }
        //}

        //public void DeleteMOBOMByMo(string mo)
        //{
        //    try
        //    {
        //        SqlTransactionManager.Begin();

        //        DataChangeMediator.AddChangeDemand(this.GetACacheSignal(mo, CacheType.MOBOM));

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.MoBOM cond = new _Schema.MoBOM();
        //                cond.MO = mo;
        //                sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM), cond, null, null);
        //            }
        //        }
        //        sqlCtx.Params[_Schema.MoBOM.fn_MO].Value = mo;
        //        _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

        //        SqlTransactionManager.Commit();
        //    }
        //    catch (Exception)
        //    {
        //        SqlTransactionManager.Rollback();
        //        throw;
        //    }
        //    finally
        //    {
        //        SqlTransactionManager.Dispose();
        //        SqlTransactionManager.End();
        //    }
        //}

        //public void CopyMOBOM(string mo)
        //{
        //    //复制当前MO的所有记录并将它们的Devilation栏位设置为0
        //    try
        //    {
        //        SqlTransactionManager.Begin();

        //        DataChangeMediator.AddChangeDemand(this.GetACacheSignal(mo, CacheType.MOBOM));

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                sqlCtx = new _Schema.SQLContext();
        //                sqlCtx.Sentence = "INSERT INTO {0} " +
        //                                "({1},{2},{3},{4},{5},{6},{7},{8},{9},{10}) " +
        //                                "(SELECT {1},{2},{3},{4},{5},0,{7},{8},{9},{10} FROM {0} " +
        //                                "WHERE {1}=@{1} AND {6}=1) ";

        //                sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.MoBOM).Name,
        //                                                                _Schema.MoBOM.fn_MO,
        //                                                                _Schema.MoBOM.fn_PartNo,
        //                                                                _Schema.MoBOM.fn_AssemblyCode,
        //                                                                _Schema.MoBOM.fn_Qty,
        //                                                                _Schema.MoBOM.fn_Group,
        //                                                                _Schema.MoBOM.fn_Deviation,
        //                                                                _Schema.MoBOM.fn_Action,
        //                                                                _Schema.MoBOM.fn_Editor,
        //                                                                _Schema.MoBOM.fn_Cdt,
        //                                                                _Schema.MoBOM.fn_Udt);

        //                sqlCtx.Params.Add(_Schema.MoBOM.fn_MO, new SqlParameter("@" + _Schema.MoBOM.fn_MO, SqlDbType.VarChar));

        //                _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
        //            }
        //        }
        //        sqlCtx.Params[_Schema.MoBOM.fn_MO].Value = mo;
        //        _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

        //        SqlTransactionManager.Commit();
        //    }
        //    catch (Exception)
        //    {
        //        SqlTransactionManager.Rollback();
        //        throw;
        //    }
        //    finally
        //    {
        //        SqlTransactionManager.Dispose();
        //        SqlTransactionManager.End();
        //    }
        //}

        //public void UpdateMOBOMForDeleteAction(string mo, string partNo, bool deviation)
        //{
        //    //将当前被选记录的对应Devilation等于0的记录的Action栏位设置为"DELETE"
        //    try
        //    {
        //        SqlTransactionManager.Begin();

        //        DataChangeMediator.AddChangeDemand(this.GetACacheSignal(mo, CacheType.MOBOM));

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.MoBOM eqCond = new _Schema.MoBOM();
        //                eqCond.MO = mo;
        //                eqCond.PartNo = partNo;
        //                eqCond.Deviation = true;

        //                sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM), new List<string>() { _Schema.MoBOM.fn_Action }, null, null, null, eqCond, null, null, null, null, null, null, null);
        //            }
        //        }
        //        DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //        sqlCtx.Params[_Schema.MoBOM.fn_MO].Value = mo;
        //        sqlCtx.Params[_Schema.MoBOM.fn_PartNo].Value = partNo;
        //        sqlCtx.Params[_Schema.MoBOM.fn_Deviation].Value = deviation;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.MoBOM.fn_Action)].Value = "DELETE";
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.MoBOM.fn_Udt)].Value = cmDt;
        //        _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

        //        SqlTransactionManager.Commit();
        //    }
        //    catch (Exception)
        //    {
        //        SqlTransactionManager.Rollback();
        //        throw;
        //    }
        //    finally
        //    {
        //        SqlTransactionManager.Dispose();
        //        SqlTransactionManager.End();
        //    }
        //}

        //public ModelBOM GetModelBOM(int modelBOMId)
        //{
        //    try
        //    {
        //        ModelBOM ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                cond.ID = modelBOMId;
        //                cond.Flag = 1;//ModelBOM表加Flag
        //                sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), cond, null, null);
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag; //ModelBOM表加Flag
        //            }
        //        }
        //        sqlCtx.Params[_Schema.ModelBOM.fn_ID].Value = modelBOMId;
        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            if (sqlR != null)
        //            {
        //                if (sqlR.Read())
        //                {
        //                    ret = new ModelBOM();
        //                    ret.Alternative_item_group = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Alternative_item_group]);
        //                    ret.Base_qty = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Base_qty]);
        //                    ret.Bom = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Bom]);
        //                    ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Cdt]);
        //                    ret.Change_number = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Change_number]);
        //                    ret.Component = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Component]);
        //                    ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Editor]);
        //                    ret.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_ID]);
        //                    ret.Item_number = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Item_number]);
        //                    ret.Item_text = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Item_text]);
        //                    ret.Material = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Material]);
        //                    ret.Material_group = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Material_group]);
        //                    ret.Plant = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Plant]);
        //                    ret.Priority = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Priority]);
        //                    ret.Quantity = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Quantity]);
        //                    ret.Sub_items = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Sub_items]);
        //                    ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Udt]);
        //                    ret.UOM = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_UOM]);
        //                    ret.Usage_probability = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Usage_probability]);
        //                    ret.Valid_from_date = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Valid_from_date]);
        //                    ret.Valid_to_date = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Valid_to_date]);
        //                    ret.Tracker.Clear();
        //                }
        //            }
        //        }
        //        return ret;
        //    }
        //    catch(Exception)
        //    {
        //        throw;
        //    }
        //}

        //public void UpdateModelBOM(ModelBOM item)
        //{
        //    try
        //    {
        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                cond.ID = item.ID;
        //                sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), null, new List<string>() { _Schema.ModelBOM.fn_ID, _Schema.ModelBOM.fn_Flag }, null, null, cond, null, null, null, null, null, null, null);
        //            }
        //        }
        //        sqlCtx.Params[_Schema.ModelBOM.fn_ID].Value = item.ID;

        //        DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Alternative_item_group)].Value = item.Alternative_item_group;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Base_qty)].Value = item.Base_qty;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Bom)].Value = item.Bom;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Change_number)].Value = item.Change_number;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Component)].Value = item.Component;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Editor)].Value = item.Editor;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Item_number)].Value = item.Item_number;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Item_text)].Value = item.Item_text;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Material)].Value = item.Material;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Material_group)].Value = item.Material_group;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Plant)].Value = item.Plant;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Priority)].Value = item.Priority;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Quantity)].Value = item.Quantity;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Sub_items)].Value = item.Sub_items;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Udt)].Value = cmDt;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_UOM)].Value = item.UOM;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Usage_probability)].Value = item.Usage_probability;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Valid_from_date)].Value = item.Valid_from_date;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Valid_to_date)].Value = item.Valid_to_date;
        //        _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        ////public void ChangeModelBOM(ModelBOM item, int oldId)
        ////{
        ////    try
        ////    {
        ////        _Schema.SQLContext sqlCtx = null;
        ////        lock (MethodBase.GetCurrentMethod())
        ////        {
        ////              if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        ////              {
        ////                  _Schema.ModelBOM cond = new _Schema.ModelBOM();
        ////                  cond.ID = oldId;
        ////                  sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), null, null, null, null, cond, null, null, null, null, null, null, null);
        ////              }
        ////        }
        ////        sqlCtx.Params[_Schema.ModelBOM.fn_ID].Value = oldId;
        ////        DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        ////        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Alternative_item_group)].Value = item.Alternative_item_group;
        ////        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Base_qty)].Value = item.Base_qty;
        ////        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Bom)].Value = item.Bom;
        ////        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Cdt)].Value = cmDt;
        ////        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Change_number)].Value = item.Change_number;
        ////        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Component)].Value = item.Component;
        ////        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Editor)].Value = item.Editor;
        ////        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_ID)].Value = item.ID;
        ////        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Item_number)].Value = item.Item_number;
        ////        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Item_text)].Value = item.Item_text;
        ////        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Material)].Value = item.Material;
        ////        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Material_group)].Value = item.Material_group;
        ////        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Plant)].Value = item.Plant;
        ////        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Priority)].Value = item.Priority;
        ////        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Quantity)].Value = item.Quantity;
        ////        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Sub_items)].Value = item.Sub_items;
        ////        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Udt)].Value = cmDt;
        ////        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_UOM)].Value = item.UOM;
        ////        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Usage_probability)].Value = item.Usage_probability;
        ////        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Valid_from_date)].Value = item.Valid_from_date;
        ////        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Valid_to_date)].Value = item.Valid_to_date;
        ////        _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        ////    }
        ////    catch (Exception)
        ////    {
        ////        throw;
        ////    }
        ////}

        //public void IncludeItemToAlternativeItemGroup(string value, string parent, string code)
        //{
        //    //实现的SQL   UPDATE ModelBOM SET Alternative_item_group=”’+value+”’ where Material='" + parent + "' and Component='" + code + "'";
        //    try
        //    {
        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                cond.Material = parent;
        //                cond.Component = code;
        //                cond.Flag = 1;//ModelBOM表加Flag
        //                sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), new List<string>() { _Schema.ModelBOM.fn_Alternative_item_group }, null, null, null, cond, null, null, null, null, null, null, null);
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag; //ModelBOM表加Flag
        //            }
        //        }
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = parent;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = code;
        //        DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Alternative_item_group)].Value = value;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Udt)].Value = cmDt;
        //        _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public void AddModelBOM(ModelBOM item)
        //{
        //    try
        //    {
        //        SqlTransactionManager.Begin();

        //        //GYB: 新增记录的时候，需要按照Material/Component/Flag='0' 检查是否存在删除记录,如果有则Update,否则Insert
        //        if (PeekDeletedModelBOM(item))
        //            RecoverPartModelBOM(item);
        //        else
        //            InsertModelBOM(item);

        //        SqlTransactionManager.Commit();
        //    }
        //    catch (Exception)
        //    {
        //        SqlTransactionManager.Rollback();
        //        throw;
        //    }
        //    finally
        //    {
        //        SqlTransactionManager.Dispose();
        //        SqlTransactionManager.End();
        //    }
        //}

        //private bool PeekDeletedModelBOM(ModelBOM item)
        //{
        //    SqlDataReader sqlR = null;
        //    try
        //    {
        //        bool ret = false;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                cond.Material = item.Material;
        //                cond.Component = item.Component;
        //                cond.Flag = 1;
        //                sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), null, new List<string>() { _Schema.ModelBOM.fn_ID }, cond, null, null, null, null, null, null, null);

        //                sqlCtx.Sentence = sqlCtx.Sentence.Replace("WHERE", "WITH (UPDLOCK) WHERE");

        //                sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = 0;
        //            }
        //        }
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = item.Material;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = item.Component;
        //        sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        if (sqlR != null && sqlR.Read())
        //        {
        //            item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_ID]);
        //            ret = true;
        //        }
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (sqlR != null)
        //        {
        //            sqlR.Close();
        //        }
        //    }
        //}

        //private void RecoverPartModelBOM(ModelBOM item)
        //{
        //    try
        //    {
        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                cond.Flag = 1;
        //                cond.ID = item.ID;
        //                sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), null, new List<string>() { _Schema.ModelBOM.fn_ID, _Schema.ModelBOM.fn_Component, _Schema.ModelBOM.fn_Material }, null, null, cond, null, null, null, null, null, null, null);

        //                sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = 0;
        //            }
        //        }
        //        sqlCtx.Params[_Schema.ModelBOM.fn_ID].Value = item.ID;

        //        DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Alternative_item_group)].Value = item.Alternative_item_group;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Base_qty)].Value = item.Base_qty;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Bom)].Value = item.Bom;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Change_number)].Value = item.Change_number;
        //        //sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = item.Component;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Editor)].Value = item.Editor;
        //        //sqlCtx.Params[_Schema.ModelBOM.fn_ID].Value = item.ID;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Item_number)].Value = item.Item_number;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Item_text)].Value = item.Item_text;
        //        //sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = item.Material;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Material_group)].Value = item.Material_group;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Plant)].Value = item.Plant;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Priority)].Value = item.Priority;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Quantity)].Value = item.Quantity;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Sub_items)].Value = item.Sub_items;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Udt)].Value = cmDt;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_UOM)].Value = item.UOM;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Usage_probability)].Value = item.Usage_probability;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Valid_from_date)].Value = item.Valid_from_date;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Valid_to_date)].Value = item.Valid_to_date;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Flag)].Value = 1;
        //        _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //private void InsertModelBOM(ModelBOM item)
        //{
        //    try
        //    {
        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM));
        //            }
        //        }
        //        DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Alternative_item_group].Value = item.Alternative_item_group;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Base_qty].Value = item.Base_qty;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Bom].Value = item.Bom;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Cdt].Value = cmDt;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Change_number].Value = item.Change_number;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = item.Component;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Editor].Value = item.Editor;
        //        //sqlCtx.Params[_Schema.ModelBOM.fn_ID].Value = item.ID;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Item_number].Value = item.Item_number;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Item_text].Value = item.Item_text;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = item.Material;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Material_group].Value = item.Material_group;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Plant].Value = item.Plant;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Priority].Value = item.Priority;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Quantity].Value = item.Quantity;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Sub_items].Value = item.Sub_items;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Udt].Value = cmDt;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_UOM].Value = item.UOM;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Usage_probability].Value = item.Usage_probability;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Valid_from_date].Value = item.Valid_from_date;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Valid_to_date].Value = item.Valid_to_date;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = 1;//ModelBOM表加Flag
        //        item.ID = Convert.ToInt32( _Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public void DeleteModelBOMByCode(string parentCode, string code)
        //{
        //    //"DELETE FROM ModelBOM where Material='" + parentCode + "' and Component='"    + code + "'"
        //    try
        //    {
        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.ModelBOM eqCond = new _Schema.ModelBOM();
        //                eqCond.Material = parentCode;
        //                eqCond.Component = code;
        //                eqCond.Flag = 1;

        //                //sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), eqCond, null, null);
        //                sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), new List<string>() { _Schema.ModelBOM.fn_Flag }, null, null, null, eqCond, null, null, null, null, null, null, null);

        //                sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = eqCond.Flag;
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Flag)].Value = 0;
        //            }
        //        }
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = parentCode;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = code;
        //        DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Udt)].Value = cmDt;
        //        _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public IList<string> GetModelBOMByMaterialAndAlternativeItemGroup(string code, string itemGroup)
        //{
        //    //SELECT Component from ModelBOM where Material='code' and 
        //    //Alternative_item_group = 'itemGroup'
        //    try
        //    {
        //        IList<string> ret = new List<string>();

        //        _Schema.SQLContext sqlCtx = ComposeSubSQLForGetModelBOMByMaterialAndAlternativeItemGroup(code, itemGroup);
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = code;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Alternative_item_group].Value = itemGroup;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = 1;//ModelBOM表加Flag
        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            while (sqlR != null && sqlR.Read())
        //            {
        //                string item = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Component]);
        //                ret.Add(item);
        //            }
        //        }
        //        return ret;
        //    }
        //    catch(Exception)
        //    {
        //        throw;
        //    }
        //}

        //public void IncludeAllItemToAlternativeItemGroup(string code1, string code2, string itemGroup1, string itemGroup2)
        //{
        //    //UPDATE ModelBOM SET Alternative_item_group='itemGroup1' where Material='code1' and Component in 
        //    //(SELECT Component from ModelBOM where Material='code2' and 
        //    //Alternative_item_group = 'itemGroup2')
        //    try
        //    {
        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                cond.Material = code1;

        //                _Schema.ModelBOM insetCond = new _Schema.ModelBOM();
        //                insetCond.Component = "INSET";

        //                sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), new List<string>() { _Schema.ModelBOM.fn_Alternative_item_group }, null, null, null, cond, null, null, null, null, null, null, insetCond);

        //                _Schema.SQLContext sqlCtx_sub = ComposeSubSQLForGetModelBOMByMaterialAndAlternativeItemGroup(code2, itemGroup2);

        //                foreach (KeyValuePair<string, SqlParameter> value in sqlCtx_sub.Params)
        //                {
        //                    sqlCtx.Params.Add("s_" + value.Key, new SqlParameter("@s_" + value.Value.ParameterName.Substring(1), value.Value.SqlDbType));
        //                }
        //                sqlCtx.Sentence = sqlCtx.Sentence.Replace(_Schema.Func.DecInSet(_Schema.ModelBOM.fn_Component), sqlCtx_sub.Sentence.Replace("@" + _Schema.ModelBOM.fn_Material, "@s_" + _Schema.ModelBOM.fn_Material).Replace("@" + _Schema.ModelBOM.fn_Alternative_item_group, "@s_" + _Schema.ModelBOM.fn_Alternative_item_group).Replace("@" + _Schema.ModelBOM.fn_Flag, "@s_" + _Schema.ModelBOM.fn_Flag));//ModelBOM表加Flag
        //            }
        //        }
        //        DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = code1;
        //        sqlCtx.Params["s_" + _Schema.ModelBOM.fn_Material].Value = code2;
        //        sqlCtx.Params["s_" + _Schema.ModelBOM.fn_Alternative_item_group].Value = itemGroup2;
        //        sqlCtx.Params["s_" + _Schema.ModelBOM.fn_Flag].Value = 1;//ModelBOM表加Flag
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Alternative_item_group)].Value = itemGroup1;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Udt)].Value = cmDt;
        //        _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //private _Schema.SQLContext ComposeSubSQLForGetModelBOMByMaterialAndAlternativeItemGroup(string material, string alternativeItemGroup)
        //{
        //    _Schema.SQLContext sqlCtx = null;
        //    lock (MethodBase.GetCurrentMethod())
        //    {
        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        {
        //            _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //            cond.Material = material;
        //            cond.Alternative_item_group = alternativeItemGroup;
        //            cond.Flag = 1;//ModelBOM表加Flag
        //            sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "DISTINCT", new List<string>() { _Schema.ModelBOM.fn_Component }, cond, null, null, null, null, null, null, null);
        //        }
        //    }
        //    return sqlCtx;
        //}

        //public string ExcludeAlternativeItem(string parent, string code)
        //{
        //    ////UPDATE ModelBOM SET Alternative_item_group=(SELECT max(CONVERT(int,Alternative_item_group)) + 1 from ModelBOM) where Material='" + parent + "' and Component='" + code + "
        //    //UPDATE ModelBOM SET Alternative_item_group=newid() where Material='" + parent + "' and Component='" + code + "
        //    try
        //    {
        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                cond.Material = parent;
        //                cond.Component = code;
        //                cond.Flag = 1;//ModelBOM表加Flag

        //                sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), new List<string>() { _Schema.ModelBOM.fn_Alternative_item_group }, null, null, null, cond, null, null, null, null, null, null, null);

        //                //_Schema.SQLContext sqlCtx_sub = ComposeSubSQLForExcludeAlternativeItem();
        //                //sqlCtx.Sentence = sqlCtx.Sentence.Replace(sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Alternative_item_group)].ParameterName,string.Format("({0})+1",sqlCtx_sub.Sentence));
        //                //sqlCtx.Params.Remove(_Schema.Func.DecSV(_Schema.ModelBOM.fn_Alternative_item_group));

        //                sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag;//ModelBOM表加Flag
        //            }
        //        }
        //        DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //        Guid newGuid = Guid.NewGuid();
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = parent;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = code;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Alternative_item_group)].Value = newGuid.ToString();
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Udt)].Value = cmDt;
        //        _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        return newGuid.ToString();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        ////private _Schema.SQLContext ComposeSubSQLForExcludeAlternativeItem()
        ////{
        ////    //SELECT max(CONVERT(int,Alternative_item_group)) + 1 from ModelBOM
        ////    _Schema.SQLContext sqlCtx = null;
        //        //lock (MethodBase.GetCurrentMethod())
        //        //{
        ////    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        ////    {
        ////        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "MAX", new List<string>() { _Schema.ModelBOM.fn_Alternative_item_group }, null, null, null, null, null, null, null, null);
        ////    }
        ////}
        ////    return sqlCtx;
        ////}

        //public void DeleteSubModelByCode(string code)
        //{
        //    try
        //    {
        //        //"DELETE FROM ModelBOM where Material='" + code + "'";
        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                //ModelBOM表加Flag
        //                _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                cond.Material = code;
        //                cond.Flag = 1;
        //                //sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), cond, null, null);
        //                sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), new List<string>() { _Schema.ModelBOM.fn_Flag }, null, null, null, cond, null, null, null, null, null, null, null);

        //                sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag;              //ModelBOM表加Flag
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Flag)].Value = 0;  //ModelBOM表加Flag
        //            }
        //        }
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = code;
        //        DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Udt)].Value = cmDt;
        //        _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public DataTable GetModelBOMTypes()
        //{
        //    //SELECT distinct Material_group, flag=1 FROM ModelBOM  
        //    //union 
        //    //SELECT DISTINCT PartType , flag=0 
        //    //FROM Part a 
        //    //left join (SELECT DISTINCT Material_group FROM ModelBOM) as b 
        //    //on a.PartType=b.Material_group
        //    //WHERE b.Material_group is null
        //    //order by Material_group
        //    try
        //    {
        //        _Schema.SQLContext sqlCtx1 = ComposeSubSQLForGetModelBOMTypes_FromModelBOM();
        //        DataTable ret1 = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx1.Sentence, sqlCtx1.Params.Values.ToArray<SqlParameter>());

        //        _Schema.SQLContext sqlCtx2 = ComposeSubSQLForGetModelBOMTypes_Part();
        //        DataTable ret2 = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx2.Sentence, sqlCtx2.Params.Values.ToArray<SqlParameter>());

        //        var set1 = (from item in DataTableExtensions.AsEnumerable(ret1) select item).ToList();
        //        var set2 = (from item in DataTableExtensions.AsEnumerable(ret2) select item).ToList();

        //        var setU = (from item in set1.Union(set2) orderby item.Field<string>(0) select item).ToList();

        //        if (setU != null && setU.Count > 0)
        //            return DataTableExtensions.CopyToDataTable<DataRow>(setU);
        //        else
        //            return new DataTable();
        //    }
        //    catch(Exception)
        //    {
        //        throw;
        //    }
        //}

        //private _Schema.SQLContext ComposeSubSQLForGetModelBOMTypes_FromModelBOM()
        //{
        //    _Schema.SQLContext sqlCtx = null;
        //    lock (MethodBase.GetCurrentMethod())
        //    {
        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        {
        //            _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //            cond.Flag = 1;//ModelBOM表加Flag
        //            sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "DISTINCT", new List<string>() { _Schema.ModelBOM.fn_Material_group }, cond, null, null, null, null, null, null, null);
        //            sqlCtx.Sentence = sqlCtx.Sentence.Replace(" FROM ", ", 1 AS flag FROM ");
        //            sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag;//ModelBOM表加Flag
        //        }
        //    }
        //    return sqlCtx;
        //}

        //private _Schema.SQLContext ComposeSubSQLForGetModelBOMTypes_Part()
        //{
        //    //SELECT DISTINCT PartType, flag=0 
        //    //FROM Part A 
        //    //LEFT JOIN (SELECT DISTINCT Material_group FROM ModelBOM) AS B 
        //    //ON A.PartType=B.Material_group
        //    //WHERE B.Material_group IS NULL
        //    _Schema.SQLContext sqlCtx = null;
        //    lock (MethodBase.GetCurrentMethod())
        //    {
        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        {
        //            sqlCtx = new _Schema.SQLContext();

        //            sqlCtx.Sentence =   "SELECT DISTINCT {2}, flag=0 " +
        //                                "FROM {0} A " +
        //                                "LEFT JOIN (SELECT DISTINCT {3} FROM {1} WHERE {5}=1) AS B " +
        //                                "ON A.{2}=B.{3} " +
        //                                "WHERE B.{3} IS NULL AND A.{4}=1 ";//Part表加Flag

        //            sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.Part).Name,
        //                                                             typeof(_Schema.ModelBOM).Name,
        //                                                             _Schema.Part.fn_PartType,
        //                                                             _Schema.ModelBOM.fn_Material_group,
        //                                                             _Schema.Part.fn_Flag,//Part表加Flag
        //                                                             _Schema.ModelBOM.fn_Flag);//ModelBOM表加Flag

        //            _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);

        //            #region OLD
        //            //sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Part), "DISTINCT", new List<string>() { _Schema.Part.fn_PartType }, null, null, null, null, null, null, null, null);
        //            //sqlCtx.Sentence = sqlCtx.Sentence.Replace(" FROM ", ", 0 AS flag FROM ");
        //            //sqlCtx.Sentence = sqlCtx.Sentence + string.Format(" AND {0} NOT IN (SELECT DISTINCT {1} FROM ModelBOM) ", _Schema.Part.fn_PartType, _Schema.ModelBOM.fn_Material_group);
        //            #endregion
        //        }
        //    }
        //    return sqlCtx;
        //}

        //public DataTable GetModelBOMCodes(string parentCode, string match)
        //{
        //    //SELECT distinct Material, BOMApproveDate 
        //    //FROM ModelBOM mb left JOIN Model on (mb.Material = Model.Model) 
        //    //where Material_group=@parentCode
        //    //if (match != "")
        //    //{
        //    //   and Material like '%" + match + "%'";
        //    //}
        //    //order by Material
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        _Schema.TableAndFields tf1 = null;
        //        _Schema.TableAndFields tf2 = null;
        //        _Schema.TableAndFields[] tblAndFldsesArray = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
        //            {
        //                Console.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId);
        //                tf1 = new _Schema.TableAndFields();
        //                tf1.Table = typeof(_Schema.ModelBOM);
        //                _Schema.ModelBOM eqCond = new _Schema.ModelBOM();
        //                eqCond.Material_group = parentCode;
        //                eqCond.Flag = 1;//ModelBOM表加Flag
        //                tf1.equalcond = eqCond;
        //                _Schema.ModelBOM likeCond = new _Schema.ModelBOM();
        //                likeCond.Material = "%match%";
        //                tf1.likecond = likeCond;
        //                tf1.ToGetFieldNames.Add(_Schema.ModelBOM.fn_Material);

        //                tf2 = new _Schema.TableAndFields();
        //                tf2.Table = typeof(_Schema.Model);
        //                tf2.ToGetFieldNames.Add(_Schema.Model.fn_BOMApproveDate);
        //                tf2.ToGetFieldNames.Add(_Schema.Model.fn_Editor);

        //                List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
        //                _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.ModelBOM.fn_Material, tf2, _Schema.Model.fn_model);
        //                tblCnntIs.Add(tc1);

        //                _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

        //                tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2 };

        //                _Schema.TableBiJoinedLogic tblBiJndLgc = new _Schema.TableBiJoinedLogic();
        //                tblBiJndLgc.Add(tf1);
        //                tblBiJndLgc.Add(_Schema.Func.LEFTJOIN);
        //                tblBiJndLgc.Add(tf2);
        //                tblBiJndLgc.Add(tc1);

        //                sqlCtx = _Schema.Func.GetConditionedComprehensiveJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts, tblBiJndLgc);

        //                sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Func.DecAliasInner(tf1.alias, _Schema.ModelBOM.fn_Material));

        //                sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.ModelBOM.fn_Flag)].Value = eqCond.Flag;//ModelBOM表加Flag
        //            }
        //        }
        //        tf1 = tblAndFldsesArray[0];
        //        tf2 = tblAndFldsesArray[1];

        //        _Schema.SQLContext sqlCtx_final = new _Schema.SQLContext(sqlCtx);
        //        sqlCtx_final.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.ModelBOM.fn_Material_group)].Value = parentCode;
        //        if (string.IsNullOrEmpty(match))
        //        {
        //            sqlCtx_final.Sentence = sqlCtx_final.Sentence.Replace(_Schema.Func.DecAliasInner(tf1.alias, _Schema.ModelBOM.fn_Material) + " LIKE " + sqlCtx_final.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.ModelBOM.fn_Material)].ParameterName, "1=1");
        //            sqlCtx_final.Params.Remove(_Schema.Func.DecAlias(tf1.alias, _Schema.ModelBOM.fn_Material));
        //        }
        //        else
        //        {
        //            sqlCtx_final.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.ModelBOM.fn_Material)].Value = "%" + match  + "%";
        //        }
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx_final.Sentence, sqlCtx_final.Params.Values.ToArray<SqlParameter>());
        //        ret = _Schema.Func.SortColumns(ret, new int[] { sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias,_Schema.ModelBOM.fn_Material)],
        //                                                        sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias,_Schema.Model.fn_BOMApproveDate)],
        //                                                        sqlCtx.Indexes[_Schema.Func.DecAlias(tf2.alias,_Schema.Model.fn_Editor)]
        //                                                        });
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public DataTable GetParentModelBOMByCode(string code)
        //{
        //    //SELECT distinct Material, Material_group, Component FROM ModelBOM where Component=@code
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                cond.Component = code;
        //                cond.Flag = 1;//ModelBOM表加Flag
        //                sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "DISTINCT", new List<string>() { _Schema.ModelBOM.fn_Material, _Schema.ModelBOM.fn_Material_group, _Schema.ModelBOM.fn_Component }, cond, null, null, null, null, null, null, null);
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag;//ModelBOM表加Flag
        //            }
        //        }
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = code;
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        ret = _Schema.Func.SortColumns(ret, new int[] { sqlCtx.Indexes[_Schema.ModelBOM.fn_Material],
        //                                                        sqlCtx.Indexes[_Schema.ModelBOM.fn_Material_group],
        //                                                        sqlCtx.Indexes[_Schema.ModelBOM.fn_Component]
        //                                                        });    
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public DataTable GetSubModelBOMByCode(string code)
        //{
        //    //SELECT A.Component AS Material, B.Material_group, A.Quantity, A.Alternative_item_group FROM ModelBOM AS A
        //    //INNER JOIN (
        //    //SELECT DISTINCT Material, Material_group FROM ModelBOM 
        //    //UNION 
        //    //SELECT DISTINCT PartNo AS Material, PartType AS Material_group FROM Part
        //    //LEFT OUTER JOIN (SELECT DISTINCT Material FROM ModelBOM) AS C
        //    //ON Part.PartNo=C.Material WHERE C.Material IS NULL
        //    //) AS B ON A.Component=B.Material WHERE A.Material='code' ORDER BY A.Material
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                sqlCtx = new _Schema.SQLContext();

        //                sqlCtx.Sentence =   "SELECT A.{6} AS {2}, B.{3}, A.{7}, A.{8} FROM {0} AS A " + 
        //                                    "INNER JOIN (" +
        //                                    "SELECT DISTINCT {2}, {3} FROM {0} WHERE {10}=1 " + //ModelBOM表加Flag
        //                                    "UNION " + 
        //                                    "SELECT DISTINCT {4} AS {2}, {5} AS {3} FROM {1} " +
        //                                    "LEFT OUTER JOIN (SELECT DISTINCT {2} FROM {0} WHERE {10}=1 ) AS C " +
        //                                    "ON {1}.{4}=C.{2} WHERE C.{2} IS NULL AND {1}.{9}=1 " + //Part表加Flag
        //                                    ") AS B ON A.{6}=B.{2} WHERE A.{2}=@{2} AND A.{10}=1 ORDER BY A.{2}";
        //                sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.ModelBOM).Name,
        //                                                                typeof(_Schema.Part).Name,
        //                                                                _Schema.ModelBOM.fn_Material,
        //                                                                _Schema.ModelBOM.fn_Material_group,
        //                                                                _Schema.Part.fn_PartNo,
        //                                                                _Schema.Part.fn_PartType,
        //                                                                _Schema.ModelBOM.fn_Component,
        //                                                                _Schema.ModelBOM.fn_Quantity,
        //                                                                _Schema.ModelBOM.fn_Alternative_item_group,
        //                                                                _Schema.Part.fn_Flag,//Part表加Flag
        //                                                                _Schema.ModelBOM.fn_Flag);//ModelBOM表加Flag

        //                #region OLD
        //                //sqlCtx.Sentence = "SELECT a.{2} AS {3}, b.{4}, a.{7}, a.{8} " + 
        //                //                  "FROM {0} AS a INNER JOIN " +
        //                //                  "(SELECT DISTINCT {3}, {4} FROM {0} " + 
        //                //                  "UNION " + 
        //                //                  "SELECT DISTINCT {5} AS {3}, {6} AS {4} FROM {1} " +
        //                //                  "WHERE {5} NOT IN (SELECT {3} FROM {0})) AS b " +
        //                //                  "ON a.{2}=b.{3} AND a.{3}=@{3} ORDER BY a.{3} ";
    
        //                // sqlCtx.Sentence = string.Format(sqlCtx.Sentence,typeof(_Schema.ModelBOM).Name,
        //                //                                                typeof(_Schema.Part).Name,
        //                //                                                _Schema.ModelBOM.fn_Component,
        //                //                                                _Schema.ModelBOM.fn_Material,
        //                //                                                _Schema.ModelBOM.fn_Material_group,
        //                //                                                _Schema.Part.fn_PartNo,
        //                //                                                _Schema.Part.fn_PartType,
        //                //                                                _Schema.ModelBOM.fn_Quantity,
        //                //                                                _Schema.ModelBOM.fn_Alternative_item_group);
        //                #endregion

        //                sqlCtx.Params.Add(_Schema.ModelBOM.fn_Material, new SqlParameter("@" + _Schema.ModelBOM.fn_Material, SqlDbType.VarChar));

        //                _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
        //            }
        //        }
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = code;
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

        //        #region OLD
        //        //SELECT distinct Component as Material, Material_group, Quantity, Alternative_item_group FROM ModelBOM where Material=@code
        //        //_Schema.SQLContext sqlCtx = null;
        //        //lock (MethodBase.GetCurrentMethod())
        //        //{
        //        //    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        //    {
        //        //        _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //        //        cond.Material = code;
        //        //        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "DISTINCT", new List<string>() { _Schema.ModelBOM.fn_Component, _Schema.ModelBOM.fn_Material_group, _Schema.ModelBOM.fn_Quantity, _Schema.ModelBOM.fn_Alternative_item_group }, cond, null, null, null, null, null, null, null);
        //        //    }
        //        //}
        //        //sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = code;
        //        //ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        //ret = _Schema.Func.SortColumns(ret, new int[] { sqlCtx.Indexes[_Schema.ModelBOM.fn_Component],
        //        //                                    sqlCtx.Indexes[_Schema.ModelBOM.fn_Material_group],
        //        //                                    sqlCtx.Indexes[_Schema.ModelBOM.fn_Quantity],
        //        //                                    sqlCtx.Indexes[_Schema.ModelBOM.fn_Alternative_item_group]});
        //        #endregion

        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public DataTable GetAlternativeItems(string code, string alternativeItemGroup)
        //{
        //    //SELECT distinct Component as Material from ModelBOM where Material=@code and Alternative_item_group=@alternativeItemGroup
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                cond.Material = code;
        //                cond.Alternative_item_group = alternativeItemGroup;
        //                cond.Flag = 1;//ModelBOM表加Flag
        //                sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "DISTINCT", new List<string>() { _Schema.ModelBOM.fn_Component }, cond, null, null, null, null, null, null, null);
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag;//ModelBOM表加Flag 
        //            }
        //        }
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = code;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Alternative_item_group].Value = alternativeItemGroup;
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public DataTable GetOffspringModelBOM(string code)
        //{
        //    //SELECT distinct Material, AssemblyCode 
        //    //FROM ModelBOM left join MO on ModelBOM.Material=MO.Model left join MoBOM on MO.MO=MoBOM.MO 
        //    //where Material in (select Component from ModelBOM where Material=@code)
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        _Schema.TableAndFields tf1 = null;
        //        _Schema.TableAndFields tf2 = null;
        //        _Schema.TableAndFields tf3 = null;
        //        _Schema.TableAndFields[] tblAndFldsesArray = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
        //            {
        //                tf1 = new _Schema.TableAndFields();
        //                tf1.Table = typeof(_Schema.ModelBOM);
        //                _Schema.ModelBOM eqCond = new _Schema.ModelBOM();
        //                eqCond.Flag = 1;//ModelBOM表加Flag
        //                tf1.equalcond = eqCond;
        //                _Schema.ModelBOM insetCond = new _Schema.ModelBOM();
        //                insetCond.Material = "INSET";
        //                tf1.inSetcond = insetCond;
        //                tf1.ToGetFieldNames.Add(_Schema.ModelBOM.fn_Material);

        //                tf2 = new _Schema.TableAndFields();
        //                tf2.Table = typeof(_Schema.MO);
        //                tf2.ToGetFieldNames = null;

        //                tf3 = new _Schema.TableAndFields();
        //                tf3.Table = typeof(_Schema.MoBOM);
        //                tf3.ToGetFieldNames.Add(_Schema.MoBOM.fn_AssemblyCode);

        //                List<_Schema.TableConnectionItem> tblCnntIs = new List<_Schema.TableConnectionItem>();
        //                _Schema.TableConnectionItem tc1 = new _Schema.TableConnectionItem(tf1, _Schema.ModelBOM.fn_Material, tf2, _Schema.MO.fn_Model);
        //                tblCnntIs.Add(tc1);
        //                _Schema.TableConnectionItem tc2 = new _Schema.TableConnectionItem(tf2, _Schema.MO.fn_Mo, tf3, _Schema.MoBOM.fn_MO);
        //                tblCnntIs.Add(tc2);

        //                _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

        //                tblAndFldsesArray = new _Schema.TableAndFields[] { tf1, tf2, tf3 };

        //                _Schema.TableBiJoinedLogic tblBiJndLgc = new _Schema.TableBiJoinedLogic();
        //                tblBiJndLgc.Add(tf1);
        //                tblBiJndLgc.Add(_Schema.Func.LEFTJOIN);
        //                tblBiJndLgc.Add(tf2);
        //                tblBiJndLgc.Add(tc1);
        //                tblBiJndLgc.Add(_Schema.Func.LEFTJOIN);
        //                tblBiJndLgc.Add(tf3);
        //                tblBiJndLgc.Add(tc2);

        //                sqlCtx = _Schema.Func.GetConditionedComprehensiveJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts, tblBiJndLgc);

        //                sqlCtx.Params[_Schema.Func.DecAlias(tf1.alias, _Schema.ModelBOM.fn_Flag)].Value = eqCond.Flag;//ModelBOM表加Flag;

        //                _Schema.SQLContext sqlCtx_sub = ComposeSubSQLForGetComponentByMaterial(code);

        //                foreach (KeyValuePair<string, SqlParameter> value in sqlCtx_sub.Params)
        //                {
        //                    sqlCtx.Params.Add("s_" + value.Key, new SqlParameter("@s_" + value.Value.ParameterName.Substring(1), value.Value.SqlDbType));
        //                }
        //                sqlCtx.Sentence = sqlCtx.Sentence.Replace(_Schema.Func.DecAlias(tf1.alias, _Schema.Func.DecInSet(_Schema.ModelBOM.fn_Material)), sqlCtx_sub.Sentence.Replace("@" + _Schema.ModelBOM.fn_Material, "@s_" + _Schema.ModelBOM.fn_Material).Replace("@" + _Schema.ModelBOM.fn_Flag, "@s_" + _Schema.ModelBOM.fn_Flag));//ModelBOM表加Flag
        //            }
        //        }
        //        tf1 = tblAndFldsesArray[0];
        //        tf2 = tblAndFldsesArray[1];
        //        tf3 = tblAndFldsesArray[2];

        //        sqlCtx.Params["s_" + _Schema.ModelBOM.fn_Material].Value = code;
        //        sqlCtx.Params["s_" + _Schema.ModelBOM.fn_Flag].Value = 1;//ModelBOM表加Flag
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        ret = _Schema.Func.SortColumns(ret, new int[] { sqlCtx.Indexes[_Schema.Func.DecAlias(tf1.alias, _Schema.ModelBOM.fn_Material)],
        //                                                        sqlCtx.Indexes[_Schema.Func.DecAlias(tf3.alias, _Schema.MoBOM.fn_AssemblyCode)]
        //                                                        });                
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public DataTable GetMoBOMByModel(string model)
        //{
        //    //SELECT distinct MO, Qty, PrintQty as StartQty, Udt FROM MO where Model=@model
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.MO cond = new _Schema.MO();
        //                cond.Model = model;
        //                sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MO), "DISTINCT", new List<string>() { _Schema.MO.fn_Mo, _Schema.MO.fn_Qty, _Schema.MO.fn_Print_Qty, _Schema.MO.fn_Udt }, cond, null, null, null, null, null, null, null);
        //            }
        //        }
        //        sqlCtx.Params[_Schema.MO.fn_Model].Value = model;
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        ret = _Schema.Func.SortColumns(ret, new int[] { sqlCtx.Indexes[_Schema.MO.fn_Mo],
        //                                                        sqlCtx.Indexes[_Schema.MO.fn_Qty],
        //                                                        sqlCtx.Indexes[_Schema.MO.fn_Print_Qty],
        //                                                        sqlCtx.Indexes[_Schema.MO.fn_Udt]
        //                                                        });
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public DataTable GetMaterialsByCode(string code)
        //{
        //    //SELECT Material from ModelBOM where Material =@code
        //    //UNION 
        //    //SELECT PartNo as Material from Part where PartNo=@code
        //    try
        //    {
        //        _Schema.SQLContext sqlCtx1 = ComposeSubSQLForGetMaterialsByCode_FromModelBOM(code);
        //        sqlCtx1.Params[_Schema.ModelBOM.fn_Material].Value = code;
        //        sqlCtx1.Params[_Schema.ModelBOM.fn_Flag].Value = 1;//ModelBOM表加Flag
        //        DataTable ret1 = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx1.Sentence, sqlCtx1.Params.Values.ToArray<SqlParameter>());

        //        _Schema.SQLContext sqlCtx2 = ComposeSubSQLForGetMaterialsByCode_FromPart(code);
        //        sqlCtx2.Params[_Schema.Part.fn_PartNo].Value = code;
        //        sqlCtx2.Params[_Schema.Part.fn_Flag].Value = 1;//Part表加Flag
        //        DataTable ret2 = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx2.Sentence, sqlCtx2.Params.Values.ToArray<SqlParameter>());

        //        var set1 = (from item in DataTableExtensions.AsEnumerable(ret1) select item).ToList();
        //        var set2 = (from item in DataTableExtensions.AsEnumerable(ret2) select item).ToList();

        //        var setU = (from item in set1.Union(set2) select item).ToList();

        //        if (setU != null && setU.Count > 0)
        //            return DataTableExtensions.CopyToDataTable<DataRow>(setU);
        //        else
        //            return new DataTable();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //private _Schema.SQLContext ComposeSubSQLForGetMaterialsByCode_FromModelBOM(string code)
        //{
        //    _Schema.SQLContext sqlCtx = null;
        //    lock (MethodBase.GetCurrentMethod())
        //    {
        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        {
        //            _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //            cond.Material = code;
        //            cond.Flag = 1;//ModelBOM表加Flag
        //            sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "DISTINCT", new List<string>() { _Schema.ModelBOM.fn_Material }, cond, null, null, null, null, null, null, null);
        //        }
        //    }
        //    return sqlCtx;
        //}

        //private _Schema.SQLContext ComposeSubSQLForGetMaterialsByCode_FromPart(string code)
        //{
        //    _Schema.SQLContext sqlCtx = null;
        //    lock (MethodBase.GetCurrentMethod())
        //    {
        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        {
        //            _Schema.Part cond = new _Schema.Part();
        //            cond.PartNo = code;
        //            cond.Flag = 1;//Part表加Flag
        //            sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Part), "DISTINCT", new List<string>() { _Schema.Part.fn_PartNo }, cond, null, null, null, null, null, null, null);
        //        }
        //    }
        //    return sqlCtx;
        //}

        ////public DataTable GetMaterialByModel(string code)
        ////{
        ////    //SELECT Material FROM Model where Model=@code
        ////}

        //public DataTable GetMaterialById(string code)
        //{
        //    //SELECT Material,Material_group FROM ModelBOM where Material=@code
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                cond.Material = code;
        //                cond.Flag = 1;//ModelBOM表加Flag
        //                sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), null, new List<string>() { _Schema.ModelBOM.fn_Material, _Schema.ModelBOM.fn_Material_group }, cond, null, null, null, null, null, null, null);
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag; //ModelBOM表加Flag
        //            }
        //        }
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = code;
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        ret = _Schema.Func.SortColumns(ret, new int[] { sqlCtx.Indexes[_Schema.ModelBOM.fn_Material],
        //                                                        sqlCtx.Indexes[_Schema.ModelBOM.fn_Material_group]
        //                                                        });  
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public DataTable GetModelById(string code)
        //{
        //    //SELECT Model FROM Model where Model=@code
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.Model cond = new _Schema.Model();
        //                cond.model = code;
        //                sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Model), null, new List<string>() { _Schema.Model.fn_model }, cond, null, null, null, null, null, null, null);
        //            }
        //        }
        //        sqlCtx.Params[_Schema.Model.fn_model].Value = code;
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public DataTable GetComponentQuantityByMaterial(string code)
        //{
        //    //SELECT distinct Component, Quantity FROM ModelBOM where Material=@model
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                cond.Material = code;
        //                cond.Flag = 1;//ModelBOM表加Flag
        //                sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "DISTINCT", new List<string>() { _Schema.ModelBOM.fn_Component, _Schema.ModelBOM.fn_Quantity }, cond, null, null, null, null, null, null, null);
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag; //ModelBOM表加Flag
        //            }
        //        }
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = code;
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        ret = _Schema.Func.SortColumns(ret, new int[] { sqlCtx.Indexes[_Schema.ModelBOM.fn_Component],
        //                                                        sqlCtx.Indexes[_Schema.ModelBOM.fn_Quantity]
        //                                                        }); 
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public DataTable GetTypeOfCode(string code)
        //{
        //    //SELECT Material_group FROM  ModelBOM WHERE Material = 'code' 
        //    //UNION
        //    //SELECT PartType FROM  Part  WHERE PartNo not in (SELECT Material FROM ModelBOM ) AND  PartNo ='code'
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                sqlCtx = new _Schema.SQLContext();

        //                sqlCtx.Sentence = "SELECT {5} FROM {0} WHERE {2}=@Code AND {7}=1 " + //ModelBOM表加Flag
        //                                    "UNION " + 
        //                                    "SELECT {4} FROM {1} " + 
        //                                    "LEFT OUTER JOIN {0} ON {1}.{3}={0}.{2} " +
        //                                    "WHERE {0}.{2} IS NULL AND {3}=@Code AND {1}.{6}=1 AND {0}.{7}=1 ";//Part表加Flag//ModelBOM表加Flag

        //                sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.ModelBOM).Name,
        //                                                                 typeof(_Schema.Part).Name,
        //                                                                 _Schema.ModelBOM.fn_Material,
        //                                                                 _Schema.Part.fn_PartNo,
        //                                                                 _Schema.Part.fn_PartType,
        //                                                                 _Schema.ModelBOM.fn_Material_group,
        //                                                                 _Schema.Part.fn_Flag,//Part表加Flag
        //                                                                 _Schema.ModelBOM.fn_Flag//ModelBOM表加Flag
        //                                                                 );


        //                #region OLD
        //                //sqlCtx.Sentence = "SELECT {0} FROM {1} WHERE {2}=@Code " + 
        //                //                  "UNION " +
        //                //                  "SELECT {3} FROM {4} WHERE {5} NOT IN (SELECT {2} FROM {1}) AND {5}=@Code ";

        //                //sqlCtx.Sentence = string.Format(sqlCtx.Sentence, _Schema.ModelBOM.fn_Material_group,
        //                //                                                typeof(_Schema.ModelBOM).Name,
        //                //                                                _Schema.ModelBOM.fn_Material,
        //                //                                                _Schema.Part.fn_PartType,
        //                //                                                typeof(_Schema.Part).Name,
        //                //                                                _Schema.Part.fn_PartNo);
        //                #endregion

        //                sqlCtx.Params.Add("Code", new SqlParameter("@Code", SqlDbType.VarChar));

        //                _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
        //            }
        //        }
        //        sqlCtx.Params["Code"].Value = code;
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

        //        #region OLD
        //        //_Schema.SQLContext sqlCtx = null;
        //        //lock (MethodBase.GetCurrentMethod())
        //        //{
        //        //    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        //    {
        //        //        _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //        //        cond.Material = code;
        //        //        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "DISTINCT", new List<string>() { _Schema.ModelBOM.fn_Material_group }, cond, null, null, null, null, null, null, null);
        //        //    }
        //        //}
        //        //sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = code;
        //        //ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        #endregion

        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public DataTable GetAlternativeItemGroupBySpecial(string parent, string code)
        //{
        //    //SELECT distinct Alternative_item_group from ModelBOM where Material=@parent and Component=@code
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                cond.Material = parent;
        //                cond.Component = code;
        //                cond.Flag = 1;//ModelBOM表加Flag
        //                sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "DISTINCT", new List<string>() { _Schema.ModelBOM.fn_Alternative_item_group }, cond, null, null, null, null, null, null, null);
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag;//ModelBOM表加Flag
        //            }
        //        }
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = parent;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = code;
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public DataTable GetComponentByMaterial(string code)
        //{
        //    //SELECT distinct Component from ModelBOM where Material=@code
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContext sqlCtx = ComposeSubSQLForGetComponentByMaterial(code);
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = code;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = 1;//ModelBOM表加Flag
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        return ret;
        //    }
        //    catch(Exception)
        //    {
        //        throw;
        //    }
        //}

        //public DataTable GetPartById(string code)
        //{
        //    //SELECT [PartNo] FROM [Part] where [PartNo]='code'
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.Part cond = new _Schema.Part();
        //                cond.PartNo = code;
        //                cond.Flag = 1;//Part表加Flag
        //                sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Part), null, new List<string>() { _Schema.Part.fn_PartNo }, cond, null, null, null, null, null, null, null);

        //                sqlCtx.Params[_Schema.Part.fn_Flag].Value = cond.Flag;//Part表加Flag
        //            }
        //        }
        //        sqlCtx.Params[_Schema.Part.fn_PartNo].Value = code;
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public DataTable GetPartNoByType(string type, string match)
        //{
        //    //SELECT [PartNo] FROM [Part] where [PartType] ='type' and PartNo like '%" + match + "%' order by [PartNo] ";
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.Part cond = new _Schema.Part();
        //                cond.PartType = type;
        //                cond.Flag = 1;//Part表加Flag

        //                _Schema.Part likeCond = new _Schema.Part();
        //                likeCond.PartNo = "%" + match + "%";

        //                sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.Part), null, new List<string>() { _Schema.Part.fn_PartNo }, cond, likeCond, null, null, null, null, null, null);

        //                sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.Part.fn_PartNo);

        //                sqlCtx.Params[_Schema.Part.fn_Flag].Value = cond.Flag;//Part表加Flag
        //            }
        //        }
        //        sqlCtx.Params[_Schema.Part.fn_PartType].Value = type;
        //        sqlCtx.Params[_Schema.Part.fn_PartNo].Value = "%" + match + "%";
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //private _Schema.SQLContext ComposeSubSQLForGetComponentByMaterial(string code)
        //{
        //    _Schema.SQLContext sqlCtx = null;
        //    lock (MethodBase.GetCurrentMethod())
        //    {
        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        {
        //            _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //            cond.Material = code;
        //            cond.Flag = 1;//ModelBOM表加Flag
        //            sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "DISTINCT", new List<string>() { _Schema.ModelBOM.fn_Component }, cond, null, null, null, null, null, null, null);
        //        }
        //    }
        //    return sqlCtx;
        //}

        //public int getMaxGroupNo()
        //{
        //    try
        //    {
        //        int ret = -1;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM), "MAX", new List<string>() { _Schema.MoBOM.fn_Group }, null, null, null, null, null, null, null, null);
        //            }
        //        }
        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            if (sqlR != null && sqlR.Read())
        //            {
        //                ret = GetValue_Int32(sqlR, sqlCtx.Indexes["MAX"]);
        //            }
        //        }
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public void saveGroupNo(int mobomId, string mo, int group)
        //{
        //    try
        //    {
        //        SqlTransactionManager.Begin();

        //        DataChangeMediator.AddChangeDemand(this.GetACacheSignal(mo, CacheType.MOBOM));

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.MoBOM cond = new _Schema.MoBOM();
        //                cond.ID = mobomId;
        //                sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM), new List<string>() { _Schema.MoBOM.fn_Group }, null, null, null, cond, null, null, null, null, null, null, null);
        //            }
        //        }
        //        DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //        sqlCtx.Params[_Schema.MoBOM.fn_ID].Value = mobomId;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.MoBOM.fn_Group)].Value = group;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.MoBOM.fn_Udt)].Value = cmDt;
        //        _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

        //        SqlTransactionManager.Commit();
        //    }
        //    catch (Exception)
        //    {
        //        SqlTransactionManager.Rollback();
        //        throw;
        //    }
        //    finally
        //    {
        //        SqlTransactionManager.Dispose();
        //        SqlTransactionManager.End();
        //    }
        //}

        //public DateTime getMaxUdt(string mo)
        //{
        //    try
        //    {
        //        DateTime ret = DateTime.MinValue;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.MoBOM eqCond = new _Schema.MoBOM();
        //                eqCond.MO = mo;
        //                sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MoBOM), "MAX", new List<string>() { _Schema.MoBOM.fn_Udt }, eqCond, null, null, null, null, null, null, null);
        //            }
        //        }
        //        sqlCtx.Params[_Schema.MoBOM.fn_MO].Value = mo;
        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            if (sqlR != null)
        //            {
        //                if (sqlR.Read())
        //                {
        //                    ret = GetValue_DateTime(sqlR, sqlCtx.Indexes["MAX"]);
        //                }
        //            }
        //        }
        //        return ret;
        //    }
        //    catch(Exception)
        //    {
        //        throw;
        //    }
        //}

        //public IList<ModelBOM> findModelBomByMaterialAndComponent(string parentCode, string oldCode)
        //{
        //    //Select * from ModelBom where Material='" + parentCode + "' and Component='" + oldCode + "'"
        //    try
        //    {
        //        IList<ModelBOM> ret = new List<ModelBOM>();

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.ModelBOM eqCond = new _Schema.ModelBOM();
        //                eqCond.Material = parentCode;
        //                eqCond.Component = oldCode;
        //                eqCond.Flag = 1;//ModelBOM表加Flag
        //                sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), eqCond, null, null);
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = eqCond.Flag;//ModelBOM表加Flag
        //            }
        //        }
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = parentCode;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = oldCode;
        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            if (sqlR != null)
        //            {
        //                while (sqlR.Read())
        //                {
        //                    ModelBOM item = new ModelBOM();
        //                    item.Alternative_item_group = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Alternative_item_group]);
        //                    item.Base_qty = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Base_qty]);
        //                    item.Bom = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Bom]);
        //                    item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Cdt]);
        //                    item.Change_number = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Change_number]);
        //                    item.Component = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Component]);
        //                    item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Editor]);
        //                    item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_ID]);
        //                    item.Item_number = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Item_number]);
        //                    item.Item_text = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Item_text]);
        //                    item.Material = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Material]);
        //                    item.Material_group = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Material_group]);
        //                    item.Plant = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Plant]);
        //                    item.Priority = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Priority]);
        //                    item.Quantity = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Quantity]);
        //                    item.Sub_items = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Sub_items]);
        //                    item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Udt]);
        //                    item.UOM = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_UOM]);
        //                    item.Usage_probability = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Usage_probability]);
        //                    item.Valid_from_date = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Valid_from_date]);
        //                    item.Valid_to_date = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.ModelBOM.fn_Valid_to_date]);
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

        //public int ExcludeAlternativeItemToNull(string parent, string code)
        //{
        //    //UPDATE ModelBOM SET Alternative_item_group=NULL where Material='" + parent + "' and Component='" + code + "'
        //    try
        //    {
        //        int ret = 0;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                cond.Material = parent;
        //                cond.Component = code;
        //                cond.Flag = 1;//ModelBOM表加Flag
        //                sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), new List<string>() { _Schema.ModelBOM.fn_Alternative_item_group }, null, null, null, cond, null, null, null, null, null, null, null);
        //                sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag;//ModelBOM表加Flag
        //            }
        //        }
        //        DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = parent;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = code;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Alternative_item_group)].Value = null;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Udt)].Value = cmDt;
        //        ret = _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public DataTable GetPartCheckSettingList(string customer, string model)
        //{
        //    //如果model=="", [Model]='model'条件变成[Model]='model' OR [Model] IS NULL两个条件
        //    //SELECT [Station],[PartType],[ID],[Customer],[Model]
        //    //  FROM [IMES_GetData_Datamaintain].[dbo].[PartCheckSetting]
        //    // where [Customer]='customer' AND [Model]='model'
        //    //order by [Station],[PartType]
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContext sqlCtx = null;

        //        if (string.IsNullOrEmpty(model))
        //            sqlCtx = ComposeForGetPartCheckSettingList(customer);
        //        else
        //            sqlCtx = ComposeForGetPartCheckSettingList(customer, model);
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        ret = _Schema.Func.SortColumns(ret, new int[] { sqlCtx.Indexes[_Schema.PartCheckSetting.fn_WC],
        //                                                        sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Tp],
        //                                                        sqlCtx.Indexes[_Schema.PartCheckSetting.fn_ID],
        //                                                        sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Customer],
        //                                                        sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Model]});   
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //private _Schema.SQLContext ComposeForGetPartCheckSettingList(string customer, string model)
        //{
        //    _Schema.SQLContext sqlCtx = null;
        //    lock (MethodBase.GetCurrentMethod())
        //    {
        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        {
        //            _Schema.PartCheckSetting cond = new _Schema.PartCheckSetting();
        //            cond.Customer = customer;
        //            cond.Model = model;
        //            sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), null, new List<string>() { _Schema.PartCheckSetting.fn_WC, _Schema.PartCheckSetting.fn_Tp, _Schema.PartCheckSetting.fn_ID, _Schema.PartCheckSetting.fn_Customer, _Schema.PartCheckSetting.fn_Model }, cond, null, null, null, null, null, null, null);
        //            sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.PartCheckSetting.fn_WC, _Schema.PartCheckSetting.fn_Tp })); 
        //        }
        //    }
        //    sqlCtx.Params[_Schema.PartCheckSetting.fn_Customer].Value = customer;
        //    sqlCtx.Params[_Schema.PartCheckSetting.fn_Model].Value = model;
        //    return sqlCtx;
        //}

        //private _Schema.SQLContext ComposeForGetPartCheckSettingList(string customer)
        //{
        //    _Schema.SQLContext sqlCtx = null;
        //    lock (MethodBase.GetCurrentMethod())
        //    {
        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        {
        //            _Schema.PartCheckSetting cond = new _Schema.PartCheckSetting();
        //            cond.Customer = customer;
        //            _Schema.PartCheckSetting noecond = new _Schema.PartCheckSetting();
        //            noecond.Model = string.Empty;
        //            sqlCtx = _Schema.Func.GetConditionedFuncSelectExt(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), null, new List<string>() { _Schema.PartCheckSetting.fn_WC, _Schema.PartCheckSetting.fn_Tp, _Schema.PartCheckSetting.fn_ID, _Schema.PartCheckSetting.fn_Customer, _Schema.PartCheckSetting.fn_Model }, cond, null, null, null, null, null, null, null, noecond);
        //            sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.PartCheckSetting.fn_WC, _Schema.PartCheckSetting.fn_Tp }));
        //            sqlCtx.Params[_Schema.PartCheckSetting.fn_Model].Value = noecond.Model; 
        //        }
        //    }
        //    sqlCtx.Params[_Schema.PartCheckSetting.fn_Customer].Value = customer;
        //    return sqlCtx;
        //}

        //public void AddPartCheckSetting(PartCheckSetting partCheckSetting)
        //{
        //    try
        //    {
        //        SqlTransactionManager.Begin();

        //        DataChangeMediator.AddChangeDemand(this.GetACacheSignal(string.Empty, CacheType.PartCheckSetting));

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting));
        //            }
        //        }
        //        DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //        sqlCtx.Params[_Schema.PartCheckSetting.fn_Cdt].Value = cmDt;
        //        sqlCtx.Params[_Schema.PartCheckSetting.fn_Customer].Value = partCheckSetting.Customer;
        //        sqlCtx.Params[_Schema.PartCheckSetting.fn_Editor].Value = partCheckSetting.Editor;
        //        //sqlCtx.Params[_Schema.PartCheckSetting.fn_ID].Value = partCheckSetting.ID;
        //        sqlCtx.Params[_Schema.PartCheckSetting.fn_Model].Value = partCheckSetting.Model;
        //        sqlCtx.Params[_Schema.PartCheckSetting.fn_Tp].Value = partCheckSetting.Tp;
        //        sqlCtx.Params[_Schema.PartCheckSetting.fn_Udt].Value = cmDt;
        //        sqlCtx.Params[_Schema.PartCheckSetting.fn_WC].Value = partCheckSetting.WC;
        //        sqlCtx.Params[_Schema.PartCheckSetting.fn_ValueType].Value = partCheckSetting.ValueType; 
        //        partCheckSetting.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));

        //        SqlTransactionManager.Commit();
        //    }
        //    catch (Exception)
        //    {
        //        SqlTransactionManager.Rollback();
        //        throw;
        //    }
        //    finally
        //    {
        //        SqlTransactionManager.Dispose();
        //        SqlTransactionManager.End();
        //    }
        //}

        //public void SavePartCheckSetting(PartCheckSetting partCheckSetting)
        //{
        //    try
        //    {
        //        SqlTransactionManager.Begin();

        //        DataChangeMediator.AddChangeDemand(this.GetACacheSignal(string.Empty, CacheType.PartCheckSetting));

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting));
        //            }
        //        }
        //        DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //        sqlCtx.Params[_Schema.PartCheckSetting.fn_Customer].Value = partCheckSetting.Customer;
        //        sqlCtx.Params[_Schema.PartCheckSetting.fn_Editor].Value = partCheckSetting.Editor;
        //        sqlCtx.Params[_Schema.PartCheckSetting.fn_ID].Value = partCheckSetting.ID;
        //        sqlCtx.Params[_Schema.PartCheckSetting.fn_Model].Value = partCheckSetting.Model;
        //        sqlCtx.Params[_Schema.PartCheckSetting.fn_Tp].Value = partCheckSetting.Tp;
        //        sqlCtx.Params[_Schema.PartCheckSetting.fn_Udt].Value = cmDt;
        //        sqlCtx.Params[_Schema.PartCheckSetting.fn_WC].Value = partCheckSetting.WC;
        //        sqlCtx.Params[_Schema.PartCheckSetting.fn_ValueType].Value = partCheckSetting.ValueType; 
        //        _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

        //        SqlTransactionManager.Commit();
        //    }
        //    catch (Exception)
        //    {
        //        SqlTransactionManager.Rollback();
        //        throw;
        //    }
        //    finally
        //    {
        //        SqlTransactionManager.Dispose();
        //        SqlTransactionManager.End();
        //    }
        //}

        //public void DeletePartCheckSetting(PartCheckSetting partCheckSetting)
        //{
        //    try
        //    {
        //        SqlTransactionManager.Begin();

        //        DataChangeMediator.AddChangeDemand(this.GetACacheSignal(string.Empty, CacheType.PartCheckSetting));

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting));
        //            }
        //        }
        //        sqlCtx.Params[_Schema.PartCheckSetting.fn_ID].Value = partCheckSetting.ID;
        //        _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());

        //        SqlTransactionManager.Commit();
        //    }
        //    catch (Exception)
        //    {
        //        SqlTransactionManager.Rollback();
        //        throw;
        //    }
        //    finally
        //    {
        //        SqlTransactionManager.Dispose();
        //        SqlTransactionManager.End();
        //    }
        //}

        //public DataTable GetExistPartCheckSetting(string customer, string model, string station, string partType, string valueType, int id)
        //{
        //    //判断Station、Part Type的值对已经被Part Check List中其他Part Type使用
        //    //如果model=="", [Model]='model'条件变为[Model]='model' OR  [Model] IS NULL两个条件
        //    //如果station=="", [Station]='station' 条件变为[Station]='station'  OR  [Station] IS NULL两个条件
        //    //如果partType=="", [PartType]='partType'条件变为[PartType]='partType' OR  [PartType] IS NULL两个条件
        //    //如果valueType=="", [ValueType]='valueType'条件变为[ValueType]='valueType' OR  [ValueType] IS NULL两个条件
        //    //SELECT [ID] FROM [IMES_GetData].[dbo].[PartCheckSetting]
        //    //WHERE [Customer]='customer' AND [Model]='model'
        //    //AND [Station]='station' AND [PartType]='partType' AND ValueType='valueType'
        //    //AND ID<>'id'
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContextCollection sqlSet = new _Schema.SQLContextCollection();
        //        int i = 0;
        //        sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_customer(customer));
        //        sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_id(id));
        //        if (string.IsNullOrEmpty(model))
        //            sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_model());
        //        else
        //            sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_model(model));
        //        if (string.IsNullOrEmpty(station))
        //            sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_station());
        //        else
        //            sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_station(station));
        //        if (string.IsNullOrEmpty(partType))
        //            sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_partType());
        //        else
        //            sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_partType(partType));
        //        if (string.IsNullOrEmpty(valueType))
        //            sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_valueType());
        //        else
        //            sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_valueType(valueType));

        //        _Schema.SQLContext sqlCtx = sqlSet.MergeToOneAndQuery();
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //         throw;
        //    }
        //}

        //public DataTable GetExistPartCheckSetting(string customer, string model, string station, string partType, string valueType)
        //{
        //    //判断Station、Part Type的值对已经被Part Check List中其他Part Type使用
        //    //如果model=="", [Model]='model'条件变为[Model]='model' OR  [Model] IS NULL两个条件
        //    //如果station=="", [Station]='station' 条件变为[Station]='station'  OR  [Station] IS NULL两个条件
        //    //如果partType=="", [PartType]='partType'条件变为[PartType]='partType' OR  [PartType] IS NULL两个条件
        //    //如果valueType=="", [ValueType]='valueType'条件变为[ValueType]='valueType' OR  [ValueType] IS NULL两个条件
        //    //SELECT [ID] FROM [IMES_GetData].[dbo].[PartCheckSetting]
        //    //WHERE [Customer]='customer' AND [Model]='model'
        //    //AND [Station]='station' AND [PartType]='partType' AND ValueType='valueType'
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContextCollection sqlSet = new _Schema.SQLContextCollection();
        //        int i = 0;
        //        sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_customer(customer));
        //        if (string.IsNullOrEmpty(model))
        //            sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_model());
        //        else
        //            sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_model(model));
        //        if (string.IsNullOrEmpty(station))
        //            sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_station());
        //        else
        //            sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_station(station));
        //        if (string.IsNullOrEmpty(partType))
        //            sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_partType());
        //        else
        //            sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_partType(partType));
        //        if (string.IsNullOrEmpty(valueType))
        //            sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_valueType());
        //        else
        //            sqlSet.AddOne(i++, ComposeForGetExistPartCheckSetting_valueType(valueType));

        //        _Schema.SQLContext sqlCtx = sqlSet.MergeToOneAndQuery(); 
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //private _Schema.SQLContext ComposeForGetExistPartCheckSetting_customer(string customer)
        //{
        //    _Schema.SQLContext sqlCtx = null;
        //    lock (MethodBase.GetCurrentMethod())
        //    {
        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        {
        //            _Schema.PartCheckSetting cond = new _Schema.PartCheckSetting();
        //            cond.Customer = customer;
        //            sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), null, new List<string>() { _Schema.PartCheckSetting.fn_ID }, cond, null, null, null, null, null, null, null);
        //        }
        //    }
        //    sqlCtx.Params[_Schema.PartCheckSetting.fn_Customer].Value = customer;
        //    return sqlCtx;
        //}

        //private _Schema.SQLContext ComposeForGetExistPartCheckSetting_id(int id)
        //{
        //    _Schema.SQLContext sqlCtx = null;
        //    lock (MethodBase.GetCurrentMethod())
        //    {
        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        {
        //            _Schema.PartCheckSetting ncond = new _Schema.PartCheckSetting();
        //            ncond.ID = id;
        //            sqlCtx = _Schema.Func.GetConditionedFuncSelectWith3NotConds(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), null, new List<string>() { _Schema.PartCheckSetting.fn_ID }, null, null, null, null, null, null, null, null, ncond, null, null);
        //        }
        //    }
        //    sqlCtx.Params[_Schema.PartCheckSetting.fn_ID].Value = id;
        //    return sqlCtx;
        //}

        //private _Schema.SQLContext ComposeForGetExistPartCheckSetting_model(string model)
        //{
        //    _Schema.SQLContext sqlCtx = null;
        //    lock (MethodBase.GetCurrentMethod())
        //    {
        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        {
        //            _Schema.PartCheckSetting cond = new _Schema.PartCheckSetting();
        //            cond.Model = model;
        //            sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), null, new List<string>() { _Schema.PartCheckSetting.fn_ID }, cond, null, null, null, null, null, null, null);
        //        }
        //    }
        //    sqlCtx.Params[_Schema.PartCheckSetting.fn_Model].Value = model;
        //    return sqlCtx;
        //}

        //private _Schema.SQLContext ComposeForGetExistPartCheckSetting_model()
        //{
        //    _Schema.SQLContext sqlCtx = null;
        //    lock (MethodBase.GetCurrentMethod())
        //    {
        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        {
        //            _Schema.PartCheckSetting noecond = new _Schema.PartCheckSetting();
        //            noecond.Model = string.Empty;
        //            sqlCtx = _Schema.Func.GetConditionedFuncSelectExt(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), null, new List<string>() { _Schema.PartCheckSetting.fn_ID }, null, null, null, null, null, null, null, null, noecond);
        //        }
        //    }
        //    sqlCtx.Params[_Schema.PartCheckSetting.fn_Model].Value = string.Empty;
        //    return sqlCtx;
        //}

        //private _Schema.SQLContext ComposeForGetExistPartCheckSetting_station(string station)
        //{
        //    _Schema.SQLContext sqlCtx = null;
        //    lock (MethodBase.GetCurrentMethod())
        //    {
        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        {
        //            _Schema.PartCheckSetting cond = new _Schema.PartCheckSetting();
        //            cond.WC = station;
        //            sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), null, new List<string>() { _Schema.PartCheckSetting.fn_ID }, cond, null, null, null, null, null, null, null);
        //        }
        //    }
        //    sqlCtx.Params[_Schema.PartCheckSetting.fn_WC].Value = station;
        //    return sqlCtx;
        //}

        //private _Schema.SQLContext ComposeForGetExistPartCheckSetting_station()
        //{
        //    _Schema.SQLContext sqlCtx = null;
        //    lock (MethodBase.GetCurrentMethod())
        //    {
        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        {
        //            _Schema.PartCheckSetting noecond = new _Schema.PartCheckSetting();
        //            noecond.WC = string.Empty;
        //            sqlCtx = _Schema.Func.GetConditionedFuncSelectExt(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), null, new List<string>() { _Schema.PartCheckSetting.fn_ID }, null, null, null, null, null, null, null, null, noecond);
        //        }
        //    }
        //    sqlCtx.Params[_Schema.PartCheckSetting.fn_WC].Value = string.Empty;
        //    return sqlCtx;
        //}

        //private _Schema.SQLContext ComposeForGetExistPartCheckSetting_partType(string partType)
        //{
        //    _Schema.SQLContext sqlCtx = null;
        //    lock (MethodBase.GetCurrentMethod())
        //    {
        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        {
        //            _Schema.PartCheckSetting cond = new _Schema.PartCheckSetting();
        //            cond.Tp = partType;
        //            sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), null, new List<string>() { _Schema.PartCheckSetting.fn_ID }, cond, null, null, null, null, null, null, null);
        //        }
        //    }
        //    sqlCtx.Params[_Schema.PartCheckSetting.fn_Tp].Value = partType;
        //    return sqlCtx;
        //}

        //private _Schema.SQLContext ComposeForGetExistPartCheckSetting_partType()
        //{
        //    _Schema.SQLContext sqlCtx = null;
        //    lock (MethodBase.GetCurrentMethod())
        //    {
        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        {
        //            _Schema.PartCheckSetting noecond = new _Schema.PartCheckSetting();
        //            noecond.Tp = string.Empty;
        //            sqlCtx = _Schema.Func.GetConditionedFuncSelectExt(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), null, new List<string>() { _Schema.PartCheckSetting.fn_ID }, null, null, null, null, null, null, null, null, noecond);
        //        }
        //    }
        //    sqlCtx.Params[_Schema.PartCheckSetting.fn_Tp].Value = string.Empty;
        //    return sqlCtx;
        //}

        //private _Schema.SQLContext ComposeForGetExistPartCheckSetting_valueType(string valueType)
        //{
        //    _Schema.SQLContext sqlCtx = null;
        //    lock (MethodBase.GetCurrentMethod())
        //    {
        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        {
        //            _Schema.PartCheckSetting cond = new _Schema.PartCheckSetting();
        //            cond.ValueType = valueType;
        //            sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), null, new List<string>() { _Schema.PartCheckSetting.fn_ID }, cond, null, null, null, null, null, null, null);
        //        }
        //    }
        //    sqlCtx.Params[_Schema.PartCheckSetting.fn_ValueType].Value = valueType;
        //    return sqlCtx;
        //}

        //private _Schema.SQLContext ComposeForGetExistPartCheckSetting_valueType()
        //{
        //    _Schema.SQLContext sqlCtx = null;
        //    lock (MethodBase.GetCurrentMethod())
        //    {
        //        if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //        {
        //            _Schema.PartCheckSetting noecond = new _Schema.PartCheckSetting();
        //            noecond.ValueType = string.Empty;
        //            sqlCtx = _Schema.Func.GetConditionedFuncSelectExt(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), null, new List<string>() { _Schema.PartCheckSetting.fn_ID }, null, null, null, null, null, null, null, null, noecond);
        //        }
        //    }
        //    sqlCtx.Params[_Schema.PartCheckSetting.fn_ValueType].Value = string.Empty;
        //    return sqlCtx;
        //}

        //public PartCheckSetting FindPartCheckSettingById(int id)
        //{
        //    try
        //    {
        //        PartCheckSetting ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.PartCheckSetting cond = new _Schema.PartCheckSetting();
        //                cond.ID = id;
        //                sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting), cond, null, null);
        //            }
        //        }

        //        sqlCtx.Params[_Schema.PartCheckSetting.fn_ID].Value = id;

        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
        //        {
        //            if (sqlR != null)
        //            {
        //                if (sqlR.Read())
        //                {
        //                    ret = new PartCheckSetting();
        //                    ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Cdt]);
        //                    ret.Customer = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Customer]);
        //                    ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Editor]);
        //                    ret.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_ID]);
        //                    ret.Model = GetValue_StrForNullable(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Model]);
        //                    ret.Tp = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Tp]);
        //                    ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Udt]);
        //                    ret.ValueType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_ValueType]);
        //                    ret.WC = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_WC]);
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

        //public DataTable GetMaterialInfo(string pn)
        //{
        //    //SELECT [Model] AS PartNo,'' AS Descr,
        //    //       0 AS WHERETYPE
        //    //      ,[BOMApproveDate] AS [BOMApproveDate]
        //    //      ,[Editor]
        //    //      ,[Cdt]
        //    //      ,[Udt]
        //    //  FROM [IMES_GetData_Datamaintain].[dbo].[Model]
        //    //WHERE [Model]= 'pn'
        //    //union 
        //    //SELECT [PartNo] AS PartNo,Descr AS Descr,
        //    //       1 AS WHERETYPE,
        //    //       GetDate() AS [BOMApproveDate]
        //    //      ,[Editor]
        //    //      ,[Cdt]
        //    //      ,[Udt]
        //    //  FROM [IMES_GetData_Datamaintain].[dbo].[Part]
        //    //WHERE [PartNo]='pn' AND Flag=1
        //    //order by WHERETYPE
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                sqlCtx = new _Schema.SQLContext();

        //                sqlCtx.Sentence =       "SELECT {2} AS {7},'' AS {12} " + 
        //                                            ",0 AS WHERETYPE " + 
        //                                            ",{3} AS {3} " + 
        //                                            ",{4} " + 
        //                                            ",{5} " + 
        //                                            ",{6} " + 
        //                                        "FROM {0} " + 
        //                                        "WHERE {2}=@{7} " + 
        //                                    "UNION " +
        //                                        "SELECT {7} AS {7},{12} AS {12} " +
        //                                            ",1 AS WHERETYPE " + 
        //                                            ",GetDate() AS {3} " + 
        //                                            ",{8} " + 
        //                                            ",{9} " + 
        //                                            ",{10} " + 
        //                                        "FROM {1} " +
        //                                        "WHERE {7}=@{7} AND {11}=1 " + 
        //                                    "ORDER BY WHERETYPE ";

        //                sqlCtx.Sentence = string.Format(sqlCtx.Sentence,typeof(_Schema.Model).Name,
        //                                                                typeof(_Schema.Part).Name,
        //                                                                _Schema.Model.fn_model,
        //                                                                _Schema.Model.fn_BOMApproveDate,
        //                                                                _Schema.Model.fn_Editor,
        //                                                                _Schema.Model.fn_Cdt,
        //                                                                _Schema.Model.fn_Udt,
        //                                                                _Schema.Part.fn_PartNo,
        //                                                                _Schema.Part.fn_Editor,
        //                                                                _Schema.Part.fn_Cdt,
        //                                                                _Schema.Part.fn_Udt,
        //                                                                _Schema.Part.fn_Flag,
        //                                                                _Schema.Part.fn_Descr);

        //                sqlCtx.Params.Add(_Schema.Part.fn_PartNo, new SqlParameter("@" + _Schema.Part.fn_PartNo, SqlDbType.VarChar));

        //                _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
        //            }
        //        }
        //        sqlCtx.Params[_Schema.Part.fn_PartNo].Value = pn;
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public DataTable GetParentInfo(string current)
        //{
        //    //SELECT distinct a.Material, b.Descr, c.Flag, c.ApproveDate FROM ModelBOM  AS a 
        //    //left outer join 
        //    //(SELECT Descr, PartNo
        //    //  FROM [IMES_GetData_Datamaintain].[dbo].[Part]
        //    //WHERE Flag=1) AS b
        //    //ON a.Material=b.PartNo
        //    //left outer join 
        //    //(Select 'Model' AS Flag, BOMApproveDate AS ApproveDate, Model from Model) AS c
        //    //on a.Material=c.Model 
        //    //where a.Component='current' and a.Flag=1
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                sqlCtx = new _Schema.SQLContext();

        //                string strApproveDate = "ApproveDate";

        //                sqlCtx.Sentence =   "SELECT DISTINCT a.{3}, b.{5}, c.{4}, c.{11} FROM {0} AS a " + 
        //                                    "LEFT OUTER JOIN " + 
        //                                    "(SELECT {5}, {6} FROM {1} WHERE {7}=1) AS b " + 
        //                                    "ON a.{3}=b.{6} " +
        //                                    "LEFT OUTER JOIN " +
        //                                    "(SELECT '{8}' AS {4}, {10} AS {11}, {8} FROM {2}) AS c " +
        //                                    "ON a.{3}=c.{8} " +
        //                                    "WHERE a.{9}=@{9} AND a.{4}=1 ";

        //                sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.ModelBOM).Name,
        //                                                                typeof(_Schema.Part).Name,
        //                                                                typeof(_Schema.Model).Name,
        //                                                                _Schema.ModelBOM.fn_Material,
        //                                                                _Schema.ModelBOM.fn_Flag,
        //                                                                _Schema.Part.fn_Descr,
        //                                                                _Schema.Part.fn_PartNo,
        //                                                                _Schema.Part.fn_Flag,
        //                                                                _Schema.Model.fn_model,
        //                                                                _Schema.ModelBOM.fn_Component,
        //                                                                _Schema.Model.fn_BOMApproveDate,
        //                                                                strApproveDate);

        //                sqlCtx.Params.Add(_Schema.ModelBOM.fn_Component, new SqlParameter("@" + _Schema.ModelBOM.fn_Component, SqlDbType.VarChar));

        //                _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
        //            }
        //        }
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = current;
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public void DeleteModelBomByMaterialAndComponent(string parentCode, string oldCode)
        //{
        //    //DELETE FROM [IMES_GetData_Datamaintain].[dbo].[ModelBOM]
        //    //where Material='" + parentCode + "' and Component='" + oldCode + "' AND Flag=0
        //    try
        //    {
        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                cond.Material = parentCode;
        //                cond.Component = oldCode;
        //                cond.Flag = 1;
        //                sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), cond, null, null);

        //                sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = 0; 
        //            }
        //        }
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Component].Value = oldCode;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Material].Value = parentCode; 
        //        _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public void RemoveModelBOMById(int id, string editor)
        //{
        //    try
        //    {
        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.ModelBOM eqCond = new _Schema.ModelBOM();
        //                eqCond.ID = id;
        //                eqCond.Flag = 1;

        //                sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), new List<string>() { _Schema.ModelBOM.fn_Flag, _Schema.ModelBOM.fn_Editor }, null, null, null, eqCond, null, null, null, null, null, null, null);

        //                sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = eqCond.Flag;
        //                sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Flag)].Value = 0;
        //            }
        //        }
        //        sqlCtx.Params[_Schema.ModelBOM.fn_ID].Value = id;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Editor)].Value = editor;
        //        DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Udt)].Value = cmDt;
        //        _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public void SetNewAlternativeGroup(IList<int> ids, string editor)
        //{
        //    //UPDATE ModelBOM SET Alternative_item_group=newid() where ID in(idlist) AND Flag=1
        //    try
        //    {
        //        if (ids != null && ids.Count > 0)
        //        {
        //            Guid newGuid = Guid.NewGuid();
        //            IList<int> batch = new List<int>();
        //            int i = 0;
        //            foreach (int entry in ids)
        //            {
        //                batch.Add(entry);
        //                if ((i + 1) % batchSQLCnt == 0 || i == ids.Count - 1)
        //                {
        //                    SetNewAlternativeGroupBatch_Inner(batch, editor, newGuid);
        //                    batch.Clear();
        //                }
        //                i++;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //private void SetNewAlternativeGroupBatch_Inner(IList<int> ids, string editor, Guid newGuid)
        //{
        //    try
        //    {
        //        if (ids != null && ids.Count > 0)
        //        {
        //            #region
        //            //_Schema.SQLContextCollection sqlCtxSet = new _Schema.SQLContextCollection();
        //            //int i = 0;
        //            //foreach (int entry in ids)
        //            //{
        //            //    _Schema.SQLContext sqlCtx = ComposeForSetNewAlternativeGroup(entry);
        //            //    sqlCtxSet.AddOne(i, sqlCtx);
        //            //    i++;
        //            //}
        //            //_Schema.SQLContext sqlCtxBatch = sqlCtxSet.MergeToOneNonQuery();
        //            //_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtxBatch.Sentence, sqlCtxBatch.Params.Values.ToArray<SqlParameter>());
        //            #endregion

        //            _Schema.SQLContext sqlCtx = null;
        //            sqlCtx = ComposeForSetNewAlternativeGroup(ids, editor, newGuid);
        //            _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //private _Schema.SQLContext ComposeForSetNewAlternativeGroup(IList<int> ids, string editor, Guid newGuid)
        //{
        //    try
        //    {
        //        _Schema.SQLContext ret = null;
        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.ModelBOM insetCond = new _Schema.ModelBOM();
        //                insetCond.ID = 1;//"INSET";
        //                _Schema.ModelBOM eqCond = new _Schema.ModelBOM();
        //                eqCond.Flag = 1;
        //                sqlCtx = _Schema.Func.GetConditionedUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), new List<string>() { _Schema.ModelBOM.fn_Alternative_item_group, _Schema.ModelBOM.fn_Editor }, null, null, null, eqCond, null, null, null, null, null, null, insetCond);

        //                sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = eqCond.Flag;
        //                //sqlCtx.Sentence = sqlCtx.Sentence.Replace(sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Alternative_item_group)].ParameterName, "LOWER(CONVERT(VARCHAR(255),NEWID()))");
        //                //sqlCtx.Params.Remove(_Schema.Func.DecSV(_Schema.ModelBOM.fn_Alternative_item_group));
        //            }
        //        }
        //        ret = new _Schema.SQLContext(sqlCtx);
        //        DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //        ret.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Editor)].Value = editor;
        //        ret.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Alternative_item_group)].Value = newGuid.ToString();
        //        ret.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Udt)].Value = cmDt;
        //        ret.Sentence = ret.Sentence.Replace(_Schema.Func.DecInSet(_Schema.ModelBOM.fn_ID), _Schema.Func.ConvertInSet(ids));
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public void DeleteRefreshModelByModel(string model, string editor)
        //{
        //    //Delete From RefreshModel where Model='model'AND Editor='editor'
        //    try
        //    {
        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.RefreshModel cond = new _Schema.RefreshModel();
        //                cond.model = model;
        //                cond.Editor = editor;
        //                sqlCtx = _Schema.Func.GetConditionedDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.RefreshModel), cond, null, null);
        //            }
        //        }
        //        sqlCtx.Params[_Schema.RefreshModel.fn_model].Value = model;
        //        sqlCtx.Params[_Schema.RefreshModel.fn_Editor].Value = editor;
        //        _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public DataTable GetExistRefreshModelByModel(string model, string editor)
        //{
        //    //SELECT Model FROM RefreshModel where Model='model'AND Editor='editor'
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.RefreshModel cond = new _Schema.RefreshModel();
        //                cond.model = model;
        //                cond.Editor = editor;
        //                sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.RefreshModel), null, new List<string>() { _Schema.RefreshModel.fn_model }, cond, null, null, null, null, null, null, null);
        //            }
        //        }
        //        sqlCtx.Params[_Schema.RefreshModel.fn_model].Value = model;
        //        sqlCtx.Params[_Schema.RefreshModel.fn_Editor].Value = editor;
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public void AddRefreshModel(string model, string editor)
        //{
        //    //INSERT INTO RefreshModel (Model, Editor) VALUES ('model','editor')
        //    try
        //    {
        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                sqlCtx = _Schema.Func.GetCommonInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.RefreshModel));
        //            }
        //        }
        //        sqlCtx.Params[_Schema.RefreshModel.fn_model].Value = model;
        //        sqlCtx.Params[_Schema.RefreshModel.fn_Editor].Value = editor;
        //        _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public DataTable GetRefreshModelList(string editor)
        //{
        //    //SELECT Model FROM RefreshModel WHERE Editor='editor'
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.RefreshModel cond = new _Schema.RefreshModel();
        //                cond.Editor = editor;
        //                sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.RefreshModel), null, new List<string>() { _Schema.RefreshModel.fn_model }, cond, null, null, null, null, null, null, null);
        //            }
        //        }
        //        sqlCtx.Params[_Schema.RefreshModel.fn_Editor].Value = editor;
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        ////public ModelBOM FindModelBOMByID(int id)
        ////{
        ////    try
        ////    {
        ////        ModelBOM ret = null;

        ////        ret = this.GetModelBOM(id);

        ////        return ret;
        ////    }
        ////    catch (Exception)
        ////    {
        ////        throw;
        ////    }
        ////}

        //public DataTable GetTreeTable(string model, int limitCount, ref int getStatus)
        //{
        //    try
        //    {
        //        DataTable ret = null;

        //        SqlParameter[] paramsArray = new SqlParameter[3];
        //        paramsArray[0] = new SqlParameter("@model", SqlDbType.VarChar);
        //        paramsArray[0].Value = model;
        //        paramsArray[1] = new SqlParameter("@limitCount", SqlDbType.Int);
        //        paramsArray[1].Value = limitCount;
        //        paramsArray[2] = new SqlParameter("@returnCode", SqlDbType.Int);
        //        paramsArray[2].Value = getStatus;
        //        paramsArray[2].Direction = ParameterDirection.Output;
        //        ret = _Schema.SqlHelper.ExecuteDataFillConsiderOutParams(_Schema.SqlHelper.ConnectionString_BOM, CommandType.StoredProcedure, "GetModelBOMAutoDL", paramsArray);
        //        getStatus = (int)paramsArray[2].Value;
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public DataTable GetPartTypeInfoByCode(string code)
        //{
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                sqlCtx = new _Schema.SQLContext();

        //                string strCode = "Code";

        //                sqlCtx.Sentence = "SELECT {2} AS {8}, {3} AS {6} FROM {0} WHERE {2}=@{8} AND {4}=1 " +
        //                                    "UNION " +
        //                                  "SELECT {5} AS {8}, {6} AS {6} FROM {1} WHERE {5}=@{8} AND {7}=1 ";


        //                sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.ModelBOM).Name,
        //                                                                typeof(_Schema.Part).Name,
        //                                                                _Schema.ModelBOM.fn_Material,
        //                                                                _Schema.ModelBOM.fn_Material_group,
        //                                                                _Schema.ModelBOM.fn_Flag,
        //                                                                _Schema.Part.fn_PartNo,
        //                                                                _Schema.Part.fn_PartType,
        //                                                                _Schema.Part.fn_Flag,
        //                                                                strCode);

        //                sqlCtx.Params.Add(strCode, new SqlParameter("@" + strCode, SqlDbType.VarChar));

        //                _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
        //            }
        //        }
        //        sqlCtx.Params["Code"].Value = code;
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        return ret;
        //    }
        //    catch(Exception)
        //    {
        //        throw;
        //    }
        //}

        //public DataTable GetSubModelBOMByCode(IList<int> idList)
        //{
        //    try
        //    {
        //        DataTable ret = null;

        //        if (idList != null && idList.Count > 0)
        //        {
        //            IList<int> batch = new List<int>();
        //            int i = 0;
        //            foreach (int it in idList)
        //            {
        //                batch.Add(it);
        //                if ((i + 1) % batchSQLCnt == 0 || i == idList.Count - 1)
        //                {
        //                    DataTable dt = GetSubModelBOMByCode_Inner(batch);
        //                    if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
        //                    {
        //                        if (ret == null)
        //                        {
        //                            ret = dt;
        //                        }
        //                        else
        //                        {
        //                            foreach(DataRow dr in dt.Rows)
        //                            {
        //                                ret.Rows.Add(dr.ItemArray);
        //                            }
        //                        }
        //                    }
        //                    batch.Clear();
        //                }
        //                i++;
        //            }
        //        }
        //        //2010-04-22 Liu Dong(eB1-4)         Modify ITC-1136-0133
        //        if (ret != null && ret.Rows != null && ret.Rows.Count > 0)
        //        {
        //            DataView dv = ret.DefaultView;
        //            dv.Sort = string.Format("{0},{1}",_Schema.ModelBOM.fn_Alternative_item_group,_Schema.ModelBOM.fn_Priority);
        //            ret = dv.ToTable();
        //        }
        //        //2010-04-22 Liu Dong(eB1-4)         Modify ITC-1136-0133
        //        return ret;
        //    }
        //    catch(Exception)
        //    {
        //        throw;
        //    }
        //}

        //private DataTable GetSubModelBOMByCode_Inner(IList<int> idList)
        //{
        //    try
        //    {
        //        DataTable ret = null;
                
        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                sqlCtx = new _Schema.SQLContext();

        //                string strCode = "Code";
        //                string strDataFromType = "dataFromType";

        //                sqlCtx.Sentence =   "SELECT a.{3} AS {11}, CASE WHEN b.{17} IS NULL THEN b.{14} ELSE c.{14} END AS {14}, a.{4}," +
        //                                    "a.{5}, a.{6} " +
        //                                    ",a.{7}, a.{8}, a.{9}, a.{2} FROM {0} AS a " +
        //                                    "LEFT OUTER JOIN " +
        //                                    "(" +
        //                                      "SELECT DISTINCT {11} AS {16}, {12} AS {14}, 0 AS {17} FROM {0} WHERE {10}=1" +
        //                                    ") AS b ON b.{16}=a.{3} " +
        //                                    "LEFT OUTER JOIN " +
        //                                    "(" +
        //                                      "SELECT {13} AS {16}, {14} AS {14} " +
        //                                      "FROM {1} WHERE {15}=1" +
        //                                    ") AS c ON c.{16}=a.{3} " +
        //                                    "WHERE a.{10}=1 AND a.{2} IN (INSET[{2}]) " +
        //                                    "ORDER BY a.{5}, a.{6}";

        //                sqlCtx.Sentence = string.Format(sqlCtx.Sentence,typeof(_Schema.ModelBOM).Name,
        //                                                                typeof(_Schema.Part).Name,
        //                                                                _Schema.ModelBOM.fn_ID,
        //                                                                _Schema.ModelBOM.fn_Component,
        //                                                                _Schema.ModelBOM.fn_Quantity,
        //                                                                _Schema.ModelBOM.fn_Alternative_item_group,
        //                                                                _Schema.ModelBOM.fn_Priority,
        //                                                                _Schema.ModelBOM.fn_Editor,
        //                                                                _Schema.ModelBOM.fn_Cdt,
        //                                                                _Schema.ModelBOM.fn_Udt,
        //                                                                _Schema.ModelBOM.fn_Flag,
        //                                                                _Schema.ModelBOM.fn_Material,
        //                                                                _Schema.ModelBOM.fn_Material_group,
        //                                                                _Schema.Part.fn_PartNo,
        //                                                                _Schema.Part.fn_PartType,
        //                                                                _Schema.Part.fn_Flag,
        //                                                                strCode,
        //                                                                strDataFromType);

        //                _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
        //            }
        //        }
        //        string Sentence = sqlCtx.Sentence.Replace(_Schema.Func.DecInSet(_Schema.ModelBOM.fn_ID), _Schema.Func.ConvertInSet(idList));
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, Sentence, null);
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public DataTable GetParantInfo(IList<string> currentComponents)
        //{
        //    try
        //    {
        //        DataTable ret = null;

        //        if (currentComponents != null && currentComponents.Count > 0)
        //        {
        //            IList<string> batch = new List<string>();
        //            int i = 0;
        //            foreach (string it in currentComponents)
        //            {
        //                batch.Add(it);
        //                if ((i + 1) % batchSQLCnt == 0 || i == currentComponents.Count - 1)
        //                {
        //                    DataTable dt = GetParantInfoe_Inner(batch);
        //                    if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
        //                    {
        //                        if (ret == null)
        //                        {
        //                            ret = dt;
        //                        }
        //                        else
        //                        {
        //                            foreach (DataRow dr in dt.Rows)
        //                            {
        //                                ret.Rows.Add(dr.ItemArray);
        //                            }
        //                        }
        //                    }
        //                    batch.Clear();
        //                }
        //                i++;
        //            }
        //        }
        //        if (ret != null && ret.Rows != null && ret.Rows.Count > 0)
        //        {
        //            DataView dv = ret.DefaultView;
        //            ret = dv.ToTable(true);
        //        }
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //private DataTable GetParantInfoe_Inner(IList<string> currentComponent)
        //{
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                sqlCtx = new _Schema.SQLContext();

        //                //string strCode = "Code";
        //                //string strDataFromType = "dataFromType";

        //                sqlCtx.Sentence =   "SELECT DISTINCT a.{3}, b.{6}, c.{4}, c.ApproveDate FROM {0} AS a " +
        //                                    "LEFT OUTER JOIN " +
        //                                    "(SELECT {6}, {7} FROM {1} WHERE {8}=1) AS b " +
        //                                    "ON a.{3}=b.{7} " +
        //                                    "LEFT OUTER JOIN " +
        //                                    "(SELECT 'Model' AS {4}, {9} AS ApproveDate, {10} FROM {2}) AS c " +
        //                                    "ON a.{3}=c.{10} " +
        //                                    "WHERE a.{5} IN (INSET[{5}]) AND a.{4}=1 ";

        //                sqlCtx.Sentence = string.Format(sqlCtx.Sentence, typeof(_Schema.ModelBOM).Name,
        //                                                                 typeof(_Schema.Part).Name,
        //                                                                 typeof(_Schema.Model).Name,
        //                                                                 _Schema.ModelBOM.fn_Material,
        //                                                                 _Schema.ModelBOM.fn_Flag,
        //                                                                 _Schema.ModelBOM.fn_Component,
        //                                                                 _Schema.Part.fn_Descr,
        //                                                                 _Schema.Part.fn_PartNo,
        //                                                                 _Schema.Part.fn_Flag,
        //                                                                 _Schema.Model.fn_BOMApproveDate,
        //                                                                 _Schema.Model.fn_model);

        //                _Schema.Func.InsertIntoCache(MethodBase.GetCurrentMethod().MetadataToken, sqlCtx);
        //            }
        //        }
        //        string Sentence = sqlCtx.Sentence.Replace(_Schema.Func.DecInSet(_Schema.ModelBOM.fn_Component), _Schema.Func.ConvertInSet(currentComponent));
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, Sentence, null);
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public DataTable GetComponentByMaterial(IList<string> codeList)
        //{
        //    try
        //    {
        //        DataTable ret = null;

        //        if (codeList != null && codeList.Count > 0)
        //        {
        //            IList<string> batch = new List<string>();
        //            int i = 0;
        //            foreach (string it in codeList)
        //            {
        //                batch.Add(it);
        //                if ((i + 1) % batchSQLCnt == 0 || i == codeList.Count - 1)
        //                {
        //                    DataTable dt = GetComponentByMaterial_Inner(batch);
        //                    if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
        //                    {
        //                        if (ret == null)
        //                        {
        //                            ret = dt;
        //                        }
        //                        else
        //                        {
        //                            foreach (DataRow dr in dt.Rows)
        //                            {
        //                                ret.Rows.Add(dr.ItemArray);
        //                            }
        //                        }
        //                    }
        //                    batch.Clear();
        //                }
        //                i++;
        //            }
        //        }
        //        if (ret != null && ret.Rows != null && ret.Rows.Count > 0)
        //        {
        //            DataView dv = ret.DefaultView;
        //            ret = dv.ToTable(true);
        //        }
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //private DataTable GetComponentByMaterial_Inner(IList<string> codeList)
        //{
        //    //SELECT distinct Component from ModelBOM where Material in 'codeList' And Flag=1;
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                cond.Flag = 1;
        //                _Schema.ModelBOM insetCond = new _Schema.ModelBOM();
        //                insetCond.Material = "INSET";
        //                sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), "DISTINCT", null, cond, null, null, null, null, null, null, insetCond);

        //                sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag;
        //            }
        //        }
        //        string Sentence = sqlCtx.Sentence.Replace(_Schema.Func.DecInSet(_Schema.ModelBOM.fn_Material), _Schema.Func.ConvertInSet(codeList));
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public void UpdateGroupQuantity(string qty, string groupNo, string editor)
        //{
        //    //UPDATE [IMES_GetData_Datamaintain].[dbo].[ModelBOM]
        //    //SET  [Quantity] = 'qty'
        //    //,[Editor] = 'editor'
        //    //,[Udt] = getdate()
        //    //WHERE [Alternative_item_group]='groupNo' And [Quantity]<> 'qty' and Flag=1
        //    try
        //    {
        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.ModelBOM cond = new _Schema.ModelBOM();
        //                cond.Alternative_item_group = groupNo;
        //                cond.Flag = 1;
        //                _Schema.ModelBOM neqCond = new _Schema.ModelBOM();
        //                neqCond.Quantity = qty;
        //                sqlCtx = _Schema.Func.GetConditionedUpdateWith3NotConds(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.ModelBOM), new List<string>() { _Schema.ModelBOM.fn_Quantity, _Schema.ModelBOM.fn_Editor }, null, null, null, cond, null, null, null, null, null, null, null, neqCond, null, null);

        //                sqlCtx.Params[_Schema.ModelBOM.fn_Flag].Value = cond.Flag;
        //            }
        //        }
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Alternative_item_group].Value = groupNo;
        //        sqlCtx.Params[_Schema.ModelBOM.fn_Quantity].Value = qty;

        //        DateTime cmDt = _Schema.SqlHelper.GetDateTime();
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Udt)].Value = cmDt;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Quantity)].Value = qty;
        //        sqlCtx.Params[_Schema.Func.DecSV(_Schema.ModelBOM.fn_Editor)].Value = editor;
        //        _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public IList<PartCheckSetting> GetPartCheckSettingList()
        //{
        //    //SELECT [Station],[Customer],[Model],[PartType],[ValueType],[Editor],[Cdt],[Udt],[ID]
        //    //  FROM [PartCheckSetting]
        //    //order by [Station],[Customer],[Model],[PartType],[ValueType]
        //    try
        //    {
        //        IList<PartCheckSetting> ret = new List<PartCheckSetting>();

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheckSetting));
        //                sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[] { _Schema.PartCheckSetting.fn_WC, _Schema.PartCheckSetting.fn_Customer, _Schema.PartCheckSetting.fn_Model, _Schema.PartCheckSetting.fn_Tp, _Schema.PartCheckSetting.fn_ValueType}));
        //            }
        //        }
        //        using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, null))
        //        {
        //            if (sqlR != null)
        //            {
        //                while (sqlR.Read())
        //                {
        //                    PartCheckSetting item = new PartCheckSetting();
        //                    item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Cdt]);
        //                    item.Customer = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Customer]);
        //                    item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Editor]);
        //                    item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_ID]);
        //                    item.Model = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Model]);
        //                    item.Tp = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Tp]);
        //                    item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_Udt]);
        //                    item.ValueType = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_ValueType]);
        //                    item.WC = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartCheckSetting.fn_WC]);
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

        //public DataTable GetValueTypeListByCustomerAndPartType(string customer, string partType)
        //{

        //    //SELECT distinct [ValueType]   
        //    //  FROM [IMES_GetData_Datamaintain].[dbo].[PartCheck]
        //    //  where Customer='customer' AND PartType='partType' 
        //    //order by [ValueType]
        //    try
        //    {
        //        DataTable ret = null;

        //        _Schema.SQLContext sqlCtx = null;
        //        lock (MethodBase.GetCurrentMethod())
        //        {
        //            if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
        //            {
        //                _Schema.PartCheck eqcond = new _Schema.PartCheck();
        //                eqcond.Customer = customer;
        //                eqcond.PartType = partType;
        //                sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartCheck), "DISTINCT", new List<string>() { _Schema.PartCheck.fn_ValueType }, eqcond, null, null, null, null, null, null, null);
        //                sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, _Schema.PartCheck.fn_ValueType);
        //            }
        //        }
        //        sqlCtx.Params[_Schema.PartCheck.fn_Customer].Value = customer;
        //        sqlCtx.Params[_Schema.PartCheck.fn_PartType].Value = partType;
        //        ret = _Schema.SqlHelper.ExecuteDataFill(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //#region Defered

        //public void AddMOBOMDefered(IUnitOfWork uow, MOBOM item)
        //{
        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        //}

        //public void UpdateMOBOMDefered(IUnitOfWork uow, MOBOM item, string oldMo)
        //{
        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item, oldMo);
        //}

        //public void DeleteMOBOMDefered(IUnitOfWork uow, MOBOM item)
        //{
        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        //}

        //public void CopyMOBOMDefered(IUnitOfWork uow, string mo)
        //{
        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), mo);
        //}

        //public void UpdateMOBOMForDeleteActionDefered(IUnitOfWork uow, string mo, string partNo, bool deviation)
        //{
        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), mo, partNo, deviation);
        //}

        //public void UpdateModelBOMDefered(IUnitOfWork uow, ModelBOM item)
        //{
        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        //}

        ////public void ChangeModelBOMDefered(IUnitOfWork uow, ModelBOM item, int oldId)
        ////{
        ////    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item, oldId);
        ////}

        //public void IncludeItemToAlternativeItemGroupDefered(IUnitOfWork uow, string value, string parent, string code)
        //{
        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), value, parent, code);
        //}

        //public void AddModelBOMDefered(IUnitOfWork uow, ModelBOM item)
        //{
        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        //}

        //public void DeleteModelBOMByCodeDefered(IUnitOfWork uow, string parentCode, string code)
        //{
        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), parentCode, code);
        //}

        //public void IncludeAllItemToAlternativeItemGroupDefered(IUnitOfWork uow, string parent, string code1, string code2)
        //{
        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), parent, code1, code2);
        //}

        //public void ExcludeAlternativeItemDefered(IUnitOfWork uow, string parent, string code)
        //{
        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), parent, code);
        //}

        //public void DeleteSubModelByCodeDefered(IUnitOfWork uow, string code)
        //{
        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), code);
        //}

        //public void saveGroupNoDefered(IUnitOfWork uow, int mobomId, string mo, int group)
        //{
        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), mobomId, mo, group);
        //}

        //public void DeleteMOBOMByMoDefered(IUnitOfWork uow, string mo)
        //{
        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), mo);
        //}

        //public void ExcludeAlternativeItemToNullDefered(IUnitOfWork uow, string parent, string code)
        //{
        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), parent, code);
        //}

        //public void AddPartCheckSettingDefered(IUnitOfWork uow, PartCheckSetting partCheckSetting)
        //{
        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), partCheckSetting);
        //}

        //public void SavePartCheckSettingDefered(IUnitOfWork uow, PartCheckSetting partCheckSetting)
        //{
        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), partCheckSetting);
        //}

        //public void DeletePartCheckSettingDefered(IUnitOfWork uow, PartCheckSetting partCheckSetting)
        //{
        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), partCheckSetting);
        //}

        //public void DeleteModelBomByMaterialAndComponentDefered(IUnitOfWork uow, string parentCode, string oldCode)
        //{
        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), parentCode, oldCode);
        //}

        //public void RemoveModelBOMByIdDefered(IUnitOfWork uow, int id, string editor)
        //{
        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), id, editor);
        //}

        //public void SetNewAlternativeGroupDefered(IUnitOfWork uow, IList<int> ids, string editor)
        //{
        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), ids, editor);
        //}

        //public void DeleteRefreshModelByModelDefered(IUnitOfWork uow, string model, string editor)
        //{
        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), model, editor);
        //}

        //public void AddRefreshModelDefered(IUnitOfWork uow, string model, string editor)
        //{
        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), model, editor);
        //}

        //public void UpdateGroupQuantityDefered(IUnitOfWork uow, string qty, string groupNo, string editor)
        //{
        //    AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), qty, groupNo, editor);
        //}

        //#endregion

        //#endregion

        #region

        public IList<ACAdaptor> GetAllACAdaptor()
        {
            try
            {
                IList<ACAdaptor> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetCommonSelect<AcAdapMaintain>(tk, AcAdapMaintain.fn_assemb);
                    }
                }

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<AcAdapMaintain, ACAdaptor, ACAdaptor>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<ACAdaptor> GetACAdaptorByAssembly(string Assembly)
        {
            try
            {
                IList<ACAdaptor> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        AcAdapMaintain cond = new AcAdapMaintain();
                        cond.assemb = Assembly + "%";
                        sqlCtx = FuncNew.GetConditionedSelect<AcAdapMaintain>(tk, null, null, new ConditionCollection<AcAdapMaintain>(new LikeCondition<AcAdapMaintain>(cond)), AcAdapMaintain.fn_assemb);
                    }
                }
                sqlCtx.Param(AcAdapMaintain.fn_assemb).Value = Assembly + "%";

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<AcAdapMaintain, ACAdaptor, ACAdaptor>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteSelectedACAdaptor(int id)
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
                        AcAdapMaintain cond = new AcAdapMaintain();
                        cond.id = id;
                        sqlCtx = FuncNew.GetConditionedDelete<AcAdapMaintain>(tk, new ConditionCollection<AcAdapMaintain>(new EqualCondition<AcAdapMaintain>(cond)));
                    }
                }
                sqlCtx.Param(AcAdapMaintain.fn_id).Value = id;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddOneAcAdaptor(ACAdaptor item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<AcAdapMaintain>(tk);
                    }
                }

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<AcAdapMaintain, ACAdaptor>(sqlCtx, item);

                sqlCtx.Param(AcAdapMaintain.fn_cdt).Value = cmDt;
                sqlCtx.Param(AcAdapMaintain.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateOneAcAdaptor(ACAdaptor newItem)
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
                        AcAdapMaintain cond = new AcAdapMaintain();
                        cond.id = newItem.id;
                        AcAdapMaintain setv = FuncNew.SetColumnFromField<AcAdapMaintain, ACAdaptor>(newItem, AcAdapMaintain.fn_id);
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<AcAdapMaintain>(tk, new SetValueCollection<AcAdapMaintain>(new CommonSetValue<AcAdapMaintain>(setv)), new ConditionCollection<AcAdapMaintain>(new EqualCondition<AcAdapMaintain>(cond)));
                    }
                }
                sqlCtx.Param(AcAdapMaintain.fn_id).Value = newItem.id;

                sqlCtx = FuncNew.SetColumnFromField<AcAdapMaintain, ACAdaptor>(sqlCtx, newItem, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(AcAdapMaintain.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<GradeInfo> GetAllGrades()
        {
            try
            {
                IList<GradeInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetCommonSelect<Hp_Grade>(tk, Hp_Grade.fn_family, Hp_Grade.fn_series);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Hp_Grade, GradeInfo, GradeInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<GradeInfo> GetGradesByFamily(string family)
        {
            try
            {
                IList<GradeInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Hp_Grade cond = new Hp_Grade();
                        cond.family = family;
                        sqlCtx = FuncNew.GetConditionedSelect<Hp_Grade>(tk, null, null, new ConditionCollection<Hp_Grade>(new EqualCondition<Hp_Grade>(cond)), Hp_Grade.fn_series);
                    }
                }
                sqlCtx.Param(Hp_Grade.fn_family).Value = family;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Hp_Grade, GradeInfo, GradeInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddSelectedGrade(GradeInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Hp_Grade>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<Hp_Grade, GradeInfo>(sqlCtx, item);

                sqlCtx.Param(Hp_Grade.fn_cdt).Value = cmDt;
                sqlCtx.Param(Hp_Grade.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteSelectedGrade(int id)
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
                        Hp_Grade cond = new Hp_Grade();
                        cond.id = id;
                        sqlCtx = FuncNew.GetConditionedDelete<Hp_Grade>(tk, new ConditionCollection<Hp_Grade>(new EqualCondition<Hp_Grade>(cond)));
                    }
                }
                sqlCtx.Param(Hp_Grade.fn_id).Value = id;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateSelectedGrade(GradeInfo item)
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
                        sqlCtx = FuncNew.GetCommonUpdate<Hp_Grade>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<Hp_Grade, GradeInfo>(sqlCtx, item);

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(Hp_Grade.fn_udt).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<BTOceanOrder> GetAllBTOceanOrder()
        {
            try
            {
                IList<BTOceanOrder> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetCommonSelect<Bt_Seashipmentsku>(tk, Bt_Seashipmentsku.fn_model);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Bt_Seashipmentsku, BTOceanOrder, BTOceanOrder>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<BTOceanOrder> GetListByPdLineAndModel(string pdLine, string model)
        {
            try
            {
                IList<BTOceanOrder> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Bt_Seashipmentsku cond = new Bt_Seashipmentsku();
                        cond.pdLine = pdLine;
                        cond.model = model;
                        sqlCtx = FuncNew.GetConditionedSelect<Bt_Seashipmentsku>(tk, null, null, new ConditionCollection<Bt_Seashipmentsku>(new EqualCondition<Bt_Seashipmentsku>(cond)), Bt_Seashipmentsku.fn_id);
                    }
                }
                sqlCtx.Param(Bt_Seashipmentsku.fn_pdLine).Value = pdLine;
                sqlCtx.Param(Bt_Seashipmentsku.fn_model).Value = model;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Bt_Seashipmentsku, BTOceanOrder, BTOceanOrder>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddBTOceanOrder(BTOceanOrder obj)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Bt_Seashipmentsku>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<Bt_Seashipmentsku, BTOceanOrder>(sqlCtx, obj);

                sqlCtx.Param(Bt_Seashipmentsku.fn_cdt).Value = cmDt;
                sqlCtx.Param(Bt_Seashipmentsku.fn_udt).Value = cmDt;

                obj.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteBTOceanOrderByPdlineAndModel(string pdLine, string model)
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
                        Bt_Seashipmentsku cond = new Bt_Seashipmentsku();
                        cond.pdLine = pdLine;
                        cond.model = model;
                        sqlCtx = FuncNew.GetConditionedDelete<Bt_Seashipmentsku>(tk, new ConditionCollection<Bt_Seashipmentsku>(new EqualCondition<Bt_Seashipmentsku>(cond)));
                    }
                }
                sqlCtx.Param(Bt_Seashipmentsku.fn_pdLine).Value = pdLine;
                sqlCtx.Param(Bt_Seashipmentsku.fn_model).Value = model;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateBTOceanOrderbyPdlineAndModel(BTOceanOrder obj, string pdLine, string model)
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
                        Bt_Seashipmentsku cond = new Bt_Seashipmentsku();
                        cond.pdLine = pdLine;
                        cond.model = model;
                        Bt_Seashipmentsku setv = FuncNew.SetColumnFromField<Bt_Seashipmentsku, BTOceanOrder>(obj, Bt_Seashipmentsku.fn_id, Bt_Seashipmentsku.fn_cdt);
                        setv.udt = DateTime.Now;
                        sqlCtx = FuncNew.GetConditionedUpdate<Bt_Seashipmentsku>(tk, new SetValueCollection<Bt_Seashipmentsku>(new CommonSetValue<Bt_Seashipmentsku>(setv)), new ConditionCollection<Bt_Seashipmentsku>(new EqualCondition<Bt_Seashipmentsku>(cond)));
                    }
                }
                sqlCtx.Param(Bt_Seashipmentsku.fn_pdLine).Value = pdLine;
                sqlCtx.Param(Bt_Seashipmentsku.fn_model).Value = model;

                sqlCtx = FuncNew.SetColumnFromField<Bt_Seashipmentsku, BTOceanOrder>(sqlCtx, obj, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(Bt_Seashipmentsku.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetTPsFromDescType()
        {
            try
            {
                IList<string> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetConditionedSelect<DescType>(tk, "DISTINCT", new string[] { DescType.fn_tp }, new ConditionCollection<DescType>(), DescType.fn_tp);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(DescType.fn_tp));
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

        public IList<DescTypeInfo> GetBOMDescrsByTp(string tp)
        {
            try
            {
                IList<DescTypeInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        DescType cond = new DescType();
                        cond.tp = tp;
                        sqlCtx = FuncNew.GetConditionedSelect<DescType>(tk, null, null, new ConditionCollection<DescType>(new EqualCondition<DescType>(cond)), DescType.fn_code);
                    }
                }
                sqlCtx.Param(DescType.fn_tp).Value = tp;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<DescType, DescTypeInfo, DescTypeInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<DescTypeInfo> GetBOMDescrsByCode(string code)
        {
            try
            {
                IList<DescTypeInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        DescType cond = new DescType();
                        cond.code = code;
                        sqlCtx = FuncNew.GetConditionedSelect<DescType>(tk, null, null, new ConditionCollection<DescType>(new EqualCondition<DescType>(cond)), DescType.fn_tp);
                    }
                }
                sqlCtx.Param(DescType.fn_code).Value = code;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<DescType, DescTypeInfo, DescTypeInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateBOMDescrById(DescTypeInfo item)
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
                        sqlCtx = FuncNew.GetCommonUpdate<DescType>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<DescType, DescTypeInfo>(sqlCtx, item);

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(DescType.fn_udt).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertBOMDescr(DescTypeInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<DescType>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<DescType, DescTypeInfo>(sqlCtx, item);

                sqlCtx.Param(DescType.fn_cdt).Value = cmDt;
                sqlCtx.Param(DescType.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteBOMDescrById(int id)
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
                        DescType cond = new DescType();
                        cond.id = id;
                        sqlCtx = FuncNew.GetConditionedDelete<DescType>(tk, new ConditionCollection<DescType>(new EqualCondition<DescType>(cond)));
                    }
                }
                sqlCtx.Param(DescType.fn_id).Value = id;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DescTypeInfo FindBOMDescrById(int id)
        {
            try
            {
                DescTypeInfo ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        DescType cond = new DescType();
                        cond.id = id;
                        sqlCtx = FuncNew.GetConditionedSelect<DescType>(tk, null, null, new ConditionCollection<DescType>(new EqualCondition<DescType>(cond)));
                    }
                }
                sqlCtx.Param(DescType.fn_id).Value = id;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<DescType, DescTypeInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteMoBOMByComponent(string component)
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
                        MoBOM_NEW cond = new MoBOM_NEW();
                        cond.component = component;
                        sqlCtx = FuncNew.GetConditionedDelete<MoBOM_NEW>(tk, new ConditionCollection<MoBOM_NEW>(new EqualCondition<MoBOM_NEW>(cond)));
                    }
                }
                sqlCtx.Param(MoBOM_NEW.fn_component).Value = component;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetPnListByModelAndBomNodeType(string model, string bomNodeType)
        {
            try
            {
                IList<string> ret = new List<string>();

                ITableAndFields tf1 = null;
                ITableAndFields tf2 = null;
                ITableAndFields[] tafa = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        tf1 = new TableAndFields<Part_NEW>();
                        Part_NEW cond = new Part_NEW();
                        cond.bomNodeType = bomNodeType;
                        cond.flag = 1;
                        tf1.Conditions.Add(new EqualCondition<Part_NEW>(cond));
                        tf1.AddRangeToGetFieldNames(Part_NEW.fn_partNo);

                        tf2 = new TableAndFields<ModelBOM_NEW>();
                        ModelBOM_NEW cond2 = new ModelBOM_NEW();
                        cond2.material = model;
                        cond2.flag = 1;
                        tf2.Conditions.Add(new EqualCondition<ModelBOM_NEW>(cond2));
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<Part_NEW, ModelBOM_NEW>(tf1, Part_NEW.fn_partNo, tf2, ModelBOM_NEW.fn_component));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts);

                        sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_flag)).Value = cond.flag;
                        sqlCtx.Param(g.DecAlias(tf2.Alias, ModelBOM_NEW.fn_flag)).Value = cond2.flag;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_bomNodeType)).Value = bomNodeType;
                sqlCtx.Param(g.DecAlias(tf2.Alias, ModelBOM_NEW.fn_material)).Value = model;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, Part_NEW.fn_partNo)));
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

        public IList<string> GetPnListByModelAndBomNodeType(string model, string bomNodeType, string descr)
        {
            try
            {
                IList<string> ret = new List<string>();

                ITableAndFields tf1 = null;
                ITableAndFields tf2 = null;
                ITableAndFields[] tafa = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        tf1 = new TableAndFields<Part_NEW>();
                        Part_NEW cond = new Part_NEW();
                        cond.bomNodeType = bomNodeType;
                        cond.descr = descr;
                        cond.flag = 1;
                        tf1.Conditions.Add(new EqualCondition<Part_NEW>(cond));
                        tf1.AddRangeToGetFieldNames(Part_NEW.fn_partNo);

                        tf2 = new TableAndFields<ModelBOM_NEW>();
                        ModelBOM_NEW cond2 = new ModelBOM_NEW();
                        cond2.material = model;
                        cond2.flag = 1;
                        tf2.Conditions.Add(new EqualCondition<ModelBOM_NEW>(cond2));
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<Part_NEW, ModelBOM_NEW>(tf1, Part_NEW.fn_partNo, tf2, ModelBOM_NEW.fn_component));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts);

                        sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_flag)).Value = cond.flag;
                        sqlCtx.Param(g.DecAlias(tf2.Alias, ModelBOM_NEW.fn_flag)).Value = cond2.flag;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_bomNodeType)).Value = bomNodeType;
                sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_descr)).Value = descr;
                sqlCtx.Param(g.DecAlias(tf2.Alias, ModelBOM_NEW.fn_material)).Value = model;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, Part_NEW.fn_partNo)));
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

        public IList<MoBOMInfo> GetPnListByModelAndBomNodeTypeAndDescr(string model, string bomNodeType, string descrPrefix)
        {
            try
            {
                IList<MoBOMInfo> ret = null;

                ITableAndFields tf1 = null;
                ITableAndFields tf2 = null;
                ITableAndFields[] tafa = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        tf1 = new TableAndFields<Part_NEW>();
                        Part_NEW cond = new Part_NEW();
                        cond.bomNodeType = bomNodeType;
                        cond.flag = 1;
                        tf1.Conditions.Add(new EqualCondition<Part_NEW>(cond));
                        Part_NEW cond3 = new Part_NEW();
                        cond3.descr = descrPrefix + "%";
                        tf1.Conditions.Add(new LikeCondition<Part_NEW>(cond3));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<ModelBOM_NEW>();
                        ModelBOM_NEW cond2 = new ModelBOM_NEW();
                        cond2.material = model;
                        cond2.flag = 1;
                        tf2.Conditions.Add(new EqualCondition<ModelBOM_NEW>(cond2));

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<Part_NEW, ModelBOM_NEW>(tf1, Part_NEW.fn_partNo, tf2, ModelBOM_NEW.fn_component));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts);

                        sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_flag)).Value = cond.flag;
                        sqlCtx.Param(g.DecAlias(tf2.Alias, ModelBOM_NEW.fn_flag)).Value = cond2.flag;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_bomNodeType)).Value = bomNodeType;
                sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_descr)).Value = descrPrefix + "%";
                sqlCtx.Param(g.DecAlias(tf2.Alias, ModelBOM_NEW.fn_material)).Value = model;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<ModelBOM_NEW, MoBOMInfo, MoBOMInfo>(ret, sqlR, sqlCtx, tf2.Alias);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetCTOBomDescr(string mpno, string spno)
        {
            try
            {
                IList<string> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Ctobom cond = new Ctobom();
                        cond.mpno = mpno;
                        cond.spno = spno;
                        sqlCtx = FuncNew.GetConditionedSelect<Ctobom>(tk, "DISTINCT", new string[] { Ctobom.fn_descr }, new ConditionCollection<Ctobom>(new EqualCondition<Ctobom>(cond)), Ctobom.fn_descr);
                    }
                }
                sqlCtx.Param(Ctobom.fn_mpno).Value = mpno;
                sqlCtx.Param(Ctobom.fn_spno).Value = spno;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();

                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(Ctobom.fn_descr));
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

        public IList<CtoBomInfo> GetCTOBomList(string mpno, string spno)
        {
            try
            {
                IList<CtoBomInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Ctobom cond = new Ctobom();
                        cond.mpno = mpno;
                        cond.spno = spno;
                        sqlCtx = FuncNew.GetConditionedSelect<Ctobom>(tk, null, null, new ConditionCollection<Ctobom>(new EqualCondition<Ctobom>(cond)), Ctobom.fn_id);
                    }
                }
                sqlCtx.Param(Ctobom.fn_mpno).Value = mpno;
                sqlCtx.Param(Ctobom.fn_spno).Value = spno;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Ctobom, CtoBomInfo, CtoBomInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetOsCodeFromBomCode(string pno)
        {
            try
            {
                IList<string> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Bom_Code cond = new Bom_Code();
                        cond.part_number = pno;
                        sqlCtx = FuncNew.GetConditionedSelect<Bom_Code>(tk, "DISTINCT", new string[] { Bom_Code.fn_os_code }, new ConditionCollection<Bom_Code>(new EqualCondition<Bom_Code>(cond)), Bom_Code.fn_os_code);
                    }
                }
                sqlCtx.Param(Bom_Code.fn_part_number).Value = pno;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();

                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(Bom_Code.fn_os_code));
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

        public IList<GradeInfo> GetGradesBySeries(string family)
        {
            try
            {
                IList<GradeInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        Hp_Grade cond = new Hp_Grade();
                        cond.family = family;
                        sqlCtx = FuncNew.GetConditionedSelect<Hp_Grade>(tk, null, null, new ConditionCollection<Hp_Grade>(new EqualCondition<Hp_Grade>(cond)), Hp_Grade.fn_id);
                    }
                }
                sqlCtx.Param(Hp_Grade.fn_family).Value = family;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Hp_Grade, GradeInfo, GradeInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetDescriptionOfDescTypeListByTpAndCode(string tp, string code)
        {
            try
            {
                IList<string> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        DescType cond = new DescType();
                        cond.tp = tp;
                        cond.code = code;
                        sqlCtx = FuncNew.GetConditionedSelect<DescType>(tk, "DISTINCT", new string[] { DescType.fn_description }, new ConditionCollection<DescType>(new EqualCondition<DescType>(cond)), DescType.fn_description);
                    }
                }
                sqlCtx.Param(DescType.fn_tp).Value = tp;
                sqlCtx.Param(DescType.fn_code).Value = code;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();

                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(DescType.fn_description));
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

        public IList<string> GetMaterialByComponent(string partNo)
        {
            try
            {
                IList<string> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        ModelBOM_NEW cond = new ModelBOM_NEW();
                        cond.component = partNo;
                        cond.flag = 1;
                        sqlCtx = FuncNew.GetConditionedSelect<ModelBOM_NEW>(tk, "DISTINCT", new string[] { ModelBOM_NEW.fn_material }, new ConditionCollection<ModelBOM_NEW>(new EqualCondition<ModelBOM_NEW>(cond)), ModelBOM_NEW.fn_material);

                        sqlCtx.Param(ModelBOM_NEW.fn_flag).Value = cond.flag;
                    }
                }
                sqlCtx.Param(ModelBOM_NEW.fn_component).Value = partNo;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(ModelBOM_NEW.fn_material));
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

        public IList<StationInfo> GetAllPartCollectionStation()
        {
            try
            {
                IList<StationInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetConditionedSelectForFuncedField<StationCheck>(tk, "DISTINCT", new string[][] { 
                            new string[]{StationCheck.fn_station, StationCheck.fn_station}, 
                            new string[]{StationCheck.fn_id, string.Format("(SELECT TOP 1 {0} FROM {1} t1 WHERE t1.{2}={3})", Station.fn_name, ToolsNew.GetTableName(typeof(Station)), Station.fn_station, StationCheck.fn_station)}
                            }, new ConditionCollection<StationCheck>(), StationCheck.fn_station);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<StationInfo>();
                        while (sqlR.Read())
                        {
                            var item = new StationInfo();
                            item.StationId = g.GetValue_Str(sqlR, sqlCtx.Indexes(StationCheck.fn_station));
                            item.Descr = g.GetValue_Str(sqlR, sqlCtx.Indexes(StationCheck.fn_id));
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

        #region . Defered .


        public void DeleteSelectedACAdaptorDefered(IUnitOfWork uow, int id)
        {
            InvokeBody.AddOneInvokeBody(this, uow, MethodBase.GetCurrentMethod(), id);
        }

        public void AddOneAcAdaptorDefered(IUnitOfWork uow, ACAdaptor item)
        {
            InvokeBody.AddOneInvokeBody(this, uow, MethodBase.GetCurrentMethod(), item);
        }

        public void UpdateOneAcAdaptorDefered(IUnitOfWork uow, ACAdaptor newItem)
        {
            InvokeBody.AddOneInvokeBody(this, uow, MethodBase.GetCurrentMethod(), newItem);
        }

        public void AddSelectedGradeDefered(IUnitOfWork uow, GradeInfo item)
        {
            InvokeBody.AddOneInvokeBody(this, uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeleteSelectedGradeDefered(IUnitOfWork uow, int id)
        {
            InvokeBody.AddOneInvokeBody(this, uow, MethodBase.GetCurrentMethod(), id);
        }

        public void UpdateSelectedGradeDefered(IUnitOfWork uow, GradeInfo item)
        {
            InvokeBody.AddOneInvokeBody(this, uow, MethodBase.GetCurrentMethod(), item);
        }

        public void AddBTOceanOrderDefered(IUnitOfWork uow, BTOceanOrder obj)
        {
            InvokeBody.AddOneInvokeBody(this, uow, MethodBase.GetCurrentMethod(), obj);
        }

        public void DeleteBTOceanOrderByPdlineAndModelDefered(IUnitOfWork uow, string pdLine, string model)
        {
            InvokeBody.AddOneInvokeBody(this, uow, MethodBase.GetCurrentMethod(), pdLine, model);
        }

        public void UpdateBTOceanOrderbyPdlineAndModelDefered(IUnitOfWork uow, BTOceanOrder obj, string pdLine, string model)
        {
            InvokeBody.AddOneInvokeBody(this, uow, MethodBase.GetCurrentMethod(), obj, pdLine, model);
        }

        public void UpdateBOMDescrByIdDefered(IUnitOfWork uow, DescTypeInfo item)
        {
            InvokeBody.AddOneInvokeBody(this, uow, MethodBase.GetCurrentMethod(), item);
        }

        public void InsertBOMDescrDefered(IUnitOfWork uow, DescTypeInfo item)
        {
            InvokeBody.AddOneInvokeBody(this, uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeleteBOMDescrByIdDefered(IUnitOfWork uow, int id)
        {
            InvokeBody.AddOneInvokeBody(this, uow, MethodBase.GetCurrentMethod(), id);
        }

        public void DeleteMoBOMByComponentDefered(IUnitOfWork uow, string component)
        {
            InvokeBody.AddOneInvokeBody(this, uow, MethodBase.GetCurrentMethod(), component);
        }

        #endregion

        #endregion

        #region IBOMRepository Members

        public IFlatBOM GetModelFlatBOMByStationModel(string customer, string station, string line, string family, string model, object mainObj)
        {
            try
            {
                if (line != null && line.Length > 1)
                    line = line.Substring(0, 1);

                IFlatBOM ret = null;

                IHierarchicalBOM hrchBom = GetHierarchicalBOMByModel(model);
                 IList<string> checkItemTypes = null;
                #region arrange this code to getCheckItemTypeByPriority function
               
                ////if (!string.IsNullOrEmpty(line))
                ////{
                ////    checkItemTypes = FindCheckItemTypeListFromStationCheck(station, line);
                ////}
                ////if (checkItemTypes == null || checkItemTypes.Count < 1)
                ////{
                ////    checkItemTypes = FindCheckItemTypeListFromStationCheckWithLineIsNull(station);
                ////}
                //StationCheckInfo condition = new StationCheckInfo();
                //condition.customer = customer;
                //condition.station = station;
                //var res = SttRepository.GetStationCheckInfoList(condition);
                //if (res != null && res.Count > 0)
                //{

                //    IList<CheckItemTypePriority> priorityList = new List<CheckItemTypePriority>();
                //    foreach (var item in res)
                //    {
                //        //Line+Model
                //        if (string.IsNullOrEmpty(item.family) &&
                //            !string.IsNullOrEmpty(item.line) &&
                //           (new Regex(item.line)).IsMatch(line) &&
                //            !string.IsNullOrEmpty(item.model) &&
                //            (new Regex(item.model)).IsMatch(model))
                //        {
                //            priorityList.Add(new CheckItemTypePriority() { Priority = 0, CheckItemType = item.checkItemType });
                //            continue;
                //        }
                //        //Line+Family
                //        if (string.IsNullOrEmpty(item.model) &&
                //             !string.IsNullOrEmpty(item.line) &&
                //            (new Regex(item.line)).IsMatch(line) &&
                //            !string.IsNullOrEmpty(item.family) &&
                //            (new Regex(item.family)).IsMatch(family))
                //        {
                //            priorityList.Add(new CheckItemTypePriority() { Priority = 1, CheckItemType = item.checkItemType });
                //            continue;
                //        }

                //        //Model
                //        if (string.IsNullOrEmpty(item.line) &&
                //            string.IsNullOrEmpty(item.family) &&
                //            !string.IsNullOrEmpty(item.model) &&
                //            (new Regex(item.model)).IsMatch(model))
                //        {
                //            priorityList.Add(new CheckItemTypePriority() { Priority = 2, CheckItemType = item.checkItemType });
                //            continue;
                //        }

                //        //Family
                //        if (string.IsNullOrEmpty(item.line) &&
                //             string.IsNullOrEmpty(item.model) &&
                //            !string.IsNullOrEmpty(item.family) &&
                //            (new Regex(item.family)).IsMatch(family))
                //        {
                //            priorityList.Add(new CheckItemTypePriority() { Priority = 3, CheckItemType = item.checkItemType });
                //            continue;
                //        }

                //        //Line
                //        if (!string.IsNullOrEmpty(item.line)
                //            && (new Regex(item.line)).IsMatch(line)
                //            && string.IsNullOrEmpty(item.family)
                //            && string.IsNullOrEmpty(item.model))
                //        {
                //            priorityList.Add(new CheckItemTypePriority() { Priority = 4, CheckItemType = item.checkItemType });
                //            continue;
                //        }

                //        //Line,Family,Model are all empty
                //        if (string.IsNullOrEmpty(item.line)
                //            && string.IsNullOrEmpty(item.family)
                //            && string.IsNullOrEmpty(item.model))
                //        {
                //            priorityList.Add(new CheckItemTypePriority() { Priority = 5, CheckItemType = item.checkItemType });
                //        }
                //    }

                //    //Vincent add Check Station/CheckItemType
                //    //Mantis 2499: StationCheck 檢查邏輯處理全部都不符合情況
                //    if (priorityList.Count > 0)
                //    {
                //        int priority = priorityList.Min(x => x.Priority);

                //        checkItemTypes = (from p in priorityList
                //                          where p.Priority == priority
                //                          select p.CheckItemType
                //                                     ).Distinct().ToList();
                //    }
                //}
                #endregion
                checkItemTypes = getCheckItemTypeByPriority(customer, station, line, family, model);
                if (checkItemTypes != null && checkItemTypes.Count > 0)
                {
                    foreach (string name in checkItemTypes)
                    {
                        IPartPolicy ppcy = PrtPcyRepository.GetPolicy(name);
                        if (ppcy == null)
                        {
                            throw new Exception("No setup CheckItemType:" + name + ", please check CheckItemType maintain!");
                        }
                        IFlatBOM fltBom = ppcy.FilterBOM(hrchBom, station, mainObj);
                        if (fltBom != null)
                        {
                            //Vincent[2014-04-17]:add policy object  in all FlatBomItem for different CheckItemType name and same dll name case
                            // save performance don't get policy data with each FlatBomItem
                            foreach (IFlatBOMItem item in fltBom.BomItems)
                            {
                                FlatBOMItem bomItem = item as FlatBOMItem;
                                if (bomItem != null)
                                {
                                    bomItem.GetType().GetField("_policy", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(bomItem, ppcy);
                                    
                                }
                            }

                            if (ret == null)
                            {
                                ret = fltBom;
                            }
                            else
                            {
                                ret.Merge(fltBom);
                            }
                        }
                    }
                }
                else
                {
                    throw new FisException("BOM001",new string[]{});
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
              
        public IHierarchicalBOM GetHierarchicalBOMByModel(string model)
        {
            try
            {
                if (!IsCached())
                    return GetHierarchicalBOMByModel_DB(model);

                IHierarchicalBOM bom = GetHierarchicalBOMByModel_Cache(model);
                if (bom == null)
                {
                    bom = GetHierarchicalBOMByModel_DB(model);
                    if (bom != null && MdlRepository.Find(model) != null)
                    {
                        lock (_syncObj_cache)
                        {
                            if (!_cache.Contains(model))
                                AddToCache(model, bom);
                        }
                    }
                }
                return bom;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IFlatBOM GetFlatBOMByPartTypeModel(string partType, string model, string station, object mainObj)
        {
            try
            {
                IFlatBOM ret = null;

                IHierarchicalBOM hrchBom = GetHierarchicalBOMByModel(model);

                IPartPolicy ppcy = PrtPcyRepository.GetPolicy(partType);
                IFlatBOM fltBom = ppcy.FilterBOM(hrchBom, station, mainObj);
                if (fltBom != null)
                {
                    //Vincent[2014-04-17]:add policy object  in all FlatBomItem for different CheckItemType name and same dll name case
                    // save performance don't get policy data with each FlatBomItem
                    foreach (FlatBOMItem item in fltBom.BomItems)
                    {
                        item.GetType().GetField("_policy", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(item, ppcy);
                    }

                    if (ret == null)
                    {
                        ret = fltBom;
                    }
                    else
                    {
                        ret.Merge(fltBom);
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IBOMNode> GetParentBomNode(string pn)
        {
            try
            {
                IList<IBOMNode> ret = null;

                IList<MoBOMInfo> parents = GetChildrenInModelBOMReverse(pn);
                if (parents != null)
                {
                    ret = new List<IBOMNode>();
                    foreach(MoBOMInfo parent in parents)
                    {
                        IPart parentPart = PrtRepository.Find(parent.material);
                        if (parentPart != null)
                        {
                            IBOMNode parentItem = new BOMNode(parentPart, int.Parse(parent.quantity), parent.alternative_item_group);
                            ret.Add(parentItem);
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

        public IList<IBOMNode> GetParentBomNodeByPnListAndBomNodeType(IList<string> pnList, string bomNodeType)
        {
            try
            {
                IList<IBOMNode> ret = null;

                IDictionary<string, IList<MoBOMInfo>> parents = GetChildrenInModelBOMReverse(pnList);
                if (parents != null)
                {
                    ret = new List<IBOMNode>();
                    foreach (KeyValuePair<string, IList<MoBOMInfo>> kvp in parents)
                    {
                        IPart parentPart = PrtRepository.Find(kvp.Key);
                        if (parentPart != null && parentPart.BOMNodeType == bomNodeType)
                        {
                            foreach (MoBOMInfo item in kvp.Value)
                            {
                                IBOMNode parentItem = new BOMNode(parentPart, int.Parse(item.quantity), item.alternative_item_group);
                                IPart childPart = PrtRepository.Find(item.component);
                                if (childPart != null)
                                {
                                    IBOMNode childItem = new BOMNode(childPart, -1);
                                    parentItem.AddChild(childItem);
                                }
                                ret.Add(parentItem);
                            }
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

        public IList<IBOMNode> GetParentBomNodeByPnListAndBomNodeType(IList<string> pnList)
        {
            try
            {
                IList<IBOMNode> ret = null;

                IDictionary<string, IList<MoBOMInfo>> parents = GetChildrenInModelBOMReverse(pnList);
                if (parents != null)
                {
                    ret = new List<IBOMNode>();
                    foreach (KeyValuePair<string, IList<MoBOMInfo>> kvp in parents)
                    {
                        IPart parentPart = PrtRepository.Find(kvp.Key);
                        if (parentPart != null)
                        {
                            foreach(MoBOMInfo item in kvp.Value)
                            {
                                IBOMNode parentItem = new BOMNode(parentPart, int.Parse(item.quantity), item.alternative_item_group);
                                IPart childPart = PrtRepository.Find(item.component);
                                if (childPart != null)
                                {
                                    IBOMNode childItem = new BOMNode(childPart, -1);
                                    parentItem.AddChild(childItem);
                                }
                                ret.Add(parentItem);
                            }
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

        #region Vincent disable Code
        //private IHierarchicalBOM GetHierarchicalBOMByModel_DB(string model)
        //{
        //    try
        //    {
        //        IHierarchicalBOM ret = null;

        //        IPart rootPart = PrtRepository.Find(model);               
        //        //if (rootPart != null)
        //        //{
        //            IBOMNode root = new BOMNode(rootPart, 1);
        //            ret = new HierarchicalBOM(root);
        //            IList<IBOMNode> allNodes = new List<IBOMNode>();
        //            ret.Nodes = allNodes;
        //            ret.Model = model;
        //            IList<MoBOMInfo> rootChildren = GetChildrenInModelBOM(model);
        //            if (rootChildren != null && rootChildren.Count > 0)
        //            {
        //                foreach(MoBOMInfo childItem in rootChildren)
        //                {
        //                    IPart nodePart = PrtRepository.Find(childItem.component);
        //                    if (nodePart != null)
        //                    {
        //                        IBOMNode newNode = new BOMNode(nodePart, int.Parse(childItem.quantity), childItem.alternative_item_group);
        //                        root.AddChild(newNode);
        //                        allNodes.Add(newNode);

        //                        int iDeepLimit = 20;
        //                        try
        //                        {
        //                            RecursivelyConstructHierarchicalBOM(newNode, childItem, allNodes, iDeepLimit - 1);
        //                        }
        //                        catch (FisException fex)
        //                        {
        //                            if (fex.mErrcode == "BOML01")
        //                            {
        //                                throw new FisException("BOML01", new string[] { iDeepLimit.ToString(), model });
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        //}
        //        return ret;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        #endregion

        private IHierarchicalBOM GetHierarchicalBOMByModel_Cache(string model)
        {
            try
            {
                IHierarchicalBOM bom = null;
                lock (_syncObj_cache)
                {
                    if (_cache.Contains(model))
                        bom = (IHierarchicalBOM)_cache[model];
                }
                return bom;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }
        }

        private void RecursivelyConstructHierarchicalBOM(IBOMNode node, MoBOMInfo peerInfo, IList<IBOMNode> allNodes, int iDeepLimit)
        {
            IList<MoBOMInfo> rootChildren = GetChildrenInModelBOM(peerInfo.component);
            if (rootChildren != null && rootChildren.Count > 0)
            {
                if (iDeepLimit == 0)
                {
                    throw new FisException("BOML01", new string[] { "", "" }); 
                }

                foreach (MoBOMInfo childItem in rootChildren)
                {
                    IPart nodePart = PrtRepository.Find(childItem.component);
                    if (nodePart != null)
                    {
                        IBOMNode newNode = new BOMNode(nodePart, int.Parse(childItem.quantity), childItem.alternative_item_group);
                        node.AddChild(newNode);
                        allNodes.Add(newNode);
                        RecursivelyConstructHierarchicalBOM(newNode, childItem, allNodes, iDeepLimit - 1);
                    }
                }
            }
        }

        private IList<MoBOMInfo> GetChildrenInModelBOM(string material)
        {
            try
            {
                IList<MoBOMInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        ModelBOM_NEW cond = new ModelBOM_NEW();
                        cond.material = material;
                        cond.flag = 1;
                        sqlCtx = FuncNew.GetConditionedSelect<ModelBOM_NEW>(tk, null, null, new ConditionCollection<ModelBOM_NEW>(new EqualCondition<ModelBOM_NEW>(cond)), ModelBOM_NEW.fn_priority);

                        sqlCtx.Param(ModelBOM_NEW.fn_flag).Value = cond.flag;
                    }
                }
                sqlCtx.Param(ModelBOM_NEW.fn_material).Value = material;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<ModelBOM_NEW, MoBOMInfo, MoBOMInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<MoBOMInfo> GetChildrenInModelBOMReverse(string componet)
        {
            try
            {
                IList<MoBOMInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        ModelBOM_NEW cond = new ModelBOM_NEW();
                        cond.component = componet;
                        cond.flag = 1;
                        sqlCtx = FuncNew.GetConditionedSelect<ModelBOM_NEW>(tk, null, null, new ConditionCollection<ModelBOM_NEW>(new EqualCondition<ModelBOM_NEW>(cond)), ModelBOM_NEW.fn_priority);

                        sqlCtx.Param(ModelBOM_NEW.fn_flag).Value = cond.flag;
                    }
                }
                sqlCtx.Param(ModelBOM_NEW.fn_component).Value = componet;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<ModelBOM_NEW, MoBOMInfo, MoBOMInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IDictionary<string, IList<MoBOMInfo>> GetChildrenInModelBOMReverse(IList<string> componets)
        {
            try
            {
                IDictionary<string, IList<MoBOMInfo>> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        ModelBOM_NEW cond = new ModelBOM_NEW();
                        cond.component = "[INSET]";
                        ModelBOM_NEW cond2 = new ModelBOM_NEW();
                        cond2.flag = 1;
                        sqlCtx = FuncNew.GetConditionedSelect<ModelBOM_NEW>(tk, null, null, new ConditionCollection<ModelBOM_NEW>(new InSetCondition<ModelBOM_NEW>(cond), new EqualCondition<ModelBOM_NEW>(cond2)), ModelBOM_NEW.fn_priority);
                        
                        sqlCtx.Param(ModelBOM_NEW.fn_flag).Value = cond2.flag;
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(ModelBOM_NEW.fn_component), g.ConvertInSet(componets));
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new Dictionary<string, IList<MoBOMInfo>>();
                        while (sqlR.Read())
                        {
                            MoBOMInfo item = null;
                            item = FuncNew.SetFieldFromColumnWithoutReadReader<ModelBOM_NEW, MoBOMInfo>(item, sqlR, sqlCtx);
                            IList<MoBOMInfo> set = null;
                            if (!ret.ContainsKey(item.material))
                            {
                                set = new List<MoBOMInfo>();
                                ret.Add(item.material, set);
                            }
                            else
                            {
                                set = ret[item.material];
                            }
                            set.Add(item);
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

        private IList<string> FindCheckItemTypeListFromStationCheck(string station, string line)
        {
            try
            {
                IList<string> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        StationCheck cond = new StationCheck();
                        cond.station = station;
                        cond.line = line;
                        sqlCtx = FuncNew.GetConditionedSelect<StationCheck>(tk, "DISTINCT", new string[] { StationCheck.fn_checkItemType }, new ConditionCollection<StationCheck>(new EqualCondition<StationCheck>(cond)), StationCheck.fn_checkItemType);
                    }
                }
                sqlCtx.Param(StationCheck.fn_station).Value = station;
                sqlCtx.Param(StationCheck.fn_line).Value = line;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();

                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(StationCheck.fn_checkItemType));
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

        private IList<string> FindCheckItemTypeListFromStationCheckWithLineIsNull(string station)
        {
            try
            {
                IList<string> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        StationCheck cond = new StationCheck();
                        cond.station = station;
                        StationCheck cond2 = new StationCheck();
                        cond2.line = "[NULL]";
                        sqlCtx = FuncNew.GetConditionedSelect<StationCheck>(tk, "DISTINCT", new string[] { StationCheck.fn_checkItemType }, new ConditionCollection<StationCheck>(
                            new EqualCondition<StationCheck>(cond),
                            new NullCondition<StationCheck>(cond2)), StationCheck.fn_checkItemType);
                    }
                }
                sqlCtx.Param(StationCheck.fn_station).Value = station;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();

                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(StationCheck.fn_checkItemType));
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

        //public IFlatBOM GetMoFlatBOMByStation(string station, string line, string mo)
        //{
        //    
        //}

        //public IHierarchicalBOM GetHierarchicalBOMByMo(string mo)
        //{
        //    
        //}

        public IList<string> GetPartInfoValueListByModelAndBomNodeTypeAndInfoType(string model, string bomNodeType, string infoType, string notEqInfoType, string notEqInfoValue)
        {
            try
            {
                IList<string> ret = new List<string>();

                ITableAndFields tf1 = null;
                ITableAndFields tf2 = null;
                ITableAndFields tf3 = null;
                ITableAndFields[] tafa = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        tf1 = new TableAndFields<Part_NEW>();
                        Part_NEW cond = new Part_NEW();
                        cond.bomNodeType = bomNodeType;
                        cond.flag = 1;
                        tf1.Conditions.Add(new EqualCondition<Part_NEW>(cond));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<ModelBOM_NEW>();
                        ModelBOM_NEW cond2 = new ModelBOM_NEW();
                        cond2.material = model;
                        cond2.flag = 1;
                        tf2.Conditions.Add(new EqualCondition<ModelBOM_NEW>(cond2));
                        tf2.ClearToGetFieldNames();

                        tf3 = new TableAndFields<_Metas.PartInfo>();
                        _Metas.PartInfo cond3 = new _Metas.PartInfo();
                        cond3.infoType = infoType;
                        tf3.Conditions.Add(new EqualCondition<_Metas.PartInfo>(cond3));
                        tf3.AddRangeToGetFieldNames(_Metas.PartInfo.fn_infoValue);

                        tafa = new ITableAndFields[] { tf1, tf2, tf3 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<Part_NEW, ModelBOM_NEW>(tf1, Part_NEW.fn_partNo, tf2, ModelBOM_NEW.fn_component),
                            new TableConnectionItem<Part_NEW, _Metas.PartInfo>(tf1, Part_NEW.fn_partNo, tf3, _Metas.PartInfo.fn_partNo)
                            );

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(" AND t1.{0} NOT IN (SELECT d.{1} FROM {2} d WHERE d.{1}=t1.{0} AND d.{3}=@notEq{3} AND d.{4}=@notEq{4})", Part_NEW.fn_partNo, _Metas.PartInfo.fn_partNo, ToolsNew.GetTableName(typeof(_Metas.PartInfo)), _Metas.PartInfo.fn_infoType, _Metas.PartInfo.fn_infoValue);
                        sqlCtx.AddParam(_Metas.PartInfo.fn_infoType, new SqlParameter("@notEq" + _Metas.PartInfo.fn_infoType, ToolsNew.GetDBFieldType<_Metas.PartInfo>(_Metas.PartInfo.fn_infoType)));
                        sqlCtx.AddParam(_Metas.PartInfo.fn_infoValue, new SqlParameter("@notEq" + _Metas.PartInfo.fn_infoValue, ToolsNew.GetDBFieldType<_Metas.PartInfo>(_Metas.PartInfo.fn_infoValue)));

                        sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_flag)).Value = cond.flag;
                        sqlCtx.Param(g.DecAlias(tf2.Alias, ModelBOM_NEW.fn_flag)).Value = cond2.flag;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];
                tf3 = tafa[2];

                sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_bomNodeType)).Value = bomNodeType;
                sqlCtx.Param(g.DecAlias(tf2.Alias, ModelBOM_NEW.fn_material)).Value = model;
                sqlCtx.Param(g.DecAlias(tf3.Alias, _Metas.PartInfo.fn_infoType)).Value = infoType;
                sqlCtx.Param(_Metas.PartInfo.fn_infoType).Value = notEqInfoType;
                sqlCtx.Param(_Metas.PartInfo.fn_infoValue).Value = notEqInfoValue;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf3.Alias, _Metas.PartInfo.fn_infoValue)));
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

        public bool CheckExistModelBOMByMaterialAndPartDescrLike(string material, string descrInStr)
        {
            try
            {
                bool ret = false;

                ITableAndFields tf1 = null;
                ITableAndFields tf2 = null;
                ITableAndFields[] tafa = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        tf1 = new TableAndFields<Part_NEW>();
                        Part_NEW cond0 = new Part_NEW();
                        cond0.flag = 1;
                        tf1.Conditions.Add(new EqualCondition<Part_NEW>(cond0));
                        Part_NEW cond = new Part_NEW();
                        cond.descr = "%" + descrInStr + "%";
                        tf1.Conditions.Add(new LikeCondition<Part_NEW>(cond, "UPPER({0})", "UPPER({0})"));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<ModelBOM_NEW>();
                        ModelBOM_NEW cond2 = new ModelBOM_NEW();
                        cond2.material = material;
                        cond2.flag = 1;
                        tf2.Conditions.Add(new EqualCondition<ModelBOM_NEW>(cond2));
                        tf2.AddRangeToGetFieldNames(ModelBOM_NEW.fn_id);

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<Part_NEW, ModelBOM_NEW>(tf1, Part_NEW.fn_partNo, tf2, ModelBOM_NEW.fn_component));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "COUNT", tafa, tblCnnts);

                        sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_flag)).Value = cond0.flag;
                        sqlCtx.Param(g.DecAlias(tf2.Alias, ModelBOM_NEW.fn_flag)).Value = cond2.flag;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_descr)).Value = "%" + descrInStr + "%";
                sqlCtx.Param(g.DecAlias(tf2.Alias, ModelBOM_NEW.fn_material)).Value = material;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        int cnt = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
                        ret = cnt > 0 ? true : false;
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<MoBOMInfo> GetModelBomList(MoBOMInfo condition)
        {
            try
            {
                IList<MoBOMInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::ModelBOM_NEW cond = mtns::FuncNew.SetColumnFromField<mtns::ModelBOM_NEW, MoBOMInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::ModelBOM_NEW>(null, null, new mtns::ConditionCollection<mtns::ModelBOM_NEW>(new mtns::EqualCondition<mtns::ModelBOM_NEW>(cond)), mtns::ModelBOM_NEW.fn_id);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::ModelBOM_NEW, MoBOMInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::ModelBOM_NEW, MoBOMInfo, MoBOMInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<MoBOMInfo> GetModelBomListByMaterials(string[] pnList)
        {
            try
            {
                IList<MoBOMInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::ModelBOM_NEW cond = new mtns::ModelBOM_NEW();
                        cond.material = "[INSET]";
                        mtns::ModelBOM_NEW cond1 = new mtns::ModelBOM_NEW();
                        cond1.flag = 1;
                        sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::ModelBOM_NEW>(tk, null, null, new mtns::ConditionCollection<mtns::ModelBOM_NEW>(new mtns::InSetCondition<mtns::ModelBOM_NEW>(cond), new EqualCondition<mtns::ModelBOM_NEW>(cond1)), mtns::ModelBOM_NEW.fn_material, mtns::ModelBOM_NEW.fn_component);
                        sqlCtx.Param(ModelBOM_NEW.fn_flag).Value = cond1.flag;
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(mtns::ModelBOM_NEW.fn_material), g.ConvertInSet(new List<string>(pnList)));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::ModelBOM_NEW, MoBOMInfo, MoBOMInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<MoBOMInfo> GetModelBomListByComponents(string[] pnList)
        {
            try
            {
                IList<MoBOMInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::ModelBOM_NEW cond = new mtns::ModelBOM_NEW();
                        cond.component = "[INSET]";
                        mtns::ModelBOM_NEW cond1 = new mtns::ModelBOM_NEW();
                        cond1.flag = 1;
                        sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::ModelBOM_NEW>(tk, null, null, new mtns::ConditionCollection<mtns::ModelBOM_NEW>(new mtns::InSetCondition<mtns::ModelBOM_NEW>(cond), new EqualCondition<mtns::ModelBOM_NEW>(cond1)), mtns::ModelBOM_NEW.fn_material, mtns::ModelBOM_NEW.fn_component);
                        sqlCtx.Param(ModelBOM_NEW.fn_flag).Value = cond1.flag;
                    }
                }
                string Sentence = sqlCtx.Sentence.Replace(g.DecInSet(mtns::ModelBOM_NEW.fn_component), g.ConvertInSet(new List<string>(pnList)));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::ModelBOM_NEW, MoBOMInfo, MoBOMInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetPartInfoValueListByModelAndBomNodeTypeAndInfoTypes(string model, string bomNodeType, string infoType, string infoValue, string infoType2)
        {
            try
            {
                IList<string> ret = new List<string>();

                ITableAndFields tf1 = null;
                ITableAndFields tf2 = null;
                ITableAndFields tf3 = null;
                ITableAndFields tf4 = null;
                ITableAndFields[] tafa = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        tf1 = new TableAndFields<Part_NEW>();
                        Part_NEW cond = new Part_NEW();
                        cond.bomNodeType = bomNodeType;
                        cond.flag = 1;
                        tf1.Conditions.Add(new EqualCondition<Part_NEW>(cond));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<ModelBOM_NEW>();
                        ModelBOM_NEW cond2 = new ModelBOM_NEW();
                        cond2.material = model;
                        cond2.flag = 1;
                        tf2.Conditions.Add(new EqualCondition<ModelBOM_NEW>(cond2));
                        tf2.ClearToGetFieldNames();

                        tf3 = new TableAndFields<_Metas.PartInfo>();
                        _Metas.PartInfo cond3 = new _Metas.PartInfo();
                        cond3.infoType = infoType;
                        cond3.infoValue = infoValue;
                        tf3.Conditions.Add(new EqualCondition<_Metas.PartInfo>(cond3));
                        tf3.ClearToGetFieldNames();

                        tf4 = new TableAndFields<_Metas.PartInfo>();
                        _Metas.PartInfo cond4 = new _Metas.PartInfo();
                        cond4.infoType = infoType2;
                        tf4.Conditions.Add(new EqualCondition<_Metas.PartInfo>(cond4));
                        tf4.AddRangeToGetFieldNames(_Metas.PartInfo.fn_infoValue);

                        tafa = new ITableAndFields[] { tf1, tf2, tf3, tf4 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<Part_NEW, ModelBOM_NEW>(tf1, Part_NEW.fn_partNo, tf2, ModelBOM_NEW.fn_component),
                            new TableConnectionItem<Part_NEW, _Metas.PartInfo>(tf1, Part_NEW.fn_partNo, tf3, _Metas.PartInfo.fn_partNo),
                            new TableConnectionItem<Part_NEW, _Metas.PartInfo>(tf1, Part_NEW.fn_partNo, tf4, _Metas.PartInfo.fn_partNo)
                            );

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts);

                        sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_flag)).Value = cond.flag;
                        sqlCtx.Param(g.DecAlias(tf2.Alias, ModelBOM_NEW.fn_flag)).Value = cond2.flag;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];
                tf3 = tafa[2];
                tf4 = tafa[3];

                sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_bomNodeType)).Value = bomNodeType;
                sqlCtx.Param(g.DecAlias(tf2.Alias, ModelBOM_NEW.fn_material)).Value = model;
                sqlCtx.Param(g.DecAlias(tf3.Alias, _Metas.PartInfo.fn_infoType)).Value = infoType;
                sqlCtx.Param(g.DecAlias(tf3.Alias, _Metas.PartInfo.fn_infoValue)).Value = infoValue;
                sqlCtx.Param(g.DecAlias(tf4.Alias, _Metas.PartInfo.fn_infoType)).Value = infoType2;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf4.Alias, _Metas.PartInfo.fn_infoValue)));
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
        
        public IList<string> GetPartInfoValueListByModelAndBomNodeTypeAndInfoType(string model, string bomNodeType, string infoType)
        {
            try
            {
                IList<string> ret = new List<string>();

                ITableAndFields tf1 = null;
                ITableAndFields tf2 = null;
                ITableAndFields tf3 = null;
                ITableAndFields[] tafa = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        tf1 = new TableAndFields<Part_NEW>();
                        Part_NEW cond = new Part_NEW();
                        cond.bomNodeType = bomNodeType;
                        cond.flag = 1;
                        tf1.Conditions.Add(new EqualCondition<Part_NEW>(cond));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<ModelBOM_NEW>();
                        ModelBOM_NEW cond2 = new ModelBOM_NEW();
                        cond2.material = model;
                        cond2.flag = 1;
                        tf2.Conditions.Add(new EqualCondition<ModelBOM_NEW>(cond2));
                        tf2.ClearToGetFieldNames();

                        tf3 = new TableAndFields<_Metas.PartInfo>();
                        _Metas.PartInfo cond3 = new _Metas.PartInfo();
                        cond3.infoType = infoType;
                        tf3.Conditions.Add(new EqualCondition<_Metas.PartInfo>(cond3));
                        tf3.AddRangeToGetFieldNames(_Metas.PartInfo.fn_infoValue);

                        tafa = new ITableAndFields[] { tf1, tf2, tf3 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<Part_NEW, ModelBOM_NEW>(tf1, Part_NEW.fn_partNo, tf2, ModelBOM_NEW.fn_component),
                            new TableConnectionItem<Part_NEW, _Metas.PartInfo>(tf1, Part_NEW.fn_partNo, tf3, _Metas.PartInfo.fn_partNo)
                            );

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts);

                        sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_flag)).Value = cond.flag;
                        sqlCtx.Param(g.DecAlias(tf2.Alias, ModelBOM_NEW.fn_flag)).Value = cond2.flag;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];
                tf3 = tafa[2];

                sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_bomNodeType)).Value = bomNodeType;
                sqlCtx.Param(g.DecAlias(tf2.Alias, ModelBOM_NEW.fn_material)).Value = model;
                sqlCtx.Param(g.DecAlias(tf3.Alias, _Metas.PartInfo.fn_infoType)).Value = infoType;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf3.Alias, _Metas.PartInfo.fn_infoValue)));
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

        public IList<string> GetESOPListByModel(string model)
        {
            try
            {
                IList<string> ret = new List<string>();

                ITableAndFields tf1 = null;
                ITableAndFields tf2 = null;
                ITableAndFields tf3 = null;
                ITableAndFields tf4 = null;
                ITableAndFields[] tafa = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        tf1 = new TableAndFields<Part_NEW>();
                        Part_NEW cond = new Part_NEW();
                        cond.bomNodeType = "PL";
                        cond.flag = 1;
                        tf1.Conditions.Add(new EqualCondition<Part_NEW>(cond));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<ModelBOM_NEW>();
                        ModelBOM_NEW cond2 = new ModelBOM_NEW();
                        cond2.material = model;
                        cond2.flag = 1;
                        tf2.Conditions.Add(new EqualCondition<ModelBOM_NEW>(cond2));
                        tf2.ClearToGetFieldNames();

                        tf3 = new TableAndFields<_Metas.PartInfo>();
                        _Metas.PartInfo cond3 = new _Metas.PartInfo();
                        cond3.infoType = "Picture";
                        tf3.Conditions.Add(new EqualCondition<_Metas.PartInfo>(cond3));
                        _Metas.PartInfo cond31 = new _Metas.PartInfo();
                        cond31.infoValue = "Y";
                        tf3.Conditions.Add(new EqualCondition<_Metas.PartInfo>(cond31, "RTRIM({0})"));
                        tf3.ClearToGetFieldNames();

                        tf4 = new TableAndFields<_Metas.PartInfo>();
                        _Metas.PartInfo cond4 = new _Metas.PartInfo();
                        cond4.infoType = "PictureName";
                        tf4.Conditions.Add(new EqualCondition<_Metas.PartInfo>(cond4));
                        tf4.AddRangeToGetFieldNames(_Metas.PartInfo.fn_infoValue);

                        tafa = new ITableAndFields[] { tf1, tf2, tf3, tf4 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<Part_NEW, ModelBOM_NEW>(tf1, Part_NEW.fn_partNo, tf2, ModelBOM_NEW.fn_component),
                            new TableConnectionItem<Part_NEW, _Metas.PartInfo>(tf1, Part_NEW.fn_partNo, tf3, _Metas.PartInfo.fn_partNo),
                            new TableConnectionItem<Part_NEW, _Metas.PartInfo>(tf1, Part_NEW.fn_partNo, tf4, _Metas.PartInfo.fn_partNo)
                            );

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "TOP 4", tafa, tblCnnts);

                        sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_flag)).Value = cond.flag;
                        sqlCtx.Param(g.DecAlias(tf2.Alias, ModelBOM_NEW.fn_flag)).Value = cond2.flag;

                        sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_bomNodeType)).Value = cond.bomNodeType;
                        sqlCtx.Param(g.DecAlias(tf3.Alias, _Metas.PartInfo.fn_infoType)).Value = cond3.infoType;
                        sqlCtx.Param(g.DecAlias(tf3.Alias, _Metas.PartInfo.fn_infoValue)).Value = cond31.infoValue;
                        sqlCtx.Param(g.DecAlias(tf4.Alias, _Metas.PartInfo.fn_infoType)).Value = cond4.infoType;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];
                tf3 = tafa[2];
                tf4 = tafa[3];

                sqlCtx.Param(g.DecAlias(tf2.Alias, ModelBOM_NEW.fn_material)).Value = model;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf4.Alias, _Metas.PartInfo.fn_infoValue)));
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

        public IList<MoBOMInfo> GetModelBomList(string infoType, string vendorCT)
        {
            try
            {
                IList<MoBOMInfo> ret = null;

                ITableAndFields tf1 = null;
                ITableAndFields tf2 = null;
                ITableAndFields[] tafa = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        tf1 = new TableAndFields<mtns.PartInfo>();
                        mtns.PartInfo cond = new mtns.PartInfo();
                        cond.infoType = infoType;
                        tf1.Conditions.Add(new EqualCondition<mtns.PartInfo>(cond));
                        mtns.PartInfo cond1 = new mtns.PartInfo();
                        cond1.infoValue = vendorCT;
                        tf1.Conditions.Add(new LikeCondition<mtns.PartInfo>(cond1, "", "'%' + LEFT({0}, 5) + '%'"));
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<ModelBOM_NEW>();
                        ModelBOM_NEW cond2 = new ModelBOM_NEW();
                        cond2.flag = 1;
                        tf2.Conditions.Add(new EqualCondition<ModelBOM_NEW>(cond2));
 
                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<mtns.PartInfo, ModelBOM_NEW>(tf1, mtns.PartInfo.fn_partNo, tf2, ModelBOM_NEW.fn_component));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts, "t2." + mtns::ModelBOM_NEW.fn_material, "t2." + mtns::ModelBOM_NEW.fn_component);

                        sqlCtx.Param(g.DecAlias(tf2.Alias, ModelBOM_NEW.fn_flag)).Value = cond2.flag;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, mtns.PartInfo.fn_infoType)).Value = infoType;
                sqlCtx.Param(g.DecAlias(tf1.Alias, mtns.PartInfo.fn_infoValue)).Value = vendorCT;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::ModelBOM_NEW, MoBOMInfo, MoBOMInfo>(ret, sqlR, sqlCtx, tf1.Alias);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<HpWwanlabelInfo> GetHpWwanlabelInfoByModuleNoPrefix(string wwanKpas)
        {
            try
            {
                IList<HpWwanlabelInfo> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Hp_Wwanlabel cond = new mtns::Hp_Wwanlabel();
                        cond.moduleNo = wwanKpas;
                        sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::Hp_Wwanlabel>(tk, null, null, new mtns::ConditionCollection<mtns::Hp_Wwanlabel>(
                            new EqualCondition<mtns::Hp_Wwanlabel>(cond, "LEFT({0},CHARINDEX('-',{0})-1)")), mtns::Hp_Wwanlabel.fn_id);
                    }
                }
                sqlCtx.Param(mtns.Hp_Wwanlabel.fn_moduleNo).Value = wwanKpas;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = mtns::FuncNew.SetFieldFromColumn<mtns::Hp_Wwanlabel, HpWwanlabelInfo, HpWwanlabelInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckExistMaterialByPno(string pno)
        {
            try
            {
                bool ret = false;

                ITableAndFields tf1 = null;
                ITableAndFields tf2 = null;
                ITableAndFields[] tafa = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        tf1 = new TableAndFields<_Metas.Pak_Chn_Tw_Light>();
                        tf1.ClearToGetFieldNames();

                        tf2 = new TableAndFields<_Metas.ModelBOM_NEW>();
                        _Metas.ModelBOM_NEW cond = new _Metas.ModelBOM_NEW();
                        cond.material = pno;
                        cond.flag = 1;
                        tf2.Conditions.Add(new EqualCondition<_Metas.ModelBOM_NEW>(cond));
                        tf2.AddRangeToGetFieldNames(_Metas.ModelBOM_NEW.fn_material);
                        tf2.SubDBCalalog = _Schema.SqlHelper.DB_GetData;

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.Pak_Chn_Tw_Light, _Metas.ModelBOM_NEW>(tf1, _Metas.Pak_Chn_Tw_Light.fn_partNo, tf2, _Metas.ModelBOM_NEW.fn_component));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "COUNT", tafa, tblCnnts);

                        sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.ModelBOM_NEW.fn_flag)).Value = cond.flag;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.ModelBOM_NEW.fn_material)).Value = pno;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PAK, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        int cnt = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
                        ret = cnt > 0 ? true : false;
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
        /// 判断空值并赋值(string)
        /// </summary>
        /// <param name="sqlDataReader"></param>
        /// <param name="iCol"></param>
        protected string GetValue_Str(SqlDataReader sqlDataReader, int iCol)
        {
            if (!sqlDataReader.IsDBNull(iCol))
                return sqlDataReader.GetString(iCol).TrimEnd();
            else
                return string.Empty;
        }

        public IList<string> CheckIfExistProductPartWithBom(string descr, string model, string productId)
        {
            try
            {
                IList<string> ret = new List<string>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                        sqlCtx.Sentence = "SELECT c.PartSn,b.PartNo FROM {0} a " +
                                            "INNER JOIN {1} b " +
                                            "ON a.{4}=b.{7} " +
                                            "AND b.{8}=@{8} " +
                                            "LEFT JOIN {2}..{3} c " +
                                            "ON b.{9}=c.{13} " +
                                            "AND c.{11}=@{11} " +
                                            "WHERE a.{5}=@{5} "+
                                            "AND a.{6}=1 " + 
                                            "AND b.{10}=1 "; 
                                            //+ "AND c.{12} IS NULL " +                                                                                        

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, ToolsNew.GetTableName(typeof(ModelBOM_NEW)),
                                                                         ToolsNew.GetTableName(typeof(Part_NEW)),
                                                                         _Schema.SqlHelper.DB_FA,
                                                                         ToolsNew.GetTableName(typeof(_Metas.Product_Part)),
                                                                         ModelBOM_NEW.fn_component,
                                                                         ModelBOM_NEW.fn_material,
                                                                         ModelBOM_NEW.fn_flag,
                                                                         Part_NEW.fn_partNo,
                                                                         Part_NEW.fn_descr,
                                                                         Part_NEW.fn_partNo,
                                                                         Part_NEW.fn_flag,
                                                                         _Metas.Product_Part.fn_productID,
                                                                         _Metas.Product_Part.fn_partSn,
                                                                         _Metas.Product_Part.fn_partNo
                                                                         );

                        sqlCtx.AddParam(ModelBOM_NEW.fn_material, new SqlParameter("@" + ModelBOM_NEW.fn_material, ToolsNew.GetDBFieldType<ModelBOM_NEW>(ModelBOM_NEW.fn_material)));
                        sqlCtx.AddParam(Part_NEW.fn_descr, new SqlParameter("@" + Part_NEW.fn_descr, ToolsNew.GetDBFieldType<Part_NEW>(Part_NEW.fn_descr)));
                        sqlCtx.AddParam(_Metas.Product_Part.fn_productID, new SqlParameter("@" + _Metas.Product_Part.fn_productID, ToolsNew.GetDBFieldType<_Metas.Product_Part>(_Metas.Product_Part.fn_productID)));
                    }
                }
                sqlCtx.Param(ModelBOM_NEW.fn_material).Value = model;
                sqlCtx.Param(Part_NEW.fn_descr).Value = descr;
                sqlCtx.Param(_Metas.Product_Part.fn_productID).Value = productId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string partsn = GetValue_Str(sqlR, 0);
                        ret.Add(partsn);

                        //int cnt = g.GetValue_Int32(sqlR, 0);
                        //ret = cnt > 0 ? true : false;
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckIfExistModelBomWithPart(string descrLike1, string descrLike2, string model)
        {
            try
            {
                bool ret = false;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new _Metas.SQLContextNew();

                        sqlCtx.Sentence = "SELECT COUNT(b.{2}) FROM {0} a, {1} b WHERE a.{4} = b.{2} AND (b.{3} LIKE @{3} OR b.{3} LIKE @{3}$1) AND a.{5} = @{5}";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, ToolsNew.GetTableName(typeof(ModelBOM_NEW)),
                                                                         ToolsNew.GetTableName(typeof(Part_NEW)),
                                                                         Part_NEW.fn_partNo,
                                                                         Part_NEW.fn_descr,
                                                                         ModelBOM_NEW.fn_component,
                                                                         ModelBOM_NEW.fn_material
                                                                         );

                        sqlCtx.AddParam(Part_NEW.fn_descr, new SqlParameter("@" + Part_NEW.fn_descr, ToolsNew.GetDBFieldType<Part_NEW>(Part_NEW.fn_descr)));
                        sqlCtx.AddParam(Part_NEW.fn_descr + "$1", new SqlParameter("@" + Part_NEW.fn_descr + "$1", ToolsNew.GetDBFieldType<Part_NEW>(Part_NEW.fn_descr)));
                        sqlCtx.AddParam(ModelBOM_NEW.fn_material, new SqlParameter("@" + ModelBOM_NEW.fn_material, ToolsNew.GetDBFieldType<ModelBOM_NEW>(ModelBOM_NEW.fn_material)));

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param(Part_NEW.fn_descr).Value = descrLike1;
                sqlCtx.Param(Part_NEW.fn_descr + "$1").Value = descrLike2;
                sqlCtx.Param(ModelBOM_NEW.fn_material).Value = model;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        int cnt = g.GetValue_Int32(sqlR, 0);
                        ret = cnt > 0;
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckIfExistDoubleBomWithPart(string model, string descrLike, string bomNodeType, string bomNodeType2)
        {
            try
            {
                bool ret = false;

                ITableAndFields tf1 = null;
                ITableAndFields tf2 = null;
                ITableAndFields tf3 = null;
                ITableAndFields tf4 = null;
                ITableAndFields[] tafa = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        tf1 = new TableAndFields<_Metas.ModelBOM_NEW>();
                        var cond1 = new _Metas.ModelBOM_NEW();
                        cond1.flag = 1;
                        cond1.material = model;
                        tf1.Conditions.Add(new EqualCondition<_Metas.ModelBOM_NEW>(cond1));
                        tf1.AddRangeToGetFieldNames(_Metas.ModelBOM_NEW.fn_id);

                        tf2 = new TableAndFields<_Metas.Part_NEW>();
                        var cond2 = new _Metas.Part_NEW();
                        cond2.flag = 1;
                        cond2.bomNodeType = bomNodeType;
                        tf2.Conditions.Add(new EqualCondition<_Metas.Part_NEW>(cond2));
                        var cond21 = new _Metas.Part_NEW();
                        cond21.descr = "%" + descrLike + "%";
                        tf2.Conditions.Add(new LikeCondition<_Metas.Part_NEW>(cond21));
                        tf2.ClearToGetFieldNames();

                        tf3 = new TableAndFields<_Metas.ModelBOM_NEW>();
                        var cond3 = new _Metas.ModelBOM_NEW();
                        cond3.flag = 1;
                        tf3.Conditions.Add(new EqualCondition<_Metas.ModelBOM_NEW>(cond3));
                        tf3.ClearToGetFieldNames();

                        tf4 = new TableAndFields<_Metas.Part_NEW>();
                        var cond4 = new _Metas.Part_NEW();
                        cond4.flag = 1;
                        cond4.bomNodeType = bomNodeType2;
                        tf4.Conditions.Add(new EqualCondition<_Metas.Part_NEW>(cond4));
                        tf4.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2, tf3, tf4 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<_Metas.ModelBOM_NEW, _Metas.Part_NEW>(tf1, _Metas.ModelBOM_NEW.fn_component, tf2, _Metas.Part_NEW.fn_partNo),
                            new TableConnectionItem<_Metas.Part_NEW, _Metas.ModelBOM_NEW>(tf2, _Metas.Part_NEW.fn_partNo, tf3, _Metas.ModelBOM_NEW.fn_material),
                            new TableConnectionItem<_Metas.ModelBOM_NEW, _Metas.Part_NEW>(tf3, _Metas.ModelBOM_NEW.fn_component, tf4, _Metas.Part_NEW.fn_partNo)
                            );

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "COUNT", tafa, tblCnnts);

                        sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.ModelBOM_NEW.fn_flag)).Value = cond1.flag;
                        sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.Part_NEW.fn_flag)).Value = cond2.flag;
                        sqlCtx.Param(g.DecAlias(tf3.Alias, _Metas.ModelBOM_NEW.fn_flag)).Value = cond3.flag;
                        sqlCtx.Param(g.DecAlias(tf4.Alias, _Metas.Part_NEW.fn_flag)).Value = cond4.flag;
                    }
                }
                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];
                tf3 = tafa[2];
                tf4 = tafa[3];

                sqlCtx.Param(g.DecAlias(tf1.Alias, _Metas.ModelBOM_NEW.fn_material)).Value = model;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.Part_NEW.fn_bomNodeType)).Value = bomNodeType;
                sqlCtx.Param(g.DecAlias(tf2.Alias, _Metas.Part_NEW.fn_descr)).Value = "%" + descrLike + "%";
                sqlCtx.Param(g.DecAlias(tf4.Alias, _Metas.Part_NEW.fn_bomNodeType)).Value = bomNodeType2;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        int cnt = g.GetValue_Int32(sqlR, sqlCtx.Indexes("COUNT"));
                        ret = cnt > 0 ? true : false;
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetPartNoInModelBOMByBomNodeTypeAndStartWithDescr(IList<string> modelList, string bomNodeType, string startWithBomDescr)
        {
            try
            {
                IList<string> ret = new List<string>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();                      
                        sqlCtx.Sentence = @"select Distinct b.PartNo
                                                         from ModelBOM a, Part  b
                                                         where a.Component = b.PartNo and
                                                               a.Material in ('{0}')     and
                                                               b.BomNodeType =@BomNodeType   and
                                                               b.Descr like @Descr ";

                        sqlCtx.AddParam("BomNodeType", new SqlParameter("@BomNodeType" , SqlDbType.VarChar));
                        sqlCtx.AddParam("Descr", new SqlParameter("@Descr", SqlDbType.VarChar ));
                          SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("BomNodeType").Value = bomNodeType;
                sqlCtx.Param("Descr").Value = startWithBomDescr + "%";
                string sentence = string.Format(sqlCtx.Sentence, string.Join("','", modelList.ToArray()));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, 
                                                                                                                    CommandType.Text, 
                                                                                                                    sentence, 
                                                                                                                    sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string partsn = GetValue_Str(sqlR, 0);
                        ret.Add(partsn);                      
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetPartNoInModelBOMByBomNodeTypeAndContainDescr(IList<string> modelList, string bomNodeType, string containBomDescr)
        {
             try
            {
                IList<string> ret = new List<string>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();                      
                        sqlCtx.Sentence = @"select Distinct b.PartNo
                                                         from ModelBOM a, Part  b
                                                         where a.Component = b.PartNo and
                                                               a.Material in ('{0}')     and
                                                               b.BomNodeType =@BomNodeType   and
                                                               b.Descr like @Descr ";

                        sqlCtx.AddParam("BomNodeType", new SqlParameter("@BomNodeType" , SqlDbType.VarChar));
                        sqlCtx.AddParam("Descr", new SqlParameter("@Descr", SqlDbType.VarChar ));
                          SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("BomNodeType").Value = bomNodeType;
                sqlCtx.Param("Descr").Value = "%"+containBomDescr + "%";
                string sentence = string.Format(sqlCtx.Sentence, string.Join("','", modelList.ToArray()));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, 
                                                                                                                    CommandType.Text, 
                                                                                                                    sentence, 
                                                                                                                    sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string partsn = GetValue_Str(sqlR, 0);
                        ret.Add(partsn);                      
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

        #region ICache Members

        public bool IsCached()
        {
            return DataChangeMediator.CheckCacheSwitchOpen(DataChangeMediator.CacheSwitchType.BOM);
        }

        public void ProcessItem(CacheUpdateInfo item)
        {
            if (item.Type == IMES.DataModel.CacheType.BOM)
                LoadOneCache(item.Item);
        }

        public void ClearCache()
        {
            lock (_syncObj_cache)
            {
                _cache.Flush();
            }
        }

        private void LoadOneCache(string pk) //model
        {
            try
            {
                lock (_syncObj_cache)
                {
                    if (_cache.Contains(pk))
                    {
                        _cache.Remove(pk);
                    }
                    #region For YWH
                    /*
                    IHierarchicalBOM bom = GetHierarchicalBOMByModel_DB(pk);
                    if (bom != null)
                    {
                        AddToCache(pk, bom);
                    }
                    */
                    #endregion
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }
        }

        private IMES.DataModel.CacheUpdateInfo GetACacheSignal(string pk, string type)
        {
            IMES.DataModel.CacheUpdateInfo ret = new IMES.DataModel.CacheUpdateInfo();
            ret.Cdt = ret.Udt = _Schema.SqlHelper.GetDateTime();
            ret.Updated = false;
            ret.Type = type;
            ret.Item = pk;
            return ret;
        }

        private void AddToCache(string key, object obj)
        {
            //Vincent 2015-11-15 modify none callback and static _cacheAbsoluteTime
            //_cache.Add(key, obj, CacheItemPriority.Normal, new MOBOMRefreshAction(), new AbsoluteTime(TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["TOSC_MOBOMCache"].ToString()))));
            //_cache.Add(key, obj, CacheItemPriority.Normal, null, _cacheAbsoluteTime);
            _cache.Add(key, obj, CacheItemPriority.Normal, null, new AbsoluteTime(TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["TOSC_MOBOMCache"].ToString()))));
        }

        [Serializable]
        private class MOBOMRefreshAction : ICacheItemRefreshAction
        {
            public void Refresh(string key, object expiredValue, CacheItemRemovedReason removalReason)
            {
            }
        }

        
        #endregion
        #region for RCTO
        public bool CheckIfExistProductPartWithBomForRCTO(string descr, string model, string productId)
        {
            try
            {
                bool ret = false;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                        sqlCtx.Sentence = "SELECT COUNT(1) FROM {0} a " +
                                            "INNER JOIN {1} b " +
                                            "ON a.{4}=b.{7} " +
                                            "AND b.{8}=@{8} " +
                                            "LEFT JOIN {2}..{3} c " +
                                            "ON b.{9}=c.{13} " +
                                            "AND c.{11}=@{11} " +
                                            "WHERE a.{5}=@{5} " +
                                            "AND c.{12} IS NULL " +
                                            "AND a.{6}=1 " +
                                            "AND b.{10}=1 ";

                        sqlCtx.Sentence = string.Format(sqlCtx.Sentence, ToolsNew.GetTableName(typeof(ModelBOM_NEW)),
                                                                         ToolsNew.GetTableName(typeof(Part_NEW)),
                                                                         _Schema.SqlHelper.DB_FA,
                                                                         ToolsNew.GetTableName(typeof(_Metas.Product_Part)),
                                                                         ModelBOM_NEW.fn_component,
                                                                         ModelBOM_NEW.fn_material,
                                                                         ModelBOM_NEW.fn_flag,
                                                                         Part_NEW.fn_partNo,
                                                                         Part_NEW.fn_descr,
                                                                         Part_NEW.fn_partNo,
                                                                         Part_NEW.fn_flag,
                                                                         _Metas.Product_Part.fn_productID,
                                                                         _Metas.Product_Part.fn_partSn,
                                                                         _Metas.Product_Part.fn_partNo
                                                                         );

                        sqlCtx.AddParam(ModelBOM_NEW.fn_material, new SqlParameter("@" + ModelBOM_NEW.fn_material, ToolsNew.GetDBFieldType<ModelBOM_NEW>(ModelBOM_NEW.fn_material)));
                        sqlCtx.AddParam(Part_NEW.fn_descr, new SqlParameter("@" + Part_NEW.fn_descr, ToolsNew.GetDBFieldType<Part_NEW>(Part_NEW.fn_descr)));
                        sqlCtx.AddParam(_Metas.Product_Part.fn_productID, new SqlParameter("@" + _Metas.Product_Part.fn_productID, ToolsNew.GetDBFieldType<_Metas.Product_Part>(_Metas.Product_Part.fn_productID)));
                    }
                }
                sqlCtx.Param(ModelBOM_NEW.fn_material).Value = model;
                sqlCtx.Param(Part_NEW.fn_descr).Value = descr;
                sqlCtx.Param(_Metas.Product_Part.fn_productID).Value = productId;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        int cnt = g.GetValue_Int32(sqlR, 0);
                        ret = cnt > 0 ? true : false;
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

        #region Asset Part
        public IList<string> GetAssetPartNo(string model, IList<string> astType)
        {
            try
            {
                IList<string> ret = new List<string>() ;
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"select a.Component
                                                            from ModelBOM a, 
                                                                     Part b,
                                                                     @ASTType c
                                                            where a.Component = b.PartNo and
                                                                      b.Descr  = c.data and
                                                                      a.Material = @Material and
                                                                      a.Component like '2TG%'  and
                                                                      b.Flag=1";

                        sqlCtx.AddParam("Material", new SqlParameter("@Material", SqlDbType.VarChar));

                        SqlParameter para1 = new SqlParameter("@ASTType", SqlDbType.Structured);
                        para1.TypeName = "TbStringList";
                        para1.Direction = ParameterDirection.Input;
                        sqlCtx.AddParam("ASTType", para1);

                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                sqlCtx.Param("Material").Value = model;

                DataTable dt1 = IMES.Infrastructure.Repository._Schema.SQLData.ToDataTable(astType);
                sqlCtx.Param("ASTType").Value = dt1;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader_OnTrans(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                                             CommandType.Text,
                                                                                                                                             sqlCtx.Sentence,
                                                                                                                                             sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ret.Add(sqlR.GetString(0).Trim());
                       
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


        #region ChecItemTypeRule
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="family"></param>
        /// <returns></returns>
        public IList<CheckItemTypeRuleDef> GetCheckItemTypeRuleWithPriority(string itemType, string line, string station, string family)
        {
            try
            {
                IList<CheckItemTypeRuleDef> ret = GetCheckItemTypeRuleByItemType(itemType);

                ret = ret.Where(x => getCheckItemTypeRulePriority(x, line, station, family)).OrderBy(x => x.Priority).ToList();
                return ret;

                //                MethodBase mthObj = MethodBase.GetCurrentMethod();
                //                int tk = mthObj.MetadataToken;
                //                SQLContextNew sqlCtx = null;
                //                lock (mthObj)
                //                {
                //                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //                    {
                //                        sqlCtx = new SQLContextNew();
                //                        sqlCtx.Sentence = @"select ID, CheckItemType, Line, Station, Family, 
                //                                                                       BomNodeType,PartDescr,PartType, 
                //                                                                       MatchRule, CheckRule, SaveRule, Descr, 
                //                                                                       Editor, Cdt, Udt,NeedUniqueCheck,NeedCommonSave,
                //                                                                       NeedSave,CheckTestKPCount,FilterExpression,
                //                                                                        (case when Line=@Line and Station=@Station and Family=@Family then
                //                                                                               1
                //                                                                             when Line=@Line and Station=@Station and Family='' then
                //                                                                               2
                //                                                                             when Line=@Line and Station='' and Family=@Family then
                //                                                                               3
                //                                                                             when Line='' and Station=@Station and Family=@Family then
                //                                                                               4
                //                                                                             when Line=@Line and Station='' and Family='' then
                //                                                                               5
                //                                                                             when Line='' and Station=@Station and Family='' then
                //                                                                               6
                //                                                                             when Line='' and Station='' and Family=@Family then
                //                                                                               7
                //                                                                             else
                //                                                                               8  
                //                                                                         end) as Priority     
                //                                                                  from CheckItemTypeRule
                //                                                                  where CheckItemType=@CheckItemType and      
                //                                                                        (Line='' or Line=@Line  ) and
                //                                                                        (Station='' or Station=@Station) and
                //                                                                         (Family='' or Family=@Family)
                //                                                                order by Priority";

                //                        sqlCtx.AddParam("CheckItemType", new SqlParameter("@CheckItemType", SqlDbType.VarChar));
                //                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                //                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                //                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));

                //                        SQLCache.InsertIntoCache(tk, sqlCtx);
                //                    }
                //                }

                //                sqlCtx.Param("CheckItemType").Value = itemType;
                //                 sqlCtx.Param("Line").Value = line;
                //                 sqlCtx.Param("Station").Value = station;
                //                 sqlCtx.Param("Family").Value = family;



                //                 using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM,
                //                                                                                                                                             CommandType.Text,
                //                                                                                                                                             sqlCtx.Sentence,
                //                                                                                                                                             sqlCtx.Params))
                //                {
                //                    while (sqlR != null && sqlR.Read())
                //                    {
                //                        CheckItemTypeRuleDef item = IMES.Infrastructure.Repository._Schema.SQLData.ToObjectByField<CheckItemTypeRuleDef>(sqlR);
                //                        ret.Add(item);

                //                    }
                //                }
                //                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Regex by Family
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="family"></param>
        /// <returns></returns>
        public IList<CheckItemTypeRuleDef> GetCheckItemTypeRuleWithPriorityByRegex(string itemType, string line, string station, string family)
        {
            try
            {
                IList<CheckItemTypeRuleDef> ret = GetCheckItemTypeRuleByItemType(itemType);
                ret = ret.Where(x => getCheckItemTypeRulePriorityByRegex(x, line, station, family)).OrderBy(x => x.Priority).ToList();
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool getCheckItemTypeRulePriority(CheckItemTypeRuleDef rule, string line, string station, string family)
        {

            bool isLine = !string.IsNullOrEmpty(rule.Line);
            bool isStation = !string.IsNullOrEmpty(rule.Station);
            bool isFamily = !string.IsNullOrEmpty(rule.Family);


            if (rule.Line == line &&
               rule.Station == station &&
               family == rule.Family)
            {
                rule.Priority = 1;
                return true;
            }


            if (!isFamily &&
               rule.Line == line &&
               rule.Station == station)
            {
                rule.Priority = 2;
                return true;
            }

            if (!isStation &&
              rule.Line == line &&
              family == rule.Family)
            {
                rule.Priority = 3;
                return true;
            }

            if (!isLine &&
               rule.Station == station &&
              family == rule.Family)
            {
                rule.Priority = 4;
                return true;
            }

            if (!isStation && !isFamily &&
              rule.Line == line)
            {
                rule.Priority = 5;
                return true;
            }

            if (!isLine && !isFamily &&
                  rule.Station == station)
            {
                rule.Priority = 6;
                return true;
            }

            if (!isLine && !isStation &&
              family == rule.Family)
            {
                rule.Priority = 7;
                return true;
            }

            if (!isLine && !isStation && !isFamily)
            {
                rule.Priority = 8;
                return true;
            }

            return false;

        }

        private bool getCheckItemTypeRulePriorityByRegex(CheckItemTypeRuleDef rule, string line, string station, string family)
        {
            bool isLine = !string.IsNullOrEmpty(rule.Line);
            bool isStation = !string.IsNullOrEmpty(rule.Station);
            bool isFamily = !string.IsNullOrEmpty(rule.Family);

            bool matchFamily = (isFamily ? Regex.IsMatch(family, rule.Family) : family == rule.Family);

            if (rule.Line == line &&
               rule.Station == station &&
               matchFamily)
            {
                rule.Priority = 1;
                return true;
            }


            if (!isFamily &&
               rule.Line == line &&
               rule.Station == station)
            {
                rule.Priority = 2;
                return true;
            }

            if (!isStation &&
              rule.Line == line &&
              matchFamily)
            {
                rule.Priority = 3;
                return true;
            }

            if (!isLine &&
               rule.Station == station &&
              matchFamily)
            {
                rule.Priority = 4;
                return true;
            }

            if (!isStation && !isFamily &&
              rule.Line == line)
            {
                rule.Priority = 5;
                return true;
            }

            if (!isLine && !isFamily &&
                  rule.Station == station)
            {
                rule.Priority = 6;
                return true;
            }

            if (!isLine && !isStation && matchFamily)
            {
                rule.Priority = 7;
                return true;
            }

            if (!isLine && !isStation && !isFamily)
            {
                rule.Priority = 8;
                return true;
            }

            return false;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public IList<CheckItemTypeRuleDef> GetCheckItemTypeRuleByItemType(string itemType)
        {
            try
            {
                IList<CheckItemTypeRuleDef> ret = new List<CheckItemTypeRuleDef>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                CheckItemTypeRuleDef condition = new CheckItemTypeRuleDef
                {
                    CheckItemType = itemType
                };
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        //sqlCtx = new SQLContextNew();                       
                        CheckItemTypeRule cond = FuncNew.SetColumnFromField<CheckItemTypeRule, CheckItemTypeRuleDef>(condition);

                        sqlCtx = FuncNew.GetConditionedSelect<CheckItemTypeRule>(tk, null, null,
                                                                                           new ConditionCollection<CheckItemTypeRule>(new EqualCondition<CheckItemTypeRule>(cond)),
                                                                                          CheckItemTypeRule.fn_line, CheckItemTypeRule.fn_station, CheckItemTypeRule.fn_family);


                        //}
                        //                        sqlCtx = FuncNew.SetColumnFromField<T, R>(sqlCtx, condition);
                        //                        sqlCtx.Sentence = @"select ID, CheckItemType, Line, Station, Family, 
                        //                                                                       BomNodeType,PartDescr,PartType, 
                        //                                                                       MatchRule, CheckRule, SaveRule, Descr, 
                        //                                                                       Editor, Cdt, Udt,NeedUniqueCheck,NeedCommonSave,
                        //                                                                       NeedSave,CheckTestKPCount,FilterExpression 
                        //                                                                  from CheckItemTypeRule
                        //                                                                  where CheckItemType=@CheckItemType
                        //                                                                order by Line,Station,Family";

                        //                        sqlCtx.AddParam("CheckItemType", new SqlParameter("@CheckItemType", SqlDbType.VarChar));

                        //                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                //sqlCtx.Param("CheckItemType").Value = itemType;


                sqlCtx = FuncNew.SetColumnFromField<CheckItemTypeRule, CheckItemTypeRuleDef>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                                                                             CommandType.Text,
                                                                                                                                             sqlCtx.Sentence,
                                                                                                                                             sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<CheckItemTypeRule, CheckItemTypeRuleDef, CheckItemTypeRuleDef>(ret, sqlR, sqlCtx);
                    //while (sqlR != null && sqlR.Read())
                    //{
                    //    CheckItemTypeRuleDef item = IMES.Infrastructure.Repository._Schema.SQLData.ToObject<CheckItemTypeRuleDef>(sqlR);
                    //    ret.Add(item);

                    //}
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<string> GetChechItemTypeList()
        {
            try
            {
                IList<string> ret = new List<string>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        //sqlCtx = new SQLContextNew();


                        sqlCtx = FuncNew.GetConditionedSelect<CheckItemTypeRule>(tk, "DISTINCT",
                                   new string[] { CheckItemTypeRule.fn_checkItemType },
                                   new ConditionCollection<CheckItemTypeRule>());
                        //                        sqlCtx = new SQLContextNew();
                        //                        sqlCtx.Sentence = @"select Distinct Name
                        //                                                           from CheckItemType";


                        //                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                                                                             CommandType.Text,
                                                                                                                                             sqlCtx.Sentence,
                                                                                                                                             sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {


                        ret.Add(((string)sqlR[0]).Trim());

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
        /// 
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="family"></param>
        /// <returns></returns>
        public bool CheckExistCheckItemTypeRule(string itemType, string line, string station, string family)
        {
            try
            {
                bool ret = false;
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                CheckItemTypeRuleDef condition = new CheckItemTypeRuleDef
                {
                    CheckItemType = itemType,
                    Line = line,
                    Station = station,
                    Family = family
                };

                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        CheckItemTypeRule cond = FuncNew.SetColumnFromField<CheckItemTypeRule, CheckItemTypeRuleDef>(condition);

                        sqlCtx = FuncNew.GetConditionedSelect<CheckItemTypeRule>(tk, "TOP 1", new string[] { CheckItemTypeRule.fn_id },
                                                                                           new ConditionCollection<CheckItemTypeRule>(new EqualCondition<CheckItemTypeRule>(cond)));

                        //                        sqlCtx = new SQLContextNew();
                        //                        sqlCtx.Sentence = @"select top 1 ID  
                        //                                            from CheckItemTypeRule
                        //                                            where   CheckItemType=@CheckItemType and      
                        //		                                            Line=@Line  and
                        //		                                            Station=@Station and
                        //		                                            Family=@Family ";

                        //                        sqlCtx.AddParam("CheckItemType", new SqlParameter("@CheckItemType", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));
                        //                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                //sqlCtx.Param("CheckItemType").Value = itemType;
                //sqlCtx.Param("Line").Value = line;
                //sqlCtx.Param("Station").Value = station;
                //sqlCtx.Param("Family").Value = family;
                sqlCtx = FuncNew.SetColumnFromField<CheckItemTypeRule, CheckItemTypeRuleDef>(sqlCtx, condition);
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                                                                             CommandType.Text,
                                                                                                                                             sqlCtx.Sentence,
                                                                                                                                             sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.HasRows)
                    {
                        ret = true;
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
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool CheckExistCheckItemTypeRule(CheckItemTypeRuleDef item)
        {
            try
            {
                bool ret = false;
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                string checkItemType = item.CheckItemType;
                string editor = item.Editor;
                DateTime cdt = item.Cdt;
                DateTime udt = item.Udt;

                item.CheckItemType = null;
                item.Editor = null;
                item.Udt = DateTime.MinValue;
                item.Cdt = DateTime.MinValue;

                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        CheckItemTypeRule cond = FuncNew.SetColumnFromField<CheckItemTypeRule, CheckItemTypeRuleDef>(item);

                        sqlCtx = FuncNew.GetConditionedSelect<CheckItemTypeRule>(tk, "TOP 1", new string[] { CheckItemTypeRule.fn_id },
                                                                                           new ConditionCollection<CheckItemTypeRule>(new EqualCondition<CheckItemTypeRule>(cond)));
                        //                        sqlCtx = new SQLContextNew();
                        //                        sqlCtx.Sentence = @"select top 1 ID  
                        //                                            from CheckItemTypeRule
                        //                                            where --CheckItemType=@CheckItemType and      
                        //		                                            Line=@Line  and
                        //		                                            Station=@Station and
                        //		                                            Family=@Family and
                        //		                                            BomNodeType = @BomNodeType and
                        //		                                            PartDescr = @PartDescr and
                        //		                                            PartType =@PartType and
                        //		                                            MatchRule = @MatchRule and
                        //                                            		
                        //		                                            CheckRule = @CheckRule and
                        //		                                            SaveRule = @SaveRule and
                        //		                                            Descr = @Descr and
                        //		                                            NeedUniqueCheck = @NeedUniqueCheck and
                        //		                                            NeedCommonSave = @NeedCommonSave and
                        //		                                            NeedSave = @NeedSave and
                        //		                                            CheckTestKPCount = @CheckTestKPCount ";

                        //                        //sqlCtx.AddParam("CheckItemType", new SqlParameter("@CheckItemType", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));

                        //                        sqlCtx.AddParam("BomNodeType", new SqlParameter("@BomNodeType", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("PartDescr", new SqlParameter("@PartDescr", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("PartType", new SqlParameter("@PartType", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("MatchRule", new SqlParameter("@MatchRule", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("CheckRule", new SqlParameter("@CheckRule", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("SaveRule", new SqlParameter("@SaveRule", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("Descr", new SqlParameter("@Descr", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("NeedUniqueCheck", new SqlParameter("@NeedUniqueCheck", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("NeedCommonSave", new SqlParameter("@NeedCommonSave", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("NeedSave", new SqlParameter("@NeedSave", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("CheckTestKPCount", new SqlParameter("@CheckTestKPCount", SqlDbType.VarChar));
                        //                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                //sqlCtx.Param("CheckItemType").Value = item.CheckItemType;
                //sqlCtx.Param("Line").Value = item.Line;
                //sqlCtx.Param("Station").Value = item.Station;
                //sqlCtx.Param("Family").Value = item.Family;
                //sqlCtx.Param("BomNodeType").Value = item.BomNodeType;
                //sqlCtx.Param("PartDescr").Value = item.PartDescr;
                //sqlCtx.Param("PartType").Value = item.PartType;
                //sqlCtx.Param("MatchRule").Value = item.MatchRule;
                //sqlCtx.Param("CheckRule").Value = item.CheckRule;
                //sqlCtx.Param("SaveRule").Value = item.SaveRule;
                //sqlCtx.Param("Descr").Value = item.Descr;
                //sqlCtx.Param("NeedUniqueCheck").Value = item.NeedUniqueCheck;
                //sqlCtx.Param("NeedCommonSave").Value = item.NeedCommonSave;
                //sqlCtx.Param("NeedSave").Value = item.NeedSave;
                //sqlCtx.Param("CheckTestKPCount").Value = item.CheckTestKPCount;
                sqlCtx = FuncNew.SetColumnFromField<CheckItemTypeRule, CheckItemTypeRuleDef>(sqlCtx, item);


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                                                                             CommandType.Text,
                                                                                                                                             sqlCtx.Sentence,
                                                                                                                                             sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.HasRows)
                    {
                        ret = true;
                    }
                }
                item.CheckItemType = checkItemType;
                item.Editor = editor;
                item.Cdt = cdt;
                item.Udt = udt;
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemType"></param>
        public void AddCheckItemTypeRule(CheckItemTypeRuleDef itemType)
        {
            try
            {

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                DateTime now = DateTime.Now;
                if (itemType.Cdt == DateTime.MinValue)
                {
                    itemType.Cdt = now;
                }
                if (itemType.Udt == DateTime.MinValue)
                {
                    itemType.Udt = now;
                }
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetAquireIdInsert<CheckItemTypeRule>(tk);

                        //                        sqlCtx = new SQLContextNew();
                        //                        sqlCtx.Sentence = @"insert  into CheckItemTypeRule (CheckItemType, Line, Station, Family, 
                        //                                                                               BomNodeType,PartDescr,PartType, 
                        //                                                                                MatchRule, CheckRule, SaveRule, Descr, 
                        //                                                                               Editor, Cdt, Udt,NeedUniqueCheck,NeedCommonSave,
                        //                                                                               NeedSave,CheckTestKPCount)
                        //                                                        Values( @CheckItemType, @Line, @Station, @Family, 
                        //                                                                @BomNodeType,@PartDescr,@PartType, 
                        //                                                                @MatchRule, @CheckRule, @SaveRule, @Descr, 
                        //                                                                @Editor, GETDATE(), GETDATE(),
                        //                                                                @NeedUniqueCheck,@NeedCommonSave,
                        //                                                                @NeedSave,@CheckTestKPCount) ";

                        //                        sqlCtx.AddParam("CheckItemType", new SqlParameter("@CheckItemType", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));

                        //                        sqlCtx.AddParam("BomNodeType", new SqlParameter("@BomNodeType", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("PartDescr", new SqlParameter("@PartDescr", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("PartType", new SqlParameter("@PartType", SqlDbType.VarChar));

                        //                        sqlCtx.AddParam("MatchRule", new SqlParameter("@MatchRule", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("CheckRule", new SqlParameter("@CheckRule", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("SaveRule", new SqlParameter("@SaveRule", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("Descr", new SqlParameter("@Descr", SqlDbType.VarChar));

                        //                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("NeedUniqueCheck", new SqlParameter("@NeedUniqueCheck", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("NeedCommonSave", new SqlParameter("@NeedCommonSave", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("CheckTestKPCount", new SqlParameter("@CheckTestKPCount", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("NeedSave", new SqlParameter("@NeedSave", SqlDbType.VarChar));


                        //                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                //sqlCtx.Param("CheckItemType").Value = itemType.CheckItemType;
                //sqlCtx.Param("Line").Value = string.IsNullOrEmpty(itemType.Line) ? "" : itemType.Line;
                //sqlCtx.Param("Station").Value = string.IsNullOrEmpty(itemType.Station) ? "" : itemType.Station;
                //sqlCtx.Param("Family").Value = string.IsNullOrEmpty(itemType.Family) ? "" : itemType.Family;

                //sqlCtx.Param("BomNodeType").Value = string.IsNullOrEmpty(itemType.BomNodeType) ? "" : itemType.BomNodeType;
                //sqlCtx.Param("PartDescr").Value = string.IsNullOrEmpty(itemType.PartDescr) ? "" : itemType.PartDescr;
                //sqlCtx.Param("PartType").Value = string.IsNullOrEmpty(itemType.PartType) ? "" : itemType.PartType;

                //sqlCtx.Param("MatchRule").Value = string.IsNullOrEmpty(itemType.MatchRule) ? "" : itemType.MatchRule;
                //sqlCtx.Param("CheckRule").Value = string.IsNullOrEmpty(itemType.CheckRule) ? "" : itemType.CheckRule;
                //sqlCtx.Param("SaveRule").Value = string.IsNullOrEmpty(itemType.SaveRule) ? "" : itemType.SaveRule;
                //sqlCtx.Param("Descr").Value = string.IsNullOrEmpty(itemType.Descr) ? "" : itemType.Descr;

                //sqlCtx.Param("Editor").Value = itemType.Editor;
                //sqlCtx.Param("NeedUniqueCheck").Value = itemType.NeedUniqueCheck;
                //sqlCtx.Param("NeedCommonSave").Value = itemType.NeedCommonSave;
                //sqlCtx.Param("NeedSave").Value = itemType.NeedSave;
                //sqlCtx.Param("CheckTestKPCount").Value = itemType.CheckTestKPCount;

                sqlCtx = FuncNew.SetColumnFromField<CheckItemTypeRule, CheckItemTypeRuleDef>(sqlCtx, itemType);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                                                                 CommandType.Text,
                                                                                                                                             sqlCtx.Sentence,
                                                                                                                                             sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void DeleteCheckItemTypeRule(int id)
        {
            try
            {

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                CheckItemTypeRuleDef condition = new CheckItemTypeRuleDef
                {
                    ID = id
                };
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        CheckItemTypeRule cond = FuncNew.SetColumnFromField<CheckItemTypeRule, CheckItemTypeRuleDef>(condition);

                        sqlCtx = FuncNew.GetConditionedDelete<CheckItemTypeRule>(new ConditionCollection<CheckItemTypeRule>(new EqualCondition<CheckItemTypeRule>(cond)));
                        //                        sqlCtx = new SQLContextNew();
                        //                        sqlCtx.Sentence = @"delete  CheckItemTypeRule
                        //                                                            where ID=@ID   ";

                        //                        sqlCtx.AddParam("ID", new SqlParameter("@ID", SqlDbType.Int));

                        //                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                //sqlCtx.Param("ID").Value = id;
                sqlCtx = FuncNew.SetColumnFromField<CheckItemTypeRule, CheckItemTypeRuleDef>(sqlCtx, condition);
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                                                                 CommandType.Text,
                                                                                                                                             sqlCtx.Sentence,
                                                                                                                                             sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemType"></param>
        public void UpdateCheckItemTypeRule(CheckItemTypeRuleDef itemType)
        {


            try
            {

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                CheckItemTypeRuleDef condition = new CheckItemTypeRuleDef
                {
                    ID = itemType.ID
                };
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        CheckItemTypeRule cond = FuncNew.SetColumnFromField<CheckItemTypeRule, CheckItemTypeRuleDef>(condition);

                        CheckItemTypeRule setv = FuncNew.SetColumnFromField<CheckItemTypeRule, CheckItemTypeRuleDef>(itemType, CheckItemTypeRule.fn_id);
                        sqlCtx = FuncNew.GetConditionedUpdate<CheckItemTypeRule>(new SetValueCollection<CheckItemTypeRule>(new CommonSetValue<CheckItemTypeRule>(setv)),
                                                                                                                   new ConditionCollection<CheckItemTypeRule>(new EqualCondition<CheckItemTypeRule>(cond)));
                        //                        sqlCtx = new SQLContextNew();
                        //                        sqlCtx.Sentence = @"update CheckItemTypeRule 
                        //                                                            set CheckItemType=@CheckItemType, 
                        //                                                                     Line=@Line, 
                        //                                                                     Station=@Station, 
                        //                                                                     Family=@Family, 
                        //                                                                     BomNodeType=@BomNodeType,
                        //                                                                     PartDescr=@PartDescr,
                        //                                                                     PartType=@PartType, 
                        //                                                                     MatchRule=@MatchRule, 
                        //                                                                     CheckRule=@CheckRule, 
                        //                                                                     SaveRule=@SaveRule, 
                        //                                                                     Descr=@Descr, 
                        //                                                                     Editor=@Editor,
                        //                                                                     Udt=GETDATE(),
                        //                                                                     NeedUniqueCheck=@NeedUniqueCheck,
                        //                                                                     NeedCommonSave=@NeedCommonSave,
                        //                                                                     CheckTestKPCount=@CheckTestKPCount,
                        //                                                                     NeedSave=@NeedSave
                        //                                                            where ID=@ID ";

                        //                        sqlCtx.AddParam("ID", new SqlParameter("@ID", SqlDbType.Int));

                        //                        sqlCtx.AddParam("CheckItemType", new SqlParameter("@CheckItemType", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("Line", new SqlParameter("@Line", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("Station", new SqlParameter("@Station", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("Family", new SqlParameter("@Family", SqlDbType.VarChar));

                        //                        sqlCtx.AddParam("BomNodeType", new SqlParameter("@BomNodeType", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("PartDescr", new SqlParameter("@PartDescr", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("PartType", new SqlParameter("@PartType", SqlDbType.VarChar));


                        //                        sqlCtx.AddParam("MatchRule", new SqlParameter("@MatchRule", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("CheckRule", new SqlParameter("@CheckRule", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("SaveRule", new SqlParameter("@SaveRule", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("Descr", new SqlParameter("@Descr", SqlDbType.VarChar));

                        //                        sqlCtx.AddParam("Editor", new SqlParameter("@Editor", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("NeedUniqueCheck", new SqlParameter("@NeedUniqueCheck", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("NeedCommonSave", new SqlParameter("@NeedCommonSave", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("CheckTestKPCount", new SqlParameter("@CheckTestKPCount", SqlDbType.VarChar));
                        //                        sqlCtx.AddParam("NeedSave", new SqlParameter("@NeedSave", SqlDbType.VarChar));

                        //                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }

                //sqlCtx.Param("ID").Value = itemType.ID;
                //sqlCtx.Param("CheckItemType").Value = itemType.CheckItemType;
                //sqlCtx.Param("Line").Value = string.IsNullOrEmpty(itemType.Line) ? "" : itemType.Line;
                //sqlCtx.Param("Station").Value = string.IsNullOrEmpty(itemType.Station) ? "" : itemType.Station;
                //sqlCtx.Param("Family").Value = string.IsNullOrEmpty(itemType.Family) ? "" : itemType.Family;

                //sqlCtx.Param("BomNodeType").Value = string.IsNullOrEmpty(itemType.BomNodeType) ? "" : itemType.BomNodeType;
                //sqlCtx.Param("PartDescr").Value = string.IsNullOrEmpty(itemType.PartDescr) ? "" : itemType.PartDescr;
                //sqlCtx.Param("PartType").Value = string.IsNullOrEmpty(itemType.PartType) ? "" : itemType.PartType;


                //sqlCtx.Param("MatchRule").Value = string.IsNullOrEmpty(itemType.MatchRule) ? "" : itemType.MatchRule;
                //sqlCtx.Param("CheckRule").Value = string.IsNullOrEmpty(itemType.CheckRule) ? "" : itemType.CheckRule;
                //sqlCtx.Param("SaveRule").Value = string.IsNullOrEmpty(itemType.SaveRule) ? "" : itemType.SaveRule;
                //sqlCtx.Param("Descr").Value = string.IsNullOrEmpty(itemType.Descr) ? "" : itemType.Descr;

                //sqlCtx.Param("Editor").Value = itemType.Editor;

                //sqlCtx.Param("NeedUniqueCheck").Value = itemType.NeedUniqueCheck;
                //sqlCtx.Param("NeedCommonSave").Value = itemType.NeedCommonSave;
                //sqlCtx.Param("NeedSave").Value = itemType.NeedSave;
                //sqlCtx.Param("CheckTestKPCount").Value = itemType.CheckTestKPCount;

                sqlCtx = FuncNew.SetColumnFromField<CheckItemTypeRule, CheckItemTypeRuleDef>(sqlCtx, condition);
                sqlCtx = FuncNew.SetColumnFromField<CheckItemTypeRule, CheckItemTypeRuleDef>(sqlCtx, itemType, true);
                string udtName = g.DecSV(CheckItemTypeRule.fn_udt);
                if (sqlCtx.ParamKeys.Contains(udtName))
                {
                    DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                    sqlCtx.Param(g.DecSV(CheckItemTypeRule.fn_udt)).Value = cmDt;
                }

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                                                                 CommandType.Text,
                                                                                                                                             sqlCtx.Sentence,
                                                                                                                                             sqlCtx.Params);

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region  Part Forbid function
        /// <summary>
        /// call Filter & match rule
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="station"></param>
        /// <param name="line"></param>
        /// <param name="family"></param>
        /// <param name="model"></param>
        /// <param name="mainObj"></param>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        public IFlatBOM GetModelFlatBOMByStationModelAndPartForbid(string customer, string station,
                                                                                                string line, string family,
                                                                                                string model, object mainObj,
                                                                                                string sessionKey)
        {
            try
            {
                string pdLine = line;
                if (line != null && line.Length > 1)
                    line = line.Substring(0, 1);

                IFlatBOM ret = null;

                IHierarchicalBOM hrchBom = GetHierarchicalBOMByModel(model);
                IList<string> checkItemTypes = null;
                
                checkItemTypes = getCheckItemTypeByPriority(customer, station, line, family, model);
                if (checkItemTypes != null && checkItemTypes.Count > 0)
                {
                    //Get PartForbid condition
                    IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                    IList<PartForbidPriorityInfo> partForbidPriorityInfoList = partRep.GetPartForbidWithFirstPriority(customer, pdLine, family, model, sessionKey);
                    foreach (string name in checkItemTypes)
                    {
                        IPartPolicy ppcy = PrtPcyRepository.GetPolicy(name);
                        if (ppcy == null)
                        {
                            throw new Exception("No setup CheckItemType:" + name + ", please check CheckItemType maintain!");
                        }

                        IFlatBOM fltBom = ppcy.FilterBOM(hrchBom, station, mainObj);
                        if (fltBom != null)
                        {
                            //Vincent[2014-04-17]:add policy object  in all FlatBomItem for different CheckItemType name and same dll name case
                            // save performance don't get policy data with each FlatBomItem
                            foreach (IFlatBOMItem item in fltBom.BomItems)
                            {
                                FlatBOMItem bomItem = item as FlatBOMItem;
                                if (bomItem != null)
                                {
                                    bomItem.GetType().GetField("_policy", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(bomItem, ppcy);
                                    bomItem.GetType().GetField("_partForbidList", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(bomItem, partForbidPriorityInfoList);
                                    bomItem.GetType().GetField("_sessionKey", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(bomItem, sessionKey);
                                }
                            }

                            if (ret == null)
                            {
                                ret = fltBom;
                            }
                            else
                            {
                                ret.Merge(fltBom);
                            }
                        }
                    }
                }
                else
                {
                    throw new FisException("BOM001", new string[] { });
                }
                return ret ?? new FlatBOM(); 
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IList<string> getCheckItemTypeByPriority(string customer, string station, 
                                                                                  string line, string family,
                                                                                  string model)
        {
            IList<string> checkItemTypes = null;
              
            StationCheckInfo condition = new StationCheckInfo();
            condition.customer = customer;
            condition.station = station;
            var res = SttRepository.GetStationCheckInfoList(condition);
            if (res != null && res.Count > 0)
            {

                IList<CheckItemTypePriority> priorityList = new List<CheckItemTypePriority>();
                foreach (var item in res)
                {
                    //Line+Model
                    if (string.IsNullOrEmpty(item.family) &&
                        !string.IsNullOrEmpty(item.line) &&
                       (new Regex(item.line)).IsMatch(line) &&
                        !string.IsNullOrEmpty(item.model) &&
                        (new Regex(item.model)).IsMatch(model))
                    {
                        priorityList.Add(new CheckItemTypePriority() { Priority = 0, CheckItemType = item.checkItemType });
                        continue;
                    }
                    //Line+Family
                    if (string.IsNullOrEmpty(item.model) &&
                         !string.IsNullOrEmpty(item.line) &&
                        (new Regex(item.line)).IsMatch(line) &&
                        !string.IsNullOrEmpty(item.family) &&
                        (new Regex(item.family)).IsMatch(family))
                    {
                        priorityList.Add(new CheckItemTypePriority() { Priority = 1, CheckItemType = item.checkItemType });
                        continue;
                    }

                    //Model
                    if (string.IsNullOrEmpty(item.line) &&
                        string.IsNullOrEmpty(item.family) &&
                        !string.IsNullOrEmpty(item.model) &&
                        (new Regex(item.model)).IsMatch(model))
                    {
                        priorityList.Add(new CheckItemTypePriority() { Priority = 2, CheckItemType = item.checkItemType });
                        continue;
                    }

                    //Family
                    if (string.IsNullOrEmpty(item.line) &&
                         string.IsNullOrEmpty(item.model) &&
                        !string.IsNullOrEmpty(item.family) &&
                        (new Regex(item.family)).IsMatch(family))
                    {
                        priorityList.Add(new CheckItemTypePriority() { Priority = 3, CheckItemType = item.checkItemType });
                        continue;
                    }

                    //Line
                    if (!string.IsNullOrEmpty(item.line)
                        && (new Regex(item.line)).IsMatch(line)
                        && string.IsNullOrEmpty(item.family)
                        && string.IsNullOrEmpty(item.model))
                    {
                        priorityList.Add(new CheckItemTypePriority() { Priority = 4, CheckItemType = item.checkItemType });
                        continue;
                    }

                    //Line,Family,Model are all empty
                    if (string.IsNullOrEmpty(item.line)
                        && string.IsNullOrEmpty(item.family)
                        && string.IsNullOrEmpty(item.model))
                    {
                        priorityList.Add(new CheckItemTypePriority() { Priority = 5, CheckItemType = item.checkItemType });
                    }
                }

                //Vincent add Check Station/CheckItemType
                //Mantis 2499: StationCheck 檢查邏輯處理全部都不符合情況
                if (priorityList.Count > 0)
                {
                    int priority = priorityList.Min(x => x.Priority);

                    checkItemTypes = (from p in priorityList
                                      where p.Priority == priority
                                      select p.CheckItemType
                                                 ).Distinct().ToList();
                }
            }
            return checkItemTypes;
        }

        
        #endregion

        #region for Docking get Component
        public IList<string> GetPnListByModelAndBomNodeType(IList<string> modelList, string bomNodeType, string prefixDescr)
        {
            try
            {
                IList<string> ret = new List<string>();

                ITableAndFields tf1 = null;
                ITableAndFields tf2 = null;
                ITableAndFields[] tafa = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        tf1 = new TableAndFields<Part_NEW>();
                        Part_NEW cond = new Part_NEW();
                        cond.bomNodeType = bomNodeType;
                        cond.flag = 1;
                        tf1.Conditions.Add(new EqualCondition<Part_NEW>(cond));
                        Part_NEW cond3 = new Part_NEW();
                        cond3.descr = prefixDescr + "%";
                        tf1.Conditions.Add(new LikeCondition<Part_NEW>(cond3));

                        tf1.AddRangeToGetFieldNames(Part_NEW.fn_partNo);

                        tf2 = new TableAndFields<ModelBOM_NEW>();
                        ModelBOM_NEW cond2 = new ModelBOM_NEW();
                        cond2.flag = 1;
                        tf2.Conditions.Add(new EqualCondition<ModelBOM_NEW>(cond2));

                        ModelBOM_NEW cond4 = new ModelBOM_NEW();
                        cond4.material = "[INSET]";
                        tf2.Conditions.Add(new InSetCondition<ModelBOM_NEW>(cond4));
                        tf2.ClearToGetFieldNames();

                        tafa = new ITableAndFields[] { tf1, tf2 };

                        TableConnectionCollection tblCnnts = new TableConnectionCollection(
                            new TableConnectionItem<Part_NEW, ModelBOM_NEW>(tf1, Part_NEW.fn_partNo, tf2, ModelBOM_NEW.fn_component));

                        sqlCtx = FuncNew.GetConditionedJoinedSelect(tk, "DISTINCT", tafa, tblCnnts);

                        sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_flag)).Value = cond.flag;
                        sqlCtx.Param(g.DecAlias(tf2.Alias, ModelBOM_NEW.fn_flag)).Value = cond2.flag;
                    }
                }



                tafa = sqlCtx.TableFields;
                tf1 = tafa[0];
                tf2 = tafa[1];

                sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_bomNodeType)).Value = bomNodeType;
                sqlCtx.Param(g.DecAlias(tf1.Alias, Part_NEW.fn_descr)).Value = prefixDescr;
                string Sentence = sqlCtx.Sentence.Replace(g.DecAlias(tf2.Alias, g.DecInSet(mtns::ModelBOM_NEW.fn_material)), g.ConvertInSet(new List<string>(modelList)));

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                                                   CommandType.Text,
                                                                                                                   Sentence,
                                                                                                                   sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(g.DecAlias(tf1.Alias, Part_NEW.fn_partNo)));
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

        public IList<string> WhereUsedComponent(string component)
        {
            try
            {
                IList<string> ret = new List<string>();

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::ModelBOM_NEW cond = new mtns::ModelBOM_NEW();
                        cond.component = component;
                        cond.flag = 1;
                        sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::ModelBOM_NEW>(tk, null, new string[] { mtns::ModelBOM_NEW.fn_material },
                                   new mtns::ConditionCollection<mtns::ModelBOM_NEW>(new mtns::EqualCondition<mtns::ModelBOM_NEW>(cond)),
                                   mtns::ModelBOM_NEW.fn_cdt);

                    }
                }
                sqlCtx.Param(ModelBOM_NEW.fn_component).Value = component;
                sqlCtx.Param(ModelBOM_NEW.fn_flag).Value = 1;


                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            ret.Add(g.GetValue_Str(sqlR, 0));
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

        #region Get ModelBOM CTE

        private IHierarchicalBOM GetHierarchicalBOMByModel_DB(string model)
        {
            try
            {
                IHierarchicalBOM ret = null;

                //IPart rootPart = PrtRepository.Find(model);
                IPart rootPart = null;

                IBOMNode root = new BOMNode(rootPart, 1);
                ret = new HierarchicalBOM(root);
                IDictionary<int, IList<IPart>> allPartList = new Dictionary<int, IList<IPart>>();
                IList<IBOMNode> allNodes = new List<IBOMNode>();
                ret.Nodes = allNodes;
                ret.Model = model;
                //IList<MoBOMInfo> rootChildren = GetChildrenInModelBOM(model);
                IList<ModelBOM> modelBOMList = GetModelBOM(model);
                var rootChildren = modelBOMList.Where(x => x.Level == 1 && x.Flag == 1).ToList();
                if (rootChildren != null && rootChildren.Count > 0)
                {
                    var partNoList = rootChildren.Select(x => x.Component).Distinct().ToList();
                    IList<IPart> rootPartList = PrtRepository.FindPart(partNoList);
                    allPartList.Add(1, rootPartList);
                    foreach (ModelBOM childItem in rootChildren)
                    {
                        //IPart nodePart = PrtRepository.Find(childItem.Component);
                        IPart nodePart = rootPartList.Where(x => x != null && x.PN == childItem.Component).FirstOrDefault();
                        if (nodePart != null)
                        {
                            IBOMNode newNode = new BOMNode(nodePart, int.Parse(childItem.Quantity), childItem.Alternative_item_group);
                            root.AddChild(newNode);
                            allNodes.Add(newNode);

                            int iDeepLimit = 20;
                            try
                            {
                                RecursivelyConstructHierarchicalBOM(newNode, childItem, allNodes, modelBOMList, iDeepLimit - 1, 2, allPartList);
                            }
                            catch (FisException fex)
                            {
                                if (fex.mErrcode == "BOML01")
                                {
                                    throw new FisException("BOML01", new string[] { iDeepLimit.ToString(), model });
                                }
                            }

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

        private void RecursivelyConstructHierarchicalBOM(IBOMNode node, ModelBOM modelBom,
                                                                                    IList<IBOMNode> allNodes, IList<ModelBOM> allModelBom,
                                                                                    int iDeepLimit, int level,
                                                                                    IDictionary<int, IList<IPart>> allPartList)
        {
            IList<ModelBOM> rootChildren = allModelBom.Where(x => x.Level == level &&
                                                                                                    x.Material == modelBom.Component &&
                                                                                                    x.Flag == 1).ToList();
            if (rootChildren != null && rootChildren.Count > 0)
            {
                if (iDeepLimit == 0)
                {
                    throw new FisException("BOML01", new string[] { "", "" });
                }

                IList<IPart> nodePartList = null;
                if (allPartList.ContainsKey(level))
                {
                    nodePartList = allPartList[level];
                }
                else
                {
                    var partNoList = allModelBom.Where(x => x.Level == level && x.Flag == 1).Select(x => x.Component).Distinct().ToList();
                    nodePartList = PrtRepository.FindPart(partNoList);
                    allPartList.Add(level, nodePartList);
                }

                foreach (ModelBOM childItem in rootChildren)
                {
                    //IPart nodePart = PrtRepository.Find(childItem.Component);
                    IPart nodePart = nodePartList.Where(x => x != null && x.PN == childItem.Component).FirstOrDefault();
                    if (nodePart != null)
                    {
                        IBOMNode newNode = new BOMNode(nodePart, int.Parse(childItem.Quantity), childItem.Alternative_item_group);
                        node.AddChild(newNode);
                        allNodes.Add(newNode);
                        RecursivelyConstructHierarchicalBOM(newNode, childItem, allNodes, allModelBom, iDeepLimit - 1, level + 1, allPartList);
                    }

                }
            }
        }

        public IList<ModelBOM> GetModelBOM(string material)
        {
            try
            {
                IList<ModelBOM> ret = new List<ModelBOM>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = new SQLContextNew();
                        sqlCtx.Sentence = @"with temp
                                                           as 
                                                           (
		                                                        select a.Material,
				                                                        a.Quantity, 
				                                                        a.Component, 
				                                                        a.Alternative_item_group,								
				                                                        a.Flag,	  
				                                                        1 as [Level], 
				                                                        a.Cdt, 
				                                                        a.Udt 
		                                                        from ModelBOM a 
		                                                        where Material= @Material		
	                                                        union all
		                                                        select  a1.Material, 
				                                                        a1.Quantity, 
				                                                        a1.Component,
				                                                        a1.Alternative_item_group,				
				                                                        a1.Flag,	   
			                                                            b1.[Level]+1, 
			                                                            a1.Cdt, 
			                                                            a1.Udt 
		                                                        from ModelBOM a1 
		                                                        inner join  temp b1 on(a1.Material = b1.Component)		   
	                                                        )	
                                                        select * from temp	order by [Level]";
                        sqlCtx.AddParam("Material", new SqlParameter("@Material", SqlDbType.VarChar));  
                        SQLCache.InsertIntoCache(tk, sqlCtx);
                    }
                }
                sqlCtx.Param("Material").Value = material;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM,
                                                                                                                            CommandType.Text,
                                                                                                                            sqlCtx.Sentence,
                                                                                                                            sqlCtx.Params))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        ret.Add(IMES.Infrastructure.Repository._Schema.SQLData.ToObjectByField<ModelBOM>(sqlR));
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

        #region Remove ModelBom Cache
        /// <summary>
        /// for Remove Cache item 
        /// </summary>
        /// <param name="modelNameList"></param>
        public void RemoveCacheByKeyList(IList<string> modelNameList)
        {
            try
            {
                lock (_syncObj_cache)
                {
                    foreach (string pk in modelNameList)
                    {
                        if (_cache.Contains(pk))
                        {
                            _cache.Remove(pk);
                        }
                    }
                }
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
    }

    public class CheckItemTypePriority
    {
        public int Priority;
        public string CheckItemType;
    }
}
