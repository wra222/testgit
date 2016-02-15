








/*
����  ��������������
����  ��������
ʱ��  ��2000��8��
����  ��RecordSet -- ����Ҫ��ʾ�����ݼ�¼����
����ֵ��
*/

function clsTable(RecordSet,ObjectName)
{
//public property 	
	this.Height = 100;				//���߶ȣ�����������ͷ��
	this.Widths = null;				//����ֶο�����顣
	this.TableWidth = 100
	this.modi = new Array();				//�ɱ༭�ֶ����� true -- �ɱ༭��
	this.rs_main = RecordSet;		//���ݼ�¼����
	this.Title = true;				//�Ƿ���ʾ����	
	this.AddDelete = false;			//�Ƿ�������ɾ����¼
	this.RowStr	=""					//�õ���ǰ�е��ִ���
	this.arrColor = new Array("#fffbe7","#e7e7bd") //��¼�еı���ɫ	
	this.UseSort = ""				//ָ�������TDC�ؼ�
	this.IsEmpty = false			//�ж��Ƿ�ǰ����Ƿ�Ϊ��
	this.Divide = ","				//ʹ�õķָ���
	this.outerSelect = false		//ʹ���ⲿѡ��SELECT��ǩ����ѡ�����ݷ��ڸ��С�
	
//public Method
	this.Display = Display;			//�õ�HTML�ִ���
	this.MouseDown_Event = MouseDown_Event //����¼�
	this.AddEvent = AddEvent;		//����һ����¼��
	this.GetRowNumber = GetRownum	//�õ���ǰ������ֵ��
	this.Refresh = Refresh			//ˢ�±��
	this.ColumnStyle = ColumnStyle	//�趨һ��������ԡ�
	this.TableStyle = TableStyle	//�趨��������ԡ�
	this.HeadStyle = HeadStyle		//�趨����������ԡ�
	this.Delete = DelRow			//ɾ����ǰ��
	this.Append = Append			//����һ�У������ֶ���ʽ��
	this.SetRow = SetRow			//����һ������
	this.UseControl = UseControl	//ʹ���û�ָ���Ĳ���ؼ���
	this.ControlReturn = ControlReturn //�����û�����ؼ����ڸ�����ݡ�
	this.PageColumn = PageColumn    //���ú����ҳ����
	this.HideColumn = new Array()			//����ָ�����ֶ�
	this.seedControl = new Array()
	this.ControlValue = new Array()
	this.FieldsType = new Array()
	this.MaxSize = new Array()
	this.EventRow = EventRow
	this.ClearV = ClearV
	
//private property and method
	this.Name = ObjectName;			//���ɶ�������ơ�
    this.PreRow = -1;				//����ǰһ�м�¼��
	this.strColumn = ""				//�������ִ���
	this.strTable = 'Style="color:black;font-size:9pt"'	//�������ִ���
	this.strHead = 'Style="font-family:����;font-size:9pt"'	//���������ִ���	
	this.AskSum = false;					//�Ƿ����ӡ��ϼơ�
	this.SumArr = new Array(RecordSet.Fields.Count);	//�ϼ��ֶκϼ�ֵ��
	this.SumDot = 2
	this.EditRow = EditRow;			//�Ե�ǰ�н��пɱ༭����
	this.UseSum = UseSum;			//���Ӻϼ��ֶΣ����ظ����á�
	this.currentRow = -1;			//��¼Ŀǰ������ֵ��
    this.preElement = null;			//���浱ǰ�У��������е������ʱ�ָ�����
    this.preColor = null;			//���浱ǰ����ɫ��
	this.BackRefresh = BackRefresh  //ˢ�±���ɫ
	this.SaveRow = SaveRow			//��������е����ݵ���¼����
	this.Sort = Sort
	this.UseForm = ""
	this.Pages = 1
	this.PageArr = new Array()		
	this.NewPage = NewPage
	this.WhichPage = 0
	this.SortColumn = -1			//ָ�����ĸ��ֶ�����
	this.SortAsc = ""				//���򷽷���""Ϊ����"-"Ϊ����
	this.HighLightRow = HighLightRow
	this.DealKeyPress = DealKeyPress
	this.DealBack = DealBack
	//this.UnderKey = false
	this.ShowCount = false
	this.FreeSelect = FreeSelect
	this.StrFree = ""
	this.StrCookie = ""
	this.ShowFree = ShowFree
	this.SetFree = SetFree
	this.NotEmpty = -1
	
	this.Dbclick = "this.Dbclick = ''"
	this.SortLeft = 0
	this.RestorScroll = RestorScroll
	
	this.Filter = FilterReset
	this.strFilter = this.rs_main.Fields(0).Name + " <> UndEfinedp"
	this.ControlPath = "../../CommonControl/"
	this.NoKey = false			//true -- ��֧�ּ��̿�������
	this.UpDown = null			//�����������ƶ�ʱ���õĺ���
	
}

function SetCookieTable(sName, sValue)
{
  document.cookie = sName + "=" + escape(sValue) + ";expires=Mon, 31 Dec 9999 23:59:59 UTC;";
}


function GetCookieTable(sName)
{
  // cookies are separated by semicolons
  var aCookie = document.cookie.split("; ");
  for (var i=0; i < aCookie.length; i++)
  {
    // a name/value pair (a crumb) is separated by an equal sign
    var aCrumb = aCookie[i].split("=");
    if (sName == aCrumb[0]) 
    {
		if (aCrumb.length > 1)	    
			return unescape(aCrumb[1]);
		else
			return unescape("");
    }
  }

  // a cookie with the requested name does not exist
  return null;
}


function ShowFree()
{
//alert("ShowFree")
	var sFree = GetCookieTable(this.StrFree)
	if (sFree == null || sFree == "" || sFree == "null")
	{
		this.SetFree()
		return
	}
	var ArrFree = sFree.split(",")
	this.NewPage(0,ArrFree)
}

function SetFree()
{
//alert("SetCookieTable")
//document.cookie = ""
	this.StrCookie = GetCookieTable(this.StrFree)
	var sFeatures="dialogHeight: " + 360 + "px;dialogWidth: " + 340 + "px;status:no;help:0;";
	var para = this;
	var sCookie=window.showModalDialog(this.ControlPath + "SelectShow.asp", para, sFeatures)
	if (sCookie != null && sCookie != "null")
	{	SetCookieTable(this.StrFree,sCookie)
		if (sCookie != "")
		{	//��ʾ�Զ����ֶ�
			var ArrFree = sCookie.split(",")
			this.NewPage(0,ArrFree)
		}
		else
		{	
			if (this.PageArr.length > 1)
				//����ҳ����ʾ��һҳ
				this.NewPage(1)
			else
			{	//û��ҳ������ʾȫ��
				var ArrAll = new Array()
				for (var ki = 0; ki<this.Widths.length; ki++)
					ArrAll[ki] = ki			
				this.NewPage(0,ArrAll)

				//��ť������
				if (this.StrFree != "")
					eval(this.UseForm + this.Name + "0img.src = '" + this.ControlPath + "Images/" + "0.gif'")
				
			}
		}
	}

}

