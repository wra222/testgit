/*
 * INVENTEC corporation (c)2007 all rights reserved. 
 * Description: 常量类包括脚本
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2007-05-5 Li.Ling-Nan(206016)   Create 
 * 
 * Known issues:
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace waitCover
{
    class JavaScriptConstant
    {      
        internal static string CLIENT_ID = "@clientID";
        internal static string KeyDownFun = "@forkeydownreplace";
        internal static string WaitingCoverDiv = @"
        <script type='text/javascript'>

         //启动等待画面
         function beginWaitingCoverDiv()
         {  
            var divCover = document.getElementById('" + CLIENT_ID + @"');
            //divCover.style.width = 1024;
            //divCover.style.height = 768;
            //divCover.style.width = document.body.offsetWidth;
            //divCover.style.height = document.body.offsetHeight;
            divCover.style.top = 0;
            divCover.style.left = 0;
            divCover.style.zIndex = 99;
            divCover.style.display = 'block';

            //body on focus()
            document.body.focus();

            //set body's height
            document.body.style.height = '100%';

            //tab key is not use
            document.onkeydown = function()
            {
                return false;
            };  
            
            var arrSelectObj = window.document.all.tags('select'); 
            var waitSelLength = arrSelectObj.length;

              //all dropdownlist is not use
	        for (var i = 0; i < waitSelLength; i++)
            {
                var oElement = arrSelectObj.item(i);   
                oElement.disabled=true;
            }      

//            //all dropdownlist is not use
//	        for (var i = 0; i < window.document.all.tags('select').length; i++)
//            {
//                var oElement = window.document.all.tags('select').item(i);   
//                oElement.disabled=true;
//            }
//
//             //all text is not use
//	        for (var i = 0; i < window.document.all.tags('text').length; i++)
//            {
//                var oElement = window.document.all.tags('text').item(i);
//                oElement.disabled=true;
//            }
        }

        //关闭等待画面
        function endWaitingCoverDiv()
        {
            var divCover = document.getElementById('" + CLIENT_ID + @"');
            divCover.style.display = 'none';

             //tab key is  use
            document.onkeydown = function()
            {
                if('" + KeyDownFun + @"'=='')
                {
                    return true;
                }
                else
                {
                    return eval('" + KeyDownFun + @"');
                }
            };

            var arrSelectObj = window.document.all.tags('select'); 
            var waitSelLength = arrSelectObj.length;

              //all dropdownlist is not use
	        for (var i = 0; i < waitSelLength; i++)
            {
                var oElement = arrSelectObj.item(i);   
                oElement.disabled=false;
            }      

//            //all dropdownlist is use
//            for (i = 0; i < window.document.all.tags('select').length; i++)
//            {
//               var oElement = window.document.all.tags('select').item(i);
//               oElement.disabled = false;
//            }
//
//              //all text is not use
//	        for (var i = 0; i < window.document.all.tags('text').length; i++)
//            {
//                var oElement = window.document.all.tags('text').item(i);
//                oElement.disabled=false;
//            }

        }

        function IsCoverDivWaiting()
        {
            var divCover = document.getElementById('" + CLIENT_ID + @"');
            if((divCover!=null)&&(divCover!=undefined)&&(divCover.style.display == 'block' ||divCover.style.display == '' ||divCover.style.display==true))
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
        </script>
        ";     
    }
}
