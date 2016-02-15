using System;
using System.Linq;
using System.Collections.Generic;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using System.Reflection;
using System.Windows.Forms;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Misc;

namespace IMES.FisObject.Common.Part.PartPolicy
{
    public class PartPolicy: IPartPolicy
    {
        private string _partCheckType;
        private string _filterModule;
        private string _matchModule;
        private string _checkModule;
        private string _saveModule;
        private bool _needUniqueCheck;
        private bool _needCommonsave;
        private bool _needPartForbidCheck;
        private bool _needDefectComponentCheck;
        private bool _repairPartType;

       public PartPolicy(string partCheckType, string filterModule, string matchModule, string checkModule, string saveModule,
                                  bool needUniqueCheck, bool needCommonsave, bool needPartForbidCheck, bool needDefectComponentCheck,
                                  bool repairPartType)
       {
            _partCheckType = partCheckType;
            _filterModule = filterModule;
            _matchModule = matchModule;
            _checkModule = checkModule;
            _saveModule = saveModule;
            _needUniqueCheck = needUniqueCheck;
            _needCommonsave = needCommonsave;
            _needPartForbidCheck = needPartForbidCheck;
            _needDefectComponentCheck = needDefectComponentCheck;
            _repairPartType = repairPartType;
        }

        public IFlatBOM FilterBOM(IHierarchicalBOM hierarchicalBOM, string station, object mainObject)
        {
            IFlatBOM ret = null;
#if DEBUG
            IFilterModule filter = (IFilterModule)GetInstance(_filterModule, typeof(IFilterModule));
#else
            IFilterModule filter = PartSpecialCodeContainer.GetInstance.GetFilterModule(_filterModule);
#endif
            if (filter != null)
            {
                ret = (IFlatBOM) filter.FilterBOM(hierarchicalBOM, station, mainObject);
            }
            else
            {
#if DEBUG
                IFilterModuleEx filterEx = (IFilterModuleEx)GetInstance(_filterModule, typeof(IFilterModuleEx));
#else
                IFilterModuleEx filterEx = PartSpecialCodeContainer.GetInstance.GetFilterModuleEx(_filterModule);
#endif
                if (filterEx != null)
                {
                    ret = (IFlatBOM)filterEx.FilterBOMEx(hierarchicalBOM, station, _partCheckType,mainObject);
                }
            }
            return ret;
        }

        public void BindTo(PartUnit part, IPartOwner owner, string station, string editor, string key)
        {
#if DEBUG
            ISaveModule saver = (ISaveModule)GetInstance(_saveModule, typeof(ISaveModule));
#else
            ISaveModule saver = PartSpecialCodeContainer.GetInstance.GetSaveModule(_saveModule);
#endif
            if (saver != null)
            {
                saver.Save(part, owner, station, key);
            }

            if (NeedCommonSave)
            {
                IProductPart newPart = new ProductPart();
                newPart.PartSn = part.Sn;        //sn
                newPart.PartID = part.Pn;       //pn
                newPart.BomNodeType = part.Type; //BomNodeType
                newPart.PartType = part.ValueType; //part.Type
                newPart.CheckItemType = part.ItemType; //CheckItemType
                newPart.CustomerPn = part.CustPn??string.Empty;
                newPart.Iecpn = part.IECPn??string.Empty;
                newPart.Station = station;
                newPart.Editor = editor;
                owner.AddPart(newPart);
            }
        }

        public void BindTo(PartUnit part, IPartOwner owner, string station, string editor, string key,
                                    Nullable<bool> needCommonSaveRule, Nullable<bool> needSaveRule)
        {
#if DEBUG
            ISaveModule saver = (ISaveModule)GetInstance(_saveModule, typeof(ISaveModule));
#else
            ISaveModule saver = PartSpecialCodeContainer.GetInstance.GetSaveModule(_saveModule);
#endif
            if (saver != null)
            {
                bool saveFlag = true;
                if (needSaveRule.HasValue)
                {
                    saveFlag = needSaveRule.Value;
                }

                if (saveFlag)
                {
                    saver.Save(part, owner, station, key);
                }
            }

            bool commonSaveFlag = _needCommonsave;
            if (needCommonSaveRule.HasValue)
            {
                commonSaveFlag = needCommonSaveRule.Value;
            }

            if (commonSaveFlag)
            {
                IProductPart newPart = new ProductPart();
                newPart.PartSn = part.Sn;        //sn
                newPart.PartID = part.Pn;       //pn
                newPart.BomNodeType = part.Type; //BomNodeType
                newPart.PartType = part.ValueType; //part.Type
                newPart.CheckItemType = part.ItemType; //CheckItemType
                newPart.CustomerPn = part.CustPn ?? string.Empty;
                newPart.Iecpn = part.IECPn ?? string.Empty;
                newPart.Station = station;
                newPart.Editor = editor;
                owner.AddPart(newPart);
            }
        }

