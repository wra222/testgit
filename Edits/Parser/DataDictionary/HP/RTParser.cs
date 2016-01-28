using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Inventec.HPEDITS.Parser
{
	public class RTParser : HPFlatFilePaser
	{
		public RTParser(string aDataFilePath, string aConnectionString):base(aDataFilePath, aConnectionString)
		{}

		protected override bool InsertToDatabase(string aDataLine)
		{
			string theQuery		= this.BuildInsertToTableQuery(aDataLine.Split(','), "[dbo].[PAK.PAKRT]");
			System.Data.SqlClient.SqlCommand theCommand		= new SqlCommand(theQuery, this.m_Connection);
			int theResult		= theCommand.ExecuteNonQuery();
			if (theResult == 1)
			{
				return true;
			}
			return false;
		}
	}
}
