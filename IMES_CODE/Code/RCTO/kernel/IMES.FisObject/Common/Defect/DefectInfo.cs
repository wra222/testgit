using System;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.Common.Defect
{
    /// <summary>
    /// DefectInfo用于表示Defect周边的相关数据，分为下列类型：Component,Obligation,Cause,MajorPart,Distribution,Responsibility,4M,TrackingStation,Cover,Uncover
    /// 各种类型的结构相同，且存放在同一table中，因此用同一个类表示；
    /// </summary>
    public class DefectInfo : FisObjectBase, IAggregateRoot
    {
        public DefectInfo(int id, string type, string code, string description, string customerid)
        {
            _id = id;
            _type = type;
            _code = code;
            _description = description;
            _customerid = customerid;

            this._tracker.MarkAsAdded(this);
        }

        public DefectInfo()
        {
            this._tracker.MarkAsAdded(this);
        }

        private int _id;
        private string _type;
        private string _code;
        private string _description;
        private string _customerid;
        private DateTime _cdt;
        private DateTime _udt;
        private string _editor;
        private string _engdescription;

        /// <summary>
        /// Id
        /// </summary>
        public int Id
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
        /// 数据类型，范围：Component,Obligation,Cause,MajorPart,Distribution,Responsibility,4M,TrackingStation,Cover,Uncover
        /// </summary>
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _type = value;
            }
        }

        /// <summary>
        /// 字符代码
        /// </summary>
        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _code = value;
            }
        }

        /// <summary>
        /// 描述
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
        /// 所属客户
        /// </summary>
        public string CustomerId
        {
            get
            {
                return _customerid;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _customerid = value;
            }
        }

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
        /// English 描述
        /// </summary>
        public string EngDescription
        {
            get
            {
                return _engdescription;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                _engdescription = value;
            }
        }

        /// <summary>
        /// DefectInfo类型
        /// </summary>
        public static class DefectInfoType
        {
            public const string Component = "Component";
            public const string Obligation = "Obligation";
            public const string SaCause = "SACause";
            public const string FaCause = "FACause";
            public const string MajorPart = "MajorPart";
            public const string Distribution = "Distribution";
            public const string Responsibility = "Responsibility";
            public const string M4 = "4M";
            public const string TrackingStatus = "TrackingStatus";
            public const string Cover = "Cover";
            public const string Uncover = "Uncover";
            public const string Mark = "Mark";
        }

        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return this._id; }
        }

        #endregion
    }
}