<%--
 * 
 * Copyright (C) 2017 Provincia Autonoma di Trento
 *
 * This file is part of <nome applicativo>.
 * Pitre is free software: you can redistribute it and/or modify
 * it under the terms of the LGPL as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Pitre is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the LGPL v. 3
 * along with Pitre. If not, see <https://www.gnu.org/licenses/lgpl.html>.
 * 
 */ --%>

<%@ Page Title="GFP: prenotazione" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="pre.aspx.cs" Inherits="prenota" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<script>
  function preventBack(){window.history.forward();}
  setTimeout("preventBack()", 0);
  window.onunload=function(){null};
</script>
<style>
    .clb { clear: both; }
    .fl { float: left; }
    .titolo {  margin: 0px auto; font-size: 22px; text-align: center; padding: 2px; box-sizing: border-box; }
    .titolouno { width: 1014px; text-align: center; margin: 0px auto; padding: 2px; box-sizing: border-box; background-color: lightblue;}
    .hrsalmone { width: 1024px; margin: auto; background-color: lightsalmon; height: 6px; box-sizing:border-box; border-radius: 3px; }
    .hrbianca { background-color: white; height: 6px; }
    .menu { width: 1024px; margin: 0px auto; background-color: lightsalmon; border-radius: 6px; box-sizing:border-box; padding:3px; height: 28px; }
    .nomecampo { padding-right: 10px; text-align: right;  float: left; }
    .auto-style1 {
        margin-bottom: 0px;
    }
    .topindex {
        width: 110px;
        float: left;
    }
    .news {
        width: 1024px;
        height: auto;
        margin: 0px auto;
        color: white;
        background-color: cornflowerblue;
        padding: 6px;
        border-radius: 8px;
    }
</style>
<br />
<div class="clb"></div>
<div class="titolo">PRENOTAZIONE AUTOMEZZI</div>
<div class="news">NEWS: ELENCO COLONNINE DI RICARICA ELETTRICA: <a href = "data/PrimieroeTrentiino.pdf" target="_blank">QUI L&#39;ELENCO DELLE STAZIONI</a ></div>
<div style="width: 1024px;  margin: 0px auto; ">
<div style="text-align: left; width: 100px; float: left;">
    <a href = "data/20181204_Prenotazione.pdf" target="_blank">Istruzioni</a ></div>
<div class="topindex">
    <a href = "aiutoguidarifornimento.aspx" target="_blank">Guide</a ></div>
<div class="topindex">
    <a href = "https://www.meteotrentino.it/#!/home" target="_blank">Meteo Trentino</a ></div>
<div style="width: 150px; float: left;">
    <a href = "http://www.viaggiareintrentino.it/it/HomeNew/Contenuti-principali/Consulta-Mappa" target="_blank">Viaggiare in Trentino</a ></div>
</div>
<div class="clb"></div>
<div class="menu"> 
        <div style="width: 544px; float: left; height:26px;">
            <asp:Label ID="LBenvenuto" runat="server" Text=" "></asp:Label>
        </div>
        <div style="width: 70px; float: left;"><asp:Button ID="Nuova" runat="server" Text="Nuova" style="width: 60px;" OnClick="Nuova_Click" ToolTip="Per azzerare i valori inseriti" /></div>
        <div style="width: 140px; float: left;"><asp:Button ID="bmieprenotazioni" runat="server" Text="Le mie prenotazioni" style="width: 130px;" OnClick="bmieprenotazioni_Click" ToolTip="Per vedere l'elenco delle prenotazioni attive e passate con possibilità di cancellare/modificare i dati"/></div>
        <div style="width: 90px; float: left;"><asp:Button ID="bMyData" runat="server" Text="I miei dati" style="width: 80px;" OnClick="bMyData_Click" ToolTip="Passare alla pagina dei miei dati"/></div>
        <div style="width: 80px; float: left;"><asp:Button ID="bhome" runat="server" Text="Home" style="width: 70px;" OnClick="bhome_Click" ToolTip="Ritornare alla home page"/></div>
        <div style="width: 90px; float: left;"><asp:Button ID="buscita" runat="server" Text="Uscita" style="width: 80px;" OnClick="bUscita_Click" ToolTip="Scollegarsi dall'applicazione"/></div>        
</div>
<div class="clb hrbianca"></div>

<asp:Panel ID="PImput" runat="server" Visible ="true">

<div class="titolouno" style="width: 1024px;">DESTINAZIONE</div>
<div class="clb hrbianca"></div>
<div style="width: 1024px;  margin: 0px auto; ">
    <div class="nomecampo" style="width: 100px;">Provincia</div>
    <div style="width:170px; float: left; ">
        <asp:DropDownList ID="ddlProvincia" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged"  width="160px" ToolTip="Scegliere la provincia di destinazione"></asp:DropDownList>
    </div>
    <div  class="nomecampo" style="width:100px;">Comune/loc.</div>
    <div style="width:270px; float: left;">
        <asp:DropDownList ID="ddlComune" runat="server" AutoPostBack="false" width="260px" onchange="gettextclient(this, 'MainContent_COMODO');" Load="gettextclient(this, 'MainContent_COMODO');" ToolTip="Scegliere il comune/località di destinazione. Troverete anche le località limitrofe al capoluogo e quella speciale 'Riservato'"></asp:DropDownList>
    </div>
    <div class="nomecampo" style="width: 110px; float: left;">Paese</div>
    <div style="width: 230px; float: left;">
        <asp:DropDownList ID="ddlStato" AutoPostBack="true" runat="server" Width="220px" OnSelectedIndexChanged="ddlStato_SelectedIndexChanged" ToolTip="Scegliere lo Stato di destinazione"></asp:DropDownList>
    </div>
