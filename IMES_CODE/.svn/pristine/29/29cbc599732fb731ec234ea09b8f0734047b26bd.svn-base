/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: OQCOutputImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-24   Tong.Zhi-Yong     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Repair;
using IMES.Maintain.Interface;
using IMES.FisObject.FA.Product;
using log4net;
using IMES.Route;
using IMES.DataModel;
using System.Collections;
using IMES.FisObject.Common.Material;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Maintain.Interface.MaintainIntf;
namespace IMES.Maintain.Implementation
{
    /// <summary>
    /// IOQCOutput接口的实现类
    /// </summary>
    public class CollectionMaterialLot : MarshalByRefObject, ICollectionMaterialLot 
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
     

        #region CollectionMaterialLot members
        public void UpdateMaterialByCtList(IList<string> ctList,string stage,string editor,string station,string action,string line)
        {
            try
            {
                IMaterialRepository MaterialRepository = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository, Material>();
                IUnitOfWork uow = new UnitOfWork();
                foreach (string ct in ctList)  // For Mantis0000539
                {
                  Material m=MaterialRepository.Find(ct);
                  MaterialLog mLog = new MaterialLog();
                  mLog.Status="Collect";
                  mLog.Line="";
                  mLog.Stage=stage;
                  mLog.Editor=editor;
                  mLog.PreStatus = m.Status;
                  mLog.Action = "Combine Lot";
                  m.AddMaterialLog(mLog);
                  MaterialRepository.Update(m, uow);
                }

                //MaterialRepository.AddMultiMaterialCurStatusLogDefered
                //    (uow, ctList, action, stage, line, station, "", editor);
                 
                 MaterialRepository.UpdateMultiMaterialCurStatusDefered(uow, ctList, station, editor);
                 uow.Commit();
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(CollectionMaterialLot)UpdateMaterialByCtList ");
            }
        
        }
        public List<string> GetLotInfoByCT(string ct, string materialType, string station, string processType)
        {
            List<string> ret = new List<string>();
            try
            {
                IMaterialRepository MaterialRepository = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository, Material>();
                IMaterialBoxRepository MaterialBoxRepository = RepositoryFactory.GetInstance().GetRepository<IMaterialBoxRepository, MaterialBox>();
                Material objMaterial = MaterialRepository.Find(ct);
                if (objMaterial == null)
                {
                    throw new FisException("CHK1003", new string[] { ct });
                }
                // else if (objMaterial.Status.Trim() == station)
                else if (objMaterial.Status.Trim() == station && objMaterial.Stage.Equals(processType)) // For Mantis 539
                {
                    throw new FisException("CHK1006", new string[] { ct, objMaterial.Stage });

                }
                else if (!objMaterial.CheckMaterailStatus(station))
                {
                    throw new FisException("CHK1004", new string[] {objMaterial.Status});

                }
                else
                {
                    int q = MaterialRepository.GetCombinedMaterialLotQty(materialType, objMaterial.LotNo);
                    MaterialLot objLot = MaterialBoxRepository.GetMaterialLot(materialType,objMaterial.LotNo);
                    ret.Add(objLot.SpecNo);
                    ret.Add(objLot.Qty.ToString());
                    ret.Add(objLot.LotNo.Trim());
                    ret.Add(q.ToString());
                    return ret;
                }


            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(CollectionMaterialLot)GetLotInfoByCT ");
            }
        }
        public void AddMaterialCtList(IList<string> ctList, string materialType, string lotNo, string status, string preStatus, string stage, string line, string editor)
        {
            try
            {
                IMaterialRepository MaterialRepository = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository, Material>();
                IUnitOfWork uow = new UnitOfWork();
                MaterialRepository.AddMultiMaterialLogDefered
                    (uow, ctList, "Combine Lot", stage, line, preStatus, status, "", editor);
                MaterialRepository.AddMultiMaterialCTDefered
                    (uow, ctList, materialType, lotNo, stage, line, preStatus, status, editor);
              
                uow.Commit();
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(CollectionMaterialLot)AddMaterialCtList ");
            }
        }
        public void CheckExistCtAndLotNo(string ct, string lotNo,string station)
        {
            try
            {
                IMaterialRepository MaterialRepository = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository, Material>();
                Material obj = MaterialRepository.Find(ct);

                if (obj == null)
                {
                    throw new FisException("CHK1003", new string[] { ct });
                }
                else if (obj.Status.Trim()==station)
                {
                    throw new FisException("CHK1006", new string[] { ct,obj.Stage });

                }
                else if (!obj.CheckMaterailStatus(station))
                {
                    throw new FisException("CHK1004", new string[] { obj.Status });

                }
                else if (obj.LotNo.Trim() != lotNo.Trim())
                {
                    throw new FisException("CHK1005", new string[] { obj.LotNo });
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(CollectionMaterialLot)CheckExistCtAndLotNo CT No: " + ct);
            }
        }
        public void CheckExistCT(string ct)
        {
            try
            {
                IMaterialRepository MaterialRepository = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository, Material>();
                Material obj = MaterialRepository.Find(ct);
             
                if (obj != null)
                {
                    throw new FisException("CHK1002", new string[] { ct });
                
                }
               
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(CollectionMaterialLot)CheckExistCT CT No: " + ct);
            }
        }
        public List<string> GetMaterialByLot(string lotNo, string materialType)
        {
            try
            {
                List<string> ret = new List<string>();
                IMaterialRepository MaterialRepository = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository, Material>();
                IMaterialBoxRepository MaterialBoxRepository = RepositoryFactory.GetInstance().GetRepository<IMaterialBoxRepository, MaterialBox>();
                MaterialLot objLot = MaterialBoxRepository.GetMaterialLot(materialType,lotNo);
                if(objLot==null)
                {
                  throw new FisException("CHK1001", new string[] { lotNo });
                }
                int q =MaterialRepository.GetCombinedMaterialLotQty(materialType, lotNo);
                ret.Add(objLot.SpecNo.Trim());
                ret.Add(objLot.Qty.ToString());
                ret.Add(q.ToString());
                return ret;
                // LotNo,SpecNo, Lot Qty, Combined Qty
              //  IMaterialRepository
                //MaterialBoxRepository.GetMaterialLotQtyGroupStatus
                //    MaterialBoxRepository.ge
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(CollectionMaterialLot)GetMaterialByLot LotNo: " + lotNo);
            }
        }
        public void CheckBoxId(string boxId)
        {
            try
            {
                IMaterialBoxRepository MaterialBoxRepository = RepositoryFactory.GetInstance().GetRepository<IMaterialBoxRepository, MaterialBox>();
                MaterialBox maObj = MaterialBoxRepository.Find(boxId);
                if (maObj != null)
                {
                    throw new FisException("CHK1000", new string[] { boxId });
                }
              

            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(CollectionMaterialLot)CheckBoxId BoxID: " + boxId);
            }
        
        
        }
        public void AddMaterialBox(string boxId, string specNo, string lotNo, int qty,string materialType,string feedType,string status, string editor)
        {

            try
            {
                IMaterialBoxRepository MaterialBoxRepository = RepositoryFactory.GetInstance().GetRepository<IMaterialBoxRepository, MaterialBox>();
                MaterialBox newMater = new MaterialBox();
                newMater.BoxId = boxId.Trim();
                newMater.SpecNo = specNo.Trim();
                newMater.LotNo = lotNo.Trim();
                newMater.Qty = qty;
                newMater.FeedType = feedType;
                newMater.MaterialType = materialType;
                newMater.Editor = editor;
                newMater.Status = status;
                IUnitOfWork uow = new UnitOfWork();
                MaterialBoxRepository.Add(newMater, uow);
               // MaterialLot objLot=  MaterialBoxRepository.GetMaterialLot(lotNo.Trim());
                //if (objLot == null)
                //{

                //}
                //else
                //{ 
               
                //}
             //   IMaterialRepository MaterialRepository = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository, Material>();
               //MaterialRepository.Add(
                uow.Commit();

            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(AddMaterialBox)CheckBoxId BoxID: " + boxId);
            }
        
        
        
        }


        #endregion
    }
}
