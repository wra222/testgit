using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.FisObject.PAK.BSam
{
    public class BSamModel : FisObjectBase//, IAggregateRoot
    {
        public BSamModel()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
       
        private string _A_Part_Model = "";
        private string _C_Part_Model = "";
        private string _HP_A_Part = "";
        private string _HP_C_SKU = "";
        private int _QtyPerCarton = 0;
       
        private string _Editor = "";
        private DateTime _Cdt = DateTime.MinValue;
        private DateTime _Udt = DateTime.MinValue;

        
        public string A_Part_Model 
        {
            get
            {
                return this._A_Part_Model;
            }
            //set
            //{
            //    this._tracker.MarkAsModified(this);
            //    this._a_Part_model = value;
            //}
        }

        public string C_Part_Model
        {
            get
            {
                return this._C_Part_Model;
            }
            //set
            //{
            //    this._tracker.MarkAsModified(this);
            //    this._c_Part_model = value;
            //}
        }

        public string HP_A_Part
        {
            get
            {
                return this._HP_A_Part;
            }
            //set
            //{
            //    this._tracker.MarkAsModified(this);
            //    this._hp_a_Part = value;
            //}
        }

        public string HP_C_SKU
        {
            get
            {
                return this._HP_C_SKU;
            }
            //set
            //{
            //    this._tracker.MarkAsModified(this);
            //    this._hp_c_Part_Sku = value;
            //}
        }


        public int QtyPerCaton
        {
            get
            {
                return this._QtyPerCarton;
            }
            //set
            //{
            //    this._tracker.MarkAsModified(this);
            //    this._qtyPerCarton = value;
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
            //    this._editor = value;
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
            get { return _A_Part_Model; }
        }

        #endregion

    }
}