</div>
<div style="clear: both; width: 1024px; margin: 0px auto;">
<asp:Label ID="lComuneEstero" runat="server" Text="Comune estero" Visible="false" Enabled="false" class="nomecampo" style="clear: both; width: 772px; padding-right: 8px;float: left; text-align:right;"></asp:Label>
<asp:TextBox ID="tComuneEstero" Enabled="false" Visible="false" runat="server" onchange="gettextclient(this, 'MainContent_COMODO');" Style="border-width: 1px; border-radius: 4px; border-color: blue; width: 220px; box-sizing: border-box; float: left;"></asp:TextBox>
<div class="clb hrbianca"></div>
<div style="width: 1024px; margin: 0px auto;background-color: white; padding: 0px;">
    <div class="titolouno" style="width:501px; float: left; padding: 2px;">DATA E ORA DI INIZIO (entro 31 gg. da oggi)</div>
    <div style="width: 14px; float: left; background-color: white; float: left;"><pre></pre></div>
    <div class="titolouno" style="width:501px; float: left; padding: 2px;">DATA E ORA DI FINE&nbsp; (entro 34 gg. da oggi)</div>
</div>
<div class="clb hrbianca"></div>
<div style="width: 1024px; margin: 0px auto;">
    <div class="nomecampo" style="width: 90px;">Data inizio</div>
    <div style="width: 210px; vertical-align: Top; text-align: left; float: left; ">
        <asp:Calendar ID="CldInizio" runat="server" BackColor="White" BorderColor="#999999" Caption="Seleziona giorno inizio" CellPadding="4" DayNameFormat="Shortest" 
            Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" Width="200px" 
            OnSelectionChanged="CldInizio_SelectionChanged" OnDayRender="CldInizio_DayRender"  OnVisibleMonthChanged="CldInizio_VisibleMonth"
            ToolTip="Scegliere il giorno di partenza">
        <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
        <NextPrevStyle VerticalAlign="Bottom" />
        <OtherMonthDayStyle ForeColor="#808080" />
        <SelectedDayStyle BackColor="#003399" Font-Bold="True" ForeColor="White" />
        <SelectorStyle BackColor="#CCCCCC" />
        <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
        <TodayDayStyle BackColor="White" ForeColor="Black" BorderColor="Blue" BorderWidth="2" />
        <WeekendDayStyle BackColor="#FFFFCC" />
        </asp:Calendar>
    </div>
    <div class="nomecampo" style="width: 70px;">ora inizio</div>
    <div style="width: 55px; vertical-align: Top; text-align: right; float: left;  "><asp:DropDownList ID="ddlOrainizio" runat="server" OnSelectedIndexChanged="ddlOraInizio_SelectedIndexChanged" 
        Width="55" AutoPostBack="false" ToolTip="Scegliere l'ora di inizio missione"></asp:DropDownList></div>
    <div style="width: 5px; vertical-align: Top; text-align: center; float: left;  ">:</div>
    <div style="width: 55px; vertical-align: Top; text-align: left; float: left;  "><asp:DropDownList ID="ddlMininizio" runat="server" OnSelectedIndexChanged="ddlMinInizio_SelectedIndexChanged" Width="55" ToolTip="Scegliere i minuti di inizio missione"></asp:DropDownList></div>
    <div style="width: 10px; float:left;"></div>
    <div class="nomecampo" style="width: 90px;">Data fine</div>
    <div style="width: 210px; vertical-align: Top; text-align: left; float: left; ">
        <asp:Calendar ID="CldFine" runat="server" BackColor="White" BorderColor="#999999" Caption="Seleziona giorno fine" CellPadding="4" DayNameFormat="Shortest" 
            Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" Width="200px" 
            OnSelectionChanged="CldFine_SelectionChanged" OnDayRender="CldFine_DayRender" OnVisibleMonthChanged="CldInizio_VisibleMonth"
            ToolTip="Scegliere il giorno di arrivo">
        <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
        <NextPrevStyle VerticalAlign="Bottom" />
        <OtherMonthDayStyle ForeColor="#808080" />
        <SelectedDayStyle BackColor="#0033CC" Font-Bold="True" ForeColor="White" />
        <SelectorStyle BackColor="#CCCCCC" />
        <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
        <TodayDayStyle BackColor="White" ForeColor="Black"  BorderColor="Blue" BorderWidth="2px" />
        <WeekendDayStyle BackColor="#FFFFCC" />
        </asp:Calendar>
    </div>
    <div style="float:left; width:190px;">
        <div class="nomecampo" style="width: 60px; padding-right:5px;">ora fine</div>
        <div style="width: 55px; vertical-align: Top; text-align: right; float: left;  "><asp:DropDownList ID="ddlOrafine" runat="server" OnSelectedIndexChanged="ddlOraInizio_SelectedIndexChanged" Width="55" ToolTip="Scegliere l'ora di fine missione"></asp:DropDownList></div>
        <div style="width: 5px; vertical-align: Top; text-align: center; float: left;  ">:</div>
        <div style="width: 55px; vertical-align: Top; text-align: left; float: left;"><asp:DropDownList ID="ddlMinfine" runat="server" OnSelectedIndexChanged="ddlMinInizio_SelectedIndexChanged" Width="55" ToolTip="Scegliere i minuti di fine missione"></asp:DropDownList></div>
        <asp:Label ID="lZero" runat="server" width="188" visible="true" text="ATTENZIONE: le ore 00:00 indicano l'inizio del giorno!" Style="clear:both; padding:5px; background-color:crimson; color:aliceblue; border-radius:4px;"></asp:Label>
    </div>
