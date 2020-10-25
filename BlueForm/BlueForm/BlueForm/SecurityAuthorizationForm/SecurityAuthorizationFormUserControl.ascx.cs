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
/* Security Authorization Form
 * 
 * Author: Ryan King
 * 
 * Date: August 14, 2019
 * 
 * Purpose: This form is used to grant access to sensitive company information for new hires.
 * 
 * 
 * A unique ID is grabbed from the corresponding NHESAF, or generated new if this form is not connected to anything else.
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
 * 
 * Different fields are included throughout the form to account for different types of employees.
 * Therefore, many fields may end up remaining blank.
 * The form automatically generates a PDF which is sent to SAP Security.
 * It contains all the fields, as well as a timestamp when it was generated.
 * All data submitted on these forms are also stored in a SharePoint list.
 * 
 * This form can be initialized by clicking the SAP checkbox on the New Hire Employee System Access Form.
 * If initialized this way, the two forms will share an identical timestamp and unique ID, which will allow them to be
 * queried with the timestamp/unique ID as the primary key (Note: this WILL NOT WORK if the hyperlink at the top is clicked).
 * Also, this form should be submitted FIRST when the SAP checkbox on the corresponding form is clicked on.
 * 
 * Submitting this form will automatically close it if it was opened as part of a New Hire Employee System Access Form with the SAP checkbox.
 * 
 * Clicking the link on the top to the other form will not give an identical timestamp or unique ID.
 * 
 * This form can be done independently of the New Hire Employee System Access Form.
 */
namespace BlueForm.SecurityAuthorizationForm
{
    public partial class SecurityAuthorizationFormUserControl : UserControl
    {
        // The timestamp from either the corresponding NHESAF or a new one if this was made independently of the former
        DateTime timestamp = SecurityAuthorizationFormUserControl.GetTimeStamp();
        string id = SecurityAuthorizationFormUserControl.GetUniqueID();

        // Useful website information
        Guid guidSite = SPContext.Current.Site.ID;
        Guid guidWeb = SPContext.Current.Web.ID;

        // A list of names of people who work at the company
        System.Collections.Generic.List<string> names;

        // The filename
        string fileName;
        // The Sharp logo to be placed on the PDF
        const string image = "http://njspdevd01:2544/nhesaf/PublishingImages/sharplogo_r_halfx-RGB-RW-20mm.jpg";

        // Obtains the current user's display name, e.g. "King, Ryan" as opposed to "KingRy", which is the username
        protected string GetUserFullName(string domain, string userName)
        {
            DirectoryEntry userEntry = new DirectoryEntry("WinNT://" + domain + "/" + userName + ",User");
            return (string)userEntry.Properties["fullname"].Value;
        }

        // Called when the page gets loaded
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                name.Text = GetUserFullName(Environment.UserDomainName, Environment.UserName);

