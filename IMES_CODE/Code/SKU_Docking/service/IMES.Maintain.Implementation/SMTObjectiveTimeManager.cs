/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Implementation for SMT Objective Time Page
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN For RCTO.docx –2012/7/11 
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN For RCTO.docx –2012/7/11            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-7-11   Jessica Liu            Create
* Known issues:
* TODO：
* ITC-1361-0185, Jessica Liu, 2012-9-5
* ITC-1361-0187, Jessica Liu, 2012-9-5
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using IMES.Infrastructure;
using IMES.DataModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.PCA.MB;
using System.Data;

namespace IMES.Maintain.Implementation
{
    public class SMTObjectiveTimeManager : MarshalByRefObject, IMES.Maintain.Interface.MaintainIntf.ISMTObjectiveTime
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);      
        
        #region ISMTObjectiveTime Members

        /// <summary> 
        ///取得所有的SMTLine记录
        /// </summary>
        /// <returns>返回数据库中所有存在的SMTLine记录</returns>
        public IList<SMTLineDef> GetAllSMTLineInfo()
        {
            try 
            {
                List<SMTLineDef> retLst = new List<SMTLineDef>();

                IMBRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                IList<SMTLineDef> getData = itemRepository.GetSMTLineList();

                if (getData != null)
                {
                    for (int i = 0; i < getData.Count; i++)
                    {
                        SMTLineDef data = getData[i];
                        SMTLineDef item = new SMTLineDef();

                        item.Line = data.Line;
                        item.ObjectiveTime = data.ObjectiveTime;
                        item.StartTime = (System.DateTime)data.StartTime;
                        item.EndTime = (System.DateTime)data.EndTime;
                        item.Remark = data.Remark;
                        item.Editor = data.Editor;
                        item.Cdt = (System.DateTime)data.Cdt;
                        item.Udt = (System.DateTime)data.Udt;

                        retLst.Add(item);
                    }
                }

                return retLst;

            }
            catch(FisException fex)
            {
                logger.Error(fex.Message);
                throw fex;
            }
            catch(System.Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }

        /// <summary>
        /// 删除选中的一条SMTLineDef记录
        /// </summary>
        /// <param name="item">被选中的SMTLineDef</param>
        public void DeleteOneSMTLine(SMTLineDef item)
        {
            try
            {
                IMBRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                itemRepository.RemoveSMTLine(item);
            }
            catch (FisException fex)
            {
                logger.Error(fex.Message);
                throw fex;
            }
            catch (System.Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }

        /// <summary>
        /// 添加一条符合条件的SMTLineDef记录,
        /// 当所添加的记录中的assembly在其他记录中存在时,抛出异常
        /// </summary>
        /// <param name="item"></param>
        public void AddOneSMTLine(SMTLineDef item)
        {
            try
            {
                IMBRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();

                //Line与存在记录的Line的名称相同，则提示业务异常
                SMTLineDef newObj = new SMTLineDef();
                newObj.Line = item.Line;
                IList<SMTLineDef> exists = itemRepository.GetExistSMTLine(newObj);
                if (exists != null && exists.Count > 0)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    //ITC-1361-0185, Jessica Liu, 2012-9-5
                    //ex = new FisException("DMT066", erpara);    //????
                    ex = new FisException("DMT152", erpara);    //Line已经存在，不能保存!
                    throw ex;

                }

                SMTLineDef newItem = new SMTLineDef();
                newItem.Line = item.Line;
                newItem.ObjectiveTime = item.ObjectiveTime;
                newItem.StartTime = (System.DateTime)item.StartTime;
                newItem.EndTime = (System.DateTime)item.EndTime;
                newItem.Remark = item.Remark;
                newItem.Editor = item.Editor;                

                itemRepository.AddSMTLine(newItem);

            }
            catch (FisException fex)
            {
                logger.Error(fex.Message);
                throw;
            }
            catch (System.Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }

        /// <summary>
        /// 更新选中记录,
        /// 当此记录中assembly在其他的记录中存在时,抛出异常
        /// </summary>
        /// <param name="newItem"></param>
        /// <param name="oldLine"></param>
        public void UpdateOneSMTLine(SMTLineDef newItem, string oldLine)
        {
            try
            {
                IMBRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();

                //新Line与存在记录的Line的名称相同，则提示业务异常
                SMTLineDef newObj = new SMTLineDef();
                newObj.Line = newItem.Line;
                IList<SMTLineDef> exists = itemRepository.GetExistSMTLine(newObj);
                /* ITC-1361-0187, Jessica Liu, 2012-9-5
                if (exists != null && exists.Count > 0)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT152", erpara);    //Line已经存在，不能保存!
                    throw ex;

                }
                */

                SMTLineDef tempObj = new SMTLineDef();
                tempObj.Line = oldLine;
                IList<SMTLineDef> oldexists = itemRepository.GetExistSMTLine(tempObj);
                if (oldexists == null || oldexists.Count <= 0)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT082", erpara);
                    throw ex;

                }

                SMTLineDef tempItem = new SMTLineDef();
                tempItem.Line = newItem.Line;
                tempItem.ObjectiveTime = newItem.ObjectiveTime;
                tempItem.StartTime = (System.DateTime)newItem.StartTime;
                tempItem.EndTime = (System.DateTime)newItem.EndTime;
                tempItem.Remark = newItem.Remark;
                tempItem.Editor = newItem.Editor; 
                tempItem.Cdt = Convert.ToDateTime(newItem.Cdt);
                tempItem.Udt = DateTime.Now;

                itemRepository.ChangeSMTLine(tempItem, tempObj);

            }
            catch (FisException fex)
            {
                logger.Error(fex.Message);
                throw fex;
            }
            catch (System.Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }

        /// <summary>
        /// 获取当前的时间
        /// </summary>
        public DateTime GetCurDate()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// 获取Line信息列表
        /// </summary>
        /// <returns>返回数据库中所有存在的Line相关记录</returns>
        public DataTable GetLineList()      
        {
            DataTable lineList = new DataTable();

            try
            {
                //select Remark,Line from Dept order by Section,substring(Line,4,1), substring(Line,3,1)
                IMBRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                lineList = itemRepository.GetLineList();
            }
            catch (Exception)
            {
                throw;
            }

            return lineList;
        }

        /*
        /// <summary>
        /// 根据输入的assembly模糊查询(assembly%)
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns>返回符合条件的SMTLine记录</returns>
        public IList<SMTLineDef> GetSMTLineByAssembly(string assembly)
        {
            try
            {
                IList<SMTLineDef> SMTLineList = new List<SMTLineDef>();
                IMBRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
                SMTLineList = itemRepository.GetSMTLineByAssembly(assembly);
                return SMTLineList;
            }
            catch (FisException fex)
            {
                logger.Error(fex.Message);
                throw fex;
            }
            catch (System.Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }
        */

        #endregion
    }
}
