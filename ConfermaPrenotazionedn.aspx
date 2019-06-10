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

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConfermaPrenotazionedn.aspx.cs" Inherits="ConfermaPrenotazionedn" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Gestione flotta: conferma prenotazione</title>
<style>
    .centra-text {
        text-align: center;
    }
    .hrsalmone {
        Width: 1024px; margin: auto; background-color: lightsalmon; height: 6px;
    }
    .hrbianca {
        Width: 1024px; margin: auto; border: 2px; background-color: white; height: 8px; text-align:center;  clear: both;
    }
    .container {
        position: relative;
        width: 1024px;
        margin: 0px auto;
        text-align: center;
        padding: 5px;
        box-sizing: border-box;
    }
    .clearfix {
        content : "";
        clear: both;
        display: table;
    }
    .dentroadx {
        position: absolute;
        top: 25px;
        right: 2px;
        width: 171px;
        height: 78px;
        border: none;
    }
    .noprint {
        display:none;
    }
    @page {size: 210mm 297mm; margin: 16mm;}
    </style>
<script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyA2hJKSHX_tREFJCyXca2toiuOdegB6euk"></script>
<script type="text/javascript">
var directionDisplay;
var directionsService = new google.maps.DirectionsService();
var map;

function initialize() {
        //alert("Si Comincia!");
    calcRoute();
    directionsDisplay = new google.maps.DirectionsRenderer();
    var map;
    var trento = new google.maps.LatLng(46.0747793, 11.1217486);
    var myOptions = {
        zoom: 8,
        //mapTypedId: google.maps.MapTypedId.ROADMAP,
        center: trento
    }
    map = new google.maps.Map(document.getElementById('percorso'), myOptions);

    directionsDisplay.setMap(map);
    directionsDisplay.setPanel(document.getElementById('panel'));

    var selectTags = document.getElementsByTagName('select');
    for(i=0;i<selectTags.length;i++){
        selectTags[i].onchange=function(){ calcRoute('Trento'); };
    }
}

function calcRoute() {
var op = document.getElementById('npartenza');
var oa = document.getElementById('narrivo');
var partenza = op.value;
var arrivo = oa.value;
//alert("Si parte da: " + partenza + " e si arriva a: " + arrivo);
var request =
{	origin:partenza,
	destination:arrivo,
	provideRouteAlternatives: true,
	travelMode: google.maps.DirectionsTravelMode.DRIVING
};
directionsService.route(request, function(response, status) {
	if (status == google.maps.DirectionsStatus.OK) {
		directionsDisplay.setDirections(response);
	}
	else {
	    alert('Google non riesce a trovare la destinazione: ' + status);
	    var d = document.getElementById("PanelMappa");
	    d.Visible = "false";
	    d.hidden = "true";
	    var d = document.getElementById("sStato");
	    d.Visible = "false";
	    d.hidden = "true";
	    var d = document.getElementById("btOKMappa");
	    d.Visible = "false";
	    d.hidden = "true";
	}
});

}
window.onload = initialize;
function stampasmart() {
    var d = document.getElementById("pbottoni");
    d.Visible = "false";
    d.hidden = "true";
    var d = document.getElementById("sStato");
    d.Visible = "false";
    d.hidden = "true";
    window.print();
    d.Visible = "true";
    d.hidden = "false";
    var d = document.getElementById("sStato");
    d.Visible = "false";
    d.hidden = "true";
    location.reload();

    window.open("pre.aspx");
}
function stampasmart2() {
    var Win = window.open('', '', 'left=0,top=0,width=1280,height=800,toolbar=0,scrollbars=0,status=0');
    var content = "<html>";
    content += "<body onload=\"toglibottoni(); window.print(); window.close();\">";
    content += document.body.innerHTML;
    content += "</body>";
    content += "</html>";
    content += "<script type=\'text/javascript\'>";
    content += "function toglibottoni() {";
    content += "  var d = document.getElementById(\'dpanelbottoni\');";
    content += "  d.Visible = \'false\';";
    content += "  d.hidden = \'true\';";
    content += "  var d = document.getElementById(\'sStato\');";
    content += "  d.Visible = \'false\';";
    content += "  d.hidden = \'true\';";
    content += "  var d = document.getElementById(\'npartenza\');";
    content += "  d.Visible = \'false\';";
    content += "  d.hidden = \'true\';";
    content += "  var d = document.getElementById(\'narrivo\');";
    content += "  d.Visible = \'false\';";
    content += "  d.hidden = \'true\';";
    content += "  var d = document.getElementById(\'Estremi\');";
    content += "  d.Visible = \'true\';";
    content += "  d.hidden = \'false\';";
    content += "}";
    content += "<\/script>";
    Win.document.write(content);
    Win.document.close();
}

