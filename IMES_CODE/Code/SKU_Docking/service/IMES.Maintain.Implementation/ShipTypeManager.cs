using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.FisObject.Common.Model;
using IMES.DataModel;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.Maintain.Implementation
{
    class ShipTypeManager : MarshalByRefObject, IShipType
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
       
        //取得全部的ShipType 
        public List<ShipTypeMaintain> GetAllShipType()
        {
            List<ShipTypeMaintain> shipTypeMaintainList = new List<ShipTypeMaintain>();
            IList<ShipType> fisObjectList = modelRepository.GetAllShipType();

            if (fisObjectList != null)
            {
                foreach (ShipType temp in fisObjectList)
                {
                    ShipTypeMaintain shipTypeMaintain = new ShipTypeMaintain();
                    shipTypeMaintain.shipType = temp.shipType;
                    shipTypeMaintain.Description = temp.Description;
                    shipTypeMaintain.Editor = temp.Editor;
                    shipTypeMaintain.Cdt = temp.Cdt;
                    shipTypeMaintain.Udt = temp.Udt;

                    shipTypeMaintainList.Add(shipTypeMaintain);
                }
            }

            return shipTypeMaintainList;
        }

        //判断ShipType是否已经存在 
        public bool IfShipTypeIsEXists(string shipType)
        {
            return modelRepository.IfShipTypeIsEXists(shipType);
        }

        //更新ShipType
        public void UpdateShipType(ShipTypeMaintain shipType)
        {
            ShipType fisObject = new ShipType();
            fisObject.shipType = shipType.shipType;
            fisObject.Description = shipType.Description;
            fisObject.Editor = shipType.Editor;
            fisObject.Udt = DateTime.Now;
            modelRepository.UpdateShipType(fisObject);
        }

        //更新ShipType
        public void UpdateShipType(ShipTypeMaintain shipType, string oldShipType)
        {
            ShipType fisObject = new ShipType();
            fisObject.shipType = shipType.shipType;
            fisObject.Description = shipType.Description;
            fisObject.Editor = shipType.Editor;
            fisObject.Cdt = DateTime.Now;
            fisObject.Udt = DateTime.Now; 
            UnitOfWork uow = new UnitOfWork();
            modelRepository.DeleteShipTypeByKeyDefered(uow, oldShipType);
            modelRepository.InsertShipTypeDefered(uow, fisObject);
            uow.Commit();
        }

        //插入新的ShipType纪录
        public void InsertShipType(ShipTypeMaintain shipType)
        {
            ShipType fisObject = new ShipType();
            fisObject.shipType = shipType.shipType;
            fisObject.Description = shipType.Description;
            fisObject.Editor = shipType.Editor;
            fisObject.Cdt = DateTime.Now;
            fisObject.Udt = DateTime.Now;
            modelRepository.InsertShipType(fisObject);
        }

        //判断ShipType是否已经被Model使用
        public bool IfShipTypeIsInUse(string shipType)
        {
            return modelRepository.IfShipTypeIsInUse(shipType);
        }

        //取得ShipType
        public ShipTypeMaintain GetShipTypeByKey(string shipType)
        {
            ShipType temp = modelRepository.GetShipTypeByKey(shipType);
            ShipTypeMaintain shipTypeMaintain = new ShipTypeMaintain();
            shipTypeMaintain.shipType = temp.shipType;
            shipTypeMaintain.Description = temp.Description;
            shipTypeMaintain.Editor = temp.Editor;
            shipTypeMaintain.Cdt = temp.Cdt;
            shipTypeMaintain.Udt = temp.Udt;
            return shipTypeMaintain;
        }

        //删除ShipType
        public void DeleteShipTypeByKey(string shipType)
        {
            modelRepository.DeleteShipTypeByKey(shipType);
        }
    }
}
