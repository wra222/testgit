using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(AstDefine))]
    [Serializable]
    public class AstDefineInfo
    {
        [ORMapping(AstDefine.fn_astType)]
        public string AstType = null;

        [ORMapping(AstDefine.fn_astCode)]
        public string AstCode = null;

        [ORMapping(AstDefine.fn_astLocation)]
        public string AstLocation = null;

        [ORMapping(AstDefine.fn_needAssignAstSN)]
        public string NeedAssignAstSN = null;

        [ORMapping(AstDefine.fn_assignAstSNStation)]
        public string AssignAstSNStation = null;

        [ORMapping(AstDefine.fn_combineStation)]
        public string CombineStation = null;

        [ORMapping(AstDefine.fn_hasCDSIAst)]
        public string HasCDSIAst = null;

        [ORMapping(AstDefine.fn_needPrint)]
        public string NeedPrint = null;

        [ORMapping(AstDefine.fn_needScanSN)]       
        public string NeedScanSN = null;

        [ORMapping(AstDefine.fn_checkUnique)]    
        public string CheckUnique = null;

        [ORMapping(AstDefine.fn_comment)]
        public string comment = null;

        [ORMapping(AstDefine.fn_hasUPSAst)]
        public string HasUPSAst = null;

        //[ORMapping(AstDefine.fn_upsassignstation)]
        //public string UPSAssignStation = null;

        //[ORMapping(AstDefine.fn_upscombinestation)]
        //public string UPSCombineStation = null;

        [ORMapping(AstDefine.fn_needBindUPSPO)]
        public string NeedBindUPSPO = null;

        [ORMapping(AstDefine.fn_editor)]
        public String Editor = null;

        [ORMapping(AstDefine.fn_cdt)]
        public DateTime Cdt = DateTime.MinValue;

        [ORMapping(AstDefine.fn_udt)]
        public DateTime Udt = DateTime.MinValue;      
    }
}
