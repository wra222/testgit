// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 对应Delivery表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-04   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectBase;
using System.Collections.ObjectModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;
using System.Reflection;

namespace IMES.FisObject.PAK.DN
{
    /// <summary>
    /// Delivery类
    /// </summary>
    [ORMapping(typeof(mtns.Delivery))]
    public class Delivery : FisObjectBase, IAggregateRoot
    {
        private static IDeliveryRepository _dnRepository = null;
        private static IDeliveryRepository DnRepository
        {
            get
            {
                if (_dnRepository == null)
                    _dnRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                return _dnRepository;
            }
        }

        private static IProductRepository _prdctRepository = null;
        private static IProductRepository PrdctRepository
        {
            get
            {
                if (_prdctRepository == null)
                    _prdctRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                return _prdctRepository;
            }
        }

        public Delivery()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        [ORMapping(mtns.Delivery.fn_deliveryNo)]
        private string _deliveryNo = null;
        [ORMapping(mtns.Delivery.fn_shipmentNo)]
        private string _shipmentno = null;
        [ORMapping(mtns.Delivery.fn_poNo)]
        private string _pono = null;
        [ORMapping(mtns.Delivery.fn_model)]
        private string _model = null;
        [ORMapping(mtns.Delivery.fn_shipDate)]
        private DateTime _shipdate = DateTime.MinValue;
        [ORMapping(mtns.Delivery.fn_qty)]
        private int _qty = int.MinValue;
        [ORMapping(mtns.Delivery.fn_status)]
        private string _status = null;
        [ORMapping(mtns.Delivery.fn_editor)]
        private string _editor = null;
        [ORMapping(mtns.Delivery.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;
        [ORMapping(mtns.Delivery.fn_udt)]
        private DateTime _udt = DateTime.MinValue;

        /// <summary>
        /// DN序号
        /// </summary>
        public string DeliveryNo
        {
            get
            {
                return this._deliveryNo;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._deliveryNo = value;
            }
        }

        /// <summary>
        /// Shipment序号
        /// </summary>
        public string ShipmentNo
        {
            get
            {
                return this._shipmentno;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._shipmentno = value;
            }
        }

        /// <summary>
        /// PoNo
        /// </summary>
        public string PoNo
        {
            get
            {
                return this._pono;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._pono = value;
            }
        }

        /// <summary>
        /// Model
        /// </summary>
        public string ModelName
        {
            get
            {
                return this._model;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._model = value;
            }
        }

        /// <summary>
        /// Model
        /// </summary>
        public string Model
        {
            get
            {
                return this._model;
            }
        }

        /// <summary>
        /// 出货日期
        /// </summary>
        public DateTime ShipDate
        {
            get
            {
                return this._shipdate;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._shipdate = value;
            }
        }

        /// <summary>
        /// DN包含的机器数量
        /// </summary>
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

        /// <summary>
        /// 状态
        /// </summary>
        public string Status
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

        /// <summary>
        /// 修改时间
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

        /// <summary>
        /// property name 抓取 property value
        /// </summary>
        /// <param name="name">Property Name</param>
        /// <returns> property value</returns>
        public object GetProperty(string name)
        {
            PropertyInfo prop = GetType().GetProperty(name);
            if (prop != null)
            {
                return prop.GetValue(this, null);
            }
            return null;
        }
        #endregion

        #region . Sub-Instances .
        private IList<DeliveryInfo> _deliveryInfoes;
        private object _syncObj_deliveryInfoes = new object();

        public IList<DeliveryInfo> DeliveryInfoes
        {
            get
            {
                lock (_syncObj_deliveryInfoes)
                {
                    if (_deliveryInfoes == null)
                    {
                        DnRepository.FillDeliveryAttributes(this);
                    }
                    if (_deliveryInfoes != null)
                    {
                        return new ReadOnlyCollection<DeliveryInfo>((from item in _deliveryInfoes orderby item.InfoType select item).ToList());// 2010-03-01   Liu Dong                     Modify ITC-1136-0025 
                    }
                    else
                        return null;
                }
            }
        }

        /// <summary>
        /// 获取检查目标对象的扩展属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <returns>属性值</returns>
        public object GetExtendedProperty(string name)
        {
            if (this.DeliveryInfoes == null)
                return null;
            return _deliveryInfoes.Where(x => x.InfoType == name).Select(x => x.InfoValue).FirstOrDefault();

            //var values = (from p in this.DeliveryInfoes
            //              where p.InfoType == name
            //              select p.InfoValue).ToArray();
            //if (values != null && values.Length > 0)
            //{
            //    return values.First();
            //}
            //return null;
        }

        /// <summary>
        /// 设置检查目标对象的扩展属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
        /// <param name="editor"></param>
        public void SetExtendedProperty(string name, object value, string editor)
        {

            if (string.IsNullOrEmpty(name))
                return;

            lock (_syncObj_deliveryInfoes)
            {
                object naught = this.DeliveryInfoes;

                DeliveryInfo[] arr = (from p in DeliveryInfoes where p.InfoType == name select p).ToArray();

                if (arr.Length > 0)
                {
                    DeliveryInfo pdi = arr[0];
                    pdi.Editor = editor;
                    pdi.InfoValue = value.ToString();
                }
                else
                {
                    DeliveryInfo pdi = new DeliveryInfo();
                    pdi.Editor = editor;
                    pdi.InfoType = name;
                    pdi.InfoValue = value.ToString();
                    pdi.DeliveryNo = this.DeliveryNo;

                    pdi.Tracker = _tracker.Merge(pdi.Tracker);
                    _deliveryInfoes.Add(pdi);
                }
                _tracker.MarkAsModified(this);
            }
        }

        private IList<DeliveryPallet> _dnplts = null;
        private object _syncObj_dnplts = new object();

        /// <summary>
        /// 获取Delivery的DeliverPallet列表
        /// </summary>
        public IList<DeliveryPallet> DnPalletList
        {
            get
            {
                lock (_syncObj_dnplts)
                {
                    if (_dnplts == null)
                    {
                        DnRepository.FillDnPalletList(this);
                    }
                    if (_dnplts != null)
                    {
                        return new ReadOnlyCollection<DeliveryPallet>(_dnplts);
                    }
                    else
                        return null;
                }
            }
        }

        private IList<DeliveryLog> _logs = null;
        private object _syncObj_logs = new object();
        /// <summary>
        /// 获取Dn所有log
        /// </summary>
        public IList<DeliveryLog> DeliveryLogs
        {
            get
            {
                lock (_syncObj_logs)
                {
                    if (_logs == null)
                    {
                        DnRepository.FillDeliveryLogs(this);
                    }
                    else
                    {
                        IList<DeliveryLog> szAllAdd = new List<DeliveryLog>();
                        foreach (DeliveryLog pl in _logs)
                        {
                            if (pl.ID > 0)
                            {
                                szAllAdd.Clear();
                                break;
                            }
                            else
                            {
                                szAllAdd.Add(pl);
                            }
                        }
                        if (szAllAdd.Count > 0)
                        {
                            DnRepository.FillDeliveryLogs(this);
                            foreach (DeliveryLog pl in szAllAdd)
                            {
                                _logs.Add(pl);
                            }
                        }
                    }
                    if (_logs != null)
                    {
                        return new ReadOnlyCollection<DeliveryLog>(_logs);
                    }
                    else
                        return null;
                }
            }
        }

        private object _syncObj_DeliveryEx = new object();
        private DeliveryEx _deliveryExInfo = null;
        public DeliveryEx DeliveryEx
        {
            get 
            {
                lock (_syncObj_DeliveryEx)
                {
                    if (_deliveryExInfo == null)
                    {
                        DnRepository.FillDeliveryEx(this);
                    }
                    return _deliveryExInfo;
                }
            }
            set
            {
                lock (_syncObj_DeliveryEx)
                {
                    object naught = this.DeliveryEx;
                    bool isAdd = false;
                    if (_deliveryExInfo == null) isAdd = true;

                    _deliveryExInfo = value;
                    _deliveryExInfo.Tracker.Clear();

                    if (isAdd)
                        _deliveryExInfo.Tracker.MarkAsAdded(_deliveryExInfo);
                    else
                        _deliveryExInfo.Tracker.MarkAsModified(_deliveryExInfo);

                    _deliveryExInfo.Tracker = _tracker.Merge(_deliveryExInfo.Tracker);

                    _tracker.MarkAsModified(this);
                }
            }
        }
        #endregion

        /// <summary>
        /// 判断绑定到DN上的机器是否和Qty相同
        /// 即判断Product表中绑定该Delivery的机器数量与Delivery的Qty相同
        /// </summary>
        /// <returns>DN与产品序号是否已全部绑定完毕</returns>
        public bool IsDNFull(int currentStationCombineQty)
        {
            int combinedQty = PrdctRepository.GetCombinedQtyByDN(this.DeliveryNo);
            if (this.Qty <= combinedQty + currentStationCombineQty)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断是否DN对应的所有Pallet上的机器已刷满
        /// 即Delivery相关所有Delivery_Pallet的Status都为1
        /// </summary>
        /// <returns></returns>
        public bool IsAllDNPalletFull()
        {
            if (DnPalletList != null)
            {
                foreach (DeliveryPallet tempDeliveryPalet in DnPalletList)
                {
                    if (tempDeliveryPalet.Status == "0")
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 获取DN绑定的Pallet数量
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        public int GetDNPalletQty(string palletNo)
        {
            if (DnPalletList != null)
            {
                foreach (DeliveryPallet tempDeliveryPalet in DnPalletList)
                {
                    if (tempDeliveryPalet.PalletID == palletNo)
                    {
                        return tempDeliveryPalet.DeliveryQty;
                    }
                }
            }

            return 0;
        }

        /// <summary>
        /// 更新DeliveryPalletStatus
        /// </summary>
        public void UpdateDeliveryPalletStatus(DeliveryPallet myDeliveryPalet)
        {
            if (myDeliveryPalet == null)
                return;

            lock (_syncObj_dnplts)
            {
                object naught = this.DnPalletList;
                if (this._dnplts == null)
                    return;
                int idx = 0;
                bool find = false;
                foreach (DeliveryPallet dp in this._dnplts)
                {
                    if (dp.Key.Equals(myDeliveryPalet.Key))
                    {
                        myDeliveryPalet.ID = dp.ID;
                        dp.Editor = myDeliveryPalet.Editor;
                        dp.Status = myDeliveryPalet.Status;
                        find = true;
                        break;
                    }
                    idx++;
                }
                if (find)
                {
                    this._dnplts[idx] = myDeliveryPalet;
                    this._tracker.MarkAsModified(this._dnplts[idx]);
                }

                //this._tracker.MarkAsModified(this);
            }
        }

        /// <summary>
        /// 记录Dn过站记录
        /// </summary>
        /// <param name="log">Dn过站记录</param>
        public void AddLog(DeliveryLog log)
        {
            if (log == null)
                return;

            lock (_syncObj_logs)
            {
                if (this._logs == null)
                {
                    this._logs = new List<DeliveryLog>();
                }

                //object naught = this.ProductLogs;
                if (this._logs.Contains(log))
                    return;

                log.Tracker = this._tracker.Merge(log.Tracker);
                this._logs.Add(log);
                this._tracker.MarkAsAdded(log);
                this._tracker.MarkAsModified(this);

                LoggingInfoFormat("Tracker Content: {0}", this._tracker.ToString());
            }
        }

        #region Overrides of FisObjectBase
        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return _deliveryNo; }
        }

        #endregion
    }
}