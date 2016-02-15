using System;
using System.Collections.Generic;
using IMES.Maintain.Interface.MaintainIntf;
using System.Data;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.Station;
using IMES.FisObject.PAK.Pallet;
//ITC-1361-0079

namespace IMES.Maintain.Implementation
{

    public class LightNoManager : MarshalByRefObject, ILightNo
    {

        public const string KITTING_TYPE_FA_KITTING="FA Kitting";

        //1）取KittingCode表格
          //select distinct(Left(rtrim(Line),1)) as Code, '' as Descr, 'Yes' as IsLine 
          //from Line (nolock) where Stage=@stage 
          //union select rtrim(Family) as Code, Descr, '' as IsLine 
          //from Family (nolock) order by Descr, Code

        public DataTable GetKittingCodeListFromLine(String stage)
        {

            DataTable retLst = new DataTable();
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                retLst = itemRepository.GetKittingCodeListFromLine(stage);
            }
            catch (Exception)
            {
                throw;
            }
            return retLst;

        }

        //2）取KittingCode表格
        //SELECT Code AS KittingCode
        //,[Descr] 
        //'' 
        //FROM LabelKitting 
        //WHERE Type='type'
        //ORDER BY KittingCode  //'FA Label','FA Label'情况下

        public DataTable GetKittingCodeList(String type)
        {

            DataTable retLst = new DataTable();
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                retLst = itemRepository.GetKittingCodeList(type);
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


        //3)取PDLine列表
        //'FA', 'PAK'情况下
        //select distinct Left(rtrim(Line),1) as Code from Line (nolock) where Stage=@stage order by Code
        public IList<SelectInfoDef> GetPdLineList(String stage)
        {
            stage = Null2String(stage);
            List<SelectInfoDef> result = new List<SelectInfoDef>();
            if (stage == "")
            {
                return result;
            }

            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                DataTable getData = itemRepository.GetPdLineListForLightNo(stage);
                for (int i = 0; i < getData.Rows.Count; i++)
                {
                    SelectInfoDef item = new SelectInfoDef();
                    string getValue = getData.Rows[i][0].ToString().Trim();
                    string getText = getData.Rows[i][0].ToString().Trim();
                    item.Text = getText;
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

        public IList<SelectInfoDef> GetLightNoStationList(String type)
        {
            type = Null2String(type);
            List<SelectInfoDef> result = new List<SelectInfoDef>();
            if (type == "")
            {
                return result;
            }

            try
            {
                IStationRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();
                DataTable getData = itemRepository.GetLightNoStationList(type);
                for (int i = 0; i < getData.Rows.Count; i++)
                {
                    SelectInfoDef item = new SelectInfoDef();
                    string getValue = getData.Rows[i][0].ToString().Trim();
                    string getText = getData.Rows[i][1].ToString().Trim();
                    item.Text = getText;
                    item.Value = getText;// getValue;
                    result.Add(item);
                }

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }


        

        //4)取lightNo列表信息
        //select distinct b.Code,b.PartNo,b.Tp,convert(int,b.LightNo) as LightNo,Qty,Sub,Safety_Stock,Max_Stock,b.Remark, b.ID 
        //from (select PartType from IMES2012_GetData.dbo.KitLoc where PdLine = @code) a, IMES2012_FA.dbo.WipBuffer b where b.Tp like a.PartType and b.Code=@code and 
        //b.KittingType = @kittingType order by convert(int,b.LightNo)
        public DataTable GetLightNoList(string kittingType, string code)
        {
            DataTable retLst = new DataTable();
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                retLst = itemRepository.GetLightNoList(kittingType, code);
            }
            catch (Exception)
            {
                throw;
            }
            return retLst;
        }

        //select distinct b.Code,b.PartNo,b.Tp,convert(int,b.LightNo) as LightNo,Qty,Sub,Safety_Stock,Max_Stock,b.Remark, b.ID  
        //from (select PartType from IMES2012_GetData.dbo.KitLoc where PdLine = @code) a, IMES2012_FA.dbo.WipBuffer b where b.Tp like a.PartType and b.Code=@code and 
        //b.KittingType = @kittingType
        //AND Tp not like 'DDD Kitting%' order by convert(int,b.LightNo)
        public DataTable GetLightNoListPAK(string kittingType, string code)
        {
            DataTable retLst = new DataTable();
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                retLst = itemRepository.GetLightNoListPAK(kittingType, code);
            }
            catch (Exception)
            {
                throw;
            }
            return retLst;

        }

        //6) select distinct Code,PartNo,Tp,convert(int,LightNo) as LightNo,Qty,Sub,Safety_Stock,Max_Stock,Remark, b.ID  
        //from IMES2012_FA.dbo.WipBuffer where Code=@code and KittingType = @kittingType 
        //order by convert(int,LightNo)
        public DataTable GetLightNoListFami(string kittingType, string code)
        {
            DataTable retLst = new DataTable();
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                retLst = itemRepository.GetLightNoListFami(kittingType, code);
            }
            catch (Exception)
            {
                throw;
            }
            return retLst;
        }

        //7) select distinct Code,PartNo,Tp,convert(int,LightNo) as LightNo,Qty,Sub,Safety_Stock,Max_Stock,Remark, b.ID  
        //from IMES2012_FA.dbo.WipBuffer where Code=@code and KittingType = @kittingType 
        //AND Tp not like 'DDD Kitting%' 
        //order by convert(int,LightNo)

        public DataTable GetLightNoListFamiPAK(string kittingType, string code)
        {
            DataTable retLst = new DataTable();
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                retLst = itemRepository.GetLightNoListFamiPAK(kittingType, code);
            }
            catch (Exception)
            {
                throw;
            }
            return retLst;
        }

