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

namespace IMES.Maintain.Implementation
{
    public class Celdata : MarshalByRefObject, ICeldata
    {
        #region ICeldata 成员

        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
        public IList<IMES.DataModel.CeldataDef> GetAllCeldatas()
        {
            IList<CeldataDef> CeldataItems = new List<CeldataDef>();
            try 
            {
                IList<CeldataInfo> CeldataVos = itemRepository.GetAllCeldatas();
                if (CeldataVos != null)
                {
                    foreach (CeldataInfo vo in CeldataVos)
                    {
                        CeldataDef def = new CeldataDef();
                        def.platform = vo.platform;
                        def.productSeriesName = vo.productSeriesName;
                        def.category = vo.category;
                        def.grade = vo.grade;
                        def.tec = vo.tec;
                        def.zmod = vo.zmod;
                        def.cdt = vo.cdt.ToString("yyyy-MM-dd hh:mm:ss");
                        def.editor = vo.editor;
                        CeldataItems.Add(def);
                    }
                }
            }
            catch(Exception ee)
            {
                logger.Error(ee.Message);
                throw;
            }
            return CeldataItems;
        }

        public string AddCeldataItem(IMES.DataModel.CeldataDef item)
        {
           
            FisException fe = null;
            string id="";
            try 
            {
                IList<CeldataDef> defpos = GetAllCeldatas();
                if(defpos!=null)
                {
                    foreach(CeldataDef def in defpos)
                    {
                        if (def.zmod.ToUpper().Trim() == item.zmod.ToUpper().Trim())
                        {
                            List<string> param = new List<string>();
                            fe = new FisException("DMT158",param);
                            throw fe;
                          
                        }
                    }

                    //IList<CeldataInfo> list = new List<CeldataInfo>();
                    //list = itemRepository.GetCeldataInfoByRangeAndId(item.begin, item.begin, 0);
                    //if (list != null && list.Count > 0)
                    //{
                    //    List<string> param = new List<string>();
                    //    fe = new FisException("DMT156", param);
                    //    throw fe;
                    //}

                    //list = itemRepository.GetCeldataInfoByRangeAndId(item.end, item.end, 0);
                    //if (list != null && list.Count > 0)
                    //{
                    //    List<string> param = new List<string>();
                    //    fe = new FisException("DMT156", param);
                    //    throw fe;
                    //}

                    //list = itemRepository.GetCeldataInfoByRangeAndIdReversely(item.begin, item.end, 0);
                    //if (list != null && list.Count > 0)
                    //{
                    //    List<string> param = new List<string>();
                    //    fe = new FisException("DMT156", param);
                    //    throw fe;
                    //}
                }
                CeldataInfo newInfo = new CeldataInfo();

                newInfo.platform = item.platform; //item.code.ToUpper();
                newInfo.productSeriesName = item.productSeriesName;// item.begin.ToUpper();
                newInfo.category = item.category;// item.end.ToUpper();
                newInfo.grade = item.grade;
                newInfo.tec = item.tec;
                newInfo.zmod = item.zmod;
                newInfo.editor = item.editor;
                newInfo.cdt = DateTime.Now;
                
                itemRepository.AddCeldataItem(newInfo);
            }
            catch(Exception ee)
            {
                logger.Error(ee.Message);
                throw;
            }
            return id;

        }

        public void DeleteCeldataItem(string zmod)
        {
            try 
            {
                itemRepository.DeleteCeldataItem(zmod);
            }
            catch(Exception ee)
            {
                logger.Error(ee.Message);
                throw;
            }
        }

        #endregion
    }
}
