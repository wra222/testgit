// created by itc205033

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.Model;
using IMES.DataModel;
//using IMES.Station.Interface.CommonIntf;

namespace IMES.Maintain.Interface.MaintainIntf
{
 
    public interface IFamilyManagerEx
    {

        IList<FamilyInfo> FindFamiliesByCustomerOrderByFamily(string customer);

        IList<string> GetFamilyByCustomer(string customer);

        IList<IMES.DataModel.ModelInfo> GetModelByFamily(string family);

        void DeleteFamily(string family, string customer, string editor);
    }
  
}