        //新增的id返回到结构里
        public string AddLightNo(WipBuffer item, Boolean codeIsLine)
        {
            String result = "";
            try
            {
                string code=item.Code;
                string partNo = item.PartNo;
                string lightNo = Null2String(item.LightNo);
                string Line;
                
                if (item.Line.Trim()=="")
                   Line ="";
                else
                   Line = item.Line.Substring(0, 1);

                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();

                string kittingType = item.KittingType;
                //对应@family varchar(30),@loc char(4)

                if (kittingType == KITTING_TYPE_FA_KITTING)
                {
                    DataTable CheckResult = itemRepository.ExecOpKittingAutoCheck(code, lightNo, Line);
                    //DataTable CheckResult = itemRepository.ExecOpKittingAutoCheck(code, lightNo, kittingType);

                    if (CheckResult.Rows.Count > 0)
                    {
                        if (Null2String(CheckResult.Rows[0][0]) == "0")
                        {
                            //!!!msgbox DB.Recordset.fields(1)
                            //注意将存储过程中的Rows[0][1]转换为编号！！！
                            //if (Null2String(CheckResult.Rows[0][1]) == "1")
                            //{
                            //浮動庫位，不能手動Maintain!
                            List<string> erpara = new List<string>();
                            FisException ex;
                            ex = new FisException("DMT092", erpara);
                            throw ex;
                            //}
                        }
                    }
                }

                if (partNo.EndsWith(")") != true)
                {
                    DataTable PartData = itemRepository.GetPartInfoByPartNo(partNo);
                    if (PartData.Rows.Count <= 0)
                    {
                        //该PartNo不存在!
                        List<string> erpara = new List<string>();
                        FisException ex;
                        ex = new FisException("DMT091", erpara);
                        throw ex;
                    }
                    else
                    {
                        //tp中存part表中的descr
                        item.Tp = Null2String(PartData.Rows[0][0]);
                    }
                }


                Boolean exist=false;
                if (codeIsLine == true)
                {
                    exist = itemRepository.ExistWipBuffer(code, partNo, lightNo, item.KittingType);
                }
                else
                {   //item.line
                    exist = itemRepository.ExistWipBuffer(code, partNo, lightNo, item.KittingType,item.Line);
                }

                if (exist == true)
                {
                    //已经存在具有相同Code和PartNo的WipBuffer记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT090", erpara);
                    throw ex;

                }

                item.Cdt = DateTime.Now;
                item.Udt = DateTime.Now;
                itemRepository.AddLightNo(item);
                result = item.ID.ToString();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        //9) 更新的数据
        public void UpdateLightNo(WipBuffer item, Boolean codeIsLine)
        {

            try
            {
                string code = item.Code;
                string partNo = item.PartNo;
                int id = item.ID;

                string Line;

                if (item.Line.Trim() == "")
                    Line = "";
                else
                    Line = item.Line.Substring(0, 1);

                //!!!find 出来WipBuffer item
                string lightNo = Null2String(item.LightNo);
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();

                string kittingType = item.KittingType;
                //!!! need
                //DataTable CheckResult = itemRepository.ExecOpKittingAutoCheck(code, lightNo, kittingType);
                //对应@family varchar(30),@loc char(4)

                if (kittingType == KITTING_TYPE_FA_KITTING)
                {
                    DataTable CheckResult = itemRepository.ExecOpKittingAutoCheck(code, lightNo, Line);

                    if (CheckResult.Rows.Count > 0)
                    {
                        if (Null2String(CheckResult.Rows[0][0]) == "0")
                        {
                            //!!!msgbox DB.Recordset.fields(1)
                            //注意将存储过程中的Rows[0][1]转换为编号！！！ 
                            //if (Null2String(CheckResult.Rows[0][1]) == "1")
                            //{
                            ////浮動庫位，不能手動Maintain!
                            List<string> erpara = new List<string>();
                            FisException ex;
                            ex = new FisException("DMT092", erpara);
                            throw ex;
                            //}
                        }
                    }
                }

                if (partNo.EndsWith(")") != true)
                {
                    DataTable PartData = itemRepository.GetPartInfoByPartNo(partNo);
                    if (PartData.Rows.Count <= 0)
                    {
                        //该PartNo不存在!
                        List<string> erpara = new List<string>();
                        FisException ex;
                        ex = new FisException("DMT091", erpara);
                        throw ex;
                    }
                    else
                    {
                        //tp中存part表中的descr
                        item.Tp = Null2String(PartData.Rows[0][0]);
                    }
                }


                Boolean exist=false;
                if (codeIsLine == true)
                {
                    exist = itemRepository.ExistWipBufferExceptCode(code, partNo, lightNo, id, item.KittingType);
                }
                else
                {
                    //item.line
                    exist = itemRepository.ExistWipBufferExceptCode(code, partNo, lightNo, id, item.KittingType,item.Line);
                }

                if (exist == true)
                {
                    //已经存在具有相同Code和PartNo的WipBuffer记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT090", erpara);
                    throw ex;

                }

                WipBuffer newItem = itemRepository.FindWipBufferById(id);

                if (newItem == null)
                {
                    //这条WipBuffer记录已经不存在，不能保存！
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT093", erpara);
                    throw ex;
                }

                newItem.Line = item.Line;
                newItem.Code = item.Code;
                newItem.Editor = item.Editor;
                newItem.KittingType = item.KittingType;
                newItem.LightNo = lightNo;
                newItem.Max_Stock = item.Max_Stock;
                newItem.PartNo = item.PartNo;
                newItem.Qty = item.Qty;
                newItem.Remark = item.Remark;
                newItem.Safety_Stock = item.Safety_Stock;
                newItem.Station = item.Station;
                newItem.Sub = item.Sub;
                newItem.Tp = item.Tp;
                newItem.Udt = DateTime.Now;

                itemRepository.UpdateLightNo(newItem);

            }
            catch (Exception)
            {
                throw;
            }
        }

        //10) DELETE FROM [WipBuffer]
        //WHERE ID='id'
        public void DeleteLightNo(int id)
        {
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                itemRepository.DeleteLightNo(id);
            }
            catch (Exception)
            {
                throw;
            }

        }

        //11) SELECT Descr 
        //FROM [Part]
        //where [PartNo]='partNo'

        public DataTable GetPartInfoByPartNo(string partNo)
        {
            DataTable retLst = new DataTable();
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                retLst = itemRepository.GetPartInfoByPartNo(partNo);
            }
            catch (Exception)
            {
                throw;
            }
            return retLst;

        }


