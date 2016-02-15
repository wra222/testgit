Imports IDAutomation.Windows.Forms.PDF417Barcode
Public Class Form1
    Inherits System.Windows.Forms.Form
    Dim NewBarcode As PDF417Barcode = New PDF417Barcode()

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents CreateBarcodeData As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.CreateBarcodeData = New System.Windows.Forms.Button
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'CreateBarcodeData
        '
        Me.CreateBarcodeData.Location = New System.Drawing.Point(8, 120)
        Me.CreateBarcodeData.Name = "CreateBarcodeData"
        Me.CreateBarcodeData.Size = New System.Drawing.Size(232, 28)
        Me.CreateBarcodeData.TabIndex = 0
        Me.CreateBarcodeData.Text = "Create Barcode Data"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(8, 28)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(680, 64)
        Me.TextBox1.TabIndex = 1
        Me.TextBox1.Text = "Enter Your Data To Encode Here..."
        '
        'TextBox2
        '
        Me.TextBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(8, 175)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(680, 222)
        Me.TextBox2.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 157)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(672, 18)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "The barcode data below will create a correct barcode when printed in our font."
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 15)
        Me.ClientSize = New System.Drawing.Size(696, 357)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.CreateBarcodeData)
        Me.Name = "Form1"
        Me.Text = "IDAutomation.com Barcode Encoder Example"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub CreateBarcodeData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreateBarcodeData.Click
        TextBox2.Text = NewBarcode.FontEncoder(TextBox1.Text, 0, 0, 0, False, PDF417Barcode.PDF417Modes.Text, True)
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub
End Class
