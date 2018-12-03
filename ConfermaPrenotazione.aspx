<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConfermaPrenotazione.aspx.cs" Inherits="ConfermaPrenotazione" %>
<!doctype html>
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
    .auto-style1 {
        width: 1025px;
        height: 248px;
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
    var trento = new google.maps.LatLng(46.0747793, 11.1217486);
    var myOptions = {
        zoom: 8,
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
	/*waypoints:
	[
    		{
      		location: 'Mezzolombardo',
		     stopover: false
    		}
	], */
	provideRouteAlternatives: true,
	travelMode: google.maps.DirectionsTravelMode.DRIVING,
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
    var Win = window.open('', '', 'left=0,top=0,width=1024,height=800,toolbar=0,scrollbars=0,status  =0');
    var content = "<html>";
    content += "<body onload=\"toglibottoni(); window.print(); window.close();\">";
    content += document.body.innerHTML;
    content += "</body>";
    content += "</html>";
    content += "<script type=\'text/javascript\'>";
    content += "function toglibottoni() {";
    content += "  var d = document.getElementById(\'pbottoni\');";
    content += "  d.Visible = \'false\';";
    content += "  d.hidden = \'true\';";
    content += "  var d = document.getElementById(\'sStato\');";
    content += "  d.Visible = \'false\';";
    content += "  d.hidden = \'true\';";
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
    var d = document.getElementById("pbottoni");
    d.Visible = "true";
    d.hidden = "false";
    var d = document.getElementById("sStato");
    d.Visible = "true";
    d.hidden = "false";
    window.open('pre.aspx');
}

</script>

</head>
<body>
<form id="form1" runat="server">
<div style="clear: both; margin: 0px auto; width:1024px; HEIGHT:190PX">
    <asp:Image ID="Image1" runat="server" SRC="../IMG/PAT.PNG" Height="90"/>
    <asp:Image ID="Image2" runat="server" SRC="../IMG/TRENTINO.PNG" Height="80px" ALIGN="RIGHT"/>
    <div style="clear:both; text-align: center;">DIPARTIMENTO ORGANIZZAZIONE, PERSONALE E AFFARI GENERALI</div>
    <div style="clear:both; text-align: center;">UFF. GESTIONI GENERALI</div>
    <br />
</div>
<div style="clear: both; margin: 0px auto; width:1024px; float:left; text-align: center; font-size: x-large">FOGLIO DI PRENOTAZIONE</div>
<br /><br />
<div class="container" style="border-radius: 6px; border-color: black; height:auto; z-index: 1">
    <div id="dpanelbottoni" style="clear: both; width: 1024px; margin: 0px auto; text-align: center;"> 
        <div style="width: 580px; float: left; height:26px;"></div>   
        <asp:Panel ID="pbottoni" runat="server" UpdatePanel="always"> 
            <div style="width: 90px; float: left; ">
                <a href="pre.aspx?l=si"><img src="home.png" border="0" style="float: left; height:26px; vertical-align: central;"/></a>
                <div style="width:5px; float:left;">
                </div>
                <a href="pre.aspx?l=si" style="float: left; text-align: center; width: 35px; height:26px; vertical-align: central; font-size:larger;">Home</a>
            </div>
            <div style="width: 80px; float: left;"><input type="button" id="btnPrint" onclick="stampasmart2();" value="Stampa"/></div> 
            <div style="width: 245px; float: left;"><asp:Button ID="btOKMappa" runat="server" Text="Nascondi inidicazioni stradali e mappa" width="250px" OnClick="btOKMappa_Click"/></div> 
        </asp:Panel>
    </div>    
    <div style="clear: both; margin: 0px auto; text-align: center;" class="auto-style1">
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
            <asp:TableCell runat="server">sede ritiro chiavi:</asp:TableCell>
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
<br /><br /><br />
<div style="clear: both;"> </div>
<div id="Autorizzazione" style="text-align: left; border: solid; border-color: black; border-radius: 8px; width: 1024px; box-sizing: border-box ;height: auto; padding: 22px">
<div style="margin: 0px auto; text-align: center;"><b><U>AUTORIZZAZIONE ALL’UTILIZZO DI AUTOVEICOLI DI SERVIZIO DELL’AMMINISTRAZIONE PROVINCIALE</U></b></div><br />
<div style="text-align: left;">Il presentre foglio di prenotazione costituisce anche <strong>AUTORIZZAZIONE</strong> a condurre l’automezzo per esigenze di servizio, la stessa è efficace solo per le date e gli orari sopra-indicati.</div>

<div id="data"><span>      </span>Trento,<span>   </span><asp:Label ID="lData" runat="server" Text="___/___/2017"></asp:Label></div>
    
<div style="text-align: right;">DIPARTIMENTO ORGANIZZAZIONE, PERSONALE E AFFARI GENERALI</div>
</div>
<br /><br />
<div style="text-align: left;">
CONTROLLARE ATTENTAMENTE LE CONDIZIONI DEL VEICOLO. RIPORTARE QUI LE EVENTUALI ANOMALIE CON UNA BREVE NOTA.
</div>
<div id="note" style="border-style: double; border-color: black; border-radius: 8px; width: 1024px; height: 680px; box-sizing: border-box;">
</div>
<br />
<br />
<br />
<br />
<br />
<asp:Panel ID="PanelMappa" runat="server" Visible ="true">
<div id="percorso" style="clear:both; width:100%; height: 560px; border: solid 2px; border-color:  blue; box-sizing: border-box;"></div><div id="panel table" style="font-size: 8px;"></div>
<div id="panel" style="float: left; width: 70%; border: solid 2px; border-color:  red; box-sizing: border-box; font-size: 8px;"></div>
</asp:Panel>
<p class="clb" style="margin: 0px auto; width: 1024px;">
<asp:TextBox ID="sStato" runat="server" style="margin: 0px auto; text-align: center; border-width: 1px; border-radius: 3px; Border-Color: blue; width: 1024px; box-sizing: border-box;"></asp:TextBox>
</p>
</div>
</form>
</body>
</html>