        ////12) SELECT [ID] 
        ////FROM [WipBuffer]
        ////where Code='code' AND PartNo='partNo'
        //Boolean ExistWipBuffer(string code, string partNo);


        ////SELECT [ID] 
        ////FROM [WipBuffer]
        ////where Code='code' AND PartNo='partNo' AND ID<>'id'
        //Boolean ExistWipBufferExceptCode(string code, string partNo, int id)


        //15) SELECT distinct(PartType)
        //FROM KitLoc
        //ORDER BY PartType
        public IList<SelectInfoDef> GetLightNoPartType()
        {
            List<SelectInfoDef> result = new List<SelectInfoDef>();

            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                DataTable getData = itemRepository.GetLightNoPartType();
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



        //16)执行存储过程
        //DataTable ExecOpKittingAutoCheck(string code, string lightNo, string kittingType)
        //Exec op_KittingAutoCheck 'code','lightNo','kittingType'
        //返回存储过程中的select数据

        //DELETE FROM [TmpKit]
        //WHERE PdLine='curLine'
        //INSERT INTO TmpKit
        //(PdLine
        //,Model
        //)
        //VALUES
        //(@line
        //,@model)
        public void ImportTmpKit(List<TmpKitInfoDef> items, string curLine, string type)
        {
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                UnitOfWork uow = new UnitOfWork();
                //目前用同一个表
                itemRepository.DeleteTmpKitDefered(uow, curLine, type);
                itemRepository.ImportTmpKitDefered(uow, items);
                itemRepository.DeleteRealModelDataDefered(uow, curLine, type);
                uow.Commit();
                DataTable uploadResult = itemRepository.GetRealModelData(curLine, type);
                if (uploadResult.Rows.Count <= 0)
                {
                    //没有成功上传的Model!
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT094", erpara);
                    throw ex;

                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        //DELETE FROM [TmpPAKKit]
        //WHERE PdLine='curLine'
        //INSERT INTO TmpPAKKit
        //(PdLine
        //,Model
        //,Qty
        //)
        //VALUES
        //(@line
        //,@model
        //,Qty) 
        public void ImportTmpPAKKit(List<TmpKitInfoDef> items, string curLine,string type)
        {
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                UnitOfWork uow = new UnitOfWork();
                itemRepository.DeleteTmpKitDefered(uow, curLine, type);
                itemRepository.ImportTmpKitDefered(uow, items);
                itemRepository.DeleteRealModelDataDefered(uow, curLine, type);
                uow.Commit();
                DataTable uploadResult = itemRepository.GetRealModelData(curLine, type);
                if (uploadResult.Rows.Count <= 0)
                {
                    //没有成功上传的Model!
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT094", erpara);
                    throw ex;

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //19) 执行存储过程 For FA
        //DataTable ExecOpKittingLocCheck(string pdline, string partType)
        //Exec op_KittingLocCheck @pdline,@partType
        //返回存储过程中的select数据
        public void UploadModelForWipBufferFA(string pdLine, string parkType)
        {
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                //UnitOfWork uow = new UnitOfWork();
                SqlTransactionManager.Begin();

                //!!!need////////////////////////
                DataTable CheckResult = itemRepository.ExecOpKittingLocCheck(pdLine, parkType);

                if (CheckResult.Rows.Count > 0)
                {
                    if (Null2String(CheckResult.Rows[0][0]) == "0")
                    {
                        SqlTransactionManager.Commit();
                    }
                    else
                    {
                        string errorNum = Null2String(CheckResult.Rows[0][0]);
                        SqlTransactionManager.Rollback();
                        DealSendFAUploadError(errorNum);                        
                    }

                }
                ///////////////////////

                //uow.Commit();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }
            
        }

        private static void DealSendFAUploadError(string errorNum)
        {
            //!!!待修改
            if (errorNum == "1")
            {
                //msgbox DB.Recordset.fields(1)
                List<string> erpara = new List<string>();
                FisException ex;
                ex = new FisException("DMT121", erpara);
                throw ex;
            }
            else if (errorNum == "2")
            {
                //msgbox DB.Recordset.fields(1)
                List<string> erpara = new List<string>();
                FisException ex;
                ex = new FisException("DMT122", erpara);
                throw ex;
            }
            else if (errorNum == "3")
            {
                //msgbox DB.Recordset.fields(1)
                List<string> erpara = new List<string>();
                FisException ex;
                ex = new FisException("DMT123", erpara);
                throw ex;
            }
            else if (errorNum == "4")
            {
                //msgbox DB.Recordset.fields(1)
                List<string> erpara = new List<string>();
                FisException ex;
                ex = new FisException("DMT124", erpara);
                throw ex;
            }
            else if (errorNum =="5")
            {
                //msgbox DB.Recordset.fields(1)
                List<string> erpara = new List<string>();
                FisException ex;
                ex = new FisException("DMT153", erpara);
                throw ex;
            }

        }

        private static void DealSendPAKUploadError(string errorNum)
        {
            //!!!待修改
            if (errorNum == "1")
            {
                //msgbox DB.Recordset.fields(1)
                List<string> erpara = new List<string>();
                FisException ex;
                ex = new FisException("DMT121", erpara);
                throw ex;
            }
            else if (errorNum == "2")
            {
                //msgbox DB.Recordset.fields(1)
                List<string> erpara = new List<string>();
                FisException ex;
                ex = new FisException("DMT125", erpara);
                throw ex;
            }
            else if (errorNum == "3")
            {
                //msgbox DB.Recordset.fields(1)
                List<string> erpara = new List<string>();
                FisException ex;
                ex = new FisException("DMT126", erpara);
                throw ex;
            }
           
        }


        //20) 执行存储过程For PAK
        //DataTable ExecOpPAKKitLocFV(string pdline)
        //Exec op_PAKKitLoc_FV @pdline 
        //返回存储过程中的select数据
        public DataTable UploadModelForWipBufferPAK(string pdLine)
        {
            DataTable result = new DataTable();
            try
            {
                IProductRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
                //UnitOfWork uow = new UnitOfWork();
                SqlTransactionManager.Begin();
                //!!!need///////////////////////////

                DataTable CheckResult = itemRepository.ExecOpPAKKitLocFV(pdLine);

                if (CheckResult.Rows.Count > 0)
                {
                    if (Null2String(CheckResult.Rows[0][0]) == "0")
                    {
                        result = CheckResult;
                        SqlTransactionManager.Commit();
                        
                    }
                    else
                    {
                        string errorNum = Null2String(CheckResult.Rows[0][0]);
                        SqlTransactionManager.Rollback();
                        if (errorNum == "4")
                        {
                            string info = Null2String(CheckResult.Rows[0][1]);
                            //msgbox DB.Recordset.fields(1)
                            List<string> erpara = new List<string>();
                            erpara.Add(info);
                            FisException ex;
                            ex = new FisException("DMT127", erpara);
                            throw ex;

                        }
                        else
                        {
                            DealSendPAKUploadError(errorNum);
                        }
                    }
                }
                else
                {
                    //成功但SPno,Descr,sum(Qty),LightNo from #Bom_Loc group by SPno,Descr,LightNo order by SPno  没有数据的情况
                    //提示成功
                    SqlTransactionManager.Commit();
                }
                //////////////////////////////

                //uow.Commit();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }

            return result;
        }

        public DataTable GetLightNoFromSp(string code, string kittingType, string isLine)
        {
            DataTable retLst = new DataTable();
            try
            {
                IPalletRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();
                retLst = itemRepository.ExecSpRptKittingLoc(code,kittingType,isLine);
            }
            catch (Exception)
            {
                throw;
            }
            return retLst;

        }

    }
}
