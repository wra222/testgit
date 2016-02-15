using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
   public  interface IAssetRange
    {
       /// <summary>
       /// 得到所有的AssetRanges记录
       /// </summary>
       /// <returns></returns>
       IList<AssetRangeDef> GetAllAssetRanges();
       /// <summary>
       /// 添加一条AssetRange
       /// 当所要添加的记录中的code与其他存在的记录重复时,抛出异常
       /// </summary>
       /// <param name="item"></param>
       /// <returns>添加成功后返回此记录的ID</returns>
       string AddAssetRangeItem(AssetRangeDef item);
       /// <summary>
       /// 更新一条AssetRange
       /// 当所要添加的记录中的code与其他存在的记录重复时,抛出异常
       /// 当所要更新的记录已经不存在时,抛出异常
       /// </summary>
       /// <param name="item"></param>
       void UpdateAssetRangeItem(AssetRangeDef item);
       /// <summary>
       /// 删除一条AssetRange
       /// </summary>
       /// <param name="id"></param>
       void DeleteAssetRangeItem(int id);

       int GetBeginLength(string Code);

       IList<string> GetCodeListInAssetRange();

       IList<AssetRangeDef> GetAssetRangeByCode(string code);

      void CheckAssetRange(string code, string beginNum, string endNum);

      void CloseActiveRange(AssetRangeDef item);

      void CheckAddRangeItem(AssetRangeDef item, string side);

      void CheckUpdateRangeItem(AssetRangeDef item, string side);

    }
}
