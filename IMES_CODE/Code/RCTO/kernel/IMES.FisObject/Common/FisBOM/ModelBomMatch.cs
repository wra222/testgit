using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.FisObject.Common.FisBOM
{
    public class ModelBomMatch
    {
        public List<PNQty> GetPNQty(string type, IHierarchicalBOM GivenBom)
        {
            List<PNQty> result = new List<PNQty>();
            IList<IBOMNode> SearchBOMNodes = new List<IBOMNode>();
            switch (type)
            {
                case "WWAN":
                    SearchBOMNodes = GivenBom.FirstLevelNodes.Where(bomNode => BMP1KPType.Contains<string>(bomNode.Part.BOMNodeType) && bomNode.Part.Descr.StartsWith("WWAN")).ToList();
                    return GetPNQtyBMP1(SearchBOMNodes);
                case "WL":
                    SearchBOMNodes = GivenBom.FirstLevelNodes.Where(bomNode => BMP1KPType.Contains<string>(bomNode.Part.BOMNodeType) && bomNode.Part.Descr.StartsWith("WIRELESS")).ToList();
                    return GetPNQtyBMP1(SearchBOMNodes);
                case "HDD":
                    SearchBOMNodes = GivenBom.FirstLevelNodes.Where(bomNode => BMP1KPType.Contains<string>(bomNode.Part.BOMNodeType) && bomNode.Part.Descr.StartsWith("HDD")).ToList();
                    return GetPNQtyBMP1(SearchBOMNodes);
                case "DDR":
                    SearchBOMNodes = GivenBom.FirstLevelNodes.Where(bomNode => BMP1KPType.Contains<string>(bomNode.Part.BOMNodeType) && (bomNode.Part.Descr.Contains("MEM") || bomNode.Part.Descr.Contains("RAM"))).ToList();
                    return GetPNQtyBMP1(SearchBOMNodes);
                case "CPU":
                    SearchBOMNodes = GivenBom.FirstLevelNodes.Where(bomNode => bomNode.Part.BOMNodeType == "BM" && bomNode.Part.Descr.Contains("CPU")).ToList();
                    return GetPNQtyBM(SearchBOMNodes);
                case "LCM":
                    SearchBOMNodes = GivenBom.FirstLevelNodes.Where(bomNode => BMP1KPType.Contains<string>(bomNode.Part.BOMNodeType) && bomNode.Part.Descr.StartsWith("LCM")).ToList();
                    return GetPNQtyBMP1(SearchBOMNodes);
                case "BAT":
                    SearchBOMNodes = GivenBom.FirstLevelNodes.Where(bomNode => VKC4Type.Contains<string>(bomNode.Part.BOMNodeType)).ToList();
                    return GetPNQtyBAT(SearchBOMNodes);
                case "KB":
                    SearchBOMNodes = GivenBom.FirstLevelNodes.Where(bomNode => BMP1KPType.Contains<string>(bomNode.Part.BOMNodeType) && (bomNode.Part.Descr.Contains("KB") || bomNode.Part.Descr.Contains("K/B"))).ToList();
                    return GetPNQtyBMP1(SearchBOMNodes);
                case "ODD":
                    SearchBOMNodes = GivenBom.FirstLevelNodes.Where(bomNode => BMP1KPType.Contains<string>(bomNode.Part.BOMNodeType) && (bomNode.Part.Descr.StartsWith("DVD")
                        || bomNode.Part.Descr.StartsWith("CD") || bomNode.Part.Descr.StartsWith("ODD") || bomNode.Part.Descr.StartsWith("COMBO") || bomNode.Part.Descr.StartsWith("VCD"))).ToList();
                    return GetPNQtyBMP1(SearchBOMNodes);
                case "Inverter":
                    SearchBOMNodes = GivenBom.FirstLevelNodes.Where(bomNode => bomNode.Part.BOMNodeType == "PL" && bomNode.Part.Descr.Contains("Inverter")).ToList();
                    return GetPNQtyPL(SearchBOMNodes);
                case "TPDL":
                    SearchBOMNodes = GivenBom.FirstLevelNodes.Where(bomNode => bomNode.Part.BOMNodeType == "PL" && bomNode.Part.Descr == "JGS" && bomNode.Part.GetAttribute("Descr") == "TPDL").ToList();
                    return GetPNQtyPL(SearchBOMNodes);
                case "BTDL":
                    SearchBOMNodes = GivenBom.FirstLevelNodes.Where(bomNode => bomNode.Part.BOMNodeType == "PL" && bomNode.Part.Descr == "JGS" && bomNode.Part.GetAttribute("Descr") == "BTDL").ToList();
                    return GetPNQtyPL(SearchBOMNodes);
                case "TPCB":
                    SearchBOMNodes = GivenBom.FirstLevelNodes.Where(bomNode => bomNode.Part.BOMNodeType == "PL" && bomNode.Part.Descr == "JGS" && bomNode.Part.GetAttribute("Descr") == "TPCB").ToList();
                    return GetPNQtyPL(SearchBOMNodes);
                case "BTCB":
                    SearchBOMNodes = GivenBom.FirstLevelNodes.Where(bomNode => bomNode.Part.BOMNodeType == "PL" && bomNode.Part.Descr == "JGS" && bomNode.Part.GetAttribute("Descr") == "BTCB").ToList();
                    return GetPNQtyPL(SearchBOMNodes);
                case "TPCB2":
                    SearchBOMNodes = GivenBom.FirstLevelNodes.Where(bomNode => bomNode.Part.BOMNodeType == "PL" && bomNode.Part.Descr == "JGS" && bomNode.Part.GetAttribute("Descr") == "TPCB2" && bomNode.Part.PN.StartsWith("151")).ToList();
                    return GetPNQtyPL(SearchBOMNodes);
                default:
                    break;
            }

            return result;
        }
        //WWAN~WL~HDD~DDR~CPU~LCM~BAT~KB~ODD~Inverter~TPDL~BTDL~TPCB~BTCB~BTCB2~MB~VGA
        string[] BMP1Type = { "WWAN", "" };
        string[] PLType = { "", "" };

        private List<PNQty> GetPNQtyBMP1(IList<IBOMNode> BMP1Node)
        {
            List<PNQty> result = new List<PNQty>();
            if (BMP1Node != null && BMP1Node.Count > 0)
            {

                foreach (IBOMNode temp in BMP1Node)
                {
                    if (temp.Part.BOMNodeType == "KP")
                    {
                        string pn = temp.Part.GetAttribute(P1PN);
                        if (!string.IsNullOrEmpty(pn))
                        {
                            result.Add(new PNQty(pn, temp.Qty));
                        }
                    }
                    else if (temp.Part.BOMNodeType == "BM" || temp.Part.BOMNodeType == "P1")
                    {
                        if (temp.Children != null)
                        {
                            Dictionary<int, PNQty> ChildrenPNQty = new Dictionary<int, PNQty>();
                            IList<IBOMNode> childreBomNodes = temp.Children;
                            foreach (IBOMNode leafNode in childreBomNodes)
                            {
                                if (leafNode.Part.BOMNodeType == "KP" && !ChildrenPNQty.ContainsKey(leafNode.Qty))
                                {
                                    string pn = leafNode.Part.GetAttribute(P1PN);
                                    if (!string.IsNullOrEmpty(pn))
                                    {
                                        ChildrenPNQty.Add(leafNode.Qty, new PNQty(pn, leafNode.Qty * temp.Qty));
                                    }
                                }
                            }
                            result.AddRange(ChildrenPNQty.Values);
                        }
                    }
                }
            }
            return result;
        }

        private List<PNQty> GetPNQtyBM(IList<IBOMNode> BMNode)
        {
            List<PNQty> result = new List<PNQty>();
            if (BMNode != null && BMNode.Count > 0)
            {
                foreach (IBOMNode temp in BMNode)
                {
                    if (temp.Children != null)
                    {
                        Dictionary<int, PNQty> ChildrenPNQty = new Dictionary<int, PNQty>();
                        IList<IBOMNode> childreBomNodes = temp.Children;
                        foreach (IBOMNode leafNode in childreBomNodes)
                        {
                            if (leafNode.Part.BOMNodeType == "P1" && !ChildrenPNQty.ContainsKey(leafNode.Qty))
                            {
                                string pn = leafNode.Part.PN;
                                if (!string.IsNullOrEmpty(pn))
                                {
                                    ChildrenPNQty.Add(leafNode.Qty, new PNQty(pn, leafNode.Qty));
                                }
                            }
                        }
                        result.AddRange(ChildrenPNQty.Values);
                    }
                }
            }
            return result;
        }

        private List<PNQty> GetPNQtyBAT(IList<IBOMNode> NodeList)
        {
            List<PNQty> result = new List<PNQty>();
            if (NodeList != null && NodeList.Count > 0)
            {
                foreach (IBOMNode temp in NodeList)
                {
                    if (temp.Children != null)
                    {
                        IList<IBOMNode> P1Nodes = temp.Children;
                        foreach (IBOMNode P1Node in P1Nodes)
                        {
                            if (P1Node.Part.BOMNodeType == "P1" && P1Node.Part.Descr.StartsWith("BAT") && P1Node.Children != null)
                            {
                                IList<IBOMNode> KPNodes = P1Node.Children;
                                foreach (IBOMNode KPNode in KPNodes)
                                {
                                    if (KPNode.Part.BOMNodeType == "KP")
                                    {
                                        result.Add(new PNQty(P1Node.Part.PN, temp.Qty * P1Node.Qty * KPNode.Qty));
                                    }
                                }
                            }
                        }

                    }
                }
            }
            return result;
        }

        private List<PNQty> GetPNQtyPL(IList<IBOMNode> PLNodes)
        {
            List<PNQty> result = new List<PNQty>();
            if (PLNodes != null && PLNodes.Count > 0)
            {
                foreach (IBOMNode PLNode in PLNodes)
                {
                    result.Add(new PNQty(PLNode.Part.PN, PLNode.Qty));
                }
            }
            return result;
        }

        private IList<IBOMNode> GetBOMNodes(string[] bomNodeType, IHierarchicalBOM GivenBom)
        {
            return GivenBom.FirstLevelNodes.Where(bomNode => bomNodeType.Contains<string>(bomNode.Part.BOMNodeType)).ToList();

        }

        public const string P1PN = "P1PN";
        private static readonly string[] BMP1KPType = { "BM", "P1", "KP" };
        private static readonly string[] VKC4Type = { "VK", "C4" };
    }




    public class PNQty
    {
        public string Pn;
        public int Qty;

        public PNQty(string pn, int qty)
        {
            this.Pn = pn;
            this.Qty = qty;
        }
    }


}
