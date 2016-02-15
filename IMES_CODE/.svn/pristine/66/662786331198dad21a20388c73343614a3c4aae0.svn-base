/*
\u529f\u80fd  \uff1a\u521b\u5efa\u8868\u683c\u7c7b\u5bf9\u8c61\u3002
\u4f5c\u8005  \uff1a\u674e\u7ffc\u5d69
\u65f6\u95f4  \uff1a2000\u5e748\u6708
\u53c2\u6570  \uff1aRecordSet -- \u5305\u542b\u8981\u663e\u793a\u7684\u6570\u636e\u8bb0\u5f55\u96c6\u3002
\u8fd4\u56de\u503c\uff1a
*/

function clsTablePrn(RecordSet,ObjectName)
{
//public property
	this.Height = 100;				//\u8868\u683c\u9ad8\u5ea6\uff0c\u4e0d\u5305\u62ec\u6807\u9898\u5934\u3002
	this.Widths = null;				//\u8868\u683c\u5b57\u6bb5\u5bbd\u5ea6\u6570\u7ec4\u3002
	this.TableWidth = 100
	this.modi = new Array();				//\u53ef\u7f16\u8f91\u5b57\u6bb5\u6570\u7ec4 true -- \u53ef\u7f16\u8f91\u3002
	this.rs_main = RecordSet;		//\u6570\u636e\u8bb0\u5f55\u96c6\u3002
	this.Title = true;				//\u662f\u5426\u663e\u793a\u6807\u9898
	this.AddDelete = false;			//\u662f\u5426\u53ef\u4ee5\u6dfb\u52a0\u5220\u9664\u8bb0\u5f55
	this.RowStr	=""					//\u5f97\u5230\u5f53\u524d\u884c\u7684\u5b57\u4e32\u3002
	this.arrColor = new Array("#fffbe7","#e7e7bd") //\u8bb0\u5f55\u884c\u7684\u80cc\u666f\u8272
	this.UseSort = ""				//\u6307\u5b9a\u6392\u5e8f\u7684TDC\u63a7\u4ef6
	this.IsEmpty = false			//\u5224\u65ad\u662f\u5426\u5f53\u524d\u8868\u683c\u662f\u5426\u4e3a\u7a7a
	this.Divide = ","				//\u4f7f\u7528\u7684\u5206\u9694\u7b26
	this.outerSelect = false		//\u4f7f\u7528\u5916\u90e8\u9009\u62e9SELECT\u6807\u7b7e\uff0c\u5c06\u9009\u62e9\u5185\u5bb9\u653e\u5728\u683c\u4e2d\u3002

//public Method
	this.Display = DisplayPrn;			//\u5f97\u5230HTML\u5b57\u4e32\u3002
	this.MouseDown_Event = MouseDown_EventPrn //\u9f20\u6807\u4e8b\u4ef6
	this.AddEvent = AddEventPrn;		//\u589e\u52a0\u4e00\u6761\u8bb0\u5f55\u3002
	this.GetRowNumber = GetRownumPrn	//\u5f97\u5230\u5f53\u524d\u884c\u7d22\u5f15\u503c\u3002
	this.Refresh = RefreshPrn			//\u5237\u65b0\u8868\u683c\u3002
	this.ColumnStyle = ColumnStylePrn	//\u8bbe\u5b9a\u4e00\u5217\u5916\u89c2\u5c5e\u6027\u3002
	this.TableStyle = TableStylePrn	//\u8bbe\u5b9a\u8868\u5916\u89c2\u5c5e\u6027\u3002
	this.HeadStyle = HeadStylePrn		//\u8bbe\u5b9a\u6807\u9898\u5916\u89c2\u5c5e\u6027\u3002
	this.Delete = DelRowPrn			//\u5220\u9664\u5f53\u524d\u884c
	this.Append = AppendPrn			//\u589e\u52a0\u4e00\u884c\uff08\u7a0b\u5e8f\u624b\u52a8\u65b9\u5f0f\uff09
	this.SetRow = SetRowPrn			//\u8bbe\u7f6e\u4e00\u884c\u5185\u5bb9
	this.UseControl = UseControlPrn	//\u4f7f\u7528\u7528\u6237\u6307\u5b9a\u7684\u63d2\u5165\u63a7\u4ef6\u3002
	this.ControlReturn = ControlReturnPrn //\u8bbe\u7f6e\u7528\u6237\u63d2\u5165\u63a7\u4ef6\u6240\u5728\u683c\u7684\u5185\u5bb9\u3002
	this.PageColumn = PageColumnPrn    //\u8bbe\u7f6e\u6a2a\u5411\u5206\u9875\u529f\u80fd
	this.HideColumn = new Array()			//\u9690\u85cf\u6307\u5b9a\u7684\u5b57\u6bb5
	this.seedControl = new Array()
	this.ControlValue = new Array()
	this.FieldsType = new Array()
	this.MaxSize = new Array()
	this.EventRow = EventRowPrn
	this.ClearV = ClearVPrn

//private property and method
	this.Name = ObjectName;			//\u751f\u6210\u5bf9\u8c61\u7684\u540d\u79f0\u3002
    this.PreRow = -1;				//\u9ad8\u4eae\u524d\u4e00\u884c\u8bb0\u5f55\u3002
	this.strColumn = ""				//\u5217\u5c5e\u6027\u5b57\u4e32\u3002
	this.strTable = 'Style="color:black;font-size:9pt"'	//\u8868\u5c5e\u6027\u5b57\u4e32\u3002
	this.strHead = 'Style="font-family:\u5b8b\u4f53;font-size:9pt"'	//\u6807\u9898\u5c5e\u6027\u5b57\u4e32\u3002
	this.AskSum = false;					//\u662f\u5426\u589e\u52a0\u201c\u5408\u8ba1\u201c
	this.SumArr = new Array(RecordSet.Fields.Count);	//\u5408\u8ba1\u5b57\u6bb5\u5408\u8ba1\u503c\u3002
	this.SumDot = 2
	this.EditRow = EditRowPrn;			//\u5bf9\u5f53\u524d\u884c\u8fdb\u884c\u53ef\u7f16\u8f91\u5904\u7406\u3002
	this.UseSum = UseSumPrn;			//\u589e\u52a0\u5408\u8ba1\u5b57\u6bb5\uff0c\u53ef\u91cd\u590d\u8c03\u7528\u3002
	this.currentRow = -1;			//\u8bb0\u5f55\u76ee\u524d\u884c\u7d22\u5f15\u503c\u3002
    this.preElement = null;			//\u4fdd\u5b58\u5f53\u524d\u884c\uff0c\u5f53\u5176\u5b83\u884c\u70b9\u51fb\u9ad8\u4eae\u65f6\u6062\u590d\u8be5\u884c
    this.preColor = null;			//\u4fdd\u5b58\u5f53\u524d\u884c\u989c\u8272\u3002
	this.BackRefresh = BackRefreshPrn  //\u5237\u65b0\u80cc\u666f\u8272
	this.SaveRow = SaveRowPrn			//\u4fdd\u5b58\u9ad8\u4eae\u884c\u7684\u5185\u5bb9\u5230\u8bb0\u5f55\u96c6\u3002
	this.Sort = SortPrn
	this.UseForm = ""
	this.Pages = 1
	this.PageArr = new Array()
	this.NewPage = NewPagePrn
	this.WhichPage = 0
	this.SortColumn = -1			//\u6307\u5b9a\u7531\u54ea\u4e2a\u5b57\u6bb5\u6392\u5e8f
	this.SortAsc = ""				//\u6392\u5e8f\u65b9\u6cd5\uff1a""\u4e3a\u6b63\u5e8f\uff0c"-"\u4e3a\u9006\u5e8f\u3002
	this.HighLightRow = HighLightRowPrn
	this.DealKeyPress = DealKeyPressPrn
	this.DealBack = DealBackPrn
	//this.UnderKey = false
	this.ShowCount = false
	this.FreeSelect = FreeSelectPrn
	this.StrFree = ""
	this.StrCookie = ""
	this.ShowFree = ShowFreePrn
	this.SetFree = SetFreePrn
	this.NotEmpty = -1

	this.Dbclick = "this.Dbclick = ''"
	this.SortLeft = 0
	this.RestorScroll = RestorScrollPrn

	this.Filter = FilterResetPrn
	this.strFilter = this.rs_main.Fields(0).Name + " <> UndEfinedp"
	this.ControlPath = "../../CommonControl/"
	this.NoKey = false			//true -- \u4e0d\u652f\u6301\u952e\u76d8\u63a7\u5236\u4e0a\u4e0b
	this.UpDown = null			//\u9ad8\u4eae\u884c\u4e0a\u4e0b\u79fb\u52a8\u65f6\u8c03\u7528\u7684\u51fd\u6570

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


function ShowFreePrn()
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

function SetFreePrn()
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
		{	//\u663e\u793a\u81ea\u5b9a\u4e49\u5b57\u6bb5
			var ArrFree = sCookie.split(",")
			this.NewPage(0,ArrFree)
		}
		else
		{
			if (this.PageArr.length > 1)
				//\u82e5\u5206\u9875\uff0c\u663e\u793a\u7b2c\u4e00\u9875
				this.NewPage(1)
			else
			{	//\u6ca1\u5206\u9875\uff0c\u5219\u663e\u793a\u5168\u90e8
				var ArrAll = new Array()
				for (var ki = 0; ki<this.Widths.length; ki++)
					ArrAll[ki] = ki
				this.NewPage(0,ArrAll)

				//\u6309\u94ae\u4e0d\u4e0b\u9677
				if (this.StrFree != "")
					eval(this.UseForm + this.Name + "0img.src = '" + this.ControlPath + "Images/" + "0.gif'")

			}
		}
	}

}

