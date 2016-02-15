using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITPDLCheckForRCTO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSN"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        ArrayList InputProductID(string ID, string line, string editor, string station, string customer);

        ArrayList Save(string prodID, string TpdlCT);

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="productID"></param>
        void Cancel(string productID);
      
    }
}
                