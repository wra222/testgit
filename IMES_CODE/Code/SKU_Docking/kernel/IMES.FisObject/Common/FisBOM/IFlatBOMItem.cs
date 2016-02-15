using System.Collections.Generic;
using IMES.FisObject.Common.Part;
using System;

namespace IMES.FisObject.Common.FisBOM
{
    public interface IFlatBOMItem
    {
        /// <summary>
        /// CheckItemType
        /// </summary>
        string CheckItemType { get; }

        /// <summary>
        /// 要求刷入的Part数量
        /// </summary>
        int Qty { get; }

        /// <summary>
        /// UI 表格PartNo栏位显示内容
        /// </summary>
        string PartNoItem { get; set; }

        /// <summary>
        /// BomNodeType
        /// </summary>
        string Tp { get; set; }

        /// <summary>
        /// UI 表格Description栏位显示内容
        /// </summary>
        string Descr { get; set; }

        /// <summary>
        /// try to match a string
        /// </summary>
        /// <param name="target">需要进行match的字符串</param>
        /// <param name="station">当前Station</param>
        /// <returns>match到的part</returns>
        PartUnit Match(string target, string station);


        /// <summary>
        /// 公用料列表
        /// </summary>
        IList<IPart> AlterParts { get; }

        /// <summary>
        /// 当前站之前已经绑定的Part
        /// </summary>
        IList<PartUnit> PrecheckedPart { get; }

        /// <summary>
        /// 当前站绑定的Part
        /// </summary>
        IList<PartUnit> CheckedPart { get; }

        /// <summary>
        /// 当前站之前节点绑定的Part
        /// </summary>
        IList<PartUnit> StationPreCheckedPart { get; }

        /// <summary>
        /// Model
        /// </summary>
        string Model { get; set; }

        string AlternativeItemGroup { get; set; }

        bool HasBinded { get; set; }

        Nullable<bool> NeedCheckUnique { get; set; }

        Nullable<bool> NeedSave { get; set; }

        Nullable<bool> NeedCommonSave { get; set; }     

        /// <summary>
        /// 对匹配成功的Part进行BOMItem级别的Check
        /// 如检查Part与BomItem中已刷料的兼容性等
        /// </summary>
        void Check(PartUnit pu, IPartOwner owner, string station, IFlatBOM bom);

        /// <summary>
        /// 将指定Part作为CheckedPart增加到BOMItem中
        /// 如果BOMItem已满需要抛业务异常
        /// </summary>
        /// <param name="pu">要添加的Part</param>
        void AddCheckedPart(PartUnit pu);

        void ClearCheckedPart();

        /// <summary>
        /// 将指定Part作为CheckedPart增加到BOMItem中
        /// 如果BOMItem已满需要抛业务异常
        /// </summary>
        /// <param name="pu">要添加的Part</param>
        void AddStationPreCheckedPart(PartUnit pu);

        /// <summary>
        /// 是否已刷满
        /// </summary>
        /// <returns>是否已刷满</returns>
        bool IsFull();

        void Save(PartUnit pu, IPartOwner owner, string station, string editor, string key);
        void Save(IPartOwner owner, string station, string editor, string key);
        void AddAlterPart(IPart part);

        //add PartForbid function
         IList<IMES.DataModel.PartForbidPriorityInfo> PartForbidList { get;}
         string SessionKey{ get;}
         IList<IMES.DataModel.CheckItemTypeRuleDef> CheckItemTypeRuleList { get; set; }
         object Tag { get; set; }
         void AddPrecheckedPart(PartUnit pu);
         void ClearPrecheckedPart();
         IFlatBOMItem RelationBomItem { get; set; }

         /// <summary>
         /// CheckItem 斑醚絏
         /// </summary>
         string GUID { get; }
    }
}