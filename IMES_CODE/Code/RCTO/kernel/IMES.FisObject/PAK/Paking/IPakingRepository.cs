// INVENTEC corporation (c)2011 all rights reserved. 
// Description: PAK_PAKComn对象Repository接口
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-1    210003                       create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
namespace IMES.FisObject.PAK.Paking
{
    public interface IPakingRepository : IRepository<PAK_PAKComn>
    {
        /// <summary>
        /// Doc_Type list : select distinct DOC_CAT from [PAK.PAKRT] where DOC_CAT like 'Pack List%' order by DOC_CAT  desc
        /// </summary>
        /// <param name="DOCCAT"></param>
        /// <returns></returns>
        IList<string> GetDocTypeByDOCCAT();
        /// <summary>
        /// Region list : select distinct REGION from v_PAKComn nolock
        /// </summary>
        /// <returns></returns>
        IList<string> GetRegions();

        //Carrier List : select distinct INTL_CARRIER from v_PAKComn nolock
        IList<string> GetCarriers();
        //exists (select * from [PAK_PAKComn] nolock where left(InternalID,10)=@DN)
        bool DNExist(string dn);
        //exists(select * from v_Shipment_PAKComn nolock where CONSOL_INVOICE=@DN or SHIPMENT = @DN)
        bool ShipmentExist(string dn);
        //exists(select * from v_Shipment_PAKComn nolock where WAYBILL_NUMBER=@DN )
        bool WayBillNumberExist(string dn);
        bool IsEDIUploadedForDN(string dn);
        bool IsEDIUploadedForShipment(string dn);
        bool IsEDIUploadedForWayBillNumber(string dn);
        bool IsInstructionForDN(string dn);
        bool IsInstructionForShipment(string dn);
        bool IsInstructionForWayBillNumber(string dn);
        void SavePakingList(string dn);
    }
}
