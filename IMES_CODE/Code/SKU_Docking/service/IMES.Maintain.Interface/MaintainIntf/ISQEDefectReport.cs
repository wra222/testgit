using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface ISQEDefectReport
    {
        ///1.取得所输CTNo相关的不良记录信息
        /// <summary>
        /// select a.Line,b.Defect,b.Cause,b.Cdt,b.Udt
        ///from ProductRepair a(nolock), ProductRepair_DefectInfo b(nolock) 
        ///where a.ID=b. ProductRepairID and b.VendorCT=left(@sn,14)
        ///union
        ///select '',MpDefect,'',Udt,Udt from IqcCause1(nolock) where CtLabel=@sn
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        IList<SQEDefectCTNoInfo> GetSQEDefectCTNoInfo(string sn);


        //2.取得partType的方法
        //select left(rtrim(b.Descr),3) as Descr from ProductRepair_DefectInfo a(nolock), Part b(nolock),ModelBom c(nolock) 
        //where left(a.VendorCT,5)=c.Component and a.VendorCT=left(@sn,14) and c.Material=b.PartNo
        //union
        //select left(rtrim(b.Descr),3) as Descr from IqcCause1 a(nolock), Part b(nolock),ModelBom c(nolock) where left
        //(a.CtLabel,5)=c.Component and a.CtLabel=@sn and c.Material=b.PartNo
        IList<string> GetPartTypeSQEDefectReport(string sn);


        //3.获取ProductRepair_DefectInfo 数据
        //select
        //DefectCode,Cause,Obligation,Component,MajorPart,Remark,VendorCT,
        //Editor,Cdt,Udt
        //where VendorCT=@sn
        IList<SQEDefectProductRepairReportInfo> GetSQEDefectProductRepairInfo(string sn);


        //4.添加一条IqcKp数据
        void AddIqcKp(IqcKpDef iqcKp);


        //5.查找IqcKp数据，返回其list
        //select * from IqcKp 
        //where tp=@tp and CtLabel=@CtLabel and Defect=@Defect
        IList<IqcKpDef> GetIqcKpByTypeCtLabelAndDefect(string tp, string CtLabel, string Defect);


        //6.查找IqcKp数据，返回其list
        //select * from IqcKp 
        //where tp=@tp and CtLabel=@CtLabel and Defect=@Defect and Cause = @Cause
        IList<IqcKpDef> GetIqcKpByTypeCtLabelAndDefect(string tp, string CtLabel, string Defect, string Cause);


        //7.更新IqcKp数据；
        //UPDATE IMES2012_FA.dbo.IqcKp
        //SET CtLabel = @CtLabel
        //,Model = @Model
        //,Tp = @Tp
        //,Defect = @Defect
        //,Cause = @Cause,
        //,Location = @Location
        //,Obligation = @Obligation
        //,Remark = @Remark
        //,Result = @Result
        //,Editor = @Editor
        //,Cdt = @Cdt
        //,Udt = @Udt
        //WHERE CtLabel = CtLabel and Tp = tpand Defect = defect
        void UpdateIqcKp(IqcKpDef iqcKp, string tp, string ctLabel, string defect);


        //8.根据type获取DefectCode数据
        //Select * from DefectCode where Type=@type order by Defect
        IList<DefectCodeInfo> GetDefecSQEDefectInfo(string type);

        //9.根据type获取DefectInfo数据
        IList<DefectInfoDef> GetDefectInfoByType(string type);
        //10 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctNo"></param>
        /// <returns></returns>
        IList<string> GetIecpnList(string ctNo);



    }
}