/*
����  ������ѡ��ָ���ֶΡ�
����  ��������
ʱ��  ��2000��11��
����  ��SaveName �������ݵ����ơ�
*/
function FreeSelect(SaveName)
{
	this.StrFree = SaveName
	//this.Pages ++
}
/*
����  ������ָ���С�
����  ��������
ʱ��  ��2000��11��
����  ��RowNum �кţ�base 0��
*/
function HighLightRow(RowNum)
{

	
	

	var oNode
	eval("oNode = " + this.Name + "TableSpan.firstChild.firstChild.childNodes(" + RowNum + ").firstChild")
	this.MouseDown_Event(oNode,null,true)
	var iTop
	eval("iTop = " + this.Name + "TableSpan.scrollTop" )	

	if (oNode.offsetTop < iTop)
		iTop = oNode.offsetTop
	else if (oNode.offsetTop + oNode.offsetHeight > iTop + this.Height -20)
		iTop = oNode.offsetTop + oNode.offsetHeight - this.Height + 20
	
	eval(this.Name + "TableSpan.scrollTop = " + iTop)	

}

/*
����  ���Ƿ��ҳ��ʾ�ֶΣ�ָ���ּ�ҳ��ʾ��
����  ��������
ʱ��  ��2000��8��
����  ��Columns ָ��������ֶ�
*/
function PageColumn(Page0,Page1,Page2,Page3,Page4)
{
	if (Page0 != null)
		this.Pages ++
	if (Page1 != null)
		this.Pages ++
	if (Page2 != null)
		this.Pages ++
	if (Page3 != null)
		this.Pages ++
	if (Page4 != null)
		this.Pages ++
		
	for (var i=0; i<this.Pages-1; i++)
		eval("this.PageArr[i + 1] = Page" + i)

}

/*
����  ���Ƿ��ҳ��ʾ�ֶΡ�
����  ��������
ʱ��  ��2000��8��
����  ��PageNum ָ����ʾ�ڼ�ҳ
        Free �Ƿ�Ϊ����ѡ��ҳ 
*/
function NewPage(PageNum,Free,SetHide)
{
	//�õ���ҳ���ֶ���
	var temp
	if (Free != null)
		this.PageArr[PageNum] = Free
	
	temp = this.PageArr[PageNum]
	if (temp == null || temp =="null" || temp == "")
	{
		this.SetFree()
			return
	}
	
	//�������ֶζ�����ʾ��ֻ��ʾ��ҳ�ֶ�
	for(var i=0; i<this.Widths.length; i++)
		this.HideColumn[i] = false
	for(var i=0; i<temp.length; i++)
		this.HideColumn[temp[i]] = true

	if (SetHide == true)
		return
		
	if (this.preElement != null)
	{	//��¼�µ�ǰ�����е�λ��
		var oldPos	
		eval("oldPos = " + this.Name + "TableSpan.scrollTop")	
		oldPos = this.preElement.offsetTop - oldPos
	}
		
	//ˢ�º���ʾ�벻��ʾ�����ò���Ч��
	var i,oRows,iTemp

	//���������ֶ����ء�
	eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild.childNodes")
	for (i=0; i<oRows.length; i++)
	{
			for (var n=0; n<this.HideColumn.length; n++)
				if (this.HideColumn[n] != true)
					oRows(i).childNodes(n).style.display = "none"
				else
					oRows(i).childNodes(n).style.display = "block"
	}


	iTemp = 23 //����¿��
	
	//�������ر�ͷ����β���ء�
	for (var n=0; n<this.HideColumn.length; n++)
	{
			
		if (this.HideColumn[n] != true)
		{	
			if (this.Title)
				eval(this.Name + "H" + n + ".style.display = 'none'")
			if (this.AskSum)
				eval(this.Name + "T" + n + ".style.display = 'none'")
		}
		else
		{	
			if (this.Title)
				eval(this.Name + "H" + n + ".style.display = 'block'")
			if (this.AskSum)
				eval(this.Name + "T" + n + ".style.display = 'block'")
			iTemp += this.Widths[n]
		}
	}

	if (this.HideColumn.length >= this.Widths.length)
	{
		iTemp -= 4
		if (this.TableWidth > iTemp)
//			{	eval(this.Name + "TableAll.width = " + iTemp)
			 eval(this.Name + "TableSpan.style.width = " + iTemp)
//			}
		else
		{	//eval(this.Name + "TableAll.width = " + this.TableWidth)
//			eval(this.Name + "TableSpan.style.width = " + 10)
			eval(this.Name + "TableSpan.style.width = " + this.TableWidth)
		}

		eval(this.Name + "TableHead.style.width = " + iTemp)
		if (this.AskSum)
		eval(this.Name + "TableTail.style.width = " + iTemp)
		iTemp -= 18
//		eval(this.Name + "TableBody.style.width = " + 10)
//		eval(this.Name + "TableBody.width = " + 10)
		
		eval(this.Name + "TableBody.style.width = " + iTemp)
		eval(this.Name + "TableBody.width = " + iTemp)

	}

	//��TableBody��ȱ�Сʱ�����������������Χȴ���ı䣬ֻ��ʹ�����·������ܸı䡣
	var oNode
	eval("oNode = " + this.Name + "TableBody.lastChild.lastChild")
	if (oNode.style.height == "20px")
		oNode.style.height = "21px"
	else
		oNode.style.height = "20px"
	//��TableBody��ȱ�Сʱ�����������������Χȴ���ı䣬ֻ��ʹ�����Ϸ������ܸı䡣

/*	???
	
	if (this.preElement != null)
	{
		this.preElement.style.backgroundColor = "midnightblue"
		this.preElement.style.color = "white"
	}
*/	
	this.WhichPage = PageNum
	
	//���¶�λ�����С�
	if (this.preElement != null)
	{	var iTop
		iTop = this.preElement.offsetTop - oldPos
		eval(this.Name + "TableSpan.scrollTop = " + iTop)	
	}

	
	//�����а�ţ̌��ֻ����ҳ��ť����
	if (this.StrFree != "")
		eval(this.UseForm + this.Name + "0img.src = '" + this.ControlPath + "Images/" + "0.gif'")
	for (var j=1; j<this.Pages; j++)
		eval(this.UseForm + this.Name + j + "img.src = '" + this.ControlPath + "Images/" + j +".gif'")

	eval(this.UseForm + this.Name + PageNum + "img.src = '" + this.ControlPath + "Images/" + PageNum +"_.gif'")

}



/*
����  ���Ƿ�����
����  ��������
ʱ��  ��2000��8��
����  ��Columns ָ��������ֶ�
*/
function Sort(column)
{
	
	//��������ֶ�������
	if (this.rs_main.Fields(column).Name == "���")
		return
		
	var Ascend, Element
	
	Ascend = "" //�����Ƿ���
	Element = window.event.srcElement
	
	//�ж��Ƿ��м�¼�ɹ�����
	if (this.rs_main.RecordCount >1)
	{
		//�Ƿ�����������¼���������
		if (Element.className == "HeadSort")
		{
			//�Ƿ���ǰһ��������С�
			if (this.SortColumn == column)
			{
				//������任����ʽ�� 
				if (this.SortAsc == "")
					//��-->��
					this.SortAsc = "-"
				else
					//��-->��
					this.SortAsc = ""
			}
			else
			{	//����������
				this.SortColumn = column
				this.SortAsc = ""
			}
			
		}
		eval("this.SortLeft = " + this.Name + "TableSpan.scrollLeft" )		
		//ˢ�½��棬�Ա㱣�浱ǰ����Ϣ
		this.Refresh()
		//ִ�������趨TDC�ؼ�����
		eval(this.UseSort + ".CaseSensitive = 'FALSE'")
		eval(this.UseSort + ".Sort = " + "'" + this.SortAsc + this.rs_main.Fields(column).Name + "'")
		this.Filter()
	}
}

