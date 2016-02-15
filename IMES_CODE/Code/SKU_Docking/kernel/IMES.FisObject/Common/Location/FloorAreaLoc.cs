using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.Common.Location
{
    [ORMapping(typeof(mtns.FloorAreaLoc))]
    public class FloorAreaLoc:FisObjectBase, IAggregateRoot
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FloorAreaLoc()
        {
            //this._tracker.MarkAsAdded(this);
        }

        [ORMapping(mtns.FloorAreaLoc.fn_locID)]
        public string LocID = null;

        [ORMapping(mtns.FloorAreaLoc.fn_floor)]
        public string Floor = null;

        [ORMapping(mtns.FloorAreaLoc.fn_area)]
        public string Area = null;

        [ORMapping(mtns.FloorAreaLoc.fn_loc)]
        public string Loc = null;

        [ORMapping(mtns.FloorAreaLoc.fn_unit)]
        public string Unit = null;

        [ORMapping(mtns.FloorAreaLoc.fn_category)]
        public string Category = null;

        [ORMapping(mtns.FloorAreaLoc.fn_model)]
        public string Model = null;

        [ORMapping(mtns.FloorAreaLoc.fn_qty)]
        public Int32 Qty = int.MinValue;

        [ORMapping(mtns.FloorAreaLoc.fn_remainQty)]
        public Int32 RemainQty = int.MinValue;

        [ORMapping(mtns.FloorAreaLoc.fn_fullLocQty)]
        public Int32 FullLocQty = int.MinValue;

        [ORMapping(mtns.FloorAreaLoc.fn_status)]
        public string Status = null;

        [ORMapping(mtns.FloorAreaLoc.fn_holdInput)]
        public string HoldInput = null;

        [ORMapping(mtns.FloorAreaLoc.fn_holdOutput)]
        public string HoldOutput = null;

        [ORMapping(mtns.FloorAreaLoc.fn_remark)]
        public string Remark = null;

        [ORMapping(mtns.FloorAreaLoc.fn_editor)]
        public string Editor = null;

        [ORMapping(mtns.FloorAreaLoc.fn_cdt)]
        public DateTime Cdt = DateTime.MinValue;

        [ORMapping(mtns.FloorAreaLoc.fn_udt)]
        public DateTime Udt = DateTime.MinValue;

        #region IFisObject Members
        public override object Key
        {
            get { return LocID; }
        }

        #endregion
    }
}