</div>
<div class="clb hrbianca"></div><div class="clb hrbianca"></div>
<div style="width: 1024px; margin: 0px auto;background-color: white; padding: 0px;">
    <div class="titolouno" style="width:501px; float: left; padding: 2px;">DOVE RITIRARE IL VEICOLO</div><div style="width: 14px; float: left; background-color: white; float: left; padding: 0px;"><pre> </pre></div>
    <div class="titolouno" style="width:501px; float: left; margin-right: 0px; padding: 2px;">PASSEGGERI</div>
</div>
<div style="clear: both; width: 1024px; margin: 0px auto;">
    <div class="nomecampo" style="width: 90px;">Punto di ritiro</div>
    <div style="width:340px; float: left; ">
        <asp:DropDownList ID="ddlRitiro" AutoPostBack="true" runat="server" width="330px" onchange="gettextclient(this, 'MainContent_COMODO1');" Load="gettextclient(this, 'MainContent_COMODO1');" OnSelectedIndexChanged="ddlRitiro_SelectedIndexChanged" ToolTip="Scegliere il punto di ritiro. E'obbligatorio ritirare e consegnare il veicolo nello stesso punto"></asp:DropDownList>
    </div>
    <div style="width: 70px; float: left;"><pre> </pre></div>
    <div class="nomecampo" style="width: 270px;">Numero viaggiatori oltre al conducente</div>
    <div style="width: 65px; float: left;"><asp:DropDownList ID="ddlPasseggeri" runat="server" Width="55" ToolTip="Indicare il numero di passegggeri"></asp:DropDownList></div>
</div>
<div class="clb hrbianca"></div>
<div style="width: 1024px; margin: 0px auto;">
    <div class="nomecampo" style="width: 90px;">Motivo </div>
    <div style="width:920px; float: left; ">
        <asp:TextBox ID="tMotivo" AutoPostBack="false" runat="server" width="910px"></asp:TextBox>
    </div>
</div>
<br />
</div>
</asp:Panel>
<div class="clb hrbianca"></div><br />
<div style="width: 1024px; margin: 0px auto; text-align:center">
    <asp:Button ID="bVerifica" runat="server" width="250px" Text="Verifica richiesta" onclick="bVerifica_Click"
        ToolTip="Dopo aver inserito i dati della richiesta, viene verifacata la disponibilità di automezzi idonei per la missione"/>
    <asp:Button ID="bModifica" runat="server" width="250px" Text="Modifica" OnClick="bModifica_Click" visible ="false"
        ToolTip="Modificare i dati della richiesta" Style="right:right;"/>
    <span style="width:10px; float:left;"></span>
    <asp:Button ID="bInfo" runat="server" Text="Visualizza infografica" OnClick="bInfo_Click" 
        ToolTip=" Visualizza informazioni relative alla disponibilità dei veicoli per sede e alla distribuzione delle prenotazioni nel giorno della partenza."/> 
    <%--<asp:linkbutton ID="lHelp" runat="server" text="Help infografica" OnClick="lbHelp_Click" Style="float: right;"></asp:linkbutton>  --%>
    <a href="data\infografica.pdf" target="_blank" Style="float: right;">Help infografica</a>
    <asp:Button ID="bElencoDisponibili" runat="server" Text="Elenco veicoli disponibili in altre sedi" onclick="bElencoDisponibili_Click" Visible="false" 
        ToolTip="Verificare la disponibilità di veicoli idonei eventualmenmte disponibili in sedi diverse da quella selezionata"/>
</div>

<div class="clb hrbianca"></div>

<asp:Panel ID="pFiltri" runat="server" Visible="true">
<div id="filtro" style="clear: both; margin: 0px auto; text-align: center;">
    <asp:CheckBox ID="cbFiltri" runat="server" autopostback="true" OnCheckedChanged="cbFiltriOnOff" Text="Togli filtri ?" Visible="false"/>