                names = new System.Collections.Generic.List<string>();
                // Load all the names into the active directory
                using (SPSite site = new SPSite(guidSite))
                {
                    using (SPWeb web = site.OpenWeb(guidWeb))
                    {
                        SPList odoclib = web.Lists["Employees"];
                        if (odoclib != null && odoclib.ItemCount > 0)
                        {
                            SPListItemCollection listitemcoll = odoclib.GetItems();

                            // Clear out the labels to accommodate potentially new and different names from the last time this form was accessed
                            ClearLabels();

                            // Start at the letter A and go all the way to Z
                            int index = 0;
                            char letter = 'A';
                            System.Collections.Generic.List<string> group = new System.Collections.Generic.List<string>();
                            dateOfRequest.Text = BlueForm.BlueFormUserControl.GetTimeStamp().Month.ToString("D2") + "/" + BlueForm.BlueFormUserControl.GetTimeStamp().Day.ToString("D2") + "/" + BlueForm.BlueFormUserControl.GetTimeStamp().Year.ToString("D4");
                            if (listitemcoll != null && listitemcoll.Count > 0)
                            {   
                                foreach (SPListItem item in listitemcoll)
                                {
                                    // Grab the next name and add it to the list
                                    string nextName = item["Title"].ToString();
                                    names.Add(nextName);
                                    // Get information from the current user if s/he comes up in the list
                                    if (nextName.Equals(GetUserFullName(Environment.UserDomainName, Environment.UserName), StringComparison.OrdinalIgnoreCase)) 
                                    {
                                        //name.Text = GetUserFullName(Environment.UserDomainName, Environment.UserName);
                                        department.Text = item["Department"].ToString();
                                        businessUnit.Text = item["BusinessUnit"].ToString();
                                    }
                                }

                                // Sort the names array; it could be out of order
                                names.Sort();

                                while(index < names.Count)
                                {
                                    // Clear out the group list for the next letter
                                    group.Clear();


                                    while (names[index][0] == letter) // Loop letter by letter
                                    {
                                        // Keep adding the next name if it starts with the current letter
                                        group.Add(names[index]);
                                        index++;
                                        if (index >= names.Count)
                                        {
                                            break;
                                        }
                                    }

                                    // Turn this list into a long string
                                    string list = ListToString(group);

                                    // Add each string to its respective letter
                                    switch(letter)
                                    {
                                        case 'A': employeesA.Text += list;
                                        break;
                                        case 'B': employeesB.Text += list;
                                        break;
                                        case 'C': employeesC.Text += list;
                                        break;
                                        case 'D': employeesD.Text += list;
                                        break;
                                        case 'E': employeesE.Text += list;
                                        break;
                                        case 'F': employeesF.Text += list;
                                        break;
                                        case 'G': employeesG.Text += list;
                                        break;
                                        case 'H': employeesH.Text += list;
                                        break;
                                        case 'I': employeesI.Text += list;
                                        break;
                                        case 'J': employeesJ.Text += list;
                                        break;
                                        case 'K': employeesK.Text += list;
                                        break;
                                        case 'L': employeesL.Text += list;
                                        break;
                                        case 'M': employeesM.Text += list;
                                        break;
                                        case 'N': employeesN.Text += list;
                                        break;
                                        case 'O': employeesO.Text += list;
                                        break;
                                        case 'P': employeesP.Text += list;
                                        break;
                                        case 'Q': employeesQ.Text += list;
                                        break;
                                        case 'R': employeesR.Text += list;
                                        break;
                                        case 'S': employeesS.Text += list;
                                        break;
                                        case 'T': employeesT.Text += list;
                                        break;
                                        case 'U': employeesU.Text += list;
                                        break;
                                        case 'V': employeesV.Text += list;
                                        break;
                                        case 'W': employeesW.Text += list;
                                        break;
                                        case 'X': employeesX.Text += list;
                                        break;
                                        case 'Y': employeesY.Text += list;
                                        break;
                                        case 'Z': employeesZ.Text += list;
                                        break;
                                        default: break;
                                    }

                                    // Go to the next letter
                                    letter++;
                                }
                            }
                        }
                    }
                } 
            }
        }

        // This obtains the timestamp from the New Hire Employee System Access Form, if any.
        // If none exists, it returns a new timestamp.
        public static DateTime GetTimeStamp() 
        {
            // Can return the same timestamp as the sister form
            if (BlueForm.BlueFormUserControl.TimeStamp == null)
            {
                return DateTime.Now;
            }
            else 
            {
                return BlueForm.BlueFormUserControl.TimeStamp;
            }
        }

        // Gets the unique ID from the corresponding blue form or
        // generates a new one if this form is being completed on its own.
        public static string GetUniqueID() 
        {
            if (String.IsNullOrEmpty(BlueForm.BlueFormUserControl.UniqueID))
            {
                DateTime dt = BlueForm.BlueFormUserControl.GetTimeStamp();
                return dt.Year.ToString("D4") + dt.Month.ToString("D2") + dt.Day.ToString("D2") + dt.Hour.ToString("D2") + dt.Minute.ToString("D2") + dt.Second.ToString("D2");
            }
            else 
            {
                return BlueForm.BlueFormUserControl.UniqueID;
            }
        }

        // Clear all of the invisible letter labels
        private void ClearLabels() 
        {
            employeesA.Text = "";
            employeesB.Text = "";
            employeesC.Text = "";
            employeesD.Text = "";
            employeesE.Text = "";
            employeesF.Text = "";
            employeesG.Text = "";
            employeesH.Text = "";
            employeesI.Text = "";
            employeesJ.Text = "";
            employeesK.Text = "";
            employeesM.Text = "";
            employeesN.Text = "";
            employeesO.Text = "";
            employeesP.Text = "";
            employeesQ.Text = "";
            employeesR.Text = "";
            employeesS.Text = "";
            employeesT.Text = "";
            employeesU.Text = "";
            employeesV.Text = "";
            employeesW.Text = "";
            employeesX.Text = "";
            employeesY.Text = "";
            employeesZ.Text = "";
        }

        // Converts a list into a string, with each element delimited by tildes ("~").
        // For example, "Jones, Tom~Smith, Jedediah"
        private string ListToString(System.Collections.Generic.List<string> list) 
        {
            string final = "";

            for (int index = 0; index < list.Count; index++) 
            { 
                if (index > 0)
                {
                    final += "~";
                }
                final += list[index].ToString();
            }
            return final;
        }

        // Accessor for the names list
        public System.Collections.Generic.List<string> GetNames() 
        {
            return names;
        }

        // Called when the Submit button is clicked
        protected void submit_Click(object sender, EventArgs e) 
        {
            string errorString = CheckForErrors();

            if (IsNullEmptyOrWhitespace(errorString))
            {
                AddToSharePoint();
                errors.ForeColor = System.Drawing.Color.ForestGreen;
                errors.Text = "Success!";
            }
            else
            {
                errors.Text = errorString;
                errors.ForeColor = System.Drawing.Color.Red;
            }
        }
        
        // Adds the file to SharePoint.
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
                        SPList list = web.Lists.TryGetList("SecurityAuthorizationRecords");
                        if (list != null)
                        {
                            // Add a new item to the successfully opened list
                            SPListItem NewItem = list.Items.Add();
                            {
                                web.AllowUnsafeUpdates = true;

                                // Add the unique ID
                                NewItem["Title"] = id;

                                // Add all of the fields from the form
                                NewItem["Name"] = name.Text;

                                NewItem["DateOfRequest"] = dateOfRequest.Text;

                                NewItem["Department"] = department.Text;

                                NewItem["BusinessUnit"] = businessUnit.Text;

                                NewItem["IsConsultant"] = isConsultant.Checked;

                                // Only add a date if the new user is a consultant
                                if (isConsultant.Checked)
                                {
                                    NewItem["ValidUntil"] = consultantDate.Text;
                                }
                                else
                                {
                                    NewItem["ValidUntil"] = null;
                                }

                                NewItem["IsReplacement"] = isReplacement.Checked;

                                if (isReplacement.Checked)
                                {
                                    NewItem["Replacement"] = replacedEmployee.Text;
                                }
                                else 
                                {
                                    NewItem["Replacement"] = null;
                                }

                                NewItem["Location"] = location.Text;

                                NewItem["SalesOrganization"] = salesOrg.Text;

                                NewItem["SalesGroup"] = salesGroup.Text;

                                NewItem["SalesDivision"] = salesDivision.Text;

                                NewItem["SalesOffice"] = salesOffice.Text;

                                NewItem["SalesDistrict"] = salesDistrict.Text;

                                NewItem["RepCode"] = repCode.Text;

                                NewItem["ECCCopy"] = eccUser.Text;

                                NewItem["ECCAdditional"] = eccAdditional.Text;

                                NewItem["PlanningBooks"] = planningBooks.Text;

                                NewItem["ProductDepartments"] = productDepartments.Text;

                                NewItem["BWCopy"] = bwUser.Text;

                                NewItem["BWAdditional"] = bwAdditional.Text;

                                NewItem["CRMCopy"] = crmUser.Text;

                                NewItem["CRMAdditional"] = crmAdditional.Text;

                                NewItem["PayPalAccess"] = paypalAccess.Text;

                                NewItem["B2BCopy"] = b2bUser.Text;

                                NewItem["B2BAdditional"] = b2bAdditional.Text;

                                NewItem["HyperionCopy"] = hyperionUser.Text;

                                NewItem["HyperionAdditional"] = hyperionAdditional.Text;

                                NewItem["DeltaCopy"] = deltaUser.Text;

                                NewItem["DeltaAdditional"] = deltaAdditional.Text;

                                NewItem["NextGenCopy"] = nextGenUser.Text;

                                NewItem["NextGenAdditional"] = nextGenAdditional.Text;

                                NewItem["GRCCopy"] = grcUser.Text;

                                NewItem["GRCAdditional"] = grcAdditional.Text;

                                NewItem["DepartmentHead"] = departmentHead.Text;

                                NewItem["DepartmentHeadDate"] = (IsNullEmptyOrWhitespace(departmentHeadDate.Text) ? DateTime.Now.ToLongDateString() : departmentHeadDate.Text);

                                NewItem["DataOwner"] = dataOwner.Text;

                                NewItem["DataOwnerDate"] = (IsNullEmptyOrWhitespace(dataOwnerDate.Text) ? DateTime.Now.ToLongDateString() : dataOwnerDate.Text);

                                NewItem["SecurityAdmin"] = securityAdmin.Text;

                                NewItem["SecurityAdminDate"] = (IsNullEmptyOrWhitespace(securityAdminDate.Text) ? DateTime.Now.ToLongDateString() : securityAdminDate.Text);

                                NewItem["Timestamp"] = timestamp;

                                // Update the item on the server
                                NewItem.Update();

                                // Convert the information to PDF form and send it in an email
                                ConvertToPDF();//Uncomment this later

                                // Clear the form fields
                                Clear();
                            }
                        }
                        else
                        {
                            // Alert the user if the list was not found
                            errors.Text = "List not found.";
                            errors.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Alert the user of any exception that may have occurred
                errors.Text = ex.Message;
                errors.ForeColor = System.Drawing.Color.Red;
                return;
            }
        }

        // Equivalent of the String.IsNullOrWhiteSpace()
        // method found in future .NET frameworks.
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

        // My custom combination of String.IsNullOrEmpty()
        // and String.IsNullOrWhiteSpace() which checks for
        // all three of the aforementioned conditions.
        public bool IsNullEmptyOrWhitespace(string value) 
        {
            return String.IsNullOrEmpty(value) || IsNullOrWhiteSpace(value);
        }

        // Converts the data into a PDF.
        // This method makes extensive usage of the ternary operator for brevity and to save space.
        protected void ConvertToPDF() 
        {
            try 
            {
                // Create a filename of the format YYYYMMDDHHMMSSLastFirstSecurityAuthorizationForm.pdf
                fileName = BlueForm.BlueFormUserControl.GetTimeStamp().Year.ToString("D4") + BlueForm.BlueFormUserControl.GetTimeStamp().Month.ToString("D2") + BlueForm.BlueFormUserControl.GetTimeStamp().Day.ToString("D2") + BlueForm.BlueFormUserControl.GetTimeStamp().Hour.ToString("D2")
                    + BlueForm.BlueFormUserControl.GetTimeStamp().Minute.ToString("D2") + BlueForm.BlueFormUserControl.GetTimeStamp().Second.ToString("D2") + Environment.UserName + "SecurityAuthorizationForm.pdf";

                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        // Build lots of tables in HTML with line breaks in between
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<img style='margin: auto; text-align: center;' src='" + image + "' alt='Sharp Logo' />");

                        sb.Append("<br />");

                        sb.Append("<h1 style='text-align: center; font-weight: bold;'>Security Authorization Form</h1>");

                        sb.Append("<br />");

                        sb.Append("<table style='border: none; width: 100%;'>");
                        sb.Append("<tr><th style='font-weight: bold; text-align: center; padding: 2px;' colspan='2'>User Information</tr>");
                        sb.Append("<tr><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Name (Last, First)</th><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Date of Request</th></tr>");
                        sb.Append("<tr><td style='text-align: center; padding: 2px;' colspan='1'>" + name.Text + "</td><td style='text-align: center; padding: 2px;' colspan='1'>" + dateOfRequest.Text + "</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<br />");

                        sb.Append("<table style='border: none; width: 100%;'>");
                        sb.Append("<tr><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Department</th><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Business Unit</th></tr>");
                        sb.Append("<tr><td style='text-align: center; padding: 2px;' colspan='1'>" + department.Text + "</td><td style='text-align: center; padding: 2px;' colspan='1'>" + businessUnit.Text + "</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<br />");

                        sb.Append("<table style='border: none; width: 100%;'>");
                        sb.Append("<tr><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Consultant, Valid To</th><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Location</th></tr>");
                        sb.Append("<tr><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(consultantDate.Text) ? consultantDate.Text : "N/A") + "</td><td style='text-align: center; padding: 2px;' colspan='1'>" + location.Text + "</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<br />");

                        sb.Append("<table style='border: none; width: 100%;'>");
                        sb.Append("<tr><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>New Employee, Replacement For</th><th></th></tr>");
                        sb.Append("<tr><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(replacedEmployee.Text) ? replacedEmployee.Text : "N/A") + "</td><td></td></tr>");
                        sb.Append("</table>");

                        sb.Append("<br />");
                        sb.Append("<br />");

                        sb.Append("<table style='border: none; width: 100%;'>");
                        sb.Append("<tr><th style='font-weight: bold; text-align: center; padding: 2px;' colspan='2'>Sales Area Information (Required for all Sales Related Requests—APO, BW, B2B)</th></tr>");
                        sb.Append("<tr><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Sales Organization</th><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Sales Group</th></tr>");
                        sb.Append("<tr><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(salesOrg.Text) ? salesOrg.Text : "N/A") + "</td><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(salesGroup.Text) ? salesGroup.Text : "N/A") + "</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<br />");

                        sb.Append("<table style='border: none; width: 100%;'>");
                        sb.Append("<tr><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Sales Division</th><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Sales Office</th></tr>");
                        sb.Append("<tr><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(salesDivision.Text) ? salesDivision.Text : "N/A") + "</td><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(salesOffice.Text) ? salesOffice.Text : "N/A") + "</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<br />");

                        sb.Append("<table style='border: none; width: 100%;'>");
                        sb.Append("<tr><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Sales District</th><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Rep Code</th></tr>");
                        sb.Append("<tr><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(salesDistrict.Text) ? salesDistrict.Text : "N/A") + "</td><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(repCode.Text) ? repCode.Text : "N/A") + "</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<br />");

                        sb.Append("<table style='border: none; width: 100%;'>");
                        sb.Append("<tr><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Request By</th><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Phone</th></tr>");
                        sb.Append("<tr><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(requestBy.Text) ? requestBy.Text : "N/A") + "</td><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(phone.Text) ? phone.Text : "N/A") + "</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<br />");
                        sb.Append("<br />");
                        sb.Append("<br />");

                        sb.Append("<table style='border: none; width: 100%;'>");
                        sb.Append("<tr><th style='font-weight: bold; text-align: center; padding: 2px;' colspan='2'>System Information</th></tr>");
                        sb.Append("<tr><th style='text-align: center; padding: 2px;' colspan='2'>SAP ECC</th></tr>");
                        sb.Append("<tr><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Copy Profile Of</th><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>New or Change—Additional Roles</th></tr>");
                        sb.Append("<tr><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(eccUser.Text) ? eccUser.Text : "N/A") + "</td><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(eccAdditional.Text) ? eccAdditional.Text : "N/A") + "</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<br />");

                        sb.Append("<table style='border: none; width: 100%;'>");
                        sb.Append("<tr><th style='text-align: center; padding: 2px;' colspan='2'>SAP APO</th></tr>");
                        sb.Append("<tr><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Planning Book(s)</th><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Product Department(s)</th></tr>");
                        sb.Append("<tr><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(planningBooks.Text) ? productDepartments.Text : "N/A") + "</td><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(eccAdditional.Text) ? eccAdditional.Text : "N/A") + "</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<br />");

                        sb.Append("<table style='border: none; width: 100%;'>");
                        sb.Append("<tr><th style='text-align: center; padding: 2px;' colspan='2'>SAP BW</th></tr>");
                        sb.Append("<tr><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Copy Profile Of</th><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>New or Change—Additional Roles</th></tr>");
                        sb.Append("<tr><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(bwUser.Text) ? bwUser.Text : "N/A") + "</td><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(bwAdditional.Text) ? bwAdditional.Text : "N/A") + "</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<br />");

                        sb.Append("<table style='border: none; width: 100%;'>");
                        sb.Append("<tr><th style='text-align: center;' colspan='2'>SAP CRM</th></tr>");
                        sb.Append("<tr><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Copy Profile Of</th><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>New or Change—Additional Roles</th></tr>");
                        sb.Append("<tr><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(crmUser.Text) ? crmUser.Text : "N/A") + "</td><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(crmAdditional.Text) ? crmAdditional.Text : "N/A") + "</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<br />");

                        // Determine the access string by which button was checked
                        string access;
                        if (full.Checked)
                        {
                            access = "Full Access";
                        }
                        else if (limited.Checked)
                        {
                            access = "Limited Access";
                        }
                        else if (view.Checked)
                        {
                            access = "View";
                        }
                        else 
                        {
                            access = "N/A";
                        }

                        sb.Append("<br />");

                        sb.Append("<p style='text-align: center;'>Paypal Access: " + access + "</p>");

                        sb.Append("<br />");
                        sb.Append("<br />");

                        sb.Append("<table style='border: none; width: 100%;'>");
                        sb.Append("<tr><th style='text-align: center; padding: 2px;' colspan='2'>Sharp B2B</th></tr>");
                        sb.Append("<tr><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Copy Profile Of</th><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>New or Change—Additional Roles</th></tr>");
                        sb.Append("<tr><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(b2bUser.Text) ? b2bUser.Text : "N/A") + "</td><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(b2bAdditional.Text) ? b2bAdditional.Text : "N/A") + "</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<br />");

                        sb.Append("<table style='border: none; width: 100%;'>");
                        sb.Append("<tr><th style='text-align: center; padding: 2px;' colspan='2'>Hyperion</th></tr>");
                        sb.Append("<tr><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Copy Profile Of</th><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>New or Change—Additional Roles</th></tr>");
                        sb.Append("<tr><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(hyperionUser.Text) ? hyperionUser.Text : "N/A") + "</td><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(hyperionAdditional.Text) ? hyperionAdditional.Text : "N/A") + "</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<br />");

                        sb.Append("<table style='border: none; width: 100%;'>");
                        sb.Append("<tr><th style='text-align: center; padding: 2px;' colspan='2'>Delta Paymetrics</th></tr>");
                        sb.Append("<tr><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Copy Profile Of</th><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>New or Change—Additional Roles</th></tr>");
                        sb.Append("<tr><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(deltaUser.Text) ? deltaUser.Text : "N/A") + "</td><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(deltaAdditional.Text) ? deltaAdditional.Text : "N/A") + "</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<br />");

                        sb.Append("<table style='border: none; width: 100%;'>");
                        sb.Append("<tr><th style='text-align: center; padding: 2px;' colspan='2'>NextGen</th></tr>");
                        sb.Append("<tr><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Copy Profile Of</th><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>New or Change—Additional Roles</th></tr>");
                        sb.Append("<tr><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(nextGenUser.Text) ? nextGenUser.Text : "N/A") + "</td><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(nextGenAdditional.Text) ? nextGenAdditional.Text : "N/A") + "</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<br />");

                        sb.Append("<table style='border: none; width: 100%;'>");
                        sb.Append("<tr><th style='text-align: center; padding: 2px;' colspan='2'>GRC Control Reporting</th></tr>");
                        sb.Append("<tr><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Copy Profile Of</th><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>New or Change—Additional Roles</th></tr>");
                        sb.Append("<tr><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(grcUser.Text) ? grcUser.Text : "N/A") + "</td><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(grcAdditional.Text) ? grcAdditional.Text : "N/A") + "</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<br />");
                        sb.Append("<br />");
                        sb.Append("<br />");

                        sb.Append("<table style='border: none; width: 100%;'>");
                        sb.Append("<tr><th style='font-weight: bold; text-align: center; padding: 2px;' colspan='2'>Approvers</th></tr>");
                        sb.Append("<tr><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Department Head (Route to the proper data owner)</th><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Date</th></tr>");
                        sb.Append("<tr><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(departmentHead.Text) ? departmentHead.Text : "N/A") + "</td><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(departmentHeadDate.Text) ? departmentHeadDate.Text : "N/A") + "</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<br />");

                        sb.Append("<table style='border: none; width: 100%;'>");
                        sb.Append("<tr><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Data Owner (Business Process Owner, Sub-owner)</th><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Date</th></tr>");
                        sb.Append("<tr><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(dataOwner.Text) ? dataOwner.Text : "N/A") + "</td><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(dataOwnerDate.Text) ? dataOwnerDate.Text : "N/A") + "</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<br />");

                        sb.Append("<table style='border: none; width: 100%;'>");
                        sb.Append("<tr><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>SAP Security Admin</th><th style='font-size: 10px; text-align: center; padding: 2px;' colspan='1'>Date</th></tr>");
                        sb.Append("<tr><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(securityAdmin.Text) ? securityAdmin.Text : "N/A") + "</td><td style='text-align: center; padding: 2px;' colspan='1'>" + (!IsNullEmptyOrWhitespace(securityAdminDate.Text) ? securityAdminDate.Text : "N/A") + "</td></tr>");
                        sb.Append("</table>");

                        sb.Append("<br />");
                        sb.Append("<br />");
                        sb.Append("<br />");
                        sb.Append("<br />");
                        sb.Append("<br />");
                        sb.Append("<br />");
                        sb.Append("<br />");
                        sb.Append("<br />");
                        sb.Append("<br />");
                        sb.Append("<br />");
                        sb.Append("<br />");
                        sb.Append("<br />");                     
                        sb.Append("<br />");
                        sb.Append("<br />");
                        sb.Append("<br />");
                        sb.Append("<br />");
                        sb.Append("<br />");
                        sb.Append("<br />");

                        // Generate a timestamp to print on the bottom
                        string generated = "Generated on " + DateTime.Now.ToLongDateString() + " at " + DateTime.Now.ToString("%h") + ":" + DateTime.Now.Minute.ToString("D2") + ":" + DateTime.Now.Second.ToString("D2") + " ";

                        // Differentiate between A.M. and P.M.
                        if (DateTime.Now.Hour < 12)
                        {
                            generated += "A.M.";
                        }
                        else
                        {
                            generated += "P.M.";
                        }

                        sb.Append("<p style='text-align: center'>" + generated + "</p>");

                        StringReader sr = new StringReader(sb.ToString());

                        //Using the ItextSharp Document
                        Document document = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
                        HTMLWorker htmlParser = new HTMLWorker(document);

                        using (MemoryStream stream = new MemoryStream())
                        {
                            //Using the ItextSharp PdfWriter to create an instance of the document using the memory stream.
                            PdfWriter writer = PdfWriter.GetInstance(document, stream);
                            writer.SetFullCompression();

                            //Open the document; otherwise, we won't be able to add to it.
                            document.Open();

                            // Parse the HTML with the HTMLWorker
                            htmlParser.Parse(sr);

                            // Close the document
                            document.Close();

                            // Get the byte array of the memory stream
                            byte[] bytes = stream.ToArray();
                            const string fromPassword = "tlhingan";

                            // Validate the credentials
                            ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;

                            // Attach the PDF as an attachment
                            Attachment att = new Attachment(new MemoryStream(bytes), fileName);

                            // Replace these with SAP security email address and password
                            var fromAddress = new MailAddress(SPContext.Current.Web.CurrentUser.Email, GetUserFullName(Environment.UserDomainName, Environment.UserName));
                            //var fromAddress = new MailAddress("coinstampwebs@gmail.com", "Ryan King");
                            //var toAddress = new MailAddress("ryan.king0720@gmail.com", "Ryan King");
                            //var toAddress = new MailAddress("rajasundaraa@sharpsec.com", "Rajasundaram, Arulraj");
                            // const string fromPassword = "Sharp.2";

                            string subject = "Security Authorization Form " + fileName;
                            const string body = "";
                            
                            // Set up the SMTP client
                            
                            SmtpClient smtp = new SmtpClient();  
                            smtp.UseDefaultCredentials = false;  
                            smtp.Credentials = new System.Net.NetworkCredential("KingRy@sharpsec.com", "Sharp.2");
                            smtp.Host = "smtp.office365.com";  
                            smtp.Port = 587;  // This is critical
                            smtp.EnableSsl = true;  // This is critical
                            
                            /*// Old Gmail client used for debugging (DO NOT USE!)
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

                            // Instantiate the MailMessage object
                            MailMessage message = new MailMessage();

                            // Fill out the basic fields of the MailMessage object
                            message.From = fromAddress;
                            //message.To.Add(toAddress);
                            // Addresses for SAP Security
                            
                            message.To.Add("SAPSecurity@sharpamericas.com");
                            message.To.Add("SAPSecurity@sharpsec.com");
                            message.To.Add("SAPSecurity@sharpusa.com");
                            message.To.Add("SAPSecurity@sharpusa.mail.onmicrosoft.com");
                            message.To.Add("SAPSecurity@sharpusa.onmicrosoft.com");
                            
                            message.Subject = subject;
                            message.Body = body;

                            // Add the PDF as an attachment
                            message.Attachments.Add(att);

                            // Send the message
                            //smtp.Send(message);//Uncomment this later

                            // Report a successful send
                            errors.Text = "Success!";
                            errors.ForeColor = System.Drawing.Color.ForestGreen;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Catch any exception that may have occurred
                MsgBox("An exception occurred. Message: " + e.Message);
            }
        }

        protected void Clear() 
        {
            // Reset all of the initially empty fields for the submission of another form
            name.Text = "";
            dateOfRequest.Text = "";
            department.Text = "";
            businessUnit.Text = "";
            isConsultant.Checked = false;
            isReplacement.Checked = false;
            isNotReplacement.Checked = false;
            view.Checked = false;
            limited.Checked = false;
            full.Checked = false;
            consultantDate.Text = "";
            replacedEmployee.Text = "";
            location.Text = "";
            salesOrg.Text = "";
            salesGroup.Text = "";
            salesDivision.Text = "";
            salesOffice.Text = "";
            salesDistrict.Text = "";
            repCode.Text = "";
            requestBy.Text = "";
            phone.Text = "";
            eccUser.Text = "";
            eccAdditional.Text = "";
            planningBooks.Text = "";
            productDepartments.Text = "";
            bwUser.Text = "";
            bwAdditional.Text = "";
            crmUser.Text = "";
            crmAdditional.Text = "";
            b2bUser.Text = "";
            b2bAdditional.Text = "";
            hyperionUser.Text = "";
            hyperionAdditional.Text = "";
            deltaUser.Text = "";
            deltaAdditional.Text = "";
            nextGenUser.Text = "";
            nextGenAdditional.Text = "";
            grcUser.Text = "";
            grcAdditional.Text = "";
            departmentHead.Text = "";
            departmentHeadDate.Text = "";
            dataOwner.Text = "";
            dataOwnerDate.Text = "";
            securityAdmin.Text = "";
            securityAdminDate.Text = "";
            ClearLabels();
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

        // Checks for errors on the form by determining if all of the
        // required fields have been filled out.
        // Returns an error message on failure and null on success.
        protected string CheckForErrors() 
        {
            string headerString = "We have encountered the following error";
            string errorString = "";
            int errors = 0;

            if (IsNullEmptyOrWhitespace(name.Text))
            {
                errorString += "You did not specify a name.<br />";
                errors++;
            }

            if (IsNullEmptyOrWhitespace(dateOfRequest.Text))
            {
                errorString += "You did not specify the date of this request.<br />";
                errors++;
            }

            if (IsNullEmptyOrWhitespace(department.Text))
            {
                errorString += "You did not specify a department.<br />";
                errors++;
            }

            if (IsNullEmptyOrWhitespace(businessUnit.Text))
            {
                errorString += "You did not specify a business unit.<br />";
                errors++;
            }

            if ((!isConsultant.Checked && IsNullEmptyOrWhitespace(consultantDate.Text) && (((!isReplacement.Checked && !isNotReplacement.Checked) || isNotReplacement.Checked) && IsNullEmptyOrWhitespace(replacedEmployee.Text))))
            {
                errorString += "You did not choose a position for the employee.<br />";
                errors++;
            }

            if (isConsultant.Checked && isReplacement.Checked)
            {
                errorString += "You must choose only one position for the employee.<br />";
                errors++;
            }

            if (isConsultant.Checked && !isReplacement.Checked && IsNullEmptyOrWhitespace(consultantDate.Text))
            {
                errorString += "You did not specify a consultant date.<br />";
                errors++;
            }

            if (IsNullEmptyOrWhitespace(location.Text))
            {
                errorString += "You did not specify a location.<br />";
                errors++;
            }

            if (isReplacement.Checked && !isConsultant.Checked && IsNullEmptyOrWhitespace(replacedEmployee.Text)) 
            {
                errorString += "You did not specify an employee being replaced.<br />";
                errors++;
            }

            if (IsNullEmptyOrWhitespace(departmentHead.Text))
            {
                errorString += "You did not specify a department head.<br />";
                errors++;
            }

            if (IsNullEmptyOrWhitespace(departmentHeadDate.Text))
            {
                errorString += "You did not specify a department head date.<br />";
                errors++;
            }

            if (IsNullEmptyOrWhitespace(dataOwner.Text))
            {
                errorString += "You did not specify a data owner.<br />";
                errors++;
            }

            if (IsNullEmptyOrWhitespace(dataOwnerDate.Text))
            {
                errorString += "You did not specify a data owner date.<br />";
                errors++;
            }

            if (IsNullEmptyOrWhitespace(securityAdmin.Text))
            {
                errorString += "You did not specify an SAP Security admin.<br />";
                errors++;
            }

            if (IsNullEmptyOrWhitespace(securityAdminDate.Text))
            {
                errorString += "You did not specify an SAP Security admin date.<br />";
                errors++;
            }

            if (errors > 1)
            {
                headerString += "s";
            }

            headerString += " in your form:<br />" + errorString;

            if (errors > 0)
            {
                return headerString;
            }
            else 
            {
                return null;
            }
        }

        protected void cvConsultant_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (isConsultant.Checked && !isReplacement.Checked && IsNullEmptyOrWhitespace(consultantDate.Text))
            {
                args.IsValid = false;
            }
            else 
            {
                args.IsValid = true;
            }
        }

        protected void cvReplacedEmployee_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (isReplacement.Checked && !isConsultant.Checked && IsNullEmptyOrWhitespace(consultantDate.Text) && IsNullEmptyOrWhitespace(replacedEmployee.Text))
            {
                args.IsValid = false;
            }
            else 
            {
                args.IsValid = true;
            }
        }
    }
}