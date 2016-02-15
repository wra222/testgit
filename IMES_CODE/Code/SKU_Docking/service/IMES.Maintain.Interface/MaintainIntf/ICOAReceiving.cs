using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface ICOAReceiving
    {
        /// <summary>
        /// 将用户上传的文件数据存放在临时表中
        /// </summary>
        /// <param name="dataLst"></param>
        void SaveTXTIntoTmpTable(IList<COAReceivingDef> dataLst);
        /// <summary>
        /// 清除当前client用户之前上传的数据.
        /// </summary>
        /// <param name="pc">client端的ip</param>
         void RemoveTmpTableItem(string pc);
        /// <summary>
        /// 根据pc查询上传过的数据.
        /// </summary>
        /// <param name="pc"></param>
         IList<COAReceivingDef> GetTmpTableItem(string pc);
        /// <summary>
        /// 将用户上传的数据保存到COAReceiveTable表中
        /// </summary>
        /// <param name="pc">client端ip</param>
        bool saveItemIntoCOAReceiveTable(string pc);

        IList<COAReceivingDef> ReadTXTFile(string path,string username,string ip);
        
    }
}
