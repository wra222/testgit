/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Interface for Image Download Page
 * UI:CI-MES12-SPEC-FA-UI Image Download.docx –2011/10/28 
 * UC:CI-MES12-SPEC-FA-UC Image Download.docx –2011/10/28            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-4   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* UC 具体业务：[FA].[dbo].[rpt_ITCNDTS_SET_IMAGEDOWN_14]-数据接口尚未定义（in Activity：DoImageDownloadSave）
* UC 具体业务：检查Customer SN 是否存在（具体见接口需求表）-数据接口尚未定义（in Activity：CheckCPQSNO）
* UC 具体业务：select @grade=Grade from PAK.dbo.HP_Grade where Family=@descr-数据接口尚未定义（in Activity：DoImageDownloadSave）
* UC 具体业务：根据@flag 进行不同的处理（具体见接口需求表）-数据接口尚未定义（in Activity：DoImageDownloadSave）
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    public interface IImageDownload
    {
        /// <summary>
        /// 对输入的CPQSNO进行检查
        /// </summary>
        /// <param name="inputSn">cpqsno</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        /// <returns>Model and ProdID</returns>
        IList<String> checkCPQSNO(string cpqsno, string pdLine, string editor, string stationId, string customerId);


        /// <summary>
        /// SN Check第二次输入SN处理，返回PAQC以及ALC/NO-ALC
        /// </summary>
        /// <param name="cpqsno">cpqsno</param>
        /// <param name="bios">bios</param>
        /// <param name="image">image</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        void DoSave(string cpqsno, string bios, string image, string pdLine, string editor, string stationId, string customerId);


        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }
}