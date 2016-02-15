// INVENTEC corporation (c)2011 all rights reserved. 
// Description: filter bom, light on
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-06   itc207013                   create
// 2012-03-01   itc207013                   ITC-1360-0836
// 2012-03-01   itc207013                   ITC-1360-0837
// 2012-03-01   itc207013                   ITC-1360-0923
// 2012-03-01   itc207013                   ITC-1360-0994
// 2012-03-01   itc207013                   ITC-1360-1143
// 2012-05-01   itc207013                   ITC-1360-1774
// Known issues:

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.Station;


namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class LightOn : BaseActivity
    {
        private readonly string DDDKitStation6 = "PKOK";

        /// <summary>
        /// 
        /// </summary>
        public LightOn()
        {
            InitializeComponent();
        }

        /// <summary>
        /// light on
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            //find current station descr
            var StationRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository, IStation>();
            IStation currentStation = StationRepository.Find(Station);
            if (currentStation == null || string.IsNullOrEmpty(currentStation.Descr))
            {
                throw new FisException("PKIT01", new string[] { Station });
            }
            string currentStationDescr = currentStation.Descr;
            CurrentSession.AddValue(Session.SessionKeys.StationDescr, currentStationDescr);

            IStation Kit6Station = StationRepository.Find(DDDKitStation6);
            if (Kit6Station == null || string.IsNullOrEmpty(Kit6Station.Descr))
            {
                throw new FisException("PKIT01", new string[] { DDDKitStation6 });
            }


            IProduct currentProduct = CurrentSession.GetValue(Session.SessionKeys.Product) as Product;

            //Get all wipbuffer for all station, this line
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IList<WipBuffer> allWipbufList = productRepository.GetWipBufferByKittingTypeAndCode("PAK Kitting", Line.Substring(0, 1));
            if (allWipbufList == null || allWipbufList.Count == 0)
            {
                throw new FisException("PKIT03", new string[] { });
            }

            // from wipbuffer maintain station list get nextstation should go
            string ShouldGoStation = "";
            int ShouldStationIndex = -1;
            string pkStation = "-PK01-PK02-PK03-PK04-PK05-PKOK-";
            List<string> stationList = (from tempWip in allWipbufList
                                        orderby tempWip.Station
                                        select tempWip.Station).Distinct<string>().ToList<string>();

            if (!stationList.Contains(Kit6Station.Descr))
            {
                stationList.Add(Kit6Station.Descr);
            }

            if (pkStation.Contains("-" + currentProduct.Status.StationId + "-"))
            {
                IStation preStation = StationRepository.Find(currentProduct.Status.StationId);
                if (preStation == null || string.IsNullOrEmpty(preStation.Descr))
                {
                    throw new FisException("PKIT01", new string[] { currentProduct.Status.StationId });
                }
                string preStationDescr = preStation.Descr;


                for (int i = 0; i < stationList.Count() - 1; i++)
                {
                    if (stationList.ElementAt(i) == preStationDescr)
                    {
                        ShouldStationIndex = i + 1;
                        ShouldGoStation = stationList.ElementAt(i + 1);
                        break;
                    }
                }
            }
            else
            {
                ShouldStationIndex = 0;
                ShouldGoStation = stationList.ElementAt(0);
            }

            // if can't get next station should go,then set next station is kit6
            if (ShouldStationIndex == -1)
            {
                ShouldStationIndex = stationList.Count();


                ShouldGoStation = Kit6Station.Descr;
            }

            IFlatBOM totalBom = (IFlatBOM)CurrentSession.GetValue(Session.SessionKeys.SessionBom);

            // if next station doesn't contain any part in wippbuffer, then get next station of this station
            for (int i = ShouldStationIndex; i < stationList.Count() - 1; i++)
            {
                if (ShouldGoStation != currentStationDescr)
                {
                    var shouldStationWBList = from tempWip in allWipbufList
                                              where tempWip.Station == ShouldGoStation
                                              select tempWip;

                    foreach (IFlatBOMItem bomItem in totalBom.BomItems)
                    {
                        bool match = false;
                        foreach (WipBuffer wipbufobj in shouldStationWBList)
                        {
                            if (PartMatchWipBuffer(wipbufobj.PartNo, bomItem.AlterParts))
                            {
                                match = true;
                                break;
                            }
                        }
                        if (match)
                        {
                            throw new FisException("PKIT02", new string[] { ShouldGoStation });
                        }
                    }
                    ShouldStationIndex = ShouldStationIndex + 1;
                    ShouldGoStation = stationList.ElementAt(ShouldStationIndex);

                }
                else
                {
                    break;
                }
            }

            if (ShouldGoStation != currentStationDescr)
            {
                throw new FisException("PKIT02", new string[] { ShouldGoStation });
            }



            //get bom,current station wipbuffer
            IList<IFlatBOMItem> filteredBomItems = new List<IFlatBOMItem>();
            var wipbufObjs = from tempWip in allWipbufList
                             where tempWip.Station == ShouldGoStation
                             select tempWip;
            IList<WipBuffer> wipbufferList = new List<WipBuffer>();

            if (wipbufObjs != null)
            {
                wipbufferList = wipbufObjs.ToList<WipBuffer>();
            }


            IList<string> matchePnList = new List<string>();
            // kit6 get all part by remove kit1-5
            if (Station == DDDKitStation6)
            {

                List<string> station15List = new List<string>();
                for (int i = 0; i < stationList.Count(); i++)
                {
                    if (stationList[i].Trim() == "DDD Kitting 1" || stationList[i].Trim() == "DDD Kitting 2" || stationList[i].Trim() == "DDD Kitting 3" || stationList[i].Trim() == "DDD Kitting 4" || stationList[i].Trim() == "DDD Kitting 5")
                    {
                        station15List.Add(stationList[i]);
                    }
                }
                for (int i = 0; i < station15List.Count(); i++)
                {

                    var wipbufObjtemp = from tempWip in allWipbufList
                                        where tempWip.Station == station15List.ElementAt(i)
                                        select tempWip;
                    if (wipbufObjtemp != null)
                    {
                        IList<int> removeIndexList = new List<int>();

                        foreach (WipBuffer wipbufobj in wipbufObjtemp)
                        {

                            for (int j = 0; j < totalBom.BomItems.Count; j++)
                            {
                                if (PartMatchWipBuffer(wipbufobj.PartNo, totalBom.BomItems[j].AlterParts))
                                {
                                    removeIndexList.Add(j);
                                }
                            }
                        }

                        var sortedIndexList = (from tempIndex in removeIndexList
                                               orderby tempIndex descending
                                               select tempIndex).Distinct<int>();
                        for (int k = 0; k < sortedIndexList.Count(); k++)
                        {
                            totalBom.BomItems.RemoveAt(sortedIndexList.ElementAt<int>(k));
                        }
                    }
                }

                foreach (IFlatBOMItem bomItem in totalBom.BomItems)
                {
                    foreach (WipBuffer wipbufobj in wipbufObjs)
                    {
                        if (PartMatchWipBuffer(wipbufobj.PartNo, bomItem.AlterParts))
                        {
                            matchePnList.Add(wipbufobj.PartNo);
                        }
                    }
                }


                foreach (IFlatBOMItem bomItem in totalBom.BomItems)
                {
                    filteredBomItems.Add(bomItem);
                }
            }
            else
            {
                // kit 1-5,get part by wipbuffer cross bom 
                if (wipbufObjs == null || wipbufObjs.Count() == 0)
                {
                    var ex = new FisException("PKIT03", new string[] { });
                    throw ex;
                }


                foreach (IFlatBOMItem bomItem in totalBom.BomItems)
                {
                    bool match = false;
                    foreach (WipBuffer wipbufobj in wipbufObjs)
                    {
                        if (PartMatchWipBuffer(wipbufobj.PartNo, bomItem.AlterParts))
                        {
                            match = true;
                            matchePnList.Add(wipbufobj.PartNo);
                        }
                    }
                    if (match)
                    {
                        filteredBomItems.Add(bomItem);
                    }
                }
            }


            if (filteredBomItems == null || filteredBomItems.Count == 0)
            {
                throw new FisException("PKIT03", new string[] { });
            }

            for (int i = wipbufferList.Count - 1; i >= 0; i--)
            {
                if (!matchePnList.Contains<string>(wipbufferList[i].PartNo))
                {
                    wipbufferList.RemoveAt(i);
                }
            }

            CurrentSession.AddValue(Session.SessionKeys.PizzaKitWipBuffer, wipbufferList);
            //replace with new bom
            IFlatBOM filteredBom = new FlatBOM(filteredBomItems);
            CurrentSession.AddValue(Session.SessionKeys.SessionBom, filteredBom);

            //light on
            if (wipbufferList != null && wipbufferList.Count() > 0)
            {
                IPalletRepository pltRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
                List<string> TagIDList = new List<string>();
                foreach (WipBuffer wipbufobj in wipbufferList)
                {
                    IList<KittingLocPLMappingStInfo> kitlocstinfos = pltRepository.GetKitLocPLMapST(Line.Substring(0, 1), currentStationDescr, short.Parse(wipbufobj.LightNo));
                    if (kitlocstinfos != null && kitlocstinfos.Count > 0)
                    {
                        TagIDList.Add(kitlocstinfos[0].tagID);
                    }
                }
                pltRepository.UpdateKitLocationFVOnDefered(CurrentSession.UnitOfWork, TagIDList.ToArray<string>(), true, false, "1");
            }

            return base.DoExecute(executionContext);
        }

        private bool PartMatchWipBuffer(string pn, IList<IPart> partlist)
        {
            if (pn == partlist[0].PN)
            {
                return true;
            }
            //foreach (IPart part in partlist)
            //{
            //    if (pn == part.PN)
            //    {
            //        return true;
            //    }
            //    //if (pn.Substring(0, 3).Equals("DIB"))
            //    //{
            //    //    pn = pn.Substring(3);
            //    //}
            //    //if (pn == part.PN)
            //    //{
            //    //    return true;
            //    //}
            //}
            return false;
        }
    }
}
