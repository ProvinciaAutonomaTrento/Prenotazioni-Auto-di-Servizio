<%@ Page Title="Atrezzi operatore" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="registro.aspx.cs" Inherits="registro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<style>
    .auto-testata {
        max-width: 100%;
        height: 121px;
        border: none;
        z-index: -1;
    }
    .centra {
        margin: 0px auto;
    }
    .text-center { text-align: center; }
    .clearfix {
        content: "";
        clear: both;
        display: table;
    }
    .hrsalmone {
        Width: 1024px; margin: auto; background-color: lightsalmon; height: 6px;
    }
    .hrbianca {
        Width: 1024px; margin: auto; border: 2px; background-color: white; height: 8px;
        clear: both;
    }
    .tb {
        border-width: 1px; border-radius: 4px; Border-Color: blue;  box-sizing: border-box; text-align: left; padding: 1px; margin: 2px; float:left;
    }

    .menu { width: 1024px; margin: 0px auto; background-color: lightsalmon; border-radius: 6px; box-sizing:border-box; padding:3px; z-index: -22; height: 28px;}
    .col1 {
        width: 10px;
    }
    .col3 {
        width: 90px;
        text-align: left;
    }
    .colinput {
        width: 150px;
        text-align:left;
    }

    .menucentro {
        border: 4px solid; 
        width: 633px; height: auto; 
        border-color: green; 
        background-color: lightgreen; 
        border-radius: 6px; 
        padding: 10px; 
        box-sizing: border-box;
        text-align: center; 
        margin: 0px auto;
    }
</style>
<br />
<div class="hrbianca"></div>
<div style="height: 22px; text-align: center; font-size: 22px;">REGISTRO PRENOTAZIONI</div>
    <br /><div class="hrbianca"></div>
<div class="menu">
    <div style="width: 880px; float: left; height:26px; text-align: left; ">
        <asp:Label ID="LBenvenuto" runat="server" Text=" "></asp:Label>
    </div>    
    <asp:Button ID="bUscita" runat="server" Text="Uscita" Width="90px" OnClick="bUscita_Click" Style="float: left; "/>
    <div style="width: 40px; text-align: center; float: left;" id="home"><a href="menu.aspx?l=anagrafica"><img src="img/home.png" /></a></div>
