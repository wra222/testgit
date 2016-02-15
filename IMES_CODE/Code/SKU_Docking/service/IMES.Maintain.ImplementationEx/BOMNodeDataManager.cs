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
using IMES.FisObject.Common.ModelBOM;
using IMES.FisObject.Common.Warranty;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.NumControl;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
//ITC-1136-0125 fix

namespace IMES.Maintain.Implementation
{
    public class BOMNodeDataManager : MarshalByRefObject,IBOMNodeData
    {

        public static string LevelString = System.Configuration.ConfigurationSettings.AppSettings["MODEL_BOM_DEEP_LEVEL"].ToString();
        public int BOM_MAX_DEEP_LEVEL = Int32.Parse(LevelString); 
        #region implement IBOMNodeData

        //current,当前添加节点的父节点，包括它本身，和它所有的父节点，是model的都要记到 refresh model表中

        //返回值需要填充的项
        //public string id;
        //public string parent; 参数中的current
        //public string curent;
        //public string desc;
        //public bool isPart;
        //public bool isModel;   

        //parent,当前节点父节点的信息
        public TreeNodeDef AddModelBOM(ModelBOMInfoDef item, ChangeNodeInfoDef parent)
        {
            //添加时需要检查是否在part表或在model表及另一个表中存在
            //然后取得该点在树上的信息（返回值）
            //添加数据

            TreeNodeDef result = new TreeNodeDef();
            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOMNodeData>();
                //判断是否已经存在此父子关系
                //strSql = "INSERT ModelBom (Material, Material_group, Component, Quantity) values('" + parentCode + "','" + type + "','" + oldCode + "','" + qty + "')";
                IList<BOMNodeData> itemList = itemRepository.findModelBomByMaterialAndComponent(parent.NodeName, item.Component);
                if (itemList.Count > 0)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    //!!!need change
                    //数据库中已经存在相同的父子的关系
                    ex = new FisException("DMT017", erpara);
                    throw ex;
                }

                //判断添加的子节点是一个Part
                //SELECT PartNo as Code,Descr from Part where PartNo='code' AND Flag=1 AND AutoDL='Y'
                string partDesc = "";
                DataTable existPartNos = itemRepository.GetExistPartNo(item.Component);
                if (existPartNos == null || existPartNos.Rows.Count == 0)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    //!!!need change
                    //添加的子节点不是一个Part
                    ex = new FisException("DMT018", erpara);
                    throw ex;
                }

                partDesc = Null2String(existPartNos.Rows[0][1]);

                //!!!找父节点的Type（Material_group）!!!还要找part中的type
                //GetMaterialById
                string type = "";
                //SELECT Material AS Code, Material_group AS PartType FROM ModelBOM where Material='code' AND Flag=1
                //UNION SELECT [PartNo] AS Code ,[PartType]  AS PartType    
                //  FROM [IMES_GetData_Datamaintain].[dbo].[Part]
                //where Flag=1 AND [PartNo]='code'

                //need change
                DataTable resultTypeInfo = itemRepository.GetPartTypeInfoByCode(parent.NodeName);
                if (resultTypeInfo.Rows.Count > 0)
                {
                    type = Null2String(resultTypeInfo.Rows[0][1]);
                }

                Hashtable modelIds = new Hashtable();
                //附带增加检查当前partNo是否存在于父节点
                Dictionary<string, string> parents = new Dictionary<string, string>();
                GetNeedRefreshModelIncludeCurrentList(parent, modelIds, parents);
                Dictionary<string, string> children = new Dictionary<string, string>();
                GetAllChildren(item.Component, children);
                Boolean isExist = false;

                isExist = JudgeErrorParentChildren(parents, children, isExist);

