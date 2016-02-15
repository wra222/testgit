using System;
using System.Collections.Generic;
using IMES.DataModel;
using System.Collections;


namespace IMES.Docking.Interface.DockingIntf
{
    public interface IShipToLabelPrintForDocking
    {
        ArrayList Print(string pdline, string editor, string station, string customer, string id, IList<PrintItem> printItems);

        IList<string> GetValueByName(string name);

        ArrayList RePrint(string pdline, string editor, string station, string customer, string id, IList<PrintItem> printItems);
    }
}
