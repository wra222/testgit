using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.FisObject.Common.Line;
using System.Data;
using IMES.DataModel;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;

namespace IMES.Maintain.Implementation
{
    class PdLineStationManager : MarshalByRefObject, IPdLineStation
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ILineRepository lineRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();

        /// <summary>
        /// 根据Line检索Line_Station
        /// </summary>
        /// <returns></returns>
        public DataTable GetPdLineStationListByLine(string line)
        {
            return lineRepository.GetPaLineStationListByLine(line);
        }

        /// <summary>
        /// 取得全部的Stage 
        /// </summary>
        /// <returns></returns>
        public IList<string> GetAllStage()
        {
            return lineRepository.GetAllStage();
        }

        /// <summary>
        ///取得PdLine 
        /// </summary>
        /// <returns></returns>
        public IList<string> GetLineByCustAndStage(string Cust, string stage)
        {
            return lineRepository.GetLineByCustAndStage(Cust, stage);
        }

        /// <summary>
        /// 删除line和Station的关系
        /// </summary>
        /// <returns></returns>
        public void DeleteLineStationByID(int id)
        {
            lineRepository.DeleteLineStationByID(id);
        }

        /// <summary>
        /// 判断line和Station的关系是否已经存在
        /// </summary>
        /// <returns></returns>
        public bool IFLineStationIsExists(string line, string station)
        {
            return lineRepository.IFLineStationIsExists(line,station);
        }

        /// <summary>
        /// 添加LineStation,返回新纪录的ID 
        /// </summary>
        /// <returns></returns>
        public int AddLineStation(LineStation lineStation)
        {
            lineRepository.AddLineStation(lineStation);
            return lineStation.ID;
        }


        /// <summary>
        /// 更新LineStation纪录 
        /// </summary>
        /// <returns></returns>
        public void UpdateLineStation(LineStation lineStation)
        {
            lineRepository.UpdateLineStation(lineStation);
        }

        //取得station的列表
        //选项包括所有的Station，按创建时间排序
        //SELECT [Station]
        //        FROM [IMES_GetData_Datamaintain].[dbo].[Station]
        //ORDER BY [Cdt]
        public IList<SelectInfoDef> GetStationList()
        {
            List<SelectInfoDef> result = new List<SelectInfoDef>();

            try
            {

                IStationRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();
                DataTable getData = itemRepository.GetStationList();
                for (int i = 0; i < getData.Rows.Count; i++)
                {
                    SelectInfoDef item = new SelectInfoDef();
                    item.Value = Null2String(getData.Rows[i][0]);
                    item.Text = Null2String(getData.Rows[i][0]);
                    result.Add(item);
                }

            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        private static String Null2String(Object _input)
        {
            if (_input == null)
            {
                return "";
            }
            return _input.ToString().Trim();
        } 
    }
}
