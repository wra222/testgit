// INVENTEC corporation ©2011 all rights reserved. 
// Description:PAQC Output 
// UI:CI-MES12-SPEC-PAK-UC PAQC Output.docx –2011/10/20 
// UC:CI-MES12-SPEC-PAK-UC PAQC Output.docx –2011/10/20                          
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-10-20   Du.Xuan                      Create 
// Known issues:
// TODO：

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 机器抽检，检查抽中的机器是否有不良。如果有不良需要记录不良信息，然后送到维修站
    /// </summary>
    public interface IProductReinput
    {
        #region "methods interact with the running workflow"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodList"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        ArrayList CheckProdList(IList<string> prodList, string editor, string station, string customer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="passList"></param>
        /// <param name="failList"></param>
        /// <param name="reStation"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        ArrayList CheckProdPassList(IList<DataModel.ProductInfo> passList, IList<DataModel.ProductInfo> failList,
                                string reStation, string editor, string station, string customer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="passList"></param>
        /// <param name="reStation"></param>
        /// <param name="isPrint"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="line"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        ArrayList Save(IList<DataModel.ProductInfo> passList, string reStation, bool isPrint,
                            string editor, string station, string customer, string line, IList<PrintItem> printItems);
        #endregion

        #region "methods do not interact with the running workflow"

        #endregion
    }
}
