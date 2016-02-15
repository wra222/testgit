using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.Common.ModelBOM
{
    /// <summary>
    /// BOMNodeData表对应的实体
    /// </summary>
    public class BOMNodeData : FisObjectBase, IAggregateRoot
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BOMNodeData()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .

        private int _id;
        private string _material = null;
        private string _plant = null;
      
        private string _component = null;
      
        private string _quantity = null;
      
        private string _alternative_item_group = null;
        private string _priority = null;
        private int _flag;
       
        private string _editor = null;
        private DateTime _cdt;
        private DateTime _udt;

        /// <summary>
        /// ID
        /// </summary>
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

        /// <summary>
        /// Material
        /// </summary>
        public string Material
        {
            get
            {
                return this._material;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._material = value;
            }
        }

        /// <summary>
        /// Plant
        /// </summary>
        public string Plant
        {
            get
            {
                return this._plant;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._plant = value;
            }
        }

      

        /// <summary>
        /// Component
        /// </summary>
        public string Component
        {
            get
            {
                return this._component;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._component = value;
            }
        }

      
        /// <summary>
        /// Quantity
        /// </summary>
        public string Quantity
        {
            get
            {
                return this._quantity;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._quantity = value;
            }
        }

       

        /// <summary>
        /// Alternative_item_group
        /// </summary>
        public string Alternative_item_group
        {
            get
            {
                return this._alternative_item_group;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._alternative_item_group = value;
            }
        }

        /// <summary>
        /// Priority
        /// </summary>
        public string Priority
        {
            get
            {
                return this._priority;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._priority = value;
            }
        }


        /// <summary>
        /// Flag
        /// </summary>
        public int Flag
        {
            get
            {
                return this._flag;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._flag = value;
            }
        }
      

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
                this._editor = value;
            }
        }

        /// <summary>
        /// Cdt
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

        /// <summary>
        /// Udt
        /// </summary>
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

        #endregion

        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return this._id; }
        }

        #endregion
    }
}
