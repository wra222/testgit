using System;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;
using IMES.DataModel;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.FisObject.Common.Line;
using System.Data;


namespace IMES.Maintain.Implementation
{
    public class QCRatioManager : MarshalByRefObject,IQCRatio
    {
        //const int BOM_MAX_DEEP_LEVEL=18;
        #region Implementation of IQCRatio
        //        /////
        //取得某customer下QCRatioList 
        //SELECT [IMES_FA].[dbo].[QCRatio].[Family]
        //      ,[IMES_FA].[dbo].[QCRatio].[QCRatio]
        //      ,[IMES_FA].[dbo].[QCRatio].[EOQCRatio]
        //  FROM [IMES_FA].[dbo].[QCRatio] inner join 
        //[AT_IMES_GetData].[dbo].[Family] on [IMES_FA].[dbo].[QCRatio].[Family]=[AT_IMES_GetData].[dbo].[Family].[Family]
        //WHERE [AT_IMES_GetData].[dbo].[Family].[CustomerID]='customer' order by [Family]

        //change to:
         //SELECT A.[Family],A.[QCRatio],A.[EOQCRatio],A.Editor,A.Cdt,A.Udt FROM [QCRatio] AS A INNER JOIN
         //(
         //SELECT Family FROM [Family] WHERE [CustomerID]='customer' 
         //UNION 
         //SELECT Customer AS [Family] FROM Customer WHERE Customer='customer'
         //) AS C ON A.[Family]=C.[Family] ORDER BY [Family]
        public DataTable GetQCRatioList(string customer)
        {
            DataTable retLst = new DataTable();
            try
            {
                IFamilyRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
                //retLst = itemRepository.GetQCRatioList(customer);                
                retLst = itemRepository.GetQCRatioList2(customer);                
            }
            catch (Exception)
            {
                throw;
            }

            return retLst;
        }

        //若Family为空，则将当前Customer保存到记录中的Family栏位


        //GetExistQCRatio(String customer, string QCRatioId)

        //SELECT [IMES_FA].[dbo].[QCRatio].[Family]
        //  FROM [IMES_FA].[dbo].[QCRatio] inner join 
        //[IMES_GetData].[dbo].[Family] on [IMES_FA].[dbo].[QCRatio].[Family]=[IMES_GetData].[dbo].[Family].[Family]
        //WHERE [IMES_GetData].[dbo].[Family].[CustomerID]='customer' 
        //AND [IMES_FA].[dbo].[QCRatio].[Family]='QCRatioId'

        //change to:
        //     SELECT [IMES_FA_Datamaintain].[dbo].[QCRatio].[Family]
        //          FROM [IMES_FA_Datamaintain].[dbo].[QCRatio] inner join
        //    (select [IMES_GetData_Datamaintain].[dbo].[Family].Family from [IMES_GetData_Datamaintain].[dbo].[Family]
        //        WHERE [IMES_GetData_Datamaintain].[dbo].[Family].[CustomerID]='customer' union 
        //        select [IMES_GetData_Datamaintain].dbo.Customer.Customer AS [Family] from [IMES_GetData_Datamaintain].dbo.Customer
        //        WHERE [IMES_GetData_Datamaintain].dbo.Customer.Customer='customer'
        //        ) AS C
        //on [IMES_FA_Datamaintain].[dbo].[QCRatio].[Family]=C.[Family]
        //        WHERE [IMES_FA_Datamaintain].[dbo].[QCRatio].[Family]='QCRatioId'
        //change to:
        //SELECT [Family]      
        //  FROM [QCRatio]
        //WHERE [QCRatio].[Family]=@ QCRatioFamily

