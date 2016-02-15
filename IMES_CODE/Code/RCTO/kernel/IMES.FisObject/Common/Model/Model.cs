// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 对Model相关对象的操作
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-22   Yuan XiaoWei                 create
// 2010-03-01   Liu Dong                     Modify ITC-1136-0025 
// Known issues:
using System;
using System.Linq;
using System.Collections.Generic;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.ObjectModel;
using System.Reflection;

namespace IMES.FisObject.Common.Model
{
    /// <summary>
    /// 机型类
    /// </summary>
    public class Model : FisObjectBase, IAggregateRoot
    {
        private static IModelRepository _mdlRepository;
        private static IModelRepository MdlRepository
        {
            get
            {
                if (_mdlRepository == null)
                    _mdlRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                return _mdlRepository;
            }
        }

        private static IFamilyRepository _fmlRepository;
        private static IFamilyRepository FmlRepository
        {
            get
            {
                if (_fmlRepository == null)
                    _fmlRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository, Family>();
                return _fmlRepository;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Model()
        {
            _tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .

        private string _model;
        private string _family;
        private string _custPN;
        private string _region;
        private string _shipType;
        private string _status;
        private string _osCode;
        private string _osDesc;
        private string _price;
        private DateTime _bomApproveDate;
        private string _editor;
        private DateTime _cdt;
        private DateTime _udt;

        /// <summary>
        /// 机型名称
        /// </summary>
        public string ModelName 
        {
            get
            {
                return _model;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _model = value;
            }
        }

        /// <summary>
        /// 机型所属的Family名称
        /// </summary>
        public string FamilyName
        {
            get
            {
                return _family;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _family = value;
            }
        }

        /// <summary>
        /// 机型的CustomerPn
        /// </summary>
        public string CustPN
        {
            get
            {
                return _custPN;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _custPN = value;
            }
        }

        /// <summary>
        /// Region
        /// </summary>
        public string Region
        {
            get
            {
                return _region;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _region = value;
            }
        }

        /// <summary>
        /// ShipType
        /// </summary>
        public string ShipType
        {
            get
            {
                return _shipType;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _shipType = value;
            }
        }

        /// <summary>
        /// 机型状态, 可能的取值范围???
        /// </summary>
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _status = value;
            }
        }

        /// <summary>
        /// 机型的OsCode???
        /// </summary>
        public string OSCode
        {
            get
            {
                return _osCode;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _osCode = value;
            }
        }

        /// <summary>
        /// 机型的OsDesc???
        /// </summary>
        public string OSDesc
        {
            get
            {
                return _osDesc;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _osDesc = value;
            }
        }

        /// <summary>
        /// Price
        /// </summary>
        public string Price
        {
            get
            {
                return _price;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _price = value;
            }
        }

        /// <summary>
        /// BOMApproveDate
        /// </summary>
        public DateTime BOMApproveDate
        {
            get
            {
                return _bomApproveDate;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _bomApproveDate = value;
            }
        }

        /// <summary>
        /// Editor
        /// </summary>
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

        /// <summary>
        /// 创建时间
        /// </summary>
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

        /// <summary>
        /// 修改时间
        /// </summary>
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

        #region . Sub-Instances .

        /// <summary>
        /// 机型的扩展属性
        /// </summary>
        private IList<ModelInfo> _attributes;
        private object _syncObj_attributes = new object();
        private Family _fmlObj = null;
        private object _syncObj_fmlObj = new object();

        /// <summary>
        /// 机型所属的Family对象
        /// </summary>
        public Family Family
        {
            get 
            {
                lock (_syncObj_fmlObj)
                {
                    if (_fmlObj == null)
                    {
                        FmlRepository.FillFamilyObj(this);
                    }
                    return _fmlObj;
                }
            }
        }

        /// <summary>
        /// Attributes of Model
        /// </summary>
        public IList<ModelInfo> Attributes
        {
            get
            {
                lock (_syncObj_attributes)
                {
                    if (_attributes == null)
                    {
                        MdlRepository.FillModelAttributes(this);
                    }
                    if (_attributes != null)
                    {
                        return new ReadOnlyCollection<ModelInfo>((from item in _attributes orderby item.Name select item).ToList());// 2010-03-01   Liu Dong                     Modify ITC-1136-0025 
                    }
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取机型的指定扩展属性
        /// </summary>
        /// <param name="name">扩展属性名称</param>
        /// <returns>扩展属性值</returns>
        public string GetAttribute(string name)
        {
            lock (_syncObj_attributes)
            {
                object naught = this.Attributes;
                if (_attributes != null)
                {
                    foreach (ModelInfo item in _attributes)
                    {
                        if (item.Name == name)
                            return item.Value;
                    }
                }
                return null;
            }
        }

        public IList<ModelInfo> GetAttributes(string name)
        {
            IList<ModelInfo> ret = new List<ModelInfo>();
            lock (_syncObj_attributes)
            {
// ReSharper disable UnusedVariable
                object naught = Attributes;
// ReSharper restore UnusedVariable
                if (_attributes != null)
                {
                    foreach (ModelInfo item in _attributes)
                    {
                        if (item.Name == name)
                            ret.Add(item);
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// 增加Model属性
        /// </summary>
        /// <param name="item">属性</param>
        public void AddAttribute(ModelInfo item)
        {
            if (item == null)
                return;

            lock (_syncObj_attributes)
            {
                object naught = Attributes;
                if (_attributes != null)
                {
                    if (_attributes.Contains(item))
                        return;

                    item.Tracker = _tracker.Merge(item.Tracker);
                    _attributes.Add(item);
                    _tracker.MarkAsAdded(item);
                    _tracker.MarkAsModified(this);
                }
            }
        }

        /// <summary>
        /// 更新Model属性
        /// </summary>
        /// <param name="item">属性</param>
        public void ChangeAttribute(ModelInfo item)
        {
            if (item == null)
                return;

            lock (_syncObj_attributes)
            {
                object naught = Attributes;
                if (this._attributes != null)
                {
                    int idx = 0;
                    bool find = false;
                    foreach (ModelInfo mi in _attributes)
                    {
                        if (mi.Key.Equals(item.Key))
                        {
                            find = true;
                            break;
                        }
                        idx++;
                    }
                    if (find)
                    {
                        _attributes[idx] = item;
                        _tracker.MarkAsModified(_attributes[idx]);
                        _tracker.MarkAsModified(this);
                    }
                }
            }
        }

        /// <summary>
        /// 删除Model属性
        /// </summary>
        /// <param name="item">属性名</param>
        public void DeleteAttribute(ModelInfo item)
        {
            if (item == null)
                return;

            lock (_syncObj_attributes)
            {
                object naught = Attributes;
                if (_attributes != null)
                {
                    ModelInfo miFound = null;
                    foreach (ModelInfo mi in _attributes)
                    {
                        if (mi.Key.Equals(item.Key))
                        {
                            miFound = mi;
                            break;
                        }
                    }
                    if (miFound != null)
                    {
                        //this._attributes.Remove(miFound);
                        miFound.Tracker = null;
                        _tracker.MarkAsDeleted(miFound);
                        _tracker.MarkAsModified(this);
                    }
                }
            }
        }

        #endregion

        public bool IsCTO
        {    
            get
            {
                //机型12码第七码是字母的是BTO，第七码是数字的是CTO,Y为特殊类型
                var seventhChar = _model.ElementAt(6);
                if (seventhChar >= '0' && seventhChar <= '9')
                {
                    return true;
                }
                return false;
            }
        }

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
        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return _model; }
        }

        #endregion
    }
}