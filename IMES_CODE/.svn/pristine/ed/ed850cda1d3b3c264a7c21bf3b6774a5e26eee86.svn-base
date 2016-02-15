using System;
using System.Collections.Generic;
using IMES.DataModel;
using System.Collections;


namespace IMES.Docking.Interface.DockingIntf
{
    public interface IProdIdPrintForDocking
    {
        IList<PrintItem> ProductIdPrint(
            string pdLine, string model, string mo, int qty, bool IsNextMonth, string editor, string station, string customer, string ecr,
            out IList<string> startProdIdAndEndProdId, IList<PrintItem> printItems);


        /// <summary>
        /// 获取Model列表
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<string> GetModelList(string family);
        

        IList<MOInfo> GetMOList(string model);

        MOInfo GetMOInfo(string MOId);

        /// <summary>
        /// 获取Family列表
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<IMES.DataModel.FamilyInfo> GetFamilyList(string customer);


        IList<string> GetProdIdHeader(string name);

        ArrayList ProdIdRePrint(string prodid, string reason, string editor, string station, string customer, string pCode, IList<PrintItem> printItems);
    }
}
