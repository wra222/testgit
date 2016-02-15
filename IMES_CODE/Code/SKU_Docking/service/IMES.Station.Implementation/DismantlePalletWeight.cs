// INVENTEC corporation (c)2012 all rights reserved. 
// Description: DismantlePalletWeight
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-02-20   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Route;
using IMES.Infrastructure.WorkflowRuntime;
using System.Workflow.Runtime;
using log4net;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Station.Implementation
{
    /// <summary>
    ///
    /// </summary>
    public class DismantlePalletWeight : MarshalByRefObject, IDismantlePalletWeight
    {


        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const Session.SessionType currentSessionType = Session.SessionType.MB;

        #region IDismantlePalletWeight Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="palletOrDn"></param>
        /// <returns></returns>
        public IList<IMES.DataModel.DNPalletWeight> Query(string palletOrDn)
        {

            logger.Debug(" Query start, PalletOrDn:" + palletOrDn);
            try
            {
                IPalletRepository CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                IList<IMES.DataModel.DNPalletWeight> result = CurrentRepository.QueryPalletWeight(palletOrDn);
                if (result == null || result.Count == 0)
                {
                    throw new FisException("PAK086", new string[] { });
                }

                return result;

            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug(" Query end, PalletOrDn:" + palletOrDn);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="palletOrDn"></param>
        /// <param name="editor"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public IList<IMES.DataModel.DNPalletWeight> Dismantle(string palletOrDn, string editor, string line, string station, string customer)
        {

            logger.Debug(" Dismantle start, PalletOrDn:" + palletOrDn);
            try
            {
                IPalletRepository CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                IList<IMES.DataModel.DNPalletWeight> result = CurrentRepository.DismantlePalletWeight(palletOrDn, editor);
                if (result == null || result.Count == 0)
                {
                    throw new FisException("PAK086", new string[] { });
                }

                return result;

            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug(" Dismantle end, PalletOrDn:" + palletOrDn);
            }
        }

        #endregion
    }
}
