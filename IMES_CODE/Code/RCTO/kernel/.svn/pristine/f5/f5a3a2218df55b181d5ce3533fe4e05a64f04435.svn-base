using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using System.Reflection;
using IMES.DataModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.ObjectModel;

namespace IMES.FisObject.Common.Model
{
    /// <summary>
    /// Family of product
    /// </summary>
    public class Family : FisObjectBase, IAggregateRoot
    {
        /// <summary>
        /// Family构造函数
        /// </summary>
        public Family(string family, string description, string customer)
        {
            _familyName = family;
            _description = description;
            _customer = customer;

            this._tracker.MarkAsAdded(this);
        }
        
        private static readonly IFamilyRepository FmlRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository, Family>();
        

        #region . Essential Fields .

        private string _familyName;
        private string _description;
        private string _customer;
        private string _editor;
        private DateTime _cdt;
        private DateTime _udt;

        /// <summary>
        /// FamilyName
        /// </summary>
        public string FamilyName
        {
            get
            {
                return _familyName;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _familyName = value;
            }
        }

        /// <summary>
        /// Description
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _description = value;
            }
        }

        ///<summary>
        /// Customer
        ///</summary>
        public string Customer
        {
            get
            {
                return _customer;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _customer = value;
            }
        }

        ///<summary>
        /// Editor
        ///</summary>
        public string Editor
        {
            get
            {
                return _editor;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _editor = value;
            }
        }

        ///<summary>
        /// Cdt
        ///</summary>
        public DateTime Cdt
        {
            get
            {
                return _cdt;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _cdt = value;
            }
        }

        ///<summary>
        /// Udt
        ///</summary>
        public DateTime Udt
        {
            get
            {
                return _udt;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _udt = value;
            }
        }

        #endregion

        public object GetProperty(string name)
        {
            object value = null;
            PropertyInfo prop = this.GetType().GetProperty(name);
            if (prop != null)
            {
                value = prop.GetValue(this, null);
            }
            return value;
        }

        private IList<FamilyInfoDef> _attributes;
        private object _syncObj_attributes = new object();

        /// <summary>
        /// Attributes of Family
        /// </summary>
        public IList<FamilyInfoDef> Attributes
        {
            get
            {
                lock (_syncObj_attributes)
                {
                    if (_attributes == null)
                    {
                        _attributes = FmlRepository.GetExistFamilyInfo(null);
                    }
                    if (_attributes != null)
                    {
                        return new ReadOnlyCollection<FamilyInfoDef>(_attributes);
                    }
                    return null;
                }
            }
        }

        public string GetAttribute(string name)
        {
            if (this.Attributes != null)
            {
                return _attributes.Where(x => x.name == name).Select(x => x.value).FirstOrDefault();
            }
            return null;

        }        

        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return _familyName; }
        }

        #endregion
    }
}
