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
	this.LightBKColor = "midnightblue" //�����б���ɫ
	this.LightColor = "white"    //������������ɫ
	this.TableLineColor = "black"        //��������ɫ


	this.UseSort = ""				//ָ�������TDC�ؼ�
	this.IsEmpty = false			//�ж��Ƿ�ǰ����Ƿ�Ϊ��
	this.Divide = "\u001C"				//ʹ�õķָ���
	this.outerSelect = false		//ʹ���ⲿѡ��SELECT��ǩ����ѡ�����ݷ��ڸ��С�

//public Method
	this.Display = _Display;			//�õ�HTML�ִ���
	this.MouseDown_Event = _MouseDown_Event //����¼�
	this.ManualMouseUp = _MouseDown_Event	//����¼�
	this.TDMouseDown = _TDMouseDown
	this.AddEvent = _AddEvent;		//����һ����¼��
	this.GetRowNumber = _GetRownum	//�õ���ǰ������ֵ��
	this.Refresh = _Refresh			//ˢ�±��
	this.ColumnStyle = _ColumnStyle	//�趨һ��������ԡ�
	this.TableStyle = _TableStyle	//�趨��������ԡ�
	this.HeadStyle = _HeadStyle		//�趨����������ԡ�
	this.Delete = _DelRow			//ɾ����ǰ��
	this.Append = _Append			//����һ�У������ֶ���ʽ��
	this.SetRow = _SetRow			//����һ������
	this.SetCell = _SetCell			//���õ�ǰ��������һ�е�����
	this.UseControl = _UseControl	//ʹ���û�ָ���Ĳ���ؼ���
	this.ControlReturn = _ControlReturn //�����û�����ؼ����ڸ�����ݡ�
	this.PageColumn = _PageColumn    //���ú����ҳ����
	this.HideColumn = new Array()			//����ָ�����ֶ�
	this.seedControl = new Array()
	this.ControlValue = new Array()
	this.FieldsType = new Array()
	this.MaxSize = new Array()
	this.EventRow = _EventRow
	this.ClearV = _ClearV

//private property and method
	this.Name = ObjectName;			//���ɶ�������ơ�
    this.PreRow = -1;				//����ǰһ�м�¼��
	this.strColumn = ""				//�������ִ���
	this.strTable = 'Style="color:black;font-size:9pt"'	//�������ִ���
	this.strHead = "font-size:10pt"				//���������ִ���
	this.AskSum = false;					//�Ƿ����ӡ��ϼơ�
	this.SumArr = new Array(RecordSet.Fields.Count);	//�ϼ��ֶκϼ�ֵ��
	this.SumDot = 2
	this.EditRow = _EditRow;			//�Ե�ǰ�н��пɱ༭����
	this.UseSum = _UseSum;			//���Ӻϼ��ֶΣ����ظ����á�
	this.currentRow = -1;			//��¼Ŀǰ������ֵ��
    this.preElement = null;			//���浱ǰ�У��������е������ʱ�ָ�����
    this.preColor = null;			//���浱ǰ����ɫ��
	this.BackRefresh = _BackRefresh  //ˢ�±���ɫ
	this.SaveRow = _SaveRow			//��������е����ݵ���¼����
	this.Sort = _Sort
	this.UseForm = ""
	this.Pages = 1
	this.PageArr = new Array()
	this.NewPage = _NewPage
	this.WhichPage = 0
	this.SortColumn = -1			//ָ�����ĸ��ֶ�����
	this.SortAsc = ""				//���򷽷���""Ϊ����"-"Ϊ����
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

	this.Filter = _FilterReset	//ʹ�ù��˵ķ��� ���ֶ���=ɸѡ�ִ�����ע�⣺ʹ�ø÷���ǰһ��Ҫʹ��UseSort���ԡ�
	this.strFilter = ""    // �ⲿ�Ĺ�������this.rs_main.Fields(0).Name + " <> -1.0249E5"

	this.ControlPath = "../../../commoncontrol/"	//����ͼƬ�ͶԻ�������·��
	this.DownEventSrc = null  //event.srcElement
	this.NoKey = false			//true -- ��֧�ּ��̿�������
	this.BeforeUpDown = null	//�����������ƶ�֮ǰ���õĺ���
	this.UpDown = null			//�����������ƶ�ʱ���õĺ���
	this.BeforeChange = null
	this.OuterControl = -1		//ָ����һ�пؼ��滻��ǰ�ı�
	this.OuterControl1 = -1		//ָ����һ�пؼ��滻��ǰ�ı�
	this.OuterControl2 = -1		//ָ����һ�пؼ��滻��ǰ�ı�
	this.OuterControl3 = -1		//ָ����һ�пؼ��滻��ǰ�ı�
	this.OuterControl4 = -1		//ָ����һ�пؼ��滻��ǰ�ı�
	this.SetZ = true			//����ֵ��Ϊ�ջ�Ƿ�ʱ�Ƿ񽫸��ֶ���Ϊ0
	this.ShowZ = true			//����ֵ��Ϊ0ʱ�Ƿ���ʾ
	this.GetFirstEnableChild = _GetFirstEnableChild
	this.NoFocus = false	//����ĳ��ʱ�����ý���


	this.BeforeHighLight=null	//������֮ǰ�¼�
	this.BeforeSave = null		//��������֮ǰ�¼�
	this.BeforeNew = null		//��������֮ǰ�¼�
	this.AfterNew = null		//��������֮���¼�
	this.AfterChange = null		//�����б任ֻ���¼����������������ı任��
	this.AfterNewPage = null	//�»�һҳ�¼�

	this.DefaultSort = -1		//ȱʡ���

	this.DHContent = new Array()	//�ϲ������ܳ� ("","�ϲ�1","","","","�ϲ�2","","")	//˫��������ݡ�
	this.DHSpan = new Array()		//�ϲ���� (1,2,2,1,1,2,2,1)		//˫����Ŀ�ȡ�
	this.DHSignal = new Array()		//�ϲ��ֶα�ǡ�(0,1,1,0,0,1,1,0)		//˫����ָʾ�������Ƿ�Ϊ˫�㡣

	this.UseCombin = _UseCombin		//ʹ�úϲ�����
	this.ChangeK = _ChangeK			//ת�幦�ܣ�����<..>��ת�ɡ�&lt;...��


	//���Ӳ�ͬ�����԰汾
	//aa = bb.sub
	if (document.charset == "gb2312")
	{

		this.JILUSHU = "��¼���� "
		this.HEJI = "�ϼ� "
		this.KONG = "��"
		this.NOSORT = "�����ѱ��޸ģ����ȱ������ݣ� "
	}
	else if (document.charset == "big5")
	{
		this.JILUSHU = "�O���ơG "
		this.HEJI = "�X�p "
		this.KONG = "�@"
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

	this.OverFlow = "auto"	//�����趨 "scroll" "auto"
	this.MaxLine = 99999
	this.PreLine = -1
	this.GetOrder = new Array()
	this.SetOrder = _SetOrder

	for (var i=0; i<this.rs_main.Fields.Count; i++)
	{
		this.GetOrder[i] = i

	}

	this.UseHTML = false		//�Ƿ�ʹ���ⲿָ����HTML
	this.ReversSort = false		//�Ƿ�ı��ֶ���ʾ˳��
	this.FastField = -2
	this.preFastElement = null
	this.DisplayFast = _DisplayFast	//��ʾ�̶��ֶ�
	this.DisplayM = _DisplayM
	this.BodyHTML = _BodyHTML
	this.HeadHTML = _HeadHTML

	this.UseSearch = ""				//�Ƿ�ʹ�ÿ��ٲ�ѯ,""������ʹ�ã�"All"���������ֶΣ�"Page"������ǰҳ, "AllLine"�����и���,"PageLine"�����и���
	this.SearchItem = null;       //������ѯ��������Text����Select
	this.SearchHTML = "<Input id=" + this.Name + "SearchValue style='width:100;height:20' value=''>"
	this.SearchFilter = _SearchFilter	//�õ������ִ�
	this.BeforeSearch = null		//���ٲ�ѯ֮ǰ�¼�
	this.strSearch = ""				//��ǰ��ѯ����
	this.AdjustLine = 0				//����ʹ�ÿ��ٲ�ѯʱ���������


	this.CellColorArr = new Array()	//ָ����Ԫ����ʽ������
	this.CellColor = _CellColor		//ָ����Ԫ����ʽ
	this.CellColorColumn = -1		//ָ����Ԫ����ʽ�Ĺؼ�����
	this.ChangeHead = null			//�趨�󴢷��¼�
	this.ToUnicode = _ToUnicode		//ת����unicode
	this.ReplaceChar = _ReplaceChar	//�滻�ַ����罫'�滻�� \\\'
	this.ToRe = _ToRe


	this.SetWidth = _SetWidth
	this.ScreenWidth = 800
	this.ScreenHeight = 600
	this.adjustRateW = 1.011
	this.adjustRateH = 1.082

	this.checkBoxColumn = -1	//�����Դ�����Ҫʹ��CheckBox����λ���0~n ������λ��ֵ����checked���ʾ��ѡ�С�
	this.checkBoxStyle = ""
	this.checkAll = _CheckAll	//checkBoxȫѡ����
	this.checkItem = _CheckItem	//checkBoxѡ�񷽷�

	this.arrDelLog = new Array()		//����������ӡ��޸ġ�ɾ���ļ�¼��0����ɾ�� 1�������� 2����ɾ�� ���������ֵͬʱ�����¼��������
	this.arrModiLog = new Array()		//����������ӡ��޸ġ�ɾ���ļ�¼��0����ɾ�� 1�������� 2����ɾ�� ���������ֵͬʱ�����¼��������
	this.arrSortModi = null;
	this.DealLogWhenDel = _DealLogWhenDel		//���Ӽ�¼����ɾ��һ����¼������ļ�¼����ֵ�ı䣬��Ҫ����arrModiLog�е�������ModiLogDel
	this.GetAMDString = _RecordsetToString  //�õ�����(A)���༭(M)��ɾ���ַ���(D)��
	this.ClearAMD = _ClearAMD		//�������(A)���༭(M)��ɾ���ַ���(D)����ʱ����
	this.Clear = _Clear			//��ձ�����ݡ�
	this.canKeyDelete = false
	this.MaxSortNumber = 0
	this.Calculator= new Array()		//�Զ����㹫ʽ���飬����ʵ��ָ���е��Զ����㡣��ʽ���á�<�к�>=���㹫ʽ�����㹫ʽ�������������Ƶı��ʽ,���ʽ�п��԰���<�к�>
	this.CalculateColumn = _CalculateColumn	//��ָ������ʵ���Զ�����
	this.FindSameRow = _FindSameRow		//�����ڵ�ǰ����У��Ƿ������ͬ������
	this.BeforeSortModi = _BeforeSortModi	//��������ǰ����Ҫ����������ӵ�����arrModi
	this.AfterSortModi = _AfterSortModi	//�������˺󣬸��¸������ӵ�����arrModi
	this.CanNotChangRow = false;         // ���λ��еĲ���
	this.SearchValue = _SearchValue
	this.SetSelect = _SetSelect
	this.dataurl = ""       //����TDC�ؼ���dataURL���ѱ��ж��Ƿ�����ȡ�������ˡ�
	this.CalculatorAll = _CalculatorAll //�����е��Զ�������λ���м���
	this.scrollTop = 0;

}

