using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.FisObject.Common.Part;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Text.RegularExpressions;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.Common.Misc;
using Metas = IMES.Infrastructure.Repository._Metas;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Repository._Schema;

namespace IMES.Maintain.Implementation
{
    public class ModelAssemblyCode : MarshalByRefObject, IModelAssemblyCode
   {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
        IMiscRepository miscRep = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
        public IList<ModelAssemblyCodeInfo> GetAllModelAssemblyCodeInfo()
        {
            try
            {
                IList<ModelAssemblyCodeInfo> ModelAssemblyCodeInfoList = miscRep.GetData<Metas.ModelAssemblyCode, ModelAssemblyCodeInfo>(null);
                return ModelAssemblyCodeInfoList;

            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw;
            }
    
        }
        public void AddModelAssemblyCodeInfo(ModelAssemblyCodeInfo item)
        {
            CheckDuplicateData(item.Model, item.AssemblyCode);
            miscRep.InsertData<IMES.Infrastructure.Repository._Metas.ModelAssemblyCode, ModelAssemblyCodeInfo>(item);
        }
        public void DelModelAssemblyCodeInfo(string model, string assemblycode)
        {
            ModelAssemblyCodeInfo item = new ModelAssemblyCodeInfo { Model = model, AssemblyCode = assemblycode };
            miscRep.DeleteData<IMES.Infrastructure.Repository._Metas.ModelAssemblyCode, ModelAssemblyCodeInfo>(item);
        }
        public void UpdateModelAssemblyCodeInfo(ModelAssemblyCodeInfo item, string model, string assemblycode)
        {
             ModelAssemblyCodeInfo condition = new ModelAssemblyCodeInfo { Model = model, AssemblyCode = assemblycode };
             if (item.Model != model)
             {
                 CheckDuplicateData(item.Model, item.AssemblyCode);
             }
             miscRep.UpdateData<IMES.Infrastructure.Repository._Metas.ModelAssemblyCode, ModelAssemblyCodeInfo>(condition, item);
        }
        public void CheckDuplicateData(string model, string assemblycode)
        {
            ModelAssemblyCodeInfo item = new ModelAssemblyCodeInfo { Model = model };
            IList<ModelAssemblyCodeInfo> astDefineInfoList = miscRep.GetData<Metas.ModelAssemblyCode, ModelAssemblyCodeInfo>(item);
            if (astDefineInfoList.Count > 0)
            {
                throw new FisException("Duplicate Model!");
            }
        }
   }
}
