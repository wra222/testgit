using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.FisObject.PCA.MB;
using log4net;
using IMES.DataModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PCA.MBMO;

namespace IMES.Maintain.Implementation
{
    public class MBMaintainManager : MarshalByRefObject, IRCTOMBMaintain
    {

        #region IRCTOMaintain 成员
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
        IMBMORepository mbmoRepository = RepositoryFactory.GetInstance().GetRepository<IMBMORepository, IMBMO>();

        public IList<string> GetFamilyInfo(string nodeType)
        {
            IList<string> list = new List<string>();
            list = partRepository.GetDescrListFromPartByBomNodeType(nodeType);
            return list;
        }


        public IList<string> GetCodeInfo(string descr)
        {
            IList<string> list = new List<string>();
            list = partRepository.GetPartInfoValueByPartDescr(descr, "MB", "MAC", "T");
            return list;
        }

        public void addMBMaintain(RctombmaintainInfo info)
        {
            IList<RctombmaintainInfo> list = new List<RctombmaintainInfo>();
            RctombmaintainInfo cond = new RctombmaintainInfo();
            cond.family = info.family;
            list = mbmoRepository.GetRctombmaintainInfoList(cond);
            bool bFind = false;
            foreach (RctombmaintainInfo temp in list)
            {
                if (temp.code == info.code && temp.type == info.type)
                {
                    //IMBMORepository::
                    //void UpdateRctombmaintainInfo(RctombmaintainInfo setValue, RctombmaintainInfo condition);
                    bFind = true;
                    RctombmaintainInfo setValue = new RctombmaintainInfo();
                    RctombmaintainInfo cond1 = new RctombmaintainInfo();
                    cond1.family = temp.family;
                    cond1.code = temp.code;
                    cond1.type = temp.type;

                    setValue.remark = info.remark;
                    setValue.editor = info.editor;
                    mbmoRepository.UpdateRctombmaintainInfo(setValue, cond1);
                    //List<string> erpara = new List<string>();
                    //FisException ex;
                    //ex = new FisException("DMT154", erpara);
                    //throw ex;
                }
            }
            if(bFind == false)
                mbmoRepository.AddRctombmaintainInfo(info);
        }

        public void deleteMBMaintain(RctombmaintainInfo info)
        {
            mbmoRepository.DeleteRctombmaintainInfo(info);
        }

        public IList<RctombmaintainInfo> getMBMaintaininfo(string family)
        {
            IList<RctombmaintainInfo> list = new List<RctombmaintainInfo>();
            RctombmaintainInfo cond = new RctombmaintainInfo();
            cond.family = family;
            list = mbmoRepository.GetRctombmaintainInfoList(cond);

            return list;
        }

        #endregion
    }
}
