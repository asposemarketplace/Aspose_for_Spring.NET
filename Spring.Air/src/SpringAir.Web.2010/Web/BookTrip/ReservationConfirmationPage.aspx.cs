#region Licence

/*
 * Copyright © 2002-2005 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

#region Imports

using System;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;

using Spring.Web.UI;
using Spring.Web.UI.Controls;
using SpringAir.Domain;

using Aspose.Words;
using Aspose.Words.Drawing;
using Aspose.Words.Tables;

using Aspose.BarCode;
#endregion

/// <summary>
/// The SpringAir reservation confirmation page.
/// </summary>
/// <author>Aleksandar Seovic</author>
/// <version>$Id: ReservationConfirmationPage.aspx.cs,v 1.2 2006/12/07 04:22:00 aseovic Exp $</version>
public partial class ReservationConfirmationPage : Page
{
    #region Fields

    protected ReservationConfirmation confirmation;

    #endregion

    #region Model Management and Data Binding Methods

    protected override void InitializeModel()
    {
        confirmation = (ReservationConfirmation)Session[Constants.ReservationConfirmationKey];
    }

    protected override void LoadModel(object savedModel)
    {
        confirmation = (ReservationConfirmation)savedModel;
    }

    protected override object SaveModel()
    {
        return confirmation;
    }

    #endregion

    #region Page Lifecycle Methods

    protected override void OnInitializeControls(EventArgs e)
    {
        base.OnInitializeControls(e);

        // Check for license and apply if exists
        string licenseFile = Server.MapPath("~/App_Data/Aspose.Words.lic");
        if (File.Exists(licenseFile))
        {
            Aspose.Words.License license = new Aspose.Words.License();
            license.SetLicense(licenseFile);
        }

        if (!IsPostBack)
        {
            if (confirmation == null)
            {
                Response.Redirect("~/Web/BookTrip/TripForm.aspx");
                return;
            }

            itinerary.DataSource = confirmation.Reservation.Itinerary.Flights;
            itinerary.DataBind();

            Session.Remove(Constants.ReservationConfirmationKey);
            Session.Remove(Constants.SuggestedFlightsKey);
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        confirmationLabel.Text = GetMessage("confirmation", confirmation.ConfirmationNumber);
    }

    #endregion

    private string CurrentPageURL
    {
        get
        {
            string url = Request.Url.Authority + HttpContext.Current.Request.RawUrl.ToString();

            if (Request.ServerVariables["HTTPS"] == "on")
            {
                url = "https://" + url;
            }
            else
            {
                url = "http://" + url;
            }

            return url;
        }
    }

    private string BaseURL
    {
        get
        {
            string url = Request.Url.Authority;

            if (Request.ServerVariables["HTTPS"] == "on")
            {
                url = "https://" + url;
            }
            else
            {
                url = "http://" + url;
            }

            return url;
        }
    }

    protected void SavePdfButton_Click(object sender, EventArgs e)
    {
        GenerateOutPutDocument(confirmation.ConfirmationNumber + ".pdf");
    }

    protected void SaveWordButton_Click(object sender, EventArgs e)
    {
        GenerateOutPutDocument(confirmation.ConfirmationNumber + ".docx");
    }

    protected void GenerateOutPutDocument(string outFileName)
    {
        //Open the template document
        Document doc = new Document(Server.MapPath("~/App_Data/DocumentBuilderDemo.docx"));

        //Once the builder is created, its cursor is positioned at the beginning of the document.
        DocumentBuilder builder = new DocumentBuilder(doc);

        builder.MoveToHeaderFooter(HeaderFooterType.HeaderPrimary);
        BarCodeBuilder barCode = CreateBarCode();
        builder.InsertImage(barCode.BarCodeImage);

        builder.MoveToDocumentStart();

        System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath("~/Web/Images/spring-air-logo-header.jpg"));
        builder.InsertImage(image);

        builder.InsertParagraph();
        builder.ParagraphFormat.ClearFormatting();
        builder.Font.ClearFormatting(); 

        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
        builder.ParagraphFormat.Shading.ForegroundPatternColor = System.Drawing.Color.White;
        builder.ParagraphFormat.Shading.Texture = TextureIndex.TextureSolid;
        builder.ParagraphFormat.LeftIndent = ConvertUtil.InchToPoint(0.3);
        builder.ParagraphFormat.SpaceBefore = 12;
        builder.ParagraphFormat.SpaceAfter = 12;
              
        builder.Font.Name = "Arial";
        builder.Font.Size = 9;
        builder.Write("ELECTRONIC TICKET - PASSENGER ITINERARY/RECEIPT");
        builder.InsertBreak(BreakType.LineBreak);
        builder.Writeln("CUSTOMER COPY - Powered by ASPOSE");
        builder.ParagraphFormat.ClearFormatting();

        builder.InsertHtml("<hr>");

        BuildBookingTable(builder);

        builder.MoveToDocumentEnd();
        builder.InsertBreak(BreakType.LineBreak);

        builder.InsertHtml("<hr>");

        builder.InsertParagraph();
        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
        builder.InsertImage(barCode.BarCodeImage);


        doc.Save(Response, outFileName, ContentDisposition.Inline, null);
        Response.End();
    }

    public void BuildBookingTable(DocumentBuilder builder)
    {
        //Reset to default font and paragraph formatting.
        builder.Font.ClearFormatting();
        builder.ParagraphFormat.ClearFormatting();

        builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
        builder.InsertParagraph();
        builder.Writeln(confirmationLabel.Text);

        builder.Font.Color = System.Drawing.Color.Black;
        builder.Font.Size = 10;
        builder.Font.Name = "Tahoma";

        builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;

        BorderCollection borders = builder.CellFormat.Borders;
        borders.LineStyle = LineStyle.Single;
        borders.Color = System.Drawing.Color.Black;

        builder.StartTable();
        builder.Font.Bold = true;
        builder.InsertCell();
        
        builder.Write(GetMessage("flightNumber"));
        builder.CellFormat.Shading.BackgroundPatternColor = System.Drawing.Color.LightGray;
        
        builder.InsertCell();
        builder.Write(GetMessage("departureDate"));
        builder.CellFormat.Shading.BackgroundPatternColor = System.Drawing.Color.LightGray;
        
        builder.InsertCell();
        builder.Write(GetMessage("departureAirport"));
        builder.CellFormat.Shading.BackgroundPatternColor = System.Drawing.Color.LightGray;
        
        builder.InsertCell();
        builder.Write(GetMessage("destinationAirport"));
        builder.CellFormat.Shading.BackgroundPatternColor = System.Drawing.Color.LightGray;
        
        builder.InsertCell();
        builder.Write(GetMessage("aircraft"));
        builder.CellFormat.Shading.BackgroundPatternColor = System.Drawing.Color.LightGray;
        builder.EndRow();

        builder.CellFormat.ClearFormatting();
        builder.Font.ClearFormatting();
        
        foreach (Flight flight in confirmation.Reservation.Itinerary.Flights)
        {
            builder.InsertCell();
            builder.Write(flight.FlightNumber);

            builder.InsertCell();
            builder.Write(flight.DepartureTime.ToString());

            builder.InsertCell();
            builder.Write(flight.DepartureAirport.Description);

            builder.InsertCell();
            builder.Write(flight.DestinationAirport.Description);

            builder.InsertCell();
            builder.Write(flight.Aircraft.Model);
            builder.EndRow();
        }

        builder.EndTable();
    }

    private BarCodeBuilder CreateBarCode()
    {
        // Instantiate a LinearBarCode object
        BarCodeBuilder barCode = new BarCodeBuilder();
        barCode.SymbologyType = Symbology.Code128;
        barCode.CodeText = confirmation.ConfirmationNumber;
        barCode.CodeLocation = CodeLocation.Below;
        barCode.ImageQuality = ImageQualityMode.Default;

        return barCode;
    }

}