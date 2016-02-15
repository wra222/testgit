/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description: FamilyInfo Maintain
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
using IMES.FisObject.Common.Part;
namespace IMES.Maintain.Implementation
{
    public class FamilyInfoManager : MarshalByRefObject, IFamilyInfo
    {

        #region IFamilyInfo Members
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //public static IList<PakChnTwLightInfo> PakChnTwLightLst = new List<PakChnTwLightInfo>();
        IFamilyRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
        public IList<FamilyInfoDef> GetAllFamilyInfo()
        {
            IList<FamilyInfoDef> FamilyInfoLst = new List<FamilyInfoDef>();
            FamilyInfoDef NewFamilyInfoItem = new FamilyInfoDef(); 
            try 
            {
                FamilyInfoLst = (IList<FamilyInfoDef>)itemRepository.GetExistFamilyInfo(NewFamilyInfoItem);
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
            return FamilyInfoLst;
        }


        public void DeleteSelectedFamilyInfo(FamilyInfoDef FamilyInfoItem)
        {
              
            try 
            {
                itemRepository.RemoveFamilyInfo(FamilyInfoItem);
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

        public void UpdateSelectedFamilyInfo(FamilyInfoDef FamilyInfoItem)
        {
            try 
            {
                FamilyInfoDef SetFamilyInfoItem =new FamilyInfoDef();
                FamilyInfoDef ConFamilyInfoItem = new FamilyInfoDef();
                SetFamilyInfoItem.family = FamilyInfoItem.family;
                SetFamilyInfoItem.name = FamilyInfoItem.name;
                SetFamilyInfoItem.value = FamilyInfoItem.value;
                SetFamilyInfoItem.descr = FamilyInfoItem.descr;
                SetFamilyInfoItem.editor = FamilyInfoItem.editor;
                SetFamilyInfoItem.udt = DateTime.Now;
                ConFamilyInfoItem.id = FamilyInfoItem.id;
                itemRepository.UpdateFamilyInfo(SetFamilyInfoItem, ConFamilyInfoItem);
            }
            catch(Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            return ;
        }

        public int AddSelectedFamilyInfo(FamilyInfoDef FamilyInfoItem)
        {

            FamilyInfoDef NewFamilyInfoItem = new FamilyInfoDef();
            NewFamilyInfoItem.family = FamilyInfoItem.family;
            NewFamilyInfoItem.name = FamilyInfoItem.name;
            NewFamilyInfoItem.value = FamilyInfoItem.value;
            NewFamilyInfoItem.descr = FamilyInfoItem.descr;
            IList<FamilyInfoDef> FamilyInfoLst = new List<FamilyInfoDef>();
            int getID = -1;
            try 
            {
                itemRepository.AddFamilyInfo(FamilyInfoItem);
                getID = FamilyInfoItem.id;
                FamilyInfoLst = (IList<FamilyInfoDef>)itemRepository.GetExistFamilyInfo(NewFamilyInfoItem);
                if (FamilyInfoLst != null && FamilyInfoLst.Count != 0)
                {
                    getID = FamilyInfoLst[0].id;
                }
            }
            catch(Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            return getID;
        }

        public IList<ConstValueTypeInfo> GetFamilyInfoNameList(string Type)
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                IList<ConstValueTypeInfo> retLst = itemRepository.GetConstValueTypeList(Type);//itemRepository.GetTypeListFromConstValue();

                return retLst;
            }
            catch (Exception)
            {
                throw;
            } 
        }

        #endregion


    }
}
