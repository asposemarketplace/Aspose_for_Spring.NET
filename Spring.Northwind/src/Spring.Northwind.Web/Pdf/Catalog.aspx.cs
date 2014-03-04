using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using Aspose.Pdf.Generator;
using System.Data;
using System.Data.OleDb;
using Spring.Northwind.Dao;
using Spring.Northwind.Domain;
using System.Globalization;

namespace Spring.Northwind.Web.Pdf
{
    public partial class Catalog : System.Web.UI.Page
    {
        IList<Product> productsList = null;
        IList<Category> CategoryList = null;

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
            Aspose.Pdf.Generator.Pdf pdf = GetCatalog();
            pdf.Save("catalog.pdf", SaveType.OpenInAcrobat, Response);
            Response.End();
        }

        public Aspose.Pdf.Generator.Pdf GetCatalog()
        {
            productsList = productDao.GetAll();
            CategoryList = categoryDao.GetAll();

            Aspose.Pdf.Generator.Pdf pdf = new Aspose.Pdf.Generator.Pdf();
            pdf.IsTruetypeFontMapCached = false;

            // If you have purchased a license,
            // Set license like this: 
            // string licenseFile = MapPath("License") + "\\Aspose.Pdf.lic"; 
            // Aspose.Pdf.License lic = new Aspose.Pdf.License();
            // lic.SetLicense(licenseFile);

            string xmlFile = Server.MapPath("~/App_Data/xml/Catalog.xml");
            string path = Server.MapPath("~/img");

            pdf.BindXML(xmlFile, null);

            Section section = pdf.Sections["section1"];
            Paragraph logoPara = section.Paragraphs["Logo"];
            Image logoImage = (Image)logoPara;

            logoImage.ImageInfo.File = path + "\\FallCatalog.jpg";
            logoImage.ImageScale = 0.77F;

            Section section2 = pdf.Sections["section2"];
            Paragraph beveragePara = section2.Paragraphs["BeverageTable"];
            Table beverageTable = (Table)beveragePara;
            Row row1 = beverageTable.Rows[0];
            Cell row1Cell2 = row1.Cells[1];
            Image beverageImage = (Image)row1Cell2.Paragraphs[0];
            beverageImage.ImageInfo.File = path + "\\Beverage.jpg";

            Table beveragesTable1 = (Table)section2.Paragraphs["BeverageTable1"];
            FillTable(beveragesTable1, GetProductsByCateGoryID(1));

            Paragraph condimentsPara = section2.Paragraphs["CondimentsTable"];
            Table condimentsTable = (Table)condimentsPara;
            Row row1CondimentTable = condimentsTable.Rows[0];
            Cell row1Cell2CondimentTable = row1CondimentTable.Cells[1];
            Image condimentsImage = (Image)row1Cell2CondimentTable.Paragraphs[0];
            condimentsImage.ImageInfo.File = path + "\\Condiments.jpg";

            Table condimentsTable1 = (Table)section2.Paragraphs["CondimentsTable1"];
            FillTable(condimentsTable1, GetProductsByCateGoryID(2));

            Paragraph confectionsPara = section2.Paragraphs["ConfectionsTable"];
            Table confectionsTable = (Table)confectionsPara;
            Row row1ConfectionsTable = confectionsTable.Rows[0];
            Cell row1Cell2ConfectionsTable = row1ConfectionsTable.Cells[1];
            Image confectionsImage = (Image)row1Cell2ConfectionsTable.Paragraphs[0];
            confectionsImage.ImageInfo.File = path + "\\Confections.jpg";

            Table confectionsTable1 = (Table)section2.Paragraphs["ConfectionsTable1"];
            FillTable(confectionsTable1, GetProductsByCateGoryID(3));

            Paragraph dairyPara = section2.Paragraphs["DairyTable"];
            Table dairyTable = (Table)dairyPara;
            Row row1DairyTable = dairyTable.Rows[0];
            Cell row1Cell2DairyTable = row1DairyTable.Cells[1];
            Image diaryImage = (Image)row1Cell2DairyTable.Paragraphs[0];
            diaryImage.ImageInfo.File = path + "\\Dairy.jpg";

            Table dairyTable1 = (Table)section2.Paragraphs["DairyTable1"];
            FillTable(dairyTable1, GetProductsByCateGoryID(4));

            Paragraph grainsPara = section2.Paragraphs["GrainsTable"];
            Table grainsTable = (Table)grainsPara;
            Row row1GrainsTable = grainsTable.Rows[0];
            Cell row1Cell2GrainsTable = row1GrainsTable.Cells[1];
            Image grainsImage = (Image)row1Cell2GrainsTable.Paragraphs[0];
            grainsImage.ImageInfo.File = path + "\\Grains.jpg";

            Table grainsTable1 = (Table)section2.Paragraphs["GrainsTable1"];
            FillTable(grainsTable1, GetProductsByCateGoryID(5));

            Paragraph meatPara = section2.Paragraphs["MeatTable"];
            Table meatTable = (Table)meatPara;
            Row row1MeatTable = meatTable.Rows[0];
            Cell row1Cell2MeatTable = row1MeatTable.Cells[1];
            Image meatImage = (Image)row1Cell2MeatTable.Paragraphs[0];
            meatImage.ImageInfo.File = path + "\\Meat.jpg";

            Table meatTable1 = (Table)section2.Paragraphs["MeatTable1"];
            FillTable(meatTable1, GetProductsByCateGoryID(6));

            Paragraph producePara = section2.Paragraphs["ProduceTable"];
            Table produceTable = (Table)producePara;
            Row row1ProduceTable = produceTable.Rows[0];
            Cell row1Cell2ProduceTable = row1ProduceTable.Cells[1];
            Image produceImage = (Image)row1Cell2ProduceTable.Paragraphs[0];
            produceImage.ImageInfo.File = path + "\\Produce.jpg";

            Table produceTable1 = (Table)section2.Paragraphs["ProduceTable1"];
            FillTable(produceTable1, GetProductsByCateGoryID(7));

            Paragraph seafoodPara = section2.Paragraphs["SeafoodTable"];
            Table seafoodTable = (Table)seafoodPara;
            Row row1SeafoodTable = seafoodTable.Rows[0];
            Cell row1Cell2SeafoodTable = row1SeafoodTable.Cells[1];
            Image seafoodImage = (Image)row1Cell2SeafoodTable.Paragraphs[0];
            seafoodImage.ImageInfo.File = path + "\\Seafood.jpg";

            Table seafoodTable1 = (Table)section2.Paragraphs["SeafoodTable1"];
            FillTable(seafoodTable1, GetProductsByCateGoryID(8));

            return pdf;
        }

