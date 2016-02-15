using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using Inventec.HPEDITS.Parser;

public partial class UploadASPFlatFile : System.Web.UI.UserControl
{
	private string m_SavePath;
	public string SavePath
	{
		get { return m_SavePath; }
		set { m_SavePath = value; }
	}

	private Inventec.HPEDITS.Parser.FlatFile m_FlatFile;
	public Inventec.HPEDITS.Parser.FlatFile FlatFile
	{
		get { return m_FlatFile; }
		set { m_FlatFile = value; }
	}

	private string m_ConnectionString;
	public string ConnectionString
	{
		get { return m_ConnectionString; }
		set { m_ConnectionString = value; }
	}

	private string m_DataDictionaryPath;
	public string DataDictionaryPath
	{
		get { return m_DataDictionaryPath; }
		set { m_DataDictionaryPath = value; }
	}


	protected void Page_Load(object sender, EventArgs e)
	{

	}
	protected void btnUpload_Click(object sender, EventArgs e)
	{
		if (this.fuASPFlatFile.HasFile)
		{
			string[] theFileName	= this.fuASPFlatFile.FileName.Split('.');
			string theFileSavePath	= String.Format("{0}\\{1}_{2}{3}{4}{5}{6}{7}.{8}", this.SavePath, theFileName[0], 
				DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, 
				theFileName[1]);

			this.fuASPFlatFile.SaveAs(theFileSavePath);
			Response.Write(theFileSavePath);

			DataDictionaryManagerment theDDM	= new DataDictionaryManagerment(this.DataDictionaryPath);
			SAPFlatFileParser theSAPParser;
			switch (this.FlatFile)
			{
				case FlatFile.Comn:
					theSAPParser	= new ComnParser(theFileSavePath, this.ConnectionString, theDDM);
					break;
				case FlatFile.Paltno:
					theSAPParser	= new PaltnoParser(theFileSavePath, this.ConnectionString, theDDM);
					break;
				case FlatFile.Edi850raw:
					theSAPParser	= new EDI850RAWParser(theFileSavePath, this.ConnectionString, theDDM);
					break;
                case FlatFile.Edi850rawINSTR:
                    theSAPParser = new EDI850RAWINSTRParser(theFileSavePath, this.ConnectionString, theDDM);
                    break;
				default:
					theSAPParser	= new SAPFlatFileParser(theFileSavePath, this.ConnectionString);
				    break;
			}
			theSAPParser.LoadDataFile();
		}
	}
}
