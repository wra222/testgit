/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: Data access layer implements interface
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ================ =====================================
 * 2009-11-04 Li Hai-Jun       Create                    
 * Known issues:
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using com.inventec.imes.IDAL;
using com.inventec.imes.Model;
using com.inventec.imes.DBUtility;

namespace com.inventec.imes.SQLServerDAL
{
    public class ModelBOM : IModelBOM
    {
        private string connectString;
        private const string SQL_SELECT_CATEGORIES = "SELECT Material_group FROM ModelBOM";
        private bool debug = true;

        public ModelBOM()
        {
            connectString = "";
        }

        public ModelBOM(string connString)
        {
            connectString = connString;
        }

        /// <summary>
        /// Get information on a specific ModelBOM
        /// </summary>
        /// <param name="modelBOMId">Unique identifier for a ModelBOM</param>
        /// <returns>Business Entity representing a ModelBOM</returns>
        public ModelBOMInfo GetModelBOM(long modelBOMId)
        {
            ModelBOMInfo info = new ModelBOMInfo();

            return info;
        }

        /// <summary>
        /// Method to get all types
        /// </summary>		
        /// <returns>Interface to Model Collection Generic of vs</returns>
        public IList<string> GetModelBOMTypes()
        {
            IList<string> types = new List<string>();
            string strSql = "SELECT distinct Material_group, flag=1 FROM ModelBOM union ";
            strSql += "SELECT distinct PartType as Material_group, flag=0  from PartType order by Material_group";

            using (SqlDataReader rdr = SQLHelper.ExecuteReader(SQLHelper.ConnectionStringLocalTransaction, CommandType.Text, strSql, null))
            {
                while (rdr.Read())
                {
                    types.Add(rdr.GetString(0).Trim() + "&" + rdr.GetInt32(1));
                }
            }

            return types;
        }

        /// <summary>
        /// Method to get all codes
        /// </summary>
        /// <param name="parentCode">Unique identifier for a ModelBOM</param>
        /// <param name="match">match string</param>
        /// <returns>Interface to Model Collection Generic of vs</returns>
        public IList<string> GetModelBOMCodes(string parentCode, string match)
        {
            IList<string> codes = new List<string>();
            string strSql = "SELECT distinct Material, BOMApproveDate FROM ModelBOM mb left JOIN Model on (mb.Material = Model.Model) where Material_group='" + parentCode + "'";
            if (match != "")
            {
                strSql += " and Material like '%" + match + "%'";
            }
            strSql += " order by Material";

            using (SqlDataReader rdr = SQLHelper.ExecuteReader(SQLHelper.ConnectionStringLocalTransaction, CommandType.Text, strSql, null))
            {
                while (rdr.Read())
                {
                    if (rdr.IsDBNull(1))
                    {
                        codes.Add(rdr.GetString(0).Trim() + "&");
                    }
                    else
                    {
                        string str1 = rdr.GetString(0).Trim();
                        string str2 = rdr.GetDateTime(1).ToString("yyyy-MM-dd");
                        codes.Add(rdr.GetString(0).Trim() + "&" + rdr.GetDateTime(1).ToString("yyyy-MM-dd"));
                    }
                }
            }

            return codes;
        }

        /// <summary>
        /// Method to get specified ModelBOM's children.
        /// </summary>		
        /// <param name="code">Unique identifier for a ModelBOM</param>
        /// <returns>Interface to Model Collection Generic of vs</returns>
        public IList<ModelBOMInfo> GetParentModelBOMByCode(string code)
        {
            IList<ModelBOMInfo> boms = new List<ModelBOMInfo>();
            string strSql = "SELECT distinct(Material), Material_group, Component FROM ModelBOM where Component='" + code + "'";

            using (SqlDataReader rdr = SQLHelper.ExecuteReader(SQLHelper.ConnectionStringLocalTransaction, CommandType.Text, strSql, null))
            {
                while (rdr.Read())
                {
                    ModelBOMInfo info = new ModelBOMInfo();
                    info.Material = rdr.GetString(0).Trim();
                    info.Material_group = rdr.GetString(1).Trim();
                    info.Quantity = rdr.GetString(2).Trim();
                    boms.Add(info);
                }
            }

            return boms;
        }