</div>
</asp:Panel>
<asp:Panel id="pRiepilogo" runat="server" visible="false" style="width: 1024px; margin: 0px auto; text-align: center;">
    <br />
    <div class="hrbianca"></div>
    <asp:Table ID="tRiepilogo" runat="server" Width="1024px" Visible ="false" HorizontalAlign="Center">
        <asp:TableRow runat="server">
            <asp:TableCell runat="server" Width="10px"></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" HorizontalAlign="Left">
            <asp:TableCell runat="server" Width="10px"></asp:TableCell>
            <asp:TableCell runat="server">Destinazione:</asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"><asp:Label ID="lDestinazione" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" HorizontalAlign="Left">
            <asp:TableCell runat="server" Width="10px"></asp:TableCell>
            <asp:TableCell runat="server">partenza il giorno:</asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"><asp:Label ID="lPartenza" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" HorizontalAlign="Left">
            <asp:TableCell runat="server" Width="10px"></asp:TableCell>
            <asp:TableCell runat="server">rientro il giorno:</asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"><asp:Label ID="lRientro" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" HorizontalAlign="Left">
            <asp:TableCell runat="server" Width="10px"></asp:TableCell>
            <asp:TableCell runat="server">sede ritiro veicolo:</asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"><asp:Label ID="lRitiro" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" HorizontalAlign="Left">
            <asp:TableCell runat="server" Width="10px"></asp:TableCell>
            <asp:TableCell runat="server">viaggiatori oltre al conducente:</asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"><asp:Label ID="lPasseggeri" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" HorizontalAlign="Left" ID="trVeicolo" Visible="false">
            <asp:TableCell runat="server" Width="10px"></asp:TableCell>
            <asp:TableCell runat="server">autoveicolo:</asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"><asp:Label ID="lVeicolo" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" HorizontalAlign="Left" ID="TableRow1" Visible="true">
            <asp:TableCell runat="server" Width="10px"></asp:TableCell>
            <asp:TableCell runat="server">Posteggio numero:</asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"><asp:Label ID="lPosteggio" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Panel>

<asp:Panel ID="pAcconsento" runat="server" Visible="false" style="clear: both; width: 1200px; margin: 0px auto;">
<div class="clb hrbianca"></div>
<div style="width: 1024px; margin: 0px auto; text-align: center;">
    Ho letto e accetto quanto disposto dal disciplinare d'uso dei veicoli messi a disposizione <u><strong>per motivi di servizio</strong></u>
    <div style="width:20px; float:left;"><span> </span></div>
    <asp:CheckBox ID="cbAcconsento" runat="server" tooltip="E' necessario aver letto il disciplinare e accettarne il contenuto"/>
    <div style="width:20px; float:left;"><span>  </span></div>
    <asp:Button ID="bConferma" runat="server" Text="Conferma richiesta" OnClick="bConferma_Click" Visible="false" tooltip="Per confermare la richiesta di prenotazione"/>
    <div style="float: left; width:10px;"><span>  </span></div>
</div>  
</asp:Panel>
<asp:Panel ID="pPrenota" runat="server" visible="false" style="clear: both; width: 1024px; margin: 0px auto;">
    <div class="hrbianca"></div>
    <asp:GridView ID="GW" runat="server" AllowSorting="false" Width="1024px" AutoGenerateColumns="false" visible ="false" 
            DataKeyNames="id" PageSize="999" ShowHeaderWhenEmpty="True" Sortmode="Automatic" 
            style="text-align: left" CellPadding="2" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="GW_SelectedIndexChanged" Font-Size="Small" OnSorting="GW_Sorting" 
            tooltip="Elenco dei veicoli disponibili. Valutare con attenzione il tipo di veicolo, il tipo di cambio, l'alimentazione, la trazione e le gomme montate">
        <Columns>
            <asp:CommandField ButtonType="Button" SelectText="Prenota" ShowSelectButton="True"></asp:CommandField>
            <asp:Boundfield DataField="num." headertext="Num." HeaderStyle-HorizontalAlign="center"><ItemStyle Width="50px" HorizontalAlign="center" Wrap="false"/></asp:Boundfield>
            <asp:Boundfield DataField="marca" headertext="Marca" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="80px" HorizontalAlign="left" Wrap="true"/></asp:Boundfield> 
            <asp:Boundfield DataField="modello" headertext="Modello" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="150px" HorizontalAlign="left" Wrap="true"/></asp:Boundfield> 
            <asp:Boundfield DataField="classificazione" headertext="Tipo" HeaderStyle-HorizontalAlign="center"><ItemStyle Width="200px" HorizontalAlign="left" Wrap="true"/></asp:Boundfield> 
            <asp:Boundfield DataField="posti" headertext="Posti" HeaderStyle-HorizontalAlign="center"><ItemStyle Width="40px" HorizontalAlign="center" Wrap="false"/></asp:Boundfield> 
            <asp:Boundfield DataField="cambio" headertext="Cambio" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="120px" HorizontalAlign="left" Wrap="true"/></asp:Boundfield>  
            <asp:Boundfield DataField="alimentazione" headertext="Alimentazione" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="110px" HorizontalAlign="left" Wrap="true"/></asp:Boundfield> 
            <asp:Boundfield DataField="gomme" headertext="Gomme" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="90px" HorizontalAlign="left" Wrap="true"/></asp:Boundfield> 
            <asp:Boundfield DataField="trazione" headertext="Trazione" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="90px" HorizontalAlign="left" Wrap="true"/></asp:Boundfield> 
            <asp:Boundfield DataField="posteggio" headertext="Posteggio" HeaderStyle-HorizontalAlign="center"><ItemStyle Width="40px" HorizontalAlign="center" Wrap="true"/></asp:Boundfield> 
            <asp:Boundfield DataField="colore" headertext="Colore" HeaderStyle-HorizontalAlign="center"><ItemStyle Width="0px" HorizontalAlign="center" Wrap="false"/></asp:Boundfield> 
            <asp:Boundfield DataField="flotta_ek" headertext="Flotta_ek" HeaderStyle-HorizontalAlign="center"><ItemStyle Width="0px" HorizontalAlign="center" Wrap="false"/></asp:Boundfield> 
        </Columns>
        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BorderStyle="Solid" BackColor="#506C8C" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
