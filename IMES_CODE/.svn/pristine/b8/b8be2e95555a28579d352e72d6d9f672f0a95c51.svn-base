/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: 
 *              
 * Update: 
 * Date         Name            Reason 
 * ========== ================= =====================================
 * 2010-2-20   itc210001        create
 * 
 * Known issues:Any restrictions about this file 
 *          
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IDefectMaintain
    {
        IList<DefectCodeInfo> GetDefectCodeList(string type);

        string GetDefect(string type, string defect);

        void DeleteDefectCode(string type, string defect);

        void UpdateDefectCode(DefectCodeInfo dfc);

        void InsertDefectCode(DefectCodeInfo dfc);

        int CheckExistsRecord(string Defect);

        IList<DefectCodeInfo> GetDefectCodeList();

        //根据defect删除defect数据
        //delete from defectCode where Defect = @defect
        void DeleteDefectCode(string defect);

    }
}
