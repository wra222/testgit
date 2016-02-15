using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.PAK.StandardWeight
{
    public class PalletType : FisObjectBase, IAggregateRoot
    {
        public PalletType()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        private int _ID;
        private string _ShipWay;
        private string _RegId;
        private string _Type;
        private string _StdPltFullQty;
        private int _MaxQty;
        private int _MinQty;
        private int _PalletLayer;
        private string _Code;
        private decimal _PltWeight;
        private string _MinusPltWeight="0";
        private string _CheckCode = "";
        private string _ChepPallet = "0";
        private string _OceanType;

        private string _Editor;
        private DateTime _Cdt= DateTime.MinValue;
        private DateTime _Udt=DateTime.MinValue;

        /// <summary>
        /// ID
        /// </summary>
        public int ID
        {
            get
            {
                return this._ID;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._ID = value;
            }
        }

        /// <summary>
        /// ShipWay
        /// </summary>
        public string ShipWay
        {
            get
            {
                return this._ShipWay;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._ShipWay = value;
            }
        }


        /// <summary>
        /// RegId
        /// </summary>
        public string RegId
        {
            get
            {
                return this._RegId;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._RegId = value;
            }
        }


        /// <summary>
        ///Type
        /// </summary>
        public string Type
        {
            get
            {
                return this._Type;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Type = value;
            }
        }

        /// <summary>
        ///_StdPltFullQty
        /// </summary>
        public string StdPltFullQty
        {
            get
            {
                return this._StdPltFullQty;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._StdPltFullQty = value;
            }
        }

        /// <summary>
        ///_MaxQty
        /// </summary>
        public int MaxQty
        {
            get
            {
                return this._MaxQty;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._MaxQty = value;
            }
        }

        /// <summary>
        ///_MinQty
        /// </summary>
         public int MinQty
        {
            get
            {
                return this._MinQty;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._MinQty = value;
            }
        }

         /// <summary>
         ///PalletLayer
         /// </summary>
         public int PalletLayer
         {
             get
             {
                 return this._PalletLayer;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._PalletLayer = value;
             }
         }

         /// <summary>
         ///Code
         /// </summary>
         public string Code
         {
             get
             {
                 return this._Code;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._Code = value;
             }
         }

         /// <summary>
         ///_PltWeight
         /// </summary>
         public decimal PltWeight
         {
             get
             {
                 return this._PltWeight;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._PltWeight = value;
             }
         }

         /// <summary>
         ///_MinusPltWeight
         /// </summary>
         public string MinusPltWeight
         {
             get
             {
                 return this._MinusPltWeight;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._MinusPltWeight = value;
             }
         }


         /// <summary>
         ///_CheckCode
         /// </summary>
         public string CheckCode
         {
             get
             {
                 return this._CheckCode;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._CheckCode = value;
             }
         }

         /// <summary>
         ///_ChepPallet
         /// </summary>
         public string ChepPallet
         {
             get
             {
                 return this._ChepPallet;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._ChepPallet = value;
             }
         }

         public string OceanType
         {
             get
             {
                 return this._OceanType;
             }
             set
             {
                 this._tracker.MarkAsModified(this);
                 this._OceanType = value;
             }
         }

        /// <summary>
        ///Editor
        /// </summary>
        public string Editor
        {
            get
            {
                return this._Editor;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Editor = value;
            }
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
            set
            {
                this._tracker.MarkAsModified(this);
                this._Cdt = value;
            }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Udt
        {
            get
            {
                return this._Udt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Udt = value;
            }
        }

        #endregion

        #region IFisObject Members
        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return this._ID; }
        }

        #endregion
    }
}
