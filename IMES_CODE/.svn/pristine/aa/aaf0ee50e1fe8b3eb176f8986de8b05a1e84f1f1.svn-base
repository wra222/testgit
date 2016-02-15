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
    public interface IPrintContentWarranty
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
        ArrayList InputCustomerSN(string customerSN, string line, string editor, string station, string customer);
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        IList<PrintItem> WarrantyPrint(string productID, Boolean conPrintFlag, IList<PrintItem> printItems);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        IList<PrintItem> ConfigurationPrint(string productID, IList<PrintItem> printItems);

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="productID"></param>
        void Cancel(string productID);
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSN"></param>
        /// <param name="reason"></param>
        /// <param name="printItems"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        ArrayList ReprintConfigurationLabel(string customerSN, string reason, IList<PrintItem> printItems, string line, string editor, string station, string customer);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSN"></param>
        /// <param name="reason"></param>
        /// <param name="printItems"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        ArrayList ReprintWarrantyLabel(string customerSN, string reason, IList<PrintItem> printItems, string line, string editor, string station, string customer);
    }
}
                