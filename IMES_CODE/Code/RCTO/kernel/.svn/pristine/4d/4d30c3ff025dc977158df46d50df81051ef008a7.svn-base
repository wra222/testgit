// 2010-03-03 Liu Dong(eB1-4)         Modify ITC-1103-0237
// 2010-03-22 Liu Dong(eB1-4)         Modify 与Yolanda 确认SB 类型的Part ，在Generate SMT MO 和MB Label Print 不要限制其MDL 属性必须以B SIDE 字串作为结尾
// 2010-06-02 Liu Dong(eB1-4)         Modify 修改成: RIGHT(ECR, 2) = RIGHT(@UIEcr, 2)
// 2010-06-04 Liu Dong(eB1-4)         Modify ECR后2位匹配就算存在.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.PCA.MBModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.DataModel;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Repository._Schema;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;
using fons = IMES.FisObject.Common.Part;

//using IMES.FisObject.Common.Part;
//

namespace IMES.Infrastructure.Repository.PCA
{
    public class MBModelRepository : BaseRepository<IMBModel>, IMBModelRepository
    {
        private static GetValueClass g = new GetValueClass();

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

        private static IMES.FisObject.PCA.MB.IMBRepository _mbRepository = null;
        private static IMES.FisObject.PCA.MB.IMBRepository MbRepository
        {
            get
            {
                if (_mbRepository == null)
                    _mbRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.PCA.MB.IMBRepository, IMES.FisObject.PCA.MB.IMB>();
                return _mbRepository;
            }
        }

        private static IMES.FisObject.Common.Model.IModelRepository _mdlRepository = null;
        private static IMES.FisObject.Common.Model.IModelRepository MdlRepository
        {
            get
            {
                if (_mdlRepository == null)
                    _mdlRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Model.IModelRepository, IMES.FisObject.Common.Model.Model>();
                return _mdlRepository;
            }
        }

        private static IMES.FisObject.Common.Model.IFamilyRepository _familyRepository = null;
        private static IMES.FisObject.Common.Model.IFamilyRepository FamilyRepository
        {
            get
            {
                if (_familyRepository == null)
                    _familyRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Model.IFamilyRepository>();
                return _familyRepository;
            }
        }
        #endregion

        #region Overrides of BaseRepository<IMBModel>

        protected override void PersistNewItem(IMBModel item)
        {
            throw new NotImplementedException("Normal");
        }

        protected override void PersistUpdatedItem(IMBModel item)
        {
            throw new NotImplementedException("Normal");
        }

        protected override void PersistDeletedItem(IMBModel item)
        {
            throw new NotImplementedException("Normal");
        }

        #endregion

        #region Implementation of IRepository<IMBModel>

        /// <summary>
        /// 根据对象key获取对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>对象实例</returns>
        public override IMBModel Find(object key)
        {
            try
            {
                MBModel ret = null;

                IMES.FisObject.Common.Part.IPart part = PrtRepository.Find(key);

                if (part != null)
                {
                    string mbCode = string.Empty;
                    string mbType = MbRepository.TryToGetMBType(part, out mbCode);

                    ret = new MBModel(
                        part.PN,
                        mbCode,
                        part.GetAttribute("MDL"),
                        mbType,
                        part.Descr
                        );
                    ret.Tracker.Clear();
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
        public override IList<IMBModel> FindAll()
        {
            throw new NotImplementedException("Normal");
        }

        /// <summary>
        /// 添加一个对象
        /// </summary>
        /// <param name="item">新添加的对象</param>
        public override void Add(IMBModel item, IUnitOfWork work)
        {
            base.Add(item, work);
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="item">需删除的对象</param>
        public override void Remove(IMBModel item, IUnitOfWork work)
        {
            base.Remove(item, work);
        }

        /// <summary>
        /// 更新指定对象
        /// </summary>
        /// <param name="item">需更新的对象</param>
        /// <param name="work"></param>
        public override void Update(IMBModel item, IUnitOfWork work)
        {
            base.Update(item, work);
        }

        #endregion

        #region Implementation of IMBModelRepository

        /// <summary>
        /// 取得主板代码集合
        /// </summary>
        /// <returns>主板代码集合</returns>
        public IList<MB_CODEInfo> GetMBCodeList()
        {
            try
            {
                IList<MB_CODEInfo> ret = new List<MB_CODEInfo>();

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartInfo invalCond = new _Schema.PartInfo();
                        invalCond.InfoType = "INSET";
                        sqlCtx = Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartInfo), "DISTINCT", new List<string>() { _Schema.PartInfo.fn_InfoValue }, null, null, null, null, null, null, null, invalCond);
                        sqlCtx.Sentence = sqlCtx.Sentence.Replace(Func.DecInSet(_Schema.PartInfo.fn_InfoType), Func.ConvertInSet(new List<string>(MB.MBType.GetAllTypes())));
                    }
                }

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            MB_CODEInfo item = new MB_CODEInfo();
                            item.friendlyName = item.id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartInfo.fn_InfoValue]);
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

