/*
    fixed bug ITC-934-0087 on Feb 27 2009 by itc204011
*/
function clsTable(RecordSet,ObjectName)
{
    this.errSign="ITC||||||||||ITC";
    this.ShowSumCol=0;
	this.Height = 100;
	this.Widths = null;
	this.TableWidth = 100
	this.modi = new Array();
	this.rs_main = RecordSet;
	this.Title = true;
	this.AddDelete = false;
	this.RowStr	=""
	this.arrColor = new Array("#FFFFFF","#DCE8DC")
	this.LightBKColor = "#5D98E8"
	this.LightColor = "white"
	this.TableLineColor = "gray"
	this.UseSort = ""
	this.IsEmpty = false
	this.Divide = "\u001C"
	this.outerSelect = false
	this.Display = _Display;
	this.MouseDown_Event = _MouseDown_Event
	this.ManualMouseUp = _MouseDown_Event
	this.TDMouseDown = _TDMouseDown
	this.AddEvent = _AddEvent;
	this.GetRowNumber = _GetRownum
	this.Refresh = _Refresh
	this.ColumnStyle = _ColumnStyle
	this.TableStyle = _TableStyle
	this.HeadStyle = _HeadStyle
	this.Delete = _DelRow
	this.Append = _Append
	this.SetRow = _SetRow
	this.SetCell = _SetCell
	this.UseControl = _UseControl
	this.ControlReturn = _ControlReturn
	this.PageColumn = _PageColumn
	this.HideColumn = new Array()
	this.seedControl = new Array()
	this.ControlValue = new Array()
	this.FieldsType = new Array()
	this.MaxSize = new Array()
	this.EventRow = _EventRow
	this.ClearV = _ClearV
	this.Name = ObjectName;
    this.PreRow = -1;
	this.strColumn = ""
	this.strTable = 'Style="color:black;font-size:9pt"'
	this.strHead = "font-size:10pt"
	this.AskSum = false;
	this.SumArr = new Array(RecordSet.Fields.Count);
	this.SumDot = 2
	this.EditRow = _EditRow;
	this.UseSum = _UseSum;
	this.currentRow = -1;
    this.preElement = null;
    this.preColor = null;
	this.BackRefresh = _BackRefresh
	this.SaveRow = _SaveRow
	this.Sort = _Sort
	this.UseForm = ""
	this.Pages = 1
	this.PageArr = new Array()
	this.NewPage = _NewPage
	this.WhichPage = 0
	this.SortColumn = -1
	this.SortAsc = ""
	this.HighLightRow = _HighLightRow
	this.DealKeyPress = _DealKeyPress
	this.DealBack = _DealBack
	this.ShowCount = false
	this.FreeSelect = _FreeSelect
	this.StrFree = ""
	this.StrCookie = ""
	this.ShowFree = _ShowFree
	this.SetFree = _SetFree
	this.NotEmpty = -1
	this.Dbclick = "this.Dbclick = ''"
	this.SortLeft = 0
	this.RestoreScroll = _RestoreScroll
	this.ScrollVer = _ScrollVer
	this.ChangePage = _ChangePage
	this.FirstRow = 0
	this.LightRow = -1
	this.Filter = _FilterReset
	this.strFilter = ""
	this.ControlPath = "../../commoncontrol/tableEdit/"
	this.DownEventSrc = null
	this.NoKey = false
	this.BeforeUpDown = null
	this.UpDown = null
	this.BeforeChange = null
	this.OuterControl = -1
	this.OuterControl1 = -1
	this.OuterControl2 = -1
	this.OuterControl3 = -1
	this.OuterControl4 = -1
	this.SetZ = true
	this.ShowZ = true
	this.GetFirstEnableChild = _GetFirstEnableChild
	this.NoFocus = false
	this.BeforeHighLight=null
	this.BeforeSave = null
	this.BeforeNew = null
	this.AfterNew = null
	this.AfterChange = null
	this.AfterNewPage = null
	this.DefaultSort = -1
	this.DHContent = new Array()
	this.DHSpan = new Array()
	this.DHSignal = new Array()
	this.UseCombin = _UseCombin
	this.ChangeK = _ChangeK
	this.initTableData = _InitTableData;//this method was added by lzy
	
	if (document.charset == "gb2312")
	{
		this.JILUSHU = "��¼�� "
		this.HEJI = "�ϼ� "
		this.KONG = "��"
		this.NOSORT = "����ѱ��޸ģ����ȱ�����ݣ� "
	}
	else if (document.charset == "big5")
	{
		this.JILUSHU = "?O���?G "
		this.HEJI = "??heji  "
		this.KONG = "?@"
		this.NOSORT=""
	}
	else if (document.charset == "utf-8")
	{
		this.JILUSHU = "\u8bb0\u5f55\u6570\uff1a"
		this.HEJI = "\u5408\u8ba1\uff1a"
		this.KONG = "\u3000"
		this.NOSORT = "\u6570\u636e\u5df2\u88ab\u4fee\u6539\uff0c\u8bf7\u5148\u4fdd\u5b58\u6570\u636e\uff01"
	}
	else
	{
		this.JILUSHU = "Record Count:"
		this.HEJI = "SUM"
		this.KONG = "  "
		this.NOSORT = "Please save data before you do this!"
	}
	this.OverFlow = "auto"
	this.MaxLine = 99999
	this.PreLine = -1
	this.GetOrder = new Array()
	this.SetOrder = _SetOrder
	for (var i=0; i<this.rs_main.Fields.Count; i++)
	{
		this.GetOrder[i] = i
	}
	this.UseHTML = false
	this.ReversSort = false
	this.FastField = -2
	this.preFastElement = null
	this.DisplayFast = _DisplayFast
	this.DisplayM = _DisplayM
	this.BodyHTML = _BodyHTML
	this.HeadHTML = _HeadHTML
	this.UseSearch = ""
	this.SearchItem = null;
	this.SearchHTML = "<Input id=" + this.Name + "SearchValue style='width:100;height:20' value=''>"
	this.SearchFilter = _SearchFilter
	this.BeforeSearch = null
	this.strSearch = ""
	this.AdjustLine = 0
	this.CellColorArr = new Array()
	this.CellColor = _CellColor
	this.CellColorColumn = -1
	this.ChangeHead = null
	this.ToUnicode = _ToUnicode
	this.ReplaceChar = _ReplaceChar
	this.ToRe = _ToRe
	this.SetWidth = _SetWidth
	this.ScreenWidth = 800
	this.ScreenHeight = 600
	this.adjustRateW = 1.011
	this.adjustRateH = 1.082
	this.checkBoxColumn = -1
	this.checkBoxStyle = ""
	this.checkAll = _CheckAll
	this.checkItem = _CheckItem
	this.arrDelLog = new Array()
	this.arrModiLog = new Array()
	this.arrSortModi = null;
	this.DealLogWhenDel = _DealLogWhenDel
	this.GetAMDString = _RecordsetToString
	this.ClearAMD = _ClearAMD
	this.Clear = _Clear
	this.canKeyDelete = false
	this.MaxSortNumber = 0
	this.Calculator= new Array()
	this.CalculateColumn = _CalculateColumn
	this.FindSameRow = _FindSameRow
	this.BeforeSortModi = _BeforeSortModi
	this.AfterSortModi = _AfterSortModi
	this.CanNotChangRow = false;
	this.SearchValue = _SearchValue
	this.SetSelect = _SetSelect
	this.dataurl = ""
	this.CalculatorAll = _CalculatorAll
	this.scrollTop = 0;
    this.TDCErrorMsg=TDCErrorMsg;

}
function _CalculatorAll()
{
		if (this.rs_main.RecordCount > 0  && this.Calculator.length>0)
		{
			var tempPosition = this.rs_main.absolutePosition;
			this.rs_main.MoveFirst()
			while (!this.rs_main.EOF)
			{
					this.CalculateColumn();
    				this.rs_main.MoveNext()
			}
			if (tempPosition > 0)
				this.rs_main.absolutePosition = tempPosition;
        }
}
function _SetSelect()
{
     if (this.SearchItem != null)  {
            for (var iCol=0; iCol<this.SearchItem.length; iCol++)  {
                if (this.SearchItem[iCol] == "select")
                 {
                    var arrOption = new Array();
                    this.rs_main.moveFirst();
                    while (!this.rs_main.EOF && !this.IsEmpty) {
                        arrOption["<option value=\"" + this.ChangeK(this.rs_main.fields(iCol).value,iCol)+ "\">"+  this.rs_main.fields(iCol).value] = "";
                        this.rs_main.moveNext();
                    }
                    var arrJoin = new Array()
                    arrJoin[arrJoin.length] = "<Select id=" + this.Name + "SearchValue style='width:100;height:20;font-size:9pt' >";
                    for (var k in arrOption)  {
                        arrJoin[arrJoin.length] = k;
                    }
                    arrJoin[arrJoin.length] = "</Select>";
                    this.SearchItem[iCol] = arrJoin.join("");
                }
            }
    }
}
function _SearchValue()
{
    if (this.SearchItem != null)  {
        var iCol;
        iCol = event.srcElement.value*1;
        this.SetSelect()
        if (this.SearchItem[iCol] == "" || this.SearchItem[iCol] == null)  {
            eval( this.Name + "ValueContainer.innerHTML = \"<Input id=" + this.Name + "SearchValue style='width:100;height:20' >\"")
        }
        else {
           eval( this.Name + "ValueContainer.innerHTML = this.SearchItem[iCol]")
        }
    }
}
function _BeforeSortModi()
{
   	var srcRecordSet = this.rs_main
   	this.arrSortModi = new Array()
   	for (var k in this.arrModiLog)
   	{
   		if (k > srcRecordSet.recordCount || srcRecordSet.recordCount==0 || k==0)
   			continue;
   		srcRecordSet.AbsolutePosition = k;
   		this.arrSortModi[k] = srcRecordSet.GetString(2,1,",","");
	}
}
function _AfterSortModi()
{
    var oNode = document.getElementById(this.UseSort)
    if (oNode != null) {
        if (this.dataurl != "" &&  this.dataurl != oNode.DataURL  &&  this.SearchItem != null) {
            for (var iCol=0; iCol<this.SearchItem.length; iCol++ ) {
                if (this.SearchItem[iCol] != "")
                    this.SearchItem[iCol] = "select"
            }
        }
        this.dataurl =  oNode.DataURL;
    }
   	var srcRecordSet = this.rs_main
   	var tempArr = this.arrModiLog
	if (this.arrSortModi == null)
		return;
   	this.arrModiLog = new Array()
   	for (var k in this.arrSortModi)
   	{
		for (var i=1; i<=this.rs_main.recordCount; i++)
		{
			this.rs_main.absolutePosition = i;
			if (srcRecordSet.GetString(2,1,",","")==this.arrSortModi[k])
			{	this.arrModiLog[i] = tempArr[k];
				break;
			}
		}
	}
	this.arrSortModi = null
}
function _FindSameRow(columns,values,isCur)
{
    //start wwg add 2006/9/20    
    if(this.IsEmpty==true)
        return false;
    //end
    var ipos = this.rs_main.absolutePosition;
	var arrColumn = columns.split(this.Divide);
	var arrValue = values.split(this.Divide)
	var same=0;
	if ( !this.IsEmpty && ipos<0)
	    ipos=1;
	if (ipos>0)
	{	for (var i=1; i<=this.rs_main.recordCount; i++)
		{
			if (i-1 == this.LightRow && isCur)
				continue;
			same=0;
			this.rs_main.absolutePosition = i;
			for (var j=0; j<arrColumn.length; j++)
				if (this.rs_main.fields(arrColumn[j]*1).value == arrValue[j])
					same++;
			if (same==arrColumn.length)
				break;
		}
		this.rs_main.absolutePosition = ipos;
	}
	return same==arrColumn.length
}
function _CalculateColumn()
{
	for (var key in this.Calculator)
	{
	   var 	strCalculate = this.Calculator[key]
       for (var i=0; i<this.Widths.length; i++)
       {    if ((this.FieldsType[i]+"").substr(0,3) == "num")  {
                if (isNaN(parseFloat(this.rs_main.fields(i).value)))
                    this.rs_main.fields(i).value = 0;
            }
       }
//  	   strCalculate = strCalculate.replace(/\</g, this.Name + ".rs_main.fields(");
//   	   strCalculate = strCalculate.replace(/\>/g, ")");
//	   strCalculate = strCalculate.replace("=", "=Math.round((") + ")* Math.pow(10,this.SumDot)) / Math.pow(10,this.SumDot)"
	   eval(strCalculate);
	}
}
function _CheckAll()
{
	if (this.rs_main.recordCount <= 0)
		return 0;
	this.rs_main.moveFirst()
	if (event.srcElement.checked)
	{
		for (var i = 0; i< this.rs_main.recordCount; i++)
		{	this.rs_main.fields(this.checkBoxColumn).value = "checked"
			this.rs_main.moveNext()
		}
	}
	else
	{
		for (var i = 0; i< this.rs_main.recordCount; i++)
		{	this.rs_main.fields(this.checkBoxColumn).value = ""
			this.rs_main.moveNext()
		}
	}
	this.ChangePage()
}
function _CheckItem()
{
   	var rownum = event.srcElement.parentElement.parentElement.name;
   	this.rs_main.absolutePosition = rownum*1 + 1
	if (event.srcElement.checked)
	{
		this.rs_main.fields(this.checkBoxColumn).value = "checked"
	}
	else
	{
		this.rs_main.fields(this.checkBoxColumn).value = ""
	}
	if (this.LightRow > -1 && this.LightRow < this.rs_main.recordCount)
	this.rs_main.absolutePosition = this.LightRow*1 + 1
}
function _SearchFilter(isAll)
{
	var strF,strTrim,oNode
		var strTemp = ""
	if (isAll != null)
		strF = ""
	else
	{
		oNode = document.all(this.Name + "Search")
		if (oNode == null)
			return
		strF = oNode.options(oNode.selectedIndex).text
		oNode = document.all(this.Name + "SearchValue")
		if (oNode == null)
			return
		strTrim = this.ToRe(oNode.value)
		var iIndex = document.getElementById(this.Name + "Search").value *1
		if (strTrim == "")
			strF += "=\"" + strTrim +"\""
		else if (oNode.tagName == "SELECT" || (this.FieldsType[iIndex]+"").substr(0,3) == "num")
		    strF += "=\"" + strTrim+"\""
		else
            strF += "=\"*" + strTrim +"*\""
	}
	this.strSearch = strF
	var bReturn
	eval("bReturn = " + this.BeforeSearch)
	if (bReturn == false)
	{
		event.returnValue =false
		return false
	}
    if (this.strFilter == strF)
		return false
    if (this.UseSearch != null)
    {
       this.SetSelect()
       eval( "this.SearchHTML = " + this.Name + "ValueContainer.innerHTML");
       if (isAll != null)
       {     eval(this.Name + "SearchValue.value = ''")
             this.SearchHTML = "<Input id=" + this.Name + "SearchValue style='width:100;height:20' value=''>"
       }
    }
	this.Filter(strF)
}
function _CellColor(key,column,style)
{
	var strKey
	strKey = "A" + key + column
	this.CellColorArr[strKey] = style
}
function _SetOrder(strOrder)
{
	if (strOrder == null)
	{
		for (var i=0; i<this.rs_main.Fields.Count; i++)
			this.GetOrder[i] = i
		this.FastField = -2
		return true
	}
	var iFast = strOrder.substring(strOrder.length-1,strOrder.length) * 1
	var arrShow = (strOrder.substring(0,strOrder.length-2) + "").split(",")
	var iShow = 0
	var sChecked = "," + strOrder.substring(0,strOrder.length-2) + ","
	for (var i=0 ; i<this.rs_main.Fields.Count; i++)
	{	if (sChecked.indexOf(","+i+",") >= 0)
		{
			this.GetOrder[i] = arrShow[iShow] * 1
			iShow++
		}
		else
		{
			this.GetOrder[i] = i
		}
	}
	for (var i=0; i<arrShow.length-1; i++)
	{
		if (arrShow[i]*1 > arrShow[i+1]*1)
		{
			this.ReversSort = true
			this.FastField = iFast
			return false
		}
	}
	if (this.ReversSort == true)
	{
			this.ReversSort = false
			this.FastField = iFast
			return false
	}
	if (this.FastField != iFast)
	{
			this.FastField = iFast
			return false
	}
}
function _ChangeK(key,icol,isReturn)
{
   key = key + ""
   if (key == "undefined" || key == undefined)
   		key = "";

   if (icol != null && this.FieldsType[icol] == "numMoney" && key * 1 != 0)
   {	
		key = Math.round(key * Math.pow(10,this.SumDot)) / Math.pow(10,this.SumDot);
		var strKey = key + "";
		var arrKey = strKey.split(".");
		var strNewKey = "";
		var tempIntString = arrKey[0];
		var length = arrKey[0].length;
		
		while (length > 3)
		{	if (length == 4 && tempIntString.substr(0,1) == "-")
				break;
			strNewKey = "," + tempIntString.substr(length-3,3) + strNewKey
			length = length - 3;
		}
		
		strNewKey = tempIntString.substr(0,length) + strNewKey
		
		if (arrKey.length == 1)
			arrKey[1] = "00";
			
		arrKey[1] = arrKey[1] + "" + "00";
		
		strNewKey = strNewKey + "." + arrKey[1].substr(0,2);
		key = strNewKey;
		return key;
   		
   }
   

   if (	this.UseHTML == true || isReturn==true)
		return key;
    key = key.replace(/&/g,"&amp;")
	key = key.replace(/</g, "&lt;")
    key = key.replace(/>/g, "&gt;")
    key = key.replace(/  /g, " &nbsp;")
    key = key.replace(/"/g, "&quot;")
   

   
   return key;
}
function _UseCombin(Columns,CombinName)
{
	var i,j, arrCol
	if (Columns == null || CombinName == null)
		return false
	if (this.DHContent.length == 0)
	{
		for (i=0; i<this.Widths.length; i++)
		{
			this.DHContent[i] = ""
			this.DHSpan[i] = 1
			this.DHSignal[i] = 0
		}
	}
	arrCol = Columns.split(",")
	for (i=0;i<arrCol.length;i++)
	{
		if (this.DHSpan[arrCol[i]*1] != 1)
			return false
		if (this.DHSignal[arrCol[i]*1] == 1)
			return false
		if (arrCol[i]*1 >= this.Widths.length)
			return false
	}
	this.DHContent[arrCol[0]*1] = CombinName
	for (i=0;i<arrCol.length;i++)
	{
		this.DHSpan[arrCol[i]*1] = arrCol.length
		this.DHSignal[arrCol[i]*1] = 1
	}
}
function _HeadHTML(sWid)
{
	var iWidth = 0
	var sDisplay = ""
	var sUse = new Array()
	var	DivWidth = 0
	var iFast = ""
	var sNext = ""
	var sWidth = ""
	var sRowCol = ""
	var sSort
	var TABLE_HEAD_H = "<TABLE id='" + this.Name + "TableHead" + iFast + "' cellpadding=2  cellspacing=0 style='border-style:solid;border-width:1px;border-color:black' style='width:" + sWid + "'>"
	var TABLE_TAIL_H = "</TABLE>"
	var TABLE_TAIL = "</TABLE>"
	var HEAD_ROW_BEGIN = "<TR style='word-break:break-all;border-color:black' name=Head bgcolor=#C7D0D9 align=center>"
	var HEAD_ROW_END = "</TR>"
	var HEAD_BODY_BEGIN = "<TD " + "Style='border-right:1px solid black;" + this.strHead +  "' ><b>"
	var DHHEAD_BODY_BEGIN = "<TD " + "Style='border-right:1px solid black;border-bottom:1px solid black;" + this.strHead +  "' ><b>"
	var HEAD_BODY_END = "</TD>"
	if (this.DHSpan.length > 0)
		sRowCol = " rowspan=2 "
		sUse[sUse.length] = TABLE_HEAD_H + HEAD_ROW_BEGIN
		for (var field=0; field<this.Widths.length; field++)
		{
			iWidth = this.Widths[this.GetOrder[field]] - 5
			if (this.HideColumn.length > 0)
			{
				if (this.HideColumn[field] != true)
				{	sDisplay = " style='display:none'"
				}
				else
				{	sDisplay = ""
					DivWidth += this.Widths[this.GetOrder[field]]
				}
			}
			else
				DivWidth += this.Widths[this.GetOrder[field]]
			if (this.DHSignal.length > 0 )
			{
				if (this.DHSignal[field] == 0)
					sWidth = HEAD_BODY_BEGIN.replace("<TD","<TD " + sRowCol + "width=" + iWidth  + "px id=" + this.Name + "H" + iFast + iFast + iFast + iFast + "" + field + sDisplay)
				else
				{	sNext += HEAD_BODY_BEGIN.replace("<TD","<TD width=" + iWidth  + "px id=" + this.Name + "H" + iFast + "" + iFast + iFast + iFast + iFast + sDisplay)
					if (this.DHContent[field] != "" && this.DHContent[field] != null)
						sWidth = DHHEAD_BODY_BEGIN.replace("<TD","<TD colspan=" + this.DHSpan[field] + " id=" + this.Name + "1H" + iFast + iFast + iFast + iFast + "" + field + sDisplay) + this.DHContent[field] + HEAD_BODY_END
					else
						sWidth = ""
				}
			}
			else
				sWidth = HEAD_BODY_BEGIN.replace("<TD","<TD width=" + iWidth  + "px id=" + this.Name + "H" + iFast + iFast + iFast + iFast + "" + field + sDisplay)
			sUse[sUse.length] = sWidth
			if (this.UseSort == "")
			{
				if (this.DHSignal[field] == 1)
					sNext += this.rs_main.Fields(this.GetOrder[field]).Name
				else
				{
					if (this.checkBoxColumn == this.GetOrder[field])
						sUse[sUse.length] = "<INPUT TYPE=CHECKBOX NAME=" + this.Name + "cball id=" + this.Name + "cball onclick='" + this.Name + ".checkAll()'  " + this.checkBoxStyle + "></input>"
					else
						sUse[sUse.length] = this.rs_main.Fields(this.GetOrder[field]).Name
				}
			}
			else
			{
				if (this.GetOrder[field] == this.SortColumn)
				{
					if (this.SortAsc == "")
						sSort = " background:transparent url(" + this.ControlPath + "images/up.gif) no-repeat bottom right;"
					else
						sSort = " background:transparent url(" + this.ControlPath + "images/down.gif) no-repeat bottom right;"
				}
				if (this.DHSignal[field] == 1)
					sNext += "<div class='HeadSort' onclick='" + this.Name + ".Sort(" + this.GetOrder[field] + ")" + "' style='text-decoration:underline; CURSOR:hand;" + sSort + "'>" + this.rs_main.Fields(this.GetOrder[field]).Name + "</div>"
				else
				{
					if (this.checkBoxColumn == this.GetOrder[field])
						sUse[sUse.length] = "<INPUT TYPE=CHECKBOX NAME=" + this.Name + "cball id=" + this.Name + "cball onclick='" + this.Name + ".checkAll()'   " + this.checkBoxStyle + "></input>"
					else
						sUse[sUse.length] = "<div class='HeadSort' onclick='" + this.Name + ".Sort(" + this.GetOrder[field] + ")" + "' style='text-decoration:underline; CURSOR:hand;" + sSort + "'>" + this.rs_main.Fields(this.GetOrder[field]).Name + "</div>"
				}
				sSort = ""
			}
			if (this.DHSignal[field] == 1)
				sNext += HEAD_BODY_END
			else
				sUse[sUse.length] = HEAD_BODY_END
		}
		sUse[sUse.length] =  "<TH " + sRowCol + "id='aa11' width=13 style='" + this.strHead + "'></TH>"
		sUse[sUse.length] =  HEAD_ROW_END + HEAD_ROW_BEGIN + sNext + HEAD_ROW_END + TABLE_TAIL_H
		return sUse.join("")
}
function _BodyHTML(iRow,iFast,iLeft)
{
	var sumWidth = 1
	for (i=0; i<this.Widths.length; i++)
	{	if (this.HideColumn.length > 0)
		{	if (this.HideColumn[i] == true)
				sumWidth += (this.Widths[this.GetOrder[i]])
		}
		else
			sumWidth += (this.Widths[this.GetOrder[i]])
	}
	var TABLE_HEAD = "<TABLE id='" + this.Name + "TableBody" + iFast + "' cellpadding=2  cellspacing=1 bordercolor=Silver bgcolor=Gray " + this.strTable + " onKeyDown='" + this.Name + ".DealKeyPress()' style='width:" + sumWidth + "px;position:absolute;z-index:100;left:" + iLeft + "' onmousewheel='" + this.Name +".ScrollVer(event.wheelDelta )' >"
	var TABLE_TAIL = "</TABLE>"
	var ROW_BEGIN = "<TR classid='" + this.Name + "TableRow" + iFast + "' align=left bordercolor=Silver name=rownum >"
	var ROW_END = "</TR>"
	var CELL_BEGIN = "<TD class=" + this.Name + "FFIIEE>"
	var CELL_END = "</TD>"
	var sStyle = ""
	var sBackC = ""
	var sBody = new Array()
	var jj = 0
	var iWidth = 0
	var sDisplay = ""
	var sColor = ""
	if (this.rs_main.RecordCount > iRow )
	{
		this.rs_main.absolutePosition = iRow+1
	}
	sBody[0] = TABLE_HEAD
	for (var j = iRow; j<this.rs_main.RecordCount; j++)
	{
		if (this.rs_main.EOF)
			break
		if (this.rs_main.Fields(0).value == "-1.0249E5")
		{	this.rs_main.MoveNext()
			continue
		}
		if ((j-iRow) > this.Height / 20)
			break
		this.CalculateColumn();
		if (this.rs_main.Fields(this.GetOrder[this.Widths.length-1]).Name == "CC_COLOR" && this.rs_main.Fields(this.GetOrder[this.Widths.length-1]).value != "")
			sBackC = " Style='background-color:" + this.rs_main.Fields(this.GetOrder[this.Widths.length-1]).value + "; height:20px; color:black; word-break:break-all'"
		else
			sBackC = " Style='background-color:" + this.arrColor[jj % 2] + "; height:20px; color:black; word-break:break-all'"
		sBody[sBody.length] =  ROW_BEGIN.replace("rownum", ""+ j + sBackC)
		jj += 1
		for (field=0; field<this.Widths.length; field++)
		{
			if (this.CellColorColumn >= 0)
			{
				sColor = this.CellColorArr["A" + this.rs_main.Fields(this.GetOrder[this.CellColorColumn]).Value + field]
				if (sColor == null)
					sColor = ""
			}
			if (this.HideColumn.length > 0)
			{
				if (this.HideColumn[field] != true)
					sDisplay = " style='display:none" + sColor + "'"
				else
					sDisplay = " style='" + sColor + "'"
			}
			else
				sDisplay = " style='" + sColor + "'"
			sWidth = CELL_BEGIN.replace("FFIIEE","" + field + sDisplay)
			sBody[sBody.length] = sWidth
			if (this.checkBoxColumn == this.GetOrder[field])
			{
				sBody[sBody.length] = "<INPUT TYPE=CHECKBOX NAME=" + this.Name + "cb" + field + " id=" + this.Name + "cb" + field + " onclick='" + this.Name + ".checkItem()' " + this.rs_main.Fields(this.GetOrder[field]).Value + "  " + this.checkBoxStyle + "></input>"
			}
			else
			{
				if (this.SetZ == true || this.rs_main.Fields(this.GetOrder[field]).Value * 1 != 0   || (this.FieldsType[this.GetOrder[field]]+"").substr(0,3) != "num" )
				{
					if (this.ShowZ == true || this.rs_main.Fields(this.GetOrder[field]).Value != 0)
						sBody[sBody.length] =  this.ChangeK(this.rs_main.Fields(this.GetOrder[field]).Value,this.GetOrder[field])
				}
			}
			sBody[sBody.length] = CELL_END
		}
		sBody[sBody.length] =  ROW_END
		this.rs_main.MoveNext()
	}
	sBackC = " Style='background-color:" + this.arrColor[jj % 2] + "; height:20px; color:black; word-break:break-all'"
	sBackC =  ROW_BEGIN.replace("rownum", ""+ (jj*1+iRow*1) + sBackC)
	jj += 1
	sBody[sBody.length] =  sBackC.replace("TableRow'","TableRow' id=" + this.Name + "AddRow" + iFast + "")
	sDisplay = ""
	for (field=0; field<this.Widths.length; field++)
	{
		if (this.HideColumn.length > 0)
		{
			if (this.HideColumn[field] != true)
				sDisplay = " style='display:none'"
			else
				sDisplay = ""
		}
		iWidth = this.Widths[this.GetOrder[field]] - 5
		sWidth = CELL_BEGIN.replace("FFIIEE","" + field + sDisplay + " name='Virtul'  width=" + iWidth +  " id=" + this.Name + "Add" + iFast + "")
		sBody[sBody.length] =  sWidth
		sBody[sBody.length] = CELL_END
	}
	sDisplay = ""
	for(var bl = jj ; bl < this.Height / 20 + 1; bl++)
	{
		sBackC = " Style='background-color:" + this.arrColor[bl % 2] + "; height:20px; color:black; word-break:break-all'"
		sBody[sBody.length] =  ROW_BEGIN.replace("rownum", ""+ (bl*1 + iRow*1) + sBackC)
		for (field=0; field<this.Widths.length; field++)
		{
			if (this.HideColumn.length > 0)
			{
				if (this.HideColumn[field] != true)
					sDisplay = " style='display:none'"
				else
					sDisplay = ""
			}
			iWidth = this.Widths[this.GetOrder[field]] - 5
			sWidth = CELL_BEGIN.replace("FFIIEE","" + field + sDisplay + " name='Virtul'  width=" + iWidth)
			sBody[sBody.length] =  sWidth
			sBody[sBody.length] = CELL_END
		}
	}
	sBody[sBody.length] =  ROW_END
	sBody[sBody.length] =  TABLE_TAIL
	sWidth = sBody.join("")
	return sWidth
}
function _ChangePage()
{

	var sHide,jj,oNode,oNode2,bAdd,iLightRow, sHide,iPreLine
	jj = 0
	if (this.rs_main.RecordCount == 0)
		bAdd = true
	else
		bAdd = false
	iPreLine = this.PreLine
	iLightRow = this.LightRow
	this.Refresh(true)
	this.LightRow = iLightRow
	this.PreLine = iPreLine
	iLightRow = -1
	eval(this.Name + "TableSpan.scrollTop = 0")
	oNode = document.all(this.Name + "TableSpan")
	oNode.firstChild.outerHTML = this.BodyHTML(this.FirstRow,"",oNode.firstChild.style.left)
	oNode2 = document.all(this.Name + "TableSpan" + this.FastField + "K")
	if (oNode2 != null)
		oNode2.firstChild.outerHTML = this.BodyHTML(this.FirstRow,this.FastField,0)
	var oNodes
	eval("oNodes = " + this.Name + "TableSpan.firstChild.firstChild.childNodes")
	for (var i=0; i<oNodes.length; i++)
	{
		if (oNodes(i).name == this.LightRow+"" && oNodes(i).firstChild.id != this.Name + "Add")
		{
		this.ManualMouseUp(oNodes(i).firstChild,null,true)
		}
	}
}
function _ScrollVer(wheelHeight,scrolltop)
{
	var oNode,oTable
	eval("oNode = " + this.Name + "TableSpan2")
	eval("oTable = " + this.Name + "TableBody")
	eval(this.Name + "SpanHead.scrollLeft = oNode.scrollLeft")
	eval(this.Name + "TableBody.style.left = -oNode.scrollLeft")
	if (this.AskSum)
		eval(this.Name + "SpanTail.scrollLeft = oNode.scrollLeft")
	var iTop
	if (wheelHeight != null)
	{    oNode.scrollTop =  oNode.scrollTop - (wheelHeight/6)
         return
    }
    if (scrolltop != null) {
        oNode.scrollTop = this.scrollTop;
        return;
    }
	    this.scrollTop = iTop = oNode.scrollTop
	iTop = Math.floor(iTop / Math.floor(this.Height / 8))
	if (iTop >= this.rs_main.RecordCount)
		iTop = this.rs_main.RecordCount - 1
	if (iTop < 0)
		iTop = 0
	if (iTop != this.FirstRow)
	{
		if (this.preElement != null)
		{
			var bReturn
			eval("bReturn = " + this.BeforeSave)
			if (bReturn == false)
			{
				event.returnValue =false
				return false
			}
		}
		this.FirstRow = iTop
		this.NoFocus = true
		this.ChangePage()
		this.NoFocus = false
		eval(this.Name + "VirtualFocus.focus()")
	}
}
function SetCookieTable(sName, sValue)
{
  document.cookie = sName + "=" + escape(sValue) + ";expires=Mon, 31 Dec 9999 23:59:59 UTC;";
}
function GetCookieTable(sName)
{
  var aCookie = document.cookie.split("; ");
  for (var i=0; i < aCookie.length; i++)
  {
    var aCrumb = aCookie[i].split("=");
    if (sName == aCrumb[0])
    {
		if (aCrumb.length > 1)
			return unescape(aCrumb[1]);
		else
			return unescape("");
    }
  }
  return null;
}
function _ShowFree()
{
	var sFree = GetCookieTable(this.StrFree)
	if (sFree == null || sFree == "" || sFree == "null")
	{
		this.SetFree()
		return
	}
	var ArrFree = sFree.substring(0,sFree.length-2).split(",")
	this.NewPage(0,ArrFree)
}
function _SetFree()
{
	this.StrCookie = GetCookieTable(this.StrFree)
	var sFeatures="dialogHeight: " + 360 + "px;dialogWidth: " + 340 + "px;status:no;help:0;";
	var para = this;
	var oNode = null
	var sCookie=window.showModalDialog(this.ControlPath + "SelectShow.html", para, sFeatures)
	if (sCookie != null && sCookie != "null")
	{	SetCookieTable(this.StrFree,sCookie)
		if (sCookie != "")
		{
			var ArrFree = sCookie.substring(0,sCookie.length -2).split(",")
			this.NewPage(0,ArrFree,0)
			if (this.SetOrder(sCookie) == false)
			{null}
			eval("oNode = " + this.Name + "Disp.parentElement" )
			this.FastField = sCookie.substring(sCookie.length -1,sCookie.length) * 1
			if (this.FastField > 0)
				oNode.innerHTML = this.DisplayFast(this.FastField)
			else
				oNode.innerHTML = this.Display()
			eval(this.ChangeHead)
		}
		else
		{
				eval("oNode = " + this.Name + "Disp.parentElement" )
			if (this.FastField > 0)
			{		oNode.innerHTML = this.DisplayFast(this.FastField)
				return
			}
			if (this.PageArr.length > 1)
				this.NewPage(1)
			else
			{
				var ArrAll = new Array()
				for (var ki = 0; ki<this.Widths.length; ki++)
					ArrAll[ki] = ki
				this.NewPage(0,ArrAll)
				if (this.StrFree != "")
					eval(this.UseForm + this.Name + "0img.src = '" + this.ControlPath + "images/" + "0.gif'")
			}
		}
	}
}
function _FreeSelect(SaveName)
{
	this.StrFree = SaveName
}
function _HighLightRow(RowNum,DownShow)
{
	if (this.CanNotChangRow)
		return;
	this.ClearV()
	this.SetCell()
	if (this.rs_main.RecordCount == 0)
		return;
	if (this.preElement != null && DownShow == null)
	{
		var bReturn
		eval("bReturn = " + this.BeforeSave)
		if (bReturn == false)
			return false
	}
	var oNode, iHScroll,iRow, iTop
	iTop = 0
	eval("oNode = " + this.Name + "TableSpan.firstChild.firstChild.childNodes(0).firstChild")
	eval("oNode = " + this.Name + "Measure")
	if (oNode.offsetWidth <= this.TableWidth-18)
		iHScroll = 0
	else
		iHScroll = 1
	if (RowNum > this.FirstRow && (RowNum - this.FirstRow) < (this.Height / 20))
	{
		eval("oNode = " + this.Name + "TableSpan.firstChild.firstChild.childNodes(" + (RowNum - this.FirstRow) + ").firstChild")
		if (oNode.parentElement.offsetTop + oNode.parentElement.offsetHeight < this.Height-18*iHScroll)
			if (DownShow == "AddNew")
				this.ChangePage()
			else
				this.ManualMouseUp(oNode,null,true)
		else
		{
			iTop = oNode.parentElement.offsetTop + oNode.parentElement.offsetHeight - this.Height + 18 * iHScroll
			for (var i=0; i< this.Height/20; i++)
			{	eval("oNode = " + this.Name + "TableSpan.firstChild.firstChild.childNodes(i).firstChild")
				iTop -= oNode.parentElement.offsetHeight
				if (iTop <= 0)
				{	iRow = i+1
					break;
				}
			}
			this.FirstRow = this.FirstRow + iRow
			this.LightRow = RowNum
			this.ChangePage()
		}
	}
	else
	{
		if (DownShow != true)
		{
			this.FirstRow = RowNum
			this.LightRow = RowNum
			this.ChangePage()
		}
		else
		{
			iTop = 0
			for (var i=0; i< this.Height/20; i++)
			{	eval("oNode = " + this.Name + "TableSpan.firstChild.firstChild.childNodes(i).firstChild")
				iTop += oNode.offsetHeight
				if (iTop >= this.Height - 18 * iHScroll)
				{	iRow = i-1
					break;
				}
			}
			if (this.LightRow - this.FirstRow < iRow)
				this.LightRow = this.FirstRow + iRow
			else
			{	this.FirstRow = this.FirstRow + Math.floor((this.Height-20)/20)
				this.ChangePage()
				iTop = 0
				for (var i=0; i< this.Height/20; i++)
				{	eval("oNode = " + this.Name + "TableSpan.firstChild.firstChild.childNodes(i).firstChild")
					iTop += oNode.offsetHeight
					if (iTop > this.Height - 18 * iHScroll)
					{	iRow = i-1
						break;
					}
				}
				this.LightRow = this.FirstRow + iRow
			}
			if (this.LightRow > this.rs_main.RecordCount)
				this.LightRow = this.rs_main.RecordCount
			if (this.FirstRow > this.rs_main.RecordCount)
				this.FirstRow = this.rs_main.RecordCount-1
			if (this.FirstRow < 0)
				this.FirstRow = 0
			this.ChangePage()
		}
	}
	window.setTimeout('eval("' + this.Name + 'TableSpan2.scrollTop = ' + this.FirstRow * Math.floor(this.Height / 8) + '")',20)
}
function _PageColumn(Page0,Page1,Page2,Page3,Page4)
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
function _NewPage(PageNum,Free,SetHide)
{
	var temp, iScrillH
	if (PageNum != 0)
	{
		oNode = document.all(this.Name  +  "FastD")
		if (oNode != null)
			oNode.style.display = "none"
	}
	else
	{
		oNode = document.all(this.Name  +  "FastD")
		if (oNode != null)
			oNode.style.display = "block"
	}
	if (SetHide != 0)
	{
		if (PageNum == 0)
		{
			var temp = GetCookieTable(this.StrFree)
			if (temp != null && temp != "null" && temp != "")
				this.SetOrder(temp)
		}
		else
		{
				this.SetOrder()
		}
	}
	var headNode = document.all(this.Name + "SpanHead")
	if (headNode != null)
		headNode.innerHTML = this.HeadHTML(headNode.firstChild.style.width)
	if (Free != null)
		this.PageArr[PageNum] = Free
	temp = this.PageArr[PageNum]
	if (temp == null || temp =="null" || temp == "")
	{
		this.SetFree()
			return
	}
	if (this.OverFlow == "scroll")
	{
		iScrillH = 18
	}
	else
		iScrillH = 0
	for(var i=0; i<this.Widths.length; i++)
		this.HideColumn[i] = false
	for(var i=0; i<temp.length; i++)
		this.HideColumn[temp[i]] = true
	if (SetHide == true)
		return
	this.ClearV()
	if (this.preElement != null)
	{
		var oldPos
		eval("oldPos = " + this.Name + "TableSpan.scrollTop")
		oldPos = this.preElement.offsetTop - oldPos
	}
	var i,oRows,iTemp
	eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild.childNodes")
	for (i=0; i<oRows.length; i++)
	{
			for (var n=0; n<this.HideColumn.length; n++)
				if (this.HideColumn[n] != true)
					oRows(i).childNodes(n).style.display = "none"
				else
					oRows(i).childNodes(n).style.display = "block"
	}
	iTemp = 23
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
			iTemp += this.Widths[this.GetOrder[n]]
		}
	}
	if (this.HideColumn.length >= this.Widths.length)
	{
		iTemp -= 4
		if (this.TableWidth+2 >= (iTemp))
		{
			eval(this.Name + "TableAll.width = " + iTemp)
			eval(this.Name + "TableSpan.style.width = " + (iTemp-18))
			eval(this.Name + "TableSpan.style.height = " + (this.Height-iScrillH))
			eval(this.Name + "TableSpan2.style.width = " + iTemp)
		}
		else
		{
			eval(this.Name + "TableSpan.style.width = " + (this.TableWidth-18))
			iScrillH = 18
			eval(this.Name + "TableSpan.style.height = " + (this.Height-iScrillH))
			eval(this.Name + "TableSpan2.style.width = " + this.TableWidth)
		}
		eval(this.Name + "TableHead.style.width = " + iTemp)
		if (this.AskSum)
		eval(this.Name + "TableTail.style.width = " + iTemp)
		iTemp -= 18
		eval(this.Name + "TableBody.style.width = " + iTemp)
		eval(this.Name + "TableBody.width = " + iTemp)
	}
	eval(this.Name + "Measure.style.width = " + iTemp)
	eval(this.Name + "TableBody.style.left = 0")
	eval(this.Name + "TableSpan2.scrollLeft = 0")
	var oNode
	eval("oNode = " + this.Name + "TableBody.lastChild.lastChild")
	if (oNode.style.height == "20px")
		oNode.style.height = "21px"
	else
		oNode.style.height = "20px"
	this.WhichPage = PageNum
	if (this.preElement != null)
	{	var iTop
		iTop = this.preElement.offsetTop - oldPos
		eval(this.Name + "TableSpan.scrollTop = " + iTop)
	}
	this.ChangePage()
	if (this.StrFree != "")
		eval(this.UseForm + this.Name + "0img.src = '" + this.ControlPath + "images/" + "0.gif'")
	for (var j=1; j<this.Pages; j++)
		eval(this.UseForm + this.Name + j + "img.src = '" + this.ControlPath + "images/" + j +".gif'")
	eval(this.UseForm + this.Name + PageNum + "img.src = '" + this.ControlPath + "images/" + PageNum +"_.gif'")
	if (document.all(this.Name + "Search") != null)
	{
		var sSearch = "<Select id=" + this.Name + "Search onchange='" + this.Name + "SearchValue.value=\"\"'>"
		for (var ss=0; ss<this.Widths.length; ss++)
		{
			if ((this.UseSearch != "PageLine" && this.UseSearch != "Page") || this.HideColumn[ss] != false)
			{
					sSearch += "<Option>" + this.rs_main.Fields(ss).name
			}
		}
		sSearch += "</Select>"
		document.all(this.Name + "Search").outerHTML = sSearch
		document.all(this.Name + "SearchValue").value = ""
	}
	eval(this.AfterNewPage)
}
function _Sort(column)
{
	//if click on the column which has been indicated by setting property "DefaultSort", nothing will happen
	/*if (column == this.DefaultSort)
		return*/

	if (this.arrModiLog.length > 0)
	{	this.BeforeSortModi()
	}
	if (this.preElement != null)
	{
		var bReturn
		eval("bReturn = " + this.BeforeSave)
		if (bReturn == false)
			return false
	}
	var Ascend, Element
	Ascend = ""
	Element = window.event.srcElement
	if (this.rs_main.RecordCount >1)
	{
		if (Element.className == "HeadSort")
		{
			if (this.SortColumn == column)
			{
				if (this.SortAsc == "")
					this.SortAsc = "-"
				else
					this.SortAsc = ""
			}
			else
			{
				this.SortColumn = column
				this.SortAsc = ""
			}
		}
		eval("this.SortLeft = " + this.Name + "TableSpan2.scrollLeft" )
		this.Refresh(true)
		eval(this.UseSort + ".CaseSensitive = 'FALSE'")
		eval(this.UseSort + ".Sort = " + "'" + this.SortAsc + this.rs_main.Fields(column).Name + "'")
		var oNode,iLeft
		eval("oNode = " + this.Name + "TableSpan2")
		iLeft = oNode.scrollLeft
		this.Filter()
		this.FirstRow = 0
		this.LightRow = -1
		this.ChangePage()
	}
}
function _FilterReset(outerFilter)
{
	if (this.arrModiLog.length > 0)
	{	this.BeforeSortModi()
	}
	var sFilter
	sFilter = this.rs_main.Fields(0).Name + " <> -1.0249E5"
	if (outerFilter != null)
		this.strFilter =  outerFilter
	if (this.strFilter != "" && this.strFilter != null)
		sFilter = this.rs_main.Fields(0).Name + " <> -1.0249E5" + " & ("  + this.strFilter+ ")"
	var oFil
	eval("oFil = " + this.UseSort + ".object")
	oFil.Filter = sFilter
	eval(this.UseSort + ".reset()")
}
function _RestoreScroll()
{
	eval(this.Name + "TableSpan2.scrollLeft = " + "this.SortLeft")
}
function _ControlReturn(strVal,col)
{
	if (this.seedControl.length >= col)
		eval(this.Name + "Cshow" + col + ".innerText = strVal")
}
function _UseControl(Col,strContent,ShowChange,Position)
{
	strContent = strContent.replace(/\n/g, "")
	strContent = strContent.replace(/\r/g, "")
	if (Col<this.Widths.length)
    {
    	this.seedControl[Col] = strContent
    	if (ShowChange == false)
    		this.ControlValue[Col] = " style='display:none'"
    	else
    		this.ControlValue[Col] = " style='display:block'"
    }
}
function _UseSum(Column,isCal)
{
	if (Column <= this.Widths.length)
	{
		this.AskSum = true;
		this.SumArr[Column] = 0
		if (this.rs_main.RecordCount > 0 && isCal)
		{
			var tempPosition = this.rs_main.absolutePosition;
			this.rs_main.MoveFirst()
			while (!this.rs_main.EOF)
			{
				if (parseFloat(this.rs_main.Fields(this.GetOrder[Column]).value) != parseFloat(this.rs_main.Fields(this.GetOrder[Column]).value))
				{	this.rs_main.MoveNext()
					continue
				}
				if (this.rs_main.Fields(this.GetOrder[Column]).value != null)
					this.SumArr[Column] += parseFloat(this.rs_main.Fields(this.GetOrder[Column]).value)
				this.rs_main.MoveNext()
			}
			if (tempPosition > 0)
				this.rs_main.absolutePosition = tempPosition;
			this.SumArr[Column] = Math.round(this.SumArr[Column] * Math.pow(10,this.SumDot)) / Math.pow(10,this.SumDot)
		}
	}
}
function _ColumnStyle(ColNum,strStyle)
{
	this.strColumn += "   ." + this.Name + ColNum + "{" + strStyle + "}    "
}
function _TableStyle(strStyle)
{
	this.strTable = ' Style="' + strStyle + ' "'
}
function _HeadStyle(strStyle)
{
	this.strHead =  strStyle
}
function _DisplayFast(fastnum)
{
	var itop,sDis
	if (this.StrFree != "")
		itop = 8
	else
		itop = 2
	if (this.WhichPage >0)
		sDis = "none"
	else
		sDis = "block"
	if (fastnum > 0)
	{
		var str1, str2
		str1 = "<div id=" + this.Name  +  "FastD style='position:absolute;top:" + itop + ";left:2;display:" + sDis + "'>" + this.DisplayM(null,null,true,fastnum) + "</div>"
		if (this.WhichPage > 0)
		{
			this.SetOrder()
		}
		str2 = this.DisplayM();
		return  str2 + str1
	}
	else
	{
		if (this.WhichPage > 0)
		{
			this.SetOrder()
		}
		return this.DisplayM()
	}
}
function _SetWidth()
{
  var sScrW = window.screen.width;
  var sScrH = window.screen.height;
  var oldWidth = 0;
  var newWidth = 0;
  var oldTableWidth = this.TableWidth;
  if (this.ScreenWidth != sScrW && this.ScreenWidth > 0)
  {
  	var changeRate
  	changeRate = (sScrW / this.ScreenWidth)*this.adjustRateW;
  	for (var i=0 ; i<this.Widths.length ; i++)
  	{
		if (this.HideColumn.length > 0)
		{	if (this.HideColumn[i] == true)
		  		oldWidth += this.Widths[i]
		}
		else
		  	oldWidth += this.Widths[i]
  		this.Widths[i] =  Math.round(this.Widths[i]*changeRate);
		if (this.HideColumn.length > 0)
		{	if (this.HideColumn[i] == true)
		  		newWidth += this.Widths[i]
		}
		else
		  	newWidth += this.Widths[i]
  	}
	this.TableWidth =  Math.round(this.TableWidth*changeRate);
	if (oldTableWidth > oldWidth)
	   this.TableWidth = newWidth + (oldTableWidth - oldWidth)
	this.Height *= (sScrH / this.ScreenHeight)*this.adjustRateH
	this.ScreenWidth = sScrW
	this.ScreenHeight = sScrH
  }
}
// wwg add for TDCERR

