using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.CheckItemModule.Utility
{
    public interface ITreeTraversal
    {
        bool CheckCondition(object node);
    }
}
