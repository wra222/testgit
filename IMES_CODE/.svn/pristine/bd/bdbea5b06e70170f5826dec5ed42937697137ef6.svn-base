<%--
' INVENTEC corporation (c)2008 all rights reserved. 
' Description: 模板预览页面
'             
' Update: 
' Date         Name                Reason 
' ==========   ==================  =====================================    
' 2009-05-04   liu xiaoling(EB2)  create
' Known issues:Any restrictions about this file
--%> 
<%@ Page Language="C#"%>
<%@ Import Namespace=" System.IO" %>
<%@ Import Namespace="com.inventec.system" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="com.inventec.template.structure" %>


<head id=Head1 runat="server">
    <title>Untitled Page</title>
</head>

<script runat="server">
    void Page_Load(object sender, EventArgs e)
    {


        //获取url传递的参数
        String picName = Request.QueryString["name"];
        String page = Request.QueryString["page"];

        if (!String.IsNullOrEmpty(picName))
        {
            //Session[picName] = "R0lGODlhCwAMAJH/AP///7+/vwAAfwAAACwAAAAACwAMAEACHYyPMSDsPVabLiYUnx43xQ9+wkiSUYmeqBm2HVIAADs=";
            List<ImageInfo> imgList = (List<ImageInfo>)System.Web.HttpContext.Current.Session[picName];

            System.IO.Stream stream = new System.IO.MemoryStream(imgList[int.Parse(page)].Image);
            System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
            int sourceWidth = img.Width;
            int sourceHeight = img.Height;
            float verticalResolution = img.VerticalResolution;
            float horizontalResolution = img.HorizontalResolution;
                                    
            if (page.Equals("-1"))
            {
                this.Response.Write(imgList.Count);
            }
            else
            {
                this.Response.BinaryWrite(imgList[int.Parse(page)].Image);
            }
            this.Response.End();
        }
       
      




    }
</script>








