<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="schedaubicazioni.aspx.cs" Inherits="schedaubicazioni" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <script type="text/javascript">
    function antemappa() {
        //var am = document.getElementById("<% =iMappa.ClientID %>");
        //alert("Nome file selezionato: " + am.value);
        //alert("Nome file selezionato: " + ufile.value);
        //alert("Nome file selezionato: " + ufile.file);
        //var ss = document.getElementById("spanmappa");
        //ss.innerHTML = "<img src='DATA/2011032217251709indicazione.jpg' border='4' width='385' height='265'>";
        return;
    }
</script>
<script type="text/javascript">
    function AnteprimaMappa(ufile) {
        //var am = document.getElementById("<% =iMappa.ClientID %>");
        //alert("Nome file selezionato: " + am.value);
        //alert("Nome file selezionato: " + ufile.value);
        //alert("Nome file selezionato: " + ufile.file);
        //var ss = document.getElementById("spanmappa");
        //ss.innerHTML = "<img src='DATA/2011032217251709indicazione.jpg' border='4' width='385' height='265'>";
        return;
    }
</script>
<script type="text/javascript">
    var validFilesTypes = ["bmp", "gif", "png", "jpg", "jpeg", "pdf"];
    function ValidateFile() {
        var file = document.getElementById("<%=fuMappa.ClientID%>");
        var label = document.getElementById("<%=Lmappa.ClientID%>");        
        var path = file.value;
        var ext = path.substring(path.lastIndexOf(".") + 1, path.length).toLowerCase();
        var isValidFile = false;
        for (var i = 0; i < validFilesTypes.length; i++) {
            if (ext == validFilesTypes[i]) {
                isValidFile = true;
                break;
            }
        }
        if (!isValidFile) {
            label.style.color = "red";
            label.innerHTML = "Invalid File. Please upload a File with" + " extension:\n\n" + validFilesTypes.join(", ");
        }
        return isValidFile;
    }
    function ValidateFileFoto() {
        var file = document.getElementById("<%=fuFoto.ClientID%>");
        var label = document.getElementById("<%=Lfoto.ClientID%>");
        var path = file.value;
        var ext = path.substring(path.lastIndexOf(".") + 1, path.length).toLowerCase();
        var isValidFile = false;
        for (var i = 0; i < validFilesTypes.length; i++) {
            if (ext == validFilesTypes[i]) {
                isValidFile = true;
                break;
            }
        }
        if (!isValidFile) {
            label.style.color = "red";
            label.innerHTML = "Invalid File. Please upload a File with" + " extension:\n\n" + validFilesTypes.join(", ");
        }
        return isValidFile;
    }
</script>
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
    .auto-style1 {
        width: 200px;
        float: left;
    }
    .tb {
        border-width: 1px; border-radius: 4px; Border-Color: blue;  box-sizing: border-box; text-align: left; padding: 1px; margin: 2px; float:left;
    }
