using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Inventec.HPEDITS.Parser
{
	public class EDI850RAWParser : SAPFlatFileParser
	{
        public string PreKeyValue = "";
        public EDI850RAWParser(string aDataFilePath, string aConnectionString, DataDictionaryManagerment aDDM):base(aDataFilePath, aConnectionString)
		{
			this.m_DataDictionaryManagerment	= aDDM;
			this.m_DataDictionaryList			= this.m_DataDictionaryManagerment.Edi850rawList;
			//this.m_DataDictionaryTable	= this.m_DataDictionary.Tables["edi850raw"];
		}
	
		protected override bool Insert(string aDataLine)
		{			
			string[] theValues		= aDataLine.Split('~');
            //delete exists Data from PAK.PAKComn
            string DeleteQuery = "";
            if (PreKeyValue != theValues[0])
            {
                PreKeyValue = theValues[0];
                DeleteQuery = this.BuildDeleteTagbleQuery("[dbo].[PAK.PAKEdi850raw]", "PO_NUM", theValues[0]);
            }
			//insert data into PAK.PAKEdi850raw
			string theQuery		= this.BuildInsertToTableQuery(theValues, "[dbo].[PAK.PAKEdi850raw]", "E");
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
			    return true;
			}
			return false;


			//insert data into PAK.PACKLIST
			//string theEDI850RAWKey	= theValues[0];
			//string theInternalID	= this.FindInternalID(theEDI850RAWKey);
			//if (theInternalID == String.Empty)
			//{
			//    return false;
			//}

			//string theQuery = BuildInsertQuery(theValues, theInternalID, EDataType.edi850raw);
			//System.Data.SqlClient.SqlCommand theCommand		= new SqlCommand(theQuery, this.m_Connection);
			//int theResult	= theCommand.ExecuteNonQuery();
			//if (theResult == 1)
			//{
			//    return true;
			//}
			//return false;
		}

		//[Obsolete]
		//private void Insert2EDI850RAWTable(string[] theValues)
		//{
		//    StringBuilder theFieldsSQL = new StringBuilder();
		//    StringBuilder theValuesSQL = new StringBuilder();

		//    DataRow[] theRows = this.m_DataDictionaryTable.Select("1=1", "index");

		//    //add SHIP_TO_NAME
		//    theFieldsSQL.Append("[SHIP_TO_NAME],");
		//    theValuesSQL.Append(String.Format("'{0}',", theValues[0]));
		//    int theValuesIndex = 2;
		//    foreach (DataRow theRow in theRows)
		//    {
		//        while (Convert.ToInt32(theRow["index"]) > theValuesIndex)
		//        {
		//            theFieldsSQL.Append(String.Format("[E{0}],", theValuesIndex));
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

		//    while (theValuesIndex < theValues.Length)
		//    {
		//        theFieldsSQL.Append(String.Format("[E{0}],", theValuesIndex));
		//        theValuesSQL.Append(String.Format("'{0}',", theValues[theValuesIndex-1]));
		//        theValuesIndex++;
		//    }

		//    theFieldsSQL.Remove(theFieldsSQL.Length - 1, 1);
		//    theValuesSQL.Remove(theValuesSQL.Length - 1, 1);
		//    string theSQLQuery = string.Format("insert into [PAK.PAKEdi850raw] ({0}) values ({1})", theFieldsSQL.ToString(), theValuesSQL.ToString());
		//    System.Data.SqlClient.SqlCommand theInsertCommand = new SqlCommand(theSQLQuery, this.m_Connection);
		//    theInsertCommand.ExecuteNonQuery();
		//}

		private string FindInternalID(string aKey)
		{
			string theInternalID;

			string theQuery		= string.Format("select InternalID from [Pak.PackingList] where [SHIP_TO_NAME] = '{0}' and DataType = 0", aKey);
			System.Data.SqlClient.SqlCommand theCommand		= new SqlCommand(theQuery, this.m_Connection);
			object theObj		= theCommand.ExecuteScalar();

			if (theObj == null)
			{
				return String.Empty;
			}
			theInternalID	= theObj.ToString();
			return theInternalID;
		}
	}
}
