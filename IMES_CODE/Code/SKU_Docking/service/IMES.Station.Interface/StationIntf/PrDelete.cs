using System.Collections.Generic;

namespace IMES.Station.Interface.StationIntf
{
    public interface IPrDelete
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prsn"></param>
        /// <returns></returns>
        IList<string> FindInfoForPrSN(string prsn);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputSN"></param>
        /// <param name="productID"></param>
        /// <param name="productSN"></param>
        /// <param name="checkSN"></param>
        /// <param name="partNo"></param>
        /// <param name="model"></param>
        /// <param name="station"></param>
        /// <param name="editor"></param>
        /// <param name="pdline"></param>
        /// <param name="customer"></param>
        void DelPrSN(string inputSN, string productID, string productSN, string checkSN, string partNo, string model,
                        string station, string editor, string pdline, string customer);
    }
}
