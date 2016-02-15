using System;
using System.Linq;
using IMES.Infrastructure.FisObjectBase;
using IMES.DataModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IMES.FisObject.Common.Line
{
    /// <summary>
    /// 用于表示Line的业务实体
    /// </summary>
    public class Line : FisObjectBase, IAggregateRoot
    {
        public Line()
        {
            this._tracker.MarkAsAdded(this);
        }

        public Line(LineInfo item)
        {
            this._cdt = item.cdt;
            this._customerid = item.customerID;
            this._descr = item.descr;
            this._editor = item.editor;
            this._id = item.line;
            this._stageid = item.stage;
            this._udt = item.udt;

            this._tracker.MarkAsAdded(this);
        }

        private const string _subTrackerName = "LineRelation";
        public string SubTrackerName
        {
            get
            {
                return _subTrackerName;
            }
        }
        private static ILineRepository _lineRepository;
        private static ILineRepository LineRepository
        {
            get
            {
                if (_lineRepository == null)
                    _lineRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();
                return _lineRepository;
            }
        }

        private string _id;
        private string _customerid;
        private string _stageid;
        private string _descr;
        private string _editor;
        private DateTime _cdt;
        private DateTime _udt;

        /// <summary>
        /// Line Id
        /// </summary>
        public string Id 
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
        /// CustomerId
        /// </summary>
        public string CustomerId
        {
            get
            {
                return _customerid;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _customerid = value;
            }
        }

        /// <summary>
        /// StageId
        /// </summary>
        public string StageId
        {
            get
            {
                return _stageid;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _stageid = value;
            }
        }

        /// <summary>
        /// PdLine描述
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
        /// 维护人员
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
        /// 创建时间
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

        /// <summary>
        /// 更新时间
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


        public System.Data.DataRowState GetRelationTableState()
        {
            return this._tracker.GetState(_subTrackerName);
        }

        #region external object
        private object _syncLineExObj = new object();
        private LineEx _lineEx = null;
        public LineEx LineEx
        {
            get
            {
                lock (_syncLineExObj)
                {
                    if (_lineEx== null)
                    {
                        LineRepository.FillLineEx(this);
                        if (_lineEx != null)
                        {
                            _lineEx.Parent = this;
                        }
                        LineRepository.FillLineSpeed(this);
                    }
                    return _lineEx;
                }
            }

            set
            {
                lock (_syncLineExObj)
                {
                    if (value == null)
                        return;

                    if (string.IsNullOrEmpty(value.AliasLine))
                        return;
                    
                    if (_lineEx == null)
                    {
                        object naught = this.LineEx;
                    }

                    if (_lineEx == null)
                    {
                        value.Line = _id;
                        _lineEx = value;
                        _lineEx.Parent = this;
                    }
                    else
                    {
                        _lineEx.Line = _id;
                        _lineEx.AliasLine = value.AliasLine;
                        _lineEx.AvgManPower = value.AvgManPower;
                        _lineEx.AvgSpeed = value.AvgSpeed;
                        _lineEx.AvgStationQty = value.AvgStationQty;
                        _lineEx.Editor = value.Editor;
                        _lineEx.Owner = value.Owner;
                        _lineEx.IEOwner = value.IEOwner;
                        
                    }
                    _tracker.MarkAsModified(_subTrackerName);
                }

            }
        }

        //public void SetLineEx(LineEx lineEx)
        //{
        //    if (lineEx==null)
        //        return;

        //    if (string.IsNullOrEmpty(lineEx.AliasLine))
        //        return;

        //    lock (_syncLineExObj)
        //    {
        //        lineEx.Line = this.Id;
        //        object naught = this.LineEx;

        //        IList<LineEx> arr = (from p in _lineEx where p.AliasLine == lineEx.AliasLine select p).ToList();

        //        if (arr.Count > 0)
        //        {
        //            foreach (LineEx item in arr)
        //            {
        //                item.AvgManPower = lineEx.AvgManPower;
        //                item.AvgSpeed = lineEx.AvgSpeed;
        //                item.AvgStationQty = lineEx.AvgStationQty;
        //                item.Owner = lineEx.Owner;
        //                item.IEOwner = lineEx.IEOwner;
        //                item.Editor = lineEx.Editor;
        //            }
        //        }
        //        else
        //        {
        //            _lineEx.Add(lineEx);
        //        }
        //        _tracker.MarkAsModified(_subTrackerName);
        //    }
        //}

        

        private object _syncLineSpeedObj = new object();
        private IList<LineSpeed> _lineSpeed = null;
        public IList<LineSpeed> LineSpeed
        {
            get
            {
                lock (_syncLineSpeedObj)
                {
                    if (_lineSpeed == null)
                    {
                        LineRepository.FillLineSpeed(this);
                        if (_lineSpeed != null)
                        {
                            foreach (LineSpeed item in _lineSpeed)
                            {
                                item.Parent = this;
                            }
                        }
                    }
                    if (_lineSpeed != null)
                        return new ReadOnlyCollection<LineSpeed>(_lineSpeed);
                    else
                        return null;
                }
            }
        }

        public void SetLineSpeed(LineSpeed lineSpeed)
        {
            if (lineSpeed == null)
                return;

            if (string.IsNullOrEmpty(lineSpeed.Station))
                return;

            lock (_syncLineSpeedObj)
            {
                if (_lineEx == null)
                {
                    object naught1 = this.LineEx;
                }

                if (_lineSpeed == null)
                {
                    object naught = this.LineSpeed;
                }
                
                if (_lineEx == null)
                    return;

                lineSpeed.AliasLine = _lineEx.AliasLine;

                IList<LineSpeed> arr = (from p in _lineSpeed
                                                        where p.Station == lineSpeed.Station  
                                                        select p).ToList();

                if (arr.Count > 0)
                {
                    foreach (LineSpeed item in arr)
                    {   
                        item.IsHoldStation = lineSpeed.IsHoldStation;
                        item.LimitSpeed = lineSpeed.LimitSpeed;
                        item.LimitSpeedExpression = lineSpeed.LimitSpeedExpression;
                        item.IsCheckPass = lineSpeed.IsCheckPass;
                        item.Editor = lineSpeed.Editor;
                    }
                }
                else
                {
                    lineSpeed.Parent = this;
                    _lineSpeed.Add(lineSpeed);
                }

                _tracker.MarkAsModified(_subTrackerName);

            }
        }

        #endregion
        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return _id; }
        }

        #endregion
    }
}