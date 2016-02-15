








/*
功能  ：创建表格类对象。
作者  ：李翼嵩
时间  ：2000年8月
参数  ：RecordSet -- 包含要显示的数据记录集。
返回值：
*/

function clsTable(RecordSet,ObjectName)
{
//public property 	
	this.Height = 100;				//表格高度，不包括标题头。
	this.Widths = null;				//表格字段宽度数组。
	this.TableWidth = 100
	this.modi = new Array();				//可编辑字段数组 true -- 可编辑。
	this.rs_main = RecordSet;		//数据记录集。
	this.Title = true;				//是否显示标题	
	this.AddDelete = false;			//是否可以添加删除记录
	this.RowStr	=""					//得到当前行的字串。
	this.arrColor = new Array("#fffbe7","#e7e7bd") //记录行的背景色	
	this.UseSort = ""				//指定排序的TDC控件
	this.IsEmpty = false			//判断是否当前表格是否为空
	this.Divide = ","				//使用的分隔符
	this.outerSelect = false		//使用外部选择SELECT标签，将选择内容放在格中。
	
//public Method
	this.Display = Display;			//得到HTML字串。
	this.MouseDown_Event = MouseDown_Event //鼠标事件
	this.AddEvent = AddEvent;		//增加一条记录。
	this.GetRowNumber = GetRownum	//得到当前行索引值。
	this.Refresh = Refresh			//刷新表格。
	this.ColumnStyle = ColumnStyle	//设定一列外观属性。
	this.TableStyle = TableStyle	//设定表外观属性。
	this.HeadStyle = HeadStyle		//设定标题外观属性。
	this.Delete = DelRow			//删除当前行
	this.Append = Append			//增加一行（程序手动方式）
	this.SetRow = SetRow			//设置一行内容
	this.UseControl = UseControl	//使用用户指定的插入控件。
	this.ControlReturn = ControlReturn //设置用户插入控件所在格的内容。
	this.PageColumn = PageColumn    //设置横向分页功能
	this.HideColumn = new Array()			//隐藏指定的字段
	this.seedControl = new Array()
	this.ControlValue = new Array()
	this.FieldsType = new Array()
	this.MaxSize = new Array()
	this.EventRow = EventRow
	this.ClearV = ClearV
	
//private property and method
	this.Name = ObjectName;			//生成对象的名称。
    this.PreRow = -1;				//高亮前一行记录。
	this.strColumn = ""				//列属性字串。
	this.strTable = 'Style="color:black;font-size:9pt"'	//表属性字串。
	this.strHead = 'Style="font-family:宋体;font-size:9pt"'	//标题属性字串。	
	this.AskSum = false;					//是否增加“合计“
	this.SumArr = new Array(RecordSet.Fields.Count);	//合计字段合计值。
	this.SumDot = 2
	this.EditRow = EditRow;			//对当前行进行可编辑处理。
	this.UseSum = UseSum;			//增加合计字段，可重复调用。
	this.currentRow = -1;			//记录目前行索引值。
    this.preElement = null;			//保存当前行，当其它行点击高亮时恢复该行
    this.preColor = null;			//保存当前行颜色。
	this.BackRefresh = BackRefresh  //刷新背景色
	this.SaveRow = SaveRow			//保存高亮行的内容到记录集。
	this.Sort = Sort
	this.UseForm = ""
	this.Pages = 1
	this.PageArr = new Array()		
	this.NewPage = NewPage
	this.WhichPage = 0
	this.SortColumn = -1			//指定由哪个字段排序
	this.SortAsc = ""				//排序方法：""为正序，"-"为逆序。
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
	this.NoKey = false			//true -- 不支持键盘控制上下
	this.UpDown = null			//高亮行上下移动时调用的函数
	
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
		{	//显示自定义字段
			var ArrFree = sCookie.split(",")
			this.NewPage(0,ArrFree)
		}
		else
		{	
			if (this.PageArr.length > 1)
				//若分页，显示第一页
				this.NewPage(1)
			else
			{	//没分页，则显示全部
				var ArrAll = new Array()
				for (var ki = 0; ki<this.Widths.length; ki++)
					ArrAll[ki] = ki			
				this.NewPage(0,ArrAll)

				//按钮不下陷
				if (this.StrFree != "")
					eval(this.UseForm + this.Name + "0img.src = '" + this.ControlPath + "Images/" + "0.gif'")
				
			}
		}
	}

}

