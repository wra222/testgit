/*
 *Issue
 *ITC-1361-0045   itc210012  2011-01-16
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using log4net;
using IMES.FisObject.Common.Line;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.Maintain.Implementation
{
    public class PAKitLocManager : MarshalByRefObject,IPAKitLoc
    {

        #region IPAKitLoc 成员
        public const string PAKKITTING = "PAKKitting";
        public const string STAGEMODEL = "PAK";
        public static IList<PAKitLocDef> pakObjLst = new List<PAKitLocDef>();
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
        IPartRepository itemPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
        public IList<string> GetAllPdLine()
        {
            ILineRepository itemLineRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository>();
            IList<string> pdLineLst = null;

            try 
            {

                IList<LineInfo> infoLst=itemLineRepository.GetAllPdLineListByStage(STAGEMODEL);
                pdLineLst = new List<string>();
                foreach(LineInfo inf in infoLst)
                {
                   
                    pdLineLst.Add(inf.line);
                }
            }
            catch(Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
            return pdLineLst;

        }

        

        public string AddPAKitLoc(IMES.DataModel.PAKitLocDef item)
        {
            string id = "";
            if(item!=null)
            {

                try 
                {
                    FisException ex;
                    IList<PAKitLoc> pakRes=itemRepository.GetPAKitlocByPdLine(item.pdLine);
                    if(pakRes!=null)
                    {
                        foreach(PAKitLoc loc in pakRes)
                        {
                            if(loc.partNo.Trim().ToLower()==item.partNo.Trim().ToLower())
                            {
                                List<string> param = new List<string>();
                                ex = new FisException("DMT080", param);
                                throw ex;
                            }
                        }
                    }
                    PAKitLoc addLoc = new PAKitLoc();
                    addLoc.location = item.location;
                    addLoc.partNo = item.partNo;
                    addLoc.pdLine = item.pdLine;
                    addLoc.station = item.station;
                    addLoc.descr = item.Descr;
                    addLoc.editor = item.editor;
                    addLoc.udt = DateTime.Now;
                    addLoc.cdt = DateTime.Now;
                    IUnitOfWork uo = new UnitOfWork();
                    itemRepository.AddPAKitLocDefered(uo, addLoc);
                    IList<PAKitLoc> pakLst=itemRepository.GetPAKitlocByStation(item.station);
                    foreach(PAKitLoc loc in pakLst){
                        if (loc.location != item.location)
                        {
                            loc.location = item.location;
                            itemRepository.UpdatePAKitLocDefered(uo, loc);
                        }
                        
                    }
                    //itemRepository.AddPAKitLoc(addLoc);
                    uo.Commit();
                    id = addLoc.id.ToString();
                }
                catch(Exception ee)
                {
                    logger.Error(ee.Message);
                    throw;
                }

            }
            return id;
            
        }

        public void UpdatePAKitLoc(IMES.DataModel.PAKitLocDef newItem)
        {
            try 
            {
                FisException ex=null;
                Boolean duplicateFlag = false, notExistFlag = true;
                    IList<PAKitLoc> pakRes = itemRepository.GetPAKitlocByPdLine(newItem.pdLine);
                    if (pakRes != null)
                    {
                        foreach (PAKitLoc loc in pakRes)
                        {
                            if (loc.id == newItem.id)
                            {
                                notExistFlag = false;
                            }
                            if (loc.id!=newItem.id&&loc.partNo.Trim().ToLower() == newItem.partNo.Trim().ToLower())
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
                            ex = new FisException("DMT080", param);
                            throw ex;
                        }
                   }
                
                PAKitLoc updateLoc = new PAKitLoc();
                updateLoc.udt = DateTime.Now;
                updateLoc.id = newItem.id;
                updateLoc.location = newItem.location;
                updateLoc.partNo = newItem.partNo;
                updateLoc.pdLine = newItem.pdLine;
                updateLoc.station = newItem.station;
                updateLoc.descr = newItem.Descr;
                updateLoc.editor = newItem.editor;
                IUnitOfWork uo = new UnitOfWork();
                //itemRepository.UpdatePAKitLoc(updateLoc);
                itemRepository.UpdatePAKitLocDefered(uo, updateLoc);
                //IList<PAKitLoc> pakLst=itemRepository.GetPAKitlocByStation(newItem.station);
                IList<PAKitLoc> pakLst = itemRepository.GetPAKitlocByStationAndPdLine(newItem.station, newItem.pdLine);
                foreach (PAKitLoc loc in pakLst)
                {
                    if (loc.location != newItem.location)
                    {
                        loc.location = newItem.location;
                        loc.editor = newItem.editor;
                        itemRepository.UpdatePAKitLocDefered(uo, loc);
                    }

                }
                uo.Commit();
            }
            catch(Exception ee)
            {
                logger.Error(ee.Message);
                throw;
            }

        }

        public void DeletePAKitLoc(IMES.DataModel.PAKitLocDef oldItem)
        {
           
                try 
                {
                    itemRepository.DeletePAKitLoc(Convert.ToInt32(oldItem.id));
                }
                catch(Exception ee)
                {
                    logger.Error(ee.Message);
                    throw;
                }
            
        }

        public IList<string> GetAllTypeDescr()
        {
            IList<string> typeDescrLst = null;
            try 
            {
                 typeDescrLst=itemPartRepository.GetAllTypeDescr();
            }
            catch(Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
            return typeDescrLst;
        }

        public IList<string> GetPartNoByTypeDescr(string typeDescr)
        {
            IList<string> partNo = new List<string>();
            if(!String.IsNullOrEmpty(typeDescr))
            {
                try 
                {
                    partNo=itemPartRepository.GetPartNoByTypeDescr(typeDescr);
                }
                catch(Exception ee)
                {
                    throw ee;
                }
            }
            return partNo;
            
        }

        #endregion

       

        #region IPAKitLoc 成员


        IList<PAKitLocDef> IPAKitLoc.GetPAKitlocByPdLine(string pdline)
        {
            IList<PAKitLocDef> conditionResult=new List<PAKitLocDef>();
            if(!String.IsNullOrEmpty(pdline))
            {
                try 
                {
                    IList<PAKitLoc> result = itemRepository.GetPAKitlocByPdLine(pdline);
                    if(result!=null)
                    {
                        foreach(PAKitLoc loc in result)
                        {
                            PAKitLocDef def=new PAKitLocDef();
                            def.id=loc.id;
                            def.location=loc.location;
                            def.partNo=loc.partNo;
                            def.pdLine=loc.pdLine;
                            def.station=loc.station;
                            def.Descr=loc.descr;
                            def.editor=loc.editor;
                            def.cdt=loc.cdt.ToString("yyyy-MM-dd HH:mm:ss");
                            def.udt = loc.udt.ToString("yyyy-MM-dd HH:mm:ss");
                            conditionResult.Add(def);
                        }
                    }
                    
                }
                catch(Exception ee)
                {
                    logger.Error(ee.Message);
                    throw ee;
                }
            }
            return conditionResult;
        }

        #endregion

        #region IPAKitLoc 成员


        public IList<string> GetAllPAKikittingStationName()
        {
            IList<string> stations = new List<string>();
            try 
            {
                stations=itemPartRepository.GetAllPAKikittingStationName(PAKKITTING);
            }
            catch(Exception)
            {
                throw;
            }
            return stations;
        }

        public IList<string> FindCodeByFamily(string family)
        {
            IList<string> codeLst = new List<string>();
            try 
            {
                codeLst = itemPartRepository.GetPartInfoValueByPartDescr(family,"MB");
            }
            catch(Exception)
            {
                throw;
            }
            return codeLst;
        }

        #endregion

        
    }
}