/*
\u529f\u80fd  \uff1a\u81ea\u7531\u9009\u62e9\u6307\u5b9a\u5b57\u6bb5\u3002
\u4f5c\u8005  \uff1a\u674e\u7ffc\u5d69
\u65f6\u95f4  \uff1a2000\u5e7411\u6708
\u53c2\u6570  \uff1aSaveName \u4fdd\u5b58\u5185\u5bb9\u7684\u540d\u79f0\u3002
*/
function FreeSelectPrn(SaveName)
{
	this.StrFree = SaveName
	//this.Pages ++
}
/*
\u529f\u80fd  \uff1a\u9ad8\u4eae\u6307\u5b9a\u884c\u3002
\u4f5c\u8005  \uff1a\u674e\u7ffc\u5d69
\u65f6\u95f4  \uff1a2000\u5e7411\u6708
\u53c2\u6570  \uff1aRowNum \u884c\u53f7\uff08base 0\uff09
*/
function HighLightRowPrn(RowNum)
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
\u529f\u80fd  \uff1a\u662f\u5426\u5206\u9875\u663e\u793a\u5b57\u6bb5\uff0c\u6307\u5b9a\u5206\u51e0\u9875\u663e\u793a\u3002
\u4f5c\u8005  \uff1a\u674e\u7ffc\u5d69
\u65f6\u95f4  \uff1a2000\u5e748\u6708
\u53c2\u6570  \uff1aColumns \u6307\u5b9a\u6392\u5e8f\u7684\u5b57\u6bb5
*/
function PageColumnPrn(Page0,Page1,Page2,Page3,Page4)
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
\u529f\u80fd  \uff1a\u662f\u5426\u5206\u9875\u663e\u793a\u5b57\u6bb5\u3002
\u4f5c\u8005  \uff1a\u674e\u7ffc\u5d69
\u65f6\u95f4  \uff1a2000\u5e748\u6708
\u53c2\u6570  \uff1aPageNum \u6307\u5b9a\u663e\u793a\u7b2c\u51e0\u9875
        Free \u662f\u5426\u4e3a\u81ea\u7531\u9009\u62e9\u9875
*/
function NewPagePrn(PageNum,Free,SetHide)
{
	//\u5f97\u5230\u8be5\u9875\u7684\u5b57\u6bb5\u4eec
	var temp
	if (Free != null)
		this.PageArr[PageNum] = Free

	temp = this.PageArr[PageNum]
	if (temp == null || temp =="null" || temp == "")
	{
		this.SetFree()
			return
	}

	//\u5c06\u6240\u6709\u5b57\u6bb5\u90fd\u4e0d\u663e\u793a\uff0c\u53ea\u663e\u793a\u8be5\u9875\u5b57\u6bb5
	for(var i=0; i<this.Widths.length; i++)
		this.HideColumn[i] = false
	for(var i=0; i<temp.length; i++)
		this.HideColumn[temp[i]] = true

	if (SetHide == true)
		return

	if (this.preElement != null)
	{	//\u8bb0\u5f55\u4e0b\u5f53\u524d\u9ad8\u4eae\u884c\u7684\u4f4d\u7f6e
		var oldPos
		eval("oldPos = " + this.Name + "TableSpan.scrollTop")
		oldPos = this.preElement.offsetTop - oldPos
	}

	//\u5237\u65b0\u540e\u663e\u793a\u4e0e\u4e0d\u663e\u793a\u7684\u8bbe\u7f6e\u624d\u751f\u6548\u3002
	var i,oRows,iTemp

	//\u5c06\u987b\u9690\u85cf\u5b57\u6bb5\u9690\u85cf\u3002
	eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild.childNodes")
	for (i=0; i<oRows.length; i++)
	{
			for (var n=0; n<this.HideColumn.length; n++)
				if (this.HideColumn[n] != true)
					oRows(i).childNodes(n).style.display = "none"
				else
					oRows(i).childNodes(n).style.display = "block"
	}


	iTemp = 23 //\u8868\u7684\u65b0\u5bbd\u5ea6

	//\u5c06\u987b\u9690\u85cf\u8868\u5934\u3001\u8868\u5c3e\u9690\u85cf\u3002
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

	//\u5f53TableBody\u5bbd\u5ea6\u53d8\u5c0f\u65f6\uff0c\u6a2a\u5411\u6eda\u52a8\u6761\u6eda\u52a8\u8303\u56f4\u5374\u4e0d\u6539\u53d8\uff0c\u53ea\u6709\u4f7f\u7528\u4ee5\u4e0b\u65b9\u6cd5\u624d\u80fd\u6539\u53d8\u3002
	var oNode
	eval("oNode = " + this.Name + "TableBody.lastChild.lastChild")
	if (oNode.style.height == "20px")
		oNode.style.height = "21px"
	else
		oNode.style.height = "20px"
	//\u5f53TableBody\u5bbd\u5ea6\u53d8\u5c0f\u65f6\uff0c\u6a2a\u5411\u6eda\u52a8\u6761\u6eda\u52a8\u8303\u56f4\u5374\u4e0d\u6539\u53d8\uff0c\u53ea\u6709\u4f7f\u7528\u4ee5\u4e0a\u65b9\u6cd5\u624d\u80fd\u6539\u53d8\u3002

/*	???

	if (this.preElement != null)
	{
		this.preElement.style.backgroundColor = "midnightblue"
		this.preElement.style.color = "white"
	}
*/
	this.WhichPage = PageNum

	//\u91cd\u65b0\u5b9a\u4f4d\u9ad8\u4eae\u884c\u3002
	if (this.preElement != null)
	{	var iTop
		iTop = this.preElement.offsetTop - oldPos
		eval(this.Name + "TableSpan.scrollTop = " + iTop)
	}


	//\u5c06\u6240\u6709\u6309\u94ae\u62ac\u8d77\uff0c\u53ea\u5c06\u8be5\u9875\u6309\u94ae\u4e0b\u9677
	if (this.StrFree != "")
		eval(this.UseForm + this.Name + "0img.src = '" + this.ControlPath + "Images/" + "0.gif'")
	for (var j=1; j<this.Pages; j++)
		eval(this.UseForm + this.Name + j + "img.src = '" + this.ControlPath + "Images/" + j +".gif'")

	eval(this.UseForm + this.Name + PageNum + "img.src = '" + this.ControlPath + "Images/" + PageNum +"_.gif'")

}



