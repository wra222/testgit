using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.PCA.MBModel;
using IMES.FisObject.Common.Line;
using IMES.FisObject.PCA.MBMO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.Misc;
using IMES.FisObject.Common.MO;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Warranty;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.NumControl;
using System.Data;

namespace IMES.Maintain.Implementation
{
    public class PilotRunPrintInfoManager : MarshalByRefObject,IPilotRunPrintInfo
    {

        //SELECT Build FROM PilotRunPrintBuild ORDER BY Build
        public IList<SelectInfoDef> GetBuildList()
        {
            List<SelectInfoDef> result = new List<SelectInfoDef>();
            try
            {
                IMiscRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                DataTable getData = itemRepository.GetBuildList();
                for (int i = 0; i < getData.Rows.Count; i++)
                {
                    SelectInfoDef item = new SelectInfoDef();
                    item.Text = Null2String(getData.Rows[i][0]);
                    item.Value = item.Text;
                    result.Add(item);
                }

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }


        //IF NOT EXISTS(SELECT * FROM PilotRunPrintBuild WHERE Build = @Build)
        //    INSERT INTO [PilotRunPrintBuild]([Build],[Editor],[Cdt])
        //        VALUES(@Build, @Editor, GETDATE())
        public void AddBuild(string Build, string editor)
        {
            try
            {
                IMiscRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                itemRepository.AddBuild(Build, editor);
            }
            catch (Exception)
            {
                throw;
            }

        }

        //判断Build是否正在被使用
        //Boolean IsBuildInUse(string Build)
        //IF EXISTS(SELECT * FROM [PilotRunPrintInfo] where [Build]= 'Build')
        //返回true
        //否则false

        //DELETE FROM PilotRunPrintBuild WHERE Build = @Build
        public void DeleteBuild(string build)
        {
            try
            {
                IMiscRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                Boolean isInUse = itemRepository.IsBuildInUse(build);
                if (isInUse != true)
                {
                    itemRepository.DeleteBuild(build);
                }
                else
                {
                    //这个Build已被使用，不能删除
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT044", erpara);
                    throw ex;

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        //SELECT [Type] FROM PilotRunPrintType
        //    ORDER BY [Type]
        public IList<SelectInfoDef> GetPrintTypeList()
        {
            List<SelectInfoDef> result = new List<SelectInfoDef>();
            try
            {
                IMiscRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                DataTable getData = itemRepository.GetPrintTypeList();
                for (int i = 0; i < getData.Rows.Count; i++)
                {
                    SelectInfoDef item = new SelectInfoDef();
                    item.Text = Null2String(getData.Rows[i][0]);
                    item.Value = item.Text;
                    result.Add(item);
                }

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
        //    IF NOT EXISTS(SELECT * FROM PilotRunPrintType WHERE Type = @Type)
        //INSERT INTO PilotRunPrintType([Type])
        //    VALUES(@Type
        public void AddPrintType(string type, string editor)
        {
            try
            {
                IMiscRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                itemRepository.AddPrintType(type);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //判断type是否正在被使用
        //Boolean IsPrintTypeInUse(string type)
        //IF EXISTS( SELECT *  FROM [PilotRunPrintInfo] WHERE [Type]='type')
        //返回true
        //否则false

        //DELETE FROM PilotRunPrintType WHERE Type = @Type
        public void DeletePrintType(string type)
        {
            try
            {
                IMiscRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                Boolean isInUse = itemRepository.IsPrintTypeInUse(type);
                if (isInUse != true)
                {
                    itemRepository.DeletePrintType(type);
                }
                else
                {
                    //这个Type已被使用，不能删除
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT045", erpara);
                    throw ex;

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

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
        public void BSUpdate(string family, string build, string sku, string model, string editor)
        {
            try
            {
                IMiscRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                Boolean isExist = itemRepository.IsBuildExist(build);
                if (isExist == true)
                {
                    itemRepository.BSUpdate(family,build,sku,model,editor);
                }
                else
                {
                    //选择的Build在数据库中已不存在
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT046", erpara);
                    throw ex;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        //SELECT a.Family, a.Model, a.Build, a.SKU, a.[Type], a.Descr as [Description], a.Editor, a.Cdt, a.Udt, a.ID 
        //    FROM PilotRunPrintInfo a, PilotRunPrintType b
        //    WHERE a.Model = @Model
        //        AND a.Type = b.Type
        //    ORDER BY b.ID
        public DataTable GetPrintInfoList(string model)
        {
            DataTable result=new DataTable();
            try
            {
                IMiscRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                result = itemRepository.GetPrintInfoList(model);

            }
            catch (Exception)
            {
                throw;
            }

            return result;

        }



        //UPDATE PilotRunPrintInfo SET Descr = @Description, Editor = @Editor, Udt = GETDATE()
        //        WHERE ID=id
        public void UpdatePrintInfo(PilotRunPrintInfo item)
        {
            try
            {
                IMiscRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();

                Boolean isExistBuild = itemRepository.IsBuildExist(item.Build);
                if (isExistBuild != true)
                {
                    //选择的Build在数据库中已不存在
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT046", erpara);
                    throw ex;
                }

                Boolean isExistType = itemRepository.IsPrintTypeExist(item.Type);
                if (isExistType != true)
                {
                    //选择的Type在数据库中已不存在
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT047", erpara);
                    throw ex;
                }

                UnitOfWork uow = new UnitOfWork();
                itemRepository.UpdatePrintInfoDefered(uow, item.ID, item.Descr, item.Editor);
                itemRepository.BSUpdateDefered(uow,item.Family, item.Build, item.SKU, item.Model, item.Editor);
                uow.Commit();
             
            }
            catch (Exception)
            {
                throw;
            }

        }

        //判断是否存在相同Model和Type的记录
        //Boolean IsExistPrintInfo(string model, string type)
        //IF EXISTS(SELECT * FROM PilotRunPrintInfo WHERE Model = @Model AND Type = @Type)
        //返回true
        //否则false

        //INSERT INTO [PilotRunPrintInfo]([Family],[Model],[Build],[SKU],[Type],[Descr],[Editor],[Cdt],[Udt])
        //        VALUES (@Family, @Model, @Build, @SKU, @Type, @Description, @Editor, GETDATE(), GETDATE())
        //需要在返回的ITEM的ID中填上当前新加入的记录的ID
        public string AddPrintInfo(PilotRunPrintInfo item)
        {
            string result = "";
            try
            {
                IMiscRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                Boolean isExistBuild = itemRepository.IsBuildExist(item.Build);
                if (isExistBuild != true)
                {
                    //选择的Build在数据库中已不存在
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT046", erpara);
                    throw ex;
                }

                Boolean isExistType = itemRepository.IsPrintTypeExist(item.Type);
                if (isExistType != true)
                {
                    //选择的Type在数据库中已不存在
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT047", erpara);
                    throw ex;
                }

                Boolean isExistPrintInfo = itemRepository.IsExistPrintInfo(item.Model,item.Type);
                if (isExistPrintInfo == true)
                {
                    //已经存在具有相同Model、Type的PilotRunPrintInfo记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT048", erpara);
                    throw ex;
                }

                item.Cdt = DateTime.Now;
                item.Udt = DateTime.Now;
                UnitOfWork uow = new UnitOfWork();
                itemRepository.AddPrintInfoDefered(uow,item);
                itemRepository.BSUpdateDefered(uow,item.Family, item.Build, item.SKU, item.Model, item.Editor);
                uow.Commit();
                result = item.ID.ToString();

            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        private static String Null2String(Object _input)
        {
            if (_input == null)
            {
                return "";
            }
            return _input.ToString().Trim();
        }

        //DELETE FROM PilotRunPrintInfo WHERE ID=id
        public void DeletePrintInfo(string id)
        {
            try
            {
                IMiscRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                int iID = Int32.Parse(id);
                itemRepository.DeletePrintInfo(iID);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
