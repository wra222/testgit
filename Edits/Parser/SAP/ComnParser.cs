using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace Inventec.HPEDITS.Parser
{
	public class ComnParser : SAPFlatFileParser
	{
        public string PreKeyValue = "";
		public ComnParser(string aDataFilePath, string aConnectionString, DataDictionaryManagerment aDDM):base(aDataFilePath, aConnectionString)
		{
			this.m_DataDictionaryManagerment	= aDDM;
			this.m_DataDictionaryList			= this.m_DataDictionaryManagerment.ComnList;
			//this.m_DataDictionaryTable	= this.m_DataDictionary.Tables["comn"];
		}



		protected override bool Insert(string aDataLine)
		{
			string[] theValues = aDataLine.Split('~');
            //delete exists Data from PAK.PAKComn
            string DeleteQuery = "";
            if (PreKeyValue != theValues[0])
            {
                PreKeyValue = theValues[0];
                DeleteQuery = this.BuildDeleteTagbleQuery("[dbo].[PAK.PAKComn]", "InternalID", theValues[0]);
            }
            //insert data into PAK.PAKComn
			string theQuery		= this.BuildInsertToTableQuery(theValues, "[dbo].[PAK.PAKComn]", "C");
            //theQuery = DeleteQuery + theQuery;
            try
            {
                System.Data.SqlClient.SqlCommand deleteCommand = new SqlCommand(DeleteQuery, this.m_Connection);
                int deleteResult = deleteCommand.ExecuteNonQuery();
            }
            catch { }
			System.Data.SqlClient.SqlCommand theCommand		= new SqlCommand(theQuery, this.m_Connection);
			int theResult		= theCommand.ExecuteNonQuery();
			if (theResult == 1)
			{
				// update "Model" field
				DataDictionary theCONSOL_INVOICE	= ((List<DataDictionary>)this.m_DataDictionaryList).Find(delegate(DataDictionary aDD)
				{
					return "HP_PN" == aDD.Name;
				});
				string theModel			= theValues[theCONSOL_INVOICE.Index-1].Split('/')[0];
//                string theUpdateSQL = String.Format("UPDATE [dbo].[PAK.PAKComn] SET [Model] = '{0}',BOX_UNIT_QTY = 1 WHERE [InternalID] = '{1}'", theModel, theValues[0]);
                string boxunitqty = theValues[44].Split('.')[0];
                string theUpdateSQL = "";
                if (boxunitqty.Trim().Equals("0")){
                    theUpdateSQL = String.Format("UPDATE [dbo].[PAK.PAKComn] SET [Model] = '{0}',BOX_UNIT_QTY = 1 WHERE [InternalID] = '{1}'", theModel, theValues[0]);
                }else{
                    theUpdateSQL = String.Format("UPDATE [dbo].[PAK.PAKComn] SET [Model] = '{0}',BOX_UNIT_QTY = ltrim('{2}') WHERE [InternalID] = '{1}'", theModel, theValues[0], boxunitqty);
                }

			    theCommand.CommandText	= theUpdateSQL;
				theCommand.ExecuteNonQuery();
				return true;
			}
			return false;

			//insert data into PAK.PACKLIST
			//string theQuery = BuildInsertQuery(theValues, theValues[0], EDataType.comn);			
			//System.Data.SqlClient.SqlCommand theCommand		= new SqlCommand(theQuery, this.m_Connection);
			//int theResult	= theCommand.ExecuteNonQuery();
			//if (theResult == 1)
			//{
			//    return true;
			//}
			//return false;
		}

		//[Obsolete]
		//private void Insert2ComnTable(string[] theValues)
		//{
		//    StringBuilder theFieldsSQL = new StringBuilder();
		//    StringBuilder theValuesSQL = new StringBuilder();

		//    DataRow[] theRows = this.m_DataDictionaryTable.Select("1=1", "index");

		//    //add InternalID
		//    theFieldsSQL.Append("[InternalID],");
		//    theValuesSQL.Append(String.Format("'{0}',", theValues[0]));
		//    int theValuesIndex = 2;
		//    foreach (DataRow theRow in theRows)
		//    {
		//        while (Convert.ToInt32(theRow["index"]) > theValuesIndex)
		//        {
		//            theFieldsSQL.Append(String.Format("[C{0}],", theValuesIndex));
		//            theValuesSQL.Append(String.Format("'{0}',", theValues[theValuesIndex-1]));
		//            theValuesIndex++;
		//        }

		//        theFieldsSQL.Append(String.Format("[{0}],", theRow["name"]));
		//        switch (theRow["type"].ToString())
		//        {
		//            case "Numeric":
		//                if (String.IsNullOrEmpty(theValues[theValuesIndex-1]) == true)
		//                {
		//                    // value
		//                    theValuesSQL.Append("0,");
		//                    break;
		//                }
		//                // value
		//                theValuesSQL.Append(String.Format("{0},", theValues[theValuesIndex-1]));
		//                break;
		//            default:
		//                // value
		//                theValuesSQL.Append(String.Format("'{0}',", theValues[theValuesIndex-1].ToString().Replace("'","''")));
		//                break;
		//        }
		//        // 防止同一个value对应多个数据库字段
		//        theValuesIndex	= Convert.ToInt32(theRow["index"])+1;
		//    }

		//    while (theValuesIndex <= theValues.Length)
		//    {
		//        theFieldsSQL.Append(String.Format("[C{0}],", theValuesIndex));
		//        theValuesSQL.Append(String.Format("'{0}',", theValues[theValuesIndex-1]));
		//        theValuesIndex++;
		//    }

		//    theFieldsSQL.Remove(theFieldsSQL.Length - 1, 1);
		//    theValuesSQL.Remove(theValuesSQL.Length - 1, 1);
		//    string theSQLQuery = string.Format("insert into [PAK.PAKComn] ({0}) values ({1})", theFieldsSQL.ToString(), theValuesSQL.ToString());
		//    System.Data.SqlClient.SqlCommand theInsertCommand = new SqlCommand(theSQLQuery, this.m_Connection);
		//    theInsertCommand.ExecuteNonQuery();
		//}

	}
}
