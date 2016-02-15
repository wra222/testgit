using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.PCA.EcrVersion;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Util;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;
using fons = IMES.FisObject.PCA.EcrVersion;
using IMES.DataModel;

namespace IMES.Infrastructure.Repository.PCA
{
    public class EcrVersionRepository : BaseRepository<fons::EcrVersion>, IEcrVersionRepository
    {
        private static GetValueClass g = new GetValueClass();

        #region Overrides of BaseRepository<IProduct>

        protected override void PersistNewItem(fons::EcrVersion item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Added)
                {
                    this.PersistInsertEcrVersion(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistUpdatedItem(fons::EcrVersion item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Modified)
                {
                    this.PersistUpdateEcrVersion(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        protected override void PersistDeletedItem(fons::EcrVersion item)
        {
            StateTracker tracker = item.Tracker;
            try
            {
                if (tracker.GetState(item) == DataRowState.Deleted)
                {
                    this.PersistDeleteEcrVersion(item);
                }
            }
            finally
            {
                tracker.Clear();
            }
        }

        #endregion

        #region Implementation of IRepository<IProduct>

        public override fons::EcrVersion Find(object key)
        {
            try
            {
                fons::EcrVersion ret = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.EcrVersion cond = new _Schema.EcrVersion();
                        cond.ID = (int)key;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.EcrVersion), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.EcrVersion.fn_ID].Value = (int)key;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new fons::EcrVersion();
                        ret.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_Cdt]);
                        ret.Remark = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_remark]);
                        ret.ECR = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_ECR]);
                        ret.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_Editor]);
                        ret.Family = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_Family]);
                        ret.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_ID]);
                        ret.IECVer = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_IECVer]);
                        ret.MBCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_MBCode]);
                        ret.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_Udt]);
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

        public override IList<fons::EcrVersion> FindAll()
        {
            try
            {
                IList<fons::EcrVersion> ret = new List<fons::EcrVersion>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.EcrVersion));
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, null))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        fons::EcrVersion item = new fons::EcrVersion();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_Cdt]);
                        item.Remark = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_remark]);
                        item.ECR = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_ECR]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_Editor]);
                        item.Family = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_Family]);
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_ID]);
                        item.IECVer = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_IECVer]);
                        item.MBCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_MBCode]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_Udt]);
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

        public override void Add(fons::EcrVersion item, IUnitOfWork uow)
        {
            base.Add(item, uow);
        }

        public override void Remove(fons::EcrVersion item, IUnitOfWork uow)
        {
            base.Remove(item, uow);
        }

        public override void Update(fons::EcrVersion item, IUnitOfWork uow)
        {
            base.Update(item, uow);
        }

        #endregion

        #region . Inners .

        private void PersistInsertEcrVersion(fons::EcrVersion item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetAquireIdInsert(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.EcrVersion));
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.EcrVersion.fn_Cdt].Value = cmDt;
                sqlCtx.Params[_Schema.EcrVersion.fn_remark].Value = item.Remark;
                sqlCtx.Params[_Schema.EcrVersion.fn_ECR].Value = item.ECR;
                sqlCtx.Params[_Schema.EcrVersion.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.EcrVersion.fn_Family].Value = item.Family;
                sqlCtx.Params[_Schema.EcrVersion.fn_IECVer].Value = item.IECVer;
                sqlCtx.Params[_Schema.EcrVersion.fn_MBCode].Value = item.MBCode;
                sqlCtx.Params[_Schema.EcrVersion.fn_Udt].Value = cmDt;
                item.ID = Convert.ToInt32(_Schema.SqlHelper.ExecuteScalar(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistUpdateEcrVersion(fons::EcrVersion item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonUpdate(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.EcrVersion));
                    }
                }
                sqlCtx.Params[_Schema.EcrVersion.fn_ID].Value = item.ID;
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Params[_Schema.EcrVersion.fn_remark].Value = item.Remark;
                sqlCtx.Params[_Schema.EcrVersion.fn_ECR].Value = item.ECR;
                sqlCtx.Params[_Schema.EcrVersion.fn_Editor].Value = item.Editor;
                sqlCtx.Params[_Schema.EcrVersion.fn_Family].Value = item.Family;
                sqlCtx.Params[_Schema.EcrVersion.fn_IECVer].Value = item.IECVer;
                sqlCtx.Params[_Schema.EcrVersion.fn_MBCode].Value = item.MBCode;
                sqlCtx.Params[_Schema.EcrVersion.fn_Udt].Value = cmDt;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void PersistDeleteEcrVersion(fons::EcrVersion item)
        {
            try
            {
                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = _Schema.Func.GetCommonDelete(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.EcrVersion));
                    }
                }
                sqlCtx.Params[_Schema.EcrVersion.fn_ID].Value = item.ID;
                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>());
            }
            catch (Exception)
            {
                LoggingError(this.GetType(), MethodBase.GetCurrentMethod());
                throw;
            }
        }

        #endregion

        #region For Maintain

        public IList<fons::EcrVersion> GetECRVersionListByFamily(string family)
        {
            //SELECT RTRIM(Family), RTRIM(MBCode), RTRIM(ECR), RTRIM(IECVer) as [IEC Version],
            //RTRIM(CustVer) as [Customer Version], RTRIM(Editor), Cdt as [Create Date], Udt as [Update Date]
            //FROM EcrVersion 
            //WHERE Family = @Family
            //ORDER BY Family, MBCode, ECR
            try
            {
                IList<fons::EcrVersion> ret = new List<fons::EcrVersion>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.EcrVersion cond = new _Schema.EcrVersion();
                        cond.Family = family;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.EcrVersion), cond, null, null);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(_Schema.Func.OrderBy, string.Join(",", new string[]{_Schema.EcrVersion.fn_Family, _Schema.EcrVersion.fn_MBCode, _Schema.EcrVersion.fn_ECR}));
                    }
                }
                sqlCtx.Params[_Schema.EcrVersion.fn_Family].Value = family;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        fons::EcrVersion item = new fons::EcrVersion();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_Cdt]);
                        item.Remark = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_remark]);
                        item.ECR = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_ECR]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_Editor]);
                        item.Family = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_Family]);
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_ID]);
                        item.IECVer = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_IECVer]);
                        item.MBCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_MBCode]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_Udt]);
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

        public IList<fons::EcrVersion> GetECRVersionByFamilyMBCodeAndECR(string family, string mbCode, string ecr)
        {
            // SELECT * FROM IMES_PCA..EcrVersion 
            // WHERE Family = @Family AND MBCode = @MBCode AND ECR = @ECR
            try
            {
                IList<fons::EcrVersion> ret = new List<fons::EcrVersion>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.EcrVersion cond = new _Schema.EcrVersion();
                        cond.Family = family;
                        cond.MBCode = mbCode;
                        cond.ECR = ecr;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.EcrVersion), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.EcrVersion.fn_Family].Value = family;
                sqlCtx.Params[_Schema.EcrVersion.fn_MBCode].Value = mbCode;
                sqlCtx.Params[_Schema.EcrVersion.fn_ECR].Value = ecr;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        fons::EcrVersion item = new fons::EcrVersion();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_Cdt]);
                        item.Remark = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_remark]);
                        item.ECR = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_ECR]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_Editor]);
                        item.Family = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_Family]);
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_ID]);
                        item.IECVer = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_IECVer]);
                        item.MBCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_MBCode]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_Udt]);
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

        public IList<string> GetFamilyInfoListForECRVersion()
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
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Family>(tk, "DISTINCT", new string[] { mtns::Family.fn_family }, new ConditionCollection<mtns::Family>(), mtns::Family.fn_family);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::Family.fn_family));
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

        public void UpdateEcrVersionMaintain(fons::EcrVersion item, string family, string mbcode, string ecr)
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
                        mtns::EcrVersion cond = new mtns::EcrVersion();
                        cond.family = family;
                        cond.mbcode = mbcode;
                        cond.ecr = ecr;
                        mtns::EcrVersion setv = FuncNew.SetColumnFromField<mtns::EcrVersion, fons::EcrVersion>(item, mtns::EcrVersion.fn_id, mtns::EcrVersion.fn_cdt);
                        setv.udt = DateTime.Now;
                        sqlCtx = FuncNew.GetConditionedUpdate<mtns::EcrVersion>(tk, new SetValueCollection<mtns::EcrVersion>(new CommonSetValue<mtns::EcrVersion>(setv)), new ConditionCollection<mtns::EcrVersion>(new EqualCondition<mtns::EcrVersion>(cond)));
                    }
                }
                sqlCtx.Param(mtns::EcrVersion.fn_family).Value = family;
                sqlCtx.Param(mtns::EcrVersion.fn_mbcode).Value = mbcode;
                sqlCtx.Param(mtns::EcrVersion.fn_ecr).Value = ecr;

                sqlCtx = FuncNew.SetColumnFromField<mtns::EcrVersion, fons::EcrVersion>(sqlCtx, item, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::EcrVersion.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<fons.EcrVersion> GetECRVersionByMBCodeAndECR(string mbCode, string ecr)
        {
            // SELECT * FROM IMES_PCA..EcrVersion 
            // WHERE Family = @Family AND MBCode = @MBCode AND ECR = @ECR
            try
            {
                IList<fons::EcrVersion> ret = new List<fons::EcrVersion>();

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.EcrVersion cond = new _Schema.EcrVersion();
                        cond.MBCode = mbCode;
                        cond.ECR = ecr;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.EcrVersion), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.EcrVersion.fn_MBCode].Value = mbCode;
                sqlCtx.Params[_Schema.EcrVersion.fn_ECR].Value = ecr;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    while (sqlR != null && sqlR.Read())
                    {
                        fons::EcrVersion item = new fons::EcrVersion();
                        item.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_Cdt]);
                        item.Remark = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_remark]);
                        item.ECR = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_ECR]);
                        item.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_Editor]);
                        item.Family = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_Family]);
                        item.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_ID]);
                        item.IECVer = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_IECVer]);
                        item.MBCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_MBCode]);
                        item.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_Udt]);
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

        public IList<string> GetFamilyInfoListForSA(string bomNodeType)
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
                        mtns::Part_NEW cond = new mtns::Part_NEW();
                        cond.bomNodeType = bomNodeType;
                        cond.flag = 1;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Part_NEW>(tk, "DISTINCT", new string[] { mtns::Part_NEW.fn_descr }, new ConditionCollection<mtns::Part_NEW>(new EqualCondition<mtns::Part_NEW>(cond)), mtns::Part_NEW.fn_descr);

                        sqlCtx.Param(mtns::Part_NEW.fn_flag).Value = cond.flag;
                    }
                }
                sqlCtx.Param(mtns::Part_NEW.fn_bomNodeType).Value = bomNodeType;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns::Part_NEW.fn_descr));
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

        public void UpdateEcrVersionMaintainDefered(IUnitOfWork uow, fons::EcrVersion item, string family, string mbcode, string ecr)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item, family, mbcode, ecr);
        }

        #endregion

        #endregion


        #region for FRUMBVer
        public IList<FruMBVerInfo> GetFruMBVer()
        {
            try
            {
                IList<FruMBVerInfo> ret = new List<FruMBVerInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetCommonSelect<_Metas.FruMBVer>(tk, _Metas.FruMBVer.fn_partNo, _Metas.FruMBVer.fn_ver);

                    }
                }

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<mtns::FruMBVer, FruMBVerInfo, FruMBVerInfo>(ret, sqlR, sqlCtx);
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }           
        }

        public IList<FruMBVerInfo> GetFruMBVer(FruMBVerInfo condition)
        {
            try
            {
                IList<FruMBVerInfo> ret = new List<FruMBVerInfo>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();
               
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {

                    FruMBVer cond = FuncNew.SetColumnFromField<FruMBVer, FruMBVerInfo>(condition);

                    sqlCtx = FuncNew.GetConditionedSelect<FruMBVer>(null, null,
                                                                                                                            new ConditionCollection<FruMBVer>(new EqualCondition<FruMBVer>(cond)),
                                                                                                                           _Metas.FruMBVer.fn_ver);

                   
                }
                sqlCtx = FuncNew.SetColumnFromField<FruMBVer, FruMBVerInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<mtns::FruMBVer, FruMBVerInfo, FruMBVerInfo>(ret, sqlR, sqlCtx);
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }           
        }

        public IList<string> GetPartNoInFruMBVer()
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
                        sqlCtx = FuncNew.GetConditionedSelect<FruMBVer>(tk, "DISTINCT", new string[] { FruMBVer.fn_partNo }, new ConditionCollection<FruMBVer>(), FruMBVer.fn_partNo); 
                      
                    }
                }

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(FruMBVer.fn_partNo));
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

        public void InsertFruMBVer(FruMBVerInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<FruMBVer>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<FruMBVer, FruMBVerInfo>(sqlCtx, item);

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

        public void UpdateFruMBVer(FruMBVerInfo item)
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
                        FruMBVer cond = new FruMBVer();
                        cond.id = item.id;
                        FruMBVer setv = FuncNew.SetColumnFromField<FruMBVer, FruMBVerInfo>(item, FruMBVer.fn_id);
                        setv.udt = DateTime.Now;
                       
                        sqlCtx = FuncNew.GetConditionedUpdate<FruMBVer>(tk,
                                                                                                                          new SetValueCollection<FruMBVer>(new CommonSetValue<FruMBVer>(setv)),
                                                                                                                          new ConditionCollection<FruMBVer>(new EqualCondition<FruMBVer>(cond)));
                    }
                }

                sqlCtx.Param(FruMBVer.fn_id).Value = item.id;

                sqlCtx = FuncNew.SetColumnFromField<FruMBVer, FruMBVerInfo>(sqlCtx, item, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(FruMBVer.fn_udt)).Value = cmDt;

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

        public void RemoveFruMBVer(int id)
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
                        FruMBVer cond = new FruMBVer();
                        cond.id = id;

                        sqlCtx = FuncNew.GetConditionedDelete<_Metas.FruMBVer>(tk, new ConditionCollection<_Metas.FruMBVer>(new EqualCondition<_Metas.FruMBVer>(cond)));
                    }
                }

                sqlCtx.Param(_Metas.FruMBVer.fn_id).Value = id;
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

        public IList<fons::EcrVersion> GetECRVersion(fons::EcrVersion condition)
        {
            try
            {
                IList<fons::EcrVersion> ret = new List<fons::EcrVersion>();
                MethodBase mthObj = MethodBase.GetCurrentMethod();

                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {

                    mtns::EcrVersion cond = FuncNew.SetColumnFromField<mtns::EcrVersion, fons::EcrVersion>(condition);

                    sqlCtx = FuncNew.GetConditionedSelect<mtns::EcrVersion>(null, null,
                                                                                                                            new ConditionCollection<mtns::EcrVersion>(new EqualCondition<mtns::EcrVersion>(cond)),
                                                                                                                           mtns::EcrVersion.fn_iecver);


                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::EcrVersion, fons::EcrVersion>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData,
                                                                                                                             CommandType.Text,
                                                                                                                             sqlCtx.Sentence,
                                                                                                                             sqlCtx.Params))
                {

                    ret = FuncNew.SetFieldFromColumn<mtns::EcrVersion, fons::EcrVersion, fons::EcrVersion>(ret, sqlR, sqlCtx);
                }

                return ret;
            }
            catch (Exception)
            {
                throw;
            }           
        }
        #endregion
    }
}
