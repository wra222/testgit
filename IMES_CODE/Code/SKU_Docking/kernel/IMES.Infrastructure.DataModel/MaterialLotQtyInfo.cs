using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
   
    [Serializable]
    public class MaterialLotQtyInfo
    {
        public String LotNo {get; set;}
        public String Status { get; set; }
        public int Qty { get; set; }
       
    }
}
