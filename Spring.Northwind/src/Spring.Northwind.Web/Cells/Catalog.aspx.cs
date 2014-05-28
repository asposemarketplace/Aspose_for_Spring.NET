// Copyright (c) Aspose 2002-2014. All Rights Reserved.

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using Aspose.Cells;
using System.Data;
using System.Data.OleDb;
using Spring.Northwind.Dao;
using Spring.Northwind.Domain;
using System.Globalization;
using System.Web;
using System.Drawing;

namespace Spring.Northwind.Web.Cells
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
            //Create a workbook based on the custom method of a class
            Workbook workbook = CreateCatalog();            
            workbook.Save(HttpContext.Current.Response, "Catalog.xlsx", ContentDisposition.Attachment, new OoxmlSaveOptions(SaveFormat.Xlsx));
            
            //end response to avoid unneeded html
            HttpContext.Current.Response.End();      
        }

        public Workbook CreateCatalog()
        {
            //Open a template file
            string designerFile = MapPath("~/App_Data/xls/Northwind.xls");
            Workbook workbook = new Workbook(designerFile);

            productsList = productDao.GetAll();
            CategoryList = categoryDao.GetAll();
                        
            //Create a new datatable
            DataTable dataTable2 = new DataTable();
            //Get a worksheet
            Worksheet sheet = workbook.Worksheets["Sheet2"];
            //Name the sheet
            sheet.Name = "Catalog";
            //Get the worksheet cells
            Aspose.Cells.Cells cells = sheet.Cells;

            int currentRow = 55;

            //Add LightGray color to color palette
            workbook.ChangePalette(Color.LightGray, 55);
            //Get the workbook's styles collection
            StyleCollection styles = workbook.Styles;
            //Set CategoryName style with formatting attributes
            int styleIndex = styles.Add();
            Style styleCategoryName = styles[styleIndex];
            styleCategoryName.Font.Size = 14;
            styleCategoryName.Font.Color = Color.Blue;
            styleCategoryName.Font.IsBold = true;
            styleCategoryName.Font.Name = "Times New Roman";

            //Set Description style with formatting attributes
            styleIndex = styles.Add();
            Style styleDescription = styles[styleIndex];
            styleDescription.Font.Name = "Times New Roman";
            styleDescription.Font.Color = Color.Blue;
            styleDescription.Font.IsItalic = true;

            //Set ProductName style with formatting attributes
            styleIndex = styles.Add();
            Style styleProductName = styles[styleIndex];
            styleProductName.Font.IsBold = true;

            //Set Title style with formatting attributes
            styleIndex = styles.Add();
            Style styleTitle = styles[styleIndex];
            styleTitle.Font.IsBold = true;
            styleTitle.Font.IsItalic = true;
            styleTitle.ForegroundColor = Color.LightGray;

            styleIndex = styles.Add();
            Style styleNumber = styles[styleIndex];
            styleNumber.Font.Name = "Times New Roman";
            styleNumber.Number = 8;

            //Create the styleflag struct
            StyleFlag styleflag = new StyleFlag();
            styleflag.All = true;
            //Get the horizontal page breaks collection
            HorizontalPageBreakCollection hPageBreaks = sheet.HorizontalPageBreaks;

            DataTable dataTable1 = ConvertCategoriesToDataTable(CategoryList);

            for (int i = 0; i < dataTable1.Rows.Count; i++)
            {
                currentRow += 2;
                cells.SetRowHeight(currentRow, 20);
                cells[currentRow, 1].SetStyle(styleCategoryName);
                DataRow categoriesRow = dataTable1.Rows[i];

                //Write CategoryName
                cells[currentRow, 1].PutValue((string)categoriesRow["CategoryName"]);

                //Write Description
                currentRow++;
                cells[currentRow, 1].PutValue((string)categoriesRow["Description"]);
                cells[currentRow, 1].SetStyle(styleDescription);

                dataTable2.Clear();
                dataTable2 = GetProductsByCateGoryID(Convert.ToInt32(categoriesRow["CategoryID"].ToString()));
                
                currentRow += 2;
                //Import the datatable to the sheet
                cells.ImportDataTable(dataTable2, true, currentRow, 1);
                //Create a range
                Range range = cells.CreateRange(currentRow, 1, 1, 4);
                //Apply style to the range
                range.ApplyStyle(styleTitle, styleflag);
                //Create a range
                range = cells.CreateRange(currentRow + 1, 1, dataTable2.Rows.Count, 1);
                //Apply style to the range
                range.ApplyStyle(styleProductName, styleflag);
                //Create a range
                range = cells.CreateRange(currentRow + 1, 4, dataTable2.Rows.Count, 1);
                //Apply style to the range
                range.ApplyStyle(styleNumber, styleflag);

                currentRow += dataTable2.Rows.Count;
                //Apply horizontal page breaks
                hPageBreaks.Add(currentRow, 0);
            }

            //Remove the unnecessary worksheets in the workbook
            for (int i = 0; i < workbook.Worksheets.Count; i++)
            {
                sheet = workbook.Worksheets[i];
                if (sheet.Name != "Catalog")
                {
                    workbook.Worksheets.RemoveAt(i);
                    i--;
                }

            }
            //Return the generated workbook
            return workbook;
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

        public DataTable ConvertCategoriesToDataTable<T>(IList<T> data)
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

            return table;
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