</asp:GridView>
</asp:Panel>
<asp:Panel ID="pGWDD" runat="server" visible="false"  style="clear: both; width: 1024px; margin: 0px auto;">
    <hr class="hrbianca" />
    <asp:GridView ID="GWDD" runat="server" AllowSorting="False" Width="1024px"  AutoPostBack="true"
            DataKeyNames="id" PageSize="999" ShowHeaderWhenEmpty="True"
            style="text-align: left" CellPadding="2" ForeColor="#333333" GridLines="None" 
            OnSelectedIndexChanged="GWDD_SelectedIndexChanged" Font-Size="Small"
            tooltip="Selezionare l'autista da contattare per effettuare la missione assieme">
        <Columns>
            <asp:CommandField ButtonType="Button" SelectText="Seleziona" ShowSelectButton="True"></asp:CommandField>
        </Columns>
        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BorderStyle="Solid" BackColor="#506C8C" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" /> 
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
    </asp:GridView>
</asp:Panel>
<asp:Panel ID="pGWPRE" runat="server" visible="false" style="clear: both; width: 1024px; margin: 0px auto;">
<asp:GridView ID="GWPRE" runat="server" AllowSorting="False" Width="1024px" AutoPostBack="true"
            DataKeyNames="id" PageSize="999" ShowHeaderWhenEmpty="True" AutoGenerateColumns="false"
            style="text-align: left" CellPadding="2" ForeColor="#333333" GridLines="None"  Font-Size="Small"
            OnSelectedIndexChanged="GWPRE_SelectedIndexChanged" OnRowDeleting="GWPRE_RowDeleting" OnRowDataBound="GWPRE_RowDataBound"
            tooltip="E' possibile cancellare/modificare la missione. Si raccomanda di cancellare tempestivamente le misisoni annullate e di modificare l'orario di rientro se avvenuto con largo anticipo">
        <Columns>
            <asp:TemplateField HeaderText="MODIFICA" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="70px" HorizontalAlign="left" Wrap="false"/>
                <ItemTemplate>
                    <asp:Button runat="Server" align="left" ButtonType="Button" SelectText="Modifica" ShowSelectButton="True" Text="Modifica" CommandName="select"></asp:Button>
                </ItemTemplate>
            </asp:TemplateField> 
            <asp:TemplateField HeaderText="CANCELLA" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="70px" HorizontalAlign="left" Wrap="false"/>
                <ItemTemplate>
                    <asp:Button runat="Server" align="left" ButtonType="Button" SelectText="Cancella" ShowDeleteButton="True" Text="Cancella" CommandName="delete"></asp:Button>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:Boundfield DataField="partenza" headertext="PARTENZA" HeaderStyle-HorizontalAlign="left" Visible="true" ><ItemStyle Width="130px" HorizontalAlign="left" wrap="false"/></asp:Boundfield>
            <asp:Boundfield DataField="arrivo" headertext="ARRIVO" HeaderStyle-HorizontalAlign="left" Visible="true"><ItemStyle Width="130px" HorizontalAlign="left" wrap="false"/></asp:Boundfield>
            <asp:Boundfield DataField="cap" headertext="CAP" HeaderStyle-HorizontalAlign="center" Visible="true"><ItemStyle Width="60px" HorizontalAlign="center" wrap="false"/></asp:Boundfield>
            <asp:Boundfield DataField="comune" headertext="DESTINAZIONE" HeaderStyle-HorizontalAlign="left" Visible="true"><ItemStyle Width="150px" HorizontalAlign="left" wrap="false"/></asp:Boundfield>
            <asp:Boundfield DataField="posteggio" headertext="POSTEGGIO" HeaderStyle-HorizontalAlign="left" Visible="true"><ItemStyle Width="160px" HorizontalAlign="left" wrap="false"/></asp:Boundfield>            
            <asp:Boundfield DataField="mezzo" headertext="VEICOLO" HeaderStyle-HorizontalAlign="left" Visible="true"><ItemStyle Width="190px" HorizontalAlign="left" wrap="false"/></asp:Boundfield>
            <asp:Boundfield DataField="id" headertext="ID" HeaderStyle-HorizontalAlign="left" Visible="true"><ItemStyle Width="0px" HorizontalAlign="left" wrap="false"/></asp:Boundfield>            
            <asp:Boundfield DataField="colore" headertext="colore" HeaderStyle-HorizontalAlign="left" Visible="true"><ItemStyle Width="0px" HorizontalAlign="left" wrap="false"/></asp:Boundfield>            
        </Columns>
        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BorderStyle="Solid" BackColor="#506C8C" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" /> 
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
</asp:GridView>
</asp:Panel>
<asp:Panel ID="pMezziDisponibili" runat="server" visible = "false" style="clear: both; width: 1024px; margin: 0px auto;">
    <div class="hrbianca"></div>
    <asp:GridView ID="GWMezziDisponibili" runat="server" AllowSorting="false" Width="1024px" AutoGenerateColumns="false"  
            DataKeyNames="id" PageSize="999" ShowHeaderWhenEmpty="True" Sortmode="Automatic" 
            style="text-align: left" CellPadding="2" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="GW_SelectedIndexChanged" 
            OnSorting="GW_Sorting" 
            Font-Size="Small" 
            tooltip="Modificare il punto di ritiro, se si è individuato un automezzo idoneo alla missione">
        <Columns>
            <asp:TemplateField HeaderText="Punto di ritiro" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="320px" HorizontalAlign="left" Wrap="false"/>
                <ItemTemplate>
                    <asp:Label ID="Comune" runat="server" Text='<%# Bind("comune") %>'></asp:Label>
                    <asp:Label ID="ubicazione" runat="server" Text='<%# Bind("ubicazione") %>'></asp:Label>
                    <asp:Label ID="Via" runat="server" Text='<%# Bind("via") %>'></asp:Label>
                    <asp:Label ID="Civico" runat="server" Text='<%# Bind("civico") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField> 
            <asp:Boundfield DataField="num." headertext="num." HeaderStyle-HorizontalAlign="center"><ItemStyle Width="50px" HorizontalAlign="center" Wrap="false"/></asp:Boundfield> 
            <asp:Boundfield DataField="marca" headertext="Marca" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="100px" HorizontalAlign="left" Wrap="false"/></asp:Boundfield> 
            <asp:Boundfield DataField="modello" headertext="Modello" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="160px" HorizontalAlign="left" Wrap="false"/></asp:Boundfield>             
            <asp:Boundfield DataField="posti" headertext="Posti" HeaderStyle-HorizontalAlign="center"><ItemStyle Width="60px" HorizontalAlign="center" Wrap="false"/></asp:Boundfield> 
            <asp:Boundfield DataField="gomme" headertext="Gomme" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="130px" HorizontalAlign="left" Wrap="false"/></asp:Boundfield> 
            <asp:Boundfield DataField="trazione" headertext="Trazione" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="90px" HorizontalAlign="left" Wrap="false"/></asp:Boundfield>
            <asp:Boundfield DataField="colore" headertext="colore" HeaderStyle-HorizontalAlign="left" Visible="true"><ItemStyle Width="0px" HorizontalAlign="left" wrap="false"/></asp:Boundfield>            
        </Columns>
        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BorderStyle="Solid" BackColor="#506C8C" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
