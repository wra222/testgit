/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description: PAKCHN(TW)LabelLightNo Maintain
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012/05/18   kaisheng           (Reference Ebook SourceCode) Create
 * * issue:
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using log4net;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.FisBOM;
namespace IMES.Maintain.Implementation
{
    public class PAKLabelLightNoManager : MarshalByRefObject, IPAKLabelLightNo
    {

        #region IPAKLabelLightNo Members
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //public static IList<PakChnTwLightInfo> PakChnTwLightLst = new List<PakChnTwLightInfo>();
        IPalletRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();
        public IList<PakChnTwLightInfo> GetAllPAKLabelLightNo()
        {
            IList<PakChnTwLightInfo> PakChnTwLightLst = new List<PakChnTwLightInfo>();
            PakChnTwLightInfo NewLightItem =new PakChnTwLightInfo(); 
            try 
            {
                PakChnTwLightLst = (IList<PakChnTwLightInfo>)itemRepository.GetPakChnTwLightInfoList(NewLightItem);
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
            return PakChnTwLightLst;
        }


        public void DeleteSelectedPAKLabelLightNo(PakChnTwLightInfo LightNoItem)
        {
              
            try 
            {
                itemRepository.DeletePakChnTwLightInfo(LightNoItem);
            }
            catch (FisException fe)
            {
                logger.Error(fe.Message);
                throw fe;
            }

            catch(Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            return ;
        }

        public void UpdateSelectedPAKLabelLightNo(PakChnTwLightInfo LightNoItem)
        {
            try 
            {
                PakChnTwLightInfo SetLightItem =new PakChnTwLightInfo();
                PakChnTwLightInfo ConLightItem =new PakChnTwLightInfo();
                SetLightItem.lightNo = LightNoItem.lightNo;
                SetLightItem.model = LightNoItem.model;
                SetLightItem.partNo = LightNoItem.partNo;
                SetLightItem.descr = LightNoItem.descr;
                SetLightItem.type = LightNoItem.type;
                SetLightItem.editor = LightNoItem.editor;
                SetLightItem.udt = DateTime.Now;
                ConLightItem.id = LightNoItem.id;
                itemRepository.UpdatePakChnTwLightInfo(SetLightItem, ConLightItem);
            }
            catch(Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            return ;
        }

        #endregion

        #region IPAKLabelLightNo Members


        public int AddSelectedPAKLabelLightNo(PakChnTwLightInfo LightNoItem)
        {

            PakChnTwLightInfo NewLightItem = new PakChnTwLightInfo();
            NewLightItem.lightNo = LightNoItem.lightNo;
            NewLightItem.model = LightNoItem.model;
            NewLightItem.partNo = LightNoItem.partNo;
            NewLightItem.type = LightNoItem.type;
            NewLightItem.descr = LightNoItem.descr;
            IList<PakChnTwLightInfo> PakChnTwLightLst = new List<PakChnTwLightInfo>();
            int getID = -1;
            try 
            {
                itemRepository.InsertPakChnTwLightInfo(LightNoItem);
                getID = LightNoItem.id;
                PakChnTwLightLst = (IList<PakChnTwLightInfo>)itemRepository.GetPakChnTwLightInfoList(NewLightItem);
                if (PakChnTwLightLst != null && PakChnTwLightLst.Count != 0)
                {
                    getID = PakChnTwLightLst[0].id;
                }
            }
            catch(Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            return getID;
        }

        #endregion


    }
}
