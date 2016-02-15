using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.inventec.imes.Model;

namespace com.inventec.imes.IDAL
{
    public interface IModelBOM
    {
        /// <summary>
        /// Method to get all ModelBOMs
        /// </summary>		
        /// <returns>Interface to Model Collection Generic of vs</returns>
        //IList<ModelBOMInfo> GetModelBOMs();

        /// <summary>
        /// Get information on a specific ModelBOM
        /// </summary>
        /// <param name="modelBOMId">Unique identifier for a ModelBOM</param>
        /// <returns>Business Entity representing a ModelBOM</returns>
        ModelBOMInfo GetModelBOM(long modelBOMId);

        /// <summary>
        /// Method to get all types
        /// </summary>		
        /// <returns>Interface to Model Collection Generic of string</returns>
        IList<string> GetModelBOMTypes();

        /// <summary>
        /// Method to get all codes
        /// </summary>	
        /// <param name="parentCode">Unique identifier for a parent ModelBOM</param>
        /// <param name="match">define the match string to search the desire range of ModelBOMs</param>
        /// <returns>Interface to Model Collection Generic of string</returns>
        IList<string> GetModelBOMCodes(string parentCode, string match);

        /// <summary>
        /// Method to get specified ModelBOM's parent.
        /// </summary>
        /// <param name="code">Unique identifier for a ModelBOM</param>
        /// <returns>Interface to Model Collection Generic of ModelBOMInfo</returns>
        IList<ModelBOMInfo> GetParentModelBOMByCode(string code);

        /// <summary>
        /// Method to get specified ModelBOM's Offspring.
        /// </summary>
        /// <param name="code">Unique identifier for a ModelBOM</param>
        /// <returns>Interface to Model Collection Generic of vs</returns>
        IList<string> GetOffSpringModelBOMByCode(string code);

        /// <summary>
        /// Method to get specified ModelBOM's children.
        /// <param name="code">Unique identifier for a ModelBOM</param>
        /// </summary>		
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
        /// Method to remove alternative items.
        /// </summary>
        /// <param name="code">Unique identifier for a ModelBOM</param>
        /// <returns>result</returns>
        int RemoveAlternativeItem(string code);

        /// <summary>
        /// Method to approve a model
        /// </summary>	
        /// <param name="code">a valid code</param>
        /// <param name="op">operator</param>
        /// <returns>If approve successfully, return 0;otherwise return error code.</returns>
        int ApproveModel(string code, string op);

        /// <summary>
        /// Method to get MoBOMs via a specified model
        /// </summary>	
        /// <param name="model">a valid model code</param>
        /// <returns>Interface to Model Collection Generic of MoBOMInfo</returns>
        IList<MoBOMInfo> GetMoBOMByModel(string model);

        /// <summary>
        /// Method to exclude an item from an alternative item group
        /// </summary>
        /// <param name="parent">a valid code</param>
        /// <param name="code">a valid code</param>
        /// <returns>If exclude the item successfully, return 0, else return error code</returns>
        int ExcludeAlternativeItem(string parent, string code);

        /// <summary>
        /// Method to delete a sub code from sub code list
        /// </summary>	
        /// <param name="code">a valid code</param>
        /// <returns>If delete the item successfully, return 0, else return error code</returns>
        int DeleteModelBOMByCode(string parentCode, string code);

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
        /// Method to add an item
        /// </summary>
        /// <param name="parentCode">identifier of parent</param>
        /// <param name="oldCode">identifier of an item</param>
        /// <param name="item">ModelBOMInfo object</param>
        /// <param name="bAddNew">Add new ModelBOM</param>
        /// <returns>If adding an item successfully, return 0, else return error code</returns>
        int UpdateModelBOM(string parentCode, string type, string oldCode, string code, string qty, bool bAddNew);

        /// <summary>
        /// Method to add a new code
        /// </summary>	
        /// <param name="code">a valid code</param>
        /// <param name="type">atype of code</param>
        /// <returns>If add successfully, return 0;otherwise return error code.</returns>
        int AddNewCode(string code, string type);

        /// <summary>
        /// Method to test if a code is already exist in ModelBOM table
        /// </summary>	
        /// <param name="code">a valid code</param>
        /// <returns>If exist, return 0;otherwise return error code.</returns>
        int IsCodeExist(string code);

        /// <summary>
        /// Method to save a code as a new code
        /// </summary>	
        /// <param name="code">a valid code</param>
        /// <param name="newCode">a valid code</param>
        /// <returns>If operate successfully, return 0;otherwise return error code.</returns>
        int SaveCodeAs(string code, string newCode);

        /// <summary>
        /// Method to refresh mo
        /// </summary>	
        /// <param name="mo">Array of mo</param>
        /// <param name="model">model</param>
        /// <returns>If operate successfully, return 0;otherwise return error code.</returns>
        int RefreshBOM(string[] mo, string model);

        /// <summary>
        /// Method to get type of a specified code
        /// </summary>	
        /// <param name="code">a specified code</param>
        /// <returns>The type of the specified code.</returns>
        string GetTypeOfCode(string code);
    }
}
