using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using System.Collections.ObjectModel;

namespace IMES.FisObject.FA.LCM
{
    /// <summary>
    /// LCM实体
    /// </summary>
    public class LCM : FisObjectBase, IAggregateRoot
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public LCM(string iecSn, string pn, string vendorSn, IList<LCMME> meParts)
        {
            _iecSn = iecSn;
            _pn = pn;
            _vendorSn = vendorSn;
            _meParts = meParts;

            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .

        private string _iecSn;
        private string _pn;
        private string _vendorSn;

        /// <summary>
        /// IECSn
        /// </summary>
        public string IECSn
        {
            get { return _iecSn; }
        }

        /// <summary>
        /// Pn
        /// </summary>
        public string Pn
        {
            get { return _pn; }
        }

        /// <summary>
        /// VendorSn
        /// </summary>
        public string VendorSn
        {
            get { return _vendorSn; }
        }

        #endregion

        #region . Sub-Instances .

        private IList<LCMME> _meParts = null;
        private object _syncObj_mePart = new object();

        /// <summary>
        /// LCM绑定的所有ME
        /// </summary>
        public IList<LCMME> MEParts
        {
            get 
            {
                lock (_syncObj_mePart)
                {
                    return new ReadOnlyCollection<LCMME>(_meParts);
                }
            }
        }

        /// <summary>
        /// 为LCM绑定一个ME
        /// </summary>
        /// <param name="me"></param>
        public void AddMEParts(LCMME me)
        {
            if (me == null)
                return;

            lock (_syncObj_mePart)
            {
                me.Tracker = this._tracker.Merge(me.Tracker);
                this._meParts.Add(me);
                this._tracker.MarkAsAdded(me);
                this._tracker.MarkAsModified(this);
            }
        }

        #endregion

        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return _iecSn; }
        }

        #endregion
    }
}
