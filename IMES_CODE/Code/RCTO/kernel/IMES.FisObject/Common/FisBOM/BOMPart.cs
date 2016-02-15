﻿using System;
using System.Collections.Generic;
using System.Text;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part.PartPolicy;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.FisObject.Common.FisBOM
{
    public class BOMPart : FisObjectBase, IBOMPart 
    {
        private readonly IPart _part;
        private readonly string _customer;
        private readonly string _model;
        private string _valueType;
        private Model.Model _modelObj = null;
        private readonly object _syncObjModelObj = new object();

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"/> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BOMPart()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public BOMPart(IPart part, string customer, string model)
        {
            _customer = customer;
            _part = part;
            _model = model;
        }

        private IPartStrategy PartStrategy
        {
            get
            {
                return PartStrategyFactory.GetPartStrategy(this.Type, _customer);
            }
        }


        /// <summary>
        /// get AssemblCode:
        ///    Model
        ///    Famiy+Region
        ///    Family
        ///    Pn
        /// </summary>
        public IList<string> AssemblyCode
        {
            get
            {
                IList<string> ret = new List<string>();
                IList<Common.Part.AssemblyCode> allAssCodes = AssCodeRepository.GetAssemblyCodeListByPartNo(this.PN);

                //Model
                foreach (var code in allAssCodes)
                {
                    if (this.Model.Equals(code.Model))
                    {
                        ret.Add(code.AssCode);
                    }
                }
                if (ret.Count > 0) return ret;

                string family = this.ModelObj.FamilyName;
                string region = this.ModelObj.Region;

                //Family + Region
                foreach (var code in allAssCodes)
                {
                    if (family.Equals(code.Family) && region.Equals(code.Region) && string.IsNullOrEmpty(code.Model))
                    {
                        ret.Add(code.AssCode);
                    }
                }
                if (ret.Count > 0) return ret;

                //Family
                foreach (var code in allAssCodes)
                {
                    if (family.Equals(code.Family) && string.IsNullOrEmpty(code.Model) && string.IsNullOrEmpty(code.Region))
                    {
                        ret.Add(code.AssCode);
                    }
                }
                if (ret.Count > 0) return ret;

                //Pn
                foreach (var code in allAssCodes)
                {
                    if (string.IsNullOrEmpty(code.Family) && string.IsNullOrEmpty(code.Model) && string.IsNullOrEmpty(code.Region))
                    {
                        ret.Add(code.AssCode);
                    }
                }
                return ret;
            }
        }

        /// <summary>
        /// Part
        /// </summary>
        public IPart Part
        {
            get { return _part; }
        }

        /// <summary>
        /// Customer
        /// </summary>
        public string Customer
        {
            get { return _customer; }
        }

        /// <summary>
        /// Model
        /// </summary>
        public string Model
        {
            get { return _model; }
        }

        /// <summary>
        /// Model Object
        /// </summary>
        public Model.Model ModelObj
        {
            get
            {
                lock (_syncObjModelObj)
                {
                    if (_modelObj == null)
                    {
                        var repository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, IMES.FisObject.Common.Model.Model>();
                        _modelObj = repository.Find(this.Model);
                    }
                    return _modelObj;
                }
            }
        }

        /// <summary>
        /// 将指定字符串与此Part进行Match
        /// </summary>
        /// <param name="target">指定字符串</param>
        /// <param name="station">当前Station</param>
        /// <param name="valueType">需要匹配的Part属性类别</param>
        /// <returns>是否匹配</returns>
        public bool Match(string target, string station, string valueType)
        {
            bool containCheckBit;
            bool matched = this.PartStrategy.Match(target, this, station, valueType, out containCheckBit);
            //if (matched)
            //{
            //    if (containCheckBit)
            //    {
            //        this.MatchedSn = target.Substring(0, target.Length - 1);
            //    }
            //    else
            //    {
            //        this.MatchedSn = target;
            //    }
            //    this.ValueType = valueType;
            //}
            return matched;
        }

        /// <summary>
        /// Part检查，通常包括唯一性检查等
        /// </summary>
        /// <param name="owner">owner</param>
        /// <param name="station">station</param>
        public void Check(IPartOwner owner, string station)
        {
            this.PartStrategy.Check(this, owner, station);
        }

        /// <summary>
        /// 检查Part是否被Hold
        /// </summary>
        /// <param name="family">family</param>
        /// <param name="model">model</param>
        public void CheckHold(string family, string model)
        {
            throw new NotImplementedException(); string assCode = null;
            //if (this.TypeGroup.Equals("KP") && !string.IsNullOrEmpty(MatchedSn))
            //{
            //    assCode = MatchedSn.Substring(0, 6);
            //}

            ////get forbidden data
            //IPartRepository rep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            //IList<PartForbidden> forbiddens = rep.GetPartForbidden(this.ModelObj.FamilyName, this.Model, this.Descr, this.PN, assCode);
            //if (forbiddens == null || forbiddens.Count == 0)
            //{
            //    return;
            //}
            //List<string> erpara = new List<string>();
            //erpara.Add(this.PN);
            //var ex = new FisException("CHK058", erpara);
            //throw ex;
        }

//        public void BindTo(object bindTarget)
//        {
//            this.PartStrategy.BindTo(bindTarget);
//        }

        /// <summary>
        /// PartNumber, Part的唯一性标识
        /// </summary>
        public string PN
        {
            get { return _part.PN; }
            set
            {
                this._part.PN = value;
            }
        }

        /// <summary>
        /// Same with DB Name
        /// </summary>
        public string PartNo
        {
            get { return _part.PN; }
        }


        public string BOMNodeType
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Part具体类型: CPU, DIMM
        /// </summary>
        public string Type
        {
            get { return _part.Type; }
            set
            {
                this._part.Type = value;
            }
        }

        /// <summary>
        /// same as Table Column Name
        /// </summary>
        public string PartType
        {
            get { return _part.Type; }
        }

        /// <summary>
        /// 类型分组
        /// </summary>
        public string TypeGroup
        {
            get { return this._part.TypeGroup; }
        }

        /// <summary>
        /// Part的CustomerPn
        /// </summary>
        public string CustPn
        {
            get { return _part.CustPn; }
            set
            {
                this._part.CustPn = value;
            }
        }

        /// <summary>
        /// same as table column name
        /// </summary>
        public string CustPartNo
        {
            get { return _part.CustPn; }
        }

        /// <summary>
        /// Part描述信息
        /// </summary>
        public string Descr
        {
            get { return _part.Descr; }
            set
            {
                this._part.Descr = value;
            }
        }


        /// <summary>
        /// Remark
        /// </summary>
        public string Remark
        {
            get { return _part.Remark; }
            set
            {
                this._part.Remark = value;
            }
        }

        /// <summary>
        /// AutoDL
        /// </summary>
        public string AutoDL
        {
            get { return _part.AutoDL; }
            set
            {
                this._part.AutoDL = value;
            }
        }

        /// <summary>
        /// 维护人员工号
        /// </summary>
        public string Editor
        {
            get { return _part.Editor; }
            set
            {
                this._part.Editor = value;
            }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Udt
        {
            get { return _part.Udt; }
            set
            {
                this._part.Udt = value;
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Cdt
        {
            get { return _part.Cdt; }
            set
            {
                this._part.Cdt = value;
            }
        }

        /// <summary>
        /// Part描述信息 2
        /// </summary>
        public string Descr2
        {
            get { return _part.Descr2; }
            set
            {
                this._part.Descr2 = value;
            }
        }

        private static IAssemblyCodeRepository _assCodeRepository = null;
        private static IAssemblyCodeRepository AssCodeRepository
        {
            get
            {
                if (_assCodeRepository == null)
                    _assCodeRepository = RepositoryFactory.GetInstance().GetRepository<IAssemblyCodeRepository, AssemblyCode>();
                return _assCodeRepository;
            }
        }

        /// <summary>
        /// Part属性
        /// </summary>
        public IList<PartInfo> Attributes
        {
            get { return _part.Attributes; }
        }

        /// <summary>
        /// 获取Part扩展属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <returns>属性值</returns>
        public string GetAttribute(string name)
        {
            return _part.GetAttribute(name);
        }

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return _part.Key; }
        }

        /// <summary>
        /// 增加Part属性
        /// </summary>
        /// <param name="attr"></param>
        public void AddAttribute(PartInfo attr)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 更新Part属性
        /// </summary>
        /// <param name="attr"></param>
        public void ChangeAttribute(PartInfo attr)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除Part属性
        /// </summary>
        /// <param name="attr"></param>
        public void DeleteAttribute(PartInfo attr)
        {
            throw new NotImplementedException();
        }

        public string GetProperty(string propertyName)
        {
            if (_part != null)
            {
                return _part.GetProperty(propertyName);
            } 
            return null;
        }

        /// <summary>
        /// 根据属性名获取属性值， 属性包括基础属性和扩展属性，仅用于KP类型
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="assemblyCode">属性值</param>
        /// <returns></returns>
        public string GetProperty(string propertyName, string assemblyCode)
        {
            if (_part != null)
            {
                return _part.GetProperty(propertyName, assemblyCode);
            }
            return null;
        }

        /// <summary>
        /// 获取Part扩展属性，仅用于KP类型
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="assemblyCode">assCode</param>
        /// <returns>属性值</returns>
        public string GetAttribute(string name, string assemblyCode)
        {
            if (_part != null)
            {
                return _part.GetAttribute(name, assemblyCode);
            }
            return null;
        }

        public void SetKey(string key)
        {
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (_part != null)
            {
                sb.AppendFormat("IPart:{0};", _part.Key);
            }
            if (_customer != null)
            {
                sb.AppendFormat("Customer:{0};", _customer);
            }
            return string.Empty;
        }
    }
}