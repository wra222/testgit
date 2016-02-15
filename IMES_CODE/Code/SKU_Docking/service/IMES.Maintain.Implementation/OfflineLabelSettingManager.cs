using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.PCA.MBModel;
using IMES.FisObject.Common.Line;
using IMES.FisObject.PCA.MBMO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.Misc;
using IMES.FisObject.Common.MO;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Warranty;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.NumControl;
using System.Data;
using log4net;
using IMES.Infrastructure.Repository.Common;
using IMES.FisObject.Common.PrintItem;
namespace IMES.Maintain.Implementation
{
    class OfflineLabelSettingManager : MarshalByRefObject, IOfflineLabelSetting
    {

        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 获取所有offlinelablesetting数据
        /// </summary>
        /// <returns></returns>
        public IList<OfflineLableSettingDef> getAllOfflineLabelSetting()
        {
            IList<OfflineLableSettingDef> tmpOfflineList = new List<OfflineLableSettingDef>();
            try
            {
                ILabelTypeRepository iplr = RepositoryFactory.GetInstance().GetRepository<ILabelTypeRepository>();
                tmpOfflineList = iplr.GetAllOfflineLabelSetting();
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }

            return tmpOfflineList;

        }

        /// <summary>
        /// 根据filename获取offlinelabelsetting数据
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public IList<OfflineLableSettingDef> getOfflineLabelSetting(string FileName)
        {
            IList<OfflineLableSettingDef> offlineList = new List<OfflineLableSettingDef>();
            try
            {
                ILabelTypeRepository iplr = RepositoryFactory.GetInstance().GetRepository<ILabelTypeRepository>();
                offlineList = iplr.GetOfflineLabelSetting(FileName);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }

            return offlineList;
        }

        /// <summary>
        /// 添加一条offlinelabelsetting数据
        /// </summary>
        /// <param name="obj"></param>
        public void addOfflineLabelSetting(OfflineLableSettingDef obj)
        {
            try
            {
                ILabelTypeRepository iplr = RepositoryFactory.GetInstance().GetRepository<ILabelTypeRepository>();
            IList<OfflineLableSettingDef> lstOffline= new List<OfflineLableSettingDef>();
                lstOffline = iplr.GetOfflineLabelSetting(obj.fileName);
                if (lstOffline == null || lstOffline.Count <= 0)
                {
                    string[] getParams = resetParams(obj);
                    OfflineLableSettingDef OfSetting = new OfflineLableSettingDef();
                    OfSetting.fileName = obj.fileName;
                    OfSetting.labelSpec = obj.labelSpec;
                    OfSetting.description = obj.description;
                    OfSetting.PrintMode = obj.PrintMode;
                    OfSetting.SPName = obj.SPName;
                    OfSetting.param1 = getParams[0];
                    OfSetting.param2 = getParams[1];
                    OfSetting.param3 = getParams[2];
                    OfSetting.param4 = getParams[3];
                    OfSetting.param5 = getParams[4];
                    OfSetting.param6 = getParams[5];
                    OfSetting.param7 = getParams[6];
                    OfSetting.param8 = getParams[7];
                    OfSetting.cdt = obj.cdt;
                    OfSetting.editor = obj.editor;
                    OfSetting.udt = obj.udt;
                    iplr.AddOfflineLabelSetting(OfSetting);
                }
                else
                {
                    //已经存在具有相同File的OfflineLableSettingDef记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT084", erpara);
                    throw ex;
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
                throw;
            }
        }

        /// <summary>
        /// 更新offlinelabelsetting数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="oldFileName"></param>
        public void updateOfflineLabelSetting(OfflineLableSettingDef obj, string oldFileName)
        {
            try
            {
                string[] getParams = resetParams(obj);
                ILabelTypeRepository iplr = RepositoryFactory.GetInstance().GetRepository<ILabelTypeRepository>();
                OfflineLableSettingDef OfSetting = new OfflineLableSettingDef();
                OfSetting.fileName = obj.fileName;
                OfSetting.labelSpec = obj.labelSpec;
                OfSetting.description = obj.description;
                OfSetting.PrintMode = obj.PrintMode;
                OfSetting.SPName = obj.SPName;
                OfSetting.param1 = getParams[0];
                OfSetting.param2 = getParams[1];
                OfSetting.param3 = getParams[2];
                OfSetting.param4 = getParams[3];
                OfSetting.param5 = getParams[4];
                OfSetting.param6 = getParams[5];
                OfSetting.param7 = getParams[6];
                OfSetting.param8 = getParams[7];
                OfSetting.cdt = obj.cdt;
                OfSetting.editor = obj.editor;
                OfSetting.udt = obj.udt;
                if (oldFileName.Trim() == obj.fileName.Trim())
                {
                    iplr.UpdateOfflineLabelSetting(OfSetting, oldFileName);
                }
                else if (iplr.GetOfflineLabelSetting(obj.fileName).Count <= 0)
                {
                    OfflineLableSettingDef oldOfflineLabelSetting = this.getOfflineLabelSetting(oldFileName).First();
                    this.deleteOfflineLabelSetting(oldOfflineLabelSetting);
                    iplr.AddOfflineLabelSetting(OfSetting);
                }
                else
                {
                    //已经存在具有相同File的OfflineLableSettingDef记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT084", erpara);
                    throw ex;
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
                throw;
            }
        }

        /// <summary>
        /// 删除offlinelabelsetting数据
        /// </summary>
        /// <param name="obj"></param>
        public void deleteOfflineLabelSetting(OfflineLableSettingDef obj)
        {
            ILabelTypeRepository iplr = RepositoryFactory.GetInstance().GetRepository<ILabelTypeRepository>();
            OfflineLableSettingDef OfSetting = new OfflineLableSettingDef();
            OfSetting.id = obj.id;
            OfSetting.fileName = obj.fileName;
            OfSetting.labelSpec = obj.labelSpec;
            OfSetting.description = obj.description;
            OfSetting.PrintMode = obj.PrintMode;
            OfSetting.SPName = obj.SPName;
            OfSetting.param1 = obj.param1;
            OfSetting.param2 = obj.param2;
            OfSetting.param3 = obj.param3;
            OfSetting.param4 = obj.param4;
            OfSetting.param5 = obj.param5;
            OfSetting.param6 = obj.param6;
            OfSetting.param7 = obj.param7;
            OfSetting.param8 = obj.param8;
            OfSetting.cdt = obj.cdt;
            OfSetting.editor = obj.editor;
            OfSetting.udt = obj.udt;
            try
            {
                iplr.DeleteOfflineLabelSetting(OfSetting);
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }


        /// <summary>
        /// 根据ParamsX是否为空进行重置
        /// </summary>
        /// <param name="olsd"></param>
        /// <returns></returns>
        private string[] resetParams(OfflineLableSettingDef olsd)
        {
            string[] paramArray = new string[8];
            paramArray[0] = olsd.param1;
            paramArray[1] = olsd.param2;
            paramArray[2] = olsd.param3;
            paramArray[3] = olsd.param4;
            paramArray[4] = olsd.param5;
            paramArray[5] = olsd.param6;
            paramArray[6] = olsd.param7;
            paramArray[7] = olsd.param8;

            for (int i = 0; i <= 7; i++)
            {
                if (paramArray[i].Trim() != "")
                    continue;
                else
                {
                    for (int j = i + 1; j <= 7; j++)
                    {
                        if (paramArray[j].Trim() != "")
                        {
                            paramArray[i] = paramArray[j];
                            paramArray[j] = "";
                            break;
                        }
                    }
                }
            }
            return paramArray;

        }
    }
}
