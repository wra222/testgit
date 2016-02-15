using System;
using System.Linq;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure;


namespace IMES.FisObject.PAK.Pizza
{
    /// <summary>
    /// Pizza log
    /// </summary>   
    public class PizzaLog : FisObjectBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PizzaLog()
        {
            this._tracker.MarkAsAdded(this);
        }
        
        private int _id = int.MinValue;
        private string _pizzaID = null;
        private string _model = null;      
        private string _station = null;        
        private string _line = null;
        private string _descr = null;
        private string _editor = null;       
        private DateTime _cdt = DateTime.MinValue;
       

        /// <summary>
        /// Pizza ID
        /// </summary>
        public string PizzaID
        {
            get
            {
                return _pizzaID;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _pizzaID = value;
            }
        }

        /// <summary>
        /// Log腔ID
        /// </summary>
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _id = value;
            }
        }

        /// <summary>
        /// Pizza儂倰
        /// </summary>
        public string Model
        {
            get
            {
                return _model;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _model = value;
            }
        }

        /// <summary>
        /// Pizza冪徹腔Station
        /// </summary>
        public string Station
        {
            get
            {
                return _station;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _station = value;
            }
        }       

        /// <summary>
        /// Pizza垀婓腔Line
        /// </summary>
        public string Line
        {
            get
            {
                return _line;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _line = value;
            }
        }


        /// <summary>
        /// Pizza垀婓腔Descr
        /// </summary>
        public string Descr
        {
            get
            {
                return _descr;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _descr = value;
            }
        }

        /// <summary>
        /// 婓蜆桴揭燴Product腔埜馱晤瘍
        /// </summary>
        public string Editor
        {
            get
            {
                return _editor;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _editor = value;
            }
        }

        /// <summary>
        /// Pizza俇傖議桴腔奀潔
        /// </summary>
        public DateTime Cdt
        {
            get
            {
                return _cdt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _cdt = value;
            }
        }

        #region Overrides of FisObjectBase

        /// <summary>
        /// 勤砓梓尨key, 婓肮濬倰FisObject毓峓囀峔珨
        /// </summary>
        public override object Key
        {
            get { return this._id; }
        }

        #endregion
    }
}
