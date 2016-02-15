using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Query.Interface.QueryIntf
{
      public interface IFA_LocQuery
    {
          /// <summary>
        /// station =DownLoad||RunIn
        ///  @QueryType=Time||Line||SN/Model
          /// </summary>
          /// <param name="Connection"></param>
          /// <param name="Station"></param>
          /// <param name="QueryType"></param>
          /// <param name="FromDate"></param>
          /// <param name="ToDate"></param>
          /// <param name="Line"></param>
          /// <param name="List"></param>
         /// <param name="Model"></param>
          /// <returns></returns>
          DataTable GetLocRinDown(string Connection, string Station, string QueryType, string FromDate, string ToDate, string Line, string List,IList<string> Model);
    }
}
