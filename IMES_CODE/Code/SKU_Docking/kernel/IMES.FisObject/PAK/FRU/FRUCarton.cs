using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.FisObject.PAK.FRU
{
    public class FRUCarton : FisObjectBase, IAggregateRoot
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FRUCarton(string cartonID, string model, int qty)
        {
            _cartonID = cartonID;
            _model = model;
            _qty = qty;
            this._tracker.MarkAsAdded(this);
        }

        private static IFRUCartonRepository _cartonRepository = null;
        private static IFRUCartonRepository CartonRepository
        {
            get
            {
                if (_cartonRepository == null)
                    _cartonRepository = RepositoryFactory.GetInstance().GetRepository<IFRUCartonRepository, FRUCarton>();
                return _cartonRepository;
            }
        }

        #region . Essential Fields .

        private readonly string _cartonID;
        private readonly string _model;
        private readonly int _qty;

        public string CartonID
        {
            get { return _cartonID; }
        }

        public string Model
        {
            get { return _model; }
        }

        public int Qty
        {
            get { return _qty; }
        }

        #endregion

        #region . Sub-Instances .

        private readonly IList<IFRUPart> _parts = null;
        private readonly object _syncObj_cartonParts = new object();
        private readonly IList<IFRUPart> _gifts = null;
        private readonly object _syncObj_gifts = new object();

        public IList<IFRUPart> Parts
        {
            get
            {
                lock (_syncObj_cartonParts)
                {
                    if (_parts == null)
                    {
                        CartonRepository.FillCartonParts(this);
                    }
                    if (_parts != null)
                    {
                        return new ReadOnlyCollection<IFRUPart>(_parts);
                    }
                    else
                        return null;
                }
            }
        }

        public IList<IFRUPart> Gifts
        {
            get
            {
                lock (_syncObj_gifts)
                {
                    if (_gifts == null)
                    {
                        CartonRepository.FillCartonGifts(this);
                    }
                    if (_gifts != null)
                    {
                        return new ReadOnlyCollection<IFRUPart>(_gifts);
                    }
                    else
                        return null;
                }
            }
        }

        #endregion

        public void AddPart(IFRUPart part)
        {
            if (part == null)
                return;

            lock (_syncObj_cartonParts)
            {
                object naught = this.Parts;
                if (this._parts.Contains(part))
                    return;

                ((FRUPart)part).Tracker = this._tracker.Merge(((FRUPart)part).Tracker);
                this._parts.Add(part);
                this._tracker.MarkAsAdded(part);
                this._tracker.MarkAsModified(this);
            }
        }

        public void AddGift(IFRUPart giftPart)
        {
            if (giftPart == null)
                return;

            lock (_syncObj_gifts)
            {
                object naught = this.Gifts;
                if (this._gifts.Contains(giftPart))
                    return;

                ((FRUPart)giftPart).Tracker = this._tracker.Merge(((FRUPart)giftPart).Tracker);
                this._gifts.Add(giftPart);
                this._tracker.MarkAsAdded(giftPart);
                this._tracker.MarkAsModified(this);
            }
        }

        public void RemoveAllGift()
        {
            lock (_syncObj_gifts)
            {

                object naught = Gifts;
                if (this._gifts != null)
                {
                    foreach (IFRUPart pt in this._gifts)
                    {
                        ((FRUPart)pt).Tracker = null;
                        this._tracker.MarkAsDeleted(pt);
                    }
                }
                this._tracker.MarkAsModified(this);
            }
        }

        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return _cartonID; }
        }

        #endregion
    }
}
