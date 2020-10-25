<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SecurityAuthorizationFormUserControl.ascx.cs" Inherits="BlueForm.SecurityAuthorizationForm.SecurityAuthorizationFormUserControl" %>
<%@ Register assembly="AjaxControlToolkit, Version=3.0.30930.28736, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js" type="text/javascript"></script>
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/js/bootstrap.min.js" type="text/javascript"></script>
<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
<script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type="text/javascript"></script>

<!--
Project: Security Authorization Form

Author: Ryan King

Date: August 27, 2019

Purpose: This form is used to grant access to sensitive company information.
It has many different fields for different types of employees.
Therefore, not all of them will be filled out for every form sent.

All forms are saved into SharePoint upon submission.
If this form is being done in tandem with a New Hire Employee System Access Form,
the two will share an identical timestamp and unique ID. Otherwise, this will be
independent of any other form.

RequiredFieldValidators prevent the user from submitting this form until all the required
fields are filled out, namely the ones at the top section and the bottom section.

The replacement employee fields use a JQuery autocomplete feature which takes from a database
of names.
-->

<script type="text/javascript">
    $(function () {
        var namesA = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesA").html();
        var namesB = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesB").html();
        var namesC = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesC").html();
        var namesD = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesD").html();
        var namesE = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesE").html();
        var namesF = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesF").html();
        var namesG = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesG").html();
        var namesH = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesH").html();
        var namesI = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesI").html();
        var namesJ = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesJ").html();
        var namesK = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesK").html();
        var namesL = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesL").html();
        var namesM = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesM").html();
        var namesN = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesN").html();
        var namesO = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesO").html();
        var namesP = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesP").html();
        var namesQ = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesQ").html();
        var namesR = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesR").html();
        var namesS = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesS").html();
        var namesT = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesT").html();
        var namesU = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesU").html();
        var namesV = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesV").html();
        var namesW = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesW").html();
        var namesX = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesX").html();
        var namesY = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesY").html();
        var namesZ = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesZ").html();

        var employeesA = namesA.split('~');
        var employeesB = namesB.split('~');
        var employeesC = namesC.split('~');
        var employeesD = namesD.split('~');
        var employeesE = namesE.split('~');
        var employeesF = namesF.split('~');
        var employeesG = namesG.split('~');
        var employeesH = namesH.split('~');
        var employeesI = namesI.split('~');
        var employeesJ = namesJ.split('~');
        var employeesK = namesK.split('~');
        var employeesL = namesL.split('~');
        var employeesM = namesM.split('~');
        var employeesN = namesN.split('~');
        var employeesO = namesO.split('~');
        var employeesP = namesP.split('~');
        var employeesQ = namesQ.split('~');
        var employeesR = namesR.split('~');
        var employeesS = namesS.split('~');
        var employeesT = namesT.split('~');
        var employeesU = namesU.split('~');
        var employeesV = namesV.split('~');
        var employeesW = namesW.split('~');
        var employeesX = namesX.split('~');
        var employeesY = namesY.split('~');
        var employeesZ = namesZ.split('~');

        var employees = [];

        employees = employees.concat(employeesA, employeesB, employeesC, employeesD, employeesE,
        employeesF, employeesG, employeesH, employeesI, employeesJ, employeesK,
        employeesL, employeesM, employeesN, employeesO, employeesP, employeesQ,
        employeesR, employeesS, employeesT, employeesU, employeesV, employeesW,
        employeesX, employeesY, employeesZ);

        var $k = jQuery.noConflict();
        $k(".dropdown").autocomplete({
            source: employees
        });
    });

    $(function () {
        var $i = jQuery.noConflict();
        $i(".date").datepicker({
            minDate: 0
        });
    });

    function pageLoad() {
        var namesA = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesA").html();
        var namesB = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesB").html();
        var namesC = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesC").html();
        var namesD = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesD").html();
        var namesE = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesE").html();
        var namesF = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesF").html();
        var namesG = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesG").html();
        var namesH = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesH").html();
        var namesI = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesI").html();
        var namesJ = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesJ").html();
        var namesK = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesK").html();
        var namesL = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesL").html();
        var namesM = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesM").html();
        var namesN = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesN").html();
        var namesO = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesO").html();
        var namesP = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesP").html();
        var namesQ = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesQ").html();
        var namesR = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesR").html();
        var namesS = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesS").html();
        var namesT = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesT").html();
        var namesU = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesU").html();
        var namesV = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesV").html();
        var namesW = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesW").html();
        var namesX = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesX").html();
        var namesY = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesY").html();
        var namesZ = $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_employeesZ").html();

        var employeesA = namesA.split('~');
        var employeesB = namesB.split('~');
        var employeesC = namesC.split('~');
        var employeesD = namesD.split('~');
        var employeesE = namesE.split('~');
        var employeesF = namesF.split('~');
        var employeesG = namesG.split('~');
        var employeesH = namesH.split('~');
        var employeesI = namesI.split('~');
        var employeesJ = namesJ.split('~');
        var employeesK = namesK.split('~');
        var employeesL = namesL.split('~');
        var employeesM = namesM.split('~');
        var employeesN = namesN.split('~');
        var employeesO = namesO.split('~');
        var employeesP = namesP.split('~');
        var employeesQ = namesQ.split('~');
        var employeesR = namesR.split('~');
        var employeesS = namesS.split('~');
        var employeesT = namesT.split('~');
        var employeesU = namesU.split('~');
        var employeesV = namesV.split('~');
        var employeesW = namesW.split('~');
        var employeesX = namesX.split('~');
        var employeesY = namesY.split('~');
        var employeesZ = namesZ.split('~');

        var employees = [];

        employees = employees.concat(employeesA, employeesB, employeesC, employeesD, employeesE,
        employeesF, employeesG, employeesH, employeesI, employeesJ, employeesK,
        employeesL, employeesM, employeesN, employeesO, employeesP, employeesQ,
        employeesR, employeesS, employeesT, employeesU, employeesV, employeesW,
        employeesX, employeesY, employeesZ);

        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_replacedEmployee").unbind();
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_eccUser").unbind();
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_bwUser").unbind();
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_crmUser").unbind();
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_b2bUser").unbind();
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_hyperionUser").unbind();
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_deltaUser").unbind();
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_nextGenUser").unbind();
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_grcUser").unbind();
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_departmentHead").unbind();
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_dataOwner").unbind();
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_securityAdmin").unbind();

        var $j = jQuery.noConflict();
        $j(".dropdown").autocomplete({
            source: employees
        });
    }



    // Instructions from Sainath in "authorization.txt" on my desktop.
    // IDs
    // Name: ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_name
    // Date of Request: ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_dateOfRequest
    // Department: ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_department
    // Business Unit: ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_businessUnit
    // Consultant checkbox: ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_isConsultant
    // Consultant textbox: ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_consultantDate
    // Consultant location textbox: ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_location
    // New Employee Yes: ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_isReplacement
    // New Employee No: ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_isNotReplacement
    // Replaced employee: ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_replacedEmployee
    // View PayPal: ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_view
    // Limited access to PayPal: ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_limited
    // Full access to PayPal: ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_full

    function checkFlag() {
        var n = sessionStorage.getItem("name");
        var u = sessionStorage.getItem("uniqueID");

        if ((n != null && u != null) && document.getElementById("ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_errors").innerHTML != "Success!") {
            window.setTimeout(checkFlag, 100); /* this checks the flag every 100 milliseconds*/
        } else {
            close();
        }
    }

    $(document).ready(function () {
        var name = sessionStorage.getItem("name");
        var uniqueID = sessionStorage.getItem("uniqueID");

        if (name != null) {
            $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_name").val(name);
        }

        if (uniqueID != null) {
            $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_uniqueID").val(uniqueID);
        }

        $('.phone').keyup(function () {
            $(this).val($(this).val().replace(/[^0-9]*(\d{3})[^0-9]*\-?[^0-9]*(\d{3})[^0-9]*\-?[^0-9]*(\d{4})[^0-9]*/, '$1-$2-$3'))
        });

        checkFlag();

        /*
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_isConsultant").click(function () {
        if ($(this).is(":checked")) {
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_consultantDate").removeAttr("disabled");

        var $k = jQuery.noConflict();
        $k("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_consultantDate").datepicker({
        minDate: 0
        });

        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_replacedEmployee").val("");
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_isReplacement").attr("disabled", "disabled");
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_isNotReplacement").attr("disabled", "disabled");
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_replacedEmployee").attr("disabled", "disabled");
        } else {
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_consultantDate").val("");
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_consultantDate").attr("disabled", "disabled");
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_isReplacement").removeAttr("disabled");
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_isNotReplacement").removeAttr("disabled");
        }
        });        

        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_view").click(function () {
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_paypalAccess").html("View");
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_limited").removeAttr("checked");
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_full").removeAttr("checked");
        });

        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_limited").click(function () {
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_paypalAccess").html("Limited");
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_view").removeAttr("checked");
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_full").removeAttr("checked");
        });

        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_full").click(function () {
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_paypalAccess").html("Full");
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_view").removeAttr("checked");
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_limited").removeAttr("checked");
        });

        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_isReplacement").click(function () {
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_isNotReplacement").removeAttr("checked");
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_replacement").html("Yes");
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_replacedEmployee").removeAttr("disabled");
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_consultantDate").val("");
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_consultantDate").attr("disabled", "disabled");
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_isConsultant").attr("disabled", "disabled");
        });

        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_isNotReplacement").click(function () {
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_isReplacement").removeAttr("checked");
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_replacement").html("No");
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_replacedEmployee").val("");
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_replacedEmployee").attr("disabled", "disabled");
        $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_isConsultant").removeAttr("disabled");
        });
        */
    });
    

    /*
    $("#ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_submit").click(function () {
        close();
    });
    */
