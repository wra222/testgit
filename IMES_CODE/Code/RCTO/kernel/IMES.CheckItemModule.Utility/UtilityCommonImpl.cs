using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using System.Management.Instrumentation;
using System.Text.RegularExpressions;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Line;
using IMES.FisObject.Common.Misc;
using IMES.FisObject.Common.MO;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.PrintItem;
using IMES.FisObject.Common.Process;
using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.Warranty;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.PCA.MBMO;
using IMES.FisObject.PCA.MBModel;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;

using System.Data;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.PAK.BSam;
using IMES.Infrastructure.Util;
using IMES.Resolve.Common;

namespace IMES.CheckItemModule.Utility
{
    public sealed class UtilityCommonImpl
    {

        private static readonly UtilityCommonImpl _Instance = new UtilityCommonImpl();
        
        private UtilityCommonImpl()
        {
        }

        /// <summary>
        /// GetInstance
        /// </summary>
        /// <returns></returns>
        public static UtilityCommonImpl GetInstance()
        {
            //if (_Instance == null)
            //{
            //    _Instance = new UtilityCommonImpl();
            //}
            return _Instance;
        }
        public readonly IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
        public readonly IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
        public readonly IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
        public readonly IMBRepository mbRep = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
        public readonly IProcessRepository currentProcessRepository = RepositoryFactory.GetInstance().GetRepository<IProcessRepository, Process>();

        #region export class
        public readonly BomFilterRule FilterMatchRule = new BomFilterRule();
        #endregion

        /// <summary>
        /// CheckCT by Family and VC
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public void CheckCT_by_FamilyVC(string ct, string family, string vendorCode)
        {
            //IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            ConstValueInfo info = new ConstValueInfo();
            info.type = GlobalConstName.ConstValue.CTCheckRule;
            info.name = family + GlobalConstName.TildeStr + vendorCode;
            IList<ConstValueInfo> retList = partRepository.GetConstValueInfoList(info);
            if (null == retList || retList.Count == 0)
                return;

            string pattern = retList[0].value;
            pattern = pattern.Replace("?", "\\w").Replace("*", "\\w*").Replace("%", "\\w+");
            pattern = "^" + pattern + "$";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(ct))
            {
                FisException fex = new FisException("CHK1084", new string[] { ct });
                fex.stopWF = false;
                throw fex;
            }
        }

        public string GetCTByCheckItemType(string checkItemType, string subject, out bool hasCTMatchRule)
        {
            hasCTMatchRule = false;
           // IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            ConstValueInfo info = new ConstValueInfo();
            info.type =GlobalConstName.ConstValue.CTMatchRule;
            info.name = checkItemType;
            IList<ConstValueInfo> retList = partRepository.GetConstValueInfoList(info);
            if (null == retList || retList.Count == 0)
                return null;

            hasCTMatchRule = true;
            string pattern = retList[0].value;
            Match match = Regex.Match(subject, pattern, RegexOptions.Compiled);
            if (match.Success)
            {
                return match.Groups[GlobalConstName.RegEx.CT].Value;
            }
            else
            {
                return string.Empty;
            }
        }

        public  PartUnit GetPartUnitAndCheckVendorCode(FlatBOMItem bomItem, string ct)
        {
            PartUnit ret = null;
            IList<IPart> parts = bomItem.AlterParts;
            if (parts != null)
            {
                foreach (IPart part in parts)
                {
                    IList<PartInfo> part_infos = part.Attributes;
                    if (part_infos != null)
                    {
                        foreach (PartInfo part_info in part_infos)
                        {
                            if (part_info.InfoType.Equals(GlobalConstName.PartInfo.VendorCode))
                            {
                                if (part_info.InfoValue.Trim().Equals(ct.Substring(0, 5)))
                                {
                                    ret = new PartUnit(part.PN, ct, part.BOMNodeType, part.Type, string.Empty, part.CustPn, bomItem.CheckItemType);
                                    break;
                                }
                            }
                        }
                    }
                    if (ret != null)
                    {
                        break;
                    }
                }
            }
            return ret;
        }

        #region Get Product or Session from main_object
         public IProduct GetProduct(object main_object, string checkItemType)
        {
            if (main_object == null)
            {
                throw new FisException("Can not get Product object in " + checkItemType + " module (main_object is null)");
            }
           // string objType = main_object.GetType().ToString();
            IMES.Infrastructure.Session session = null;
            IProduct iprd = null;
            if (main_object.GetType().Equals(typeof(IMES.FisObject.FA.Product.Product)))
            {
                iprd = (IProduct)main_object;
            }
            else if (main_object.GetType().Equals(typeof(IMES.Infrastructure.Session)))
            {
                session = (IMES.Infrastructure.Session)main_object;
                iprd = (IProduct)session.GetValue(Session.SessionKeys.Product);
            }

            if (iprd == null)
            {
                throw new FisException("Can not get Product object in " + checkItemType + " module");
            }
            return iprd;
        }


         public Session GetSession(object main_object, string checkItemType)
        {
            if (main_object == null)
            {
                throw new FisException("Can not get Session instance from SessionManager in " + checkItemType + " module (main_object is null)");
            }
           // string objType = main_object.GetType().ToString();
            IMES.Infrastructure.Session session = null;
          
            if (main_object.GetType().Equals(typeof(IMES.FisObject.FA.Product.Product)))
            {
                IProduct  iprd = (IProduct)main_object;
                session = SessionManager.GetInstance.GetSession(iprd.ProId, Session.SessionType.Product);
                if (session == null && !string.IsNullOrEmpty(iprd.CUSTSN))
                {
                    session = SessionManager.GetInstance.GetSession(iprd.CUSTSN, Session.SessionType.Product);
                }
            }
            else if (main_object.GetType().Equals(typeof(IMES.Infrastructure.Session)))
            {
                session = (IMES.Infrastructure.Session)main_object;
               
            }        
            
            if (session == null)
            {
                throw new FisException("Can not get Session instance from SessionManager in " + checkItemType + " module");
            }
            return session;
        }