function printPage(id) {
    var html = "<html>";
    html += document.body.innerHTML;
    html += "</html>";
    var printWin = window.open('', '', 'left=0,top=0,width=1024,height=800,toolbar=0,scrollbars=0,status  =0');
    printWin.document.write(html);
    printWin.document.close();
    printWin.focus();
    printWin.print();
    printWin.close();
    var d = document.getElementById("sStato");
    d.Visible = "true";
    d.hidden = "false";
    var d = document.getElementById("npartenza");
    d.Visible = "true";
    d.hidden = "false";
    var d = document.getElementById("narrivo");
    d.Visible = "true";
    d.hidden = "false";
    var d = document.getElementById("dpanelbottoni");
    d.Visible = "true";
    d.hidden = "false";
    window.open('pre.aspx');
}

</script>

</head>
<body>
<form id="form1" runat="server" >
<div style="position: relative; width:1024px; Height: 92px; text-align:center; margin: 0px auto; ">
    <div style="width:230px; float:left; "><asp:Image ID="Image1" runat="server" SRC="../IMG/PAT.PNG"  alternateText="Logo Provincia Autonoma di Trento" height="90px"/></div>
    <div style="float:left; text-align: center; vertical-align: top; width:550px; ">DIPARTIMENTO ORGANIZZAZIONE, PERSONALE E AFFARI GENERALI<br />UFF. GESTIONI GENERALI</div>
    <div style="width:230px;float:left;align-content:center;"><asp:Image ID="Image2" runat="server" SRC="../IMG/TRENTINO.PNG"  ALIGN="RIGHT" AlternateText="Logo Trentino" height="80px"/></div>