/*
\u529f\u80fd  \uff1a\u662f\u5426\u6392\u5e8f\u3002
\u4f5c\u8005  \uff1a\u674e\u7ffc\u5d69
\u65f6\u95f4  \uff1a2000\u5e748\u6708
\u53c2\u6570  \uff1aColumns \u6307\u5b9a\u6392\u5e8f\u7684\u5b57\u6bb5
*/
function SortPrn(column)
{

	//\u82e5\u662f\u5e8f\u53f7\u5b57\u6bb5\u5219\u4e0d\u6392\u5e8f
	if (this.rs_main.Fields(column).Name == "\u5e8f\u53f7")
		return

	var Ascend, Element

	Ascend = "" //\u6b63\u5e8f\u8fd8\u662f\u53cd\u5e8f
	Element = window.event.srcElement

	//\u5224\u65ad\u662f\u5426\u6709\u8bb0\u5f55\u53ef\u4f9b\u6392\u5e8f
	if (this.rs_main.RecordCount >1)
	{
		//\u662f\u5426\u70b9\u51fb\u6807\u9898\u540e\u7531\u4e8b\u4ef6\u89e6\u53d1\u6392\u5e8f\u3002
		if (Element.className == "HeadSort")
		{
			//\u662f\u5426\u8fd8\u662f\u524d\u4e00\u6b21\u6392\u5e8f\u7684\u5217\u3002
			if (this.SortColumn == column)
			{
				//\u82e5\u662f\u5219\u53d8\u6362\u6392\u5e8f\u65b9\u5f0f\u3002
				if (this.SortAsc == "")
					//\u6b63-->\u53cd
					this.SortAsc = "-"
				else
					//\u53cd-->\u6b63
					this.SortAsc = ""
			}
			else
			{	//\u5426\u5219\u6309\u7167\u6b63\u5e8f
				this.SortColumn = column
				this.SortAsc = ""
			}

		}
		eval("this.SortLeft = " + this.Name + "TableSpan.scrollLeft" )
		//\u5237\u65b0\u754c\u9762\uff0c\u4ee5\u4fbf\u4fdd\u5b58\u5f53\u524d\u884c\u4fe1\u606f
		this.Refresh()
		//\u6267\u884c\u6392\u5e8f\uff0c\u8bbe\u5b9aTDC\u63a7\u4ef6\u6392\u5e8f\u3002
		eval(this.UseSort + ".CaseSensitive = 'FALSE'")
		eval(this.UseSort + ".Sort = " + "'" + this.SortAsc + this.rs_main.Fields(column).Name + "'")
		this.Filter()
	}
}

function FilterResetPrn(strFilter)
{
	var sFilter
	sFilter = ""
	if (strFilter != "" && strFilter != null)
		sFilter = " & (" + strFilter + ")"

	this.strFilter = this.rs_main.Fields(0).Name + " <> UndEfinedp" + sFilter
	eval(this.UseSort + ".object.Filter = '" + this.strFilter + "'")
	eval(this.UseSort + ".Reset()")
}


function RestorScrollPrn()
{
	eval(this.Name + "TableSpan.scrollLeft = " + "this.SortLeft")


}


/*
\u529f\u80fd  \uff1a\u8bbe\u7f6e\u7528\u6237\u63d2\u5165\u63a7\u4ef6\u6240\u5728\u683c\u7684\u5185\u5bb9\u3002
\u4f5c\u8005  \uff1a\u674e\u7ffc\u5d69
\u65f6\u95f4  \uff1a2000\u5e748\u6708
\u53c2\u6570  \uff1astrVal -- \u8981\u663e\u793a\u7684\u5185\u5bb9\u3002
        Col -- \u63d2\u5165\u5b57\u6bb5\u7684\u4f4d\u7f6e\u3002
*/
function ControlReturnPrn(strVal,col)
{
	if (this.seedControl.length >= col)
		eval(this.Name + "Cshow" + col + ".innerText = strVal")
}

/*
\u529f\u80fd  \uff1a\u4f7f\u7528\u7528\u6237\u6307\u5b9a\u63d2\u5165\u63a7\u4ef6\u3002
\u4f5c\u8005  \uff1a\u674e\u7ffc\u5d69
\u65f6\u95f4  \uff1a2000\u5e748\u6708
\u53c2\u6570  \uff1aCol -- \u63d2\u5165\u5b57\u6bb5\u7684\u4f4d\u7f6e\u3002
        strContent -- \u63d2\u5165\u63a7\u4ef6\u7684\u5185\u5bb9\u5b57\u4e32(HTML)
        ShowChange -- true   \u663e\u793a\u63a7\u4ef6\u63a7\u5236\u503c
        ShowChange -- false  \u4e0d\u663e\u793a\u63a7\u4ef6\u63a7\u5236\u503c
        Position -- "Front"  \u63a7\u4ef6\u663e\u793a\u5728\u503c\u524d
        Position -- "Behind"  \u63a7\u4ef6\u663e\u793a\u5728\u503c\u540e
*/
function UseControlPrn(Col,strContent,ShowChange,Position)
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
\u529f\u80fd  \uff1a\u6dfb\u52a0\u5408\u8ba1\u5b57\u6bb5\u3002\u82e5\u591a\u6b21\u8c03\u7528\uff0c\u5219\u5bf9\u591a\u4e2a\u5b57\u6bb5\u8fdb\u884c\u5408\u8ba1\u3002
\u4f5c\u8005  \uff1a\u674e\u7ffc\u5d69
\u65f6\u95f4  \uff1a2000\u5e748\u6708
\u53c2\u6570  \uff1aColumn -- \u9700\u8981\u5408\u8ba1\u7684\u5b57\u6bb5\u7f16\u53f7\uff08base 0\uff09\u3002
\u8fd4\u56de\u503c\uff1a
*/
function UseSumPrn(Column)
{
	if (Column <= this.Widths.length)
	{
		this.AskSum = true;
		//\u82e5\u8bb0\u5f55\u4e3a\u7a7a\uff0c\u5219\u4e0d\u5408\u8ba1\u3002
		this.SumArr[Column] = 0
		if (this.rs_main.RecordCount > 0 )
		{
			this.rs_main.MoveFirst()
			//\u4f9d\u6b21\u8fdb\u884c\u5408\u8ba1
			while (!this.rs_main.EOF)
			{
				//\u82e5\u8be5\u5b57\u6bb5\u4e0d\u662f\u6570\u503c\u578b\uff0c\u5219\u4e0d\u8fdb\u884c\u5408\u8ba1\u3002
				if (typeof(this.rs_main.Fields(Column).value) != "number")
				{	this.rs_main.MoveNext()
					continue
				}
				//\u8fdb\u884c\u5408\u8ba1
				if (this.rs_main.Fields(Column).value != null)
					this.SumArr[Column] += parseFloat(this.rs_main.Fields(Column).value)
				this.rs_main.MoveNext()
			}

			this.SumArr[Column] = Math.round(this.SumArr[Column] * Math.pow(10,this.SumDot)) / Math.pow(10,this.SumDot)
		}
	}
}


