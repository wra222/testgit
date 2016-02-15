using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.FisObject.PCA.MB;
using log4net;
using IMES.DataModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;

namespace IMES.Maintain.Implementation
{
    public class ITCNDDefectCheckManager : MarshalByRefObject, IITCNDDefectCheck
    {

        #region IITCNDDefectCheck 成员
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IMBRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
        public void AddITCNDDefectCheck(IMES.DataModel.ITCNDDefectCheckDef item)
        {
            try 
            {
                Boolean existFamily=false;
               ITCNDDefectCheckDef dataLst =itemRepository.CheckExistByFamilyAndCode(item.family,item.code);
                //IList<ITCNDDefectCheckDef> dataLst=GetAllITCNDDefectCheckItems();
                //foreach(ITCNDDefectCheckDef def in dataLst)
                {
                    if (dataLst != null)
                    {
                        FisException fe=null;
                        List<string> param = new List<string>();
                        fe = new FisException("DMT117",param);
                        throw fe;
                    }
                }
            //    if(!existFamily)
                {
                    itemRepository.AddITCNDDefectCheck(item);
                }
            }
            catch(Exception)
            {
                throw;
            }

        }

        public IList<IMES.DataModel.ITCNDDefectCheckDef> GetAllITCNDDefectCheckItems()
        {
            IList<ITCNDDefectCheckDef> dataLst = new List<ITCNDDefectCheckDef>();
            try 
            {
                dataLst=itemRepository.GetAllITCNDDefectChecks();
            }
            catch(Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }
            return dataLst;
        }

        public void RemoveITCNDDefectCheck(IMES.DataModel.ITCNDDefectCheckDef item)
        {
            try 
            {
                if(!String.IsNullOrEmpty(item.family))
                {
                    string family = item.family.Trim();
                    string code = item.code.Trim();
                    //itc-1361-0111  itc210012  2012-02-28
                    itemRepository.RemoveITCNDDefectCheckbyFamilyAndCode(family, code);
                }
                
            }
            catch(Exception ee)
            {
                logger.Error(ee.Message);
                throw ee;
            }

        }

        #endregion
    }
}
