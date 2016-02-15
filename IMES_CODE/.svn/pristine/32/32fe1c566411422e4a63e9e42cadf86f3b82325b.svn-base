using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IShipType
    {
        //取得全部的ShipType 
        //select * from ShipType order by ShipType 
        List<ShipTypeMaintain> GetAllShipType();

        //判断ShipType是否已经存在 
        //select * from ShipType where ShipType=？
        bool IfShipTypeIsEXists(string shipType);

        //更新ShipType
        //Update ShipType Set Description=?,Editor=?,Udt=getdate() where ShipType=? 
        void UpdateShipType(ShipTypeMaintain shipType);

        //更新ShipType
        void UpdateShipType(ShipTypeMaintain shipType,string oldShipType);

        //插入新的ShipType纪录
        //insert ShipType(ShipType,Description,Editor,Cdt,Udt)values(?,?,?,getdate(),getdate()) 
        void InsertShipType(ShipTypeMaintain shipType);

        //判断ShipType是否已经被Model使用
        //   select count(1) from Model where ShipType=?
        bool IfShipTypeIsInUse(string shipType);

        //取得ShipType
        ShipTypeMaintain GetShipTypeByKey(string shipType);

        //删除ShipType
        void DeleteShipTypeByKey(string shipType);
    }
}