function FilterReset(strFilter)
{
	var sFilter
	sFilter = ""
	if (strFilter != "" && strFilter != null)
		sFilter = " & (" + strFilter + ")"

	this.strFilter = this.rs_main.Fields(0).Name + " <> UndEfinedp" + sFilter
	eval(this.UseSort + ".object.Filter = '" + this.strFilter + "'")
	eval(this.UseSort + ".Reset()")
}


function RestorScroll()
{
	eval(this.Name + "TableSpan.scrollLeft = " + "this.SortLeft")		


}


/*
����  �������û�����ؼ����ڸ�����ݡ�
����  ��������
ʱ��  ��2000��8��
����  ��strVal -- Ҫ��ʾ�����ݡ�
        Col -- �����ֶε�λ�á�
*/
function ControlReturn(strVal,col)
{
	if (this.seedControl.length >= col)
		eval(this.Name + "Cshow" + col + ".innerText = strVal")
}

/*
����  ��ʹ���û�ָ������ؼ���
����  ��������
ʱ��  ��2000��8��
����  ��Col -- �����ֶε�λ�á�
        strContent -- ����ؼ��������ִ�(HTML)
        ShowChange -- true   ��ʾ�ؼ�����ֵ
        ShowChange -- false  ����ʾ�ؼ�����ֵ
        Position -- "Front"  �ؼ���ʾ��ֵǰ
        Position -- "Behind"  �ؼ���ʾ��ֵ��
*/
function UseControl(Col,strContent,ShowChange,Position)
{
	//this.htmlControl = strContent
	if (Col<this.Widths.length)
    {
    	this.seedControl[Col] = strContent
    	if (ShowChange == false)
    		this.ControlValue[Col] = " style='display:none'"
    	else
    		this.ControlValue[Col] = " style='display:block'"
    }
		
}
/*
����  ����Ӻϼ��ֶΡ�����ε��ã���Զ���ֶν��кϼơ�
����  ��������
ʱ��  ��2000��8��
����  ��Column -- ��Ҫ�ϼƵ��ֶα�ţ�base 0����
����ֵ��
*/
function UseSum(Column)
{
	if (Column <= this.Widths.length)
	{
		this.AskSum = true;
		//����¼Ϊ�գ��򲻺ϼơ�
		this.SumArr[Column] = 0
		if (this.rs_main.RecordCount > 0 )
		{
			this.rs_main.MoveFirst()
			//���ν��кϼ�
			while (!this.rs_main.EOF)
			{
				//�����ֶβ�����ֵ�ͣ��򲻽��кϼơ�
				if (typeof(this.rs_main.Fields(Column).value) != "number")
				{	this.rs_main.MoveNext()
					continue
				}
				//���кϼ�
				if (this.rs_main.Fields(Column).value != null)
					this.SumArr[Column] += parseFloat(this.rs_main.Fields(Column).value)
				this.rs_main.MoveNext()
			}	
			
			this.SumArr[Column] = Math.round(this.SumArr[Column] * Math.pow(10,this.SumDot)) / Math.pow(10,this.SumDot)
		}
	}
}


/*
����  ���趨��������ԡ�
����  ��������
ʱ��  ��2000��8��
����  ��ColNum -- ���ֶα�ţ�base 0��
        strStyle -- ��ʽ���ִ������磺"Width:80; color:red"��
����ֵ��
*/
function ColumnStyle(ColNum,strStyle)
{
	
	this.strColumn += "   ." + this.Name + ColNum + "{" + strStyle + "}    " 
}

/*
����  ���趨��������ԡ�
����  ��������
ʱ��  ��2000��8��
����  ��strStyle -- ��ʽ���ִ������磺"Width:80; color:red"��
����ֵ��
*/
function TableStyle(strStyle)
{
	
	this.strTable = ' Style="' + strStyle + ' "' 
}

/*
����  ���趨����������ԡ�
����  ��������
ʱ��  ��2000��8��
����  ��strStyle -- ��ʽ���ִ������磺"Width:80; color:red"��
����ֵ��
*/
function HeadStyle(strStyle)
{
	
	this.strHead =  strStyle  
}


