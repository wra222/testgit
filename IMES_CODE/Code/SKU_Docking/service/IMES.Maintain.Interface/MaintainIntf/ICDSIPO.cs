/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:UI for CDSI PO Page
 *             
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/05/18
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/05/18            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-05-18  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface ICDSIPO
    {
        IList<SnoDetPoMoInfo> GetList();
        void Add(SnoDetPoMoInfo item);
        void Delete(string id);
        void Update(string id, SnoDetPoMoInfo item);
        bool CheckIfExist(string id);
        bool CheckIfExistProduct(string id);
        string GetMOByProductID(string id);
    }
}
