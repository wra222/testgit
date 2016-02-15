using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;
using IMES.FisObject.Common.Part.PartPolicy;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;
using mtns = IMES.Infrastructure.Repository._Metas;
using IMES.DataModel;
using dmns = IMES.DataModel;

namespace IMES.Infrastructure.Repository.Common
{
    public class PartPolicyRepository : IPartPolicyRepository
    {
        private static GetValueClass g = new GetValueClass();

        #region IPartPolicyRepository Members

        public IPartPolicy GetPolicy(string partCheckType)
        {
            try
            {
                IPartPolicy ret = null;

                MethodBase mthObj = MethodBase.GetCurrentMethod();
                int tk = mthObj.MetadataToken;
                SQLContextNew sqlCtx = null;
                lock (mthObj)
                {
                    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                    {
                        CheckItemType cond = new CheckItemType();
                        cond.name = partCheckType;
                        sqlCtx = FuncNew.GetConditionedSelect<CheckItemType>(tk, "TOP 1", null, new ConditionCollection<CheckItemType>(new EqualCondition<CheckItemType>(cond)), CheckItemType.fn_cdt + FuncNew.DescendOrder);
                    }
                }
                sqlCtx.Param(CheckItemType.fn_name).Value = partCheckType;

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null && sqlR.Read())
                    {
                        ret = new PartPolicy(
                            g.GetValue_Str(sqlR, sqlCtx.Indexes(CheckItemType.fn_name)),
                            g.GetValue_Str(sqlR, sqlCtx.Indexes(CheckItemType.fn_filterModule)),
                            g.GetValue_Str(sqlR, sqlCtx.Indexes(CheckItemType.fn_matchModule)),
                            g.GetValue_Str(sqlR, sqlCtx.Indexes(CheckItemType.fn_checkModule)),
                            g.GetValue_Str(sqlR, sqlCtx.Indexes(CheckItemType.fn_saveModule)),
                            g.GetValue_Bit(sqlR, sqlCtx.Indexes(CheckItemType.fn_needUniqueCheck)),
                            g.GetValue_Bit(sqlR, sqlCtx.Indexes(CheckItemType.fn_needCommonSave)),
                            g.GetValue_Bit(sqlR, sqlCtx.Indexes(CheckItemType.fn_needPartForbidCheck)),
                            g.GetValue_Bit(sqlR, sqlCtx.Indexes(CheckItemType.fn_needDefectComponentCheck)),
                            g.GetValue_Bit(sqlR, sqlCtx.Indexes(CheckItemType.fn_repairPartType))
                            );
                    }
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> GetNameFromCheckItemType(dmns.CheckItemTypeInfo condition)
        {
            try
            {
                IList<string> ret = null;

                //MethodBase mthObj = MethodBase.GetCurrentMethod();
                //int tk = mthObj.MetadataToken;
                mtns::SQLContextNew sqlCtx = null;
                //lock (mthObj)
                //{
                //    if (!SQLCache.PeerTheSQL(tk, out sqlCtx))
                //    {
                mtns::CheckItemType cond = mtns::FuncNew.SetColumnFromField<mtns::CheckItemType, dmns.CheckItemTypeInfo>(condition);
                sqlCtx = mtns::FuncNew.GetConditionedSelect<mtns::CheckItemType>(null, new string[] { mtns.CheckItemType.fn_name }, new mtns::ConditionCollection<mtns::CheckItemType>(new mtns::EqualCondition<mtns::CheckItemType>(cond)), mtns::CheckItemType.fn_name);
                //    }
                //}
                sqlCtx = mtns::FuncNew.SetColumnFromField<mtns::CheckItemType, dmns.CheckItemTypeInfo>(sqlCtx, condition);

                using (SqlDataReader sqlR = _Schema.SqlHelper.ExecuteReader(_Schema.SqlHelper.ConnectionString_GetData, CommandType.Text, sqlCtx.Sentence, sqlCtx.Params))
                {
                    if (sqlR != null)
                    {
                        ret = new List<string>();
                        while (sqlR.Read())
                        {
                            string item = g.GetValue_Str(sqlR, sqlCtx.Indexes(mtns.CheckItemType.fn_name));
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

        #endregion
    }
}