</script>

<link rel="stylesheet" type="text/css" href="http://code.jquery.com/ui/1.12.1/themes/cupertino/jquery-ui.css" />
<link rel="stylesheet" type="text/css" href="http://njspdevd01:2544/nhesaf/SiteAssets/SecurityAuthorizationForm.css" />

<asp:UpdatePanel runat="server">
<ContentTemplate>
<p class="label large"><strong>Note</strong>: If this form is being completed in tandem with the <a href="http://njspdevd01:2544/nhesaf/Pages/default.aspx">New Hire Employee System Access Form</a>, please submit this form first.</p>

<br />

<h2 class="subheader">User Information</h2>
<br />
<!--These labels are used to pass in names of employees from a database. They contain each name in a long string, letter by letter.-->
<asp:Label ID="employeesA" class="invisible" runat="server"></asp:Label>
<asp:Label ID="employeesB" class="invisible" runat="server"></asp:Label>
<asp:Label ID="employeesC" class="invisible" runat="server"></asp:Label>
<asp:Label ID="employeesD" class="invisible" runat="server"></asp:Label>
<asp:Label ID="employeesE" class="invisible" runat="server"></asp:Label>
<asp:Label ID="employeesF" class="invisible" runat="server"></asp:Label>
<asp:Label ID="employeesG" class="invisible" runat="server"></asp:Label>
<asp:Label ID="employeesH" class="invisible" runat="server"></asp:Label>
<asp:Label ID="employeesI" class="invisible" runat="server"></asp:Label>
<asp:Label ID="employeesJ" class="invisible" runat="server"></asp:Label>
<asp:Label ID="employeesK" class="invisible" runat="server"></asp:Label>
<asp:Label ID="employeesL" class="invisible" runat="server"></asp:Label>
<asp:Label ID="employeesM" class="invisible" runat="server"></asp:Label>
<asp:Label ID="employeesN" class="invisible" runat="server"></asp:Label>
<asp:Label ID="employeesO" class="invisible" runat="server"></asp:Label>
<asp:Label ID="employeesP" class="invisible" runat="server"></asp:Label>
<asp:Label ID="employeesQ" class="invisible" runat="server"></asp:Label>
<asp:Label ID="employeesR" class="invisible" runat="server"></asp:Label>
<asp:Label ID="employeesS" class="invisible" runat="server"></asp:Label>
<asp:Label ID="employeesT" class="invisible" runat="server"></asp:Label>
<asp:Label ID="employeesU" class="invisible" runat="server"></asp:Label>
<asp:Label ID="employeesV" class="invisible" runat="server"></asp:Label>
<asp:Label ID="employeesW" class="invisible" runat="server"></asp:Label>
<asp:Label ID="employeesX" class="invisible" runat="server"></asp:Label>
<asp:Label ID="employeesY" class="invisible" runat="server"></asp:Label>
<asp:Label ID="employeesZ" class="invisible" runat="server"></asp:Label>

