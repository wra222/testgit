/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012-01-06   zhu lei           Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Text;
//using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;
namespace IMES.Docking.Interface.DockingIntf
{
    /// <summary>
    /// [MB Label Print] ʵ�������������ܣ�
    /// 1.	����Mo������MB SNo������MB SNo��Mo�Ķ�Ӧ��ϵ������ӡBarcode��
    /// 2.	���ĳЩMB SNo��Mo�Ĺ�����ϵ��
    /// ��������Ŀ����������
    /// 1.	ʵ��ͨ��Mo�ܿ�MB SNO
    /// 2.	ʵ����������׷��
    /// </summary>
    public interface IMBLabelPrint
    {
        /// <summary>
        /// Print
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="isNextMonth"></param>
        /// <param name="mbCode"></param>
        /// <param name="mo"></param>
        /// <param name="qty"></param>
        /// <param name="dateCode"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customerId"></param>
        /// <param name="startProdIdAndEndProdId"></param>
        /// <param name="_111"></param>
        /// <param name="factor"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        IList<PrintItem> Print(
            string pdLine,
            bool isNextMonth, 
            string mbCode,
            string mo,
            int qty,
            string dateCode,
            string editor, string stationId, string customerId,
            out IList<string> startProdIdAndEndProdId, string _111, string factor, IList<PrintItem> printItems);

        /// <summary>
        /// Reprint
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="customerId"></param>
        /// <param name="reason"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        IList<PrintItem> RePrint(
            string mbSno,
            string customerId,
            string reason,
            string editor, string stationId,
            IList<PrintItem> printItems);


        ///// <summary>
        ///// 1.2	UC-PCA-MLP-02 Find (�ݲ�ʵ��)
        ///// �����û�ѡ���[MB_CODE/PCB]/[111]/[Mo]/[This Month] or [Next Month]��Find & Display ����MB SNo ��Start MB SNo��
        ///// </summary>
        ///// <param name="mo">MO�ִ�</param>
        ///// <param name="isNextMonth">"ThisMonth"��"NextMonth"</param>
        ///// <param name="editor">operator</param>
        ///// <returns>MB SNo</returns>
        //string Find(
        //    string mo,
        //    string isNextMonth,
        //    string editor, string stationId, string customerId);

        ///// <summary>
        ///// 1.3	UC-PCA-MLP-03 Dismantle
        ///// ���MB SNo��Mo�Ĺ�����ϵ
        ///// �쳣�����
        ///// a.	���û������[Start MB SNo]���򱨸���󣺡�������Start MB/SB/VB NO !!��
        ///// b.	���û������[Dismantle Reason]���򱨸���󣺡���������Dismantle Reason��
        ///// c.	����û������[End MB SNo]<[Start MB SNo]���򱨸���󣺡�End MB SNo ������� Start MB SNo"
        ///// d.	����û������[End MB SNo]��Mo��[Start MB SNo] ��Mo ��ͬ���򱨸���󣺡�Start MB SNo �� End MB SNo ��Mo ��ͬ����
        ///// �쳣�����
        ///// a.	������û������[Start MB SNo]��[End MB SNo] ��Χ����[SnoDetPCB] ����û�м�¼���ڣ��򱨸���󣺡�MB NO is not exist !!����ע����Ҫ��ȷ�ı�����MB NO��VB NO ������SB NO �����ڡ�
        ///// b.	���ָ����MB SNo �Ѿ�Ͷ���������򱨸����MB SNo: @mbsno �Ѿ�Ͷ�����������ܽ���Dismantle!!��
        ///// </summary>
        ///// <param name="startMBSNo">��ʼMB SNo</param>
        ///// <param name="endMBSNo">����MB SNo</param>
        ///// <param name="reason">���ԭ��</param>
        ///// <param name="editor">operator</param>
        ///// <param name="stationId">stationId</param>
        ///// <param name="customerId">customerId</param>
        //void Dismantle(
        //    string startMBSNo,
        //    string endMBSNo,
        //    string reason,
        //    string editor, string stationId, string customerId);


        /// <summary>
        /// ȡ��������
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);


        //void Reprint(
        //    string startMBSNo,
        //    string endMBSNo,
        //    string reason,
        //    string editor, string stationId, string customerId);

      //  ArrayList GetMBNoList(string beginNo, string endNo);
        //bool CheckIsProduct(string beginNo, string endNo, string SA1StationName, out string ExistMB);
    }
}