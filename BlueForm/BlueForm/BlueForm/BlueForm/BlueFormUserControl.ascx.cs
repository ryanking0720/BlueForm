using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.DirectoryServices;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Resources;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace BlueForm.BlueForm
{
    /* New Hire Employee System Access Form (Blue Form)
     * 
     * Author: Ryan King
     * 
     * Date: August 14, 2019
     * 
     * Purpose: This form is used to grant access to a desktop or laptop computer for a new hire.
     * It must contain the new hire's first, middle, and last names, their personal mobile phone number, 
     * and their Sharp phone number. A checkbox labeled SAP will automatically open up a Security Authorization
     * Form with an identical timestamp (Note: this WILL NOT WORK if the hyperlink at the top is clicked). This allows the two forms to be queried together with the timestamp as a
     * primary key. If such a form is initialized this way, the Security Authorization Form should be submitted BEFORE this one.
     * 
     * A unique ID is generated, which can be used with a corresponding Security Authorization Form.
     * YYYYMMDDHHSS is the format.
     * 
     * YYYY represents the current year.
     * 
     * MM represents the current month with one leading zero.
     * 
     * DD represents the current day with one leading zero.
     * 
     * HH represents the current hour in 24-hour time with one leading zero.
     * 
     * MM represents the current minute with one leading zero.
     * 
     * SS represents the current second with one leading zero.
     * 
     * This unique ID is kept in the Title field on its SharePoint list.
     * A timestamp is also generated to be used with the Security Authorization Form.
     * 
     * This form generates a PDF and sends it to the Sharp Support Center upon submission. All the fields of the form are visible
     * on the PDF, along with a timestamp at the bottom showing when it was generated.
     * It also submits the information to a SharePoint list.
     * 
     * Clicking the link to the other form will not give an identical timestamp.
     * 
     * This form can be done independently of the Security Authorization Form.
     */
    public partial class BlueFormUserControl : UserControl
    {
        // The filename
        string fileName;
        // The Sharp logo to be placed on the PDF
        const string image = "http://njspdevd01:2544/nhesaf/PublishingImages/sharplogo_r_halfx-RGB-RW-20mm.jpg";
        // The MemoryStream involved in parsing the PDF
        MemoryStream ms;
        // The PDF document
        Document document;
        // The document as a byte array
        Byte[] result;
        // Fonts to use in the PDF
        const string seguisym = "http://njspdevd01:2544/nhesaf/Documents/seguisym.ttf";
        BaseFont helvetica = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.EMBEDDED);
        BaseFont segoeUiSymbolsBase = BaseFont.CreateFont(seguisym, BaseFont.CP1252, BaseFont.EMBEDDED);
        iTextSharp.text.Font segoeUiSymbols;
        iTextSharp.text.Font helveticaEntry;
        iTextSharp.text.Font helveticaHeader;
        
        // Timestamp automatically set to now
        static DateTime timestamp = DateTime.Now;
        static string id = timestamp.Year.ToString("D4") + timestamp.Month.ToString("D2") + timestamp.Day.ToString("D2") + timestamp.Hour.ToString("D2") + timestamp.Minute.ToString("D2") + timestamp.Second.ToString("D2");

        // Property for the timestamp
        public static DateTime TimeStamp {
            get {
                return timestamp;
            }

            set {
                timestamp = value;
            }
        }

        // Property for the unique ID
        public static string UniqueID{
            get{
                return id;
            }

            set{
                id = value;
            }
        }

        // Obtains the current user's display name, e.g. "King, Ryan" as opposed to "KingRy", which is the username
        protected string GetUserFullName(string domain, string userName)
        {
            DirectoryEntry userEntry = new DirectoryEntry("WinNT://" + domain + "/" + userName + ",User");
            return (string)userEntry.Properties["fullname"].Value;
        }

        // Called when the page loads initially
        protected void Page_Load(object sender, EventArgs e)
        {
            // Fonts to be used on the PDF
            segoeUiSymbols = new iTextSharp.text.Font(segoeUiSymbolsBase, 12, iTextSharp.text.Font.NORMAL);
            helveticaEntry = new iTextSharp.text.Font(helvetica, 12, iTextSharp.text.Font.NORMAL);
            helveticaHeader = new iTextSharp.text.Font(helvetica, 10, iTextSharp.text.Font.NORMAL);

            // The user's full name
            string name = GetUserFullName(Environment.UserDomainName, Environment.UserName);

            name = name.Replace(",","");

            string[] parts = name.Split();

            // Place the user's first and last names into their corresponding textboxes
            firstName.Text = parts[1];

            lastName.Text = parts[0];

            // Take note of the unique ID
            uniqueID.Text = DateTime.Now.Year.ToString("D4") + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2")
                + DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2");
        }

        // Sets the timestamp
        public static void SetTimeStamp(DateTime dt) 
        {
            timestamp = dt;
        }

        // Returns the timestamp
        public static DateTime GetTimeStamp() 
        {
            return timestamp;
        }

        // Serves the same purpose as a System.Windows.Forms.MessageBox object
        // as a JavaScript alert box.
        protected void MsgBox(string sMessage)
        {
            // Make a new script with an alert box and the argument as a message
            string msg = "<script type=\"text/javascript\">";
            msg += "alert(\"" + sMessage + "\");";
            msg += "</script>";

            // Post the message to the top of the document
            Response.Write(msg);
        }

        // The code of String.IsNullOrWhiteSpace(),
        // which is not available in this .NET framework.
        public bool IsNullOrWhiteSpace(String value)
        {
            if (value == null)
            {
                return true;
            }

            for (int i = 0; i < value.Length; i++)
            {
                if (!Char.IsWhiteSpace(value[i]))
                {
                    return false;
                }
            }

            return true;
        }

        // Checks to see if a string is null, empty, or whitespace
        protected bool IsNullEmptyOrWhitespace(string str)
        {
            return String.IsNullOrEmpty(str) || IsNullOrWhiteSpace(str);
        }

        // Checks for errors on the form
        protected string CheckForErrors()
        {
            int errors = 0;
            string headerString = "We have found the following error", errorString = "";

            // Check to see if the textboxes are filled
            if (!AreTextboxesFilled())
            {
                errorString += "You did not fill in all of the required textboxes.\r\n";
                errors++;
            }

            // Check to see if the user selected a device
            if (!IsDeviceSelected())
            {
                errorString += "You did not select a device.\r\n";
                errors++;
            }

            // Format the error string and display it if necessary
            if (errors > 0)
            {
                if (errors > 1)
                {
                    headerString += "s";
                }
                headerString += " in your form:\r\n";

                headerString += errorString;

                return headerString;
            }
            else
            {
                return null;
            }
        }

        // Used to determine if a device was selected
        protected bool IsDeviceSelected()
        {
            return !IsNullEmptyOrWhitespace(device.SelectedValue) && device.SelectedValue != "None Selected";
        }

        // Used to determine if all textboxes have values
        protected bool AreTextboxesFilled()
        {
            return IsTextboxFilled(firstName) && IsTextboxFilled(middleName) && IsTextboxFilled(lastName) && IsTextboxFilled(sharpPhone) && IsTextboxFilled(mobilePhone);
        }

        // Used to determine if an individual textbox has been filled with a nontrivial value
        protected bool IsTextboxFilled(TextBox tb)
        {
            return !IsNullEmptyOrWhitespace(tb.Text);
        }


        /*// Deprecated methods used in the grid layout (DO NOT USE!)
        // Used to determine if both checkbox groups are checked
        protected bool AreCheckboxesChecked()
        {
            return AreTopCheckboxesChecked() && AreBottomCheckboxesChecked();
        }

        // Used to determine if the top checkbox group is checked
        protected bool AreTopCheckboxesChecked()
        {
            return prod.Checked || qa.Checked || dev.Checked;
        }

        // Used to determine if the bottom checkbox group is checked
        protected bool AreBottomCheckboxesChecked()
        {
            return ecc.Checked || crm.Checked || bw.Checked;
        }
        */


        // Handles the Submit button being clicked
        protected void submit_Click(object sender, EventArgs e)
        {
            // See if there are any errors before continuing
            string errorString = CheckForErrors();
            results.Font.Size = 16;
            // Display the errors, if any
            if (errorString != null)
            {
                results.Text = errorString;
                results.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                // Write the data into a PDF with iTextSharp
                ConvertToPDF();

                // Place the data into a database entry in SharePoint
                AddToSharePoint();

                results.Text = "Success!";
                results.ForeColor = System.Drawing.Color.ForestGreen;
                results.Visible = true;
            }
        }

        // Used to add the PDF data to SharePoint
        protected void AddToSharePoint() 
        {
            try
            {
                // Attempt to open the SharePoint site
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        // Attempt to open the database
                        SPList list = web.Lists.TryGetList("BlueFormRecords");
                        if (list != null)
                        {
                            // Add a new item to the successfully opened list
                            SPListItem NewItem = list.Items.Add();
                            {
                                web.AllowUnsafeUpdates = true;

                                NewItem["Title"] = uniqueID.Text;

                                // Put the primary key into the database entry
                                NewItem["CreatedBy"] = web.CurrentUser.LoginName;

                                // Add all of the fields from the form
                                NewItem["FirstName"] = firstName.Text;

                                NewItem["MiddleName"] = middleName.Text;

                                NewItem["LastName"] = lastName.Text;

                                NewItem["Device"] = device.SelectedValue;

                                NewItem["SharpPhone"] = sharpPhone.Text;

                                NewItem["MobilePhone"] = mobilePhone.Text;


                                /*// Deprecated fields used in the grid layout (DO NOT USE!)
                                NewItem["Prod"] = prod.Checked.ToString();

                                NewItem["QA"] = qa.Checked.ToString();

                                NewItem["Dev"] = dev.Checked.ToString();

                                NewItem["ECC"] = ecc.Checked.ToString();

                                NewItem["CRM"] = crm.Checked.ToString();

                                NewItem["BW"] = bw.Checked.ToString();
                                */


                                NewItem["Timestamp"] = timestamp;

                                // Update the item on the server
                                NewItem.Update();

                                // Reset all of the initially empty fields for the submission of another form
                                firstName.Text = "";
                                middleName.Text = "";
                                lastName.Text = "";
                                device.SelectedValue = "Please select a device";
                                sharpPhone.Text = "";
                                mobilePhone.Text = "";
                                prod.Checked = false;
                                qa.Checked = false;
                                dev.Checked = false;
                                ecc.Checked = false;
                                crm.Checked = false;
                                bw.Checked = false;
                            }
                        }
                        else
                        {
                            // Alert the user if the list was not found
                            MsgBox("List not found.");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Alert the user of any exception that may have occurred
                MsgBox(ex.Message);
                return;
            }
        }

        // Used in the old grid layout (DO NOT USE!)
        /*
        private string GetMark(CheckBox first, CheckBox second) 
        {
            if (first.Checked && second.Checked)
            {
                return "Yes";
            }
            else
            {
                return "No";
            }
        }
        */

        // Used to convert the form data into a PDF
        private void ConvertToPDF()
        {
            result = null;

            try
            {
                //Get Title name, using default Guid for Title, to create a filename.
                fileName = DateTime.Now.Year.ToString("D4") + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2") + DateTime.Now.Hour.ToString("D2")
                    + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2") + Environment.UserName + "BlueForm.pdf";

                //Creating the Document in memory first.
                ms = new MemoryStream();
                
                    //Using the ItextSharp Document
                    document = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
                    
                        //Using the ItextSharp PdfWriter to create an instance of the document using the memory stream.
                        PdfWriter writer = PdfWriter.GetInstance(document, ms);
                        writer.SetFullCompression();
 
                        //Open the document; otherwise, we won't be able to add to it.
                        document.Open();
 
                        //Create a PDF table with 3 columns
                        PdfPTable table = new PdfPTable(3);
                        table.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                        // Create a second PDF table with 3 columns
                        PdfPTable table2 = new PdfPTable(3);
                        table2.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                        // Create a third PDF table with 4 columns
                        PdfPTable table3 = new PdfPTable(4);
                        table3.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                        Paragraph title = new Paragraph("Employee System Access Information\n\n");
                        title.Font = helveticaEntry;
                        title.Alignment = Element.ALIGN_CENTER;

                        //Create the first Cell, make it span across 2 columns.
                        Paragraph subtitle = new Paragraph("Employee Details\n\n");
                        subtitle.Font = helveticaEntry;
                        subtitle.Alignment = Element.ALIGN_CENTER; //0=Left, 1=Centre, 2=Right

                        // Load the Sharp logo up on the top of the page
                        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(image);
                        logo.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;
                        document.Add(logo);

                        //Add the cell to the table.
                        document.Add(title);
                        document.Add(subtitle);
                        
                        // Column with first, middle, and last name headers
                        PdfPCell firstTop = new PdfPCell(new Phrase("First Name"));
                        firstTop.Colspan = 1;
                        firstTop.HorizontalAlignment = 1;
                        firstTop.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        firstTop.Phrase.Font.Size = 10;
                        table.AddCell(firstTop);

                        PdfPCell middleTop = new PdfPCell(new Phrase("Middle Name"));
                        middleTop.Colspan = 1;
                        middleTop.HorizontalAlignment = 1;
                        middleTop.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        middleTop.Phrase.Font.Size = 10;
                        table.AddCell(middleTop);

                        PdfPCell lastTop = new PdfPCell(new Phrase("Last Name"));
                        lastTop.Colspan = 1;
                        lastTop.HorizontalAlignment = 1;
                        lastTop.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        lastTop.Phrase.Font.Size = 10;
                        table.AddCell(lastTop);


                        // Column with first, middle, and last name values
                        PdfPCell firstBottom = new PdfPCell(new Phrase(firstName.Text));
                        firstBottom.Colspan = 1;
                        firstBottom.HorizontalAlignment = 1;
                        firstBottom.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        firstBottom.Phrase.Font = helveticaEntry;
                        table.AddCell(firstBottom);
                        
                        PdfPCell middleBottom = new PdfPCell(new Phrase(middleName.Text));
                        middleBottom.Colspan = 1;
                        middleBottom.HorizontalAlignment = 1;
                        middleBottom.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        middleBottom.Phrase.Font = helveticaEntry;
                        table.AddCell(middleBottom);

                        PdfPCell lastBottom = new PdfPCell(new Phrase(lastName.Text));
                        lastBottom.Colspan = 1;
                        lastBottom.HorizontalAlignment = 1;
                        lastBottom.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        lastBottom.Phrase.Font = helveticaEntry;
                        table.AddCell(lastBottom);

                        // Headers for the device section
                        PdfPCell deviceTop = new PdfPCell(new Phrase("Device"));
                        deviceTop.Colspan = 1;
                        deviceTop.HorizontalAlignment = 1;
                        deviceTop.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        deviceTop.Phrase.Font.Size = 10;
                        table2.AddCell(deviceTop);

                        PdfPCell sharpTop = new PdfPCell(new Phrase("Sharp Phone"));
                        sharpTop.Colspan = 1;
                        sharpTop.HorizontalAlignment = 1;
                        sharpTop.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        sharpTop.Phrase.Font.Size = 10;
                        table2.AddCell(sharpTop);

                        PdfPCell mobileTop = new PdfPCell(new Phrase("Mobile Phone"));
                        mobileTop.Colspan = 1;
                        mobileTop.HorizontalAlignment = 1;
                        mobileTop.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        mobileTop.Phrase.Font.Size = 10;
                        table2.AddCell(mobileTop);




                        // Values for the device section
                        PdfPCell deviceBottom = new PdfPCell(new Phrase(device.Text));
                        deviceBottom.Colspan = 1;
                        deviceBottom.HorizontalAlignment = 1;
                        deviceBottom.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        deviceBottom.Phrase.Font = helveticaEntry;
                        table2.AddCell(deviceBottom);

                        PdfPCell sharpBottom = new PdfPCell(new Phrase(sharpPhone.Text));
                        sharpBottom.Colspan = 1;
                        sharpBottom.HorizontalAlignment = 1;
                        sharpBottom.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        sharpBottom.Phrase.Font = helveticaEntry;
                        table2.AddCell(sharpBottom);

                        PdfPCell mobileBottom = new PdfPCell(new Phrase(mobilePhone.Text));
                        mobileBottom.Colspan = 1;
                        mobileBottom.HorizontalAlignment = 1;
                        mobileBottom.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        mobileBottom.Phrase.Font = helveticaEntry;
                        table2.AddCell(mobileBottom);



                        // Add the table to the document
                        document.Add(table);

                        // Filler to pad out the empty space
                        Paragraph filler = new Paragraph("\n\n\n");
                        filler.Font = helveticaEntry;
                        document.Add(filler);

                        document.Add(table2);

                        document.Add(filler);

                        PdfPTable sapTable = new PdfPTable(1);
                        sapTable.HorizontalAlignment = 1;

                        // Information for the SAP field
                        PdfPCell sapTop = new PdfPCell(new Phrase("SAP"));
                        sapTop.Colspan = 1;
                        sapTop.HorizontalAlignment = 1;
                        sapTop.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        sapTop.Phrase.Font.Size = 10;
                        sapTable.AddCell(sapTop);

                        PdfPCell sapBottom = new PdfPCell(new Phrase((!IsNullEmptyOrWhitespace(checkedLabel.Text) ? "Yes" : "No")));
                        sapBottom.Colspan = 1;
                        sapBottom.HorizontalAlignment = 1;
                        sapBottom.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        sapBottom.Phrase.Font = helveticaEntry;
                        sapTable.AddCell(sapBottom);

                        document.Add(sapTable);
                        /*// Information for the old grid layout (deprecated)
                        Paragraph applicationList = new Paragraph("\n\n\nSAP Access\n\n");
                        applicationList.Alignment = Element.ALIGN_CENTER;
                        applicationList.Font = helveticaEntry;
                        document.Add(applicationList);

                        // Row 1

                        PdfPCell blank = new PdfPCell(new Phrase());
                        blank.Colspan = 1;
                        blank.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        blank.Phrase.Font = helveticaEntry;
                        table3.AddCell(blank);

                        PdfPCell prodHeader = new PdfPCell(new Phrase("PROD"));
                        prodHeader.Colspan = 1;
                        prodHeader.HorizontalAlignment = 1;
                        prodHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        prodHeader.Phrase.Font.Size = 10;

                        table3.AddCell(prodHeader);

                        PdfPCell qaHeader = new PdfPCell(new Phrase("QA"));
                        qaHeader.Colspan = 1;
                        qaHeader.HorizontalAlignment = 1;
                        qaHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        qaHeader.Phrase.Font.Size = 10;
                        table3.AddCell(qaHeader);

                        PdfPCell devHeader = new PdfPCell(new Phrase("DEV"));
                        devHeader.Colspan = 1;
                        devHeader.HorizontalAlignment = 1;
                        devHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        devHeader.Phrase.Font.Size = 10;
                        table3.AddCell(devHeader);

                        // Row 2

                        PdfPCell eccHeader = new PdfPCell(new Phrase("ECC"));
                        eccHeader.Colspan = 1;
                        eccHeader.HorizontalAlignment = 1;
                        eccHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        eccHeader.Phrase.Font.Size = 10;
                        table3.AddCell(eccHeader);

                        PdfPCell prodEcc = new PdfPCell();
                        prodEcc.Phrase = new Phrase();
                        prodEcc.Phrase.Font = segoeUiSymbols;
                        prodEcc.Phrase = new Phrase(GetMark(prod, ecc));
                        prodEcc.Colspan = 1;
                        prodEcc.HorizontalAlignment = 1;
                        prodEcc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table3.AddCell(prodEcc);

                        PdfPCell qaEcc = new PdfPCell();
                        qaEcc.Phrase = new Phrase();
                        qaEcc.Phrase.Font = segoeUiSymbols;
                        qaEcc.Phrase = new Phrase(GetMark(qa, ecc));
                        qaEcc.Colspan = 1;
                        qaEcc.HorizontalAlignment = 1;
                        qaEcc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table3.AddCell(qaEcc);

                        PdfPCell devEcc = new PdfPCell();
                        devEcc.Phrase = new Phrase(GetMark(dev, ecc));
                        devEcc.Phrase.Font = segoeUiSymbols;
                        devEcc.Colspan = 1;
                        devEcc.HorizontalAlignment = 1;
                        devEcc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table3.AddCell(devEcc); 

                        // Row 3

                        PdfPCell crmHeader = new PdfPCell(new Phrase("CRM"));
                        crmHeader.Colspan = 1;
                        crmHeader.HorizontalAlignment = 1;
                        crmHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        crmHeader.Phrase.Font.Size = 10;
                        table3.AddCell(crmHeader);

                        PdfPCell prodCrm = new PdfPCell();
                        prodCrm.Phrase = new Phrase(GetMark(prod, crm));
                        prodCrm.Phrase.Font = segoeUiSymbols;
                        prodCrm.Colspan = 1;
                        prodCrm.HorizontalAlignment = 1;
                        prodCrm.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table3.AddCell(prodCrm);

                        PdfPCell qaCrm = new PdfPCell();
                        qaCrm.Phrase = new Phrase(GetMark(qa, crm));
                        qaCrm.Phrase.Font = segoeUiSymbols;
                        qaCrm.Colspan = 1;
                        qaCrm.HorizontalAlignment = 1;
                        qaCrm.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table3.AddCell(qaCrm);

                        PdfPCell devCrm = new PdfPCell();
                        devCrm.Phrase = new Phrase(GetMark(dev, crm));
                        devCrm.Phrase.Font = segoeUiSymbols;
                        devCrm.Colspan = 1;
                        devCrm.HorizontalAlignment = 1;
                        devCrm.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table3.AddCell(devCrm);

                        // Row 4

                        PdfPCell bwHeader = new PdfPCell(new Phrase("BW"));
                        bwHeader.Colspan = 1;
                        bwHeader.HorizontalAlignment = 1;
                        bwHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        bwHeader.Phrase.Font.Size = 10;
                        table3.AddCell(bwHeader);

                        PdfPCell prodBw = new PdfPCell();
                        prodBw.Phrase = new Phrase(GetMark(prod, bw));
                        prodBw.Phrase.Font = segoeUiSymbols;
                        prodBw.Colspan = 1;
                        prodBw.HorizontalAlignment = 1;
                        prodBw.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table3.AddCell(prodBw);

                        PdfPCell qaBw = new PdfPCell();
                        qaBw.Phrase = new Phrase(GetMark(qa, bw));
                        qaBw.Phrase.Font = segoeUiSymbols;
                        qaBw.Colspan = 1;
                        qaBw.HorizontalAlignment = 1;
                        qaBw.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table3.AddCell(qaBw);

                        PdfPCell devBw = new PdfPCell();
                        devBw.Phrase = new Phrase(GetMark(dev, bw));
                        devBw.Phrase.Font = segoeUiSymbols;
                        devBw.Colspan = 1;
                        devBw.HorizontalAlignment = 1;
                        devBw.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table3.AddCell(devBw);

                        document.Add(table3);
                        */
                        // The timestamp
                        string generated = "\n\n\n\n\n\n\n\n\n\n\nGenerated on " + DateTime.Now.ToLongDateString() + " at " + DateTime.Now.ToString("%h") + ":" + DateTime.Now.Minute.ToString("D2") + ":" + DateTime.Now.Second.ToString("D2") + " ";

                        if (DateTime.Now.Hour < 12)
                        {
                            generated += "A.M.";
                        }
                        else 
                        {
                            generated += "P.M.";
                        }

                        // More filler
                        document.Add(filler);
                        document.Add(filler);
                        document.Add(filler);

                        Paragraph footer = new Paragraph(generated);
                        footer.Alignment = Element.ALIGN_CENTER;

                        document.Add(footer);

                        document.Close();
                    
                    //Convert the final memory stream into a byte array.
                    result = ms.ToArray();

                    // Configure email and send the PDF as part of a blank message
                    ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;

                    // Instantiate the Attachment object
                    Attachment att = new Attachment(new MemoryStream(result), fileName);

                    
                    // Replace these with Sharp Support Center email address and password
                    var fromAddress = new MailAddress(SPContext.Current.Web.CurrentUser.Email, GetUserFullName(Environment.UserDomainName, Environment.UserName));
                    var toAddress = new MailAddress("rajasundaraa@sharpsec.com", "Rajasundaram, Arulraj");

                    string subject = "Blue Form " + fileName;
                    const string body = "";

                    // Initialize the SMTP client
                    SmtpClient smtp = new SmtpClient();
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("KingRy@sharpsec.com", "Sharp.2");
                    smtp.Host = "smtp.office365.com";
                    smtp.Port = 587;  // this is critical
                    smtp.EnableSsl = true;  // this is critical

                    /*// Old Gmail SMTP client used for testing (deprecated)
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    */

                    // Initialize the Mail Message object
                    MailMessage message = new MailMessage();

                    // Fil out the subject, body, sender, and recipient
                    message.Subject = subject;
                    message.Body = body;
                    message.From = fromAddress;
                    message.To.Add(toAddress);
                    // Addresses for SEC Suport Center
                    /*
                    message.To.Add("SECSupportCenter@sharpamericas.com");
                    message.To.Add("SECSupportCenter@sharpsec.com");
                    message.To.Add("SECSupportCenter@sharpusa.com");
                    message.To.Add("SharpSupportCenter@sharpamericas.com");
                    message.To.Add("sharpsupportcenter@sharpsec.com");
                    message.To.Add("sharpsupportcenter@sharpusa.com");
                    message.To.Add("sharpsupportcenter@sharpusa.mail.onmicrosoft.com");
                    message.To.Add("sharpsupportcenter@sharpusa.onmicrosoft.com");
                    */
                    // Attach the PDF
                    message.Attachments.Add(att);
                    //smtp.Send(message);//Uncomment this later

                    // Show a successful result
                    results.Text = "Success!";
                    results.ForeColor = System.Drawing.Color.ForestGreen;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error trying to convert item to PDF. Message: " + Ex.Message);
            }
        }

        // This handles if the SAP checkbox was checked.
        // It will help the PDF give the correct value for the SAP field,
        // even if the user forgets a field and the checkbox
        // becomes re-enabled on postback, because it sets a hidden label
        // with a value.
        protected void sap_CheckedChanged(object sender, EventArgs e)
        {
            // Determines if the SAP checkbox was checked, even on refresh
            if (sap.Checked)
            {
                checkedLabel.Text = "Yes";
            }
            else 
            {
                checkedLabel.Text = "No";
            }
        }
    }
}