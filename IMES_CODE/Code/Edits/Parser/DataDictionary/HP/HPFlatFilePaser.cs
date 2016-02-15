using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.IO;

namespace Inventec.HPEDITS.Parser
{
	public class HPFlatFilePaser
	{
		protected SqlConnection m_Connection;
		protected SqlDataAdapter m_Adapter;
		protected List<string> m_ColumnNameList;

		private string m_DataFilePath;
		protected string DataFilePath
		{
			get { return m_DataFilePath; }
			set { m_DataFilePath = value; }
		}

		public HPFlatFilePaser(string aDataFilePath, string aConnectionString)
		{
			this.DataFilePath		= aDataFilePath;

			this.m_Connection		= new SqlConnection();
			this.m_Connection.ConnectionString = aConnectionString;

			this.m_ColumnNameList	= new List<string>();
		}

		public int LoadDataFile(string aFilePath)
		{
			this.DataFilePath	= aFilePath;
			return this.LoadDataFile();
		}
		public int LoadDataFile()
		{
			// not set data file path
			if (String.IsNullOrEmpty(this.DataFilePath) == true)
			{
				return 0;
			}

			System.IO.TextReader theReader;
			int theDataCounter	= 0;
			theReader			= new StreamReader(this.DataFilePath);
			this.LoadColumnName(theReader.ReadLine());

			using (m_Connection)
			{
				m_Connection.Open();
				string theDataLine;
				
				while ((theDataLine = theReader.ReadLine()) != null)
				{
					if (String.IsNullOrEmpty(theDataLine.Trim()))
					{
						continue;
					}
					this.InsertToDatabase(theDataLine);
					theDataCounter++;
				}
				m_Connection.Close();
			}
			return theDataCounter;
		}

		protected virtual bool InsertToDatabase(string aDataLine)
		{return false;}

		private void LoadColumnName(string aColumnNameLine)
		{
			string[] theColumnNames		= aColumnNameLine.Split(',');
			foreach (string theColumn in theColumnNames)
			{
				this.m_ColumnNameList.Add(theColumn);
			}
		}

		protected string BuildInsertToTableQuery(string[] aValues, string aTableName)
		{
			StringBuilder theFieldsSQL = new StringBuilder();
			StringBuilder theValuesSQL = new StringBuilder();

			foreach (string theColumn in this.m_ColumnNameList)
			{
				theFieldsSQL.Append(String.Format("{0},", theColumn));
			}

			foreach (string theValue in aValues)
			{
				theValuesSQL.Append(String.Format("'{0}',", theValue));
			}

			//remove last ','
			theFieldsSQL.Remove(theFieldsSQL.Length - 1, 1);
			theValuesSQL.Remove(theValuesSQL.Length - 1, 1);
			return string.Format("insert into {0} ({1}) values ({2})", aTableName, theFieldsSQL.ToString(), theValuesSQL.ToString());
		}
	}
}
