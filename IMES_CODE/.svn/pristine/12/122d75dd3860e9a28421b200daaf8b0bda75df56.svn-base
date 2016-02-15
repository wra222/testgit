using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.FisObject.Common.Part;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Text.RegularExpressions;
using IMES.FisObject.Common.NumControl;

namespace IMES.Maintain.Implementation
{
    public class AssetRangeManager : MarshalByRefObject, IAssetRange
    {
        #region IAssetRange 成员

        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IPartRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
        public IList<IMES.DataModel.AssetRangeDef> GetAllAssetRanges()
        {
            IList<AssetRangeDef> assetRangeItems = new List<AssetRangeDef>();
            try 
            {
                IList<AssetRangeInfo> assetRangeVos = itemRepository.GetAllAssetRanges();

                if(assetRangeVos!=null)
                {
                    foreach (AssetRangeInfo vo in assetRangeVos)
                    {
                        AssetRangeDef def = new AssetRangeDef();
                        def.id = vo.id;
                        def.code = vo.code;
                        def.begin = vo.begin;
                        def.end = vo.end;
                        def.remark = vo.remark;
                        def.editor = vo.editor;
                        def.cdt = vo.cdt.ToString("yyyy-MM-dd hh:mm:ss");
                        def.udt = vo.udt.ToString("yyyy-MM-dd hh:mm:ss");
                        assetRangeItems.Add(def);
                    }
                }
            }
            catch(Exception ee)
            {
                logger.Error(ee.Message);
                throw;
            }
            return assetRangeItems;
        }

        public string AddAssetRangeItem(IMES.DataModel.AssetRangeDef item)
        {
           
            //Exception e = null;
            string id="";
            try 
            {

                //IList<AssetRangeCodeInfo> lst = itemRepository.GetDuplicateAssetRange(item.code, item.begin, item.end);
                //string msg = "";
                //foreach (AssetRangeCodeInfo check in lst)
                //{
                //    if (item.begin.Length == check.Begin.Length)
                //    {
                //        msg += check.Begin.ToString() + "~" + check.End.ToString() + Environment.NewLine; 
                //    }
                //}
                //if (msg != "")
                //{
                //    e = new Exception("重複範圍：" + msg);
                //    throw e; 
                //}

                AssetRangeInfo newInfo = new AssetRangeInfo();

                newInfo.code = item.code; //item.code.ToUpper();
                newInfo.begin = item.begin;// item.begin.ToUpper();
                newInfo.end = item.end;// item.end.ToUpper();
                newInfo.remark = item.remark;
                newInfo.editor = item.editor;
                newInfo.cdt = DateTime.Now;
                newInfo.udt = DateTime.Now;
                newInfo.status = "R";
                itemRepository.AddAssetRangeItem(newInfo);
                id = newInfo.id.ToString();
            }
            catch(Exception ee)
            {
                logger.Error(ee.Message);
                throw;
            }
            return id;

        }

        public void UpdateAssetRangeItem(IMES.DataModel.AssetRangeDef item)
        {
            //Exception e = null;
            try
            {
                //IList<AssetRangeCodeInfo> lst = itemRepository.GetDuplicateAssetRange(item.code, item.begin, item.end);

                //if (lst.Count > 1 && item.begin.Length == lst[0].Begin.Length)
                //{
                //    string msg = "";
                //    foreach (AssetRangeCodeInfo errorrange in lst)
                //    {
                //        msg += errorrange.Begin.ToString() + "~" + errorrange.End.ToString() + Environment.NewLine;
                //    }
                //    e = new Exception("重複範圍：" + msg);
                //    throw e;
                //}
                //else if (lst.Count == 1)
                //{
                //    if (lst[0].ID != item.id && item.begin.Length == lst[0].Begin.Length)
                //    {
                //        string msg = lst[0].Begin.ToString() + "~" + lst[0].End.ToString();
                //        e = new Exception("重複範圍：" + msg);
                //        throw e;
                //    }
                //}

                AssetRangeInfo updatePo = new AssetRangeInfo();
                
                updatePo.udt = DateTime.Now;
                updatePo.id = item.id;
                updatePo.editor = item.editor;
                updatePo.code = item.code; //item.code.ToUpper();
                updatePo.begin = item.begin; //item.begin.ToUpper();
                updatePo.end = item.end; //item.end.ToUpper();
                updatePo.status = "R";
                updatePo.remark = item.remark;
                itemRepository.UpdateAssetRangeItem(updatePo);
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw;
            }

        }

        public void CheckAddRangeItem(AssetRangeDef item, string side)
        {
            Exception e = null;
            try
            {
                IList<AssetRangeCodeInfo> lst = itemRepository.GetDuplicateAssetRange(item.code, item.begin, item.end);
                string msg = "";
                foreach (AssetRangeCodeInfo check in lst)
                {
                    if (item.begin.Length == check.Begin.Length)
                    {
                        msg += check.Begin.ToString() + "~" + check.End.ToString() + Environment.NewLine;
                    }
                }
                if (msg != "")
                {
                    e = new Exception("重複範圍" + side + "：" + msg);
                    throw e;
                }
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw;
            }
        }

