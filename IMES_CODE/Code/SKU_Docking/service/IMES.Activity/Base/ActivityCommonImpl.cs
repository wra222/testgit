﻿/*
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.ReprintLog;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using log4net;
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.PAK.COA;
using IMES.FisObject.Common.Hold;
using IMES.FisObject.Common.TestLog;
using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.Material;
using System.Net.Mail;
using IMES.FisObject.Common.Line;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure.Utility.Generates.impl;
using IMES.Infrastructure.Utility.Generates.intf;
using System.Globalization;
using IMES.FisObject.PAK.Carton;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActivityCommonImpl
    {
        #region singlton 
        private ActivityCommonImpl()
        {
        }
        private static readonly ActivityCommonImpl _instance = new ActivityCommonImpl();
        /// <summary>
        /// Instance used for singleton
        /// </summary>
        public static ActivityCommonImpl Instance
        {
            get
            {
                //if (_instance == null)
                //    _instance = new ActivityCommonImpl();
                return _instance;
            }
        }

        #endregion

        IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
        INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
        IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
        IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        ICOAStatusRepository coaRep = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
        IHoldRepository holdRep = RepositoryFactory.GetInstance().GetRepository<IHoldRepository>();
        IStationRepository stationRep = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();
        IMaterialRepository materialRepository = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository, Material>();
        IMaterialBoxRepository materialBoxRepository = RepositoryFactory.GetInstance().GetRepository<IMaterialBoxRepository, MaterialBox>();
        IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
        ILineRepository lineRep = RepositoryFactory.GetInstance().GetRepository<ILineRepository>();
        IFamilyRepository familyRep = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();

        #region static/const variable

        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static object _syncRoot_GetSeq = new object();
        private static object _syncRoot_GetAst = new object();
        private static object _syncRoot_GetCT = new object();
        private static object _syncRoot_GetNum = new object();
        private object _sync_GetMAC = new object();
        private static string ShippingAssetTag = "shipping";
        //private static string ChassisAssetTag = "chassis";

        #endregion

        #region members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultPrefix">defaultPrefix</param>
        /// <param name="family">family</param>
        /// <returns></returns>
        public string GetCustSNPreFix3(string defaultPrefix, string family)
        {
            logger.Debug("(ActivityCommonImpl)GetCustSNPreFix3 start, family:" + family);

            try
            {
                string prefix = defaultPrefix;

                //IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IList<ConstValueInfo> cvInfo = new List<ConstValueInfo>();

                ConstValueInfo cvCond = new ConstValueInfo();
                cvCond.type = "CustSNExceptRule";
                cvCond.name = family;
                cvInfo = partRepository.GetConstValueInfoList(cvCond);
                if (cvInfo != null && cvInfo.Count > 0)
                {
                    ConstValueInfo tmp = cvInfo[0];
					prefix = tmp.value;
                }
                return prefix;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(ActivityCommonImpl)GetCustSNPreFix3 end, family:" + family);
            }
        }

        // mantis 1904
/*
(Digit 1 * 3) + (Digit 2 * 1) + (Digit 3 * 3) + (Digit 4 * 1) + (Digit 5 * 3) + (Digit 6 * 1) + (Digit 7 * 3) = Sum
Sum /10 = Number & Remainder
10 – Remainder = Checksum Digit（如果10 – Remainder=10则Checksum Digit为0）
*/
        /// <summary>
        /// 资产标签分配Range时需加校验码
        /// </summary>
        public string GetAstChecksum(string cust, string v)
        {
            bool isChecksum = false;
            IList<ConstValueTypeInfo> lstConstValueType = partRepository.GetConstValueTypeList("ASTPostFixCheckSum");
            if (null != lstConstValueType)
                foreach (ConstValueTypeInfo vi in lstConstValueType)
                {
                    if (cust.Equals(vi.value))
                    {
                        isChecksum = true;
                        break;
                    }
                }
            if (!isChecksum)
                return v;

            int sum = 0;
            int[] seeds = { 3, 1 };
            char[] cs = v.ToCharArray();
            for (int i=0; i<cs.Length; i++)
            {
                int n = (cs[i] - '0');
                if ((n > 9) || (n < 0))
                {
                    List<string> errpara = new List<string>();
                    throw new FisException("CHK1017", errpara);
                }
                n *= seeds[i % 2];
                sum += n;
            }

            int checksum = sum % 10;
            if (checksum != 0)
                checksum = 10 - checksum;
            v += checksum.ToString();
            return v;
        }

        /// <summary>
        /// GetConstValueListByType
        /// </summary>
		public IList<ConstValueInfo> GetConstValueListByType(string type, string orderby)
        {
            IList<ConstValueInfo> retLst = new List<ConstValueInfo>();
            try
            {
                if (!String.IsNullOrEmpty(type))
                {
                    //IPartRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                    var resultLst = partRepository.GetConstValueListByType(type);
                    if (!string.IsNullOrEmpty(orderby))
                    {
                        var tmpLst = from item in resultLst orderby item.id select item;
                        retLst = tmpLst.ToList();
                    }
                    else
                    {
                        retLst = resultLst;
                    }
                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }

        }
        /// <summary>
        /// GetConstValueTypeByType
        /// </summary>
        public IList<ConstValueTypeInfo> GetConstValueTypeByType(string type)
        {
            //IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<ConstValueTypeInfo> constValueTypeList = partRepository.GetConstValueTypeList(type);
            return constValueTypeList;
        }
        /// <summary>
        /// Get ConstValue by Type and Value
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetConstValueByTypeAndName(string type, string name, string defaultValue)
        {
           
            if (!String.IsNullOrEmpty(type) && !string.IsNullOrEmpty(name))
            {
                //IPartRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                ConstValueInfo info =new ConstValueInfo{
                                                        type=type, 
                                                         name=name};
                var resultLst = partRepository.GetConstValueInfoList(info);
                if (resultLst != null && resultLst.Count>0)
                {
                    return resultLst[0].value;
                }
                else
                {
                    return defaultValue;
                }
            }
            return defaultValue; 
        }

        /// <summary>
        /// 檢查model
        /// 參數: model, 是否匹配的大項(SKU,Docking...)
        /// </summary>
        public bool CheckModelByProcReg(string model, string ProcRegName)
        {
            bool isPass = false;
            IList<ConstValueInfo> lstConst = GetConstValueListByType("ProcReg", "Name").Where(x => x.value.Trim() != "").ToList();

            string[] regArr = null;
            if (! string.IsNullOrEmpty( ProcRegName ))
                regArr = ProcRegName.Split(',');
            else
                regArr = new string[]{};

            foreach (ConstValueInfo c in lstConst)
            {
                if (regArr.Contains(c.name))
                {
                    //Regex regex = new Regex(c.value);
                    if (Regex.IsMatch(model, c.value))
                    {
                        isPass = true;
                        break;
                    }
                }
            }
            return isPass;
        }
        /// <summary>
        /// 檢查model
        /// 參數: model, 是否匹配regex
        /// </summary>
        public bool CheckModelByProcReg(IList<ConstValueInfo> procRegList, string model, bool isPC)
        {
            bool isPass = false;
            foreach (ConstValueInfo c in procRegList)
            {
                if ((isPC && c.description == "Y") ||
                    (!isPC && c.description == "N"))
                {
                    //Regex regex = new Regex(c.value);
                    if (Regex.IsMatch(model, c.value, RegexOptions.Compiled))
                    {
                        isPass = true;
                        break;
                    }
                }
            }
            return isPass;
        }
        /// <summary>
        /// 檢查Model 為哪一個大類
        /// </summary>
        /// <param name="model">機型</param>      
        /// <returns></returns>
        public string CheckModelByProcReg(string model)
        {
           
            IList<ConstValueInfo> lstConst = GetConstValueListByType("ProcReg", "").Where(x => x.value.Trim() != "").ToList();

            return lstConst.Where(x => Regex.IsMatch(model, x.value)).Select(y => y.name).FirstOrDefault();
            
        }
		
		/// <summary>
        /// 檢查model
        /// 參數: model, 是否為整機
        /// </summary>
        public bool CheckModelByProcReg(string model, bool isPC)
        {
            bool isPass = false;
            IList<ConstValueInfo> lstConst = GetConstValueListByType("ProcReg", "Name").Where(x => x.value.Trim() != "").ToList();

            foreach (ConstValueInfo c in lstConst)
            {
                if ((isPC && c.description=="Y") ||
                    (!isPC && c.description == "N"))
                {
                    //Regex regex = new Regex(c.value);
                    if (Regex.IsMatch(model, c.value, RegexOptions.Compiled))
                    {
                        isPass = true;
                        break;
                    }
                }
            }
            return isPass;
        }

        /// <summary>
        /// 檢查model
        /// 參數: model, 是否匹配regex
        /// </summary>
        public bool CheckModelByRegex(string model, string regexModel)
        {
            bool isPass = false;
            //Regex regex = new Regex(regexModel);
            if (Regex.IsMatch(model,regexModel, RegexOptions.Compiled))
            {
                isPass = true;
            }
            return isPass;
        }
        /// <summary>
        /// 檢查model
        /// 參數: model, lstRegex
        /// </summary>
        public bool CheckModelByProcReg(string model, ref IList<Regex> lstRegex)
        {
            bool isPass = false;
            foreach (Regex regex in lstRegex)
            {
                if (regex.IsMatch(model))
                {
                    isPass = true;
                    break;
                }
            }
            return isPass;
        }

        /// <summary>
        /// GetRegexByProcReg
        /// 參數: 是否匹配的大項(SKU,Docking...)
        /// </summary>
        public IList<Regex> GetRegexByProcReg(string ProcRegName)
        {
            IList<Regex> lstRegex = new List<Regex>();
            IList<ConstValueInfo> lstConst = GetConstValueListByType("ProcReg", "Name").Where(x => x.value.Trim() != "").ToList();

            string[] regArr = null;
            if (!string.IsNullOrEmpty(ProcRegName))
                regArr = ProcRegName.Split(',');
            else
                regArr = new string[] { };

            foreach (ConstValueInfo c in lstConst)
            {
                if (regArr.Contains(c.name))
                {
                    Regex regex = new Regex(c.value);
                    lstRegex.Add(regex);
                }
            }
            return lstRegex;
        }

        /// <summary>
        ///  prefix attached date format 
        /// </summary>
        /// <param name="astNumber"></param>
        /// <param name="cust"></param>
        /// <returns></returns>
        public string CheckAndAddPreFixDateAst( string cust,string astNumber)
        {
            string ret = astNumber;
            ConstValueInfo info = new ConstValueInfo();
            info.type = "PreFixDateAST";
            info.name = cust;
            //IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<ConstValueInfo> retList = partRepository.GetConstValueInfoList(info);
            if (retList != null && retList.Count != 0)
            {
                if (!string.IsNullOrEmpty(retList[0].value))
                {
                    ret = DateTime.Now.ToString( retList[0].value.Trim()) + ret;
                }
            }

            return ret;
        }

        /// <summary>
        /// Check AST need add postFix
        /// </summary>
        /// <param name="cust"></param>
        /// <param name="astNumber"></param>
        /// <returns></returns>
        public string CheckAndAddPostFixAst(string cust, string astNumber)
        {
            string ret = astNumber;
            ConstValueInfo info = new ConstValueInfo();
            info.type = "AST";
            info.name = cust;
            //IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<ConstValueInfo> retList = partRepository.GetConstValueInfoList(info);
            if (retList != null && retList.Count != 0)
            {
                if (!string.IsNullOrEmpty(retList[0].value))
                {
                    ret += retList[0].value.Trim();                  
                }
            }

            return ret;
        }
        #endregion

        #region Check WIN8
        /// <summary>
        /// GetSite
        /// </summary>
        /// <returns></returns>
        public string GetSite()
        {
            //IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<string> valueList = partRepository.GetValueFromSysSettingByName("Site");
            if (valueList.Count == 0)
            {
                throw new FisException("PAK095", new string[] { "Site" });
            }
            return valueList[0].Trim(); //
        }

        /// <summary>
        /// CheckIsWIN8
        /// </summary>
        /// <param name="bom"></param>
        /// <returns></returns>
        public bool CheckIsWIN8(IHierarchicalBOM bom)
        {
            string site = GetSite();
            IList<IBOMNode> P1BomNodeList = bom.GetFirstLevelNodesByNodeType("P1");
            bool bWIN8 = false;
            if (site == "IPC")
            {
                foreach (IBOMNode bomNode in P1BomNodeList)
                {
                    if (bomNode.Part.Descr.StartsWith("ECOA"))
                    {
                        bWIN8 = true;
                        break;
                    }
                }
            }
            else
            {
                foreach (IBOMNode bomNode in P1BomNodeList)
                {
                    if (bomNode.Part.Descr.ToUpper().Contains("COA (WIN8)"))
                    {
                        bWIN8 = true;
                        break;
                    }
                }
            }
            return bWIN8;

        }
        #endregion

        #region for unpack Pizza
        /// <summary>
        ///  PAK unpack Pizza kitting
        /// </summary>
        /// <param name="currentSession"></param>
        /// <param name="editor"></param>
        ///  <param name="uow"></param>
        public void unPackPizzaPart(Session currentSession, IUnitOfWork uow,string editor)
        {
            //IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            Product prod = (Product)currentSession.GetValue(Session.SessionKeys.Product);
           // IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            Pizza pizza = prod.PizzaObj;
            Pizza secondPizza = prod.SecondPizzaObj;

            if (pizza != null)
            {
                unPackPizzaOfficeCD(uow, prod.PizzaID,prod.CUSTSN,  editor);
               
                repPizza.BackUpPizzaPart(prod.PizzaID, editor);
                pizza.RemoveAllParts();
               // repPizza.Remove(pizza, uow);
               
                pizza.Status.LineID = prod.Status.Line;
                pizza.Status.StationID = "00";
                repPizza.Update(pizza, uow);                
            }

            if (secondPizza != null)
            {
                repPizza.BackUpPizzaPart(secondPizza.PizzaID, editor);
                secondPizza.RemoveAllParts();
                repPizza.Remove(secondPizza, uow);
                
                //IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                //string[] prodids = new string[1];
                //prodids[0] = prod.ProId;

                //currentProductRepository.DeleteProductInfo(prodids, "KIT2");
            }
        }

        /// <summary>
        /// PAK unpack Product Part
        /// </summary>
        /// <param name="currentSession"></param>
        ///  <param name="uow"></param>
        /// <param name="editor"></param>
        public void unPackPAKProductPart(Session currentSession, IUnitOfWork uow, string editor)
        {
            Product productPartOwner = (Product)currentSession.GetValue(Session.SessionKeys.Product);
            string ProId = productPartOwner.ProId;

            //IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();

            IList<ProductPart> part_P1 = productRepository.GetProductPartByBomNodeTypeAndDescrLike(productPartOwner.ProId, "P1", "Royalty");
            if (part_P1.Count > 0)
            {
                foreach (ProductPart item in part_P1)
                {
                    if (!string.IsNullOrEmpty(item.PartSn))
                    {
                        unPackCOAStatus(uow, productPartOwner.CUSTSN, item.PartSn, editor);
                    }
                }
                productRepository.BackUpProductPartByBomNodeTypeAndDescrLike(ProId, editor, "P1", "Royalty%");
                productRepository.RemoveProductPartByBomNodeTypeAndDescrLike(ProId, "P1", "Royalty%");
                //currentProductRepository.Update(productPartOwner, uow);
            }

            IList<ProductPart> part_HomeCard = productRepository.GetProductPartByBomNodeTypeAndDescrLike(productPartOwner.ProId, "P1", "Home Card");
            if (part_HomeCard.Count > 0)
            {
                foreach (ProductPart item in part_HomeCard)
                {
                    if (!string.IsNullOrEmpty(item.PartSn))
                    {
                        unPackCOAStatus(uow, productPartOwner.CUSTSN, item.PartSn, editor);
                    }
                }
                productRepository.BackUpProductPartByBomNodeTypeAndDescrLike(productPartOwner.ProId, editor, "P1", "Home Card");
                productRepository.RemoveProductPartByBomNodeTypeAndDescrLike(ProId, "P1", "Home Card");
                //currentProductRepository.Update(productPartOwner, uow);
            }
            IList<ProductPart> part_NYLON = productRepository.GetProductPartByDescrLike(productPartOwner.ProId, "NYLON");
            if (part_NYLON.Count > 0)
            {

                foreach (ProductPart item in part_NYLON)
                {
                    if (!string.IsNullOrEmpty(item.PartSn))
                    {
                        unPackCOAStatus(uow, productPartOwner.CUSTSN, item.PartSn, editor);
                    }
                }

                productRepository.BackUpProductPartByDescrLike(productPartOwner.ProId, editor, "NYLON%");
                productRepository.RemoveProductPartByDescrLike(ProId, "NYLON%");
                //currentProductRepository.Update(productPartOwner, uow);
            }
            IList<ProductPart> part_Poster = productRepository.GetProductPartByDescrLike(productPartOwner.ProId, "Poster");
            if (part_Poster.Count > 0)
            {
                foreach (ProductPart item in part_Poster)
                {
                    if (!string.IsNullOrEmpty(item.PartSn))
                    {
                        unPackCOAStatus(uow, productPartOwner.CUSTSN, item.PartSn, editor);
                    }
                }
                productRepository.BackUpProductPartByDescrLike(productPartOwner.ProId, editor, "Poster%");
                productRepository.RemoveProductPartByDescrLike(ProId, "Poster%");
                //currentProductRepository.Update(productPartOwner, uow);
            }

            if (productRepository.CheckExistProductPart(productPartOwner.ProId, "PS"))
            {
                productRepository.BackUpProductPartByBomNodeType(productPartOwner.ProId, editor, "PS");
                productRepository.RemoveProductPartByBomNodeType(productPartOwner.ProId, "PS");
                //currentProductRepository.Update(productPartOwner, uow);
            }
        }

        /// <summary>
        /// Add office CD in COAStatus的狀態改為P1,並增加一條狀態為A2的COALog
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="pizzaId"></param>
        /// <param name="custSn"></param>
        /// <param name="editor"></param>
        public void unPackPizzaOfficeCD(IUnitOfWork uow, string pizzaId, string custSn,string editor)
        {
            //IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            //Check Office CD, need COAStatus的狀態改為P1,並增加一條狀態為A2的COALog 

            IList<string> partsn = repPizza.GetPartNoListFromPizzaPart(new string[]{pizzaId}, "P1", "DESC", "OOA");       



            if (partsn != null && partsn.Count > 0)
            {
                //ICOAStatusRepository coaRep = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
                foreach (string  partSn in partsn)
                {

                    COAStatus CurCOA = coaRep.Find(partSn);
                    if (CurCOA != null)
                    {
                        //DateTime now = DateTime.Now;
                        COALog logA2Status = new COALog()
                        {
                            COASN = partSn,
                            Editor = editor,
                            LineID = custSn,
                            StationID = "A2",
                            Tp = "",
                            Cdt = DateTime.Now
                        };

                        CurCOA.addCOALog(logA2Status);

                        COALog logP1Status = new COALog()
                        {
                            COASN = partSn,
                            Editor = editor,
                            LineID = custSn,
                            StationID = "P1",
                            Tp = "",
                            Cdt = DateTime.Now
                        };

                        CurCOA.addCOALog(logP1Status);

                        CurCOA.Status = "P1";
                        CurCOA.Editor = editor;
                        CurCOA.Cdt = DateTime.Now;
                        coaRep.Update(CurCOA, uow);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="custSn"></param>
        /// <param name="partSn"></param>
        /// <param name="editor"></param>
        public void unPackCOAStatus(IUnitOfWork uow, string custSn, string partSn, string editor)
        {
           // ICOAStatusRepository coaRep = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
           COAStatus CurCOA = coaRep.Find(partSn);
            if (CurCOA != null)
            {
                //DateTime now = DateTime.Now;
                COALog logA2Status = new COALog()
                {
                    COASN = partSn,
                    Editor = editor,
                    LineID = custSn,
                    StationID = "A2",
                    Tp = "",
                    Cdt = DateTime.Now
                };

                CurCOA.addCOALog(logA2Status);

                COALog logP1Status = new COALog()
                {
                    COASN = partSn,
                    Editor = editor,
                    LineID = custSn,
                    StationID = "P1",
                    Tp = "",
                    Cdt = DateTime.Now
                };

                CurCOA.addCOALog(logP1Status);

                CurCOA.Status = "P1";
                CurCOA.Editor = editor;
                CurCOA.Cdt = DateTime.Now;
                coaRep.Update(CurCOA, uow);
            }
            
        }

        /// <summary>
        /// Add 並增加一條狀態為A2的COALog
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="pizzaId"></param>
        /// <param name="custSn"></param>
        /// <param name="editor"></param>
        public void unPackPizzaOfficeCDLog(IUnitOfWork uow, string pizzaId, string custSn, string editor)
        {
            //IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            //Check Office CD, need COAStatus的狀態改為P1,並增加一條狀態為A2的COALog 

            IList<string> partsn = repPizza.GetPartNoListFromPizzaPart(new string[] { pizzaId }, "P1", "DESC", "OOA");

            if (partsn != null && partsn.Count > 0)
            {
                //ICOAStatusRepository coaRep = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
                foreach (string partSn in partsn)
                {

                    COAStatus CurCOA = coaRep.Find(partSn);
                    if (CurCOA != null)
                    {
                        DateTime now = DateTime.Now;
                        COALog logA2Status = new COALog()
                        {
                            COASN = partSn,
                            Editor = editor,
                            LineID = custSn,
                            StationID = "A2",
                            Tp = "",
                            Cdt = now
                        };

                        CurCOA.addCOALog(logA2Status);                        
                        coaRep.Update(CurCOA, uow);
                    }
                }
            }
        }

        /// <summary>
        /// PackOfficeCD Log 
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="partSNList"></param>
        /// <param name="custSn"></param>
        /// <param name="editor"></param>
        public void unPackOfficeCDLog(IUnitOfWork uow, IList<string> partSNList, string custSn, string editor)
        {
            if (partSNList != null && partSNList.Count > 0)
            {
                //ICOAStatusRepository coaRep = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
                foreach (string partSn in partSNList)
                {

                    COAStatus CurCOA = coaRep.Find(partSn);
                    if (CurCOA != null)
                    {
                        DateTime now = DateTime.Now;
                        COALog logA2Status = new COALog()
                        {
                            COASN = partSn,
                            Editor = editor,
                            LineID = custSn,
                            StationID = "A2",
                            Tp = "",
                            Cdt = now
                        };

                        CurCOA.addCOALog(logA2Status);
                        coaRep.Update(CurCOA, uow);
                    }
                }
            }
        }
        
        /// <summary>
        /// CheckDomesticDN
        /// </summary>
        /// <param name="regId"></param>
        /// <returns></returns>
        public bool CheckDomesticDN(string regId)
        {
            IList<string> domesticRegIdList= new List<string>(){
                "SCN",
                "QCN",
                "CN",
                "QET",
                "ET"
            };
            return domesticRegIdList.Contains(regId);
        }
        #endregion

        #region future Hold
        /// <summary>
        /// Check Future Hold
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="session"></param>
        /// <param name="prod"></param>
        /// <param name="line"></param>
        /// <param name="pdLine"></param>
        /// <param name="checkInStation"></param>
        /// <param name="editor"></param>
        public void CheckAndSetFutureHold(IUnitOfWork uow, 
                                                                Session session,
                                                                IProduct prod, 
                                                                string line, 
                                                                string pdLine, 
                                                                string checkInStation,
                                                                string editor)
        {
            //IHoldRepository holdRep = RepositoryFactory.GetInstance().GetRepository<IHoldRepository>();
            //IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
           // IStationRepository stationRep = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();
            //Check Check out Station 是否為Hold Station
            string curStation = prod.Status.StationId;
            if(string.IsNullOrEmpty(curStation))
            {
                return;
            }

            string isHold=stationRep.GetStationAttrValue(curStation, "IsHold");
            session.AddValue("IsHoldStation", "N");
            if (isHold == "Y")
            {
                session.AddValue("IsHoldStation", "Y");
                return;
            }

            //Check PreStation 是否為Hold Station 
            IList<ProductStatusExInfo> preStatusList = productRepository.GetProductPreStation(new List<string> { prod.ProId });
            if (preStatusList != null && preStatusList.Count > 0)
            {
                string preStation = preStatusList[0].PreStation;
                if (!string.IsNullOrEmpty(preStation))
                {
                    session.AddValue("PreStation", preStatusList[0]);
                    isHold = stationRep.GetStationAttrValue(preStation, "IsHold");
                    if (isHold == "Y")
                    {   
                        session.AddValue("IsPreHoldStation", "Y");                      
                        return;
                    }
                    session.AddValue("IsPreHoldStation", "N"); 
                }
            }

            //IList<HoldRulePriorityInfo> hodRuleInfoList = holdRep.GetHoldRulePriority(line, prod.Family, prod.Model, prod.CUSTSN, checkInStation).Where(x=>x.Priority>0).ToList();
            IList<HoldRulePriorityInfo> hodRuleInfoList = orderHoldRule(line, prod.Family, prod.Model, prod.CUSTSN, checkInStation).Where(x => x.Priority > 0).ToList();
            if (hodRuleInfoList != null && hodRuleInfoList.Count > 0)
            {
                hodRuleInfoList=hodRuleInfoList.OrderBy(x => x.Priority).ToList();
                HoldRulePriorityInfo holdInfo = hodRuleInfoList[0];
                string preStation =prod.Status.StationId; 
                writeHoldStation(uow, 
                                        prod,
                                        pdLine,
                                        "FutureHold",
                                         holdInfo.HoldStation,
                                         holdInfo.HoldCode,
                                         "Descr:" +holdInfo.HoldDescr +"~PreStation:" + preStation + "~CurStation:" + checkInStation + "~FutureHold ID:" + holdInfo.ID.ToString() + "~FutureHold Priority:" + holdInfo.Priority,
                                         editor);               

                //throw error message
                throw new FisException("CQCHK0030", new string[] { prod.ProId, prod.CUSTSN, holdInfo.ID.ToString(), holdInfo.HoldDescr});

            }
        }

        private IList<HoldRulePriorityInfo> orderHoldRule(string line, string family, string model, string sn, string checkInStation)
        {
             //IHoldRepository holdRep = RepositoryFactory.GetInstance().GetRepository<IHoldRepository>();
             IList<HoldRuleInfo> hodRuleInfoList = holdRep.GetHoldRule(new HoldRuleInfo { CheckInStaion = checkInStation });
             IList<HoldRulePriorityInfo> holdRulePriorityList = new List<HoldRulePriorityInfo>();  
            if (hodRuleInfoList != null && hodRuleInfoList.Count > 0)
             {
                 holdRulePriorityList = (from p in hodRuleInfoList
                                                                     select new HoldRulePriorityInfo
                                                                     {
                                                                         ID = p.ID,
                                                                         HoldCode = p.HoldCode,
                                                                         HoldDescr = p.HoldDescr,
                                                                         HoldStation = p.HoldStaion,
                                                                         Priority = getHoldRulePriority(line, family, model, sn, p)
                                                                     }).ToList();
             }             
             return holdRulePriorityList;                                                                    

        }

        private int getHoldRulePriority(string line, string family, string model, string sn, HoldRuleInfo holdRuleInfo)
        {
            if (!string.IsNullOrEmpty(holdRuleInfo.CUSTSN) && holdRuleInfo.CUSTSN == sn)
            {
                return 1;
            }
            else if ( line==holdRuleInfo.Line &&
                        string.IsNullOrEmpty(holdRuleInfo.Family) &&
                        Regex.IsMatch(model, holdRuleInfo.Model) &&
                        string.IsNullOrEmpty(holdRuleInfo.CUSTSN))
            {
                return 2;
            }
            else if (line == holdRuleInfo.Line &&
                        string.IsNullOrEmpty(holdRuleInfo.Model) &&
                       family==holdRuleInfo.Family &&
                        string.IsNullOrEmpty(holdRuleInfo.CUSTSN))
            {
                return 3;
            }
            else if (string.IsNullOrEmpty(holdRuleInfo.Line) &&
                        string.IsNullOrEmpty(holdRuleInfo.Family) &&
                        Regex.IsMatch(model, holdRuleInfo.Model) &&
                        string.IsNullOrEmpty(holdRuleInfo.CUSTSN))
            {
                return 4;
            }
            else if (string.IsNullOrEmpty(holdRuleInfo.Line) &&
                        string.IsNullOrEmpty(holdRuleInfo.Model) &&
                        family== holdRuleInfo.Family &&
                        string.IsNullOrEmpty(holdRuleInfo.CUSTSN))
            {
                return 5;
            }
            else if (string.IsNullOrEmpty(holdRuleInfo.Family) &&
                        string.IsNullOrEmpty(holdRuleInfo.Model) &&
                        line== holdRuleInfo.Line &&
                        string.IsNullOrEmpty(holdRuleInfo.CUSTSN))
            {
                return 6;
            }
            else
            {
                return -1;
            }

        }

        /// <summary>
        /// Hold Station
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="prod"></param>
        /// <param name="line"></param>
        /// <param name="holdActionName"></param>
        /// <param name="holdStation"></param>
        /// <param name="holdCode"></param>
        /// <param name="holdDescr"></param>
        /// <param name="editor"></param>
        public void writeHoldStation(IUnitOfWork uow, 
                                                 IProduct prod, 
                                                 string line, 
                                                 string holdActionName,
                                                  string holdStation, 
                                                  string holdCode, 
                                                  string holdDescr, 
                                                    string editor)
        {
           // IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();

            DateTime now = DateTime.Now;
            string preStation = prod.Status.StationId;
            uow = uow ?? new UnitOfWork();

            #region update ProductStatusEx
            IList<IMES.DataModel.TbProductStatus> stationList = productRepository.GetProductStatus(new List<string> { prod.ProId });
            productRepository.UpdateProductPreStationDefered(uow, stationList);
            #endregion

            #region update ProductStatus
            prod.UpdateStatus(new IMES.FisObject.FA.Product.ProductStatus()
            {
                Line = line,
                ReworkCode="",
                 ProId=prod.ProId,
                 TestFailCount=0,
                StationId = holdStation,
                Status = StationStatus.Fail,
                Editor = editor,
                Udt = now
            });           
          
            #endregion

            #region write Productlog

            ProductLog productLog = new ProductLog
            {
                Model = prod.Model,
                Status = IMES.FisObject.Common.Station.StationStatus.Fail,
                Editor = editor,
                Line = line,
                Station = holdStation,
                Cdt = now
            };
            prod.AddLog(productLog);
            #endregion

            #region add test log
            string actionName = holdActionName;
            string errorCode = "";
            string descr = holdDescr; // "PreStation:" + preStation + "~CurStation:" + checkInStation + "~FutureHold ID:" + holdInfo.ID.ToString() + "~FutureHold Priority:" + holdInfo.Priority;


            TestLog testLog = new TestLog(0, prod.ProId, line, "", holdStation, TestLog.TestLogStatus.Fail, "",
                                                                actionName, errorCode, descr, editor, "PRD", now);

            prod.AddTestLog(testLog);
            //add defect
            TestLogDefect defectItem = new TestLogDefect(0, 0, holdCode, editor, now);
            testLog.AddTestLogDefect(defectItem);
            #endregion


            productRepository.Update(prod, uow);
            uow.Commit();
        }
        #endregion

        #region changeCPU
        /// <summary>
        ///  NeedCheckMaterialCpuStatus
        /// </summary>
        /// <param name="product"></param>
        /// <param name="partSn"></param>
        /// <param name="session"></param>
        public bool NeedCheckMaterialCpuStatus(IProduct product, string partSn, ref Session session)
        {
            if (string.IsNullOrEmpty(partSn) || partSn.Length < 5)
                return false;
            String currentVendorCode = partSn.Substring(0, 5);

            string valCheckMaterialStatus = "";
            IList<ConstValueInfo> valueList = partRepository.GetConstValueListByType("CheckMaterialStatus");
            if (null != valueList)
            {
                var valueListCpu = (from p in valueList
                                    where p.name == "CPU"
                                    select p).ToList();
                if (valueListCpu != null && valueListCpu.Count > 0)
                    valCheckMaterialStatus = valueListCpu[0].value;
            }
            if ("Y" != valCheckMaterialStatus && "N" != valCheckMaterialStatus)
            {
                // 请联系IE维护是否需要检查CPU状态
                throw new FisException("CQCHK0050", new string[] { });
            }

            bool needCheckMaterialStatus = false;
            if ("Y" == valCheckMaterialStatus)
            {
                needCheckMaterialStatus = true;

                IList<ConstValueTypeInfo> lstConstValueType = partRepository.GetConstValueTypeList("NoCheckCPUStatus");
                if (null != lstConstValueType)
                {
                    foreach (ConstValueTypeInfo cvt in lstConstValueType)
                    {
                        if (cvt.value == product.Family)
                        {
                            needCheckMaterialStatus = false;
                            break;
                        }
                    }
                }
            }

            if (needCheckMaterialStatus)
            {
               // IMaterialRepository MaterialRepository = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository, Material>();

                IList<string> lst = new List<string>();
                lst.Add(partSn);
                IList<Material> lstMaterials = materialRepository.GetMaterialByMultiCT(lst);
                if (null == lstMaterials || lstMaterials.Count == 0)
                {
                    // 此CPU:@CPUCT未收集
                    FisException fex = new FisException("CQCHK0051", new string[] { partSn });
                    //fex.stopWF = false;
                    throw fex;
                }

                Material mat = lstMaterials[0];
                if ("Collect" != mat.Status && "Dismantle" != mat.Status)
                {
                    // 此CPU：@CPUCT为不可结合状态
                    FisException fex = new FisException("CQCHK0052", new string[] { partSn });
                   // fex.stopWF = false;
                    throw fex;
                }

                string spec = "";
               // IMaterialBoxRepository materialBoxRepository = RepositoryFactory.GetInstance().GetRepository<IMaterialBoxRepository, MaterialBox>();
                IList<MaterialBox> lstBox = materialBoxRepository.GetMaterialBoxByLot("CPU", mat.LotNo);
                if (null != lstBox && lstBox.Count > 0)
                    spec = lstBox[0].SpecNo;

                string infor = "";
                //IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                IHierarchicalBOM curBOM = bomRepository.GetHierarchicalBOMByModel(product.Model);
                if (curBOM == null)
                {
                    throw new FisException("CHK986", new string[] { "Model ", "BOM", product.Model });
                }
                IList<IBOMNode> bomNodeLst = curBOM.FirstLevelNodes;
                if (null != bomNodeLst)
                {
                    foreach (IBOMNode bn in bomNodeLst)
                    {
                        IPart p = bn.Part;
                        if (p.Descr != null && p.Descr.IndexOf("CPU") >= 0 && p.Attributes != null)
                        {
                            foreach (PartInfo pi in p.Attributes)
                            {
                                if ("Infor".Equals(pi.InfoType))
                                {
                                    infor = pi.InfoValue;
                                    break;
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(infor))
                            break;
                    }
                }
                if (spec == "" || spec != infor)
                {
                    // 请核对CPU料号
                    FisException fex = new FisException("CQCHK0053", new string[] { });
                    //fex.stopWF = false;
                    throw fex;
                }

                session.AddValue("MaterialCpu", mat);

            } // needCheckMaterialStatus

            if (needCheckMaterialStatus)
                session.AddValue("CheckMaterialCpuStatus", "Y");
            else
                session.AddValue("CheckMaterialCpuStatus", "N");

            return needCheckMaterialStatus;
        }

        /// <summary>
        ///  UpdateMaterialCpuStatus
        /// </summary>
        /// <param name="partSn"></param>
        /// <param name="action"></param>
        /// <param name="status"></param>
		/// <param name="throwExceptionWhenNoCpuSn"></param>
		/// <param name="needCheckCollected"></param>
        /// <param name="session"></param>
        public void UpdateMaterialCpuStatus(string partSn, string action, string status, bool throwExceptionWhenNoCpuSn, bool needCheckCollected, ref Session session)
        {
            if (string.IsNullOrEmpty(partSn))
            {
				if (throwExceptionWhenNoCpuSn)
				{
					FisException fex = new FisException("Err: Empty SN when UpdateMaterialCpuStatus");
					//fex.stopWF = false;
					throw fex;
				}
				else
				{
					return;
				}
            }

            string snCpu = partSn;

            string CheckMaterialStatus = session.GetValue("CheckMaterialCpuStatus") as string;
            
            //IMaterialRepository MaterialRepository = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository, Material>();

            IList<string> lst = new List<string>();
            lst.Add(partSn);
            IList<Material> lstMaterials = materialRepository.GetMaterialByMultiCT(lst);
            if (null == lstMaterials || lstMaterials.Count == 0)
            {
                if (needCheckCollected && throwExceptionWhenNoCpuSn)
				{
					// 此CPU:@CPUCT未收集
					FisException fex = new FisException("CQCHK0051", new string[] { partSn });
					//fex.stopWF = false;
					throw fex;
				}
				else
				{
					return;
				}
            }

            Material mat = lstMaterials[0];

            MaterialLog mlog = new MaterialLog();
            mlog.Action = action;
            mlog.Cdt = DateTime.Now;
            mlog.Comment = "";
            mlog.Editor = session.Editor;
            mlog.Line = session.Line;
            mlog.MaterialCT = snCpu;
            mlog.PreStatus = mat.Status;
            mlog.Stage = "FA";
            mlog.Status = status;

            mat.AddMaterialLog(mlog);

            mat.PreStatus = mat.Status;
            mat.Status = status;
            mat.Udt = DateTime.Now;
            materialRepository.Update(mat, session.UnitOfWork);
        }
		
        #endregion

        #region CombinedAstRange
        /// <summary>
        /// CheckAndSetReleaseAstNumber
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="code"></param>
        /// <param name="astType"></param>
        /// <param name="station"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        public string CheckAndSetReleaseAstNumber(string productId, string code, string astType,string station, string editor)
        {
            try
            {
                string ast = "";
               // IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                SqlTransactionManager.Begin();
                lock (_syncRoot_GetAst)
                {

                    //2.Check release Asset number
                    ReleasedAstNumberInfo releasedAst = productRepository.GetAvailableReleaseAstNumberWithReadPast(code, astType, "Release");
                    if (releasedAst != null)
                    {
                        ast = releasedAst.AstNo;
                        IUnitOfWork uow = new UnitOfWork();
                        productRepository.UpdateCombinedAstNumberDefered(uow,
                                                                                             new CombinedAstNumberInfo
                                                                                             {
                                                                                                 ID = releasedAst.ID,
                                                                                                 ProductID = productId,
                                                                                                 Station = station,
                                                                                                 State = "Used",
                                                                                                 Editor = editor,
                                                                                                 Udt = DateTime.Now
                                                                                             });
                        uow.Commit();
                    }
                    SqlTransactionManager.Commit();
                }
                return ast;
            }
            catch (Exception e)     //2012-7-19
            {
                SqlTransactionManager.Rollback();
                throw e;
            }
            finally                 //2012-7-19
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }
        }
        /// <summary>
        /// CheckAndGetUsedAst
        /// </summary>
        /// <param name="session"></param>
        /// <param name="productId"></param>
        /// <param name="code"></param>
        /// <param name="astType"></param>
        /// <param name="station"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        public string CheckAndGetUsedAst(Session session, string productId, string code, string astType,string station, string editor)
        {
            //IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
            //1.Check used Asset number
            IList<CombinedAstNumberInfo> usedAstInfoList = productRepository.GetCombinedAstNumber(new CombinedAstNumberInfo { ProductID = productId, AstType = astType, Code = code, State = "Used" });
            if (usedAstInfoList != null && usedAstInfoList.Count > 0)
            {
                CombinedAstNumberInfo usedAstInfo = usedAstInfoList[0];
                usedAstInfo.Udt = DateTime.Now;
                usedAstInfo.Remark = "ReAssigned";
                usedAstInfo.Station = station;
                usedAstInfo.Editor = editor;
                productRepository.UpdateCombinedAstNumberDefered(session.UnitOfWork, usedAstInfo);
                return usedAstInfo.AstNo;

            }

            return null;
        }
        /// <summary>
        /// InsertCombinedAstNumber
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="code"></param>
        /// <param name="astType"></param>
        /// <param name="astNo"></param>
        /// <param name="station"></param>
        /// <param name="editor"></param>
        public void InsertCombinedAstNumber(string productId, string code, string astType, string astNo,string station,string editor)
        {
            //IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
            productRepository.InsertCombinedAstNumber(new CombinedAstNumberInfo
            {
                Code = code,
                AstNo = astNo,
                AstType = astType,
                ProductID = productId,
                Station = station,
                UnBindProductID = "",
                UnBindStation = "",
                State = "Used",
                Editor = editor,
                Remark = "",
                Cdt = DateTime.Now,
                Udt = DateTime.Now
            });
        }
        #endregion

        #region get print log BegNo && EndNo
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="printLogBegNo"></param>
        /// <param name="sessionName"></param>
        /// <returns></returns>
        public string GetPrintLogBegNoValue(Session session, PrintLogBegNoEnum printLogBegNo, string sessionName)
        {
            string ret = null;
            if (printLogBegNo == PrintLogBegNoEnum.ID)
            {
                IProduct prod = (IProduct)session.GetValue(Session.SessionKeys.Product);
                if (prod != null)
                {
                    ret = prod.ProId;
                }
                else
                {
                    IMES.FisObject.PCA.MB.IMB mb = (IMES.FisObject.PCA.MB.IMB)session.GetValue(Session.SessionKeys.MB);
                    if (mb != null)
                    {
                        ret = mb.Sn;
                    }
                }
            }
            else if (printLogBegNo == PrintLogBegNoEnum.CustSN)
            {
                IProduct prod = (IProduct)session.GetValue(Session.SessionKeys.Product);
                if (prod != null)
                {
                    ret = prod.CUSTSN;
                }
                else
                {
                    ret = session.GetValue(Session.SessionKeys.CustSN) as string;
                }
            }
            else if (printLogBegNo == PrintLogBegNoEnum.DeliveryNo)
            {
                ret = session.GetValue(Session.SessionKeys.DeliveryNo) as string;
            }
            else if (printLogBegNo == PrintLogBegNoEnum.PalletNo)
            {
                ret = session.GetValue(Session.SessionKeys.PalletNo) as string;
            }
            else if (printLogBegNo == PrintLogBegNoEnum.ModelName)
            {
                ret = session.GetValue(Session.SessionKeys.ModelName) as string;
            }
            else if (printLogBegNo == PrintLogBegNoEnum.Key)
            {
                ret = session.Key;
            }
            else if (!string.IsNullOrEmpty(sessionName))
            {
                ret = session.GetValue(sessionName) as string;
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="sessionName"></param>
        /// <returns></returns>
        public string GetPrintLogEndNoValue(Session session, string sessionName)
        {
            string ret = null;
            if (!string.IsNullOrEmpty(sessionName))
            {
                ret = session.GetValue(sessionName) as string;
            }
            return ret;
        }
        #endregion

        #region get Seesionkey and check
        /// <summary>
        /// Check nullorEmpty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public T IsNullOrEmpty<T>(Session session, string name) where T : class
        {
            T ret = IsNull<T>(session, name);
            Type typeT = ret.GetType();

            if (typeT == typeof(string))
            {
                if ("".Equals(ret))
                {
                    throw new FisException("CQCHK0006", new List<string> { name });
                }
            }
            return ret;
        }
        /// <summary>
        /// Check Null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public T IsNull<T>(Session session, string name)
        {
            object ret = session.GetValue(name);
            if (ret == null)
            {
                throw new FisException("CQCHK0006", new List<string> { name });
            }

            return (T)ret;
        }
        #endregion

        #region ConstValue ConstValueType 操作
        /// <summary>
        /// ConstValue
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IList<ConstValueInfo> ConstValue(string type, string name, out string value)
        {
            IList<ConstValueInfo> retLst = new List<ConstValueInfo>();
            value = null;
            if (!String.IsNullOrEmpty(type))
            {
                retLst = partRepository.GetConstValueInfoList(new ConstValueInfo { type = type });
                value = ConstValue(retLst, type, name);
            }
            return retLst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="descr"></param>
        /// <returns></returns>
        public IList<ConstValueInfo> ConstValue(string type, string name, out string value, out string descr)
        {
            IList<ConstValueInfo> retLst = new List<ConstValueInfo>();
            value = null;
            descr = null;
            if (!String.IsNullOrEmpty(type))
            {
                retLst = partRepository.GetConstValueInfoList(new ConstValueInfo { type = type });
                value = ConstValue(retLst, type, name, out descr);
            }
            return retLst;
        }

        /// <summary>
        ///  get out descr 
        /// </summary>
        /// <param name="constValueList"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="descr"></param>
        /// <returns></returns>
        public string ConstValue(IList<ConstValueInfo> constValueList, string type, string name, out string descr)
        {
            if (constValueList == null || constValueList.Count == 0)
            {
                throw new FisException("CQCHK0026", new string[] { type });
            }

            ConstValueInfo constValueInfo = constValueList.Where(x => x.name == name).FirstOrDefault();

            if (constValueInfo == null || string.IsNullOrEmpty(constValueInfo.value))
            {
                throw new FisException("CQCHK0049", new string[] { type, name });
            }
            descr = constValueInfo.description;
            return constValueInfo.value;
        }

        /// <summary>
        /// if not found then  don't throw error 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="infoList"></param>
        /// <param name="matchValue"></param>
        /// <returns></returns>
        public bool TryConstValue(string type, string name, out  IList<ConstValueInfo> infoList, out ConstValueInfo matchValue)
        {
            infoList = new List<ConstValueInfo>();
            matchValue = null;
            if (!String.IsNullOrEmpty(type))
            {
                infoList = partRepository.GetConstValueInfoList(new ConstValueInfo { type = type });
                matchValue = infoList.Where(x => x.name == name).FirstOrDefault();
                if (matchValue == null)
                {
                    return false;
                }
                return true;
            }
            return false;
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="infoList"></param>
       /// <param name="name"></param>
       /// <param name="matchValue"></param>
       /// <returns></returns>
        public bool TryConstValue(IList<ConstValueInfo> infoList, string name, out ConstValueInfo matchValue)
        {
            matchValue = null;
            matchValue = infoList.Where(x => x.name == name).FirstOrDefault();
            if (matchValue != null)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// ConstValue
        /// </summary>
        /// <param name="constValueList"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string ConstValue(IList<ConstValueInfo> constValueList, string type, string name)
        {
            if (constValueList == null || constValueList.Count == 0)
            {
                throw new FisException("CQCHK0026", new string[] { type });
            }

            string value = constValueList.Where(x => x.name == name).Select(y => y.value).FirstOrDefault();

            if (string.IsNullOrEmpty(value))
            {
                throw new FisException("CQCHK0049", new string[] { type, name });
            }
            return value;
        }
        /// <summary>
        /// TryConstValueType
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IList<ConstValueTypeInfo> ConstValueType(string type, string value)
        {
            IList<ConstValueTypeInfo> retLst = new List<ConstValueTypeInfo>();
            value = null;
            if (!String.IsNullOrEmpty(type))
            {
                retLst = partRepository.GetConstValueTypeList(type);
                ConstValueType(retLst, type, value);
            }
            return retLst;
        }
        /// <summary>
        /// don't throw error
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="valueList"></param>
        /// <returns></returns>
        public bool TryConstValueType(string type, string value, out  IList<ConstValueTypeInfo> valueList)
        {
            valueList = new List<ConstValueTypeInfo>();
            if (!String.IsNullOrEmpty(type))
            {
                valueList = partRepository.GetConstValueTypeList(type);
                if (valueList.Any(x => x.value == value))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// try matched data
        /// </summary>
        /// <param name="valueList"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryConstValueType(IList<ConstValueTypeInfo> valueList, string value)
        {
            if (valueList.Any(x => x.value == value))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// don't throw error
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="valueList"></param>
        /// <returns></returns>
        public bool TryConstValueTypeWithRE(string type, string value, out  IList<ConstValueTypeInfo> valueList)
        {
            valueList = new List<ConstValueTypeInfo>();
            if (!String.IsNullOrEmpty(type))
            {
                valueList = partRepository.GetConstValueTypeList(type);
                if (valueList.Any(x => Regex.IsMatch(value, x.value)))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// try matched data
        /// </summary>
        /// <param name="valueList"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryConstValueTypeWithRE(IList<ConstValueTypeInfo> valueList, string value)
        {
            if (valueList.Any(x => Regex.IsMatch(value, x.value)))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// ConstValueType
        /// </summary>
        /// <param name="constValueTypes"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public void ConstValueType(IList<ConstValueTypeInfo> constValueTypes, string type, string value)
        {

            if (constValueTypes == null || constValueTypes.Count == 0)
            {
                throw new FisException("TRC002", new string[] { type });
            }

            if (!string.IsNullOrEmpty(value))
            {
                if (!constValueTypes.Any(x => x.value == value))
                {
                    throw new FisException("CHK990", new string[] { "ConstValueTye", type, value });
                }
            }
        }

        /// <summary>
        /// Get SysSetting Data 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetSysSetting(string name, string defaultValue)
        {
           // IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<string> valueList = partRepository.GetValueFromSysSettingByName(name);
            if (defaultValue == null)
            {
                if (valueList.Count == 0)
                {
                    throw new FisException("PAK095", new string[] { name });
                }
                else
                {
                    return valueList[0].Trim();
                }
            }
            else
            {
                return valueList.Count == 0 ? defaultValue : valueList[0].Trim();
            }
        }
        /// <summary>
        /// Send email utility
        /// </summary>
        /// <param name="mailTo"></param>
        /// <param name="mailCC"></param>
        /// <param name="mailSubject"></param>
        /// <param name="mailBody"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool SendMail(IList<string> mailTo,
                                                    IList<string> mailCC,
                                                    string mailSubject,
                                                    string mailBody,
                                                    out string error)
        {

            try
            {
                error = "";
                MailMessage mailMsg = new MailMessage();
                if (string.IsNullOrEmpty(mailSubject) || mailTo.Count == 0)
                    return false;

                string mailServer = GetSysSetting("MailServer", null);
                string mailFrom = GetSysSetting("MailFromAddress", "");

                mailMsg.From = new MailAddress(mailFrom);

                foreach (string add in mailTo)
                {
                    if (add.TrimEnd().Length > 0)
                    {
                        mailMsg.To.Add(new MailAddress(add.TrimEnd()));
                    }
                }

                foreach (string add in mailCC)
                {
                    if (add.TrimEnd().Length > 0)
                    {
                        mailMsg.CC.Add(new MailAddress(add.TrimEnd()));
                    }
                }

                mailMsg.Subject = mailSubject;
                mailMsg.Body = mailBody;
                mailMsg.IsBodyHtml = true;

                SmtpClient client = new SmtpClient(mailServer, 25);
                if (mailMsg.To.Count > 0)
                {
                    client.Send(mailMsg);
                }
                return true;
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;

            }

        }
        #endregion

        #region Get Line and aliasLine
        /// <summary>
        /// Get Line object and Check LineEx
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="notExistsThrowError"></param>
        /// <returns></returns>
        public Line GetLine(string pdLine, bool notExistsThrowError)
        {
            if (!string.IsNullOrEmpty(pdLine))
            {
                //ILineRepository lineRep = RepositoryFactory.GetInstance().GetRepository<ILineRepository>();
                Line line = lineRep.Find(pdLine);
                if (line == null ||
                   line.LineEx == null ||
                   string.IsNullOrEmpty(line.LineEx.AliasLine))
                {
                    if (notExistsThrowError)
                    {
                        throw new FisException("IDL001", new List<string>());
                    }
                }
                return line;
            }

            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="pdLine"></param>
        /// <param name="notExistsThrowError"></param>
        /// <returns></returns>
        public Line CheckAndSetLine(Session session, string pdLine, bool notExistsThrowError)
        {
            Line line = (IMES.FisObject.Common.Line.Line)session.GetValue(Session.SessionKeys.LineInfo);
            if (line == null)
            {
                ActivityCommonImpl commonImp = ActivityCommonImpl.Instance;
                line = GetLine(pdLine, notExistsThrowError);
                session.AddValue(Session.SessionKeys.LineInfo, line);
            }
            return line;
        }
        #endregion

        #region Generate next number
        /// <summary>
        /// Get next sequnece number
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="typeName">Nunmber Control Type</param>
        /// <param name="prefixCode"></param>
        /// <param name="startNum"></param>
        /// <param name="endNum"></param>
        /// <param name="seqBase">10,16 Base</param>
        /// <returns></returns>
        public string GetNextSequence(string customer, string typeName, string prefixCode,
                                                        string startNum, string endNum, SequenceBaseEnum seqBase)
        {
            NumControl numCtrl = null;
            //INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
            string maxSeqStr = string.Empty;
            int digits = startNum.Length;
            if (digits != endNum.Length)
            {
                throw new Exception(string.Format(" Start Number:{0} End Number:{1} length is different!!", startNum, endNum));
            }

            try
            {
                SqlTransactionManager.Begin();
                lock (_syncRoot_GetSeq)
                {
                    bool insOrUpd = true;
                    string seq = string.Empty;
                    string maxNumStr = string.Empty;
                    //流水码的取得                   
                    numCtrl = numCtrlRepository.GetMaxValue(typeName, prefixCode);
                    //檢查有沒有lock index, 沒有lock index, 改變查詢條件
                    if (numCtrl == null)
                    {
                        NumControl data = numCtrlRepository.GetMaxValue(typeName, "Lock");
                        numCtrl = numCtrlRepository.GetMaxValue(typeName, prefixCode);
                    }
                    //if (string.IsNullOrEmpty(maxMo))
                    if (numCtrl == null)
                    {
                        seq = startNum;
                        insOrUpd = true;
                    }
                    else
                    {
                        string maxMo = numCtrl.Value;
                        seq = maxMo.Substring(maxMo.Length - digits, digits);
                        insOrUpd = false;
                    }
                    if (insOrUpd)
                    {
                        maxNumStr = seq;
                    }
                    else
                    {
                        if (seqBase == SequenceBaseEnum.Base10)
                        {
                            maxNumStr = GetNextAndCheck10Base(seq, startNum, endNum);
                        }
                        else if (seqBase == SequenceBaseEnum.Base36)
                        {
                            maxNumStr = GetNextAndCheck36Base(seq, startNum, endNum);
                        }
                        else
                        {
                            maxNumStr = GetNextAndCheck16Base(seq, startNum, endNum);
                        }
                    }
                    maxSeqStr = prefixCode + maxNumStr;

                    if (insOrUpd)
                    {
                        numCtrlRepository.InsertNumControl(new NumControl(0, typeName, prefixCode, maxSeqStr, customer));
                    }
                    else
                    {
                        numCtrl.Value = maxSeqStr;
                        numCtrlRepository.SaveMaxNumber(numCtrl, false);
                    }
                }
                SqlTransactionManager.Commit();
                return maxSeqStr;
            }
            catch (Exception e)
            {
                SqlTransactionManager.Rollback();
                throw e;
            }
            finally
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }
        }
        /// <summary>
        ///  Generate NextSequenceWithCharString
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="typeName"></param>
        /// <param name="prefixCode"></param>
        /// <param name="startNum"></param>
        /// <param name="endNum"></param>
        /// <param name="seqChar"></param>
        /// <returns></returns>
        public string GetNextSequenceWithCharString(string customer, string typeName, string prefixCode,
                                                       string startNum, string endNum, string seqChar)
        {
            NumControl numCtrl = null;
            //INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
            string maxSeqStr = string.Empty;
            int digits = startNum.Length;
            if (digits != endNum.Length)
            {
                throw new Exception(string.Format(" Start Number:{0} End Number:{1} length is different!!", startNum, endNum));
            }

            try
            {
                SqlTransactionManager.Begin();
                lock (_syncRoot_GetNum)
                {
                    bool insOrUpd = true;
                    string seq = string.Empty;
                    string maxNumStr = string.Empty;
                    //流水码的取得                   
                    numCtrl = numCtrlRepository.GetMaxValue(typeName, prefixCode, customer);
                    //檢查有沒有lock index, 沒有lock index, 改變查詢條件
                    if (numCtrl == null)
                    {
                        NumControl data = numCtrlRepository.GetMaxValue(typeName, "Lock");
                        numCtrl = numCtrlRepository.GetMaxValue(typeName, prefixCode, customer);
                    }
                    if (numCtrl == null)
                    {
                        maxSeqStr = startNum;
                        insOrUpd = true;
                    }
                    else
                    {
                        string maxMo = numCtrl.Value;
                        ISequenceConverter seqCvt = new SequenceConverterNormal(seqChar, digits, endNum, startNum, '0');
                        maxSeqStr = seqCvt.NumberRule.IncreaseToNumber(maxMo, 1);
                        insOrUpd = false;
                    }

                    if (insOrUpd)
                    {
                        numCtrlRepository.InsertNumControl(new NumControl(0, typeName, prefixCode, maxSeqStr, customer));
                    }
                    else
                    {
                        numCtrl.Value = maxSeqStr;
                        numCtrlRepository.SaveMaxNumber(numCtrl, false);
                    }
                }
                SqlTransactionManager.Commit();
                return maxSeqStr;
            }
            catch (Exception e)
            {
                SqlTransactionManager.Rollback();
                throw e;
            }
            finally
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }
        }

        private IList<string> GetNextSequenceWithCharString(string customer, string typeName, string prefixCode,
                                                     string startNum, string endNum, string seqChar, int qty)
        {
            IList<string> ret = new List<String>();
            NumControl numCtrl = null;
            //INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
            string maxSeqStr = string.Empty;
            int digits = startNum.Length;
            if (digits != endNum.Length)
            {
                throw new Exception(string.Format(" Start Number:{0} End Number:{1} length is different!!", startNum, endNum));
            }

            try
            {
                SqlTransactionManager.Begin();
                lock (_syncRoot_GetSeq)
                {
                    bool insOrUpd = true;
                    string seq = string.Empty;
                    string maxNumStr = string.Empty;

                    ISequenceConverter seqCvt = new SequenceConverterNormal(seqChar, digits, endNum, startNum, '0');

                    //流水码的取得                   
                    numCtrl = numCtrlRepository.GetMaxValue(typeName, prefixCode, customer);
                    //檢查有沒有lock index, 沒有lock index, 改變查詢條件
                    if (numCtrl == null)
                    {
                        NumControl data = numCtrlRepository.GetMaxValue(typeName, "Lock");
                        numCtrl = numCtrlRepository.GetMaxValue(typeName, prefixCode, customer);
                    }
                    // string maxMo = null; ;
                    if (numCtrl == null)
                    {
                        //maxMo = startNum;
                        maxSeqStr = startNum;
                        ret.Add(startNum);
                        insOrUpd = true;
                    }
                    else
                    {
                        //maxMo = numCtrl.Value;
                        maxSeqStr = seqCvt.NumberRule.IncreaseToNumber(numCtrl.Value, 1);
                        ret.Add(maxSeqStr);
                        //maxMo = maxSeqStr;
                        insOrUpd = false;
                    }

                    for (int i = 1; i < qty; i++)
                    {
                        maxSeqStr = seqCvt.NumberRule.IncreaseToNumber(maxSeqStr, 1);
                        ret.Add(maxSeqStr);
                    }

                    if (insOrUpd)
                    {
                        numCtrlRepository.InsertNumControl(new NumControl(0, typeName, prefixCode, maxSeqStr, customer));
                    }
                    else
                    {
                        numCtrl.Value = maxSeqStr;
                        numCtrlRepository.SaveMaxNumber(numCtrl, false);
                    }
                }
                SqlTransactionManager.Commit();
                return ret;
            }
            catch (Exception e)
            {
                SqlTransactionManager.Rollback();
                throw e;
            }
            finally
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="typeName"></param>
        /// <param name="prefixCode"></param>
        /// <param name="supplierList"></param>
        /// <param name="startNum"></param>
        /// <param name="endNum"></param>
        /// <param name="seqChar"></param>
        /// <returns></returns>
        public IList<string> GetNextCTSequenceWithCharString(string customer, string typeName, string prefixCode, IList<string> supplierList,
                                                                                 string startNum, string endNum, string seqChar)
        {
            IList<string> ret = new List<string>();
            NumControl numCtrl = null;
           // INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
            string maxSeqStr = string.Empty;
            int digits = startNum.Length;
            if (digits != endNum.Length)
            {
                throw new Exception(string.Format(" Start Number:{0} End Number:{1} length is different!!", startNum, endNum));
            }

            try
            {
                SqlTransactionManager.Begin();
                lock (_syncRoot_GetCT)
                {
                    bool insOrUpd = true;
                    string seq = string.Empty;
                    string maxNumStr = string.Empty;
                    //流水码的取得                   
                    numCtrl = numCtrlRepository.GetMaxValue(typeName, prefixCode);
                    //檢查有沒有lock index, 沒有lock index, 改變查詢條件
                    if (numCtrl == null)
                    {
                        NumControl data = numCtrlRepository.GetMaxValue(typeName, "Lock");
                        numCtrl = numCtrlRepository.GetMaxValue(typeName, prefixCode);
                    }

                    if (numCtrl == null)
                    {
                        maxSeqStr = supplierList[0] + "~" + startNum;
                        ret.Add(supplierList[0]);
                        ret.Add(startNum);
                        insOrUpd = true;
                    }
                    else
                    {
                        string maxMo = numCtrl.Value;
                        string[] seqNum = maxMo.Split(new char[] { '~' });
                        string supplier = null;
                        string maxNumber = null;
                        if (seqNum.Length < 2)
                        {
                            supplier = supplierList[0];
                            maxNumber = maxMo;
                        }
                        else
                        {
                            supplier = seqNum[0];
                            maxNumber = seqNum[1];
                        }

                        if (maxNumber == endNum)
                        {
                            //get next supplier Code
                            int index = supplierList.IndexOf(supplier);
                            if (index < 0 || index == (supplierList.Count - 1))
                            {
                                //throw over max number
                            }
                            else
                            {
                                supplier = supplierList[index+1];
                                maxNumber = startNum;
                            }
                        }
                        

                        ISequenceConverter seqCvt = new SequenceConverterNormal(seqChar, digits, endNum, startNum, '0');
                        maxNumber = seqCvt.NumberRule.IncreaseToNumber(maxNumber, 1);
                        maxSeqStr = supplier + "~" + maxNumber;
                        ret.Add(supplier);
                        ret.Add(maxNumber);
                        insOrUpd = false;
                    }

                    if (insOrUpd)
                    {
                        numCtrlRepository.InsertNumControl(new NumControl(0, typeName, prefixCode, maxSeqStr, customer));
                    }
                    else
                    {
                        numCtrl.Value = maxSeqStr;
                        numCtrlRepository.SaveMaxNumber(numCtrl, false);
                    }
                }
                SqlTransactionManager.Commit();
                return ret;
            }
            catch (Exception e)
            {
                SqlTransactionManager.Rollback();
                throw e;
            }
            finally
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }
        }


        private string GetNextAndCheck16Base(string currentNum, string minNum, string maxNum)
        {
            int digit = minNum.Length;
            string digitFormat = "X" + digit.ToString();
            long curNum = Convert.ToInt64(currentNum, 16);
            long beginNum = Convert.ToInt64(minNum, 16);
            long endNum = Convert.ToInt64(maxNum, 16);
            if (curNum >= beginNum && curNum <= endNum)
            {
                long nextSeqNum = curNum + 1;
                return nextSeqNum.ToString(digitFormat);
            }
            else
            {
                return minNum;
            }
        }

        private string GetNextAndCheck10Base(string currentNum, string minNum, string maxNum)
        {
            int digit = minNum.Length;
            string digitFormat = "D" + digit.ToString();
            long curNum = long.Parse(currentNum);
            long beginNum = long.Parse(minNum);
            long endNum = long.Parse(maxNum);
            if (curNum >= beginNum && curNum <= endNum)
            {
                long nextSeqNum = curNum + 1;
                return nextSeqNum.ToString(digitFormat);
            }
            else
            {
                return minNum;
            }
        }

        private string GetNextAndCheck36Base(string currentNum, string minNum, string maxNum)
        {

            long curNum = Base36.Decode(currentNum);
            long beginNum = Base36.Decode(minNum);
            long endNum = Base36.Decode(maxNum);
            if (curNum >= beginNum && curNum <= endNum)
            {
                long nextSeqNum = curNum + 1;
                return Base36.Encode(nextSeqNum).ToUpper();
            }
            else
            {
                return minNum;
            }
        }

        private static class Base36
        {
            private const string CharList = "0123456789abcdefghijklmnopqrstuvwxyz";

            /// <summary>
            /// Encode the given number into a Base36 string
            /// </summary>
            /// <param name="input"></param>
            /// <returns></returns>
            public static String Encode(long input)
            {
                if (input < 0) throw new ArgumentOutOfRangeException("input", input, "input cannot be negative");

                char[] clistarr = CharList.ToCharArray();
                var result = new Stack<char>();
                while (input != 0)
                {
                    result.Push(clistarr[input % 36]);
                    input /= 36;
                }
                return new string(result.ToArray());
            }

            /// <summary>
            /// Decode the Base36 Encoded string into a number
            /// </summary>
            /// <param name="input"></param>
            /// <returns></returns>
            public static Int64 Decode(string input)
            {
                var reversed = input.ToLower().Reverse();
                long result = 0;
                int pos = 0;
                foreach (char c in reversed)
                {
                    result += CharList.IndexOf(c) * (long)Math.Pow(36, pos);
                    pos++;
                }
                return result;
            }
        }
        /// <summary>
        /// GetDateFormat
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public string GetDateFormat(DateTime date, string format)
        {
            string dateStr = date.ToString("yyyyMMdd");
            string ret = "";
            switch (format.ToUpper())
            {
                case "Y":
                    ret = dateStr.Substring(3, 1);
                    break;
                case "YY":
                    ret = dateStr.Substring(2, 2);
                    break;
                case "YYY":
                    ret = dateStr.Substring(1, 3);
                    break;
                case "YYYY":
                    ret = dateStr.Substring(0, 4);
                    break;
                case "MM":
                    ret = dateStr.Substring(4, 2);
                    break;
                case "M":
                    int month = int.Parse(dateStr.Substring(4, 2));
                    if (month < 10)
                    {
                        ret = month.ToString();
                    }
                    else
                    {
                        ret = ((char)((month - 10) + 65)).ToString();
                    }
                    break;
                case "YYYYMM":
                    ret = dateStr.Substring(0, 6);
                    break;
                case "YYMM":
                    ret = dateStr.Substring(2, 4);
                    break;
                case "YMM":
                    ret = dateStr.Substring(3, 3);
                    break;
                case "YM":
                    ret = dateStr.Substring(3, 1);
                    month = int.Parse(dateStr.Substring(4, 2));
                    if (month < 10)
                    {
                        ret = ret + month.ToString();
                    }
                    else
                    {
                        ret = ret + ((char)((month - 10) + 65)).ToString();
                    }
                    break;
                case "YW0":
                case "YW1":
                case "YW2":
                case "YW3":
                case "YW4":
                case "YW5":
                case "YW6":
                    int dw = int.Parse(format.Substring(2, 1));
                    string year = "";
                    string wk = GetWeekNo(date, (DayOfWeek)dw, out year);
                    ret = year.Substring(3, 1) + wk;
                    break;
                case "YYW0":
                case "YYW1":
                case "YYW2":
                case "YYW3":
                case "YYW4":
                case "YYW5":
                case "YYW6":
                    dw = int.Parse(format.Substring(2, 1));
                    year = "";
                    wk = GetWeekNo(date, (DayOfWeek)dw, out year);
                    ret = year.Substring(2, 2) + wk;
                    break;
                case "YYWW":
                    year = "";
                    wk = GetISO8601WeekNo(date, out year);
                    ret = year.Substring(2, 2) + wk;
                    break;
                case "YWW":
                    year = "";
                    wk = GetISO8601WeekNo(date, out year);
                    ret = year.Substring(3, 1) + wk;
                    break;

                case "DD":
                    ret = date.ToString("dd");
                    break;
                case "W0":
                case "W1":
                case "W2":
                case "W3":
                case "W4":
                case "W5":
                case "W6":
                    dw = int.Parse(format.Substring(1, 1));
                    wk = GetWeekNo(date, (DayOfWeek)dw, out year);
                    ret = wk;
                    break;
                case "WW":
                    wk = GetISO8601WeekNo(date, out year);
                    ret = wk;
                    break;
                default:
                    ret = getSpecialDate(date, format);
                    break;

            }
            return ret;
        }
        /// <summary>
        /// Date@Y,StartYear,編號1234567890ABCDFG
        /// Date@M,編號1234567890ABCDFG
        /// Date@D,編號1234567890ABCDFG
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        private string getSpecialDate(DateTime date, string format)
        {
            string[] name = format.Split(',');
            if (name.Length > 1)
            {
                if (name[0] == "Y" && name.Length > 2)
                {
                    int year = date.Year;
                    int startYear = int.Parse(name[1]);
                    int diffYear = year - startYear;
                    if (diffYear < 0)
                    {
                        throw new Exception("Date@" + format + " start year is less than this year");
                    }
                    return name[2].Substring(diffYear, 1);
                }
                else if (name[0] == "M" && name.Length > 1)
                {
                    int month = date.Month - 1;
                    return name[1].Substring(month, 1);
                }
                else if (name[0] == "D" && name.Length > 1)
                {
                    int day = date.Day - 1;
                    return name[1].Substring(day, 1);
                }
                else
                {
                    throw new Exception("Not support Date@" + format + " format");
                }
            }
            else
            {
                throw new Exception("Not support Date@" + format + " format");
            }
        }

        /// <summary>
        /// GetWeekNo
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dw"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public string GetWeekNo(DateTime date, DayOfWeek dw, out string year)
        {
            CultureInfo cul = CultureInfo.CurrentCulture;
            int firstDayWeek = cul.Calendar.GetWeekOfYear(date,
                      CalendarWeekRule.FirstDay,
                     dw);
            year = (firstDayWeek > 51 && date.Month == 1 ? date.Year - 1 : date.Year).ToString("D4");
            return firstDayWeek.ToString("D2");
        }

        /// <summary>
        /// ISO8601
        /// </summary>
        /// <param name="date"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public string GetISO8601WeekNo(DateTime date, out string year)
        {
            Calendar cal = CultureInfo.InvariantCulture.Calendar;
            DayOfWeek day = cal.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                date = date.AddDays(3);
            }

            int intYear = date.Year;

            int weekDay = cal.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            if ((weekDay >= 52) && (date.Month < 12)) { intYear--; }

            year = intYear.ToString("D4");
            return weekDay.ToString("D2");
        }

       /// <summary>
        /// GetSequencePreFix
       /// </summary>
       /// <param name="snFormat"></param>
       /// <param name="product"></param>
       /// <param name="seqSpec"></param>
       /// <param name="preFixList"></param>
       /// <returns></returns>
        public IList<string> GetSequencePreFix(string snFormat, IProduct product, out string seqSpec, out IList<string> preFixList)
        {
            seqSpec = null;
            IList<string> ret = new List<string>();
            preFixList = new List<string>();
            string[] sections = snFormat.Split(new char[] { '~' });
            DateTime now = DateTime.Now;
            foreach (string name in sections)
            {
                int index = name.IndexOf("@");
                if (index < 0)
                {
                    //Fixed mantis 0002837: Lenovo Poker product SN 与OBI SN 编码规则需求 需要有空白
                    ret.Add(name);
                }
                else
                {
                    string objName = name.Substring(0, index).ToUpper();
                    string objMethod = name.Substring(index + 1).Trim();
                    string value = null;
                    bool hasPrefix = false;
                    if (objName.StartsWith("PREFIX:"))
                    {
                        objName = objName.Substring(7);
                        hasPrefix = true;
                    }

                    if (objName == "MODELINFO")
                    {
                        value = getSeqFormatWithModelInfo(product, objMethod, name);
                    }
                    else if (objName == "FAMILYINFO")
                    {
                        value = getSeqFormatWithFamilyInfo(product, objMethod, name);
                    }
                    else if (objName == "DATE")
                    {
                        value = GetDateFormat(now, objMethod);
                    }
                    else if (objName == "LINE" && (objMethod.ToUpper() == "ALIAS" || objMethod.ToUpper() == "ALIASLINE"))
                    {
                        Line line = GetLine(product.Status.Line, true);
                        value = line.LineEx.AliasLine;

                    }
                    else if (objName == "SYS")
                    {
                        value = GetSysSetting(objMethod, null);
                    }
                    else if (objName == "NUMBER")
                    {
                        //ret.Add(name);
                        value = name;
                        if (string.IsNullOrEmpty(seqSpec))
                        {
                            seqSpec = objMethod;
                        }
                    }
                    else
                    {
                        throw new Exception("Generate SN format is wrong " + name);
                    }

                    ret.Add(value);
                    if (hasPrefix)
                    {
                        preFixList.Add(value);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// By MB
        /// </summary>
        /// <param name="snFormat"></param>
        /// <param name="mb"></param>
        /// <param name="seqSpec"></param>
        /// <param name="preFixList"></param>
        /// <returns></returns>
        public IList<string> GetSequencePreFix(string snFormat, IMB mb, out string seqSpec, out IList<string> preFixList)
        {
            seqSpec = null;
            IList<string> ret = new List<string>();
            preFixList = new List<string>();
            string[] sections = snFormat.Split(new char[] { '~' });
            DateTime now = DateTime.Now;
            foreach (string name in sections)
            {
                int index = name.IndexOf("@");
                if (index < 0)
                {
                    //Fixed mantis 0002837: Lenovo Poker product SN 与OBI SN 编码规则需求 需要有空白
                    ret.Add(name);
                }
                else
                {
                    string objName = name.Substring(0, index).ToUpper();
                    string objMethod = name.Substring(index + 1).Trim();
                    string value = null;
                    bool hasPrefix = false;
                    if (objName.StartsWith("PREFIX:"))
                    {
                        objName = objName.Substring(7);
                        hasPrefix = true;
                    }

                    if (objName == "MODELINFO" || objName == "PARTINFO")
                    {
                        value = getSeqFormatWithModelInfo(mb, objMethod, name);
                    }
                    else if (objName == "FAMILYINFO")
                    {
                        value = getSeqFormatWithFamilyInfo(mb, objMethod, name);
                    }
                    else if (objName == "DATE")
                    {
                        value = GetDateFormat(now, objMethod);
                    }
                    else if (objName == "LINE" && (objMethod.ToUpper() == "ALIAS" || objMethod.ToUpper() == "ALIASLINE"))
                    {
                        Line line = GetLine(mb.MBStatus.Line, true);
                        value = line.LineEx.AliasLine;
                    }
                    else if (objName == "SYS")
                    {
                        value = GetSysSetting(objMethod, null);
                    }
                    else if (objName == "NUMBER")
                    {
                        //ret.Add(name);
                        value = name;
                        if (string.IsNullOrEmpty(seqSpec))
                        {
                            seqSpec = objMethod;
                        }
                    }
                    else
                    {
                        throw new Exception("Generate SN format is wrong " + name);
                    }

                    ret.Add(value);
                    if (hasPrefix)
                    {
                        preFixList.Add(value);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="snFormat"></param>
        /// <param name="part"></param>
        /// <param name="seqSpec"></param>
        /// <param name="preFixList"></param>
        /// <returns></returns>
        public IList<string> GetSequencePreFix(string snFormat, IPart part, out string seqSpec, out IList<string> preFixList)
        {
            seqSpec = null;
            IList<string> ret = new List<string>();
            preFixList = new List<string>();
            string[] sections = snFormat.Split(new char[] { '~' });
            DateTime now = DateTime.Now;
            foreach (string name in sections)
            {
                int index = name.IndexOf("@");
                if (index < 0)
                {
                    //Fixed mantis 0002837: Lenovo Poker product SN 与OBI SN 编码规则需求 需要有空白
                    ret.Add(name);
                }
                else
                {
                    string objName = name.Substring(0, index).ToUpper();
                    string objMethod = name.Substring(index + 1).Trim();
                    string value = null;
                    bool hasPrefix = false;
                    if (objName.StartsWith("PREFIX:"))
                    {
                        objName = objName.Substring(7);
                        hasPrefix = true;
                    }

                    if (objName == "MODELINFO")
                    {
                        value = getSeqFormatWithModelInfo(part, objMethod, name);
                    }
                    if (objName == "PARTINFO")
                    {
                        value = getSeqFormatWithModelInfo(part, objMethod, name);
                    }
                    else if (objName == "DATE")
                    {
                        value = GetDateFormat(now, objMethod);
                    }
                    else if (objName == "SYS")
                    {
                        value = GetSysSetting(objMethod, null);
                    }
                    else if (objName == "NUMBER")
                    {
                        //ret.Add(name);
                        value = name;
                        if (string.IsNullOrEmpty(seqSpec))
                        {
                            seqSpec = objMethod;
                        }
                    }
                    else
                    {
                        throw new Exception("Generate SN format is wrong " + name);
                    }

                    ret.Add(value);
                    if (hasPrefix)
                    {
                        preFixList.Add(value);
                    }
                }
            }

            return ret;
        }
        

        private string getSeqFormatWithModelInfo(IProduct product, string name, string sectionName)
        {
            string value = product.ModelObj.GetAttribute(name);
            IList<ConstValueInfo> sectionValueList = null;
            ConstValueInfo sectionValue = null;
            if (string.IsNullOrEmpty(value))
            {
                throw new FisException("PAK085", new List<string> { product.Model, name });
            }

            if (TryConstValue(sectionName, value, out sectionValueList, out sectionValue))
            {
                return sectionValue.value;
            }
            else if (sectionValueList == null || sectionValueList.Count == 0)
            {
                return value;
            }
            else
            {
                throw new FisException("CQCHK0026", new List<string> { sectionName + "." + name });
            }
        }

        /// <summary>
        /// by MB
        /// </summary>
        /// <param name="mb"></param>
        /// <param name="name"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        private string getSeqFormatWithModelInfo(IMB mb, string name, string sectionName)
        {
            IPart part = mb.ModelObj.PartObj;

            if (part == null)
            {
                throw new FisException("CHK1077", new string[] { mb.Model });
            }
            string value = part.GetAttribute(name);
            IList<ConstValueInfo> sectionValueList = null;
            ConstValueInfo sectionValue = null;
            if (string.IsNullOrEmpty(value))
            {
                throw new FisException("CHK1235", new List<string> { mb.Model, name });
            }

            if (TryConstValue(sectionName, value, out sectionValueList, out sectionValue))
            {
                return sectionValue.value;
            }

            else //if (sectionValueList == null || sectionValueList.Count == 0)
            {
                return value.Replace(".", string.Empty); //Acer PartNo  remove  '.' character
            }
            //else
            //{
            //    throw new FisException("CQCHK0026", new List<string> { sectionName + "." + name });
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="part"></param>
        /// <param name="name"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        private string getSeqFormatWithModelInfo(IPart part, string name, string sectionName)
        {

            if (part == null)
            {
                throw new FisException("CHK1077", new string[] { "" });
            }
            string value = part.GetAttribute(name);
            IList<ConstValueInfo> sectionValueList = null;
            ConstValueInfo sectionValue = null;
            if (string.IsNullOrEmpty(value))
            {
                throw new FisException("CHK1235", new List<string> { part.PN, name });
            }

            if (TryConstValue(sectionName, value, out sectionValueList, out sectionValue))
            {
                return sectionValue.value;
            }

            else //if (sectionValueList == null || sectionValueList.Count == 0)
            {
                return value.Replace(".", string.Empty);  //Acer PartNo  remove  '.' character
            }
            //else
            //{
            //    throw new FisException("CQCHK0026", new List<string> { sectionName + "." + name });
            //}
        }


        private string getSeqFormatWithFamilyInfo(IProduct product, string name, string sectionName)
        {
            string family = product.Family;
            //IFamilyRepository familyRep = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
            IList<FamilyInfoDef> familyInfoList = familyRep.GetExistFamilyInfo(new FamilyInfoDef { family = family, name = name });

            if (familyInfoList == null || familyInfoList.Count == 0)
            {
                throw new FisException("CHK1036", new List<string> { family, name });
            }

            string value = familyInfoList[0].value;
            IList<ConstValueInfo> sectionValueList = null;
            ConstValueInfo sectionValue = null;

            if (TryConstValue(sectionName, value, out sectionValueList, out sectionValue))
            {
                return sectionValue.value;
            }
            else if (sectionValueList == null || sectionValueList.Count == 0)
            {
                return value;
            }
            else
            {
                throw new FisException("CQCHK0026", new List<string> { sectionName + "." + name });
            }
        }

        private string getSeqFormatWithFamilyInfo(IMB mb, string name, string sectionName)
        {
            string family = mb.Family;
            //IFamilyRepository familyRep = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
            IList<FamilyInfoDef> familyInfoList = familyRep.GetExistFamilyInfo(new FamilyInfoDef { family = family, name = name });

            if (familyInfoList == null || familyInfoList.Count == 0)
            {
                throw new FisException("CHK1036", new List<string> { family, name });
            }

            string value = familyInfoList[0].value;
            IList<ConstValueInfo> sectionValueList = null;
            ConstValueInfo sectionValue = null;

            if (TryConstValue(sectionName, value, out sectionValueList, out sectionValue))
            {
                return sectionValue.value;
            }
            else //if (sectionValueList == null || sectionValueList.Count == 0)
            {
                return value;
            }
            //else
            //{
            //    throw new FisException("CQCHK0026", new List<string> { sectionName + "." + name });
            //}
        }       

        /// <summary>
        /// GetNextSequence
        /// </summary>
        /// <param name="product"></param>
        /// <param name="customer"></param>
        /// <param name="numType"></param>
        /// <param name="numSpecFormat"></param>
        /// <returns></returns>
        public string GetNextSequence(IProduct product,
                                                        string customer,
                                                       string numType,
                                                       string numSpecFormat)
        {
            //string seqFormat = null;
            //IList<ConstValueInfo> valueList = ConstValue(constValueofType, constValueofName, out seqFormat);
            string seqSpec = null;
            IList<string> prefixList = null;
            IList<string> seqNumList = GetSequencePreFix(numSpecFormat, product, out seqSpec, out prefixList);

            if (string.IsNullOrEmpty(customer))
            {
                throw new Exception("customer is empty or null, please check Family setup!");
            }

            if (string.IsNullOrEmpty(seqSpec))
            {
                throw new FisException("CQCHK50095", new List<string> { numSpecFormat });
            }
            string[] seqSpecList = seqSpec.Split(new char[] { ',', ';', ':' });
            if (seqSpecList.Length < 3)
            {
                throw new FisException("CQCHK50095", new List<string> { numSpecFormat });
            }

            string prefix = "";

            if (prefixList == null || prefixList.Count == 0)  //未設置Prefix setting
            {
                prefixList = seqNumList;
            }

            foreach (string name in prefixList)
            {
                if (!name.ToUpper().StartsWith("NUMBER@"))
                {
                    prefix = prefix + name;
                }
            }
            string nextSequence = GetNextSequenceWithCharString(customer,
                                                                                                    numType,
                                                                                                    prefix,
                                                                                                    seqSpecList[1].Trim(),
                                                                                                    seqSpecList[2].Trim(),
                                                                                                    seqSpecList[0].Trim());
            string nextNum = "";
            foreach (string name in seqNumList)
            {
                if (!name.ToUpper().StartsWith("NUMBER@"))
                {
                    nextNum = nextNum + name;
                }
                else
                {
                    nextNum = nextNum + nextSequence;
                }
            }

            return nextNum;
        }

        /// <summary>
        /// by MB
        /// </summary>
        /// <param name="mb"></param>
        /// <param name="customer"></param>
        /// <param name="numType"></param>
        /// <param name="numSpecFormat"></param>
        /// <returns></returns>
        public string GetNextSequence(IMB mb,
                                                        string customer,
                                                       string numType,
                                                       string numSpecFormat)
        {
            //string seqFormat = null;
            //IList<ConstValueInfo> valueList = ConstValue(constValueofType, constValueofName, out seqFormat);
            string seqSpec = null;
            IList<string> prefixList = null;

            if (string.IsNullOrEmpty(customer))
            {
                throw new Exception("customer is empty or null, please check Family setup!");
            }

            IList<string> seqNumList = GetSequencePreFix(numSpecFormat, mb, out seqSpec, out prefixList);

            if (string.IsNullOrEmpty(seqSpec))
            {
                throw new FisException("CQCHK50095", new List<string> { numSpecFormat });
            }
            string[] seqSpecList = seqSpec.Split(new char[] { ',', ';', ':' });
            if (seqSpecList.Length < 3)
            {
                throw new FisException("CQCHK50095", new List<string> { numSpecFormat });
            }

            string prefix = "";

            if (prefixList == null || prefixList.Count == 0)  //未設置Prefix setting
            {
                prefixList = seqNumList;
            }

            foreach (string name in prefixList)
            {
                if (!name.ToUpper().StartsWith("NUMBER@"))
                {
                    prefix = prefix + name;
                }
            }
            string nextSequence = GetNextSequenceWithCharString(customer,
                                                                                                    numType,
                                                                                                    prefix,
                                                                                                    seqSpecList[1].Trim(),
                                                                                                    seqSpecList[2].Trim(),
                                                                                                    seqSpecList[0].Trim());
            string nextNum = "";
            foreach (string name in seqNumList)
            {
                if (!name.ToUpper().StartsWith("NUMBER@"))
                {
                    nextNum = nextNum + name;
                }
                else
                {
                    nextNum = nextNum + nextSequence;
                }
            }

            return nextNum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mb"></param>
        /// <param name="customer"></param>
        /// <param name="numType"></param>
        /// <param name="numSpecFormat"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public IList<string> GetNextSequence(IMB mb,
                                                        string customer,
                                                       string numType,
                                                       string numSpecFormat,
                                                        int qty)
        {
            //string seqFormat = null;
            //IList<ConstValueInfo> valueList = ConstValue(constValueofType, constValueofName, out seqFormat);
            string seqSpec = null;
            IList<string> prefixList = null;
            IList<string> seqNumList = GetSequencePreFix(numSpecFormat, mb, out seqSpec, out prefixList);

            if (string.IsNullOrEmpty(customer))
            {
                throw new Exception("customer is empty or null, please check Family setup!");
            }

            if (string.IsNullOrEmpty(seqSpec))
            {
                throw new FisException("CQCHK50095", new List<string> { numSpecFormat });
            }
            string[] seqSpecList = seqSpec.Split(new char[] { ',', ';', ':' });
            if (seqSpecList.Length < 3)
            {
                throw new FisException("CQCHK50095", new List<string> { numSpecFormat });
            }

            string prefix = "";

            if (prefixList == null || prefixList.Count == 0)  //未設置Prefix setting
            {
                prefixList = seqNumList;
            }

            foreach (string name in prefixList)
            {
                if (!name.ToUpper().StartsWith("NUMBER@"))
                {
                    prefix = prefix + name;
                }
            }
            IList<string> nextSequenceList = GetNextSequenceWithCharString(customer,
                                                                                                    numType,
                                                                                                    prefix,
                                                                                                    seqSpecList[1].Trim(),
                                                                                                    seqSpecList[2].Trim(),
                                                                                                    seqSpecList[0].Trim(),
                                                                                                    qty);
            IList<string> nextNumList = new List<string>();
            foreach (string nextSequence in nextSequenceList)
            {
                string nextNum = string.Empty;
                foreach (string name in seqNumList)
                {
                    if (!name.ToUpper().StartsWith("NUMBER@"))
                    {
                        nextNum = nextNum + name;
                    }
                    else
                    {
                        nextNum = nextNum + nextSequence;
                    }
                }
                nextNumList.Add(nextNum);
            }

            return nextNumList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="part"></param>
        /// <param name="customer"></param>
        /// <param name="numType"></param>
        /// <param name="numSpecFormat"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public IList<string> GetNextSequence(IPart part,
                                                       string customer,
                                                      string numType,
                                                      string numSpecFormat,
                                                      int qty)
        {
            //string seqFormat = null;
            //IList<ConstValueInfo> valueList = ConstValue(constValueofType, constValueofName, out seqFormat);
            string seqSpec = null;
            IList<string> prefixList = null;
            IList<string> seqNumList = GetSequencePreFix(numSpecFormat, part, out seqSpec, out prefixList);

            if (string.IsNullOrEmpty(customer))
            {
                throw new Exception("customer is empty or null, please check Family setup!");
            }

            if (string.IsNullOrEmpty(seqSpec))
            {
                throw new FisException("CQCHK50095", new List<string> { numSpecFormat });
            }
            string[] seqSpecList = seqSpec.Split(new char[] { ',', ';', ':' });
            if (seqSpecList.Length < 3)
            {
                throw new FisException("CQCHK50095", new List<string> { numSpecFormat });
            }

            string prefix = "";

            if (prefixList == null || prefixList.Count == 0)  //未設置Prefix setting
            {
                prefixList = seqNumList;
            }

            foreach (string name in prefixList)
            {
                if (!name.ToUpper().StartsWith("NUMBER@"))
                {
                    prefix = prefix + name;
                }
            }

            IList<string> nextSequenceList = GetNextSequenceWithCharString(customer,
                                                                                                    numType,
                                                                                                    prefix,
                                                                                                    seqSpecList[1].Trim(),
                                                                                                    seqSpecList[2].Trim(),
                                                                                                    seqSpecList[0].Trim(),
                                                                                                    qty);
            IList<string> nextNumList = new List<string>();
            foreach (string nextSequence in nextSequenceList)
            {
                string nextNum = string.Empty;
                foreach (string name in seqNumList)
                {
                    if (!name.ToUpper().StartsWith("NUMBER@"))
                    {
                        nextNum = nextNum + name;
                    }
                    else
                    {
                        nextNum = nextNum + nextSequence;
                    }
                }
                nextNumList.Add(nextNum);
            }
            return nextNumList;
        }
        #endregion

        #region Check Asset Tag
        /// <summary>
        /// HasShippingAstTag
        /// </summary>
        /// <param name="astDefineInfoList"></param>
        /// <returns></returns>
        public bool HasShippingAstTag(IList<AstDefineInfo> astDefineInfoList)
        {
            return astDefineInfoList.Any(x => x.AstLocation.ToLower() == ShippingAssetTag);
        }
        /// <summary>
        /// IsShippingAstTag
        /// </summary>
        /// <param name="astDefineInfo"></param>
        /// <returns></returns>
        public bool IsShippingAstTag(AstDefineInfo astDefineInfo)
        {
            return astDefineInfo.AstLocation.ToLower() == ShippingAssetTag;
        }
        /// <summary>
        /// AllShippingAstTag
        /// </summary>
        /// <param name="astDefineInfoList"></param>
        /// <returns></returns>
        public bool AllShippingAstTag(IList<AstDefineInfo> astDefineInfoList)
        {
            return astDefineInfoList.All(x => x.AstLocation.ToLower() == ShippingAssetTag);
        }
        /// <summary>
        /// Get Ast Shipping Location
        /// </summary>
        public string AstShippingLocation
        {
            get
            {
                return ShippingAssetTag;
            }
        }
        #endregion

        #region  add  by  hjy for Assign MAC and Sequence number
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="astType"></param>
        /// <param name="astNo"></param>
        /// <returns></returns>
        public IList<CombinedAstNumberInfo> GetCombinedAstCode(string productId, string astType, string astNo)
        {
            //IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
            return productRepository.GetCombinedAstNumber(new CombinedAstNumberInfo
            {
                ProductID = productId,
                AstType = astType,
                AstNo = astNo,
                State = "Used"
            });
        }
        /// <summary>
        /// Combined Number
        /// </summary>
        /// <param name="session"></param>
        /// <param name="productId"></param>
        /// <param name="code"></param>
        /// <param name="astType"></param>
        /// <param name="astNo"></param>
        /// <param name="station"></param>
        /// <param name="editor"></param>
        public void InsertCombinedAstNumber(Session session, string productId, string code, string astType, string astNo, string station, string editor)
        {
            //IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
            productRepository.InsertCombinedAstNumberDefered(session.UnitOfWork, new CombinedAstNumberInfo
            {
                Code = code,
                AstNo = astNo,
                AstType = astType,
                ProductID = productId,
                Station = station,
                UnBindProductID = "",
                UnBindStation = "",
                State = "Used",
                Editor = editor,
                Remark = "",
                Cdt = DateTime.Now,
                Udt = DateTime.Now
            });
        }
        /// <summary>
        /// Release combined number
        /// </summary>
        /// <param name="session"></param>
        /// <param name="productId"></param>
        /// <param name="astType"></param>
        /// <param name="astNo"></param>
        /// <param name="station"></param>
        /// <param name="editor"></param>
        public void ReleaseCombinedAstNumber(Session session, string productId, string astType, string astNo, string station, string editor)
        {
            //IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
            IList<CombinedAstNumberInfo> combinedList = productRepository.GetCombinedAstNumber(new CombinedAstNumberInfo { AstNo = astNo, ProductID = productId, AstType = astType });
            if (combinedList != null && combinedList.Count > 0)
            {
                CombinedAstNumberInfo combined = combinedList[0];
                combined.UnBindProductID = combined.ProductID;
                combined.UnBindStation = station;
                combined.ProductID = "";
                combined.State = "Release";
                combined.Station = "";
                combined.Editor = editor;
                combined.Udt = DateTime.Now;
                productRepository.UpdateCombinedAstNumberDefered(session.UnitOfWork, combined);

            }
        }
        //private static object _syncRoot_GetSeq = new object();
        /// <summary>
        /// Get MAC Base on Rule
        /// </summary>
        /// <param name="cust"></param>
        /// <param name="oldRule"></param>
        /// <returns></returns>
        public string GetMAC(string cust, bool oldRule)
        {
            try
            {
                SqlTransactionManager.Begin();
                lock (_sync_GetMAC)
                {
                   // INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

                    MACRange currentRange = numCtrlRepository.GetMACRange(cust, new string[] { "R", "A" });
                    if (currentRange == null)
                    {
                        throw new FisException("ICT014", new string[] { });
                    }
                    else
                    {
                        NumControl currentMaxNum = numCtrlRepository.GetMaxValue("MAC", cust);
                        if (currentMaxNum == null)
                        {
                            currentMaxNum = new NumControl();
                            currentMaxNum.NOName = cust;
                            currentMaxNum.NOType = "MAC";
                            currentMaxNum.Value = currentRange.BegNo;
                            currentMaxNum.Customer = "";

                            IUnitOfWork uof = new UnitOfWork();
                            numCtrlRepository.InsertNumControlDefered(uof, currentMaxNum);
                            if (currentMaxNum.Value == currentRange.EndNo)
                            {
                                numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Closed);
                            }
                            else
                            {
                                numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Active);
                            }
                            uof.Commit();
                            SqlTransactionManager.Commit();
                            return currentMaxNum.Value;

                        }
                        else
                        {
                            if (currentMaxNum.Value == currentRange.EndNo)
                            {
                                numCtrlRepository.SetMACRangeStatus(currentRange.ID, MACRange.MACRangeStatus.Closed);
                                currentRange = numCtrlRepository.GetMACRange(cust, new string[] { "R" });
                                if (currentMaxNum.Value == currentRange.BegNo || currentMaxNum.Value == currentRange.EndNo)
                                {
                                    throw new FisException("ICT018", new string[] { currentMaxNum.Value });
                                }
                            }
                            //0002589: 平板MAC 分配逻辑改善 & 
                            //   0002594: 平板在Assign Model站BTMAC和WIFIMAC分配逻辑改善
                            if (oldRule)
                            {
                                currentMaxNum.Value = GetNextAndCheckChange34Base(currentMaxNum.Value, currentRange.BegNo, currentRange.EndNo);
                            }
                            else
                            {
                                currentMaxNum.Value = GetNextAndCheck16Base(currentMaxNum.Value, currentRange.BegNo, currentRange.EndNo, true);
                            }

                            IUnitOfWork uof = new UnitOfWork();
                            numCtrlRepository.Update(currentMaxNum, uof);
                            if (currentMaxNum.Value == currentRange.EndNo)
                            {
                                numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Closed);
                            }
                            else
                            {
                                numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Active);
                            }
                            uof.Commit();
                            SqlTransactionManager.Commit();
                            return currentMaxNum.Value;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SqlTransactionManager.Rollback();
                throw e;
            }
            finally
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cust"></param>
        /// <param name="oldRule"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public IList<string> GetMAC(string cust, bool oldRule, int qty)
        {
            try
            {

                IList<string> ret = new List<string>();
                if (qty == 1)
                {
                    ret.Add(GetMAC(cust, oldRule));
                    return ret;
                }

                SqlTransactionManager.Begin();
                lock (_sync_GetMAC)
                {
                    //INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

                    MACRange currentRange = numCtrlRepository.GetMACRange(cust, new string[] { "R", "A" });
                    if (currentRange == null)
                    {
                        throw new FisException("ICT014", new string[] { });
                    }
                    else
                    {
                        NumControl currentMaxNum = numCtrlRepository.GetMaxValue("MAC", cust);
                        if (currentMaxNum == null)
                        {
                            currentMaxNum = new NumControl();
                            currentMaxNum.NOName = cust;
                            currentMaxNum.NOType = "MAC";
                            currentMaxNum.Value = currentRange.BegNo;
                            currentMaxNum.Customer = "";

                            ret.Add(currentRange.BegNo);
                            qty--;

                            IUnitOfWork uof = new UnitOfWork();
                            if (qty > 0 && currentMaxNum.Value == currentRange.EndNo)
                            {
                                numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Closed);
                                currentRange = numCtrlRepository.GetMACRange(cust, new string[] { "R", "A" });
                                if (currentRange == null)
                                {
                                    throw new FisException("ICT014", new string[] { });
                                }
                            }

                            int remaingCount = qty;
                            for (int i = 0; i < qty; i++)
                            {
                                remaingCount--;
                                if (oldRule)
                                {
                                    currentMaxNum.Value = GetNextAndCheckChange34Base(currentMaxNum.Value, currentRange.BegNo, currentRange.EndNo);
                                }
                                else
                                {
                                    currentMaxNum.Value = GetNextAndCheck16Base(currentMaxNum.Value, currentRange.BegNo, currentRange.EndNo, false);
                                }

                                ret.Add(currentMaxNum.Value);

                                if (remaingCount > 0 && currentMaxNum.Value == currentRange.EndNo)
                                {
                                    numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Closed);
                                    currentRange = numCtrlRepository.GetMACRange(cust, new string[] { "R", "A" });
                                    if (currentRange == null)
                                    {
                                        throw new FisException("ICT014", new string[] { });
                                    }
                                }
                            }

                            numCtrlRepository.InsertNumControlDefered(uof, currentMaxNum);
                            if (currentMaxNum.Value == currentRange.EndNo)
                            {
                                numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Closed);
                            }
                            else
                            {
                                numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Active);
                            }
                            uof.Commit();
                            SqlTransactionManager.Commit();
                            return ret;

                        }
                        else
                        {
                            if (currentMaxNum.Value == currentRange.EndNo)
                            {
                                numCtrlRepository.SetMACRangeStatus(currentRange.ID, MACRange.MACRangeStatus.Closed);
                                currentRange = numCtrlRepository.GetMACRange(cust, new string[] { "R", "A" });
                                if (currentMaxNum.Value == currentRange.BegNo || currentMaxNum.Value == currentRange.EndNo)
                                {
                                    throw new FisException("ICT018", new string[] { currentMaxNum.Value });
                                }
                            }
                            //0002589: 平板MAC 分配逻辑改善 & 
                            //   0002594: 平板在Assign Model站BTMAC和WIFIMAC分配逻辑改善
                            bool first = true;
                            int remaingCount = qty;
                            IUnitOfWork uof = new UnitOfWork();
                            for (int i = 0; i < qty; i++)
                            {
                                remaingCount--;
                                if (oldRule)
                                {
                                    currentMaxNum.Value = GetNextAndCheckChange34Base(currentMaxNum.Value, currentRange.BegNo, currentRange.EndNo);
                                }
                                else
                                {
                                    currentMaxNum.Value = GetNextAndCheck16Base(currentMaxNum.Value, currentRange.BegNo, currentRange.EndNo, first);
                                }

                                ret.Add(currentMaxNum.Value);
                                first = false;
                                if (remaingCount > 0 && currentMaxNum.Value == currentRange.EndNo)
                                {
                                    numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Closed);
                                    currentRange = numCtrlRepository.GetMACRange(cust, new string[] { "R", "A" });
                                    if (currentRange == null)
                                    {
                                        throw new FisException("ICT014", new string[] { });
                                    }
                                }
                            }

                            // IUnitOfWork uof = new UnitOfWork();
                            numCtrlRepository.Update(currentMaxNum, uof);
                            if (currentMaxNum.Value == currentRange.EndNo)
                            {
                                numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Closed);
                            }
                            else
                            {
                                numCtrlRepository.SetMACRangeStatusDefered(uof, currentRange.ID, MACRange.MACRangeStatus.Active);
                            }
                            uof.Commit();
                            SqlTransactionManager.Commit();
                            return ret;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SqlTransactionManager.Rollback();
                throw e;
            }
            finally
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }
        }

        /// <summary>
        /// get 16 base next by=umber
        /// </summary>
        /// <param name="currentNum"></param>
        /// <param name="minNum"></param>
        /// <param name="maxNum"></param>
        ///  <param name="firstTime"></param>
        /// <returns></returns>
        private string GetNextAndCheck16Base(string currentNum, string minNum, string maxNum, bool firstTime)
        {
            long curNum = Convert.ToInt64(currentNum, 16);
            long beginNum = Convert.ToInt64(minNum, 16);
            long endNum = Convert.ToInt64(maxNum, 16);
            if (curNum >= beginNum && curNum < endNum)
            {
                long nextSeqNum = curNum + 1;
                return nextSeqNum.ToString("X12");
            }
            else if (curNum < beginNum || curNum > endNum)  //重新range case
            {
                return beginNum.ToString("X12");
            }
            else
            {
                throw new FisException("GEN022", new string[] { currentNum });
            }
        }



        private string GetNextAndCheckChange34Base(string currentNum, string minNum, string maxNum)
        {
            string change34MaxNum = Change34(currentNum);
            string change34BeginNo = Change34(minNum);
            string change34EndNo = Change34(maxNum);
            if (string.Compare(change34MaxNum, change34BeginNo) < 0 || string.Compare(change34MaxNum, change34EndNo) > 0)
            {
                return minNum;
            }
            else
            {
                IMES.Infrastructure.Utility.Generates.intf.ISequenceConverter seqCvt = new IMES.Infrastructure.Utility.Generates.impl.SequenceConverterNormal("0123456789ABCDEF", 4, "FFFF", "0000", '0');
                string sequenceNumber = currentNum.Substring(currentNum.Length - 4, 4);
                string change34Sequence = sequenceNumber.Substring(1, 3).Insert(1, sequenceNumber.Substring(0, 1));
                change34Sequence = seqCvt.NumberRule.IncreaseToNumber(change34Sequence, 1);
                sequenceNumber = change34Sequence.Substring(1, 3).Insert(1, change34Sequence.Substring(0, 1));

                return currentNum.Substring(0, currentNum.Length - 4) + sequenceNumber;
            }
        }

        private string Change34(string input)
        {
            string sequenceNumber = input.Substring(input.Length - 4, 4);
            string change34Sequence = sequenceNumber.Substring(1, 3).Insert(1, sequenceNumber.Substring(0, 1));
            return input.Substring(0, input.Length - 4) + change34Sequence;
        }

        /// <summary>
        /// Decide MAC Rule
        /// </summary>
        /// <param name="mb"></param>
        /// <returns></returns>
        public bool DecideMACOldRule(IMB mb)
        {
            string macRule = partRepository.GetPartInfoValue(mb.Model, "MACRule");
            if (macRule == "2")
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Decide MAC Rule
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        public bool DecideMACOldRule(string family)
        {
            //IFamilyRepository familyRep = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
            IList<FamilyInfoDef> familyInfoList = familyRep.GetExistFamilyInfo(new FamilyInfoDef { family = family, name = "MACRule" });
            if (familyInfoList != null && familyInfoList.Count > 0)
            {
                if (familyInfoList[0].value == "2")
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region Check UPS device logical
        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="defineInfo"></param>
        /// <returns></returns>
        public string DecideAssignAstStation(IProduct product, AstDefineInfo defineInfo)
        {
            return defineInfo.AssignAstSNStation;
            //if (defineInfo.HasUPSAst == "Y" && product.IsUPSDevice)
            //{
            //    return defineInfo.AssignAstSNStation;
            //}
            //else
            //{
            //    return defineInfo.AssignAstSNStation;
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="defineInfo"></param>
        /// <returns></returns>
        public string DecideCombineStation(IProduct product, AstDefineInfo defineInfo)
        {
            return defineInfo.CombineStation;
            //if (defineInfo.HasUPSAst == "Y" && product.IsUPSDevice)
            //{
            //    return defineInfo.CombineStation;
            //}
            //else
            //{
            //    return defineInfo.CombineStation;
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="defineList"></param>
        /// <param name="currentStation"></param>
        /// <returns></returns>
        public bool checkUPSDeviceInAssignStation(IProduct product, 
                                                    IList<AstDefineInfo> defineList, 
                                                    string currentStation)
        {
            if (product.IsUPSDevice)
            {
                if (defineList.Any(define => define.HasUPSAst == "Y" &&
                                           define.AssignAstSNStation == currentStation))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="defineList"></param>
        /// <param name="currentStation"></param>
        /// <returns></returns>
        public bool checkUPSDeviceInCombineStation(IProduct product,
                                                   IList<AstDefineInfo> defineList,
                                                   string currentStation)
        {
            if (product.IsUPSDevice)
            {
                if (defineList.Any(define => define.HasUPSAst == "Y" &&
                                           define.CombineStation == currentStation))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Check CDSI, CNRS, UPS, None UPS
        /// </summary>
        /// <param name="session"></param>
        /// <param name="prod"></param>
        /// <param name="needGenAstDefineList"></param>
        /// <param name="needGenAstPartList"></param>
        /// <param name="currentStation"></param>
        public void checkGenAstDefineAndPart(Session session,
                                                                    IProduct prod, 
                                                                   IList<AstDefineInfo> needGenAstDefineList, 
                                                                   IList<IPart> needGenAstPartList,
                                                                    string currentStation)
        {

            session.AddValue(Session.SessionKeys.HasCDSI, "N");
            session.AddValue(Session.SessionKeys.HasCNRS, "N");
            session.AddValue(Session.SessionKeys.NeedGenAstDefineList, needGenAstDefineList);
            if (prod.IsCDSI)
            {
                #region 檢查CDSI 機器及過濾AV料號
                string atsnav = (string)prod.GetModelProperty("ATSNAV");
                if (!string.IsNullOrEmpty(atsnav))  //CDSI case
                {
                    IPart cdsiPart = null;
                    if (needGenAstPartList.Count == 1)
                    {
                        cdsiPart = needGenAstPartList[0];
                    }
                    else
                    {
                        cdsiPart = needGenAstPartList.Where(x => x.Attributes.Any(y => y.InfoType == "AV" && y.InfoValue == atsnav))
                                                                       .FirstOrDefault();
                    }
                    if (cdsiPart == null)
                    {
                        throw new FisException("CHK522", new string[] { prod.ProId });
                    }

                    //過濾CDSI Part No & AstDefine
                    needGenAstPartList = needGenAstPartList.Where(x => x.BOMNodeType != cdsiPart.BOMNodeType || x.Descr != cdsiPart.Descr).ToList();
                   // needGenAstDefineList = needGenAstDefineList.Where(x => needGenAstPartList.Any(y => y.BOMNodeType == x.AstType && y.Descr == x.AstCode)).ToList();

                    session.AddValue(Session.SessionKeys.HasCDSI, "Y");
                    session.AddValue(Session.SessionKeys.AVPartNo, atsnav);
                    session.AddValue(Session.SessionKeys.CDSIPart, cdsiPart);                    
                }
                else //CNRS case
                {
                    IPart cnrsPart = null;
                    if (needGenAstPartList.Count == 1)
                    {
                        cnrsPart = needGenAstPartList[0];
                    }
                    else
                    {
                        cnrsPart = needGenAstPartList.Where(x => x.BOMNodeType == "AT" && x.Descr == "ATSN3").FirstOrDefault();
                    }
                    if (cnrsPart == null)
                    {
                        throw new FisException("CQCHK1092", new string[] { prod.ProId, "ATSN3" });
                    }

                    //過濾CNRS Part No & AstDefine
                    needGenAstPartList = needGenAstPartList.Where(x => x.BOMNodeType != cnrsPart.BOMNodeType || x.Descr != cnrsPart.Descr).ToList();
                    //needGenAstDefineList = needGenAstDefineList.Where(x => needGenAstPartList.Any(y => y.BOMNodeType == x.AstType && y.Descr == x.AstCode)).ToList();
                    session.AddValue(Session.SessionKeys.AVPartNo, cnrsPart.Attributes.Where(x => x.InfoType == "AV").
                                                                                                  Select(y => y.InfoValue).FirstOrDefault());
                    session.AddValue(Session.SessionKeys.HasCNRS, "Y");
                    session.AddValue(Session.SessionKeys.CNRSPart, cnrsPart); 
                }
                #endregion
            }

            if (needGenAstPartList.Count > 0 && needGenAstDefineList.Count > 0)
            {
                bool hasPart = needGenAstDefineList.All(x => needGenAstPartList.Any(y => y.BOMNodeType == x.AstType && y.Descr == x.AstCode));
                if (!hasPart)
                {
                    //throw new FisException("CQCHK1092", new List<string> { prod.ProId,string.Join(",", needGenAstDefineList.Select(x=>x.AstCode).ToArray())});
                    return;
                }

                if (checkUPSDeviceInAssignStation(prod, needGenAstDefineList, currentStation))
                {
                    session.AddValue(Session.SessionKeys.IsUPSDevice, "Y");
                }
                else
                {
                    session.AddValue(Session.SessionKeys.IsUPSDevice, "N");
                }              

                session.AddValue(Session.SessionKeys.GenerateASTSN, "Y");
                //session.AddValue(Session.SessionKeys.NeedGenAstPartList, needCombineAstList.Select(x => x.AstPart).ToList());
                //session.AddValue(Session.SessionKeys.NeedGenAstDefineList, needCombineAstList.Select(x => x.AstDefine).ToList());

                session.AddValue(Session.SessionKeys.NeedGenAstPartList, needGenAstPartList);
                session.AddValue(Session.SessionKeys.NeedGenAstDefineList, needGenAstDefineList);

            }

        }

        #endregion

        #region Generate Ast
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="part"></param>
        /// <param name="curProduct"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="editor"></param>
        /// <param name="cust3AstNum"></param>
        /// <returns></returns>
        public  string AssignAstNumber(Session session,
                                                                   IPart part,
                                                                   IProduct curProduct,
                                                                    string station,
                                                                    string customer,
                                                                    string editor,
                                                                    out string cust3AstNum)
        {
            IList<ConstValueInfo> cvInfoList = null;
            ConstValueInfo cvInfo = null;
            string custNum = null;
            cust3AstNum = null;
            if (TryConstValue("PreFixSNAST", part.PN, out cvInfoList, out cvInfo))
            {

                if (string.IsNullOrEmpty(curProduct.CUSTSN))
                {
                    throw new FisException("CQCHK1108", new string[] { curProduct.ProId });
                }
                else
                {
                    custNum = cvInfo.value.Trim() + curProduct.CUSTSN;
                }

            }
            else
            {
                #region 產生ATSN 需要
                string astPo = (string)session.GetValue("AstPo");
                string cust = null;
                string cust3 = null;
                if (!string.IsNullOrEmpty(astPo))  //CNRS device assign AST Code
                {
                    cust = astPo;
                }
                else
                {
                    cust = curProduct.ModelObj.GetAttribute("Cust");                    
                }

                cust3 = curProduct.ModelObj.GetAttribute("Cust3");

                if (!string.IsNullOrEmpty(cust))
                {
                    custNum = GetAstNumber(session, curProduct, cust, station, customer, editor);
                }

                //检查Declare @Cust3 = ModelInfo.Value (Conditon: Model=#Prodocut.Model and Name = ‘Cust3’) ，
                //若@Cust3 不为空且不为Null，则执行下面AST的分配工作：
                if (!string.IsNullOrEmpty(cust3))
                {
                    cust3AstNum = GetAstNumber(session, curProduct, cust3, station, customer, editor);
                }
                #endregion
            }
            return custNum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="curProduct"></param>
        /// <param name="cust"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        public string GetAstNumber(Session session,  IProduct curProduct, string cust, string station, string customer, string editor)
        {
            string custNum = string.Empty;
            custNum = CheckAndGetUsedAst(session, curProduct.ProId, cust, "AST", station, editor);
            if (custNum == null)
            {
                custNum = CheckAndSetReleaseAstNumber(curProduct.ProId, cust, "AST", station, editor);
            }
            if (string.IsNullOrEmpty(custNum))
            {
                custNum = generateAstCode(session, cust, customer);

                // Checksum
                custNum = GetAstChecksum(cust, custNum);

                custNum = CheckAndAddPreFixDateAst(cust, custNum);

                //Check postfix 
                custNum = CheckAndAddPostFixAst(cust, custNum);
                //ConstValueInfo cond = new ConstValueInfo();
                //cond.type = "AST";
                //cond.name = cust;
                //IList<ConstValueInfo> valList = partRepository.GetConstValueInfoList(cond);
                //if (valList.Count > 0 && !String.IsNullOrEmpty(valList[0].value))
                //{
                //    custNum += valList[0].value.Trim();
                //}
                InsertCombinedAstNumber(curProduct.ProId, cust, "AST", custNum, station, editor);
            }
            return custNum;
        }

        private  string generateAstCode(Session session,string cust, string customer)
        {
            try
            {
                var currenProduct = (IProduct)session.GetValue(Session.SessionKeys.Product);
                string customerSN = (string)session.GetValue(Session.SessionKeys.CustSN);

                string custNum = "";
                string numType = "AST";

                //IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

                //INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

                SqlTransactionManager.Begin();
                lock (_syncRoot_GetAst)
                {
                    AssetRangeCodeInfo currentRange = partRepository.GetAssetRangeByStatus(cust, new string[] { "A", "R" });

                    if (currentRange == null)
                    {
                        throw new FisException("CHK200", new string[] { customerSN });
                    }

                    NumControl currentMaxNum = numCtrlRepository.GetMaxValue(numType, cust);

                    if (currentMaxNum == null)
                    {
                        #region 第一次產生Serial Number
                        //Check new Range
                        checkAssetNum(customerSN,
                                                     currentRange.Begin, currentRange.End);
                        currentMaxNum = new NumControl();
                        currentMaxNum.NOName = cust;
                        currentMaxNum.NOType = numType;
                        currentMaxNum.Value = currentRange.Begin;
                        currentMaxNum.Customer = customer;

                        IUnitOfWork uof = new UnitOfWork();
                        numCtrlRepository.InsertNumControlDefered(uof, currentMaxNum);
                        if (currentMaxNum.Value == currentRange.End)
                        {
                            partRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "C");
                        }
                        else
                        {
                            partRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "A");
                        }
                        uof.Commit();
                        SqlTransactionManager.Commit();
                        custNum = currentMaxNum.Value;
                        #endregion
                    }
                    else
                    {
                        #region no need check endNumber
                        //if (currentMaxNum.Value == currentRange.End)
                        //{
                        //    ipartRepository.SetAssetRangeStatus(currentRange.ID, "C");
                        //    currentRange = ipartRepository.GetAssetRangeByStatus(cust, new string[] { "R" });
                        //    if (currentRange == null ||
                        //        currentMaxNum.Value.Equals(currentRange.Begin) ||
                        //        currentMaxNum.Value.Equals(currentRange.End))
                        //    {
                        //        throw new FisException("CHK200", new string[] { customerSN });
                        //    }
                        //    else
                        //    {
                        //        #region 更換新Range產生Serial Number
                        //        //Check new Range
                        //        CheckAssetNum(customerSN, currentRange.Begin, currentRange.End);
                        //        IUnitOfWork uof = new UnitOfWork();
                        //        currentMaxNum.Value = currentRange.Begin;
                        //        numCtrlRepository.Update(currentMaxNum, uof);
                        //        ipartRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "A");
                        //        uof.Commit();
                        //        SqlTransactionManager.Commit();
                        //        custNum = currentMaxNum.Value;
                        //        #endregion
                        //    }
                        //}
                        #endregion

                        if (currentRange.Status == "R" &&
                           checkNewRange(currentMaxNum.Value, currentRange.Begin, currentRange.End))
                        {
                            #region 更換新Range產生Serial Number
                            //Check new Range
                            checkAssetNum(customerSN, currentRange.Begin, currentRange.End);
                            IUnitOfWork uof = new UnitOfWork();
                            currentMaxNum.Value = currentRange.Begin;
                            numCtrlRepository.Update(currentMaxNum, uof);
                            if (currentRange.Begin.CompareTo(currentRange.End) < 0)
                            {
                                partRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "A");
                            }
                            else
                            {
                                partRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "C");
                            }
                            uof.Commit();
                            SqlTransactionManager.Commit();
                            custNum = currentMaxNum.Value;
                            #endregion
                        }
                        else
                        {
                            currentMaxNum.Value = genNextAssetNum(customerSN, currentMaxNum.Value, currentRange.End);
                            IUnitOfWork uof = new UnitOfWork();
                            numCtrlRepository.Update(currentMaxNum, uof);
                            if (currentMaxNum.Value == currentRange.End)
                            {
                                partRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "C");
                            }
                            else
                            {
                                partRepository.SetAssetRangeStatusDefered(uof, currentRange.ID, "A");
                            }
                            uof.Commit();
                            SqlTransactionManager.Commit();
                            custNum = currentMaxNum.Value;
                        }
                    }
                }

                if (cust == "SCUSTA-1")
                {
                    custNum = "000" + custNum + "00";
                }

                return custNum;
            }
            catch (Exception e)     //2012-7-19
            {
                SqlTransactionManager.Rollback();
                throw e;
            }
            finally                 //2012-7-19
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }
        }

        private string genNextAssetNum(string custSN, string lastNum, string rangeMaxNum)
        {

            //Get last num PreStr & Sequence number
            string lastAssetNum = "";
            string lastPreString = "";
            string EndNum = "";
            string preEndString = "";

            Match lastNumMatch = null;
            Match endNumMatch = null;
            checkAssetNum(custSN, lastNum, rangeMaxNum, ref lastNumMatch, ref endNumMatch);

            lastAssetNum = lastNumMatch.Value;
            lastPreString = lastNum.Substring(0, lastNumMatch.Index);
            EndNum = endNumMatch.Value;
            preEndString = rangeMaxNum.Substring(0, endNumMatch.Index);
            long largestNum = long.Parse(lastAssetNum);
            long end = long.Parse(EndNum);
            largestNum++;
            int largestNumCount = lastAssetNum.Length;
            string strNewLargestCustNum = largestNum.ToString().PadLeft(largestNumCount, '0');
            return preEndString + strNewLargestCustNum;

        }

        private void checkAssetNum(string custSN,
                                                            string lastNum,
                                                            string rangeMaxNum,
                                                            ref Match lastNumMatch,
                                                            ref Match endNumMatch)
        {

            string lastPreString = "";
            string preEndString = "";

            if (lastNum.Length != rangeMaxNum.Length)
            {
                throw new FisException("CHK201", new string[] { custSN });
            }

            MatchCollection matches = Regex.Matches(lastNum, @"\d+");
            if (matches.Count == 0)
            {
                throw new FisException("CHK201", new string[] { custSN });
            }
            lastNumMatch = matches[matches.Count - 1];

            matches = Regex.Matches(rangeMaxNum, @"\d+");
            if (matches.Count == 0)
            {
                throw new FisException("CHK201", new string[] { custSN });
            }
            endNumMatch = matches[matches.Count - 1];

            if (lastNumMatch.Index != endNumMatch.Index ||
                lastNumMatch.Length != endNumMatch.Length)
            {
                throw new FisException("CHK201", new string[] { custSN });
            }

            //最後不適數字
            if (lastNum.Length != (lastNumMatch.Index + lastNumMatch.Length))
            {
                throw new FisException("CHK201", new string[] { custSN });
            }

            lastPreString = lastNum.Substring(0, lastNumMatch.Index);
            preEndString = rangeMaxNum.Substring(0, endNumMatch.Index);

            if (lastPreString != preEndString)
            {
                throw new FisException("CHK201", new string[] { custSN });
            }

            if (lastNumMatch.Value.CompareTo(endNumMatch.Value) == 1)
            {
                throw new FisException("CHK201", new string[] { custSN });
            }

        }

        private void checkAssetNum(string custSN,
                                                            string lastNum,
                                                            string rangeMaxNum)
        {
            Match lastNumMatch = null;
            Match endNumMatch = null;
            checkAssetNum(custSN, lastNum, rangeMaxNum, ref lastNumMatch, ref endNumMatch);
        }

        private bool checkNewRange(string maxNum, string beginNum, string endNum)
        {

            int iBegin = beginNum.CompareTo(maxNum);
            int iEnd = endNum.CompareTo(maxNum);
            if ((iBegin == 1 && iEnd == -1 && maxNum.Length == endNum.Length) || iBegin == 0 || iEnd == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
    }

    /// <summary>
    /// QC Type 
    /// </summary>
    public enum QCTypeEnum
    {
        /// <summary>
        /// EPIA
        /// </summary>
        EPIA = 1,    //EPIA
        /// <summary>
        /// PIA
        /// </summary>
        PIA = 2,   //PIA
        /// <summary>
        /// PAQC
        /// </summary>
        PAQC = 3,  //PAQC
        /// <summary>
        /// RPAQC
        /// </summary>
        RPAQC = 4 //RCTO PAQC
    }

    /// <summary>
    /// PrintLogBegNoEnum
    /// </summary>
    public enum PrintLogBegNoEnum
    {
        /// <summary>
        /// productID or MBSN
        /// </summary>
        ID = 1,  
        /// <summary>
        /// CustSN
        /// </summary>
        CustSN = 2,
        /// <summary>
        /// PalletNo
        /// </summary>
        PalletNo = 3,
        /// <summary>
        /// DeliveryNo
        /// </summary>
        DeliveryNo = 4,
        /// <summary>
        /// ModelName
        /// </summary>
        ModelName = 5,
        /// <summary>
        /// Session Key
        /// </summary>
        Key = 6,
        /// <summary>
        /// SessionKeyValue
        /// </summary>
        SessionKeyValue = 7

    }

    /// <summary>
    /// SequenceBaseEnum
    /// </summary>
    public enum SequenceBaseEnum
    {
        /// <summary>
        /// Base10
        /// </summary>
        Base10 = 1,
        /// <summary>
        /// Base16
        /// </summary>
        Base16 = 2,
        /// <summary>
        /// Base36
        /// </summary>
        Base36 = 3

    }
    
    

}