</asp:GridView>
</asp:Panel>
<asp:Panel ID="pConferma" runat="server" Visible="false" style="border-color: blue; border-radius: 6px; border: 3px; width: 1024px;margin: 0px auto;text-align:center;">
    <div class="hrbianca"></div>
    <asp:Table ID="tConfermaCancellazione" runat="server" Width="1024px" HorizontalAlign="Center" CssClass="auto-style1">
        <asp:TableRow runat="server">
            <asp:TableCell runat="server" Width="10px"></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" HorizontalAlign="Left">
            <asp:TableCell runat="server" Width="10px"></asp:TableCell>
            <asp:TableCell runat="server">Destinazione:</asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"><asp:Label ID="lccDestinazione" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" HorizontalAlign="Left">
            <asp:TableCell runat="server" Width="10px"></asp:TableCell>
            <asp:TableCell runat="server">partenza il giorno:</asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"><asp:Label ID="lccPartenza" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" HorizontalAlign="Left">
            <asp:TableCell runat="server" Width="10px"></asp:TableCell>
            <asp:TableCell runat="server">rientro il giorno:</asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"><asp:Label ID="lccArrivo" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" HorizontalAlign="Left">
            <asp:TableCell runat="server" Width="10px"></asp:TableCell>
            <asp:TableCell runat="server">sede ritiro veicolo:</asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"><asp:Label ID="lccUbi" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" HorizontalAlign="Left">
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" HorizontalAlign="Left" ID="TableRow3" Visible="true">
            <asp:TableCell runat="server" Width="10px"><br /><br /></asp:TableCell>
            <asp:TableCell runat="server" ColumnSpan="3" HorizontalAlign="Center"><asp:Button ID="cbcConferma" runat="server" Text="Conferma richiesta di cancellazione!" 
                Visible="true" OnClick="cbcCancella_Click" ToolTip="Confermare la richiesta di cancellazione della prenotazione"/></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
    </asp:Table>       
