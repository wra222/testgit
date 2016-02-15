<%@ Page Language="C#" %>
<%@ Import Namespace=" System.IO" %>
<%@ Import Namespace="com.inventec.system" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Collections.Generic" %>


<head id=Head1 runat="server">
    <title>Untitled Page</title>
</head>

<script runat="server">
    void Page_Load(object sender, EventArgs e)
    {


        //获取url传递的参数
        String picName = Request.QueryString["name"];

        if (!String.IsNullOrEmpty(picName))
        {
            //Session[picName] = "R0lGODlhCwAMAJH/AP///7+/vwAAfwAAACwAAAAACwAMAEACHYyPMSDsPVabLiYUnx43xQ9+wkiSUYmeqBm2HVIAADs=";
            IDictionary map = (IDictionary)System.Web.HttpContext.Current.Session[Constants.SESSEIONPICMAP];
            String picContent = (String)map[picName];
            if (!String.IsNullOrEmpty(picContent))
            {
                this.Response.ContentType = "image/GIF";
                string uploadPath = Server.MapPath("~/webroot/images");
                string file = Path.Combine(uploadPath, Guid.NewGuid().ToString()) + ".tmp";

                System.IO.FileStream fs1 = (FileStream)StreamConvertString.FromBase64String(picContent, uploadPath, file);

                BinaryReader br = new BinaryReader(fs1);

                byte[] photo = br.ReadBytes((int)fs1.Length);
                System.IO.File.Delete(file);
                this.Response.BinaryWrite(photo);
                this.Response.End();
            }
        }
       
      




    }
</script>



