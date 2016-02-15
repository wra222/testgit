/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:Service for APT maintain Page
 *             
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN For RCTO.docx
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN For RCTO.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-07-18  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IActualProductionTime
    {
        IList<ConstValueInfo> GetProductionCauseList();
        IList<SmttimeInfo> GetSMTTimeInfoList(DateTime date);
        void AddSMTTimeInfo(SmttimeInfo item);
        void UpdateSMTTimeInfo(SmttimeInfo cond, SmttimeInfo item);
        void DeleteSMTTimeInfo(SmttimeInfo cond);
        bool CheckExistSMTTimeInfo(SmttimeInfo cond);
        IList<SMTLineDef> GetSMTLineInfoListByLineList(IList<string> lineList);
        IList<DeptInfo> GetDeptInfoList();
    }
}