function _CalculatorAll()
{

		if (this.rs_main.RecordCount > 0  && this.Calculator.length>0)
		{
			var tempPosition = this.rs_main.absolutePosition;
			this.rs_main.MoveFirst()
			//���ν��кϼ�
			while (!this.rs_main.EOF)
			{
				//�����ֶβ�����ֵ�ͣ��򲻽��кϼơ�
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
            //��һ��ʹ�ã�ƴװSelect����
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
        //������ѡ����Select����Input����
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
    //���´�������Display�󣬿��ٲ�ѯ�Ƿ���Ҫ�������á�
    var oNode = document.getElementById(this.UseSort)

    if (oNode != null) {
        if (this.dataurl != "" &&  this.dataurl != oNode.DataURL  &&  this.SearchItem != null) {
        //���°��ſ��ٲ�ѯ��select����
            for (var iCol=0; iCol<this.SearchItem.length; iCol++ ) {
                if (this.SearchItem[iCol] != "")
                    this.SearchItem[iCol] = "select"
            }
        }

        this.dataurl =  oNode.DataURL;

    }
    //���´�����������²���AMD�������
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
�����ڵ�ǰ����У��Ƿ������ͬ������
���£���1��2��3���Ƿ����ֵΪs001,aaa,ccc����
columns:"1,2,3"
values: "s001,aaa,ccc"
isCur: �Ƿ��ų��Ե�ǰ�бȽ� true �ų� false �Ƚ�
����ֵ������true ������false
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
��ָ������ʵ���Զ����㣬����Ҫ�ⲿ���ã�ֻҪ�趨calculator���Լȿ�
calculator�����趨�������£�
��������.Calculator= new Array("<3>=<2>!=''?0:<5>*<6>","<4>=<2>==''?0:<5>*<6>","<9>=<0>*<1>")

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

//checkBoxȫѡ����
function _CheckAll()
{
	if (this.rs_main.recordCount <= 0)
		return 0;

	this.rs_main.moveFirst()
	if (event.srcElement.checked)
	{	//ѡ��״̬��
		for (var i = 0; i< this.rs_main.recordCount; i++)
		{	this.rs_main.fields(this.checkBoxColumn).value = "checked"
			this.rs_main.moveNext()
		}
	}
	else
	{	//δѡ��״̬��
		for (var i = 0; i< this.rs_main.recordCount; i++)
		{	this.rs_main.fields(this.checkBoxColumn).value = ""
			this.rs_main.moveNext()
		}
	}

	this.ChangePage()

}

//checkBoxѡ�񷽷�
function _CheckItem()
{
   	var rownum = event.srcElement.parentElement.parentElement.name;
   	
   	this.rs_main.absolutePosition = rownum*1 + 1


	if (event.srcElement.checked)
	{	//ѡ��״̬��
		this.rs_main.fields(this.checkBoxColumn).value = "checked"
	}
	else
	{	//δѡ��״̬��
		this.rs_main.fields(this.checkBoxColumn).value = ""
	}
	
	if (this.LightRow > -1 && this.LightRow < this.rs_main.recordCount)
	this.rs_main.absolutePosition = this.LightRow*1 + 1

}


//�õ������ִ�
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


//�趨˳����
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
����  ��������HTML�ַ�ת����&???�ַ���
����  ��������
ʱ��  ��2001��8��
����  ��key--��ת�����ִ�
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
����  ��ʹ�úϲ����ܣ���
����  ��������
ʱ��  ��2001��8��
����  ��Columns--�е��ִ�����"3,4,5"
		CombinName -- �ϲ�����ܳơ�
*/

function _UseCombin(Columns,CombinName)
{
	var i,j, arrCol
	//i = bb.suhb()
	if (Columns == null || CombinName == null)
		return false
	//��ʼ������
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
	//���Ϸ���
	for (i=0;i<arrCol.length;i++)
	{
		if (this.DHSpan[arrCol[i]*1] != 1)
			return false

		if (this.DHSignal[arrCol[i]*1] == 1)
			return false

		if (arrCol[i]*1 >= this.Widths.length)
			return false
	}

	//Ϊ�ϲ����ݸ�ֵ��
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

		//������ı���ͷ
		for (var field=0; field<this.Widths.length; field++)
		{
			//�Ƿ�ָ���ֶο��
			iWidth = this.Widths[this.GetOrder[field]] - 5
			//������ʾ�ֶ�
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
				//��ָ������
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
			{	//ָ������
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

		//������֮��
		sUse[sUse.length] =  "<TH " + sRowCol + "id='aa11' width=13 style='" + this.strHead + "'></TH>"


		//������¼ͷ��
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
	//������
	//����ʼ�п�ʼ����lines�м�¼
	if (this.rs_main.RecordCount > iRow )
	{
		this.rs_main.absolutePosition = iRow+1
	}
	sBody[0] = TABLE_HEAD

	for (var j = iRow; j<this.rs_main.RecordCount; j++)
	{
		//����β��������ѭ����
		if (this.rs_main.EOF)
			break

		if (this.rs_main.Fields(0).value == "-1.0249E5")
		{	this.rs_main.MoveNext()
			continue
		}

		//�������߶ȣ�������ѭ��
		if ((j-iRow) > this.Height / 20)
			break
		//�Զ�������λֵ
		this.CalculateColumn();
		//���ñ���ɫ
		if (this.rs_main.Fields(this.GetOrder[this.Widths.length-1]).Name == "CC_��ɫ" && this.rs_main.Fields(this.GetOrder[this.Widths.length-1]).value != "")
			sBackC = " Style='background-color:" + this.rs_main.Fields(this.GetOrder[this.Widths.length-1]).value + "; height:20px; color:black; word-break:break-all'"
		else
			sBackC = " Style='background-color:" + this.arrColor[jj % 2] + "; height:20px; color:black; word-break:break-all'"


		sBody[sBody.length] =  ROW_BEGIN.replace("rownum", ""+ j + sBackC)
		jj += 1
		//���ż�¼�еĸ����ֶΡ�
		for (field=0; field<this.Widths.length; field++)
		{	//�Ƿ�ָ����ȡ�
			if (this.CellColorColumn >= 0)
			{
				sColor = this.CellColorArr["A" + this.rs_main.Fields(this.GetOrder[this.CellColorColumn]).Value + field]
				if (sColor == null)
					sColor = ""
			}
			//������ʾ�ֶ�
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
			//if (this.rs_main.Fields(field).Name == "���")
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

		//��¼������
		sBody[sBody.length] =  ROW_END
		//��һ����¼��
		this.rs_main.MoveNext()
	}



	//����һ�������Ա������������С�
	//���ñ���ɫ
	sBackC = " Style='background-color:" + this.arrColor[jj % 2] + "; height:20px; color:black; word-break:break-all'"
	sBackC =  ROW_BEGIN.replace("rownum", ""+ (jj*1+iRow*1) + sBackC)
	jj += 1
	sBody[sBody.length] =  sBackC.replace("TableRow'","TableRow' id=" + this.Name + "AddRow" + iFast + "")
	sDisplay = ""
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
		iWidth = this.Widths[this.GetOrder[field]] - 5
		//sStyle = sStyle + "  " + "." + this.Name + field + "{width:" + iWidth + " }"
		//sWidth = CELL_BEGIN.replace("FFIIEE","" + field + sDisplay + " name='Virtul'  width=" + iWidth +  " id=" + this.Name + "Add onclick='return " + this.Name + ".Append()'")
		sWidth = CELL_BEGIN.replace("FFIIEE","" + field + sDisplay + " name='Virtul'  width=" + iWidth +  " id=" + this.Name + "Add" + iFast + "")
		sBody[sBody.length] =  sWidth
		sBody[sBody.length] = CELL_END
	}

	sDisplay = ""
	//���ӿ���,���հ���
	for(var bl = jj ; bl < this.Height / 20 + 1; bl++)
	{
		sBackC = " Style='background-color:" + this.arrColor[bl % 2] + "; height:20px; color:black; word-break:break-all'"			//������ʾ�ֶ�
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

			//�Ƿ�ָ����ȡ�
			iWidth = this.Widths[this.GetOrder[field]] - 5
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

	//���汳�����ݣ�������ʧ��
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





	//�̶��ֶ�
	//aa = bb.ss
/*
	if (this.rs_main.RecordCount > 0)
	{	this.rs_main.MoveFirst()
		this.rs_main.Move(this.FirstRow)
	}





	for (j = 0; j<=this.Height / 20; j++)
	{	//����β��������ѭ����
		//if (this.rs_main.EOF)
		//	break
		if (j > this.Height / 20)
			break


		if (bAdd == true)
			oNode.childNodes(jj).id = this.Name + "AddRow"
		else
			oNode.childNodes(jj).id = ""

		//���ż�¼�еĸ����ֶΡ�
		if (this.rs_main.EOF || this.rs_main.RecordCount<1)
		{
			for (field=0; field<this.Widths.length; field++)
			{	//�հ�����
				//oNode.childNodes(jj).childNodes(field).Name = this.Name

				if (this.HideColumn[field] != false)
				{
					oNode.childNodes(jj).childNodes(field).innerText =  ""
					//�̶��ֶ�
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

			if (this.rs_main.Fields(this.GetOrder[this.Widths.length-1]).Name == "CC_��ɫ")
			{
				if (this.rs_main.Fields(this.GetOrder[this.Widths.length-1]).value != "")
					oNode.childNodes(j).style.backgroundColor = this.rs_main.Fields(this.GetOrder[this.Widths.length-1]).value
				else
					oNode.childNodes(j).style.backgroundColor = this.arrColor[j % 2]

			}

			for (field=0; field<this.Widths.length; field++)
			{	//�Ƿ�ָ����ȡ�
				if (this.HideColumn[field] != false)
				{

					if (this.SetZ == true || this.rs_main.Fields(this.GetOrder[field]).Value * 1 != 0)
					{	oNode.childNodes(jj).childNodes(field).innerHTML = this.ChangeK(this.rs_main.Fields(this.GetOrder[field]).Value)
						//�̶��ֶ�
						if (oNode2 != null)
							oNode2.childNodes(jj).childNodes(field).innerHTML = this.ChangeK(this.rs_main.Fields(this.GetOrder[field]).Value)

					}
					else
					{	oNode.childNodes(jj).childNodes(field).innerHTML =  ""

						//�̶��ֶ�
						if (oNode2 != null)
							oNode2.childNodes(jj).childNodes(field).innerHTML = ""

					}
					oNode.childNodes(jj).childNodes(field).name = ''
					oNode.childNodes(jj).childNodes(field).id = null
					oNode.childNodes(jj).name = "" + (this.FirstRow * 1 + jj * 1)
				}

				//if (this.rs_main.Fields(field).Name == "���")
				//	oNode.childNodes(jj).childNodes(field).innerText =  this.rs_main.absolutePosition


			}

			//��һ����¼��
			this.rs_main.MoveNext()
			if (this.rs_main.EOF)
				bAdd = true
		}

		//�Ƿ��ڱ�ҳ���ڸ����С�
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

	//������ͷ
	eval(this.Name + "SpanHead.scrollLeft = oNode.scrollLeft")
	//��������
	eval(this.Name + "TableBody.style.left = -oNode.scrollLeft")
	//������β
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
		//����ĳ��ʱ�����ý���
		this.NoFocus = true

		this.ChangePage()
		this.NoFocus = false

		//����һ����������ڻ�ý��㡣�������������ʱ���ƶ����������ٰ����¼����������
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
		{	//��ʾ�Զ����ֶ�
			var ArrFree = sCookie.substring(0,sCookie.length -2).split(",")
			//aa = bb.sub()
			this.NewPage(0,ArrFree,0)
			//�趨�ֶ��Ⱥ�˳��
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
					eval(this.UseForm + this.Name + "0img.src = '" + this.ControlPath + "images/" + "0.gif'")

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
function _FreeSelect(SaveName)
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
	{	//���ڸ���
		//eval("oNode = " + this.Name + "TableSpan.firstChild.firstChild.childNodes(" + ��RowNum - this.FirstRow�� + ").firstChild")
		eval("oNode = " + this.Name + "TableSpan.firstChild.firstChild.childNodes(" + (RowNum - this.FirstRow) + ").firstChild")
		if (oNode.parentElement.offsetTop + oNode.parentElement.offsetHeight < this.Height-18*iHScroll)
			//�ڿɼ��ķ�Χ��
			if (DownShow == "AddNew")
				this.ChangePage()
			else
				this.ManualMouseUp(oNode,null,true)
		else
		{	//���ڿɼ��ķ�Χ�ڣ�����ɼ�λ�ã������趨��

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
	{	//�����ڸ���
		if (DownShow != true)
		{	//������ʾ����
			this.FirstRow = RowNum
			this.LightRow = RowNum
			this.ChangePage()
		}
		else
		{	//������ʾ��β
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
����  ���Ƿ��ҳ��ʾ�ֶΣ�ָ���ּ�ҳ��ʾ��
����  ��������
ʱ��  ��2000��8��
����  ��Columns ָ��������ֶ�
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
����  ���Ƿ��ҳ��ʾ�ֶΡ�
����  ��������
ʱ��  ��2000��8��
����  ��PageNum ָ����ʾ�ڼ�ҳ
        Free �Ƿ�Ϊ����ѡ��ҳ
*/
function _NewPage(PageNum,Free,SetHide)
{
	//�õ���ҳ���ֶ���
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

	//�����趨��ʾ˳��
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

	//���ڿ��ܱ任�����������ñ�ͷ��
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

	//�������ֶζ�����ʾ��ֻ��ʾ��ҳ�ֶ�
	for(var i=0; i<this.Widths.length; i++)
		this.HideColumn[i] = false
	for(var i=0; i<temp.length; i++)
		this.HideColumn[temp[i]] = true

	if (SetHide == true)
		return

	//����ռ�¼
	this.ClearV()

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
			iTemp += this.Widths[this.GetOrder[n]]
		}
	}


/*	//���й̶��ֶ�
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



		//�������ر�ͷ����β���ء�
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
		//��ʱ iTemp Ϊ���пɼ��ֶο�Ⱥ�
		if (this.TableWidth+2 >= (iTemp))
		{//���������������
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
			//���й̶��ֶ�
		//	if (this.FastField > 0)
		//	{
		//		eval(this.Name + "TableBody" + this.FastField + ".style.width = " + iTemp)
		//		eval(this.Name + "TableBody" + this.FastField + ".width = " + iTemp)

		//	}

	}

	//�������ú�����������
	eval(this.Name + "Measure.style.width = " + iTemp)
	eval(this.Name + "TableBody.style.left = 0")
	eval(this.Name + "TableSpan2.scrollLeft = 0")

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


	//������ʾ����
	this.ChangePage()

	//�����а�ţ̌��ֻ����ҳ��ť����
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


	//������ҳ�¼�
	eval(this.AfterNewPage)
}



/*
����  ���Ƿ�����
����  ��������
ʱ��  ��2000��8��
����  ��Columns ָ��������ֶ�
*/
function _Sort(column)
{
	//��������ֶ�������
	//if (this.rs_main.Fields(column).Name == "���")
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
		eval("this.SortLeft = " + this.Name + "TableSpan2.scrollLeft" )
		//ˢ�½��棬�Ա㱣�浱ǰ����Ϣ
		this.Refresh(true)
		//ִ�������趨TDC�ؼ�����
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
//����󱣳ֺ������λ��

		//������ͷ
		eval(this.Name + "SpanHead.scrollLeft = iLeft")
		//��������
		eval(this.Name + "TableBody.style.left = -iLeft")
		//������β
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
	//�������ѱ��༭�����޷�������ˣ����ȱ�������������ˡ�
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
����  �������û�����ؼ����ڸ�����ݡ�
����  ��������
ʱ��  ��2000��8��
����  ��strVal -- Ҫ��ʾ�����ݡ�
        Col -- �����ֶε�λ�á�
*/
function _ControlReturn(strVal,col)
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
����  ����Ӻϼ��ֶΡ�����ε��ã���Զ���ֶν��кϼơ�
����  ��������
ʱ��  ��2000��8��
����  ��Column -- ��Ҫ�ϼƵ��ֶα�ţ�base 0����
����ֵ��
*/
function _UseSum(Column,isCal)
{
	if (Column <= this.Widths.length)
	{
		this.AskSum = true;
		//����¼Ϊ�գ��򲻺ϼơ�
		this.SumArr[Column] = 0
		if (this.rs_main.RecordCount > 0 && isCal)
		{
			var tempPosition = this.rs_main.absolutePosition;
			this.rs_main.MoveFirst()
			//���ν��кϼ�
			while (!this.rs_main.EOF)
			{
				//�����ֶβ�����ֵ�ͣ��򲻽��кϼơ�

				//if (typeof(this.rs_main.Fields(this.GetOrder[Column]).value) != "number")
				if (parseFloat(this.rs_main.Fields(this.GetOrder[Column]).value) != parseFloat(this.rs_main.Fields(this.GetOrder[Column]).value))
				{	this.rs_main.MoveNext()
					continue
				}
				//���кϼ�
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
����  ���趨��������ԡ�
����  ��������
ʱ��  ��2000��8��
����  ��ColNum -- ���ֶα�ţ�base 0��
        strStyle -- ��ʽ���ִ������磺"Width:80; color:red"��
����ֵ��
*/
function _ColumnStyle(ColNum,strStyle)
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
function _TableStyle(strStyle)
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
function _HeadStyle(strStyle)
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

		//�ֶ�˳����ܱ仯����������ҳ�����������ֶε�ԭ��˳����ʾ
		if (this.WhichPage > 0)
		{
			this.SetOrder()
		}
		str2 = this.DisplayM();
		return  str2 + str1

	}
	else
	{
		//�ֶ�˳����ܱ仯����������ҳ�����������ֶε�ԭ��˳����ʾ
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
		this.modi = new Array();				//�ɱ༭�ֶ����� true -- �ɱ༭��
		this.AskSum = false;					//�Ƿ����ӡ��ϼơ�
		this.seedControl = new Array()
		this.ControlValue = new Array()
		this.AddDelete = false;			//�Ƿ�������ɾ����¼

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
	//�������ֶζ�����ʾ��ֻ��ʾ��ҳ�ֶ�
	var temp, FastWidth
	FastWidth = this.TableWidth
	//����checkbox column style
	if (this.checkBoxColumn != -1)
		this.ColumnStyle(this.checkBoxColumn,"text-align:center;");

	//�趨�ֶ��Ⱥ�˳��

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

		//�������
		//if (this.rs_main.Fields(i).Name == "���" && this.rs_main.RecordCount > 0)
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
	//******************************************** �������� ***********************************
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
		var sRowCol = ""	//�����rowspan=2
		var sNext = ""
		DivWidth = 0

	//�Ƿ���ʾ���⡣
	if (this.Title)
	{	//������ͷ
		//aa.sub()
		sSort = ""
		sUse[sUse.length] = TABLE_HEAD_H + HEAD_ROW_BEGIN
		if (this.DHSpan.length > 0)
			sRowCol = " rowspan=2 "

		//������ı���ͷ
		for (field=0; field<this.Widths.length; field++)
		{
			//�Ƿ�ָ���ֶο��
			iWidth = this.Widths[this.GetOrder[field]] - 5
			//������ʾ�ֶ�
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
				//��ָ������
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
			{	//ָ������
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

		//������֮��
		sUse[sUse.length] =  "<TH " + sRowCol + "id='aa11' width=13 style='" + this.strHead + "'></TH>"
		//������¼ͷ��
		sUse[sUse.length] =  HEAD_ROW_END + HEAD_ROW_BEGIN + sNext + HEAD_ROW_END + TABLE_TAIL_H
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
	var j = 0
	var sColor = ""	//��Ԫ�����ʽ
	for (j = 0; j<this.rs_main.RecordCount; j++)
	{	//����β��������ѭ����
		if (this.rs_main.EOF)
			break

		//��ֹ��ʾ�����¼
		if (this.rs_main.Fields(0).value == "-1.0249E5")
		{	this.rs_main.MoveNext()
			continue
		}

		if (j > this.Height / 20)
			break
		
		//�Զ�������λֵ
		this.CalculateColumn();
		
		//���ñ���ɫ
		if (this.rs_main.Fields(this.GetOrder[this.Widths.length-1]).Name == "CC_��ɫ" && this.rs_main.Fields(this.GetOrder[this.Widths.length-1]).value != "")
			sBackC = " Style='background-color:" + this.rs_main.Fields(this.GetOrder[this.Widths.length-1]).value + "; height:20px; color:black; word-break:break-all'"
		else
			sBackC = " Style='background-color:" + this.arrColor[jj % 2] + "; height:20px; color:black; word-break:break-all'"


		sBody[sBody.length] =  ROW_BEGIN.replace("rownum", ""+ jj + sBackC)
		jj += 1
		//���ż�¼�еĸ����ֶΡ�
		for (field=0; field<this.Widths.length; field++)
		{	//�Ƿ�ָ����ȡ�

			if (this.CellColorColumn >= 0)
			{
				sColor = this.CellColorArr["A" + this.rs_main.Fields(this.GetOrder[this.CellColorColumn]).Value + field]
				if (sColor == null)
					sColor = ""
			}
			
			//������ʾ�ֶ�
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
			//if (this.rs_main.Fields(field).Name == "���")
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
	sBody[sBody.length] =  sBackC.replace("TableRow'","TableRow' id=" + this.Name + "AddRow" + iFast + "")
	sDisplay = ""
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
		iWidth = this.Widths[this.GetOrder[field]] - 5
		sStyle += this.FieldsType[this.GetOrder[field]] == "num"?"   ." + this.Name + field + "{text-align:right}    ":""
		//sWidth = CELL_BEGIN.replace("FFIIEE","" + field + sDisplay + " name='Virtul'  width=" + iWidth +  " id=" + this.Name + "Add onclick='return " + this.Name + ".Append()'")
		sWidth = CELL_BEGIN.replace("FFIIEE","" + field + sDisplay + " name='Virtul'  width=" + iWidth +  " id=" + this.Name + "Add" + iFast + "")
		sBody[sBody.length] =  sWidth
		sBody[sBody.length] = CELL_END
	}

	sDisplay = ""
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
			iWidth = this.Widths[this.GetOrder[field]] - 5
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
		//�������ڵĿ��
		var ihigh
		ihigh = this.Height
		//���������������������߶�-18���Ա���ȫ��ʾ�����������
		if (this.OverFlow == "scroll")
			ihigh = ihigh - 18
		else
		{

		//���������������������߶�-18���Ա���ȫ��ʾ�����������

			if ((this.TableWidth-15) < (sumWidth2))
				ihigh = ihigh - 18

		}

/*
���²��֣��ǲ���������������Ѹ����ʾ�Ļ��ơ�

*/
		sUse[sUse.length] = "<br><Span id='Ret_3' style='position:relative; left:0; top:0'>"
		if (iFast == "")
		{
			//TableSpan2--���ڲ�����������span,�ں�һ���߶Ƚ��Ʊ��ĵ�DIV��Measure�����Ӷ�����������
			sUse[sUse.length] = "<SPAN id='" + this.Name + "TableSpan2" + iFast + "' style ='left:0; top:20; height:" + this.Height + ";width:" + (sumWidth + 18) + "; OVERFLOW:" + this.OverFlow + "; z-index:1500' onscroll='" + this.Name + ".ScrollVer()' ><DIV id=" + this.Name + "Measure STYLE='WIDTH:" + (DivWidth+1) + ";HEIGHT:" + (this.rs_main.RecordCount * Math.floor(this.Height / 8) + this.Height+1) + ";POSITION:absolute;z-index:2000'></DIV></SPAN>"
			//���ڴ�ű���DIV����TableSpan2����ʱ��ֻ�任�ñ����ʾ�ĵ����ݣ�����ʵ�ʹ���
			sUse[sUse.length] = "<DIV id='" + this.Name + "TableSpan' style ='left:0; height:" + ihigh + ";width:" + (sumWidth ) + "; position:absolute; OVERFLOW:hidden;z-index:100'>" + sWidth + "</DIV>"
			//����һ����������ڻ�ý��㡣�������������ʱ���ƶ����������ٰ����¼����������
			sUse[sUse.length] = "<INPUT TYPE=TEXT id='" + this.Name + "VirtualFocus" + iFast + "' style ='left:" + (3+sumWidth) + "; top:" + (this.Height) + ";height:1;width:0; position:absolute; OVERFLOW:hidden;z-index:107'" + " onKeyDown='" + this.Name + ".DealKeyPress()'  >"
		}
		else
		{
			//TableSpan2--���ڲ�����������span,�ں�һ���߶Ƚ��Ʊ��ĵ�DIV��Measure�����Ӷ�����������
			sUse[sUse.length] = "<SPAN id='" + this.Name + "TableSpan2" + iFast + "' style ='left:0; top:20; height:" + ihigh + ";width:" + (FastWidth) + "; OVERFLOW:hidden; z-index:1500' onscroll='" + this.Name + ".ScrollVer()'><DIV id=" + this.Name + "Measure" + iFast + " STYLE='WIDTH:" + (DivWidth+1) + ";HEIGHT:" + (this.rs_main.RecordCount * Math.floor(this.Height / 8) + this.Height+1) + ";POSITION:absolute;z-index:2000'></DIV></SPAN>"

			//���ڴ�ű���DIV����TableSpan2����ʱ��ֻ�任�ñ����ʾ�ĵ����ݣ�����ʵ�ʹ���
			sUse[sUse.length] = "<DIV id='" + this.Name + "TableSpan" + iFast + "K' style ='left:0; height:" + ihigh + ";width:" + (FastWidth ) + "; position:absolute; OVERFLOW:hidden;z-index:100'>" + sWidth + "</DIV>"

		}
		//sUse[sUse.length] = "<SPAN id='" + this.Name + "TableSpan' style ='height:" + this.Height + ";width:" + (sumWidth + 18) + "; OVERFLOW:auto' onscroll='" + this.Name + ".ScrollVer()'>" + sWidth + "<DIV STYLE='WIDTH:5;HEIGHT:" + (this.rs_main.RecordCount * 20) + ";POSITION:absolute'></DIV></SPAN>"
		sUse[sUse.length] = "</SPAN>"

	}

	//����β���Ƿ���ʾ����
	//�Ƿ���ʾ����
	if (this.AskSum && iFast == "")
	{

	//������β
		sUse[sUse.length] = TABLE_HEAD_T + HEAD_ROW_BEGIN.replace("<TR ","<TR id=" + this.Name + "Sums" + iFast + " ")

		//������ı���ͷ
		for (field=0; field<this.Widths.length; field++)
		{
				if (field != 0 & this.SumArr[field] != null)
				{
					this.UseSum(field,true)
			    }

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
						sDisplay = " style='display:none;font-size:8pt;font-weight:400; text-align:right'"
					else
						sDisplay = " style='font-size:8pt;font-weight:400; text-align:right'"
				}
			}


			//�Ƿ�ָ���ֶο��
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



	//���ر���HTML�ַ�����
	sUse[sUse.length] = TABLE_END

	//�Ƿ�����ҳѡ��
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
		//�Ƿ���ʾ��¼��
		var sCount = ""
		if (this.ShowCount == true)
			sCount = this.JILUSHU + this.rs_main.RecordCount

		//�Ƿ���ʾ����ѡ���ֶ�
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
		{//ʹ�ÿ��ٲ�ѯ������
			if (iFast == "")
				sTemp = "<TABLE id='" + this.Name + "Disp" + iFast + "'height=22 border=1 width=" + FastWidth + "><TR><TD align=left id=" + this.Name + "Count" + iFast + " style='font-family:����;font-size:9pt;filter:none' width=30%>" + sCount + "</TD><TD align=middle>" + sSearch + "</TD><TD align=middle>" + sFree + "</TD><TD align=right>" + sHide + "</TD></TR></TABLE>" + sTemp
			else
				//sTemp = "<TABLE id='" + this.Name + "Disp" + iFast + "'height=0' style='position:absolute;top:22'><TR><TD></TD><TD></TD><TD></TD></TR></TABLE>" + sTemp
				sTemp = "<Div style='POSITION: absolute; TOP: " + iFH + "px;'>" + sTemp + "</Div>"
		}
		else
		{//ʹ�ÿ��ٲ�ѯ��������
			if (iFast == "")
				sTemp = "<TABLE id='" + this.Name + "Disp" + iFast + "'height=22 width=" + FastWidth + "><TR><TD align=left id=" + this.Name + "Count" + iFast + " style='font-family:����;font-size:9pt;filter:none' width=30%>" + sCount + "</TD><TD align=middle>" + sSearch + "</TD><TD align=middle>" + sFree + "</TD><TD align=right>" + sHide + "</TD></TR></TABLE>" + sTemp
			else
				//sTemp = "<TABLE id='" + this.Name + "Disp" + iFast + "'height=0' style='position:absolute;top:22'><TR><TD></TD><TD></TD><TD></TD></TR></TABLE>" + sTemp
				sTemp = "<Div style='POSITION: absolute; TOP: " + iFH + "px;'>" + sTemp + "</Div>"

		}
	}
	//�����и�ʽ
	sTemp = sTemp + "   " + "<Style> <!--" + this.strColumn + sStyle + "--> </style>"
	
	//��֪Ϊʲô���ռ�¼���޷�������ʱAddNew��ֻ�ó����²ߡ�--- 1
	if (this.rs_main.RecordCount < 1 && noAdd != true)
	{
		this.rs_main.AddNew()
		if (this.rs_main.RecordCount > 0)
			this.rs_main(0) = "-1.0249E5"

		this.IsEmpty = true
	}
	else
		this.IsEmpty = false

	//������������������ʾ��
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
���ܣ�  ˢ�±���ɫ��
���ߣ�  ������
���ڣ�  2000.7
����ֵ��������괦��������¼���ݡ��ֶ����ֶμ��ÿո�ֿ���
*/
function _BackRefresh()
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
���ܣ�  �������ɾ��ѡ�е��к󣬴���ModiLog�����DelLog���顣
���ߣ�  ������
���ڣ�  2000.7
*/
function _DealLogWhenDel()
{
	//���ü�¼û�н��й��κβ���������롰ɾ�������顣
	if (this.arrModiLog[this.rs_main.AbsolutePosition] == null )
		this.arrDelLog[this.arrDelLog.length] = this.RowStr
	//���ü�¼����������¼�������ɾ�����顣
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
���ܣ�  �ӱ����ɾ��ѡ�е��С�
���ߣ�  ������
���ڣ�  2000.7
*/
function _DelRow(iEleRow)
{
	var oRows
	var oNodes
	//�Ƿ���ѡ�е���
	if (this.preElement != null)
		if (this.preElement.firstChild.name != "Virtul")
		{
			//ɾ������
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

			//����ɾ����ĸı�DelLog����
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
			//ѡ�б�־�ÿ�
			//�ָ�����Ϊ����


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
			//���������߶�
			eval("oRows = " + this.Name + "Measure")
			oRows.style.height = this.rs_main.RecordCount * Math.floor(this.Height / 8) + this.Height + 1
			//ˢ�±���
			//this.BackRefresh()
			if (this.FirstRow >= this.rs_main.RecordCount)
				this.FirstRow = this.rs_main.RecordCount - 1

			if (this.FirstRow < 0)
				this.FirstRow = 0

			this.ChangePage()

			this.ClearV()

			//�Ƿ���ʾ����
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
���ܣ�  ����ѡ���е����ݡ�
���ߣ�  ������
���ڣ�  2000.7
������  Content -- �������ݣ���","�ָ���
*/
function _SetRow(Content)
{
	var oNodes,oFastNodes,i,strTD

	if (this.preElement == null)
		return null

	if (this.preElement.firstChild.name == "Virtul")
		return null



	//��������
	strTD = Content.split(this.Divide)
	for (var key in this.Calculator)
	{
	   var 	strCalculate = this.Calculator[key]
	   strCalculate = strCalculate.replace(/\</g, "strTD[");
	   strCalculate = strCalculate.replace(/\>/g, "]");

	   strCalculate = strCalculate.replace("=", "=Math.round((") + ")* Math.pow(10,this.SumDot)) / Math.pow(10,this.SumDot)"
	   eval(strCalculate);	
	}
	//�õ����е�Ԫ��
	oNodes = this.preElement.childNodes
	//�̶���λ�ı��
	if (this.preFastElement != null)
		oFastNodes = this.preFastElement.childNodes
	else
		oFastNodes = null

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
			//��֤�Ƿ�Ϊ��ֵ
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
���ܣ�  ���������һ�С�
���ߣ�  ������
���ڣ�  2000.7
������  Content -- �������ݣ���","�ָ���
*/

function _Append(Content)
{
	var iRow,i,strTD,oNodes
	//�õ���������


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

	//��ǰ��ʾλ��
	//eval("iRow = " + this.Name + ".rs_main.RecordCount * 20")
	eval("iRow = " + this.Name + ".rs_main.RecordCount")
	//this.FirstRow = this.rs_main.RecordCount


	//���һ��
	eval(this.Name + ".AddEvent(true,strTD)")

	//���������߶�
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
���ܣ�  �������ֶ�����������Զ����һ�С�
���ߣ�  ������
���ڣ�  2000.7
������  bAppend -- �Ƿ��ֶ�
		Cells   -- ��Ԫ��������
*/
function _AddEvent(bAppend,Cells)
{
	if (bAppend !=true && this.AddDelete!=true )
		return null
	//����һ�м�¼
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

	//���θ�ֵ
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

	//����һ��ʱ,�����������ӱ�־.
	this.arrModiLog[this.rs_main.AbsolutePosition] = 1

	//window.alert("Count2 = " + this.rs_main.RecordCount)


/*
//##############################################
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
//##############################################
*/
	this.ClearV()

}

/*
���ܣ�  �������<input>��MouseDown������Move���Ǹ����к�MouseUp,
		�������Ӧ�¼��г����ر�IE��Ϊ�������ô˺����������������
		����ʱ����ӦMouseUp�¼���
���ߣ�  ������
���ڣ�  2001.3
*/


function _TDMouseDown()
{
	var srcNode		//��ʱ���󣬱���MouseUp������
	//alert(2)
	//��Ϊ������������ʾcheckbox�������������δ˹��ܣ��������ؼ����в�����
	return
	if (event.srcElement.tagName != "TD")
	{
	//��MouseDown�¼������ڱ���еĲ���ؼ�ʱ��Ϊ��ֹMouseOut����MouseUp
	//�ر����¼���������
		eval("srcNode = " + this.Name + ".DownEventSrc")

		//��ֹ�ظ����棬����ֻ����λ�����ʱ������
		if (srcNode == null)
		{	//����
			eval(this.Name + ".DownEventSrc = " + this.Name + "TableAll.onmouseup")
			//����Mouseup�¼���
			eval(this.Name + "TableAll.onmouseup=null")
		}
	}
	else
	{	//�ָ��¼�����
		eval("srcNode = " + this.Name + ".DownEventSrc")
		if (srcNode != null)
		{	//�ָ�
			eval(this.Name + "TableAll.onmouseup = srcNode")
			//�������ֵ
			eval(this.Name + ".DownEventSrc = null")
		}
	}


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

function _MouseDown_Event(Source,Direct,keyControl)
{
	var elerow,fastRow,strRe, EnableFileld
	//aa = bb.sub()
	//alert(1)
        if (this.CanNotChangRow)
            return
	this.ClearV()
	//�¼�Դ
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


	//��Ϊ�Ҽ����˳�
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

		//����׷���¼��ĸ������Ƿ�Ϊ��¼�ж���
		if (elerow.parentElement.parentElement != null)
			elerow = elerow.parentElement;
		if (elerow.parentElement.classid == this.Name + "TableRow" || elerow.parentElement.classid == this.Name + "TableRow" + this.FastField)
		{
			elerow = elerow.parentElement;
		}

    //���Ǽ�¼������Ӧ���¼�
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
		//�Զ�ɾ��һ����¼
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

				//������ѡ���ʱ�������Զ�ɾ����
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

		//�õ���һ���������ֶ�
		EnableFileld = this.GetFirstEnableChild()

		//�Զ�����һ����¼
		//if (this.AddDelete == true && elerow.childNodes(EnableFileld).id == this.Name + "Add" && (this.rs_main.RecordCount < this.MaxLine))
		if (this.AddDelete == true && elerow.childNodes(EnableFileld).id == this.Name + "Add" )
		{

			this.AddDelete == false
			elerow.childNodes(EnableFileld).id = null
			//this.LightRow = this.rs_main.RecordCount
			this.AddEvent()
			//���������߶�
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

		//�����ǵ�һ����Ӧ���¼����ָ���һ�ε���������ɫ��
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
		//���汳��ɫ���Ա��ָ���
		this.preColor = elerow.style.backgroundColor;
		//ָ���µı���ɫ��
		elerow.style.backgroundColor = this.LightBKColor    //"blue";
		elerow.style.color = this.LightColor    //"yellow"


		//���ظü�¼���ݡ�

		//����ǰ�Ĳ���
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

		//Ϊ�˱�����������¼�����ʹ�ø÷����ָ����̴���
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


		//������У��Ա��ָ���

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
����  ����һ��ʧȥ����ʱ������������ݣ������¼��
����  ��������
ʱ��  ��2000��8��
����  ��
����ֵ��
*/
function _SaveRow(NoCh)
{
//���������Ϊ��ֻ�����¼�������ı���ۡ�
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
//��¼���������

//�����������
	for (i=0; i<this.Widths.length; i++)
	{
		strRow += this.rs_main.Fields(i).Value

		if (i < this.Widths.length -1)
					strRow += this.Divide

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
					strTemp += ""
					strTemp = strTemp.replace(/(^\s*)|(\s*$)/ig,"");
					//if (NoCh == null)
					//	eval(this.UseForm + this.Name + "Text" + i + ".blur()")

					if (this.FieldsType[i] == "num")
					{
						//aa = bb.sub()
						if (parseFloat(strTemp) * 1 != parseFloat(strTemp)|| parseFloat(strTemp) == 0)
						{	//������Ϊ�㣬��ֱ�ӷ���
							if (this.SetZ == false)
								strTemp = "*SetZ*"
							else
								strTemp = 0
							//this.preElement.childNodes(i).innerText = this.rs_main.Fields(i).value
							//this.preElement.childNodes(i).style.padding=2
							//�����¼������
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

					//�����¼������
					if (strTemp != "*SetZ*" )
						this.rs_main.Fields(this.GetOrder[i]).value = strTemp;
					//strRow += strTemp + this.Divide
					if (this.preFastElement != null)
						this.preFastElement.childNodes(i).innerHTML = this.preElement.childNodes(i).innerHTML
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
							//�����¼������
							this.rs_main.Fields(this.GetOrder[i]).value = strTemp;
						//strRow += strTemp + this.Divide

						if (this.preFastElement != null)
							this.preFastElement.childNodes(i).innerHTML = this.preElement.childNodes(i).innerHTML

						continue
				}

			////���������ݱ���
			//if (this.preElement.childNodes(i).name != "Virtul")
			//{	strTemp = this.preElement.childNodes(i).innerText
				//this.rs_main.Fields(i).value = strTemp
			//	strRow += strTemp + this.Divide
			//}
		}

		if (this.preFastElement != null)
			this.preFastElement.childNodes(i).innerHTML = this.preElement.childNodes(i).innerHTML

	}
	//�����¼������
	//this.rs_main.update()



	//�жϵ�ǰ���Ƿ��޸�
	for (i=0; i<this.Widths.length; i++)
	{	strRe += this.rs_main.Fields(i).Value

		if (i < this.Widths.length -1)
					strRe += this.Divide
	}
	if (strRe != strRow)
	{	this.arrModiLog[this.rs_main.AbsolutePosition] = this.arrModiLog[this.rs_main.AbsolutePosition] | 2
	
		//�Ƿ���ʾ����
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
����  ��
����  ��������
ʱ��  ��2000��8��
����  ��
����ֵ��
*/
function _EditRow(Rownum,oFast)
{
//outControl �����ⲿ������ʱ�ı�����ʾģʽ
	var i,j,strTemp,strCats,strStyle,iWidth,strRe,oRows,oFastRows,oParent,ilen
	var selectText = null	//�����趨��һ��ѡ���ѡ��
	var sInHT
	strRe = ""	//�����ִ�
	eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild.childNodes(Rownum-this.FirstRow).childNodes")

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
		if (Rownum < this.rs_main.RecordCount)
		{	this.rs_main.MoveFirst()
			this.rs_main.Move(Rownum)
		}

		eval(this.BeforeNew)

		//��������ɨ��
		for (i=0; i<this.Widths.length; i++)
		{
			//���»���ʵ��
			//oRows(i).style.borderTop="1 solid #000000"
			//oRows(i).style.borderBottom="1 solid #000000"
			//�����ص��У�����û�и��¹����������¸�ֵ
			if (Rownum < this.rs_main.RecordCount)
			{
/* 2004-08-10 commen for don't net auto sort, and replace this function with add sort number save to EJB
				//if (this.rs_main.Fields(i).Name == "���")
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
			//�����пɱ༭��Ϊ�����
			if (this.modi != null)
			{
				//�����µı༭��
				if (this.modi[i] && oRows(i).name != "Virtul")
				{	//�༭����
					if (this.seedControl[i] != null & oRows(i).name != "Virtul")
						iWidth = this.Widths[this.GetOrder[i]] - 28
					else
						iWidth = this.Widths[this.GetOrder[i]] - 1
					//�༭��ȱʡ����
					if (this.SetZ == true || this.rs_main.Fields(this.GetOrder[i]).Value * 1 != 0  || this.FieldsType[this.GetOrder[i]] != "num")
					{
						if (this.ShowZ == true || this.rs_main.Fields(this.GetOrder[i]).Value != 0   || this.FieldsType[this.GetOrder[i]] != "num")
							strTemp =  this.rs_main.Fields(this.GetOrder[i]).Value
						else
							strTemp = ""
					}
					else
						strTemp = ""
					//�༭����
					strStyle = ' Style="WIDTH:' + iWidth + 'px; border-bottom:1 solid black;"'
					//�Ƿ�Ϊ��ֵ
					if (this.FieldsType[i] == "num" || this.FieldsType[i] == "strnum")
						strStyle += " onKeyDown='_DealNumberPress()' onKeyPress='_DealNumberPress()' onpaste='event.returnValue=false' ondrop='event.returnValue=false' onchange='" + this.Name +".SetCell()'"
						//strStyle += " onKeyDown='_DealNumberPress()' onKeyPress='_DealNumberPress()' onpaste='event.returnValue=false' ondrop='event.returnValue=false' onchange='alert(1)'"
					else
						strStyle += "  ondrop='event.returnValue=false' "
					//ȱʡֵ
					//alert(strTemp)
					//alert(this.ChangeK(strTemp))
					strStyle += ' value="' + this.ChangeK(strTemp) + '"'
					//�Ƿ�ָ����󳤶�
					if (this.MaxSize[i] != null)
						strStyle += " maxlength=" + this.MaxSize[i]
					//��ǰ��<TD></TD>�У�����༭��HTML�ĵ���
					oRows(i).innerHTML = "<input type=Text id=" + this.Name + "Text" + i + strStyle + " >"
					oRows(i).style.padding = 0

					//�趨��һ���ı���Ϊ�н��㣬ѡ��״̬
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

			//���ɿؼ���
			if 	(i < this.seedControl.length)
			if (this.seedControl[i] != null  && oRows(i).name != "Virtul")
			{
				//�����µ�ѡ���,��ǰ��<TD></TD>�У�����ѡ���HTML�ĵ���
				oRows(i).style.padding = 0
				//����ؼ�����
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
	//����ǰ����Ϊǰһ�У��Ա���һ�αȽϡ�
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
����  ���õ���ǰ�е�����ֵ��
����  ��������
ʱ��  ��2000��8��
����  ��
����ֵ������ֵ��
*/
function _GetRownum()
{
	return this.LightRow * 1;
}

/*
����  ��ˢ�±�����������
����  ��������
ʱ��  ��2000��8��
����  ��
����ֵ��
*/
function _Refresh(withEvent)
{
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

				//������ѡ���ʱ�������Զ�ɾ����
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

	//�����ǵ�һ����Ӧ���¼����ָ���һ�ε���������ɫ��

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

function _ClearV(NoCount)
{
	//��֪Ϊʲô���ռ�¼���޷�������ʱAddNew��ֻ�ó����²ߡ�--- 1
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

	
	//������������������ʾ��
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


//ֻ�ܳ������ֺ�С���㡣
 if (charCode>31 && (charCode<48 ||charCode>57) && (charCode<96 ||charCode>105) && charCode != 46 && charCode != 110 && charCode != 190 && charCode != 37 && charCode != 39  && charCode != 35  && charCode != 36)
   	event.returnValue =false;

//����ͬʱ��������С���㡣
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
		case 37:	//��

			break;

		case 38:	//��
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

		case 39:	//��

			break;
		case 13:	//�س�
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

		case 40:	//��
			//eval("oRows = " + this.Name + "TableSpan.firstChild.firstChild.childNodes")

			if (this.currentRow < this.rs_main.RecordCount)
			{	iRow += 1
				//���Զ�ɾ�����ٽ�����������������������Ӵ˶δ��룬Ϊ�˽�ֹ���̲����µ��Զ�ɾ����
				
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
���ַ���ת����unicode
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
	var arrAMD = new Array("D","A","M","A"); //"D"ɾ�� "A"���� "M"�޸�
   	var arrRow = new Array();
   	var arrMask = mask.split(",");

	//�������Ӻ��޸ĵļ�¼
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
	//����ɾ���ļ�¼
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
��' �滻�� \\\'
����������Ȼ
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
