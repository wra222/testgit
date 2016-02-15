using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;

namespace IMES.FisObject.Common.Part
{
    /// <summary>
    /// Part类型的描述，一个part类型可能存在多个不同的描述
    /// </summary>
    public class PartTypeDescription : FisObjectBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partType"></param>
        /// <param name="description"></param>
        public PartTypeDescription(int id, string partType, string description)
        {
            _id = id;
            _partType = partType;
            _description = description;

            this._tracker.MarkAsAdded(this);
        }

        private int _id;
        private string _partType;
        private string _description;

        /// <summary>
        /// Id
        /// </summary>
        public int ID
        {
            get { return _id; }
            set 
            {
                _id = value;
                this._tracker.MarkAsModified(this);
            }
        }

        /// <summary>
        /// Part类型
        /// </summary>
        public string PartType
        {
            get { return _partType; }
        }

        /// <summary>
        /// 类型描述
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return _partType; }
        }

        #endregion
    }
}
