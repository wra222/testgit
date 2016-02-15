using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Data;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface  IModelBOM
    {

        /// <summary>
        /// Get information on a specific ModelBOM
        /// </summary>
        /// <param name="modelBOMId">Unique identifier for a ModelBOM</param>
        /// <returns>Business Entity representing a ModelBOM</returns>
        //ModelBOMInfo GetModelBOM(long modelBOMId);
        
        /// <summary>
        /// Method to get all types
        /// </summary>		
        /// <returns>Interface to Model Collection Generic of vs</returns>
        IList<string> GetModelBOMTypes();
        

        /// <summary>
        /// Method to get all codes
        /// </summary>
        /// <param name="parentCode">Unique identifier for a ModelBOM</param>
        /// <param name="match">match string</param>
        /// <returns>Interface to Model Collection Generic of vs</returns>
        IList<string> GetModelBOMCodes(string fromType,string parentCode, string match);
        

        /// <summary>
        /// Method to get specified ModelBOM's children.
        /// </summary>		
        /// <param name="code">Unique identifier for a ModelBOM</param>
        /// <returns>Interface to Model Collection Generic of vs</returns>
        IList<ModelBOMInfo> GetParentModelBOMByCode(string code);
        
        /// <summary>
        /// Method to get specified ModelBOM's children.
        /// </summary>
        /// <param name="code">Unique identifier for a ModelBOM</param>
        /// <returns>Interface to Model Collection Generic of vs</returns>
        IList<ModelBOMInfo> GetSubModelBOMByCode(string code);
        

        /// <summary>
        /// Method to get specified ModelBOM's alternative items.
        /// <param name="code">Unique identifier for a ModelBOM</param>
        /// <param name="alternativeItemGroup">Alternative item group for a ModelBOM</param>
        /// </summary>		
        /// <returns>Interface to Model Collection Generic of ModelBOMInfo</returns>
        IList<ModelBOMInfo> GetAlternativeItems(string code, string alternativeItemGroup);
        
      
        /// <summary>
        /// Method to get specified ModelBOM's GetOffSprings.
        /// </summary>
        /// <param name="code">Unique identifier for a ModelBOM</param>
        /// <returns>Interface to Model Collection Generic of string</returns>
        IList<string> GetOffSpringModelBOMByCode(string code);
        
        /// <summary>
        /// Method to approve a model
        /// </summary>	
        /// <param name="code">a valid code</param>
        /// <param name="op">operator</param>
        /// <returns>If approve successfully, return 0;otherwise return error code.</returns>
        //int ApproveModel(string code, string op);
        
        /// <summary>
        /// Method to get MoBOMs via a specified model
        /// </summary>	
        /// <param name="model">a valid model code</param>
        /// <returns>Interface to Model Collection Generic of MoBOMInfo</returns>
        IList<MoBOMInfo> GetMoBOMByModel(string model);
        

        /// <summary>
        /// Method to include an item to alternative item group
        /// </summary>
        /// <param name="parent">a valid code</param>
        /// <param name="code1">a valid code</param>
        /// <param name="code2">a valid code</param>
        /// <returns>If set successfully, return 0;otherwise return error code.</returns>
        int IncludeItemToAlternativeItemGroup(string parent, string code1, string code2);
        

        /// <summary>
        /// Method to include all item to alternative item group
        /// </summary>
        /// <param name="parent">a valid code</param>
        /// <param name="code1">a valid code</param>
        /// <param name="code2">a valid code</param>
        /// <returns>If set successfully, return 0;otherwise return error code.</returns>
        int IncludeAllItemToAlternativeItemGroup(string parent, string code1, string code2);
        
        /// <summary>
        /// Method to exclude an item from an alternative item group
        /// </summary>
        /// <param name="parent">a valid code</param>
        /// <param name="code">a valid code</param>
        /// <returns>If exclude the item successfully, return 0, else return error code</returns>
        string ExcludeAlternativeItem(string parent, string code);

        /// <summary>
        /// Method to exclude an item to null alternative item group
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        int ExcludeAlternativeItemToNull(string parent, string code);
        
        /// <summary>
        /// Method to delete a sub code from sub code list
        /// </summary>	
        /// <param name="code">a valid code</param>
        /// <returns>If delete the item successfully, return 0, else return error code</returns>
        int DeleteModelBOMByCode(string parentCode, string code);
        
       
        /// <summary>
        /// Method to add an item
        /// </summary>
        /// <param name="parentCode">identifier of parent</param>
        /// <param name="oldCode">identifier of an item</param>
        /// <param name="item">ModelBOMInfo object</param>
        /// <param name="bAddNew">Add new ModelBOM</param>
        /// <returns>If adding an item successfully, return 0, else return error code</returns>
        int UpdateModelBOM(string parentCode, string type, string oldCode, string code, string qty, bool bAddNew, string editor);
       


        /// <summary>
        /// Method to test if a code is already exist in ModelBOM and model table
        /// </summary>	
        /// <param name="code">a valid code</param>
        /// <returns>If exist, return 0;otherwise return error code.</returns>
        int IsCodeExist(string code);
        

        /// <summary>
        /// Method to test if a code is already exist in ModelBOM table
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        int IsCodeExistInModelBOM(string code);

        /// <summary>
        /// Method to save a code as a new code
        /// </summary>	
        /// <param name="code">a valid code</param>
        /// <param name="newCode">a valid code</param>
        /// <returns>If operate successfully, return 0;otherwise return error code.</returns>
        int SaveCodeAs(string code, string newCode);
        
        /// <summary>
        /// Method to refresh MoBOM
        /// </summary>	
        /// <param name="mo">Array of mo</param>
        /// <param name="model">model</param>
        /// <returns>If operate successfully, return 0;otherwise return error code.</returns>
        //int RefreshBOM(string[] mo, string model);
        
        
        /// <summary>
        /// Method to get type of a specified code
        /// </summary>	
        /// <param name="code">a specified code</param>
        /// <returns>The type of the specified code.</returns>
        string GetTypeOfCode(string code);



        /// <summary>
        /// 取得输入的项信息, 判断是否存在于Part表，Model表
        /// </summary>
        /// <param name="pn"></param>
        /// <returns></returns>
        InputMaterialInfoDef GetMaterialInfo(string pn);

        /// <summary>
        /// 复制一个ModelBOM, 以新的Code作为Materrial
        /// </summary>
        /// <param name="code"></param>
        /// <param name="newCode"></param>
        /// <param name="editor"></param>
        /// <param name="isNeedCheck"></param>
        /// <returns></returns>
        string SaveModelBOMAs(string code, string newCode, string editor, Boolean isNeedCheck);

        /// <summary>
        /// 取得树的父子关系
        /// </summary>
        /// <param name="pn"></param>
        /// <returns></returns>
        DataTable GetTreeTable(string pn);

        /// <summary>
        /// 取得子节点的信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="needIDList"></param>
        /// <returns></returns>
        DataTable GetSubModelBOMList(List<string> needIDList);

        /// <summary>
        ///  Approve Model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        ApproveInfoDef ApproveModel(string model, string editor); 

        /// <summary>
        /// 取得直接父节点
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        DataTable GetParentInfo(string code);

        /// <summary>
        /// 取得所有父节点信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        DataTable GetAllParentInfo(string code);

        /// <summary>
        /// 删除多个父节点
        /// </summary>
        /// <param name="idList"></param>
        /// <param name="current"></param>
        void Delete(List<string> idList, ChangeNodeInfoDef current, string editor);

        /// <summary>
        /// 设共用料
        /// </summary>
        /// <param name="idList"></param>
        /// <param name="current"></param>
        void SetNewAlternativeGroup(List<string> idStrList, ChangeNodeInfoDef current, string editor);

        /// <summary>
        /// 添加ModelBOM
        /// </summary>
        /// <param name="item"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        TreeNodeDef AddModelBOM(ModelBOMInfoDef item, ChangeNodeInfoDef current);

        /// <summary>
        /// 保存ModelBOM
        /// </summary>
        /// <param name="item"></param>
        /// <param name="current"></param>
        TreeNodeDef SaveModelBOM(ModelBOMInfoDef item, ChangeNodeInfoDef parent);

        /// <summary>
        /// 取得修改的Model对应的MO的list
        /// </summary>
        /// <returns></returns>
        List<RefreshMoBomInfoDef> GetRefreshMOBomList(string editor);

        /// <summary>
        /// 以当前的ModelBOM，更新MOBOM
        /// </summary>
        /// <param name="needRefresh"></param>
        void RefreshBOM(List<RefreshMoBomInfoDef> needRefresh, string editor);
        

    }
}