        /// <summary>
        /// Method to get specified ModelBOM's children.
        /// </summary>
        /// <param name="code">Unique identifier for a ModelBOM</param>
        /// <returns>Interface to Model Collection Generic of vs</returns>
        public IList<ModelBOMInfo> GetSubModelBOMByCode(string code)
        {
            IList<ModelBOMInfo> result = new List<ModelBOMInfo>();
            string strSql = "SELECT Component as Material, Material_group, Quantity, Alternative_item_group FROM ModelBOM where Material='" + code + "'";

            using (SqlDataReader rdr = SQLHelper.ExecuteReader(SQLHelper.ConnectionStringLocalTransaction, CommandType.Text, strSql, null))
            {
                while (rdr.Read())
                {
                    ModelBOMInfo info = new ModelBOMInfo();
                    if (!rdr.IsDBNull(0))
                    {
                        info.Material = rdr.GetString(0).Trim();
                    }
                    if (!rdr.IsDBNull(1))
                    {
                        info.Material_group = rdr.GetString(1).Trim();
                    }
                    if (!rdr.IsDBNull(2))
                    {
                        info.Quantity = rdr.GetString(2).Trim();
                    }
                    if (!rdr.IsDBNull(3))
                    {
                        info.Alternative_item_group = rdr.GetString(3).Trim();
                    }

                    result.Add(info);
                }
            }

            return result;
        }

        /// <summary>
        /// Method to get specified ModelBOM's alternative items.
        /// <param name="code">Unique identifier for a ModelBOM</param>
        /// <param name="alternativeItemGroup">Alternative item group for a ModelBOM</param>
        /// </summary>		
        /// <returns>Interface to Model Collection Generic of ModelBOMInfo</returns>
        public IList<ModelBOMInfo> GetAlternativeItems(string code, string alternativeItemGroup)
        {
            IList<ModelBOMInfo> result = new List<ModelBOMInfo>();
            //if (debug)
            //{
            //    //模拟数据，以后变为从ModelBOM表中取得
            //    ModelBOMInfo info = new ModelBOMInfo();
            //    info.Material = "TC0000000002";
            //    result.Add(info);
            //    info = new ModelBOMInfo();
            //    info.Material = "TC0000000003";
            //    result.Add(info);
            //}

            string strSql = "SELECT distinct Component as Material from ModelBOM where Material='" + code + "'";
            strSql += " and Alternative_item_group='" + alternativeItemGroup + "'";

            using (SqlDataReader rdr = SQLHelper.ExecuteReader(SQLHelper.ConnectionStringLocalTransaction, CommandType.Text, strSql, null))
            {
                while (rdr.Read())
                {
                    ModelBOMInfo info = new ModelBOMInfo();
                    info.Material = rdr.GetString(0).Trim();

                    result.Add(info);
                }
            }

            return result;
        }

