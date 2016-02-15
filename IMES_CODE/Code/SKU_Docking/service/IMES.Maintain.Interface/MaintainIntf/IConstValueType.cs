using System;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Maintain.Interface.MaintainIntf
{
    /// <summary>
    /// IConstValueType接口
    /// </summary>
    public interface IConstValueType
    {
        /// <summary>
        /// select ID, Type, Value, Description, Editor, Cdt, Udt
        ///where Type=@Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IList<ConstValueTypeInfo> GetConstValueTypeList(string type);

        /// <summary>
        /// update ConstValueType
        /// SET Type, Value, Description, Editor, Cdt, Udt
        ///where ID=@ID
        /// </summary>
        /// <param> </param>
        /// <returns></returns>
        void UpdateConstValueType(ConstValueTypeInfo info);

        /// <summary>
        /// Insert ConstValueType(Type, Value, Description, Editor, Cdt, Udt)
        ///values(Type, Value, Description, Editor, Cdt, Udt)
        /// </summary>
        /// <param> </param>
        /// <returns></returns>
        void InsertConstValueType(ConstValueTypeInfo info);


        // <summary>
        /// Insert ConstValueType(Type, Value, Description, Editor, Cdt, Udt)
        /// select @Type, Data, @Descr, @Editor, getdate(),getdate()
        /// from @stringList
        /// </summary>
        /// <param> </param>
        /// <returns></returns>
        void InsertMultiConstValueType(string type, IList<string> values, string descr, string editor);

        /// <summary>
        /// Delete ConstValueType
        /// where ID =@ID
        /// </summary>
        /// <param> </param>
        /// <returns></returns>
        void RemoveConstValueType(int id);

        void RemoveConstValueType(IList<string> ids);

        /// <summary>
        /// Delete ConstValueType
        /// where Type = @type and Value in (@value)
        /// </summary>
        /// <param> </param>
        /// <returns></returns>
        void RemoveMultiConstValueType(string type, string value);

        /// <summary>
        /// select distinct Type from ConstValueType 
        /// </summary>
        /// <returns></returns>
        IList<ConstValueTypeInfo> GetConstValueTypeList();


        /// <summary>
        /// select ID, Type, Value, Description, Editor, Cdt, Udt
        ///where Type=@Type and Value=@value
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IList<ConstValueTypeInfo> GetConstValueTypeList(string type,string value);
    }
}
