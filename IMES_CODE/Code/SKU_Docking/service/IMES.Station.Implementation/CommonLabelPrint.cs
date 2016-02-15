using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
//using IMES.Station.Interface.CommonIntf;
using System.Workflow.Runtime;
using IMES.DataModel;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MBMO;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.Extend;
using IMES.FisObject.Common.PrintItem;
using IMES.FisObject.Common.PrintLog;
using IMES.Route;
using IMES.FisObject.Common.Station;
using IMES.FisObject.FA.Product;

namespace IMES.Station.Implementation
{

    /// <summary>
    /// </summary>
    public partial class CommonLabelPrint : MarshalByRefObject, ICommonLabelPrint
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ILabelTypeRepository labelTypeRep = RepositoryFactory.GetInstance().GetRepository<ILabelTypeRepository>();

        /// <summary>
        /// </summary>                
        public ArrayList GetOfflineLabelSettingList(string editor, string customer)
        {
            ArrayList retLst = new ArrayList();
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}( Editor:{1} Customer:{2})", methodName, editor, customer);
            try
            {
                IList<OfflineLableSettingDef> lstOfflineLable = labelTypeRep.GetAllOfflineLabelSetting();

                retLst.Add(lstOfflineLable.Where(y => !(y.PrintMode==0 && string.IsNullOrEmpty(y.SPName))).Select(x => x.description).ToList());
                
                return retLst;
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
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

        private void ParamAdd(ArrayList paramLst, string val)
        {
            if (!string.IsNullOrEmpty(val))
            {
                paramLst.Add(val);
            }
        }

        /// <summary>
        /// </summary>                
        public ArrayList GetOfflineLabelSetting(string labelDescr, string editor, string customer)
        {
            ArrayList retLst = new ArrayList();
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}( Editor:{1} Customer:{2} labelDescr:{3})", methodName, editor, customer, labelDescr);
            try
            {
                IList<OfflineLableSettingDef> lstOfflineLable = labelTypeRep.GetAllOfflineLabelSetting();
                OfflineLableSettingDef lbl = lstOfflineLable.Where(x => x.description == labelDescr).FirstOrDefault();

                if (null == lbl)
                {
                    throw new Exception("Not Found.");
                }

                retLst.Add(lbl.PrintMode);
                retLst.Add(lbl.labelSpec);
                retLst.Add(lbl.fileName);
                retLst.Add(lbl.SPName);

                ArrayList paramLst = new ArrayList();
                ParamAdd(paramLst, lbl.param1);
                ParamAdd(paramLst, lbl.param2);
                ParamAdd(paramLst, lbl.param3);
                ParamAdd(paramLst, lbl.param4);
                ParamAdd(paramLst, lbl.param5);
                ParamAdd(paramLst, lbl.param6);
                ParamAdd(paramLst, lbl.param7);
                ParamAdd(paramLst, lbl.param8);
                retLst.Add(paramLst);

                return retLst;
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
                logger.DebugFormat("END: {0}()", methodName);
            }
        }

    }

}