                //if (isExist == true || parent.NodeName == item.Component)
                if (isExist == true)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    //!!!need change
                    //添加的子节点与直接或间接父节点有嵌套
                    ex = new FisException("DMT019", erpara);
                    throw ex;
                }

                UnitOfWork uow = new UnitOfWork();

                foreach (String modelId in modelIds.Keys)
                {
                    //Refreshmodel表
                    //删除原来的modelId
                    //添加新的modelId
                    itemRepository.DeleteRefreshModelByModelDefered(uow, modelId, item.Editor);
                    itemRepository.AddRefreshModelDefered(uow, modelId, item.Editor);
                }

                //需在一个事物中
                itemRepository.DeleteModelBomByMaterialAndComponentDefered(uow, parent.NodeName, item.Component);

                BOMNodeData itemNew = new BOMNodeData();
                itemNew.Material = parent.NodeName;
                // itemNew.Material_group = type;
                itemNew.Component = item.Component;
                itemNew.Quantity = item.Quantity;
                itemNew.Priority = item.Priority;
                itemNew.Editor = item.Editor;
                itemNew.Cdt = DateTime.Now;
                itemNew.Udt = DateTime.Now;
                itemNew.Plant = "";
                itemNew.Alternative_item_group = "";
                itemRepository.AddModelBOMDefered(uow, itemNew);
                uow.Commit();

                result.id = itemNew.ID.ToString();
                result.curent = itemNew.Component;
                result.desc = partDesc;

            }
            catch (Exception)
            {
                throw;
            }

            return result;

        }



        //for new ModelBOM///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ///////////
        /// </summary>
        /// <param name="pn"></param>
        /// <returns></returns>
        /// 

        //SELECT [Model] AS PartNo, ‘’  AS Descr,
        //                   0 AS WHERETYPE
        //      ,[BOMApproveDate] AS [BOMApproveDate]
        //      ,[Editor]
        //      ,[Cdt]
        //      ,[Udt]
        //  FROM [IMES_GetData_Datamaintain].[dbo].[Model]
        //WHERE [Model]= 'pn'
        //union 
        //SELECT [PartNo] AS PartNo,
        //       Descr  AS Descr,
        //       1 AS WHERETYPE,
        //       GetDate() AS [BOMApproveDate]
        //      ,[Editor]
        //      ,[Cdt]
        //      ,[Udt]
        //  FROM [IMES_GetData_Datamaintain].[dbo].[Part]
        //WHERE [PartNo]='pn' AND Flag=1
        //order by WHERETYPE
        public InputMaterialInfoDef GetMaterialInfo(string pn)
        {

            InputMaterialInfoDef info = new InputMaterialInfoDef();
            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOMNodeData>();
                DataTable getData = new DataTable();
                getData = itemRepository.GetMaterialInfo(pn);
                if (getData.Rows.Count > 0)
                {
                    info.PartNo = Null2String(getData.Rows[0][0]).Trim();
                    //如果part表中有description,在后面，所以取最后的description
                    info.Description = Null2String(getData.Rows[getData.Rows.Count - 1][1]).Trim();
                    string type = Null2String(getData.Rows[0][2]).Trim();

                    if (getData.Rows.Count > 1)
                    {
                        info.IsModel = "True";
                        info.IsPart = "True";
                    }
                    else if (type == "0")
                    {
                        info.IsModel = "True";
                        info.IsPart = "False";
                    }
                    else
                    {
                        info.IsPart = "True";
                        info.IsModel = "False";
                    }

                    info.BOMApproveDate = Null2String(getData.Rows[0][3]).Trim();
                    if (info.BOMApproveDate != "")
                    {
                        info.BOMApproveDate = ((DateTime)getData.Rows[0][3]).ToString("yyyy-MM-dd");
                    }
                    info.Editor = Null2String(getData.Rows[0][4]).Trim();
                    info.Cdt = ((DateTime)getData.Rows[0][5]).ToString("yyyy-MM-dd");
                    info.Udt = ((DateTime)getData.Rows[0][6]).ToString("yyyy-MM-dd");
                }
                else
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    //!!!need change
                    //输入的[Model/PN]不是合法的Model或Part
                    ex = new FisException("DMT013", erpara);
                    throw ex;
                }

            }
            catch (Exception)
            {
                throw;
            }

            return info;

        }


        //取得树的父子关系列表
        //调用存储过程 GetModelBOMAutoDL(pn) 取得树的结果DataTable
        public DataTable GetTreeTable(string pn)
        {
            DataTable result;
            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOMNodeData>();
                int getStatus = 0;
                result = itemRepository.GetTreeTable(pn, BOM_MAX_DEEP_LEVEL, ref getStatus);
                if (getStatus == -1)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    //!!!need change right!!!
                    //要取得物品的数据在数据库中不正确, 有父子的嵌套存在
                    ex = new FisException("DMT014", erpara);
                    throw ex;
                }

                if (result == null)
                {
                    return new DataTable();
                }
            }
            //catch (SqlException e)
            //{
            //    if (e.Number == 2627)
            //    {
            //        List<string> erpara = new List<string>();
            //        FisException ex;
            //        //!!!need change right!!!
            //        //要取得物品的数据在数据库中不正确, 有父子的嵌套存在
            //        ex = new FisException("DMT014", erpara);
            //        throw ex;
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}
            catch (Exception)
            {

                throw;
            }
            return result;
        }

        public TreeNodeDef SaveModelBOM(ModelBOMInfoDef item, ChangeNodeInfoDef parent)
        {

            //添加时需要检查是否在part表或在model表及另一个表中存在
            //然后取得该点在树上的信息（返回值）

            TreeNodeDef result = new TreeNodeDef();
            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOMNodeData>();
                //判断是否已经存在此父子关系
                //Select * from ModelBom where Material='" + parentCode + "' and Component='" + oldCode + "'
                IList<BOMNodeData> itemList = itemRepository.findModelBomByMaterialAndComponent(parent.NodeName, item.Component);
                if (itemList.Count > 0 && itemList[0].ID.ToString() != item.ID)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    //!!!need change
                    //数据库中已经存在相同的父子的关系
                    ex = new FisException("DMT017", erpara);
                    throw ex;
                }

                //判断修改的子节点是一个Part
                //SELECT PartNo as Code,Descr from Part where PartNo='code' AND Flag=1  AND AutoDL='Y'
                string partDesc = "";
                DataTable existPartNos = itemRepository.GetExistPartNo(item.Component);
                if (existPartNos == null || existPartNos.Rows.Count == 0)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    //!!!need change
                    //子节点不是一个Part
                    ex = new FisException("DMT018", erpara);
                    throw ex;
                }

                partDesc = Null2String(existPartNos.Rows[0][1]);

                //!!!找父节点的Type（Material_group）!!!还要找part中的type
                //GetMaterialById
                string type = "";
                //SELECT Material AS Code, Material_group AS PartType FROM ModelBOM where Material='code' AND Flag=1
                //UNION SELECT [PartNo] AS Code ,[PartType]  AS PartType    
                //  FROM [IMES_GetData_Datamaintain].[dbo].[Part]
                //where Flag=1 AND [PartNo]='code'

                //父节点的Part信息
                DataTable resultTypeInfo = itemRepository.GetPartTypeInfoByCode(parent.NodeName);
                if (resultTypeInfo.Rows.Count > 0)
                {
                    type = Null2String(resultTypeInfo.Rows[0][1]);
                }

                Hashtable modelIds = new Hashtable();
                Dictionary<string, string> parents = new Dictionary<string, string>();
                GetNeedRefreshModelIncludeCurrentList(parent, modelIds, parents);
                Dictionary<string, string> children = new Dictionary<string, string>();
                GetAllChildren(item.Component, children);
                Boolean isExist = false;
                isExist = JudgeErrorParentChildren(parents, children, isExist);

                //if (isExist == true || parent.NodeName == item.Component)
                if (isExist == true)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    //!!!need change
                    //子节点与直接或间接父节点有嵌套
                    ex = new FisException("DMT019", erpara);
                    throw ex;
                }
                
                UnitOfWork uow = new UnitOfWork();

                foreach (String modelId in modelIds.Keys)
                {
                    //Refreshmodel表
                    //删除原来的modelId
                    //添加新的modelId
                    itemRepository.DeleteRefreshModelByModelDefered(uow, modelId, item.Editor);
                    itemRepository.AddRefreshModelDefered(uow, modelId, item.Editor);
                }

                //需在一个事物中
                //itemRepository.DeleteModelBomByMaterialAndComponentDefered(uow, parent.NodeName, item.Component);
                //空的不是公用料
                IMiscRepository iMiscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                MoBOMInfo condition = new MoBOMInfo();
                condition.id = Int32.Parse(item.ID);

                IList<MoBOMInfo> itemNew = iMiscRepository.GetData<IMES.Infrastructure.Repository._Metas.ModelBOM_NEW, MoBOMInfo>(condition);

                //BOMNodeData itemNew = itemRepository.GetModelBOM2(Int32.Parse(item.ID));

                Boolean isNeedChangeGroupNo = false;
                //判断有共用料时再做，并且数据有变化时再做
                if (Null2String(itemNew[0].alternative_item_group) != "" && itemNew[0].quantity != item.Quantity)
                {
                    isNeedChangeGroupNo = true;

                }
                itemNew[0].material = parent.NodeName;
                itemNew[0].component = item.Component;
                itemNew[0].quantity = item.Quantity;
                itemNew[0].priority = item.Priority;
                itemNew[0].flag = int.Parse(item.Flag);
                itemNew[0].editor = item.Editor;
                itemNew[0].udt = DateTime.Now;

                iMiscRepository.UpdateDataByIDDefered<IMES.Infrastructure.Repository._Metas.ModelBOM_NEW, MoBOMInfo>(uow, condition, itemNew[0]);
                //itemRepository.UpdateModelBOMDefered(uow, itemNew);


                if (isNeedChangeGroupNo == true)
                {
                    itemRepository.UpdateGroupQuantityDefered(uow, item.Quantity, itemNew[0].alternative_item_group, item.Editor);
                }
                uow.Commit();

                result.id = item.ID.ToString();
                result.curent = itemNew[0].component;
                result.desc = partDesc;
                
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }


        // "execute SaveModelBOMAs '" + code + "', '" + newCode + "'", editor;
        //isNeedCheck, 是否需要检查newCode已经被使用，首次时需要检查，用户确认后再次isNeedCheck=false
        //调用PROCEDURE SaveModelBOMAs
        public string SaveModelBOMAs(string code, string newCode, string editor, Boolean isNeedCheck)
        {
            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOMNodeData>();
                IModelRepository iModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
                IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                IMiscRepository iMiscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                if (isNeedCheck == true)
                {
                    bool isModel = false;
                    #region check is Model or Part
                    Model model = iModelRepository.Find(code);
                    IPart part = iPartRepository.GetPartByPartNo(code);
                    if (model == null && part == null)
                    {
                        List<string> erpara = new List<string>();
                        FisException ex;
                        ex = new FisException("Input Error");
                        throw ex;
                    }
                    if (model != null)
                    {
                        isModel = true;
                    }

                    #endregion
                    if (isModel)
                    {
                        #region model
                        Model newmodel = iModelRepository.Find(newCode);

                        if (newmodel == null)
                        {
                            IUnitOfWork uow = new UnitOfWork();
                            iModelRepository.CopyModelDefered(uow, code, newCode, 0, editor);
                            uow.Commit();
                        }
                        #endregion
                    }
                    else
                    {
                        #region part
                        IPart newpart = iPartRepository.Find(newCode);
                        if (newpart == null)
                        {
                            IUnitOfWork uow = new UnitOfWork();
                            iPartRepository.CopyPartlDefered(uow, code, newCode, 0, editor);
                            uow.Commit();
                        }
                        #endregion
                    }
                    DataTable result = itemRepository.GetMaterialById(newCode);
                    if (result.Rows.Count > 0)
                    {
                        List<string> erpara = new List<string>();
                        FisException ex;
                        //!!!need change right!!!
                        //您所输入的Code已存在于Model BOM中
                        ex = new FisException("DMT012", erpara);
                        throw ex;
                    }
                }
                //IMiscRepository itemRepository2 = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                itemRepository.SaveModelBOMAs(code, newCode, editor);
            }
            catch (Exception)
            {
                throw;
            }
            return newCode;
        }

        public string SaveAs(string code, string newCode, string editor, Boolean isNeedCheck)
        {
            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOMNodeData>();
                IModelRepository iModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
                IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                IMiscRepository iMiscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                if (isNeedCheck == true)
                {
                    bool isModel = false;
                    #region check is Model or Part
                    Model model = iModelRepository.Find(code);
                    IPart part = iPartRepository.GetPartByPartNo(code);
                    if (model == null && part == null)
                    {
                        List<string> erpara = new List<string>();
                        FisException ex;
                        ex = new FisException("Input Error");
                        throw ex;
                    }
                    if (model != null)
                    {
                        isModel = true;
                    }

                    #endregion
                    IUnitOfWork uow = new UnitOfWork();
                    if (isModel)
                    {
                        #region model
                        Model newmodel = iModelRepository.Find(newCode);
                        if (newmodel == null)
                        {
                            iModelRepository.CopyModelDefered(uow, code, newCode, 0, editor);
                        }
                        else
                        {
                            List<string> erpara = new List<string>();
                            erpara.Add(newCode);
                            FisException ex;
                            ex = new FisException("CHK1252", erpara);
                            throw ex;
                        }
                        #endregion
                    }
                    else
                    {
                        #region part
                        PartDef condition = new PartDef();
                        condition.partNo = newCode;
                        IList<PartDef> newpartlist = iMiscRepository.GetData<IMES.Infrastructure.Repository._Metas.Part_NEW, PartDef>(condition);
                        if (newpartlist.Count == 0)
                        {
                            iPartRepository.CopyPartlDefered(uow, code, newCode, 0, editor);
                        }
                        else
                        {
                            List<string> erpara = new List<string>();
                            erpara.Add(newCode);
                            FisException ex;
                            ex = new FisException("CHK1251", erpara);
                            throw ex;
                        }
                        #endregion
                    }
                    DataTable result = itemRepository.GetMaterialById(newCode);
                    if (result.Rows.Count > 0)
                    {
                        List<string> erpara = new List<string>();
                        FisException ex;
                        ex = new FisException("DMT012", erpara);
                        throw ex;
                    }
                    itemRepository.SaveModelBOMAsDefered(uow, code, newCode, editor);
                    uow.Commit();
                }

            }
            catch (Exception)
            {
                throw;
            }
            return newCode;
        }

        public string SaveAsDummy(string code, string editor, string customer, Boolean isNeedCheck)
        {
            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOMNodeData>();
                IModelRepository iModelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
                IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                IMiscRepository iMiscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
                Model model = iModelRepository.Find(code);
                if (model == null)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("Input:" + code + " Error");
                    throw ex;
                }

                IList<FisObject.Common.Model.ModelInfo> modelInfoList = iModelRepository.GetModelInfoByModelAndName(code, "RealModel");
                if (modelInfoList.Count != 0)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("Model:" + code + " This Model is not RealModel ,can't Save as Dummy!");
                    throw ex;
                }

                DataTable ret = null;
                string getDummyModel = "";
                SqlParameter[] paramsArray = new SqlParameter[2];
                paramsArray[0] = new SqlParameter("@Model", SqlDbType.VarChar);
                paramsArray[0].Value = code;
                paramsArray[1] = new SqlParameter("@Customer", SqlDbType.VarChar);
                paramsArray[1].Value = customer;
                ret = IMES.Infrastructure.Repository._Schema.SqlHelper.ExecuteDataFill(IMES.Infrastructure.Repository._Schema.SqlHelper.ConnectionString_BOM, CommandType.StoredProcedure, "GetDummyModel", paramsArray);
                if (ret == null || ret.Rows.Count == 0)
                {
                    throw new Exception("SP:GetDummyModel Exec Error");
                }
                getDummyModel = ret.Rows[0][0].ToString();
                IUnitOfWork uow = new UnitOfWork();
                Model newmodel = iModelRepository.Find(getDummyModel);
                if (newmodel == null)
                {
                    iModelRepository.CopyModelDefered(uow, code, getDummyModel, 0, editor);
                }
                else
                {
                    List<string> erpara = new List<string>();
                    erpara.Add(getDummyModel);
                    FisException ex;
                    ex = new FisException("CHK1252", erpara);
                    throw ex;
                }

                IMES.FisObject.Common.Model.ModelInfo item = new IMES.FisObject.Common.Model.ModelInfo();
                item.ModelName = getDummyModel;
                item.Name = "RealModel";
                item.Value = code;
                item.Description = "RealModel";
                item.Editor = editor;
                item.Cdt = DateTime.Now;
                item.Udt = DateTime.Now;
                iModelRepository.AddModelInfoDefered(uow, item);
                DataTable result = itemRepository.GetMaterialById(getDummyModel);
                if (result.Rows.Count > 0)
                {
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT012", erpara);
                    throw ex;
                }
                itemRepository.SaveModelBOMAsDefered(uow, code, getDummyModel, editor);
                uow.Commit();
                return getDummyModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //current,当前被更改关系的父节点，包括它本身，和它所有的父节点，是approve过的model的都要记到 refresh model表中
        public void Delete(List<string> idList, ChangeNodeInfoDef current, string editor)
        {
            try
            {
                Hashtable modelIds = new Hashtable();
                //GetNeedRefreshModelIncludeCurrentList(current, modelIds);
                //////////////////////////////////////

                UnitOfWork uow = new UnitOfWork();
                IModelBOMRepository itemRepositoryBom = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOMNodeData>();
                //Refreshmodel表添加数据，找不到在添加
                foreach (String modelId in modelIds.Keys)
                {
                    //Refreshmodel表
                    //删除原来的modelId
                    //添加新的modelId
                    //itemRepositoryBom.DeleteRefreshModelByModelDefered(uow, modelId, editor);
                    itemRepositoryBom.AddRefreshModelDefered(uow, modelId, editor);
                }

                for (int i = 0; i < idList.Count; i++)
                {
                    int id = Int32.Parse(idList[i]);
                    itemRepositoryBom.RemoveModelBOMByIdDefered(uow, id, editor);
                }

                uow.Commit();

            }
            catch (Exception)
            {
                throw;
            }
        }


        //取全部父节点信息
        //返回4列，需要隐藏2列
        //a.Material, b.Descr, c.Flag, c.ApproveDate
        public DataTable GetAllParentInfo(string code)
        {

            DataTable allCollectResult = new DataTable();
            //datatable里面排序
            DealGetAllParentInfo(code, allCollectResult);

            //if (allCollectResult.Rows.Count == 0)
            //{
            //    return allCollectResult;
            //}

            DataView dv = allCollectResult.DefaultView;
            //!!!need change
            dv.Sort = "Material,Flag,Descr";
            DataTable allSortedResult = dv.ToTable();

            if (allSortedResult.Rows.Count <= 0)
            {
                return new DataTable();
            }

            string tmpCurrent = Null2String(allSortedResult.Rows[0][0]);

            DataTable result = new DataTable();
            result.Columns.Add("Material");
            result.Columns.Add("Descr");
            result.Columns.Add("Flag");
            result.Columns.Add("ApproveDate");

            DataRow item;
            for (int i = 0; i < allSortedResult.Rows.Count; i++)
            {
                if (tmpCurrent != Null2String(allSortedResult.Rows[i][0]))
                {
                    item = allSortedResult.Rows[i - 1];
                    DataRow dr = CloneDataRow(item, result);
                    result.Rows.Add(dr);
                    tmpCurrent = Null2String(allSortedResult.Rows[i][0]);
                }
            }

            item = allSortedResult.Rows[allSortedResult.Rows.Count - 1];
            DataRow drlast = CloneDataRow(item, result);
            result.Rows.Add(drlast);
            return result;
            //DataSet dataset=new DataSet ();
            //dataset.Tables.Add(allCollectResult);
            //dataset.Tables(0).DefaultView.Sort = "id desc";    

        }



        //SELECT distinct a.Material, b.Descr, c.Flag, c.ApproveDate FROM ModelBOM  AS a 
        //left outer join 
        //(SELECT Descr, PartNo
        //  FROM [IMES_GetData_Datamaintain].[dbo].[Part]
        //WHERE Flag=1) AS b
        //ON a.Material=b.PartNo
        //left outer join 
        //(Select 'Model' AS Flag, BOMApproveDate AS ApproveDate, Model from Model) AS c
        //on a.Material=c.Model 
        //where a.Component='current' and a.Flag=1

        //c.Flag,标识是否是'Model'

        //返回4列，需要隐藏2列
        public DataTable GetParentInfo(string code)
        {
            //datatable里面排序，distinct
            DataTable result = new DataTable();
            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOMNodeData>();
                List<string> codeList =new List<string>();
                codeList.Add(code);
                result = itemRepository.GetParentInfo(codeList);
                if (result == null)
                {
                    return new DataTable();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }


        /// <summary>
        /// 设置共用料
        /// </summary>
        /// <param name="idList"></param>
        /// <param name="current"></param>
        /// 


        //SELECT a.Component as Material, b.PartType, a.Quantity, 
        //a.Alternative_item_group, a.Priority 
        //,a.[Editor],a.[Cdt],a.[Udt],a.ID FROM ModelBOM AS a
        //left outer join 
        //(select Material AS Code, Material_group AS PartType from ModelBOM where Flag=1
        //  UNION SELECT [PartNo] AS Code ,[PartType]  AS PartType    
        //  FROM [IMES_GetData_Datamaintain].[dbo].[Part] where Flag=1          
        //) AS b on a.Component=b.Code 
        //WHERE  a.Flag=1 AND a.ID IN (idList)
        //order by a.Component
        public void SetNewAlternativeGroup(List<string> idStrList, ChangeNodeInfoDef current, string editor)
        {
            Hashtable modelIds = new Hashtable();
            if (idStrList.Count == 0)
            {
                return;
            }

            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOMNodeData>();
                List<int> idList = new List<int>();
                for (int i = 0; i < idStrList.Count; i++)
                {
                    int id = int.Parse(idStrList[i]);
                    idList.Add(id);

                }
                DataTable result = itemRepository.GetSubModelBOMByCode(idList);

                ////数据库中的数据已经被其他人修改
                //if (idStrList.Count != result.Rows.Count)
                //{
                //    List<string> erpara = new List<string>();
                //    FisException ex;
                //    //!!!need change right!!!
                //    //数据库中的数据已经被其他人修改，建议重新输入[Model/PN]，刷新界面以获取最新数据
                //    ex = new FisException("DMT001", erpara);
                //    throw ex;
                //}

                string firstGroup = Null2String(result.Rows[0][2]);
                string firstQty = Null2String(result.Rows[0][4]);

                //判断Qty和Type应该一致
                for (int i = 1; i < result.Rows.Count; i++)
                {
                    string currentGroup = Null2String(result.Rows[i][2]);
                    string currentQty = Null2String(result.Rows[i][4]);
                    if (currentGroup != firstGroup || currentQty != firstQty)
                    {
                        //!!!check是否要细化
                        List<string> erpara = new List<string>();
                        FisException ex;
                        //!!!need change right!!!
                        //设置共用料的项目的Qty或Type有不相同的
                        ex = new FisException("DMT016", erpara);
                        throw ex;
                    }

                }

                GetNeedRefreshModelIncludeCurrentList(current, modelIds);

                //事物中存共用料，和存Refresh Model
                UnitOfWork uow = new UnitOfWork();
                //foreach (String modelId in modelIds.Keys)
                //{
                //    //Refreshmodel表
                //    //删除原来的modelId
                //    //添加新的modelId
                //    itemRepository.DeleteRefreshModelByModelDefered(uow, modelId, editor);
                //    itemRepository.AddRefreshModelDefered(uow, modelId, editor);
                //}
                //!!!need change
                //调用设置共用料UPDATE ModelBOM SET Alternative_item_group=newid() where ID in(idlist) AND Flag=1
                itemRepository.SetNewAlternativeGroupDefered(uow, idList, editor);
                uow.Commit();

            }
            catch (Exception)
            {
                throw;
            }


        }




        public void CacheUpdate_ForBOM(string component)
        {
            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOMNodeData>();
                //IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository>();
                itemRepository.CacheUpdate_ForBOM(component);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region BOMNodeRelation

        private static string RootBOMNodeType = "PC";

        public IList<BOMNodeRelation> FindBOMNodeRelationChild(string ParentType)
        {
            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository>();
                return itemRepository.FindBOMNodeRelationChild(ParentType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<BOMNodeRelation> FindBOMNodeRelationByPair(string BOMNodeType, string ChildBOMNodeType)
        {
            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository>();
                return itemRepository.FindBOMNodeRelationByPair(BOMNodeType, ChildBOMNodeType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<string> FindBOMNodeRelationTypes(string TYPE)
        {
            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository>();
                DataTable result = itemRepository.FindBOMNodeRelationByRoot(TYPE);
                if (result == null)
                {
                    return new List<string>();
                }
                Hashtable types = new Hashtable();
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    if (null != result.Rows[i]["ChildBOMNodeType"])
                    {
                        string t = result.Rows[i]["ChildBOMNodeType"].ToString();
                        if (!types.ContainsKey(t))
                        {
                            types.Add(t, "");
                        }
                    }
                }
                return types.Keys.OfType<string>().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteBOMNodeRelation(string BOMNodeType, string ChildBOMNodeType)
        {
            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository>();
                itemRepository.DeleteBOMNodeRelation(BOMNodeType, ChildBOMNodeType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<BOMNodeRelation> GetBOMNodeRelation()
        {
            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository>();
                return itemRepository.GetBOMNodeRelation();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public string AddBOMNodeRelation(BOMNodeRelation r)
        {
            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository>();
                return itemRepository.AddBOMNodeRelation(r);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateBOMNodeRelation(BOMNodeRelation r)
        {
            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository>();
                itemRepository.UpdateBOMNodeRelation(r);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateBOMNodeRelation(BOMNodeRelation r, int ID)
        {
            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository>();
                itemRepository.UpdateBOMNodeRelation(r, ID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteBOMNodeRelation(int ID)
        {
            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository>();
                itemRepository.DeleteBOMNodeRelation(ID);
            }
            catch (Exception)
            {
                throw;
            }
        }



        public DataTable FindBOMNodeRelationByRoot(string TYPE)
        {
            DataTable result;
            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository>();

                /*if ("".Equals(ID))
                {
                    //ID = "PC";

                    IList<BOMNodeRelation> lst = itemRepository.FindBOMNodeRelationByPair("", "PC");
                    if (lst.Count == 0)
                    {
                        BOMNodeRelation r = new BOMNodeRelation();
                        r.BOMNodeType = "";
                        r.ChildBOMNodeType = "PC";
                        r.Descr = "";
                        r.Editor = "System";
                        ID = itemRepository.AddBOMNodeRelation(r);
                    }
                    else
                    {
                        ID = lst[0].ID.ToString();
                    }
                }*/

                result = itemRepository.FindBOMNodeRelationByRoot(TYPE);
                if (result == null)
                {
                    return new DataTable();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public IList<string> FindBOMNodeRelationParentsByRoot(string TYPE, string ChildType)
        {
            DataTable result;
            IList<string> ret = new List<string>();
            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository>();

                result = itemRepository.FindBOMNodeRelationParentsByRoot(TYPE, ChildType);
                if (result == null)
                {
                    return ret;
                }

                Hashtable parent = new Hashtable();
                parent.Add(TYPE, "");
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    if (null != result.Rows[i]["Tree"])
                    {
                        string[] tree = result.Rows[i]["Tree"].ToString().Split( ',' );
                        for (int j = 0; j < tree.Length; j++)
                        {
                            if (!parent.ContainsKey(tree[j]))
                                parent.Add(tree[j], "");
                        }
                    }
                }
                return parent.Keys.OfType<string>().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetTreeTableByID(string ID)
        {
            DataTable result;
            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository>();

                result = itemRepository.GetTreeTableByID(ID);

                if (result == null)
                {
                    return new DataTable();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }


        public bool ChekkMatchingBOMRelation(string ParentPartNo, string ChildPartNo, ref string ParentType, ref string ChildType)
        {
            IList<string> ret = new List<string>();
            DataTable result;
            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository>();
                result = itemRepository.GetBomNodeType(ParentPartNo, ChildPartNo);

                //string ParentType = "", ChildType = "";
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    if (ParentPartNo.Equals(result.Rows[i]["PartNo"].ToString().Trim()))
                        ParentType = result.Rows[i]["BomNodeType"].ToString().Trim();
                    else if (ChildPartNo.Equals(result.Rows[i]["PartNo"].ToString().Trim()))
                        ChildType = result.Rows[i]["BomNodeType"].ToString().Trim();
                }
                if ("".Equals(ParentType))
                {
                    ParentType = RootBOMNodeType;
                }

                result = itemRepository.GetChildBOMTypes(ParentType);

                for (int i = 0; i < result.Rows.Count; i++)
                {
                    string subType = "";
                    if (null != result.Rows[i]["ChildBOMNodeType"])
                        subType = result.Rows[i]["ChildBOMNodeType"].ToString().Trim();
                    if (ChildType.Equals(subType))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        public IList<string> GetModelsFromModelBOM(string modelNo, int rowCount)
        {
            IList<string> lst = null;

            try
            {
                IModelBOMRepository modelRepositoryEx = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository>();
                lst = modelRepositoryEx.GetModelsFromModelBOM(modelNo, rowCount);
                return lst;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public IList<string> GetPartsFromModelBOM(string partNo, int rowCount)
        {
            IList<string> lst = null;

            try
            {
                IModelBOMRepository modelRepositoryEx = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository>();
                lst = modelRepositoryEx.GetPartsFromModelBOM(partNo, rowCount);
                return lst;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region internal function


        private static String Null2String(Object _input)
        {
            if (_input == null)
            {
                return "";
            }
            return _input.ToString().Trim();
        } 

      



        /// <summary>
        /// 将当前节点和其所有父节点中是model的数据，放到modelIds中
        /// </summary>
        /// <param name="current">当前节点</param>
        /// <param name="modelIds">存放当前节点和其所有父节点中是model的数据</param>
        private void GetNeedRefreshModel(ChangeNodeInfoDef current, Hashtable modelIds, Dictionary<string, string> parents)
        {
            if (current.IsModel == true)
            {
                modelIds.Add(current.NodeName, current.NodeName);
            }

            try
            {
                parents.Add(current.NodeName, current.NodeName);
            }
            catch
            {
                //不需要处理
            }

            DataTable allCollectResult = new DataTable();
            //datatable里面排序
            DealGetAllParentInfo(current.NodeName, allCollectResult);

            for (int i = 0; i < allCollectResult.Rows.Count; i++)
            {
                String NodeName = Null2String(allCollectResult.Rows[i][0]);
                string isModelStr = Null2String(allCollectResult.Rows[i][2]);
                string ApproveDate = Null2String(allCollectResult.Rows[i][3]);
                Boolean isModel = false;
                if (isModelStr != "")
                {
                    //c.Flag,标识是否是'Model'
                    if (isModelStr == "Model")
                    {
                        isModel = true;
                    }
                }
                //需要Approve过的model才加入refreshModel的表中
                if (isModel == true && NodeName != "" && ApproveDate != "" && !modelIds.ContainsKey(NodeName))
                {
                    modelIds.Add(NodeName, NodeName);
                }

                try
                {
                    parents.Add(NodeName, NodeName);
                }
                catch
                {
                    //不需要处理
                }
                //if (currentPartNo!= "" && currentPartNo == NodeName)
                //{
                //    isExist = true;
                //}

            }
        }


        private void GetAllChildren(string component, Dictionary<string, string> children)
        {
            children.Add(component, component);
            DataTable allCollectResult = new DataTable();
            DealGetAllChildrenInfo(component, allCollectResult);

            for (int i = 0; i < allCollectResult.Rows.Count; i++)
            {
                String partName = Null2String(allCollectResult.Rows[i][0]);
                try
                {
                    children.Add(partName, partName);
                }
                catch
                {
                    //不需要处理
                }
            }

        }

        private void DealGetAllChildrenInfo(string code, DataTable allCollectResult)
        {
            DataTable getData = GetChildrenInfo(code);
            allCollectResult.Merge(getData, true, System.Data.MissingSchemaAction.Add);
            int i = 1;
            while (getData.Rows.Count > 0)
            {
                if (i >= BOM_MAX_DEEP_LEVEL)
                {
                    break;
                }
                getData = GetAllChildrenInfo(getData, allCollectResult);
                i = i + 1;
            }
        }

        private DataTable GetAllChildrenInfo(DataTable codeList, DataTable collectResult)
        {
            DataTable result = new DataTable();
            //for (int i = 0; i < codeList.Rows.Count; i++)
            //{
            //    string currentCode = Null2String(codeList.Rows[i][0]);
            //    result.Merge(GetChildrenInfo(currentCode), true, System.Data.MissingSchemaAction.Add);
            //}
            List<string> partList = new List<string>();
            for (int i = 0; i < codeList.Rows.Count; i++)
            {
                string currentCode = Null2String(codeList.Rows[i][0]);
                partList.Add(currentCode);
            }
            result = GetChildrenInfo(partList);
            collectResult.Merge(result, true, System.Data.MissingSchemaAction.Add);

            return result;

        }

        private void DealGetAllParentInfo(string code, DataTable allCollectResult)
        {
            DataTable getData = GetParentInfo(code);
            allCollectResult.Merge(getData, true, System.Data.MissingSchemaAction.Add);
            int i = 1;
            while (getData.Rows.Count > 0)
            {
                if (i >= BOM_MAX_DEEP_LEVEL)
                {
                    break;
                }
                getData = GetAllParentInfo(getData, allCollectResult);
                i = i + 1;
            }
        }

        //取得包括当前节点在内的需要记录在refreshModel表中的记录，放在modelIds中
        private void GetNeedRefreshModelIncludeCurrentList(ChangeNodeInfoDef current, Hashtable modelIds)
        {
            Dictionary<string, string> parents = new Dictionary<string, string>();
            GetNeedRefreshModelIncludeCurrentList(current, modelIds, parents);
        }

        //取得包括当前节点在内的需要记录在refreshModel表中的记录，放在modelIds中
        private void GetNeedRefreshModelIncludeCurrentList(ChangeNodeInfoDef current, Hashtable modelIds, Dictionary<string, string> parents)
        {
            //获取current是否approve过的model
            ChangeNodeInfoDef parent = new ChangeNodeInfoDef();
            parent.NodeName = current.NodeName;
            parent.IsModel = false;
            IModelRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            Model item = itemRepository.Find(parent.NodeName);
            if (item != null && Null2String(item.BOMApproveDate) != "")
            {
                parent.IsModel = true;
            }
            //取得需要记到数据库refreshModel中的数据
            GetNeedRefreshModel(parent, modelIds, parents);
        }

        private static Boolean JudgeErrorParentChildren(Dictionary<string, string> parents, Dictionary<string, string> children, Boolean isExist)
        {
            foreach (String partName in children.Keys)
            {
                if (parents.ContainsKey(partName))
                {
                    isExist = true;
                    break;
                }
            }
            return isExist;
        }


        private DataRow CloneDataRow(DataRow item, DataTable result)
        {
            DataRow dr = result.NewRow();
            for (int j = 0; j < item.ItemArray.Count(); j++)
            {
                dr[j] = Null2String(item[j]);
            }
            return dr;
        }

        //"SELECT distinct Component from ModelBOM where Material='" + code + "' AND Flag=1";
        private DataTable GetChildrenInfo(string code)
        {
            //datatable里面排序，distinct
            DataTable result = new DataTable();
            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOMNodeData>();
                result = itemRepository.GetComponentByMaterial(code);
                if (result == null)
                {
                    return new DataTable();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        //与上面同，重载， 参数是list, sql是in
        private DataTable GetChildrenInfo(List<string> codes)
        {
            //datatable里面排序，distinct
            DataTable result = new DataTable();
            if (codes.Count == 0)
            {
                return result;
            }

            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOMNodeData>();
                result = itemRepository.GetComponentByMaterial(codes);
                if (result == null)
                {
                    return new DataTable();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        //被DealGetAllParentInfo调用
        private DataTable GetAllParentInfo(DataTable codeList, DataTable collectResult)
        {
            DataTable result = new DataTable();
            List<string> partList = new List<string>();
            for (int i = 0; i < codeList.Rows.Count; i++)
            {
                string currentCode = Null2String(codeList.Rows[i][0]);
                //result.Merge(GetParentInfo(currentCode),true,System.Data.MissingSchemaAction.Add);
                partList.Add(currentCode);
            }
            result = GetParentInfo(partList);
            collectResult.Merge(result, true, System.Data.MissingSchemaAction.Add);

            return result;

        }

        //同上，但是参数是List, Sql是in
        public DataTable GetParentInfo(List<string> codes)
        {
            //datatable里面排序，distinct
            DataTable result = new DataTable();
            if (codes.Count == 0)
            {
                return result;
            }

            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOMNodeData>();
                result = itemRepository.GetParentInfo(codes);
                if (result == null)
                {
                    return new DataTable();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        #endregion



     

       // #region Implementation of IModelBOM
       // //public IList<CustomerInfo> GetCustomerList()
       // //{

       // //    IList<CustomerInfo> retLst = null;

       // //    try
       // //    {
       // //        IMiscRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
       // //        retLst = itemRepository.GetCustomerList();
       // //        return retLst;
       // //    }
       // //    catch (Exception)
       // //    {
       // //        throw;
       // //    }
       // //}

       // /// <summary>
       // /// Method to get all types
       // /// </summary>		
       // /// <returns>Interface to Model Collection Generic of vs</returns>
       // /// 
       // //取得所有部件类型，flag区分from哪个表
       //             //string strSql = "SELECT distinct Material_group, flag=1 FROM ModelBOM union ";
       //     //strSql += "SELECT distinct PartType as Material_group, flag=0  from PartType order by Material_group";


       // //SELECT distinct Material_group, flag=1 FROM ModelBOM Where ModelBOM.flag=1
       // //union 
       // //SELECT distinct PartType as Material_group, flag=0 from Part  
       // //where PartType not in (SELECT distinct Material_group FROM ModelBOM Where ModelBOM.flag=1)
       // //order by Material_group

       // //change to
       // //SELECT distinct Material_group, flag=1 FROM ModelBOM  
       // //union 
       // //SELECT DISTINCT PartType , flag=0 
       // //FROM Part a left join (SELECT DISTINCT Material_group FROM ModelBOM)  as b on a.PartType=b.Material_group
       // //WHERE b.Material_group is null
       // //order by Material_group
       // public IList<string> GetModelBOMTypes()
       // {
       //     List<string> retLst = new List<string>();
       //     try
       //     {
       //         IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOM>();
       //         DataTable result = itemRepository.GetModelBOMTypes();
       //         for (int i = 0; i < result.Rows.Count; i++)
       //         {
       //             String item = Null2String(result.Rows[i][0]).Trim() + "&";
       //             if (result.Rows[i][1] != null)
       //             {
       //                 item = item + Int32.Parse(Null2String(result.Rows[i][1]));
       //             }
       //             retLst.Add(item);
       //         }

       //         return retLst;
       //     }
       //     catch (Exception)
       //     {
       //         throw;
       //     }
       // }

       // /// <summary>
       // /// Method to get all codes
       // /// </summary>
       // /// <param name="parentCode">Unique identifier for a ModelBOM</param>
       // /// <param name="match">match string</param>
       // /// <returns>Interface to Model Collection Generic of vs</returns>
       // /// 

       // //SELECT distinct Material, BOMApproveDate, Model .Editor FROM ModelBOM mb left JOIN Model 
       // //    on (mb.Material = Model.Model) where Material_group='" + parentCode + "' AND mb.Flag=1
        
       // //增加需求： DataTable GetPartNoByType(String type)

       // //SELECT [PartNo] FROM [Part] where [PartType] ='type' and PartNo like '%" + match + "%' order by [PartNo]";

       // public IList<string> GetModelBOMCodes(string fromType, string parentCode, string match)
       // {

       //     List<string> retLst = new List<string>();
       //     try
       //     {
       //         IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOM>();

       //         if (fromType == "1")
       //         {
       //             DataTable result = itemRepository.GetModelBOMCodes(parentCode, match);
       //             for (int i = 0; i < result.Rows.Count; i++)
       //             {
       //                 string item;
       //                 if (result.Rows[i][1].ToString() == "")
       //                 {
       //                     item = Null2String(result.Rows[i][0]) + "&";
       //                 }
       //                 else
       //                 {
       //                     string str1 = result.Rows[i][0].ToString().Trim();
       //                     string str2 = ((DateTime)result.Rows[i][1]).ToString("yyyy-MM-dd");
       //                     string str3 = Null2String(result.Rows[i][2]);
       //                     item = str1 + "&" + str2 + "&" + str3;
       //                 }

       //                 retLst.Add(item);
       //             }
       //         }
       //         else
       //         {
       //             DataTable result = itemRepository.GetPartNoByType(parentCode, match);
       //             for (int i = 0; i < result.Rows.Count; i++)
       //             {
       //                 string item;
       //                 item = Null2String(result.Rows[i][0]) + "&";
       //                 retLst.Add(item);
       //             }
       //         }
       //         return retLst;
       //     }
       //     catch (Exception)
       //     {
       //         throw;
       //     }

       // }

       // /// <summary>
       // /// Method to get specified ModelBOM's children.
       // /// </summary>		
       // /// <param name="code">Unique identifier for a ModelBOM</param>
       // /// <returns>Interface to Model Collection Generic of vs</returns>
       // /// 

       // //SELECT distinct(Material), Material_group, Component FROM ModelBOM where Component='+ code + ' and Flag=1
       // public IList<ModelBOMInfo> GetParentModelBOMByCode(string code)
       // {
       //     List<ModelBOMInfo> retLst = new List<ModelBOMInfo>();
       //     try
       //     {
       //         IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOM>();
       //         DataTable result = itemRepository.GetParentModelBOMByCode(code);
       //         for (int i = 0; i < result.Rows.Count; i++)
       //         {
       //             ModelBOMInfo info = new ModelBOMInfo();
       //             info.Material = Null2String(result.Rows[i][0]).Trim();
       //             info.Material_group = Null2String(result.Rows[i][1]).Trim();
       //             info.Quantity = Null2String(result.Rows[i][2]).Trim();

       //             retLst.Add(info);
       //         }

       //         return retLst;
       //     }
       //     catch (Exception)
       //     {
       //         throw;                
       //     }

       // }

       // /// <summary>
       // /// Method to get specified ModelBOM's children.
       // /// </summary>
       // /// <param name="code">Unique identifier for a ModelBOM</param>
       // /// <returns>Interface to Model Collection Generic of vs</returns>
       // /// 

       // //string strSql = "SELECT Component as Material, Material_group, Quantity, Alternative_item_group FROM ModelBOM where Material='" + code + "'";

       ////SELECT a.Component as Material, b.Material_group, a.Quantity, 
       //// a.Alternative_item_group FROM ModelBOM AS a
       //// inner join (select distinct Material, Material_group from ModelBOM Where Flag=1 
       //// union SELECT distinct PartNo AS Material, PartType as Material_group FROM Part
       //// where PartNo not in (SELECT Material FROM ModelBOM Where Flag=1 )) AS b on a.Component=b.Material 
       //// WHERE a.Material='code' AND a.Flag=1 
       //// order by a.Material

       // //change to:
       // //SELECT a.Component as Material, b.Material_group, a.Quantity, 
       // //a.Alternative_item_group FROM ModelBOM AS a
       // //inner join (select distinct Material, Material_group from ModelBOM 
       // //union 
       // //SELECT distinct PartNo AS Material, PartType as Material_group FROM Part
       // //left outer join (select distinct Material FROM ModelBOM) AS c
       // //ON Part.PartNo=c.Material
       // //where c.Material is null
       // //) AS b on a.Component=b.Material 
       // //WHERE a.Material='code' 
       // //order by a.Material

       // public IList<ModelBOMInfo> GetSubModelBOMByCode(string code)  //!!!
       // {
       //     IList<ModelBOMInfo> retLst = new List<ModelBOMInfo>();

       //     try
       //     {
       //         IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOM>();
       //         DataTable result = itemRepository.GetSubModelBOMByCode(code);
       //         for (int i = 0; i < result.Rows.Count; i++)
       //         {
       //             ModelBOMInfo info = new ModelBOMInfo();
       //             info.Material = Null2String(result.Rows[i][0]).Trim();
       //             info.Material_group = Null2String(result.Rows[i][1]).Trim();
       //             info.Quantity = Null2String(result.Rows[i][2]).Trim();
       //             info.Alternative_item_group = Null2String(result.Rows[i][3]).Trim();
       //             retLst.Add(info);
       //         }

       //         return retLst;
       //     }
       //     catch (Exception)
       //     {
       //         throw;                
       //     }

       // }

       // /// <summary>
       // /// Method to get specified ModelBOM's alternative items.
       // /// <param name="code">Unique identifier for a ModelBOM</param>
       // /// <param name="alternativeItemGroup">Alternative item group for a ModelBOM</param>
       // /// </summary>		
       // /// <returns>Interface to Model Collection Generic of ModelBOMInfo</returns>
       // /// 

       // //   SELECT distinct Component as Material from ModelBOM where Material='code '
       // //and Alternative_item_group='alternativeItemGroup'
       // //AND Flag=1

       // public IList<ModelBOMInfo> GetAlternativeItems(string code, string alternativeItemGroup)
       // {
       //     IList<ModelBOMInfo> retLst = new List<ModelBOMInfo>();

       //     try
       //     {
       //         IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOM>();
       //         DataTable result = itemRepository.GetAlternativeItems(code, alternativeItemGroup);
       //         for (int i = 0; i < result.Rows.Count; i++)
       //         {
       //             ModelBOMInfo info = new ModelBOMInfo();
       //             info.Material = Null2String(result.Rows[i][0]).Trim();
       //             retLst.Add(info);
       //         }

       //         return retLst;
       //     }
       //     catch (Exception)
       //     {
       //         throw;
       //     }

       // }

       //     //        string strSql = "SELECT distinct(Material), AssemblyCode FROM ModelBOM  left join MO on ModelBOM.Material=MO.Model ";
       //     //strSql += "left join MoBOM on MO.MO=MoBOM.MO ";
       //     //strSql += "where Material in (select Component from ModelBOM where Material='" + code + "')";
       // //change to     SELECT distinct Component from ModelBOM where Material='" + code + "' AND Flag=1
       // //!!!
       // private IList<string> GetOffspringModelBOM(string code)
       // {
       //     IList<string> retLst = new List<string>();
       //     try
       //     {
       //         IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOM>();
       //         //DataTable result = itemRepository.GetOffspringModelBOM(code);
       //         DataTable result = itemRepository.GetComponentByMaterial(code);
       //         for (int i = 0; i < result.Rows.Count; i++)
       //         {
       //             String item;
       //             //String strTmp=Null2String(result.Rows[i][1]);
       //             //if (strTmp == "")
       //             //{
       //                 item = Null2String(result.Rows[i][0]);
       //             //}
       //             //else
       //             //{
       //             //    item = Null2String(result.Rows[i][0]).Trim() + "(" + strTmp + ")";
       //             //}
       //             retLst.Add(item);
       //         }

       //         return retLst;
       //     }
       //     catch (Exception)
       //     {
       //         throw;                
       //     }

       // }

       // /// <summary>
       // /// Method to get specified ModelBOM's GetOffSprings.
       // /// </summary>
       // /// <param name="code">Unique identifier for a ModelBOM</param>
       // /// <returns>Interface to Model Collection Generic of string</returns>
       // public IList<string> GetOffSpringModelBOMByCode(string code)
       // {
       //     IList<string> temp = GetOffspringModelBOM(code);

       //     IList<string> child;
       //     IList<string> result = new List<string>();

       //     foreach (string item in temp)
       //     {
       //         child = GetOffspringModelBOM(item);
       //         if (child.Count() > 0)
       //         {
       //             result.Add(item + "&1");
       //         }
       //         else
       //         {
       //             result.Add(item + "&0");
       //         }
       //     }
       //     return result;
       // }

       // /// <summary>
       // /// Method to get MoBOMs via a specified model
       // /// </summary>	
       // /// <param name="model">a valid model code</param>
       // /// <returns>Interface to Model Collection Generic of MoBOMInfo</returns>
       // /// 

       // //string strSql = "SELECT distinct(MO), Qty, PrintQty as StartQty, Udt FROM MO where Model='" + model + "'";
       // //public IList<MoBOMInfo> GetMoBOMByModel(string model)
       // //{
       // //    IList<MoBOMInfo> retLst = new List<MoBOMInfo>();

       // //    try
       // //    {
       // //        IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOM>();
       // //        DataTable result = itemRepository.GetMoBOMByModel(model);
       // //        for (int i = 0; i < result.Rows.Count; i++)
       // //        {
       // //            MoBOMInfo info = new MoBOMInfo();
       // //            info.Mo = Null2String(result.Rows[i][0]).Trim();
       // //            info.Qty =  Null2String(result.Rows[i][1]).Trim();
       // //            info.StartQty = Null2String(result.Rows[i][2]).Trim();
       // //            if (result.Rows[i][3].ToString() != "")
       // //            {
       // //                info.UpdateDate = ((DateTime)result.Rows[i][3]).ToString("yyyy-MM-dd");
       // //            }
       // //            retLst.Add(info);

       // //        }

       // //        return retLst;
       // //    }
       // //    catch (Exception)
       // //    {
       // //        throw;                
       // //    }

       // //}

       // /// <summary>
       // /// Method to include an item to alternative item group
       // /// </summary>
       // /// <param name="parent">a valid code</param>
       // /// <param name="code1">a valid code</param>
       // /// <param name="code2">a valid code</param>
       // /// <returns>If set successfully, return 0;otherwise return error code.</returns>
       // /// 

       // // string strSql = "UPDATE ModelBOM SET Alternative_item_group=(SELECT Alternative_item_group from ModelBOM where Material='" + parent + "' and Component='" + code1 + "') where Material='" + parent + "' and Component='" + code2 + "'";
       // //change to:
       // //SELECT Alternative_item_group from ModelBOM where Material='" + parent + "' and Component='" + code + "' AND Flag=1
       // //UPDATE ModelBOM SET Alternative_item_group=”’+value+”’ where Material='" + parent + "' and Component='" + code + "'"  AND Flag=1
       // public int IncludeItemToAlternativeItemGroup(string parent, string code1, string code2)
       // {

       //     try
       //     {
       //         IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOM>();
       //         DataTable result=itemRepository.GetAlternativeItemGroupBySpecial(parent,code1);
                
       //         if(result.Rows.Count>0)
       //         {
       //             String value=result.Rows[0][0].ToString();
       //             itemRepository.IncludeItemToAlternativeItemGroup(value,parent,code2);
       //         }
       //     }
       //     catch (Exception)
       //     {
       //         //throw;
       //         return -1;
       //     }
       //     return 0;
       // }

       // /// <summary>
       // /// Method to include all item to alternative item group
       // /// </summary>
       // /// <param name="parent">a valid code</param>
       // /// <param name="code1">a valid code</param>
       // /// <param name="code2">a valid code</param>
       // /// <returns>If set successfully, return 0;otherwise return error code.</returns>
       // /// 

       // //string strSql = "UPDATE ModelBOM SET Alternative_item_group=(SELECT Alternative_item_group from ModelBOM where Material='" + parent + "' and Component='" + code1 + "') where Material='" + parent + "' and Component in ";
       // //strSql += "(SELECT Component from ModelBOM where Material='" + parent + "' and Alternative_item_group = (select distinct Alternative_item_group from ModelBOM where Material='" + parent + "' and Component='" + code2 + "'))";

       // ////////////////////////////
       // //  UPDATE ModelBOM SET Alternative_item_group='itemGroup1' where Material='code1' and Component in 
       // //(SELECT Component from ModelBOM where Material='code2' and 
       // //Alternative_item_group = 'itemGroup2' AND Flag=1) AND Flag=1  
       // public int IncludeAllItemToAlternativeItemGroup(string parent, string code1, string code2)
       // {
       //     try
       //     {
       //         IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOM>();
       //         DataTable resultAltGroupNew=itemRepository.GetAlternativeItemGroupBySpecial(parent,code1);
                
       //         String valueAltGroupNew=null;
       //         if(resultAltGroupNew.Rows.Count>0)
       //         {
       //             valueAltGroupNew=resultAltGroupNew.Rows[0][0].ToString();
       //         }

       //         String valueAltGroupOld=null;
       //         DataTable resultAltGroupOld=itemRepository.GetAlternativeItemGroupBySpecial(parent,code2);
       //         if(resultAltGroupOld.Rows.Count>0)
       //         {
       //             valueAltGroupOld=resultAltGroupOld.Rows[0][0].ToString();
       //         }
       //         if(valueAltGroupNew!=null && valueAltGroupOld!=null)
       //         {
       //             itemRepository.IncludeAllItemToAlternativeItemGroup(parent, parent, valueAltGroupNew, valueAltGroupOld);
       //         }

       //     }
       //     catch (Exception)
       //     {
       //         //throw;
       //         return -1;
       //     }
       //     return 0;

       // }

       // /// <summary>
       // /// Method to exclude an item from an alternative item group
       // /// </summary>
       // /// <param name="parent">a valid code</param>
       // /// <param name="code">a valid code</param>
       // /// <returns>If exclude the item successfully, return 0, else return error code</returns>
       // /// 

       // //UPDATE ModelBOM SET Alternative_item_group=newid() where Material=' parent ' and Component=' code ' AND Flag=1
       // //返回当时的newid()
       // public string ExcludeAlternativeItem(string parent, string code)
       // {
       //     try
       //     {
       //         string result;
       //         IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOM>();
       //         result=itemRepository.ExcludeAlternativeItem(parent, code);
       //         return result;
       //     }
       //     catch (Exception)
       //     {
       //         //throw;
       //         return "-1";
       //     }
       //     return "0";

       // }

       // //string strSql = "UPDATE ModelBOM SET Alternative_item_group=NULL where Material='" + parent + "' and Component='" + code + "'";       
       // public int ExcludeAlternativeItemToNull(string parent, string code)
       // {
       //     try
       //     {
       //         IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOM>();
       //         itemRepository.ExcludeAlternativeItemToNull(parent, code);
       //     }
       //     catch (Exception)
       //     {
       //         //throw;
       //         return -1;
       //     }
       //     return 0;

       // }

       // /// <summary>
       // /// Method to delete a sub code from sub code list
       // /// </summary>	
       // /// <param name="code">a valid code</param>
       // /// <returns>If delete the item successfully, return 0, else return error code</returns>
       // /// 

       // //string delSql = "DELETE FROM ModelBOM where Material='" + parentCode + "' and Component='" + code + "'";
       // //change to 
       // //       UPDATE [IMES_GetData_Datamaintain].[dbo].[ModelBOM]
       // //  SET [Flag] = 0
       // //WHERE  Material=' parentCode ' and Component=' code '
       // public int DeleteModelBOMByCode(string parentCode, string code)
       // {
       //     try
       //     {
       //         IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOM>();
       //         itemRepository.DeleteModelBOMByCode(parentCode, code);
       //     }
       //     catch (Exception)
       //     {
       //         //throw;
       //         return -1;
       //     }
       //     return 0;

       // }

       // //string strSql = "SELECT distinct Component from ModelBOM where Material='" + code + "'";
       // //string delSql = "DELETE FROM ModelBOM where Material='" + code + "'";

       // //no use?
       // private int DeleteSubModelByCode(string code, UnitOfWork unit)
       // {
       //     try
       //     {
       //         IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOM>();
       //         DataTable result=itemRepository.GetComponentByMaterial(code);
       //         for (int i = 0; i < result.Rows.Count; i++)
       //         {
       //             if (result.Rows[i][0] == null)
       //             {
       //                 itemRepository.DeleteSubModelByCodeDefered(unit,code);
       //             }
       //             else
       //             {
       //                 DeleteSubModelByCode(result.Rows[i][0].ToString(),unit);
       //             }
       //         }

       //     }
       //     catch (Exception)
       //     {
       //         //throw;
       //         return -1;
       //     }
       //     return 0;
       // }

       // /// <summary>
       // /// Method to add an item
       // /// </summary>
       // /// <param name="parentCode">identifier of parent</param>
       // /// <param name="oldCode">identifier of an item</param>
       // /// <param name="item">ModelBOMInfo object</param>
       // /// <param name="bAddNew">Add new ModelBOM</param>
       // /// <returns>If adding an item successfully, return 0, else return error code</returns>
       // /// 

       // //strSql = "SELECT Material from ModelBOM where Material = '" + code + "' " AND Flag=1
       // //strSql += "UNION SELECT PartNo as Material from Part where PartNo='" + code + "'";
       // public int UpdateModelBOM(string parentCode, string type, string oldCode, string code, string qty, bool bAddNew, string editor) 
       // {

       //     try
       //     {
       //         bool bUpdate = false;
       //         IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOM>();
       //         DataTable result=itemRepository.GetMaterialsByCode(code);
       //         for (int i = 0; i < result.Rows.Count; i++)
       //         {
       //             if (result.Rows[i][0] != null)
       //             {
       //                 bUpdate = true;
       //             }
       //         }

       //         int ret = -2;
       //         if (!bUpdate)
       //         {
       //             return ret;
       //         }

       //         if (bAddNew)
       //         {
       //             //strSql = "INSERT ModelBom (Material, Material_group, Component, Quantity) values('" + parentCode + "','" + type + "','" + oldCode + "','" + qty + "')";

       //             IList<ModelBOM> itemList = itemRepository.findModelBomByMaterialAndComponent(parentCode, code);
       //             if (itemList.Count > 0)
       //             {
       //                 throw new ApplicationException("Already exist the relation in database.");
       //             }

       //             //!!! need
       //             //需在一个事物中
       //             //itemRepository.DeleteDelModelBomByMaterialAndComponent(parentCode, code);

       //             ModelBOM item = new ModelBOM();
       //             item.Material = parentCode;
       //            // item.Material_group = type;
       //             item.Component = code;
       //             item.Quantity = qty;
       //             item.Editor = editor;
       //             itemRepository.AddModelBOM (item);

       //         }
       //         else
       //         {
       //             IList<ModelBOM> itemListTest = itemRepository.findModelBomByMaterialAndComponent(parentCode, code);
       //             if (itemListTest.Count > 0 && oldCode != code)
       //             {
       //                 throw new ApplicationException("Already exist the relation in database.");
       //             }

       //             UnitOfWork unit = new UnitOfWork();
       //             IList<ModelBOM> itemList=itemRepository.findModelBomByMaterialAndComponent(parentCode,oldCode);

       //             //需在一个事物中
       //             if (itemList.Count > 0)
       //             {
       //                 //itemRepository.DeleteDelModelBomByMaterialAndComponent(parentCode, code);                  
       //             }
       //             for (int i = 0; i < itemList.Count; i++)
       //             {
       //                 ModelBOM item = itemList[i];
       //                 item.Component = code;
       //                 item.Quantity = qty;
       //                 item.Editor = editor;
       //                 itemRepository.UpdateModelBOMDefered(unit, item);
       //             }
       //             unit.Commit();
       //             //strSql = "UPDATE ModelBom set Component='" + code + "',Quantity='" + qty + "' where Material='" + parentCode + "' and Component='" + oldCode + "'";
       //         }

       //     }
       //     catch (Exception)
       //     {
       //         throw;
       //     }
       //     return 1;

       // }

   

       // /// <summary>
       // /// Method to test if a code is already exist in ModelBOM table
       // /// </summary>	
       // /// <param name="code">a valid code</param>
       // /// <returns>If exist, return 0;otherwise return error code.</returns>
       // /// 

       // //string strSql = "SELECT Material FROM ModelBOM where Material='" + code + "'" AND Flag=1
       // //strSql = "SELECT Model FROM Model where Model='" + code + "'";
       // public int IsCodeExist(string code)
       // {
       //    int ret = 0;
       //    try
       //     {
       //         IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOM>();
       //         DataTable result1=itemRepository.GetMaterialById(code);
                
       //         ret = result1.Rows.Count>0 ? 1 : 0;
       //         DataTable result2=itemRepository.GetModelById(code);
       //         ret+=result2.Rows.Count>0 ? 2 : 0;

       //     }
       //     catch (Exception)
       //     {
       //         throw;
       //     }
       //     return ret;
              
       // }

       // //string strSql = "SELECT Material FROM ModelBOM where Material='" + code + "'" AND Flag=1
       // public int IsCodeExistInModelBOM(string code)
       // {
       //     int ret = 0;
       //     try
       //     {
       //         IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOM>();
       //         DataTable result1 = itemRepository.GetMaterialById(code);
       //         ret = result1.Rows.Count > 0 ? 1 : 0;
       //     }
       //     catch (Exception)
       //     {
       //         throw;
       //     }
       //     return ret;

       // }

       // /// <summary>
       // /// Method to save a code as a new code
       // /// </summary>	
       // /// <param name="code">a valid code</param>
       // /// <param name="newCode">a valid code</param>
       // /// <returns>If operate successfully, return 0;otherwise return error code.</returns>

       // // "execute SaveModelBOMAs '" + code + "', '" + newCode + "'";
       // public int SaveCodeAs(string code, string newCode)
       // {
       //     int ret = 0;
       //     //try
       //     //{
       //     //    IMiscRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
       //     //    itemRepository.SaveModelBOMAs(code, newCode);
       //     //}
       //     //catch (Exception)
       //     //{
       //     //    //throw;
       //     //    return -1;
       //     //}
       //     return ret;
       // }

       // //strSql = "SELECT Component, Quantity FROM ModelBOM where Material='" + model + "'";

       // //no use?
       // private string GetMo(string model, int level, int qty)
       // {
       //     // Force to return when the level is over 10 to avoid infinite recurrence
       //     if (level >= 10)
       //     {
       //         return "";
       //     }
       //     string ret = "";
       //     try
       //     {
       //         IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOM>();
       //         DataTable result=itemRepository.GetComponentQuantityByMaterial(model);

       //         if (result.Rows.Count > 0)
       //         {
       //             for (int i = 0; i < result.Rows.Count;i++ )
       //             {
       //                 if (result.Rows[i][0]!=null)
       //                 {
       //                     String value="0";
       //                     if(result.Rows[i][1]!=null)
       //                     {
       //                         value = Null2String(result.Rows[i][1]);
       //                     }
       //                     int tmpQty = int.Parse(value) * qty;
       //                     ret = GetMo(Null2String(result.Rows[i][0]), level + 1, tmpQty);
       //                 }
       //             }
       //         }
       //         else
       //         {
       //             ret = model + "&" + qty;
       //         }
       //     }
       //     catch (Exception)
       //     {
       //         throw;                
       //     }
       //     return ret;

       // }

       
       // /// <summary>
       // /// Method to get type of a specified code
       // /// </summary>	
       // /// <param name="code">a specified code</param>
       // /// <returns>The type of the specified code.</returns>
       // /// 

       //  //strSql = "SELECT Material_group from ModelBOM where Material = '" + code + "' "; 去掉

       // //SELECT     Material_group FROM  ModelBOM WHERE Material = 'code' AND Flag=1 UNION
       // //SELECT     PartType FROM  Part  WHERE PartNo not in (SELECT Material FROM ModelBOM WHERE Flag=1) AND     PartNo ='code'

       // //change to:
       // //SELECT     Material_group FROM  ModelBOM WHERE Material = 'code' UNION
       // //SELECT     PartType FROM  Part 
       // //left outer join ModelBOM on Part.PartNo=ModelBOM.Material
       // // WHERE ModelBOM.Material is NULL
       // //AND     PartNo ='code'
       // public string GetTypeOfCode(string code)
       // {
       //     string retType = null;
       //     try
       //     {
       //         IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOM>();
       //         DataTable result = itemRepository.GetTypeOfCode(code);

       //         for (int i = 0; i < result.Rows.Count; i++)
       //         {
       //             if (result.Rows[i][0] != null)
       //             {
       //                 retType = result.Rows[i][0].ToString().Trim();
       //             }
       //         }

       //     }
       //     catch (Exception)
       //     {
       //         throw;
       //     }
       //     return retType;

       // }
        
       // #endregion










        //SELECT a.Component as Material, case when b.dataFromType is null then b.PartType else c.PartType end AS PartType, a.Quantity, 
        //a.Alternative_item_group, a.Priority 
        //,a.[Editor],a.[Cdt],a.[Udt],a.ID FROM ModelBOM AS a
        //left outer join 
        //(
        //  select distinct Material AS Code, Material_group AS PartType, 0 as dataFromType from ModelBOM where Flag=1
        //) AS b on b.Code=a.Component
        //left outer join 
        //(
        //  SELECT [PartNo] AS Code ,[PartType]  AS PartType    
        //  FROM [IMES_GetData_Datamaintain].[dbo].[Part] where Flag=1
        //) AS c on c.Code=a.Component
        //WHERE  a.Flag=1 AND a.ID IN (idList)
        //order by a.Alternative_item_group, a.Priority
        //needIDList 需要返回的list
        public DataTable GetSubModelBOMList(List<string> needIDList)
        {
            DataTable result = new DataTable();
            if (needIDList.Count == 0)
            {
                return result;
            }
            try
            {
                IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOMNodeData>();
                List<int> idList = new List<int>();
                for (int i = 0; i < needIDList.Count; i++)
                {
                    int id = int.Parse(needIDList[i]);
                    idList.Add(id);

                }
                result = itemRepository.GetSubModelBOMByCode(idList);
                //if (needIDList.Count != result.Rows.Count)
                //{
                //    List<string> erpara = new List<string>();
                //    FisException ex;
                //    //!!!need change right!!!
                //    //数据库中的数据已经被其他人修改，建议重新输入[Model/PN]，刷新界面以获取最新数据
                //    ex = new FisException("DMT001", erpara);
                //    throw ex;
                //}

            }
            catch (Exception)
            {
                throw;
            }
            return result;

        }


        // /// <summary>
       // /// Method to approve a model
       // /// </summary>	
       // /// <param name="code">a valid code</param>
       // /// <param name="op">operator</param>
       // /// <returns>If approve successfully, return 0;otherwise return error code.</returns>
       // /// 

       // //string strSql = "UPDATE Model SET Editor='" + editor + "', BOMApproveDate = '" + DateTime.Now + "' where Model='" + model + "'";
       // public ApproveInfoDef ApproveModel(string model, string editor) 
       // {

       //     ApproveInfoDef result = new ApproveInfoDef();
       //     try
       //     {
       //         UnitOfWork uow = new UnitOfWork();
       //         IModelRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
       //         Model item = itemRepository.Find(model);
       //         if (item == null)
       //         {
       //             List<string> erpara = new List<string>();
       //             FisException ex;
       //             //!!!need change
       //             //这条Model记录已经不存在，不能被Approve
       //             ex = new FisException("DMT015", erpara);
       //             throw ex;
       //         }

       //         item.Editor = editor;
       //         item.BOMApproveDate = DateTime.Now;
       //         item.Udt = DateTime.Now;

       //         //返回值
       //         result.Editor = item.Editor;
       //         result.ApproveDate= item.BOMApproveDate.ToString("yyyy-MM-dd"); 

       //         itemRepository.Update(item, uow);
       //         uow.Commit();

       //     }
       //     catch (Exception)
       //     {
       //         throw;                
       //     }
       //     return result;
       // }

        

      

       



       

      

       

     

       
        
       

      

       
       
       
      

       // //DataTable GetRefreshModelList(string editor)
       // //SELECT Model FROM RefreshModel  WHERE Editor=’ editor’ 

       // public List<RefreshMoBomInfoDef> GetRefreshMOBomList(string editor)
       // {
       //     List<RefreshMoBomInfoDef> result = new List<RefreshMoBomInfoDef>();
       //     try
       //     {
       //         //GetRefreshModelList();
       //         IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOM>();

       //         DataTable ModelList = itemRepository.GetRefreshModelList(editor);
       //         for (int i = 0; i < ModelList.Rows.Count; i++)
       //         {
       //             RefreshMoBomInfoDef item = new RefreshMoBomInfoDef();
       //             string model = Null2String(ModelList.Rows[i][0]);
       //             item.Model = model;
       //             //SELECT distinct(MO), Qty, PrintQty as StartQty, Udt FROM MO where Model='" + model + ' AND Status <>'C'

       //             IMORepository itemRepositoryMo = RepositoryFactory.GetInstance().GetRepository<IMORepository>();
       //             DataTable resultList = itemRepositoryMo.GetMoByModel(model);
                    
       //             for (int j = 0; j < resultList.Rows.Count; j++)
       //             {
       //                 MoBomNodeInfoDef MoBomNode = new MoBomNodeInfoDef();
       //                 MoBomNode.MO = Null2String(resultList.Rows[j][0]);
       //                 MoBomNode.Qty = Null2String(resultList.Rows[j][1]);
       //                 MoBomNode.PrintQty = Null2String(resultList.Rows[j][2]);
       //                 item.MoBomItem.Add(MoBomNode);
       //             }
       //             result.Add(item);

       //         }    

       //     }
       //     catch (Exception)
       //     {
       //         throw;
       //     }
       //     return result;
       // }


       // //!!!注意，前台Refresh提交时，Model下没有MO的也提交，只不过List中单元数为0
       // //部分不成功的显示出来，成功的继续
       // public void RefreshBOM(List<RefreshMoBomInfoDef> needRefresh, string editor)
       // {
       //     List<string> faildModelList=new List<string> ();
            
       //     for (int i = 0; i < needRefresh.Count; i++)
       //     {
       //         string model = Null2String(needRefresh[i].Model);
       //         try
       //         {                    
       //             List<string> lstMo = new List<string>();
       //             for (int j = 0; j < needRefresh[i].MoBomItem.Count; j++)
       //             {
       //                 lstMo.Add(Null2String(needRefresh[i].MoBomItem[j].MO));
       //             }
       //             RefreshBOM(lstMo, model, editor);
       //         }
       //         catch
       //         {
       //             faildModelList.Add(model);
       //         }
       //     }

       //     if (faildModelList.Count > 0)
       //     {
       //         List<string> erpara = new List<string>();
       //         FisException ex;
       //         //!!!need change
       //         //以下MO没有Save成功
       //         //有MO的Refresh BOM操作因未知原因而失败，请重新执行Refresh操作
       //         ex = new FisException("DMT020", erpara);
       //         throw ex;
       //     }
        
       // }

       // /// <summary>
       // /// Method to refresh MoBOM
       // /// </summary>	
       // /// <param name="mo">Array of mo</param>
       // /// <param name="model">model</param>
       // /// <returns>If operate successfully, return 0;otherwise return error code.</returns>
       // private void RefreshBOM(List<string> mo, string model,string editor)
       // {

       //     try
       //     {
       //         IModelBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IModelBOMRepository, BOM>();
       //         UnitOfWork unit = new UnitOfWork();
       //         IMiscRepository itemRepository2 = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();

       //         string moList="";
       //         for (int j = 0; j<mo.Count; j++)
       //         {
       //             itemRepository.DeleteMOBOMByMoDefered(unit, mo[j]);
       //             if (j != 0)
       //             {
       //                 moList =moList+ "~" ;                        
       //             }
       //             moList = moList + mo[j];
       //         }
       //         //itemRepository2.GenerateVirtualMoBOMDefered(unit, model, BOM_MAX_DEEP_LEVEL, moList);
       //         if (mo.Count > 0)
       //         {
       //             itemRepository2.GenerateVirtualMoBOMForMaintainDefered(unit, model, BOM_MAX_DEEP_LEVEL, moList);
       //         }
       //         //Delete From RefreshModel where Model='model'
       //         itemRepository.DeleteRefreshModelByModelDefered(unit, model, editor);
       //         unit.Commit();
       //     }
       //     catch (Exception)
       //     {
       //         throw;
       //     }
       // }

       // //ModelBOM GetModelBOM(int modelBOMId)
    }
}