        public IList<MB_CODEAndMDLInfo> GetMBCodeAndMdlList()
        {
            try
            {
                IList<MB_CODEAndMDLInfo> ret = new List<MB_CODEAndMDLInfo>();

                SQLContext sqlCtx = null;
                TableAndFields tf1 = null;
                TableAndFields tf2 = null;
                TableAndFields tf3 = null;
                TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new TableAndFields();
                        tf1.Table = typeof(Part);
                        // 2010-03-03 Liu Dong(eB1-4)         Modify ITC-1103-0237
                        Part invalCond1 = new Part();
                        invalCond1.PartType = "INSET";
                        tf1.inSetcond = invalCond1;
                        // 2010-03-03 Liu Dong(eB1-4)         Modify ITC-1103-0237
                        tf1.ToGetFieldNames = null;

                        tf2 = new TableAndFields();
                        tf2.Table = typeof(_Schema.PartInfo);
                        _Schema.PartInfo invalCond = new _Schema.PartInfo();
                        invalCond.InfoType = "INSET";
                        tf2.inSetcond = invalCond;
                        tf2.ToGetFieldNames.Add(_Schema.PartInfo.fn_InfoValue);

                        tf3 = new TableAndFields();
                        tf3.Table = typeof(_Schema.PartInfo);
                        _Schema.PartInfo equalCond = new _Schema.PartInfo();
                        equalCond.InfoType = "MDL";
                        tf3.equalcond = equalCond;
                        // 2010-03-22 Liu Dong(eB1-4)         Modify 与Yolanda 确认SB 类型的Part ，在Generate SMT MO 和MB Label Print 不要限制其MDL 属性必须以B SIDE 字串作为结尾
                        //PartInfo likeCond = new PartInfo();
                        //likeCond.InfoValue = "%B SIDE";
                        //tf3.likecond = likeCond;
                        // 2010-03-22 Liu Dong(eB1-4)         Modify 与Yolanda 确认SB 类型的Part ，在Generate SMT MO 和MB Label Print 不要限制其MDL 属性必须以B SIDE 字串作为结尾
                        tf3.ToGetFieldNames.Add(_Schema.PartInfo.fn_InfoValue);

                        List<TableConnectionItem> tblCnntIs = new List<TableConnectionItem>();
                        TableConnectionItem tc1 = new TableConnectionItem(tf1, Part.fn_PartNo, tf2, _Schema.PartInfo.fn_PartNo);
                        tblCnntIs.Add(tc1);
                        TableConnectionItem tc2 = new TableConnectionItem(tf1, Part.fn_PartNo, tf3, _Schema.PartInfo.fn_PartNo);
                        tblCnntIs.Add(tc2);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new TableAndFields[] { tf1, tf2, tf3 };
                        sqlCtx = Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);

                        // 2010-03-03 Liu Dong(eB1-4)         Modify ITC-1103-0237
                        sqlCtx.Sentence = sqlCtx.Sentence.Replace(Func.DecAlias(tf1.alias, Func.DecInSet(Part.fn_PartType)), Func.ConvertInSet(new List<string>(MB.MBType.GetAllTypes())));
                        // 2010-03-03 Liu Dong(eB1-4)         Modify ITC-1103-0237
                        sqlCtx.Sentence = sqlCtx.Sentence.Replace(Func.DecAlias(tf2.alias, Func.DecInSet(_Schema.PartInfo.fn_InfoType)), Func.ConvertInSet(new List<string>(MB.MBType.GetAllTypes())));

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(Func.OrderBy, Func.DecAliasInner(tf3.alias, _Schema.PartInfo.fn_InfoValue));

                        sqlCtx.Params[Func.DecAlias(tf3.alias, _Schema.PartInfo.fn_InfoType)].Value = equalCond.InfoType;
                        // 2010-03-22 Liu Dong(eB1-4)         Modify 与Yolanda 确认SB 类型的Part ，在Generate SMT MO 和MB Label Print 不要限制其MDL 属性必须以B SIDE 字串作为结尾
                        //sqlCtx.Params[Func.DecAlias(tf3.alias, PartInfo.fn_InfoValue)].Value = likeCond.InfoValue;
                        // 2010-03-22 Liu Dong(eB1-4)         Modify 与Yolanda 确认SB 类型的Part ，在Generate SMT MO 和MB Label Print 不要限制其MDL 属性必须以B SIDE 字串作为结尾
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];
                tf3 = tblAndFldsesArray[2];

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            MB_CODEAndMDLInfo item = new MB_CODEAndMDLInfo();
                            item.mbCode = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, _Schema.PartInfo.fn_InfoValue)]);
                            item.mdl = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf3.alias, _Schema.PartInfo.fn_InfoValue)]);
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

        public IList<MB_CODEAndMDLInfo> GetMBCodeAndMdlListExceptPrinted()
        {
            try
            {
                IList<MB_CODEAndMDLInfo> ret = new List<MB_CODEAndMDLInfo>();

                SQLContext sqlCtx = null;
                TableAndFields tf1 = null;
                TableAndFields tf2 = null;
                TableAndFields tf3 = null;
                TableAndFields tf4 = null;
                TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new TableAndFields();
                        tf1.Table = typeof(Part);
                        Part invalCond1 = new Part();
                        invalCond1.PartType = "INSET";
                        tf1.inSetcond = invalCond1;
                        tf1.ToGetFieldNames = null;

                        tf2 = new TableAndFields();
                        tf2.Table = typeof(_Schema.PartInfo);
                        _Schema.PartInfo invalCond = new _Schema.PartInfo();
                        invalCond.InfoType = "INSET";
                        tf2.inSetcond = invalCond;
                        tf2.ToGetFieldNames.Add(_Schema.PartInfo.fn_InfoValue);

                        tf3 = new TableAndFields();
                        tf3.Table = typeof(_Schema.PartInfo);
                        _Schema.PartInfo equalCond = new _Schema.PartInfo();
                        equalCond.InfoType = "MDL";
                        tf3.equalcond = equalCond;
                        // 2010-03-22 Liu Dong(eB1-4)         Modify 与Yolanda 确认SB 类型的Part ，在Generate SMT MO 和MB Label Print 不要限制其MDL 属性必须以B SIDE 字串作为结尾
                        //PartInfo likeCond = new PartInfo();
                        //likeCond.InfoValue = "%B SIDE";
                        //tf3.likecond = likeCond;
                        // 2010-03-22 Liu Dong(eB1-4)         Modify 与Yolanda 确认SB 类型的Part ，在Generate SMT MO 和MB Label Print 不要限制其MDL 属性必须以B SIDE 字串作为结尾
                        tf3.ToGetFieldNames.Add(_Schema.PartInfo.fn_InfoValue);

                        tf4 = new TableAndFields();
                        tf4.Table = typeof(SMTMO);
                        SMTMO necond = new SMTMO();
                        necond.Status = "C";
                        tf4.notEqualcond = necond;
                        tf4.subDBCalalog = SqlHelper.DB_PCA;
                        tf4.ToGetFieldNames = null;

                        List<TableConnectionItem> tblCnntIs = new List<TableConnectionItem>();
                        TableConnectionItem tc1 = new TableConnectionItem(tf1, Part.fn_PartNo, tf2, _Schema.PartInfo.fn_PartNo);
                        tblCnntIs.Add(tc1);
                        TableConnectionItem tc2 = new TableConnectionItem(tf1, Part.fn_PartNo, tf3, _Schema.PartInfo.fn_PartNo);
                        tblCnntIs.Add(tc2);
                        TableConnectionItem tc3 = new TableConnectionItem(tf1, Part.fn_PartNo, tf4, SMTMO.fn_IECPartNo);
                        tblCnntIs.Add(tc3);
                        TableConnectionItem tc4 = new TableConnectionItem(tf4, SMTMO.fn_Qty, tf4, SMTMO.fn_PrintQty, "{0}>{1}");
                        tblCnntIs.Add(tc4);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new TableAndFields[] { tf1, tf2, tf3, tf4 };
                        sqlCtx = Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace(Func.DecAlias(tf1.alias, Func.DecInSet(Part.fn_PartType)), Func.ConvertInSet(new List<string>(MB.MBType.GetAllTypes())));

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace(Func.DecAlias(tf2.alias, Func.DecInSet(_Schema.PartInfo.fn_InfoType)), Func.ConvertInSet(new List<string>(MB.MBType.GetAllTypes())));

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(Func.OrderBy, Func.DecAliasInner(tf3.alias, _Schema.PartInfo.fn_InfoValue));

                        sqlCtx.Params[Func.DecAlias(tf3.alias, _Schema.PartInfo.fn_InfoType)].Value = equalCond.InfoType;
                        // 2010-03-22 Liu Dong(eB1-4)         Modify 与Yolanda 确认SB 类型的Part ，在Generate SMT MO 和MB Label Print 不要限制其MDL 属性必须以B SIDE 字串作为结尾
                        //sqlCtx.Params[Func.DecAlias(tf3.alias, PartInfo.fn_InfoValue)].Value = likeCond.InfoValue;
                        // 2010-03-22 Liu Dong(eB1-4)         Modify 与Yolanda 确认SB 类型的Part ，在Generate SMT MO 和MB Label Print 不要限制其MDL 属性必须以B SIDE 字串作为结尾
                        sqlCtx.Params[Func.DecAlias(tf4.alias, SMTMO.fn_Status)].Value = necond.Status;
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];
                tf3 = tblAndFldsesArray[2];
                tf4 = tblAndFldsesArray[3];

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            MB_CODEAndMDLInfo item = new MB_CODEAndMDLInfo();
                            item.mbCode = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, _Schema.PartInfo.fn_InfoValue)]);
                            item.mdl = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf3.alias, _Schema.PartInfo.fn_InfoValue)]);
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
        /// 取得同一主板代码的所有主板型号对象
        /// </summary>
        /// <param name="mbCode">主板代码</param>
        /// <returns>主板型号对象集合</returns>
        public IList<IMBModel> GetMBModelByMBCode(string mbCode)
        {
            try
            {
                IList<IMBModel> ret = new List<IMBModel>();

                SQLContext sqlCtx = null;
                TableAndFields tf1 = null;
                TableAndFields tf2 = null;
                TableAndFields tf3 = null;
                TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new TableAndFields();
                        tf1.Table = typeof(Part);
                        // 2010-03-03 Liu Dong(eB1-4)         Modify ITC-1103-0237
                        Part invalCond1 = new Part();
                        invalCond1.PartType = "INSET";
                        tf1.inSetcond = invalCond1;
                        // 2010-03-03 Liu Dong(eB1-4)         Modify ITC-1103-0237
                        tf1.ToGetFieldNames.Add(Part.fn_PartNo);
                        tf1.ToGetFieldNames.Add(Part.fn_Descr);

                        tf2 = new TableAndFields();
                        tf2.Table = typeof(_Schema.PartInfo);
                        _Schema.PartInfo cond = new _Schema.PartInfo();
                        cond.InfoValue = mbCode;
                        tf2.equalcond = cond;
                        _Schema.PartInfo invalCond = new _Schema.PartInfo();
                        invalCond.InfoType = "INSET";
                        tf2.inSetcond = invalCond;
                        tf2.ToGetFieldNames.Add(_Schema.PartInfo.fn_InfoType);
                        tf2.ToGetFieldNames.Add(_Schema.PartInfo.fn_InfoValue);

                        tf3 = new TableAndFields();
                        tf3.Table = typeof(_Schema.PartInfo);
                        _Schema.PartInfo equalCond = new _Schema.PartInfo();
                        equalCond.InfoType = "MDL";
                        tf3.equalcond = equalCond;
                        tf3.ToGetFieldNames.Add(_Schema.PartInfo.fn_InfoValue);

                        List<TableConnectionItem> tblCnntIs = new List<TableConnectionItem>();
                        TableConnectionItem tc1 = new TableConnectionItem(tf1, Part.fn_PartNo, tf2, _Schema.PartInfo.fn_PartNo);
                        tblCnntIs.Add(tc1);
                        TableConnectionItem tc2 = new TableConnectionItem(tf1, Part.fn_PartNo, tf3, _Schema.PartInfo.fn_PartNo);
                        tblCnntIs.Add(tc2);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new TableAndFields[] { tf1, tf2, tf3 };
                        sqlCtx = Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, null, ref tblAndFldsesArray, tblCnnts);

                        // 2010-03-03 Liu Dong(eB1-4)         Modify ITC-1103-0237
                        sqlCtx.Sentence = sqlCtx.Sentence.Replace(Func.DecAlias(tf1.alias, Func.DecInSet(Part.fn_PartType)), Func.ConvertInSet(new List<string>(MB.MBType.GetAllTypes())));
                        // 2010-03-03 Liu Dong(eB1-4)         Modify ITC-1103-0237
                        sqlCtx.Sentence = sqlCtx.Sentence.Replace(Func.DecAlias(tf2.alias, Func.DecInSet(_Schema.PartInfo.fn_InfoType)), Func.ConvertInSet(new List<string>(MB.MBType.GetAllTypes())));

                        sqlCtx.Params[Func.DecAlias(tf3.alias, _Schema.PartInfo.fn_InfoType)].Value = equalCond.InfoType;
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];
                tf3 = tblAndFldsesArray[2];

                sqlCtx.Params[Func.DecAlias(tf2.alias, _Schema.PartInfo.fn_InfoValue)].Value = mbCode;

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            IMBModel item = new MBModel(GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, Part.fn_PartNo)]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, _Schema.PartInfo.fn_InfoValue)]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf3.alias, _Schema.PartInfo.fn_InfoValue)]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, _Schema.PartInfo.fn_InfoType)]),
                                                        GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, Part.fn_Descr)]));
                            ((MBModel)item).Tracker.Clear();
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

        public string getTypeByModel(string model)
        {
            try
            {
                IMES.FisObject.Common.Part.IPart part = PrtRepository.Find(model);
                if (part != null)
                {
                    return part.Type;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<EcrVersionInfo> getEcrVersionsByEcrAndMbcode(string ecr, string mbcode)
        {
            try
            {
                if (ecr != null)
                {
                    if (ecr.Length > 2)
                        ecr = ecr.Substring(ecr.Length - 2);
                }
                else
                {
                    ecr = string.Empty;
                }

                IList<EcrVersionInfo> ret = new List<EcrVersionInfo>();
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        // 2010-06-02 Liu Dong(eB1-4)         Modify 修改成: RIGHT(ECR, 2) = RIGHT(@UIEcr, 2)
                        _Schema.EcrVersion cond = new _Schema.EcrVersion();
                        //cond.ECR = ecr;
                        cond.MBCode = mbcode;
                        _Schema.EcrVersion likeCond = new _Schema.EcrVersion();
                        likeCond.ECR = "%" + ecr;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.EcrVersion), null, null, cond, likeCond, null, null, null, null, null, null);
                        // 2010-06-02 Liu Dong(eB1-4)         Modify 修改成: RIGHT(ECR, 2) = RIGHT(@UIEcr, 2)
                    }
                }
                sqlCtx.Params[_Schema.EcrVersion.fn_ECR].Value = "%" + ecr;
                sqlCtx.Params[_Schema.EcrVersion.fn_MBCode].Value = mbcode;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            EcrVersionInfo val = new EcrVersionInfo();
                            val.Cdt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_Cdt]);
                            val.Remark = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_remark]);
                            val.ECR = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_ECR]);
                            val.Editor = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_Editor]);
                            val.Family = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_Family]);
                            val.ID = GetValue_Int32(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_ID]);
                            val.IECVer = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_IECVer]);
                            val.MBCode = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_MBCode]);
                            val.Udt = GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_Udt]);
                            ret.Add(val);
                        }
                    }
                }
                return ret;
            }
            catch(Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get 111 Level List from database
        /// </summary>
        /// <param name="mbCodeId">MB_CODE Identifier</param>
        /// <returns>111 Level Info List</returns>
        public IList<_111LevelInfo> Get111LevelList(string mbCodeId)
        {
            try
            {
                IList<_111LevelInfo> ret = new List<_111LevelInfo>();
                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.PartInfo invalCond = new _Schema.PartInfo();
                        invalCond.InfoType = "INSET";

                        _Schema.PartInfo equalCond = new _Schema.PartInfo();
                        equalCond.InfoValue = mbCodeId;

                        sqlCtx = Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.PartInfo), "DISTINCT", new List<string>() { _Schema.PartInfo.fn_PartNo }, equalCond, null, null, null, null, null, null, invalCond);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace(Func.DecInSet(_Schema.PartInfo.fn_InfoType), Func.ConvertInSet(new List<string>(MB.MBType.GetAllTypes())));
                    }
                }

                sqlCtx.Params[_Schema.PartInfo.fn_InfoValue].Value = mbCodeId;

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            _111LevelInfo item = new _111LevelInfo();
                            item.friendlyName = item.id = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.PartInfo.fn_PartNo]);
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

        public IList<_111LevelInfo> Get111LevelListExceptPrinted(string mbCodeId)
        {
            try
            {
                IList<_111LevelInfo> ret = new List<_111LevelInfo>();

                SQLContext sqlCtx = null;
                TableAndFields tf1 = null;
                TableAndFields tf2 = null;
                TableAndFields tf3 = null;
                TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new TableAndFields();
                        tf1.Table = typeof(Part);
                        Part invalCond1 = new Part();
                        invalCond1.PartType = "INSET";
                        tf1.inSetcond = invalCond1;
                        tf1.ToGetFieldNames = null;

                        tf2 = new TableAndFields();
                        tf2.Table = typeof(_Schema.PartInfo);
                        _Schema.PartInfo eqCond = new _Schema.PartInfo();
                        eqCond.InfoValue = mbCodeId;
                        tf2.equalcond = eqCond;
                        _Schema.PartInfo invalCond = new _Schema.PartInfo();
                        invalCond.InfoType = "INSET";
                        tf2.inSetcond = invalCond;
                        tf2.ToGetFieldNames.Add(_Schema.PartInfo.fn_PartNo);

                        tf3 = new TableAndFields();
                        tf3.Table = typeof(SMTMO);
                        SMTMO necond = new SMTMO();
                        necond.Status = "C";
                        tf3.notEqualcond = necond;
                        tf3.subDBCalalog = SqlHelper.DB_PCA;
                        tf3.ToGetFieldNames = null;

                        List<TableConnectionItem> tblCnntIs = new List<TableConnectionItem>();
                        TableConnectionItem tc1 = new TableConnectionItem(tf1, Part.fn_PartNo, tf2, _Schema.PartInfo.fn_PartNo);
                        tblCnntIs.Add(tc1);
                        TableConnectionItem tc2 = new TableConnectionItem(tf1, Part.fn_PartNo, tf3, SMTMO.fn_IECPartNo);
                        tblCnntIs.Add(tc2);
                        TableConnectionItem tc3 = new TableConnectionItem(tf3, SMTMO.fn_Qty, tf3, SMTMO.fn_PrintQty, "{0}>{1}");
                        tblCnntIs.Add(tc3);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new TableAndFields[] { tf1, tf2, tf3 };
                        sqlCtx = Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Sentence = sqlCtx.Sentence.Replace(Func.DecAlias(tf1.alias, Func.DecInSet(Part.fn_PartType)), Func.ConvertInSet(new List<string>(MB.MBType.GetAllTypes())));
                        sqlCtx.Sentence = sqlCtx.Sentence.Replace(Func.DecAlias(tf2.alias, Func.DecInSet(_Schema.PartInfo.fn_InfoType)), Func.ConvertInSet(new List<string>(MB.MBType.GetAllTypes())));

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(Func.OrderBy, Func.DecAliasInner(tf2.alias, _Schema.PartInfo.fn_PartNo));

                        sqlCtx.Params[Func.DecAlias(tf3.alias, SMTMO.fn_Status)].Value = necond.Status;
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];
                tf3 = tblAndFldsesArray[2];

                sqlCtx.Params[Func.DecAlias(tf2.alias, _Schema.PartInfo.fn_InfoValue)].Value = mbCodeId;

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            _111LevelInfo item = new _111LevelInfo();
                            item.friendlyName = item.id = GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf2.alias, _Schema.PartInfo.fn_PartNo)]);
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
        /// 取得1397阶信息列表
        /// </summary>
        /// <param name="familyId">Family标识</param>
        /// <returns>1397阶信息列表</returns>
        public IList<_1397LevelInfo> Get1397LevelList(string familyId)
        {
            try
            {
                return MdlRepository.Get1397ListFor008(familyId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Lazy load of MBCode of MBModel
        /// </summary>
        /// <param name="mbModel"></param>
        /// <returns></returns>
        public MBModel FillMBCodeObj(MBModel mbModel)
        {
            try
            {
                IMES.FisObject.PCA.MBModel.MBCode newFieldVal = null;

                _Schema.SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!_Schema.Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        _Schema.MBCode cond = new _Schema.MBCode();
                        cond.mbCode = mbModel.Mbcode;
                        sqlCtx = _Schema.Func.GetConditionedSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.MBCode), cond, null, null);
                    }
                }
                sqlCtx.Params[_Schema.MBCode.fn_mbCode].Value = mbModel.Mbcode;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        newFieldVal = new IMES.FisObject.PCA.MBModel.MBCode(GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MBCode.fn_mbCode]),
                                                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MBCode.fn_Description]),
                                                                            GetValue_Int16(sqlR, sqlCtx.Indexes[_Schema.MBCode.fn_MultiQty]),
                                                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MBCode.fn_Editor]),
                                                                            GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MBCode.fn_Cdt]),
                                                                            GetValue_DateTime(sqlR, sqlCtx.Indexes[_Schema.MBCode.fn_Udt]),
                                                                            GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.MBCode.fn_type]));
                    }
                }
                mbModel.GetType().GetField("_mbCode", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(mbModel, newFieldVal);

                return mbModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mbModel"></param>
        /// <returns></returns>
        public IMES.FisObject.Common.Model.Family FillFamilyObj(MBModel mbModel)
        {
            IMES.FisObject.Common.Model.Family family = FamilyRepository.Find(mbModel.Family);
            mbModel.GetType().GetField("_familyObj", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(mbModel, family);
            return family;
        }

        public IList<string> GetMBCodeListFromEcrVersion()
        {
            //SELECT DISTINCT MBCode FROM IMES_PCA..EcrVersion ORDER BY MBCode
            try
            {
                IList<string> ret = new List<string>();

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        sqlCtx = Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.EcrVersion), "DISTINCT", new List<string>() { _Schema.EcrVersion.fn_MBCode }, null, null, null, null, null, null, null, null);
                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(Func.OrderBy, _Schema.EcrVersion.fn_MBCode);
                    }
                }
                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            string str = GetValue_Str(sqlR, sqlCtx.Indexes[_Schema.EcrVersion.fn_MBCode]);
                            ret.Add(str);
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

        public bool IsEcrExistInEcrVersion(string ecr)
        {
            try
            {
                bool ret = false;

                SQLContext sqlCtx = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx))
                    {
                        // 2010-06-04 Liu Dong(eB1-4)         Modify ECR后2位匹配就算存在.
                        //_Schema.EcrVersion cond = new _Schema.EcrVersion();
                        //cond.ECR = ecr;
                        _Schema.EcrVersion likeCond = new _Schema.EcrVersion();
                        likeCond.ECR = "%" + ecr;
                        sqlCtx = _Schema.Func.GetConditionedFuncSelect(MethodBase.GetCurrentMethod().MetadataToken, typeof(_Schema.EcrVersion), "COUNT", new List<string>() { _Schema.EcrVersion.fn_ID }, null, likeCond, null, null, null, null, null, null);
                    }
                }
                sqlCtx.Params[_Schema.EcrVersion.fn_ECR].Value = "%" + ecr;
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        if (sqlR.Read())
                        {
                            int cnt = GetValue_Int32(sqlR, sqlCtx.Indexes["COUNT"]);
                            ret = cnt > 0 ? true : false;
                        }
                    }
                }
                return ret;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void AddSmtctInfo(SmtctInfo item)
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
                        sqlCtx = FuncNew.GetAquireIdInsert<Smtct>(tk);
                    }
                }
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();

                sqlCtx = FuncNew.SetColumnFromField<Smtct, SmtctInfo>(sqlCtx, item);

                sqlCtx.Param(Smtct.fn_cdt).Value = cmDt;
                sqlCtx.Param(Smtct.fn_udt).Value = cmDt;

                item.id = _Schema.SqlHelper.ExecuteScalarForAquireIdInsert(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteSmtctInfo(SmtctInfo condition)
        {
            try
            {
                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                Smtct cond = FuncNew.SetColumnFromField<Smtct, SmtctInfo>(condition);
                sqlCtx = FuncNew.GetConditionedDelete<Smtct>(new ConditionCollection<Smtct>(new EqualCondition<Smtct>(cond)));
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Smtct, SmtctInfo>(sqlCtx, condition);

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateSmtctInfo(SmtctInfo setValue, SmtctInfo condition)
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
                mtns::Smtct cond = mtns::FuncNew.SetColumnFromField<mtns::Smtct, SmtctInfo>(condition);
                mtns::Smtct setv = mtns::FuncNew.SetColumnFromField<mtns::Smtct, SmtctInfo>(setValue);
                setv.udt = DateTime.Now;

                sqlCtx = mtns::FuncNew.GetConditionedUpdate<mtns::Smtct>(new mtns::SetValueCollection<mtns::Smtct>(new mtns::CommonSetValue<mtns::Smtct>(setv)), new mtns::ConditionCollection<mtns::Smtct>(new mtns::EqualCondition<mtns::Smtct>(cond)));
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Smtct, SmtctInfo>(sqlCtx, condition);
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::Smtct, SmtctInfo>(sqlCtx, setValue, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::Smtct.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<SmtctInfo> GetSmtctInfoList(SmtctInfo condition)
        {
            try
            {
                IList<SmtctInfo> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                Smtct cond = FuncNew.SetColumnFromField<Smtct, SmtctInfo>(condition);
                sqlCtx = FuncNew.GetConditionedSelect<Smtct>(null, null, new ConditionCollection<Smtct>(new EqualCondition<Smtct>(cond)), Smtct.fn_family);
                //    }
                //}
                sqlCtx = FuncNew.SetColumnFromField<Smtct, SmtctInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_PCA, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<Smtct, SmtctInfo, SmtctInfo>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region . Defered .

        public void AddSmtctInfoDefered(IUnitOfWork uow, SmtctInfo item)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), item);
        }

        public void DeleteSmtctInfoDefered(IUnitOfWork uow, SmtctInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), condition);
        }

        public void UpdateSmtctInfoDefered(IUnitOfWork uow, SmtctInfo setValue, SmtctInfo condition)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), setValue, condition);
        }

        #endregion

        #endregion

        #region For Maintain

        public IList<string> GetMBFamilyList()
        {
            try
            {
                IList<string> ret = new List<string>();

                SQLContext sqlCtx = null;
                TableAndFields tf1 = null;
                TableAndFields tf2 = null;
                //TableAndFields tf3 = null;
                TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new TableAndFields();
                        tf1.Table = typeof(Part);
                        //Part invalCond1 = new Part();
                        //invalCond1.PartType = "INSET";
                        //tf1.inSetcond = invalCond1;
                        tf1.ToGetFieldNames.Add(Part.fn_Descr);

                        tf2 = new TableAndFields();
                        tf2.Table = typeof(PCB);
                        tf2.subDBCalalog = SqlHelper.DB_PCA;
                        tf2.ToGetFieldNames = null;

                        //tf3 = new TableAndFields();
                        //tf3.Table = typeof(PartInfo);
                        //PartInfo invalCond = new PartInfo();
                        //invalCond.InfoType = "INSET";
                        //tf3.inSetcond = invalCond;
                        //tf3.ToGetFieldNames = null;

                        List<TableConnectionItem> tblCnntIs = new List<TableConnectionItem>();
                        TableConnectionItem tc1 = new TableConnectionItem(tf1, Part.fn_PartNo, tf2, PCB.fn_PCBModelID);
                        tblCnntIs.Add(tc1);
                        //TableConnectionItem tc2 = new TableConnectionItem(tf1, Part.fn_PartNo, tf3, PartInfo.fn_PartNo);
                        //tblCnntIs.Add(tc2);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new TableAndFields[] { tf1, tf2 };//, tf3 };
                        sqlCtx = Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);

                        //sqlCtx.Sentence = sqlCtx.Sentence.Replace(Func.DecAlias(tf1.alias, Func.DecInSet(Part.fn_PartType)), Func.ConvertInSet(new List<string>(MB.MBType.GetAllTypes())));
                        //sqlCtx.Sentence = sqlCtx.Sentence.Replace(Func.DecAlias(tf3.alias, Func.DecInSet(PartInfo.fn_InfoType)), Func.ConvertInSet(new List<string>(MB.MBType.GetAllTypes())));

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(Func.OrderBy, Func.DecAliasInner(tf1.alias, Part.fn_Descr));
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];
                //tf3 = tblAndFldsesArray[2];

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            ret.Add(GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, Part.fn_Descr)]));
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

        public IList<string> GetMBModelList()
        {
            try
            {
                IList<string> ret = new List<string>();

                SQLContext sqlCtx = null;
                TableAndFields tf1 = null;
                TableAndFields tf2 = null;
                TableAndFields[] tblAndFldsesArray = null;
                lock (MethodBase.GetCurrentMethod())
                {
                    if (!Func.PeerTheSQL(MethodBase.GetCurrentMethod().MetadataToken, out sqlCtx, out tblAndFldsesArray))
                    {
                        tf1 = new TableAndFields();
                        tf1.Table = typeof(Part);
                        tf1.ToGetFieldNames.Add(Part.fn_PartNo);

                        tf2 = new TableAndFields();
                        tf2.Table = typeof(PCB);
                        tf2.subDBCalalog = SqlHelper.DB_PCA;
                        tf2.ToGetFieldNames = null;

                        List<TableConnectionItem> tblCnntIs = new List<TableConnectionItem>();
                        TableConnectionItem tc1 = new TableConnectionItem(tf1, Part.fn_PartNo, tf2, PCB.fn_PCBModelID);
                        tblCnntIs.Add(tc1);

                        _Schema.TableConnectionCollection tblCnnts = new _Schema.TableConnectionCollection(tblCnntIs.ToArray());

                        tblAndFldsesArray = new TableAndFields[] { tf1, tf2 };
                        sqlCtx = Func.GetConditionedJoinedSelect(MethodBase.GetCurrentMethod().MetadataToken, "DISTINCT", ref tblAndFldsesArray, tblCnnts);

                        sqlCtx.Sentence = sqlCtx.Sentence + string.Format(Func.OrderBy, Func.DecAliasInner(tf1.alias, Part.fn_PartNo));
                    }
                }
                tf1 = tblAndFldsesArray[0];
                tf2 = tblAndFldsesArray[1];

                using (SqlDataReader sqlR = SqlHelper.ExecuteReader(SqlHelper.ConnectionString_BOM, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params.Values.ToArray<SqlParameter>()))
                {
                    if (sqlR != null)
                    {
                        while (sqlR.Read())
                        {
                            ret.Add(GetValue_Str(sqlR, sqlCtx.Indexes[Func.DecAlias(tf1.alias, Part.fn_PartNo)]));
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

        public IList<IMES.FisObject.PCA.MBModel.MBCode> GetAllUniteMB()
        {
            try
            {
                IList<IMES.FisObject.PCA.MBModel.MBCode> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        sqlCtx = FuncNew.GetCommonSelect<mtns::Mbcode>(tk, mtns::Mbcode.fn_mbcode);
                    }
                }
                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Mbcode, IMES.FisObject.PCA.MBModel.MBCode, IMES.FisObject.PCA.MBModel.MBCode>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<IMES.FisObject.PCA.MBModel.MBCode> GetLstByMB(string mbCode)
        {
            try
            {
                IList<IMES.FisObject.PCA.MBModel.MBCode> ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        mtns::Mbcode cond = new mtns::Mbcode();
                        cond.mbcode = mbCode;
                        sqlCtx = FuncNew.GetConditionedSelect<mtns::Mbcode>(tk, null, null, new ConditionCollection<mtns::Mbcode>(new EqualCondition<mtns::Mbcode>(cond)), mtns::Mbcode.fn_cdt);
                    }
                }
                sqlCtx.Param(mtns::Mbcode.fn_mbcode).Value = mbCode;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    ret = FuncNew.SetFieldFromColumn<mtns::Mbcode, IMES.FisObject.PCA.MBModel.MBCode, IMES.FisObject.PCA.MBModel.MBCode>(ret, sqlR, sqlCtx);
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddUniteMB(IMES.FisObject.PCA.MBModel.MBCode obj)
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
                        sqlCtx = FuncNew.GetCommonInsert<mtns::Mbcode>(tk);
                    }
                }
                sqlCtx = FuncNew.SetColumnFromField<mtns::Mbcode, IMES.FisObject.PCA.MBModel.MBCode>(sqlCtx, obj);

                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(mtns::Mbcode.fn_cdt).Value = cmDt;
                sqlCtx.Param(mtns::Mbcode.fn_udt).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteUniteMB(string mbCode)
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
                        mtns::Mbcode cond = new mtns::Mbcode();
                        cond.mbcode = mbCode;
                        sqlCtx = FuncNew.GetConditionedDelete<mtns::Mbcode>(tk, new ConditionCollection<mtns::Mbcode>(new EqualCondition<mtns::Mbcode>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Mbcode.fn_mbcode).Value = mbCode;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateUniteMB(IMES.FisObject.PCA.MBModel.MBCode obj, string mbCode)
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
                        mtns::Mbcode cond = new mtns::Mbcode();
                        cond.mbcode = mbCode;
                        mtns::Mbcode setv = FuncNew.SetColumnFromField<mtns::Mbcode, IMES.FisObject.PCA.MBModel.MBCode>(obj);
                        setv.udt = DateTime.Now;

                        sqlCtx = FuncNew.GetConditionedUpdate<mtns::Mbcode>(tk, new SetValueCollection<mtns::Mbcode>(new CommonSetValue<mtns::Mbcode>(setv)), new ConditionCollection<mtns::Mbcode>(new EqualCondition<mtns::Mbcode>(cond)));
                    }
                }
                sqlCtx.Param(mtns::Mbcode.fn_mbcode).Value = mbCode;

                sqlCtx = FuncNew.SetColumnFromField<mtns::Mbcode, IMES.FisObject.PCA.MBModel.MBCode>(sqlCtx, obj, true);
                DateTime cmDt = _Schema.SqlHelper.GetDateTime();
                sqlCtx.Param(g.DecSV(mtns::Mbcode.fn_udt)).Value = cmDt;

                _Schema.SqlHelper.ExecuteNonQuery(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region . Defered .

        public void AddUniteMBDefered(IUnitOfWork uow, IMES.FisObject.PCA.MBModel.MBCode obj)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), obj);
        }

        public void DeleteUniteMBDefered(IUnitOfWork uow, string mbCode)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), mbCode);
        }

        public void UpdateUniteMBDefered(IUnitOfWork uow, IMES.FisObject.PCA.MBModel.MBCode obj, string mbCode)
        {
            AddOneInvokeBody(uow, MethodBase.GetCurrentMethod(), obj, mbCode);
        }

        #endregion

        #endregion
    }
}
