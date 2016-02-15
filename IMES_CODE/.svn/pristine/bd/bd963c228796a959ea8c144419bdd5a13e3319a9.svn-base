using System.Text;
using IMES.DataModel;
using System.Data;
using System.Collections.Generic;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface ILightStation
    {
        //SELECT RTRIM(Code) as Code, RTRIM([Type]) as Type, RTRIM(Descr) as Description, 
        //           RTRIM(Remark) as Remark, RTRIM(Editor) as Editor, Cdt as [Create Date], Udt as [Update Date]
        // FROM KittingCode WHERE [Type]='Kitting' 
        // ORDER BY Code, Description
        DataTable GetLightStationList();

        //IF EXISTS(SELECT * FROM [IMES_FA].[dbo].[KittingCode] WHERE Code = @Code AND [Type] = 'Kitting')
        //         UPDATE [IMES_FA].[dbo].[KittingCode]
        //                   SET Descr = @Description,Remark = @Remark,Editor = @Remark,Udt = GETDATE()
        //                   WHERE Code = @Code AND [Type] = 'Kitting' ELSE INSERT INTO [IMES_FA].[dbo].[KittingCode]
        //([Code],[Type],[Descr],[Remark],[Editor],[Cdt],[Udt]) VALUES(@Code, 'Kitting', @Description, @Remark, @Editor, GETDATE(), GETDATE())
        void SaveKittingCode(KittingCodeDef item);
 
        //DELETE FROM [IMES_FA].[dbo].[KittingCode] WHERE Code = @Code AND [Type] = 'Kitting'
        void DeleteKittingCode(string code);

        //IF EXISTS(SELECT * FROM [IMES_FA].[dbo].[KittingCode] WHERE Code = @Code AND [Type] = 'Kitting')
        //返回true
        //否则false
        //bool IsKittingCodeExist(string code);

        
        //UPDATE [IMES_FA].[dbo].[KittingCode]
        // SET Descr = @Description,Remark = @Remark,Editor = @Remark,Udt = GETDATE()
        //WHERE Code = @Code AND [Type] = 'Kitting'
        string UpdateKittingCode(string code, string descr,string remark, string editor);

        //INSERT INTO [IMES_FA].[dbo].[KittingCode]
        // ([Code],[Type],[Descr],[Remark],[Editor],[Cdt],[Udt]) VALUES(@Code, 'Kitting', 
        //@Description, @Remark, @Editor, GETDATE(), GETDATE())
        string AddKittingCode(KittingCodeDef item);

    }
}