<asp:Label ID="uniqueID" class="invisible" runat="server"></asp:Label>

<!--These fields are required-->
<div class="flex">
    <div class="block">
        <asp:Label class="label large" runat="server">Name (Last, First) <span class="required" title="required">*</span></asp:Label>
        <br />
        <asp:TextBox ID="name" class="text large" runat="server"></asp:TextBox>
        <br />
        <asp:RequiredFieldValidator ID="rfvName" class="validator large noborder" ControlToValidate="name" InitialValue="" ErrorMessage="Please enter a name" runat="server"></asp:RequiredFieldValidator>
    </div>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <div class="block">
        <asp:Label class="label large" runat="server">Date of Request <span class="required" title="required">*</span></asp:Label>
        <br />
        <asp:TextBox ID="dateOfRequest" class="text date large" runat="server"></asp:TextBox>
        <br />
        <asp:RequiredFieldValidator ID="rfvDateOfRequest" class="validator large noborder" ControlToValidate="dateOfRequest" InitialValue="" ErrorMessage="Please enter a date" runat="server"></asp:RequiredFieldValidator>
    </div>
</div>

<br />
<br />

<div class="flex">
    <div class="block">
        <asp:Label class="label large" runat="server">Department <span class="required" title="required">*</span></asp:Label>
        <br />
        <asp:TextBox ID="department" class="text large" runat="server"></asp:TextBox>
        <br />
        <asp:RequiredFieldValidator ID="rfvDepartment" class="validator" ControlToValidate="department" InitialValue="" ErrorMessage="Please enter a department" runat="server"></asp:RequiredFieldValidator>
    </div>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <div class="block">
        <asp:Label class="label large" runat="server">Business Unit <span class="required" title="required">*</span></asp:Label>
        <br />
        <asp:TextBox ID="businessUnit" class="text large" runat="server"></asp:TextBox>
        <br />
        <asp:RequiredFieldValidator ID="rfvBusinessUnit" class="validator large noborder" ControlToValidate="businessUnit" InitialValue="" ErrorMessage="Please enter a business unit" runat="server"></asp:RequiredFieldValidator>
    </div>
