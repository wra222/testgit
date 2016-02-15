var IMESUti = IMESUti || {};

IMESUti.BomTool=(
 
function(){    	
// bomTool=IMESUti.BomTool(bomList,tbl,DEFAULT_ROW_NUM);
	return function(bomItemList, gridName, defaultRowCount) {
			var _bomItemList = bomItemList;
			var _gridName = gridName;
			var _defaultRowCount = defaultRowCount;
			return { BindBomGridEx : function() {
							
								var _gid='#' + _gridName +' tr';
							
				     			for (var i = 0,bi; bi=_bomItemList[i]; i++) {
				     			
				     			   if(i>(defaultRowCount-2))
				     			   {
				     			     this.AddRow(_gridName,bi);
				     			     //  (_gridName,bi);
				     			      continue;
				     			   }
								   var _tr=  $(_gid).eq(i+1).find('td');
								   _tr.eq(0).html(bi.type || '' );
								   _tr.eq(1).html(bi.description || '');
								   var _p=bi.PartNoItem.split(',');
								   if(_p.length>1)
								   {
								    _tr.eq(2).html(_p[0]+',...').attr('title',bi.PartNoItem);
							       }
								   else
								   {
								     _tr.eq(2).html(bi.PartNoItem);
								   }
							      _tr.eq(3).html(bi.qty);
								  _tr.eq(4).html(bi.scannedQty);
								  _tr.eq(5).html('');
								  
								   
								 }
							},
    
					BindBomGrid : function() {
									for (var i = 0; i < _bomItemList.length; i++) {
										var rowArray = new Array();
										var rw;
										var collection = _bomItemList[i].collectionData;
										var parts = _bomItemList[i].parts;
										var tmpstr = "";
										if (_bomItemList[i].type == null) {
											rowArray.push("");
										}
										else {
											rowArray.push(_bomItemList[i].type);
										}

										if (_bomItemList[i].description == null) {
											rowArray.push("");
										}
										else {
											rowArray.push(_bomItemList[i].description);
										}
										if (_bomItemList[i].PartNoItem == null) {
											rowArray.push("");
										}
										else {
											rowArray.push(_bomItemList[i].PartNoItem);
										}
										rowArray.push(_bomItemList[i].qty);
										rowArray.push(_bomItemList[i].scannedQty);
										rowArray.push(""); //["collectionData"]);
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
									}
								},
 
					GetMatchGridIdxAndUpdateQty:function(matchPart) {
					           // $.map() =  .Select
		                       var _arr=$.map(_bomItemList, function(item) { 
                                      return item.GUID; 
                                    });
                                var _idx = $.inArray(matchPart.FlatBomItemGuid,_arr);
                                if(_idx==-1)
                                {
                                   return _idx;
                                }
                                else
                                {
                                  _bomItemList[_idx].scannedQty++;
                                  var _tr=$("#" + _gridName).find(" tr:eq("+parseInt(_idx+1)+")");
                                  _tr.siblings().removeClass("MatchItem");
                                  _tr.addClass("MatchItem");
                                  return _idx;
                                }
                        },

					GetMatchAndUpdateQty: function(matchPart) {
							var SubstitutePartNo = "";
							for (var i = 0; i < _bomItemList.length; i++) {
								if (_bomItemList[i].parts.length >= 1) {
									for (var j = 0; j < _bomItemList[i].parts.length; j++)
									{
										SubstitutePartNo += _bomItemList[i].parts[j].id;
										if (j < _bomItemList[i].parts.length - 1)
										{
											SubstitutePartNo += ";";
										}
									}
								}
								else{
									SubstitutePartNo = "";
								}
								if (SubstitutePartNo.indexOf(matchPart.PNOrItemName) != -1) {
									_bomItemList[i].scannedQty++;
									return i;
								}
							}
							return -1;
						},
    
   

			UpdateMatchPartDataInGrid: function(idx, matchPart) {
							var con = document.getElementById(_gridName).rows[idx + 1];
							con.cells[4].innerText = _bomItemList[idx].scannedQty;
							//con.cells[5].innerText = con.cells[5].innerText + " " + matchPart.PNOrItemName;
							con.cells[5].innerText = con.cells[5].innerText + " " + matchPart.CollectionData;
							con.cells[5].title = con.cells[5].innerText;
							
						},
            DoAllMatchAction: function(matchPart) {
                var idx=this.GetMatchGridIdxAndUpdateQty(matchPart);
                this.UpdateMatchPartDataInGrid(idx,matchPart);
                // $.grep()= .Where  Ist.where(x==.'xxx').toList
                var _ar = $.grep(_bomItemList, function(_bi) {
                      return  _bi.qty !=_bi.scannedQty;
                    });
                   var _r=  _ar.length == 0; //=> FinishScan or not
                   if(_r)
                   {
                      $("#" + _gridName).find(" tr").removeClass('MatchItem');
                   }
                   return _r;
            },
			CheckFinishScan : function() {
								var ret = true;
								for (var i = 0; i < _bomItemList.length; i++) {
									if (_bomItemList[i].qty != _bomItemList[i].scannedQty) {
										ret = false;
										break;
									}
								}
								if(ret)
								{
								 $("#" + _gridName).find(" tr").removeClass('MatchItem')
								}
							   return ret;
						},
			AddRow : function(gr,bi) {
			         var _c= $('#'+gr+' > tbody > tr').length;
		         var _x = _c % 2;
		         var _s='iMes_grid_RowGvExt';
		         if(_x==0)
		         {
		           _s='iMes_grid_AlternatingRowGvExt';
		         }
		          $('#'+gr+' tbody').append(
				         $('<tr>').append(
						        $('<td>').text(bi.type || '' ),
						        $('<td>').text(bi.description || '' ),
						        $('<td>').text(bi.PartNoItem || '' ),
						        $('<td>').text(bi.qty),
						        $('<td>').text(bi.scannedQty),
								$('<td>').text('')
						        
					         ).addClass(_s)
			        );
		   }	
		}
	}	
})();

