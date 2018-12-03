<%@ Page Title="Atrezzi operatore" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="aiutoguidarifornimento.aspx.cs" Inherits="aiutoguidarifornimento" %>


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
        border-color: limegreen; 
        width: 333px; height: auto; 
        position: relative; 
        background-color: cornsilk; 
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
        width: 372px; 
        background-color:cornsilk;
        text-align: left; 
        border-width: 4px; border-color: deepskyblue; border-style: solid; border-radius: 6px; 
        padding: 10px; 
        box-sizing: border-box;        
    }
    .menudxuso {
        width: 333px; 
        text-align: left; 
        border-width: 4px; border-color: lightpink; border-style: solid; border-radius: 6px; 
        padding: 10px; 
        box-sizing: border-box;        
    }
    .menudxcarta {
        width: 333px; 
        text-align: left; 
        border-width: 4px; border-color: lightblue; border-style: solid; border-radius: 6px; 
        padding: 10px; 
        box-sizing: border-box;        
    }
    .menudxnorma {
        width: 333px; 
        text-align: left; 
        border-width: 4px; border-color: Highlight; border-style: solid; border-radius: 6px; 
        padding: 10px; 
        box-sizing: border-box;        
    }
    .panelbox { padding: 5px; }
    </style>
<br />
<div class="hrbianca"></div>
<div class="auto-style1" style="height: 22px;">SUPPORTO INFORMATIVO/FORMATIVO,&nbsp; ALLA GUIDA,&nbsp; AL RIFORNIMENTO, ECC...</div><br /><div class="hrbianca"></div>
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
            <span style="font-size: 20px; background-color:burlywood;">La registrazione</span><br />
            <span style="font-size: 15px;"><a href = "data/Registrazione.pdf" target="_blank">Registrazione nuovo utente</a></span><br />
        </div>
        </asp:Panel>

        <asp:Panel ID="PRegistro" runat="server" Visible ="true" CssClass="panelbox">
        <div class="menuRegistro">
            <span style="font-size: 20px; background-color:burlywood;">Prenotare un autoveicolo</span><br />
            <span style="font-size: 15px;"><a href = "data/Prenotazione.pdf" target="_blank">Prenotare un autoveicolo</a></span>
        </div>
        </asp:Panel>

        <asp:Panel ID="pGestione" runat="server" Visible ="true" CssClass="panelbox">
         <div class="menugestione">
            <span style="font-size: 20px;">Come fare rifornimento</span><br />
            <span style="font-size: 15px;">
            <a href = "https://www.youtube.com/watch?v=yWF0gUAL7Io" target="_blank">Veicoli elettrici</a><br />
            <a href = "https://www.youtube.com/watch?v=DG1aOTm9Igk" target="_blank">Veicoli ibridi - benzina/elettrici</a><br />
            <a href = "http://www.omniauto.it/magazine/22087/come-si-fa-il-pieno-di-gpl-gas-auto" target="_blank">Veicoli ibridi - benzina/GPL</a><br />
            </span>
        </div>
        </asp:Panel>
    </div>
    <div id="Menudx" class="menudx">
        <asp:Panel ID="pTabelle" runat="server" Visible ="true" CssClass="panelbox">
        <div class="menudxuso">
            <span style="font-size: 20px; background-color:burlywood;">Uso veicoli</span><br />
            <div style="font-size: 15px;">
                <a href = "http://motori.quotidiano.net/comefare/guidare-auto-cambio-automatico.htm" target="_blank">cambio automatico</a><br />
                <a href = "" target="_blank">veicoli totalemnte elettrici (in allestimento)</a><br />
            </div>
        </div>
        </asp:Panel>
        <asp:Panel ID="pCarta" runat="server" Visible ="true" CssClass="panelbox">
        <div class="menudxcarta">
            <span style="font-size: 20px; background-color:burlywood;">Carta carburante</span><br />
            <div style="font-size: 15px;">
                <a href = "data/fuelcard.pdf" target="_blank">Come fare rifornimento con le carte carburante</a><br />
            </div>
        </div>
        </asp:Panel>
        <asp:Panel ID="pDisciplinare" runat="server" Visible ="true" CssClass="panelbox">
        <div class="menudxnorma">
            <span style="font-size: 20px; background-color:burlywood;">Normativa</span><br />
            <div style="font-size: 15px;">
                <a href = "data/disciplinare_delibera 170_del_01122017.pdf" target="_blank">disciplinare</a><br />
                <a href = "http://www.aci.it/i-servizi/normative/codice-della-strada.html" target="_blank">codice della strada</a><br />
            </div>
        </div>
        </asp:Panel>
    </div>
</div>

<hr class="hrbianca" style="height:6px;" />
<p class="centra-text clearfix">
<asp:TextBox ID="sStato" runat="server" style="border-width: 1px; border-radius: 4px; Border-Color: blue; width: 1024px; box-sizing: border-box;"></asp:TextBox>
</p>

</asp:Content>