        public void CheckUpdateRangeItem(AssetRangeDef item, string side)
        {
            Exception e = null;
            try
            {
                IList<AssetRangeCodeInfo> lst = itemRepository.GetDuplicateAssetRange(item.code, item.begin, item.end);
                if (lst.Count > 1 && item.begin.Length == lst[0].Begin.Length)
                {
                    string msg = "";
                    foreach (AssetRangeCodeInfo errorrange in lst)
                    {
                        msg += errorrange.Begin.ToString() + "~" + errorrange.End.ToString() + Environment.NewLine;
                    }
                    e = new Exception("重複範圍：" + msg);
                    throw e;
                }
                else if (lst.Count == 1)
                {
                    if (lst[0].ID != item.id && item.begin.Length == lst[0].Begin.Length)
                    {
                        string msg = lst[0].Begin.ToString() + "~" + lst[0].End.ToString();
                        e = new Exception("重複範圍：" + msg);
                        throw e;
                    }
                }
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw;
            }
        }

        public void DeleteAssetRangeItem(int id)
        {
            try 
            {
                itemRepository.DeleteAssetRangeItem(id);
            }
            catch(Exception ee)
            {
                logger.Error(ee.Message);
                throw;
            }
        }

        public int GetBeginLength(string Code)
        {
            try
            {
                return itemRepository.GetAssetRangeLength(Code);
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw;
            }
        }

        public IList<string> GetCodeListInAssetRange()
        {
            try
            {
                return itemRepository.GetCodeListInAssetRange();
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw;
            }
        }

        public IList<AssetRangeDef> GetAssetRangeByCode(string code)
        {
            IList<AssetRangeDef> assetRangeItems = new List<AssetRangeDef>();
            try
            {
                IList<AssetRangeInfo> assetRangeVos = itemRepository.GetAssetRangeByCode(code);

                if (assetRangeVos != null)
                {
                    foreach (AssetRangeInfo vo in assetRangeVos)
                    {
                        AssetRangeDef def = new AssetRangeDef();
                        def.id = vo.id;
                        def.code = vo.code;
                        def.begin = vo.begin;
                        def.end = vo.end;
                        def.status = vo.status;
                        def.remark = vo.remark;
                        def.editor = vo.editor;
                        def.cdt = vo.cdt.ToString("yyyy-MM-dd hh:mm:ss");
                        def.udt = vo.udt.ToString("yyyy-MM-dd hh:mm:ss");
                        assetRangeItems.Add(def);
                    }
                }
            }
            catch (Exception ee)
            {
                logger.Error(ee.Message);
                throw;
            }
            return assetRangeItems;
        }

        public void CheckAssetRange(string code, string beginNum, string endNum)
        {
            string lastPreString = "";
            string preEndString = "";

            if (beginNum.Length != endNum.Length)
            {
                throw new FisException("DMT159", new string[] { code });//begin end長度不相等
            }

            MatchCollection matches = Regex.Matches(beginNum, @"\d+");
            if (matches.Count == 0)
            {
                throw new FisException("DMT160", new string[] { code });//begin結尾不是流水號
            }
            Match lastNumMatch = matches[matches.Count - 1];

            matches = Regex.Matches(endNum, @"\d+");
            if (matches.Count == 0)
            {
                throw new FisException("DMT161", new string[] { code });//end結尾不是流水號
            }
            Match endNumMatch = matches[matches.Count - 1];

            if (lastNumMatch.Index != endNumMatch.Index ||
                lastNumMatch.Length != endNumMatch.Length)
            {
                throw new FisException("DMT162", new string[] { code });//begin end流水號長度不相等
            }

            //最後不適數字
            if (beginNum.Length != (lastNumMatch.Index + lastNumMatch.Length))//最後一位不是數字
            {
                throw new FisException("DMT163", new string[] { code });
            }

            lastPreString = beginNum.Substring(0, lastNumMatch.Index);
            preEndString = endNum.Substring(0, endNumMatch.Index);

            if (lastPreString != preEndString)
            {
                throw new FisException("DMT164", new string[] { code });//begin end 前段字串不相符
            }

            if (lastNumMatch.Value.CompareTo(endNumMatch.Value) == 1)
            {
                throw new FisException("DMT165", new string[] { code });//begin流水號大於end流水號
            }
        }

        private bool checkNewRange(string maxNum, string beginNum, string endNum)
        {

            int iBegin = beginNum.CompareTo(maxNum);
            int iEnd = endNum.CompareTo(maxNum);
            if ((iBegin == 1 && iEnd == -1) || iBegin == 0 || iEnd == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void CloseActiveRange(AssetRangeDef item)
        {
            if (item.status != "A")
            {
                throw new Exception("Status is not Active Status!!");
            }
            INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
            NumControl currentMaxNum = numCtrlRepository.GetMaxValue("AST", item.code);
            if (currentMaxNum == null)
            {
                throw new Exception("No max Asset Number!!"); 
            }

            CheckAssetRange(item.code, item.begin, item.end);

            if (checkNewRange(currentMaxNum.Value, item.begin, item.end))
            {
                throw new Exception(string.Format("max number:{0}  is not between begin number:{2} and end number:{3)", currentMaxNum.Value, item.begin, item.end));
            }


            // update Asset Rnage number
            AssetRangeInfo updatePo = new AssetRangeInfo();

            updatePo.udt = DateTime.Now;
            updatePo.id = item.id;
            updatePo.editor = item.editor;
            updatePo.code = item.code; //item.code.ToUpper();
            updatePo.begin = item.begin; //item.begin.ToUpper();
            updatePo.end = currentMaxNum.Value;
            updatePo.status = "C";
            updatePo.remark = item.remark;
            itemRepository.UpdateAssetRangeItem(updatePo);
        }

        #endregion
    }
}
