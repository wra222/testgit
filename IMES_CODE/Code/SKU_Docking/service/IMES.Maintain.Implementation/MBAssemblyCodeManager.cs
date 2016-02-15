using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using IMES.Infrastructure.Repository.PCA;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject;
using log4net;
using IMES.FisObject.PCA.MB;

namespace IMES.Maintain.Implementation
{
    public class MBAssemblyCodeManager : MarshalByRefObject, IMBAssemblyCode
    {
        #region IMBAssemblyCode Members

        protected IMBRepository MBAssemblyCode = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// 添加MBCFG数据
        /// </summary>
        /// <param name="mbcfgDef"></param>
        public void AddMBCFG(IMES.DataModel.MBCFGDef mbcfgDef)
        {
            IList<MBCFGDef> lstMBCFG = new List<MBCFGDef>();
            lstMBCFG = MBAssemblyCode.GetMBCFGByCodeSeriesAndType(mbcfgDef.MBCode, mbcfgDef.Series, mbcfgDef.TP);
            try
            {
                MBCFGDef mbcfg = new MBCFGDef();
                mbcfg.MBCode = mbcfgDef.MBCode;
                mbcfg.Series = mbcfgDef.Series;
                mbcfg.TP = mbcfgDef.TP;
                mbcfg.CFG = mbcfgDef.CFG;
                mbcfg.Editor = mbcfgDef.Editor;
                mbcfg.Cdt = mbcfgDef.Cdt;
                mbcfg.Udt = mbcfgDef.Udt;
                if (lstMBCFG != null && lstMBCFG.Count > 0)
                {
                    //已经存在具有相同MBCFG的记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT116", erpara);
                    throw ex;

                }
                else
                {
                    MBAssemblyCode.AddMBCFG(mbcfg);
                }
            }

            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// 根据mbcode,series和type删除一条MBCFG数据
        /// </summary>
        /// <param name="MbCode"></param>
        /// <param name="Series"></param>
        /// <param name="Type"></param>
        public void DeleteMBCFG(string MbCode, string Series, string Type)
        {
            IList<MBCFGDef> lstMBCFG = new List<MBCFGDef>();
            lstMBCFG = MBAssemblyCode.GetMBCFGByCodeSeriesAndType(MbCode, Series, Type);
            try
            {
                if (lstMBCFG == null || lstMBCFG.Count <= 0)
                {   //已经不存在此MBCFG记录，不能删除
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT115", erpara);
                    throw ex;
                }
                else
                {
                    int id = lstMBCFG.First().ID;
                    MBAssemblyCode.DeleteMBCFG(id);
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
        /// 获取所有MBCFG数据
        /// </summary>
        /// <returns></returns>
        public IList<IMES.DataModel.MBCFGDef> GetAllMBCFGLst()
        {
            IList<MBCFGDef> lstMBCFG = new List<MBCFGDef>();
            try
            {
                lstMBCFG = MBAssemblyCode.GetAllMBCFGLst();
                if (lstMBCFG != null && lstMBCFG.Count > 0)
                {
                    return lstMBCFG;
                }
                else
                {
                    return null;
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
        /// 根据mbcode,series和type获取MBCFG数据
        /// </summary>
        /// <param name="MBcode"></param>
        /// <param name="Serices"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public IList<IMES.DataModel.MBCFGDef> GetMBCFGByCodeSeriesAndType(string MBcode, string Serices, string Type)
        {
            IList<MBCFGDef> lstMBCFG = new List<MBCFGDef>();
            try
            {
                lstMBCFG = MBAssemblyCode.GetMBCFGByCodeSeriesAndType(MBcode, Serices, Type);
                if (lstMBCFG != null && lstMBCFG.Count > 0)
                {
                    return lstMBCFG;
                }
                else
                {
                    return null;
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
        /// 根据mbcode,series和type获取MBCFG数据
        /// </summary>
        /// <param name="mbcfgDef"></param>
        /// <param name="MBcode"></param>
        /// <param name="Series"></param>
        /// <param name="Type"></param>
        public void UpdateMBCFG(IMES.DataModel.MBCFGDef mbcfgDef, string MBcode, string Series, string Type)
        {
            IList<MBCFGDef> lstMBCFG = new List<MBCFGDef>();
            lstMBCFG = MBAssemblyCode.GetMBCFGByCodeSeriesAndType(mbcfgDef.MBCode, mbcfgDef.Series, mbcfgDef.TP);
            try
            {
                MBCFGDef mbcfg = new MBCFGDef();
                mbcfg.MBCode = mbcfgDef.MBCode;
                mbcfg.Series = mbcfgDef.Series;
                mbcfg.TP = mbcfgDef.TP;
                mbcfg.CFG = mbcfgDef.CFG;
                mbcfg.Editor = mbcfgDef.Editor;
                mbcfg.Udt = mbcfgDef.Udt;
                if (!(mbcfg.MBCode == MBcode && mbcfg.Series == Series && mbcfg.TP == Type))
                {
                    if (lstMBCFG != null && lstMBCFG.Count > 0)
                    {
                        //已经存在具有相同MBCFG的记录
                        List<string> erpara = new List<string>();
                        FisException ex;
                        ex = new FisException("DMT116", erpara);
                        throw ex;
                    }
                }
                MBAssemblyCode.UpdateMBCFG(mbcfgDef, MBcode, Series, Type);

            }

            catch (Exception e)
            {

                throw e;
            }
        }

        #endregion
    }
}
