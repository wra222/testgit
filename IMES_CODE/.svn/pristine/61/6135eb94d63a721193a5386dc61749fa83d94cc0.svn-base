
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Data;
using IMES.Infrastructure;

using IMES.FisObject.Common.Misc;
using IMES.FisObject.Common.Part;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;

namespace IMES.Maintain.Implementation
{
    public class EnergyLabel : MarshalByRefObject, IEnergyLabel
    {

        IMiscRepository iMiscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
        #region Implementation of EnergyLabel

        /// <summary>
        /// GetEnergyLabel_Family
        /// </summary>
        /// <returns></returns>
        public IList<string> GetEnergyLabel_Family()
        {
            try
            {
                IList<string> retList = new List<string>();
                EnergyLabelInfo condition = new EnergyLabelInfo();
                IList<EnergyLabelInfo> tempEngList = new List<EnergyLabelInfo>();
                tempEngList = iMiscRepository.GetEnergyLabel(condition);
                retList = (from q in tempEngList
                           select q.family).Distinct().OrderBy(q => q).ToList<string>();
                return retList;
            }
            catch (Exception)
            {
                throw;
            } 
        }

        /// <summary>
        /// GetFamily
        /// </summary>
        /// <returns></returns>
        public IList<string> GetFamily()
        {
            try
            {
                string strSQL = @"select distinct Family from Family order by Family";
                SqlParameter paraNameType = new SqlParameter("@Type", SqlDbType.VarChar, 20);
                paraNameType.Direction = ParameterDirection.Input;
                paraNameType.Value = "";
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameType);
                List<string> list = new List<string>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                {
                    string item = dr[0].ToString().Trim();
                    list.Add(item);
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// GetChinaLevel
        /// </summary>
        /// <returns></returns>
        public IList<string> GetChinaLevel()
        {
            try
            {
                string strSQL = @"select distinct ChinaLevel from AVMaintain order by ChinaLevel";
                SqlParameter paraNameType = new SqlParameter("@Type", SqlDbType.VarChar, 20);
                paraNameType.Direction = ParameterDirection.Input;
                paraNameType.Value = "";
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameType);
                List<string> list = new List<string>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                {
                    string item = dr[0].ToString().Trim();
                    if (item != "")
                    {
                        list.Add(item);
                    }
                    
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// GetEnergyLabelByCondition
        /// </summary>
        /// <param name="condition">EnergyLabelInfo</param>
        /// <returns></returns>
        public IList<EnergyLabelInfo> GetEnergyLabelByCondition(EnergyLabelInfo condition)
        {
            try
            {
                IList<EnergyLabelInfo> retLst = new List<EnergyLabelInfo>();
                retLst = iMiscRepository.GetEnergyLabel(condition);
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }                       
        }

        /// <summary>
        /// addeditEnergyLabel
        /// </summary>
        /// <param name="condition">EnergyLabelInfo</param>
        public void addeditEnergyLabel(EnergyLabelInfo condition, string userName)
        {
            try
            {
                IList<EnergyLabelInfo> checkdata = new List<EnergyLabelInfo>();
                EnergyLabelInfo checkCondition = new EnergyLabelInfo();
                checkCondition.family = condition.family;
                checkCondition.chinaLevel = condition.chinaLevel;
                checkdata = iMiscRepository.GetEnergyLabel(checkCondition);//family, china label
                condition.udt = DateTime.Now;
                condition.cdt = DateTime.Now;
                condition.editor = userName;
                if (checkdata.Count == 0)
                {
                    iMiscRepository.InsertEnergyLabel(condition);
                }
                else
                {
                    condition.id = checkdata[0].id;
                    iMiscRepository.UpdateEnergyLabel(condition);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// DeleteEnergyLabel
        /// </summary>
        /// <param name="id">int</param>
        public void DeleteEnergyLabel(int id)
        {
            try
            {
                iMiscRepository.DeleteEnergyLabel(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}