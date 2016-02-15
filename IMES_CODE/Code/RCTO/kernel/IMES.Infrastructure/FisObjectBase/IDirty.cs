namespace IMES.Infrastructure.FisObjectBase
{
    ///<summary>
    /// Dirty tracking接口
    ///</summary>
    public interface IDirty
    {
        ///<summary>
        /// 对象是否被修改
        ///</summary>
        bool IsDirty { get; set; }

        /// <summary>
        /// 清除对象内部所有Dirty flag
        /// </summary>
        void Clean();
    }
}