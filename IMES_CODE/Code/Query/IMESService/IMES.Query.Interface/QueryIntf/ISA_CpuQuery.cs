using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Query.Interface.QueryIntf
{
    public interface ISA_CpuQuery    
    {       
       
        
        DataTable GetCPU(string Connection, string ID);
        //DataTable GetStation();
        //DataTable GetMultiProductInfo(IList<string> CustSNList);
        //void UpdateProdStatus(IList<string> CustSNList, string Station, int Status, string Editor);
    }
    
}