</div>
<p></p>
    <div id="dpanelbottoni" style="clear: both; width: 1024px; margin: 0px auto; text-align: center; vertical-align: central;"> 
        <asp:Panel ID="pbottoni" runat="server" UpdatePanel="always"  HorizontalAlign="Center"> 
            <div id="esternobottoni" style="width: 1020px; Height: 52px;">
                <div style="float: left; height:52px; width: 210px; padding: 3px; margin-top: 3px">
                    <asp:Button ID="btNuova" runat="server" Text="NUOVA PRENOTAZIONE" Height="52px" Width="180px" style="padding: 6px; vertical-align: central;" OnClick="btNuova_Click"
                        tooltip="Per ritornare alla pagina delle prenotazioni"/>
                </div>
                <div style="border: solid; border-color: blue; border-width: 3px; background-color: salmon; vertical-align: central; width: 320px; height: 52px; float: left; padding:6px; ">
                    <asp:Button ID="btCarica" runat="server" Text="CARICA FOGLIO DI PRENOTAZIONE" Width="310" Height="40" OnClick="btCarica_Click" ToolTip="Per ottenere il foglio di prenotazione. Lo troverete comunque nella casella di posta elettronica" />
                </div>
                <div style="padding: 3px; margin-top: 3px; width: 190px; padding: 3px; margin-top: 3px; float: left; text-align: center;">
                    <asp:Button ID="btEstero" runat="server" Text="DELEGA PER ESTERO" width="160" Height="52" style="padding: 6px; vertical-align: central; " OnClick="btEstero_Click" Visible="true" Enabled="false"
                        ToolTip="Per ottenere il foglio di delega alla guida dell'automezzo nei paesi esteri. Lo troverete comunque nella casella di posta elettronica"/>
                </div>
                <div style="padding: 3px; margin-top: 3px; width: 210px; padding: 3px; margin-top: 3px; float:left; ">
                    <input type="button" id="btnPrint" value="STAMPA PERCORSO" onclick="stampasmart2();" style="width: 180px; height: 52px; vertical-align: middle; padding:6px;" />
                </div>
            </div>
        </asp:Panel>
    </div>    <br /> <br />

    <div style="clear: both; margin: 0px auto; text-align: center;" >
        <div id="Estremi" style="margin: 0px auto; Width:1024px; text-align: center; font-size: x-large">ESTREMI DI PRENOTAZIONE</div>
        <br /><br />
        <asp:Table ID="tRiepilogo" runat="server" Width="1024px" Visible ="false" HorizontalAlign="Center" Font-Size="X-Large">
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
            <asp:TableCell runat="server">sede ritiro <b>chiavi</b>:</asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"><asp:Label ID="lRitiro0" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" HorizontalAlign="Left">
            <asp:TableCell runat="server" Width="10px"></asp:TableCell>
            <asp:TableCell runat="server">sede ritiro <b>veicolo</b>:</asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"><asp:Label ID="lRitiro" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" HorizontalAlign="Left" ID="trVeicolo" Visible="true">
            <asp:TableCell runat="server" Width="10px"></asp:TableCell>
            <asp:TableCell runat="server">autoveicolo:</asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"><asp:Label ID="lVeicolo" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" HorizontalAlign="Left" ID="trNumero" Visible="true">
            <asp:TableCell runat="server" Width="10px"></asp:TableCell>
            <asp:TableCell runat="server">NUMERO:</asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"><asp:Label ID="lNumero" runat="server" Font-Bold="true" Font-Size="X-Large"></asp:Label></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" HorizontalAlign="Left">
            <asp:TableCell runat="server" Width="10px"></asp:TableCell>
            <asp:TableCell runat="server">viaggiatori oltre al conducente:</asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"><asp:Label ID="lPasseggeri" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" HorizontalAlign="Left" ID="trDriver" Visible="true">
            <asp:TableCell runat="server" Width="10px"></asp:TableCell>
            <asp:TableCell runat="server">conducente:</asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"><asp:Label ID="lDriver" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" HorizontalAlign="Left" ID="trAggregato" Visible="true">
            <asp:TableCell runat="server" Width="10px"></asp:TableCell>
            <asp:TableCell runat="server">car pooling con:</asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"><asp:Label ID="lAggregato" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <input type="hidden" id="npartenza" runat="server" />
    <input type="hidden" id="narrivo"  runat="server" />
    </div>
    <br /><br />
    <div id="nota" style="clear: both; margin: 0px auto; text-align: center; width: 1024px;" >
    <strong>NOTA:</strong> RAMMENTIAMO CHE AI SENSI DEL DISCIPLINARE D’USO DELLE AUTO DI SERVIZIO I RIFORNIMENTI DI CARBURANTE VANNO EFFETTUATI TASSATIVAMENTE PRESSO LE COLONNE SELF SERVICE. 
    </div>
    <br />
    
    <asp:Panel ID="percorso" runat="server" height="800" Style="clear:both; width:1020px; margin: 0px auto; text-align: center;" >
    </asp:Panel>
    <div Style="clear:both; width:1024px; height:60px; margin: 0px auto; text-align: center;"></div>
    <div id="panel" style="clear: both; width: 70%; border: solid 2px; border-color:  red; box-sizing: border-box; font-size: 8px; margin: 0px auto; text-align: center;" ></div>
    <p class="clb" style="clear:both; margin: 0px auto; width: 1024px;">
    <asp:TextBox ID="sStato" runat="server" style="margin: 0px auto; text-align: center; border-width: 1px; border-radius: 3px; Border-Color: blue; width: 1024px; box-sizing: border-box;"></asp:TextBox>
    </p>
</form>
</body>
</html>
