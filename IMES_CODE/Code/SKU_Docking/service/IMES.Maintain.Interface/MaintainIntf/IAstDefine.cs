using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IAstDefine
    {
       /// <summary>
        /// 得到所有的AstDefine记录
       /// </summary>
       /// <returns></returns>
        IList<AstDefineInfo> GetAllAstDefineInfo();

        /// <summary>
        /// 得到所有的AstDefine记录
        /// </summary>
        /// <returns></returns>
        List<string> GetAllAstCode(string astType);

        /// <summary>
        /// 添加一条AstDefine
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        void AddAstDefineInfo(AstDefineInfo item);

        /// <summary>
        /// Delete AstDefineInfo
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        void DelAstDefineInfo(string astType, string astCode);

        /// <summary>
        /// Delete AstDefineInfo
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        void UpdateAstDefineInfo(AstDefineInfo item,string astType, string astCode);
        void CheckDuplicateData(string astType, string astCode);
        List<string> GetAssignAstStation();
        List<string> GetCombineASTStation();


    }
}
