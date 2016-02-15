using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Station.Implementation
{
    public class DeliveryPalletCompare:IEqualityComparer<AssignedDeliveryPalletInfo>
    {
         #region IEqualityComparer<AssignedDeliveryPalletInfo> Members

        bool IEqualityComparer<AssignedDeliveryPalletInfo> .Equals(AssignedDeliveryPalletInfo x, AssignedDeliveryPalletInfo y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            if (object.ReferenceEquals(x, y))
                return true;
            if (x.DeliveryNo == y.DeliveryNo &&
                x.PalletNo == y.PalletNo &&
                x.CartonQty == y.CartonQty) //&&
                //x.DeviceQty == y.DeviceQty)
                return true;

            return false;
        }

        int IEqualityComparer<AssignedDeliveryPalletInfo>.GetHashCode(AssignedDeliveryPalletInfo obj)
        {
            return (obj.DeliveryNo.GetHashCode() ^ obj.CartonQty.GetHashCode() ^ obj.PalletNo.GetHashCode());
        }
        #endregion
    }
}
