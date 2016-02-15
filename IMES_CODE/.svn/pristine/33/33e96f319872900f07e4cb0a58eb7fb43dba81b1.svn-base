using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using System.Collections.Generic;
using IMES.FisObject.Common.Model;
using IMES.FisObject.PCA.MB;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CheckMBForRCTOLabelPrint : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckMBForRCTOLabelPrint()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查是否满足complete条件, 更新Repair状态
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            try
            {
                var product = CurrentSession.GetValue(Session.SessionKeys.Product) as IProduct;
                if (product == null)
                {
                    throw new NullReferenceException("Product in session is null");
                }

                string mbsn = (string)CurrentSession.GetValue(Session.SessionKeys.MBCode);
                if (mbsn != product.PCBID)
                {
                    List<string> errpara = new List<string>();
                    FisException ex = new FisException("CHK929", errpara);
                    throw ex;
                }

                string currentModel = product.Model;
                IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                Model modelObj = modelRepository.Find(currentModel);
                if (modelObj == null)
                {
                    List<string> errpara = new List<string>();
                    errpara.Add(currentModel);
                    FisException ex = new FisException("CHK164", errpara);
                    throw ex;
                }
                else
                {
                    IList<IMES.FisObject.Common.Model.ModelInfo> info1 = new List<IMES.FisObject.Common.Model.ModelInfo>();
                    IList<IMES.FisObject.Common.Model.ModelInfo> info2 = new List<IMES.FisObject.Common.Model.ModelInfo>();
                    info1 = modelRepository.GetModelInfoByModelAndName(currentModel, "CT");
                    if (info1 == null || info1.Count == 0)
                    {
                        info2 = modelRepository.GetModelInfoByModelAndName(currentModel, "ZMODE");
                        if (info2 == null || info2.Count == 0)
                        {
                            List<string> errpara = new List<string>();
                            FisException ex = new FisException("CHK942", errpara);
                            throw ex;
                        }
                    }

                    IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                    IList<IMES.FisObject.PCA.MB.MBInfo> mbinfo = new List<IMES.FisObject.PCA.MB.MBInfo>();
                    string[] pcbnos = new string[] { mbsn };

                    mbinfo = mbRepository.GetMbInfoListByInfoTypeAndPcbNoList("MBCT", pcbnos);
                    if (mbinfo == null || mbinfo.Count == 0)
                    {
                        List<string> errpara = new List<string>();
                        FisException ex = new FisException("CHK943", errpara);
                        throw ex;
                    }
                }

                return base.DoExecute(executionContext);
            }
            catch (FisException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
