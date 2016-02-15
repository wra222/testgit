<%--
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: Web service
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ================ =====================================
 * 2009-11-09 Li Hai-Jun       Create                    
 * Known issues:
 --%>
<%@ WebService Language="C#"  Class="WebDataAccess" %>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using com.inventec.imes.Model;

//namespace MoBOMMaintain
//{
	/// <summary>
	/// WebDataAccess 的摘要说明
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
	[System.Web.Script.Services.ScriptService]
	public class WebDataAccess : System.Web.Services.WebService
	{

		[WebMethod]
		public string HelloWorld()
		{
			return "Hello World";
		}

		/// <summary>
		/// Method to get all types
		/// </summary>
        /// <param name="code">a valid code</param>
        /// <param name="match">part of a valid code</param>
		/// <returns>string array</returns>
		[WebMethod]
		public string[] GetModelBOMTypes(string code, string match)
		{
			com.inventec.imes.BLL.ModelBOM modelbom = new com.inventec.imes.BLL.ModelBOM();
            IList<string> types = modelbom.GetModelBOMCodes(code, match);

			return types.ToArray();
		}
        
        /// <summary>
		/// Method to get all types
		/// </summary>		
		/// <returns>Interface to Model Collection Generic of vs</returns>
		[WebMethod]
        public string[] GetModelBOMCodes(string parentcode, string match)
		{
			com.inventec.imes.BLL.ModelBOM modelbom = new com.inventec.imes.BLL.ModelBOM();
            IList<string> types = modelbom.GetModelBOMCodes(parentcode, match);

			return types.ToArray();
		}
        
        /// <summary>
		/// Method to get all types
		/// </summary>		
		/// <returns>Interface to Model Collection Generic of vs</returns>
        [WebMethod]
        public int DeleteModelBOMByCode(string parentCode, string code)
        {
            // To do: call bll's method to delete modelbom
            com.inventec.imes.BLL.ModelBOM modelbom = new com.inventec.imes.BLL.ModelBOM();
            int ret = modelbom.DeleteModelBOMByCode(parentCode, code);
            return ret; 
        }

        /// <summary>
        /// Method to approve a model
        /// </summary>	
        /// <param name="code">a valid code</param>
        /// <param name="op">operator</param>
        /// <returns>If approve successfully, return 0;otherwise return error code.</returns>
        [WebMethod]
        public int ApproveModel(string code, string op)
        {
            // To do: call bll's method to delete modelbom
            com.inventec.imes.BLL.ModelBOM modelbom = new com.inventec.imes.BLL.ModelBOM();
            modelbom.ApproveModel(code, op);
            return 0;
        }

        /// <summary>
        /// Method to save alternative item
        /// </summary>	
        /// <param name="code1">a valid code</param>
        /// <param name="code2">a valid code</param>
        /// <returns>If set successfully, return 0;otherwise return error code.</returns>
        [WebMethod]
        public int IncludeItemToAlternativeItemGroup(string parent, string code1, string code2)
        {
            // To do: call bll's method to set code2's alternative item group to code1's.
            com.inventec.imes.BLL.ModelBOM modelbom = new com.inventec.imes.BLL.ModelBOM();
            return modelbom.IncludeItemToAlternativeItemGroup(parent, code1, code2);
        }

        /// <summary>
        /// Method to exclude an item from an alternative item group
        /// </summary>
        /// <param name="parent">a valid code</param>
        /// <param name="code">a valid code</param>
        /// <returns>If set successfully, return 0;otherwise return error code.</returns>
        [WebMethod]
        public int ExcludeAlternativeItem(string parent, string code)
        {
            // To do: call bll's method to set code2's alternative item group to code1's.
            com.inventec.imes.BLL.ModelBOM modelbom = new com.inventec.imes.BLL.ModelBOM();
            modelbom.ExcludeAlternativeItem(parent, code);
            return 0;
        }

        /// <summary>
        /// Method to get a specified model's alternative item
        /// </summary>	
        /// <param name="code1">a valid code</param>
        /// <param name="code2">a valid code</param>
        /// <returns>If set successfully, return 0;otherwise return error code.</returns>
        [WebMethod]
        public string[] GetAlternativeItems(string code, string itemgroup)
        {
            // To do: call bll's method to set code2's alternative item group to code1's.
            com.inventec.imes.BLL.ModelBOM modelbom = new com.inventec.imes.BLL.ModelBOM();
            IList<ModelBOMInfo> ret = modelbom.GetAlternativeItems(code, itemgroup);
            IList<string> types = new List<string>();

            foreach (ModelBOMInfo item in ret)
            {
                types.Add(item.Material);
            }
            //modelbom.ApproveModel(code, op);
            return types.ToArray();
        }

        /// <summary>
        /// Method to add a new code
        /// </summary>	
        /// <param name="code">a valid code</param>
        /// <param name="type">atype of code</param>
        /// <returns>If add successfully, return 0;otherwise return error code.</returns>
        [WebMethod]
        public int AddNewCode(string code, string type)
        {
            com.inventec.imes.BLL.ModelBOM modelbom = new com.inventec.imes.BLL.ModelBOM();
            return modelbom.AddNewCode(code, type);
        }
        
        /// <summary>
        /// Method to test if a code is already exist in ModelBOM table
        /// </summary>	
        /// <param name="code">a valid code</param>
        /// <returns>If exist, return 0;otherwise return error code.</returns>
        [WebMethod]
        public int IsCodeExist(string code)
        {
            com.inventec.imes.BLL.ModelBOM modelbom = new com.inventec.imes.BLL.ModelBOM();
            return modelbom.IsCodeExist(code);
        }
        
        /// <summary>
        /// Method to refresh mo
        /// </summary>	
        /// <param name="mo">Array of mo</param>
        /// <returns>If operate successfully, return 0;otherwise return error code.</returns>
        [WebMethod]
        public int RefreshBOM(string[] mo, string model)
        {
            com.inventec.imes.BLL.ModelBOM modelbom = new com.inventec.imes.BLL.ModelBOM();
            return modelbom.RefreshBOM(mo, model);
        }
        
        /// <summary>
        /// Method to get type of a specified code
        /// </summary>	
        /// <param name="code">a specified code</param>
        /// <returns>The type of the specified code.</returns>
        [WebMethod]
        public string GetTypeOfCode(string code)
        {
            com.inventec.imes.BLL.ModelBOM modelbom = new com.inventec.imes.BLL.ModelBOM();
            return modelbom.GetTypeOfCode(code);
        }
	}
//}