using System.Text;
using IMES.DataModel;
using System.Data;
using System.Collections.Generic;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IPilotRunPrintInfo
    {

        //SELECT Build FROM PilotRunPrintBuild ORDER BY Build
        IList<SelectInfoDef> GetBuildList();


        //IF NOT EXISTS(SELECT * FROM PilotRunPrintBuild WHERE Build = @Build)
        //    INSERT INTO [PilotRunPrintBuild]([Build],[Editor],[Cdt])
        //        VALUES(@Build, @Editor, GETDATE())
        void AddBuild(string Build, string editor);

        //判断Build是否正在被使用
        //Boolean IsBuildInUse(string Build)
        //IF EXISTS(SELECT * FROM [PilotRunPrintInfo] where [Build]= 'Build')
        //返回true
        //否则false

        //DELETE FROM PilotRunPrintBuild WHERE Build = @Build
        void DeleteBuild(string build);


        //SELECT [Type] FROM PilotRunPrintType
        //    ORDER BY [Type]
        IList<SelectInfoDef> GetPrintTypeList();


        //    IF NOT EXISTS(SELECT * FROM PilotRunPrintType WHERE Type = @Type)
        //INSERT INTO PilotRunPrintType([Type])
        //    VALUES(@Type
        void AddPrintType(string type, string editor);

        //判断type是否正在被使用
        //Boolean IsPrintTypeInUse(string type)
        //IF EXISTS( SELECT *  FROM [PilotRunPrintInfo] WHERE [Type]='type')
        //返回true
        //否则false

        //DELETE FROM PilotRunPrintType WHERE Type = @Type
        void DeletePrintType(string type);

        //判断是否存在Build
        //Boolean IsBuildExist(string Build)
        //IF EXISTS( SELECT *  FROM [PilotRunPrintBuild] WHERE [Build]='Build')
        //返回true
        //否则false

        //判断是否存在PrintType
        //Boolean IsPrintTypeExist(string Type)
        //IF EXISTS( SELECT * FROM [PilotRunPrintType] WHERE [Type]='Type')
        //返回true
        //否则false

        //UPDATE PilotRunPrintInfo SET Family = @Family, Build = @Build, SKU = @SKU, Editor = @Editor, Udt = GETDATE()
        //WHERE Model = @Model
        void BSUpdate(string family,string build, string sku, string model, string editor);


        //SELECT a.Family, a.Model, a.Build, a.SKU, a.[Type], a.Descr as [Description], a.Editor, a.Cdt, a.Udt, a.ID 
        //    FROM PilotRunPrintInfo a, PilotRunPrintType b
        //    WHERE a.Model = @Model
        //        AND a.Type = b.Type
        //    ORDER BY b.ID
        DataTable GetPrintInfoList(string model);

        //判断是否存在相同Model和Type的记录
        //Boolean IsExistPrintInfo(string model, string type)
        //IF EXISTS(SELECT * FROM PilotRunPrintInfo WHERE Model = @Model AND Type = @Type)
        //返回true
        //否则false

        //UPDATE PilotRunPrintInfo SET Descr = @Description, Editor = @Editor, Udt = GETDATE()
        //        WHERE ID=id
        void UpdatePrintInfo(PilotRunPrintInfo item);


        //INSERT INTO [PilotRunPrintInfo]([Family],[Model],[Build],[SKU],[Type],[Descr],[Editor],[Cdt],[Udt])
        //        VALUES (@Family, @Model, @Build, @SKU, @Type, @Description, @Editor, GETDATE(), GETDATE())
        //需要在返回的ITEM的ID中填上当前新加入的记录的ID
        string AddPrintInfo(PilotRunPrintInfo item);

        //DELETE FROM PilotRunPrintInfo WHERE ID=id
        void DeletePrintInfo(string id);

    }
}