/*
功能  ：自由选择指定字段。
作者  ：李翼嵩
时间  ：2000年11月
参数  ：SaveName 保存内容的名称。
*/
function FreeSelect(SaveName)
{
	this.StrFree = SaveName
	//this.Pages ++
}
/*
功能  ：高亮指定行。
作者  ：李翼嵩
时间  ：2000年11月
参数  ：RowNum 行号（base 0）
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
功能  ：是否分页显示字段，指定分几页显示。
作者  ：李翼嵩
时间  ：2000年8月
参数  ：Columns 指定排序的字段
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
功能  ：是否分页显示字段。
作者  ：李翼嵩
时间  ：2000年8月
参数  ：PageNum 指定显示第几页
        Free 是否为自由选择页 
*/
function NewPage(PageNum,Free,SetHide)
{
	//得到该页的字段们
	var temp
	if (Free != null)
		this.PageArr[PageNum] = Free
	
	temp = this.PageArr[PageNum]
	if (temp == null || temp =="null" || temp == "")
	{
		this.SetFree()
			return
	}
	
	//将所有字段都不显示，只显示该页字段
	for(var i=0; i<this.Widths.length; i++)
		this.HideColumn[i] = false
	for(var i=0; i<temp.length; i++)
		this.HideColumn[temp[i]] = true

	if (SetHide == true)
		return
		
	if (this.preElement != null)
	{	//记录下当前高亮行的位置
		var oldPos	
		eval("oldPos = " + this.Name + "TableSpan.scrollTop")	
		oldPos = this.preElement.offsetTop - oldPos
	}
		
	//刷新后显示与不显示的设置才生效。
	var i,oRows,iTemp

	//将须隐藏字段隐藏。
	eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild.childNodes")
	for (i=0; i<oRows.length; i++)
	{
			for (var n=0; n<this.HideColumn.length; n++)
				if (this.HideColumn[n] != true)
					oRows(i).childNodes(n).style.display = "none"
				else
					oRows(i).childNodes(n).style.display = "block"
	}


	iTemp = 23 //表的新宽度
	
	//将须隐藏表头、表尾隐藏。
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

	//当TableBody宽度变小时，横向滚动条滚动范围却不改变，只有使用以下方法才能改变。
	var oNode
	eval("oNode = " + this.Name + "TableBody.lastChild.lastChild")
	if (oNode.style.height == "20px")
		oNode.style.height = "21px"
	else
		oNode.style.height = "20px"
	//当TableBody宽度变小时，横向滚动条滚动范围却不改变，只有使用以上方法才能改变。

/*	???
	
	if (this.preElement != null)
	{
		this.preElement.style.backgroundColor = "midnightblue"
		this.preElement.style.color = "white"
	}
*/	
	this.WhichPage = PageNum
	
	//重新定位高亮行。
	if (this.preElement != null)
	{	var iTop
		iTop = this.preElement.offsetTop - oldPos
		eval(this.Name + "TableSpan.scrollTop = " + iTop)	
	}

	
	//将所有按钮抬起，只将该页按钮下陷
	if (this.StrFree != "")
		eval(this.UseForm + this.Name + "0img.src = '" + this.ControlPath + "Images/" + "0.gif'")
	for (var j=1; j<this.Pages; j++)
		eval(this.UseForm + this.Name + j + "img.src = '" + this.ControlPath + "Images/" + j +".gif'")

	eval(this.UseForm + this.Name + PageNum + "img.src = '" + this.ControlPath + "Images/" + PageNum +"_.gif'")

}