/*
����  �����а�����ʾ����HTML�ִ������ɡ�
����  ��������
ʱ��  ��2000��8��
����  ��
����ֵ�����HTML�ִ���
*/
function Display(strJudge,NA,noAdd)
{
	//NewPage(this.WhichPage,null,true)
	//�������ֶζ�����ʾ��ֻ��ʾ��ҳ�ֶ�
	var temp
	if (this.PageArr.length > 1 || this.StrFree != "")
	{
		if (this.StrFree != "")
		{	temp = GetCookieTable(this.StrFree)
			if (temp != null && temp != "null" && temp != "") 
				this.PageArr[0] = temp.split(",")
			else 
			{	if (this.WhichPage == 0)
					this.WhichPage = 1
			}
		}
		else
		{
			if (this.WhichPage == 0)
				this.WhichPage = 1
		}
		
		temp = this.PageArr[this.WhichPage]

		if (temp != "" && temp != null)
		{
			for(var i=0; i<this.Widths.length; i++)
				this.HideColumn[i] = false
			for(var i=0; i<temp.length; i++)
				this.HideColumn[temp[i]] = true
		}
	}

	var sTemp, field, i,sWidth, sumWidth, iWidth, sHand, sHide, sSort, sStyle,bJudge
    var sUse = new Array()
				
	var sBody = new Array()
	sumWidth = 23
	for (i=0; i<this.Widths.length; i++)
	{	if (this.HideColumn.length > 0)
		{	if (this.HideColumn[i] == true)
				sumWidth += (this.Widths[i])
		}
		else
			sumWidth += (this.Widths[i])
	}
	//******************************************** �������� ***********************************
	var TABLE_BEGIN = "<TABLE name='" + this.Name + "TableAll' Id=" + this.Name + "TableAll  width=" + this.TableWidth + "px border=1 cellpadding=0 cellspacing=0 onmouseup='return " + this.Name + ".MouseDown_Event()'><TR><TD>"
	var TABLE_END = "</TD></TR></TABLE>"
	
	sumWidth -= 4
	
	var TABLE_HEAD_H = "<SPAN id='" + this.Name + "SpanHead' style='WIDTH:"+ this.TableWidth + ";OVERFLOW:hidden'><TABLE id='" + this.Name + "TableHead' cellpadding=2  cellspacing=0 style='border-style:solid;border-width:1px;border-color:black' style='width:" + sumWidth + "px'>"
	var TABLE_HEAD_T = "<SPAN id='" + this.Name + "SpanTail' style='WIDTH:"+ this.TableWidth + ";OVERFLOW:hidden'><TABLE id='" + this.Name + "TableTail' cellpadding=2  cellspacing=0 style='border-style:solid;border-width:1px;border-color:black' style='width:" + sumWidth + "px'>"
	
	sumWidth -= 18
	var TABLE_HEAD = "<TABLE id='" + this.Name + "TableBody' cellpadding=2  cellspacing=1 bordercolor=Silver bgcolor=Gray " + this.strTable + " onKeyDown='" + this.Name + ".DealKeyPress()' style='width:" + sumWidth + "px'>"
	var TABLE_TAIL_H = "</TABLE></SPAN>"
	var TABLE_TAIL = "</TABLE>"

	var HEAD_ROW_BEGIN = "<TR style='word-break:break-all;border-color:black' name=Head bgcolor=#cebe9c align=center>"
	var HEAD_ROW_END = "</TR>"

	var HEAD_BODY_BEGIN = "<TD " + "Style='border-right:1px solid black;" + this.strHead +  "' ><b>"
	var HEAD_BODY_END = "</TD>"

	var TAIL_BODY_BEGIN = "<TD " + "Style='border-right:1px solid black;' >"
	var TAIL_BODY_END = "</TD>"

	var ROW_BEGIN = "<TR classid='" + this.Name + "TableRow' align=left bordercolor=Silver name=rownum >"
	var ROW_END = "</TR>"

	var CELL_BEGIN = "<TD class=" + this.Name + "FFIIEE>"
	var CELL_END = "</TD>"
	
	var PAGE_BUTTON = '<IMG ID=' + this.Name + '#%&img src="' + this.ControlPath + 'Images/#%&.GIF" onmousedown="' + this.Name + '.NewPage(#%&)' + '" style="CURSOR: hand">'
	//var FREE_BUTTON = '<button onclick="' + this.Name + '.ShowFree()"><IMG SRC="../../CommonControl/Images/Free.gif"></button> '
	var FREE_BUTTON = '<IMG ID=' + this.Name + '0img src="' + this.ControlPath + 'Images/0.GIF" onmousedown="' + this.Name + '.ShowFree()' + '" style="CURSOR: hand">'
	if (this.WhichPage == 0)
		FREE_BUTTON = FREE_BUTTON.replace("0.GIF","0_.GIF")
	//var SET_BUTTON = '<button onmousedown="' + this.Name + '.SetFree()"><IMG SRC="../../CommonControl/Images/Set.gif"></button> '
	var SET_BUTTON = '<IMG ID=' + this.Name + 'setbutton src="' + this.ControlPath + 'Images/SetD.GIF" onmousedown="' + this.Name + '.SetFree()' + '" style="CURSOR: hand">'
	//************************************************************************************************
	

	sUse[0] = TABLE_BEGIN 
	
		var sDisplay = ""
	//�Ƿ���ʾ���⡣
	if (this.Title)
	{	//������ͷ
		sSort = ""
		sUse[sUse.length] = TABLE_HEAD_H + HEAD_ROW_BEGIN 
		//������ı���ͷ
		for (field=0; field<this.Widths.length; field++)
		{
			//������ʾ�ֶ�
			if (this.HideColumn.length > 0)
			{
				if (this.HideColumn[field] != true)
					sDisplay = " style='display:none'"
				else
					sDisplay = ""
			}
			
			//�Ƿ�ָ���ֶο��
			iWidth = this.Widths[field] - 5
			sWidth = HEAD_BODY_BEGIN.replace("<TD","<TD width=" + iWidth  + "px id=" + this.Name + "H" + field + sDisplay)
			sUse[sUse.length] = sWidth
			if (this.UseSort == "")
				//��ָ������
				sUse[sUse.length] = this.rs_main.Fields(field).Name
			else
			{	//ָ������
				if (field == this.SortColumn)
				{
					if (this.SortAsc == "")
						sSort = " background:transparent url(" + this.ControlPath + "Images/up.GIF) no-repeat bottom right;"
					else
						sSort = " background:transparent url(" + this.ControlPath + "Images/down.GIF) no-repeat bottom right;"

				}
				sUse[sUse.length] =  "<div class='HeadSort' onclick='" + this.Name + ".Sort(" + field + ")" + "' style='text-decoration:underline; CURSOR:hand;" + sSort + "'>" + this.rs_main.Fields(field).Name + "</div>"
			
				sSort = ""
			}
			sUse[sUse.length] = HEAD_BODY_END
			
		}
		
		//������֮��
		sUse[sUse.length] =  "<TH width=13 " + this.strHead + "></TH>"
		//������¼ͷ��
		sUse[sUse.length] =  HEAD_ROW_END + TABLE_TAIL_H
	}

	//������
	//����ʼ�п�ʼ����lines�м�¼
	if (this.rs_main.RecordCount > 0 )
	{
		this.rs_main.MoveFirst()
	}
	sBody[0] = TABLE_HEAD
	sStyle = ""
	var sBackC = ""
	var jj = 0
	for (j = 0; j<this.rs_main.RecordCount; j++)
	{	//����β��������ѭ����
		if (this.rs_main.EOF)
			break
		
		if (strJudge == null)
			bJudge = true
		else
			eval("bJudge = " + strJudge)
		
		if (bJudge == false)
		{
			this.rs_main.MoveNext()
			continue
		}

		//���ñ���ɫ
		sBackC = " Style='background-color:" + this.arrColor[jj % 2] + "; height:20px; color:black; word-break:break-all'"
		sBody[sBody.length] =  ROW_BEGIN.replace("rownum", ""+ jj + sBackC)
		jj += 1
		//���ż�¼�еĸ����ֶΡ�
		for (field=0; field<this.Widths.length; field++)
		{	//�Ƿ�ָ����ȡ�

			//������ʾ�ֶ�
			if (this.HideColumn.length > 0)
			{
				if (this.HideColumn[field] != true)
					sDisplay = " style='display:none'"
				else
					sDisplay = ""
			}
			
			sWidth = CELL_BEGIN.replace("FFIIEE","" + field + sDisplay)
			
			sBody[sBody.length] = sWidth
			if (this.rs_main.Fields(field).Name == "���")
			{	sBody[sBody.length] =  jj + ""
				this.rs_main.Fields(field).Value = jj + ""
			}
			else
				sBody[sBody.length] =  this.rs_main.Fields(field).Value
			sBody[sBody.length] = CELL_END
		}
		
		//��¼������
		sBody[sBody.length] =  ROW_END
		//��һ����¼��
		this.rs_main.MoveNext()
	}
	
	
	//����һ�������Ա������������С�
	//���ñ���ɫ
	sBackC = " Style='background-color:" + this.arrColor[jj % 2] + "; height:20px; color:black; word-break:break-all'"	
	sBackC =  ROW_BEGIN.replace("rownum", ""+ jj + sBackC)
	jj += 1
	sBody[sBody.length] =  sBackC.replace("TableRow'","TableRow' id=" + this.Name + "AddRow")
	for (field=0; field<this.Widths.length; field++)
	{	
		//������ʾ�ֶ�
		if (this.HideColumn.length > 0)
		{
			if (this.HideColumn[field] != true)
				sDisplay = " style='display:none'"
			else
				sDisplay = ""
		}
	
		//�Ƿ�ָ����ȡ�
		iWidth = this.Widths[field] - 5
		//sStyle = sStyle + "  " + "." + this.Name + field + "{width:" + iWidth + " }"
		sWidth = CELL_BEGIN.replace("FFIIEE","" + field + sDisplay + " name='Virtul'  width=" + iWidth +  " id=" + this.Name + "Add onclick='return " + this.Name + ".AddEvent()'")
		sBody[sBody.length] =  sWidth
		sBody[sBody.length] = CELL_END
	}

	//���ӿ���,���հ���
	for(var bl = jj ; bl < this.Height / 20 + 1; bl++)
	{
		sBackC = " Style='background-color:" + this.arrColor[bl % 2] + "; height:20px; color:black; word-break:break-all'"			//������ʾ�ֶ�
		sBody[sBody.length] =  ROW_BEGIN.replace("rownum", ""+ bl + sBackC)
		for (field=0; field<this.Widths.length; field++)
		{
			if (this.HideColumn.length > 0)
			{
				if (this.HideColumn[field] != true)
					sDisplay = " style='display:none'"
				else
					sDisplay = ""
			}
	
			//�Ƿ�ָ����ȡ�
			iWidth = this.Widths[field] - 5
			//sStyle = sStyle + "  " + "." + this.Name + field + "{width:" + iWidth + " }"
			sWidth = CELL_BEGIN.replace("FFIIEE","" + field + sDisplay + " name='Virtul'  width=" + iWidth)
			sBody[sBody.length] =  sWidth
			sBody[sBody.length] = CELL_END
		}	
	}
	
	//��¼������
	sBody[sBody.length] =  ROW_END
	sBody[sBody.length] =  TABLE_TAIL
	sWidth = sBody.join("")
	//�Ƿ�ָ���߶ȡ���ָ��������Զ�������������
	if (sumWidth+18 >= this.TableWidth)
		sumWidth = this.TableWidth -18
	if (this.Height == 0)
		sUse[sUse.length] = sWidth
	else
	{	if (this.AskSum)
			sUse[sUse.length] = "<SPAN id='" + this.Name + "TableSpan' style ='height:" + this.Height + ";width:" + (sumWidth + 18) + "; OVERFLOW:auto' onscroll='" + this.Name + "SpanHead.scrollLeft = this.scrollLeft;" + this.Name + "SpanTail.scrollLeft = this.scrollLeft	'>" + sWidth + "</SPAN>"
		else
			sUse[sUse.length] = "<SPAN id='" + this.Name + "TableSpan' style ='height:" + this.Height + ";width:" + (sumWidth + 18) + "; OVERFLOW:auto' onscroll='" + this.Name + "SpanHead.scrollLeft = this.scrollLeft;'>" + sWidth + "</SPAN>"
	}

	//����β���Ƿ���ʾ����
	//�Ƿ���ʾ����
	if (this.AskSum)
	{
		
	//������β
		sUse[sUse.length] = TABLE_HEAD_T + HEAD_ROW_BEGIN.replace("<TR ","<TR id=" + this.Name + "Sums ")
		//������ı���ͷ
		for (field=0; field<this.Widths.length; field++)
		{	
			//������ʾ�ֶ�
			if (this.HideColumn.length > 0)
			{
				if (field == 0)
				{	if (this.HideColumn[field] != true)
						sDisplay = " style='display:none;text-align:right'"
					else
						sDisplay = " style='font-size:9pt';text-align:right'"
				}
				else
				{	if (this.HideColumn[field] != true)
						sDisplay = " style='display:none;font-size:9pt;font-weight:400; text-align:right'"
					else
						sDisplay = " style='font-size:9pt;font-weight:400; text-align:right'"
				}
			}
		
		
			//�Ƿ�ָ���ֶο��
			iWidth = this.Widths[field] - 5
			sWidth = TAIL_BODY_BEGIN.replace("<TD","<TD width=" + iWidth  + "px id=" + this.Name + "T" + field + sDisplay)
			sUse[sUse.length] = sWidth
			
			if (field==0)
				sUse[sUse.length] = "�ϼ�"
			else
				if (this.SumArr[field] != null)
					sUse[sUse.length] = this.SumArr[field]
				else
					sUse[sUse.length] = "��"
			
			sUse[sUse.length] = TAIL_BODY_END
		}
		sUse[sUse.length] = "<TD width=13 " + this.strHead + "></TD>"
		sUse[sUse.length] = HEAD_ROW_END + TABLE_TAIL_H
	}
		
	
	
	//���ر���HTML�ַ�����
	sUse[sUse.length] = TABLE_END
	
	//�Ƿ�����ҳѡ��
	sTemp = sUse.join("")

	if (this.Pages > 1 || this.ShowCount == true ||this.StrFree != "")
	{
		sHide = ""
		if (this.Pages>1)
			for(var n=1; n<this.Pages; n++)
			{
				
				sHide += PAGE_BUTTON.replace("#%&","" + n)
				if (n == this.WhichPage) 
					sHide = sHide.replace("#%&","" + n + "_")
				else
					sHide = sHide.replace("#%&","" + n)
				sHide = sHide.replace("#%&","" + n)

			}
		//�Ƿ���ʾ��¼��
		var sCount = ""
		if (this.ShowCount == true)
			sCount = "��¼����" + this.rs_main.RecordCount
			
		//�Ƿ���ʾ����ѡ���ֶ�
		var sFree = ""
		if (this.StrFree != "")
		{
			sFree = SET_BUTTON
			sHide = FREE_BUTTON + sHide
		}
		
		sTemp = "<TABLE width=" + this.TableWidth + "><TR><TD align=left id=" + this.Name + "Count style='font-family:����;font-size:9pt'>" + sCount + "</TD><TD align=middle>" + sFree + "</TD><TD align=right>" + sHide + "</TD></TR></TABLE>" + sTemp
	}
	//�����и�ʽ
	sTemp = sTemp + "   " + "<Style> <!--" + this.strColumn + sStyle + "--> </style>"

	//��֪Ϊʲô���ռ�¼���޷�������ʱAddNew��ֻ�ó����²ߡ�--- 1
	if (this.rs_main.RecordCount < 1 && noAdd != true)
	{	
		this.rs_main.AddNew()
		this.rs_main(0) = "UndEfinedp"
		
		this.IsEmpty = true
	}
	else
		this.IsEmpty = false

	//������������������ʾ��	
	this.preElement = null
	this.PreRow = -1
	this.currentRow = -1;
	this.RowStr = ""

	return sTemp

}



