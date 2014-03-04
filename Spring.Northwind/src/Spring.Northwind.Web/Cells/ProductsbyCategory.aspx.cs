using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aspose.Cells;
using System.Data;
using System.Data.OleDb;
using Spring.Northwind.Dao;
using Spring.Northwind.Domain;

namespace Spring.Northwind.Web.Cells
{
    public class ProductCategory
    {
        string categoryName;

        public string CategoryName
        {
          get { return categoryName; }
          set { categoryName = value; }
        }
        string productName;

        public string ProductName
        {
          get { return productName; }
          set { productName = value; }
        }
        short unitsInStock;

        public short UnitsInStock
        {
          get { return unitsInStock; }
          set { unitsInStock = value; }
        }
    }

    public partial class ProductsbyCategory : System.Web.UI.Page
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
            //Create a workbook based on the custom method of a class
            Workbook workbook = CreateProductsByCategory();
            workbook.Save(HttpContext.Current.Response, "ProductsByCategory.xlsx", ContentDisposition.Attachment, new OoxmlSaveOptions(SaveFormat.Xlsx));

            //end response to avoid unneeded html
            HttpContext.Current.Response.End();
        }

        public Workbook CreateProductsByCategory()
        {
            productsList = productDao.GetAll();
            CategoryList = categoryDao.GetAll();

            List<ProductCategory> productCatList =  (from product in productsList
                                                    join category in CategoryList on product.CategoryID equals category.CategoryID
                                                    orderby category.CategoryName, product.ProductName
                                                    select new ProductCategory
                                                    { 
                                                        CategoryName = category.CategoryName, 
                                                        ProductName = product.ProductName, 
                                                        UnitsInStock = product.UnitsInStock 
                                                    }).ToList<ProductCategory>();


            //Open a template file
            string designerFile = MapPath("~/App_Data/xls/Northwind.xls");
            Workbook workbook = new Workbook(designerFile);

            DataTable dataTable1 = ConvertToDataTable(productCatList);

            //Get a worksheet
            Worksheet sheet = workbook.Worksheets["Sheet7"];
            //Name it
            sheet.Name = "Products By Category";
            //Get the cells
            Aspose.Cells.Cells cells = sheet.Cells;
            //Get the sheet vertical page breaks
            VerticalPageBreakCollection vPageBreaks = sheet.VerticalPageBreaks;
            //Set row heights
            cells.SetRowHeight(4, 20.25);
            cells.SetRowHeight(5, 18.75);
            ushort currentRow = 4;
            byte currentColumn = 0;

            string lastCategory = "";
            string thisCategory, nextCategory;

            int productsCount = 0;

            SetProductsByCategoryStyles(workbook);
            
            //Fill cells by inputing the values and apply styles to the data
            for (int i = 0; i < dataTable1.Rows.Count; i++)
            {
                thisCategory = (string)dataTable1.Rows[i]["CategoryName"];
                if (thisCategory != lastCategory)
                {
                    currentRow = 4;
                    if (i != 0)
                        currentColumn += 4;
                    CreateProductsByCategoryHeader(workbook, cells, currentRow, currentColumn, thisCategory);
                    lastCategory = thisCategory;
                    currentRow += 2;
                }
                cells[currentRow, currentColumn].PutValue((string)dataTable1.Rows[i]["ProductName"]);
                cells[currentRow, (byte)(currentColumn + 1)].PutValue((short)dataTable1.Rows[i]["UnitsInStock"]);

                if (i != dataTable1.Rows.Count - 1)
                {
                    nextCategory = (string)dataTable1.Rows[i + 1]["CategoryName"];
                    if (thisCategory != nextCategory)
                    {
                        Aspose.Cells.Style style = workbook.Styles["ProductsCount"];
                        cells[currentRow + 1, currentColumn].PutValue("Number of Products:");
                        cells[currentRow + 1, currentColumn].SetStyle(style);

                        style = workbook.Styles["CountNumber"];
                        cells[currentRow + 1, (byte)(currentColumn + 1)].PutValue(productsCount + 1);
                        cells[currentRow + 1, (byte)(currentColumn + 1)].SetStyle(style);
                        currentRow++;
                        productsCount = 0;
                        vPageBreaks.Add(0, currentColumn + 1);
                    }
                    else
                        productsCount++;
                }
                else
                {
                    Aspose.Cells.Style style = workbook.Styles["ProductsCount"];
                    cells[currentRow + 1, currentColumn].PutValue("Number of Products:");
                    cells[currentRow + 1, currentColumn].SetStyle(style);

                    style = workbook.Styles["CountNumber"];
                    cells[currentRow + 1, (byte)(currentColumn + 1)].PutValue(productsCount + 1);
                    cells[currentRow + 1, (byte)(currentColumn + 1)].SetStyle(style);
                }
                currentRow++;
            }

            //Remove the unnecessary worksheets in the workbook
            for (int i = 0; i < workbook.Worksheets.Count; i++)
            {
                sheet = workbook.Worksheets[i];
                if (sheet.Name != "Products By Category")
                {
                    workbook.Worksheets.RemoveAt(i);
                    i--;
                }
            }
            //Get the generated workbook
            return workbook;
        }

        private void SetProductsByCategoryStyles(Workbook workbook)
        {
            //Create a style with some specific formatting attributes
            int styleIndex = workbook.Styles.Add();
            Aspose.Cells.Style style = workbook.Styles[styleIndex];
            style.Font.IsItalic = true;
            style.Font.IsBold = true;
            style.Font.Size = 16;
            style.HorizontalAlignment = TextAlignmentType.Right;
            style.Name = "Category";

            //Create a style with some specific formatting attributes
            styleIndex = workbook.Styles.Add();
            style = workbook.Styles[styleIndex];
            style.Font.Size = 16;
            style.Font.IsBold = true;
            style.HorizontalAlignment = TextAlignmentType.Left;
            style.Name = "CategoryName";

            //Create a style with some specific formatting attributes
            styleIndex = workbook.Styles.Add();
            style = workbook.Styles[styleIndex];
            style.Font.Size = 14;
            style.Font.IsBold = true;
            style.Font.IsItalic = true;
            style.HorizontalAlignment = TextAlignmentType.Left;
            style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Medium;
            style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Medium;
            style.Name = "ProductName";

            //Create a style with some specific formatting attributes
            styleIndex = workbook.Styles.Add();
            style = workbook.Styles[styleIndex];
            style.Font.Size = 14;
            style.Font.IsBold = true;
            style.Font.IsItalic = true;
            style.HorizontalAlignment = TextAlignmentType.Right;
            style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Medium;
            style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Medium;
            style.Name = "UnitsInStock";

            //Create a style with some specific formatting attributes
            styleIndex = workbook.Styles.Add();
            style = workbook.Styles[styleIndex];
            style.Font.IsBold = true;
            style.Font.IsItalic = true;
            style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style.Name = "ProductsCount";

            //Create a style with some specific formatting attributes
            styleIndex = workbook.Styles.Add();
            style = workbook.Styles[styleIndex];
            style.HorizontalAlignment = TextAlignmentType.Left;
            style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style.Name = "CountNumber";

        }

        private void CreateProductsByCategoryHeader(Workbook workbook, Aspose.Cells.Cells cells, ushort startRow, byte startColumn, string categoryName)
        {
            //Input values and apply the styles to the cells

            Aspose.Cells.Style style = workbook.Styles["Category"];
            cells[startRow, startColumn].PutValue("Category:");
            cells[startRow, startColumn].SetStyle(style);

            style = workbook.Styles["CategoryName"];
            cells[startRow, (byte)(startColumn + 1)].PutValue(categoryName);
            cells[startRow, (byte)(startColumn + 1)].SetStyle(style);

            style = workbook.Styles["ProductName"];
            cells[startRow + 1, startColumn].PutValue("Product Name");
            cells[startRow + 1, startColumn].SetStyle(style);

            style = workbook.Styles["UnitsInStock"];
            cells[startRow + 1, (byte)(startColumn + 1)].PutValue("Units In Stock:");
            cells[startRow + 1, (byte)(startColumn + 1)].SetStyle(style);
        }

        private string GetCategoryNameByID(int catID)
        {
            Category cat = CategoryList.SingleOrDefault(x => x.CategoryID == catID);
            if (cat != null)
                return cat.CategoryName;

            return string.Empty;
        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }

            // Manual table conversion
            //table.Columns.Add("ProductName", typeof(string));
            //table.Columns.Add("ProductID", typeof(int));
            //table.Columns.Add("QuantityPerUnit", typeof(string));
            //table.Columns.Add("UnitPrice", typeof(decimal));

            //foreach (T item in data)
            //{
            //    DataRow row = table.NewRow();
            //    foreach (PropertyDescriptor prop in properties)
            //    {
            //        if (prop.Name.Equals("ProductName") || prop.Name.Equals("ProductID") || prop.Name.Equals("QuantityPerUnit") || prop.Name.Equals("UnitPrice"))
            //            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
            //    }
            //    table.Rows.Add(row);
            //}

            return table;
        }
    }
}