        public string AddQCRatio(QCRatioDef item)
        {
            String result = "";
            try
            {
                IFamilyRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();

                DataTable exists=itemRepository.GetExistQCRatio(item.Family);
                if (exists != null && exists.Rows.Count > 0)
                {
                    //已经存在具有相同Customer和Family的QCRatio记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT007", erpara);
                    throw ex;

                }
                int intQCRatio;
                int intEOQCRatio;
                int intPAQCRatio;
                int intRPAQCRatio;

                if (item.QCRatio == "")
                {
                    intQCRatio = Int32.MinValue;
                }
                else
                {
                    intQCRatio = Int32.Parse(item.QCRatio);
                }

                if (item.EOQCRatio == "")
                {
                    intEOQCRatio = Int32.MinValue;
                }
                else
                {
                    intEOQCRatio = Int32.Parse(item.EOQCRatio);
                }

                if (item.PAQCRatio  == "")
                {
                    intPAQCRatio = Int32.MinValue;
                }
                else
                {
                    intPAQCRatio = Int32.Parse(item.PAQCRatio);
                }
                if (item.RPAQCRatio == "")
                {
                    intRPAQCRatio = Int32.MinValue;
                }
                else
                {
                    intRPAQCRatio = Int32.Parse(item.RPAQCRatio);
                }

                QCRatio itemQCRatio=new QCRatio(
                    item.Family,
                    intQCRatio,
                    intEOQCRatio,
                    intPAQCRatio,
                    item.Editor,
                    DateTime.Now,
                    DateTime.Now,
                    intRPAQCRatio
                    );             
                
                itemRepository.AddQCRatio(itemQCRatio);
                result = itemQCRatio.Family;
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        //若Family为空，则将当前Customer保存到记录中的Family栏位
        public string SaveQCRatio(QCRatioDef item, string oldId)
        {
            String result = "";
            try
            {
                IFamilyRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();

                DataTable exists = itemRepository.GetExistQCRatio(item.Family);
                if (exists != null && exists.Rows.Count > 0 && oldId != item.Family)
                {
                    //已经存在具有相同Customer和Family的QCRatio记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT007", erpara);
                    throw ex;

                }
                int intQCRatio;
                int intEOQCRatio;
                int intPAQCRatio;
                int intRPAQCRatio;

                if (item.QCRatio == "")
                {
                    intQCRatio = Int32.MinValue;
                }
                else
                {
                    intQCRatio = Int32.Parse(item.QCRatio);
                }

                if (item.EOQCRatio == "")
                {
                    intEOQCRatio = Int32.MinValue;
                }
                else
                {
                    intEOQCRatio = Int32.Parse(item.EOQCRatio);
                }


                if (item.PAQCRatio == "")
                {
                    intPAQCRatio = Int32.MinValue;
                }
                else
                {
                    intPAQCRatio = Int32.Parse(item.PAQCRatio);
                }

                if (item.RPAQCRatio == "")
                {
                    intRPAQCRatio = Int32.MinValue;
                }
                else
                {
                    intRPAQCRatio = Int32.Parse(item.RPAQCRatio);
                }

                QCRatio itemQCRatio = new QCRatio(
                    item.Family,
                    intQCRatio,
                    intEOQCRatio,
                    intPAQCRatio,
                    item.Editor,
                    DateTime.Now,
                    DateTime.Now,
                    intRPAQCRatio
                    );

                itemRepository.UpdateQCRatio(itemQCRatio, oldId);
                result = itemQCRatio.Family;
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public void DeleteQCRatio(QCRatioDef item)
        {
            try
            {
                IFamilyRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
                QCRatio itemQCRatio = new QCRatio(
                    item.Family,
                    0,
                    0,
                    0,
                    "",
                    DateTime.Now,
                    DateTime.Now,
                    0);

                itemRepository.DeleteQCRatio(itemQCRatio);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        private static String Null2String(Object _input)
        {
            if (_input == null)
            {
                return "";
            }
            return _input.ToString().Trim();
        }

        //Add2012/06/12 UCupdate Add line info
        //-----------------------------------------------------------------
        public IList<SelectInfoDef> GetCustomerFamilyListAddline(String customerId)
        {

            String customer = Null2String(customerId);
            List<SelectInfoDef> result = new List<SelectInfoDef>();
            if (customer == "")
            {
                return result;
            }

            try
            {
                IFamilyRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
                ILineRepository lineRepository = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();
        
                IList<string> linelst = new List<string>();
                string[] lineType = new string[2];
                lineType[0] = "FA";
                lineType[1] = "PAK";
                linelst = lineRepository.GetLinePrefixListByStages(lineType,customer);
                foreach (string temp in linelst)
                {
                    SelectInfoDef item = new SelectInfoDef();
                    item.Text = temp;
                    item.Value = temp;
                    result.Add(item);
                }
                
                IList<Family> getData = itemRepository.GetFamilyList(customer);
                for (int i = 0; i < getData.Count; i++)
                {
                    SelectInfoDef item = new SelectInfoDef();
                    string getValue = getData[i].FamilyName.Trim();
                    item.Text = getValue;
                    item.Value = getValue;
                    result.Add(item);
                }

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
        //------------------------------------------------------------------
        public IList<SelectInfoDef> GetCustomerFamilyList(String customerId)
        {

            String customer = Null2String(customerId);
            List<SelectInfoDef> result = new List<SelectInfoDef>();
            if (customer == "")
            {
                return result;
            }

            try
            {
                IFamilyRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
                IList<Family> getData = itemRepository.GetFamilyList(customer);
                for (int i = 0; i < getData.Count; i++)
                {
                    SelectInfoDef item = new SelectInfoDef();
                    string getValue = getData[i].FamilyName.Trim();
                    item.Text = getValue;
                    item.Value = getValue;
                    result.Add(item);
                }

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public IList<SelectInfoDef> GetModelListByFamily(string Family)
        {
            Family = Null2String(Family);
            List<SelectInfoDef> result = new List<SelectInfoDef>();
            if (Family == "")
            {
                return result;
            }

            try
            {
                IFamilyRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
                DataTable getData = itemRepository.GetModelListByFamily(Family);
                for (int i = 0; i < getData.Rows.Count; i++)
                {
                    SelectInfoDef item = new SelectInfoDef();
                    string getValue = getData.Rows[i][0].ToString().Trim();
                    item.Text = getValue;
                    item.Value = getValue;
                    result.Add(item);
                }

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

    }
}