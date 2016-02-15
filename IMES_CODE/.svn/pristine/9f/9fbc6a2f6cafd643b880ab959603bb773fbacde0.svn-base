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
    public interface IFAITCNDefectCheck
    {
        //操作FA_ITCNDefect_Check表
        IList<FaItCnDefectCheckInfo> GetDefectCheckList();

        //插入新数据
        string InsertDefectCheck(FaItCnDefectCheckInfo defectItem);
        
        //根据ID更新数据
        void UpdateDefectCheck(FaItCnDefectCheckInfo defectItem);

        //根据ID删除数据
        void DeleteDefectCheck(int ID);

        int CheckExistsRecord(string Defect);

        IList<DefectCodeInfo> GetDefectCodeList();

        string GetDefect(string type, string defect);

        //获取defect列表
        //select * from DefectCode order by Defect
        IList<DefectCodeInfo> GetDefectCodeLst();

    }
}
