using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class AvailableDelivery
    {
        public string DeliveryNo
        {
            get;
            set;
        }
        public string Model
        {
            get;
            set;
        }
        /// <summary>
        /// Delivery 總台數
        /// </summary>
        public int Qty
        {
            get;
            set;
        }
        /// <summary>
        /// 此機型滿箱台數
        /// </summary>
        public int QtyPerCarton
        {
            get;
            set;
        }
        /// <summary>
        /// Carton 需裝台數
        /// </summary>
        public int CartonQty
        {
            get;
            set;
        }
        /// <summary>
        /// 此Delivery未裝台數 
        /// </summary>
        public int RemainQty
        {
            get;
            set;
        }
        /// <summary>
        /// Delivery 總箱數
        /// </summary>
        public int DNCartonQty
        {
            get;
            set;
        }

        /// <summary>
        /// Delivery 台數
        /// </summary>
        public int DNQty
        {
            get;
            set;
        }
        /// <summary>
        /// Pallet  總箱數
        /// </summary>
        public int TotalCartonQty
        {
            get;
            set;
        }       
        /// <summary>
        /// Consolidate id,沒有Consolidate Id以DeliveryNo前10碼
        /// </summary>
        public string ShipmentNo
        {
            get;
            set;
        }

        public string PalletNo
        {
            get;
            set;
        }

        public DateTime ShipDate
        {
            get;
            set;
        }

        /// <summary>
        /// 1:散裝
        /// 2:棧板
        /// </summary>
        public string PackCategory
        {
            get;
            set;
        }        

    }
}
