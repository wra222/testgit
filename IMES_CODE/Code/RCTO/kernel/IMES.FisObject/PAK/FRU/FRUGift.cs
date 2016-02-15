using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.FisObject.PAK.FRU
{
    public class FRUGift : FisObjectBase, IAggregateRoot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public FRUGift(string model, string giftID, int qty)
        {
            _model = model;
            _giftID = giftID;
            _qty = qty;
            this._tracker.MarkAsAdded(this);
        }

        private static IGiftRepository _giftRepository = null;
        private static IGiftRepository GiftRepository
        {
            get
            {
                if (_giftRepository == null)
                    _giftRepository = RepositoryFactory.GetInstance().GetRepository<IGiftRepository, FRUGift>();
                return _giftRepository;
            }
        }

        #region . Essential Fields .
        
        private readonly string _giftID;
        private readonly string _model;
        private readonly int _qty;

        public string GiftID
        {
            get { return _giftID; }
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

        private IList<IFRUPart> _parts = null;
        private object _syncObj_giftParts = new object();

        public IList<IFRUPart> Parts
        {
            get
            {
                lock (_syncObj_giftParts)
                {
                    if (_parts == null)
                    {
                        GiftRepository.FillGiftParts(this);
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

        #endregion

        public void AddPart(IFRUPart part)
        {
            if (part == null)
                return;

            lock (_syncObj_giftParts)
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

        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return _giftID; }
        }

        #endregion
    }
}
