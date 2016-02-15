using System.Collections.Generic;
using System.Collections;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 
    /// </summary>
    public class CheckItems
    {
        /// <summary>
        /// 
        /// </summary>
        public string item { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IITCNDCheck
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="printItems"></param>
        /// <param name="pdline"></param>
        /// <param name="prodid"></param>
        /// <param name="station"></param>
        /// <param name="editor"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        ArrayList CheckImageDL(IList<PrintItem> printItems, string pdline, string prodid, string station, string editor, string customer);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pdline"></param>
        /// <param name="station"></param>
        /// <param name="editor"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<string> GetPdLinePass(string pdline, string station, string editor, string customer);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodid"></param>
        /// <param name="reason"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="pCode"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        ArrayList Reprint(string prodid, string reason, string line, string editor, string station, string customer, string pCode, IList<PrintItem> printItems);

    }
}
