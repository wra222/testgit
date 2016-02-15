// INVENTEC corporation (c)2009 all rights reserved. 
// Description: Pallet类对应Pallet表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-27   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using System.Collections.ObjectModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.PAK.Pallet
{
    /// <summary>
    /// Pallet类
    /// </summary>
    [ORMapping(typeof(mtns::Pallet))]
    public class Pallet : FisObjectBase, IAggregateRoot
    {
        private static IPalletRepository _pltRepository = null;
        private static IPalletRepository PltRepository
        {
            get
            {
                if (_pltRepository == null)
                    _pltRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                return _pltRepository;
            }
        }

        public Pallet()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .

        [ORMapping(mtns::Pallet.fn_palletNo)]
        private string _palletNo = null;
        [ORMapping(mtns::Pallet.fn_ucc)]
        private string _ucc = null;
        [ORMapping(mtns::Pallet.fn_palletModel)]
        private string _palletmodel = null;
        [ORMapping(mtns::Pallet.fn_station)]
        private string _station = null;
        [ORMapping(mtns::Pallet.fn_length)]
        private decimal _length = default(decimal);//decimal.MinValue;
        [ORMapping(mtns::Pallet.fn_width)]
        private decimal _width = default(decimal);//decimal.MinValue;
        [ORMapping(mtns::Pallet.fn_height)]
        private decimal _height = default(decimal);//decimal.MinValue;
        [ORMapping(mtns::Pallet.fn_weight)]
        private decimal _weight = default(decimal);//decimal.MinValue;
        [ORMapping(mtns::Pallet.fn_weight_L)]
        private decimal _weight_l = default(decimal);//decimal.MinValue;
        [ORMapping(mtns::Pallet.fn_editor)]
        private string _editor = null;
        [ORMapping(mtns::Pallet.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;
        [ORMapping(mtns::Pallet.fn_udt)]
        private DateTime _udt = DateTime.MinValue;
        [ORMapping(mtns::Pallet.fn_floor)]
        private string _floor = "";

        /// <summary>
        /// Pallet序号
        /// </summary>
        public string PalletNo
        {
            get
            {
                return this._palletNo;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._palletNo = value;
            }
        }

        /// <summary>
        /// UCC Code
        /// </summary>
        public string UCC
        {
            get
            {
                return this._ucc;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._ucc = value;
            }
        }

        /// <summary>
        /// PalletModel
        /// </summary>
        public string PalletModel
        {
            get
            {
                return this._palletmodel;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._palletmodel = value;
            }
        }

        /// <summary>
        /// 站别
        /// </summary>
        public string Station
        {
            get
            {
                return this._station;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._station = value;
            }
        }

        /// <summary>
        /// 栈板长
        /// </summary>
        public decimal Length
        {
            get
            {
                return this._length;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._length = value;
            }
        }

        /// <summary>
        /// 栈板宽
        /// </summary>
        public decimal Width
        {
            get
            {
                return this._width;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._width = value;
            }
        }

        /// <summary>
        /// 栈板高
        /// </summary>
        public decimal Height
        {
            get
            {
                return this._height;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._height = value;
            }
        }

        /// <summary>
        /// 栈板重量
        /// </summary>
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

        public decimal Weight_L
        {
            get
            {
                return this._weight_l;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._weight_l = value;
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
        /// 更新时间
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
        /// Floor
        /// </summary>
        public string Floor
        {
            get
            {
                return this._floor;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._floor = value;
            }
        }

        #endregion

        #region . Sub-Instances .

        private FISToSAPWeight _dnWeight = null;
        private object _syncObj_dnWeight = new object();
        private FISToSAPPLTWeight _pltWeight = null;
        private object _syncObj_pltWeight = new object();
        private IList<PalletLog> _palletLogs = null;
        private object _syncObj_palletLogs = new object();
        #endregion

        /// <summary>
        /// 获取与Pallet绑定的所有log
        /// </summary>
        public IList<PalletLog> PalletLogs
        {
            get
            {
                lock (_syncObj_palletLogs)
                {
                    if (_palletLogs == null)
                    {
                        PltRepository.FillPalletLogs(this);
                    }
                    if (_palletLogs != null)
                    {
                        return new ReadOnlyCollection<PalletLog>(_palletLogs);
                    }
                    else
                        return null;
                }
            }
        }

        /// <summary>
        /// 用来记录Pallet过站Log
        /// </summary>
        /// <param name="log">Pallet过站记录</param>
        public void AddLog(PalletLog log)
        {
            if (log == null)
                return;

            lock (_syncObj_palletLogs)
            {
                object naught = this.PalletLogs;
                if (this._palletLogs.Contains(log))
                    return;

                log.Tracker = this._tracker.Merge(log.Tracker);
                this._palletLogs.Add(log);
                this._tracker.MarkAsAdded(log);
                this._tracker.MarkAsModified(this);
            }
        }

        /// <summary>
        /// 判断一个Pallet上的所有DN是不是都已经做完
        /// </summary>
        /// <param name="exceptDn">当前完成DeliveryPallet的DN的号码</param>
        /// <returns></returns>
        public bool IsPalletFull(string exceptDn)
        {
            IList<DeliveryPallet> dpList = PltRepository.GetDeliveryPallet(this.PalletNo);
            if (dpList != null)
            {
                foreach (DeliveryPallet temp in dpList)
                {
                    if (temp.Status == "0" && temp.DeliveryID != exceptDn)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return _palletNo; }
        }

        #endregion

    }
}