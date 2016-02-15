using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;
using System.Collections.ObjectModel;
using IMES.FisObject.Common.Process;

namespace IMES.FisObject.Common.Material
{
    [ORMapping(typeof(mtns.MaterialBox))]
    public class MaterialBox : FisObjectBase,IAggregateRoot
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MaterialBox()
        {
            this._tracker.MarkAsAdded(this);
        }

        private const string _subTrackerName = "MaterialRelation";
        public string SubTrackerName
        {
            get
            {
                return _subTrackerName;
            }
        }

        private static IMaterialBoxRepository _materialBoxRepository;
        private static IMaterialBoxRepository MaterialBoxRepository
        {
            get
            {
                if (_materialBoxRepository == null)
                    _materialBoxRepository = RepositoryFactory.GetInstance().GetRepository<IMaterialBoxRepository>();
                return _materialBoxRepository;
            }
        }

        private static IProcessRepository _procRepository;
        private static IProcessRepository ProcessRepository
        {
            get
            {
                if (_procRepository == null)
                    _procRepository = RepositoryFactory.GetInstance().GetRepository<IProcessRepository>();
                return _procRepository;
            }
        }

        #region . Essential Fields .
        [ORMapping(mtns.MaterialBox.fn_boxId)]
        private string _BoxId=null;
        [ORMapping(mtns.MaterialBox.fn_lotNo)]
        private string _LotNo = null;
        [ORMapping(mtns.MaterialBox.fn_materialType)]
        private string _MaterialType = null;
        [ORMapping(mtns.MaterialBox.fn_specNo)]
        private string _SpecNo = null;
        [ORMapping(mtns.MaterialBox.fn_feedType)]
        private string _FeedType = null;
        [ORMapping(mtns.MaterialBox.fn_revision)]
        private string _Revision = null;
        [ORMapping(mtns.MaterialBox.fn_dateCode)]
        private string _DateCode = null;
        [ORMapping(mtns.MaterialBox.fn_supplier)]
        private string _Supplier = null;
        [ORMapping(mtns.MaterialBox.fn_partNo)]
        private string _PartNo = null;
        [ORMapping(mtns.MaterialBox.fn_qty)]
        private int _Qty = int.MinValue;
        [ORMapping(mtns.MaterialBox.fn_status)]
        private string _Status =null;
        [ORMapping(mtns.MaterialBox.fn_comment)]
        private string _Comment = null;

        [ORMapping(mtns.MaterialBox.fn_editor)]
        private string _Editor = null;
        [ORMapping(mtns.MaterialBox.fn_cdt)]
        private DateTime _Cdt = DateTime.MinValue;
        [ORMapping(mtns.MaterialBox.fn_udt)]
        private DateTime _Udt = DateTime.MinValue;

        //Add field for RCT 147 
        [ORMapping(mtns.MaterialBox.fn_model)]
        private string _model = null;
        [ORMapping(mtns.Material.fn_line)]
        private string _line = null;
        [ORMapping(mtns.MaterialBox.fn_cartonSN)]
        private string _cartonSN = null;
        [ORMapping(mtns.MaterialBox.fn_deliveryNo)]
        private string _deliveryNo = null;
        [ORMapping(mtns.MaterialBox.fn_palletNo)]
        private string _palletNo = null;
        //[ORMapping(mtns.MaterialBox.fn_pizzaID)]
        //private string _pizzaID = null;
        [ORMapping(mtns.MaterialBox.fn_qcStatus)]
        private string _qcStatus = null;
        [ORMapping(mtns.MaterialBox.fn_boxWeight)]
        private decimal _boxWeight = default(decimal);
        
        [ORMapping(mtns.MaterialBox.fn_shipMode)]
        private string _shipMode = null;

        public string BoxId
        {
            get
            {
                return this._BoxId;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._BoxId = value;
            }
        }      
        public string LotNo
        {
            get
            {
                return this._LotNo;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._LotNo = value;
            }
        }      
        public string MaterialType
        {
            get
            {
                return this._MaterialType;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._MaterialType = value;
            }
        }        
        public string SpecNo
        {
            get
            {
                return this._SpecNo;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._SpecNo = value;
            }
        }
        public string FeedType
        {
            get
            {
                return this._FeedType;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._FeedType = value;
            }
        }

        public string Revision
        {
            get
            {
                return this._Revision;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Revision = value;
            }
        }
        public string DateCode
        {
            get
            {
                return this._DateCode;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._DateCode = value;
            }
        }
        public string Supplier
        {
            get
            {
                return this._Supplier;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Supplier = value;
            }
        }
        public string PartNo
        {
            get
            {
                return this._PartNo;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._PartNo = value;
            }
        }
        
        public int Qty
        {
            get
            {
                return this._Qty;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Qty = value;
            }
        }  
        public string Status
        {
            get
            {
                return this._Status;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Status = value;
            }
        }

