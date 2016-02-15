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

namespace IMES.Maintain.Implementation
{
    public class DeptManager : MarshalByRefObject,IDept
    {

        //select distinct(Dept) from Dept 

        public IList<string> GetDeptList()
        {
            IList<string> result = new List<string>();

            try
            {
                IMBRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                result = itemRepository.GetDeptList();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }



        //select distinct(Section) from Dept where Dept=[Dept]
        //union 
        //select 'SMT1A'
        //union
        //select 'SMT1B'
        public IList<string> GetSectionList(DeptInfo condition)
        {
            IList<string> result = new List<string>();

            try
            {
                IMBRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                result = itemRepository.GetSectionList(condition);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        //select * from Dept 
        //where Dept = [Dept]
        //and Section like '[Section]%'
        //order by Dept, Section, Line, FISLine
        public IList<DeptInfo> GetLineList(DeptInfo eqCondition, DeptInfo likeCondition)
        {
            IList<DeptInfo> result = new List<DeptInfo>();

            try
            {
                IMBRepository iMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                /*
                DeptInfo deptCondition = new DeptInfo();
                deptCondition.dept = eqCondition.dept;
                DeptInfo deptlikeCondition = new DeptInfo();
                deptlikeCondition.section = likeCondition.section + "%";*/
                result = iMBRepository.GetSectionList(eqCondition, likeCondition);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        private static String Null2String(Object _input)
        {
            if (_input == null)
            {
                return "";
            }
            return _input.ToString().Trim();
        }

        //DELETE FROM [Dept]
        public void DeleteDeptInfo(DeptInfo condition)
        {
            try
            {
                IMBRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                itemRepository.DeleteDeptInfo(condition);
            }
            catch (Exception)
            {
                throw;
            }
            return;
        }

       public string AddDeptInfo(DeptInfo item)
        {
            String result = "";
          
            try
            {
                IMBRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                itemRepository.AddDeptInfo(item);
                result = item.id.ToString();
   }
            catch (Exception)
            {
                throw;
            }

            return result;
        }


        public void UpdateDeptInfo(DeptInfo setValue, DeptInfo condition)
        {
            try
            {
                IMBRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                itemRepository.UpdateDeptInfo(setValue, condition);

            }
            catch (Exception)
            {
                throw;
            }

            return;
        }

       public IList<DeptInfo> GetDeptInfoList(DeptInfo condition)
        {
            IList<DeptInfo> result = new List<DeptInfo>();

            try
            {
                IMBRepository iMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                /*
                DeptInfo deptCondition = new DeptInfo();
                deptCondition.dept = eqCondition.dept;
                DeptInfo deptlikeCondition = new DeptInfo();
                deptlikeCondition.section = likeCondition.section + "%";*/
                result = iMBRepository.GetDeptInfoList(condition);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }
}
