using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using IMES.FisObject.PAK.COA;
using IMES.Infrastructure.UnitOfWork;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using System.IO;
using System.Configuration;

namespace IMES.Maintain.Implementation
{
    public class COAReceivingManager : MarshalByRefObject,ICOAReceiving
    {
        public char[] SPLITSTR = new char[] { ':' };
        public const string CUSTOMERNAME = "Customer Name";
        public const string INVENTECPO = "Inventec P/O";
        public const string CUSTPN = "Customer P/N";
        public const string IECPN = "IEC P/N";
        public const string DESC = "Description";
        public const string SHIPPINGDATE = "Shipping Date";
        public const string QTY = "Quantity";
        public const string BEGNO = "Start COA Number";
        public const string ENDNO = "End COA Number";
        #region ICOAReceiving 成员
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string[] COANOFormat = new string[] { "XP-", "W7-", "UL-", "V-" };
        public  const string COANOFORMATXP = "XP-";
        public  const string COANOFORMATW7 = "W7-";
        public  const string COANOFORMATUL = "UL-";
        public  const string COANOFORMATV = "V-";
        public const int SLICEINDEX = 6;//7;

        ICOAStatusRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository>();
        public void SaveTXTIntoTmpTable(IList<IMES.DataModel.COAReceivingDef> dataLst)
        {
            try 
            {
                IUnitOfWork ow = new UnitOfWork();
                if(dataLst.Count>0)
                {
                    COAReceivingDef def = dataLst[0];
                    itemRepository.RemoveTmpTableItemDefered(ow,def.pc.Trim());
                    IList<TmpTableInfo> voLst=PO2VO(dataLst);
                    itemRepository.SaveTXTIntoTmpTableDefered(ow,voLst);
                    ow.Commit();
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

        private IList<TmpTableInfo> PO2VO(IList<IMES.DataModel.COAReceivingDef> dataLst)
        {
            IList<TmpTableInfo> tmpDataLst = new List<TmpTableInfo>();

            foreach (COAReceivingDef tmp in dataLst)
            {
                TmpTableInfo info = new TmpTableInfo();
                info.po = tmp.po;
                info.shipDate = Convert.ToDateTime(tmp.shippingDate);
                info.custPN = tmp.custPN;
                info.iecpn = tmp.iecPN;
                info.descr = tmp.description;
                info.begSN = tmp.begNO;
                info.endSN = tmp.endNO;
                info.qty = Convert.ToInt32(tmp.qty);
                info.editor = tmp.editor;
                info.cdt = DateTime.Now;
                info.cust = tmp.customerName;
                info.udt = DateTime.Now;
                info.pc = tmp.pc;
                //check BeginNO EndNo...
                // EndNO BeginNo format must be same
                // EndNO must be > BeginNO
                
                //xp,w7..,需要字符为13位,并且后10位是数字，并且数字部分BeginNo<EndNo...
                //if (info.begSN.Trim().StartsWith(COANOFORMATXP) && info.endSN.Trim().StartsWith(COANOFORMATXP)
                //    ||(info.begSN.Trim().StartsWith(COANOFORMATW7) && info.endSN.Trim().StartsWith(COANOFORMATW7))
                //        ||(info.begSN.Trim().StartsWith(COANOFORMATUL) && info.endSN.Trim().StartsWith(COANOFORMATUL)))
                //{
                //    if(info.begSN.Trim().Length==13&&info.endSN.Trim().Length==13)
                    //{
                        //截取后面的数字部分,并验证是否为数字，BegNo<EndNo,qty 必须为(endNO-beginNo)+1
                        CheckBegNoAndEndNoLength(info);
                        CheckBegNoAndEndNoStartString(info,SLICEINDEX);
                        CheckBegNoAndEndNo(info,SLICEINDEX);
                        
                    //}
                    //else
                    //{
                    //    //throw Exception
                    //    //位数不对
                    //    ThrowBizException("DMT107");
                    //}
                //}
                //V-,共需要字符为11位,并且后就为是数字,数字部分BeginNo<EndNo,qty 必须为(endNO-beginNo)+1
                //else if(info.begSN.Trim().StartsWith(COANOFORMATV) && info.endSN.Trim().StartsWith(COANOFORMATV))
                //{
                    //if(info.begSN.Trim().Length==11&&info.endSN.Trim().Length==11)
                    //{
                       //截取后面的数字部分,并验证是否为数字，BegNo<EndNo..
                        //CheckBegNoAndEndNo(info,2);
                    //}
                    //else
                    //{
                    //    //throw Exception
                    //    //位数不对
                    //    ThrowBizException("DMT107");
                    //}
                //}
                //可能全是数字 符合要求，可能是不符合要求的窜..
                //else
                //{
                    //if (info.begSN.Trim().Length == 14)
                    //{
                        //CheckBegNoAndEndNo(info);
                    //}
                    //else 
                    //{
                    //    //throw Exception..
                    //    //begVal,endVal不符合要求...
                    //    ThrowBizException("DMT108");
                    //}
                    
                //}

                tmpDataLst.Add(info);
            }
            return tmpDataLst;
        }

        private static void ThrowBizException(string errorCode)
        {
            List<string> param = new List<string>();
            FisException fe = new FisException(errorCode, param);
            throw fe;
        }

        private static void CheckBegNoAndEndNoLength(TmpTableInfo info)
        {
            if (info.begSN.Trim().Length<=8||info.endSN.Trim().Length<=8)
            {
                ThrowBizException("DMT129");
            }
            //compare begno&endno length
            if (info.begSN.Trim().Length != info.endSN.Trim().Length)
            {
                ThrowBizException("DMT128");
            }
        }
        private static void CheckBegNoAndEndNoStartString(TmpTableInfo info,int startIndex) 
        {
            string startBeg = info.begSN.Trim().Substring(0,info.begSN.Trim().Length-startIndex);
            string startEnd = info.endSN.Trim().Substring(0,info.endSN.Trim().Length-startIndex);
            if(string.Compare(startBeg,startEnd,true)!=0)
            {
                ThrowBizException("DMT130");
            }
        }
        private static void CheckBegNoAndEndNo(TmpTableInfo info,int startIndex)
        {
            long beginNo, endNo;
            try
            {
                //slice begno&endno last 7 char and compare...
                beginNo = Convert.ToInt64(info.begSN.Trim().Substring(info.begSN.Trim().Length - startIndex, startIndex));
                endNo = Convert.ToInt64(info.endSN.Trim().Substring(info.endSN.Trim().Length - startIndex, startIndex));
                if (endNo < beginNo)//(endNo <= beginNo)
                {
                    //throw Excepiton
                    ThrowBizException("DMT110");
                }
                else 
                {
                    if(info.qty!=(endNo-beginNo)+1)
                    {
                        //throw qty  invali....
                        ThrowBizException("DMT111");
                    }
                }

            }
            catch (FormatException)
            {
                ThrowBizException("DMT109");
                //throw Exception...
            }
        }
        private static void CheckBegNoAndEndNo(TmpTableInfo info)
        {
            long beginNo, endNo;
            try
            {
                beginNo = Convert.ToInt64(info.begSN.Trim());
                endNo = Convert.ToInt64(info.endSN.Trim());
                if (endNo < beginNo)//(endNo <= beginNo)
                {
                    //throw Excepiton
                    ThrowBizException("DMT110");
                }
                else 
                {
                    if (info.qty != (endNo - beginNo) + 1)
                    {
                        //throw qty  invali....
                        ThrowBizException("DMT111");
                    }
                }

            }
            catch (FormatException)
            {
                ThrowBizException("DMT109");
                //throw Exception...
            }
        }

        public void RemoveTmpTableItem(string pc)
        {
            try 
            {
                itemRepository.RemoveTmpTableItem(pc);
            }
            catch(Exception)
            {
                throw;
            }

        }

       

        public bool saveItemIntoCOAReceiveTable(string pc)
        {
            try 
            {
                IUnitOfWork ow = new UnitOfWork();
                IList<TmpTableInfo> tmpDataLst=itemRepository.GetTmpTableItem(pc);
                IList<COAReceive> COADataLst = new List<COAReceive>();
                IList<COAReceive> DBCOADateLst = itemRepository.GetAllCOAReceivingItems();
                IList<COAStatus> statusLst = new List<COAStatus>();
                if (tmpDataLst != null && tmpDataLst.Count > 0)
                {
                   
                    foreach(TmpTableInfo tmp in tmpDataLst)
                    {
                        COAReceive coa = new COAReceive();
                        coa.PO = tmp.po;
                        coa.ShipDate =tmp.shipDate;
                        coa.CustPN = tmp.custPN;
                        coa.IECPN = tmp.iecpn;
                        coa.Descr = tmp.descr;
                        coa.BegSN = tmp.begSN;
                        coa.EndSN = tmp.endSN;
                        coa.Qty = tmp.qty;
                        coa.Editor = tmp.editor;
                        coa.Cdt = tmp.cdt;
                        coa.Cust = tmp.cust;
                        coa.Udt = tmp.udt;
                        //当前对象的前缀
                        string currentprefix = coa.BegSN.Trim().Substring(0, coa.BegSN.Trim().Length - SLICEINDEX);
                        //判断新加入BegNo,EndNo是否包含在在DB中BegNo,EndNo之中..
                        long begIndex = 0, endIndex = 0;
                        begIndex = Convert.ToInt64(coa.BegSN.Trim().Substring(coa.BegSN.Trim().Length - SLICEINDEX, SLICEINDEX));
                        endIndex = Convert.ToInt64(coa.EndSN.Trim().Substring(coa.EndSN.Trim().Length - SLICEINDEX, SLICEINDEX));

                        //DB无数据.
                        if (DBCOADateLst.Count == 0)
                        {
                            //if (coa.BegSN.Trim().StartsWith(COANOFORMATXP)
                            //        || (coa.BegSN.Trim().StartsWith(COANOFORMATW7) )
                            //            || (coa.BegSN.Trim().StartsWith(COANOFORMATUL)))
                            {
                                CreatStatusAllIndex(coa, ref begIndex, ref endIndex,SLICEINDEX);
                            }
                            //else if (coa.BegSN.Trim().StartsWith(COANOFORMATV))
                            //{
                            //    CreatStatusAllIndex(coa, ref begIndex, ref endIndex,2);
                            //}
                            //else 
                            //{
                            //    begIndex = Convert.ToInt64(coa.BegSN.Trim());
                            //    endIndex = Convert.ToInt64(coa.EndSN.Trim());
                            //}
                        }
                        else
                        {
                            //DB中存在数据.
                            foreach (COAReceive cr in DBCOADateLst)
                            {
                                string dbprefix = cr.BegSN.Trim().Substring(0, cr.BegSN.Trim().Length - SLICEINDEX);
                                
                                if (string.Compare(dbprefix, currentprefix, true) == 0)
                                //if (coa.BegSN.Trim().StartsWith(COANOFORMATXP) && cr.BegSN.Trim().StartsWith(COANOFORMATXP)
                                //    || (coa.BegSN.Trim().StartsWith(COANOFORMATW7) && cr.BegSN.Trim().StartsWith(COANOFORMATW7))
                                //        || (coa.BegSN.Trim().StartsWith(COANOFORMATUL) && cr.BegSN.Trim().StartsWith(COANOFORMATUL)))
                                {
                                    CheckBeginNoAndEndNoToDB(coa, ref begIndex, ref endIndex, cr, SLICEINDEX);
                                }
                                //else if (coa.BegSN.Trim().StartsWith(COANOFORMATV) && cr.BegSN.Trim().StartsWith(COANOFORMATV))
                                //{
                                //    CheckBeginNoAndEndNoToDB(coa, ref begIndex, ref endIndex, cr, 1);
                                //}
                                //数字...
                                //else
                                //{
                                //    try
                                //    {
                                //        begIndex = Convert.ToInt64(coa.BegSN.Trim());
                                //        endIndex = Convert.ToInt64(coa.EndSN.Trim());
                                //        long DBbegIndex = Convert.ToInt64(cr.BegSN.Trim());
                                //        long DBendIndex = Convert.ToInt64(cr.EndSN.Trim());
                                //        if (begIndex >= DBendIndex && begIndex <= DBendIndex || (endIndex >= DBbegIndex && endIndex <= DBendIndex))
                                //        {
                                //            //throw Exception
                                //            //记录不能再DB begno endno 之间...
                                //            ThrowBizException("DMT112");
                                //        }
                                //    }
                                //    catch (FormatException)
                                //    {
                                //        //忽略..
                                //    }
                                //}
                            }
                        }
                            for (long i = begIndex; i <= endIndex; i++)
                            {
                                
                                COAStatus status = new COAStatus();
                                //string prefix=String.Empty;
                                //if (coa.BegSN.Trim().StartsWith(COANOFORMATXP))
                                //{
                                //    prefix=COANOFORMATXP;
                                //}
                                //else if (coa.BegSN.Trim().StartsWith(COANOFORMATW7))
                                //{
                                //    prefix=COANOFORMATW7;
                                //}
                                //else if (coa.BegSN.Trim().StartsWith(COANOFORMATUL))
                                //{
                                //    prefix=COANOFORMATUL;
                                //}
                                //else if (coa.BegSN.Trim().StartsWith(COANOFORMATV))
                                //{
                                //    prefix=COANOFORMATV;
                                //}
                               
                                //if(!String.IsNullOrEmpty(prefix))
                                //{
                                //    status.COASN = prefix+i;
                                //}
                                //else
                                //{
                                //    status.COASN = i.ToString();
                                //}
                                
                                //Debug, 不足六位前面补0
                                //status.COASN = currentprefix + i;
                                status.COASN = currentprefix + string.Format("{0:D6}", i);

                                status.IECPN = coa.CustPN;
                                
                                status.LineID = "";
                                status.Editor = coa.Editor;
                                status.Status = "A0";
                                status.Cdt = DateTime.Now;
                                status.Udt = DateTime.Now;
                                statusLst.Add(status);
                            }
                            COADataLst.Add(coa);
                        }
                        
                    
                    itemRepository.SaveItemIntoCOAStatusTableDefered(ow,statusLst);
                    itemRepository.RemoveTmpTableItemDefered(ow, pc);
                    itemRepository.SaveItemIntoCOAReceiveTableDefered(ow, COADataLst);
                    ow.Commit();
                    return true;
                }
            }
            catch(Exception)
            {
                throw;
            }
            return false;
        }

        private static void CreatStatusAllIndex(COAReceive coa, ref long begIndex, ref long endIndex, int sliceIndex)
        {
            begIndex = Convert.ToInt64(coa.BegSN.Trim().Substring(coa.BegSN.Trim().Length-sliceIndex,sliceIndex));
            endIndex = Convert.ToInt64(coa.EndSN.Trim().Substring(coa.EndSN.Trim().Length-sliceIndex,sliceIndex));
        }

        private static void CheckBeginNoAndEndNoToDB(COAReceive coa, ref long begIndex, ref long endIndex, COAReceive cr, int sliceIndex)
        {
            begIndex = Convert.ToInt64(coa.BegSN.Trim().Substring(coa.BegSN.Trim().Length-sliceIndex,sliceIndex));
            endIndex = Convert.ToInt64(coa.EndSN.Trim().Substring(coa.EndSN.Trim().Length-sliceIndex,sliceIndex));
            long DBbegIndex = Convert.ToInt64(cr.BegSN.Trim().Substring(cr.BegSN.Trim().Length-sliceIndex,sliceIndex));
            long DBendIndex = Convert.ToInt64(cr.EndSN.Trim().Substring(cr.EndSN.Trim().Length-sliceIndex,sliceIndex));
            if (begIndex >= DBendIndex && begIndex <= DBendIndex || (endIndex >= DBbegIndex && endIndex <= DBendIndex))
            {
                //throw Exception
                //记录不能再DB begno endno 之间...
                ThrowBizException("DMT112");
            }
        }

        #endregion

        #region ICOAReceiving 成员


        public IList<IMES.DataModel.COAReceivingDef> GetTmpTableItem(string pc)
        {
            IList<COAReceivingDef> defDataLst = new List<COAReceivingDef>();
            try 
            {
               IList<TmpTableInfo> tmpDataList= itemRepository.GetTmpTableItem(pc);
               defDataLst=VO2PO(tmpDataList);
            }
            catch(Exception)
            {
                throw;
            }
            return defDataLst;
        }

        private IList<COAReceivingDef> VO2PO(IList<TmpTableInfo> tmpDataList)
        {
            IList<COAReceivingDef> defDataLst = new List<COAReceivingDef>();
            foreach (TmpTableInfo info in tmpDataList)
            {
                COAReceivingDef tmp = new COAReceivingDef();
                tmp.po = info.po;
                //issue code
                //ITC-1361-0060  itc210012  2012-02-01
                tmp.shippingDate = info.shipDate.ToString("yyyy-MM-dd");

                tmp.custPN = info.custPN;
                tmp.iecPN = info.iecpn;
                tmp.description = info.descr;
                tmp.begNO = info.begSN;
                tmp.endNO = info.endSN;
                tmp.qty = info.qty.ToString();

                tmp.editor = info.editor;
                //      info.cdt = DateTime.Now;
                tmp.customerName = info.cust;
                //       info.udt = DateTime.Now;
                defDataLst.Add(tmp);
            }
            return defDataLst;
        }

        #endregion



        #region ICOAReceiving 成员


        public IList<COAReceivingDef> ReadTXTFile(string path, string userName,string ip)
        {
            string line = String.Empty;

            IList<COAReceivingDef> coaLst = new List<COAReceivingDef>();
            COAReceivingDef globalObj = new COAReceivingDef();
            COAReceivingDef def = new COAReceivingDef();
            int flagBegin = 0;
            bool isQty = false, isBegNo = false, isEndNo = false;

            using (StreamReader reader = new StreamReader(path))
            {
                string l = String.Empty;

                while (!reader.EndOfStream)
                {
                    l = reader.ReadLine();
                    string[] attr = l.Split(SPLITSTR);
                    //把多行回车过滤掉...
                    //ITC-1361-0098 ITC210012 2012-02-22 
                    if (attr.Length == 1 || attr[0].Trim() == "")
                    {
                        continue;
                    }
                    string attrKey = attr[0].Trim();
                    string attrVal = attr[1].Trim();

                    if (string.Compare(CUSTOMERNAME, attrKey, true) == 0)
                    {
                        List<string> param = new List<string>();
                        param.Add("[Customer Name]");
                        CheckAttrVal(param, attrVal);
                        globalObj.customerName = attrVal;
                        flagBegin |= 1;
                    }
                    else if (string.Compare(INVENTECPO, attrKey, true) == 0)
                    {
                        List<string> param = new List<string>();
                        param.Add("[Inventec PO]");
                        CheckAttrVal(param, attrVal);
                        globalObj.po = attrVal;
                        flagBegin |= 2;
                    }
                    else if (string.Compare(CUSTPN, attrKey, true) == 0)
                    {
                        List<string> param = new List<string>();
                        param.Add("[Customer PN]");
                        CheckAttrVal(param, attrVal);
                        globalObj.custPN = attrVal;
                        flagBegin |= 4;
                    }
                    else if (string.Compare(IECPN, attrKey, true) == 0)
                    {
                        List<string> param = new List<string>();
                        param.Add("[IEC PN]");
                        CheckAttrVal(param, attrVal);
                        globalObj.iecPN = attrVal;
                        flagBegin |= 8;
                    }
                    else if (string.Compare(DESC, attrKey, true) == 0)
                    {
                        List<string> param = new List<string>();
                        param.Add("[Description]");
                        CheckAttrVal(param, attrVal);
                        globalObj.description = attrVal;
                        flagBegin |= 16;
                    }
                    else if (string.Compare(SHIPPINGDATE, attrKey, true) == 0)
                    {
                        List<string> param = new List<string>();
                        param.Add("[Shipping Date]");
                        CheckAttrVal(param, attrVal);
                        globalObj.shippingDate = attrVal;
                        flagBegin |= 32;
                    }
                    if (isQty && isBegNo && isEndNo)
                    {
                        isQty = false;
                        isBegNo = false;
                        isEndNo = false;

                        COAReceivingDef crDef = new COAReceivingDef();
                        crDef.customerName = globalObj.customerName;
                        crDef.po = globalObj.po;
                        crDef.custPN = globalObj.custPN;
                        crDef.iecPN = globalObj.iecPN;
                        crDef.description = globalObj.description;
                        crDef.shippingDate = globalObj.shippingDate;
                        crDef.qty = def.qty;
                        crDef.begNO = def.begNO;
                        crDef.endNO = def.endNO;
                        crDef.editor = userName;
                        crDef.pc = ip;
                        coaLst.Add(crDef);
                    }
                    if (string.Compare(QTY, attrKey, true) == 0 || string.Compare(BEGNO, attrKey, true) == 0 || string.Compare(ENDNO, attrKey, true) == 0)
                    {

                        if (string.Compare(QTY, attrKey, true) == 0)
                        {
                            if (!isQty)
                            {
                                List<string> param = new List<string>();
                                param.Add("[Quantity]");
                                CheckAttrVal(param, attrVal);
                                def.qty = attrVal;
                                isQty = true;
                            }
                            else
                            {
                                ThrowBizException("DMT106");
                            }

                        }

                        if (string.Compare(BEGNO, attrKey, true) == 0)
                        {
                            if (!isBegNo)
                            {
                                List<string> param = new List<string>();
                                param.Add("[Start COA Number]");
                                CheckAttrVal(param, attrVal);
                                //如果包含小写字母,抛出异常...
                                if (System.Text.RegularExpressions.Regex.IsMatch(attrVal, "[a-z]"))
                                {
                                    ThrowBizException("DMT105");
                                }
                                def.begNO = attrVal;
                                isBegNo = true;
                            }

                            else
                            {
                                ThrowBizException("DMT106");
                            }
                        }

                        if (string.Compare(ENDNO, attrKey, true) == 0)
                        {
                            if (!isEndNo)
                            {
                                List<string> param = new List<string>();
                                param.Add("[End COA Number]");
                                CheckAttrVal(param, attrVal);
                                if (System.Text.RegularExpressions.Regex.IsMatch(attrVal, "[a-z]"))
                                {
                                    ThrowBizException("DMT105");
                                }
                                def.endNO = attrVal;
                                isEndNo = true;
                            }

                            else
                            {
                                ThrowBizException("DMT106");
                            }
                        }

                    }
                }
                // read end file
                if (reader.EndOfStream)
                {
                    if (!isQty || !isBegNo || !isEndNo)
                    {
                        ThrowBizException("DMT106");
                    }
                    else
                    {
                        COAReceivingDef crDef = new COAReceivingDef();
                        crDef.customerName = globalObj.customerName;
                        crDef.po = globalObj.po;
                        crDef.custPN = globalObj.custPN;
                        crDef.iecPN = globalObj.iecPN;
                        crDef.description = globalObj.description;
                        crDef.shippingDate = globalObj.shippingDate;
                        crDef.qty = def.qty;
                        crDef.begNO = def.begNO;
                        crDef.endNO = def.endNO;
                        crDef.editor = userName;
                        crDef.pc = ip;
                        coaLst.Add(crDef);
                    }
                }

            }
            if (flagBegin != (1 | 2 | 4 | 8 | 16 | 32))
            {

                ThrowBizException("DMT106");
            }

            return coaLst;
        }

        #endregion
        //private static void ThrowBizException(string errorCode)
        //{
        //    List<string> param = new List<string>();
        //    FisException fe = new FisException(errorCode, param);
        //    throw fe;
        //}

        public static void deleteFiles(string strDir)
        {
            try
            {
                if (File.Exists(strDir))
                {
                    File.Delete(strDir);
                    Console.WriteLine("文件删除成功！");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void CheckAttrVal(List<string> param, string val)
        {
            FisException fe = null;
            if (String.IsNullOrEmpty(val.Trim()))
            {
                fe = new FisException("DMT104", param);
                throw fe;
            }
        }
       
        
       
    }
}
