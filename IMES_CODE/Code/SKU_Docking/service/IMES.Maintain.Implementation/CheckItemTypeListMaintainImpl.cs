using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.FisObject.Common.FisBOM;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject.Common.Line;
using IMES.FisObject.Common.Station;
using System.Data;
using IMES.FisObject.Common.Misc;
using IMES.FisObject.Common.Part;
using IMES.Resolve.Common;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.TestLog;

namespace IMES.Maintain.Implementation
{
    public class CheckItemTypeListMaintainImpl : MarshalByRefObject, ICheckItemTypeListMaintain       
    {

        #region CheckItemTypeRule

        public IList<CheckItemTypeRuleDef> GetCheckItemTypeRuleWithPriority(string itemType, string line, string station, string family)
        {
            try
            {
                IBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                return itemRepository.GetCheckItemTypeRuleWithPriority(itemType, line, station, family);
            }
            catch (Exception)
            {
                throw;
            } 
        }

        public IList<CheckItemTypeRuleDef> GetCheckItemTypeRuleByItemType(string itemType)
        {
            try
            {
                IBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                return itemRepository.GetCheckItemTypeRuleByItemType(itemType);
            }
            catch (Exception)
            {
                throw;
            } 
        }

        public IList<string> GetChechItemTypeList()
        {
            try
            {
                IBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                IList<string> reLst = itemRepository.GetChechItemTypeList();
                return reLst;
            }
            catch (Exception)
            {
                throw;
            } 
        }

        public bool CheckExistCheckItemTypeRule(string itemType, string line, string station, string family)
        {
            try
            {
                IBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                return itemRepository.CheckExistCheckItemTypeRule(itemType, line, station, family);
            }
            catch (Exception)
            {
                throw;
            } 
        }

        public bool CheckExistCheckItemTypeRule(CheckItemTypeRuleDef item)
        {
            try
            {
                IBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                return itemRepository.CheckExistCheckItemTypeRule(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddCheckItemTypeRule(CheckItemTypeRuleDef itemType)
        {
            try
            {
                IBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                itemRepository.AddCheckItemTypeRule(itemType);
            }
            catch (Exception)
            {
                throw;
            } 
        }

        public void DeleteCheckItemTypeRule(int id)
        {
            try
            {
                IBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                itemRepository.DeleteCheckItemTypeRule(id);
            }
            catch (Exception)
            {
                throw;
            } 
        }

        public void UpdateCheckItemTypeRule(CheckItemTypeRuleDef itemType)
        {
            try
            {
                IBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                itemRepository.UpdateCheckItemTypeRule(itemType);
            }
            catch (Exception)
            {
                throw;
            } 
        }

        public IList<string> GetAllAliasLine()
        {
            try
            {
                ILineRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository>();
                return itemRepository.GetAllAliasLine();
            }
            catch (Exception)
            {
                throw;
            } 
        }

        public DataTable GetStationInfoList()
        {
            try
            {
                IStationRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();

                return itemRepository.GetStationInfoList();
            }
            catch (Exception)
            {
                throw;
            } 
        }

        #endregion

        #region CheckItemTypeRuleExpression


        public IList<ConstValueInfo> GetConstValueList()
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                ConstValueInfo condition = new ConstValueInfo();
                condition.type = "ExpressionObject";
                return itemRepository.GetConstValueInfoList(condition);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckTextExpression(string objects, string objectType, string condition, string conditionType, string expression)
        {
            try
            {
                bool ret = false;
                if (objectType == "ProductID")
                {
                    var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                    IProduct prodcutObject = productRepository.Find(objects);
                    switch (conditionType)
                    {
                        case "":
                            ret = ResolveExpression.InvokeCondition(prodcutObject, "", expression, false);
                            break;
                        case "PartNo":
                            var partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                            IPart partObject = partRepository.Find(condition);
                            ret = ResolveExpression.InvokeCondition(prodcutObject, partObject, "", expression, false);
                            break;
                        case "DeliveryNo":
                            var deliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                            Delivery deliveryObject = deliveryRepository.Find(condition);
                            ret = ResolveExpression.InvokeCondition(prodcutObject, deliveryObject, "", expression, false);
                            break;
                        default:
                            break;
                    }

                }
                else if (objectType == "PCBNo")
                {
                    var mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                    IMB mbObject = mbRepository.Find(objects);
                    switch (conditionType)
                    {
                        case "":
                            ret = ResolveExpression.InvokeCondition(mbObject, "", expression, false);
                            break;
                        case "TestLog":
                            TestLog testLog = mbObject.TestLogs.OrderByDescending(x => x.Cdt).FirstOrDefault();
                            ret = ResolveExpression.InvokeCondition(mbObject, testLog, "", expression, false);
                            break;
                        default:
                            break;
                    }
                    return ret;
                }
                else if (objectType == "PartNo")
                {
                    var partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                    IPart partObject = partRepository.Find(objects);
                    switch (conditionType)
                    {
                        case "":
                            ret = ResolveExpression.InvokeCondition(partObject, "", expression, false);
                            break;
                        default:
                            break;
                    }
                    return ret;
                }
                else
                {
                    throw new Exception("objectType Error");
                }
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        public static String Null2String(Object _input)
        {
            if (_input == null)
            {
                return "";
            }
            return _input.ToString().Trim();
        }


    }
}
