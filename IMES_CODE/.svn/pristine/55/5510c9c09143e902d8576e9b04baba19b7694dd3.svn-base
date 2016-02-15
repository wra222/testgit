<%@ Page Language="C#"%>
<%@ Import Namespace=" System.IO" %>
<%@ Import Namespace="com.inventec.system" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="com.inventec.template.structure" %>
<%@ Import Namespace="com.inventec.template" %>



<head id=Head1 runat="server">
    <title>Untitled Page</title>
</head>

<script runat="server">
    void Page_Load(object sender, EventArgs e)
    {


        //获取url传递的参数
        /*
        BarcodeObject objBarcode = (BarcodeObject)HttpContext.Current.Session["barcode"];

        DrawLabel manager = new DrawLabel();
        ImageInfo tempimage = manager.Render(objBarcode);*/
        //获取url传递的参数
        String picName = Request.QueryString["picName"];

        ImageInfo tempimage = (ImageInfo)System.Web.HttpContext.Current.Session[picName];
        this.Response.BinaryWrite(tempimage.Image);

       this.Response.End();

    }
</script>