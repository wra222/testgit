using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

namespace Inventec.HPEDITS.Parser
{
	public class DataDictionaryManagerment
	{
		private List<DataDictionary> m_ComnList		= new List<DataDictionary>();	
		public List<DataDictionary> ComnList
		{
			get { return m_ComnList; }
			//set { m_ComnList = value; }
		}

		private List<DataDictionary> m_PaltnoList	= new List<DataDictionary>();
		public List<DataDictionary> PaltnoList
		{
			get { return m_PaltnoList; }
			//set { m_PaltnoList = value; }
		}

		private List<DataDictionary> m_Edi850rawList = new List<DataDictionary>();
		public List<DataDictionary> Edi850rawList
		{
			get { return m_Edi850rawList; }
			//set { m_Edi850raw = value; }
		}


        private List<DataDictionary> m_Edi850rawINSTRList = new List<DataDictionary>();
        public List<DataDictionary> Edi850rawINSTRList
        {
            get { return m_Edi850rawINSTRList; }
            //set { m_Edi850raw = value; }
        }


		public DataDictionaryManagerment(string aDataDictionaryFilePath)
		{
			LoadData(aDataDictionaryFilePath);
		}

		private void LoadData(string aFilePath)
		{
			StreamReader theReader = new StreamReader(aFilePath);
			string theDataLine;
			while ((theDataLine = theReader.ReadLine()) != null)
			{
				// remark
				if (theDataLine.StartsWith("//"))
				{
					continue;
				}
				// empty
				if (String.IsNullOrEmpty(theDataLine))
				{
					continue;
				}

				string[] theDatas = theDataLine.Split(',');
				//Comn
				if (String.IsNullOrEmpty(theDatas[3]) == false)
				{
					this.m_ComnList.Add(new DataDictionary(theDatas, FlatFile.Comn));
				}
				//Paltno
				if (String.IsNullOrEmpty(theDatas[4]) == false)
				{
					this.m_PaltnoList.Add(new DataDictionary(theDatas, FlatFile.Paltno));
				}
				//Edi850raw
				if (String.IsNullOrEmpty(theDatas[5]) == false)
				{
					this.m_Edi850rawList.Add(new DataDictionary(theDatas, FlatFile.Edi850raw));
				}
                //Edi850INSTR
                if (String.IsNullOrEmpty(theDatas[6]) == false)
                {
                    this.m_Edi850rawINSTRList.Add(new DataDictionary(theDatas, FlatFile.Edi850rawINSTR));
                }
			}
			this.m_ComnList.Sort(CompareDataDictionary);
			this.m_PaltnoList.Sort(CompareDataDictionary);
			this.m_Edi850rawList.Sort(CompareDataDictionary);
            this.m_Edi850rawINSTRList.Sort(CompareDataDictionary);

		}

		private static int CompareDataDictionary(DataDictionary X, DataDictionary Y)
		{
			return X.Index.CompareTo(Y.Index);
		}


		public void ReloadData(string aFilePath)
		{
			this.m_ComnList.Clear();
			this.m_PaltnoList.Clear();
			this.m_Edi850rawList.Clear();
            this.Edi850rawINSTRList.Clear();

			this.LoadData(aFilePath);
		}
	}
}
