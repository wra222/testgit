/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: implementation for ECR Version
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 
 issues code:      modify: 
 * itc-1361-0021   itc210012  2012-01-12
 * itc-1361-0020   itc210012  2012-01-12
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.FisObject.Common.Defect;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using log4net;
using IMES.Infrastructure;
using IMES.FisObject.Common.Part;

namespace IMES.Maintain.Implementation
{
    public class RepairInfoMaintainManager : MarshalByRefObject, IRepairInfoMaintain
    {

        #region IRepairInfoMaintain 成员
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IDefectInfoRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IDefectInfoRepository>();
        public int AddOrUpdateRepairInfoMaintain(IMES.DataModel.RepairInfoMaintainDef def)
        {
            
            int id = 0;
            try 
            {
                if(!string.IsNullOrEmpty(def.type)&&!string.IsNullOrEmpty(def.code))
                {
                                      
                    IList<DefectInfoDef> result = itemRepository.GetDefectInfoByTypeAndCode(def.type, def.code);

                     
                    IPartRepository itemRepositoryNew = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                    IList<string> getData = itemRepositoryNew.GetValueFromSysSettingByName("Customer");
                    if (getData.Count > 0)
                    {
                        def.customerID = getData[0];
                    }

                    if (result == null || result.Count==0)
                    {
                        
                        DefectInfoDef vo = PO2VO(def);
                        itemRepository.AddRepairInfoItem(vo);
                        id = vo.id;
                       
                    }
                    else 
                    {

                        if (result[0].id == def.id)
                        {
                            //存在 进行更新操作...
                            DefectInfoDef vo = PO2VO(def);
                            itemRepository.UpdateRepairInfoItem(vo, def.code, def.type);
                            id = def.id;
                        }
                        else 
                        {
                            List<string> param = new List<string>();
                            throw new FisException("DMT138",param);
                        }
                       
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
            return id;
        }

        private static DefectInfoDef PO2VO(IMES.DataModel.RepairInfoMaintainDef def)
        {
            DefectInfoDef vo = new DefectInfoDef();
            vo.code = def.code;
            vo.description = def.description;
            vo.type = def.type;
            vo.Editor = def.editor;
            vo.EngDescr = def.engDescr;
            vo.customerID = def.customerID;
            vo.udt = Convert.ToDateTime(def.udt);
            vo.cdt = Convert.ToDateTime(def.cdt);
            return vo;
        }

        public IList<IMES.DataModel.RepairInfoMaintainDef> GetRepairInfoByCondition(string type)
        {
            IList<IMES.DataModel.DefectInfoDef> dataList = new List<IMES.DataModel.DefectInfoDef>();
            IList<IMES.DataModel.RepairInfoMaintainDef> poList = new List<IMES.DataModel.RepairInfoMaintainDef>();
            try 
            {
                if(!String.IsNullOrEmpty(type))
                {
                    dataList = itemRepository.GetRepairInfoByCondition(type);
                    foreach (DefectInfoDef def in dataList)
                    {
                        RepairInfoMaintainDef mdef = new RepairInfoMaintainDef();
                        mdef.id = def.id;
                        mdef.type = def.type;
                        mdef.customerID = def.customerID;
                        mdef.description = def.description;
                        mdef.engDescr = def.EngDescr;
                        mdef.code = def.code;
                        mdef.editor = def.Editor;
                        mdef.cdt = def.cdt.ToString("yyyy-MM-dd hh:mm:ss");
                        mdef.udt = def.udt.ToString("yyyy-MM-dd hh:mm:ss");
                        poList.Add(mdef);
                    }
                }
                
            }
            catch(Exception)
            {
                throw;
            }
            return poList;
        }

        public void RemoveRepairInfoMaintainItem(IMES.DataModel.RepairInfoMaintainDef def)
        {
            try 
            {
                if(def!=null&&!string.IsNullOrEmpty(def.code)&&!string.IsNullOrEmpty(def.type))
                {
                    itemRepository.RemoveRepairInfoItem(def.code,def.type);
                }
                
            }
            catch(Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
