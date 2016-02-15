/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: implementation for ECR Version
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 
 * issues:
 * itc-1361-0026  itc210012  2012-01-12
 */
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

using IMES.FisObject.Common.Warranty;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.NumControl;
using System.Data;

namespace IMES.Maintain.Implementation
{
    public class StationManager : MarshalByRefObject, IStation2
    {
        //SELECT [Station]
        //      ,[StationType]
        //      ,[OperationObject]
        //      ,[Descr]
        //      ,[Editor]
        //      ,[Cdt]
        //      ,[Udt]
        //  FROM [IMES_GetData_Datamaintain].[dbo].[Station]
        //order by [Cdt]
        public DataTable GetStationInfoList()
        {
            DataTable retLst = new DataTable();
            try
            {
                IStationRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();
                retLst = itemRepository.GetStationInfoList();
            }
            catch (Exception)
            {
                throw;
            }

            return retLst;
        }

        private static String Null2String(Object _input)
        {
            if (_input == null)
            {
                return "";
            }
            return _input.ToString().Trim();
        }

        //DELETE FROM [Station]
        //      WHERE Station='station'
        public void DeleteStation(string station)
        {
            try
            {
                IStationRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();
                itemRepository.DeleteStation(station);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Boolean IsExistStation(string station)
        //IF EXISTS(
        //SELECT [Station]     
        //  FROM [Station]
        //where Station ='station)'
        //)
        //set @return='True'
        //ELSE
        //set @return='False' 

        public string AddStation(StationDef item)
        {
            String result = "";
            try
            {
                IStationRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();

                Boolean isExist = itemRepository.IsExistStation(item.station);
                if (isExist == true)
                {
                    //已经存在具有相同Station的记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT043", erpara);
                    throw ex;

                }
                bool existFlag = itemRepository.CheckStationExistByName(item.name);
                if (existFlag)
                {
                    List<string> param = new List<string>();
                    throw new FisException("DMT139", param);
                }
                Station itemNew = new Station();

                itemNew.StationId = item.station;
                itemNew.StationType = (StationType)Enum.Parse(typeof(StationType), item.stationType);
                itemNew.OperationObject = Int32.Parse(item.operationObject);
                itemNew.Descr = item.descr;
                itemNew.Name = item.name;

                itemNew.Editor = item.editor;
                itemNew.Udt = DateTime.Now;
                itemNew.Cdt = DateTime.Now;

                itemRepository.AddStation(itemNew);
                result = itemNew.StationId;
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public string UpdateStation(StationDef item, string oldStationId)
        {
            String result = "";
            try
            {
                IStationRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();
                Station itemNew = new Station();

                itemNew.StationId = item.station;
                itemNew.StationType = (StationType)Enum.Parse(typeof(StationType), item.stationType);
                itemNew.OperationObject = Int32.Parse(item.operationObject);
                itemNew.Descr = item.descr;
                itemNew.Name = item.name;
                itemNew.Editor = item.editor;
                itemNew.Udt = DateTime.Now;
                //ITC-1361-0095 ITC210012 2012-02-21
                IList<Station> stationObj = itemRepository.GetStationItemByName(item.name);

                if (stationObj.Count>0)
                {
                    if (stationObj[0].StationId != oldStationId)
                    {
                        List<string> param = new List<string>();
                        throw new FisException("DMT139", param);
                    }
                    //itemRepository.AddStation(itemNew);
                    //result = itemNew.StationId;
                }
                        //存在 进行更新操作...
                      itemRepository.UpdateStation(itemNew, oldStationId);
                      result = itemNew.StationId;
               
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }





        #region IStation2 Members

        /// <summary>
        /// 根据stationType获取station数据
        /// </summary>
        /// <param name="stationType"></param>
        /// <returns></returns>
        public IList<StationDef> getStationByStationType(string stationType)
        {
            IList<StationDef> lstStation = new List<StationDef>();
            IList<IStation> iStation = new List<IStation>();
            try
            {
                IStationRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();
                iStation = itemRepository.GetStationList(StationType.FATest);
                if (iStation != null && iStation.Count > 0)
                {
                    foreach (IStation ist in iStation)
                    {
                        StationDef sd = new StationDef();
                        sd.station = ist.StationId;
                        sd.stationType = ist.StationType.ToString();
                        sd.descr = ist.Descr;
                        sd.name = ist.Name;
                        sd.operationObject = ist.OperationObject.ToString();
                        sd.editor = ist.Editor;
                        sd.cdt = ist.Cdt.ToShortDateString();
                        sd.udt = ist.Udt.ToShortDateString();
                        lstStation.Add(sd);
                    }
                    return lstStation;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return lstStation;
        }

        #endregion

        #region for StationAttr table
        IStationRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public IList<StationAttrDef> GetStationAttr(string station)
        {
            try
            {
                return itemRepository.GetStationAttr(station);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="station"></param>
        /// <param name="attrName"></param>
        /// <returns></returns>
        public String GetStationAttrValue(string station, string attrName)
        {
            try
            {
                return itemRepository.GetStationAttrValue(station,attrName);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="StationAttrDef"></param>
        /// <returns></returns>
        public void AddStationAttr(StationAttrDef attr)
        {
            try
            {
                itemRepository.AddStationAttr(attr);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="StationAttrDef"></param>
        /// <returns></returns>
        public void UpdateStationAttr(StationAttrDef attr)
        {
            try
            {
                itemRepository.UpdateStationAttr(attr);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="station"></param>
        /// <param name="attrName"></param>
        /// <returns></returns>
        public void DeleteStationAttr(string station, string attrName)
        {
            try
            {
                itemRepository.DeleteStationAttr(station, attrName);
            }
            catch (Exception)
            {
                throw;
            }
        }



        #endregion

    }
}
