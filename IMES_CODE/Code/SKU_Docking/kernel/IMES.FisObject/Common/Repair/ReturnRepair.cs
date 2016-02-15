using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.FisObject.Common.Repair
{
    /// <summary>
    /// 成退ReturnRepair实体
    /// </summary>
    public class ReturnRepair
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ReturnRepair()
        {

        }

        private int _id;
        private int _pcbRepairID;
        private int _productRepairID;
        private int _productRepairDefectID;
        private DateTime _cdt;

        /// <summary>
        /// Id
        /// </summary>
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// PCBRepairID
        /// </summary>
        public int PCBRepairID
        {
            get { return _pcbRepairID; }
            set { _pcbRepairID = value; }
        }

        /// <summary>
        /// ProductRepairID
        /// </summary>
        public int ProductRepairID
        {
            get { return _productRepairID; }
            set { _productRepairID = value; }
        }

        /// <summary>
        /// ProductRepairDefectID
        /// </summary>
        public int ProductRepairDefectID
        {
            get { return _productRepairDefectID; }
            set { _productRepairDefectID = value; }
        }

        /// <summary>
        /// Cdt
        /// </summary>
        public DateTime Cdt
        {
            get { return _cdt; }
            set { _cdt = value; }
        }

        private int setter_PCBRepairID
        {
            set { _pcbRepairID = value; }
        }
        private int setter_ProductRepairID
        {
            set { _productRepairID = value; }
        }
        private int setter_ProductRepairDefectID
        {
            set { _productRepairDefectID = value; }
        }
    }
}
