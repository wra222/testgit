<%--
' INVENTEC corporation (c)2008 all rights reserved. 
' Description: Ä£°åÔ¤ÀÀÒ³Ãæ
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
        Guid guid = System.Guid.NewGuid();
        String uuid = guid.ToString("N");
        this.Response.Write(uuid);
        this.Response.End();
    }
</script>