</asp:Panel>
<div class="clb hrbianca"></div>
<p class="clb" style="margin: 0px auto; width: 1200px; height: 25px;clear:both;">
<asp:Label ID="sStato" runat="server" style="margin: 0px auto; text-align: center; border-width: 2px; border-radius: 3px; Border-Color: blue; box-sizing: border-box;" Height="64px" wrap="true" Width="1200px"></asp:Label>
</p>
<asp:Panel id="pChart" runat="Server" visible ="false" style="width: 1200px; margin: 0px auto; align-content:center; text-align:center; clear: both;">
<div style="width: 1200px; margin: 0px auto; align-content:center; text-align:center; clear: both;">
    <asp:Chart ID="cLibere" runat="server" Width="1200px" Height="210px" margin="0" BackColor="green" Palette="SemiTransparent" style="margin-bottom: 0px" AlternateText="Grafico disponibilità veicoli" 
        ToolTip="Sulla ascissa troverete i giorni di riferimento, sulla ordinata, troverete il numero di veicoli disponibili per quel giorno, sui punti di ritiro di Trento o sulla sede selezionata">
        <series>
            <asp:Series Name="Prenotazioni" YValuesPerPoint="2">
            </asp:Series>
        </series>
        <chartareas>
            <asp:ChartArea BackColor="Bisque" BackGradientStyle="TopBottom" Name="ChartArea1">
            </asp:ChartArea>                  
        </chartareas>
    </asp:Chart>
    <div style="margin: 0px auto; text-align: center; clear: both;">
    <asp:Label ID="lnota" Visible="false" runat="server" Text="I numeri si riferiscono ai veicoli che non hanno in carico alcuna prenotazione. Sono esclusi i mezzi che hanno anche una sola prenoatzione, magari di un periodo limitato."></asp:Label></div>
</div>
</asp:Panel>
<br />
<asp:Panel ID="PBuchi" runat="server" Visible ="false" style="margin: 0px auto; width: 1200px;">
    <asp:label runat="server">Rappresentazione oraria delle prenotazioni nel </asp:label><asp:Label runat="server"> <b>giorno di inizio missione.</b> Di colore bianco le ore non prenotate.</asp:Label>
    <asp:Table ID="tHeader" runat="server" AutoPostBack="true" BackColor="White" ClientIDMode="Predictable" ForeColor="Black" Width="1200px" Font-Size="Small" HorizontalAlign="NotSet">
        <asp:TableRow runat="server">
        </asp:TableRow>
    </asp:Table>    
    <asp:Table ID="tcale" runat="server" AutoPostBack="true" BackColor="White" ClientIDMode="Predictable" ForeColor="Black" Width="1200px" Font-Size="Small" HorizontalAlign="NotSet">
        <asp:TableRow runat="server">
        </asp:TableRow>
    </asp:Table>
</asp:Panel>
    <p class="clb" style="margin: 0px auto; width: 1024px; height: 40px;">
        &nbsp;</p>
    <p class="clb" style="margin: 0px auto; width: 1024px; height: 40px;">
        &nbsp;</p>

<asp:HiddenField ID="tDIS" runat="server" /><asp:HiddenField ID="COMODO1" runat="server" /><asp:HiddenField ID="COMODO" runat="server" />

<script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyA2hJKSHX_tREFJCyXca2toiuOdegB6euk"></script>
<script type="text/javascript">
function inizializza() {
    //var i = null;
    //if (document.getElementById('MainContent_ddlRitiro'))
    //   i = document.forms[0]['MainContent_ddlRitiro'].SelectedIndex;
    //var a = MainContent_ddlRitiro.options[i.value].text;
    var a = "";
    try {
        if (document.getElementById('MainContent_ddlRitiro') != null) {
            a = document.forms[0]['MainContent_ddlRitiro'].options[document.forms[0]['MainContent_ddlRitiro'].selectedIndex].text.substr(0, document.forms[0]['MainContent_ddlRitiro'].options[document.forms[0]['MainContent_ddlRitiro'].selectedIndex].text.lastIndexOf(","));
            if (a != "")
                getGeocode(document.forms[0]['MainContent_ddlRitiro'].options[document.forms[0]['MainContent_ddlRitiro'].selectedIndex].text.substr(0, document.forms[0]['MainContent_ddlRitiro'].options[document.forms[0]['MainContent_ddlRitiro'].selectedIndex].text.lastIndexOf(",")), "MainContent_COMODO1");
        }        
        var da = "";
        if (document.getElementById('MainContent_ddlComune') != null) {
            da = document.getElementById('MainContent_ddlComune');
            if (da.SelectedIndex != null) {
                da = Document.Forms[0]['MainContent_ddlComune'].options[document.forms[0]['MainContent_ddlComune'].SelectedIndex].Text;
            }
        }
        var dae;
        if (document.getElementById('MainContent_ddlStato') != null) {
            dae = document.forms[0]['MainContent_ddlStato'].options[document.forms[0]['MainContent_ddlStato'].selectedIndex].text;
            if (dae == "Estero" && document.forms[0]['MainContent_tComuneEstero'].value != "") {
                getGeocode(document.forms[0]['MainContent_tComuneEstero'].value, "MainContent_COMODO");
            }
        }
        else
            if (document.getElementById('MainContent_ddlProvincia') != null) {
                var p = ""; p = document.forms[0]['MainContent_ddlProvincia'].options[document.forms[0]['MainContent_ddlProvincia'].selectedIndex].text;
                var s = ""; s = p.substr(p.lastIndexOf(",") + 2);
                getGeocode(da + ", " + s + ", " + document.forms[0]['MainContent_ddlStato'].options[document.forms[0]['MainContent_ddlStato'].selectedIndex].text, "MainContent_COMODO");
            }
    }
    catch (e) {
        console.log('Errore: ', e.message); // i controlli a cui si fa riferimento non sono più presenti nella pagina
    }
    //document.getElementById("MainContent_hfConferma").value = "Falso";
}

