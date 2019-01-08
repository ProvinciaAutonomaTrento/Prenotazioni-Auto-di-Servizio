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

<%@ Page Title="Attrezzi operatore" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="menu.aspx.cs" Inherits="menu" %>

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
    .auto-style1 {
        text-align: center;
        font-size: 22px;
    }
    .menu { width: 1024px; margin: 0px auto; background-color: lightsalmon; border-radius: 6px; box-sizing:border-box; padding:3px; z-index: -22; height: 28px;}
    .contenitoresx {
        border-width: 4px; 
        border-color: orange;
        border-style: solid; 
        border-radius: 6px; 
        background-color:cornsilk; 
        width: 365px; 
        text-align: left; 
        clear:both;  
        padding: 10px; 
        box-sizing: border-box;
        height: auto;
        float: left;
    }
    .menuprenota {
        border: 4px solid;
        border-color: blue; 
        width: 333px; height: auto; 
        position: relative; 
        background-color: cornsilk; 
        text-align: left; 
        border-radius: 6px; 
        padding: 10px; 
        box-sizing: border-box;
    }
    .menusegreteria {
        border: 4px solid;
        border-color: blue; 
        width: 333px; height: auto; 
        position: relative; 
        background-color: lightblue; 
        text-align: left; 
        border-radius: 6px; 
        padding: 10px; 
        box-sizing: border-box;
    }
    .menugestione {
        border: 4px solid sandybrown; 
        width: 333px; height: auto; 
        position: relative; 
        background-color: cornsilk; 
        text-align: left; 
        border-radius: 6px; 
        padding: 10px; 
        box-sizing: border-box;
    }
    .menuflotte {
        border: 4px solid;
        border-color: cornflowerblue; 
        width: 333px; height: auto; 
        position: relative; 
        background-color: cornsilk; 
        text-align: left; 
        border-radius: 6px; 
        padding: 10px; 
        box-sizing: border-box;
    }
    .menuRegistro {
        border: 4px solid; 
        width: 333px; height: auto; 
        position: relative;
        border-color: green; 
        background-color: lightgreen; 
        text-align: left; 
        border-radius: 6px; 
        padding: 10px; 
        box-sizing: border-box;
    }
    .menudx {
        float: left; 
        margin-left: 30px; 
        width: 120px; 
        text-align: left; 
        border-width: 4px; border-color: deepskyblue; border-style: solid; border-radius: 6px; 
        padding: 10px; 
        box-sizing: border-box;        
    }
    .panelbox { padding: 5px; }
    </style>
<br />
<div class="hrbianca"></div>
<div class="auto-style1" style="height: 22px;">BOX ATTREZZI</div><br /><div class="hrbianca"></div>
<div class="menu">
    <div style="width: 915px; float: left; height:26px; text-align: left; ">
        <asp:Label ID="LBenvenuto" runat="server" Text=" "></asp:Label>
    </div>    
    <asp:Button ID="bUscita" runat="server" Text="Uscita" Width="90px" OnClick="bUscita_Click" Style="float: left; "/>
</div>
<hr class="hrbianca" />    
<div class="hrbianca"></div>
<div class="container" style="background-color:white; border-width: 6px; border-radius: 4px; margin: 0px auto; box-sizing: border-box; ">
    <div class="contenitoresx">
        <asp:Panel ID="pPrenota" runat="server" Visible ="true" CssClass="panelbox">
        <div class="menuprenota">
            <span style="font-size: 20px; ">Prenota</span><br />
            <div style="font-size: 15px;">
                <a href="pre.aspx?new=t">prenota una macchina</a><br />
                <a href="prealtro.aspx">prenota una macchina per conto di un altro utente</a><br />
            </div>
        </div>
        </asp:Panel>
        
        <asp:Panel ID="pServizio" runat="server" Visible ="false" CssClass="panelbox">
        <div class="menusegreteria">
            <span style="font-size: 20px; ">Report segreteria</span><br />
            <div style="font-size: 15px;">
                <a href="segreteria.aspx">report segreteria</a><br />
            </div>
            <div style="font-size: 15px;">
                <a href="registro_flotte.aspx">stampa registro </a><br />
            </div>
        </div>
        </asp:Panel>
        <asp:Panel ID="PRegistro" runat="server" Visible ="false" CssClass="panelbox">
           <div class="menuRegistro">
                <span style="font-size: 20px; ">Registro</span><br />        
                <div style="font-size: 15px;">
                    <a href="Registro.aspx">stampa registro</a><br />
                </div>
           </div>
        </asp:Panel>

        <asp:Panel ID="pGestione" runat="server" Visible ="false" CssClass="panelbox">
            <div class="menugestione">
                <span style="font-size: 20px; ">Gestione</span><br />        
                <div style="font-size: 15px;">
                    <a href="editpre.aspx">prenotazioni</a><br />
                    <a href="schedaveicolo.aspx?p=a">blocca una macchina</a><br />
                    <a href="anagrafica.aspx">abilitazione utenti</a><br />
                </div>
            </div>
        </asp:Panel>
        
        <asp:Panel ID="pFlotta" runat="server" Visible ="false" CssClass="panelbox">
            <div class="menuflotte">
                <span style="font-size: 20px; ">Flotte</span><br />        
                <div style="font-size: 15px;">
                    <a href="schedaetichettegruppo.aspx">etichette di flotta</a><br />
                    <a href="schedagruppo.aspx">associazione veicoli a flotta</a><br />
                    <a href="schedausergruppo.aspx">abilitazione utenti a flotta</a><br />
                </div>
            </div>
        </asp:Panel>
    </div>
    
    <asp:Panel ID="pTabelle" runat="server" Visible ="false" CssClass="panelbox">
        <div class="menudx"  Style="width: 110px;">
            <span style="font-size: 20px; background-color:burlywood;">Tabelle:</span><br />
            <div style="font-size: 15px;">
                <a href="schedaproduttori.aspx">produttori</a><br />
                <a href="schedamodello.aspx">modello</a><br />
                <a href="schedadotazioni.aspx">dotazioni</a><br />
                <a href="schedacambio.aspx">tipo cambio</a><br />
                <a href="schedatrazione.aspx">tipo trazione</a><br />
                <a href="schedagomme.aspx">pneumatici</a><br />
                <a href="schedaclassificazione.aspx">classificazione</a><br />
                <a href="schedaubicazioni.aspx">ubicazioni</a><br />
                <a href="schedaveicolo.aspx">veicoli</a><br />
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pAggiorna" runat="server" Visible ="false"   >
        <div class="menudx" Style="border-color: purple; width: 190px;">
            <span style="font-size: 20px; background-color:burlywood; ">Tabelle:</span><br />
            <div style="font-size: 15px;">
                <a href="ImportaStrutture.aspx">importa strutture</a><br />
                <a href="AggiornaStrutturaUtente.aspx">aggiorna struttura utente</a><br />
            </div>
        </div>
    </asp:Panel>
</div>

<hr class="hrbianca" style="height:6px;" />
<p class="centra-text clearfix">
<asp:Label ID="sStato" runat="server" style="border-width: 1px; border-radius: 4px; Border-Color: blue; width: 1024px; box-sizing: border-box;"></asp:Label>
</p>

</asp:Content>