/* 
���ܣ�  ˢ�±���ɫ��
���ߣ�  ������
���ڣ�  2000.7
����ֵ��������괦��������¼���ݡ��ֶ����ֶμ��ÿո�ֿ���
*/
function BackRefresh()
{
	var i,j,oRows, oNode, iTemp
	//��������¼
	this.ClearV()
	
	//�Ƿ��ҳ��ʾ
	//if (this.Pages > 0)
	//	this.NewPage(this.WhichPage)

	eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild.childNodes")
	j = oRows.length
	i=0
	
	if (this.Height / 20 + 1 > j)
		bTemp = true
	else
		bTemp = false
	//eval("bTemp = (this.Height >= " + this.Name + "TableSpan.firstChild.offsetHeight;")
	//�жϱ��Ƿ񹻸߶�
	eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild") //.childNodes

	while (bTemp)
	{
		oNode = document.createElement("TR");
		//oNode.name = "BlankBack"
		oNode.classid= this.Name + "TableRow"
		//if (i==0)
			oNode.id = this.Name + "BlankBack"
		//i++
		for (field=0; field<this.Widths.length; field++)
		{	
			oTemp = document.createElement("TD")		
			//if (field == 0)
			oTemp.innerText = "��"
			oTemp.name = "Virtul"
			if (this.HideColumn.length > 0)
				if (this.HideColumn[field] == false)	
					oTemp.style.display = "none"
			
			oTemp.className = field
			oTemp.width = this.Widths[field] - 5
			oNode.insertBefore(oTemp)
		}		
		oRows.insertBefore(oNode)
		
		eval("j = " + this.Name + "TableSpan.firstChild.offsetHeight")
		if (this.Height >= j)
			bTemp = true
		else
			bTemp = false
		
	}

	
	//ȫ����
	eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild.childNodes")
	for (i=0; i<oRows.length; i++)
	{
			//�ӱ���ɫ
			oRows(i).style.backgroundColor = this.arrColor[i % 2]
			oRows(i).style.color = "black"
			oRows(i).style.height = "20px"
			//�����к�
			//if (oRows(i).classid == "TableRow")
			oRows(i).name = i
	}
}