function TDCErrorMsg()
{
    /*
    if (this.rs_main.RecordCount > 0 )
	{
		var rs=this.rs_main;
        rs.MoveFirst();
	    if (rs.Fields(0).value.search(this.errSign)){
            alert(rs.Fields(0).value.split(this.errSign)[1]);
            //rs.Delete();
            rs.Fields(0).value="";
        }
     }
     */
}
function _Display(column, subStr,noAdd,iFast)
{
    //wwg add for TDCERR
     TDCErrorMsg();

    //below caller was add lzy.Date:2008-11-18
    this.initTableData()

    var temp = GetCookieTable(this.StrFree)
	this.SetWidth();
	this.ClearV(1)
	if (this.StrFree != "")
	{
		this.modi = new Array();
		this.AskSum = false;
		this.seedControl = new Array()
		this.ControlValue = new Array()
		this.AddDelete = false;
	}
	if (temp != null && temp != "null" && temp != "")
	{
		this.SetOrder(temp)
		var iFast = temp.substring(temp.length-1,temp.length)*1
		return this.DisplayFast(iFast)
	}
	else
		return this.DisplayM(column, subStr,noAdd,iFast)
}


//below function was added by lzy, using for .net project.
var TDCData = null;
function _InitTableData()
{    
    TDCData = new TableData();
    TDCData.UseHeader = true;    
    TDCData.recordset=this.rs_main;
}

