using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.Common.HDDCopyInfo
{
    ///<summary>
    /// HDDCopy对应实体
    ///</summary>
    public class HDDCopyInfo : FisObjectBase, IAggregateRoot
    {
        private int _id;
        private string _copyMachineId;
        private string _connectorId;
        private string _sourceHdd;
        private string _hddNo;
        private string _editor;
        private DateTime _cdt;
        private DateTime _udt;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public HDDCopyInfo(int id, string copyMachineId, string connectorId, string sourceHdd, string hddNo, string editor)
        {
            _id = id;
            _copyMachineId = copyMachineId;
            _connectorId = connectorId;
            _sourceHdd = sourceHdd;
            _hddNo = hddNo;
            _editor = editor;

            this._tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// Id
        /// </summary>
        public int Id
        {
            get { return _id; }
            set
            {
                this._tracker.MarkAsModified(this);
                _id = value;
            }
        }

        /// <summary>
        /// CopyMachineId
        /// </summary>
        public string CopyMachineId
        {
            get { return _copyMachineId; }
            set
            {
                this._tracker.MarkAsModified(this);
                _copyMachineId = value;
            }
        }

        /// <summary>
        /// ConnectorId
        /// </summary>
        public string ConnectorId
        {
            get { return _connectorId; }
            set
            {
                this._tracker.MarkAsModified(this);
                _connectorId = value;
            }
        }

        /// <summary>
        /// SourceHdd
        /// </summary>
        public string SourceHdd
        {
            get { return _sourceHdd; }
            set
            {
                this._tracker.MarkAsModified(this);
                _sourceHdd = value;
            }
        }

        /// <summary>
        /// HddNo
        /// </summary>
        public string HddNo
        {
            get { return _hddNo; }
            set
            {
                this._tracker.MarkAsModified(this);
                _hddNo = value;
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
                this._tracker.MarkAsModified(this);
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
                this._tracker.MarkAsModified(this);
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
                this._tracker.MarkAsModified(this);
                _udt = value;
            }
        }

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return _id; }
        }
    }
}
