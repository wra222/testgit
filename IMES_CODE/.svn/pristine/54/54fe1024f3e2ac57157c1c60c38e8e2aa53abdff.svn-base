using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure;
using IMES.DataModel;

namespace IMES.FisObject.Common.Location
{
    public interface ILocationRepository : IRepository<FloorAreaLoc>
    {
        void AssignLocAndMoveIn(string productID, string productAttrName,string floor, 
                                                string area, string category, string model, string unit,
                                                string editor, int fullQty,string station, string palletType);
        void AssignLocAndMoveInDefered(IUnitOfWork uow, string productID, string productAttrName, string floor,
                                                            string area, string category, string model, string unit, string editor, int fullQty,
                                                            string station,string palletType);
        void LocMoveOut(FloorAreaLoc loc, string productAttrName,string station, string editor);
        void LocMoveOutDefered(IUnitOfWork uow, FloorAreaLoc loc, string productAttrName, string station, string editor);
        IList<FloorAreaLoc> GetLocation(FloorAreaLoc condition);

        void LocMoveOutByProductIDList(IList<string> productIDList, string productAttrName,string station, string editor);
        void LocMoveOutByProductIDListDefered(IUnitOfWork uow,IList<string> productIDList, string productAttrName,string station, string editor);

        void TransferToLoc(IList<string> productIDList, string productAttrName, FloorAreaLoc srcLoc,string floor, string area,
                                                string editor, string station);
        void TransferToLocDefered(IUnitOfWork uow, IList<string> productIDList, string productAttrName, FloorAreaLoc srcLoc, string floor,
                                                            string area, string editor, string station);

        IList<string> GetProductIDListByLoc(string productAttrName,string locID);
        void UpdateProductAttr(string attrName, string oldValue, string newValue, string station, string editor);
        void UpdateProductAttrDefered(IUnitOfWork uow, string attrName, string oldValue, string newValue, string station, string editor);

        void UpdateProductListAttr(IList<string> productIDList, string attrName, string value, string station, string editor);
        void UpdateProductListAttrDefered(IUnitOfWork uow, IList<string> productIDList, string attrName, string value, string station, string editor);
        IList<AttributeInfo> GetProductListAttr(IList<string> productIDList, string attrName);

    }
}
