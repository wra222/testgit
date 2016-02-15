<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" MasterPageFile="~/MasterPage.master" Title="=== HP EDITS::Reprint Process ===" MaintainScrollPositionOnPostback="true"%>

<%@ Register Src="IDPrompt.ascx" TagName="IDPrompt" TagPrefix="uc1" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <h2>Reprint Process</h2>
    <table width="100%">
       <tr>
          <td style="width: 50%">
        <h4>Step 1:</h4>
                    <asp:Label ID="Label1" runat="server" Text="InternalID："></asp:Label>
                    <asp:TextBox ID="txtInternalID" runat="server"></asp:TextBox>
                    <asp:Button ID="btnQuery" runat="server" OnClick="btnQuery_Click" Text="Query Delivery message" /></td>
                <td style="width: 50%">
                       <h4>Step 3:</h4>
                <asp:Label ID="lblDocNum" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top" style="width: 50%">
    <asp:GridView ID="gvHPComn" runat="server" AutoGenerateColumns="False" CellPadding="4"
        DataSourceID="SqlDSHPComn" Font-Names="Arial Unicode MS" Font-Size="9pt" ForeColor="#333333"
        GridLines="Vertical">
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <Columns>
            <asp:TemplateField HeaderText="REGION" SortExpression="REGION">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("REGION") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblREGION" runat="server" Text='<%# Bind("REGION") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ORDER_TYPE" SortExpression="ORDER_TYPE">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("ORDER_TYPE") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblORDER_TYPE" runat="server" Text='<%# Bind("ORDER_TYPE") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="SALES_CHAN" SortExpression="SALES_CHAN">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("SALES_CHAN") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblSALES_CHAN" runat="server" Text='<%# Bind("SALES_CHAN") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <EditRowStyle BackColor="#999999" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDSHPComn" runat="server" ConnectionString="<%$ ConnectionStrings:HP_EDITSConnStr %>"
        SelectCommand="SELECT * FROM [dbo].[v_ComnInternalID] WHERE ([InternalID] = @InternalID)" ProviderName="<%$ ConnectionStrings:HP_EDITSConnStr.ProviderName %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtInternalID" Name="InternalID" PropertyName="Text"
                Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
                    &nbsp;
                </td>
                <td valign="top" style="width: 50%">
                <asp:Repeater ID="referenceTable" Visible="false" runat="server" OnItemCommand="referenceTable_ItemCommand">
                    <HeaderTemplate>
                        <table class="conditionTable">
                            <tr>
                                <td colspan="2"> <h3>Reference Table</h3>
                                </td>
                            </tr>                        
                    </HeaderTemplate>
                    <ItemTemplate>
                            <tr>
                                <td class="content1">
                                    <asp:LinkButton ID="LinkButton2" CommandName="Print" CommandArgument=' <%#DataBinder.Eval(Container.DataItem, "ID") %>' runat="server">Print</asp:LinkButton>
                                </td>
                                <td class="content1">
                                    <%#DataBinder.Eval(Container.DataItem, "DOC_CAT")%>&nbsp;
                                </td>                                
                                <td class="content1">
                                    <%#DataBinder.Eval(Container.DataItem, "XSL_TEMPLATE_NAME")%>&nbsp;
                                </td>                                
                            </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                            <tr>
                                <td class="content2">
                                    <asp:LinkButton ID="LinkButton2" CommandName="Print" CommandArgument=' <%#DataBinder.Eval(Container.DataItem, "ID") %>' runat="server">Print</asp:LinkButton>
                                </td>
                                <td class="content2">
                                    <%#DataBinder.Eval(Container.DataItem, "DOC_CAT")%>&nbsp;
                                </td>                                
                                <td class="content2">
                                    <%#DataBinder.Eval(Container.DataItem, "XSL_TEMPLATE_NAME")%>&nbsp;
                                </td>  
                                                          
                            </tr>
                    </AlternatingItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                </td>
            </tr>
        </table>
      <h4>Step 2:</h4>
    <table id="TABLE1">
        <tr>
            <td valign="top" style="font-size: 10pt">
             <asp:Repeater ID="conditionTable" runat="server" OnItemCommand="conditionTable_ItemCommand">
        <HeaderTemplate>
            <table class="conditionTable">
                <tr>
                    <td colspan="100"><h3>Condition Table</h3></td>
                </tr>           
                <tr>
                    <th>
                    </th>
                    <th>
                        DOC_SET_NUMBER
                    </th>
                    <th>
                        SOURCE_LANG_FORMAT_CODE
                    </th>
                    <th>
                        REGION
                    </th>
                    <th>
                        ORDER_TYPE
                    </th>
                    <th>
                        SALES_CHAN
                    </th>
                </tr> 
        </HeaderTemplate>
        <ItemTemplate>
            <tr>                
                <td class="content1">                    
                    <asp:LinkButton ID="LinkButton1" CommandName="Show" CommandArgument=' <%# DataBinder.Eval(Container.DataItem,"DOC_SET_NUMBER") %>' runat="server">Show</asp:LinkButton>                    
                </td>
                <td class="content1">
                    <%#DataBinder.Eval(Container.DataItem, "DOC_SET_NUMBER")%>&nbsp;
                </td>
                <td class="content1">
                    <%#DataBinder.Eval(Container.DataItem, "SOURCE_LANG_FORMAT_CODE")%>&nbsp;
                </td>
                <td class="content1">
                    <%#DataBinder.Eval(Container.DataItem, "REGION")%>&nbsp;
                </td>            
                <td class="content1">
                    <%#DataBinder.Eval(Container.DataItem, "ORDER_TYPE")%>&nbsp;
                </td>            
                <td class="content1">
                    <%#DataBinder.Eval(Container.DataItem, "SALES_CHAN")%>&nbsp;
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr>                
                <td class="content2">
                    <asp:LinkButton ID="LinkButton1" CommandName="Show" CommandArgument=' <%#DataBinder.Eval(Container.DataItem,"DOC_SET_NUMBER")%>' runat="server">Show</asp:LinkButton>                                        
                </td>
                <td class="content2">
                    <%#DataBinder.Eval(Container.DataItem, "DOC_SET_NUMBER")%>&nbsp;
                </td>
                <td class="content2">
                    <%#DataBinder.Eval(Container.DataItem, "SOURCE_LANG_FORMAT_CODE")%>&nbsp;
                </td>
                <td class="content2">
                    <%#DataBinder.Eval(Container.DataItem, "REGION")%>&nbsp;
                </td>            
                <td class="content2">
                    <%#DataBinder.Eval(Container.DataItem, "ORDER_TYPE")%>&nbsp;
                </td>            
                <td class="content2">
                    <%#DataBinder.Eval(Container.DataItem, "SALES_CHAN")%>&nbsp;
                </td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
                <asp:Label ID="lblQstring" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="lblselect" runat="server" Visible="False"></asp:Label></td>
            <td valign="top">
             <h4>Step 4:</h4>
                <asp:GridView ID="gvBox" runat="server" AllowPaging="True" CellPadding="2" ForeColor="#333333" GridLines="Vertical"  OnPageIndexChanging="gvBox_PageIndexChanging" OnSelectedIndexChanged="gvBox_SelectedIndexChanged" Font-Size="Small" HorizontalAlign="Left" >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <Columns>
                            <asp:CommandField SelectText="Print" ShowSelectButton="True" />
                        </Columns>
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <EditRowStyle BackColor="#999999" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDSDetail" runat="server" ConnectionString="<%$ ConnectionStrings:HP_EDITSConnStr %>"
                    SelectCommand="select SERIAL_NUM from dbo.PAK_PackkingData where internalID =@internalID ">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtInternalID" Name="internalID" PropertyName="Text" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:Button ID="btnViewXML" runat="server" OnClick="btnViewXML_Click" Text="View XML" Visible="False" />
                <asp:Button ID="btnViewPdf" runat="server" Text="View PDF" OnClick="btnViewPdf_Click" Visible="False" />
                <asp:Button ID="btnPrintPDF" runat="server" Text="Print PDF" ValidationGroup="InternalID" OnClick="btnPrintPDF_Click" Visible="False"/></td>
        </tr>
    </table>
</asp:content>