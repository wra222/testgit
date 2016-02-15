using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.DataModel;
using fons = IMES.FisObject.PCA.EcrVersion;

namespace IMES.FisObject.PCA.EcrVersion
{
    public interface IEcrVersionRepository : IRepository<EcrVersion>
    {
        #region For Maintain

        /// <summary>
        /// SELECT RTRIM(Family), RTRIM(MBCode), RTRIM(ECR), RTRIM(IECVer) as [IEC Version],
        /// RTRIM(CustVer) as [Customer Version], RTRIM(Editor), Cdt as [Create Date], Udt as [Update Date]
        /// FROM EcrVersion 
        /// WHERE Family = @Family
        /// ORDER BY Family, MBCode, ECR
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<EcrVersion> GetECRVersionListByFamily(string family);

        /// <summary>
        /// SELECT * FROM IMES_PCA..EcrVersion 
	    /// WHERE Family = @Family AND MBCode = @MBCode AND ECR = @ECR
        /// </summary>
        /// <param name="family"></param>
        /// <param name="mbCode"></param>
        /// <param name="ecr"></param>
        /// <returns></returns>
        IList<EcrVersion> GetECRVersionByFamilyMBCodeAndECR(string family, string mbCode, string ecr);

        /// <summary>
        /// SQL:SELECT Family FROM Family ORDER BY Family
        /// </summary>
        /// <returns></returns>
        IList<string> GetFamilyInfoListForECRVersion();

        /// <summary>
        /// 根据family,mbcode,ecr 更新item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="family"></param>
        /// <param name="mbcode"></param>
        /// <param name="ecr"></param>
        void UpdateEcrVersionMaintain(EcrVersion item, string family, string mbcode, string ecr);

        /// <summary>
        /// select * from EcrVersion where MBCode=@MBCode and ECR=@ECR
        /// </summary>
        /// <param name="mbCode"></param>
        /// <param name="ecr"></param>
        /// <returns></returns>
        IList<EcrVersion> GetECRVersionByMBCodeAndECR(string mbCode, string ecr);

        /// <summary>
        /// select distinct Descr from Part nolock where BomNodeType = @bomNodeType order by Descr
        /// </summary>
        /// <param name="bomNodeType"></param>
        /// <returns></returns>
        IList<string> GetFamilyInfoListForSA(string bomNodeType);

        #region . Defered . 

        void UpdateEcrVersionMaintainDefered(IUnitOfWork uow, EcrVersion item, string family, string mbcode, string ecr);

        #endregion

        #endregion

        #region for FRUMBVer
        IList<fons::EcrVersion> GetECRVersion(fons::EcrVersion condition);
        IList<FruMBVerInfo> GetFruMBVer();
        IList<FruMBVerInfo> GetFruMBVer(FruMBVerInfo condition);
        IList<string> GetPartNoInFruMBVer();
        void InsertFruMBVer(FruMBVerInfo item);
        void UpdateFruMBVer(FruMBVerInfo item);
        void RemoveFruMBVer(int id);

        #endregion
    }
}
