
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
    public class IndornesiaLabel : MarshalByRefObject, IIndornesiaLabel
    {

        IMiscRepository iMiscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
        #region Implementation of IndornesiaLabel

        /// <summary>
        /// GetIndornesiaLabel_Family
        /// </summary>
        /// <returns></returns>
        public IList<string> GetIndornesiaLabel_Family()
        {
            try
            {
                IList<string> retList = new List<string>();
                IndonesiaLabelInfo condition = new IndonesiaLabelInfo();
                IList<IndonesiaLabelInfo> tempList = new List<IndonesiaLabelInfo>();
                tempList = iMiscRepository.GetIndonesiaLabel(condition);
                retList = (from q in tempList
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
        /// GetIndonesiaLabelByCondition
        /// </summary>
        /// <param name="condition">IndonesiaLabelInfo</param>
        /// <returns></returns>
        public IList<IndonesiaLabelInfo> GetIndonesiaLabelByCondition(IndonesiaLabelInfo condition)
        {
            try
            {
                IList<IndonesiaLabelInfo> retLst = new List<IndonesiaLabelInfo>();
                retLst = iMiscRepository.GetIndonesiaLabel(condition);
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }                       
        }

        /// <summary>
        /// addeditIndonesiaLabel
        /// </summary>
        /// <param name="condition">IndonesiaLabelInfo</param>
        /// <param name="userName">string</param>
        public void addeditIndonesiaLabel(IndonesiaLabelInfo condition,string userName)
        {
            try
            {
                IList<IndonesiaLabelInfo> checkdata = new List<IndonesiaLabelInfo>();
                IndonesiaLabelInfo checkCondition = new IndonesiaLabelInfo();
                checkCondition.sku = condition.sku;
                checkdata = iMiscRepository.GetIndonesiaLabel(checkCondition);
                condition.udt = DateTime.Now;
                condition.cdt = DateTime.Now;
                condition.editor = userName;
                if (checkdata.Count == 0)
                {
                    iMiscRepository.InsertIndonesiaLabel(condition);
                }
                else
                {
                    condition.id = checkdata[0].id;
                    iMiscRepository.UpdateIndonesiaLabel(condition);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// DeleteIndonesiaLabel
        /// </summary>
        /// <param name="id">int</param>
        public void DeleteIndonesiaLabel(int id)
        {
            try
            {
                iMiscRepository.DeleteIndonesiaLabel(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}