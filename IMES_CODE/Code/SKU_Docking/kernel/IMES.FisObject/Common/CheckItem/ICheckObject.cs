using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.FisObject.Common.CheckItem
{

    /// <summary>
    /// 可以进行Item Check的检查目标对象接口
    /// 可能的实现类有MB, Product
    /// </summary>
    public interface ICheckObject
    {
        /// <summary>
        /// 获取Model指定属性
        /// </summary>
        /// <param name="item">属性名</param>
        /// <returns>指定属性</returns>
        object GetModelProperty(string item);

        /// <summary>
        /// 保存Model指定属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
        void SetModelProperty(string name, object value);

        /// <summary>
        /// 获取检查目标对象的基本属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <returns>属性值</returns>
        object GetProperty(string name);

        /// <summary>
        /// 获取检查目标对象的扩展属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <returns>属性值</returns>
        string GetExtendedProperty(string name);

        /// <summary>
        /// 设置检查目标对象的基本属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
        void SetProperty(string name, object value);

        /// <summary>
        /// 设置检查目标对象的扩展属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
        /// <param name="editor"></param>
        void SetExtendedProperty(string name, object value, string editor);

        /// <summary>
        /// 获取检查目标对象的pizza属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <returns>属性值</returns>
        object GetPizzaProperty(string name);

        /// <summary>
        /// 设置检查目标对象的pizza属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
        void SetPizzaProperty(string name, object value);
        
        /// <summary>
        /// 获取BOM中指定类型的Pn列表
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        //object GetBOMPartsByType(string item);


        /// <summary>
        /// 取得检查条目。检查条目通过界面维护并存储在数据库中。
        /// </summary>
        /// <param name="customer">客户标识(字符串)</param>
        /// <param name="station">站标识(字符串)</param>
        /// <returns>检查条目对象集合</returns>
        //IList<ICheckItem> GetExplicitCheckItem(string customer, string station);

        /// <summary>
        /// 取得检查条目。检查条目通过界面维护并存储在数据库中。
        /// </summary>
        /// <param name="customer">客户标识(字符串)</param>
        /// <param name="station">站标识(字符串)</param>
        /// <returns>检查条目对象集合</returns>
        //IList<ICheckItem> GetImplicitCheckItem(string customer, string station);

        /// <summary>
        /// Fuzzy query from ProductInfo with 'LIKE'
        /// </summary>
        /// <param name="condition">query condition such as AST%</param>
        /// <returns></returns>
        IList<object> FuzzyQueryExtendedProperty(string condition);
    }
}
