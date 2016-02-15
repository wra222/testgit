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
        /// Ҫ��ˢ���Part����
        /// </summary>
        int Qty { get; }

        /// <summary>
        /// UI ���PartNo��λ��ʾ����
        /// </summary>
        string PartNoItem { get; set; }

        /// <summary>
        /// BomNodeType
        /// </summary>
        string Tp { get; set; }

        /// <summary>
        /// UI ���Description��λ��ʾ����
        /// </summary>
        string Descr { get; set; }

        /// <summary>
        /// try to match a string
        /// </summary>
        /// <param name="target">��Ҫ����match���ַ���</param>
        /// <param name="station">��ǰStation</param>
        /// <returns>match����part</returns>
        PartUnit Match(string target, string station);


        /// <summary>
        /// �������б�
        /// </summary>
        IList<IPart> AlterParts { get; }

        /// <summary>
        /// ��ǰվ֮ǰ�Ѿ��󶨵�Part
        /// </summary>
        IList<PartUnit> PrecheckedPart { get; }

        /// <summary>
        /// ��ǰվ�󶨵�Part
        /// </summary>
        IList<PartUnit> CheckedPart { get; }

        /// <summary>
        /// ��ǰվ֮ǰ�ڵ�󶨵�Part
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
        /// ��ƥ��ɹ���Part����BOMItem�����Check
        /// ����Part��BomItem����ˢ�ϵļ����Ե�
        /// </summary>
        void Check(PartUnit pu, IPartOwner owner, string station, IFlatBOM bom);

        /// <summary>
        /// ��ָ��Part��ΪCheckedPart���ӵ�BOMItem��
        /// ���BOMItem������Ҫ��ҵ���쳣
        /// </summary>
        /// <param name="pu">Ҫ��ӵ�Part</param>
        void AddCheckedPart(PartUnit pu);

        void ClearCheckedPart();

        /// <summary>
        /// ��ָ��Part��ΪCheckedPart���ӵ�BOMItem��
        /// ���BOMItem������Ҫ��ҵ���쳣
        /// </summary>
        /// <param name="pu">Ҫ��ӵ�Part</param>
        void AddStationPreCheckedPart(PartUnit pu);

        /// <summary>
        /// �Ƿ���ˢ��
        /// </summary>
        /// <returns>�Ƿ���ˢ��</returns>
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
         /// CheckItem �ߤ@�ѧO�X
         /// </summary>
         string GUID { get; }
    }
}