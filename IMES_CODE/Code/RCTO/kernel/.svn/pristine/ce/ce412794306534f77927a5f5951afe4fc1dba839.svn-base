using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;


namespace IMES.FisObject.Common.Line
{
    /// <summary>
    /// 可設置Alias Line
    /// </summary>
    public class LineEx : FisObjectBase, IAggregateRoot
    {
        public LineEx()
        {
            this._tracker.MarkAsAdded(this);
        }

        public LineEx(string line, string aliasLine,string editor)
        {
            this._line = line;
            this._aliasLine = aliasLine;
            this._editor = editor;            

            this._tracker.MarkAsAdded(this);
        }

        private string _line="";
        private string _aliasLine="";
        private int _avgSpeed=0;
        private int _avgManPower = 0;
        private int _avgStationQty = 0;
        private string _owner ="";
        private string _ieOwner="";
        private string _editor="";
        private DateTime _udt=DateTime.Now;

        /// <summary>
        /// Line Id
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
                //if (Parent != null)
                //{
                //    Parent.Tracker.MarkAsModified(Parent.SubTrackerName);
                //}
                _line = value;
            }
        }

        /// <summary>
        /// AliasLine
        /// </summary>
        public string AliasLine
        {
            get
            {
                return _aliasLine;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                if (Parent != null)
                {
                    Parent.Tracker.MarkAsModified(Parent.SubTrackerName);
                }
                _aliasLine = value;
            }
        }

        /// <summary>
        /// AvgSpeed
        /// </summary>
        public int AvgSpeed
        {
            get
            {
                return _avgSpeed;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                if (Parent != null)
                {
                    Parent.Tracker.MarkAsModified(Parent.SubTrackerName);
                }
                _avgSpeed = value;
            }
        }


        /// <summary>
        /// AvgManPower
        /// </summary>
        public int AvgManPower
        {
            get
            {
                return _avgManPower;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                if (Parent != null)
                {
                    Parent.Tracker.MarkAsModified(Parent.SubTrackerName);
                }
                _avgManPower = value;
            }
        }


        /// <summary>
        /// AvgStationQty
        /// </summary>
        public int AvgStationQty
        {
            get
            {
                return _avgStationQty;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                if (Parent != null)
                {
                    Parent.Tracker.MarkAsModified(Parent.SubTrackerName);
                }
                _avgStationQty = value;
            }
        }

        /// <summary>
        /// OWner
        /// </summary>
        public string Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                if (Parent != null)
                {
                    Parent.Tracker.MarkAsModified(Parent.SubTrackerName);
                }
                _owner = value;
            }
        }

        /// <summary>
        /// OWner
        /// </summary>
        public string IEOwner
        {
            get
            {
                return _ieOwner;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                if (Parent != null)
                {
                    Parent.Tracker.MarkAsModified(Parent.SubTrackerName);
                }
                _ieOwner = value;
            }
        }
        /// <summary>
        /// 峎誘埜
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
                if (Parent != null)
                {
                    Parent.Tracker.MarkAsModified(Parent.SubTrackerName);
                }
                _editor = value;
            }
        }

       

        /// <summary>
        /// 載陔奀潔
        /// </summary>
        public DateTime Udt
        {
            get
            {
                return _udt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                               
                _udt = value;
            }
        }
        /// <summary>
        /// Parent object
        /// </summary>
        public Line Parent
        {
            get;
            set;
        }

        #region Overrides of FisObjectBase

        /// <summary>
        /// 勤砓梓尨key, 婓肮濬倰FisObject毓峓囀峔珨
        /// </summary>
        public override object Key
        {
            get { return _line + "," + _aliasLine; }
        }

        #endregion
    }
}
