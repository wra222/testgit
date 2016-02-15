using System;
using System.Text;
using IMES.DataModel;
using System.Data;
using System.Collections.Generic;
using IMES.FisObject.Common.Model;

namespace IMES.Maintain.Interface.MaintainIntf
{

    public interface IAssemblyVC
    {
        /// <summary>
        /// GetAssemblyVCbyCondition
        /// </summary>
        /// <param name="condition">condition</param>
        /// <returns></returns>
        IList<AssemblyVCInfo> GetAssemblyVCbyCondition(AssemblyVCInfo condition);

        /// <summary>
        /// GetFamilyList
        /// </summary>
        IList<string> GetFamilyList();

        /// <summary>
        /// GetPartNoList
        /// </summary>
        /// <param name="VC"></param>
        /// <returns></returns>
        List<string> GetPartNoList(string VC);

        /// <summary>
        /// InsertAssemblyVC
        /// </summary>
        /// <param name="item">AssemblyVCInfo</param>
        /// <returns></returns>
        void InsertAssemblyVC(AssemblyVCInfo item);

        /// <summary>
        /// UpdateAssemblyVC
        /// </summary>
        /// <param name="item">AssemblyVCInfo</param>
        /// <returns></returns>
        void UpdateAssemblyVC(AssemblyVCInfo item);

        /// <summary>
        /// DeleteAssemblyVC
        /// </summary>
        /// <param name="id">string</param>
        void DeleteAssemblyVC(string id);
    }
}
