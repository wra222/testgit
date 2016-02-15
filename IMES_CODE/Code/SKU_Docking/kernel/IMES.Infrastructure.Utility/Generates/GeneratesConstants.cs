// 2010-02-02 Liu Dong(eB1-4)         Modify ITC-1122-0060 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Infrastructure.Utility.Generates
{
    public class GeneratesConstants
    {
        public const string SMTMO = "SMTMO";
        public const string MBSn = "MBSn";
        public const string Mac = "Mac";
        public const string ProdId = "ProdId";
        public const string CustomerSn = "CustomerSn";
        public const string CT = "CT";
        public const string DCode = "DCode";
        public const string SVBSno = "SVBSno";
        public const string VTMO = "VTMO"; //Virtual MO
        public const string GIFT = "GIFT";
        public const string Pizza = "Pizza";
        public const string Carton = "Carton";
        public const string VirtualCarton = "VirtualCarton";
        public const string VirtualPallet = "VirtualPallet";
        public const string FRUCarton = "FRUCarton";
        public const string PalletId = "PalletId";

        public static string MappingToStandard(string genType)
        {
                //MB,
                //PrdId,
                //VB,
                //SB,
                //KP,
                //SMTMO,
                //MAC,
                //CustSN,
            //Pallet,
            //Pizza,
            //Carton,
            //UCC,

            switch (genType)
            {
                case SMTMO:
                    return "SMTMO";
                //case MBSn:
                //    return "";
                case "MB":
                    return "MB";
                case "VB":
                    return "VB";
                case "SB":
                    return "SB";
                case Mac:
                    return "MAC";
                case ProdId:
                    return "PrdId";
                case CustomerSn:
                    return "CustSN";
                case CT:
                    return "KP";// 2010-02-02 Liu Dong(eB1-4)         Modify ITC-1122-0060 
                case DCode:
                    return "DCode";
                case SVBSno:
                    return "SVBSno";
                case VTMO:
                    return "VTMO";
                case GIFT:
                    return "GIFT";
                case Pizza:
                    return "Pizza";
                case Carton:
                    return "Carton";
                case VirtualCarton:
                    return "VTCarton";
                case VirtualPallet:
                    return "VTPallet";
                case FRUCarton:
                    return "FRUCarton";
                case PalletId:
                    return "PalletId";
            }
            return genType;
        }
    }
}
