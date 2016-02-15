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
	this.LightBKColor = "midnightblue" //高量行背景色
	this.LightColor = "white"    //高量行字体颜色
	this.TableLineColor = "black"        //表格格线颜色


	this.UseSort = ""				//指定排序的TDC控件
	this.IsEmpty = false			//判断是否当前表格是否为空
	this.Divide = "\u001C"				//使用的分隔符
	this.outerSelect = false		//使用外部选择SELECT标签，将选择内容放在格中。

//public Method
	this.Display = _Display;			//得到HTML字串。
	this.MouseDown_Event = _MouseDown_Event //鼠标事件
	this.ManualMouseUp = _MouseDown_Event	//鼠标事件
	this.TDMouseDown = _TDMouseDown
	this.AddEvent = _AddEvent;		//增加一条记录。
	this.GetRowNumber = _GetRownum	//得到当前行索引值。
	this.Refresh = _Refresh			//刷新表格。
	this.ColumnStyle = _ColumnStyle	//设定一列外观属性。
	this.TableStyle = _TableStyle	//设定表外观属性。
	this.HeadStyle = _HeadStyle		//设定标题外观属性。
	this.Delete = _DelRow			//删除当前行
	this.Append = _Append			//增加一行（程序手动方式）
	this.SetRow = _SetRow			//设置一行内容
	this.SetCell = _SetCell			//设置当前高亮行中一列的内容
	this.UseControl = _UseControl	//使用用户指定的插入控件。
	this.ControlReturn = _ControlReturn //设置用户插入控件所在格的内容。
	this.PageColumn = _PageColumn    //设置横向分页功能
	this.HideColumn = new Array()			//隐藏指定的字段
	this.seedControl = new Array()
	this.ControlValue = new Array()
	this.FieldsType = new Array()
	this.MaxSize = new Array()
	this.EventRow = _EventRow
	this.ClearV = _ClearV

//private property and method
	this.Name = ObjectName;			//生成对象的名称。
    this.PreRow = -1;				//高亮前一行记录。
	this.strColumn = ""				//列属性字串。
	this.strTable = 'Style="color:black;font-size:9pt"'	//表属性字串。
	this.strHead = "font-size:10pt"				//标题属性字串。
	this.AskSum = false;					//是否增加“合计“
	this.SumArr = new Array(RecordSet.Fields.Count);	//合计字段合计值。
	this.SumDot = 2
	this.EditRow = _EditRow;			//对当前行进行可编辑处理。
	this.UseSum = _UseSum;			//增加合计字段，可重复调用。
	this.currentRow = -1;			//记录目前行索引值。
    this.preElement = null;			//保存当前行，当其它行点击高亮时恢复该行
    this.preColor = null;			//保存当前行颜色。
	this.BackRefresh = _BackRefresh  //刷新背景色
	this.SaveRow = _SaveRow			//保存高亮行的内容到记录集。
	this.Sort = _Sort
	this.UseForm = ""
	this.Pages = 1
	this.PageArr = new Array()
	this.NewPage = _NewPage
	this.WhichPage = 0
	this.SortColumn = -1			//指定由哪个字段排序
	this.SortAsc = ""				//排序方法：""为正序，"-"为逆序。
	this.HighLightRow = _HighLightRow
	this.DealKeyPress = _DealKeyPress
	this.DealBack = _DealBack
	//this.UnderKey = false
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

	this.Filter = _FilterReset	//使用过滤的方法 “字段名=筛选字串”。注意：使用该方法前一定要使用UseSort属性。
	this.strFilter = ""    // 外部的过滤条件this.rs_main.Fields(0).Name + " <> -1.0249E5"

	this.ControlPath = "../../../commoncontrol/"	//所用图片和对话框的相对路径
	this.DownEventSrc = null  //event.srcElement
	this.NoKey = false			//true -- 不支持键盘控制上下
	this.BeforeUpDown = null	//高亮行上下移动之前调用的函数
	this.UpDown = null			//高亮行上下移动时调用的函数
	this.BeforeChange = null
	this.OuterControl = -1		//指定哪一列控件替换当前文本
	this.OuterControl1 = -1		//指定哪一列控件替换当前文本
	this.OuterControl2 = -1		//指定哪一列控件替换当前文本
	this.OuterControl3 = -1		//指定哪一列控件替换当前文本
	this.OuterControl4 = -1		//指定哪一列控件替换当前文本
	this.SetZ = true			//当数值型为空或非法时是否将该字段置为0
	this.ShowZ = true			//当数值型为0时是否显示
	this.GetFirstEnableChild = _GetFirstEnableChild
	this.NoFocus = false	//高亮某行时，不置焦点


	this.BeforeHighLight=null	//高亮行之前事件
	this.BeforeSave = null		//换高亮行之前事件
	this.BeforeNew = null		//换高亮行之前事件
	this.AfterNew = null		//换高亮行之后事件
	this.AfterChange = null		//高亮行变换只后事件，包括滚动产生的变换。
	this.AfterNewPage = null	//新换一页事件

	this.DefaultSort = -1		//缺省序号

	this.DHContent = new Array()	//合并内容总称 ("","合并1","","","","合并2","","")	//双层表格的内容。
	this.DHSpan = new Array()		//合并跨度 (1,2,2,1,1,2,2,1)		//双层表格的跨度。
	this.DHSignal = new Array()		//合并字段标记。(0,1,1,0,0,1,1,0)		//双层表格指示，该列是否为双层。

	this.UseCombin = _UseCombin		//使用合并功能
	this.ChangeK = _ChangeK			//转义功能，将“<..>”转成“&lt;...”


	//增加不同的语言版本
	//aa = bb.sub
	if (document.charset == "gb2312")
	{

		this.JILUSHU = "记录数： "
		this.HEJI = "合计 "
		this.KONG = "　"
		this.NOSORT = "数据已被修改，请先保存数据！ "
	}
	else if (document.charset == "big5")
	{
		this.JILUSHU = "癘魁计 "
		this.HEJI = "璸 "
		this.KONG = ""
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

	this.OverFlow = "auto"	//滚动设定 "scroll" "auto"
	this.MaxLine = 99999
	this.PreLine = -1
	this.GetOrder = new Array()
	this.SetOrder = _SetOrder

	for (var i=0; i<this.rs_main.Fields.Count; i++)
	{
		this.GetOrder[i] = i

	}

	this.UseHTML = false		//是否使用外部指定的HTML
	this.ReversSort = false		//是否改变字段显示顺序
	this.FastField = -2
	this.preFastElement = null
	this.DisplayFast = _DisplayFast	//显示固定字段
	this.DisplayM = _DisplayM
	this.BodyHTML = _BodyHTML
	this.HeadHTML = _HeadHTML

	this.UseSearch = ""				//是否使用快速查询,""——不使用，"All"——所有字段，"Page"——当前页, "AllLine"——有格线,"PageLine"——有格线
	this.SearchItem = null;       //决定查询的条件是Text还是Select
	this.SearchHTML = "<Input id=" + this.Name + "SearchValue style='width:100;height:20' value=''>"
	this.SearchFilter = _SearchFilter	//得到过滤字串
	this.BeforeSearch = null		//快速查询之前事件
	this.strSearch = ""				//当前查询条件
	this.AdjustLine = 0				//调节使用快速查询时产生的误差


	this.CellColorArr = new Array()	//指定单元格样式的数组
	this.CellColor = _CellColor		//指定单元格样式
	this.CellColorColumn = -1		//指定单元格样式的关键字列
	this.ChangeHead = null			//设定后储发事件
	this.ToUnicode = _ToUnicode		//转换成unicode
	this.ReplaceChar = _ReplaceChar	//替换字符，如将'替换成 \\\'
	this.ToRe = _ToRe


	this.SetWidth = _SetWidth
	this.ScreenWidth = 800
	this.ScreenHeight = 600
	this.adjustRateW = 1.011
	this.adjustRateH = 1.082

	this.checkBoxColumn = -1	//该属性代表需要使用CheckBox的栏位编号0~n ，该栏位的值若是checked则表示被选中。
	this.checkBoxStyle = ""
	this.checkAll = _CheckAll	//checkBox全选方法
	this.checkItem = _CheckItem	//checkBox选择方法

	this.arrDelLog = new Array()		//表格内容增加、修改、删除的纪录，0——删除 1——增加 2——删除 数组的索引值同时代表记录集索引。
	this.arrModiLog = new Array()		//表格内容增加、修改、删除的纪录，0——删除 1——增加 2——删除 数组的索引值同时代表记录集索引。
	this.arrSortModi = null;
	this.DealLogWhenDel = _DealLogWhenDel		//当从记录集中删除一条记录，后面的记录索引值改变，需要调整arrModiLog中的索引。ModiLogDel
	this.GetAMDString = _RecordsetToString  //得到增加(A)、编辑(M)、删除字符串(D)。
	this.ClearAMD = _ClearAMD		//清空增加(A)、编辑(M)、删除字符串(D)的临时内容
	this.Clear = _Clear			//清空表格内容。
	this.canKeyDelete = false
	this.MaxSortNumber = 0
	this.Calculator= new Array()		//自动计算公式数组，可以实现指定列的自动计算。公式采用“<列号>=计算公式”计算公式可以是任意形势的表达式,表达式中可以包括<列号>
	this.CalculateColumn = _CalculateColumn	//对指定的列实现自动计算
	this.FindSameRow = _FindSameRow		//发现在当前表格中，是否存在相同的内容
	this.BeforeSortModi = _BeforeSortModi	//排序或过滤前，需要保存更改增加的数组arrModi
	this.AfterSortModi = _AfterSortModi	//排序或过滤后，更新更改增加的数组arrModi
	this.CanNotChangRow = false;         // 屏蔽换行的操作
	this.SearchValue = _SearchValue
	this.SetSelect = _SetSelect
	this.dataurl = ""       //保存TDC控件的dataURL，已备判断是否重新取得数据了。
	this.CalculatorAll = _CalculatorAll //对所有的自动计算栏位进行计算
	this.scrollTop = 0;

}

