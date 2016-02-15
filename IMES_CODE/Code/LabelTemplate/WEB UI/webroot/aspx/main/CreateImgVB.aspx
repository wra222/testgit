<%@ Page Language="VB"  Theme=""  ContentType="image/gif" %>
<%@ assembly name="MW6.ASPNET.Barcode" %>
<%@ import Namespace="System.Drawing" %>
<%@ import Namespace="System.Drawing.Drawing2D" %>
<%@ import Namespace="MW6.ASPNET.Barcode" %>
<script runat="server">

    Public Sub OutputImg()

        Dim objBitmap As Bitmap
        Dim objGraphics As Graphics
        Dim ActualWidth As Integer
        Dim ActualHeight As Integer

        Dim FontName As String = "Arial Narrow"
        Dim MyBarcode As BarcodeNet
        Dim p As Point

        Dim strSymbol As String
        strSymbol = Request.QueryString("Symbology")
        Dim strOrientation As String
        strOrientation = Request.QueryString("Orientation")
        
        Response.ContentType = "image/gif"

        MyBarcode = New BarcodeNet()

        MyBarcode.BackColor = Color.FromName(Request.QueryString("BackColor")) 'Color.White '
        MyBarcode.BarColor = Color.FromName(Request.QueryString("BarColor")) 'Color.Black '
        MyBarcode.CheckDigit = False
        MyBarcode.CheckDigitToText = False
        MyBarcode.Data = Request.QueryString("Data")
        MyBarcode.BarHeight = (System.Convert.ToDouble(Request.QueryString("BarHeight")) / 10) '1.0 '
        MyBarcode.NarrowBarWidth = (System.Convert.ToDouble(Request.QueryString("NarrowBarWidth")) / 10) '0.01  '
        'MyBarcode.Orientation = System.Convert.ToInt16(Request.QueryString("Orientation"))
        'MyBarcode.SymbologyType = System.Convert.ToInt16(Request.QueryString("Symbology"))
        MyBarcode.ShowText = CBool(Request.QueryString("ShowText") = "ON") 'False ' 
        'MyBarcode.Wide2NarrowRatio = 1.0 'System.Convert.ToDouble(Request.QueryString("Wide2NarrowRatio")) 
        MyBarcode.TextFont = New Font(FontName, 8)

        'MyBarcode.Orientation
        If strOrientation.Equals(com.inventec.system.Constants.ANGLE_0) Then
        
            MyBarcode.Orientation = enumOrientation.or0
            
        ElseIf strOrientation.Equals(com.inventec.system.Constants.ANGLE_90) Then
            
            MyBarcode.Orientation = enumOrientation.or90
            
        ElseIf strOrientation.Equals(com.inventec.system.Constants.ANGLE_180) Then

            MyBarcode.Orientation = enumOrientation.or180

        ElseIf strOrientation.Equals(com.inventec.system.Constants.ANGLE_270) Then

            MyBarcode.Orientation = enumOrientation.or270
            
        End If
        
        
        'MyBarcode.SymbologyType
        'MyBarcode.Wide2NarrowRatio        
        If strSymbol.Equals(com.inventec.system.Constants.SYMBOLOGY_TYPE_128_B) Then
        
            MyBarcode.SymbologyType = enumSymbologyType.syCode128_B
            
        ElseIf strSymbol.Equals(com.inventec.system.Constants.SYMBOLOGY_TYPE_128_A) Then
            
            MyBarcode.SymbologyType = enumSymbologyType.syCode128_A
            
        ElseIf strSymbol.Equals(com.inventec.system.Constants.SYMBOLOGY_TYPE_128_C) Then
            
            MyBarcode.SymbologyType = enumSymbologyType.syCode128_C
            
        ElseIf strSymbol.Equals(com.inventec.system.Constants.SYMBOLOGY_TYPE_128_Auto) Then
            
            MyBarcode.SymbologyType = enumSymbologyType.syCode128
            
        ElseIf strSymbol.Equals(com.inventec.system.Constants.SYMBOLOGY_TYPE_39) Then
            
            MyBarcode.SymbologyType = enumSymbologyType.syCode39
            MyBarcode.Wide2NarrowRatio = System.Convert.ToDouble(Request.QueryString("Wide2NarrowRatio"))
            
        ElseIf strSymbol.Equals(com.inventec.system.Constants.SYMBOLOGY_TYPE_93) Then

            MyBarcode.SymbologyType = enumSymbologyType.syCode93

        ElseIf strSymbol.Equals(com.inventec.system.Constants.SYMBOLOGY_TYPE_EANJAN_13) Then

            MyBarcode.SymbologyType = enumSymbologyType.syEAN13

        ElseIf strSymbol.Equals(com.inventec.system.Constants.SYMBOLOGY_TYPE_UPC_A) Then

            MyBarcode.SymbologyType = enumSymbologyType.syUPCA
            
        End If
        
        
        
        ' Get actual barcode width and height
        MyBarcode.GetActualSize(ActualWidth, ActualHeight)

        If (MyBarcode.Orientation = 0 Or _
                  MyBarcode.Orientation = 2) Then
            MyBarcode.SetSize(ActualWidth, ActualHeight)
            objBitmap = New Bitmap(ActualWidth, ActualHeight)
        Else
            MyBarcode.SetSize(ActualHeight, ActualWidth)
            objBitmap = New Bitmap(ActualHeight, ActualWidth)
        End If

        objGraphics = Graphics.FromImage(objBitmap)

        If MyBarcode.ShowText = True Then
            Dim size As SizeF
            size = objGraphics.MeasureString("4", New Font(FontName, 8))
        
            If (MyBarcode.Orientation = 0 Or _
                      MyBarcode.Orientation = 2) Then
                MyBarcode.SetSize(ActualWidth + size.ToSize.Width + 14, ActualHeight + size.ToSize.Height + 4)
                objBitmap = New Bitmap(ActualWidth + size.ToSize.Width + 14, ActualHeight + size.ToSize.Height + 4)
            Else
                MyBarcode.SetSize(ActualHeight + size.ToSize.Height + 4, ActualWidth + size.ToSize.Width + 14)
                objBitmap = New Bitmap(ActualHeight + size.ToSize.Height + 4, ActualWidth + size.ToSize.Width + 14)
            End If
            objGraphics = Graphics.FromImage(objBitmap)
        End If
        
        
        
        
        
		
        p = New Point(0, 0)
        MyBarcode.Render(objGraphics, p)
        objGraphics.Flush()

        objBitmap.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif)
        
    End Sub
        
</script>

 <%     OutputImg()%>