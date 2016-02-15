using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.PAK.Carton;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.DN;
using IMES.DataModel;
using System.Collections;
using IMES.Infrastructure;
using IMES.FisObject.Common.Part;

namespace IMES.Station.Implementation
{
    public sealed class CommonUtl
    {
        //static readonly IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
        static readonly ICartonRepository cartonRep = RepositoryFactory.GetInstance().GetRepository<ICartonRepository, Carton>();
        static readonly IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
        
        static public void CheckDeliveryPalletCartonQty(string palletNo)
        {            
            IList<AssignedDeliveryPalletInfo> assignedDeliveryCartonQty= cartonRep.GetAssignedDeliveryPalletCartonQty(palletNo, false);
           
            IList<AssignedDeliveryPalletInfo> deliveryPalletCartonQty = dnRep.GetDeliveryPalletListByPlt(palletNo).Select(x =>
                                                                                        new AssignedDeliveryPalletInfo
                                                                                        {
                                                                                            CartonQty = x.deliveryQty,
                                                                                            DeliveryNo = x.deliveryNo,
                                                                                            PalletNo = x.palletNo,
                                                                                             DeviceQty =x.deviceQty
                                                                                        }).ToList();
            if (assignedDeliveryCartonQty.Count != deliveryPalletCartonQty.Count)
            {
                string errorMsg = string.Format("PalletNo:{0} Delivery Qty:{1} Assign Delivery Qty:{2} is different", palletNo, deliveryPalletCartonQty.Count, assignedDeliveryCartonQty.Count);
                FisException ex = new FisException("CHK963", new string[] { errorMsg });    //此Pallet 上的Product 结合的Box Id or UCC数量不正确，请检查! 
                throw ex;
            }

            var notMatchs = deliveryPalletCartonQty.Except(assignedDeliveryCartonQty, new DeliveryPalletCompare()).ToList();
            if (notMatchs.Count > 0)
            {
                string noMatchedDNList = string.Join(",", notMatchs.Select(x => x.DeliveryNo).ToArray());
                FisException ex = new FisException("CHK963", new string[] { palletNo + " DeliveryNo:" + noMatchedDNList });    //此Pallet 上的Product 结合的Box Id or UCC数量不正确，请检查! 
                throw ex;
            }

            notMatchs = assignedDeliveryCartonQty.Except(deliveryPalletCartonQty, new DeliveryPalletCompare()).ToList();
            if (notMatchs.Count > 0)
            {
                string noMatchedDNList = string.Join(",", notMatchs.Select(x => x.DeliveryNo).ToArray());
                FisException ex = new FisException("CHK963", new string[] { palletNo + " assign DeliveryNo:" + noMatchedDNList });    //此Pallet 上的Product 结合的Box Id or UCC数量不正确，请检查! 
                throw ex;
            }         

        }

        //static public bool CheckDomesticDN(string regId)
        //{
        //    IList<string> domesticRegIdList = new List<string>(){
        //        "SCN",
        //        "QCN",
        //        "CN",
        //        "QET",
        //        "ET"
        //    };
        //    return domesticRegIdList.Contains(regId);
        //}

        //public static string GetSite(string defaultValue)
        //{
           
        //    IList<string> valueList = partRepository.GetValueFromSysSettingByName("Site");
        //    if (defaultValue == null )
        //    {
        //        if (valueList.Count == 0)
        //        {
        //            throw new FisException("PAK095", new string[] { "Site" });
        //        }
        //        else
        //        {
        //            return valueList[0].Trim(); 
        //        }
        //    }
        //    else
        //    {
        //        return valueList.Count == 0? defaultValue: valueList[0].Trim();               
        //    }           
        //}

        ///// <summary>
        ///// ConstValueType
        ///// </summary>
        ///// <param name="type"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public static IList<ConstValueTypeInfo> ConstValueType(string type, string value)
        //{
        //    IList<ConstValueTypeInfo> retLst = new List<ConstValueTypeInfo>();
        //    value = null;
        //    if (!String.IsNullOrEmpty(type))
        //    {
        //        retLst = partRepository.GetConstValueTypeList(type);
        //        ConstValueType(retLst, type, value);
        //    }
        //    return retLst;
        //}

        ///// <summary>
        ///// ConstValueType
        ///// </summary>
        ///// <param name="constValueTypes"></param>
        ///// <param name="type"></param>
        ///// <param name="value"></param>
        //public static void ConstValueType(IList<ConstValueTypeInfo> constValueTypes, string type, string value)
        //{

        //    if (constValueTypes == null || constValueTypes.Count == 0)
        //    {
        //        throw new FisException("TRC002", new string[] { type });
        //    }

        //    if (!string.IsNullOrEmpty(value))
        //    {
        //        if (!constValueTypes.Any(x => x.value == value))
        //        {
        //            throw new FisException("CHK990", new string[] { "ConstValueTye", type, value });
        //        }
        //    }
        //}

    }
}