/* 
���ܣ�  �ӱ����ɾ��ѡ�е��С�
���ߣ�  ������
���ڣ�  2000.7
*/
function DelRow()
{
	var oRows
	//�Ƿ���ѡ�е���
	if (this.preElement != null)
		if (this.preElement.firstChild.name != "Virtul")
		{
			//ɾ������
			eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild.removeChild(this.preElement)")
			this.rs_main.MoveFirst()
			this.rs_main.Move(this.PreRow)
			this.rs_main.Fields(0).value = "UndEfinedp"		
			this.rs_main.Delete(1)
			//this.rs_main.MoveFirst()
			//this.rs_main.Update()
			//ѡ�б�־�ÿ�
			this.preElement = null
			this.PreRow = -1
			this.currentRow	= -1
			this.RowStr = ""

			//ˢ�±���
			this.BackRefresh()
			
			this.ClearV()

		}
}


/* 
���ܣ�  ����ѡ���е����ݡ�
���ߣ�  ������
���ڣ�  2000.7
������  Content -- �������ݣ���","�ָ���
*/
function SetRow(Content,NoSet)
{
	var oNodes,i,strTD
	if (this.preElement == null | this.preElement.firstChild.name == "Virtul")
		return null
	//��������
	strTD = Content.split(this.Divide)
	//�õ����е�Ԫ��
	oNodes = this.preElement.childNodes
	this.rs_main.MoveFirst()
	this.rs_main.Move(this.PreRow)
	//�ֱ�ֵ
	for (i=0;i<oNodes.length;i++)
	{
		//����ֵ�ֶ��Ѹ��꣬���˳�
		if ( i > strTD.length)
			break
		
		//�����ǲ���ؼ�ʱ���Ÿ�ֵ
		if (this.seedControl[i] == null || this.modi[i] == true)
		{	
			if (NoSet == i)
				continue;
			//��֤�Ƿ�Ϊ��ֵ
			if (this.FieldsType[i] == "num")
			{
				if (parseFloat(strTD[i]) * 1 != parseFloat(strTD[i]))
					continue;
			}
		
			if (this.modi != null)
			{
				if (this.modi[i] == true)
				{	
					eval(this.UseForm + this.Name + "Text" + i + ".value = '" + strTD[i] + "'")
					this.rs_main.Fields(i).value = strTD[i]
				}
				else
				{	oNodes(i).innerText = strTD[i]
					this.rs_main.Fields(i).value = strTD[i]
				}
				
			}	
			else
			{	oNodes(i).innerText = strTD[i]
				this.rs_main.Fields(i).value = strTD[i]
			}
		}
	}
	
	this.RowStr = Content
}	

/* 
���ܣ�  ���������һ�С�
���ߣ�  ������
���ڣ�  2000.7
������  Content -- �������ݣ���","�ָ���
*/

function Append(Content)
{	var oNode,i,strTD,oNodes
	strTD = Content.split(this.Divide)
	//���һ��
	this.AddEvent(true,strTD)
	//window.alert(this.rs_main.RecordCount)
	this.rs_main.Movelast()
	for (i=0;i<this.Widths.length;i++)
		this.rs_main.Fields(i).value = strTD[i]
	
	this.rs_main.Update()
	
	if (this.rs_main.RecordCount < 1)
		return 
	var oNode
	eval("oNode = " + this.Name + "TableSpan.firstChild.firstChild.childNodes(" + this.rs_main.RecordCount + ").firstChild")
	eval("iTop = " + this.Name + "TableSpan.scrollTop" )	

	if (oNode.offsetTop < iTop)
		iTop = oNode.offsetTop
	else if (oNode.offsetTop + oNode.offsetHeight > iTop + this.Height)
		iTop = oNode.offsetTop + oNode.offsetHeight - this.Height
	
	eval(this.Name + "TableSpan.scrollTop = " + iTop)	
	
}



/* 
���ܣ�  �������ֶ�����������Զ����һ�С�
���ߣ�  ������
���ڣ�  2000.7
������  bAppend -- �Ƿ��ֶ�
		Cells   -- ��Ԫ��������
*/
function AddEvent(bAppend,Cells)
{
	if (bAppend !=true & this.AddDelete!=true )
		return null
	//����һ�м�¼
	var oNode, oRows, strCells, fields, iRows, oTemp
	//this.ClearV()
	//window.alert(this.rs_main.RecordCount)
	
	this.rs_main.Addnew()
	this.rs_main.Update()
	eval("iRows = " + this.Name + "TableSpan.firstChild.firstChild.childNodes.length ")
	//������Ԫ��
	oNode = document.createElement("TR");
	oNode.classid = this.Name + "TableRow"
	oNode.align = "left"
	oNode.bordercolor = "Silver"
	oNode.name = "" + iRows
	oNode.style.wordBreak = "break-all"
	//�������ڵ�Ԫ��Ԫ��
	for (field=0; field<this.Widths.length; field++)
	{	
		oTemp = document.createElement("TD")	
		oTemp.style.wordBreak = "break-all"
		if (this.HideColumn.length > 0)
			if (this.HideColumn[field] == false)	
				oTemp.style.display = "none"
		if (bAppend)
			oTemp.innerText = Cells[field]
		oTemp.className = field
		oNode.insertBefore(oTemp)
		oTemp = null
	}

	//�õ����
	eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild") //.childNodes
	
	
	oTemp = oRows.lastChild
	if (oTemp.id == this.Name + "BlankBack")
		//����������䱳���Ŀ��У���ɾ������
		oRows.removeChild(oTemp);

	//�ڶԵ�λ����Ӹ��С�
	//�ɱ���Զ����
	eval("oRows.insertBefore(oNode," + this.Name + "AddRow)")

	//ˢ�±���
	this.BackRefresh();
	//ģ����������������С�
	this.currentRow += 5                  
	this.MouseDown_Event(oNode.firstChild,true)

	this.ClearV()

}


