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

namespace IMES.Maintain.Interface
{
    public interface IAllKindsOfType
    {
        //SELECT DISTINCT Area as Area
	    //FROM TraceStd
	    //ORDER BY Area
        IList<AreaDef> GetAreaList();

        //SELECT DISTINCT CASE (CHARINDEX(' ', Family) - 1) WHEN -1 THEN Family
		//ELSE SUBSTRING(Family, 1, (CHARINDEX(' ', Family) - 1)) END AS Family 
	    //FROM IMES_GetData..Model
	    //WHERE LEFT(Model, 4) <> '1397'
		//AND ISNULL(Family, '') <> ''
	    //ORDER BY Family

       
        IList<string> GetFamilyList();  

        //IF @Family <> '' AND @Area <> ''
	    //SELECT Family, Area as Area, Type as Type, Editor, Cdt as [Create Date],Id
		//FROM TraceStd 
		//WHERE Family = @Family
		//	AND Area = @Area
		//ORDER BY Family, Area

        //IF @Family <> '' AND @Area = ''
	    //SELECT Family, Area as Area, Type as Type, Editor, Cdt as [Create Date] ,Id
		//FROM TraceStd 
		//WHERE Family = @Family
		//ORDER BY Family, Area
        //IF @Family = '' AND @Area <> ''
	    //SELECT Family, Area as Area, Type as Type, Editor, Cdt as [Create Date] ,Id
		//FROM TraceStd 
		//WHERE Area = @Area
		//ORDER BY Family, Area

        //IF @Family = '' AND @Area = ''
	    //SELECT Family, Area as Area, Type as Type, Editor, Cdt as [Create Date] ,Id
		//FROM TraceStd 
		//ORDER BY Family, Area

        IList<TraceStdInfo> GetTraceStdList(string family, string area);


        //IF NOT EXISTS(SELECT * FROM [IMES_FA].[dbo].[TraceStd]
        //          WHERE Family = @Family AND Area = @Area AND Type = @Type)
        //INSERT INTO [IMES_FA].[dbo].[TraceStd]([Family],[Area],[Type],[Editor],[Cdt])
        //           VALUES(@Family, @Area, @Type, @Editor, GETDATE())

        void SaveAllKindsOfTypeInfo(TraceStdInfo item);


        //DELETE FROM [IMES_FA].[dbo].[TraceStd]
        // WHERE Family = @Family AND Area = @Area AND Type = @Type

        void DeleteResult(TraceStdInfo item);

        //SELECT  Id 
        //FROM [IMES_FA].[dbo].[TraceStd]
        //WHERE Family = @Family AND Area = @Area AND Type = @Type)

        int GetId(TraceStdInfo item);

        void UpdateAllKindsOfTypeInfo(TraceStdInfo item);

        //void SaveAllKindsOfTypeInfo(IMES.DataModel.TraceStdInfo item);

        int CheckExistsRecord(TraceStdInfo item);
    }


}