</div>
<hr class="hrbianca" />    
<div class="hrbianca" style="clear:both;"></div>
<div class="container; centra;" style="background-color:white; border-width: 6px; border-radius: 4px; box-sizing: border-box;">
    <div class="menucentro">
            <table class="text-center;" >
            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td class="col3" colspan="3"><div style="border: 3px solid green; border-radius: 6px; text-align:center;">Stampa registro</div></td> 
                <td class="col1">&nbsp;</td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td class="col3">&nbsp;</td>
                <td class="auto-style1">&nbsp;</td>                
                <td class="colinput">
                    &nbsp;</td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1">&nbsp;</td>
                <td class="col1">&nbsp;</td>
                <td class="col3">Sede</td>
                <td class="col1"></td>                
                <td class="colinput">
                    <asp:DropDownList ID="ddlSedi" runat="server" Width="441px" Height="31px" Font-Size="Medium" OnSelectedIndexChanged="ddlSedi_SelectedIndexChanged" ></asp:DropDownList>
                </td>
                <td class="col1">&nbsp;</td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td class="col3">&nbsp;</td>
                <td class="auto-style1">&nbsp;</td>                
                <td class="colinput">
                    <asp:Calendar ID="cldData" runat="server" style="margin-left:40px;" OnSelectionChanged="cldData_SelectionChanged">
                        <SelectedDayStyle BackColor="#003399" />
                        <SelectorStyle BackColor="#CCCCCC" />
                        <TitleStyle BackColor="#999999" BorderColor="Black" />
                    </asp:Calendar>
                </td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td class="col3" style="vertical-align: top;">Data</td>
                <td class="col1"></td>                
                <td class="colinput" style="margin: 0px auto; text-align: center;">
                    <asp:Label ID="lData" runat="server" Text="" style="margin-left:40px; text-align: center;"></asp:Label>
                <td class="col1">&nbsp;</td>
            </tr>
            <tr>
                <td class="col1">&nbsp;</td>
                <td class="col1">&nbsp;</td>
                <td class="col3">&nbsp;</td>
                <td class="auto-style1">&nbsp;</td>
                <td class="colinput">
                    &nbsp;</td>
                <td class="col1">&nbsp;</td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td class="col3; centra;" colspan="3" >
                    <div style="width: 220px; margin-right:30px; float: left;">
                    <asp:Button ID="cbStampa" runat="server" Text="Carica registro prenotazioni" Align="center" BackColor="#FFB94F" Width="200px" EnableTheming="True" Height="30px" OnClick="cbStampa_Click"  />
                    </div>
                    <div style="width: 140px; margin-right:10px; float: left;">
                    <asp:Button ID="cbStampa0" runat="server" Text="Carica e invia mail" Align="center" BackColor="#FFB94F" Width="130px" EnableTheming="True" Height="30px" OnClick="cbMail_Click"  />
                    </div>
                    <asp:Button ID="cbStampa1" runat="server" Text="Visualizza" Align="center" BackColor="#FFB94F" Width="120px" EnableTheming="True" Height="30px" OnClick="cbShow_Click"  />
                </td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td class="col3">
                    &nbsp;</td>
                <td class="auto-style1"></td>
                <td class="col1">
                    &nbsp;</td>
                <td class="col1"></td>
            </tr>
        </table>
    </div>
    <div style="clear: both; margin: 0px auto;">
    <asp:Panel ID="pElenco" runat="server" Visible="true" style="float: left; margin: 0px auto;">
        <div style="margin: 0px auto; padding-top: 7px">
        </div>
    </asp:Panel>
    </div>
    <asp:Panel ID="pRegistro" runat="server" Visible="false" class="centra">
        <br /><br />
        <asp:GridView ID="GWRegistro" runat="server" AllowSorting="false" AutoGenerateColumns="false" visible ="true" width="1200"
            PageSize="999" ShowHeaderWhenEmpty="True" Sortmode="Automatic" 
            class="centra" CellPadding="2" ForeColor="#333333" GridLines="None" Font-Size="Small">
        <Columns>
            <asp:Boundfield DataField="partenza" headertext="Data inizio" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="220px" HorizontalAlign="left" Wrap="false"/></asp:Boundfield>
            <asp:Boundfield DataField="arrivo" headertext="Data fine" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="220px" HorizontalAlign="left" Wrap="false"/></asp:Boundfield> 
            <asp:Boundfield DataField="targa" headertext="Targa" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="120px" HorizontalAlign="left" Wrap="false"/></asp:Boundfield> 
            <asp:Boundfield DataField="numero" headertext="Num." HeaderStyle-HorizontalAlign="center"><ItemStyle Width="80px" HorizontalAlign="center" Wrap="false"/></asp:Boundfield> 
            <asp:TemplateField HeaderText="Prenotante" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="300px" HorizontalAlign="left" Wrap="false"/>
                <ItemTemplate>
                    <asp:Label ID="Cognome" runat="server" Text='<%# Bind("cognome") %>'></asp:Label>
                    <asp:Label ID="Nome" runat="server" Text='<%# Bind("nome") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField> 
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

<hr class="hrbianca" style="height:16px; clear: both;" />
<div style="margin: 0px auto; text-align:center;">
<asp:Label ID="sStato" runat="server" style="margin: 0px auto; text-align: center; border-width: 1px; border-radius: 4px; Border-Color: blue; width: 1024px; box-sizing: border-box;"></asp:Label>
</div>

</div>
</asp:Content>

