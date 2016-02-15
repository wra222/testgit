using System;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Misc;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.PrintItem;
using IMES.DataModel;
using IMES.FisObject.Common.Warranty;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.NumControl;
using System.Data;


namespace IMES.Maintain.Implementation
{
    public class CommonImpl:MarshalByRefObject,ICustomer,IFamily2,IWarranty,IMACRange,ISelectData       
    {
        public const String MACRANGE_STATUS_R = "R";
        public const String MACRANGE_STATUS_R_TEXT = "Created";
        public const String MACRANGE_STATUS_A = "A";
        public const String MACRANGE_STATUS_A_TEXT = "Active";
        public const String MACRANGE_STATUS_C = "C";
        public const String MACRANGE_STATUS_C_TEXT = "Closed";


        #region Implementation of ICustomer
        public IList<CustomerInfo> GetCustomerList()
        {

            IList<CustomerInfo> retLst = null;

            try
            {
                IMiscRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                retLst = itemRepository.GetCustomerList();
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void AddCustomer(CustomerInfo customerInfo)
        {
            FisException ex;
            List<string> paraError = new List<string>();

            try
            {
                IMiscRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();

                if (itemRepository.GetCustomerInfo(customerInfo.customer) == null)
                {
                    UnitOfWork uow = new UnitOfWork();
                    itemRepository.AddCustomerDefered(uow, customerInfo);
                    uow.Commit();
                }
                else
                {
                    ex = new FisException("DMT056", paraError);
                    throw ex;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void DeleteCustomer(CustomerInfo customerInfo)
        {
            try
            {
                IMiscRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                itemRepository.DeleteData<IMES.Infrastructure.Repository._Metas.Customer, CustomerInfo>(customerInfo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UploadCustomer(CustomerInfo condition, CustomerInfo value)
        {
            try
            {
                IMiscRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                itemRepository.UpdateDataByID<IMES.Infrastructure.Repository._Metas.Customer, CustomerInfo>(condition, value);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region Implementation of IFamily2
        /// <summary>
        /// 取得Customer下的family数据的list(按Family列的字母序排序)
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public IList<IMES.DataModel.FamilyDef> GetFamilyInfoList(String customerId)
        {

            //List<IMES.DataModel.FamilyDef> retLst = new List<FamilyDef>();

            //IMES.DataModel.FamilyDef item1 = new IMES.DataModel.FamilyDef();
            //item1.Family = "family1";
            //item1.Descr = "Descr1";
            //item1.CustomerID = "IES";

            //IMES.DataModel.FamilyDef item2 = new IMES.DataModel.FamilyDef();
            //item2.Family = "family2";
            //item2.Descr = "Descr2";
            //item2.CustomerID = "IES";

            //retLst.Add(item1);
            //retLst.Add(item2);

            //return retLst;
            List<IMES.DataModel.FamilyDef> retLst = new List<FamilyDef>();

            try
            {
                IFamilyRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
                IList<Family> getData=itemRepository.GetFamilyList(customerId);
                for(int i=0;i<getData.Count;i++)
                {
                    FamilyDef item =new FamilyDef();
                    item.CustomerID = getData[i].Customer;
                    item.Descr = getData[i].Description;
                    item.Family = getData[i].FamilyName;
                    item.Editor = getData[i].Editor;


                    if (getData[i].Cdt == DateTime.MinValue)
                    {
                        item.cdt = "";
                    }
                    else
                    {
                        item.cdt = ((System.DateTime)getData[i].Cdt).ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    if (getData[i].Udt == DateTime.MinValue)
                    {
                        item.udt = "";
                    }
                    else
                    {
                        item.udt = ((System.DateTime)getData[i].Udt).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    //item.cdt = getData[i].Cdt.ToString("yyyy-MM-dd");
                    //item.udt = getData[i].Udt.ToString("yyyy-MM-dd");
                    retLst.Add(item);
                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }

        }


        /// <summary>
        /// 取得所有family数据的list(按Family列的字母序排序)
        /// </summary>
        /// <returns></returns>
        public IList<FamilyDef> GetFamilyInfoList()
        {
            return new List<IMES.DataModel.FamilyDef>();
           //try
            //{
            //    IFamilyRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
            //    IList<Family> getData = itemRepository.GetFamilyList();
            //    for (int i = 0; i < getData.Count; i++)
            //    {
            //        FamilyDef item = new FamilyDef();
            //        item.CustomerID = getData[i].Customer;
            //        item.Descr = getData[i].Description;
            //        item.Family = getData[i].FamilyName;
            //        item.Editor = getData[i].Editor;
            //        retLst.Add(item);
            //    }
            //    return retLst;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        /// <summary>
        ///  取得一条family的记录数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public FamilyDef GetFamily(String id)
        {
            FamilyDef ret = new FamilyDef();

            ret.Family = "Family0";
            ret.Descr = "Descr0";
            return ret;

            //try
            //{
            //    IFamilyRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
            //    ret = itemRepository.Find(id);
            //    return ret;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}

        }

        /// <summary>
        /// 保存一条family的记录数据(Add)
        /// </summary>
        /// <param name="Object"></param>
        public void AddFamily(FamilyDef obj)
        {
            FamilyDef familyObj = obj;

            try
            {
                IFamilyRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();

                UnitOfWork uow = new UnitOfWork();
                Family item = new Family(obj.Family,obj.Descr,obj.CustomerID);
                item.FamilyName = obj.Family;
                item.Customer = obj.CustomerID;
                item.Description = obj.Descr;
                item.Editor = obj.Editor;
                item.Cdt = DateTime.Now;
                item.Udt = DateTime.Now;
                itemRepository.Add(item, uow);
                uow.Commit();
            }
            catch (Exception)
            {
                throw;
            }


        }


        /// <summary>
        /// 保存一条family的记录数据(update), 若Family名称为其他存在的Family的名称相同，则提示业务异常
        /// </summary>
        /// <param name="Object"></param>
        public void UpdateFamily(FamilyDef obj, String oldFamilyId)
        {
            try
            {
                IFamilyRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
                Family item = new Family(obj.Family, obj.Descr, obj.CustomerID);
                item.FamilyName = obj.Family;
                item.Customer = obj.CustomerID;
                item.Description = obj.Descr;
                item.Editor = obj.Editor;
                item.Udt = DateTime.Now;
                itemRepository.ChangeFamily(item, oldFamilyId);
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// "删除一条family的记录数据
        /// </summary>
        /// <param name="?"></param>
        public void DeleteFamily(FamilyDef obj)
        {
            try
            {
                IFamilyRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
                UnitOfWork uow = new UnitOfWork();
                Family item = itemRepository.Find(obj.Family);
                itemRepository.Remove(item, uow);
                uow.Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //SELECT [CustomerID]
        //  FROM [IMES_GetData_Datamaintain].[dbo].[Family]
        //where [Family]='family'

        /// <summary>
        /// 取得family对应的customer，若没有，返回空字串
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        public string GetCustomerByFamily(string family)
        {
            string result = "";
            string param=family.Trim();
            if(param=="")
            {
                return result;
            }

            try
            {
                IFamilyRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
                Family item= itemRepository.FindFamily(param);
                if (item == null)
                {
                    return result;
                }
                result = item.Customer;
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        #endregion

        #region Implementation of IWarranty
        /// <summary>
        /// 取得Customer Id下的Warranty数据的list(按Description栏位排序)
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public IList<WarrantyDef> GetWarrantyList(String customerId)
        {
            List<WarrantyDef> result=new List<WarrantyDef> ();

            try
            {
                IWarrantyRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IWarrantyRepository>();
                IList<Warranty> getData = itemRepository.GetWarrantyList(customerId);
                for (int i = 0; i < getData.Count; i++)
                {
                    WarrantyDef item = new WarrantyDef();
                    item.Customer = Null2String(getData[i].Customer);
                    item.DateCodeType = Null2String(getData[i].DateCodeType);
                    item.Descr = Null2String(getData[i].Descr);
                    item.Editor = Null2String(getData[i].Editor);
                    item.id = Null2String(getData[i].Id);
                    item.ShipTypeCode = Null2String(getData[i].ShipTypeCode);
                    item.ShipTypeCodeText = item.ShipTypeCode;
                    item.Type = Null2String(getData[i].Type);
                    item.TypeText = item.Type;
                    item.WarrantyCode = Null2String(getData[i].WarrantyCode);
                    item.WarrantyFormat = Null2String(getData[i].WarrantyFormat);
                    item.WarrantyFormatText = item.WarrantyFormat;

                    //item.cdt = getData[i].Cdt.ToString("yyyy-MM-dd");
                    //item.udt = getData[i].Udt.ToString("yyyy-MM-dd");


                    if (getData[i].Cdt == DateTime.MinValue)
                    {
                        item.cdt = "";
                    }
                    else
                    {
                        item.cdt = ((System.DateTime)getData[i].Cdt).ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    if (getData[i].Udt == DateTime.MinValue)
                    {
                        item.udt = "";
                    }
                    else
                    {
                        item.udt = ((System.DateTime)getData[i].Udt).ToString("yyyy-MM-dd HH:mm:ss");
                    }


                    result.Add(item);
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 取得一条Warranty的记录数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public WarrantyDef GetWarranty(String id)
        {
            WarrantyDef ret = new WarrantyDef();
            return ret;

        }

        /// <summary>
        /// 保存一条Warranty的记录数据(Add)
        /// </summary>
        /// <param name="?"></param>
        public string AddWarranty(WarrantyDef obj)
        {

            IWarrantyRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IWarrantyRepository>();
            if (itemRepository.GetExistWarranty(obj.Customer, obj.Descr).Rows.Count > 0)
            {
                //throw new ApplicationException("Already exist warranty with the same description under the customer in database.");
                List<string> erpara = new List<string>();
                FisException ex;
                ex = new FisException("DMT001", erpara);
                throw ex;
            }

            int id = 0;
            try
            {
                UnitOfWork uow = new UnitOfWork();
                Warranty item = new Warranty
                    (id, 
                    obj.Customer,
                    obj.Type,
                    obj.DateCodeType,
                    obj.WarrantyFormat,
                    obj.ShipTypeCode,
                    obj.WarrantyCode,
                    obj.Descr,
                    obj.Editor,
                    DateTime.Now,
                    DateTime.Now
                    );
  
                itemRepository.Add(item, uow);
                uow.Commit();
                id=item.Id;
            }
            catch (Exception)
            {
                throw;
            }
            return id.ToString();

        }
        /// <summary>
        ///  保存一条Warranty的记录数据 (update)
        /// </summary>
        /// <param name="Object"></param>
        public void UpdateWarranty(WarrantyDef obj, String oldWarrantyId)
        {

            IWarrantyRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IWarrantyRepository>();
            if (itemRepository.GetExistWarranty(obj.Customer, obj.Descr, Int32.Parse(obj.id)).Rows.Count > 0)
            {
                //throw new ApplicationException("Already exist warranty with the same description under the customer in database.");
                List<string> erpara = new List<string>();
                FisException ex;
                ex = new FisException("DMT001", erpara);
                throw ex;
            }

            try
            {              
                UnitOfWork uow = new UnitOfWork();
                Warranty item = itemRepository.Find(Int32.Parse(obj.id));
                item.Customer = obj.Customer;
                item.Type = obj.Type;
                item.DateCodeType = obj.DateCodeType;
                item.WarrantyFormat = obj.WarrantyFormat;
                item.ShipTypeCode = obj.ShipTypeCode;
                item.WarrantyCode = obj.WarrantyCode;
                item.Descr = obj.Descr;
                item.Editor = obj.Editor;
                item.Udt = DateTime.Now;
                itemRepository.Update(item, uow);
                uow.Commit();
            }
            catch (Exception)
            {
                throw;
            }

        }
        /// <summary>
        /// 删除一条Warranty数据
        /// </summary>
        /// <param name="?"></param>
        public void DeleteWarranty(String id)
        {
            try
            {
                IWarrantyRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IWarrantyRepository>();
                UnitOfWork uow = new UnitOfWork();
                Warranty item = itemRepository.Find(Int32.Parse(id));
                               
                itemRepository.Remove(item, uow);
                uow.Commit();
            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion

        #region Implementation of IMACRange
        /// <summary>
        /// 取得MACRange List列表（按“Code”列的字母序排序）
        /// </summary>
        /// <returns></returns>
        public IList<MACRangeDef> GetMACRangeList()
        {
            //List<MACRangeDef> result = new List<MACRangeDef>();
            //MACRangeDef item1 = new MACRangeDef();
            //item1.id = 1;
            //item1.BegNo = "BeginNo1";
            //item1.EndNo = "EndNo1";
            //item1.Editor = "aa";
            //item1.Code = "Code1";
            //item1.Status = "R";
            //item1.StatusText = "Created";
            //result.Add(item1);

            //MACRangeDef item2 = new MACRangeDef();
            //item2.id = 2;
            //item2.BegNo = "BeginNo2";
            //item2.EndNo = "EndNo2";
            //item2.Editor = "aa";
            //item2.Code = "code2";
            //item2.Status = "A";
            //item2.StatusText = "Active";
            //result.Add(item2);

            //MACRangeDef item3 = new MACRangeDef();
            //item3.id = 3;
            //item3.BegNo = "BeginNo3";
            //item3.EndNo = "EndNo3";
            //item3.Editor = "aa";
            //item3.Code = "code3";
            //item3.Status = "C";
            //item3.StatusText = "Closed";
            //result.Add(item3);

            ////R:Created; A:Active; C:Closed  CmbMacRangeStatus

            //return result;

            List<MACRangeDef> retLst = new List<MACRangeDef>();
            try
            {
                INumControlRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository>();
                IList<MACRange> result = itemRepository.GetMACRangeList();
                for (int i = 0; i < result.Count; i++)
                {
                    MACRangeDef item = new MACRangeDef();
                    item.BegNo = result[i].BegNo;
                    item.EndNo = result[i].EndNo;
                    item.Code = result[i].Code;
                    item.Editor = result[i].Editor;
                    item.id = result[i].ID.ToString();
                    item.Status = result[i].Status;
                    if (item.Status == MACRANGE_STATUS_R)
                    {
                        item.StatusText = MACRANGE_STATUS_R_TEXT;
                    }
                    else if (item.Status == MACRANGE_STATUS_A)
                    {
                        item.StatusText = MACRANGE_STATUS_A_TEXT;
                    }
                    else
                    {
                        item.StatusText = MACRANGE_STATUS_C_TEXT;
                    }

                    //item.Udt = result[i].Udt.ToString("yyyy-MM-dd");
                    //item.Cdt = result[i].Cdt.ToString("yyyy-MM-dd");


                    if (result[i].Cdt == DateTime.MinValue)
                    {
                        item.Cdt = "";
                    }
                    else
                    {
                        item.Cdt = ((System.DateTime)result[i].Cdt).ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    if (result[i].Udt == DateTime.MinValue)
                    {
                        item.Udt = "";
                    }
                    else
                    {
                        item.Udt = ((System.DateTime)result[i].Udt).ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    retLst.Add(item);

                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 取得一条MACRang的记录数据
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public MACRangeDef GetMACRange(String id)
        {
            //MACRangeDef item2 = new MACRangeDef();
            //item2.id = "1";
            //item2.BegNo = "BeginNo2";
            //item2.EndNo = "EndNo2";
            //item2.Editor = "aa";
            //item2.Code = "Code2";
            //item2.Status = "s2";
           
            //return item2;

            MACRangeDef ret = new MACRangeDef();
            try
            {
                INumControlRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository>();
                MACRange item = itemRepository.GetMACRange(Int32.Parse(id));
                ret.BegNo = item.BegNo;
                ret.Code = item.Code;
                ret.Editor = item.Editor;
                ret.EndNo = item.EndNo;
                ret.id = item.ID.ToString();
                ret.Status = item.Status;
                
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 保存一条MACRang的记录数据(Add)
        /// </summary>
        /// <param name="Object"></param>
        /// <returns></returns>
        public string AddMACRange(MACRangeDef obj)
        {
            //MACRangeDef MACRangeObj = obj;
            try
            {
                INumControlRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository>();

                //UnitOfWork uow = new UnitOfWork();
                MACRange item = new MACRange(
                    0,
                    obj.Code,
                    obj.BegNo,
                    obj.EndNo,
                    obj.Status,
                    obj.Editor,
                    DateTime.Now,
                    DateTime.Now
                    );
                itemRepository.AddMACRange(item);

                return item.ID.ToString();
                //uow.Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 保存一条MACRang的记录数据(update)
        /// </summary>
        /// <param name="Object"></param>
        /// <returns></returns>
        public void UpdateMACRange(MACRangeDef obj, String oldMACRangeId)
        {
            try
            {
                INumControlRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository>();

                UnitOfWork uow = new UnitOfWork();
                MACRange item = itemRepository.GetMACRange(Int32.Parse(obj.id));
                //if (item.Status != "R" && (item.BegNo != obj.BegNo || item.EndNo != obj.EndNo))
                //{
                //    List<string> erpara = new List<string>();
                //    FisException ex;
                //    ex = new FisException("MDT001", erpara);
                //    throw ex;
                //}

                item.Code= obj.Code;
                item.BegNo=obj.BegNo;
                item.EndNo=obj.EndNo;
                //item.Status=obj.Status;
                item.Editor=obj.Editor;
                item.Udt= DateTime.Now;

                itemRepository.UpdateMACRange(item);
                uow.Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除一条MACRang数据
        /// </summary>
        /// <param name="?"></param>
        public void DeleteMACRange(String id)
        {
            try
            {
                INumControlRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository>();
                //UnitOfWork uow = new UnitOfWork();
                //MACRange item = new MACRange(
                //                    id, "", "", "", "",
                //                    DateTime.Now(),
                //                    DateTime.Now()
                //                    );

                itemRepository.DeleteMACRange(Int32.Parse(id));
                //uow.Commit();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public MACInfoDef GetMACInfo(string code)
        {
            MACInfoDef item = new MACInfoDef();
            string param = Null2String(code);
            if(param == "")
            {
                return item;
            }

            try
            {
                INumControlRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository>();
                long totalNum = itemRepository.GetMACRangeTotalByCode(param);
                long usedNum = itemRepository.GetMACRangeTotalUsedByCode(param);
                long leftNum = totalNum -usedNum;

                item.MACRangeCode = param;
                item.MACTotal = totalNum.ToString();
                item.MACUsed = usedNum.ToString();
                item.MACleft = leftNum.ToString();

                DataTable HDCPQueryResult = itemRepository.GetHDCPQuery();
                item.HDCPQuery = "0";
                if (HDCPQueryResult.Rows.Count > 0)
                {
                    item.HDCPQuery = HDCPQueryResult.Rows[0][0].ToString();
                }

            }
            catch (Exception)
            {
                throw;
            }
            return item;
        }

        #endregion


        #region Implementation of ISelectData
        //        SELECT     Station.Station 
        //FROM         CheckItem INNER JOIN
        //Station ON CheckItem.Mode = Station.OperationObject
        //WHERE CheckItem.ID=checkItemId order by Station
        //若新选项不是“Add a new Check Item”，则Station下拉列表中选项换为与新当前CheckItem对应Mode一致（Station.OperationObject）的Station。
        public IList<SelectInfoDef> GetStationListByCheckedItemID(string checkItemId)
        {
            
            List<SelectInfoDef> result = new List<SelectInfoDef>();

            if (Null2String(checkItemId) == "")
            {
                return result;
            }

            int checkItem = Int32.Parse(checkItemId);


            return result;
        }

        //取customer下的family list
        public IList<SelectInfoDef> GetCustomerFamilyList(String customer)
        {
            List<SelectInfoDef> result = new List<SelectInfoDef>();

            return result;
        }

        //取得CheckItem下拉列表
        //SELECT [ID],[ItemName] FROM [IMES_GetData].[dbo].[CheckItem] WHERE Customer='customer' order by [ItemName]
        public IList<SelectInfoDef> GetItemNameListByCustomer(string customer)
        {
            List<SelectInfoDef> result = new List<SelectInfoDef>();

            SelectInfoDef item = new SelectInfoDef();
            item.Text = "text1";
            item.Value = "1";

            result.Add(item);
            return result;
        }

        //PartType列表，
        //SELECT [PartType]
        //       FROM [IMES_GetData].[dbo].[PartType] ORDER BY [PartType]
        //选项包括系统中所有的Part Type 
        //PartTypeAttribute
        public IList<SelectInfoDef> GetPartTypeList()
        {
            List<SelectInfoDef> result = new List<SelectInfoDef>();

            return result;
        }
        //Value Type列表

        //SELECT [Code]      
        //  FROM [IMES_GetData].[dbo].[PartTypeAttribute]
        //where PartType='partType' ORDER BY [Code]   

        //Change：1、ValueType下拉列表的选项全部换为与新选PartType相关的属性名称，其第一个选项被选取。
        //ValueType 选项包括PartNo、Descr、CustPartNo、FruNo、Vendor、IECVersion以及当前Part Type的所有Attribute
        public IList<SelectInfoDef> GetValueTypeList(string partType)
        {
            List<SelectInfoDef> result = new List<SelectInfoDef>();

            return result;
        }


        //取得Model列表, 取Family表和Model表关联？ CustomerID字长80？
        //问题：空项在前还是在后，第一个被选取, 是
        //Model 下拉列表的选项除空项外全部换为与新选Customer对应的Model，其第一个选项被选取；

        //SELECT distinct Model.Model
        //FROM  Family INNER JOIN
        //Model ON Family.Family = Model.Family WHERE CustomerID ='customer' 
        //order by Model
        public IList<SelectInfoDef> GetCustomerModelList(string customer)
        {
            List<SelectInfoDef> result = new List<SelectInfoDef>();

            return result;
        }

        //取得station的列表
        //选项包括所有的Station，按创建时间排序
        //SELECT [Station]
        //        FROM [IMES_GetData_Datamaintain].[dbo].[Station]
        //ORDER BY [Cdt]
        public IList<SelectInfoDef> GetStationList()
        {
            List<SelectInfoDef> result = new List<SelectInfoDef>();

            return result;
        }

        //取得当前客户相关的PartType
        // PartType下拉列表框中选项换为PartCheck表中当前Customer的所有相关Part Type。

        //SELECT [PartType] FROM [IMES_GetData_Datamaintain].[dbo].[PartCheck] WHERE [Customer]='customer' order by [PartType]
        public IList<SelectInfoDef> GetCustomerPartTypeList(String customer)
        {
            List<SelectInfoDef> result = new List<SelectInfoDef>();

            return result;
        }

        #endregion

        public static String Null2String(Object _input)
        {
            if (_input == null)
            {
                return "";
            }
            return _input.ToString().Trim();
        }

        /// <summary>
        /// Bartender打印时,根据存储过程名字和参数执行存储过程,获取主Bartender Name/Value
        /// </summary>
        /// <param name="currentSPName">存储过程名字</param>
        /// <param name="parameterKeys">存储过程需要的参数名字</param>
        /// <param name="parameterValues">存储过程需要的参数值</param>
        /// <returns>主bat名称</returns>
        public IList<NameValueDataTypeInfo> GetBartenderNameValueList(string currentSPName, List<string> parameterKeys, List<List<string>> parameterValues)
        {
            ILabelTypeRepository lblTypeRepository = RepositoryFactory.GetInstance().GetRepository<ILabelTypeRepository, LabelType>();
            return lblTypeRepository.GetBartendarNameValueInfo(currentSPName, parameterKeys, parameterValues);
        }
		
		private static CommonImpl _Instance = null;
		
		public static CommonImpl GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new CommonImpl();
            }
            return _Instance;
        }
		
		public string GetValueFromSysSetting(string name)
        {
            IPartRepository PartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<string> valueList = PartRepository.GetValueFromSysSettingByName(name);
            if (valueList.Count == 0)
            { throw new FisException("PAK095", new string[] { name }); }
            return valueList[0].Trim(); //
        
        }

        public IList<ConstValueTypeInfo> GetConstValueTypeListByType(string type)
        {
            IList<ConstValueTypeInfo> retLst = new List<ConstValueTypeInfo>();
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            try
            {
                if (!String.IsNullOrEmpty(type))
                {
                    retLst = partRep.GetConstValueTypeList(type);
                }
                return retLst;
            }
            catch (Exception)
            {
                throw;
            }

        }

    }

}
