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
    public class DeliveryCarton : FisObjectBase, IAggregateRoot
    {
         public DeliveryCarton()
        {
            this._tracker.MarkAsAdded(this);
        }
      

        #region . Essential Fields .
        private int _id = 0;
        private string _deliveryNo = "";
        private string _cartonSN="";       
        private string _model = "";        
        private int  _assignQty = 0;
        private int  _qty = 0;
        //private DeliveryCartonState _status = DeliveryCartonState.Reserve;      
        private string _editor = "";
        private DateTime _cdt=DateTime.MinValue;
        private DateTime _udt=DateTime.MinValue;
        

        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._id = value;
            }
        }

         public string DeliveryNo
        {
            get
            {
                return this._deliveryNo;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                if (Parent != null)
                {
                    Parent.Tracker.MarkAsModified(Parent.SubTrackerName);
                }
                this._deliveryNo = value.Trim();
            }
        }

        public string CartonSN
        {
            get
            {
                return this._cartonSN;
            }
            set
            {
                this._tracker.MarkAsModified(this);               
                this._cartonSN = value.Trim();
            }
        }

      

        public string Model
        {
            get
            {
                return this._model;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                if (Parent != null)
                {
                    Parent.Tracker.MarkAsModified(Parent.SubTrackerName);
                }
                this._model= value.Trim();
            }
        }

        public int Qty
        {
            get
            {
                return this._qty;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                if (Parent != null)
                {
                    Parent.Tracker.MarkAsModified(Parent.SubTrackerName);
                }
                this._qty = value;
            }
        }

        public int AssignQty
        {
            get
            {
                return this._assignQty;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                if (Parent != null)
                {
                    Parent.Tracker.MarkAsModified(Parent.SubTrackerName);
                }
                this._assignQty = value;
            }
        }




        //[TypeConverter(typeof(CastEnum<DeliveryCartonState>))]
        //public DeliveryCartonState Status
        //{
        //    get
        //    {
        //        return this._status;
        //    }
        //    set
        //    {
        //        this._tracker.MarkAsModified(this);
        //        this._status = value;
        //    }
        //}
  
       

        /// <summary>
        /// Editor
        /// </summary>
        public string Editor
        {
            get
            {
                return this._editor;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                if (Parent != null)
                {
                    Parent.Tracker.MarkAsModified(Parent.SubTrackerName);
                }
                this._editor = value.Trim();
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Cdt
        {
            get
            {
                return this._cdt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._cdt = value;
            }
        }

        public DateTime Udt
        {
            get
            {
                return this._udt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._udt = value;
            }
        }

        public Carton Parent
        {
            get;
            set;
        }
        #endregion

        #region IFisObject Members
        public override object Key
        {
            get { return _id; }
        }      

        #endregion
    }
    //public enum DeliveryCartonState
    //{
    //    Reserve=0,
    //    Assign,
    //    UnPack,
    //    Abort
    //}
}
