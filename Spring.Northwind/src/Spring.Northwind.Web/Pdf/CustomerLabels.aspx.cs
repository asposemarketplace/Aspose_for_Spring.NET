// Copyright (c) Aspose 2002-2014. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aspose.Pdf.Generator;
using System.Data;
using Spring.Northwind.Dao;
using Spring.Northwind.Domain;

namespace Spring.Northwind.Web.Pdf
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
            Aspose.Pdf.Generator.Pdf pdf = GetCustomerLabels();
            pdf.Save("customerlabels.pdf", SaveType.OpenInAcrobat, Response);
            Response.End();
        }

        public Aspose.Pdf.Generator.Pdf GetCustomerLabels()
        {
            Aspose.Pdf.Generator.Pdf pdf = new Aspose.Pdf.Generator.Pdf();
            pdf.IsTruetypeFontMapCached = false;

            // If you have purchased a license,
            // Set license like this: 
            // string licenseFile = MapPath("License") + "\\Aspose.Pdf.lic"; 
            // Aspose.Pdf.License lic = new Aspose.Pdf.License();
            // lic.SetLicense(licenseFile);

            string xmlFile = Server.MapPath("~/App_Data/xml/CustomerLabels.xml");
            pdf.BindXML(xmlFile, null);

            Section section = pdf.Sections["section1"];
            Aspose.Pdf.Generator.Table table1 = (Aspose.Pdf.Generator.Table)section.Paragraphs["table1"];
                       
            IList<Customer> customersList = customerDao.GetAll();

            string[] strArr = new string[customersList.Count];

            for (int i = 0; i < customersList.Count; i++)
            {
                strArr[i] = customersList[0].CompanyName.ToString() + "#$NL" + customersList[1].Address.ToString() + "#$NL" +
                    customersList[2].City.ToString() + " " + (customersList[3].Region == null ? string.Empty : customersList[3].Region.ToString()) + " " +
                    customersList[4].PostalCode.ToString() + "#$NL" + customersList[5].Country.ToString();
            }

            table1.DefaultCellTextInfo.FontSize = 10;
            table1.ImportArray(strArr, 0, 0, false);

            foreach (Row cRow in table1.Rows)
            {
                foreach (Cell curCell in cRow.Cells)
                {
                    curCell.Padding = new MarginInfo();
                    curCell.Padding.Top = 10;
                }
            }

            return pdf;
        }
    }
}