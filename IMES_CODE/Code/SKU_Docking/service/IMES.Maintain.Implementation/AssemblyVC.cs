using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.PCA.MBModel;
using IMES.FisObject.Common.Line;
using IMES.FisObject.PCA.MBMO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.Misc;
using IMES.FisObject.Common.MO;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Warranty;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.NumControl;
using System.Data;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;

namespace IMES.Maintain.Implementation
{

    public class AssemblyVC : MarshalByRefObject, IAssemblyVC
    {
        IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
        IFamilyRepository iFamilyRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();

        public IList<AssemblyVCInfo> GetAssemblyVCbyCondition(AssemblyVCInfo condition)
        {
            IList<AssemblyVCInfo> retList = new List<AssemblyVCInfo>();
            try
            {
                retList = iPartRepository.GetAssemblyVC(condition);
                
            }
            catch (Exception)
            {
                throw;
            }
            return retList;
        }

        public IList<string> GetFamilyList()
        {
            IList<Family> list = new List<Family>();
            IList<string> ret = new List<string>();
            try
            {
                list = iFamilyRepository.FindAll();
                foreach (Family item in list)
                {
                    ret.Add(item.FamilyName);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ret;
        }

        public List<string> GetPartNoList(string VC)
        {
            try
            {
                string strSQL = @"select PartNo, Descr 
                                from Part 
                                where PartNo in (
                                select PartNo from PartInfo 
                                where InfoType='VendorCode' and InfoValue=@VC)";
                SqlParameter paraNameType = new SqlParameter("@VC", SqlDbType.VarChar, 20);
                paraNameType.Direction = ParameterDirection.Input;
                paraNameType.Value = VC;
                DataTable dt = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData, CommandType.Text, strSQL, paraNameType);
                List<string> list = new List<string>(dt.Rows.Count);
                foreach (DataRow dr in dt.Rows)
                {
                    string PartNo = dr[0].ToString().Trim();
                    string Descr = dr[1].ToString().Trim();
                    string item = PartNo + " - " + Descr;
                    list.Add(item);
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertAssemblyVC(AssemblyVCInfo item)
        {
            try
            {
                iPartRepository.InsertAssemblyVC(item);
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public void UpdateAssemblyVC(AssemblyVCInfo item)
        {
            try
            {
                iPartRepository.UpdateAssemblyVC(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteAssemblyVC(string id)
        {
            try
            {
                long ID = Convert.ToInt64(id);
                iPartRepository.DeleteAssemblyVC(ID);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