/*
功能  ：是否排序。
作者  ：李翼嵩
时间  ：2000年8月
参数  ：Columns 指定排序的字段
*/
function Sort(column)
{
	
	//若是序号字段则不排序
	if (this.rs_main.Fields(column).Name == "序号")
		return
		
	var Ascend, Element
	
	Ascend = "" //正序还是反序
	Element = window.event.srcElement
	
	//判断是否有记录可供排序
	if (this.rs_main.RecordCount >1)
	{
		//是否点击标题后由事件触发排序。
		if (Element.className == "HeadSort")
		{
			//是否还是前一次排序的列。
			if (this.SortColumn == column)
			{
				//若是则变换排序方式。 
				if (this.SortAsc == "")
					//正-->反
					this.SortAsc = "-"
				else
					//反-->正
					this.SortAsc = ""
			}
			else
			{	//否则按照正序
				this.SortColumn = column
				this.SortAsc = ""
			}
			
		}
		eval("this.SortLeft = " + this.Name + "TableSpan.scrollLeft" )		
		//刷新界面，以便保存当前行信息
		this.Refresh()
		//执行排序，设定TDC控件排序。
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
功能  ：设置用户插入控件所在格的内容。
作者  ：李翼嵩
时间  ：2000年8月
参数  ：strVal -- 要显示的内容。
        Col -- 插入字段的位置。
*/
function ControlReturn(strVal,col)
{
	if (this.seedControl.length >= col)
		eval(this.Name + "Cshow" + col + ".innerText = strVal")
}

/*
功能  ：使用用户指定插入控件。
作者  ：李翼嵩
时间  ：2000年8月
参数  ：Col -- 插入字段的位置。
        strContent -- 插入控件的内容字串(HTML)
        ShowChange -- true   显示控件控制值
        ShowChange -- false  不显示控件控制值
        Position -- "Front"  控件显示在值前
        Position -- "Behind"  控件显示在值后
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
功能  ：添加合计字段。若多次调用，则对多个字段进行合计。
作者  ：李翼嵩
时间  ：2000年8月
参数  ：Column -- 需要合计的字段编号（base 0）。
返回值：
*/
function UseSum(Column)
{
	if (Column <= this.Widths.length)
	{
		this.AskSum = true;
		//若记录为空，则不合计。
		this.SumArr[Column] = 0
		if (this.rs_main.RecordCount > 0 )
		{
			this.rs_main.MoveFirst()
			//依次进行合计
			while (!this.rs_main.EOF)
			{
				//若该字段不是数值型，则不进行合计。
				if (typeof(this.rs_main.Fields(Column).value) != "number")
				{	this.rs_main.MoveNext()
					continue
				}
				//进行合计
				if (this.rs_main.Fields(Column).value != null)
					this.SumArr[Column] += parseFloat(this.rs_main.Fields(Column).value)
				this.rs_main.MoveNext()
			}	
			
			this.SumArr[Column] = Math.round(this.SumArr[Column] * Math.pow(10,this.SumDot)) / Math.pow(10,this.SumDot)
		}
	}
}


/*
功能  ：设定列外观属性。
作者  ：李翼嵩
时间  ：2000年8月
参数  ：ColNum -- 列字段编号（base 0）
        strStyle -- 样式单字串。（如："Width:80; color:red"）
返回值：
*/
function ColumnStyle(ColNum,strStyle)
{
	
	this.strColumn += "   ." + this.Name + ColNum + "{" + strStyle + "}    " 
}

/*
功能  ：设定表外观属性。
作者  ：李翼嵩
时间  ：2000年8月
参数  ：strStyle -- 样式单字串。（如："Width:80; color:red"）
返回值：
*/
function TableStyle(strStyle)
{
	
	this.strTable = ' Style="' + strStyle + ' "' 
}

/*
功能  ：设定标题外观属性。
作者  ：李翼嵩
时间  ：2000年8月
参数  ：strStyle -- 样式单字串。（如："Width:80; color:red"）
返回值：
*/
function HeadStyle(strStyle)
{
	
	this.strHead =  strStyle  
}


/*
功能  ：进行包含显示表格的HTML字串的生成。
作者  ：李翼嵩
时间  ：2000年8月
参数  ：
返回值：表格HTML字串。
*/
function Display(strJudge,NA,noAdd)
{
	//NewPage(this.WhichPage,null,true)
	//将所有字段都不显示，只显示该页字段
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
	//******************************************** 常量定义 ***********************************
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
	//是否显示标题。
	if (this.Title)
	{	//建立表头
		sSort = ""
		sUse[sUse.length] = TABLE_HEAD_H + HEAD_ROW_BEGIN 
		//建立表的标题头
		for (field=0; field<this.Widths.length; field++)
		{
			//设置显示字段
			if (this.HideColumn.length > 0)
			{
				if (this.HideColumn[field] != true)
					sDisplay = " style='display:none'"
				else
					sDisplay = ""
			}
			
			//是否指定字段宽度
			iWidth = this.Widths[field] - 5
			sWidth = HEAD_BODY_BEGIN.replace("<TD","<TD width=" + iWidth  + "px id=" + this.Name + "H" + field + sDisplay)
			sUse[sUse.length] = sWidth
			if (this.UseSort == "")
				//不指定排序
				sUse[sUse.length] = this.rs_main.Fields(field).Name
			else
			{	//指定排序
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
		
		//滚动条之上
		sUse[sUse.length] =  "<TH width=13 " + this.strHead + "></TH>"
		//建立纪录头。
		sUse[sUse.length] =  HEAD_ROW_END + TABLE_TAIL_H
	}

	//处理表格
	//从起始行开始安排lines行纪录
	if (this.rs_main.RecordCount > 0 )
	{
		this.rs_main.MoveFirst()
	}
	sBody[0] = TABLE_HEAD
	sStyle = ""
	var sBackC = ""
	var jj = 0
	for (j = 0; j<this.rs_main.RecordCount; j++)
	{	//若结尾，则跳出循环。
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

		//设置背景色
		sBackC = " Style='background-color:" + this.arrColor[jj % 2] + "; height:20px; color:black; word-break:break-all'"
		sBody[sBody.length] =  ROW_BEGIN.replace("rownum", ""+ jj + sBackC)
		jj += 1
		//安排记录中的各个字段。
		for (field=0; field<this.Widths.length; field++)
		{	//是否指定宽度。

			//设置显示字段
			if (this.HideColumn.length > 0)
			{
				if (this.HideColumn[field] != true)
					sDisplay = " style='display:none'"
				else
					sDisplay = ""
			}
			
			sWidth = CELL_BEGIN.replace("FFIIEE","" + field + sDisplay)
			
			sBody[sBody.length] = sWidth
			if (this.rs_main.Fields(field).Name == "序号")
			{	sBody[sBody.length] =  jj + ""
				this.rs_main.Fields(field).Value = jj + ""
			}
			else
				sBody[sBody.length] =  this.rs_main.Fields(field).Value
			sBody[sBody.length] = CELL_END
		}
		
		//记录结束。
		sBody[sBody.length] =  ROW_END
		//下一条记录。
		this.rs_main.MoveNext()
	}
	
	
	//增加一个空行以备将来增加新行。
	//设置背景色
	sBackC = " Style='background-color:" + this.arrColor[jj % 2] + "; height:20px; color:black; word-break:break-all'"	
	sBackC =  ROW_BEGIN.replace("rownum", ""+ jj + sBackC)
	jj += 1
	sBody[sBody.length] =  sBackC.replace("TableRow'","TableRow' id=" + this.Name + "AddRow")
	for (field=0; field<this.Widths.length; field++)
	{	
		//设置显示字段
		if (this.HideColumn.length > 0)
		{
			if (this.HideColumn[field] != true)
				sDisplay = " style='display:none'"
			else
				sDisplay = ""
		}
	
		//是否指定宽度。
		iWidth = this.Widths[field] - 5
		//sStyle = sStyle + "  " + "." + this.Name + field + "{width:" + iWidth + " }"
		sWidth = CELL_BEGIN.replace("FFIIEE","" + field + sDisplay + " name='Virtul'  width=" + iWidth +  " id=" + this.Name + "Add onclick='return " + this.Name + ".AddEvent()'")
		sBody[sBody.length] =  sWidth
		sBody[sBody.length] = CELL_END
	}

	//增加空行,填充空白区
	for(var bl = jj ; bl < this.Height / 20 + 1; bl++)
	{
		sBackC = " Style='background-color:" + this.arrColor[bl % 2] + "; height:20px; color:black; word-break:break-all'"			//设置显示字段
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
	
			//是否指定宽度。
			iWidth = this.Widths[field] - 5
			//sStyle = sStyle + "  " + "." + this.Name + field + "{width:" + iWidth + " }"
			sWidth = CELL_BEGIN.replace("FFIIEE","" + field + sDisplay + " name='Virtul'  width=" + iWidth)
			sBody[sBody.length] =  sWidth
			sBody[sBody.length] = CELL_END
		}	
	}
	
	//记录结束。
	sBody[sBody.length] =  ROW_END
	sBody[sBody.length] =  TABLE_TAIL
	sWidth = sBody.join("")
	//是否指定高度。若指定，则会自动产生滚动条。
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

	//（表尾）是否显示汇总
	//是否显示汇总
	if (this.AskSum)
	{
		
	//建立表尾
		sUse[sUse.length] = TABLE_HEAD_T + HEAD_ROW_BEGIN.replace("<TR ","<TR id=" + this.Name + "Sums ")
		//建立表的标题头
		for (field=0; field<this.Widths.length; field++)
		{	
			//设置显示字段
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
		
		
			//是否指定字段宽度
			iWidth = this.Widths[field] - 5
			sWidth = TAIL_BODY_BEGIN.replace("<TD","<TD width=" + iWidth  + "px id=" + this.Name + "T" + field + sDisplay)
			sUse[sUse.length] = sWidth
			
			if (field==0)
				sUse[sUse.length] = "合计"
			else
				if (this.SumArr[field] != null)
					sUse[sUse.length] = this.SumArr[field]
				else
					sUse[sUse.length] = "　"
			
			sUse[sUse.length] = TAIL_BODY_END
		}
		sUse[sUse.length] = "<TD width=13 " + this.strHead + "></TD>"
		sUse[sUse.length] = HEAD_ROW_END + TABLE_TAIL_H
	}
		
	
	
	//返回表格的HTML字符串。
	sUse[sUse.length] = TABLE_END
	
	//是否增加页选择
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
		//是否显示记录数
		var sCount = ""
		if (this.ShowCount == true)
			sCount = "记录数：" + this.rs_main.RecordCount
			
		//是否显示自由选择字段
		var sFree = ""
		if (this.StrFree != "")
		{
			sFree = SET_BUTTON
			sHide = FREE_BUTTON + sHide
		}
		
		sTemp = "<TABLE width=" + this.TableWidth + "><TR><TD align=left id=" + this.Name + "Count style='font-family:宋体;font-size:9pt'>" + sCount + "</TD><TD align=middle>" + sFree + "</TD><TD align=right>" + sHide + "</TD></TR></TABLE>" + sTemp
	}
	//设置列格式
	sTemp = sTemp + "   " + "<Style> <!--" + this.strColumn + sStyle + "--> </style>"

	//不知为什么，空记录集无法在运行时AddNew，只好出此下策。--- 1
	if (this.rs_main.RecordCount < 1 && noAdd != true)
	{	
		this.rs_main.AddNew()
		this.rs_main(0) = "UndEfinedp"
		
		this.IsEmpty = true
	}
	else
		this.IsEmpty = false

	//重新排序后，清除高亮显示行	
	this.preElement = null
	this.PreRow = -1
	this.currentRow = -1;
	this.RowStr = ""

	return sTemp

}



/* 
功能：  刷新背景色。
作者：  李翼嵩
日期：  2000.7
返回值：返回鼠标处的整条记录内容。字段与字段间用空格分开。
*/
function BackRefresh()
{
	var i,j,oRows, oNode, iTemp
	//清除冗余记录
	this.ClearV()
	
	//是否分页显示
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
	//判断表是否够高度
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
			oTemp.innerText = "　"
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

	
	//全部行
	eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild.childNodes")
	for (i=0; i<oRows.length; i++)
	{
			//杂背景色
			oRows(i).style.backgroundColor = this.arrColor[i % 2]
			oRows(i).style.color = "black"
			oRows(i).style.height = "20px"
			//安排行号
			//if (oRows(i).classid == "TableRow")
			oRows(i).name = i
	}
}



/* 
功能：  从表格中删除选中的行。
作者：  李翼嵩
日期：  2000.7
*/
function DelRow()
{
	var oRows
	//是否有选中的行
	if (this.preElement != null)
		if (this.preElement.firstChild.name != "Virtul")
		{
			//删除该行
			eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild.removeChild(this.preElement)")
			this.rs_main.MoveFirst()
			this.rs_main.Move(this.PreRow)
			this.rs_main.Fields(0).value = "UndEfinedp"		
			this.rs_main.Delete(1)
			//this.rs_main.MoveFirst()
			//this.rs_main.Update()
			//选中标志置空
			this.preElement = null
			this.PreRow = -1
			this.currentRow	= -1
			this.RowStr = ""

			//刷新背景
			this.BackRefresh()
			
			this.ClearV()

		}
}


/* 
功能：  设置选种行的内容。
作者：  李翼嵩
日期：  2000.7
参数：  Content -- 该行内容，以","分隔。
*/
function SetRow(Content,NoSet)
{
	var oNodes,i,strTD
	if (this.preElement == null | this.preElement.firstChild.name == "Virtul")
		return null
	//生成数组
	strTD = Content.split(this.Divide)
	//得到所有单元格。
	oNodes = this.preElement.childNodes
	this.rs_main.MoveFirst()
	this.rs_main.Move(this.PreRow)
	//分别赋值
	for (i=0;i<oNodes.length;i++)
	{
		//若赋值字段已赋完，则退出
		if ( i > strTD.length)
			break
		
		//当不是插入控件时，才赋值
		if (this.seedControl[i] == null || this.modi[i] == true)
		{	
			if (NoSet == i)
				continue;
			//验证是否为数值
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
功能：  调用者添加一行。
作者：  李翼嵩
日期：  2000.7
参数：  Content -- 该行内容，以","分隔。
*/

function Append(Content)
{	var oNode,i,strTD,oNodes
	strTD = Content.split(this.Divide)
	//添加一行
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
功能：  调用者手动，或点击表格自动添加一行。
作者：  李翼嵩
日期：  2000.7
参数：  bAppend -- 是否手动
		Cells   -- 单元内容数组
*/
function AddEvent(bAppend,Cells)
{
	if (bAppend !=true & this.AddDelete!=true )
		return null
	//插入一行记录
	var oNode, oRows, strCells, fields, iRows, oTemp
	//this.ClearV()
	//window.alert(this.rs_main.RecordCount)
	
	this.rs_main.Addnew()
	this.rs_main.Update()
	eval("iRows = " + this.Name + "TableSpan.firstChild.firstChild.childNodes.length ")
	//生成行元素
	oNode = document.createElement("TR");
	oNode.classid = this.Name + "TableRow"
	oNode.align = "left"
	oNode.bordercolor = "Silver"
	oNode.name = "" + iRows
	oNode.style.wordBreak = "break-all"
	//生成行内单元格元素
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

	//得到表格
	eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild") //.childNodes
	
	
	oTemp = oRows.lastChild
	if (oTemp.id == this.Name + "BlankBack")
		//若有用于填充背景的空行，则删除该行
		oRows.removeChild(oTemp);

	//在对的位置添加该行。
	//由表格自动添加
	eval("oRows.insertBefore(oNode," + this.Name + "AddRow)")

	//刷新背景
	this.BackRefresh();
	//模拟鼠标点击，高亮该行。
	this.currentRow += 5                  
	this.MouseDown_Event(oNode.firstChild,true)

	this.ClearV()

}


/* 
功能：  用于反映MouseDown事件。当该事件发生在记录条目上时，
        该条目高亮，尔原高亮条目正常。
作者：  李翼嵩
日期：  2000.7
参数：  Source -- 设置节点
        Direct -- 不恢复颜色
返回值：返回鼠标处的整条记录内容。字段与字段间用空格分开。

*/
function MouseDown_Event(Source,Direct,keyControl)
{
	var elerow,strRe
	//事件源
		if (Source == null)
		{
			elerow = window.event.srcElement;
		}
		else
			elerow = Source
						
		//若为右键则退出
	//	if (window.event.button == 2)
	//		return "nothing";
		//向上追溯事件的父对象，是否为记录行对象。
		if (elerow.parentElement.parentElement != null)
			elerow = elerow.parentElement;
		if (elerow.parentElement.classid == this.Name + "TableRow")
		{
			elerow = elerow.parentElement;
		}

    //若是记录行则响应该事件
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
		//若不是第一次响应该事件，恢复上一次的正常背景色。
		if (this.preElement != null && Direct != true)
		{	this.preElement.style.backgroundColor = this.preColor;
			this.preElement.style.color = "black";
		}
		//保存背景色，以备恢复。
		this.preColor = elerow.style.backgroundColor;
		//指定新的背景色。
		elerow.style.backgroundColor = "midnightblue";
		elerow.style.color = "white"
		//返回该记录内容。	


		this.RowStr = this.EditRow(elerow.name)

		this.currentRow = elerow.name
		//保存该行，以备恢复。
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
功能  ：当一行失去焦点时，保存界面内容，保存记录。
作者  ：李翼嵩
时间  ：2000年8月
参数  ：
返回值：
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
//保存界面内容
	for (i=0; i<this.Widths.length; i++)
	{
		if (this.PreRow != -1)
		{	

			//取消上一次的黑线。
			//this.preElement.childNodes(i).style.borderTop=""
			//this.preElement.childNodes(i).style.borderBottom=""

			//将前一个编辑框的内容保留到界面行内
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
							//保存记录集内容
							//this.rs_main.Fields(i).value = strTemp 
							//continue
						}
						else
							strTemp = parseFloat(strTemp)
					}
					this.preElement.childNodes(i).innerText = strTemp
					this.preElement.childNodes(i).style.padding=2
					//保存记录集内容
					this.rs_main.Fields(i).value = strTemp 
					//strRow += strTemp + this.Divide
					continue

				}
			
			//将前一个控件内容保留到页面上
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
						//保存记录集内容
						this.rs_main.Fields(i).value = strTemp
						//strRow += strTemp + this.Divide
						continue
				}
			
			////将其他内容保存
			//if (this.preElement.childNodes(i).name != "Virtul")
			//{	strTemp = this.preElement.childNodes(i).innerText
				//this.rs_main.Fields(i).value = strTemp
			//	strRow += strTemp + this.Divide
			//}
		}
	}
	//保存记录集内容
	//this.rs_main.update()
	
	//是否显示汇总
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
功能  ：
作者  ：李翼嵩
时间  ：2000年8月
参数  ：
返回值：
*/
function EditRow(Rownum)
{
//outControl 用于外部控制临时改变行显示模式
	var i,j,strTemp,strCats,strStyle,iWidth,strRe,oRows,oParent,ilen
	strRe = ""	//本行字串
	eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild.childNodes(Rownum).childNodes")
	
/*	var dat
	var s1
	var s2
	dat = new Date()
	s1 = dat.getTime()
*/


	//若点击并非当前行，进行处理
	if (this.currentRow != Rownum)
	{	
		if (this.PreRow != -1)
			this.SaveRow()
	
		//对所有列扫描		
		for (i=0; i<this.Widths.length; i++)
		{
			//上下划黑实线
			//oRows(i).style.borderTop="1 solid #000000"
			//oRows(i).style.borderBottom="1 solid #000000"
			
			strRe += oRows(i).innerText
			if (i < this.Widths.length -1 && oRows(i).name != "Virtul")
				strRe += this.Divide
			
			//若次列可编辑，为输入框。
			if (this.modi != null)
			{
				//安排新的编辑框
				if (this.modi[i] && oRows(i).name != "Virtul")
				{	//编辑框宽度
					if (this.seedControl[i] != null & oRows(i).name != "Virtul")
						iWidth = this.Widths[i] - 28
					else
						iWidth = this.Widths[i] - 1
					//编辑框缺省内容
					strTemp = "'" + oRows(i).innerText + "'"
					
					//编辑框风格
					strStyle = ' Style="WIDTH:' + iWidth + 'px; border-bottom:1 solid black;"'
					//是否为数值
					if (this.FieldsType[i] == "num")
						strStyle += " onKeyPress='DealNumberPress()'"
					//缺省值
					strStyle += " value=" + strTemp  
					//是否指定最大长度
					if (this.MaxSize[i] != null)
						strStyle += " maxlength=" + this.MaxSize[i] 
					//当前格<TD></TD>中，插入编辑框HTML文档。
					oRows(i).innerHTML = "<input type=Text id=" + this.Name + "Text" + i + strStyle + " >"
					oRows(i).style.padding = 0
				}
			}		

			//生成控件框	
			if 	(i < this.seedControl.length)
			if (this.seedControl[i] != null  && oRows(i).name != "Virtul")
			{
				//安排新的选择框,当前格<TD></TD>中，插入选择框HTML文档。
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
	//将当前行设为前一行，以备下一次比较。
	this.PreRow = Rownum;
	if (strRe.charAt(0) == "")
		return null
	return strRe
}

/*
功能  ：得到当前行的索引值。
作者  ：李翼嵩
时间  ：2000年8月
参数  ：
返回值：索引值。
*/
function GetRownum()
{
	return this.currentRow * 1;
}

/*
功能  ：刷新表格，清除高亮。
作者  ：李翼嵩
时间  ：2000年8月
参数  ：
返回值：
*/
function Refresh() {
	var strTemp
	//自动删除
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
	
	//若不是第一次响应该事件，恢复上一次的正常背景色。
		
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
				
	//若为右键则退出
	//向上追溯事件的父对象，是否为记录行对象。
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
	//不知为什么，空记录集无法在运行时AddNew，只好出此下策。--- 1
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
		eval(this.Name + "Count.innerText = '记录数：" + this.rs_main.RecordCount + "'")
	
	//}

}

function DealNumberPress()
{
 var charCode=(navigator.appName =="NetScape")?event.Which: event.keyCode;

//只能出现数字和小数点。
 if (charCode>31 && (charCode<48 ||charCode>57) && charCode != 46)
   	event.returnValue =false;

//不能同时出现两个小数点。
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
		case 37:	//左
		
			break;
		
		case 38:	//上
			if (this.currentRow > 0)
			{	iRow -= 1
				this.HighLightRow(iRow)
				Tag.focus()
				eval(this.UpDown)
			}
			event.returnValue =false;
			break;
		
		case 39:	//右
		
			break;
		case 13:	//回车
			eval(this.Dbclick)
		
			break;
			
		case 40:	//下
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


