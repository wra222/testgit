using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.FisObject.Common.Part.PartPolicy
{
    public interface IPartPolicyRepository
    {
        IPartPolicy GetPolicy(string partCheckType);

        /// <summary>
        /// 取得CheckItemType的方法：Select Name from CheckItemType order by Name
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<string> GetNameFromCheckItemType(CheckItemTypeInfo condition);
    }
}