        private IList<string> GetOffspringModelBOM(string code)
        {
            IList<string> result = new List<string>();
            string strSql = "SELECT distinct(Material), AssemblyCode FROM ModelBOM  left join MO on ModelBOM.Material=Mo.Model ";
            strSql += "left join MOBOM on MO.MO=MoBOM.MO ";
            strSql += "where Material in (select Component from ModelBOM where Material='" + code + "')";

            using (SqlDataReader rdr = SQLHelper.ExecuteReader(SQLHelper.ConnectionStringLocalTransaction, CommandType.Text, strSql, null))
            {
                while (rdr.Read())
                {
                    if (rdr.IsDBNull(1))
                    {
                        result.Add(rdr.GetString(0).Trim());
                    }
                    else
                    {
                        result.Add(rdr.GetString(0).Trim() + "(" + rdr.GetString(1).Trim() + ")");
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Method to get specified ModelBOM's GetOffSprings.
        /// </summary>
        /// <param name="code">Unique identifier for a ModelBOM</param>
        /// <returns>Interface to Model Collection Generic of string</returns>
        public IList<string> GetOffSpringModelBOMByCode(string code)
        {
            IList<string> temp = GetOffspringModelBOM(code);

            IList<string> child;
            IList<string> result = new List<string>();

            foreach (string item in temp)
            {
                child = GetOffspringModelBOM(item);
                if (child.Count() > 0)
                {
                    result.Add(item + "&1");
                }
                else
                {
                    result.Add(item + "&0");
                }
            }
            return result;
        }



        /// <summary>
        /// Method to remove alternative items.
        /// </summary>
        /// <param name="code">Unique identifier for a ModelBOM</param>
        /// <returns>result</returns>
        public int RemoveAlternativeItem(string code)
        {
            // To do: 清除db中该项对应记录的alternative_item_group栏位
            return 0;
        }

        /// <summary>
        /// Method to approve a model
        /// </summary>	
        /// <param name="code">a valid code</param>
        /// <param name="op">operator</param>
        /// <returns>If approve successfully, return 0;otherwise return error code.</returns>
        public int ApproveModel(string code, string op)
        {
            string strSql = "UPDATE Model SET Editor='" + op + "', BOMApproveDate = '" + DateTime.Now + "' where Model='" + code + "'";
            int ret = SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.Text, strSql, null);
            return ret;
        }

        /// <summary>
        /// Method to get MoBOMs via a specified model
        /// </summary>	
        /// <param name="model">a valid model code</param>
        /// <returns>Interface to Model Collection Generic of MoBOMInfo</returns>
        public IList<MoBOMInfo> GetMoBOMByModel(string model)
        {
            IList<MoBOMInfo> result = new List<MoBOMInfo>();

            string strSql = "SELECT distinct(MO), Qty, PrintQty as StartQty, Udt FROM MO where Model='" + model + "'";

            using (SqlDataReader rdr = SQLHelper.ExecuteReader(SQLHelper.ConnectionStringLocalTransaction, CommandType.Text, strSql, null))
            {
                while (rdr.Read())
                {
                    MoBOMInfo mo = new MoBOMInfo();

                    mo.Mo = rdr.GetString(0).Trim();
                    mo.Qty = "" + rdr.GetInt16(1);
                    mo.StartQty = "" + rdr.GetInt16(2);
                    mo.UpdateDate = rdr.GetDateTime(3).ToString("yyyy-MM-dd");
                    result.Add(mo);
                }
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
            string strSql = "UPDATE ModelBOM SET Alternative_item_group=(SELECT Alternative_item_group from ModelBOM where Material='" + parent + "' and Component='" + code1 + "') where Material='" + parent + "' and Component='" + code2 + "'";
            int ret = SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.Text, strSql, null);
            if (ret > 0)
            {
                ret = 0;
            }

            return ret;
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
            string strSql = "UPDATE ModelBOM SET alternative_item_group=(SELECT alternative_item_group from ModelBOM where Material='" + parent + "' and Component='" + code1 + "') where Material='" + parent + "' and Component in ";
            strSql += "(SELECT Component from ModelBOM where Material='" + parent + "' and Alternative_item_group = (select distinct Alternative_item_group from ModelBOM where Material='" + parent + "' and Component='" + code2 + "'))";
            int ret = SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.Text, strSql, null);
            if (ret > 0)
            {
                ret = 0;
            }

            return ret;
        }

        /// <summary>
        /// Method to exclude an item from an alternative item group
        /// </summary>
        /// <param name="parent">a valid code</param>
        /// <param name="code">a valid code</param>
        /// <returns>If exclude the item successfully, return 0, else return error code</returns>
        public int ExcludeAlternativeItem(string parent, string code)
        {
            string strSql = "UPDATE ModelBOM SET Alternative_item_group=(SELECT max(CONVERT(int, Alternative_item_group)) + 1 from ModelBOM) where Material='" + parent + "' and Component='" + code + "'";
            int ret = SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.Text, strSql, null);
            return ret;
        }

        /// <summary>
        /// Method to delete a sub code from sub code list
        /// </summary>	
        /// <param name="code">a valid code</param>
        /// <returns>If delete the item successfully, return 0, else return error code</returns>
        public int DeleteModelBOMByCode(string parentCode, string code)
        {
            int ret = -1;// DeleteSubModelByCode(code);
            string delSql = "DELETE FROM ModelBOM where Material='" + parentCode + "' and Component='" + code + "'";
            ret = SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.Text, delSql, null);
            if (ret > 0)
            {
                ret = 0;
            }

            return ret;
        }

