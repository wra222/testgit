using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace Inventec.HPEDITS.Parser
{

	public class SAPFlatFileParser
	{
		protected SqlConnection m_Connection;
		protected SqlDataAdapter m_Adapter;

		protected DataSet m_DataDictionary;
		//protected DataTable m_DataDictionaryTable;

		protected string m_DataFilePath;
		public string DataFilePath
		{
			get { return m_DataFilePath; }
			set 
			{
				if(File.Exists(value) == true)
				m_DataFilePath = value; 
			}
		}

		protected DataDictionaryManagerment m_DataDictionaryManagerment;
		public DataDictionaryManagerment DataDictionaryManagerment
		{
			get { return m_DataDictionaryManagerment; }
			set { m_DataDictionaryManagerment = value; }
		}
		protected IList<DataDictionary> m_DataDictionaryList;



		public SAPFlatFileParser(string aDataFilePath, string aConnectionString)
		{
			this.DataFilePath		= aDataFilePath;

			this.m_Connection		= new SqlConnection();
			this.m_Connection.ConnectionString = aConnectionString;
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
			theReader	= new StreamReader(this.DataFilePath);
			using (m_Connection)
			{
				m_Connection.Open();
				string theDataLine;
				
				while ((theDataLine = theReader.ReadLine()) != null)
				{
					this.Insert(theDataLine);
					theDataCounter++;
				}
				m_Connection.Close();
			}
			return theDataCounter;
		}




		protected virtual bool Insert(string aDataLine)
		{
			return false;
		}
        //delete exists data
        protected string BuildDeleteTagbleQuery(string aTableName, string KeyName1, string KeyValue1) 
        {
            string deletesql = "delete " + aTableName + " where " + KeyName1 + "='" + KeyValue1 + "';";
            return deletesql;
        }

        protected string BuildInsertToTableQuery(string[] aValues, string aTableName, string aPrefixColumnName)
		{
			StringBuilder theFieldsSQL = new StringBuilder();
			StringBuilder theValuesSQL = new StringBuilder();

			int theValuesIndex = 1;
			foreach (DataDictionary theDataDictionary in this.m_DataDictionaryList)
			{
				while (theDataDictionary.Index > theValuesIndex)
				{
					theFieldsSQL.Append(String.Format("[{0}{1}],", aPrefixColumnName, theValuesIndex));
					theValuesSQL.Append(String.Format("'{0}',", aValues[theValuesIndex-1].Trim()));
					theValuesIndex++;
				}
				// theDataDictionary.Index == theValuesIndex
				theFieldsSQL.Append(String.Format("[{0}],", theDataDictionary.Name));
				switch (theDataDictionary.DataTye)
				{
					case DataType.NUMERIC:
						if (String.IsNullOrEmpty(aValues[theDataDictionary.Index-1]) == true)
						{
							// value
							theValuesSQL.Append("0,");file:///F:\hvls\HPEDITS .net Source Code\HP EDITS_20080318\hvls\HPEDITS .net Source Code\HPEDITS_WebApp_20080124\App_Code\Method.cs
							break;
						}
						// value
						theValuesSQL.Append(String.Format("{0},", aValues[theDataDictionary.Index-1].Trim()));
						break;
					case DataType.TEXT:
					case DataType.DATE:
					case DataType.ALPHANUMERIC:
					default:
						// value
                       
						theValuesSQL.Append(String.Format("'{0}',", aValues[theDataDictionary.Index-1].ToString().Replace("'","''").Trim()));
						break;
				}
				theValuesIndex	= theDataDictionary.Index+1;
			}

			while (theValuesIndex <= aValues.Length)
			{
				theFieldsSQL.Append(String.Format("[{0}{1}],", aPrefixColumnName, theValuesIndex));
				theValuesSQL.Append(String.Format("'{0}',", aValues[theValuesIndex-1].Trim()));
				theValuesIndex++;
			}
			//remove last ','
			theFieldsSQL.Remove(theFieldsSQL.Length - 1, 1);
			theValuesSQL.Remove(theValuesSQL.Length - 1, 1);
			return string.Format("insert into {0} ({1}) values ({2})", aTableName, theFieldsSQL.ToString(), theValuesSQL.ToString());
		}

		protected virtual bool CheckExist(string aKey)
		{
			return false;
		}
		protected virtual bool Updata(string aDataLine)
		{
			return false;
		}
	}
}
