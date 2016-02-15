using System.Collections.Generic;
using IMES.Infrastructure.FisObjectBase;
using System;

namespace IMES.FisObject.PCA.MBMO
{
    /// <summary>
    /// MBMO�ӿ�
    /// </summary>
    public interface IMBMO : IAggregateRoot
    {
        ///<summary>
        /// MONo
        ///</summary>
        string MONo { get; set; }

        ///<summary>
        /// MB Family
        ///</summary>
        string Family { get; set; }

        ///<summary>
        /// Model
        ///</summary>
        string Model { get; set; }

        ///<summary>
        /// Mo������Mb����
        ///</summary>
        int Qty { get; set; }

        ///<summary>
        /// ��MO���Ѳ���MB��ŵ�����
        ///</summary>
        int PrintedQty { get; set; }

        ///<summary>
        /// Remark
        ///</summary>
        string Remark { get; set; }
        
        ///<summary>
        /// Status
        ///</summary>
        string Status { get; set; }

        ///<summary>
        /// Editor
        ///</summary>
        string Editor { get; set; }

        ///<summary>
        /// Cdt
        ///</summary>
        DateTime Cdt { get; }

        ///<summary>
        /// Udt
        ///</summary>
        DateTime Udt { get; }

        /// <summary>
        /// ���Ӵ�ӡ�����ǩ����
        /// </summary>
        /// <param name="byQty">��������</param>
        /// <returns>ʵ����������</returns>
        int IncreasePrintedQty(int byQty);

        /// <summary>
        /// ���ٴ�ӡ�����ǩ����
        /// </summary>
        /// <param name="byQty">��������</param>
        /// <returns>ʵ�ʼ�������</returns>
        int DecreasePrintedQty(int byQty);

        /// <summary>
        /// ��������װ�����塣�Ա㽫������������
        /// </summary>
        /// <param name="startMBSn">��ʼ�������</param>
        /// <param name="endMBSn">�����������</param>
        void DismantleMB(string startMBSn, string endMBSn);

        /// <summary>
        /// ����������Ϣ��
        /// </summary>
        /// <param name="qty">����������</param>
        /// <returns>������������ż���</returns>
        IList<string> GenerateMB(int qty);

    }
}