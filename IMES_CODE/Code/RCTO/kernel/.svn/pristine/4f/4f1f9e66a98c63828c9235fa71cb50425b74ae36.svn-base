using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;

namespace IMES.FisObject.PCA.EcrVersion
{
    /// <summary>
    /// ECRVersion实体
    /// </summary>
    [ORMapping(typeof(mtns::EcrVersion))]
    public class EcrVersion : FisObjectBase, IAggregateRoot
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EcrVersion()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        [ORMapping(mtns::EcrVersion.fn_id)]
        private int _id = int.MinValue;
        [ORMapping(mtns::EcrVersion.fn_family)]
        private string _family = null;
        [ORMapping(mtns::EcrVersion.fn_mbcode)]
        private string _mbcode = null;
        [ORMapping(mtns::EcrVersion.fn_ecr)]
        private string _ecr = null;
        [ORMapping(mtns::EcrVersion.fn_iecver)]
        private string _iecver = null;
        [ORMapping(mtns::EcrVersion.fn_remark)]
        private string _remark = null; //_custver = null;
        [ORMapping(mtns::EcrVersion.fn_editor)]
        private string _editor = null;
        [ORMapping(mtns::EcrVersion.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;
        [ORMapping(mtns::EcrVersion.fn_udt)]
        private DateTime _udt = DateTime.MinValue;

        /// <summary>
        /// ID
        /// </summary>
        public int ID
        {
            get { return _id; }
            set
            {
                _tracker.MarkAsModified(this);
                _id = value;
            }
        }

        /// <summary>
        /// Family
        /// </summary>
        public string Family
        {
            get { return _family; }
            set
            {
                _tracker.MarkAsModified(this);
                _family = value;
            }
        }

        /// <summary>
        /// MBCode
        /// </summary>
        public string MBCode
        {
            get { return _mbcode; }
            set
            {
                _tracker.MarkAsModified(this);
                _mbcode = value;
            }
        }

        /// <summary>
        /// ECR
        /// </summary>
        public string ECR
        {
            get { return _ecr; }
            set
            {
                _tracker.MarkAsModified(this);
                _ecr = value;
            }
        }

        /// <summary>
        /// IECVer
        /// </summary>
        public string IECVer
        {
            get { return _iecver; }
            set
            {
                _tracker.MarkAsModified(this);
                _iecver = value;
            }
        }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark
        {
            get { return _remark; }
            set
            {
                _tracker.MarkAsModified(this);
                _remark = value;
            }
        }

        /// <summary>
        /// Editor
        /// </summary>
        public string Editor
        {
            get { return _editor; }
            set
            {
                _tracker.MarkAsModified(this);
                _editor = value;
            }
        }

        /// <summary>
        /// Cdt
        /// </summary>
        public DateTime Cdt
        {
            get { return _cdt; }
            set
            {
                _tracker.MarkAsModified(this);
                _cdt = value;
            }
        }

        /// <summary>
        /// Udt
        /// </summary>
        public DateTime Udt
        {
            get { return _udt; }
            set
            {
                _tracker.MarkAsModified(this);
                _udt = value;
            }
        }

        #endregion

        #region IFisObject Members

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