</style>
<br />
<div class="hrbianca"></div>
<div class="centra-text" style="font-size: 22px; height: 22px;">SCHEDA UBICAZIONI</div><br /><div class="hrbianca"></div>
<hr class="hrsalmone clearfix" /><hr class="hrbianca" />
<div class="hrbianca"></div>
<div class="container" style="background-color:cornsilk; border-radius: 4px; height:auto;">
    <div style="clear:both;  width: 200px;text-align: left; float: left;">Seleziona ubicazione</div>
    <div style="width: 370px; text-align: left; float: left;">
        <asp:DropDownList ID="ddlUbi" runat="server" AutoPostBack="true" Width="350px" OnSelectedIndexChanged="ddlUbi_SelectedIndexChanged" style="float: left;"></asp:DropDownList>
    </div>
    <div style="text-align: right; width: 100px; float: left;">
        <asp:Button ID="bDel" runat="server" Text="Cancella" OnClick="bDel_Click" style="float: left; width: 80px;" />
    </div>
    <div style="text-align: right; width: 100px; float: left;">
        <asp:Button ID="bSalva" runat="server" Text="Salva dati" OnClick="bSalva_Click"  style="float: left;" />
    </div>
    <div style="text-align: right; width: 200px; float: left;">
        <asp:Button ID="bInsert" runat="server" Text="Aggiungi nuova ubicazione" OnClick="bInsert_Click"  style="float: left; width: 180px;" />
    </div>

    <a href="menu.aspx?l=ubicazioni"><img src="img/home.png" alt="home page" /></a>
    <div style="clear: both;"> </div>

    <div style="text-align: left; width: 200px; float:left;">Ubicazione</div>
    <div style="width: 370px; float: left;">
        <asp:TextBox ID="tUbi" runat="server" CssClass="tb" style="width: 350px;"></asp:TextBox>
    </div>
    <div  style="text-align: left; width: 110px; float:left;">Abilitata</div>
    <div style="text-align: left; width: 300px; float: left;">
        <asp:CheckBox ID="cbAbilitata" runat="server" CssClass="tb" style="width: 250px;"/>
    </div>
    <div style="clear: both;"> </div>
    <div  style="text-align: left; width: 200px; float: left;">Comune</div>
    <div style="text-align: left; width: 370px; float: left;">
        <asp:DropDownList ID="ddlComune" runat="server" CssClass="tb" style="width: 200px;"></asp:DropDownList>
    </div>
    <div  style="text-align: left; width: 110px; float:left;">Provincia</div>
    <div style="text-align: left; width: 300px; float: left;">
        <asp:DropDownList ID="ddlProvincia" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged" CssClass="tb" style="width: 250px;"></asp:DropDownList>
    </div>
    <div style="clear: both;"> </div>
    <div style="text-align: left; " class="auto-style1">Via</div>
    <div style="text-align: left; width: 370px; float: left; ">
        <asp:TextBox ID="tVia" runat="server" CssClass="tb" style="width: 350px;"></asp:TextBox>
    </div>
    <div style="text-align: left; width: 110px; float: left;">Civico</div>
    <div style="text-align: left; width: 160px; float: left;">
        <asp:TextBox ID="tCivico" runat="server" CssClass="tb" style="width: 100px;"></asp:TextBox>
    </div>
    <div style="clear: both;"> </div>
    <div style="text-align: left; " class="auto-style1">Aperto dalle</div>
    <div style="text-align: left; width: 370px; float: left; ">
        <asp:TextBox ID="tDalle" runat="server" CssClass="tb" style="width: 60px;"></asp:TextBox>
    </div>
    <div style="text-align: left; width: 110px; float: left;">Aperto sino alle</div>
    <div style="text-align: left; width: 160px; float: left;">
        <asp:TextBox ID="tAlle" runat="server" CssClass="tb" style="width: 60px;"></asp:TextBox>
    </div>

    <div style="clear: both;"> </div>
    <div style="text-align: left; width: 200px; float: left;">Descizione ubicazione</div>
    <div style="text-align: left; width: 810px; float: left;">
        <asp:TextBox ID="tDescrizione" runat="server" CssClass="tb" style="width: 800px;"></asp:TextBox>
    </div>
    <div style="clear: both;"> </div>    
    <div style="float: left; text-align: left;">  
        <div style="text-align: left; width: 200px; float: left;">Mappa</div>    
        <asp:FileUpload ID="fuMappa"  runat="server" AllowMultiple="false" CssClass="tb" />
        <asp:Label ID="Lmappa" runat="server" CssClass="tb"></asp:Label>
        <div style="clear: both;"> </div>    
        <div style="float: left; text-align: left;">  
            <div style="text-align: left; width: 200px; float: left;">Foto</div>    
            <asp:FileUpload ID="fuFoto"  runat="server" AllowMultiple="false" CssClass="tb"/>
            <asp:Label ID="Lfoto" runat="server" CssClass="tb"></asp:Label>
        </div> 
    </div>
    <div style="clear:both; float:left;"></div>
    <div style="width : 1024px; margin: 0px auto;">
        <div style="float:left;"><asp:Image ID="iMappa" runat="server" style="float:left; width: 500px; height: auto; box-sizing: border-box; margin : 6px;"/></div>
        <div style="float:left;"><asp:Image ID="iFoto" runat="server" style="float:left; width: 500px; height: auto; box-sizing: border-box; margin : 6px;"/></div>
        <br />
    </div>
    <div style="clear:both; float:left;">
    </div>
</div>
<p class="centra-text clearfix">
<asp:TextBox ID="sStato" runat="server" style="border-width: 1px; border-radius: 4px; Border-Color: blue; width: 1024px; box-sizing: border-box;"></asp:TextBox>
</p>

</asp:Content>
