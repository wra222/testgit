<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Order_Test.aspx.cs" Inherits="Order_Test" Title="=== TestOrder ==="%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Generate File</title>
</head>
<body>
    <form id="form1" runat="server">
        <table style="width: 676px; height: 414px">
            <tr>
                <td style="width: 377px; height: 40px;">
                    <h4>
                        Inventec GenerateOrderFile&nbsp;</h4>
                 </td>
                 <td style="height: 40px">
                 </td>
            </tr>
            <tr>
                <td style="width: 377px; height: 79px;">
                    <h4>InternalID</h4><asp:TextBox ID="BOX_DN" runat="server"></asp:TextBox><asp:Button ID="Button_Qry" runat="server" OnClick="Button_Qry_Click"  Text="Query" />
                </td>
                <td valign="top" style="height: 79px; width: 422px; text-align: center;">
                    <asp:RadioButton ID="RadioButton_Step1" runat="server" Checked="True" GroupName=" ButtonGroup"
                        Text="Step1" />
                    <asp:RadioButton ID="RadioButton_Step2" runat="server" GroupName=" ButtonGroup"  Text="Step2"  />
                    <asp:RadioButton ID="RadioButton_Step3" runat="server" GroupName=" ButtonGroup" Text="Step3"  />      
                    <asp:Label ID="LabelTestID" runat="server" Enabled="False" Height="20px" Text="ID" Width="34px"></asp:Label>
                    <asp:DropDownList ID="DropDownList_ID" runat="server"  Width="78px">
                    <asp:ListItem>01</asp:ListItem>
                    <asp:ListItem>02</asp:ListItem>
                    <asp:ListItem>03</asp:ListItem>
                    <asp:ListItem>04</asp:ListItem>
                    <asp:ListItem>05</asp:ListItem>
                    <asp:ListItem>06</asp:ListItem>
                    <asp:ListItem>07</asp:ListItem>
                    <asp:ListItem>08</asp:ListItem>
                    <asp:ListItem>09</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>11</asp:ListItem>
                    <asp:ListItem>12</asp:ListItem>
                    <asp:ListItem>13</asp:ListItem>
                    <asp:ListItem>14</asp:ListItem>
                    <asp:ListItem>15</asp:ListItem>
                    <asp:ListItem>16</asp:ListItem>
                    <asp:ListItem>17</asp:ListItem>
                    <asp:ListItem>18</asp:ListItem>
                    <asp:ListItem>19</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>21</asp:ListItem>
                    <asp:ListItem>22</asp:ListItem>
                    <asp:ListItem>23</asp:ListItem>
                    <asp:ListItem>24</asp:ListItem>
                    <asp:ListItem>25</asp:ListItem>
                        <asp:ListItem>26</asp:ListItem>
                        <asp:ListItem>27</asp:ListItem>
                        <asp:ListItem>28</asp:ListItem>
                        <asp:ListItem>29</asp:ListItem>
                        <asp:ListItem>30</asp:ListItem>
                        <asp:ListItem>31</asp:ListItem>
                        <asp:ListItem>32</asp:ListItem>
                        <asp:ListItem>33</asp:ListItem>
                        <asp:ListItem>34</asp:ListItem>
                        <asp:ListItem>35</asp:ListItem>
                        <asp:ListItem>36</asp:ListItem>
                        <asp:ListItem>37</asp:ListItem>
                        <asp:ListItem>38</asp:ListItem>
                        <asp:ListItem>39</asp:ListItem>
                        <asp:ListItem>40</asp:ListItem>
                        <asp:ListItem>41</asp:ListItem>
                        <asp:ListItem>42</asp:ListItem>
                        <asp:ListItem>43</asp:ListItem>
                        <asp:ListItem>44</asp:ListItem>
                        <asp:ListItem>45</asp:ListItem>
                        <asp:ListItem>46</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" style="width: 377px; height: 204px">
                    <asp:GridView ID="SNVIEW" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                        BackColor="LightGreen" DataSourceID="SqlDataSource_SN" ShowFooter="True" OnSelectedIndexChanged="SNVIEW_SelectedIndexChanged" Font-Size="Medium">
                        <Columns>
                            <asp:CommandField SelectText="Generate" ShowSelectButton="True" />
                            <asp:BoundField DataField="PALLET_ID" HeaderText="PALLET_ID" SortExpression="PALLET_ID" />
                            <asp:BoundField DataField="SERIAL_NUM" HeaderText="SERIAL_NUM" SortExpression="SERIAL_NUM" />
                        </Columns>
                    <SelectedRowStyle BorderColor="#FFE0C0" />
                </asp:GridView>
                </td>
                <td valign="top" style="width: 422px; height: 204px; text-align: center;">
                    <asp:GridView ID="DOCVIEW" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource_DOC" AllowSorting="True" Font-Size="Smaller">
                        <Columns>
                            <asp:BoundField DataField="DOC_SET_NUMBER" HeaderText="DOC_SET_NUMBER" SortExpression="DOC_SET_NUMBER" />
                            <asp:BoundField DataField="DOC_CAT" HeaderText="DOC_CAT" SortExpression="DOC_CAT" />
                            <asp:BoundField DataField="XSL_TEMPLATE_NAME" HeaderText="XSL_TEMPLATE_NAME" SortExpression="XSL_TEMPLATE_NAME" />
                        </Columns>
                </asp:GridView>
                </td>
            </tr>
            <tr>
                <td style="width: 377px">
                    <asp:Label ID="Label_Messege" runat="server" Text="ZipFile" Width="244px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 377px">
                    <p5>文件路径：\\10.96.183.199\HPEDITS\UploadFiles</p5>
                </td>
            </tr>
            </table>
            <table>
            <tr>
                <td style="width: 2000px; height: 53px; vertical-align: sub;">
                    <h6 style="text-align: center">Copyright 2008-07-16 Inventec Corporation</h6>
                    <h6 style="vertical-align: middle; text-align: center">英业达集团</h6>
                 </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="SqlDataSource_SN" runat="server" ConnectionString="<%$ ConnectionStrings:HP_EDITSConnStr %>"
            SelectCommand="SELECT [PALLET_ID], [SERIAL_NUM] FROM [PAK_PackkingData] WHERE ([InternalID] = @InternalID) ORDER BY [PALLET_ID]">
            <SelectParameters>
                <asp:ControlParameter ControlID="BOX_DN" DefaultValue="" Name="InternalID" PropertyName="Text"
                    Type="String" />
            </SelectParameters>
         </asp:SqlDataSource>
         <asp:SqlDataSource ID="SqlDataSource_DOC" runat="server" ConnectionString="<%$ ConnectionStrings:HP_EDITSConnStr %>"
            SelectCommand="op_DOC_Query" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="BOX_DN" DefaultValue="" Name="InternalID" PropertyName="Text"
                    Type="String" />
            </SelectParameters>
          </asp:SqlDataSource>
    </form>
</body>
</html>
