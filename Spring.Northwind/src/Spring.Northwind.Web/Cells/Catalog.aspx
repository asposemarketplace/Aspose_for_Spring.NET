﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/MasterPage.master" AutoEventWireup="true" CodeBehind="Catalog.aspx.cs" Inherits="Spring.Northwind.Web.Cells.Catalog" %>

<%-- Copyright (c) Aspose 2002-2014. All Rights Reserved.--%>


<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="server">
    <h2>Create Product Catalog report using Aspose.Cells for .NET</h2>
    <div class="componentDescriptionTxt">
        This demo illustrates how to create a well formatted Catalog report by using <a href="http://www.aspose.com/.net/excel-component.aspx">Aspose.Cells for .NET.</a>
        <ul class="noPaddingUl">
            <li>Aspose.Cells component gives you the agility to report your data in a variety of ways.</li>
            <li>Aspose.Cells component is fully functional for creating all types of reports.</li>
            <li>You may customize the size and appearance of everything on a report. </li>
            <li>You can display the information the way you want to see it.</li>
        </ul>
        The demo generates a Catalog of products in Northwind database.
        <ul class="noPaddingUl">
            <li>It has a two-page report header and uses photos for each category. </li>
            <li>It starts each category on a new page and keeps all records for a category on same page. </li>
            <li>Spring.NET NHibernate is used to retrieve the data from the Products table of Northwind database, to generate the report. </li>
            <li>You can either open the resultant excel file into MS Excel or save directly to your disk to check the results.</li>
        </ul>
        Click Process to see how example Prints a catalog of products.
    </div>
    <br />
    <asp:Button ID="btnProcess" runat="server" Text="Process" OnClick="btnProcess_Click" />
</asp:Content>
