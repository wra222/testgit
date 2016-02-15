using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.DataModel
{
    [ORMapping(typeof(mtns::Part_NEW))]
    [Serializable]
    public class PartDef
    {
        [ORMapping(mtns::Part_NEW.fn_autoDL)]
        public String autoDL = null;
        [ORMapping(mtns::Part_NEW.fn_bomNodeType)]
        public String bomNodeType = null;
        [ORMapping(mtns::Part_NEW.fn_cdt)]
        public DateTime cdt = DateTime.MinValue;
        [ORMapping(mtns::Part_NEW.fn_custPartNo)]
        public String custPartNo = null;
        [ORMapping(mtns::Part_NEW.fn_descr)]
        public String descr = null;
        [ORMapping(mtns::Part_NEW.fn_editor)]
        public String editor = null;
        [ORMapping(mtns::Part_NEW.fn_flag)]
        public Int32 flag = int.MinValue;
        [ORMapping(mtns::Part_NEW.fn_partNo)]
        public String partNo = null;
        [ORMapping(mtns::Part_NEW.fn_partType)]
        public String partType = null;
        [ORMapping(mtns::Part_NEW.fn_remark)]
        public String remark = null;
        [ORMapping(mtns::Part_NEW.fn_udt)]
        public DateTime udt = DateTime.MinValue;
    }

    [Serializable]
    public class PartTypeAndPartInfoValue
    {

        /// <summary>
        /// Id
        /// </summary>
        public int Id;
        /// <summary>
        /// PartNo
        /// </summary>
        public string MainTableKey;

        /// <summary>
        /// Name
        /// </summary>
        public string Item;

        /// <summary>
        /// Value
        /// </summary>
        public string Content;

        /// <summary>
        /// Description
        /// </summary>
        public string Description;

        /// <summary>
        /// Editor
        /// </summary>
        public string Editor;

        /// <summary>
        /// Cdt
        /// </summary>
        public DateTime Cdt;

        /// <summary>
        /// Udt
        /// </summary>
        public DateTime Udt;
    }
}