</div>

<br />
<br />
<!--Of these two fields, only one of them is required depending on what type of employee this is.-->
<div class="flex">
    <div class="block">
        <asp:Label class="label large" runat="server">Consultant</asp:Label>&nbsp;
        <asp:CheckBox ID="isConsultant" Text="Yes (Valid to…)" class="small" Checked="false" runat="server" />
        <br />
        <asp:TextBox ID="consultantDate" class="date text large" runat="server"></asp:TextBox>
        <br />
        <asp:CustomValidator ID="cvConsultant" class="validator large noborder" ErrorMessage="Please enter a consultant date" runat="server" onservervalidate="cvConsultant_ServerValidate"></asp:CustomValidator>
    </div>

    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    
    <div class="block">
        <asp:Label class="label large" runat="server">Location <span class="required" title="required">*</span></asp:Label>
        <br />
        <asp:TextBox ID="location" class="text large" runat="server"></asp:TextBox>
        <br />
        <asp:RequiredFieldValidator ID="rfvLocation" class="validator large noborder" ControlToValidate="location" InitialValue="" ErrorMessage="Please enter a location" runat="server"></asp:RequiredFieldValidator>
    </div>
</div>

<br />
<br />

<asp:Label class="label large" runat="server">New Employee</asp:Label><br />
    <label for="ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_isReplacement" class="small"><input type="radio" name="yesNo" ID="isReplacement" value="Yes" runat="server" />Yes (Replacement for…)</label>
    <label for="ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_isNotReplacement" class="small"><input type="radio" name="yesNo" ID="isNotReplacement" Value="No" runat="server" />No</label>
    <asp:Label ID="replacement" class="invisible" runat="server"></asp:Label>