/*
\u529f\u80fd  \uff1a\u8bbe\u5b9a\u5217\u5916\u89c2\u5c5e\u6027\u3002
\u4f5c\u8005  \uff1a\u674e\u7ffc\u5d69
\u65f6\u95f4  \uff1a2000\u5e748\u6708
\u53c2\u6570  \uff1aColNum -- \u5217\u5b57\u6bb5\u7f16\u53f7\uff08base 0\uff09
        strStyle -- \u6837\u5f0f\u5355\u5b57\u4e32\u3002\uff08\u5982\uff1a"Width:80; color:red"\uff09
\u8fd4\u56de\u503c\uff1a
*/
function ColumnStylePrn(ColNum,strStyle)
{

	this.strColumn += "   ." + this.Name + ColNum + "{" + strStyle + "}    "
}

/*
\u529f\u80fd  \uff1a\u8bbe\u5b9a\u8868\u5916\u89c2\u5c5e\u6027\u3002
\u4f5c\u8005  \uff1a\u674e\u7ffc\u5d69
\u65f6\u95f4  \uff1a2000\u5e748\u6708
\u53c2\u6570  \uff1astrStyle -- \u6837\u5f0f\u5355\u5b57\u4e32\u3002\uff08\u5982\uff1a"Width:80; color:red"\uff09
\u8fd4\u56de\u503c\uff1a
*/
function TableStylePrn(strStyle)
{

	this.strTable = ' Style="' + strStyle + ' "'
}

/*
\u529f\u80fd  \uff1a\u8bbe\u5b9a\u6807\u9898\u5916\u89c2\u5c5e\u6027\u3002
\u4f5c\u8005  \uff1a\u674e\u7ffc\u5d69
\u65f6\u95f4  \uff1a2000\u5e748\u6708
\u53c2\u6570  \uff1astrStyle -- \u6837\u5f0f\u5355\u5b57\u4e32\u3002\uff08\u5982\uff1a"Width:80; color:red"\uff09
\u8fd4\u56de\u503c\uff1a
*/
function HeadStylePrn(strStyle)
{

	this.strHead =  strStyle
}


