using System;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.Common.Model
{
    /// <summary>
    /// Model����չ������
    /// </summary>
    [ORMapping(typeof(mtns.ModelInfo))]
    public class ModelInfo : FisObjectBase
    {
        public ModelInfo()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        [ORMapping(mtns.ModelInfo.fn_id)]
        private long _id = int.MinValue;
        [ORMapping(mtns.ModelInfo.fn_model)]
        private string _modelName = null;
        [ORMapping(mtns.ModelInfo.fn_name)]
        private string _name = null;
        [ORMapping(mtns.ModelInfo.fn_value)]
        private string _value = null;
        [ORMapping(mtns.ModelInfo.fn_descr)]
        private string _description = null;
        [ORMapping(mtns.ModelInfo.fn_editor)]
        private string _editor = null;
        [ORMapping(mtns.ModelInfo.fn_udt)]
        private DateTime _udt = DateTime.MinValue;
        [ORMapping(mtns.ModelInfo.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;

        /// <summary>
        /// ID
        /// </summary>
        public long ID 
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
        /// ModelName
        /// </summary>
        public string ModelName
        {
            get
            {
                return _modelName;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _modelName = value;
            }
        }

        /// <summary>
        /// ��չ���Ե�����
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _name = value;
            }
        }

        /// <summary>
        /// ��չ���Ե�ֵ
        /// </summary>
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _value = value;
            }
        }

        /// <summary>
        /// ��չ���Ե�����
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _description = value;
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