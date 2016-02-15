using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(CombinedAstNumber))]
    [Serializable]
    public class CombinedAstNumberInfo
    {

        [ORMapping(CombinedAstNumber.fn_id)]
        public long ID = long.MinValue;

        [ORMapping(CombinedAstNumber.fn_code)]
        public string Code = null;

        [ORMapping(CombinedAstNumber.fn_astType)]
        public string AstType = null;


        [ORMapping(CombinedAstNumber.fn_astNo)]
        public string AstNo = null;

        [ORMapping(CombinedAstNumber.fn_productID)]
        public string ProductID = null;

        [ORMapping(CombinedAstNumber.fn_station)]
        public string Station = null;

        [ORMapping(CombinedAstNumber.fn_unBindProductID)]
        public string UnBindProductID = null;

        [ORMapping(CombinedAstNumber.fn_unBindStation)]
        public string UnBindStation = null;

        [ORMapping(CombinedAstNumber.fn_state)]
        public string State = null;

        [ORMapping(CombinedAstNumber.fn_remark)]
        public string Remark = null;

        [ORMapping(CombinedAstNumber.fn_editor)]
        public string Editor = null;

        [ORMapping(CombinedAstNumber.fn_cdt)]
        public DateTime Cdt = DateTime.MinValue;

        [ORMapping(CombinedAstNumber.fn_udt)]
        public DateTime Udt = DateTime.MinValue;


    }



}
