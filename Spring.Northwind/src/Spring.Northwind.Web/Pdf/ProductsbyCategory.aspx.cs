// Copyright (c) Aspose 2002-2014. All Rights Reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aspose.Pdf.Generator;
using System.Data;
using System.Data.OleDb;
using Spring.Northwind.Dao;
using Spring.Northwind.Domain;

namespace Spring.Northwind.Web.Pdf
{
    public partial class ProductsbyCategory : System.Web.UI.Page
    {
        private IProductDao productDao;
        private ICategoryDao categoryDao;

        public IProductDao ProductDao
        {
            set { this.productDao = value; }
        }

        public ICategoryDao CategoryDao
        {
            set { this.categoryDao = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            Aspose.Pdf.Generator.Pdf pdf = GetProductsByCategory();
            pdf.Save("productsbycategory.pdf", SaveType.OpenInAcrobat, Response);
            Response.End();
        }

        public Aspose.Pdf.Generator.Pdf GetProductsByCategory()
        {
            IList<Product> productsList = productDao.GetAll();
            IList<Category> CategoryList = categoryDao.GetAll();

            Aspose.Pdf.Generator.Pdf pdf = new Aspose.Pdf.Generator.Pdf();
            pdf.IsTruetypeFontMapCached = false;

            // If you have purchased a license,
            // Set license like this: 
            // string licenseFile = MapPath("License") + "\\Aspose.Pdf.lic"; 
            // Aspose.Pdf.License lic = new Aspose.Pdf.License();
            // lic.SetLicense(licenseFile);

            string xmlFile = Server.MapPath("~/App_Data/xml/ProductsByCategory.xml");
            pdf.BindXML(xmlFile, null);

            Section section = pdf.Sections["section1"];

            Aspose.Pdf.Generator.Table table1 = new Aspose.Pdf.Generator.Table(section);
            section.Paragraphs.Add(table1);
            table1.ColumnWidths = "314 314 314";
            table1.Border = new BorderInfo((int)BorderSide.Top, 4, new Aspose.Pdf.Generator.Color(204));
            table1.IsRowBroken = false;
            table1.DefaultCellPadding.Top = table1.DefaultCellPadding.Bottom = 15;
            table1.DefaultCellTextInfo.FontSize = 14;
            table1.Margin.Top = 10;

            int j;
            Row curRow = null;
            Cell curCell = null;
            Aspose.Pdf.Generator.Table curSubTab = null;
            string[] names = new string[]
                {"Catagory: Beverages","Catagory: Condiments","Catagory: Confections",
                 "Catagory: Dairy Products","Catagory: Grains/Cereals","Catagory: Meat/Poultry",
                 "Catagory: Produce","Catagory: Seafood"};

            for (j = 1; j <= 8; j++)
            {
                if (j == 1 || j == 4 || j == 7)
                    curRow = table1.Rows.Add();
                curCell = curRow.Cells.Add();
                curSubTab = new Aspose.Pdf.Generator.Table(curCell);
                curSubTab.DefaultCellPadding.Top = curSubTab.DefaultCellPadding.Bottom = 3;
                curCell.Paragraphs.Add(curSubTab);
                curSubTab.ColumnWidths = "214 90";

                Row row0 = curSubTab.Rows.Add();
                Aspose.Pdf.Generator.TextInfo tf1 = new Aspose.Pdf.Generator.TextInfo();
                tf1.FontSize = 16;
                tf1.FontName = "Times-Bold";

                row0.Cells.Add(names[j - 1], tf1);

                IList<Product> filteredProductsList = (from productsTable in productsList
                                                       where (productsTable.Discontinued == false) && (productsTable.CategoryID == j)
                                                       orderby productsTable.ProductName
                                                       select productsTable).ToList<Product>();

                DataTable dataTable1 = ConvertToDataTable(filteredProductsList);

                curSubTab.ImportDataTable(dataTable1, true, 1, 0);

                curSubTab.Rows[1].Border = new BorderInfo((int)(BorderSide.Top | BorderSide.Bottom), 4, new Aspose.Pdf.Generator.Color(204));

                Row lastRow = curSubTab.Rows[curSubTab.Rows.Count - 1];
                foreach (Cell cCell in lastRow.Cells)
                    cCell.Padding.Bottom = 20;

                lastRow = curSubTab.Rows.Add();
                lastRow.Cells.Add("number of products:");
                lastRow.Cells.Add(dataTable1.Rows.Count.ToString());
                lastRow.Border = new BorderInfo((int)BorderSide.Top, new Aspose.Pdf.Generator.Color(204));
            }

            curRow.Cells.Add();
            return pdf;
        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            //foreach (PropertyDescriptor prop in properties)
            //    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            //foreach (T item in data)
            //{
            //    DataRow row = table.NewRow();
            //    foreach (PropertyDescriptor prop in properties)
            //        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
            //    table.Rows.Add(row);
            //}

            // Manual table conversion
            table.Columns.Add("ProductName", typeof(string));
            table.Columns.Add("UnitsInStock", typeof(short));

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    if (prop.Name.Equals("ProductName") || prop.Name.Equals("UnitsInStock"))
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }

            return table;

        }
    }
}