        private int DeleteSubModelByCode(string code)
        {
            string strSql = "SELECT distinct Component from ModelBOM where Material='" + code + "'";
            int ret = 0;

            using (SqlDataReader rdr = SQLHelper.ExecuteReader(SQLHelper.ConnectionStringLocalTransaction, CommandType.Text, strSql, null))
            {
                while (rdr.Read())
                {
                    if (rdr.IsDBNull(0))
                    {
                        string delSql = "DELETE FROM ModelBOM where Material='" + code + "'";
                        ret = SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.Text, delSql, null);
                    }
                    else
                    {
                        ret = DeleteSubModelByCode(rdr.GetString(0).Trim());
                    }
                }
            }
            return ret;
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
            bool bUpdate = false;
            string strSql = null;
            strSql = "SELECT Material from ModelBOM where Material = '" + code + "' ";
            strSql += "UNION SELECT PartNo as Material from Part where PartNo='" + code + "'";

            using (SqlDataReader rdr = SQLHelper.ExecuteReader(SQLHelper.ConnectionStringLocalTransaction, CommandType.Text, strSql, null))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0) || (rdr.IsDBNull(0) && !rdr.IsDBNull(1)))
                    {
                        bUpdate = true;
                    }
                }
            }

            int ret = -2;
            if (!bUpdate)
            {
                return ret;
            }
            if (bAddNew)
            {
                strSql = "INSERT ModelBom (Material, Material_group, Component, Quantity) values('" + parentCode + "','" + type + "','" + oldCode + "','" + qty + "')";
                //strSql = "execute AddModelBOM '" + parentCode + "','" + item.Material + "', '" + item.Material_group + "','" + item.Quantity + "";
            }
            else
            {
                strSql = "UPDATE ModelBom set Component='" + code + "',Quantity='" + qty + "' where Material='" + parentCode + "' and Component='" + oldCode + "'";
                //strSql = "execute UpdateModelBOM '" + parentCode + "','" + oldCode + "','"+ item.Material + "', '" + item.Material_group + "','" + item.Quantity + "'";
            }

            ret = SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.Text, strSql, null);

            return ret;
        }

        /// <summary>
        /// Method to add a new code
        /// </summary>	
        /// <param name="code">a valid code</param>
        /// <returns>If add successfully, return 0;otherwise return error code.</returns>
        public int AddNewCode(string code, string type)
        {
            string strSql = "SELECT Material FROM Model where Model='" + code + "'";
            //"INSERT ModelBOM (material, material_group)  values('" + code + "', '" + type +"')";
            int ret = SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.Text, strSql, null);
            if (ret > 0)
            {
                ret = 0;
            }
            return ret;
        }

        /// <summary>
        /// Method to test if a code is already exist in ModelBOM table
        /// </summary>	
        /// <param name="code">a valid code</param>
        /// <returns>If exist, return 0;otherwise return error code.</returns>
        public int IsCodeExist(string code)
        {
            int ret = 0;
            string strSql = "SELECT Material FROM ModelBOM where Material='" + code + "'";
            using (SqlDataReader rdr = SQLHelper.ExecuteReader(SQLHelper.ConnectionStringLocalTransaction, CommandType.Text, strSql, null))
            {
                ret = rdr.HasRows ? 1 : 0;
            }

            strSql = "SELECT Model FROM Model where Model='" + code + "'";
            using (SqlDataReader rdr = SQLHelper.ExecuteReader(SQLHelper.ConnectionStringLocalTransaction, CommandType.Text, strSql, null))
            {
                ret += rdr.HasRows ? 1 : 0;
            }
            return ret;
        }

        /// <summary>
        /// Method to save a code as a new code
        /// </summary>	
        /// <param name="code">a valid code</param>
        /// <param name="newCode">a valid code</param>
        /// <returns>If operate successfully, return 0;otherwise return error code.</returns>
        public int SaveCodeAs(string code, string newCode)
        {
            string strSql = "execute SaveModelBOMAs '" + code + "', '" + newCode + "'";
            //"INSERT ModelBOM (material, material_group)  values('" + code + "', '" + type +"')";
            int ret = SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.Text, strSql, null);
            if (ret > 0)
            {
                ret = 0;
            }
            return ret;
        }

        /// <summary>
        /// Method to refresh MoBOM
        /// </summary>	
        /// <param name="mo">Array of mo</param>
        /// <param name="model">model</param>
        /// <returns>If operate successfully, return 0;otherwise return error code.</returns>
        public int RefreshBOM(string[] mo, string model)
        {
            int ret = -1;
            string strRet = "";
            string strSql = "";

            for (int i = 0; i < mo.Length; i++)
            {
                strSql = "DELETE MoBOM where MO='" + mo[i] + "'";
                ret = SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.Text, strSql, null);

                strSql = "SELECT Component, Quantity FROM ModelBOM where Material in (SELECT Model FROM MO where MO='" + mo[i] + "')";
                using (SqlDataReader rdr = SQLHelper.ExecuteReader(SQLHelper.ConnectionStringLocalTransaction, CommandType.Text, strSql, null))
                {
                    while (rdr.Read())
                    {
                        strRet = GetMo(rdr.GetString(0).Trim(), 1, int.Parse(rdr.GetString(1).Trim()));
                        string[] strTmp = strRet.Split('&');
                        strSql = "execute RefreshBom '" + mo[i] + "','" + strTmp[0] + "', " + strTmp[1];
                        ret = SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionStringLocalTransaction, CommandType.Text, strSql, null);
                        if (ret == -1)
                        {
                            return ret;
                        }
                    }
                }
            }

            return ret;
        }

        private string GetMo(string model, int level, int qty)
        {
            // Force to return when the level is over 10 to avoid infinite recurrence
            if (level >= 10)
            {
                return "";
            }

            string ret = "";
            string strSql = "";
            strSql = "SELECT Component, Quantity FROM ModelBOM where Material='" + model + "'";

            SqlDataReader rdr = SQLHelper.ExecuteReader(SQLHelper.ConnectionStringLocalTransaction, CommandType.Text, strSql, null);
            {
                if (rdr.HasRows)
                {
                    if (rdr.Read())
                    {
                        if (!rdr.IsDBNull(0))
                        {
                            int tmpQty = int.Parse(rdr.GetString(1).Trim()) * qty;
                            ret = GetMo(rdr.GetString(0).Trim(), level + 1, tmpQty);
                        }
                    }
                }
                else
                {
                    ret = model + "&" + qty;
                }
            }
            return ret;
        }

        /// <summary>
        /// Method to get type of a specified code
        /// </summary>	
        /// <param name="code">a specified code</param>
        /// <returns>The type of the specified code.</returns>
        public string GetTypeOfCode(string code)
        {
            string retType = null;
            string strSql = null;
            strSql = "SELECT Material_group from ModelBOM where Material = '" + code + "' ";

            using (SqlDataReader rdr = SQLHelper.ExecuteReader(SQLHelper.ConnectionStringLocalTransaction, CommandType.Text, strSql, null))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        retType = rdr.GetString(0).Trim();
                    }
                }
            }

            return retType;
        }
    }
}
