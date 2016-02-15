// INVENTEC corporation (c)2013 all rights reserved. 
// Description: Carton
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2013-02-01   Vincent              create
// Known issues:


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.ObjectModel;
using System.ComponentModel;
using IMES.Infrastructure.Util;

namespace IMES.FisObject.PAK.Carton
{
    public class Carton : FisObjectBase, IAggregateRoot
    {
        public Carton()
        {
            this._tracker.MarkAsAdded(this);
        }

        public Carton(string cartonSN,string station, int stationStatus, string line, string editor )
        {

            this._tracker.MarkAsAdded(this);
            _cartonSN = cartonSN;
            _editor = editor;
            CartonStatus currentStation = new CartonStatus
            {
                CartonNo = cartonSN,
                Station = station,
                Status = stationStatus,
                Line = line,
                Editor = editor
            };

            this.CurrentStation = currentStation;                                          
            
        }

        private const string _subTrackerName = "CartonRelation";
        public string SubTrackerName
        {
            get
            {
                return _subTrackerName;
            }
        }
        private static ICartonRepository _cartonRepository;
        private static ICartonRepository CartonRepository
        {
            get
            {
                if (_cartonRepository == null)
                    _cartonRepository = RepositoryFactory.GetInstance().GetRepository<ICartonRepository, Carton>();
                return _cartonRepository;
            }
        }


        #region . Essential Fields .
        private string _cartonSN="";
        private string _palletNo = "";
        private string _model = "";
        private string _boxId = "";
        private string _paqcStatus = "";
        private decimal _weight = 0;
        private int  _dnQty = 0;
        private int  _qty = 0;
        private int  _fullQty = 0;
        private CartonStatusEnum _status = CartonStatusEnum.Empty;
        private string _preStation = "";
        private int _preStationStatus = 1;
        private string _unPackPalletNo = "";
        private string _editor = "";
        private DateTime _cdt=DateTime.MinValue;
        private DateTime _udt=DateTime.MinValue;

      

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

        public string PalletNo
        {
            get
            {
                return this._palletNo;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._palletNo = value.Trim();
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
                this._model= value;
            }
        }

        public string BoxId
        {
            get
            {
                return this._boxId;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._boxId = value.Trim();
            }
        }

        public string PAQCStatus
        {
            get
            {
                return this._paqcStatus;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._paqcStatus = value.Trim();
            }
        }

        public decimal Weight
        {
            get
            {
                return this._weight;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._weight = value;
            }
        }

        public int DNQty
        {
            get
            {
                return this._dnQty;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._dnQty = value;
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
                this._qty = value;
            }
        }

        public int FullQty
        {
            get
            {
                return this._fullQty;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._fullQty = value;
            }
        }

       
        public CartonStatusEnum Status
        {
            get
            {
                return this._status;
            }
            set 
            {
                this._tracker.MarkAsModified(this);
                this._status = value;              
            }
        }

        public string PreStation
        {
            get
            {
                return this._preStation;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._preStation = value.Trim();
            }
        }

        public int PreStationStatus
        {
            get
            {
                return this._preStationStatus;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._preStationStatus = value;
            }
        }

