using System.Collections.Generic;
using System.Collections;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 
    /// </summary>
    public interface IChangeAST
    {
        ArrayList CheckProduct(string prodid, string line, string editor, string station, string customer);

        ArrayList Change(string line, string editor, string station, string customer,
                                    string prod1, string prod2, string model1, string model2,
                                    string ASTType, string PartNo, string PartSn, IList<PrintItem> printItemLst);

        void returnException5();

        void returnException2(string id);
    }
}