function _DisplayM(column, subStr,noAdd,iFast)
{
	this.ClearV(1)
	this.AfterSortModi();
	this.CalculatorAll();
	var temp, FastWidth
	FastWidth = this.TableWidth
	if (this.checkBoxColumn != -1)
		this.ColumnStyle(this.checkBoxColumn,"text-align:center;");
	if (this.PageArr.length > 1 || this.StrFree != "")
	{
		if (this.StrFree != "")
		{	temp = GetCookieTable(this.StrFree)
			if (temp != null && temp != "null" && temp != "")
			{	this.PageArr[0] = temp.substring(0,temp.length-2).split(",")
			}
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
		if (iFast != null)
		{
			if (temp != null && temp != "null" && temp != "")
				temp = this.PageArr[0]
			else
			{
				temp = new Array()
				for (var i=0; i<this.Widths.length; i++)
					temp[i] = true
			}
		}
		else
			temp = this.PageArr[this.WhichPage]
		if (temp != "" && temp != null)
		{
			for(var i=0; i<this.Widths.length; i++)
				this.HideColumn[i] = false
			for(var i=0; i<temp.length; i++)
				this.HideColumn[temp[i]] = true
		}
	}
	var sTemp, field, i,sWidth, sumWidth, iWidth, sHand, sHide, sSort, sStyle
    var sUse = new Array()
	var sBody = new Array()
	sumWidth = 23
	for (i=0; i<this.Widths.length; i++)
	{	if (this.HideColumn.length > 0)
		{	if (this.HideColumn[i] == true)
				sumWidth += (this.Widths[this.GetOrder[i]])
		}
		else
			sumWidth += (this.Widths[this.GetOrder[i]])
		if (this.GetOrder[i] == this.DefaultSort && this.rs_main.RecordCount > 0)
		{
			for(var q=1; q<=this.rs_main.RecordCount; q++)
			{	this.rs_main.absolutePosition = q;
				if (this.rs_main.Fields(this.GetOrder[i]).Value == "" )
					this.rs_main.Fields(this.GetOrder[i]).Value = this.MaxSortNumber = q;
				if (this.rs_main.Fields(this.GetOrder[i]).Value*1 > this.MaxSortNumber*1)
					this.MaxSortNumber = this.rs_main.Fields(this.GetOrder[i]).Value*1
			}
		}
	}
	if (iFast == null)
		iFast = ""
	else
	{
		var ihde = iFast
		FastWidth = 1
		this.FastField = iFast
		for (i=0; i<this.Widths.length; i++)
		{
			if (this.HideColumn.length > 0)
			{	if (this.HideColumn[this.GetOrder[i]] == true)
				{
					if (FastWidth + (this.Widths[this.GetOrder[i]]) >= this.TableWidth - 20)
						break
					FastWidth += (this.Widths[this.GetOrder[i]])
					ihde --
				}
			}
			else
			{
				if (FastWidth + (this.Widths[this.GetOrder[i]]) >= this.TableWidth - 20)
					break
				FastWidth += (this.Widths[this.GetOrder[i]])
				ihde --
			}
			if (ihde==0)
				break
		}
	}
	var TABLE_BEGIN = "<TABLE name='" + this.Name + "TableAll' Id=" + this.Name + "TableAll" + iFast + "  width=" + FastWidth + "px border=1 cellpadding=0 cellspacing=0 onmousedown='" + this.Name + ".TDMouseDown()' onmouseup='" + this.Name + ".MouseDown_Event()'><TR><TD>"
	var TABLE_END = "</TD></TR></TABLE>"
	sumWidth -= 4
	var TABLE_HEAD_H = "<SPAN id='" + this.Name + "SpanHead" + iFast + "' style='WIDTH:"+ FastWidth + ";OVERFLOW:hidden'><TABLE id='" + this.Name + "TableHead" + iFast + "' cellpadding=2  cellspacing=0 style='border-style:solid;border-width:1px;border-color:" + this.TableLineColor + "' style='width:" + sumWidth + "px'>"
	var TABLE_HEAD_T = "<br><SPAN id='" + this.Name + "SpanTail" + iFast + "' style='WIDTH:"+ FastWidth + ";OVERFLOW:hidden'><TABLE id='" + this.Name + "TableTail" + iFast + "' cellpadding=2  cellspacing=0 style='border-style:solid;border-width:1px;border-color:" + this.TableLineColor + "' style='width:" + sumWidth + "px'>"
	sumWidth -= 18
	var TABLE_HEAD = "<TABLE id='" + this.Name + "TableBody" + iFast + "' cellpadding=2  cellspacing=1 bordercolor=Silver bgcolor=Gray " + this.strTable + " onKeyDown='" + this.Name + ".DealKeyPress()' style='width:" + sumWidth + "px;position:absolute;z-index:100'   onmousewheel='" + this.Name +".ScrollVer(event.wheelDelta )' >"
	var TABLE_TAIL_H = "</TABLE></SPAN>"
	var TABLE_TAIL = "</TABLE>"
	var HEAD_ROW_BEGIN = "<TR style='word-break:break-all;border-color:" + this.TableLineColor + "' name=Head bgcolor=#D1CEC9 align=center>"
	var HEAD_ROW_END = "</TR>"
	var HEAD_BODY_BEGIN = "<TD " + "Style='border-right:1px solid " + this.TableLineColor + ";" + this.strHead +  "' ><b>"
	var DHHEAD_BODY_BEGIN = "<TD " + "Style='border-right:1px solid " + this.TableLineColor + ";border-bottom:1px solid " + this.TableLineColor + ";" + this.strHead +  "' ><b>"
	var HEAD_BODY_END = "</TD>"
	var TAIL_BODY_BEGIN = "<TD " + "Style='border-right:1px solid " + this.TableLineColor + ";" + this.strHead +  "' >"
	var TAIL_BODY_END = "</TD>"
	var ROW_BEGIN = "<TR classid='" + this.Name + "TableRow" + iFast + "' align=left bordercolor=Silver name=rownum >"
	var ROW_END = "</TR>"
	var CELL_BEGIN = "<TD class=" + this.Name + "FFIIEE>"
	var CELL_END = "</TD>"
	var SEARCH_BUTTON = '<IMG  src="' + this.ControlPath + 'images/search.gif">'
	var ALL_BUTTON = '<IMG  src="' + this.ControlPath + 'images/all.gif">'
	var PAGE_BUTTON = '<IMG ID=' + this.Name + '#%&img' + iFast + ' src="' + this.ControlPath + 'images/#%&.gif" onmousedown="' + this.Name + '.NewPage(#%&)' + '" style="CURSOR: hand">'
	var FREE_BUTTON = '<IMG ID=' + this.Name + '0img' + iFast + ' src="' + this.ControlPath + 'images/0.gif" onmousedown="' + this.Name + '.ShowFree()' + '" style="CURSOR: hand">'
	if (this.WhichPage == 0)
		FREE_BUTTON = FREE_BUTTON.replace("0.gif","0_.gif")
	var SET_BUTTON = '<IMG ID=' + this.Name + 'setbutton' + iFast + ' src="' + this.ControlPath + 'images/SetD.gif" onmousedown="' + this.Name + '.SetFree()' + '" style="CURSOR: hand">'
	if (iFast != "")
		TABLE_BEGIN = "<TABLE name='" + this.Name + "TableAll' Id=" + this.Name + "TableAll" + iFast + "  width=" + FastWidth + "px border=0 cellpadding=0 cellspacing=0 onmousedown='" + this.Name + ".TDMouseDown()' onmouseup='" + this.Name + ".MouseDown_Event()'><TR><TD>"
	sUse[0] = TABLE_BEGIN
		var sDisplay = ""
		var sRowCol = ""
		var sNext = ""
		DivWidth = 0
	if (this.Title)
	{
		sSort = ""
		sUse[sUse.length] = TABLE_HEAD_H + HEAD_ROW_BEGIN
		if (this.DHSpan.length > 0)
			sRowCol = " rowspan=2 "
		for (field=0; field<this.Widths.length; field++)
		{
			iWidth = this.Widths[this.GetOrder[field]] - 5
			if (this.HideColumn.length > 0)
			{
				if (this.HideColumn[field] != true)
				{	sDisplay = " style='display:none'"
				}
				else
				{	sDisplay = ""
					DivWidth += this.Widths[this.GetOrder[field]]
				}
			}
			else
				DivWidth += this.Widths[this.GetOrder[field]]
			if (this.DHSignal.length > 0 )
			{
				if (this.DHSignal[field] == 0)
					sWidth = HEAD_BODY_BEGIN.replace("<TD","<TD " + sRowCol + "width=" + iWidth  + "px id=" + this.Name + "H" + iFast + iFast + iFast + iFast + "" + field + sDisplay)
				else
				{	sNext += HEAD_BODY_BEGIN.replace("<TD","<TD width=" + iWidth  + "px id=" + this.Name + "H" + iFast + "" + iFast + iFast + iFast + iFast + sDisplay)
					if (this.DHContent[field] != "" && this.DHContent[field] != null)
						sWidth = DHHEAD_BODY_BEGIN.replace("<TD","<TD colspan=" + this.DHSpan[field] + " id=" + this.Name + "1H" + iFast + iFast + iFast + iFast + "" + field + sDisplay) + this.DHContent[field] + HEAD_BODY_END
					else
						sWidth = ""
				}
			}
			else
				sWidth = HEAD_BODY_BEGIN.replace("<TD","<TD width=" + iWidth  + "px id=" + this.Name + "H" + iFast + iFast + iFast + iFast + "" + field + sDisplay)
			sUse[sUse.length] = sWidth
			if (this.UseSort == "")
			{
				if (this.DHSignal[field] == 1)
					sNext += this.rs_main.Fields(this.GetOrder[field]).Name
				else
				{
					if (this.checkBoxColumn == this.GetOrder[field])
						sUse[sUse.length] = "<INPUT TYPE=CHECKBOX NAME=" + this.Name + "cball id=" + this.Name + "cball onclick='" + this.Name + ".checkAll()'   " + this.checkBoxStyle + "></input>"
					else
						sUse[sUse.length] = this.rs_main.Fields(this.GetOrder[field]).Name
				}
			}
			else
			{
				if (this.GetOrder[field] == this.SortColumn)
				{
					if (this.SortAsc == "")
						sSort = " background:transparent url(" + this.ControlPath + "images/up.gif) no-repeat bottom right;"
					else
						sSort = " background:transparent url(" + this.ControlPath + "images/down.gif) no-repeat bottom right;"
				}
				if (this.DHSignal[field] == 1)
					sNext += "<div class='HeadSort' onclick='" + this.Name + ".Sort(" + this.GetOrder[field] + ")" + "' style='text-decoration:underline; CURSOR:hand;" + sSort + "'>" + this.rs_main.Fields(this.GetOrder[field]).Name + "</div>"
				else
				{
					if (this.checkBoxColumn == this.GetOrder[field])
						sUse[sUse.length] = "<INPUT TYPE=CHECKBOX NAME=" + this.Name + "cball id=" + this.Name + "cball onclick='" + this.Name + ".checkAll()'   " + this.checkBoxStyle + "></input>"
					else
						sUse[sUse.length] = "<div class='HeadSort' onclick='" + this.Name + ".Sort(" + this.GetOrder[field] + ")" + "' style='text-decoration:underline; CURSOR:hand;" + sSort + "'>" + this.rs_main.Fields(this.GetOrder[field]).Name + "</div>"
				}
				sSort = ""
			}
			if (this.DHSignal[field] == 1)
				sNext += HEAD_BODY_END
			else
				sUse[sUse.length] = HEAD_BODY_END
		}
		sUse[sUse.length] =  "<TH " + sRowCol + "id='aa11' width=13 style='" + this.strHead + "'></TH>"
		sUse[sUse.length] =  HEAD_ROW_END + HEAD_ROW_BEGIN + sNext + HEAD_ROW_END + TABLE_TAIL_H
	}
	if (this.rs_main.RecordCount > 0 )
	{
		this.rs_main.MoveFirst()
	}
	sBody[0] = TABLE_HEAD
	sStyle = ""
	var sBackC = ""
	var jj = 0
	var j = 0
	var sColor = ""
	for (j = 0; j<this.rs_main.RecordCount; j++)
	{
		if (this.rs_main.EOF)
			break
		if (this.rs_main.Fields(0).value == "-1.0249E5")
		{	this.rs_main.MoveNext()
			continue
		}
		if (j > this.Height / 20)
			break
		this.CalculateColumn();
		if (this.rs_main.Fields(this.GetOrder[this.Widths.length-1]).Name == "CC_COLOR" && this.rs_main.Fields(this.GetOrder[this.Widths.length-1]).value != "")
			sBackC = " Style='background-color:" + this.rs_main.Fields(this.GetOrder[this.Widths.length-1]).value + "; height:20px; color:black; word-break:break-all'"
		else
			sBackC = " Style='background-color:" + this.arrColor[jj % 2] + "; height:20px; color:black; word-break:break-all'"
		sBody[sBody.length] =  ROW_BEGIN.replace("rownum", ""+ jj + sBackC)
		jj += 1
		for (field=0; field<this.Widths.length; field++)
		{
			if (this.CellColorColumn >= 0)
			{
				sColor = this.CellColorArr["A" + this.rs_main.Fields(this.GetOrder[this.CellColorColumn]).Value + field]
				if (sColor == null)
					sColor = ""
			}
			if (this.HideColumn.length > 0)
			{
				if (this.HideColumn[field] != true)
					sDisplay = " style='display:none" + sColor + "'"
				else
					sDisplay = " style='" + sColor + "'"
			}
			else
				sDisplay = " style='" + sColor + "'"
			sWidth = CELL_BEGIN.replace("FFIIEE","" + field + sDisplay)
			sBody[sBody.length] = sWidth
			if (this.checkBoxColumn == this.GetOrder[field])
			{
				sBody[sBody.length] = "<INPUT TYPE=CHECKBOX NAME=" + this.Name + "cb" + field + " id=" + this.Name + "cb" + field + " onclick='" + this.Name + ".checkItem()' " + this.rs_main.Fields(this.GetOrder[field]).Value + "  " + this.checkBoxStyle + "></input>"
			}
			else
			{
				if (this.SetZ == true || this.rs_main.Fields(this.GetOrder[field]).Value * 1 != 0  || (this.FieldsType[this.GetOrder[field]]+"").substr(0,3) != "num")
				{
					if (this.ShowZ == true || this.rs_main.Fields(this.GetOrder[field]).Value != 0)
						sBody[sBody.length] =  this.ChangeK(this.rs_main.Fields(this.GetOrder[field]).Value,this.GetOrder[field])
				}
			}
			sBody[sBody.length] = CELL_END
		}
		sBody[sBody.length] =  ROW_END
		this.rs_main.MoveNext()
	}
	sBackC = " Style='background-color:" + this.arrColor[jj % 2] + "; height:20px; color:black; word-break:break-all'"
	sBackC =  ROW_BEGIN.replace("rownum", ""+ jj + sBackC)
	jj += 1
	sBody[sBody.length] =  sBackC.replace("TableRow'","TableRow' id=" + this.Name + "AddRow" + iFast + "")
	sDisplay = ""
	for (field=0; field<this.Widths.length; field++)
	{
		if (this.HideColumn.length > 0)
		{
			if (this.HideColumn[field] != true)
				sDisplay = " style='display:none'"
			else
				sDisplay = ""
		}
		iWidth = this.Widths[this.GetOrder[field]] - 5
		sStyle += (this.FieldsType[this.GetOrder[field]]+"").substr(0,3) == "num"?"   ." + this.Name + field + "{text-align:right}    ":""
		sWidth = CELL_BEGIN.replace("FFIIEE","" + field + sDisplay + " name='Virtul'  width=" + iWidth +  " id=" + this.Name + "Add" + iFast + "")
		sBody[sBody.length] =  sWidth
		sBody[sBody.length] = CELL_END
	}
	sDisplay = ""
	for(var bl = jj ; bl < this.Height / 20 + 1; bl++)
	{
		sBackC = " Style='background-color:" + this.arrColor[bl % 2] + "; height:20px; color:black; word-break:break-all'"
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
			iWidth = this.Widths[this.GetOrder[field]] - 5
			sWidth = CELL_BEGIN.replace("FFIIEE","" + field + sDisplay + " name='Virtul'  width=" + iWidth)
			sBody[sBody.length] =  sWidth
			sBody[sBody.length] = CELL_END
		}
	}
	sBody[sBody.length] =  ROW_END
	sBody[sBody.length] =  TABLE_TAIL
	sWidth = sBody.join("")
	var sumWidth2 = sumWidth
	if (sumWidth+18 >= FastWidth)
		sumWidth = FastWidth -18
	if (this.Height == 0)
		sUse[sUse.length] = sWidth
	else
	{
		var ihigh
		ihigh = this.Height
		if (this.OverFlow == "scroll")
			ihigh = ihigh - 18
		else
		{
			if ((this.TableWidth-15) < (sumWidth2))
				ihigh = ihigh - 18
		}
		sUse[sUse.length] = "<br><Span id='Ret_3' style='position:relative; left:0; top:0'>"
		if (iFast == "")
		{
			sUse[sUse.length] = "<SPAN id='" + this.Name + "TableSpan2" + iFast + "' style ='left:0; top:20; height:" + this.Height + ";width:" + (sumWidth + 18) + "; OVERFLOW:" + this.OverFlow + "; z-index:1500' onscroll='" + this.Name + ".ScrollVer()' ><DIV id=" + this.Name + "Measure STYLE='WIDTH:" + (DivWidth+1) + ";HEIGHT:" + (this.rs_main.RecordCount * Math.floor(this.Height / 8) + this.Height+1) + ";POSITION:absolute;z-index:2000'></DIV></SPAN>"
			sUse[sUse.length] = "<DIV id='" + this.Name + "TableSpan' style ='left:0; height:" + ihigh + ";width:" + (sumWidth ) + "; position:absolute; OVERFLOW:hidden;z-index:100'>" + sWidth + "</DIV>"
			sUse[sUse.length] = "<INPUT TYPE=TEXT id='" + this.Name + "VirtualFocus" + iFast + "' style ='left:" + (3+sumWidth) + "; top:" + (this.Height) + ";height:1;width:0; position:absolute; OVERFLOW:hidden;z-index:107'" + " onKeyDown='" + this.Name + ".DealKeyPress()'  >"
		}
		else
		{
			sUse[sUse.length] = "<SPAN id='" + this.Name + "TableSpan2" + iFast + "' style ='left:0; top:20; height:" + ihigh + ";width:" + (FastWidth) + "; OVERFLOW:hidden; z-index:1500' onscroll='" + this.Name + ".ScrollVer()'><DIV id=" + this.Name + "Measure" + iFast + " STYLE='WIDTH:" + (DivWidth+1) + ";HEIGHT:" + (this.rs_main.RecordCount * Math.floor(this.Height / 8) + this.Height+1) + ";POSITION:absolute;z-index:2000'></DIV></SPAN>"
			sUse[sUse.length] = "<DIV id='" + this.Name + "TableSpan" + iFast + "K' style ='left:0; height:" + ihigh + ";width:" + (FastWidth ) + "; position:absolute; OVERFLOW:hidden;z-index:100'>" + sWidth + "</DIV>"
		}
		sUse[sUse.length] = "</SPAN>"
	}
	if (this.AskSum && iFast == "")
	{
		sUse[sUse.length] = TABLE_HEAD_T + HEAD_ROW_BEGIN.replace("<TR ","<TR id=" + this.Name + "Sums" + iFast + " ")
		for (field=0; field<this.Widths.length; field++)
		{
				if (field != 0 & this.SumArr[field] != null)
				{
					this.UseSum(field,true)
			    }
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
						sDisplay = " style='display:none;font-size:8pt;font-weight:400; text-align:right'"
					else
						sDisplay = " style='font-size:8pt;font-weight:400; text-align:right'"
				}
			}
			iWidth = this.Widths[this.GetOrder[field]] - 5
			sWidth = TAIL_BODY_BEGIN.replace("<TD","<TD width=" + iWidth  + "px id=" + this.Name + "T" + field  + iFast +  sDisplay)
			sUse[sUse.length] = sWidth
			if (field==this.ShowSumCol)
				sUse[sUse.length] = this.HEJI
			else
				if (this.SumArr[field] != null)
					sUse[sUse.length] = this.ChangeK(this.SumArr[field],field)
				else{
				    if(this.ShowSumCol==field)
					    sUse[sUse.length] = this.HEJI
					else
					    sUse[sUse.length] = this.KONG
				}
			sUse[sUse.length] = TAIL_BODY_END
		}
		sUse[sUse.length] = TAIL_BODY_BEGIN.replace("<TD","<TD width=12px ")
		sUse[sUse.length] = HEAD_ROW_END + TABLE_TAIL_H
	}
	sUse[sUse.length] = TABLE_END
	sTemp = sUse.join("")
	if (this.Pages > 1 || this.ShowCount == true ||this.StrFree != "" || this.UseSearch != "")
	{
		sHide = ""
		if (this.Pages>1 && iFast <=0)
			for(var n=1; n<this.Pages; n++)
			{
				sHide += PAGE_BUTTON.replace("#%&","" + n)
				if (n == this.WhichPage)
					sHide = sHide.replace("#%&","" + n + "_")
				else
					sHide = sHide.replace("#%&","" + n)
				sHide = sHide.replace("#%&","" + n)
			}
		var sCount = ""
		if (this.ShowCount == true)
			sCount = this.JILUSHU + this.rs_main.RecordCount
		var sFree = ""
		var iFH = 28
		if (this.StrFree != "")
		{
			sFree = SET_BUTTON
			sHide = FREE_BUTTON + sHide
			iFH = 22 + this.AdjustLine
		}
		var sSearch = ""
		if (this.UseSearch != "")
		{
			var arrFilter = new Array()
			arrFilter[0] = ""
			arrFilter[1] = ""
			if (this.strFilter != "")
			{	arrFilter = this.strFilter.split("=")
			}
			sSearch += "<Select id=" + this.Name + "Search onchange='" + this.Name + ".SearchValue()' style='font-size:9pt'>"
			sSearch += "<Option value=" + (this.Widths.length+1) + " selected>"
			for (var ss=0; ss<this.Widths.length; ss++)
			{
				if (((this.UseSearch != "PageLine" && this.UseSearch != "Page" ) || this.HideColumn[ss] != false ) && this.checkBoxColumn !=ss)
				{
					if (arrFilter[0] == this.rs_main.Fields(ss).name)
						sSearch += "<Option selected value=" + ss + ">" + this.rs_main.Fields(ss).name
					else
						sSearch += "<Option value=" + ss + ">" + this.rs_main.Fields(ss).name
				}
			}
			sSearch += "</Select>"
			arrFilter[1] = arrFilter[1].replace("*","")
			arrFilter[1] = arrFilter[1].replace("*","")
			sSearch += "<span id=" + this.Name + "ValueContainer>" + this.SearchHTML + "</span>"
			sSearch += "<Button style='height:20;width:20' onclick='" + this.Name + ".SearchFilter()' >"  + SEARCH_BUTTON+ "</Button>"
			sSearch += "<Button style='height:20;width:20' onclick='" + this.Name + ".SearchFilter(1)'>" + ALL_BUTTON + "</Button>"
		}
		if (this.UseSearch == "AllLine" || this.UseSearch == "PageLine")
		{
			if (iFast == "")
				sTemp = "<TABLE id='" + this.Name + "Disp" + iFast + "'height=22 border=1 width=" + FastWidth + "><TR><TD align=left id=" + this.Name + "Count" + iFast + " style='font-family:����;font-size:9pt;filter:none' width=30%>" + sCount + "</TD><TD align=middle>" + sSearch + "</TD><TD align=middle>" + sFree + "</TD><TD align=right>" + sHide + "</TD></TR></TABLE>" + sTemp
			else
				sTemp = "<Div style='POSITION: absolute; TOP: " + iFH + "px;'>" + sTemp + "</Div>"
		}
		else
		{
			if (iFast == "")
				sTemp = "<TABLE id='" + this.Name + "Disp" + iFast + "'height=22 width=" + FastWidth + "><TR><TD align=left id=" + this.Name + "Count" + iFast + " style='font-family:����;font-size:9pt;filter:none' width=30%>" + sCount + "</TD><TD align=middle>" + sSearch + "</TD><TD align=middle>" + sFree + "</TD><TD align=right>" + sHide + "</TD></TR></TABLE>" + sTemp
			else
				sTemp = "<Div style='POSITION: absolute; TOP: " + iFH + "px;'>" + sTemp + "</Div>"
		}
	}
	sTemp = sTemp + "   " + "<Style> <!--" + this.strColumn + sStyle + "--> </style>"
	if (this.rs_main.RecordCount < 1 && noAdd != true)
	{
		this.rs_main.AddNew()
		if (this.rs_main.RecordCount > 0)
			this.rs_main(0) = "-1.0249E5"
		this.IsEmpty = true
	}
	else
		this.IsEmpty = false
	this.preElement = null
	this.preFastElement = null
	this.PreRow = -1
	this.currentRow = -1;
	this.PreLine = -1
	this.FirstRow = 0
	this.LightRow = -1
	this.RowStr = ""
	if (iFast != "")
	{
		temp = this.PageArr[this.WhichPage]
		if (temp != "" && temp != null)
		{
			for(var i=0; i<this.Widths.length; i++)
				this.HideColumn[i] = false
			for(var i=0; i<temp.length; i++)
				this.HideColumn[temp[i]] = true
		}
	}
	return sTemp
}
function _BackRefresh()
{
	var i,j,oRows, oNode, iTemp
	this.ClearV()
	eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild.childNodes")
	j = oRows.length
	i=0
	if (this.Height / 20 + 1 > j)
		bTemp = true
	else
		bTemp = false
	eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild")
	while (bTemp)
	{
		oNode = document.createElement("TR");
		oNode.classid= this.Name + "TableRow"
			oNode.id = this.Name + "BlankBack"
		for (field=0; field<this.Widths.length; field++)
		{
			oTemp = document.createElement("TD")
			oTemp.innerText = this.KONG
			oTemp.name = "Virtul"
			if (this.HideColumn.length > 0)
				if (this.HideColumn[field] == false)
					oTemp.style.display = "none"
			oTemp.className = field
			oTemp.width = this.Widths[this.GetOrder[field]] - 5
			oNode.insertBefore(oTemp)
		}
		oRows.insertBefore(oNode)
		eval("j = " + this.Name + "TableSpan.firstChild.offsetHeight")
		if (this.Height >= j)
			bTemp = true
		else
			bTemp = false
	}
	eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild.childNodes")
	for (i=0; i<oRows.length; i++)
	{
			oRows(i).style.backgroundColor = this.arrColor[i % 2]
			oRows(i).style.color = "black"
			oRows(i).style.height = "20px"
			oRows(i).name = i
	}
}
function _DealLogWhenDel()
{
	if (this.arrModiLog[this.rs_main.AbsolutePosition] == null )
		this.arrDelLog[this.arrDelLog.length] = this.RowStr
	else
	{
		if (!(this.arrModiLog[this.rs_main.AbsolutePosition] & 1))
			this.arrDelLog[this.arrDelLog.length] = this.RowStr
	}
	var arrTemp = new Array()
	for (var iKey in this.arrModiLog)
	{
		if (iKey < this.rs_main.AbsolutePosition)
			arrTemp[iKey] = this.arrModiLog[iKey]
		else if (iKey > this.rs_main.AbsolutePosition)
			arrTemp[iKey-1] = this.arrModiLog[iKey]
	}
	this.arrModiLog = arrTemp
}
function _DelRow(iEleRow)
{
	var oRows
	var oNodes
	if (this.preElement != null)
		if (this.preElement.firstChild.name != "Virtul")
		{
			this.rs_main.MoveFirst()
			this.rs_main.Move(this.PreRow)
			if (this.rs_main.AbsolutePosition < 0)
				return
			var iRCount = this.rs_main.RecordCount
			if (this.DefaultSort>=0)
				if (this.MaxSortNumber == this.rs_main.fields(this.DefaultSort).value && this.MaxSortNumber>0)
					this.MaxSortNumber--;
			this.rs_main.Fields(this.GetOrder[0]).value = "-1.0249E5"
			this.DealLogWhenDel()
			this.rs_main.Delete()
			if (this.rs_main.RecordCount == 0)
				this.IsEmpty = true
			else
				this.IsEmpty = false
			if (this.UseSort != "")
				eval("this.rs_main = " + this.UseSort + ".recordset")
			if (this.preElement != null)
			{	this.preElement.style.backgroundColor = this.preColor;
				this.preElement.style.color = "black";
				this.preElement = null
				if (this.FastField > 0)
				{
					this.preFastElement.style.backgroundColor = this.preColor;
					this.preFastElement.style.color = "black";
					this.preFastElement = null
 				}
			}
			this.PreLine = -1
			this.PreRow = -1
			this.LightRow = -1
			this.currentRow	= -1
			this.RowStr = ""
			eval("oRows = " + this.Name + "Measure")
			oRows.style.height = this.rs_main.RecordCount * Math.floor(this.Height / 8) + this.Height + 1
			if (this.FirstRow >= this.rs_main.RecordCount)
				this.FirstRow = this.rs_main.RecordCount - 1
			if (this.FirstRow < 0)
				this.FirstRow = 0
			this.ChangePage()
			this.ClearV()
			if (this.AskSum)
			{
				for (i=0; i<this.Widths.length; i++)
				{
					if (i != 0 & this.SumArr[i] != null)
					{
						this.UseSum(i,true)
						eval(this.Name + "Sums.childNodes(i).innerText = this.ChangeK(this.SumArr[i],i)")
					}
				}
			}
			if (iEleRow != null)
			{
				eval("oNodes = " + this.Name + "TableSpan.firstChild.firstChild.childNodes")
				for (var i=0; i<oNodes.length; i++)
				{
					if (oNodes(i).name == iEleRow+"")
					{
						eval(this.Name + "VirtualFocus.focus()")
						eval(this.Name + "VirtualFocus.select()")
						return oNodes(i)
					}
				}
			}
		}
}
function _SetCell(column,value)
{
	this.SaveRow(1);
	var strTemp = this.RowStr;
	if (column == null)
	{	this.SetRow(strTemp)
		return true
	}
	if (strTemp != "" || strTemp != null)
	{
		var arrTemp = strTemp.split(this.Divide)
		arrTemp[column] = value
		strTemp = arrTemp.join(this.Divide)
		return this.SetRow(strTemp)
	}
	else
		return false
}
function _SetRow(Content)
{
	var oNodes,oFastNodes,i,strTD
	if (this.preElement == null)
		return null
	if (this.preElement.firstChild.name == "Virtul")
		return null
	strTD = Content.split(this.Divide)
	for (var key in this.Calculator)
	{
	   var 	strCalculate = this.Calculator[key]
//	   strCalculate = strCalculate.replace(/\</g, "strTD[");
//	   strCalculate = strCalculate.replace(/\>/g, "]");
//	   strCalculate = strCalculate.replace("=", "=Math.round((") + ")* Math.pow(10,this.SumDot)) / Math.pow(10,this.SumDot)"
	   eval(strCalculate);
	}
	oNodes = this.preElement.childNodes
	if (this.preFastElement != null)
		oFastNodes = this.preFastElement.childNodes
	else
		oFastNodes = null
	this.rs_main.MoveFirst()
	this.rs_main.Move(this.PreRow)
	for (i=0;i<oNodes.length;i++)
	{
		if ( i > strTD.length)
			break

        if ((this.FieldsType[i]+"").substr(0,3) == "num")
        {
            //aa = bb.sub()
            if (parseFloat(strTD[i]) * 1 != parseFloat(strTD[i])|| parseFloat(strTD[i]) == 0)
            {
                if (this.SetZ == false)
                    strTD[i] = ""
            }
        }

		if (this.seedControl[i] == null || this.modi[i] == true)
		{
			if ((this.FieldsType[i]+"").substr(0,3) == "num")
			{
				if (parseFloat(strTD[this.GetOrder[i]]) * 1 != parseFloat(strTD[this.GetOrder[i]]))
					continue;
			}
 			if (this.checkBoxColumn == i)
			{
				oNodes(i).innerHTML = "<INPUT TYPE=CHECKBOX NAME=" + this.Name + "cb" + i + " id=" + this.Name + "cb" + i + " onclick='" + this.Name + ".checkItem()' " + strTD[this.GetOrder[i]] + "  " + this.checkBoxStyle + "></input>"
			}
 			else
 			if (this.modi != null)
			{
				if (this.modi[i] == true)
				{
					//eval(this.UseForm + this.Name + "Text" + this.GetOrder[i] + ".value = '" + this.ReplaceChar(strTD[this.GetOrder[i]],"'") + "'")
					//modify by lzy, for process flow down operate
					eval(this.UseForm + this.Name + "Text" + this.GetOrder[i]).value = this.ReplaceChar(strTD[this.GetOrder[i]],"'");
				}
				else
				{	//modify by hzh , for up(or down) arrow , 2007/11/7
					if (this.UseHTML){
						oNodes(i).innerHTML = this.ChangeK(strTD[this.GetOrder[i]],this.GetOrder[i],true);
					} else{
						oNodes(i).innerText = this.ChangeK(strTD[this.GetOrder[i]],this.GetOrder[i],true)
					}
					if (oFastNodes != null)
						oFastNodes(i).innerText = this.ChangeK(strTD[this.GetOrder[i]],this.GetOrder[i],true)
				}
			}
			else
			{	oNodes(i).innerText = this.ChangeK(strTD[this.GetOrder[i]],this.GetOrder[i],true)
				if (oFastNodes != null)
					oFastNodes(i).innerText = this.ChangeK(strTD[this.GetOrder[i]],this.GetOrder[i],true)
			}
			strTD[this.GetOrder[i]]+=""
			this.rs_main.Fields(this.GetOrder[i]).value = strTD[this.GetOrder[i]];
		}
	}
	Content = strTD.join(this.Divide);
	if (this.RowStr != Content)
	{	this.arrModiLog[this.rs_main.AbsolutePosition] = this.arrModiLog[this.rs_main.AbsolutePosition] | 2
		if (this.AskSum)
		{
			for (i=0; i<this.Widths.length; i++)
			{
				if (i != 0 & this.SumArr[i] != null)
				{
					this.UseSum(i,true)
					eval(this.Name + "Sums.childNodes(i).innerText = this.ChangeK(this.SumArr[i],i)")
				}
			}
		}
    }
	this.RowStr = Content
}
function _Append(Content)
{
	var iRow,i,strTD,oNodes
	var bReturn
	if (this.preElement != null)
	{
		eval("bReturn = " + this.BeforeSave)
		if (bReturn == false)
			return false
	}
	if (Content != null)
		strTD = Content.split(this.Divide)
	else
		strTD = ""
	eval("iRow = " + this.Name + ".rs_main.RecordCount")
	eval(this.Name + ".AddEvent(true,strTD)")
	eval("oNodes = " + this.Name + "Measure")
	eval("oNodes.style.height = " + this.Name + ".rs_main.RecordCount * Math.floor(this.Height / 8) + " + this.Name + ".Height + 1")
	var iFirst
	iFirst = this.FirstRow
	eval(this.Name + ".HighLightRow(iRow,'AddNew')")
}
function _AddEvent(bAppend,Cells)
{
	if (bAppend !=true && this.AddDelete!=true )
		return null
	var oNode, oRows, strCells, iRows, oTemp,iLen
	this.ClearV()
	this.LightRow = this.rs_main.RecordCount
	this.rs_main.Addnew()
	if (this.UseSort != "")
		eval("this.rs_main = " + this.UseSort + ".recordset")
	this.rs_main.MoveLast()

	if (Cells == null)
		iLen = this.Widths.length
	else
		iLen = Cells.length
	for (var field=0; field<this.Widths.length||(field<this.rs_main.Fields.count&&field<iLen); field++)
	{
		if (this.GetOrder[field] == this.DefaultSort )
		{
			this.MaxSortNumber++;
			this.rs_main.Fields(this.GetOrder[field]).Value = this.MaxSortNumber;
		}
		else
		{
			if (field>=this.Widths.length)
			{
				this.rs_main.Fields(field).value = Cells[field]
			}
			else
			{
				if (this.AddDelete != true)
					this.rs_main.Fields(this.GetOrder[field]).value = Cells[this.GetOrder[field]]
				else
				{
					if ((this.FieldsType[field]+"").substr(0,3) == "num")
						this.rs_main.Fields(this.GetOrder[field]).value = Cells == null?0:isNaN(parseInt(Cells[field]))?0:Cells[field]
					else
						this.rs_main.Fields(this.GetOrder[field]).value = Cells == null?"":Cells[field]
				}
			}
		}
	}
	this.rs_main.Update()
	this.arrModiLog[this.rs_main.AbsolutePosition] = 1
	this.ClearV()
}
function _TDMouseDown()
{
	var srcNode
	return
	if (event.srcElement.tagName != "TD")
	{
		eval("srcNode = " + this.Name + ".DownEventSrc")
		if (srcNode == null)
		{
			eval(this.Name + ".DownEventSrc = " + this.Name + "TableAll.onmouseup")
			eval(this.Name + "TableAll.onmouseup=null")
		}
	}
	else
	{
		eval("srcNode = " + this.Name + ".DownEventSrc")
		if (srcNode != null)
		{
			eval(this.Name + "TableAll.onmouseup = srcNode")
			eval(this.Name + ".DownEventSrc = null")
		}
	}
}
function _MouseDown_Event(Source,Direct,keyControl)
{
    //below check is added by lzy, disable the right mouse.
	if (window.event.button == 2)
    {
        window.returnValue = false;
        window.cancelBubble = true;
        return false;
    }

	var elerow,fastRow,strRe, EnableFileld
        if (this.CanNotChangRow)
            return
	this.ClearV()
		if (Source == null)
		{
			elerow = window.event.srcElement;
			if (this.onEvent == true)
				return
			this.onEvent = true
		}
		else
			elerow = Source
		try
		{
			if (elerow.parentElement.parentElement != null)
				null
		}
		catch(e)
		{   this.onEvent = false
			return false
		}
		if (elerow.parentElement.parentElement != null)
			elerow = elerow.parentElement;
		if (elerow.parentElement.classid == this.Name + "TableRow" || elerow.parentElement.classid == this.Name + "TableRow" + this.FastField)
		{
			elerow = elerow.parentElement;
		}
	if (elerow.classid == this.Name + "TableRow" || elerow.classid == this.Name + "TableRow" + this.FastField)
	{	var iRow = elerow.name * 1
		elerow = document.all(this.Name + "TableBody").firstChild.childNodes(iRow-this.FirstRow)
		if (this.FastField > 0)
			fastRow = document.all(this.Name + "TableBody" + this.FastField).firstChild.childNodes(iRow-this.FirstRow)
		else
			fastRow = null
		eval("bReturn = " + this.BeforeHighLight)
		if (bReturn == false)
		{	this.onEvent = false
			return false
		}
		if (this.currentRow == elerow.name)
		{
			if (Source == null)
			{
				this.onEvent = false
			}
			return this.RowStr
		}
		if (Source == null && this.preElement != null)
		{	var bReturn,sid
			sid = this.Name + "TableSpan.firstChild.firstChild.childNodes(" + (elerow.name*1 - this.FirstRow) + ")"
			eval("bReturn = " + this.BeforeSave)
			if (bReturn == false)
			{	this.onEvent = false
				return false
			}
			eval("elerow = " + sid)
		}
		if (this.AddDelete == true && this.NotEmpty >= 0 && this.preElement != null )
		{
			if (this.preElement.firstChild.name != "Virtul")
			{
				if (this.modi != null)
				{	if (this.modi[this.NotEmpty] == true)
					{
						var editnode
						if (document.all(this.Name + "Text" + this.NotEmpty) != null)
						{
							eval("editnode = " + this.Name + "Text" + this.NotEmpty + ".value")
							if (editnode == "")
								elerow = this.Delete(elerow.name)
						}
					}
					else
					{
						if (this.preElement.childNodes(this.NotEmpty).innerText == "")
							elerow = this.Delete(elerow.name)
					}
				}
				else
				{
					if (this.preElement.childNodes(this.NotEmpty).innerText == "")
						elerow = this.Delete(elerow.name)
				}
				if (this.outerSelect == true && this.seedControl[this.NotEmpty] != null  && this.preElement != null)
				{    if (this.preElement.childNodes(this.NotEmpty).childNodes.length > 1)
					if (this.preElement.childNodes(this.NotEmpty).childNodes(1).tagName == "SELECT")
					{
						editnode = this.preElement.childNodes(this.NotEmpty).childNodes(1).options(this.preElement.childNodes(this.NotEmpty).childNodes(1).selectedIndex).text
						if (editnode == "")
							elerow = this.Delete(elerow.name)
					}
				}
			}
		}
		EnableFileld = this.GetFirstEnableChild()
		if (this.AddDelete == true && elerow.childNodes(EnableFileld).id == this.Name + "Add" )
		{
			this.AddDelete == false
			elerow.childNodes(EnableFileld).id = null
			this.AddEvent()
			eval(this.Name + "Measure.style.height = " + (this.rs_main.RecordCount * Math.floor(this.Height / 8) + this.Height * 1 + 1 * 1))
			this.ChangePage()
			this.AddDelete == true
			if (Source == null)
			{
				this.onEvent = false
			}
			return
		}
		if (this.preElement != null && Direct != true)
		{	this.preElement.style.backgroundColor = this.preColor;
			this.preElement.style.color = "black";
			if (this.FastField > 0)
			{
				this.preFastElement.style.backgroundColor = this.preColor;
				this.preFastElement.style.color = "black";
				this.preFastElement.style.height = "20px"
 			}
		}
		this.preColor = elerow.style.backgroundColor;
		elerow.style.backgroundColor = this.LightBKColor
		elerow.style.color = this.LightColor
		eval(this.BeforeChange)
		if (this.FastField > 0)
			this.RowStr = this.EditRow(elerow.name,fastRow)
		else
			this.RowStr = this.EditRow(elerow.name)
		if (this.FastField > 0)
		{	fastRow.style.backgroundColor = "midnightblue";
			fastRow.style.color = "white"
			this.preFastElement = fastRow
		}
		this.preElement = elerow;
		if (this.preFastElement != null)
		{
			this.preFastElement.style.height = this.preElement.offsetHeight
			if (this.preElement.parentElement.parentElement.parentElement.scrollTop != 0)
				this.preElement.parentElement.parentElement.parentElement.scrollTop = 0
		}
		this.currentRow = elerow.name
		this.PreLine = elerow.name
		eval(this.AfterNew)
	}
	else
	{
		while (elerow.parentElement != null && elerow.name != this.Name + "TableAll")
		{
			elerow = elerow.parentElement
		}
	}
	if (Source == null)
	{
		this.onEvent = false
	}
}
function _SaveRow(NoCh)
{
	if (this.CanNotChangRow)
		return;
	var i,j,strTemp,bTemp
    var iRow
    var strRe = ""
    var strRow = ""
	iRow = this.PreRow
	if (this.rs_main.RecordCount > 0 )
	{	this.rs_main.MoveFirst();
		this.rs_main.Move(iRow)
	}
	else 
		return;
	if (this.rs_main.AbsolutePosition < 0)
		return
	for (i=0; i<this.Widths.length; i++)
	{
		strRow += this.rs_main.Fields(i).Value
		if (i < this.Widths.length -1)
					strRow += this.Divide
		if (this.PreRow != -1)
		{
			if (this.modi != null  & this.preElement.firstChild.name != "Virtul")
			    if (this.modi[i] & this.PreRow != -1)
				{
					eval("strTemp = " + this.UseForm + this.Name + "Text" + i + ".value")
					strTemp += ""
					strTemp = strTemp.replace(/(^\s*)|(\s*$)/ig,"");
					if ((this.FieldsType[i]+"").substr(0,3) == "num")
					{
						if (parseFloat(strTemp) * 1 != parseFloat(strTemp))
						{
							if (this.SetZ == false)
								strTemp = "*SetZ*"
							else
								strTemp = 0
						}
						else
							strTemp = parseFloat(strTemp)
					}
					//if (NoCh == null)
					if (NoCh != 1)
					{
						if (strTemp != "*SetZ*" )
						{
								this.preElement.childNodes(i).innerText = this.ChangeK(strTemp,this.GetOrder[i],true)
						}
						else
						{
								this.preElement.childNodes(i).innerText = ""
						}
						this.preElement.childNodes(i).style.padding=2
					}
					if (strTemp != "*SetZ*" )
						this.rs_main.Fields(this.GetOrder[i]).value = strTemp;
					if (this.preFastElement != null)
						this.preFastElement.childNodes(i).innerHTML = this.preElement.childNodes(i).innerHTML
					continue
				}
			if 	(i < this.seedControl.length)
				if (this.seedControl[i] != null  & this.preElement.firstChild.name != "Virtul" )
				{
						eval("strTemp = " + this.Name + "Cshow" + i + ".innerText")
						strTemp = this.preElement.childNodes(i).innerText
						strTemp = strTemp.replace(this.seedControl[i],"")
						strTemp = strTemp.replace("<SPAN ID=" + this.Name + "'Cshow'" + i + ">","")
						strTemp = strTemp.replace("</SPAN>", "")
						if (this.preElement.childNodes(i).childNodes.length > 1)
						if (this.outerSelect == true && this.preElement.childNodes(i).childNodes(1).tagName == "SELECT")
						{	if (this.preElement.childNodes(i).childNodes(1).selectedIndex == -1)
								strTemp = ""
							else
								strTemp = this.preElement.childNodes(i).childNodes(1).options(this.preElement.childNodes(i).childNodes(1).selectedIndex).text
						}
						if (NoCh == null)
						{
							if (this.OuterControl != i)
								this.preElement.childNodes(i).innerHTML = this.ChangeK(strTemp,this.GetOrder[i])
							else
								this.preElement.childNodes(i).innerHTML = this.ChangeK(this.rs_main.Fields(this.GetOrder[i]).value,this.GetOrder[i])
							if (this.OuterControl1 == i)
								this.preElement.childNodes(i).innerHTML = this.ChangeK(this.rs_main.Fields(this.GetOrder[i]).value,this.GetOrder[i])
							if (this.OuterControl2 == i)
								this.preElement.childNodes(i).innerHTML = this.ChangeK(this.rs_main.Fields(this.GetOrder[i]).value,this.GetOrder[i])
							if (this.OuterControl3 == i)
								this.preElement.childNodes(i).innerHTML = this.ChangeK(this.rs_main.Fields(this.GetOrder[i]).value,this.GetOrder[i])
							if (this.OuterControl4 == i)
								this.preElement.childNodes(i).innerHTML = this.ChangeK(this.rs_main.Fields(this.GetOrder[i]).value,this.GetOrder[i])
							this.preElement.childNodes(i).style.padding = 2
						}
						if (this.OuterControl != i && this.OuterControl1 != i && this.OuterControl2 != i && this.OuterControl3 != i && this.OuterControl4 != i)
							this.rs_main.Fields(this.GetOrder[i]).value = strTemp;
						if (this.preFastElement != null)
							this.preFastElement.childNodes(i).innerHTML = this.preElement.childNodes(i).innerHTML
						continue
				}
		}
		if (this.preFastElement != null)
			this.preFastElement.childNodes(i).innerHTML = this.preElement.childNodes(i).innerHTML
	}
	for (i=0; i<this.Widths.length; i++)
	{	strRe += this.rs_main.Fields(i).Value
		if (i < this.Widths.length -1)
					strRe += this.Divide
	}
	if (strRe != strRow)
	{	this.arrModiLog[this.rs_main.AbsolutePosition] = this.arrModiLog[this.rs_main.AbsolutePosition] | 2
		if (this.AskSum)
		{
			for (i=0; i<this.Widths.length; i++)
			{
				if (i != 0 & this.SumArr[i] != null)
				{
					this.UseSum(i,true)
					eval(this.Name + "Sums.childNodes(i).innerText = this.ChangeK(this.SumArr[i],i)")
				}
			}
		}
	}
	this.RowStr = strRe;
	if (NoCh == null)
		eval(this.Name + "VirtualFocus.focus()")
}
function _EditRow(Rownum,oFast)
{
	var i,j,strTemp,strCats,strStyle,iWidth,strRe,oRows,oFastRows,oParent,ilen
	var selectText = null
	var sInHT
	strRe = ""
	eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild.childNodes(Rownum-this.FirstRow).childNodes")
	if (this.currentRow != Rownum)
	{
		if (this.PreRow != -1)
			this.SaveRow()
		if (Rownum < this.rs_main.RecordCount)
		{	this.rs_main.MoveFirst()
			this.rs_main.Move(Rownum)
		}
		eval(this.BeforeNew)
		for (i=0; i<this.Widths.length; i++)
		{
			if (Rownum < this.rs_main.RecordCount)
			{
				oRows(i).name = ''
				strRe += this.rs_main.Fields(i).Value
				if (i < this.Widths.length -1 && oRows(i).name != "Virtul")
					strRe += this.Divide
			}
			if (this.modi != null)
			{
				if (this.modi[i] && oRows(i).name != "Virtul")
				{
					if (this.seedControl[i] != null & oRows(i).name != "Virtul")
						iWidth = this.Widths[this.GetOrder[i]] - 28
					else
						iWidth = this.Widths[this.GetOrder[i]] - 1
					if (this.SetZ == true || this.rs_main.Fields(this.GetOrder[i]).Value * 1 != 0  || (this.FieldsType[this.GetOrder[i]]+"").substr(0,3) != "num")
					{
						if (this.ShowZ == true || this.rs_main.Fields(this.GetOrder[i]).Value != 0   || (this.FieldsType[this.GetOrder[i]]+"").substr(0,3) != "num")
							strTemp =  this.rs_main.Fields(this.GetOrder[i]).Value
						else
							strTemp = ""
					}
					else
						strTemp = ""
					strStyle = ' Style="WIDTH:' + iWidth + 'px; border-bottom:1 solid black;"'
					if ((this.FieldsType[i]+"").substr(0,3) == "num" || this.FieldsType[i] == "strnum")
						strStyle += " onKeyDown='_DealNumberPress()' onKeyPress='_DealNumberPress()' onpaste='event.returnValue=false' ondrop='event.returnValue=false' onchange='" + this.Name +".SetCell()'"
					else
						strStyle += "  ondrop='event.returnValue=false' "
					strStyle += ' value="' + this.ChangeK(strTemp) + '"'
					if (this.MaxSize[i] != null)
						strStyle += " maxlength=" + this.MaxSize[i]
					oRows(i).innerHTML = "<input type=Text id=" + this.Name + "Text" + i + strStyle + " >"
					oRows(i).style.padding = 0
					if (selectText == null)
					{	if (this.HideColumn.length > 0)
						{	if (this.HideColumn[i] == true)
								selectText = this.Name + "Text" + i
						}
						else
							selectText = this.Name + "Text" + i
					}
					if (oFast != null)
						oFast.childNodes(i).innerHTML = ""
				}
			}
			if 	(i < this.seedControl.length)
			if (this.seedControl[i] != null  && oRows(i).name != "Virtul")
			{
				oRows(i).style.padding = 0
				if ((this.outerSelect == true && !(this.seedControl[i] .indexOf("<button")>=0)) || this.OuterControl == i || this.OuterControl1 == i || this.OuterControl2 == i || this.OuterControl3 == i || this.OuterControl4 == i)
				{	sInHT = oRows(i).innerHTML
					oRows(i).innerHTML = ""
					oRows(i).innerHTML = "<SPAN ID=" + this.Name + "Cshow" + i + this.ControlValue[i] + ">" + oRows(i).innerHTML + "</SPAN>" + this.seedControl[i].replace(">" + sInHT, " selected>" + sInHT)
				}
				else
				{	oRows(i).innerHTML = "<SPAN ID=" + this.Name + "Cshow" + i + this.ControlValue[i] + ">" + oRows(i).innerHTML + this.seedControl[i] + "</SPAN>"
				}
			}
		}
	}
	this.PreRow = Rownum;
	this.LightRow = Rownum
	if (selectText != null && this.NoFocus == false)
	{	eval(selectText + ".focus()")
		eval(selectText + ".select()")
		this.FocuText = selectText
	}
	this.RowStr = strRe
	eval(this.AfterChange)
	if (strRe.charAt(0) == "")
		return null
	return strRe
}
function _GetRownum()
{
	return this.LightRow * 1;
}
function _Refresh(withEvent)
{
	var strTemp
	if (this.AddDelete == true && this.NotEmpty >= 0 && this.preElement != null )
	{
		if (this.preElement.firstChild.name != "Virtul")
		{
			if (this.modi != null)
			{	if (this.modi[this.NotEmpty] == true)
				{
					var editnode
					if (document.all(this.Name + "Text" + this.NotEmpty) != null)
					{
						eval("editnode = " + this.Name + "Text" + this.NotEmpty + ".value")
						if (editnode == "")
							this.Delete()
					}
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
				if (this.outerSelect == true && this.seedControl[this.NotEmpty] != null  && this.preElement != null)
				{    if (this.preElement.childNodes(this.NotEmpty).childNodes.length > 1)
					if (this.preElement.childNodes(this.NotEmpty).childNodes(1).tagName == "SELECT")
					{
						var editnode = this.preElement.childNodes(this.NotEmpty).childNodes(1).options(this.preElement.childNodes(this.NotEmpty).childNodes(1).selectedIndex).text
						if (editnode == "")
							 this.Delete()
					}
				}
	if (this.preElement != null)
	{
		if (withEvent == null)
		{
			var bReturn
			eval("bReturn = " + this.BeforeSave)
			if (bReturn == false)
				return false
		}
		this.SaveRow();
		this.preElement.style.backgroundColor = this.preColor;
		this.preElement.style.color = "black";
		this.preElement = null
		if (this.preFastElement != null)
		{
			this.preFastElement.style.backgroundColor = this.preColor;
			this.preFastElement.style.color = "black";
			this.preFastElement = null
 		}
	}
	this.PreLine = -1
	this.PreRow = -1
	this.currentRow = -1
	this.LightRow = -1
	return true
}
function _EventRow(Name)
{
	var elerow
	elerow = window.event.srcElement;
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
function _ClearV(NoCount)
{
	if (this.rs_main.RecordCount >=1)
	{
		this.rs_main.MoveFirst()
		if (this.rs_main(0) == "-1.0249E5")
			this.rs_main.Delete()
	}
	if (this.rs_main.RecordCount == 0)
		this.IsEmpty = true
	else
		this.IsEmpty = false
	if (this.ShowCount == true && NoCount == null)
		eval(this.Name + "Count.innerText = '" + this.JILUSHU + this.rs_main.RecordCount + "'")
}
function _Clear()
{
	if (this.rs_main.RecordCount >=1)
	{
		this.rs_main.MoveFirst()
		while (this.rs_main.recordcount > 0)
		{
			this.rs_main.Delete()
		}
		this.MaxSortNumber = 0;
	}
	if (this.rs_main.RecordCount == 0)
		this.IsEmpty = true
	else
		this.IsEmpty = false
	if (this.ShowCount == true )
		eval(this.Name + "Count.innerText = '" + this.JILUSHU + this.rs_main.RecordCount + "'")
	this.preElement = null
	this.preFastElement = null
	this.PreRow = -1
	this.currentRow = -1;
	this.PreLine = -1
	this.FirstRow = 0
	this.LightRow = -1
	this.RowStr = ""
	this.ChangePage();
}
function _DealNumberPress()
{
 var charCode=(navigator.appName =="NetScape")?event.Which: event.keyCode;

 if (charCode>31 && (charCode<48 ||charCode>57) && (charCode<96 ||charCode>105) && charCode != 46 && charCode != 110 && charCode != 190 && charCode != 37 && charCode != 39  && charCode != 35  && charCode != 36 && charCode != 45 && charCode != 109   && charCode != 189)
   	event.returnValue =false;
 if (event.srcElement.value.indexOf(".")>=0 && (charCode == 110 || charCode == 190))
	event.returnValue =false;
 if (event.srcElement.value.indexOf("-")>=0 && (charCode == 109 || charCode == 189))
	event.returnValue =false;
}
function _DealBack()
{
}
function _DealKeyPress()
{
	var Tag = event.srcElement.parentElement
	var valReturn = true
	var charCode=(navigator.appName =="NetScape")?event.Which: event.keyCode;
	if (charCode == 9)
	{
		var iTableWidth,iWidth
		eval("iWidth = " +this.Name + "TableBody.offsetWidth")
		eval("iTableWidth = " +this.Name + ".TableWidth")
		if (iWidth > iTableWidth)
			event.returnValue =false;
	}
	if (charCode == 32 && event.srcElement.tagName != "INPUT")
		event.returnValue =false;
	if (this.NoKey == true)
		return
	var iRow = this.currentRow * 1
	var oNode, iTop, iCurr, oRows
	switch(charCode)
	{
		case 37:
			break;
		case 38:
			if (this.currentRow > 0)
			{	iRow -= 1
			    eval(this.Name + "VirtualFocus.focus()")
				eval("valReturn = " + this.BeforeUpDown)
				if (valReturn == false)
					return false
				this.HighLightRow(iRow)
				eval(this.UpDown)
			}
			event.returnValue =false;
			break;
		case 39:
			break;
		case 13:
			event.returnValue =false;
			event.cancelBubble = true;
			var eventid = event.srcElement.id
			if (eventid.search(this.Name + "Text") !=-1 )
			{
				eventid = eventid.replace(this.Name + "Text","");
				var index = eventid*1 + 1
				while(!this.modi[index] && index<this.modi.length)
					index++;
				if (index < this.modi.length)
				{	eval(this.Name + "Text" + index + ".focus()");
					eval(this.Name + "Text" + index + ".select()");
					break
				}
				else
					eval(this.Name + "VirtualFocus.focus()")
			}
			else if (event.srcElement.tagName == "BUTTON")
			{
				break;
			}
		case 40:
			if (this.currentRow < this.rs_main.RecordCount)
			{	iRow += 1
			    eval(this.Name + "VirtualFocus.focus()")
				if (iRow == this.rs_main.RecordCount && this.AddDelete == true && iRow>0)
				{	this.SaveRow(true)
					this.rs_main.AbsolutePosition = iRow
					if (this.rs_main.Fields(this.GetOrder[this.NotEmpty]).value != "")
					{

						eval("valReturn = " + this.BeforeUpDown)
						if (valReturn == false)
							return false
						this.HighLightRow(iRow)
						eval(this.UpDown)
					}
				}
				else
				{	if (iRow < this.rs_main.RecordCount)
					{
						eval("valReturn = " + this.BeforeUpDown)
						if (valReturn == false)
							return false
						this.HighLightRow(iRow)
						eval(this.UpDown)
					}
				}
			}
			event.returnValue =false;
			break;
		case 33:
			break;
			if (this.LightRow > this.FirstRow)
				iCurr = this.FirstRow
			else
				iCurr = this.FirstRow - Math.floor((this.Height-20)/20 -1)
			if (iCurr < 0)
				iCurr = 0
			this.HighLightRow(iCurr)
			event.returnValue =false;
			break;
		case 34:
			break;
			this.HighLightRow(-1,true)
			event.returnValue =false;
			break;
		case 46:
			if (this.canKeyDelete)
				this.Delete();
			break;
		case 32:
			if (event.srcElement.tagName != "INPUT")
				event.returnValue =false;
			break;
		default:
			break;
	}
}
function _GetFirstEnableChild()
{
	if (this.HideColumn.length > 0)
	for (var i=0; i< this.HideColumn.length; i++)
	{
		if (this.HideColumn[i] == true)
			return i
	}
	return 0
}
function _ToUnicode(str)
{
 var dest="";
 var n,i;
 for ( i=0;i<str.length;i++)
 {
  n = str.charCodeAt(i);
  if ( n < 128)
  {
   switch( n )
   {
    case 34:
     dest += "&quot;"
     break;
    default:
     dest += str.charAt(i);
   }
  }
  else
  {
   dest += "\\&#" + n + ";";
  }
 }
 return dest;
}
function _ToRe(r)
{
	if (typeof(r) != "number")
	{
			r = r.replace(/\\/g, "\\\\");
			r = r.replace(/&/g, "\\&");
			r = r.replace(/\|/g, "\\|");
			r = r.replace(/\"/g, "\\\"");
			r = r.replace(/=/g, "\\=");
			r = r.replace(/\(/g, "\\(");
			r = r.replace(/\)/g, "\\)");
			r = r.replace(/\</g, "\\<");
			r = r.replace(/\>/g, "\\>");
	}
	return r
}
function _ClearAMD()
{
	this.arrDelLog = new Array()
	this.arrModiLog = new Array()
}
function _RecordsetToString(fieldDelim,RowDelim,mask,ReplaceColumn,ReplaceTable)
{
	var srcRecordSet = this.rs_main
	var arrAMD = new Array("D","A","M","A");
   	var arrRow = new Array();
   	var arrMask = mask.split(",");
   	for (var k in this.arrModiLog)
   	{
   		if (k > srcRecordSet.recordCount || srcRecordSet.recordCount==0 || k==0)
   			continue;
   		srcRecordSet.AbsolutePosition = k;
		var arrField = new Array();
		arrField[0] = arrAMD[this.arrModiLog[k]]
		for (var j=1; j <= arrMask.length; j++)
		{	if (arrMask[j-1] != ReplaceColumn)
				arrField[j] = srcRecordSet.fields(arrMask[j-1]*1).value;
			else
			{
				for (var replacekey in ReplaceTable)
				{
					if (srcRecordSet.fields(arrMask[j-1]*1).value == replacekey)
						arrField[j] = ReplaceTable[replacekey];
				}
				if (arrField[j]	== null) arrField[j] = ReplaceTable["else"]
			}
		}
		arrRow[arrRow.length] = arrField.join(fieldDelim);
   	}
	var arrDel
   	for (var k in this.arrDelLog)
   	{
		arrDel = this.arrDelLog[k].split(this.Divide)
		var arrField = new Array();
		arrField[0] = arrAMD[0]
		for (var j=1; j <= arrMask.length; j++)
			arrField[j] = arrDel[arrMask[j-1]];
		arrRow[arrRow.length] = arrField.join(fieldDelim);
   	}
	return arrRow.join(RowDelim);
}
function _ReplaceChar(strTrim,chrHave)
{
	return strTrim;
	/*
	var strTemp
	strTemp = ""
	for(var i=0; i<strTrim.length; i++)
	{
		if (strTrim.substring(i,i+1) != chrHave)
		{
			if (strTrim.substring(i,i+1) != "\\")
				strTemp += strTrim.substring(i,i+1)
			else
				strTemp += "\\\\"
		}
		else
			strTemp += "\\" + chrHave
	}
	return strTemp
	*/
}