<br />
<asp:TextBox ID="replacedEmployee" class="dropdown text large" runat="server"></asp:TextBox>
<br />
<asp:CustomValidator ID="cvReplacedEmployee" class="validator large noborder" ErrorMessage="Please enter an employee name" runat="server" onservervalidate="cvReplacedEmployee_ServerValidate"></asp:CustomValidator>
<br />
<br />
<br />
<h2 class="subheader">Sales Area Information <em>(Required for all Sales Related Requests—APO, BW, B2B)</em></h2>
<br />
<div class="flex">
    <div class="block">
        <asp:Label class="label large" runat="server">Sales Organization</asp:Label>
        <br />
        <asp:TextBox ID="salesOrg" class="text large" runat="server"></asp:TextBox>
        <br />
        <br />
    </div>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <div class="block">
        <asp:Label class="label large" runat="server">Sales Group</asp:Label>
        <br />
        <asp:TextBox ID="salesGroup" class="text large" runat="server"></asp:TextBox>
        <br />
        <br />
    </div>
</div>
<br />
<br />
<div class="flex">
    <div class="block">
        <asp:Label class="label large" runat="server">Sales Division</asp:Label>
        <br />
        <asp:TextBox ID="salesDivision" class="text large" runat="server"></asp:TextBox>
        <br />
        <br />
    </div>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <div class="block">
        <asp:Label class="label large" runat="server">Sales Office</asp:Label>
        <br />
        <asp:TextBox ID="salesOffice" class="text large" runat="server"></asp:TextBox>
        <br />
        <br />
    </div>
</div>
<br />
<br />
<div class="flex">
    <div class="block">
        <asp:Label class="label large" runat="server">Sales District</asp:Label>
        <br />
        <asp:TextBox ID="salesDistrict" class="text large" runat="server"></asp:TextBox>
        <br />
        <br />
    </div>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <div class="block">
        <asp:Label class="label large" runat="server">Rep Code</asp:Label>
        <br />
        <asp:TextBox ID="repCode" class="text large" runat="server"></asp:TextBox>
        <br />
        <br />
    </div>
</div>
<br />
<br />
<div class="flex">
    <div class="block">
        <asp:Label class="label large" runat="server">Request By</asp:Label>
        <br />
        <asp:TextBox ID="requestBy" class="text large" runat="server"></asp:TextBox>
        <br />
        <br />
    </div>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <div class="block">
        <asp:Label class="label large" runat="server">Phone</asp:Label>
        <br />
        <asp:TextBox ID="phone" class="phone text large" runat="server"></asp:TextBox>
        <br />
        <br />
    </div>
</div>
<br />
<br />
<br />
<h2 class="subheader">System Information</h2>
<br />
<h3 class="subheader">SAP ECC</h3>
<div class="flex">
    <div class="block">
        <asp:Label class="label large" runat="server">Copy profile of an existing user (only if identical)</asp:Label>
        <br />
        <asp:TextBox ID="eccUser" class="dropdown text large" runat="server"></asp:TextBox>
    </div>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <div class="block">
        <asp:Label class="label large" runat="server">New or Change—Additional Roles</asp:Label>
        <br />
        <asp:TextBox ID="eccAdditional" TextMode="MultiLine" class="text large" runat="server"></asp:TextBox>
    </div>
</div>
<br />
<h3 class="subheader">SAP APO</h3>
<div class="flex">
    <div class="block">
        <asp:Label class="label large" runat="server">Planning Book(s)</asp:Label>
        <br />
        <asp:TextBox ID="planningBooks" class="text large" runat="server"></asp:TextBox>
    </div>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <div class="block">
        <asp:Label class="label large" runat="server">Product Department(s)</asp:Label>
        <br />
        <asp:TextBox ID="productDepartments" TextMode="MultiLine" class="text large" runat="server"></asp:TextBox>
    </div>
</div>
<br />
<h3 class="subheader">SAP BW</h3>
<div class="flex">
    <div class="block">
        <asp:Label class="label large" runat="server">Copy profile of an existing user (only if identical)</asp:Label>
        <br />
        <asp:TextBox ID="bwUser" class="dropdown text large" runat="server"></asp:TextBox>
    </div>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <div class="block">
        <asp:Label class="label large" runat="server">New or Change—Additional Roles</asp:Label>
        <br />
        <asp:TextBox ID="bwAdditional" TextMode="MultiLine" class="text large" runat="server"></asp:TextBox>
    </div>
