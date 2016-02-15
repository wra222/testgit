/**
    ' ======== ============ =============================
    ' Description: create progress bar in the center of screen
    ' Author: itc204033
    ' Side Effects:bar每次增加"&nbsp;|"
    ' Date:2008-12-19
    ' ======== ============ =============================
*/ 
  
  //将读取进度初始值设定为0   
  var percent_i = 0;  
  
//定义关闭进度显示窗口函数   
  function closebar()   
  {   
      //关闭进度显示窗口   
      document.all('divProgressBar').style.display = 'none';  
      percent_i = 0;   
      document.all('bar_title').innerText = ''; 
      document.all('bar_load').style.width = '0';  
      document.all('bar_title').style.display = 'none'; 
      document.all('prsbar').style.display = 'none'; 
  }   
  
  
  //定义显示进度条函数   
  function loadbar(text_str, per_i, finished)   
  {   
      //判断是否读取完毕   
      if (finished)   
      {   
          //显示“读取完毕”   
          document.all('bar_title').innerText = text_str + '   ' + '100%'; 
 
          //显示进度条   
          document.all('bar_load').style.width =  '100%';

          //暂停1秒钟，然后关闭   
          setTimeout('closebar()', 1000);   
      }   
      else   
      {   
          //载入百分比  
          percent_i = per_i;
         
          //增加进度条长度   
          document.all('bar_load').style.width = percent_i + '%';
          //显示提示文字   
          document.all('bar_title').innerText = text_str + '   ' + percent_i + '%'; 
      }   
  }   
     
  
/*============================
  页面需加载如下div
  
      <div id="divProgressBar" align="center" style="filter:Chroma(Color=skyblue); background-color: skyblue; display: none; top: 0; width: 100%; height: 100%; z-index: 100; position: absolute">
      </div>
      <div id='bar_title' style="display:none;">loading......</div> 
      <div  id='bar_load' style="width:0px;height:5px; background-color:#5586DB;font-size:6pt; color:#FFFFFF; border: solid 1px #93BEE0; display:none;">
      </div>
================================*/