using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.DataModel;

namespace IMES.FisObject.Common.QTime
{
    public interface IQTimeRepository : IRepository<QTime>
    {
        QTime GetPriorityQTime(string line, string station,
                                                 string productId,string model, string family);

        IList<QTime> GetQTime(string line, string station);
        IList<QTime> GetQTimeByLine(string line);
        IList<QTime> GetQTimeByStation(string line);

        void RemoveQTime(string line, string station, string family);
        void RemoveQTimeDefered(IUnitOfWork uow, string Line, string station,string family);

        LineStationLastProcessTime GetLastProcessTime(string line, string station);
        void UpdateLineStationLastProcessTime(LineStationLastProcessTime time);
        void UpdateLineStationLastProcessTimeDefered(IUnitOfWork uow, LineStationLastProcessTime time);

        void AddLineStationStopPeriodLog(LineStationStopPeriodLog log);
        void AddLineStationStopPeriodLogDefered(IUnitOfWork uow, LineStationStopPeriodLog log);
        void RemoveStationStopPeriodLog(string line, string station, int remainDays);
        void RemoveStationStopPeriodLogDefered(IUnitOfWork uow, string line, string station, int remainDays);

        IList<int> CalLineStopTime(string line, string station, DateTime startTime, DateTime endTime);
        IList<int> CalLineStopTime(string line, string station, DateTime startTime);
        IList<LineStopLogInfo> CalLineStopMillionSecond(string line, string station, DateTime startTime, DateTime endTime);
    }
}