        public string UnPackPalletNo
        {
            get
            {
                return this._unPackPalletNo;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._unPackPalletNo = value.Trim();
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

        public System.Data.DataRowState GetRelationTableState()
        {
            return this._tracker.GetState(_subTrackerName);
        }
        #endregion

        #region IFisObject Members
        public override object Key
        {
            get { return _cartonSN; }
        }      

        #endregion

        #region external object
        private object _syncStatusObj = new object();
        private CartonStatus _cartonStatus;
        public CartonStatus CurrentStation
        {
            get
            {
                lock (_syncStatusObj)
                {
                    if (_cartonStatus == null)
                    {
                       CartonRepository.FillCurrentStation(this);
                       if (_cartonStatus != null)
                        {
                            _cartonStatus.Parent = this;

                        }
                    }
                    return _cartonStatus;
                }
            }
            set
            {
                lock (_syncStatusObj)
                {
                    if (value == null) return;
                    object naught = this.CurrentStation;
                   
                    if (_cartonStatus == null)
                    {
                        _cartonStatus = value;
                        _cartonStatus.CartonNo = this._cartonSN;
                        _cartonStatus.Parent = this;
                    }
                    else
                    {
                        _cartonStatus.Editor = value.Editor;
                        _cartonStatus.Line = value.Line;
                        _cartonStatus.Station = value.Station;
                        _cartonStatus.Status = value.Status;                        
                    }
                    this._tracker.MarkAsModified(_subTrackerName);               
                }
            }
        }

        private object _syncDeliveryCartonObj = new object();
        private IList<DeliveryCarton>  _deliveryCartonList;
        public IList<DeliveryCarton> DeliveryCartons
        {
            get
            {
                lock (_syncDeliveryCartonObj)
                {
                    if (_deliveryCartonList == null)
                    {
                        CartonRepository.FillDeliveryCarton(this);
                        if (_deliveryCartonList != null)
                        {
                            foreach (DeliveryCarton item in _deliveryCartonList)
                            {
                                item.Parent = this;
                            }
                        }
                    }

                    if (_deliveryCartonList != null)
                        return new ReadOnlyCollection<DeliveryCarton>(_deliveryCartonList);
                    else
                        return null;                   
                    
                }
            }            
        }

        //public void AddDelivery(DeliveryCarton deliveryCarton)
        //{
        //    object naught = this.DeliveryCartons;
        //    lock (_syncDeliveryCartonObj)
        //    {
        //        _deliveryCartonList = _deliveryCartonList ?? new List<DeliveryCarton>();
        //        _deliveryCartonList.Add(deliveryCarton);
        //    }
        //}

        //public DeliveryCarton  GetDeliveryCartonByDeliveryNo(string deliveryNo)
        //{
            
        //        lock (_syncDeliveryCartonObj)
        //        {
        //            object naught = DeliveryCartons;

        //            if (_deliveryCartonList != null)
        //            {
        //                DeliveryCarton[] arr = (from p in _deliveryCartonList
        //                                                    where p.DeliveryNo == deliveryNo &&
        //                                                               p.CartonSN == this.CartonSN
        //                                                     select p).ToArray();
        //                if (arr.Length > 0)
        //                {
        //                    return arr[0];
        //                }
        //                else
        //                {
        //                    return null;
        //                }
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }            
        //}

        public void SetDeliveryInCarton(DeliveryCarton deliveryCarton)
        {
            if (string.IsNullOrEmpty(deliveryCarton.DeliveryNo))
                return;

            if (string.IsNullOrEmpty(deliveryCarton.CartonSN))
                return;

            if (string.IsNullOrEmpty(deliveryCarton.Model))
                return;

                   

            lock (_syncCartonInfoObj)
            {
                object naught = DeliveryCartons;

                DeliveryCarton[] arr = (from p in _deliveryCartonList 
                                                     where p.DeliveryNo ==  deliveryCarton.DeliveryNo &&
                                                                p.CartonSN == deliveryCarton.CartonSN
                                                     select p).ToArray();

                if (arr.Length > 0)
                {
                    foreach (DeliveryCarton item in arr)
                    {
                        item.Model = deliveryCarton.Model;
                        item.AssignQty = deliveryCarton.AssignQty;
                        item.Qty = deliveryCarton.Qty;
                        //item.Status = deliveryCarton.Status;
                        item.Editor = deliveryCarton.Editor;
                    }
                }
                else
                {
                    //DeliveryCarton item = new DeliveryCarton
                    //{
                    //    CartonSN = this._cartonSN,
                    //    DeliveryNo = deliveryCarton.DeliveryNo,
                    //    Model = deliveryCarton.Model,
                    //     AssignQty = deliveryCarton.AssignQty,
                    //    Qty =  deliveryCarton.Qty,
                    //    //Status = deliveryCarton.Status,
                    //    Editor = deliveryCarton.Editor,
                    //    Parent=this
                    //};
                    deliveryCarton.CartonSN = this._cartonSN;
                    deliveryCarton.Parent = this;
                    //item.Tracker = _tracker.Merge(item.Tracker);
                    _deliveryCartonList.Add(deliveryCarton);
                }

                _tracker.MarkAsModified(_subTrackerName);
            }
        }

        private object _syncCartonProductObj = new object();
        private IList<CartonProduct> _cartonProductList;
        public IList<CartonProduct> CartonProducts
        {
            get
            {
                lock (_syncCartonProductObj)
                {
                    CartonRepository.FillCartonProduct(this);
                    return _cartonProductList;
                }
            }
        }


        private object _syncCartonLogObj = new object();
        private IList<CartonLog> _cartonLogList;
        public IList<CartonLog> CartonLogs
        {
            get
            {
                lock (_syncCartonLogObj)
                {
                    if (_cartonLogList == null)
                    {
                        CartonRepository.FillCartonLog(this);
                    }
                    if (_cartonLogList != null)
                        return new ReadOnlyCollection<CartonLog>(_cartonLogList);
                    else
                        return null;
                }
            }           
        }

        public void AddCartonLog(CartonLog cartonLog)
        {
          
            lock (_syncCartonLogObj)
            {
                object naught = this.CartonLogs;
                _cartonLogList.Add(cartonLog);
                _tracker.MarkAsModified(_subTrackerName);
            }
        }

        private object _syncCartonInfoObj = new object();
        private IList<CartonInfo> _cartonInfoList;
        public IList<CartonInfo> CartonInfos
        {
            get
            {
                lock (_syncCartonInfoObj)
                {
                    if (_cartonInfoList == null)
                    {
                        CartonRepository.FillCartonInfo(this);
                        if (_cartonInfoList != null)
                        {
                            foreach (CartonInfo item in _cartonInfoList)
                            {
                                item.Parent = this;
                            }
                        }
                    }
                    if (_cartonInfoList != null)
                        return new ReadOnlyCollection<CartonInfo>(_cartonInfoList);
                    else
                        return null;
                   
                }
            }

        }
        //void AddCartonInfo(CartonInfo cartonInfo)
        //{
        //    lock (_syncCartonInfoObj)
        //    {
        //        _cartonInfoList.Add(cartonInfo);
        //        _tracker.MarkAsModified(this);
        //    }
        //}
        public void SetExtendedProperty(string name, string value, string editor)
        {
          
            if (string.IsNullOrEmpty(name))
                return;

            lock (_syncCartonInfoObj)
            {
                object naught = CartonInfos;

                CartonInfo[] arr = (from p in _cartonInfoList where p.InfoType == name select p).ToArray();

                if (arr.Length > 0)
                {
                    foreach(CartonInfo item in arr)
                    {
                        item.InfoValue = value;
                        item.Editor = editor;
                    }
                }
                else
                {
                    CartonInfo item = new CartonInfo
                    {
                        CartonNo = _cartonSN,
                        InfoType = name,
                        InfoValue = value,
                        Editor = editor,
                        Parent=this
                    };

                    //item.Tracker = _tracker.Merge(item.Tracker);
                    _cartonInfoList.Add(item);                   
                }

                _tracker.MarkAsModified(_subTrackerName);
            }
        }

        private object _syncCartonQCLogObj = new object();
        private IList<CartonQCLog> _cartonQCLogList;
        public IList<CartonQCLog> CartonQCLogs
        {
            get
            {
                lock (_syncCartonQCLogObj)
                {
                    if (_cartonQCLogList == null)
                    {
                        CartonRepository.FillCartonQCLog(this);
                    }
                    if (_cartonQCLogList != null)
                        return new ReadOnlyCollection<CartonQCLog>(_cartonQCLogList);
                    else
                        return null;
                }
            }            
        }

        public void AddCartonQCLog(CartonQCLog cartonQCLog)
        {
           
            lock (_syncCartonQCLogObj)
            {
                object naught = this.CartonQCLogs;
                _cartonQCLogList.Add(cartonQCLog);
                _tracker.MarkAsModified(_subTrackerName);
            }
        }

        #endregion

        #region public method
        public int AssignQty()
        {
            int ret=0;
            object naught = this.DeliveryCartons;
            foreach (DeliveryCarton item in _deliveryCartonList)
            {
                ret = ret + item.AssignQty;
            }
            return ret;
        }
        #endregion


    }

    //[TypeConverter(typeof(CastEnum<CartonStatusEnum>))]
    public enum CartonStatusEnum
    {
        Empty = 100,
        Reserve,
        Full,
        Partial,
        UnPack,
        Abort
    }
    
   
}
