using System;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.FisObject.Common.Part;

namespace IMES.Maintain.Implementation
{
    public class BatteryImpl:MarshalByRefObject,IBattery       
    {
        public const String MACRANGE_STATUS_R = "R";
        public const String MACRANGE_STATUS_R_TEXT = "Created";
        public const String MACRANGE_STATUS_A = "A";
        public const String MACRANGE_STATUS_A_TEXT = "Active";
        public const String MACRANGE_STATUS_C = "C";
        public const String MACRANGE_STATUS_C_TEXT = "Closed";

        #region Implementation of IBattery

        /// <summary>
        /// 取得所有Battery数据的list(按Battery列的字母序排序)
        /// </summary>
        /// <returns>返回BatteryDef列表</returns>
        public IList<BatteryDef> GetAllBatteryInfoList()
        {
            List<BatteryDef> retLst = new List<BatteryDef>();

            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                IList<OlymBatteryInfo> getData = itemRepository.GetBatteryInfoList();

                if (getData != null)
                {
                    for (int i = 0; i < getData.Count; i++)
                    {
                        BatteryDef item = convert(getData[i]);
                        retLst.Add(item);
                    }
                }
               
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据batteryVC取得对应的Battery数据的list(左匹配原则，按Battery列的字母序排序)
        /// <param name="batteryVC">过滤条件batteryVC，对应数据库HPPN字段</param>
        /// <returns>返回BatteryDef列表</returns>
        public IList<BatteryDef> GetBatteryInfoList(string batteryVC)
        {
            List<BatteryDef> retLst = new List<BatteryDef>();

            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                IList<OlymBatteryInfo> getData = itemRepository.GetBatteryInfoList(batteryVC);

                if (getData != null)
                {
                    for (int i = 0; i < getData.Count; i++)
                    {
                        BatteryDef item = convert(getData[i]);

                        //item.cdt = getData[i].Cdt.ToString("yyyy-MM-dd");
                        //item.udt = getData[i].Udt.ToString("yyyy-MM-dd");
                        retLst.Add(item);
                    }
                }               
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }

        }


        private BatteryDef convert(OlymBatteryInfo data)
        {
            BatteryDef item = new BatteryDef();
            item.BatteryVC = data.hppn;
            item.Hssn = data.hssn;
            item.Editor = data.editor;

            if (data.cdt == DateTime.MinValue)
            {
                item.cdt = "";
            }
            else
            {
                item.cdt = ((System.DateTime)data.cdt).ToString("yyyy-MM-dd HH:mm:ss");
            }

            if (data.udt == DateTime.MinValue)
            {
                item.udt = "";
            }
            else
            {
                item.udt = ((System.DateTime)data.udt).ToString("yyyy-MM-dd HH:mm:ss");
            }
            return item;
        }

        /// <summary>
        /// 保存一条Battery的记录数据(Add)，若BatteryVC名称与其他存在的BatteryVC的名称相同，则提示业务异常
        /// </summary>
        /// <param name="obj">Battery结构</param>
        public void AddBattery(BatteryDef obj)
        {

            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();

                //若BatteryVC名称与其他存在的BatteryVC的名称相同，则提示业务异常
                IList<OlymBatteryInfo> exists = itemRepository.GetExistBattery(obj.BatteryVC);
                if (exists != null && exists.Count > 0)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT070", erpara);
                    throw ex;

                }

                OlymBatteryInfo item = new OlymBatteryInfo();
                item.hppn = obj.BatteryVC;
                item.hssn = obj.Hssn;
                item.editor = obj.Editor;
                item.cdt = DateTime.Now;
                item.udt = DateTime.Now;
                itemRepository. AddBattery(item);
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 保存一条Battery的记录数据(update), 若BatteryVC名称与其他存在的BatteryVC的名称相同，则提示业务异常
        /// </summary>
        /// <param name="obj">更新的Battery结构</param>
        /// <param name="oldBattery">修改前BatteryVC</param>
        public void UpdateBattery(BatteryDef obj, string oldBattery)
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();

                //若BatteryVC名称与其他存在的BatteryVC的名称相同，则提示业务异常
                IList<OlymBatteryInfo> exists = itemRepository.GetExistBattery(obj.BatteryVC);
                if (exists != null && exists.Count > 0 && oldBattery != obj.BatteryVC)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT070", erpara);
                    throw ex;
                }
                OlymBatteryInfo itemOld = itemRepository.FindBattery(oldBattery);
                if (itemOld == null)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT082", erpara);
                    throw ex;
                }
                OlymBatteryInfo item = new OlymBatteryInfo();
                item.hppn = obj.BatteryVC;
                item.hssn = obj.Hssn;
                item.editor = obj.Editor;
                item.cdt = DateTime.Now;
                item.udt = DateTime.Now;
                itemRepository.ChangeBattery(item, oldBattery);
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 删除一条Battery的记录数据
        /// </summary>
        /// <param name="obj">删除的Battery结构，关键传入BatteryVC信息</param> 
        public void DeleteBattery(BatteryDef obj)
        {
            try
            {
                IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                OlymBatteryInfo item = itemRepository.FindBattery(obj.BatteryVC);
                if (item != null)
                {
                    itemRepository.RemoveBattery(item);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        public static String Null2String(Object _input)
        {
            if (_input == null)
            {
                return "";
            }
            return _input.ToString().Trim();
        }
    }

}
