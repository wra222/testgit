using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;


namespace Inventec.HPEDITS.Parser
{
    public class EDI850RAWINSTRParser : SAPFlatFileParser
    {
        public string PreKeyValue="";
        public EDI850RAWINSTRParser(string aDataFilePath, string aConnectionString, DataDictionaryManagerment aDDM):base(aDataFilePath, aConnectionString)
		{
			this.m_DataDictionaryManagerment	= aDDM;
			this.m_DataDictionaryList			= this.m_DataDictionaryManagerment.Edi850rawINSTRList;
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
                DeleteQuery = this.BuildDeleteTagbleQuery("[dbo].[PAKEDI_INSTR]", "PO_NUM", theValues[0]); 
            }
			//insert data into PAK.PAKEdi850rawINSTR
            string theQuery = this.BuildInsertToTableQuery(theValues, "[dbo].[PAKEDI_INSTR]", "I");
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

		}


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
