using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.FisObject.PCA.MBChangeLog
{
    public interface IMBChangeLogRepository : IRepository<MBChangeLog>
    {
        /// <summary>
        /// Select * from Change_PCB nolock where OldPCBNo = @OldMBSno or NewPCBNo = @NewMBSno
        /// 
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是OR关系.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<MBChangeLog> GetMBChangeLogs(MBChangeLog condition);
    }
}
