using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using System.Data;
using System.Reflection;

namespace IMES.Query.Interface.QueryIntf
{
   public interface IUploadFile
    {
       DataTable GetFilePath(string DBConnection, string Name);
        
    }
}