        public Session GetSession(string key, Session.SessionType sessionType, string checkItemType)
        {
            Session session = SessionManager.GetInstance.GetSession(key, sessionType);
            if (session == null)
            {
                throw new FisException("Can not get Session instance from SessionManager in " + checkItemType + " module");
            }
            return session;
        }
		
        #endregion

        /// <summary>
        /// Check Null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public T IsNull<T>(Session session, string name)
        {
            object ret = session.GetValue(name);
            if (ret == null)
            {
                throw new FisException("CQCHK0006", new List<string> { name });
            }

            return (T)ret;
        }

        /// <summary>
        /// Get SysSetting Data 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetSysSetting(string name, string defaultValue)
        {
            IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<string> valueList = PartRepository.GetValueFromSysSettingByName(name);
            if (defaultValue == null)
            {
                if (valueList.Count == 0)
                {
                    throw new FisException("PAK095", new string[] { name });
                }
                else
                {
                    return valueList[0].Trim();
                }
            }
            else
            {
                return valueList.Count == 0 ? defaultValue : valueList[0].Trim();
            }
        }

        /// <summary>
        /// GetConstValueListByType
        /// </summary>
        public IList<ConstValueInfo> GetConstValueListByType(string type, string orderby)
        {
            IList<ConstValueInfo> retLst = new List<ConstValueInfo>();
            try
            {
                if (!String.IsNullOrEmpty(type))
                {
                    //IPartRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                    var resultLst = partRepository.GetConstValueListByType(type);
                    if (!string.IsNullOrEmpty(orderby))
                    {
                        var tmpLst = from item in resultLst orderby item.id select item;
                        retLst = tmpLst.ToList();
                    }
                    else
                    {
                        retLst = resultLst;
                    }
                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public IList<ConstValueTypeInfo> GetConstValueTypeByType(string type)
        {
            //IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<ConstValueTypeInfo> constValueTypeList = partRepository.GetConstValueTypeList(type);
            return constValueTypeList;
        }

        public int GetTestKPCountLimit(string partType, string station, int defaultValue)
        {
            int limitTestCount = int.Parse(GetSysSetting(GlobalConstName.SysSetting.OverTestKPCountLimit, defaultValue.ToString()));
            IList<ConstValueInfo> testContLimitList = GetConstValueListByType(GlobalConstName.ConstValue.OverTestKPCountLimit, null);
            if (testContLimitList != null || testContLimitList.Count > 0)
            {
                ConstValueInfo valueInfo = null;
                valueInfo = testContLimitList.Where(x => Regex.IsMatch(partType, x.name)).FirstOrDefault();  //first check Model
                if (valueInfo != null)
                {
                    limitTestCount = int.Parse(valueInfo.value);
                }
                else
                {
                    valueInfo = testContLimitList.Where(x => Regex.IsMatch(station, x.name)).FirstOrDefault(); //second check family
                    if (valueInfo != null)
                    {
                        limitTestCount = int.Parse(valueInfo.value);
                    }                   
                }
            }

            return limitTestCount;
        }

        public IList<QtyParts> FilterQtyParts(object main_object, string part_check_type, IList<QtyParts> lstQtyParts, string station)
        {
            IList<CheckItemTypeRuleDef> lstCheckItemTypeRuleDef = GetCheckItemTypeRule(main_object, part_check_type, station);
            Session currentSession = GetSession(main_object, part_check_type);
            IList<QtyParts> ret = new List<QtyParts>();
            if (lstQtyParts == null)
            { return ret; }
            foreach (CheckItemTypeRuleDef chk in lstCheckItemTypeRuleDef)
            {
                string[] keyArr = chk.PartDescr.Split( GlobalConstName.CommaChar);
                foreach (QtyParts q in lstQtyParts)
                {
                    var lstPart = q.Parts.Where(x => keyArr.Contains(x.Descr)).ToList();
                    if (lstPart != null && lstPart.Count > 0)
                    {
                        QtyParts qtPart = new QtyParts(q.Qty, lstPart);
                        ret.Add(qtPart);
                        foreach (IPart part in lstPart)
                        {
                            currentSession.AddValue(part_check_type + "Regx" + part.Descr, chk.MatchRule);
                        }

                        //   currentSession.AddValue(part_check_type + "Regx" + s., chk.MatchRule);
                    }

                }
            }
            return ret;
        }

        public IList<CheckItemTypeRuleDef> GetCheckItemTypeRule(object main_object, string part_check_type, string station)
        {
            IList<CheckItemTypeRuleDef> lstCheckItemTypeRuleDef = null;
            Session currentSession = GetSession(main_object,part_check_type);
            if (string.IsNullOrEmpty(station))
                station = currentSession.Station;
            //var bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            string line = currentSession.Line;
            if (line.Length > 1)
            {
                line = line.Substring(0, 1);
            }
            string model = (string)currentSession.GetValue(Session.SessionKeys.ModelName);
            if (string.IsNullOrEmpty(model))
            {
                IProduct currentProduct = currentSession.GetValue(Session.SessionKeys.Product) as IProduct;
                if (currentProduct != null)
                {
                    model = currentProduct.Model;
                }
            }
            string family = (string)currentSession.GetValue(Session.SessionKeys.FamilyName);
            if (string.IsNullOrEmpty(family))
            {
                if (!string.IsNullOrEmpty(model))
                {
                   // IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                    family = modelRep.Find(model).FamilyName;
                }

            }

            IList<CheckItemTypeRuleDef> lst =
                bomRepository.GetCheckItemTypeRuleWithPriority(part_check_type, line, station, family);
            if (lst.Count > 0)
            {
                var min = lst.Select(p => p.Priority).Min();
                lstCheckItemTypeRuleDef = lst.Where(p => p.Priority == min).ToList();
            }
            return lstCheckItemTypeRuleDef;
        }

        public IList<CheckItemTypeRuleDef> GetCheckItemTypeRuleByRegex(object main_object, string part_check_type, string station)
        {
            IList<CheckItemTypeRuleDef> lstCheckItemTypeRuleDef = null;
            Session currentSession = GetSession(main_object, part_check_type);
            if (string.IsNullOrEmpty(station))
                station = currentSession.Station;            
            string line = currentSession.Line;
            if (line.Length > 1)
            {
                line = line.Substring(0, 1);
            }
            string model = (string)currentSession.GetValue(Session.SessionKeys.ModelName);
            if (string.IsNullOrEmpty(model))
            {
                IProduct currentProduct = currentSession.GetValue(Session.SessionKeys.Product) as IProduct;
                if (currentProduct != null)
                {
                    model = currentProduct.Model;
                }
            }
            string family = (string)currentSession.GetValue(Session.SessionKeys.FamilyName);
            if (string.IsNullOrEmpty(family))
            {
                if (!string.IsNullOrEmpty(model))
                {                 
                    family = modelRep.Find(model).FamilyName;
                }

            }

            IList<CheckItemTypeRuleDef> lst =
                bomRepository.GetCheckItemTypeRuleByItemType(part_check_type);
            if (lst.Count > 0)
            {
                #region 過濾條件Line/Station/regex Family
                lst = lst.Where(x => getCheckItemTypeRulePriorityByRegex(x, line, station, family)).ToList();
                if (lst.Count > 0)
                {
                    var min = lst.Select(p => p.Priority).Min();
                    lstCheckItemTypeRuleDef = lst.Where(p => p.Priority == min).ToList();
                }
                #endregion
            }
            return lstCheckItemTypeRuleDef;
        }

        #region CheckItemTypeRule Priority rule

        private bool getCheckItemTypeRulePriorityByRegex(CheckItemTypeRuleDef rule, string line, string station, string family)
        {
            bool isLine = !string.IsNullOrEmpty(rule.Line);
            bool isStation = !string.IsNullOrEmpty(rule.Station);
            bool isFamily = !string.IsNullOrEmpty(rule.Family);

            bool matchFamily = (isFamily ? Regex.IsMatch(family, rule.Family) : family == rule.Family);

            if (rule.Line == line &&
               rule.Station == station &&
               matchFamily)
            {
                rule.Priority = 1;
                return true;
            }


            if (!isFamily &&
               rule.Line == line &&
               rule.Station == station)
            {
                rule.Priority = 2;
                return true;
            }

            if (!isStation &&
              rule.Line == line &&
              matchFamily)
            {
                rule.Priority = 3;
                return true;
            }

            if (!isLine &&
               rule.Station == station &&
              matchFamily)
            {
                rule.Priority = 4;
                return true;
            }

            if (!isStation && !isFamily &&
              rule.Line == line)
            {
                rule.Priority = 5;
                return true;
            }

            if (!isLine && !isFamily &&
                  rule.Station == station)
            {
                rule.Priority = 6;
                return true;
            }

            if (!isLine && !isStation && matchFamily)
            {
                rule.Priority = 7;
                return true;
            }

            if (!isLine && !isStation && !isFamily)
            {
                rule.Priority = 8;
                return true;
            }

            return false;

        }
        #endregion

        public bool CheckByRegexGroup(string rule, string subject, Hashtable hash)
        {
            Regex regex = new Regex(rule, RegexOptions.Compiled); //, RegexOptions.ExplicitCapture);
            Match match = regex.Match(subject);
            //if (regex.IsMatch(subject))
            if (match.Success)
            {
                GroupCollection groups = match.Groups;

                foreach (string groupName in regex.GetGroupNames())
                {
                    if (!string.IsNullOrEmpty(groupName))
                    {
                        hash.Add(groupName, groups[groupName].Value);
                    }
                }

                return true;
            }
            return false;
        }

        public bool CheckGroupNameByDescr(Hashtable hash, string descr, string checkItemType)
        {
            if (!string.IsNullOrEmpty(descr))
            {
                string[] aryGrpName = descr.Split(GlobalConstName.CommaChar);

                foreach (string grpName in aryGrpName)
                {
                    if (!hash.ContainsKey(grpName))
                    {
                        // CheckItemTypeRule[@CheckItemType] group name維護有誤!
                        //throw new FisException("CHK1125", new string[] { checkItemType });
                        return false;
                    }
                }

                return true;
            }
            return true;
        }

        public IList<IPart> VerifyGroupData_PartInfoCommon(FlatBOMItem bomItem, IList<IPart> parts, string subject, string infoType, string groupName)
        {
            IList<IPart> retParts = new List<IPart>();
            if (null == parts || parts.Count == 0)
                parts = ((FlatBOMItem)bomItem).AlterParts;
            if (parts != null)
            {
                foreach (IPart part in parts)
                {
                    IList<PartInfo> part_infos = part.Attributes;
                    if (part_infos != null)
                    {
                        foreach (PartInfo part_info in part_infos)
                        {
                            if (part_info.InfoType.Equals(infoType) && part_info.InfoValue == subject)
                            {
                                retParts.Add(part);
                                break;
                            }
                        }
                    }
                }
            }

            return retParts;
        }

        public IList<IPart> VerifyGroupData_DotPartInfoCommon(FlatBOMItem bomItem, IList<IPart> parts, string subject, string infoType)
        {
            IList<IPart> retParts = new List<IPart>();
            if (null == parts || parts.Count == 0)
                parts = ((FlatBOMItem)bomItem).AlterParts;
            if (parts != null)
            {
                foreach (IPart part in parts)
                {
                    IList<PartInfo> part_infos = part.Attributes;
                    if (part_infos != null)
                    {
                        foreach (PartInfo part_info in part_infos)
                        {
                            if (part_info.InfoType.Equals(infoType) && part_info.InfoValue.Replace(GlobalConstName.DotStr,string.Empty) == subject)
                            {
                                retParts.Add(part);
                                break;
                            }
                        }
                    }
                }
            }

            return retParts;
        }

        public IList<IPart> VerifyGroupData_PCB_MB(FlatBOMItem bomItem, IList<IPart> parts, string pcbno, string groupName, string ct)
        {
            FisException fex;
            //IMBRepository mbRep = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
            /*IMB mb = mbRep.Find(pcbno) as IMB;
            if (mb == null)
            {
                fex = new FisException("CHK174", new string[] { ct });
                fex.stopWF = false;
                throw fex;
            }*/

            IList<IPart> retParts = new List<IPart>();
            if (null == parts || parts.Count == 0)
                parts = ((FlatBOMItem)bomItem).AlterParts;
            if (parts != null)
            {
                foreach (IPart part in parts)
                {
                    IList<PartInfo> part_infos = part.Attributes;
                    if (part_infos != null)
                    {
                        foreach (PartInfo part_info in part_infos)
                        {
                            if (part_info.InfoType.Equals(GlobalConstName.FisObject.MB))
                            {
                                string mbcode = part_info.InfoValue;
                                if (pcbno.Length >= 2 && pcbno.Substring(0, 2) == mbcode)
                                {
                                    retParts.Add(part);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (retParts.Count == 0)
            {
                fex = new FisException("CHK174", new string[] { ct });
                fex.stopWF = false;
                throw fex;
            }
            
            return retParts;
        }

        public bool VerifyGroupData_PCB_ECR(string pcbno, string ecr)
        {
            //IMBRepository mbRep = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
            IMB mb = mbRep.Find(pcbno) as IMB;
            if (mb == null)
            {
                return false;
            }
            if (mb.ECR != ecr)
                return false;
            return true;
        }

        public void BlockStationMB(Session currentSession, string pcbno, string station)
        {
            //IProcessRepository CurrentProcessRepository = RepositoryFactory.GetInstance().GetRepository<IProcessRepository, Process>();
            currentProcessRepository.SFC(currentSession.Line, currentSession.Customer, station, pcbno, GlobalConstName.FisObject.MB);
        }

        public bool CheckByItemTypeRule(IPart part, IList<CheckItemTypeRuleDef> lstChkItemRule, out IList<CheckItemTypeRuleDef> filteredChkItemRules)
        {
            filteredChkItemRules = new List<CheckItemTypeRuleDef>();

            if (null == part)
                return false;
            foreach (CheckItemTypeRuleDef chkItemRule in lstChkItemRule)
            {
                if (part.BOMNodeType == chkItemRule.BomNodeType)
                {
                    if (!string.IsNullOrEmpty(chkItemRule.PartDescr))
                    {
                        if (null == part.Descr || part.Descr.IndexOf(chkItemRule.PartDescr) != 0)
                            continue;
                    }

                    if (!string.IsNullOrEmpty(chkItemRule.PartType))
                    {
                        if (null == part.Type || part.Type.IndexOf(chkItemRule.PartType) != 0)
                            continue;
                    }

                    filteredChkItemRules.Add(chkItemRule);
                    return true;
                }
            }
            return false;
        }

        public void AddSharedParts(IPart part, Hashtable share_parts_set, Hashtable descr_kp_part_set, string infoType)
        {
           // IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
            IList<IPart> share_parts = (IList<IPart>)share_parts_set[part.Descr];
            string _partNoItem = (string)descr_kp_part_set[part.Descr];
            if (null != part.Attributes)
            {
                string subValue = part.Attributes.Where(x => x.InfoType == infoType).Select(y => y.InfoValue).FirstOrDefault();
                if (!string.IsNullOrEmpty(subValue))
                {
                    string[] lstSub = subValue.Split(GlobalConstName.CommaChar );
                    foreach (string partNo in lstSub)
                    {
                        IPart subPart = partRepository.Find(partNo);
                        if (null != subPart)
                        {
                            share_parts.Add(subPart);
                            _partNoItem += GlobalConstName.CommaStr + partNo;
                        }
                    }
                    descr_kp_part_set[part.Descr] = _partNoItem;
                }
            }
        }

        /// <summary>
        /// 0003108: [Rework]BoardInput站增加Check PCBLog功能
        /// </summary>
        /// <param name="session"></param>
        /// <param name="pcbno"></param>
        public void CheckReworkMBLogByFamily(Session session, string pcbno)
        {
            IProduct product = (IProduct)session.GetValue(Session.SessionKeys.Product);
            var allowStationList = GetConstValueListByType(GlobalConstName.ConstValue.CheckAllowPCBLogByFamily,null);
            var reworkStationList = GetConstValueTypeByType(GlobalConstName.ConstValueType.JudgeMBReworkStationList);
            if (allowStationList != null && 
                allowStationList.Count > 0 &&
                reworkStationList!=null &&
                reworkStationList.Count >0 &&
                product!=null)
            {
                string family = product.Family;
                string stationList = allowStationList.Where(x => x.name == family).Select(y => y.value).FirstOrDefault();
                if (!string.IsNullOrEmpty(stationList))
                {
                    
                    //IMBRepository mbRep = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                    IMB mb = mbRep.Find(pcbno) as IMB;
                    //Check 是否重工MB
                    IList<string> mbReworkStation = reworkStationList.Select(x => x.value).ToList();
                    if (mb != null &&  
                        mb.MBLogs.Any(x => mbReworkStation.Contains(x.StationID)))
                    {
                        string[] stations = stationList.Split(new char[] { GlobalConstName.TildeChar, GlobalConstName.CommaChar });
                        if ( !mb.MBLogs.Any(x => stations.Contains(x.StationID)))
                        {
                            throw new FisException("CHK1234", new string[] { pcbno, stationList });
                        }
                    }
                }                
            }
        }
        
        #region Vincent create Common Filter and Match rule
        public class BomFilterRule
        {
            private const string CheckItemTypeRuleName = "CheckItemTypeRule_";
            /// <summary>
            /// Check Input subject by Regular Expression 
            /// </summary>
            /// <param name="ruelInfo"></param>
            /// <param name="inputStr"></param>
            /// <param name="bomCT"></param>
            /// <param name="groupName"></param>
            /// <returns></returns>
            public bool CheckSubjectByRegexGroup(CheckItemTypeRuleDef ruelInfo,
                                                                    string inputStr,
                                                                    string bomCT,
                                                                    string groupName)
            { 
                if (!string.IsNullOrEmpty(ruelInfo.MatchRule) &&
                    !string.IsNullOrEmpty(groupName))
                {
                    if (bomCT == null) return false;
                    Match match = Regex.Match(inputStr, ruelInfo.MatchRule, RegexOptions.Compiled);
                    if (match.Success &&
                        match.Groups != null &&
                        match.Groups.Count > 0)
                    {
                        Group group = match.Groups[groupName];
                        if (group != null &&
                            group.Success)
                        {
                            string ct = group.Value;
                            if (bomCT.Trim() == ct)
                            {
                                return true;
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(ruelInfo.MatchRule))
                {
                    return Regex.IsMatch(inputStr, ruelInfo.MatchRule);
                }
                return false;
            }

            /// <summary>
            /// Check Input subject by Regular Expression 
            /// </summary>
            /// <param name="rule"></param>
            /// <param name="inputStr"></param>
            /// <param name="bomCT"></param>
            /// <param name="groupName">null is none check group name</param>
            /// <returns></returns>
            public bool CheckSubjectByRegexGroup(string rule,
                                                                  string inputStr,
                                                                  string bomCT,
                                                                  string groupName)
            {
                if (!string.IsNullOrEmpty(rule) &&
                    !string.IsNullOrEmpty(groupName))
                {
                    if (bomCT == null) return false;

                    Match match = Regex.Match(inputStr, rule, RegexOptions.Compiled);
                    if (match.Success &&
                        match.Groups != null &&
                        match.Groups.Count > 0)
                    {
                        Group group = match.Groups[groupName];
                        if (group != null &&
                            group.Success)
                        {
                            string ct = group.Value;
                            if (bomCT.Trim() == ct)
                            {
                                return true;
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(rule))
                {
                    return Regex.IsMatch(inputStr, rule);
                }
                return false;
            }


            /// <summary>
            /// Check Input subject by Regular Expression 
            /// </summary>
            /// <param name="rule"></param>
            /// <param name="inputStr"></param>
            /// <param name="groupName">null is none check group name</param>
            /// <returns></returns>
            public bool CheckSubjectByRegexGroup(string rule,
                                                                  string inputStr,
                                                                  string groupName)
            {
                if (!string.IsNullOrEmpty(rule) &&
                    !string.IsNullOrEmpty(groupName))
                {

                    Match match = Regex.Match(inputStr, rule, RegexOptions.Compiled);
                    if (match.Success &&
                        match.Groups != null &&
                        match.Groups.Count > 0)
                    {
                        Group group = match.Groups[groupName];
                        if (group != null &&
                            group.Success)
                        {
                            return !string.IsNullOrEmpty(group.Value);
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(rule))
                {
                    return Regex.IsMatch(inputStr, rule);
                }
                return false;
            }


            public bool GetSubjectByRegexGroup(CheckItemTypeRuleDef ruelInfo,
                                                                    string inputStr,
                                                                    string groupName,
                                                                    out string ctValue)
            {

                ctValue = null;
                if (!string.IsNullOrEmpty(ruelInfo.MatchRule) &&
                    !string.IsNullOrEmpty(groupName))
                {
                    Match match = Regex.Match(inputStr, ruelInfo.MatchRule, RegexOptions.Compiled);
                    if (match.Success &&
                        match.Groups != null &&
                        match.Groups.Count > 0)
                    {
                        Group group = match.Groups[groupName];
                        if (group != null &&
                            group.Success)
                        {
                            ctValue = group.Value;
                            return true;
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(ruelInfo.MatchRule))
                {
                    if (Regex.IsMatch(inputStr, ruelInfo.MatchRule))
                    {
                        ctValue = inputStr;
                        return true;
                    }
                }
                return false;
            }

            public bool GetSubjectByRegexGroup(string matchRule,
                                                                   string inputStr,
                                                                   string groupName,
                                                                   out string ctValue)
            {

                ctValue = null;
                if (!string.IsNullOrEmpty(matchRule) &&
                    !string.IsNullOrEmpty(groupName))
                {
                    Match match = Regex.Match(inputStr, matchRule, RegexOptions.Compiled);
                    if (match.Success &&
                        match.Groups != null &&
                        match.Groups.Count > 0)
                    {
                        Group group = match.Groups[groupName];
                        if (group != null &&
                            group.Success)
                        {
                            ctValue = group.Value;
                            return true;
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(matchRule))
                {
                    if (Regex.IsMatch(inputStr, matchRule))
                    {
                        ctValue = inputStr;
                        return true;
                    }
                }
                return false;
            }

            public IList<string> GetRegexGroupName(string expression)
            {
                IList<string> ret = new List<string>();
                //string groupNamePattern = @"\(\?<(?<name>.+?)>.+?\)";

                MatchCollection mc = Regex.Matches(expression, GlobalConstName.RegEx.GroupNamePattern, RegexOptions.Compiled);

                foreach (Match m in mc)
                {
                    Group group = m.Groups[GlobalConstName.RegEx.GroupName];
                    if (group.Success)
                    {
                        ret.Add(group.Value);
                    }
                }

                return ret;
            }

            public void ParseCheckItemTypeRuleGroupName(CheckItemTypeRuleDef chkRule)
            {
                string matchRule = chkRule.MatchRule;
                chkRule.MatchRuleGroupNames = getGroupNameWithObjectFormat(ref matchRule);
                chkRule.MatchRule = matchRule;

                string checkRule = chkRule.CheckRule;
                chkRule.CheckRuleGroupNames = getGroupNameWithObjectFormat(ref checkRule);
                chkRule.CheckRule = checkRule;

                string saveRule = chkRule.SaveRule;
                chkRule.SaveRuleGroupNames = getGroupNameWithObjectFormat(ref saveRule);
                chkRule.SaveRule = saveRule;

            }

            private IDictionary<string, bool> getGroupNameWithObjectFormat(ref string expression)
            {
                IDictionary<string, bool> ret = new Dictionary<string, bool>();
                if (!string.IsNullOrEmpty(expression))
                {
                    IList<string> nameList = GetRegexGroupName(expression);
                    foreach (string name in nameList)
                    {
                        bool isObjectFormat = false;
                        string groupName = name;
                        if (name.Contains(GlobalConstName.DotStr))
                        {
                            groupName = name.Replace(GlobalConstName.DotStr, GlobalConstName.UnderScoreStr);
                            expression = expression.Replace(name, groupName);
                            isObjectFormat = true;
                        }

                        if (ret.ContainsKey(groupName))
                        {
                            ret[groupName] = isObjectFormat;
                        }
                        else
                        {
                            ret.Add(groupName, isObjectFormat);
                        }
                    }
                }
                return ret;
            }           

            public bool MatchCheckItemTypeRule(IPart part,
                                                                     IList<CheckItemTypeRuleDef> lstChkItemRule,
                                                                     out IList<CheckItemTypeRuleDef> filteredChkItemRules)
            {
                filteredChkItemRules = new List<CheckItemTypeRuleDef>();

                if (null == part)
                    return false;

                foreach (CheckItemTypeRuleDef chkItemRule in lstChkItemRule)
                {

                    if (!CheckBomNodeType(part.BOMNodeType, chkItemRule.BomNodeType))
                    {
                        continue;
                    }
                    if (!CheckPartTypeAndDecr(part.Descr, chkItemRule.PartDescr))
                    {
                        continue;
                    }
                    if (!CheckPartTypeAndDecr(part.Type, chkItemRule.PartType))
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(chkItemRule.FilterExpression))
                    {
                        if (!ResolveExpression.InvokeCondition(part, CheckItemTypeRuleName + chkItemRule.ID.ToString(), chkItemRule.FilterExpression, true))
                        {
                            continue;
                        }
                    }

                    filteredChkItemRules.Add(chkItemRule);

                }
                if (filteredChkItemRules.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }


            public bool MatchCheckItemTypeRule(IProduct product,
                                                                    IPart part,
                                                                     CheckItemTypeRuleDef chkItemRule,
                                                                     string bomNodeType)
            {

                if (null == part)
                    return false;


                if (!CheckBomNodeType(part.BOMNodeType, bomNodeType))
                {
                    return false;
                }
                if (!CheckPartTypeAndDecr(part.Descr, chkItemRule.PartDescr))
                {
                    return false;
                }
                if (!CheckPartTypeAndDecr(part.Type, chkItemRule.PartType))
                {
                    return false;
                }
                if (!string.IsNullOrEmpty(chkItemRule.FilterExpression))
                {
                    if (!ResolveExpression.InvokeCondition(product, part, CheckItemTypeRuleName + chkItemRule.ID.ToString(), chkItemRule.FilterExpression, true))
                    {
                        return false;
                    }
                }
                return true;
            }

            public bool MatchCheckItemTypeRule(IPart part,
                                                                     CheckItemTypeRuleDef chkItemRule)
            {

                if (null == part)
                    return false;


                if (!CheckBomNodeType(part.BOMNodeType, chkItemRule.BomNodeType))
                {
                    return false;
                }
                if (!CheckPartTypeAndDecr(part.Descr, chkItemRule.PartDescr))
                {
                    return false;
                }
                if (!CheckPartTypeAndDecr(part.Type, chkItemRule.PartType))
                {
                    return false;
                }

                if (!string.IsNullOrEmpty(chkItemRule.FilterExpression))
                {
                    if (!ResolveExpression.InvokeCondition(part, CheckItemTypeRuleName + chkItemRule.ID.ToString(), chkItemRule.FilterExpression, true))
                    {
                        return false;
                    }
                }

                return true;
            }

            public bool CheckBomNodeType(string input, string pattern)
            {
                //if (string.IsNullOrEmpty(pattern))
                //{
                //    return true;
                //}
                return input == pattern;
            }

            public bool CheckPartTypeAndDecr(string input,
                                                          string pattern)
            {
                if (string.IsNullOrEmpty(pattern))
                {
                    return true;
                }
                return Regex.IsMatch(input, pattern);
            }

            /// <summary>
            /// Return FlatBOMItem by CheckItemTypeRule
            /// </summary>
            /// <param name="bom"></param>
            /// <param name="ruelInfo"></param>
            /// <param name="product"></param>
            /// <param name="checkType"></param>
            /// <returns></returns>
            public FlatBOMItem GetFlatBomItemByCheckItemTypeRule(IHierarchicalBOM bom, CheckItemTypeRuleDef ruelInfo,
                                                                                                IProduct product, string checkType)
            {
                FlatBOMItem flatBomItem = null;
                IList<IBOMNode> bomNodeList =
                   ResolveValue.GetBom(bom, ruelInfo.BomNodeType, ruelInfo.PartType, ruelInfo.PartDescr, null);
                if (bomNodeList == null || bomNodeList.Count == 0)
                {
                    return flatBomItem;
                }
                else
                {
                    var partNoList = string.Join(GlobalConstName.DotStr, bomNodeList.Select(x => x.Part.PartNo).ToList().ToArray());
                    if (bomNodeList.Select(x => x.Qty).Distinct().ToList().Count > 1)
                    {
                        throw new FisException("Qty is not same in part list :" + partNoList);
                    }

                    //Check FilterExpression
                    if (!string.IsNullOrEmpty(ruelInfo.FilterExpression))
                    {
                        bomNodeList = bomNodeList.Where(x => ResolveExpression.InvokeCondition(product, x.Part, CheckItemTypeRuleName + ruelInfo.ID.ToString(), ruelInfo.FilterExpression, true)).ToList();
                        if (bomNodeList == null || bomNodeList.Count == 0)
                        {
                            return flatBomItem;
                        }
                    }

                    flatBomItem = new FlatBOMItem(bomNodeList[0].Qty, checkType, bomNodeList.Select(x => x.Part).ToList());
                    flatBomItem.PartNoItem = partNoList;
                    flatBomItem.Descr = ruelInfo.PartDescr;
                    flatBomItem.CheckItemTypeRuleList = new List<CheckItemTypeRuleDef>() { ruelInfo };
                    flatBomItem.Tag = bomNodeList;
                    return flatBomItem;

                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="chkItemRule"></param>
            /// <param name="tagData"></param>
            /// <param name="part"></param>
            /// <param name="inputStr"></param>
            /// <param name="iecPN"></param>
            /// <returns></returns>
            public string GetCustomPN(CheckItemTypeRuleDef chkItemRule,
                                        TagData tagData,
                                         IPart part,
                                        string inputStr,
                                        out string iecPN)
            {
                string customPN = null;
                iecPN = null;
                if (!string.IsNullOrEmpty(chkItemRule.SaveRule) &&
                     (chkItemRule.MatchRuleGroupNames == null ||
                     chkItemRule.MatchRuleGroupNames.Count == 0))
                {
                    string[] nameList = chkItemRule.SaveRule.Split(GlobalConstName.CommaChar);
                    if (nameList[0].IndexOf(GlobalConstName.DotChar) > 0)  //CustomPn case
                    {
                        customPN = ResolveValue.GetValueWithoutError(tagData.Product, tagData.PCB, tagData.DN, part, nameList[0], GlobalConstName.DotChar);
                    }
                    else if (!GetSubjectByRegexGroup(chkItemRule, inputStr, nameList[0], out customPN))
                    {
                        customPN = null;
                    }

                    if (nameList.Length > 1)   // for IecPN
                    {
                        if (nameList[1].IndexOf(GlobalConstName.DotChar) > 0)
                        {
                            iecPN = ResolveValue.GetValueWithoutError(tagData.Product, tagData.PCB, tagData.DN, part, nameList[1], GlobalConstName.DotChar);
                        }
                        else if (!GetSubjectByRegexGroup(chkItemRule, inputStr, nameList[1], out iecPN))
                        {
                            iecPN = null;
                        }

                    }
                }
                else
                {
                    customPN = part.Attributes.Where(x => x.InfoType == GlobalConstName.PartInfo.VCODE || x.InfoType == GlobalConstName.PartInfo.VC)
                                                   .Select(x => x.InfoValue).FirstOrDefault();
                    iecPN = part.Attributes.Where(x => x.InfoType == GlobalConstName.PartInfo.RDESC)
                                                   .Select(x => x.InfoValue).FirstOrDefault();
                }

                return customPN;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="chkItemRule"></param>
            /// <param name="subject"></param>
            /// <param name="tagData"></param>
            /// <param name="part"></param>
            /// <param name="customer"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public bool CheckSubjectByMatchRule(CheckItemTypeRuleDef chkItemRule,
                                                    string subject,
                                                   TagData tagData,
                                                    IPart part,
                                                    string customer,
                                                    out string value)
            {
                value = null;

                if (chkItemRule.MatchRuleGroupNames == null)
                {
                    return true;
                }

                // mantis 3605
                IList<ConstValueInfo> infoListRemoveSymbol = new List<ConstValueInfo>();
                if (!string.IsNullOrEmpty(customer))
                {
                    infoListRemoveSymbol = _Instance.GetConstValueListByType(string.Format(GlobalConstName.ConstValue.RemoveDataSymbol, customer) ,null);
                }

                foreach (KeyValuePair<string, bool> keyValue in chkItemRule.MatchRuleGroupNames)
                {
                    string name = keyValue.Key;
                    string input = null;
                    if (keyValue.Value) //object type format
                    {
                        if (tagData.GroupValueList.ContainsKey(name))
                        {
                            input = (string)tagData.GroupValueList[name];
                        }

                        if (string.IsNullOrEmpty(input))
                        {
                            input = ResolveValue.GetValueWithoutError(tagData.Product, tagData.PCB, tagData.DN, part, name, GlobalConstName.UnderScoreChar);
                        }

                        string oriName = name.Replace(GlobalConstName.UnderScoreStr, GlobalConstName.DotStr);
                        string removeSymbol = infoListRemoveSymbol.Where(x => x.name == oriName).Select(y => y.value).FirstOrDefault();
                        if (!string.IsNullOrEmpty(removeSymbol))
                        {
                            input = input.Replace(removeSymbol, string.Empty);
                        }

                        if (!CheckSubjectByRegexGroup(chkItemRule.MatchRule, subject, input, name))
                        {
                            return false;
                        }
                    }
                    else // none object type
                    {
                        if (!GetSubjectByRegexGroup(chkItemRule, subject, name, out value))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="chkItemRule"></param>
            /// <param name="tagData"></param>
            /// <param name="part"></param>
            /// <param name="subject"></param>
            /// <returns></returns>
            public string GetPartSNBySaveRule(CheckItemTypeRuleDef chkItemRule,
                                                        TagData tagData,
                                                        IPart part,
                                                       string subject)
            {
                string value = null;
                if (string.IsNullOrEmpty(chkItemRule.SaveRule) ||
                    chkItemRule.SaveRuleGroupNames == null)
                {
                    return subject;
                }
                else
                {
                    foreach (KeyValuePair<string, bool> keyValue in chkItemRule.SaveRuleGroupNames)
                    {
                        string name = keyValue.Key;
                        string input = null;
                        if (keyValue.Value) //object type format
                        {
                            if (tagData.GroupValueList.ContainsKey(name))
                            {
                                input = (string)tagData.GroupValueList[name];
                            }

                            if (string.IsNullOrEmpty(input))
                            {
                                input = ResolveValue.GetValueWithoutError(tagData.Product, tagData.PCB, tagData.DN, part, name, GlobalConstName.UnderScoreChar);
                            }

                            if (!string.IsNullOrEmpty(input))
                            {
                                return input;
                            }
                        }
                        else // none object type
                        {
                            if (GetSubjectByRegexGroup(chkItemRule.SaveRule, subject, name, out value))
                            {
                                return value;
                            }

                        }
                    }
                }
                return subject;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="chkItemRule"></param>
            /// <param name="tagData"></param>
            /// <param name="part"></param>
            /// <param name="subject"></param>
            /// <returns></returns>
            public bool CheckSubjectByCheckRule(CheckItemTypeRuleDef chkItemRule,
                                                        TagData tagData,
                                                        IPart part,
                                                       string subject)
            {
                //string value = null;
                if (string.IsNullOrEmpty(chkItemRule.CheckRule))
                {
                    return true;
                }
                else
                {
                    if (chkItemRule.CheckRuleGroupNames == null ||
                        chkItemRule.CheckRuleGroupNames.Count == 0)
                    {
                        return CheckSubjectByRegexGroup(chkItemRule.CheckRule, subject, string.Empty, null);
                    }

                    foreach (KeyValuePair<string, bool> keyValue in chkItemRule.CheckRuleGroupNames)
                    {
                        string name = keyValue.Key;
                        string input = null;
                        if (keyValue.Value) //object type format
                        {
                            if (tagData.GroupValueList.ContainsKey(name))
                            {
                                input = (string)tagData.GroupValueList[name];
                            }

                            if (string.IsNullOrEmpty(input))
                            {
                                input = ResolveValue.GetValueWithoutError(tagData.Product, tagData.PCB, tagData.DN, part, name, GlobalConstName.UnderScoreChar);
                            }

                            if (!CheckSubjectByRegexGroup(chkItemRule.CheckRule, subject, input, name))
                            {
                                return false;
                            }
                        }
                        else // none object type
                        {
                            //if (!GetSubjectByRegexGroup(chkItemRule.SaveRule, subject, name, out value))
                            if (!CheckSubjectByRegexGroup(chkItemRule.SaveRule, subject, name))
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }

        }
        #endregion
    }



}