function gettextclient(x, dove) {
    if (dove == "MainContent_COMODO1")
        getGeocode(x.options[x.selectedIndex].text.substr(0, x.options[x.selectedIndex].text.lastIndexOf(",")), "MainContent_COMODO1");
    if (dove == "MainContent_COMODO") {
        var da = "";
        da = document.forms[0]['MainContent_ddlComune'].options[document.forms[0]['MainContent_ddlComune'].selectedIndex].text;
        var dae = "";
        dae = document.forms[0]['MainContent_ddlStato'].options[document.forms[0]['MainContent_ddlStato'].selectedIndex].text;
        if (dae == "Estero" && document.forms[0]['MainContent_tComuneEstero'].value != "") {
            getGeocode(document.forms[0]['MainContent_tComuneEstero'].value, "MainContent_COMODO");
        }
        else
            if (document.getElementById('MainContent_ddlProvincia') != null) {
                var p = ""; p = document.forms[0]['MainContent_ddlProvincia'].options[document.forms[0]['MainContent_ddlProvincia'].selectedIndex].text;
                var s = ""; s = p.substr(p.lastIndexOf(",") + 2);
                getGeocode(da + ", " + s + ", " + document.forms[0]['MainContent_ddlStato'].options[document.forms[0]['MainContent_ddlStato'].selectedIndex].text, "MainContent_COMODO");
            }
    }
}

function getGeocode(posto, dovescrivo) {
        if (posto != undefined) {
            //alert("Devo calcorare il geocoding di " + posto + " e scrivere le coordinate in " + dovescrivo);		
        } else {
            alert("Non ho ricevuto indirizzi da calcolare=" + posto); return;
        }
        var geocoder = new google.maps.Geocoder();
        geocoder.geocode({ 'address': posto }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                var d = document.getElementById(dovescrivo);
                var loc = []; // no need to define it in outer function now
                loc[0] = results[0].geometry.location.lat();
                loc[1] = results[0].geometry.location.lng();
                d.value = loc[0] + "," + loc[1] + ", " + posto;
                //d.textContent= results[0].geometry.location; // + " - " + posto;
                //alert("d="+d.textContent);
                pikele(dovescrivo, loc[0], loc[1], posto);
            }
            //else {
            //    alert("Problema nella ricerca dell'indirizzo: " + status);
            //}
        });
}

function pikele(doveprendere, lati, longi, posto) {
    var gcl = document.getElementById(doveprendere);
    var elevator = new google.maps.ElevationService();
    var places = [
      { name: posto, lat: lati, lng: longi }];
    var locations = [];
    locations.push(new google.maps.LatLng(places[0].lat, places[0].lng));
    var positionalRequest = {
        'locations': locations
    }
    // Initiate the elevation request
    elevator.getElevationForLocations(positionalRequest, function (results, status) {
        if (status == google.maps.ElevationStatus.OK) {
            if (results[0]) {
                for (var i = 0; i < results.length; i++) {
                    document.getElementById(doveprendere).innerHTML +=
                      " elevation: " + results[i].elevation.toFixed(2);
                    /*document.getElementById(doveprendere).innerHTML +=
                      '"' + places[i].name + '", elevation: ' + results[i].elevation.toFixed(2) + 'm<br>'; */
                }
                var OK = "";
                var str0 = document.getElementById("MainContent_COMODO").textContent.trim();  // destinazione
                var str1 = document.getElementById("MainContent_COMODO1").textContent.trim(); // prelievo macchina
                if (str0.lastIndexOf(":") > 0 && str1.lastIndexOf(":") > 0) {
                    var i = str0.lastIndexOf(":") + 1;
                    str0 = str0.substr(i, str0.length - i - 1);
                    str0 = str0.substr(0, str0.lastIndexOf(".") + 3); // solo due decimali
                    i = str1.lastIndexOf(":") + 1;
                    str1 = str1.substr(i, str1.length - i - 1);
                    str1 = str1.substr(0, str1.lastIndexOf(".") + 3); // solo due decimali
                    if (str0 != "" && str1 != "") {
                        var atb = document.getElementById("MainContent_tDIS");
                        var x = Math.abs(str1.trim() - str0.trim()).toString();
                        if (x.lastIndexOf(".") > 0) x = x.substr(0, x.lastIndexOf(".") + 3);
                        atb.value = x + "  p" + str1 + " a" + str0;
                    }
                }
            }
        }
    });
}
window.onload = inizializza;
</script>
</asp:Content>