using System.Text;
using IMES.DataModel;
using System.Data;
using System.Collections.Generic;
using System;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IFaKittingUpload
    {
        DataTable GetListForFaFromLine(String line, String family);

        IList<SelectInfoDef> GetAllFamilyList();
    }
}
