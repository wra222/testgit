/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Defect Station
* UI:CI-MES12-SPEC-PAK-DATA MAINTAIN（II）.docx  
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-08-29   Kaisheng               Create   
* Known issues:
* TODO：
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using log4net;

using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.PAK.DN;

using IMES.Infrastructure.UnitOfWork;

namespace IMES.Maintain.Implementation
{
    public class PLTStandardManager : MarshalByRefObject, IPLTStandard
    {

        #region IPLTStandard Members
        
        public const string PUB = "<PUB>";
        public const string HP = "HP";
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IPalletRepository PalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
        public IList<PltstandardInfo> GetPLTStandardList()
        {
           IList<PltstandardInfo> PltstandardList = new List<PltstandardInfo>();
            try 
            {
                PltstandardInfo cond = new PltstandardInfo();
                PltstandardList = PalletRepository.GetPltstandardInfoList(cond);
            }
            catch(FisException fe)
            {
                logger.Error(fe.Message);
                throw fe;
            }
            catch(Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            return PltstandardList;
        }
        public string AddPLTStandard(PltstandardInfo PLTStdItem)
        {
            string id = null;
            IList<PltstandardInfo> PltstandardList = new List<PltstandardInfo>();
            try 
            {
                if (PLTStdItem == null)
                {
                    //throw exception;
                }
                else
                {
                    //Vincent:don't use SqlTransactionManage
                    //SqlTransactionManager.Begin();
                    //若txtPallet 框中的内容在Delivery_Pallet （Delivery_Pallet.PalletNo）中不存在，
                    //则提示用户，放弃后续操作
                    //IDeliveryRepository IList<DeliveryPalletInfo> GetDeliveryPalletListByPlt(string palletNo);
                    IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                    IList<DeliveryPalletInfo> DNInfoList = new List<DeliveryPalletInfo>();
                    DNInfoList = DeliveryRepository.GetDeliveryPalletListByPlt(PLTStdItem.pltno);
                    if ((DNInfoList == null) || (DNInfoList.Count == 0))
                    {
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(PLTStdItem.pltno);
                        ex = new FisException("CHK241", erpara);
                        throw ex;
                    }
                    PltstandardInfo cond = new PltstandardInfo();
                    cond.pltno = PLTStdItem.pltno;
                    PltstandardList = PalletRepository.GetPltstandardInfoList(cond);
                    if (PltstandardList.Count > 0)
                    {
                        List<string> erpara = new List<string>();
                        FisException ex;
                        ex = new FisException("DMT155", erpara);
                        throw ex;
                    }
                    //PltstandardInfo newitem = new PltstandardInfo();
                    PLTStdItem.cdt = DateTime.Now;
                    PLTStdItem.udt = DateTime.Now;
                    
                     UnitOfWork uow = new UnitOfWork();
                    //PalletRepository.AddPltstandardInfo(PLTStdItem);
                     PalletRepository.AddPltstandardInfoDefered(uow, PLTStdItem);
                    //PalletRepository.UpdateAttr(PLTStdItem.pltno, "SendStatus", "", "Set to resend to SAP", PLTStdItem.editor);
                     PalletRepository.UpdateAttrDefered(uow, PLTStdItem.pltno, "SendStatus", "", "Set to resend to SAP", PLTStdItem.editor);
                     uow.Commit();
                    //SqlTransactionManager.Commit();

                    return Convert.ToString(PLTStdItem.id);



                }


            }
            catch(Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            return id;
        }

        public void DeletePLTStandard(int id)
        {
            try 
            {
                PltstandardInfo conItem = new PltstandardInfo();
                conItem.id = id;
                PalletRepository.DeletePltstandardInfo(conItem);
            }
            catch(Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
        }

        public void UpdatePLTStandard(PltstandardInfo PLTStdItem)
        {
            try 
            {
                //若txtPallet 框中的内容在Delivery_Pallet （Delivery_Pallet.PalletNo）中不存在，
                //则提示用户，放弃后续操作
                //IDeliveryRepository IList<DeliveryPalletInfo> GetDeliveryPalletListByPlt(string palletNo);
                IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                IList<DeliveryPalletInfo> DNInfoList = new List<DeliveryPalletInfo>();
                DNInfoList = DeliveryRepository.GetDeliveryPalletListByPlt(PLTStdItem.pltno);
                if ((DNInfoList == null) || (DNInfoList.Count == 0))
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(PLTStdItem.pltno);
                    ex = new FisException("CHK241", erpara);
                    throw ex;
                }
                
                IList<PltstandardInfo> PltstandardList = new List<PltstandardInfo>();
                PltstandardInfo cond = new PltstandardInfo();
                cond.pltno = PLTStdItem.pltno;
                bool repeatFlag = false;
                PltstandardList = PalletRepository.GetPltstandardInfoList(cond);
                foreach (var node in PltstandardList)
                {
                    if (node.id != PLTStdItem.id)
                    {
                        repeatFlag = true;
                        break;
                    }
                }
                if (repeatFlag)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT155", erpara);
                    throw ex;
                }

                PltstandardInfo conItem = new PltstandardInfo();
                conItem.id = PLTStdItem.id;

                PLTStdItem.id = int.MinValue;
                PLTStdItem.udt = DateTime.Now;
                PalletRepository.UpdatePltstandardInfo(PLTStdItem, conItem);    
            }
            catch(Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
        }

        #endregion

        public IList<IMES.DataModel.PltspecificationInfo> GetPLTSpecificationList()
        {
            //Select Defect, Descr from DefectCode where Type=’PRD’ order by Defect
            IList<IMES.DataModel.PltspecificationInfo> PltSpecList = null;
            PltspecificationInfo PltSpecInfo = new PltspecificationInfo();
            try
            {
                PltSpecList = PalletRepository.GetPltspecificationInfoList(PltSpecInfo);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            return PltSpecList;
        }
    }
}
