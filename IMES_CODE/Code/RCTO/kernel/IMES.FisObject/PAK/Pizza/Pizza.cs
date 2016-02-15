// INVENTEC corporation (c)2009 all rights reserved. 
// Description: Pizza类对应Pizza表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-23   Yuan XiaoWei                 create
// 2009-10-23   Yuan XiaoWei                 ITC-1155-0071
// Known issues:
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure;
using IMES.FisObject.FA.Product;

namespace IMES.FisObject.PAK.Pizza
{
    /// <summary>
    /// Pizza类
    /// </summary>
    public class Pizza : FisObjectBase, IAggregateRoot, IPartOwner
    {
        private static IPizzaRepository _pizzaRepository = null;
        private static IPizzaRepository CurrentPizzaRepository
        {
            get
            {
                if (_pizzaRepository == null)
                    _pizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                return _pizzaRepository;
            }
        }

        private static IProductRepository _prdctRepository = null;
        private static IProductRepository PrdctRepository
        {
            get
            {
                if (_prdctRepository == null)
                    _prdctRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                return _prdctRepository;
            }
        }

        public Pizza()
        {
            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .

        private string _pizzaid;
        private string _mmiid;
        private string _customer;
        private string _family;
        private string _model;
        private string _cartonsn;
        private string _remark;
        private IProduct _bindProduct;
        private DateTime _cdt;

        /// <summary>
        /// Pizza号码绑定的Product对象
        /// </summary>
        public IProduct BindProduct
        {
            get
            {
                if (this._bindProduct == null)
                {
                    this._bindProduct = PrdctRepository.GetProductByPizzaID(this._pizzaid);
                }
                return this._bindProduct;
            }
            set
            {
                this._bindProduct = value;
            }
        }

        /// <summary>
        /// Pizza号码
        /// </summary>
        public string PizzaID
        {
            get
            {
                return this._pizzaid;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._pizzaid = value;
            }
        }

        /// <summary>
        /// MMI号码
        /// </summary>
        public string MMIID
        {
            get
            {
                return this._mmiid;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._mmiid = value;
            }
        }

        /// <summary>
        /// Pizza号码绑定的Product对象的Customer
        /// </summary>
        public string Customer
        {
            get
            {
                if (this._customer != null)
                {
                    return _customer;
                }
                else
                {
                    if (BindProduct != null)
                    {
                        this._customer = BindProduct.ModelObj.Family.Customer;
                    }
                    return _customer;
                }
            }
        }

        public string OwnerId
        {
            get { return PizzaID; }
        }

        /// <summary>
        /// Pizza号码绑定的Product对象的Family
        /// </summary>
        public string Family
        {
            get
            {
                if (this._family != null)
                {
                    return _family;
                }
                else
                {
                    if (BindProduct != null)
                    {
                        this._family = BindProduct.ModelObj.FamilyName;
                    }
                    return _family;
                }
            }
        }

        /// <summary>
        /// Pizza号码绑定的Product对象的Model
        /// </summary>
        public string Model
        {
            get
            {
               // if (this._model != null)
                if (!string.IsNullOrEmpty(_model))
                {
                    return _model;
                }
                else
                {
                    if (BindProduct != null)
                    {
                        this._model = BindProduct.Model;
                    }
                    return _model;
                }
            }
            set
            {
                if (BindProduct != null)
                {
                    this._model = BindProduct.Model;
                }
                else
                {
                    this._model = value;
                }
            }
        }

        /// <summary>
        /// CartonSN号码
        /// </summary>
        public string CartonSN
        {
            get
            {
                return this._cartonsn;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._cartonsn = value;
            }
        }


        /// <summary>
        /// Remark号码
        /// </summary>
        public string Remark
        {
            get
            {
                return this._remark;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._remark = value;
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Cdt
        {
            get { return _cdt; }
            set
            {
                //this._tracker.MarkAsModified(this);
                _cdt = value;
            }
        }

        #endregion

        #region . Sub-Instances .

        private PizzaStatus _status = null;
        private object _syncObj_status = new object();
        private IList<IProductPart> _parts = null;
        private object _syncObj_parts = new object();

        #endregion

        /// <summary>
        /// Pizza过站状态
        /// </summary>
        public PizzaStatus Status
        {
            get
            {
                lock (_syncObj_status)
                {
                    if (_status == null)
                    {
                        CurrentPizzaRepository.FillPizzaStatus(this);
                    }
                    return _status;
                }
            }
            set
            {
                lock (_syncObj_status)
                {
                    _tracker.MarkAsModified(this);
                    _status = value;
                }
            }
        }

        /// <summary>
        /// 获取与PizzaParts绑定的所有part
        /// </summary>
        public IList<IProductPart> PizzaParts
        {
            get
            {
                lock (_syncObj_parts)
                {
                    if (_parts == null)
                    {
                        CurrentPizzaRepository.FillPizzaParts(this);
                    }
                    if (_parts == null)
                    {
                        _parts = new List<IProductPart>();
                    }
                    return new ReadOnlyCollection<IProductPart>(_parts);
                }
            }
        }

        /// <summary>
        /// 更新PizzaStatus状态
        /// </summary>
        /// <param name="status"></param>
        public void UpdatePizzaStatus(PizzaStatus status)
        {
            Status = status;
            Tracker.MarkAsModified(Status);
        }

        #region Implementation of IPartOwner

        /// <summary>
        /// 检查指定part是否已经和其它PartOwner绑定, 只按照通用规则检查，
        /// 即认为已绑定Part都存放在Product_Part, PCB_Part
        /// </summary>
        /// <param name="pn">pn</param>
        /// <param name="sn">sn</param>
        public void PartUniqueCheck(string pn, string sn)
        {
            var parts = CurrentPizzaRepository.GetPizzaPartsByPartSn(sn);
            //var parts = CurrentPizzaRepository.GetPizzaPartsByPartNoAndValue(pn, sn);
            if (parts != null)
            {
                foreach (var productPart in parts)
                {
                    if (productPart.ProductID != this.PizzaID)
                    {
                        List<string> erpara = new List<string>();
                        erpara.Add("Part");
                        erpara.Add(sn);
                        erpara.Add(productPart.ProductID);
                        var ex = new FisException("CHK134", erpara);
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 检查指定part是否已经和any PartOwner绑定(including current owner), 只按照通用规则检查，
        /// 即认为已绑定Part都存放在Product_Part, PCB_Part, Pizza_Part
        /// </summary>
        /// <param name="pn">pn</param>
        /// <param name="sn">sn</param>
        public void PartUniqueCheckWithoutOwner(string pn, string sn)
        {
            var parts = CurrentPizzaRepository.GetPizzaPartsByPartSn(sn);
            //var parts = CurrentPizzaRepository.GetPizzaPartsByPartNoAndValue(pn, sn);
            if (parts != null && parts.Count > 0)
            {
                var productPart = parts.First();
                List<string> erpara = new List<string>();
                erpara.Add("Part");
                erpara.Add(sn);
                erpara.Add(parts.First().ProductID);
                var ex = new FisException("CHK134", erpara);
                throw ex;
            }
        }

        /// <summary>
        /// 檢查刷入PartNo/PartType/CT/CheckItemType 是否已收集過,若未收集過報錯
        /// </summary>
        /// <param name="partType"></param>
        /// <param name="partNoList"></param>
        /// <param name="BomNodeType"></param>
        /// <param name="checkItemType"></param>
        /// <param name="sn"></param>
        /// <returns></returns>
        public bool CheckPartBinded(string partType, IList<string> partNoList, string BomNodeType, string checkItemType, string sn)
        {
            IList<IProductPart> pizzaPartList = this.PizzaParts;
            bool hasBinded = pizzaPartList.Any(x => x.BomNodeType == BomNodeType &&
                                                                        partNoList.Contains(x.PartID) &&
                                                                        x.CheckItemType == checkItemType &&
                                                                        x.PartSn == sn);
            return hasBinded;
        }


        /// <summary>
        /// 将Part绑定到PartOwner对象
        /// 根据Part类型获取相应的IPartStrategy实现，调用IPartStrategy.BindTo()
        /// 以实现不同类型Part的绑定逻辑
        /// </summary>
        /// <param name="parts">要绑定的part列表</param>
        public void BindPart(List<IProductPart> parts)
        {
            if (parts == null)
                return;

            foreach (IProductPart part in parts)
            {
                IPartStrategy stgy = PartStrategyFactory.GetPartStrategy(part.PartType, this.Customer);
                stgy.BindTo(part, this);
            }
        }

        /// <summary>
        /// 在ProductPart列表中添加指定part
        /// 这是PartOwner的默认行为，只将
        /// Part单纯绑定到Owner不会针对不
        /// 同的Part类型进行特殊的处理
        /// </summary>
        /// <param name="part">part</param>
        public void AddPart(IProductPart part)
        {
            if (part == null)
                return;

            lock (_syncObj_parts)
            {
                object naught = this.PizzaParts;

                ((ProductPart)part).Tracker = this._tracker.Merge(((ProductPart)part).Tracker);
                this._parts.Add(part);
                this._tracker.MarkAsAdded(part);
                this._tracker.MarkAsModified(this);
            }
        }

        /// <summary>
        /// 在ProductPart列表中删除指定part
        /// </summary>
        /// <param name="partSn">part Sn</param>
        /// <param name="partPn">partPn Sn</param>
        public void RemovePart(string partSn, string partPn)
        {
            lock (_syncObj_parts)
            {
                object naught = this.PizzaParts;
                if (this._parts != null)
                {
                    foreach (IProductPart pt in this._parts)
                    {
                        if (string.Compare(pt.Value, partSn) == 0 && string.Compare(pt.PartID, partPn) == 0)
                        {
                            ((ProductPart)pt).Tracker = null;
                            this._tracker.MarkAsDeleted(pt);
                            this._tracker.MarkAsModified(this);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 删除指定CheckItemType的Part
        /// </summary>
        public void RemovePartsByType(string type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除指定BomNodeType类型的Part
        /// </summary>
        public void RemovePartsByBomNodeType(string bomNodeType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除料
        /// </summary>
        public void RemoveAllParts()
        {
            lock (_syncObj_parts)
            {
                object naught = this.PizzaParts;
                if (this._parts != null)
                {
                    foreach (IProductPart pt in this._parts)
                    {
                        ((ProductPart)pt).Tracker = null;
                        this._tracker.MarkAsDeleted(pt);
                    }
                }
                this._tracker.MarkAsModified(this);
            }
        }

        /// <summary>
        /// 根据站别获取绑定的料
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public IList<IProductPart> GetProductPartByStation(string station)
        {
            IList<IProductPart> parts = new List<IProductPart>();
            IPartRepository rep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            lock (_syncObj_parts)
            {
                object naught = this.PizzaParts;
                if (this._parts != null)
                {
                    foreach (var productPart in this._parts)
                    {
                        IPart pt = rep.Find(productPart.PartID);
                        ((ProductPart)productPart).SetPartTypeSilently(pt.Type);
                        IProductPart part = ProductPart.PartReverseSpecialDeal(productPart);
                        parts.Add(part);
                    }
                }
            }
            return parts;
        }

        #endregion

        #region Overrides of FisObjectBase

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return _pizzaid; }
        }

        #endregion

        #region ICheckObject Members
        //
        /// <summary>
        /// 获取Model指定属性
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        //        public object GetModelProperty(string item)
        //        {
        //            PropertyInfo prop = this.BindProduct.ModelObj.GetType().GetProperty(item);
        //            if (prop != null)
        //            {
        //                return prop.GetValue(this, null);
        //            }
        //            return this.BindProduct.ModelObj.GetAttribute(item);
        //        }
        //
        /// <summary>
        /// 保存Model指定属性
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        //        public void SetModelProperty(string name, object value)
        //        {
        //            throw new NotImplementedException("A property of Model can not be updated.");
        //        }
        //
        /// <summary>
        /// 获取检查目标对象的基本属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <returns>属性值</returns>
        //        public object GetProperty(string name)
        //        {
        //            PropertyInfo prop = this.GetType().GetProperty(name);
        //            if (prop != null)
        //            {
        //                return prop.GetValue(this, null);
        //            }
        //            return null;
        //        }
        //
        //        public object GetExtendedProperty(string name)
        //        {
        //            throw new NotImplementedException();
        //        }
        //
        /// <summary>
        /// 设置检查目标对象的基本属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
        //        public void SetProperty(string name, object value)
        //        {
        //            this.GetType().GetProperty(name).SetValue(this, value, null);
        //        }
        //
        //        public void SetExtendedProperty(string name, object value)
        //        {
        //            throw new NotImplementedException();
        //        }
        //
        /// <summary>
        /// 获取BOM中指定类型的Pn列表
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        //        public object GetBOMPartsByType(string item)
        //        {
        //            var repository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository, BOM>();
        //            return repository.GetPnFromMoBOMByType(this.BindProduct.MO, item);
        //        }
        //
        /// <summary>
        /// 取得检查条目。检查条目通过界面维护并存储在数据库中。
        /// </summary>
        /// <param name="customer">客户标识(字符串)</param>
        /// <param name="station">站标识(字符串)</param>
        /// <returns>检查条目对象集合</returns>
        //        public IList<ICheckItem> GetExplicitCheckItem(string customer, string station)
        //        {
        //            ICheckItemRepository rep =
        //                RepositoryFactory.GetInstance().GetRepository<ICheckItemRepository, ICheckItem>();
        //
        //            return rep.GetProductExplicitCheckItem(this.BindProduct.ProId, customer, station);
        //        }
        //
        /// <summary>
        /// 取得检查条目。检查条目通过界面维护并存储在数据库中。
        /// </summary>
        /// <param name="customer">客户标识(字符串)</param>
        /// <param name="station">站标识(字符串)</param>
        /// <returns>检查条目对象集合</returns>
        //        public IList<ICheckItem> GetImplicitCheckItem(string customer, string station)
        //        {
        //            ICheckItemRepository rep =
        //                RepositoryFactory.GetInstance().GetRepository<ICheckItemRepository, ICheckItem>();
        //
        //            return rep.GetProductImplicitCheckItem(this.BindProduct.ProId, customer, station);
        //        }
        //
        #endregion

        #region for PizzaLog
        private IList<PizzaLog> _logs = null;
        private object _syncObj_logs = new object();
        /// <summary>
        /// 获取与PizzaLog绑定的所有part
        /// </summary>
        public IList<PizzaLog> PizzaLogs
        {
            get
            {
                lock (_syncObj_logs)
                {
                    if (_logs == null)
                    {
                        CurrentPizzaRepository.FillPizzaLogs(this);
                    }
                    if (_logs == null)
                    {
                        _logs = new List<PizzaLog>();
                    }
                    return new ReadOnlyCollection<PizzaLog>(_logs);
                }
            }
        }


        public void AddPizzaLog(PizzaLog log)
        {
            if (log == null)
                return;

            lock (_syncObj_parts)
            {
                object naught = this.PizzaLogs;

                log.Tracker = _tracker.Merge(log.Tracker);
                this._logs.Add(log);
                this._tracker.MarkAsAdded(log);
                this._tracker.MarkAsModified(this);
            }
        }
        #endregion
    }
}