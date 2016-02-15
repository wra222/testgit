using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.CheckItemModule.Interface
{
    public interface IFilterModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchical_bom">hierarchicalBOM</param>
        /// <param name="station"></param>
        /// <param name="main_object"></param>
        /// <returns>IFlatBOM</returns>
        Object FilterBOM(Object hierarchicalBom, string station, object mainObject);
    }

    public interface IFilterModuleEx
    {
       /// <summary>
       /// 
       /// </summary>
       /// <param name="hierarchicalBom"></param>
       /// <param name="station"></param>
       /// <param name="checkItemType"></param>
       /// <param name="mainObject"></param>
       /// <returns></returns>
        Object FilterBOMEx(Object hierarchicalBom, string station, string checkItemType, object mainObject);
    }

    public interface IMatchModule
    {
        Object Match(string subject, object bomItem, string station);
    }
    public interface ICheckModule
    {
        void Check(object partUnit, object bomItem, string station);
    }
    public interface ISaveModule
    {
        void Save(object partUnit, object partOwner, string station, string key);
    }
}
