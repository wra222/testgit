/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: A business component to manage ModelBOM
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ================ =====================================
 * 2009-11-03 Li Hai-Jun       Create                    
 * Known issues:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.inventec.imes.DALFactory;
using com.inventec.imes.IDAL;
using com.inventec.imes.Model;

namespace com.inventec.imes.BLL
{
    public class ModelBOM
    {
        private const int MAX_SUB_COUNT = 6;
        // Get an instance of the ModelBOM DAL using the DALFactory
        private static readonly IModelBOM dal = new com.inventec.imes.SQLServerDAL.ModelBOM();// DataAccess.CreateModelBOM();

        /// <summary>
        /// Method to get all modelbom types
        /// </summary>		
        /// <returns>Interface to Model Collection Generic of string</returns>
        public IList<string> GetModelBOMTypes()
        {
            return dal.GetModelBOMTypes();
        }

        /// <summary>
        /// Method to get all modelbom codes
        /// </summary>
        /// <param name="parentCode">Unique identifier for a ModelBOM</param>
        /// <param name="match">Match string</param>
        /// <returns>Interface to Model Collection Generic of string</returns>
        public IList<string> GetModelBOMCodes(string parentCode, string match)
        {
            return dal.GetModelBOMCodes(parentCode, match);
        }

        /// <summary>
        /// Method to get all sub modelboms of specific code
        /// </summary>
        /// <param name="code">Unique identifier for a ModelBOM</param>
        /// <returns>Interface to Model Collection Generic of ModelBOMInfo</returns>
        public IList<ModelBOMInfo> GetSubModelBOMByCode(string code)
        {
            IList<ModelBOMInfo> result = dal.GetSubModelBOMByCode(code);
            int rows = (int)result.LongCount();
            while (rows < MAX_SUB_COUNT)
            {
                ModelBOMInfo info = new ModelBOMInfo();
                result.Add(info);
                rows++;
            }

            return result;
        }

        /// <summary>
        /// Method to get all alternative items
        /// </summary>
        /// <param name="code">Unique identifier for a ModelBOM</param>
        /// <param name="alternativeItemGroup">alternative item group</param>
        /// <returns>Interface to Model Collection Generic of string</returns>
        public IList<ModelBOMInfo> GetAlternativeItems(string code, string alternativeItemGroup)
        {
            IList<ModelBOMInfo> result = dal.GetAlternativeItems(code, alternativeItemGroup);
            ModelBOMInfo info;

            /*foreach (ModelBOMInfo item in result)
            {
                result.Add(item);
            }*/

            int rows = (int)result.LongCount();
            while (rows < MAX_SUB_COUNT - 1)
            {
                info = new ModelBOMInfo();
                result.Add(info);
                rows++;
            }

            return result;
        }

        /// <summary>
        /// Method to get all parent modelboms of specific code
        /// </summary>
        /// <param name="code">Unique identifier for a ModelBOM</param>
        /// <returns>Interface to Model Collection Generic of ModelBOMInfo</returns>
        public IList<ModelBOMInfo> GetParentModelBOMByCode(string code)
        {
            IList<ModelBOMInfo> result = dal.GetParentModelBOMByCode(code);
            int rows = (int)result.LongCount();

            while (rows < MAX_SUB_COUNT - 3)
            {
                ModelBOMInfo info = new ModelBOMInfo();

                result.Add(info);
                rows++;
            }

            return result;
        }

        /// <summary>
        /// Method to get all OffSpring of specific code
        /// </summary>
        /// <param name="code">Unique identifier for a ModelBOM</param>
        /// <returns>Interface to Model Collection Generic of string</returns>
        public IList<string> GetOffSpringModelBOMByCode(string code)
        {
            return dal.GetOffSpringModelBOMByCode(code);
        }

        /// <summary>
        /// Method to approve a model
        /// </summary>	
        /// <param name="code">a valid code</param>
        /// <param name="op">operator</param>
        /// <returns>If approve successfully, return 0;otherwise return error code.</returns>
        public int ApproveModel(string code, string op)
        {
            return dal.ApproveModel(code, op);
        }