        public string Comment
        {
            get
            {
                return this._Comment;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Comment = value;
            }
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

        //for RCTO 147
        public decimal BoxWeight
        {
            get { return _boxWeight; }
            set
            {
                this._tracker.MarkAsModified(this);
                _boxWeight = value;
            }
        }     

        public string CartonSN
        {
            get { return _cartonSN; }
            set
            {
                this._tracker.MarkAsModified(this);
                _cartonSN = value;
            }
        }

        public string DeliveryNo
        {
            get { return _deliveryNo; }
            set
            {
                this._tracker.MarkAsModified(this);
                _deliveryNo = value;
            }
        }

        public string PalletNo
        {
            get { return _palletNo; }
            set
            {
                this._tracker.MarkAsModified(this);
                _palletNo = value;
            }
        }

        //public string PizzaID
        //{
        //    get { return _pizzaID; }
        //    set
        //    {
        //        this._tracker.MarkAsModified(this);
        //        _pizzaID = value;
        //    }
        //}

        public string QCStatus
        {
            get { return _qcStatus; }
            set
            {
                this._tracker.MarkAsModified(this);
                _qcStatus = value;
            }
        }

        public string Model
        {
            get { return _model; }
            set
            {
                this._tracker.MarkAsModified(this);
                _model = value;
            }
        }

        public string Line
        {
            get { return _line; }
            set
            {
                this._tracker.MarkAsModified(this);
                _line = value;
            }
        }

        public string ShipMode
        {
            get { return _shipMode; }
            set
            {
                this._tracker.MarkAsModified(this);
                _shipMode = value;
            }
        }
        #endregion

        #region IFisObject Members
        public override object Key
        {
            get { return _BoxId; }
        }

        #endregion

        #region external object

        private object _syncMaterialLotObj = new object();
        private MaterialLot _materialLot;
        public MaterialLot MaterialLot
        {
            get
            {
                lock (_syncMaterialLotObj)
                {
                    if (_materialLot == null)
                    {
                        MaterialBoxRepository.FillMaterialLot(this);
                    }

                    if (_materialLot != null)
                        return _materialLot;
                    else
                        return null;
                }
            }
        }



        private object _syncMaterialBoxAttrObj = new object();
        private IList<MaterialBoxAttr> _materialBoxAttrList;
        public IList<MaterialBoxAttr> MaterialBoxAttributes
        {
            get
            {
                lock (_syncMaterialBoxAttrObj)
                {
                    if (_materialBoxAttrList == null)
                    {
                        MaterialBoxRepository.FillMaterialBoxAttr(this);
                    }
                    if (_materialBoxAttrList != null)
                        return new ReadOnlyCollection<MaterialBoxAttr>(_materialBoxAttrList);
                    else
                        return null;
                }
            }
        }

        private object _syncMaterialBoxAttrLogObj = new object();
        private IList<MaterialBoxAttrLog> _materialBoxAttrLogList;
        public IList<MaterialBoxAttrLog> MaterialBoxAttributeLogs
        {
            get
            {
                lock (_syncMaterialBoxAttrLogObj)
                {
                    if (_materialBoxAttrLogList == null)
                    {
                        MaterialBoxRepository.FillMaterialBoxAttrLog(this);
                    }
                    if (_materialBoxAttrLogList != null)
                        return new ReadOnlyCollection<MaterialBoxAttrLog>(_materialBoxAttrLogList);
                    else
                        return null;
                }
            }
        }

        public bool CheckMaterailBoxStatus(string nextStatus)
        {
            return ProcessRepository.CheckMaterialProcessStatus(_MaterialType, _Status, nextStatus);
        }
        #endregion

        #region set attribute
        public void SetAttributeValue(string name, string value, string editor, string descr)
        {
            if (string.IsNullOrEmpty(name))
                return;

            lock (_syncMaterialBoxAttrObj)
            {
                object naught = MaterialBoxAttributes;

                foreach (var attrib in _materialBoxAttrList)
                {
                    if (attrib.AttrName == name)
                    {
                        //update attribute
                        string oldValue = attrib.AttrValue;
                        attrib.AttrValue = value;
                        attrib.Editor = editor;
                        attrib.Udt = DateTime.Now;
                        attrib.Cdt = DateTime.Now;

                        var log = new MaterialBoxAttrLog
                        {
                            Editor = editor,
                            AttrName = name,
                            AttrOldValue = oldValue,
                            AttrNewValue = value,
                            BoxId = _BoxId,
                            Status = _Status,
                            Descr = descr,
                            Cdt = DateTime.Now
                        };

                        log.Tracker = _tracker.Merge(log.Tracker);
                        if (_materialBoxAttrLogList == null)
                        {
                            _materialBoxAttrLogList = new List<MaterialBoxAttrLog>();
                        }
                        _materialBoxAttrLogList.Add(log);
                        //_tracker.MarkAsModified(this);
                        _tracker.MarkAsModified(_subTrackerName);
                        return;
                    }
                }

                //add new record
                var attr = new MaterialBoxAttr
                {
                    Editor = editor,
                    AttrName = name,
                    AttrValue = value,
                    BoxId = _BoxId,                     
                    Cdt = DateTime.Now,
                    Udt = DateTime.Now
                };

                attr.Tracker = _tracker.Merge(attr.Tracker);
                _materialBoxAttrList.Add(attr);
                // _tracker.MarkAsModified(this);
                _tracker.MarkAsModified(_subTrackerName);
            }
        }

        /// <summary>
        /// 鳳硌隅扽俶硉
        /// </summary>
        /// <param name="name">扽俶靡</param>
        /// <returns>扽俶硉</returns>
        public string GetAttributeValue(string name)
        {
            var values = (from p in _materialBoxAttrList
                          where p.AttrName == name
                          select p.AttrValue).ToArray();
            if (values.Length > 0)
            {
                return values.First();
            }
            return null;
        }
        #endregion
    }
}
