using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.Common.TestLog
{
    /// <summary>
    /// 测试Log的Defect
    /// </summary>
    public class TestLogDefect : FisObjectBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TestLogDefect()
        {
            this._tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TestLogDefect(int id, int testLogId, string defectCode, string editor, DateTime cdt)
        {
            this._id = id;
            this._testLogId = testLogId;
            this._defectCode = defectCode;
            this._editor = editor;
            this._cdt = cdt;

            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .

        private int _id;
        private int _testLogId;
        private string _defectCode;
        private string _editor;
        private DateTime _cdt;

        /// <summary>
        /// ID
        /// </summary>
        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._id = value;
            }
        }

        /// <summary>
        /// product/Mb标识 
        /// </summary>
        public int TestLogID
        {
            get
            {
                return this._testLogId;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._testLogId = value;
            }
        }

        /// <summary>
        /// DefectCode
        /// </summary>
        public string DefectCode
        {
            get
            {
                return this._defectCode;
            }
            private set
            {
                this._tracker.MarkAsModified(this);
                this._defectCode = value;
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
            private set
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
            private set
            {
                this._tracker.MarkAsModified(this);
                this._cdt = value;
            }
        }

        #endregion

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
