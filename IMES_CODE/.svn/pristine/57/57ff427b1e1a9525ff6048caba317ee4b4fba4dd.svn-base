using System.Text;
using IMES.DataModel;
using System.Data;
using System.Collections.Generic;
using System;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface ILightNo
    {

        //1）取KittingCode表格
        //    SELECT [Line] AS KittingCode
        //    ,[Descr]
        //    ,'Yes' 
        //    FROM [Line] where [Stage]='stage' 
        //    UNION 
        //    SELECT [Family] AS KittingCode
        //    ,[Descr] 
        //    ,'' 
        //    FROM [Family] ORDER BY KittingCode //'FA', 'PAK'情况下

        DataTable GetKittingCodeListFromLine(String stage);

        //2）取KittingCode表格
        //SELECT Code AS KittingCode
        //,[Descr] 
        //'' 
        //FROM LabelKitting 
        //WHERE Type='type'
        //ORDER BY KittingCode  //'FA Label','FA Label'情况下

        DataTable GetKittingCodeList(String type);

        //3)取PDLine列表
        //'FA', 'PAK'情况下
        //SELECT [Line] AS pdLine where [Stage]='stage' ORDER BY [Line]
        IList<SelectInfoDef> GetPdLineList(String stage);

        //4)取lightNo列表信息
        //select distinct b.Code,b.PartNo,b.Tp,convert(int,b.LightNo) as LightNo,Qty,Sub,Safety_Stock,Max_Stock,b.Remark 
        //from (select PartType from IMES2012_GetData.dbo.KitLoc where PdLine = @code) a, IMES2012_FA.dbo.WipBuffer b where b.Tp like a.PartType and b.Code=@code and 
        //b.KittingType = @kittingType order by convert(int,b.LightNo)
        DataTable GetLightNoList(string kittingType, string code);
        
        //select distinct b.Code,b.PartNo,b.Tp,convert(int,b.LightNo) as LightNo,Qty,Sub,Safety_Stock,Max_Stock,b.Remark 
        //from (select PartType from IMES2012_GetData.dbo.KitLoc where PdLine = @code) a, IMES2012_FA.dbo.WipBuffer b where b.Tp like a.PartType and b.Code=@code and 
        //b.KittingType = @kittingType
        //AND Tp not like 'DDD Kitting%' order by convert(int,b.LightNo)
        DataTable GetLightNoListPAK(string kittingType, string code);
        
        //6) select distinct Code,PartNo,Tp,convert(int,LightNo) as LightNo,Qty,Sub,Safety_Stock,Max_Stock,Remark 
        //from IMES2012_FA.dbo.WipBuffer where Code=@code and KittingType = @kittingType 
        //order by convert(int,LightNo)
        DataTable GetLightNoListFami(string kittingType, string code);
        
        //7) select distinct Code,PartNo,Tp,convert(int,LightNo) as LightNo,Qty,Sub,Safety_Stock,Max_Stock,Remark 
        //from IMES2012_FA.dbo.WipBuffer where Code=@code and KittingType = @kittingType 
        //AND Tp not like 'DDD Kitting%' 
        //order by convert(int,LightNo)
 
        DataTable GetLightNoListFamiPAK(string kittingType, string code);
        
        //新增的id返回到结构里
        string AddLightNo(WipBuffer item,Boolean codeIsLine);        

        //9) 更新的数据
        void UpdateLightNo(WipBuffer item, Boolean codeIsLine);
        
        //10) DELETE FROM [WipBuffer]
        //WHERE ID='id'
        void DeleteLightNo(int id);
        

        //11) SELECT Descr 
        //FROM [Part]
        //where [PartNo]='partNo'
        
        DataTable GetPartInfoByPartNo(string partNo);
        

        ////12) SELECT [ID] 
        ////FROM [WipBuffer]
        ////where Code='code' AND PartNo='partNo'
        //Boolean ExistWipBuffer(string code, string partNo);
        

        ////SELECT [ID] 
        ////FROM [WipBuffer]
        ////where Code='code' AND PartNo='partNo' AND ID<>'id'
        //Boolean ExistWipBufferExceptCode(string code, string partNo, int id)
        

        //15) SELECT distinct(PartType)
        //FROM KitLoc
        //ORDER BY PartType
        IList<SelectInfoDef> GetLightNoPartType();
        


        IList<SelectInfoDef> GetLightNoStationList(String type);
        //16)执行存储过程
        //DataTable ExecOpKittingAutoCheck(string code, string lightNo, string kittingType)
        //Exec op_KittingAutoCheck 'code','lightNo','kittingType'
        //返回存储过程中的select数据
        
        //DELETE FROM [TmpKit]
        //WHERE PdLine='curLine'
        //INSERT INTO TmpKit
        //(PdLine
        //,Model
        //)
        //VALUES
        //(@line
        //,@model)
        void ImportTmpKit(List<TmpKitInfoDef> items, string curLine,string type);
      
        //DELETE FROM [TmpPAKKit]
        //WHERE PdLine='curLine'
        //INSERT INTO TmpPAKKit
        //(PdLine
        //,Model
        //,Qty
        //)
        //VALUES
        //(@line
        //,@model
        //,Qty) 
        void ImportTmpPAKKit(List<TmpKitInfoDef> items, string curLine, string type);        

        //19) 执行存储过程 For FA
        //DataTable ExecOpKittingLocCheck(string pdline, string partType)
        //Exec op_KittingLocCheck @pdline,@partType
        //返回存储过程中的select数据
        void UploadModelForWipBufferFA(string pdLine, string parkType);

        //20) 执行存储过程For PAK
        //DataTable ExecOpPAKKitLocFV(string pdline)
        //Exec op_PAKKitLoc_FV @pdline 
        //返回存储过程中的select数据
        DataTable UploadModelForWipBufferPAK(string pdLine);


        ///////////////////////////////////////////////////////////////////////////////////////////////


        ////SELECT RTRIM(Code), RTRIM(Descr) as [Description] FROM KittingCode WHERE [Type] = 'Kitting' ORDER BY Code, Descr
        //DataTable GetTabKittingCodeList();

        ////SELECT DISTINCT RTRIM(Code), RTRIM(PartNo) as [Part No], RTRIM([Tp]) as [Type], 
        ////CONVERT(int, LightNo) as LightNo,
        ////                   Qty, RTRIM(Sub) as Substitution, Safety_Stock as [Safety Stock], 
        ////Max_Stock as [Max Stock],
        ////                   Remark, Editor, Cdt as [Create Date], Udt as [Update Date], [ID]
        ////         FROM WipBuffer b
        ////         WHERE Code = @Code
        ////         ORDER BY CONVERT(int, LightNo), [Part No]
        //DataTable GetLightNoListByCode(string code);
 
        ////SELECT * FROM IMES_GetData..Part WHERE PartNo = @PartNo
        ////DataTable GetExistPartNo(string partNo);

        ////IF EXISTS(SELECT * FROM WipBuffer WHERE Code = @Code AND PartNo = @PartNo)
        ////         UPDATE [IMES_FA].[dbo].[WipBuffer]
        ////                   SET LightNo = @LightNo, Qty = @Qty, Sub = @Sub, Safety_Stock = @SafetyStock,
        ////                            Max_Stock = @MaxStock, Remark = @Remark, Editor = @Editor, Udt = GETDATE()
        ////                   WHERE Code = @Code AND PartNo = @PartNo ELSE
        ////         INSERT INTO [IMES_FA].[dbo].[WipBuffer]([Code],[PartNo],[Tp],[LightNo],[Picture],[Qty],[Sub]
        ////                               ,[Safety_Stock],[Max_Stock],[Remark],[Editor],[Cdt],[Udt])
        ////                   SELECT @Code, @PartNo, PartType, @LightNo, '', @Qty, @Substitution, @SafetyStock, @MaxStock,
        ////                            @Remark, @Editor, GETDATE(), GETDATE() 
        ////                            FROM IMES_GetData..Part WHERE PartNo = @PartNo
        //string SaveWipBuffer(WipBufferDef item);
 
        ////DELETE FROM [IMES_FA].[dbo].[WipBuffer] WHERE Code = @Code AND LightNo = @LightNo
        //void DeleteLightNo(string code,string lightNo);

        ////SELECT [ID] FROM WipBuffer WHERE Code = @Code AND PartNo = @PartNo
        ////DataTable  GetWipBufferID(string code, string partNo);


        ////IF EXISTS(SELECT * FROM WipBuffer WHERE Code = @Code AND PartNo = @PartNo)
        ////返回true
        ////否则false
        ////bool IsWipBufferExist(string code, string partNo);

        ////UPDATE [IMES_FA].[dbo].[WipBuffer]
        ////     SET LightNo = @LightNo, Qty = @Qty, Sub = @Sub, Safety_Stock = @SafetyStock,
        ////       Max_Stock = @MaxStock, Remark = @Remark, Editor = @Editor, Udt = GETDATE()
        ////       WHERE ID=id
        //string UpdateWipBuffer(WipBufferDef item);


        ////INSERT INTO [IMES_FA].[dbo].[WipBuffer]([Code],[PartNo],[Tp],[LightNo],[Picture],[Qty],
        ////[Sub],[Safety_Stock],[Max_Stock],[Remark],[Editor],[Cdt],[Udt])
        ////                            SELECT @Code, @PartNo, PartType, @LightNo, '', @Qty, 
        ////@Substitution, @SafetyStock, @MaxStock,
        ////                                    @Remark, @Editor, GETDATE(), GETDATE() 
        ////                                     FROM IMES_GetData..Part
        ////                                     WHERE PartNo = @PartNo AND Flag=1
        ////添加数据后，item中的ID存入该条记录新生成的ID
        //string AddWipBuffer(WipBufferDef item);

        DataTable GetLightNoFromSp(string code, string kittingType, string isLine);

    }
}



