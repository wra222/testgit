using System;
using System.Collections.Generic;
using System.Text;

namespace Inventec.HPEDITS.Parser
{
	public class DataDictionary
	{
		private string m_Name;
		public string Name
		{
			get { return m_Name; }
		}
		
		private DataType m_DataTye;
		public DataType DataTye
		{
			get { return m_DataTye; }
		}

		private int m_Length;
		public int Length
		{
			get { return m_Length; }
		}

		private int m_Index;
		public int Index
		{
			get { return m_Index; }
		}
		

		public DataDictionary(string aName, string aDataType, int aLength, int anIndex)
		{
			this.m_Name		= aName;
			this.m_DataTye	= (DataType)Enum.Parse(typeof(DataType), aDataType, true);
			this.m_Length	= aLength;
			this.m_Index	= anIndex;
		}
		public DataDictionary(string[] aDatas, FlatFile aFlatFile)
		{
			this.FillData(aDatas, aFlatFile);
		}

		public DataDictionary(string aDataLine, FlatFile aFlatFile)
		{
			string[] theDatas	= aDataLine.Split(',');
			this.FillData(theDatas, aFlatFile);
		}

		private void FillData(string[] aDatas, FlatFile aFlatFile)
		{
			this.m_Name			= aDatas[0];
			this.m_DataTye		= (DataType)Enum.Parse(typeof(DataType), aDatas[1], true);
			this.m_Length		= Convert.ToInt32(aDatas[2]);
			
			int thePos			= (int)aFlatFile + 3;
			if (String.IsNullOrEmpty(aDatas[thePos]) == false)
			{
				this.m_Index	= Convert.ToInt32(aDatas[thePos]);
			}
		}

	}
}