function _CalculatorAll()
{

		if (this.rs_main.RecordCount > 0  && this.Calculator.length>0)
		{
			var tempPosition = this.rs_main.absolutePosition;
			this.rs_main.MoveFirst()
			//依次进行合计
			while (!this.rs_main.EOF)
			{
				//若该字段不是数值型，则不进行合计。
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
            //第一次使用，拼装Select内容
            for (var iCol=0; iCol<this.SearchItem.length; iCol++)  {
                if (this.SearchItem[iCol] == "select")
                 {
                    var arrOption = new Array();
                    this.rs_main.moveFirst();

                    while (!this.rs_main.EOF && !this.IsEmpty) {
                        arrOption["<option value=\"" + this.ChangeK(this.rs_main.fields(iCol).value)+ "\">"+  this.rs_main.fields(iCol).value] = "";
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
        //根据所选生成Select或者Input内容
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
    //以下处理重新Display后，快速查询是否需要重新设置。
    var oNode = document.getElementById(this.UseSort)

    if (oNode != null) {
        if (this.dataurl != "" &&  this.dataurl != oNode.DataURL  &&  this.SearchItem != null) {
        //重新安排快速查询的select内容
            for (var iCol=0; iCol<this.SearchItem.length; iCol++ ) {
                if (this.SearchItem[iCol] != "")
                    this.SearchItem[iCol] = "select"
            }
        }

        this.dataurl =  oNode.DataURL;

    }
    //以下处理排序后，重新布置AMD标记数组
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
/*
发现在当前表格中，是否存在相同的内容
如下：第1，2，3列是否存在值为s001,aaa,ccc的行
columns:"1,2,3"
values: "s001,aaa,ccc"
isCur: 是否排除对当前行比较 true 排除 false 比较
返回值：存在true 不存在false
*/
function _FindSameRow(columns,values,isCur)
{
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

/*
对指定的列实现自动计算，不需要外部调用，只要设定calculator属性既可
calculator属性设定方法如下：
？？？？.Calculator= new Array("<3>=<2>!=''?0:<5>*<6>","<4>=<2>==''?0:<5>*<6>","<9>=<0>*<1>")

*/
function _CalculateColumn()
{

	for (var key in this.Calculator)
	{
	   var 	strCalculate = this.Calculator[key]
       for (var i=0; i<this.Widths.length; i++)
       {    if (this.FieldsType[i] == "num")  {
                if (isNaN(parseFloat(this.rs_main.fields(i).value)))
                    this.rs_main.fields(i).value = 0;
            }
       }

  	   strCalculate = strCalculate.replace(/\</g, this.Name + ".rs_main.fields(");
   	   strCalculate = strCalculate.replace(/\>/g, ")");

	   strCalculate = strCalculate.replace("=", "=Math.round((") + ")* Math.pow(10,this.SumDot)) / Math.pow(10,this.SumDot)"
	   eval(strCalculate);	
	}	
}	

//checkBox全选方法
function _CheckAll()
{
	if (this.rs_main.recordCount <= 0)
		return 0;

	this.rs_main.moveFirst()
	if (event.srcElement.checked)
	{	//选中状态。
		for (var i = 0; i< this.rs_main.recordCount; i++)
		{	this.rs_main.fields(this.checkBoxColumn).value = "checked"
			this.rs_main.moveNext()
		}
	}
	else
	{	//未选中状态。
		for (var i = 0; i< this.rs_main.recordCount; i++)
		{	this.rs_main.fields(this.checkBoxColumn).value = ""
			this.rs_main.moveNext()
		}
	}

	this.ChangePage()

}

//checkBox选择方法
function _CheckItem()
{
   	var rownum = event.srcElement.parentElement.parentElement.name;
   	
   	this.rs_main.absolutePosition = rownum*1 + 1


	if (event.srcElement.checked)
	{	//选中状态。
		this.rs_main.fields(this.checkBoxColumn).value = "checked"
	}
	else
	{	//未选中状态。
		this.rs_main.fields(this.checkBoxColumn).value = ""
	}
	
	if (this.LightRow > -1 && this.LightRow < this.rs_main.recordCount)
	this.rs_main.absolutePosition = this.LightRow*1 + 1

}


//得到过滤字串
function _SearchFilter(isAll)
{
	//eval(this.Name + "VirtualFocus.focus()")


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
		else if (oNode.tagName == "SELECT" || this.FieldsType[iIndex] == "num")
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


//设定顺序函数
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

/*
功能  ：将特殊HTML字符转化成&???字符。
作者  ：李翼嵩
时间  ：2001年8月
参数  ：key--待转化的字串
*/
function _ChangeK(key)
{

   key = key + ""

   if (	this.UseHTML == true)
		return key;

    key = key.replace(/&/g,"&amp;")
	key = key.replace(/</g, "&lt;")
    key = key.replace(/>/g, "&gt;")
    key = key.replace(/  /g, " &nbsp;")
    key = key.replace(/"/g, "&quot;")



   return key;
}


/*
功能  ：使用合并功能，。
作者  ：李翼嵩
时间  ：2001年8月
参数  ：Columns--列的字串，如"3,4,5"
		CombinName -- 合并后的总称。
*/

function _UseCombin(Columns,CombinName)
{
	var i,j, arrCol
	//i = bb.suhb()
	if (Columns == null || CombinName == null)
		return false
	//初始化数组
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
	//检查合法性
	for (i=0;i<arrCol.length;i++)
	{
		if (this.DHSpan[arrCol[i]*1] != 1)
			return false

		if (this.DHSignal[arrCol[i]*1] == 1)
			return false

		if (arrCol[i]*1 >= this.Widths.length)
			return false
	}

	//为合并内容赋值。
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

	var HEAD_ROW_BEGIN = "<TR style='word-break:break-all;border-color:black' name=Head bgcolor=#cebe9c align=center>"
	var HEAD_ROW_END = "</TR>"

	var HEAD_BODY_BEGIN = "<TD " + "Style='border-right:1px solid black;" + this.strHead +  "' ><b>"
	var DHHEAD_BODY_BEGIN = "<TD " + "Style='border-right:1px solid black;border-bottom:1px solid black;" + this.strHead +  "' ><b>"

	var HEAD_BODY_END = "</TD>"



	if (this.DHSpan.length > 0)
		sRowCol = " rowspan=2 "

		sUse[sUse.length] = TABLE_HEAD_H + HEAD_ROW_BEGIN

		//建立表的标题头
		for (var field=0; field<this.Widths.length; field++)
		{
			//是否指定字段宽度
			iWidth = this.Widths[this.GetOrder[field]] - 5
			//设置显示字段
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
				//不指定排序
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
			{	//指定排序
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

		//滚动条之上
		sUse[sUse.length] =  "<TH " + sRowCol + "id='aa11' width=13 style='" + this.strHead + "'></TH>"


		//建立纪录头。
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

	//************************************************************************************************

	var sStyle = ""
	var sBackC = ""
	var sBody = new Array()
	var jj = 0
	var iWidth = 0
	var sDisplay = ""
	var sColor = ""
	//处理表格
	//从起始行开始安排lines行纪录
	if (this.rs_main.RecordCount > iRow )
	{
		this.rs_main.absolutePosition = iRow+1
	}
	sBody[0] = TABLE_HEAD

	for (var j = iRow; j<this.rs_main.RecordCount; j++)
	{
		//若结尾，则跳出循环。
		if (this.rs_main.EOF)
			break

		if (this.rs_main.Fields(0).value == "-1.0249E5")
		{	this.rs_main.MoveNext()
			continue
		}

		//若超出高度，则跳出循环
		if ((j-iRow) > this.Height / 20)
			break
		//自动计算栏位值
		this.CalculateColumn();
		//设置背景色
		if (this.rs_main.Fields(this.GetOrder[this.Widths.length-1]).Name == "CC_颜色" && this.rs_main.Fields(this.GetOrder[this.Widths.length-1]).value != "")
			sBackC = " Style='background-color:" + this.rs_main.Fields(this.GetOrder[this.Widths.length-1]).value + "; height:20px; color:black; word-break:break-all'"
		else
			sBackC = " Style='background-color:" + this.arrColor[jj % 2] + "; height:20px; color:black; word-break:break-all'"


		sBody[sBody.length] =  ROW_BEGIN.replace("rownum", ""+ j + sBackC)
		jj += 1
		//安排记录中的各个字段。
		for (field=0; field<this.Widths.length; field++)
		{	//是否指定宽度。
			if (this.CellColorColumn >= 0)
			{
				sColor = this.CellColorArr["A" + this.rs_main.Fields(this.GetOrder[this.CellColorColumn]).Value + field]
				if (sColor == null)
					sColor = ""
			}
			//设置显示字段
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
			//if (this.rs_main.Fields(field).Name == "序号")
			//{	sBody[sBody.length] =  (jj - 1) + ""
			//	this.rs_main.Fields(field).Value = (jj-1) + ""
			//}
			//else
			if (this.checkBoxColumn == this.GetOrder[field])
			{
				sBody[sBody.length] = "<INPUT TYPE=CHECKBOX NAME=" + this.Name + "cb" + field + " id=" + this.Name + "cb" + field + " onclick='" + this.Name + ".checkItem()' " + this.rs_main.Fields(this.GetOrder[field]).Value + "  " + this.checkBoxStyle + "></input>"
			}
			else
			{

				if (this.SetZ == true || this.rs_main.Fields(this.GetOrder[field]).Value * 1 != 0   || this.FieldsType[this.GetOrder[field]] != "num")
				{
					if (this.ShowZ == true || this.rs_main.Fields(this.GetOrder[field]).Value != 0)
						sBody[sBody.length] =  this.ChangeK(this.rs_main.Fields(this.GetOrder[field]).Value)

				}
			}
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
	sBackC =  ROW_BEGIN.replace("rownum", ""+ (jj*1+iRow*1) + sBackC)
	jj += 1
	sBody[sBody.length] =  sBackC.replace("TableRow'","TableRow' id=" + this.Name + "AddRow" + iFast + "")
	sDisplay = ""
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
		iWidth = this.Widths[this.GetOrder[field]] - 5
		//sStyle = sStyle + "  " + "." + this.Name + field + "{width:" + iWidth + " }"
		//sWidth = CELL_BEGIN.replace("FFIIEE","" + field + sDisplay + " name='Virtul'  width=" + iWidth +  " id=" + this.Name + "Add onclick='return " + this.Name + ".Append()'")
		sWidth = CELL_BEGIN.replace("FFIIEE","" + field + sDisplay + " name='Virtul'  width=" + iWidth +  " id=" + this.Name + "Add" + iFast + "")
		sBody[sBody.length] =  sWidth
		sBody[sBody.length] = CELL_END
	}

	sDisplay = ""
	//增加空行,填充空白区
	for(var bl = jj ; bl < this.Height / 20 + 1; bl++)
	{
		sBackC = " Style='background-color:" + this.arrColor[bl % 2] + "; height:20px; color:black; word-break:break-all'"			//设置显示字段
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

			//是否指定宽度。
			iWidth = this.Widths[this.GetOrder[field]] - 5
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

	//保存背景内容，高亮消失。
	iPreLine = this.PreLine
	iLightRow = this.LightRow
	this.Refresh(true)
	this.LightRow = iLightRow
	this.PreLine = iPreLine
	iLightRow = -1
	//eval("oNode = " + this.Name + "TableSpan.firstChild.firstChild")
	eval(this.Name + "TableSpan.scrollTop = 0")


	oNode = document.all(this.Name + "TableSpan")
	//alert(oNode.firstChild.style.left)
	oNode.firstChild.outerHTML = this.BodyHTML(this.FirstRow,"",oNode.firstChild.style.left)

	oNode2 = document.all(this.Name + "TableSpan" + this.FastField + "K")

	if (oNode2 != null)
		oNode2.firstChild.outerHTML = this.BodyHTML(this.FirstRow,this.FastField,0)
		//oNode = oNode2.firstChild.firstChild





	//固定字段
	//aa = bb.ss
/*
	if (this.rs_main.RecordCount > 0)
	{	this.rs_main.MoveFirst()
		this.rs_main.Move(this.FirstRow)
	}





	for (j = 0; j<=this.Height / 20; j++)
	{	//若结尾，则跳出循环。
		//if (this.rs_main.EOF)
		//	break
		if (j > this.Height / 20)
			break


		if (bAdd == true)
			oNode.childNodes(jj).id = this.Name + "AddRow"
		else
			oNode.childNodes(jj).id = ""

		//安排记录中的各个字段。
		if (this.rs_main.EOF || this.rs_main.RecordCount<1)
		{
			for (field=0; field<this.Widths.length; field++)
			{	//空白区域。
				//oNode.childNodes(jj).childNodes(field).Name = this.Name

				if (this.HideColumn[field] != false)
				{
					oNode.childNodes(jj).childNodes(field).innerText =  ""
					//固定字段
					if (oNode2 != null)
						oNode2.childNodes(jj).childNodes(field).innerText =  ""

					oNode.childNodes(jj).name = "" + (this.FirstRow * 1 + jj * 1)
					if (bAdd == true)
						oNode.childNodes(jj).childNodes(field).id = this.Name + "Add"
					else
						oNode.childNodes(jj).childNodes(field).id = null
				}
				oNode.childNodes(jj).childNodes(field).name = 'Virtul'
			}
			bAdd = false


			oNode.childNodes(j).style.backgroundColor = this.arrColor[j % 2]

		}
		else
		{

			if (this.rs_main.Fields(this.GetOrder[this.Widths.length-1]).Name == "CC_颜色")
			{
				if (this.rs_main.Fields(this.GetOrder[this.Widths.length-1]).value != "")
					oNode.childNodes(j).style.backgroundColor = this.rs_main.Fields(this.GetOrder[this.Widths.length-1]).value
				else
					oNode.childNodes(j).style.backgroundColor = this.arrColor[j % 2]

			}

			for (field=0; field<this.Widths.length; field++)
			{	//是否指定宽度。
				if (this.HideColumn[field] != false)
				{

					if (this.SetZ == true || this.rs_main.Fields(this.GetOrder[field]).Value * 1 != 0)
					{	oNode.childNodes(jj).childNodes(field).innerHTML = this.ChangeK(this.rs_main.Fields(this.GetOrder[field]).Value)
						//固定字段
						if (oNode2 != null)
							oNode2.childNodes(jj).childNodes(field).innerHTML = this.ChangeK(this.rs_main.Fields(this.GetOrder[field]).Value)

					}
					else
					{	oNode.childNodes(jj).childNodes(field).innerHTML =  ""

						//固定字段
						if (oNode2 != null)
							oNode2.childNodes(jj).childNodes(field).innerHTML = ""

					}
					oNode.childNodes(jj).childNodes(field).name = ''
					oNode.childNodes(jj).childNodes(field).id = null
					oNode.childNodes(jj).name = "" + (this.FirstRow * 1 + jj * 1)
				}

				//if (this.rs_main.Fields(field).Name == "序号")
				//	oNode.childNodes(jj).childNodes(field).innerText =  this.rs_main.absolutePosition


			}

			//下一条记录。
			this.rs_main.MoveNext()
			if (this.rs_main.EOF)
				bAdd = true
		}

		//是否在本页存在高亮行。
		if (this.LightRow == this.FirstRow + jj)
			iLightRow = jj

		jj ++

	}
	if (iLightRow >= 0)
	{
		var oNode
		eval("oNode = " + this.Name + "TableSpan.firstChild.firstChild.childNodes(" + iLightRow + ").firstChild")
		alert("Hi")
		alert("Light")
	}

*/

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

	//调整表头
	eval(this.Name + "SpanHead.scrollLeft = oNode.scrollLeft")
	//调整表体
	eval(this.Name + "TableBody.style.left = -oNode.scrollLeft")
	//调整表尾
	if (this.AskSum)
		eval(this.Name + "SpanTail.scrollLeft = oNode.scrollLeft")

	//alert(oNode.scrollTop)
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

	//alert("scrollHeight = " + testMeasure.style.height)
	//alert("thisHeight = " + this.Height)
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
		//高亮某行时，不置焦点
		this.NoFocus = true

		this.ChangePage()
		this.NoFocus = false

		//增加一个输入框，用于获得焦点。否则表格有输入框时，移动滚动条后再按上下键会产生错误。
		eval(this.Name + "VirtualFocus.focus()")

//		if (this.KeyBak == null)
//		{
//			eval("this.KeyBak = " + this.Name + "TableBody.onkeydown")
//			eval(this.Name + "TableBody.onkeydown = null")
//		}
		//eval(this.Name + ".onkeydown = " + this.Name + ".DealKeyPress")
	//oTable.style.top = oNode.scrollTop
	}

	//if (oNode.scrollTop + oNode.offsetHeight >= oNode.scrollHeight)
	//	this.ChangePage(1)

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


function _ShowFree()
{
//alert("ShowFree")
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
//alert("SetCookieTable")
//document.cookie = ""
	this.StrCookie = GetCookieTable(this.StrFree)
	var sFeatures="dialogHeight: " + 360 + "px;dialogWidth: " + 340 + "px;status:no;help:0;";
	var para = this;
	var oNode = null
	var sCookie=window.showModalDialog(this.ControlPath + "SelectShow.html", para, sFeatures)
	if (sCookie != null && sCookie != "null")
	{	SetCookieTable(this.StrFree,sCookie)
		if (sCookie != "")
		{	//显示自定义字段
			var ArrFree = sCookie.substring(0,sCookie.length -2).split(",")
			//aa = bb.sub()
			this.NewPage(0,ArrFree,0)
			//设定字段先后顺序
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
					eval(this.UseForm + this.Name + "0img.src = '" + this.ControlPath + "images/" + "0.gif'")

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
function _FreeSelect(SaveName)
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
function _HighLightRow(RowNum,DownShow)
{

	//RowNum = RowNum - this.FirstRow

	//if (RowNum <0 || RowNum > (this.Height / 20))
	//	return
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
	//if (oNode.offsetTop + oNode.offsetHeight <= 0)
	//	return

	eval("oNode = " + this.Name + "Measure")

	if (oNode.offsetWidth <= this.TableWidth-18)
		iHScroll = 0
	else
		iHScroll = 1


	if (RowNum > this.FirstRow && (RowNum - this.FirstRow) < (this.Height / 20))
	{	//存在改行
		//eval("oNode = " + this.Name + "TableSpan.firstChild.firstChild.childNodes(" + （RowNum - this.FirstRow） + ").firstChild")
		eval("oNode = " + this.Name + "TableSpan.firstChild.firstChild.childNodes(" + (RowNum - this.FirstRow) + ").firstChild")
		if (oNode.parentElement.offsetTop + oNode.parentElement.offsetHeight < this.Height-18*iHScroll)
			//在可见的范围内
			if (DownShow == "AddNew")
				this.ChangePage()
			else
				this.ManualMouseUp(oNode,null,true)
		else
		{	//不在可见的范围内，计算可见位置，重新设定。

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

/*		var iTop
		eval("iTop = " + this.Name + "TableSpan.scrollTop" )

		if (oNode.offsetTop < iTop)
			iTop = oNode.offsetTop
		else if (oNode.offsetTop + oNode.offsetHeight > iTop + this.Height -20)
			iTop = oNode.offsetTop + oNode.offsetHeight - this.Height + 20

		eval(this.Name + "TableSpan.scrollTop = " + iTop)
*/
	}
	else
	{	//不存在该行
		if (DownShow != true)
		{	//高亮显示于首
			this.FirstRow = RowNum
			this.LightRow = RowNum
			this.ChangePage()
		}
		else
		{	//高亮显示于尾
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
	//eval(this.Name + "TableSpan2.scrollTop = " + this.FirstRow * Math.floor(this.Height / 8))
	window.setTimeout('eval("' + this.Name + 'TableSpan2.scrollTop = ' + this.FirstRow * Math.floor(this.Height / 8) + '")',20)

}

/*
功能  ：是否分页显示字段，指定分几页显示。
作者  ：李翼嵩
时间  ：2000年8月
参数  ：Columns 指定排序的字段
*/
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

/*
功能  ：是否分页显示字段。
作者  ：李翼嵩
时间  ：2000年8月
参数  ：PageNum 指定显示第几页
        Free 是否为自由选择页
*/
function _NewPage(PageNum,Free,SetHide)
{
	//得到该页的字段们
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

	//重新设定显示顺序
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

	//由于可能变换次序，重新设置表头。
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

	//将所有字段都不显示，只显示该页字段
	for(var i=0; i<this.Widths.length; i++)
		this.HideColumn[i] = false
	for(var i=0; i<temp.length; i++)
		this.HideColumn[temp[i]] = true

	if (SetHide == true)
		return

	//清除空记录
	this.ClearV()

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
			iTemp += this.Widths[this.GetOrder[n]]
		}
	}


/*	//若有固定字段
	if (this.FastField > 0 && 2>3)
	{
		eval("oRows = " + this.Name + "TableSpan" + this.FastField + "K.firstChild.firstChild.childNodes")

		for (i=0; i<oRows.length; i++)
		{
				for (var n=0; n<this.HideColumn.length; n++)
					if (this.HideColumn[n] != true)
						oRows(i).childNodes(n).style.display = "none"
					else
						oRows(i).childNodes(n).style.display = "block"
		}



		//将须隐藏表头、表尾隐藏。
		for (var n=0; n<this.HideColumn.length; n++)
		{

			if (this.HideColumn[n] != true)
			{
				if (this.Title)
					eval(this.Name + "H" + this.FastField + "" + n + ".style.display = 'none'")
			}
			else
			{
				if (this.Title)
					eval(this.Name + "H" + n + ".style.display = 'block'")
			}
		}

	}
*/

	if (this.HideColumn.length >= this.Widths.length)
	{
		iTemp -= 4
		//此时 iTemp 为所有可见字段宽度和
		if (this.TableWidth+2 >= (iTemp))
		{//不产生横向滚动条
			eval(this.Name + "TableAll.width = " + iTemp)
			eval(this.Name + "TableSpan.style.width = " + (iTemp-18))
			eval(this.Name + "TableSpan.style.height = " + (this.Height-iScrillH))
			eval(this.Name + "TableSpan2.style.width = " + iTemp)

		}
		else
		{	//eval(this.Name + "TableAll.width = " + this.TableWidth)
//			eval(this.Name + "TableSpan.style.width = " + 10)
			eval(this.Name + "TableSpan.style.width = " + (this.TableWidth-18))
			iScrillH = 18
			eval(this.Name + "TableSpan.style.height = " + (this.Height-iScrillH))
			eval(this.Name + "TableSpan2.style.width = " + this.TableWidth)
		}

		eval(this.Name + "TableHead.style.width = " + iTemp)

		if (this.AskSum)
		eval(this.Name + "TableTail.style.width = " + iTemp)
		iTemp -= 18
//		eval(this.Name + "TableBody.style.width = " + 10)
//		eval(this.Name + "TableBody.width = " + 10)

		eval(this.Name + "TableBody.style.width = " + iTemp)
		eval(this.Name + "TableBody.width = " + iTemp)
			//若有固定字段
		//	if (this.FastField > 0)
		//	{
		//		eval(this.Name + "TableBody" + this.FastField + ".style.width = " + iTemp)
		//		eval(this.Name + "TableBody" + this.FastField + ".width = " + iTemp)

		//	}

	}

	//重新设置横向滚动条宽度
	eval(this.Name + "Measure.style.width = " + iTemp)
	eval(this.Name + "TableBody.style.left = 0")
	eval(this.Name + "TableSpan2.scrollLeft = 0")

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


	//更新显示内容
	this.ChangePage()

	//将所有按钮抬起，只将该页按钮下陷
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
				//if (arrFilter[0] == this.rs_main.Fields(ss).name)
				//	sSearch += "<Option selected>" + this.rs_main.Fields(ss).name
				//else
					sSearch += "<Option>" + this.rs_main.Fields(ss).name
			}
		}
		sSearch += "</Select>"
		document.all(this.Name + "Search").outerHTML = sSearch
		document.all(this.Name + "SearchValue").value = ""
	//eval(this.Name + "Search.outerHTML = \"" + sSearch + "\"")
	}


	//触发换页事件
	eval(this.AfterNewPage)
}



/*
功能  ：是否排序。
作者  ：李翼嵩
时间  ：2000年8月
参数  ：Columns 指定排序的字段
*/
function _Sort(column)
{
	//若是序号字段则不排序
	//if (this.rs_main.Fields(column).Name == "序号")
/* 2004-08-10 commen for don't net auto sort, and replace this function with add sort number save to EJB
	if (column == this.DefaultSort)
		return
*/
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
		eval("this.SortLeft = " + this.Name + "TableSpan2.scrollLeft" )
		//刷新界面，以便保存当前行信息
		this.Refresh(true)
		//执行排序，设定TDC控件排序。
		eval(this.UseSort + ".CaseSensitive = 'FALSE'")
		eval(this.UseSort + ".Sort = " + "'" + this.SortAsc + this.rs_main.Fields(column).Name + "'")
		//eval(this.UseSort + ".object.Filter = '" + this.strFilter + "'")
		//eval(this.UseSort + ".object.Filter = '" + this.rs_main.Fields(0).Name + " <>  -1.0249E5" + this.strFilter + "'")
		//eval(this.UseSort + ".DataURL = " + this.UseSort + ".dataURL")

		//eval(this.UseSort + ".Reset()")

		var oNode,iLeft
		eval("oNode = " + this.Name + "TableSpan2")

		iLeft = oNode.scrollLeft

		this.Filter()
		this.FirstRow = 0
		this.LightRow = -1

/*
//排序后保持横向滚动位置

		//调整表头
		eval(this.Name + "SpanHead.scrollLeft = iLeft")
		//调整表体
		eval(this.Name + "TableBody.style.left = -iLeft")
		//调整表尾
		if (this.AskSum)
			eval(this.Name + "SpanTail.scrollLeft = iLeft")
		eval("oNode = " + this.Name + "TableSpan2")
		oNode.scrollLeft = iLeft
*/
		this.ChangePage()
	}
}

function _FilterReset(outerFilter)
{
	//如果表格已被编辑，则无法排序过滤，须先保存后才能排序过滤。
	if (this.arrModiLog.length > 0)
	{	this.BeforeSortModi()
	}
	var sFilter
	sFilter = this.rs_main.Fields(0).Name + " <> -1.0249E5"
	//if (strFilter != "" && strFilter != null)
	//	sFilter = " & (" + strFilter + ")"
	if (outerFilter != null)
		this.strFilter =  outerFilter


	//this.strFilter = this.rs_main.Fields(0).Name + " <> -1.0249E5" + sFilter
	if (this.strFilter != "" && this.strFilter != null)
		sFilter = this.rs_main.Fields(0).Name + " <> -1.0249E5" + " & ("  + this.strFilter+ ")"


	var oFil
	eval("oFil = " + this.UseSort + ".object")
	oFil.Filter = sFilter
	//eval("oFil = " this.UseSort + ".object.Filter = '" + sFilter + "'")
	//alert(sFilter)
	eval(this.UseSort + ".Reset()")
}

function _RestoreScroll()
{
	//alert(testTableSpan2.scrollLeft)
	eval(this.Name + "TableSpan2.scrollLeft = " + "this.SortLeft")


}


/*
功能  ：设置用户插入控件所在格的内容。
作者  ：李翼嵩
时间  ：2000年8月
参数  ：strVal -- 要显示的内容。
        Col -- 插入字段的位置。
*/
function _ControlReturn(strVal,col)
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
function _UseControl(Col,strContent,ShowChange,Position)
{
	//this.htmlControl = strContent
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
/*
功能  ：添加合计字段。若多次调用，则对多个字段进行合计。
作者  ：李翼嵩
时间  ：2000年8月
参数  ：Column -- 需要合计的字段编号（base 0）。
返回值：
*/
function _UseSum(Column,isCal)
{
	if (Column <= this.Widths.length)
	{
		this.AskSum = true;
		//若记录为空，则不合计。
		this.SumArr[Column] = 0
		if (this.rs_main.RecordCount > 0 && isCal)
		{
			var tempPosition = this.rs_main.absolutePosition;
			this.rs_main.MoveFirst()
			//依次进行合计
			while (!this.rs_main.EOF)
			{
				//若该字段不是数值型，则不进行合计。

				//if (typeof(this.rs_main.Fields(this.GetOrder[Column]).value) != "number")
				if (parseFloat(this.rs_main.Fields(this.GetOrder[Column]).value) != parseFloat(this.rs_main.Fields(this.GetOrder[Column]).value))
				{	this.rs_main.MoveNext()
					continue
				}
				//进行合计
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


/*
功能  ：设定列外观属性。
作者  ：李翼嵩
时间  ：2000年8月
参数  ：ColNum -- 列字段编号（base 0）
        strStyle -- 样式单字串。（如："Width:80; color:red"）
返回值：
*/
function _ColumnStyle(ColNum,strStyle)
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
function _TableStyle(strStyle)
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
function _HeadStyle(strStyle)
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

		//字段顺序可能变化，若是其它页重新排序，则按字段的原有顺序显示
		if (this.WhichPage > 0)
		{
			this.SetOrder()
		}
		str2 = this.DisplayM();
		return  str2 + str1

	}
	else
	{
		//字段顺序可能变化，若是其它页重新排序，则按字段的原有顺序显示
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

function _Display(column, subStr,noAdd,iFast)
{
	var temp = GetCookieTable(this.StrFree)
	this.SetWidth();
	this.ClearV(1)

	if (this.StrFree != "")
	{
		this.modi = new Array();				//可编辑字段数组 true -- 可编辑。
		this.AskSum = false;					//是否增加“合计“
		this.seedControl = new Array()
		this.ControlValue = new Array()
		this.AddDelete = false;			//是否可以添加删除记录

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

function _DisplayM(column, subStr,noAdd,iFast)
{
	this.ClearV(1)
	this.AfterSortModi();
	this.CalculatorAll();
	//NewPage(this.WhichPage,null,true)
	//将所有字段都不显示，只显示该页字段
	var temp, FastWidth
	FastWidth = this.TableWidth
	//设置checkbox column style
	if (this.checkBoxColumn != -1)
		this.ColumnStyle(this.checkBoxColumn,"text-align:center;");

	//设定字段先后顺序

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

		//安排序号
		//if (this.rs_main.Fields(i).Name == "序号" && this.rs_main.RecordCount > 0)
		if (this.GetOrder[i] == this.DefaultSort && this.rs_main.RecordCount > 0)
		{	//this.rs_main.MoveFirst()
			for(var q=1; q<=this.rs_main.RecordCount; q++)
			{	this.rs_main.absolutePosition = q;
				if (this.rs_main.Fields(this.GetOrder[i]).Value == "" ) 
					this.rs_main.Fields(this.GetOrder[i]).Value = this.MaxSortNumber = q; //+ ""
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
	//******************************************** 常量定义 ***********************************
	var TABLE_BEGIN = "<TABLE name='" + this.Name + "TableAll' Id=" + this.Name + "TableAll" + iFast + "  width=" + FastWidth + "px border=1 cellpadding=0 cellspacing=0 onmousedown='" + this.Name + ".TDMouseDown()' onmouseup='" + this.Name + ".MouseDown_Event()'><TR><TD>"

	var TABLE_END = "</TD></TR></TABLE>"

	sumWidth -= 4

	var TABLE_HEAD_H = "<SPAN id='" + this.Name + "SpanHead" + iFast + "' style='WIDTH:"+ FastWidth + ";OVERFLOW:hidden'><TABLE id='" + this.Name + "TableHead" + iFast + "' cellpadding=2  cellspacing=0 style='border-style:solid;border-width:1px;border-color:" + this.TableLineColor + "' style='width:" + sumWidth + "px'>"
	var TABLE_HEAD_T = "<br><SPAN id='" + this.Name + "SpanTail" + iFast + "' style='WIDTH:"+ FastWidth + ";OVERFLOW:hidden'><TABLE id='" + this.Name + "TableTail" + iFast + "' cellpadding=2  cellspacing=0 style='border-style:solid;border-width:1px;border-color:" + this.TableLineColor + "' style='width:" + sumWidth + "px'>"

	sumWidth -= 18
	var TABLE_HEAD = "<TABLE id='" + this.Name + "TableBody" + iFast + "' cellpadding=2  cellspacing=1 bordercolor=Silver bgcolor=Gray " + this.strTable + " onKeyDown='" + this.Name + ".DealKeyPress()' style='width:" + sumWidth + "px;position:absolute;z-index:100'   onmousewheel='" + this.Name +".ScrollVer(event.wheelDelta )' >"

	var TABLE_TAIL_H = "</TABLE></SPAN>"
	var TABLE_TAIL = "</TABLE>"

	var HEAD_ROW_BEGIN = "<TR style='word-break:break-all;border-color:" + this.TableLineColor + "' name=Head bgcolor=#cebe9c align=center>"
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
	//var FREE_BUTTON = '<button onclick="' + this.Name + '.ShowFree()"><IMG SRC="../../CommonControl/images/Free.gif"></button> '
	var FREE_BUTTON = '<IMG ID=' + this.Name + '0img' + iFast + ' src="' + this.ControlPath + 'images/0.gif" onmousedown="' + this.Name + '.ShowFree()' + '" style="CURSOR: hand">'
	if (this.WhichPage == 0)
		FREE_BUTTON = FREE_BUTTON.replace("0.gif","0_.gif")
	//var SET_BUTTON = '<button onmousedown="' + this.Name + '.SetFree()"><IMG SRC="../../CommonControl/images/Set.gif"></button> '
	var SET_BUTTON = '<IMG ID=' + this.Name + 'setbutton' + iFast + ' src="' + this.ControlPath + 'images/SetD.gif" onmousedown="' + this.Name + '.SetFree()' + '" style="CURSOR: hand">'
	//************************************************************************************************

	if (iFast != "")
		TABLE_BEGIN = "<TABLE name='" + this.Name + "TableAll' Id=" + this.Name + "TableAll" + iFast + "  width=" + FastWidth + "px border=0 cellpadding=0 cellspacing=0 onmousedown='" + this.Name + ".TDMouseDown()' onmouseup='" + this.Name + ".MouseDown_Event()'><TR><TD>"

	sUse[0] = TABLE_BEGIN

		var sDisplay = ""
		var sRowCol = ""	//多层表格rowspan=2
		var sNext = ""
		DivWidth = 0

	//是否显示标题。
	if (this.Title)
	{	//建立表头
		//aa.sub()
		sSort = ""
		sUse[sUse.length] = TABLE_HEAD_H + HEAD_ROW_BEGIN
		if (this.DHSpan.length > 0)
			sRowCol = " rowspan=2 "

		//建立表的标题头
		for (field=0; field<this.Widths.length; field++)
		{
			//是否指定字段宽度
			iWidth = this.Widths[this.GetOrder[field]] - 5
			//设置显示字段
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
				//不指定排序
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
			{	//指定排序
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

		//滚动条之上
		sUse[sUse.length] =  "<TH " + sRowCol + "id='aa11' width=13 style='" + this.strHead + "'></TH>"
		//建立纪录头。
		sUse[sUse.length] =  HEAD_ROW_END + HEAD_ROW_BEGIN + sNext + HEAD_ROW_END + TABLE_TAIL_H
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
	var j = 0
	var sColor = ""	//单元格的样式
	for (j = 0; j<this.rs_main.RecordCount; j++)
	{	//若结尾，则跳出循环。
		if (this.rs_main.EOF)
			break

		//防止显示虚拟记录
		if (this.rs_main.Fields(0).value == "-1.0249E5")
		{	this.rs_main.MoveNext()
			continue
		}

		if (j > this.Height / 20)
			break
		
		//自动计算栏位值
		this.CalculateColumn();
		
		//设置背景色
		if (this.rs_main.Fields(this.GetOrder[this.Widths.length-1]).Name == "CC_颜色" && this.rs_main.Fields(this.GetOrder[this.Widths.length-1]).value != "")
			sBackC = " Style='background-color:" + this.rs_main.Fields(this.GetOrder[this.Widths.length-1]).value + "; height:20px; color:black; word-break:break-all'"
		else
			sBackC = " Style='background-color:" + this.arrColor[jj % 2] + "; height:20px; color:black; word-break:break-all'"


		sBody[sBody.length] =  ROW_BEGIN.replace("rownum", ""+ jj + sBackC)
		jj += 1
		//安排记录中的各个字段。
		for (field=0; field<this.Widths.length; field++)
		{	//是否指定宽度。

			if (this.CellColorColumn >= 0)
			{
				sColor = this.CellColorArr["A" + this.rs_main.Fields(this.GetOrder[this.CellColorColumn]).Value + field]
				if (sColor == null)
					sColor = ""
			}
			
			//设置显示字段
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
			//if (this.rs_main.Fields(field).Name == "序号")
			//{	sBody[sBody.length] =  (jj - 1) + ""
			//	this.rs_main.Fields(field).Value = (jj-1) + ""
			//}
			//else

			if (this.checkBoxColumn == this.GetOrder[field])
			{
				sBody[sBody.length] = "<INPUT TYPE=CHECKBOX NAME=" + this.Name + "cb" + field + " id=" + this.Name + "cb" + field + " onclick='" + this.Name + ".checkItem()' " + this.rs_main.Fields(this.GetOrder[field]).Value + "  " + this.checkBoxStyle + "></input>"
			}
			else
			{
				if (this.SetZ == true || this.rs_main.Fields(this.GetOrder[field]).Value * 1 != 0  || this.FieldsType[this.GetOrder[field]] != "num")
				{

					if (this.ShowZ == true || this.rs_main.Fields(this.GetOrder[field]).Value != 0)
						sBody[sBody.length] =  this.ChangeK(this.rs_main.Fields(this.GetOrder[field]).Value)
				}
			}
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
	sBody[sBody.length] =  sBackC.replace("TableRow'","TableRow' id=" + this.Name + "AddRow" + iFast + "")
	sDisplay = ""
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
		iWidth = this.Widths[this.GetOrder[field]] - 5
		sStyle += this.FieldsType[this.GetOrder[field]] == "num"?"   ." + this.Name + field + "{text-align:right}    ":""
		//sWidth = CELL_BEGIN.replace("FFIIEE","" + field + sDisplay + " name='Virtul'  width=" + iWidth +  " id=" + this.Name + "Add onclick='return " + this.Name + ".Append()'")
		sWidth = CELL_BEGIN.replace("FFIIEE","" + field + sDisplay + " name='Virtul'  width=" + iWidth +  " id=" + this.Name + "Add" + iFast + "")
		sBody[sBody.length] =  sWidth
		sBody[sBody.length] = CELL_END
	}

	sDisplay = ""
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
			iWidth = this.Widths[this.GetOrder[field]] - 5
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
	var sumWidth2 = sumWidth


	if (sumWidth+18 >= FastWidth)
		sumWidth = FastWidth -18
	if (this.Height == 0)
		sUse[sUse.length] = sWidth
//	else
//	{	if (this.AskSum)
//			sUse[sUse.length] = "<SPAN id='" + this.Name + "TableSpan' style ='height:" + this.Height + ";width:" + (sumWidth + 18) + "; OVERFLOW:auto' onscroll='" + this.Name + "SpanHead.scrollLeft = this.scrollLeft;" + this.Name + "SpanTail.scrollLeft = this.scrollLeft	'>" + sWidth + "<DIV STYLE='WIDTH:5;HEIGHT:" + (this.rs_main.RecordCount * 20) + ";POSITION:absolute'></DIV></SPAN>"
//		else
//			sUse[sUse.length] = "<SPAN id='" + this.Name + "TableSpan' style ='height:" + this.Height + ";width:" + (sumWidth + 18) + "; OVERFLOW:auto' onscroll='" + this.Name + "SpanHead.scrollLeft = this.scrollLeft;'>" + sWidth + "<DIV STYLE='WIDTH:5;HEIGHT:" + (this.rs_main.RecordCount * 20) + ";POSITION:absolute'></DIV></SPAN>"
//	}
	else
	{
		//滚动条内的宽度
		var ihigh
		ihigh = this.Height
		//若产生横向滚动条，则表格高度-18，以便完全显示横向滚动条。
		if (this.OverFlow == "scroll")
			ihigh = ihigh - 18
		else
		{

		//若产生横向滚动条，则表格高度-18，以便完全显示横向滚动条。

			if ((this.TableWidth-15) < (sumWidth2))
				ihigh = ihigh - 18

		}

/*
以下部分，是产生滚动条而又能迅速显示的机制。

*/
		sUse[sUse.length] = "<br><Span id='Ret_3' style='position:relative; left:0; top:0'>"
		if (iFast == "")
		{
			//TableSpan2--用于产生滚动条的span,内含一个高度近似表格的的DIV（Measure），从而产生滚动条
			sUse[sUse.length] = "<SPAN id='" + this.Name + "TableSpan2" + iFast + "' style ='left:0; top:20; height:" + this.Height + ";width:" + (sumWidth + 18) + "; OVERFLOW:" + this.OverFlow + "; z-index:1500' onscroll='" + this.Name + ".ScrollVer()' ><DIV id=" + this.Name + "Measure STYLE='WIDTH:" + (DivWidth+1) + ";HEIGHT:" + (this.rs_main.RecordCount * Math.floor(this.Height / 8) + this.Height+1) + ";POSITION:absolute;z-index:2000'></DIV></SPAN>"
			//用于存放表格的DIV，当TableSpan2滚动时，只变换该表格显示的的内容，而不实际滚动
			sUse[sUse.length] = "<DIV id='" + this.Name + "TableSpan' style ='left:0; height:" + ihigh + ";width:" + (sumWidth ) + "; position:absolute; OVERFLOW:hidden;z-index:100'>" + sWidth + "</DIV>"
			//增加一个输入框，用于获得焦点。否则表格有输入框时，移动滚动条后再按上下键会产生错误。
			sUse[sUse.length] = "<INPUT TYPE=TEXT id='" + this.Name + "VirtualFocus" + iFast + "' style ='left:" + (3+sumWidth) + "; top:" + (this.Height) + ";height:1;width:0; position:absolute; OVERFLOW:hidden;z-index:107'" + " onKeyDown='" + this.Name + ".DealKeyPress()'  >"
		}
		else
		{
			//TableSpan2--用于产生滚动条的span,内含一个高度近似表格的的DIV（Measure），从而产生滚动条
			sUse[sUse.length] = "<SPAN id='" + this.Name + "TableSpan2" + iFast + "' style ='left:0; top:20; height:" + ihigh + ";width:" + (FastWidth) + "; OVERFLOW:hidden; z-index:1500' onscroll='" + this.Name + ".ScrollVer()'><DIV id=" + this.Name + "Measure" + iFast + " STYLE='WIDTH:" + (DivWidth+1) + ";HEIGHT:" + (this.rs_main.RecordCount * Math.floor(this.Height / 8) + this.Height+1) + ";POSITION:absolute;z-index:2000'></DIV></SPAN>"

			//用于存放表格的DIV，当TableSpan2滚动时，只变换该表格显示的的内容，而不实际滚动
			sUse[sUse.length] = "<DIV id='" + this.Name + "TableSpan" + iFast + "K' style ='left:0; height:" + ihigh + ";width:" + (FastWidth ) + "; position:absolute; OVERFLOW:hidden;z-index:100'>" + sWidth + "</DIV>"

		}
		//sUse[sUse.length] = "<SPAN id='" + this.Name + "TableSpan' style ='height:" + this.Height + ";width:" + (sumWidth + 18) + "; OVERFLOW:auto' onscroll='" + this.Name + ".ScrollVer()'>" + sWidth + "<DIV STYLE='WIDTH:5;HEIGHT:" + (this.rs_main.RecordCount * 20) + ";POSITION:absolute'></DIV></SPAN>"
		sUse[sUse.length] = "</SPAN>"

	}

	//（表尾）是否显示汇总
	//是否显示汇总
	if (this.AskSum && iFast == "")
	{

	//建立表尾
		sUse[sUse.length] = TABLE_HEAD_T + HEAD_ROW_BEGIN.replace("<TR ","<TR id=" + this.Name + "Sums" + iFast + " ")

		//建立表的标题头
		for (field=0; field<this.Widths.length; field++)
		{
				if (field != 0 & this.SumArr[field] != null)
				{
					this.UseSum(field,true)
			    }

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
						sDisplay = " style='display:none;font-size:8pt;font-weight:400; text-align:right'"
					else
						sDisplay = " style='font-size:8pt;font-weight:400; text-align:right'"
				}
			}


			//是否指定字段宽度
			iWidth = this.Widths[this.GetOrder[field]] - 5
			sWidth = TAIL_BODY_BEGIN.replace("<TD","<TD width=" + iWidth  + "px id=" + this.Name + "T" + field  + iFast +  sDisplay)
			sUse[sUse.length] = sWidth
			
			if (field==0)
				sUse[sUse.length] = this.HEJI
			else
				if (this.SumArr[field] != null)
					sUse[sUse.length] = this.SumArr[field]
				else
					sUse[sUse.length] = this.KONG

			sUse[sUse.length] = TAIL_BODY_END
		}
		//sUse[sUse.length] = "<TD width=13 " + this.strHead + "></TD>"
		sUse[sUse.length] = TAIL_BODY_BEGIN.replace("<TD","<TD width=12px ")
		sUse[sUse.length] = HEAD_ROW_END + TABLE_TAIL_H
	}



	//返回表格的HTML字符串。
	sUse[sUse.length] = TABLE_END

	//是否增加页选择
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
		//是否显示记录数
		var sCount = ""
		if (this.ShowCount == true)
			sCount = this.JILUSHU + this.rs_main.RecordCount

		//是否显示自由选择字段
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
			//sSearch += "<Select id=" + this.Name + "Search onchange='" + this.Name + "SearchValue.value=\"\"'>"
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
 			//sSearch += "<Input id=" + this.Name + "SearchValue style='width:80;height:20'>"

			//sSearch += "<IMG style='height:16;width:16;CURSOR: hand' onclick='" + this.Name + ".SearchFilter()' src='" + this.ControlPath + "images/Search.bmp'>"  //+ SEARCH_BUTTON+ "</Span>"
			//sSearch += "<IMG style='height:16;width:16;CURSOR: hand' onclick='" + this.Name + ".SearchFilter(1)' src='" + this.ControlPath + "images/All.bmp'>" //+ ALL_BUTTON + "</Span>"

			sSearch += "<Button style='height:20;width:20' onclick='" + this.Name + ".SearchFilter()' >"  + SEARCH_BUTTON+ "</Button>"
			sSearch += "<Button style='height:20;width:20' onclick='" + this.Name + ".SearchFilter(1)'>" + ALL_BUTTON + "</Button>"
		}

		if (this.UseSearch == "AllLine" || this.UseSearch == "PageLine")
		{//使用快速查询带格线
			if (iFast == "")
				sTemp = "<TABLE id='" + this.Name + "Disp" + iFast + "'height=22 border=1 width=" + FastWidth + "><TR><TD align=left id=" + this.Name + "Count" + iFast + " style='font-family:宋体;font-size:9pt;filter:none' width=30%>" + sCount + "</TD><TD align=middle>" + sSearch + "</TD><TD align=middle>" + sFree + "</TD><TD align=right>" + sHide + "</TD></TR></TABLE>" + sTemp
			else
				//sTemp = "<TABLE id='" + this.Name + "Disp" + iFast + "'height=0' style='position:absolute;top:22'><TR><TD></TD><TD></TD><TD></TD></TR></TABLE>" + sTemp
				sTemp = "<Div style='POSITION: absolute; TOP: " + iFH + "px;'>" + sTemp + "</Div>"
		}
		else
		{//使用快速查询不带格线
			if (iFast == "")
				sTemp = "<TABLE id='" + this.Name + "Disp" + iFast + "'height=22 width=" + FastWidth + "><TR><TD align=left id=" + this.Name + "Count" + iFast + " style='font-family:宋体;font-size:9pt;filter:none' width=30%>" + sCount + "</TD><TD align=middle>" + sSearch + "</TD><TD align=middle>" + sFree + "</TD><TD align=right>" + sHide + "</TD></TR></TABLE>" + sTemp
			else
				//sTemp = "<TABLE id='" + this.Name + "Disp" + iFast + "'height=0' style='position:absolute;top:22'><TR><TD></TD><TD></TD><TD></TD></TR></TABLE>" + sTemp
				sTemp = "<Div style='POSITION: absolute; TOP: " + iFH + "px;'>" + sTemp + "</Div>"

		}
	}
	//设置列格式
	sTemp = sTemp + "   " + "<Style> <!--" + this.strColumn + sStyle + "--> </style>"
	
	//不知为什么，空记录集无法在运行时AddNew，只好出此下策。--- 1
	if (this.rs_main.RecordCount < 1 && noAdd != true)
	{
		this.rs_main.AddNew()
		if (this.rs_main.RecordCount > 0)
			this.rs_main(0) = "-1.0249E5"

		this.IsEmpty = true
	}
	else
		this.IsEmpty = false

	//重新排序后，清除高亮显示行
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



/*
功能：  刷新背景色。
作者：  李翼嵩
日期：  2000.7
返回值：返回鼠标处的整条记录内容。字段与字段间用空格分开。
*/
function _BackRefresh()
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
功能：  当表格中删除选中的行后，处理ModiLog数组合DelLog数组。
作者：  李翼嵩
日期：  2000.7
*/
function _DealLogWhenDel()
{
	//若该记录没有进行过任何操作，则计入“删除”数组。
	if (this.arrModiLog[this.rs_main.AbsolutePosition] == null )
		this.arrDelLog[this.arrDelLog.length] = this.RowStr
	//若该纪录不是新增纪录，则计入删除数组。
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


/*
功能：  从表格中删除选中的行。
作者：  李翼嵩
日期：  2000.7
*/
function _DelRow(iEleRow)
{
	var oRows
	var oNodes
	//是否有选中的行
	if (this.preElement != null)
		if (this.preElement.firstChild.name != "Virtul")
		{
			//删除该行
			//eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild.removeChild(this.preElement)")
			this.rs_main.MoveFirst()
			this.rs_main.Move(this.PreRow)
			if (this.rs_main.AbsolutePosition < 0)
				return
			var iRCount = this.rs_main.RecordCount

			if (this.DefaultSort>=0)
				if (this.MaxSortNumber == this.rs_main.fields(this.DefaultSort).value && this.MaxSortNumber>0)
					this.MaxSortNumber--;

			this.rs_main.Fields(this.GetOrder[0]).value = "-1.0249E5"

			//处理删除后的改变DelLog数组
			this.DealLogWhenDel()

			

			this.rs_main.Delete()
			
			if (this.rs_main.RecordCount == 0)
				this.IsEmpty = true
			else
				this.IsEmpty = false


			if (this.UseSort != "")
				eval("this.rs_main = " + this.UseSort + ".recordset")
			//if (this.rs_main.RecordCount == iRCount && this.UseSort != "")
			//	this.Filter()

			//this.rs_main.MoveFirst()
			//this.rs_main.Update()
			//选中标志置空
			//恢复高亮为正常


			if (this.preElement != null)
			{	this.preElement.style.backgroundColor = this.preColor;
				this.preElement.style.color = "black";
				this.preElement = null
				if (this.FastField > 0)
				{	//var fastLightRow = document.all(this.Name + "TableBody" + this.FastField).firstChild.childNodes(this.preElement.name*1-this.FirstRow)
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
			//调整滚动高度
			eval("oRows = " + this.Name + "Measure")
			oRows.style.height = this.rs_main.RecordCount * Math.floor(this.Height / 8) + this.Height + 1
			//刷新背景
			//this.BackRefresh()
			if (this.FirstRow >= this.rs_main.RecordCount)
				this.FirstRow = this.rs_main.RecordCount - 1

			if (this.FirstRow < 0)
				this.FirstRow = 0

			this.ChangePage()

			this.ClearV()

			//是否显示汇总
			if (this.AskSum)
			{
				for (i=0; i<this.Widths.length; i++)
				{
					if (i != 0 & this.SumArr[i] != null)
					{
						this.UseSum(i,true)
						eval(this.Name + "Sums.childNodes(i).innerText = this.SumArr[i]")
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

/*
功能：  设置选种行的内容。
作者：  李翼嵩
日期：  2000.7
参数：  Content -- 该行内容，以","分隔。
*/
function _SetRow(Content)
{
	var oNodes,oFastNodes,i,strTD

	if (this.preElement == null)
		return null

	if (this.preElement.firstChild.name == "Virtul")
		return null



	//生成数组
	strTD = Content.split(this.Divide)
	for (var key in this.Calculator)
	{
	   var 	strCalculate = this.Calculator[key]
	   strCalculate = strCalculate.replace(/\</g, "strTD[");
	   strCalculate = strCalculate.replace(/\>/g, "]");

	   strCalculate = strCalculate.replace("=", "=Math.round((") + ")* Math.pow(10,this.SumDot)) / Math.pow(10,this.SumDot)"
	   eval(strCalculate);	
	}
	//得到所有单元格。
	oNodes = this.preElement.childNodes
	//固定栏位的表格
	if (this.preFastElement != null)
		oFastNodes = this.preFastElement.childNodes
	else
		oFastNodes = null

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
			//验证是否为数值
			if (this.FieldsType[i] == "num")
			{
				if (parseFloat(strTD[this.GetOrder[i]]) * 1 != parseFloat(strTD[this.GetOrder[i]]))
					continue;
			}

 			if (this.checkBoxColumn == i)
			{	//continue;
				oNodes(i).innerHTML = "<INPUT TYPE=CHECKBOX NAME=" + this.Name + "cb" + i + " id=" + this.Name + "cb" + i + " onclick='" + this.Name + ".checkItem()' " + strTD[this.GetOrder[i]] + "  " + this.checkBoxStyle + "></input>"	
			}
 			else
 			if (this.modi != null)
			{
				if (this.modi[i] == true)
				{
					eval(this.UseForm + this.Name + "Text" + this.GetOrder[i] + ".value = '" + this.ReplaceChar(strTD[this.GetOrder[i]],"'") + "'")
					
				}
				else
				{	oNodes(i).innerText = strTD[this.GetOrder[i]]

					if (oFastNodes != null)
						oFastNodes(i).innerText = strTD[this.GetOrder[i]]


				}

			}
			else
			{	oNodes(i).innerText = strTD[this.GetOrder[i]]

				if (oFastNodes != null)
					oFastNodes(i).innerText = strTD[this.GetOrder[i]]

			}
			strTD[this.GetOrder[i]]+=""
			this.rs_main.Fields(this.GetOrder[i]).value = strTD[this.GetOrder[i]].replace(/(^\s*)|(\s*$)/ig,"");

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
					eval(this.Name + "Sums.childNodes(i).innerText = this.SumArr[i]")
				}

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

function _Append(Content)
{
	var iRow,i,strTD,oNodes
	//得到内容数组


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

	//当前显示位置
	//eval("iRow = " + this.Name + ".rs_main.RecordCount * 20")
	eval("iRow = " + this.Name + ".rs_main.RecordCount")
	//this.FirstRow = this.rs_main.RecordCount


	//添加一行
	eval(this.Name + ".AddEvent(true,strTD)")

	//调整滚动高度
	eval("oNodes = " + this.Name + "Measure")
	eval("oNodes.style.height = " + this.Name + ".rs_main.RecordCount * Math.floor(this.Height / 8) + " + this.Name + ".Height + 1")

	//HighLightRow(iRow)
	var iFirst
	iFirst = this.FirstRow
	eval(this.Name + ".HighLightRow(iRow,'AddNew')")
	//eval(this.Name + "TableSpan2.scrollTop =" + iRow )
	//this.ChangePage()




/*
//##############################################
	this.rs_main.Movelast()
	for (i=0;i<this.Widths.length;i++)
		this.rs_main.Fields(i).value = strTD[this.GetOrder[i]]

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
//##############################################
*/

}



/*
功能：  调用者手动，或点击表格自动添加一行。
作者：  李翼嵩
日期：  2000.7
参数：  bAppend -- 是否手动
		Cells   -- 单元内容数组
*/
function _AddEvent(bAppend,Cells)
{
	if (bAppend !=true && this.AddDelete!=true )
		return null
	//插入一行记录
	var oNode, oRows, strCells, iRows, oTemp,iLen
	this.ClearV()
	//window.alert("Count2 = " + this.rs_main.RecordCount)
	this.LightRow = this.rs_main.RecordCount

	this.rs_main.Addnew()

	if (this.UseSort != "")
		eval("this.rs_main = " + this.UseSort + ".recordset")

	this.rs_main.MoveLast()

	if (Cells == null)
		iLen = this.Widths.length
	else
		iLen = Cells.length

	//依次赋值
	for (var field=0; field<this.Widths.length||(field<this.rs_main.Fields.count&&field<iLen); field++)
	{
/* 2004-08-10 commen for don't net auto sort, and replace this function with add sort number save to EJB

		if (this.GetOrder[field] == this.DefaultSort )
		{
			this.rs_main.Fields(this.GetOrder[field]).Value = this.rs_main.RecordCount + ""

		}
*/
		if (this.GetOrder[field] == this.DefaultSort )
		{
			this.MaxSortNumber++;
			this.rs_main.Fields(this.GetOrder[field]).Value = this.MaxSortNumber; //+ ""

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
					if (this.FieldsType[field] == "num")
						this.rs_main.Fields(this.GetOrder[field]).value = Cells == null?0:isNaN(parseInt(Cells[field]))?0:Cells[field]
					else
						this.rs_main.Fields(this.GetOrder[field]).value = Cells == null?"":Cells[field]

				}
			}
		}

	}


	this.rs_main.Update()

	//增加一行时,在数组中增加标志.
	this.arrModiLog[this.rs_main.AbsolutePosition] = 1

	//window.alert("Count2 = " + this.rs_main.RecordCount)


/*
//##############################################
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
//##############################################
*/
	this.ClearV()

}

/*
功能：  当鼠标在<input>中MouseDown，而后Move到非高亮行后MouseUp,
		则会在响应事件中出错并关闭IE。为此特设置此函数用以在以上情况
		发生时不响应MouseUp事件。
作者：  李翼嵩
日期：  2001.3
*/


function _TDMouseDown()
{
	var srcNode		//临时对象，保存MouseUp处理函数
	//alert(2)
	//因为有在所有行显示checkbox的需求，所以屏蔽此功能，否则点击控件该行不高亮
	return
	if (event.srcElement.tagName != "TD")
	{
	//当MouseDown事件发生在表格中的插入控件时，为防止MouseOut后再MouseUp
	//特保存事件处理函数。
		eval("srcNode = " + this.Name + ".DownEventSrc")

		//防止重复保存，所以只有在位保存过时才做。
		if (srcNode == null)
		{	//保存
			eval(this.Name + ".DownEventSrc = " + this.Name + "TableAll.onmouseup")
			//屏蔽Mouseup事件。
			eval(this.Name + "TableAll.onmouseup=null")
		}
	}
	else
	{	//恢复事件屏蔽
		eval("srcNode = " + this.Name + ".DownEventSrc")
		if (srcNode != null)
		{	//恢复
			eval(this.Name + "TableAll.onmouseup = srcNode")
			//清除保存值
			eval(this.Name + ".DownEventSrc = null")
		}
	}


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

function _MouseDown_Event(Source,Direct,keyControl)
{
	var elerow,fastRow,strRe, EnableFileld
	//aa = bb.sub()
	//alert(1)
        if (this.CanNotChangRow)
            return
	this.ClearV()
	//事件源
		if (Source == null)
		{
			//alert(window.event.srcElement.tagName)
			//if (window.event.srcElement.tagName == "TD")
			//	eval(this.Name + "VirtualFocus.focus()")
			elerow = window.event.srcElement;
			if (this.onEvent == true)
				return
			this.onEvent = true
		}
		else
			elerow = Source


	//若为右键则退出
	//	if (window.event.button == 2)
	//		return "nothing";

		try
		{
			if (elerow.parentElement.parentElement != null)
				null

		}
		catch(e)
		{   this.onEvent = false
			return false
		}

		//向上追溯事件的父对象，是否为记录行对象。
		if (elerow.parentElement.parentElement != null)
			elerow = elerow.parentElement;
		if (elerow.parentElement.classid == this.Name + "TableRow" || elerow.parentElement.classid == this.Name + "TableRow" + this.FastField)
		{
			elerow = elerow.parentElement;
		}

    //若是记录行则响应该事件
	if (elerow.classid == this.Name + "TableRow" || elerow.classid == this.Name + "TableRow" + this.FastField)
	{	var iRow = elerow.name * 1
		elerow = document.all(this.Name + "TableBody").firstChild.childNodes(iRow-this.FirstRow)
		if (this.FastField > 0)
			fastRow = document.all(this.Name + "TableBody" + this.FastField).firstChild.childNodes(iRow-this.FirstRow)
		else
			fastRow = null
		//
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

			//aa = bb.sub()
			eval("bReturn = " + this.BeforeSave)
			if (bReturn == false)
			{	this.onEvent = false
				return false
			}

			eval("elerow = " + sid)
		}
		//自动删除一条记录
		//if (this.AddDelete == true && this.NotEmpty >= 0 && this.preElement != null && this.LightRow == (this.rs_main.RecordCount-1))
		if (this.AddDelete == true && this.NotEmpty >= 0 && this.preElement != null )
		{
			if (this.preElement.firstChild.name != "Virtul")
			{
				if (this.modi != null)
				{	if (this.modi[this.NotEmpty] == true)
					{
						var editnode
						//this.UseForm + this.Name + "Text" + i
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

				//当出现选择框时，进行自动删除。
				if (this.outerSelect == true && this.seedControl[this.NotEmpty] != null  && this.preElement != null)
				{    if (this.preElement.childNodes(this.NotEmpty).childNodes.length > 1)
					if (this.preElement.childNodes(this.NotEmpty).childNodes(1).tagName == "SELECT")
					{
						//eval("editnode = " + this.Name + "Text" + this.NotEmpty + ".value")
						editnode = this.preElement.childNodes(this.NotEmpty).childNodes(1).options(this.preElement.childNodes(this.NotEmpty).childNodes(1).selectedIndex).text
						//alert(editnode)
						if (editnode == "")
							elerow = this.Delete(elerow.name)

					}
				}


			}
		}

		//得到第一个非隐藏字段
		EnableFileld = this.GetFirstEnableChild()

		//自动增加一条记录
		//if (this.AddDelete == true && elerow.childNodes(EnableFileld).id == this.Name + "Add" && (this.rs_main.RecordCount < this.MaxLine))
		if (this.AddDelete == true && elerow.childNodes(EnableFileld).id == this.Name + "Add" )
		{

			this.AddDelete == false
			elerow.childNodes(EnableFileld).id = null
			//this.LightRow = this.rs_main.RecordCount
			this.AddEvent()
			//调整滚动高度
			eval(this.Name + "Measure.style.height = " + (this.rs_main.RecordCount * Math.floor(this.Height / 8) + this.Height * 1 + 1 * 1))
			this.ChangePage()
			this.AddDelete == true
			//elerow.childNodes(EnableFileld).id = this.Name + "Add"
			if (Source == null)
			{
				this.onEvent = false
			}

			return
		}

		//若不是第一次响应该事件，恢复上一次的正常背景色。
		if (this.preElement != null && Direct != true)
		{	this.preElement.style.backgroundColor = this.preColor;
			this.preElement.style.color = "black";

			if (this.FastField > 0)
			{	//var fastLightRow = document.all(this.Name + "TableBody" + this.FastField).firstChild.childNodes(this.preElement.name*1-this.FirstRow)
				this.preFastElement.style.backgroundColor = this.preColor;
				this.preFastElement.style.color = "black";
				this.preFastElement.style.height = "20px"

 			}
		}
		//保存背景色，以备恢复。
		this.preColor = elerow.style.backgroundColor;
		//指定新的背景色。
		elerow.style.backgroundColor = this.LightBKColor    //"blue";
		elerow.style.color = this.LightColor    //"yellow"


		//返回该记录内容。

		//换行前的操作
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

		//为了避免滚动后按上下键出错，使用该方法恢复键盘处理。
//		if (this.KeyBak != null)
//		{
//			alert(this.KeyBak)
//			eval(this.Name + "TableBody.onkeydown = this.KeyBak")
//			this.KeyBak = null
//		}
		this.preElement = elerow;

		if (this.preFastElement != null)
		{
			this.preFastElement.style.height = this.preElement.offsetHeight
			if (this.preElement.parentElement.parentElement.parentElement.scrollTop != 0)
				this.preElement.parentElement.parentElement.parentElement.scrollTop = 0
				//this.preFastElement.parentElement.parentElement.parentElement.scrollTop = this.preElement.parentElement.parentElement.parentElement.scrollTop


		}

		this.currentRow = elerow.name
		
		this.PreLine = elerow.name
		eval(this.AfterNew)


		//保存该行，以备恢复。

		//return
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

//	if (elerow.name == this.Name + "TableAll")
//		return this.Name + "TableAll"

//	return "nothing"
}


/*
功能  ：当一行失去焦点时，保存界面内容，保存记录。
作者  ：李翼嵩
时间  ：2000年8月
参数  ：
返回值：
*/
function _SaveRow(NoCh)
{
//加入参数，为了只保存记录集而不改变外观。
	var i,j,strTemp,bTemp
    var iRow
    var strRe = ""
    var strRow = ""
	iRow = this.PreRow
	if (this.rs_main.RecordCount > 0 )
	{	this.rs_main.MoveFirst();
		this.rs_main.Move(iRow)
	}
	if (this.rs_main.AbsolutePosition < 0)
		return
//记录界面的内容

//保存界面内容
	for (i=0; i<this.Widths.length; i++)
	{
		strRow += this.rs_main.Fields(i).Value

		if (i < this.Widths.length -1)
					strRow += this.Divide

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
					strTemp += ""
					strTemp = strTemp.replace(/(^\s*)|(\s*$)/ig,"");
					//if (NoCh == null)
					//	eval(this.UseForm + this.Name + "Text" + i + ".blur()")

					if (this.FieldsType[i] == "num")
					{
						//aa = bb.sub()
						if (parseFloat(strTemp) * 1 != parseFloat(strTemp)|| parseFloat(strTemp) == 0)
						{	//若不设为零，则直接返回
							if (this.SetZ == false)
								strTemp = "*SetZ*"
							else
								strTemp = 0
							//this.preElement.childNodes(i).innerText = this.rs_main.Fields(i).value
							//this.preElement.childNodes(i).style.padding=2
							//保存记录集内容
							//this.rs_main.Fields(i).value = strTemp
							//continue
						}
						else
							strTemp = parseFloat(strTemp)
					}
					//if (NoCh == null)
					{
						if (strTemp != "*SetZ*" )
						{	//if (this.SetZ == true || this.rs_main.Fields(this.GetOrder[i]).Value * 1 != 0  )
								this.preElement.childNodes(i).innerText = strTemp
							//else
							//	this.preElement.childNodes(i).innerText = ""
						}
						else
						{

							//if (this.SetZ == true || this.rs_main.Fields(this.GetOrder[i]).Value * 1 != 0 )
							//	this.preElement.childNodes(i).innerText = this.rs_main.Fields(this.GetOrder[i]).value
								this.preElement.childNodes(i).innerText = ""


						}
						this.preElement.childNodes(i).style.padding=2
					}

					//保存记录集内容
					if (strTemp != "*SetZ*" )
						this.rs_main.Fields(this.GetOrder[i]).value = strTemp;
					//strRow += strTemp + this.Divide
					if (this.preFastElement != null)
						this.preFastElement.childNodes(i).innerHTML = this.preElement.childNodes(i).innerHTML
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
								this.preElement.childNodes(i).innerHTML = this.ChangeK(strTemp)
							else
								this.preElement.childNodes(i).innerHTML = this.ChangeK(this.rs_main.Fields(this.GetOrder[i]).value)

							if (this.OuterControl1 == i)
								this.preElement.childNodes(i).innerHTML = this.ChangeK(this.rs_main.Fields(this.GetOrder[i]).value)

							if (this.OuterControl2 == i)
								this.preElement.childNodes(i).innerHTML = this.ChangeK(this.rs_main.Fields(this.GetOrder[i]).value)

							if (this.OuterControl3 == i)
								this.preElement.childNodes(i).innerHTML = this.ChangeK(this.rs_main.Fields(this.GetOrder[i]).value)

							if (this.OuterControl4 == i)
								this.preElement.childNodes(i).innerHTML = this.ChangeK(this.rs_main.Fields(this.GetOrder[i]).value)

							this.preElement.childNodes(i).style.padding = 2
						}

						if (this.OuterControl != i && this.OuterControl1 != i && this.OuterControl2 != i && this.OuterControl3 != i && this.OuterControl4 != i)
							//保存记录集内容
							this.rs_main.Fields(this.GetOrder[i]).value = strTemp;
						//strRow += strTemp + this.Divide

						if (this.preFastElement != null)
							this.preFastElement.childNodes(i).innerHTML = this.preElement.childNodes(i).innerHTML

						continue
				}

			////将其他内容保存
			//if (this.preElement.childNodes(i).name != "Virtul")
			//{	strTemp = this.preElement.childNodes(i).innerText
				//this.rs_main.Fields(i).value = strTemp
			//	strRow += strTemp + this.Divide
			//}
		}

		if (this.preFastElement != null)
			this.preFastElement.childNodes(i).innerHTML = this.preElement.childNodes(i).innerHTML

	}
	//保存记录集内容
	//this.rs_main.update()



	//判断当前行是否被修改
	for (i=0; i<this.Widths.length; i++)
	{	strRe += this.rs_main.Fields(i).Value

		if (i < this.Widths.length -1)
					strRe += this.Divide
	}
	if (strRe != strRow)
	{	this.arrModiLog[this.rs_main.AbsolutePosition] = this.arrModiLog[this.rs_main.AbsolutePosition] | 2
	
		//是否显示汇总
		if (this.AskSum)
		{
			for (i=0; i<this.Widths.length; i++)
			{
				if (i != 0 & this.SumArr[i] != null)
				{
					this.UseSum(i,true)
					eval(this.Name + "Sums.childNodes(i).innerText = this.SumArr[i]")
				}
	
			}
		}

	}
	this.RowStr = strRe;
	//return strRow
	if (NoCh == null)
		eval(this.Name + "VirtualFocus.focus()")

}

/*
功能  ：
作者  ：李翼嵩
时间  ：2000年8月
参数  ：
返回值：
*/
function _EditRow(Rownum,oFast)
{
//outControl 用于外部控制临时改变行显示模式
	var i,j,strTemp,strCats,strStyle,iWidth,strRe,oRows,oFastRows,oParent,ilen
	var selectText = null	//用以设定第一个选择框选中
	var sInHT
	strRe = ""	//本行字串
	eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild.childNodes(Rownum-this.FirstRow).childNodes")

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
		if (Rownum < this.rs_main.RecordCount)
		{	this.rs_main.MoveFirst()
			this.rs_main.Move(Rownum)
		}

		eval(this.BeforeNew)

		//对所有列扫描
		for (i=0; i<this.Widths.length; i++)
		{
			//上下划黑实线
			//oRows(i).style.borderTop="1 solid #000000"
			//oRows(i).style.borderBottom="1 solid #000000"
			//对隐藏的列，由于没有更新过，所以重新赋值
			if (Rownum < this.rs_main.RecordCount)
			{
/* 2004-08-10 commen for don't net auto sort, and replace this function with add sort number save to EJB
				//if (this.rs_main.Fields(i).Name == "序号")
				if (i == this.DefaultSort)
					this.rs_main.Fields(this.GetOrder[i]).Value = (Rownum*1 + 1) + ""

				if (this.GetOrder[i] == this.DefaultSort )
					oRows(i).innerHTML = this.ChangeK(this.rs_main.Fields(this.GetOrder[i]).Value)
*/
				//if (this.SetZ == true || this.rs_main.Fields(this.GetOrder[i]).Value * 1 != 0   || this.FieldsType[this.GetOrder[i]] != "num")
				//	oRows(i).innerHTML = this.ChangeK(this.rs_main.Fields(this.GetOrder[i]).Value)
				//else
				//	oRows(i).innerHTML = ""
				oRows(i).name = ''
				strRe += this.rs_main.Fields(i).Value

				if (i < this.Widths.length -1 && oRows(i).name != "Virtul")
					strRe += this.Divide
			}
			//若次列可编辑，为输入框。
			if (this.modi != null)
			{
				//安排新的编辑框
				if (this.modi[i] && oRows(i).name != "Virtul")
				{	//编辑框宽度
					if (this.seedControl[i] != null & oRows(i).name != "Virtul")
						iWidth = this.Widths[this.GetOrder[i]] - 28
					else
						iWidth = this.Widths[this.GetOrder[i]] - 1
					//编辑框缺省内容
					if (this.SetZ == true || this.rs_main.Fields(this.GetOrder[i]).Value * 1 != 0  || this.FieldsType[this.GetOrder[i]] != "num")
					{
						if (this.ShowZ == true || this.rs_main.Fields(this.GetOrder[i]).Value != 0   || this.FieldsType[this.GetOrder[i]] != "num")
							strTemp =  this.rs_main.Fields(this.GetOrder[i]).Value
						else
							strTemp = ""
					}
					else
						strTemp = ""
					//编辑框风格
					strStyle = ' Style="WIDTH:' + iWidth + 'px; border-bottom:1 solid black;"'
					//是否为数值
					if (this.FieldsType[i] == "num" || this.FieldsType[i] == "strnum")
						strStyle += " onKeyDown='_DealNumberPress()' onKeyPress='_DealNumberPress()' onpaste='event.returnValue=false' ondrop='event.returnValue=false' onchange='" + this.Name +".SetCell()'"
						//strStyle += " onKeyDown='_DealNumberPress()' onKeyPress='_DealNumberPress()' onpaste='event.returnValue=false' ondrop='event.returnValue=false' onchange='alert(1)'"
					else
						strStyle += "  ondrop='event.returnValue=false' "
					//缺省值
					//alert(strTemp)
					//alert(this.ChangeK(strTemp))
					strStyle += ' value="' + this.ChangeK(strTemp) + '"'
					//是否指定最大长度
					if (this.MaxSize[i] != null)
						strStyle += " maxlength=" + this.MaxSize[i]
					//当前格<TD></TD>中，插入编辑框HTML文档。
					oRows(i).innerHTML = "<input type=Text id=" + this.Name + "Text" + i + strStyle + " >"
					oRows(i).style.padding = 0

					//设定第一个文本框为有焦点，选中状态
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

			//生成控件框
			if 	(i < this.seedControl.length)
			if (this.seedControl[i] != null  && oRows(i).name != "Virtul")
			{
				//安排新的选择框,当前格<TD></TD>中，插入选择框HTML文档。
				oRows(i).style.padding = 0
				//避免控件覆盖
				if ((this.outerSelect == true && !(this.seedControl[i] .indexOf("<button")>=0)) || this.OuterControl == i || this.OuterControl1 == i || this.OuterControl2 == i || this.OuterControl3 == i || this.OuterControl4 == i)
				{	sInHT = oRows(i).innerHTML
					oRows(i).innerHTML = ""

					oRows(i).innerHTML = "<SPAN ID=" + this.Name + "Cshow" + i + this.ControlValue[i] + ">" + oRows(i).innerHTML + "</SPAN>" + this.seedControl[i].replace(">" + sInHT, " selected>" + sInHT)
					//alert(oRows(i).innerHTML)
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

/*
功能  ：得到当前行的索引值。
作者  ：李翼嵩
时间  ：2000年8月
参数  ：
返回值：索引值。
*/
function _GetRownum()
{
	return this.LightRow * 1;
}

/*
功能  ：刷新表格，清除高亮。
作者  ：李翼嵩
时间  ：2000年8月
参数  ：
返回值：
*/
function _Refresh(withEvent)
{
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

				//当出现选择框时，进行自动删除。
				if (this.outerSelect == true && this.seedControl[this.NotEmpty] != null  && this.preElement != null)
				{    if (this.preElement.childNodes(this.NotEmpty).childNodes.length > 1)
					if (this.preElement.childNodes(this.NotEmpty).childNodes(1).tagName == "SELECT")
					{
						//eval("editnode = " + this.Name + "Text" + this.NotEmpty + ".value")
						var editnode = this.preElement.childNodes(this.NotEmpty).childNodes(1).options(this.preElement.childNodes(this.NotEmpty).childNodes(1).selectedIndex).text
						//alert(editnode)
						if (editnode == "")
							 this.Delete()

					}
				}

	//若不是第一次响应该事件，恢复上一次的正常背景色。

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
		{	//var fastLightRow = document.all(this.Name + "TableBody" + this.FastField).firstChild.childNodes(this.preElement.name*1-this.FirstRow)
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

function _ClearV(NoCount)
{
	//不知为什么，空记录集无法在运行时AddNew，只好出此下策。--- 1
	//window.alert(this.rs_main.RecordCount)
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

	//}

}

function _Clear()
{
	if (this.rs_main.RecordCount >=1)
	{
		this.rs_main.MoveFirst()

		while (this.rs_main.recordcount > 0)
		{
			this.rs_main.Delete()
			//this.rs_main.MoveNext()
		}
		this.MaxSortNumber = 0;
	}

	if (this.rs_main.RecordCount == 0)
		this.IsEmpty = true
	else
		this.IsEmpty = false

	if (this.ShowCount == true )
		eval(this.Name + "Count.innerText = '" + this.JILUSHU + this.rs_main.RecordCount + "'")

	
	//重新排序后，清除高亮显示行
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


//只能出现数字和小数点。
 if (charCode>31 && (charCode<48 ||charCode>57) && (charCode<96 ||charCode>105) && charCode != 46 && charCode != 110 && charCode != 190 && charCode != 37 && charCode != 39  && charCode != 35  && charCode != 36)
   	event.returnValue =false;

//不能同时出现两个小数点。
 if (event.srcElement.value.indexOf(".")>=0 && (charCode == 110 || charCode == 190))
	event.returnValue =false;

}

function _DealBack()
{
}

function _DealKeyPress()
{
//	if (this.UnderKey == true)
		//event.returnValue =false;
		//return
	//if (event.srcElement.id != this.Name + "TableBody")
//	this.UnderKey = true

	//var oNode, oEventFunction
	//eval("oNode = " + this.Name + "TableBody")
//	oEventfunction _= oNode.onkeydown
//	oNode.onkeydown = null
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
		case 37:	//左

			break;

		case 38:	//上
			if (this.currentRow > 0)
			{	iRow -= 1

				eval("valReturn = " + this.BeforeUpDown)
				if (valReturn == false)
					return false
				this.HighLightRow(iRow)

				//if (this.LightRow > 0)
				//	this.LightRow --
				//if (this.LightRow < this.FirstRow)
				//	this.FirstRow = this.LightRow
				//this.ChangePage()
				eval(this.UpDown)
			}
			event.returnValue =false;
			break;

		case 39:	//右

			break;
		case 13:	//回车
			//eval(this.Dbclick)
			//alert(event.srcElement.tagName)
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

		case 40:	//下
			//eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild.childNodes")

			if (this.currentRow < this.rs_main.RecordCount)
			{	iRow += 1
				//在自动删除后再进行鼠标操作，会产生错误，增加此段代码，为了禁止键盘操作下的自动删除。
				
				if (iRow == this.rs_main.RecordCount && this.AddDelete == true && iRow>0)
				{	this.SaveRow(true)
					this.rs_main.AbsolutePosition = iRow
					if (this.rs_main.Fields(this.GetOrder[this.NotEmpty]).value != "")
					{
						//valReturn = true
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
				//Tag.focus()

			}
			event.returnValue =false;
			break;

		case 33:	//PgUp
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

		case 34:	//PgDn
			//eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild.childNodes")
			break;
			this.HighLightRow(-1,true)
			event.returnValue =false;
			break;

		case 46:	//Delete
			if (this.canKeyDelete)
				this.Delete();
			break;

		case 32:	//Delete
			if (event.srcElement.tagName != "INPUT")
				event.returnValue =false;
			break;

		default:
			break;

	}


//	oNode.onkeydown = oEventFunction
//	event.returnValue =false;
//	this.UnderKey = false
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

/*
将字符串转换成unicode
*/
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
	
/*
Function descript -- Transfer recordset (such as table's rs_main) to String witch with field delimit and row delimit
para1 -- field delimit such as ","
para2 -- row delimit such as "|"
para3 -- mask to transfer field, delimit witch comma, such as "1,2,3,4,8"
para4 -- column number to replace value
para5 -- if para4 have value, how to replace value define in this para
return value --- string delimit by  para1 and para2

author: Li Yisong
*/
function _RecordsetToString(fieldDelim,RowDelim,mask,ReplaceColumn,ReplaceTable)
{

	var srcRecordSet = this.rs_main
	var arrAMD = new Array("D","A","M","A"); //"D"删除 "A"增加 "M"修改
   	var arrRow = new Array();
   	var arrMask = mask.split(",");

	//处理增加和修改的纪录
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
	//处理删除的纪录
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

/*
将' 替换成 \\\'
其它符号亦然
*/

function _ReplaceChar(strTrim,chrHave)
{
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
}