        private void FillTable(Aspose.Pdf.Generator.Table tab, DataTable dt)
        {
            NumberFormatInfo format = new NumberFormatInfo();
            format.CurrencyPositivePattern = 0;
            format.CurrencySymbol = "$";

            tab.DefaultCellTextInfo.FontSize = 10;
            tab.ImportDataTable(dt, false, 1, 0);
            foreach (Row curRow in tab.Rows)
            {
                foreach (Cell curCell in curRow.Cells)
                {
                    curCell.Padding = new MarginInfo();
                    curCell.Padding.Top = 3;
                    curCell.Padding.Bottom = 3;
                }
                if (tab.Rows.IndexOf(curRow) > 0)
                {
                    ((Aspose.Pdf.Generator.Text)curRow.Cells[3].Paragraphs[0]).TextInfo.Alignment = AlignmentType.Right;
                    ((Aspose.Pdf.Generator.Text)curRow.Cells[1].Paragraphs[0]).TextInfo.Alignment = AlignmentType.Center;
                    double price = Convert.ToDouble(((Aspose.Pdf.Generator.Text)curRow.Cells[3].Paragraphs[0]).Segments[0].Content);
                    ((Aspose.Pdf.Generator.Text)curRow.Cells[3].Paragraphs[0]).Segments[0].Content = price.ToString("C", format);
                }
                ((Aspose.Pdf.Generator.Text)curRow.Cells[0].Paragraphs[0]).Segments[0].TextInfo.FontName = "Times-Bold";
            }
        }

        public DataTable GetProductsByCateGoryID(int categoryID)
        {
            IList<Product> filteredProductsList = (from productsTable in productsList
                                                   where (productsTable.Discontinued == false) && (productsTable.CategoryID == categoryID)
                                                   orderby productsTable.ProductName
                                                   select productsTable).ToList<Product>();

            DataTable dataTable1 = ConvertToDataTable(filteredProductsList);
            return dataTable1;
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
            table.Columns.Add("ProductID", typeof(int));
            table.Columns.Add("QuantityPerUnit", typeof(string));
            table.Columns.Add("UnitPrice", typeof(decimal));

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    if (prop.Name.Equals("ProductName") || prop.Name.Equals("ProductID") || prop.Name.Equals("QuantityPerUnit") || prop.Name.Equals("UnitPrice"))
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }

            return table;
        }
    }
}