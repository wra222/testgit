using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using System.Data.SqlClient;
using System.Data;
using log4net;
using IMES.Query.DB;
using IMES.Infrastructure;
using System.Reflection;

namespace IMES.Query.Implementation
{
   public  class UploadFile :  MarshalByRefObject,IUploadFile
    {
       public DataTable GetFilePath(string DBConnection, string Name)
        {
            DataTable Result=null;
            StringBuilder sb = new StringBuilder(); 
            sb.AppendLine(" select Value from SysSetting (nolock) where Name='" + Name + "'");
            Result = SQLHelper.ExecuteDataFill(DBConnection, System.Data.CommandType.Text,
                                                 sb.ToString());
            return Result;
        }
    }
}
