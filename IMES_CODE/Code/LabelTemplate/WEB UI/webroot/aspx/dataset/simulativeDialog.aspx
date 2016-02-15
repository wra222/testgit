     
<div id="dlgN" align="center" style="display:none;filter:Chroma(Color=red); background-color: red;  top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
<fieldset style="position:relative;top:100px;width: 270px;background-color: rgb(147,191,218);">    
        <input id="dataType_s" name="dataType_N" type="radio" checked onclick="txtValue.select();"/>String&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <input id="dataType_n" name="dataType_N" type="radio" onclick="txtValue.select();"/>Number
        <br/><br />
        <%--<bug>
            BUG NO:ITC-992-0045  
            REASON:È¥µô½ûÖ¹É¾³ýµÄÊôÐÔ
        </bug>--%>
        <input id="txtValue" type="text" maxlength="100" style="width:220px"   onkeypress="if(event.keyCode==44)return false;"/> 
        <table style="width: 100%; height: 30px;background-color: rgb(147,191,218);" border="0">
            <tr height="40">
                <td width="53%">
                </td>
                <td >
                    <button onclick='completeInputValue("N")' ><%=Resources.Template.okButton%></button>&nbsp;&nbsp;&nbsp;
                    <button onclick='dlgN.style.display = "none";dataType_s.checked=true;' ><%=Resources.Template.cancelButton%></button>
                </td>
                 
            </tr>
                
        </table>
 
</fieldset>
</div>
<%--<div id="dlgIN" align="center" style="display:none;filter:Chroma(Color=red); background-color: red;  top: 0; width: 100%; height: 100%; z-index: 10000; position: absolute">
    <fieldset style="position:relative;top:100px;width: 220px;background-color: rgb(147,191,218);" align="center">  
        <div align="center" style="width:100%;">
		    <input id="types_String" name="dataType_IN" type="radio" checked onclick="txtValue.select();"  />String&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <input id="types_Number" name="dataType_IN" type="radio" onclick="txtValue.select();valueListOfIN.innerHTML=''"/>Number
        </div>
            <table style="width: 100%; background-color: rgb(147,191,218);" border="0">
                <tr>
                    <td>				 
					    <input id="txtValue_IN" type="text" maxlength="100" style="width:220px" onpaste="return false;" onkeypress="if(event.keyCode==44)return false;"/> 
                    </td>
                    <%--<td >
                       <button onclick='addItem_IN();' style="width: 30px">Add</button>&nbsp;&nbsp;
				       <button onclick='delItem_IN();' style="width: 30px">Del</button>&nbsp;&nbsp;&nbsp;
                    </td>  --%>               
<%--                </tr>--%>
			   <%-- <tr height="90">
                    <td colspan="2">
				    <SELECT NAME="valueListOfIN" id="valueListOfIN" style="width:100%;height:100%" multiple>

				    </SELECT>
                    </td>
                </tr>--%>
			   <%-- <tr height="40">
                    <td width="53%">
                    </td>
                    <td >
                        <button onclick='completeInputValue("IN")' >OK</button>&nbsp;&nbsp;&nbsp;
                        <button onclick='dlgIN.style.display = "none";'>Cancel</button>
                    </td>
                     
                </tr>
                    
            </table>
     
    </fieldset>
</div>    --%>