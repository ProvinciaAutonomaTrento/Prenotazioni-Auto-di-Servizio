<%--
 /*
 * Copyright(C) 2017 Provincia Autonoma di Trento
 *
 * This file is part of<nome applicativo>.
 * Pitre is free software: you can redistribute it and/or modify
 * it under the terms of the LGPL as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Pitre is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the LGPL v. 3
 * along with Pitre.If not, see<https://www.gnu.org/licenses/lgpl.html>.
 * 
 */ --%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="schedatrazione.aspx.cs" Inherits="schedatrazione" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <style>
    .sottoadx {
        position: absolute;
        right: 0px;
        top: 1px;
        max-width: 100%;               
        background-repeat: repeat-x;
        z-index: -2;
    }
    .dentroadx {
        position: absolute;
        top: 11px;
        right: 10px;
        width: 171px;
        height: 78px;
        border: none;
    }
    .adx {
        position: absolute;
        top: 0px;
        right: 0px;
        width: 171px;
        height: 78px;
    }
    .centra-text {
        text-align: center;
    }
    .riempidietro {
        max-width: 100%;
        height: auto;
        z-index: -1;
    }
    .auto-testata {
        max-width: 100%;
        height: 121px;
        border: none;
        z-index: -1;
    }
    .centra {
        margin: auto;
    }
    .hrsalmone {
        Width: 1024px; margin: auto; background-color: lightsalmon; height: 6px;
    }
    .hrbianca {
        Width: 1024px; margin: auto; border: 2px; background-color: white; height: 8px;
        clear: both;
    }
    .content {
        width: 1024px;
        margin: 0px auto;
        text-align: center;
    }
    .container {
        position: relative;
        width: 1024px;
        margin: 0px auto;
        text-align: center;
        padding: 5px;
        box-sizing: border-box;
    }
    .containerdx {
        position: absolute;
        width: 512px;
        left: 512px;
        height: 25px;
        float: left;
    }

    .asx {
        float: left;
        width : 508px;
        box-sizing: border-box;
        background-color: lightblue; color: black; text-align: center;        
        height: 22px;
        margin: 2px;
        border-radius: 4px;
    }
    .adx {
        float: right;        
        width : 508px;
        box-sizing: border-box;
        background-color: lightblue; color: black; text-align: center;
        padding: 1px;
        height: 22px;
        margin: 2px;
        border-radius: 4px;
    }
    .clearfix {
        content: "";
        clear: both;
        display: table;
    }
    .boxverde {
        background-color: lightblue;
        width: 1024px;
        border-radius: 4px;
    }
    .tb {
        border-width: 1px; border-radius: 4px; Border-Color: blue;  box-sizing: border-box; text-align: left; padding: 1px; margin: 2px; float:left;
    }
        </style>
<br />
<div class="hrbianca"></div>
<div class="centra-text" style="font-size: 22px; height: 22px;">SCHEDA TRAZIONE</div><br /><div class="hrbianca"></div>
<hr class="hrsalmone clearfix" /><hr class="hrbianca" />
<div class="hrbianca"></div>
<div class="container" style="background-color:cornsilk; border-radius: 4px; height:auto;">
    <div style="clear:both;  width: 200px;text-align: left; float: left;">Seleziona trzione</div>
    <div style="width: 370px; text-align: left; float: left;">
        <asp:DropDownList ID="ddl" runat="server" AutoPostBack="true" Width="350px" OnSelectedIndexChanged="ddl_SelectedIndexChanged" style="float: left;"></asp:DropDownList>
    </div>
    <div style="text-align: right; width: 80px; float: left;">
        <asp:Button ID="bDel" runat="server" Text="Cancella" OnClick="bDel_Click" style="float: left; width: 70px;" />
    </div>
    <div style="text-align: right; width: 100px; float: left;">
        <asp:Button ID="bSalva" runat="server" Text="Salva dati" OnClick="bSalva_Click"  style="float: left;" />
    </div>
    <div style="text-align: right; width: 200px; float: left;">
        <asp:Button ID="bInsert" runat="server" Text="Aggiungi nuovo tipo traz." OnClick="bInsert_Click"  style="float: left; width: 180px;" />
    </div>
    <a href="menu.aspx?l=dotazioni"><img src="img/home.png" alt="home page"/></a>
    <div style="clear: both;"> </div>
    <p></p>
    <div style="text-align: left; width: 200px; float:left;">Tipo trazione</div>
    <div style="width: 370px; float: left;">
        <asp:TextBox ID="tTesto" runat="server" CssClass="tb" style="width: 350px;"></asp:TextBox>
    </div>
    <div  style="text-align: left; width: 110px; float:left;">Abilitata</div>
    <div style="text-align: left; width: 300px; float: left;">
        <asp:CheckBox ID="cbAbilitata" runat="server" CssClass="tb" style="width: 250px;"/>
    </div>
    <div style="clear: both; width: 1024px;  margin: 0px auto; ">
        <div style="text-align: left; width: 200px; float:left;">Sicurezza</div>
        <div style="width:80px; float: left; text-align: left;">
            <asp:DropDownList ID="ddlSicurezza" AutoPostBack="false" runat="server" width="60px" style="text-align: left; margin-left: 0px;"></asp:DropDownList>
        </div>
        <div style="float: left;">    (numeri alti = maggior sicurezza)</div>
    </div>
</div>
<hr class="hrbianca" style="height:216px;" />
<p class="centra-text clearfix">
<asp:TextBox ID="sStato" runat="server" style="border-width: 1px; border-radius: 4px; Border-Color: blue; width: 1024px; box-sizing: border-box;"></asp:TextBox>
</p>

</asp:Content>