/*
\u529f\u80fd  \uff1a\u8fdb\u884c\u5305\u542b\u663e\u793a\u8868\u683c\u7684HTML\u5b57\u4e32\u7684\u751f\u6210\u3002
\u4f5c\u8005  \uff1a\u674e\u7ffc\u5d69
\u65f6\u95f4  \uff1a2000\u5e748\u6708
\u53c2\u6570  \uff1a
\u8fd4\u56de\u503c\uff1a\u8868\u683cHTML\u5b57\u4e32\u3002
*/
function DisplayPrn(strJudge,NA,noAdd)
{
	//NewPage(this.WhichPage,null,true)
	//\u5c06\u6240\u6709\u5b57\u6bb5\u90fd\u4e0d\u663e\u793a\uff0c\u53ea\u663e\u793a\u8be5\u9875\u5b57\u6bb5
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
	//******************************************** \u5e38\u91cf\u5b9a\u4e49 ***********************************
	var TABLE_BEGIN = "<TABLE name='" + this.Name + "TableAll' Id=" + this.Name + "TableAll  width=" + this.TableWidth + "px border=1 cellpadding=0 cellspacing=0 onmouseup='return " + this.Name + ".MouseDown_Event()'><TR><TD>"
	var TABLE_END = "</TD></TR></TABLE>"

	sumWidth -= 4

	var TABLE_HEAD_H = "<SPAN id='" + this.Name + "SpanHead' style='WIDTH:"+ this.TableWidth + ";OVERFLOW:hidden'><TABLE id='" + this.Name + "TableHead' cellpadding=2  cellspacing=0 style='border-style:solid;border-width:1px;border-color:black' style='width:" + sumWidth + "px'>"
	var TABLE_HEAD_T = "<SPAN id='" + this.Name + "SpanTail' style='WIDTH:"+ this.TableWidth + ";OVERFLOW:hidden'><TABLE id='" + this.Name + "TableTail' cellpadding=2  cellspacing=0 style='border-style:solid;border-width:1px;border-color:black' style='width:" + sumWidth + "px'>"

	sumWidth -= 18
	var TABLE_HEAD = "<TABLE id='" + this.Name + "TableBody' cellpadding=2  cellspacing=1 bordercolor=Silver bgcolor=Gray " + this.strTable + " onKeyDown='" + this.Name + ".DealKeyPress()' style='width:" + sumWidth + "px'>"
	var TABLE_TAIL_H = "</TABLE></SPAN>"
	var TABLE_TAIL = "</TABLE>"

	var HEAD_ROW_BEGIN = "<TR style='word-break:break-all;word-wrap:break-word;border-color:black' name=Head bgcolor=#cebe9c align=center>"
	var HEAD_ROW_END = "</TR>"

	var HEAD_BODY_BEGIN = "<TD " + "Style='border-right:1px solid black;" + this.strHead +  "' ><b>"
	var HEAD_BODY_END = "</TD>"

	var TAIL_BODY_BEGIN = "<TD " + "Style='border-right:1px solid black;' >"
	var TAIL_BODY_END = "</TD>"

	var ROW_BEGIN = "<TR classid='" + this.Name + "TableRow' align=left bordercolor=Silver name=rownum id=rowid>"
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


	sUse[0] =  TABLE_HEAD
		var sDisplay = ""
	//\u662f\u5426\u663e\u793a\u6807\u9898\u3002
	if (this.Title)
	{	//\u5efa\u7acb\u8868\u5934
		sSort = ""
		sUse[sUse.length] =  HEAD_ROW_BEGIN
		//\u5efa\u7acb\u8868\u7684\u6807\u9898\u5934
		for (field=0; field<this.Widths.length; field++)
		{
			//\u8bbe\u7f6e\u663e\u793a\u5b57\u6bb5
			if (this.HideColumn.length > 0)
			{
				if (this.HideColumn[field] != true)
				{	continue;
					sDisplay = " style='display:none'"

				}
				else
					sDisplay = ""
			}

			//\u662f\u5426\u6307\u5b9a\u5b57\u6bb5\u5bbd\u5ea6
			iWidth = this.Widths[field] - 5
			sWidth = HEAD_BODY_BEGIN.replace("<TD","<TD width=" + iWidth  + "px id=" + this.Name + "H" + field + sDisplay)
			sUse[sUse.length] = sWidth
			if (this.UseSort == "")
				//\u4e0d\u6307\u5b9a\u6392\u5e8f
				sUse[sUse.length] = this.rs_main.Fields(field).Name
			else
			{	//\u6307\u5b9a\u6392\u5e8f
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

		//\u6eda\u52a8\u6761\u4e4b\u4e0a
		sUse[sUse.length] =  "<TH width=13 " + this.strHead + "></TH>"
		//\u5efa\u7acb\u7eaa\u5f55\u5934\u3002
		sUse[sUse.length] =  HEAD_ROW_END
	}

	//\u5904\u7406\u8868\u683c
	//\u4ece\u8d77\u59cb\u884c\u5f00\u59cb\u5b89\u6392lines\u884c\u7eaa\u5f55
	if (this.rs_main.RecordCount > 0 )
	{
		this.rs_main.MoveFirst()
	}
	//sBody[0] = TABLE_HEAD
	sStyle = ""
	var sBackC = ""
	var jj = 0
	for (j = 0; j<this.rs_main.RecordCount; j++)
	{	//\u82e5\u7ed3\u5c3e\uff0c\u5219\u8df3\u51fa\u5faa\u73af\u3002
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

		//\u8bbe\u7f6e\u80cc\u666f\u8272
		sBackC = " Style='background-color:" + this.arrColor[jj % 2] + "; height:20px; color:black; word-break:break-all;word-wrap:break-word;'"
		sBody[sBody.length] =  ROW_BEGIN.replace("rownum", ""+ jj + sBackC)
		sBody[sBody.length-1] =  sBody[sBody.length-1].replace("rowid", "Row"+ jj)
		jj += 1
		//\u5b89\u6392\u8bb0\u5f55\u4e2d\u7684\u5404\u4e2a\u5b57\u6bb5\u3002
		for (field=0; field<this.Widths.length; field++)
		{	//\u662f\u5426\u6307\u5b9a\u5bbd\u5ea6\u3002

			//\u8bbe\u7f6e\u663e\u793a\u5b57\u6bb5
			if (this.HideColumn.length > 0)
			{
				if (this.HideColumn[field] != true)
				{	sDisplay = " style='display:none'"
					continue;
				}
				else
					sDisplay = ""
			}

			sWidth = CELL_BEGIN.replace("FFIIEE","" + field + sDisplay)

			sBody[sBody.length] = sWidth
			if (this.rs_main.Fields(field).Name == "\u5e8f\u53f7")
			{	sBody[sBody.length] =  jj + ""
				this.rs_main.Fields(field).Value = jj + ""
			}
			else
			{
			    var tempValue =  this.rs_main.Fields(field).Value;
			    if(tempValue==undefined || tempValue=="undefined")
			        tempValue="";
			    if(/^\d{4}-\d{1,2}-\d{1,2}$/.test(tempValue))
                    tempValue= "\u001D"+tempValue;
			    if(/^\d{9,}$/.test(tempValue))
				     tempValue= "\u001D"+tempValue;

                tempValue=tempValue.toString();
                tempValue = tempValue.split("'").join("\'");
                tempValue = tempValue.split('"').join('\"');

                //tempValue = tempValue.replace('<','\u3008');
                while(tempValue.search("<")!=-1)
		            tempValue = tempValue.replace("<", "&lt;")


				  sBody[sBody.length] =  tempValue;
	 		}
			sBody[sBody.length] = CELL_END
		}

		//\u8bb0\u5f55\u7ed3\u675f\u3002
		sBody[sBody.length] =  ROW_END
		//\u4e0b\u4e00\u6761\u8bb0\u5f55\u3002
		this.rs_main.MoveNext()
	}



	//\u8bb0\u5f55\u7ed3\u675f\u3002
	sBody[sBody.length] =  ROW_END
	sBody[sBody.length] =  TABLE_TAIL
	sWidth = sBody.join("")
	//\u662f\u5426\u6307\u5b9a\u9ad8\u5ea6\u3002\u82e5\u6307\u5b9a\uff0c\u5219\u4f1a\u81ea\u52a8\u4ea7\u751f\u6eda\u52a8\u6761\u3002
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

	//\uff08\u8868\u5c3e\uff09\u662f\u5426\u663e\u793a\u6c47\u603b
	//\u662f\u5426\u663e\u793a\u6c47\u603b
	if (this.AskSum)
	{

	//\u5efa\u7acb\u8868\u5c3e
		sUse[sUse.length] = TABLE_HEAD_T + HEAD_ROW_BEGIN.replace("<TR ","<TR id=" + this.Name + "Sums ")
		//\u5efa\u7acb\u8868\u7684\u6807\u9898\u5934
		for (field=0; field<this.Widths.length; field++)
		{
			//\u8bbe\u7f6e\u663e\u793a\u5b57\u6bb5
			if (this.HideColumn.length > 0)
			{
				if (field == 0)
				{	if (this.HideColumn[field] != true)
					{	sDisplay = " style='display:none;text-align:right'"
						continue;
					}
					else
						sDisplay = " style='font-size:9pt';text-align:right'"
				}
				else
				{	if (this.HideColumn[field] != true)
					{	sDisplay = " style='display:none;font-size:9pt;font-weight:400; text-align:right'"
						continue;
					}
					else
						sDisplay = " style='font-size:9pt;font-weight:400; text-align:right'"
				}
			}


			//\u662f\u5426\u6307\u5b9a\u5b57\u6bb5\u5bbd\u5ea6
			iWidth = this.Widths[field] - 5
			sWidth = TAIL_BODY_BEGIN.replace("<TD","<TD width=" + iWidth  + "px id=" + this.Name + "T" + field + sDisplay)
			sUse[sUse.length] = sWidth

			if (field==0)
				sUse[sUse.length] = "\u5408\u8ba1"
			else
				if (this.SumArr[field] != null)
					sUse[sUse.length] = this.SumArr[field]
				else
					sUse[sUse.length] = "\u3000"

			sUse[sUse.length] = TAIL_BODY_END
		}
		sUse[sUse.length] = "<TD width=13 " + this.strHead + "></TD>"
		sUse[sUse.length] = HEAD_ROW_END + TABLE_TAIL_H
	}



	//\u8fd4\u56de\u8868\u683c\u7684HTML\u5b57\u7b26\u4e32\u3002
	//sUse[sUse.length] = TABLE_END

	//\u662f\u5426\u589e\u52a0\u9875\u9009\u62e9
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
		//\u662f\u5426\u663e\u793a\u8bb0\u5f55\u6570
		var sCount = ""
		if (this.ShowCount == true)
			sCount = "\u8bb0\u5f55\u6570\uff1a" + this.rs_main.RecordCount

		//\u662f\u5426\u663e\u793a\u81ea\u7531\u9009\u62e9\u5b57\u6bb5
		var sFree = ""
		if (this.StrFree != "")
		{
			sFree = SET_BUTTON
			sHide = FREE_BUTTON + sHide
		}

		//sTemp = "<TABLE width=" + this.TableWidth + "><TR><TD align=left id=" + this.Name + "Count style='font-family:\u5b8b\u4f53;font-size:9pt'>" + sCount + "</TD><TD align=middle>" + sFree + "</TD><TD align=right>" + sHide + "</TD></TR></TABLE>" + sTemp
	}
	//\u8bbe\u7f6e\u5217\u683c\u5f0f
	sTemp = sTemp + "   " + "<Style> <!--" + this.strColumn + sStyle + "--> </style>"

	//\u4e0d\u77e5\u4e3a\u4ec0\u4e48\uff0c\u7a7a\u8bb0\u5f55\u96c6\u65e0\u6cd5\u5728\u8fd0\u884c\u65f6AddNew\uff0c\u53ea\u597d\u51fa\u6b64\u4e0b\u7b56\u3002--- 1
	if (this.rs_main.RecordCount < 1 && noAdd != true)
	{
		this.rs_main.AddNew()
		this.rs_main(0) = "UndEfinedp"

		this.IsEmpty = true
	}
	else
		this.IsEmpty = false

	//\u91cd\u65b0\u6392\u5e8f\u540e\uff0c\u6e05\u9664\u9ad8\u4eae\u663e\u793a\u884c
	this.preElement = null
	this.PreRow = -1
	this.currentRow = -1;
	this.RowStr = ""

	return sTemp

}



/*
\u529f\u80fd\uff1a  \u5237\u65b0\u80cc\u666f\u8272\u3002
\u4f5c\u8005\uff1a  \u674e\u7ffc\u5d69
\u65e5\u671f\uff1a  2000.7
\u8fd4\u56de\u503c\uff1a\u8fd4\u56de\u9f20\u6807\u5904\u7684\u6574\u6761\u8bb0\u5f55\u5185\u5bb9\u3002\u5b57\u6bb5\u4e0e\u5b57\u6bb5\u95f4\u7528\u7a7a\u683c\u5206\u5f00\u3002
*/
function BackRefreshPrn()
{
	var i,j,oRows, oNode, iTemp
	//\u6e05\u9664\u5197\u4f59\u8bb0\u5f55
	this.ClearV()

	//\u662f\u5426\u5206\u9875\u663e\u793a
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
	//\u5224\u65ad\u8868\u662f\u5426\u591f\u9ad8\u5ea6
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
			oTemp.innerText = "\u3000"
			oTemp.name = "Virtul"
			if (this.HideColumn.length > 0)
				if (this.HideColumn[field] == false)
				{
					continue;
					oTemp.style.display = "none"
				}
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


	//\u5168\u90e8\u884c
	eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild.childNodes")
	for (i=0; i<oRows.length; i++)
	{
			//\u6742\u80cc\u666f\u8272
			oRows(i).style.backgroundColor = this.arrColor[i % 2]
			oRows(i).style.color = "black"
			oRows(i).style.height = "20px"
			//\u5b89\u6392\u884c\u53f7
			//if (oRows(i).classid == "TableRow")
			oRows(i).name = i
	}
}



/*
\u529f\u80fd\uff1a  \u4ece\u8868\u683c\u4e2d\u5220\u9664\u9009\u4e2d\u7684\u884c\u3002
\u4f5c\u8005\uff1a  \u674e\u7ffc\u5d69
\u65e5\u671f\uff1a  2000.7
*/
function DelRowPrn()
{
	var oRows
	//\u662f\u5426\u6709\u9009\u4e2d\u7684\u884c
	if (this.preElement != null)
		if (this.preElement.firstChild.name != "Virtul")
		{
			//\u5220\u9664\u8be5\u884c
			eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild.removeChild(this.preElement)")
			this.rs_main.MoveFirst()
			this.rs_main.Move(this.PreRow)
			this.rs_main.Fields(0).value = "UndEfinedp"
			this.rs_main.Delete(1)
			//this.rs_main.MoveFirst()
			//this.rs_main.Update()
			//\u9009\u4e2d\u6807\u5fd7\u7f6e\u7a7a
			this.preElement = null
			this.PreRow = -1
			this.currentRow	= -1
			this.RowStr = ""

			//\u5237\u65b0\u80cc\u666f
			this.BackRefresh()

			this.ClearV()

		}
}


/*
\u529f\u80fd\uff1a  \u8bbe\u7f6e\u9009\u79cd\u884c\u7684\u5185\u5bb9\u3002
\u4f5c\u8005\uff1a  \u674e\u7ffc\u5d69
\u65e5\u671f\uff1a  2000.7
\u53c2\u6570\uff1a  Content -- \u8be5\u884c\u5185\u5bb9\uff0c\u4ee5","\u5206\u9694\u3002
*/
function SetRowPrn(Content,NoSet)
{
	var oNodes,i,strTD
	if (this.preElement == null | this.preElement.firstChild.name == "Virtul")
		return null
	//\u751f\u6210\u6570\u7ec4
	strTD = Content.split(this.Divide)
	//\u5f97\u5230\u6240\u6709\u5355\u5143\u683c\u3002
	oNodes = this.preElement.childNodes
	this.rs_main.MoveFirst()
	this.rs_main.Move(this.PreRow)
	//\u5206\u522b\u8d4b\u503c
	for (i=0;i<oNodes.length;i++)
	{
		//\u82e5\u8d4b\u503c\u5b57\u6bb5\u5df2\u8d4b\u5b8c\uff0c\u5219\u9000\u51fa
		if ( i > strTD.length)
			break

		//\u5f53\u4e0d\u662f\u63d2\u5165\u63a7\u4ef6\u65f6\uff0c\u624d\u8d4b\u503c
		if (this.seedControl[i] == null || this.modi[i] == true)
		{
			if (NoSet == i)
				continue;
			//\u9a8c\u8bc1\u662f\u5426\u4e3a\u6570\u503c
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
\u529f\u80fd\uff1a  \u8c03\u7528\u8005\u6dfb\u52a0\u4e00\u884c\u3002
\u4f5c\u8005\uff1a  \u674e\u7ffc\u5d69
\u65e5\u671f\uff1a  2000.7
\u53c2\u6570\uff1a  Content -- \u8be5\u884c\u5185\u5bb9\uff0c\u4ee5","\u5206\u9694\u3002
*/

function AppendPrn(Content)
{	var oNode,i,strTD,oNodes
	strTD = Content.split(this.Divide)
	//\u6dfb\u52a0\u4e00\u884c
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
\u529f\u80fd\uff1a  \u8c03\u7528\u8005\u624b\u52a8\uff0c\u6216\u70b9\u51fb\u8868\u683c\u81ea\u52a8\u6dfb\u52a0\u4e00\u884c\u3002
\u4f5c\u8005\uff1a  \u674e\u7ffc\u5d69
\u65e5\u671f\uff1a  2000.7
\u53c2\u6570\uff1a  bAppend -- \u662f\u5426\u624b\u52a8
		Cells   -- \u5355\u5143\u5185\u5bb9\u6570\u7ec4
*/
function AddEventPrn(bAppend,Cells)
{
	if (bAppend !=true & this.AddDelete!=true )
		return null
	//\u63d2\u5165\u4e00\u884c\u8bb0\u5f55
	var oNode, oRows, strCells, fields, iRows, oTemp
	//this.ClearV()
	//window.alert(this.rs_main.RecordCount)

	this.rs_main.Addnew()
	this.rs_main.Update()
	eval("iRows = " + this.Name + "TableSpan.firstChild.firstChild.childNodes.length ")
	//\u751f\u6210\u884c\u5143\u7d20
	oNode = document.createElement("TR");
	oNode.classid = this.Name + "TableRow"
	oNode.align = "left"
	oNode.bordercolor = "Silver"
	oNode.name = "" + iRows
	oNode.style.wordBreak = "break-all"
	//\u751f\u6210\u884c\u5185\u5355\u5143\u683c\u5143\u7d20
	for (field=0; field<this.Widths.length; field++)
	{
		oTemp = document.createElement("TD")
		oTemp.style.wordBreak = "break-all"
		if (this.HideColumn.length > 0)
			if (this.HideColumn[field] == false)
			{
				continue;
				oTemp.style.display = "none"

			}
		if (bAppend)
			oTemp.innerText = Cells[field]
		oTemp.className = field
		oNode.insertBefore(oTemp)
		oTemp = null
	}

	//\u5f97\u5230\u8868\u683c
	eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild") //.childNodes


	oTemp = oRows.lastChild
	if (oTemp.id == this.Name + "BlankBack")
		//\u82e5\u6709\u7528\u4e8e\u586b\u5145\u80cc\u666f\u7684\u7a7a\u884c\uff0c\u5219\u5220\u9664\u8be5\u884c
		oRows.removeChild(oTemp);

	//\u5728\u5bf9\u7684\u4f4d\u7f6e\u6dfb\u52a0\u8be5\u884c\u3002
	//\u7531\u8868\u683c\u81ea\u52a8\u6dfb\u52a0
	eval("oRows.insertBefore(oNode," + this.Name + "AddRow)")

	//\u5237\u65b0\u80cc\u666f
	this.BackRefresh();
	//\u6a21\u62df\u9f20\u6807\u70b9\u51fb\uff0c\u9ad8\u4eae\u8be5\u884c\u3002
	this.currentRow += 5
	this.MouseDown_Event(oNode.firstChild,true)

	this.ClearV()

}


/*
\u529f\u80fd\uff1a  \u7528\u4e8e\u53cd\u6620MouseDown\u4e8b\u4ef6\u3002\u5f53\u8be5\u4e8b\u4ef6\u53d1\u751f\u5728\u8bb0\u5f55\u6761\u76ee\u4e0a\u65f6\uff0c
        \u8be5\u6761\u76ee\u9ad8\u4eae\uff0c\u5c14\u539f\u9ad8\u4eae\u6761\u76ee\u6b63\u5e38\u3002
\u4f5c\u8005\uff1a  \u674e\u7ffc\u5d69
\u65e5\u671f\uff1a  2000.7
\u53c2\u6570\uff1a  Source -- \u8bbe\u7f6e\u8282\u70b9
        Direct -- \u4e0d\u6062\u590d\u989c\u8272
\u8fd4\u56de\u503c\uff1a\u8fd4\u56de\u9f20\u6807\u5904\u7684\u6574\u6761\u8bb0\u5f55\u5185\u5bb9\u3002\u5b57\u6bb5\u4e0e\u5b57\u6bb5\u95f4\u7528\u7a7a\u683c\u5206\u5f00\u3002

*/
function MouseDown_EventPrn(Source,Direct,keyControl)
{
	var elerow,strRe
	//\u4e8b\u4ef6\u6e90
		if (Source == null)
		{
			elerow = window.event.srcElement;
		}
		else
			elerow = Source

		//\u82e5\u4e3a\u53f3\u952e\u5219\u9000\u51fa
	//	if (window.event.button == 2)
	//		return "nothing";
		//\u5411\u4e0a\u8ffd\u6eaf\u4e8b\u4ef6\u7684\u7236\u5bf9\u8c61\uff0c\u662f\u5426\u4e3a\u8bb0\u5f55\u884c\u5bf9\u8c61\u3002
		if (elerow.parentElement.parentElement != null)
			elerow = elerow.parentElement;
		if (elerow.parentElement.classid == this.Name + "TableRow")
		{
			elerow = elerow.parentElement;
		}

    //\u82e5\u662f\u8bb0\u5f55\u884c\u5219\u54cd\u5e94\u8be5\u4e8b\u4ef6
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
		//\u82e5\u4e0d\u662f\u7b2c\u4e00\u6b21\u54cd\u5e94\u8be5\u4e8b\u4ef6\uff0c\u6062\u590d\u4e0a\u4e00\u6b21\u7684\u6b63\u5e38\u80cc\u666f\u8272\u3002
		if (this.preElement != null && Direct != true)
		{	this.preElement.style.backgroundColor = this.preColor;
			this.preElement.style.color = "black";
		}
		//\u4fdd\u5b58\u80cc\u666f\u8272\uff0c\u4ee5\u5907\u6062\u590d\u3002
		this.preColor = elerow.style.backgroundColor;
		//\u6307\u5b9a\u65b0\u7684\u80cc\u666f\u8272\u3002
		elerow.style.backgroundColor = "midnightblue";
		elerow.style.color = "white"
		//\u8fd4\u56de\u8be5\u8bb0\u5f55\u5185\u5bb9\u3002


		this.RowStr = this.EditRow(elerow.name)

		this.currentRow = elerow.name
		//\u4fdd\u5b58\u8be5\u884c\uff0c\u4ee5\u5907\u6062\u590d\u3002
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
\u529f\u80fd  \uff1a\u5f53\u4e00\u884c\u5931\u53bb\u7126\u70b9\u65f6\uff0c\u4fdd\u5b58\u754c\u9762\u5185\u5bb9\uff0c\u4fdd\u5b58\u8bb0\u5f55\u3002
\u4f5c\u8005  \uff1a\u674e\u7ffc\u5d69
\u65f6\u95f4  \uff1a2000\u5e748\u6708
\u53c2\u6570  \uff1a
\u8fd4\u56de\u503c\uff1a
*/
function SaveRowPrn()
{
	var i,j,strTemp,bTemp
    var iRow
    //var strRow = ""
	iRow = this.PreRow
	if (this.rs_main.RecordCount > 0 )
	{	this.rs_main.MoveFirst();
		this.rs_main.Move(iRow)
	}
//\u4fdd\u5b58\u754c\u9762\u5185\u5bb9
	for (i=0; i<this.Widths.length; i++)
	{
		if (this.PreRow != -1)
		{

			//\u53d6\u6d88\u4e0a\u4e00\u6b21\u7684\u9ed1\u7ebf\u3002
			//this.preElement.childNodes(i).style.borderTop=""
			//this.preElement.childNodes(i).style.borderBottom=""

			//\u5c06\u524d\u4e00\u4e2a\u7f16\u8f91\u6846\u7684\u5185\u5bb9\u4fdd\u7559\u5230\u754c\u9762\u884c\u5185
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
							//\u4fdd\u5b58\u8bb0\u5f55\u96c6\u5185\u5bb9
							//this.rs_main.Fields(i).value = strTemp
							//continue
						}
						else
							strTemp = parseFloat(strTemp)
					}
					this.preElement.childNodes(i).innerText = strTemp
					this.preElement.childNodes(i).style.padding=2
					//\u4fdd\u5b58\u8bb0\u5f55\u96c6\u5185\u5bb9
					this.rs_main.Fields(i).value = strTemp
					//strRow += strTemp + this.Divide
					continue

				}

			//\u5c06\u524d\u4e00\u4e2a\u63a7\u4ef6\u5185\u5bb9\u4fdd\u7559\u5230\u9875\u9762\u4e0a
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
						//\u4fdd\u5b58\u8bb0\u5f55\u96c6\u5185\u5bb9
						this.rs_main.Fields(i).value = strTemp
						//strRow += strTemp + this.Divide
						continue
				}

			////\u5c06\u5176\u4ed6\u5185\u5bb9\u4fdd\u5b58
			//if (this.preElement.childNodes(i).name != "Virtul")
			//{	strTemp = this.preElement.childNodes(i).innerText
				//this.rs_main.Fields(i).value = strTemp
			//	strRow += strTemp + this.Divide
			//}
		}
	}
	//\u4fdd\u5b58\u8bb0\u5f55\u96c6\u5185\u5bb9
	//this.rs_main.update()

	//\u662f\u5426\u663e\u793a\u6c47\u603b
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
\u529f\u80fd  \uff1a
\u4f5c\u8005  \uff1a\u674e\u7ffc\u5d69
\u65f6\u95f4  \uff1a2000\u5e748\u6708
\u53c2\u6570  \uff1a
\u8fd4\u56de\u503c\uff1a
*/
function EditRowPrn(Rownum)
{
//outControl \u7528\u4e8e\u5916\u90e8\u63a7\u5236\u4e34\u65f6\u6539\u53d8\u884c\u663e\u793a\u6a21\u5f0f
	var i,j,strTemp,strCats,strStyle,iWidth,strRe,oRows,oParent,ilen
	strRe = ""	//\u672c\u884c\u5b57\u4e32
	eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild.childNodes(Rownum).childNodes")

/*	var dat
	var s1
	var s2
	dat = new Date()
	s1 = dat.getTime()
*/


	//\u82e5\u70b9\u51fb\u5e76\u975e\u5f53\u524d\u884c\uff0c\u8fdb\u884c\u5904\u7406
	if (this.currentRow != Rownum)
	{
		if (this.PreRow != -1)
			this.SaveRow()

		//\u5bf9\u6240\u6709\u5217\u626b\u63cf
		for (i=0; i<this.Widths.length; i++)
		{
			//\u4e0a\u4e0b\u5212\u9ed1\u5b9e\u7ebf
			//oRows(i).style.borderTop="1 solid #000000"
			//oRows(i).style.borderBottom="1 solid #000000"

			strRe += oRows(i).innerText
			if (i < this.Widths.length -1 && oRows(i).name != "Virtul")
				strRe += this.Divide

			//\u82e5\u6b21\u5217\u53ef\u7f16\u8f91\uff0c\u4e3a\u8f93\u5165\u6846\u3002
			if (this.modi != null)
			{
				//\u5b89\u6392\u65b0\u7684\u7f16\u8f91\u6846
				if (this.modi[i] && oRows(i).name != "Virtul")
				{	//\u7f16\u8f91\u6846\u5bbd\u5ea6
					if (this.seedControl[i] != null & oRows(i).name != "Virtul")
						iWidth = this.Widths[i] - 28
					else
						iWidth = this.Widths[i] - 1
					//\u7f16\u8f91\u6846\u7f3a\u7701\u5185\u5bb9
					strTemp = "'" + oRows(i).innerText + "'"

					//\u7f16\u8f91\u6846\u98ce\u683c
					strStyle = ' Style="WIDTH:' + iWidth + 'px; border-bottom:1 solid black;"'
					//\u662f\u5426\u4e3a\u6570\u503c
					if (this.FieldsType[i] == "num")
						strStyle += " onKeyPress='DealNumberPress()'"
					//\u7f3a\u7701\u503c
					strStyle += " value=" + strTemp
					//\u662f\u5426\u6307\u5b9a\u6700\u5927\u957f\u5ea6
					if (this.MaxSize[i] != null)
						strStyle += " maxlength=" + this.MaxSize[i]
					//\u5f53\u524d\u683c<TD></TD>\u4e2d\uff0c\u63d2\u5165\u7f16\u8f91\u6846HTML\u6587\u6863\u3002
					oRows(i).innerHTML = "<input type=Text id=" + this.Name + "Text" + i + strStyle + " >"
					oRows(i).style.padding = 0
				}
			}

			//\u751f\u6210\u63a7\u4ef6\u6846
			if 	(i < this.seedControl.length)
			if (this.seedControl[i] != null  && oRows(i).name != "Virtul")
			{
				//\u5b89\u6392\u65b0\u7684\u9009\u62e9\u6846,\u5f53\u524d\u683c<TD></TD>\u4e2d\uff0c\u63d2\u5165\u9009\u62e9\u6846HTML\u6587\u6863\u3002
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
	//\u5c06\u5f53\u524d\u884c\u8bbe\u4e3a\u524d\u4e00\u884c\uff0c\u4ee5\u5907\u4e0b\u4e00\u6b21\u6bd4\u8f83\u3002
	this.PreRow = Rownum;
	if (strRe.charAt(0) == "")
		return null
	return strRe
}

/*
\u529f\u80fd  \uff1a\u5f97\u5230\u5f53\u524d\u884c\u7684\u7d22\u5f15\u503c\u3002
\u4f5c\u8005  \uff1a\u674e\u7ffc\u5d69
\u65f6\u95f4  \uff1a2000\u5e748\u6708
\u53c2\u6570  \uff1a
\u8fd4\u56de\u503c\uff1a\u7d22\u5f15\u503c\u3002
*/
function GetRownumPrn()
{
	return this.currentRow * 1;
}

/*
\u529f\u80fd  \uff1a\u5237\u65b0\u8868\u683c\uff0c\u6e05\u9664\u9ad8\u4eae\u3002
\u4f5c\u8005  \uff1a\u674e\u7ffc\u5d69
\u65f6\u95f4  \uff1a2000\u5e748\u6708
\u53c2\u6570  \uff1a
\u8fd4\u56de\u503c\uff1a
*/
function RefreshPrn() {
	var strTemp
	//\u81ea\u52a8\u5220\u9664
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

	//\u82e5\u4e0d\u662f\u7b2c\u4e00\u6b21\u54cd\u5e94\u8be5\u4e8b\u4ef6\uff0c\u6062\u590d\u4e0a\u4e00\u6b21\u7684\u6b63\u5e38\u80cc\u666f\u8272\u3002

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


function EventRowPrn(Name)
{
	var elerow
	elerow = window.event.srcElement;

	//\u82e5\u4e3a\u53f3\u952e\u5219\u9000\u51fa
	//\u5411\u4e0a\u8ffd\u6eaf\u4e8b\u4ef6\u7684\u7236\u5bf9\u8c61\uff0c\u662f\u5426\u4e3a\u8bb0\u5f55\u884c\u5bf9\u8c61\u3002
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

function ClearVPrn()
{
	//\u4e0d\u77e5\u4e3a\u4ec0\u4e48\uff0c\u7a7a\u8bb0\u5f55\u96c6\u65e0\u6cd5\u5728\u8fd0\u884c\u65f6AddNew\uff0c\u53ea\u597d\u51fa\u6b64\u4e0b\u7b56\u3002--- 1
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
		eval(this.Name + "Count.innerText = '\u8bb0\u5f55\u6570\uff1a" + this.rs_main.RecordCount + "'")

	//}

}

function DealNumberPress()
{
 var charCode=(navigator.appName =="NetScape")?event.Which: event.keyCode;

//\u53ea\u80fd\u51fa\u73b0\u6570\u5b57\u548c\u5c0f\u6570\u70b9\u3002
 if (charCode>31 && (charCode<48 ||charCode>57) && charCode != 46)
   	event.returnValue =false;

//\u4e0d\u80fd\u540c\u65f6\u51fa\u73b0\u4e24\u4e2a\u5c0f\u6570\u70b9\u3002
 if (event.srcElement.value.indexOf(".")>=0 && charCode == 46)
	event.returnValue =false;

}

function DealBackPrn()
{
}

function DealKeyPressPrn()
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
		case 37:	//\u5de6

			break;

		case 38:	//\u4e0a
			if (this.currentRow > 0)
			{	iRow -= 1
				this.HighLightRow(iRow)
				Tag.focus()
				eval(this.UpDown)
			}
			event.returnValue =false;
			break;

		case 39:	//\u53f3

			break;
		case 13:	//\u56de\u8f66
			eval(this.Dbclick)

			break;

		case 40:	//\u4e0b
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

