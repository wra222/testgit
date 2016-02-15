using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Util;
//using IMES.DataModel;

namespace IMES.FisObject.PAK.BSam
{
   
    public interface IBSamRepository : IRepository<BSamLocation>
    {
        BSamLocation GetMoveInLoc(string model);
        void MoveInLoc(string model,int fullQtyPerCarton,string editor,  BSamLocation loc);
        void MoveInLocDefered(IUnitOfWork uow, string model, int fullQtyPerCarton, string editor, BSamLocation loc);

        BSamLocation GetAndAssignMoveInLoc(string model, int fullCartonQty,string editor);
        void RollbackMoveInLoc(string locationId, int qty,string editor);
        void RollbackMoveInLocDefered(IUnitOfWork uow,string locationId, int qty, string editor);

        void MoveOutLoc(string locationId,string editor);
        void MoveOutLocDefered(IUnitOfWork uow,string loc,string editor);

        void FillProductIDInLoc(BSamLocation loc);

        void UpdateHoldInLocation(IList<string> Ids, bool isHold, string editor);

        void UpdateHoldOutLocation(IList<string> Ids, bool isHold, string editor);

        BSamModel GetBSamModel(string a_Part_Model);
        void UpdateBSamModelQtyPerCarton(string a_Part_Model, int qtyPerCarton, string editor);
        void UpdateBSamModelQtyPerCartonDefered(IUnitOfWork uow, string a_Part_Model, int qtyPerCarton, string editor);

        IList<BSamLocation> GetBSamLocation(BSamLocationQueryType type,string model);
        IList<string> GetAllBSamModel();
    }
}