        public PartUnit Match(string subject, IFlatBOMItem bomItem, string station)
        {
            PartUnit ret = null;
#if DEBUG
            IMatchModule matcher = (IMatchModule)GetInstance(_matchModule, typeof(IMatchModule));
#else
            IMatchModule matcher = PartSpecialCodeContainer.GetInstance.GetMatchModule(_matchModule);
#endif
            if (matcher != null)
            {
                ret = (PartUnit) matcher.Match(subject, bomItem, station);
                if (ret != null)
                {
                    ret.GetType().GetField("_flatBomItemGuid", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(ret, bomItem.GUID);
                }
            }
            return ret;
        }

        public void Check(PartUnit pu, IFlatBOMItem item, IPartOwner owner, string station, IFlatBOM bom)
        {
#if DEBUG
            ICheckModule checker = (ICheckModule)GetInstance(_checkModule, typeof(ICheckModule));
#else
            ICheckModule checker = PartSpecialCodeContainer.GetInstance.GetCheckModule(_checkModule);
#endif
            //已經Binded Part 檢查收集CT是否存在 
            if (item.HasBinded)
            {
                if (owner != null)
                {
                    var partNoList = item.AlterParts.Select(x=>x.PN).ToList();
                    bool hasBinded = owner.CheckPartBinded(pu.ValueType, partNoList, pu.Type, pu.ItemType, pu.Sn);
                    if (!hasBinded)
                    {
                        throw new FisException("MAT011", false, new string[] { pu.Sn, pu.Type, pu.ValueType, pu.ItemType });
                    }
                }
                else
                {
                    throw new FisException("MAT012", false, new string[] { pu.Sn, pu.Type, pu.ValueType, pu.ItemType });
                }
            }
            else
            {
                if (checker != null)
                {
                    checker.Check(pu, item, station);
                }

                //if have value then replace _needUniqueCheck 
                if (item.NeedCheckUnique.HasValue)
                {
                    _needUniqueCheck = item.NeedCheckUnique.Value;
                }
                
                if (NeedUniqueCheck)
                {
                    //do unique check                
                    //1.part been used on this product (mem)
                    var checkedParts = bom.GetCheckedPart();
                    if (checkedParts != null)
                    {
                        foreach (PartUnit checkPu in checkedParts)
                        {
                            if (pu.Sn == checkPu.Sn)
                            {
                                throw new FisException("CHK084", new string[] { pu.Sn });
                            }
                        }
                    }

                    //2.part been used on other product (db)
                    if (owner != null)
                    {
                        owner.PartUniqueCheck(pu.Pn, pu.Sn);
                    }

                    //3.part been used on other product (mem)
                }
                
            }

            if (NeedPartForbidCheck)
            {
                IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                string noticeMsg = "";
                if (partRep.CheckPartForbid(item.PartForbidList, pu.Type, pu.Sn, pu.Pn, out noticeMsg))
                {
                    throw new FisException("CHK1105", new string[] { pu.Type, pu.Pn, pu.Sn, noticeMsg??string.Empty});
                }
            }

            if (_needDefectComponentCheck)
            {
                IMiscRepository miscRep = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IList<IMES.DataModel.SysSettingInfo> SysSettingInfoList = partRep.GetSysSettingInfoes(new IMES.DataModel.SysSettingInfo { name = "DefectComponentPassStatus" });
                if (SysSettingInfoList == null || SysSettingInfoList.Count == 0)
                {
                    throw new FisException("PAK095", new string[] { "DefectComponentPassStatus" });
                }
                string[] passStatus = SysSettingInfoList[0].value.Split(new char[] { '~', ',', ';' });
                IList<IMES.DataModel.DefectComponentInfo> DefectComponentInfoList = miscRep.GetData<IMES.Infrastructure.Repository._Metas.DefectComponent, IMES.DataModel.DefectComponentInfo>(new IMES.DataModel.DefectComponentInfo { PartSn = pu.Sn });
                if (DefectComponentInfoList != null && DefectComponentInfoList.Count > 0)
                {
                    if (DefectComponentInfoList.Any(x => !passStatus.Contains(x.Status)))
                    {
                        throw new FisException("CQCHK1097", new string[] { pu.Sn, string.Join(",", DefectComponentInfoList.Select(x => x.Status).Distinct().ToArray()) });
                    }
                }
            }
        }

        public bool NeedUniqueCheck
        {
            get
            {
                return _needUniqueCheck;
            }
        }

        public bool NeedCommonSave
        {
            get
            {
                return _needCommonsave;
            }
        }

        public bool NeedPartForbidCheck
        {
            get
            {
                return _needPartForbidCheck;
            }
        }

        /// <summary>
        /// To check whether a PartCheckType need defect component check according to its checksetting data
        /// </summary>
        public bool NeedDefectComponentCheck
        {
            get
            {
                return _needDefectComponentCheck;
            }
        }

        /// <summary>
        /// To repair part Type according to its checksetting data
        /// </summary>
        public bool RepairPartType
        {
            get
            {
                return _repairPartType;
            }
        }


        private static object GetInstance(string programmeName, Type typeOfItf)
        {
            Object ret = null;
            Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\PartPolicyModule\\" + programmeName);
            if (ass != null)
            {
                foreach(Type tp in ass.GetTypes())
                {
                    if (tp.FullName.StartsWith("IMES.CheckItemModule") && tp.GetInterface(typeOfItf.FullName) != null)
                    {
                        ret = ass.CreateInstance(tp.FullName);
                        break;
                    }
                }
            }
            return ret;
        }
    }
}
