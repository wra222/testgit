using System.Text;
using IMES.DataModel;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IFAIModelRule
    {
        DataTable Query(string model);

        void Delete(string idFamilyInfo, string editor);

        void Save(string idFamilyInfo, string family, string modelType, string moLimitQty, string editor);

		DataTable GetFamilyFromFamilyInfo();
		
		DataTable GetFamily(string custid);
		
		IList<string> GetModelType();
    }
}