</div>
<br />
<h3 class="subheader">SAP CRM</h3>
<div class="flex">
    <div class="block">
        <asp:Label class="label large" runat="server">Copy profile of an existing user (only if identical)</asp:Label>
        <br />
        <asp:TextBox ID="crmUser" class="dropdown text large" runat="server"></asp:TextBox>
    </div>

    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    
    <div class="block">
        <asp:Label class="label large" runat="server">New or Change—Additional Roles</asp:Label>
        <br />
        <asp:TextBox ID="crmAdditional" TextMode="MultiLine" class="text large" runat="server"></asp:TextBox>
    </div>
</div>

<br />

<h3 class="subheader">PayPal</h3>
    <label class="small" for="ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_view"><input type="radio" ID="view" runat="server" name="paypal" text="View" />View</label>
    <label class="small" for="ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_limited"><input type="radio" ID="limited" runat="server" name="paypal" text="Limited Access" />Limited Access</label>
    <label class="small" for="ctl00_ctl23_g_1448646c_d7e0_4276_b4a3_af45e5626eb1_ctl00_full"><input type="radio" ID="full" runat="server" name="paypal" text="Full Access"  />Full Access</label>
    <asp:Label class="invisible" ID="paypalAccess" runat="server"></asp:Label>

<br />

<h3 class="subheader">Sharp B2B</h3>
<div class="flex">
    <div class="block">
        <asp:Label class="label large" runat="server">Copy profile of an existing user (only if identical)</asp:Label>
        <br />
        <asp:TextBox ID="b2bUser" class="dropdown text large" runat="server"></asp:TextBox>
    </div>

    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    
    <div class="block">
        <asp:Label class="label large" runat="server">New or Change—Additional Roles</asp:Label>
        <br />
        <asp:TextBox ID="b2bAdditional" class="text large" TextMode="MultiLine" runat="server"></asp:TextBox>
    </div>
</div>

<br />

<h3 class="subheader">Hyperion</h3>

<div class="flex">
    <div class="block">
        <asp:Label class="label large" runat="server">Copy profile of an existing user (only if identical)</asp:Label>
        <br />
        <asp:TextBox ID="hyperionUser" class="dropdown text large" runat="server"></asp:TextBox>
    </div>

    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    
    <div class="block">
        <asp:Label class="label large" runat="server">New or Change—Additional Roles</asp:Label>
        <br />
        <asp:TextBox ID="hyperionAdditional" TextMode="MultiLine" class="text large" runat="server"></asp:TextBox>
    </div>
</div>

<br />

<h3 class="subheader">Delta Paymetrics</h3>

<div class="flex">
    <div class="block">
        <asp:Label class="label large" runat="server">Copy profile of an existing user (only if identical)</asp:Label>
        <br />
        <asp:TextBox ID="deltaUser" class="dropdown text large" runat="server"></asp:TextBox>
    </div>

    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    
    <div class="block">
        <asp:Label class="label large" runat="server">New or Change—Additional Roles</asp:Label>
        <br />
        <asp:TextBox ID="deltaAdditional" TextMode="MultiLine" class="text large" runat="server"></asp:TextBox>
    </div>
</div>

<br />

<h3 class="subheader">NextGen</h3>

<div class="flex">
    <div class="block">
        <asp:Label class="label large" runat="server">Copy profile of an existing user (only if identical)</asp:Label>
        <br />
        <asp:TextBox ID="nextGenUser" class="dropdown text large" runat="server"></asp:TextBox>
    </div>

    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    
    <div class="block">
        <asp:Label class="label large" runat="server">New or Change—Additional Roles</asp:Label>
        <br />
        <asp:TextBox ID="nextGenAdditional" TextMode="MultiLine" class="text large" runat="server"></asp:TextBox>
    </div>
</div>

<br />

<h3 class="subheader">GRC Control Reporting</h3>

<div class="flex">
    <div class="block">
        <asp:Label class="label large" runat="server">Copy profile of an existing user (only if identical)</asp:Label>
        <br />
        <asp:TextBox ID="grcUser" class="dropdown text large" runat="server"></asp:TextBox>
    </div>

    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    
    <div class="block">
        <asp:Label class="label large" runat="server">New or Change—Additional Roles</asp:Label>
        <br />
        <asp:TextBox ID="grcAdditional" TextMode="MultiLine" class="text large" runat="server"></asp:TextBox>
    </div>
</div>

<br />
<br />
<br />

<!--I marked these fields as required.-->
<h2 class="subheader">Approvers</h2>

<br />