/* 
���ܣ�  ���ڷ�ӳMouseDown�¼��������¼������ڼ�¼��Ŀ��ʱ��
        ����Ŀ��������ԭ������Ŀ������
���ߣ�  ������
���ڣ�  2000.7
������  Source -- ���ýڵ�
        Direct -- ���ָ���ɫ
����ֵ��������괦��������¼���ݡ��ֶ����ֶμ��ÿո�ֿ���

*/
function MouseDown_Event(Source,Direct,keyControl)
{
	var elerow,strRe
	//�¼�Դ
		if (Source == null)
		{
			elerow = window.event.srcElement;
		}
		else
			elerow = Source
						
		//��Ϊ�Ҽ����˳�
	//	if (window.event.button == 2)
	//		return "nothing";
		//����׷���¼��ĸ������Ƿ�Ϊ��¼�ж���
		if (elerow.parentElement.parentElement != null)
			elerow = elerow.parentElement;
		if (elerow.parentElement.classid == this.Name + "TableRow")
		{
			elerow = elerow.parentElement;
		}

    //���Ǽ�¼������Ӧ���¼�
	if (elerow.classid == this.Name + "TableRow")
	{
		//
		if (this.currentRow == elerow.name)
			return this.RowStr
		
		if (this.AddDelete == true && this.NotEmpty >= 0 && this.preElement != null && (Source == null || keyControl == true))
		{
			if (this.preElement.firstChild.name != "Virtul")
			{
				if (this.modi != null)
				{	if (this.modi[this.NotEmpty] == true)
					{
						var editnode
						//this.UseForm + this.Name + "Text" + i
						eval("editnode = " + this.Name + "Text" + this.NotEmpty + ".value")
						if (editnode == "")
							this.Delete()
					}
					else
					{
						if (this.preElement.childNodes(this.NotEmpty).innerText == "")
							this.Delete()
					}
					
				}
				else
				{
					if (this.preElement.childNodes(this.NotEmpty).innerText == "")
						this.Delete()
				}
			}
		}
		//�����ǵ�һ����Ӧ���¼����ָ���һ�ε���������ɫ��
		if (this.preElement != null && Direct != true)
		{	this.preElement.style.backgroundColor = this.preColor;
			this.preElement.style.color = "black";
		}
		//���汳��ɫ���Ա��ָ���
		this.preColor = elerow.style.backgroundColor;
		//ָ���µı���ɫ��
		elerow.style.backgroundColor = "midnightblue";
		elerow.style.color = "white"
		//���ظü�¼���ݡ�	


		this.RowStr = this.EditRow(elerow.name)

		this.currentRow = elerow.name
		//������У��Ա��ָ���
		this.preElement = elerow;
		return this.RowStr 
	}
	
	while (elerow.parentElement != null && elerow.name != this.Name + "TableAll")
	{
		elerow = elerow.parentElement
	}
	
	if (elerow.name == this.Name + "TableAll")
		return this.Name + "TableAll"
	
	return "nothing"
}


/*
����  ����һ��ʧȥ����ʱ������������ݣ������¼��
����  ��������
ʱ��  ��2000��8��
����  ��
����ֵ��
*/
function SaveRow()
{
	var i,j,strTemp,bTemp
    var iRow 
    //var strRow = ""
	iRow = this.PreRow
	if (this.rs_main.RecordCount > 0 )
	{	this.rs_main.MoveFirst();
		this.rs_main.Move(iRow)
	}
//�����������
	for (i=0; i<this.Widths.length; i++)
	{
		if (this.PreRow != -1)
		{	

			//ȡ����һ�εĺ��ߡ�
			//this.preElement.childNodes(i).style.borderTop=""
			//this.preElement.childNodes(i).style.borderBottom=""

			//��ǰһ���༭������ݱ�������������
			if (this.modi != null  & this.preElement.firstChild.name != "Virtul")
			    if (this.modi[i] & this.PreRow != -1)
				{				
					eval("strTemp = " + this.UseForm + this.Name + "Text" + i + ".value")
					if (this.FieldsType[i] == "num")
					{
						if (parseFloat(strTemp) * 1 != parseFloat(strTemp))
						{	strTemp = 0
							//this.preElement.childNodes(i).innerText = this.rs_main.Fields(i).value
							//this.preElement.childNodes(i).style.padding=2
							//�����¼������
							//this.rs_main.Fields(i).value = strTemp 
							//continue
						}
						else
							strTemp = parseFloat(strTemp)
					}
					this.preElement.childNodes(i).innerText = strTemp
					this.preElement.childNodes(i).style.padding=2
					//�����¼������
					this.rs_main.Fields(i).value = strTemp 
					//strRow += strTemp + this.Divide
					continue

				}
			
			//��ǰһ���ؼ����ݱ�����ҳ����
			if 	(i < this.seedControl.length)
				if (this.seedControl[i] != null  & this.preElement.firstChild.name != "Virtul" )
				{
						eval("strTemp = " + this.Name + "Cshow" + i + ".innerText")
						strTemp = this.preElement.childNodes(i).innerText
						strTemp = strTemp.replace(this.seedControl[i],"")
						strTemp = strTemp.replace("<SPAN ID=" + this.Name + "'Cshow'" + i + ">","")
						strTemp = strTemp.replace("</SPAN>", "")
						if (this.outerSelect == true && this.preElement.childNodes(i).childNodes(1).tagName == "SELECT")
						{	if (this.preElement.childNodes(i).childNodes(1).selectedIndex == -1)
								strTemp = ""
							else
								strTemp = this.preElement.childNodes(i).childNodes(1).options(this.preElement.childNodes(i).childNodes(1).selectedIndex).text
						}
						this.preElement.childNodes(i).innerText = strTemp
						this.preElement.childNodes(i).style.padding = 2
						//�����¼������
						this.rs_main.Fields(i).value = strTemp
						//strRow += strTemp + this.Divide
						continue
				}
			
			////���������ݱ���
			//if (this.preElement.childNodes(i).name != "Virtul")
			//{	strTemp = this.preElement.childNodes(i).innerText
				//this.rs_main.Fields(i).value = strTemp
			//	strRow += strTemp + this.Divide
			//}
		}
	}
	//�����¼������
	//this.rs_main.update()
	
	//�Ƿ���ʾ����
	if (this.AskSum)
	{
		for (i=0; i<this.Widths.length; i++)
		{	
			if (i != 0 & this.SumArr[i] != null)
			{
				this.UseSum(i)
				eval(this.Name + "Sums.childNodes(i).innerText = this.SumArr[i]")
			}
			
		}
	}	
	
	//return strRow 
}

/*
����  ��
����  ��������
ʱ��  ��2000��8��
����  ��
����ֵ��
*/
function EditRow(Rownum)
{
//outControl �����ⲿ������ʱ�ı�����ʾģʽ
	var i,j,strTemp,strCats,strStyle,iWidth,strRe,oRows,oParent,ilen
	strRe = ""	//�����ִ�
	eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild.childNodes(Rownum).childNodes")
	
/*	var dat
	var s1
	var s2
	dat = new Date()
	s1 = dat.getTime()
*/


	//��������ǵ�ǰ�У����д���
	if (this.currentRow != Rownum)
	{	
		if (this.PreRow != -1)
			this.SaveRow()
	
		//��������ɨ��		
		for (i=0; i<this.Widths.length; i++)
		{
			//���»���ʵ��
			//oRows(i).style.borderTop="1 solid #000000"
			//oRows(i).style.borderBottom="1 solid #000000"
			
			strRe += oRows(i).innerText
			if (i < this.Widths.length -1 && oRows(i).name != "Virtul")
				strRe += this.Divide
			
			//�����пɱ༭��Ϊ�����
			if (this.modi != null)
			{
				//�����µı༭��
				if (this.modi[i] && oRows(i).name != "Virtul")
				{	//�༭����
					if (this.seedControl[i] != null & oRows(i).name != "Virtul")
						iWidth = this.Widths[i] - 28
					else
						iWidth = this.Widths[i] - 1
					//�༭��ȱʡ����
					strTemp = "'" + oRows(i).innerText + "'"
					
					//�༭����
					strStyle = ' Style="WIDTH:' + iWidth + 'px; border-bottom:1 solid black;"'
					//�Ƿ�Ϊ��ֵ
					if (this.FieldsType[i] == "num")
						strStyle += " onKeyPress='DealNumberPress()'"
					//ȱʡֵ
					strStyle += " value=" + strTemp  
					//�Ƿ�ָ����󳤶�
					if (this.MaxSize[i] != null)
						strStyle += " maxlength=" + this.MaxSize[i] 
					//��ǰ��<TD></TD>�У�����༭��HTML�ĵ���
					oRows(i).innerHTML = "<input type=Text id=" + this.Name + "Text" + i + strStyle + " >"
					oRows(i).style.padding = 0
				}
			}		

			//���ɿؼ���	
			if 	(i < this.seedControl.length)
			if (this.seedControl[i] != null  && oRows(i).name != "Virtul")
			{
				//�����µ�ѡ���,��ǰ��<TD></TD>�У�����ѡ���HTML�ĵ���
				oRows(i).style.padding = 0
				if (this.outerSelect == true)
				{	oRows(i).innerHTML = ""
					oRows(i).innerHTML = "<SPAN ID=" + this.Name + "Cshow" + i + this.ControlValue[i] + ">" + oRows(i).innerHTML + "</SPAN>" + this.seedControl[i]    
				}
				else
				{	oRows(i).innerHTML = "<SPAN ID=" + this.Name + "Cshow" + i + this.ControlValue[i] + ">" + oRows(i).innerHTML + this.seedControl[i] + "</SPAN>"
					
				}
			}		

		}
		
	}
	
/*	dat = new Date()
	s2 = dat.getTime()
	dat = s2 - s1
	nncc.innerText =  " " + dat
*/	
	//����ǰ����Ϊǰһ�У��Ա���һ�αȽϡ�
	this.PreRow = Rownum;
	if (strRe.charAt(0) == "")
		return null
	return strRe
}

