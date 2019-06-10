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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="importastrutture.aspx.cs" Inherits="importastrutture" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <style>
     .menu { width: 1024px; margin: 0px auto; background-color: lightsalmon; border-radius: 6px; box-sizing:border-box; padding:3px; z-index: -22; height: 28px;}
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
        height: auto;
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
        .auto-style4 {
            text-align: center;
            font-size: 22px;
        }
    </style>
<br />
<div class="hrbianca"></div>
<div class="auto-style4" style="height: 22px;">IMPORTA STRUTTURE</div><br /><div class="hrbianca"></div>
<div class="menu">
    <div style="width: 420px; float: left; height:26px; text-align: left; ">
        <asp:Label ID="LBenvenuto" runat="server" Text=" "></asp:Label>
    </div> 
    <div style="text-align: right; width: 185px; float: left;">
        <asp:Button ID="bHasha" runat="server" Text="Codifica password" OnClick="bhasha_Click"  style="float: left; width: 180px;" />
    </div>  
    <div style="text-align: right; width: 185px; float: left;">
        <asp:Button ID="bImport" runat="server" Text="Importa strutture provinciali" OnClick="bImport_Click"  style="float: left; width: 180px;" />
    </div>       
    <div style="text-align: right; width: 185px; float: left;">
        <asp:Button ID="bimportUser" runat="server" Text="Importa utenti provinciali" OnClick="bImportuser_Click"  style="float: left; width: 180px;" />
    </div>  
    <a href="menu.aspx?l=ubicazioni"><img src="img/home.png" alt="home page" /></a>
</div>
<div class="hrbianca"></div>
<div style="clear:both; float:left; margin: 50px; text-align: left;"> 
    Indicare il percorso e il nome del file da importare<br />
    <asp:Label runat="server" width="200px">url del file .csv da importare</asp:Label><asp:TextBox id="tPercorso" runat="server" Width="320px" ></asp:TextBox><br />
    <%--<asp:Label runat="server" width="200px">Data Source</asp:Label><asp:TextBox id="tData" runat="server" Width="320px"  ></asp:TextBox><br />
    <asp:Label runat="server" width="200px">Tabella</asp:Label><asp:TextBox id="tTabella" runat="server" Width="320px"  ></asp:TextBox><br />
    <asp:Label runat="server" width="200px">Username</asp:Label><asp:TextBox id="tUser" runat="server" Width="320px"  ></asp:TextBox><br />
    <asp:Label runat="server" width="200px">Password</asp:Label><asp:TextBox id="tPassword" runat="server" Width="320px"  ></asp:TextBox> --%>
</div>
<p class="centra-text clearfix">
<asp:Label ID="tStato" runat="server" style="clear: both; border-width: 1px; border-radius: 4px; Border-Color: blue; width: 1024px; box-sizing: border-box;"></asp:Label>
</p>
</asp:Content>
