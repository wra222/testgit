using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IWLBTDescr
    {
        //取得Part
        PartMaintainInfo GetPartByPartNo(string partNo);

        //取得Part的Type Description
        IList<string> GetPartTypeDescr(string infoType, string partNo);

        //取得全部的WLBTDescr
        IList<WLBTDescr> GetAllWLBTDescr();


        //取得某一Part的WLBTDescr
        IList<WLBTDescr> GetWLBTDescrListByPartNo(string partNo);

        //判断WLBTDescr是否已经存在
        int IFWLBTDescrIsExists(WLBTDescr descr);


        //更新WLBTDescr
        void UpdateWLBTDescr(WLBTDescr descr);

        //新增WLBTDescr纪录, 返回ID
        int InsertWLBTDescr(WLBTDescr descr);

        //删除WLBTDescr纪录
        void deleteWLBTDescr(string id);
    }
}
