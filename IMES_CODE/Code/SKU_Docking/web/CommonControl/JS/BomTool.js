 

function BomTool(bomItemList, gridName, defaultRowCount) {
    var _bomItemList = bomItemList;
    var _gridName = gridName;
    var _defaultRowCount = defaultRowCount;
    var _disableTooltip = false;
    this.DisableTooltip = function() {
        _disableTooltip = true;
    }
    this.BindBomGrid = function() {
        for (var i = 0; i < _bomItemList.length; i++) {
            var rowArray = new Array();
            var rw;
            var collection = _bomItemList[i].collectionData;
            var parts = _bomItemList[i].parts;
            var tmpstr = "";
            if (_bomItemList[i].PartNoItem == null) {
                rowArray.push("");
            }
            else {
                rowArray.push(_bomItemList[i].PartNoItem);
            }
            rowArray.push(_bomItemList[i].tp);
            if (_bomItemList[i].description == null) {
                rowArray.push("");
            }
            else {
                rowArray.push(_bomItemList[i].description);
            }
            rowArray.push(_bomItemList[i].qty);
            rowArray.push(_bomItemList[i].scannedQty);
            rowArray.push(""); //["collectionData"]);
            rowArray.push(""); //PreCheck
            if (i < _defaultRowCount) {
                eval("ChangeCvExtRowByIndex_" + _gridName + "(rowArray, false, i+1);");
                setSrollByIndex(i, true, _gridName);
            }
            else {
                eval("rw = AddCvExtRowToBottom_" + _gridName + "(rowArray, false);");
                setSrollByIndex(i, true, _gridName);
                rw.cells[1].style.whiteSpace = "nowrap";
            }
            setSrollByIndex(0, true, _gridName);
       //     if (!_disableTooltip)
        //    { SetToolTip(_gridName); }
       
        }
    }
 
    this.GetMatchGridIdxAndUpdateQty=function(matchPart) {
        for (var i = 0; i < _bomItemList.length; i++) {
            if (_bomItemList[i].GUID == matchPart.FlatBomItemGuid) {
                if (!matchPart.IsPreCheckedPart)
                { _bomItemList[i].scannedQty++; }
                return i;
            }
        }
        return -1;
    }

    this.UpdateMatchPartDataInGrid = function(idx, matchPart) {
        var con = document.getElementById(_gridName).rows[idx + 1];
        con.cells[4].innerText = _bomItemList[idx].scannedQty;
        if (matchPart.IsPreCheckedPart) {
            if (con.cells[6].innerText == "") {
                con.cells[6].innerText = matchPart.CollectionData; //CollectionData
            }
            else {
                con.cells[6].innerText = con.cells[6].innerText + "," + matchPart.CollectionData; //CollectionData
            }

        }
        else {
            if (con.cells[5].innerText  == "") {
                con.cells[5].innerText = matchPart.CollectionData;
              //  con.cells[5].innerText = matchPart.PNOrItemName;
            }
            else {
                con.cells[5].innerText = con.cells[5].innerText + "," + matchPart.CollectionData;
               // con.cells[5].innerText = con.cells[5].innerText + "," + matchPart.PNOrItemName;
            }
            //con.cells[5].title = con.cells[5].innerText;
        }
    }

    this.CheckFinishScan = function() {
        var ret = true;
        for (var i = 0; i < _bomItemList.length; i++) {
            if (_bomItemList[i].qty != _bomItemList[i].scannedQty) {
                ret = false;
                break;
            }
        }
        return ret;
    }
  
}
function SetToolTip(tbId) {
    tbId = "#" + tbId;
    $(tbId + ' tbody tr td:nth-child(1) ,' + tbId + ' tbody tr td:nth-child(3)').each(function(i) {
        //skip header
        var sContent = $(this).text();

        if (sContent.trim() != "") {
            $(this).attr("title", $(this).html()).poshytip();
            $(this).text(sContent);

        }
    });

}