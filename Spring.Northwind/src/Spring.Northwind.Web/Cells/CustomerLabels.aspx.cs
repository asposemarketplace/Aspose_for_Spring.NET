// Copyright (c) Aspose 2002-2014. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aspose.Cells;
using System.Data;
using Spring.Northwind.Dao;
using Spring.Northwind.Domain;

namespace Spring.Northwind.Web.Cells
{
    public partial class CustomerLabels : System.Web.UI.Page
    {
        private ICustomerDao customerDao;

        public ICustomerDao CustomerDao
        {
            set { this.customerDao = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            //Create a workbook based on the custom method of a class
            Workbook workbook = CreateCustomerLabels();
            workbook.Save(HttpContext.Current.Response, "CustomerLabels.xlsx", ContentDisposition.Attachment, new OoxmlSaveOptions(SaveFormat.Xlsx));

            //end response to avoid unneeded html
            HttpContext.Current.Response.End();
        }

        public Workbook CreateCustomerLabels()
        {
            IList<Customer> customersList = customerDao.GetAll();

            //Open a template file
            string designerFile = MapPath("~/App_Data/xls/Northwind.xls");

            Workbook workbook = new Workbook(designerFile);

            //Get a worksheet
            Worksheet sheet = workbook.Worksheets["Sheet4"];
            //Name the worksheet
            sheet.Name = "Customer Labels";
            //Get the cells collection in the worksheet
            Aspose.Cells.Cells cells = sheet.Cells;
            int row = 0;
            byte column = 0;

            for (int i = 0; i < customersList.Count; i++)
            {
                int remainder = i % 3;
                Cell cell;
                switch (remainder)
                {
                    case 0:
                        column = 0;
                        break;
                    case 1:
                        column = 3;
                        break;
                    case 2:
                        column = 6;
                        break;
                }
                //Get a cell
                cell = cells[row, column];
                //Put a value into it
                cell.PutValue((string)customersList[i].CompanyName);
                //Get another cell
                cell = cells[row + 1, column];
                //Put a value into it
                cell.PutValue((string)customersList[i].Address);
                //Get another cell
                cell = cells[row + 2, column];
                string contact = "";
                
                contact += (string)customersList[i].City + " ";
                contact += (string)customersList[i].Region + " ";
                contact += (string)customersList[i].PostalCode;
                
                //Put the value to it
                cell.PutValue(contact);
                //Get another cell
                cell = cells[row + 3, column];
                //Put a value to it
                cell.PutValue((string)customersList[i].Country);

                if (remainder == 2)
                    row += 5;

            }

            //Remove unnecessary worksheets in the workbook
            for (int i = 0; i < workbook.Worksheets.Count; i++)
            {
                sheet = workbook.Worksheets[i];
                if (sheet.Name != "Customer Labels")
                {
                    workbook.Worksheets.RemoveAt(i);
                    i--;
                }

            }
            //Get the generated workbook
            return workbook;

        }
    }
}