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
    public class KeyPartsDefectCollectionManager : MarshalByRefObject, IKeyPartsDefectCollection
    {
        IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
        

       
        //SELECT [Stage]
        //  FROM [IMES_GetData_Datamaintain].[dbo].[Stage]
        //order by [Stage]

        public IList<FailKPCollectionInfo> GetDefectPartList(DateTime date,string PdLine)
        {
            try
            {
                
                return iPartRepository.GetFailKPCollection(date, PdLine);
            }
            catch (Exception)
            {
                throw;
            }
        }



        //delete  	FailKPCollection                                                             
        //where ID=@ID
        public void Delete(int ID)
        {
            try
            {
                iPartRepository.DeleteFailKPCollection(ID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //IF EXISTS(
        //SELECT [Line]
        //        FROM [IMES_GetData_Datamaintain].[dbo].[Line]
        //where [Line]='line'
        //)
        //set @return='True'
        //ELSE
        //set @return='False'
        //Boolean IsExistLine(string line);

        public void AddLine(FailKPCollectionInfo item)
        {
            try
            {
                iPartRepository.AddFailKPCollection(item);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void UpdateLine(FailKPCollectionInfo item)
        {
            try
            {
                iPartRepository.UpdateFailKPCollection(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //select  ID, [Type], Value, Description, Editor, Cdt, Udt
        //from ConstValueType
        //where [Type]=@type
        //order by ID
        public IList<ConstValueTypeInfo> GetConstValueTypeList(string Type)
        {
            try
            {
                return iPartRepository.GetConstValueTypeList(Type);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<int> ExistInFailKPCollection(FailKPCollectionInfo item)
        {
            try
            {
                return iPartRepository.ExistInFailKPCollection(item);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
