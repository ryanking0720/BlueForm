<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BlueFormUserControl.ascx.cs" Inherits="BlueForm.BlueForm.BlueFormUserControl" %>

<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js" type="text/javascript"></script>
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/js/bootstrap.min.js" type="text/javascript"></script>
<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
<script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type="text/javascript"></script>

<link rel="stylesheet" type="text/css" href="http://njspdevd01:2544/nhesaf/SiteAssets/nhesaf.css" />

<script type="text/javascript">
    $(document).ready(function () {
        $('.phone').keyup(function () {
            $(this).val($(this).val().replace(/[^0-9]*(\d{3})[^0-9]*\-?[^0-9]*(\d{3})[^0-9]*\-?[^0-9]*(\d{4})[^0-9]*/, '$1-$2-$3'))
        });

        $("#ctl00_ctl23_g_2a1a07da_472b_4953_9fb1_a4883f77b6c7_ctl00_sap").click(function () {
            $("#ctl00_ctl23_g_2a1a07da_472b_4953_9fb1_a4883f77b6c7_ctl00_checkedLabel").html("Yes");
            if ($(this).is(":checked")) {
                var displayName = $("#ctl00_ctl23_g_2a1a07da_472b_4953_9fb1_a4883f77b6c7_ctl00_lastName").val() + ", " + $("#ctl00_ctl23_g_2a1a07da_472b_4953_9fb1_a4883f77b6c7_ctl00_firstName").val();
                var uniqueID = $("#ctl00_ctl23_g_2a1a07da_472b_4953_9fb1_a4883f77b6c7_ctl00_uniqueID").val();
                sessionStorage.setItem("name", displayName);
                sessionStorage.setItem("uniqueID", uniqueID);
                var win = window.open("http://njspdevd01:2544/nhesaf/Pages/Security-Authorization-Form.aspx", '_blank');
                win.focus();
                $("#ctl00_ctl23_g_2a1a07da_472b_4953_9fb1_a4883f77b6c7_ctl00_sap").attr("disabled", "disabled");
            }
        });
    });   
</script>
<asp:UpdatePanel runat="server">
<ContentTemplate>
<h1 class="header">New Hire Employee System Access Form</h1>

<br />

<p class="label"><strong>Note</strong>: If this form is being completed in tandem with the <a href="http://njspdevd01:2544/nhesaf/Pages/Security-Authorization-Form.aspx">Security Authorization Form</a>, please submit this form second.</p>

<br />

<asp:Label ID="results" class="label" runat="server"></asp:Label>

<br />

<asp:Label ID="createdBy" class="invisible" runat="server"></asp:Label>
<asp:Label ID="uniqueID" class="invisible" runat="server"></asp:Label>
<p class="right"><span class="required" title="required">*</span> <em>denotes a required field.</em></p>
<br />
<div class="flex">
    <div class="block">
        <asp:Label ID="firstNameLabel" runat="server" class="label">First Name <span class="required" title="required">*</span></asp:Label>
        <br />
        <asp:TextBox ID="firstName" class="text" runat="server"></asp:TextBox>
        <br />
        <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" class="validator" ControlToValidate="firstName" InitialValue="" ErrorMessage="Please enter a first name"></asp:RequiredFieldValidator>
    </div>

    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp

    <div class="block">
        <asp:Label ID="middleNameLabel" runat="server" class="label">Middle Name <span class="required" title="required">*</span></asp:Label>
        <br />
        <asp:TextBox ID="middleName" class="text" runat="server"></asp:TextBox>
        <br />
        <asp:RequiredFieldValidator ID="rfvMiddleName" runat="server" class="validator" ControlToValidate="middleName" InitialValue="" ErrorMessage="Please enter a middle name"></asp:RequiredFieldValidator>
    </div>

    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp

    <div class="block">
        <asp:Label ID="lastNameLabel" runat="server" class="label">Last Name <span class="required" title="required">*</span></asp:Label>
        <br />
        <asp:TextBox ID="lastName" class="text" runat="server"></asp:TextBox>
        <br />
        <asp:RequiredFieldValidator ID="rfvLastName" runat="server" class="validator" ControlToValidate="lastName" InitialValue="" ErrorMessage="Please enter a last name"></asp:RequiredFieldValidator>
    </div>
</div>
<br />
<br />
<asp:Label ID="deviceLabel" runat="server" class="label">Device <span class="required" title="required">*</span></asp:Label>
<br />
<asp:DropDownList ID="device" class="text" runat="server">
    <asp:ListItem Value="None Selected" class="listitem" runat="server">None Selected</asp:ListItem>
    <asp:ListItem Value="Desktop" Selected="False" class="listitem" runat="server">Desktop</asp:ListItem>
    <asp:ListItem Value="Laptop" Selected="False" class="listitem" runat="server">Laptop</asp:ListItem>
</asp:DropDownList>
<br />
<asp:RequiredFieldValidator ID="rfvDevice" runat="server" class="validator" ControlToValidate="device" InitialValue="None Selected" ErrorMessage="Please select a device"></asp:RequiredFieldValidator>
<br />
<br />
<asp:Label ID="sharpPhoneLabel" runat="server" class="label">Sharp Phone <span class="required" title="required">*</span></asp:Label>
<br />
<asp:TextBox ID="sharpPhone" class="phone text" runat="server"></asp:TextBox>
<br />
<asp:RequiredFieldValidator ID="rfvSharpPhone" runat="server" class="validator" ControlToValidate="sharpPhone" InitialValue="" ErrorMessage="Please enter a mobile phone number"></asp:RequiredFieldValidator>
<br />
<br />
<asp:Label ID="mobilePhoneLabel" runat="server" class="label">Mobile Phone <span class="required" title="required">*</span></asp:Label>
<br />
<asp:TextBox ID="mobilePhone" class="phone text" runat="server"></asp:TextBox>
<br />
<asp:RequiredFieldValidator ID="rfvMobilePhone" runat="server" class="validator" ControlToValidate="mobilePhone" InitialValue="" ErrorMessage="Please enter a Sharp phone number"></asp:RequiredFieldValidator>
<br />
<br />
<!--
<fieldset>
    <legend style="font-family: Segoe UI; font-size: 18px">Application List (check all that apply) <span class="required" title="required">*</span></legend>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:CheckBox ID="prod" class="label" Text="Prod" runat="server" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:CheckBox ID="qa" class="label" Text="QA" runat="server" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:CheckBox ID="dev" class="label" Text="Dev" runat="server" />
    <br />

    <asp:CheckBox ID="ecc" Font-Names="Segoe UI" Text="SAP ECC" Font-Size="Large" runat="server" />
    <br />
    <asp:CheckBox ID="crm" Font-Names="Segoe UI" Text="SAP CRM" Font-Size="Large" runat="server" />
    <br />
    <asp:CheckBox ID="bw" Font-Names="Segoe UI" Text="SAP BW" Font-Size="Large" runat="server" />
</fieldset>
-->
<asp:CheckBox ID="sap" Checked="false" class="label" Text="SAP" runat="server" oncheckedchanged="sap_CheckedChanged" />
<asp:Label ID="checkedLabel" class="invisible" runat="server"></asp:Label>
<br />
<br />
<asp:Button ID="submit" Text="Submit" class="button" runat="server" onclick="submit_Click" />
</ContentTemplate>
</asp:UpdatePanel>