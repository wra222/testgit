using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using IMES.Infrastructure;
using IMES.DataModel;


using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.FisBOM;

namespace IMES.Maintain.Implementation
{
    public class ACAdaptorManager : MarshalByRefObject, IMES.Maintain.Interface.MaintainIntf.IACAdaptor
   {
       private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
 //       public static IList<ACAdaptor> adaptorLst = new List<ACAdaptor>();
        IBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
       #region IACAdaptor Members

       public IList<ACAdaptor> GetAllAdaptorInfo()
       {
           IList<ACAdaptor> adaptorItems = new List<ACAdaptor>();
           try 
           {
               adaptorItems = (IList<ACAdaptor>)itemRepository.GetAllACAdaptor();

            //调用repository 接口 ...
               //if (adaptorLst.Count==0)
               //{
               //    for (int i = 0; i < 10; i++)
               //    {
               //        ACAdaptor a = new ACAdaptor();
                       
               //        a.Assemb = "assemb" + i;
               //        a.Adppn = "adppn" + i;
               //        a.Agency = "agency" + i;
               //        a.Supplier = "supplier" + i;
               //        a.Voltage = "voltage" + i;
               //        a.Cur = "Current" + i;
               //        a.Editor = "chenguang" + i;
               //        a.Cdt = DateTime.Now.ToString();
               //        adaptorLst.Add(a);
               //    }
               //}
               
           }
           catch(FisException fex)
           {
               logger.Error(fex.Message);
               throw fex;
           }
           catch(System.Exception e)
           {
               logger.Error(e.Message);
               throw;
           }
           return adaptorItems;
       }

       public void DeleteOneAcAdaptor(ACAdaptor item)
       {
           try
           {
               //调用repository 删除接口 ...
               //foreach (ACAdaptor acinfo in adaptorLst)
               //{
               //    if (acinfo.Assemb==item.Assemb)
               //     {
               //         adaptorLst.Remove(acinfo);
               //         break;
               //     }
               //}
               itemRepository.DeleteSelectedACAdaptor(item.id);
           }
           catch (FisException fex)
           {
               logger.Error(fex.Message);
               throw fex;
           }
           catch (System.Exception e)
           {
               logger.Error(e.Message);
               throw;
           }
       }

       public string AddOneAcAdaptor(ACAdaptor item)
       {
           string id = "";
           try
           {
               FisException ex = null;
               //调用repository 添加接口 ...
        //       foreach(ACAdaptor ar in adaptorLst)
        //       {
        //           if (ar.assemb == item.assemb)
        //            {
                        
        //               List<string> param=new List<string>();
        //               ex=new FisException("DMT063",param);
        //               throw ex;
        //            }
        //       }
        ////       if (ex==null)
        //       {
        //        adaptorLst.Add(item);
        //       }
               IList<ACAdaptor> gradeLst = (IList<ACAdaptor>)GetAllAdaptorInfo();
               foreach(ACAdaptor aa in gradeLst)
               {
                    if(aa.assemb==item.assemb)
                    {
                        List<string> param = new List<string>();
                        ex = new FisException("DMT063", param);
                        throw ex;
                    }
               }
               item.cdt = DateTime.Now;

               itemRepository.AddOneAcAdaptor(item);
               id = item.id.ToString();
           }

           catch (FisException fex)
           {
               logger.Error(fex.Message);
               throw;
           }
           catch (System.Exception e)
           {
               logger.Error(e.Message);
               throw;
           }
           
           //catch (Exception e)
           //{
           //    logger.Error(e.Message);
           //    throw e;
           //}
           return id;
       }

       public void UpdateOneAcAdaptor(ACAdaptor newItem)
       {
           try
           {
               FisException ex = null;
               Boolean duplicateFlag = false, notExistFlag = true;
              
               IList<ACAdaptor> gradeLst = (IList<ACAdaptor>)GetAllAdaptorInfo();
               
                    foreach (ACAdaptor aa in gradeLst)
                    {
                        if (aa.id == newItem.id)
                        {
                            notExistFlag = false;
                        }
                       if (aa.id != newItem.id&&aa.assemb.Trim().ToLower()==newItem.assemb.Trim().ToLower())
                       {
                           duplicateFlag = true;
                        }
                    }
                    if (notExistFlag)
                    {
                        List<string> param = new List<string>();
                        ex = new FisException("DMT083", param);
                        throw ex;
                    }
                    else if (duplicateFlag)
                    {
                        List<string> param = new List<string>();
                        ex = new FisException("DMT063", param);
                        throw ex;
                    }    
                    
               newItem.udt = DateTime.Now;
      //         newItem.cdt = oldACAdaptor.cdt;
               itemRepository.UpdateOneAcAdaptor(newItem);
               
    //           if (newItem.Assemb == oldAssembly)
               //{
               //    //调用repository 更新接口 ...
               //    ACAdaptor item = new ACAdaptor();
               //    foreach (ACAdaptor adaptor in adaptorLst)
               //    {
               //        if (adaptor.Assemb == oldAssembly)
               //         {
               //             item = adaptor;
               //             adaptorLst.Remove(adaptor);
               //             break;
               //         }
               //    }
               //    item.Assemb = newItem.Assemb;
               //    item.Adppn = newItem.Adppn;
               //    item.Agency = newItem.Agency;
               //    item.Supplier = newItem.Supplier;
               //    item.Voltage = newItem.Voltage;
               //    item.Cur = newItem.Cur;
               //    item.Editor = newItem.Editor;
               //    item.Udt = DateTime.Now.ToString();
               //    adaptorLst.Add(item);
               //}
                
      //         else 
               { 
                    //outOfWork begin
                   //调用repository的add方法添加newItem
                   //调用repository的删除方法将oldassembly删除.
                   //outOfWork commit
      //             AddOneAcAdaptor(newItem);
     //              ACAdaptor ac = new ACAdaptor();
     //              ac.Assemb = oldAssembly;
     //              DeleteOneAcAdaptor(ac);
               }
                 
           }
           catch (FisException fex)
           {
               logger.Error(fex.Message);
               throw fex;
           }
           catch (System.Exception e)
           {
               logger.Error(e.Message);
               throw;
           }
       }

       public IList<ACAdaptor> GetAdaptorByAssembly(string assembly)
       {
           IList<ACAdaptor> conditionAdaptor = new List<ACAdaptor>();
           try
           {
               //调用repository Condition Search接口 ...

               //foreach (ACAdaptor item in adaptorLst)
               //{
               //     if(item.Assemb==assembly)
               //     {
               //         conditionAdaptor.Add(item);
               //     }
               //}
               conditionAdaptor=itemRepository.GetACAdaptorByAssembly(assembly);
           }
           catch (FisException fex)
           {
               logger.Error(fex.Message);
               throw fex;
           }
           catch (System.Exception e)
           {
               logger.Error(e.Message);
               throw;
           }
           return conditionAdaptor;
       }
        
       #endregion
   }
}