<div class="flex">
    <div class="block">
        <asp:Label class="label large" runat="server">Department Head (Route to the proper data owner) <span class="required" title="required">*</span></asp:Label>
        <br />
        <asp:TextBox ID="departmentHead" class="dropdown text large" runat="server"></asp:TextBox>
        <br />
        <asp:RequiredFieldValidator ID="rfvDepartmentHead" class="validator large noborder" runat="server" ControlToValidate="departmentHead" ErrorMessage="Please enter a department head" InitialValue=""></asp:RequiredFieldValidator>
    </div>

    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    
    <div class="block">
        <asp:Label class="label large" runat="server">Date <span class="required" title="required">*</span></asp:Label>
        <br />
        <asp:TextBox ID="departmentHeadDate" class="date text large" runat="server"></asp:TextBox>
        <br />
        <asp:RequiredFieldValidator ID="rfvDepartmentHeadDate" class="validator large noborder" runat="server" ControlToValidate="departmentHeadDate" ErrorMessage="Please enter a date" InitialValue=""></asp:RequiredFieldValidator>
    </div>
</div>

<br />
<br />

<div class="flex">
    <div class="block">
        <asp:Label class="label large" runat="server">Data Owner (Business Process Owner, Sub-owner) <span class="required" title="required">*</span></asp:Label>
        <br />
        <asp:TextBox ID="dataOwner" class="dropdown text large" runat="server"></asp:TextBox>
        <br />
        <asp:RequiredFieldValidator ID="rfvDataOwner" class="validator large noborder" runat="server" ControlToValidate="dataOwner" ErrorMessage="Please enter a data owner" InitialValue=""></asp:RequiredFieldValidator>
    </div>

    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    
    <div class="block">
        <asp:Label class="label large" runat="server">Date <span class="required" title="required">*</span></asp:Label>
        <br />
        <asp:TextBox ID="dataOwnerDate" class="date text large" runat="server"></asp:TextBox>
        <br />
        <asp:RequiredFieldValidator ID="rfvDataOwnerDate" class="validator large noborder" runat="server" ControlToValidate="dataOwnerDate" ErrorMessage="Please enter a date" InitialValue=""></asp:RequiredFieldValidator>
    </div>
</div>
<br />
<br />
<div class="flex">
    <div class="block">
        <asp:Label class="label large" runat="server">SAP Security Admin <span class="required" title="required">*</span></asp:Label>
        <br />
        <asp:TextBox ID="securityAdmin" class="dropdown text large" runat="server"></asp:TextBox>
        <br />
        <asp:RequiredFieldValidator ID="rfvSecurityAdmin" runat="server" ControlToValidate="securityAdmin" ErrorMessage="Please enter an SAP Security Admin" InitialValue="" class="validator large noborder"></asp:RequiredFieldValidator>
    </div>

    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    
    <div class="block">
        <asp:Label class="label large" runat="server">Date <span class="required" title="required">*</span></asp:Label>
        <br />
        <asp:TextBox ID="securityAdminDate" class="date text large" runat="server"></asp:TextBox>
        <br />
        <asp:RequiredFieldValidator ID="rfvSecurityAdminDate" class="validator large noborder" runat="server" ControlToValidate="securityAdminDate" ErrorMessage="Please enter a date" InitialValue=""></asp:RequiredFieldValidator>
    </div>
</div>
<br />
<br />
<div class="label large">

<br />

<p><strong>WARNING</strong>: The Internal SAP Security Authorization Form is used to provide access to sensitive company information. Please consider carefully the following points:</p>

<ul>
    <li>Sensitivity of information and applications involved (data classification)</li>
    <li>Policies for information protection and dissemination (legal, regulatory, internal policies and customer requirements)</li>
    <li>Roles and responsibilities as defined within the enterprise</li>
    <li>The “need-to-have” access rights associated with the functions performed</li>
    <li>Standard but individual user access profiles for common job roles in the organization</li>
    <li>Requirements to guarantee appropriate segregation of duties (CRITICAL)</li>
</ul>

</div>
<br />
<asp:Label ID="errors" runat="server" class="error large"></asp:Label>
<br />
<asp:Button ID="submit" Text="Submit" class="button large noborder" runat="server" onclick="submit_Click" />
</ContentTemplate>
</asp:UpdatePanel>