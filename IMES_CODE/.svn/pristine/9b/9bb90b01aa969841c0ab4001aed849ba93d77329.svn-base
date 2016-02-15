using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.ComponentModel;
using IMES.Infrastructure.Util;

namespace IMES.FisObject.PAK.Carton
{
    public class CartonProduct : FisObjectBase, IAggregateRoot
    {
        public CartonProduct()
        {
            //this._tracker.MarkAsAdded(this);
        }


        #region . Essential Fields .
        private int _ID = 0;
        private string _DeliveryNo = "";
        private string _CartonSN = "";
        private string _ProductID= "";
        //private  string _Status = CartonStatusEnum.Empty.ToString();
        private string _Remark ="";
         private string _Editor = "";
        private DateTime _Cdt = DateTime.MinValue;
        private DateTime _Udt = DateTime.MinValue;


        public int ID
        {
            get
            {
                return this._ID;
            }
            //set
            //{
            //    this._tracker.MarkAsModified(this);
            //    this._id = value;
            //}
        }

        public string DeliveryNo
        {
            get
            {
                return this._DeliveryNo;
            }
            //set
            //{
            //    this._tracker.MarkAsModified(this);
            //    this._deliveryNo = value.Trim();
            //}
        }

        public string CartonSN
        {
            get
            {
                return this._CartonSN;
            }
            //set
            //{
            //    this._tracker.MarkAsModified(this);
            //    this._cartonSN = value.Trim();
            //}
        }



        public string ProductID
        {
            get
            {
                return this._ProductID;
            }
            //set
            //{
            //    this._tracker.MarkAsModified(this);
            //    this._productId = value.Trim();
            //}
        }

        //[TypeConverter(typeof(CastEnum<CartonStatusEnum>))]
        //public CartonStatusEnum Status
        //{
        //    get
        //    {
        //        return (CartonStatusEnum) Enum.Parse(typeof(CartonStatusEnum), _Status);
        //    }
        //    //set
        //    //{
        //    //    this._tracker.MarkAsModified(this);
        //    //    this._status = value;
        //    //}
        //}

        public string Remark
        {
            get
            {
                return this._Remark;
            }
            //set
            //{
            //    this._tracker.MarkAsModified(this);
            //    this._remark = value.Trim();
            //}
        }

       
        /// <summary>
        /// Editor
        /// </summary>
        public string Editor
        {
            get
            {
                return this._Editor;
            }
            //set
            //{
            //    this._tracker.MarkAsModified(this);
            //    this._editor = value.Trim();
            //}
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Cdt
        {
            get
            {
                return this._Cdt;
            }
            //set
            //{
            //    this._tracker.MarkAsModified(this);
            //    this._cdt = value;
            //}
        }

        public DateTime Udt
        {
            get
            {
                return this._Udt;
            }
            //set
            //{
            //    this._tracker.MarkAsModified(this);
            //    this._udt = value;
            //}
        }
        #endregion

        #region IFisObject Members
        public override object Key
        {
            get { return _ID; }
        }

        #endregion
    }
    
}

