using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using System.Data;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;

namespace IMES.Maintain.Implementation
{
    public class LightStationManager : MarshalByRefObject,ILightStation
    {
        //SELECT RTRIM(Code) as Code, RTRIM([Type]) as Type, RTRIM(Descr) as Description, 
        //           RTRIM(Remark) as Remark, RTRIM(Editor) as Editor, Cdt as [Create Date], Udt as [Update Date]
        // FROM KittingCode WHERE [Type]='Kitting' 
        // ORDER BY Code, Description
        public DataTable GetLightStationList()
        {
            DataTable result;
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                result = itemRepository.GetLightStationList();
                if (result == null)
                {
                    return new DataTable();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        //IF EXISTS(SELECT * FROM [IMES_FA].[dbo].[KittingCode] WHERE Code = @Code AND [Type] = 'Kitting')
        //         UPDATE [IMES_FA].[dbo].[KittingCode]
        //                   SET Descr = @Description,Remark = @Remark,Editor = @Remark,Udt = GETDATE()
        //                   WHERE Code = @Code AND [Type] = 'Kitting' ELSE INSERT INTO [IMES_FA].[dbo].[KittingCode]
        //([Code],[Type],[Descr],[Remark],[Editor],[Cdt],[Udt]) VALUES(@Code, 'Kitting', @Description, @Remark, @Editor, GETDATE(), GETDATE())
        public void SaveKittingCode(KittingCodeDef item)
        {
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();

                KittingCode newItem = new KittingCode();
                newItem.Code = item.Code;
                newItem.Editor = item.Editor;
                newItem.Descr = item.Descr;
                newItem.Remark = item.Remark;
                newItem.Type = "Kitting"; 
                itemRepository.SaveKittingCode(newItem);               

            }
            catch (Exception)
            {
                throw;
            }
        }


        //IF EXISTS(SELECT * FROM [IMES_FA].[dbo].[KittingCode] WHERE Code = @Code AND [Type] = 'Kitting')
        //返回true
        //否则false
        //bool IsKittingCodeExist(string code);


        //UPDATE [IMES_FA].[dbo].[KittingCode]
        // SET Descr = @Description,Remark = @Remark,Editor = @Remark,Udt = GETDATE()
        //WHERE Code = @Code AND [Type] = 'Kitting'
        public string UpdateKittingCode(string code, string descr, string remark, string editor)
        {
            string result=code;
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                itemRepository.UpdateKittingCode(code, descr, remark,editor);

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        //INSERT INTO [IMES_FA].[dbo].[KittingCode]
        // ([Code],[Type],[Descr],[Remark],[Editor],[Cdt],[Udt]) VALUES(@Code, 'Kitting', 
        //@Description, @Remark, @Editor, GETDATE(), GETDATE())
        public string AddKittingCode(KittingCodeDef item)
        {
            string result = "";
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();

                Boolean isExistKittingCode=itemRepository.IsKittingCodeExist(item.Code);
                if (isExistKittingCode == true)
                {
                    //已经存在具有相同Code, 类型为Kitting的KittingCode记录
                    //!!!need change
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT052", erpara);
                    throw ex;
                }
                KittingCode newItem = new KittingCode();
                newItem.Code = item.Code;
                newItem.Editor = item.Editor;
                newItem.Descr = item.Descr;
                newItem.Remark = item.Remark;
                newItem.Type = "Kitting";
                newItem.Cdt = DateTime.Now;
                newItem.Udt = DateTime.Now;
                itemRepository.AddKittingCode(newItem);
                result = newItem.Code.ToString();

            }
            catch (Exception)
            {
                throw;
            }
            return result;

        }


        //DELETE FROM [IMES_FA].[dbo].[KittingCode] WHERE Code = @Code AND [Type] = 'Kitting'
        public void DeleteKittingCode(string code)
        {
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                itemRepository.DeleteKittingCode(code);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
