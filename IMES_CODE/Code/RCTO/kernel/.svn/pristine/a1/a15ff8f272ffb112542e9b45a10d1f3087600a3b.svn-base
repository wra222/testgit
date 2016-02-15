using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Text.RegularExpressions;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
namespace IMES.CheckItemModule.Utility
{
    public class Uti
    {
        static public bool CheckCPUOnBoard(HierarchicalBOM bom)
        {
            string mbcode = "";
            string mbFamily = "";
            if (bom.FirstLevelNodes != null)
            {
                for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                {
                    if (bom.FirstLevelNodes.ElementAt(i).Part.BOMNodeType.Equals("MB"))
                    {
                        foreach (PartInfo pi in bom.FirstLevelNodes.ElementAt(i).Part.Attributes)
                        {
                            if ("MB".Equals(pi.InfoType))
                            {
                                mbcode = pi.InfoValue;
                                break;
                            }
                        }
                        mbFamily = bom.FirstLevelNodes.ElementAt(i).Part.Descr;
                        break;
                    }
                }
            }
            return
                 mbcode != "" && Uti.CheckCPUOnBoard("MBCode", mbcode)|| 
                 mbFamily != "" && Uti.CheckCPUOnBoard("Family", mbFamily);
         }


       static private bool CheckCPUOnBoard(string name, string checkValue)
        {
            IList<ConstValueInfo> lstConst = GetConstValueListByType("CPUOnBoard", "Name").
                                                                Where(x => x.value.Trim() != "").ToList();
            foreach (ConstValueInfo c in lstConst)
            {
                if (name.Equals(c.name))
                {
                    Regex regex = new Regex(c.value);
                    if (regex.IsMatch(checkValue))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

       static public IList<ConstValueInfo> GetConstValueListByType(string type, string orderby)
       {
           IList<ConstValueInfo> retLst = new List<ConstValueInfo>();
           try
           {
               if (!String.IsNullOrEmpty(type))
               {
                   IPartRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                   var resultLst = palletRepository.GetConstValueListByType(type);
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

       static public bool CheckModelByProcReg(string model, List<string> ProcRegNameList)
       {
           bool isPass = false;
           IList<ConstValueInfo> lstConst = GetConstValueListByType("ProcReg")
                                                            .Where(x => !string.IsNullOrEmpty(x.value) && ProcRegNameList.Contains(x.name)).ToList();



           foreach (ConstValueInfo c in lstConst)
           {
               if (Regex.IsMatch(model, c.value))
               {
                   isPass = true;
                   break;
               }
           }
           return isPass;
       }

       static public  IList<ConstValueInfo> GetConstValueListByType(string type)
       {
           IList<ConstValueInfo> retLst = new List<ConstValueInfo>();
           try
           {
               if (!String.IsNullOrEmpty(type))
               {
                   IPartRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                   retLst = palletRepository.GetConstValueListByType(type);
               }
               return retLst;
           }
           catch (Exception)
           {
               throw;
           }

       }


       static public IProduct GetProduct(object main_object,string partCheckType)
       {
           string objType = main_object.GetType().ToString();
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
               throw new FisException("Can not get Product object in " + partCheckType + " module");
           }
           return iprd;
       }


       static public Session GetSession(string key, Session.SessionType sessionType ,string partCheckType)
       {
           Session session = SessionManager.GetInstance.GetSession(key, sessionType);
           if (session == null)
           {
               throw new FisException("Can not get Session instance from SessionManager in " + partCheckType + " module");
           }
           return session;
       }

    }
}
