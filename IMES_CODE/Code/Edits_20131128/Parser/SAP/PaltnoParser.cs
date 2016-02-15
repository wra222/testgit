using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Inventec.HPEDITS.Parser
{
	public class PaltnoParser : SAPFlatFileParser
	{
        public string PreKeyValue = "";
        public PaltnoParser(string aDataFilePath, string aConnectionString, DataDictionaryManagerment aDDM):base(aDataFilePath, aConnectionString)
		{
			this.m_DataDictionaryManagerment	= aDDM;
			this.m_DataDictionaryList			= this.m_DataDictionaryManagerment.PaltnoList;
			//this.m_DataDictionaryTable	= this.m_DataDictionary.Tables["paltno"];
		}



		protected override bool Insert(string aDataLine)
		{
			string[] theValues	= aDataLine.Split('~');
            //delete exists Data from PAK.PAKComn
            string DeleteQuery = "";
            if (PreKeyValue != theValues[0])
            {
                PreKeyValue = theValues[0];
                DeleteQuery = this.BuildDeleteTagbleQuery("[dbo].[PAK.PAKPaltno]", "InternalID", theValues[0]);
            }
			//insert data into PAK.PAKPaltno
			string theQuery		= this.BuildInsertToTableQuery(theValues, "[dbo].[PAK.PAKPaltno]", "P");
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
			//string theQuery		= BuildInsertQuery(theValues, theValues[0], EDataType.paltno);
			//System.Data.SqlClient.SqlCommand theCommand		= new SqlCommand(theQuery, this.m_Connection);
			//int theResult		= theCommand.ExecuteNonQuery();

			//if (theResult == 1)
			//{
			//    return true;
			//}
			return false;
		}
		
		//[Obsolete]
		//private void Insert2PaltnoTable(string[] theValues)
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
		//            theFieldsSQL.Append(String.Format("[P{0}],", theValuesIndex));
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
		//        theFieldsSQL.Append(String.Format("[P{0}],", theValuesIndex));
		//        theValuesSQL.Append(String.Format("'{0}',", theValues[theValuesIndex-1]));
		//        theValuesIndex++;
		//    }
		//    theFieldsSQL.Remove(theFieldsSQL.Length - 1, 1);
		//    theValuesSQL.Remove(theValuesSQL.Length - 1, 1);
		//    string theSQLQuery = string.Format("insert into [PAK.PAKPaltno] ({0}) values ({1})", theFieldsSQL.ToString(), theValuesSQL.ToString());
		//    System.Data.SqlClient.SqlCommand theInsertCommand = new SqlCommand(theSQLQuery, this.m_Connection);
		//    theInsertCommand.ExecuteNonQuery();
		//}
	}
}
