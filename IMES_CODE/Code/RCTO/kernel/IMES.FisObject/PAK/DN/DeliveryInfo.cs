// INVENTEC corporation (c)2010 all rights reserved. 
// Description: Delivery����չ������
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-03-31   Yuan XiaoWei                 create
// Known issues:

using System;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.PAK.DN
{
    /// <summary>
    /// Delivery����չ������
    /// </summary>
    [ORMapping(typeof(mtns.DeliveryInfo))]
    public class DeliveryInfo : FisObjectBase, IAggregateRoot
    {
        public DeliveryInfo()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        [ORMapping(mtns.DeliveryInfo.fn_id)]
        private int _id = int.MinValue;
        [ORMapping(mtns.DeliveryInfo.fn_deliveryNo)]
        private string _deliveryNo = null;
        [ORMapping(mtns.DeliveryInfo.fn_infoType)]
        private string _infoType = null;
        [ORMapping(mtns.DeliveryInfo.fn_infoValue)]
        private string _infoValue = null;
        [ORMapping(mtns.DeliveryInfo.fn_editor)]
        private string _editor = null;
        [ORMapping(mtns.DeliveryInfo.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;
        [ORMapping(mtns.DeliveryInfo.fn_udt)]
        private DateTime _udt = DateTime.MinValue;

        /// <summary>
        /// ����ID
        /// </summary>
        public int ID 
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
        /// DN����
        /// </summary>
        public string DeliveryNo
        {
            get
            {
                return _deliveryNo;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _deliveryNo = value;
            }
        }

        /// <summary>
        /// ��չ���Ե�����
        /// </summary>
        public string InfoType
        {
            get
            {
                return _infoType;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _infoType = value;
            }
        }

        /// <summary>
        /// ��չ���Ե�ֵ
        /// </summary>
        public string InfoValue
        {
            get
            {
                return _infoValue;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _infoValue = value;
            }
        }

        /// <summary>
        /// ά����Ա����
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
        /// ����ʱ��
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
        /// ����ʱ��
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

        #endregion

        #region Overrides of FisObjectBase

        /// <summary>
        /// �����ʾkey, ��ͬ����FisObject��Χ��Ψһ
        /// </summary>
        public override object Key
        {
            get { return this._id; }
        }

        #endregion
    }
}