        /// <summary>
        /// Method to get MoBOMs via a specified model
        /// </summary>	
        /// <param name="model">a valid model code</param>
        /// <returns>Interface to Model Collection Generic of MoBOMInfo</returns>
        public IList<MoBOMInfo> GetMoBOMByModel(string model)
        {
            IList<MoBOMInfo> result = dal.GetMoBOMByModel(model);
            int rows = (int)result.LongCount();
            while (rows < MAX_SUB_COUNT)
            {
                MoBOMInfo info = new MoBOMInfo();
                result.Add(info);
                rows++;
            }
            return result;
        }

        /// <summary>
        /// Method to include an item to alternative item group
        /// </summary>
        /// <param name="parent">a valid code</param>
        /// <param name="code1">a valid code</param>
        /// <param name="code2">a valid code</param>
        /// <returns>If set successfully, return 0;otherwise return error code.</returns>
        public int IncludeItemToAlternativeItemGroup(string parent, string code1, string code2)
        {
            return dal.IncludeItemToAlternativeItemGroup(parent, code1, code2);
        }

        /// <summary>
        /// Method to include all item to alternative item group
        /// </summary>
        /// <param name="parent">a valid code</param>
        /// <param name="code1">a valid code</param>
        /// <param name="code2">a valid code</param>
        /// <returns>If set successfully, return 0;otherwise return error code.</returns>
        public int IncludeAllItemToAlternativeItemGroup(string parent, string code1, string code2)
        {
            return dal.IncludeAllItemToAlternativeItemGroup(parent, code1, code2);
        }

        /// <summary>
        /// Method to exclude an item from an alternative item group
        /// </summary>
        /// <param name="parent">a valid code</param>
        /// <param name="code">a valid code</param>
        /// <returns>If exclude the item successfully, return 0, else return error code</returns>
        public int ExcludeAlternativeItem(string parent, string code)
        {
            return dal.ExcludeAlternativeItem(parent, code);
        }

        /// <summary>
        /// Method to exclude an item from an alternative item group
        /// </summary>	
        /// <param name="code">a valid code</param>
        /// <returns>If exclude the item successfully, return 0, else return error code</returns>
        public int DeleteModelBOMByCode(string parentCode, string code)
        {
            return dal.DeleteModelBOMByCode(parentCode, code);
        }

        /// <summary>
        /// Method to add an item
        /// </summary>
        /// <param name="parentCode">identifier of parent</param>
        /// <param name="oldCode">identifier of an item</param>
        /// <param name="item">ModelBOMInfo object</param>
        /// <param name="bAddNew">Add new ModelBOM</param>
        /// <returns>If adding an item successfully, return 0, else return error code</returns>
        public int UpdateModelBOM(string parentCode, string type, string oldCode, string code, string qty, bool bAddNew)
        {
            int ret = 0;
            ret = dal.UpdateModelBOM(parentCode, type, oldCode, code, qty, bAddNew);
            return ret;
        }

        /// <summary>
        /// Method to add a new code
        /// </summary>	
        /// <param name="code">a valid code</param>
        /// <param name="type">atype of code</param>
        /// <returns>If add successfully, return 0;otherwise return error code.</returns>
        public int AddNewCode(string code, string type)
        {
            return dal.AddNewCode(code, type);
        }

        /// <summary>
        /// Method to test if a code is already exist in ModelBOM table
        /// </summary>	
        /// <param name="code">a valid code</param>
        /// <returns>If exist, return 0;otherwise return error code.</returns>
        public int IsCodeExist(string code)
        {
            return dal.IsCodeExist(code);
        }

        /// <summary>
        /// Method to save a code as a new code
        /// </summary>	
        /// <param name="code">a valid code</param>
        /// <param name="newCode">a valid code</param>
        /// <returns>If operate successfully, return 0;otherwise return error code.</returns>
        public int SaveCodeAs(string code, string newCode)
        {
            return dal.SaveCodeAs(code, newCode);
        }

        /// <summary>
        /// Method to refresh mo
        /// </summary>	
        /// <param name="mo">Array of mo</param>
        /// <param name="model">model</param>
        /// <returns>If operate successfully, return 0;otherwise return error code.</returns>
        public int RefreshBOM(string[] mo, string model)
        {
            return dal.RefreshBOM(mo, model);
        }

        /// <summary>
        /// Method to get type of a specified code
        /// </summary>	
        /// <param name="code">a specified code</param>
        /// <returns>The type of the specified code.</returns>
        public string GetTypeOfCode(string code)
        {
            return dal.GetTypeOfCode(code);
        }
    }
}
