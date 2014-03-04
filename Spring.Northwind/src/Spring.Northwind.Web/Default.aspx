<%@ Page Language="C#" MasterPageFile="~/Shared/MasterPage.master" AutoEventWireup="true" Inherits="_Default" CodeBehind="Default.aspx.cs" %>

<asp:Content ID="content" ContentPlaceHolderID="content" runat="server">

    <h1>Welcome to Spring.NET Northwind sample application!</h1>

    <p>
        This sample demostrates Spring.NET NHibernate integration and concepts. All data access is done using NHibernate and
        all transactions are automatically handled by Spring.NET.
    </p>

    <asp:LinkButton ID="customerList" runat="server" OnClick="customerList_Click">Proceed to customer listing &raquo;</asp:LinkButton>
    <br />
    <br />

    <h2>Aspose.Cells Examples</h2>
    <ul class="noPaddingUl">
        <li><a href="Cells/Catalog.aspx">Products Catalog &raquo;</a></li>
        <li><a href="Cells/CustomerLabels.aspx">Customer Labels &raquo;</a></li>
        <li><a href="Cells/ProductsbyCategory.aspx">Products by Category &raquo;</a></li>
    </ul>

    <h2>Aspose.Pdf Examples</h2>
    <ul class="noPaddingUl">
        <li><a href="Pdf/Catalog.aspx">Products Catalog &raquo;</a></li>
        <li><a href="Pdf/CustomerLabels.aspx">Customer Labels &raquo;</a></li>
        <li><a href="Pdf/ProductsbyCategory.aspx">Products by Category &raquo;</a></li>
    </ul>

</asp:Content>
