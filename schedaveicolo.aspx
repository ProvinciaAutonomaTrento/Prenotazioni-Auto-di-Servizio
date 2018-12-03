<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="schedaveicolo.aspx.cs" Inherits="schedaveicolo" %>

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
    .auto-style3 {
        position: relative;
        width: 1024px;
        margin: 0px auto;
        text-align: center;
        height: 447px;
        left: 0px;
        top: 0px;
    }
</style>
<br />
<div class="hrbianca"></div>
<div class="centra-text" style="font-size: 22px; height: 22px;">SCHEDA AUTOMEZZI</div><br /><div class="hrbianca"></div>
<hr class="hrsalmone clearfix" /><hr class="hrbianca" />
<div class="hrbianca"></div>
<div class="auto-style3" style="background-color:cornsilk; border-radius: 4px;">
    <div style="float: left; width: 140px;">Seleziona automezzo</div>
    <div style="float: left; width: 220px;">
        <asp:DropDownList ID="ddlMezzi" OnSelectedIndexChanged="ddlMezzi_SelectedIndexChanged" AutoPostBack="true" runat="server" Width="210px"></asp:DropDownList>
    </div>
    <div style="float: left; width: 50px; text-align: right;padding-right: 10px;">Targa</div>
    <div style="float: left; width: 105px;">
        <asp:DropDownList ID="ddlTarga" OnSelectedIndexChanged="ddlTarga_SelectedIndexChanged" AutoPostBack="true" runat="server" Width="100px"></asp:DropDownList>
    </div>
    <div style="float: left; width: 60px; text-align: right;padding-right: 10px;">Numero</div>
    <div style="float: left; width: 70px;">
        <asp:DropDownList ID="ddlNumero" OnSelectedIndexChanged="ddlNumero_SelectedIndexChanged" AutoPostBack="true" runat="server" Width="50px"></asp:DropDownList>
    </div>
    <div style="width: 80px; height: auto; text-align: center; float: left;">
        <asp:Button ID="bDel" runat="server" Text="Cancella" OnClick="bDel_Click" style="float: left; width: 70px;" />
    </div>
    <div style="text-align: right; width: 82px; padding-left: 2px; float: left;">
        <asp:Button ID="bSalva" runat="server" Text="Salva dati" OnClick="bSalva_Click"  style="float: left; width: 80px;" />
    </div>
    <div style="text-align: right; width: 165px; float: left;">
        <asp:Button ID="bInsert" runat="server" Text="Aggiungi nuovo veicolo" OnClick="bInsert_Click"  style="float: left; width: 160px;" />
    </div>

    <a href="menu.aspx?l=ubicazioni"><img src="img/home.png" alt="home page" /></a>
    <hr style="clear: both; height: 2px; color: black;" />
    <div style="clear: both;"></div>
    <div style="text-align: left; width: 150px; float: left;">Targa</div>
    <div style="text-align: left; width: 120px; float: left;"><asp:TextBox ID="tTarga" runat="server" Style="text-align: left; width: 100px; margin: 0px; float: left;"></asp:TextBox></div>
    <div style="text-align: left; width: 60px; float: left;">Numero</div>
    <div style="text-align: left; width: 200px; float: left;" ><asp:TextBox ID="tNumero" runat="server" Style="text-align: left; width: 50px; margin: 0px; float: left;"></asp:TextBox></div>
    <div  style="text-align: left; width: 120px; float:left;">Abilitato</div>
    <div style="text-align: left; width: 300px; float: left;">
        <asp:CheckBox ID="cbAbilitata" runat="server" CssClass="tb" style="width: 350px;"/>
    </div>
    <div style="clear: both;"></div>
    <div  style="text-align: left; width: 150px; float: left;">Produttore</div>
    <div style="text-align: left; width: 380px; float: left;"><asp:DropDownList ID="ddlProduttore" runat="server" CssClass="tb" style="width: 340px; float: left;"></asp:DropDownList></div>
    <div  style="text-align: left; width: 120px; float:left;">Modello</div>
    <div style="text-align: left; width: 370px; float: left;"><asp:DropDownList ID="ddlModello" runat="server" CssClass="tb" style="width:340px;"></asp:DropDownList></div>
    <div style="text-align: left; width: 150px; clear: both; float: left;">Trazione</div>
    <div style="text-align: left; width: 380px; float: left;"><asp:DropDownList ID="ddlTrazione" runat="server" CssClass="tb" style="width: 340px; float: left;"></asp:DropDownList></div>
    <div  style="text-align: left; width: 120px; float:left;">Alimentazione</div>
    <div style="text-align: left; width: 350px; float: left;"><asp:DropDownList ID="ddlAlimentazione" runat="server" CssClass="tb" style="width: 240px; float: left;"></asp:DropDownList></div>
    <div style="text-align: left; width: 150px; clear: both; float: left;">Posti</div>
    <div style="text-align: left; width: 380px; float: left;"><asp:DropDownList ID="ddlPosti" runat="server" CssClass="tb" style="width: 60px; float: left;"></asp:DropDownList></div>    
    <div  style="text-align: left; width: 120px; float:left;">Euro</div>
    <div style="text-align: left; width: 350px; float: left;"><asp:DropDownList ID="ddlEuro" runat="server" CssClass="tb" style="width: 60px; float: left;"></asp:DropDownList></div>
    <div style="text-align: left; width: 150px; clear: both; float: left;">Cambio</div>
    <div style="text-align: left; width: 380px; float: left;"><asp:DropDownList ID="ddlCambio" runat="server" CssClass="tb" style="width: 300px; float: left;"></asp:DropDownList></div>    
    <div  style="text-align: left; width: 120px; float:left;">Cavalli (cv)</div>
    <div style="text-align: left; width: 350px; float: left;"><asp:TextBox ID="tCavalli" runat="server" Style="text-align: left; width: 50px; margin: 0px; float: left;"></asp:TextBox></div>    
    <div style="text-align: left; width: 150px; clear: both; float: left;">Dotazione 1</div>
    <div style="text-align: left; width: 380px; float: left;"><asp:DropDownList ID="ddlDotazione1" runat="server" CssClass="tb" style="width: 340px; float: left;"></asp:DropDownList></div>
    <div  style="text-align: left; width: 120px; float:left;">Dotazione 2</div>
    <div style="text-align: left; width: 350px; float: left;"><asp:DropDownList ID="ddlDotazione2" runat="server" CssClass="tb" style="width: 340px; float: left;"></asp:DropDownList></div>
    <div style="text-align: left; width: 150px; clear: both; float: left;">Portata max (quintali)</div>
    <div style="text-align: left; width: 380px; float: left;"><asp:TextBox ID="tPortata" runat="server" Style="text-align: left; width: 50px; margin: 0px; float: left;"></asp:TextBox></div>    
    <div  style="text-align: left; width: 120px; float:left;">Colore</div>
    <div style="text-align: left; width: 350px; float: left;"><asp:TextBox ID="tColore" runat="server" Style="text-align: left; width: 250px; margin: 0px; float: left;"></asp:TextBox></div>    
    <div style="text-align: left; width: 150px; clear: both; float: left;">Classificazione</div>
    <div style="text-align: left; width: 380px; float: left;"><asp:DropDownList ID="ddlCla" runat="server" CssClass="tb" style="width: 370px; float: left;"></asp:DropDownList></div>
    <div  style="text-align: left; width: 120px; float:left;">Pneumatici</div>
    <div style="text-align: left; width: 350px; float: left;"><asp:DropDownList ID="ddlGomme" runat="server" CssClass="tb" style="width: 340px; float: left;"></asp:DropDownList></div>
    <div style="text-align: left; width: 150px; clear: both; float: left;">Descizione e uso</div>
    <div style="text-align: left; width: 858px; float: left;"><asp:TextBox ID="tDescrizione" runat="server" Style="text-align: left; width: 840px; margin: 0px; float: left;"></asp:TextBox></div>
    <div style="text-align: left; width: 150px; clear: both; float: left;">Scadenza contratto noleggio</div>
    <div style="text-align: left; width: 380px; float: left;"><asp:TextBox ID="tScadenza" runat="server" CssClass="tb" style="width: 120px; float: left;"></asp:TextBox></div>
    <div  style="text-align: left; width: 120px; float:left;">Scatola nera ?</div>
    <div style="text-align: left; width: 300px; float: left;">
        <asp:CheckBox ID="cbBlackBox" runat="server" CssClass="tb" style="width: 350px;"/>
    </div>   
    <hr style="clear: both; height: 2px; color: black;" />
    <div style="text-align: left; width: 150px; clear: both; float: left;">Area di ritiro</div>
    <div style="text-align: left; width: 380px; float: left;"><asp:DropDownList ID="ddlPosteggio" runat="server" CssClass="tb" style="width: 340px; float: left;"></asp:DropDownList></div>
    <div  style="text-align: left; width: 120px; float:left;">Ritiro chiavi</div>
    <div style="text-align: left; width: 350px; float: left;"><asp:DropDownList ID="ddlRitiro" runat="server" CssClass="tb" style="width: 340px; float: left;"></asp:DropDownList></div>    
    <div style="text-align: left; width: 150px; clear: both; float: left;">Posteggio numero</div>
    <div style="text-align: left; width: 380px; float: left;"><asp:TextBox ID="tPosteggio" runat="server" Style="text-align: left; width: 120px; margin: 0px; float: left;"></asp:TextBox></div>
    <hr style="clear: both; height: 2px; color: none;" />
    <div style="clear:both; float: left; text-align: left;"> 
        <div style="text-align: left; width: 150px; float: left;">Libretto fronte/retro</div>    
        <asp:FileUpload ID="fuLibretto"  runat="server" AllowMultiple="false" CssClass="tb" />
        <asp:Label ID="Llibretto" runat="server" CssClass="tb"  Width="300px" ></asp:Label>
        <asp:CheckBox ID="cbLibretto" runat="server" Text="elimina foto" />
    </div>
    <div style="clear: both;"> </div>    
    <div style="float: left; text-align: left;">  
        <div style="text-align: left; width: 150px; float: left;">Foto veicolo (lato dx)</div>    
        <asp:FileUpload ID="fuFotodx"  runat="server" AllowMultiple="false" CssClass="tb"/>
        <asp:Label ID="Lfotodx" runat="server" CssClass="tb" Width="300px"></asp:Label>
        <asp:CheckBox ID="cbdx" runat="server" Text="elimina foto" />
    </div> 
    <div style="clear:both; float: left; text-align: left;">  
        <div style="text-align: left; width: 150px; float: left;">Foto veicolo (fronte)</div>    
        <asp:FileUpload ID="fuFronte"  runat="server" AllowMultiple="false" CssClass="tb"/>
        <asp:Label ID="Lfronte" runat="server" CssClass="tb" Width="300px"></asp:Label>
        <asp:CheckBox ID="cbfr" runat="server" Text="elimina foto" />
    </div>
    <div style="clear: both; float: left; text-align: left;">  
        <div style="text-align: left; width: 150px; float: left;">Foto veicolo (plancia)</div>    
        <asp:FileUpload ID="fuInt"  runat="server" AllowMultiple="false" CssClass="tb"/>
        <asp:Label ID="Linterni" runat="server" CssClass="tb" Width="300px"></asp:Label>
        <asp:CheckBox ID="cbint" runat="server" Text="elimina foto" />
    </div>
    <div style="clear:both; float:left;"></div>
    <div style="width : 1024px; margin: 0px auto;">
        <asp:Image ID="iLibretto" runat="server" style="float:left; width: 250px; height: auto; box-sizing: border-box; margin : 6px; margin-left: 0px;"/>
        <asp:Image ID="iFotodx" runat="server" style="float:left; width: 250px; height: auto; box-sizing: border-box; margin : 6px; margin-left: 0px;"/>
        <asp:Image ID="iFotofr" runat="server" style="float:left; width: 250px; height: auto; box-sizing: border-box; margin : 6px; margin-left: 0px;"/>
        <asp:Image ID="iFotoint" runat="server" style="float:left; width: 250px; height: auto; box-sizing: border-box; margin : 6px; margin-left: 0px;"/>
    </div>
</div>
<div style="clear:both; float:left;"> </div>
<p class="centra-text clearfix">
<asp:TextBox ID="sStato" runat="server" style="clear: both; border-width: 1px; border-radius: 4px; Border-Color: blue; width: 1024px; box-sizing: border-box;"></asp:TextBox>
</p>
</asp:Content>
