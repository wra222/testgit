using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.PCA.MBModel;
using IMES.FisObject.Common.Line;
using IMES.FisObject.PCA.MBMO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.Misc;
using IMES.FisObject.Common.MO;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Warranty;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.NumControl;
using System.Data;

namespace IMES.Maintain.Implementation
{
    
    public class LineManager : MarshalByRefObject,ILine
    {

        //SELECT [Line]
        //      ,[Descr]
        //      ,[Editor]
        //      ,[Cdt]
        //      ,[Udt]
        //      ,[CustomerID]
        //      ,[Stage]
        //  FROM [IMES_GetData_Datamaintain].[dbo].[Line]
        //where [CustomerID]='CustomerID' AND [Stage]='stage'
        //order by [Line]
        public DataTable GetLineInfoList(string customer, string stage)
        {
            DataTable retLst = new DataTable();
            try
            {
                ILineRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository>();
                retLst = itemRepository.GetLineByStage(customer,stage);
            }
            catch (Exception)
            {
                throw;
            }

            return retLst;
        }

        private static String Null2String(Object _input)
        {
            if (_input == null)
            {
                return "";
            }
            return _input.ToString().Trim();
        }
        //SELECT [Stage]
        //  FROM [IMES_GetData_Datamaintain].[dbo].[Stage]
        //order by [Stage]

        public List<SelectInfoDef> GetStageList()
        {
            List<SelectInfoDef> result = new List<SelectInfoDef>();

            try
            {
                ILineRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository>();
                DataTable getData = itemRepository.GetStageList();
                for (int i = 0; i < getData.Rows.Count; i++)
                {
                    SelectInfoDef item = new SelectInfoDef();
                    item.Text = Null2String(getData.Rows[i][0]);
                    item.Value = item.Text;
                    result.Add(item);
                }

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }



        //DELETE FROM [Line]
        //      WHERE Line='line'
        public void DeleteLine(string line)
        {
            try
            {
                ILineRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository>();
                IUnitOfWork uow = new UnitOfWork();
                itemRepository.DeleteLineDefered(uow, line);
                uow.Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //IF EXISTS(
        //SELECT [Line]
        //        FROM [IMES_GetData_Datamaintain].[dbo].[Line]
        //where [Line]='line'
        //)
        //set @return='True'
        //ELSE
        //set @return='False'
        //Boolean IsExistLine(string line);

        public string AddLine(LineDef item)
        {
            String result = "";
            try
            {
                ILineRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository>();

                Boolean isExist = itemRepository.IsExistLine(item.line);
                if (isExist==true)
                {
                    //已经存在具有相同Line的记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT042", erpara);
                    throw ex;

                }

                isExist = itemRepository.IsExistLineDescr(item.descr);
                if (isExist == true)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT146", erpara);
                    throw ex;
                }

                Line itemNew = new Line();
                itemNew.CustomerId = item.customerID;
                itemNew.StageId = item.stage;
                itemNew.Id = item.line;

                itemNew.Descr = item.descr;
                itemNew.Editor = item.editor;
                itemNew.Udt = DateTime.Now;
                itemNew.Cdt = DateTime.Now;

                itemRepository.AddLine(itemNew);
                result = itemNew.Id;
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }


        public string UpdateLine(LineDef item, string oldLineId)
        {
            String result = "";
            try
            {
                ILineRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository>();
                Boolean isExist = itemRepository.IsExistLineDescrExceptLine(item.descr,item.line);
                if (isExist == true)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT146", erpara);
                    throw ex;
                }

                Line itemNew = new Line();
                itemNew.CustomerId=item.customerID;
                itemNew.StageId=item.stage;
                itemNew.Id=item.line;

                itemNew.Descr=item.descr;
                itemNew.Editor=item.editor;
                itemNew.Udt=DateTime.Now;

                itemRepository.UpdateLine(itemNew, oldLineId);
                result = itemNew.Id;
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }


        public void SaveLineEx(LineExinfo item)
        {
            try
            {
                ILineRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository>();
                
                Line line = itemRepository.Find(item.line);
                IUnitOfWork work = new UnitOfWork();
                if (line == null)
                {
                    line = new Line();
                    LineEx itemEx = new LineEx();


                    line.CustomerId = item.customerID;
                    line.Id = item.line;
                    line.StageId = item.stage;
                    line.Descr = item.descr;
                    line.Editor = item.editor;
                    itemEx.Line = item.line;
                    itemEx.AliasLine = item.AliasLine;
                    itemEx.AvgManPower = item.AvgManPower;
                    itemEx.AvgSpeed = item.AvgSpeed;
                    itemEx.AvgStationQty = item.AvgStationQty;
                    itemEx.IEOwner = item.IEOwner;
                    itemEx.Owner = item.Owner;
                    itemEx.Editor = item.editor;

                    line.LineEx = itemEx;
                    itemRepository.Add(line, work);
                    //itemRepository.AddLine(Line);
                    //add
                }
                else
                {
                   
                    line.CustomerId = item.customerID;
                    line.StageId = item.stage;
                    line.Descr = item.descr;

                    if (line.LineEx == null)
                    {
                        line.LineEx = new LineEx
                        {
                            Line = item.line,
                            AliasLine = item.AliasLine
                        };
                    }
                    else
                    {
                        line.LineEx.AliasLine = item.AliasLine;
                    }

                    line.LineEx.AvgManPower = item.AvgManPower;
                    line.LineEx.AvgSpeed = item.AvgSpeed;
                    line.LineEx.AvgStationQty = item.AvgStationQty;
                    line.LineEx.IEOwner = item.IEOwner;
                    line.LineEx.Owner = item.Owner;
                    line.LineEx.Editor = item.editor;
                    itemRepository.Update(line, work);
                    //update
                }
                work.Commit();
                //Boolean isExist = itemRepository.IsExistLine(item.line);
                //if (isExist == true)
                //{
                //    //已经存在具有相同Line的记录
                //    List<string> erpara = new List<string>();
                //    FisException ex;
                //    ex = new FisException("DMT042", erpara);
                //    throw ex;

                //}

                //isExist = itemRepository.IsExistLineDescr(item.descr);
                //if (isExist == true)
                //{
                //    List<string> erpara = new List<string>();
                //    FisException ex;
                //    ex = new FisException("DMT146", erpara);
                //    throw ex;
                //}

                //Line itemNew = new Line();
                //itemNew.CustomerId = item.customerID;
                //itemNew.StageId = item.stage;
                //itemNew.Id = item.line;

                //itemNew.Descr = item.descr;
                //itemNew.Editor = item.editor;
                //itemNew.Udt = DateTime.Now;
                //itemNew.Cdt = DateTime.Now;

                //itemRepository.AddLine(itemNew);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