/*
����  ���õ���ǰ�е�����ֵ��
����  ��������
ʱ��  ��2000��8��
����  ��
����ֵ������ֵ��
*/
function GetRownum()
{
	return this.currentRow * 1;
}

/*
����  ��ˢ�±�����������
����  ��������
ʱ��  ��2000��8��
����  ��
����ֵ��
*/
function Refresh() {
	var strTemp
	//�Զ�ɾ��
	if (this.AddDelete == true && this.NotEmpty >= 0 && this.preElement != null )
	{
		if (this.preElement.firstChild.name != "Virtul")
		{
			if (this.modi != null)
			{	if (this.modi[this.NotEmpty] == true)
				{
					var editnode
					//this.UseForm + this.Name + "Text" + i
					eval("editnode = " + this.Name + "Text" + this.NotEmpty + ".value")
					if (editnode == "")
						this.Delete()
				}
				else
				{
					if (this.preElement.childNodes(this.NotEmpty).innerText == "")
						this.Delete()
				}
					
			}
			else
			{
				if (this.preElement.childNodes(this.NotEmpty).innerText == "")
					this.Delete()
			}
		}
	}
	
	//�����ǵ�һ����Ӧ���¼����ָ���һ�ε���������ɫ��
		
	this.SaveRow();
	if (this.preElement != null)
	{
		this.preElement.style.backgroundColor = this.preColor;
		this.preElement.style.color = "black";
		
		this.preElement = null
	}

	
	this.PreRow = -1
	this.currentRow = -1	
}


function EventRow(Name)
{
	var elerow
	elerow = window.event.srcElement;
				
	//��Ϊ�Ҽ����˳�
	//����׷���¼��ĸ������Ƿ�Ϊ��¼�ж���
	if (elerow.parentElement.parentElement != null)
		elerow = elerow.parentElement;
	
	if (elerow.parentElement.classid == this.Name + "TableRow")
	{
		elerow = elerow.parentElement;
	}
	
	
	if (Name != true)
		return elerow.name
	else
		return elerow

}

function ClearV()
{
	//��֪Ϊʲô���ռ�¼���޷�������ʱAddNew��ֻ�ó����²ߡ�--- 1
	//window.alert(this.rs_main.RecordCount)
	if (this.rs_main.RecordCount >=1)
	{
		this.rs_main.MoveFirst()
		if (this.rs_main(0) == "UndEfinedp")	
			this.rs_main.Delete()
	}
	
	if (this.rs_main.RecordCount == 0)
		this.IsEmpty = true
	else
		this.IsEmpty = false
	
	if (this.ShowCount == true)
		eval(this.Name + "Count.innerText = '��¼����" + this.rs_main.RecordCount + "'")
	
	//}

}

function DealNumberPress()
{
 var charCode=(navigator.appName =="NetScape")?event.Which: event.keyCode;

//ֻ�ܳ������ֺ�С���㡣
 if (charCode>31 && (charCode<48 ||charCode>57) && charCode != 46)
   	event.returnValue =false;

//����ͬʱ��������С���㡣
 if (event.srcElement.value.indexOf(".")>=0 && charCode == 46)
	event.returnValue =false;

}

function DealBack()
{
}

function DealKeyPress()
{
//	if (this.UnderKey == true)
//		return false
	
//	this.UnderKey = true
	
	//var oNode, oEventFunction
	//eval("oNode = " + this.Name + "TableBody")
//	oEventFunction = oNode.onkeydown 
//	oNode.onkeydown = null

	if (this.NoKey == true)
		return

	var Tag = event.srcElement.parentElement 
	var charCode=(navigator.appName =="NetScape")?event.Which: event.keyCode;
//	alert(charCode)
	var iRow = this.currentRow * 1
	var oNode, iTop, iCurr, oRows
	switch(charCode)
	{
		case 37:	//��
		
			break;
		
		case 38:	//��
			if (this.currentRow > 0)
			{	iRow -= 1
				this.HighLightRow(iRow)
				Tag.focus()
				eval(this.UpDown)
			}
			event.returnValue =false;
			break;
		
		case 39:	//��
		
			break;
		case 13:	//�س�
			eval(this.Dbclick)
		
			break;
			
		case 40:	//��
			eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild.childNodes")

			if (this.currentRow < oRows.length-1)
			{	iRow += 1
				this.HighLightRow(iRow)
				Tag.focus()
				eval(this.UpDown)
			}
			event.returnValue =false;
			break;
		
		case 33:	//PgUp
			eval("iCurr = " + this.Name + "TableSpan.scrollTop")	
			for(iRow; iRow>0; iRow--)
			{

				eval("oNode = " + this.Name + "TableSpan.firstChild.firstChild.childNodes(" + iRow + ").firstChild")
				iTop = oNode.offsetTop - 32
				if (iCurr - iTop >= this.Height) 
					break;
			}
			
			this.HighLightRow(iRow)
			eval(this.UpDown)
			event.returnValue =false;
			break;
		
		case 34:	//PgDn
			eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild.childNodes")
			eval("iCurr = " + this.Name + "TableSpan.scrollTop")	
			for(iRow; iRow<oRows.length-1; iRow++)
			{

				eval("oNode = " + this.Name + "TableSpan.firstChild.firstChild.childNodes(" + iRow + ").firstChild")
				iTop = oNode.offsetTop + oNode.offsetHeight + 32
				if (iTop - iCurr >= this.Height * 2) 
					break;
			}
			
			this.HighLightRow(iRow)
			eval(this.UpDown)
			event.returnValue =false;
			break;

		case 46:	//Delete
		
			break;

		case 32:	//Delete
			event.returnValue =false;
			break;
			
		default:
			break;
	
	}
	

//	oNode.onkeydown = oEventFunction
//	event.returnValue =false;
//	this.UnderKey = false
}


