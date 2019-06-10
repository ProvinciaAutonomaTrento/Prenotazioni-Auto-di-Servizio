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

<%@ Page Title="Atrezzi operatore" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="editpre.aspx.cs" Inherits="editpre" %>


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
        Width: 1200px; margin: auto; background-color: lightsalmon; height: 6px;
    }
    .hrbianca {
        Width: 1200px; margin: auto; border: 2px; background-color: white; height: 8px;
        clear: both;
    }
    .content {
        width: 1360px;
        margin: 0px auto;
        text-align: center;
    }
    .container {
        position: relative;
        width: 1360px;
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
        width: 1200px;
        border-radius: 4px;
    }
    .tb {
        border-width: 1px; border-radius: 4px; Border-Color: blue;  box-sizing: border-box; text-align: left; padding: 1px; margin: 2px; float:left;
    }

    .menu { width: 1200px; margin: 0px auto; background-color: lightsalmon; border-radius: 6px; box-sizing:border-box; padding:3px; z-index: -22; height: 28px;}
    .col1 { width: 10px; }
    .col3 { width: 210px; text-align: left; }
    .col3a { width: 110px; text-align: left; }
    .colinput { width: 241px; text-align: left; }
    .boxleft { width: 220px; float: left; }
    .auto-style1 {
        width: 10px;
        height: 23px;
    }
    .auto-style2 {
        width: 241px;
        text-align: left;
        height: 23px;
    }
</style>
<br />
<div class="hrbianca"></div>
<div style="height: 22px; text-align: center; font-size: 22px;">PRENOTAZIONI</div>
    <br /><div class="hrbianca"></div>
<div class="menu">
    <div style="width: 770px; float: left; height:26px; text-align: left; ">
        <asp:Label ID="LBenvenuto" runat="server" Text=" "></asp:Label>
    </div> 
 
    <div style="float:left;"">
        <asp:Button ID="bUscita" runat="server" Text="Uscita" Width="90px" OnClick="bUscita_Click" Style="float: left; "/>
    </div>
    <div style="text-align: right; float: right;">
        <a href="menu.aspx?l=anagrafica"><img src="img/home.png" /></a>
    </div>
    <div style="float:right; text-align: right; width: 120px;">
        <asp:Button ID="bnuova" runat="server" Text="Nuova ricerca" Width="110px" Style="float: left; " Visible="false" OnClick="bnuova_Click"/>
    </div> 
</div>
 
<div class="hrbianca"></div>

<div id="sfondo" class="container" style="clear:both; margin: 0px auto; width: 1350px; text-align: center; background-color:cornsilk; border-width: 6px; border-radius: 4px; height:auto; box-sizing: border-box; ">
    <asp:Panel ID="pFiltro" runat="server" DefaultButton="cbCerca">
    <div id="colonna1" style="float:left; width: 430px; height: auto; margin-left: 200px;">
        <div style="padding-left:10px; margin: 0px auto; border-radius: 6px;">
        <table border="0" style="border: medium solid #1F5DB2; border-radius: 6px; padding: 10px; box-sizing:padding-box;">
            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td class="col3a"></td>
                <td class="col1"></td>
                <td class="colinput"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td class="col3a" colspan="3" style="background-color: #F4AF4C; font-size: medium; font-weight: bold; color: #000000; padding: 3px;">Ricerca per nome<br /></td>
                <td class="col1">&nbsp;</td>
            </tr>
            <tr>
                <td class="col1">&nbsp;</td>
                <td class="col1">&nbsp;</td>
                <td class="col3a">Username</td>
                <td class="col1">&nbsp;</td>                
                <td class="colinput">
                    <asp:TextBox ID="tNikname" runat="server" ></asp:TextBox>
                </td>
                <td class="col1">&nbsp;</td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td class="col3a">Nome</td>
                <td class="col1">&nbsp;</td>                
                <td class="colinput">
                    <asp:TextBox ID="tNome" runat="server"></asp:TextBox>
                </td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1">&nbsp;</td>
                <td class="col1">&nbsp;</td>
                <td class="col3a">Cognome</td>
                <td class="col1">&nbsp;</td>                
                <td class="colinput">
                    <asp:TextBox ID="tCognome" runat="server"  ></asp:TextBox>
                <td class="col1">&nbsp;</td>
            </tr>
            <tr>
                <td class="col1">&nbsp;</td>
                <td class="col1">&nbsp;</td>
                <td class="col3a">Matricola</td>
                <td class="col1">&nbsp;</td>
                <td class="colinput">
                    <asp:TextBox ID="tMatricola" runat="server" ></asp:TextBox>
                </td>
                <td class="col1">&nbsp;</td>
            </tr>
             <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td class="col3a"></td>
                <td class="col1"></td>
                <td class="colimput"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1">&nbsp;</td>
                <td class="col1">&nbsp;</td>
                <td class="col3a"><asp:Label ID="lPotere" runat="server">Potere</asp:Label></td>
                <td class="col1">&nbsp;</td>
                <td class="colimput" style="margin: 0px auto; text-align: left;"><asp:DropDownList ID="ddlPotere" runat="server"  Enabled="false"></asp:DropDownList></td>
                <td class="col1">&nbsp;</td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td class="col3a"></td>
                <td class="col1"></td>
                <td class="colinput"></td>
                <td class="col1"></td>
            </tr>
        </table>
    </div>
    <div id="scelterapide" style="padding-left:10px; margin: 0px; border-radius: 6px;">
        <table border="0" style="border: medium solid #1F5DB2; border-radius: 6px; padding: 10px; box-sizing:padding-box;">
            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td class="col3a"></td>
                <td class="col1"></td>
                <td class="colinput" ></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td class="col3a" colspan="3" style="background-color: #fcd081; font-size: medium; font-weight: bold; color: #000000; padding: 3px; text-align: center;">Ricerche rapide prenotazioni<br /></td>
                <td class="col1">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style4"></td>
                <td class="auto-style4"></td>
                <td colspan="3" class="centra"><asp:Button ID="cbRapido" runat="server" Text="di oggi" Align="left" BackColor="#FFB94F" Width="202px" EnableTheming="True" Height="30px" class="bradius" CommandArgument="oggi" OnCommand="cbRapido_Command"/></td>
                <td class="auto-style4"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td colspan="3" class="centra"><asp:Button ID="cbIeri" runat="server" Text="di ieri" Align="left" BackColor="#FFB94F" Width="202px" EnableTheming="True" Height="30px" class="bradius" CommandArgument="ieri" OnCommand="cbRapido_Command"/></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="auto-style1"></td>
                <td class="auto-style1"></td>
                <td colspan="3" class="centra"><asp:Button ID="cbDomani" runat="server" Text="di domani" Align="left" BackColor="#FFB94F" Width="202px" EnableTheming="True" Height="30px" class="bradius" CommandArgument="domani" OnCommand="cbRapido_Command"/></td>
                <td class="auto-style1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td colspan="3" class="centra"><asp:Button ID="cbSettimana" runat="server" Text="settimana corrente" Align="left" BackColor="#FFB94F" Width="202px" EnableTheming="True" Height="30px" class="bradius" CommandArgument="corrente" OnCommand="cbRapido_Command"/></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td colspan="3" class="centra"><asp:Button ID="cbSettimanapassata" runat="server" Text="settimana passata" Align="left" BackColor="#FFB94F" Width="202px" EnableTheming="True" Height="30px" class="bradius" CommandArgument="passata" OnCommand="cbRapido_Command"/></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td colspan="3" class="centra"><asp:Button ID="idSettimanaprosima" runat="server" Text="settimana prossima" Align="left" BackColor="#FFB94F" Width="202px" EnableTheming="True" Height="30px" class="bradius" CommandArgument="prossima" OnCommand="cbRapido_Command"/></td>
                <td class="col1"></td>
            </tr>
        </table>

        </table>
    </div>
    </div>
    
    <div style="padding-left:10px; width: 500px; height: auto; float:left; margin: 0px auto; border-radius: 6px;">
            <table border="0" style="border: medium solid #1F5DB2; border-radius: 6px; float: left; padding: 10px; box-sizing:padding-box;">
            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td class="col3"></td>
                <td class="col1"></td>
                <td class="col3"></td>
                <td class="col1"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td colspan="4" style="background-color: #F4AF4C; font-size: medium; font-weight: bold; color: #000000; padding: 3px;">Ricerca per targa, numero, data, sede<br /></td>
                <td class="col1"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td class="col3">Targa</td>
                <td class="col1"></td>                
                <td class="col3">
                    <asp:DropDownList ID="ddlTarga" runat="server"></asp:DropDownList>
                </td>
                <td class="col1">&nbsp;</td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td class="col3">Numero</td>
                <td class="col1"></td>                
                <td class="col3">
                    <asp:DropDownList ID="ddlNumero" runat="server"></asp:DropDownList>
                </td>
                <td class="col1"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td class="col3">Data Inizio</td>
                <td class="col1"></td>                
                <td class="col3">Data Fine</td>
                <td class="col1"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td class="col3"><asp:Calendar ID="cldDA" runat="server" BackColor="White" BorderColor="#999999" Caption="Seleziona giorno di inizio" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" widht="200px"></asp:Calendar></td>
                <td class="col1"></td>
                <td class="col3"><asp:Calendar ID="cldA" runat="server" BackColor="White" BorderColor="#999999" Caption="Seleziona giorno di fine" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" widht="200px"></asp:Calendar></td>
                <td class="col1"></td>
                <td class="col1"></td>
            </tr>
             <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td class="col3">Punto di prelievo</td>
                <td class="col1"></td>
                <td class="col1" colspan="2"><asp:DropDownList ID="ddlUbi" runat="server" width="254" Enabled="true"></asp:DropDownList></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1">&nbsp;</td>
                <td class="col1"></td>
                <td class="col3">&nbsp;</td>
                <td class="col1">&nbsp;</td>
                <td class="col3">&nbsp;</td>
                <td class="col1">&nbsp;</td>
                <td class="col1">&nbsp;</td>
            </tr>
        </table>
    </div>

    <br /><br>
    <div style="clear: both; margin: 0px auto; margin-top: 25px; text-align: center; width:800px">
        <div style="width: 400px; margin: 0px auto; text-align: center; float: left;">
            <asp:Button ID="cbCerca" runat="server" Text="Cerca" Align="center" BackColor="#FFB94F" Width="380px" EnableTheming="True"  Height="30px" OnClick="cbCerca_Click"  />
        </div>
        <div style="width: 400px; margin: 0px auto; float: right;">
            <asp:Button ID="cbReset" runat="server" Text="Reset" Align="center" BackColor="#FFB94F" Width="150px" EnableTheming="True"  Height="30px" OnClick="cbReset_Click"  />
        </div>
    </div>
    </asp:Panel>
    <p class="centra-text clearfix">
        <asp:TextBox ID="sStato" runat="server" style="border-width: 1px; border-radius: 4px; Border-Color: blue; width: 1350px; box-sizing: border-box;"></asp:TextBox>
    </p>
</div>

<hr class="hrbianca" />

<div style="clear: both; margin: 0px auto">
    <asp:Panel ID="Panel1" runat="server" Visible="true" style="clear: both; margin: 0px auto; width: 1350px">
        <div style="margin: 0px auto; padding-top: 7px">          
        <asp:GridView ID="gwPrenotazioni" runat="server"  AutoPostBack="true" AutoGenerateColumns="false" Visible="false" AllowSorting="True"
            DataKeyNames="id" PageSize="999" width="1350" ShowHeaderWhenEmpty="True" HorizontalAlign="Center"
            style="text-align: left; float: left; " CellPadding="2" ForeColor="#333333" GridLines="None" Font-Size="Small"
            OnSelectedIndexChanged="gPrenotazioni_SelectedIndexChanged" 
            OnRowDeleting="gPrenotazioni_RowDeleting"  
            OnRowDataBound="gwPrenotazioni_RowDataBound" 
            OnSorting = "gwPrenotazioni_Sorting"  >
           
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BorderStyle="Solid" BackColor="#506C8C" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            
            <Columns>
            <asp:TemplateField HeaderText="Modifica" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="75px" HorizontalAlign="left" Wrap="false"/>
                <ItemTemplate>
                    <asp:Button runat="Server" align="left" ButtonType="Button" SelectText="Modifica" ShowSelectButton="True" Text="Modifica" CommandName="select"></asp:Button>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Cancella" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="75px" HorizontalAlign="left" Wrap="false"/>
                <ItemTemplate>
                    <asp:Button runat="Server" align="left" ButtonType="Button" SelectText="Cancella" ShowDeleteButton="True" Text="Cancella" CommandName="delete"></asp:Button>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="partenza" headertext="Partenza" HeaderStyle-HorizontalAlign="center" sortexpression="partenza"><ItemStyle Width="140px" HorizontalAlign="left" Wrap="false" /></asp:Boundfield>
            <asp:BoundField DataField="arrivo" headertext="Arrivo" HeaderStyle-HorizontalAlign="center" sortexpression="arrivo"><ItemStyle Width="140px" HorizontalAlign="left" Wrap="false"/></asp:Boundfield>
            <asp:BoundField DataField="dove_comune" headertext="Destinazione" HeaderStyle-HorizontalAlign="left" sortexpression="dove_comune"><ItemStyle Width="160px" HorizontalAlign="left" /></asp:Boundfield>            
            <asp:TemplateField>
            <HeaderTemplate>
                <asp:LinkButton ID="LinkButtonMarca" runat="server" Text="Marca, modello" CommandName="Sort" CommandArgument="marca, modello" Width="295px" HorizontalAlign="left" Wrap="true" linkcolor="white" linkvisitedcolor="white" DisabledLinkColor="white" LinkBehavior="white" style="color: white;"></asp:LinkButton>
            </HeaderTemplate>                            
            <ItemTemplate>
                <asp:Label ID="ltmarca" runat="server" Text='<%# Bind("marca") %>'></asp:Label>
                <asp:Label ID="modello" runat="server" Text='<%# Bind("modello") %>'></asp:Label>
            </ItemTemplate>
            </asp:TemplateField>        
            <asp:boundfield datafield="targa" headertext="Targa" HeaderStyle-HorizontalAlign="center" headerstyle-wrap="false" sortexpression="targa"><ItemStyle Width="70px" HorizontalAlign="center" /></asp:Boundfield>          
            <asp:boundfield datafield="numero" headertext="Num." HeaderStyle-HorizontalAlign="center" headerstyle-wrap="false" sortexpression="numero"><ItemStyle Width="50px" HorizontalAlign="center" /></asp:Boundfield>
            <asp:BoundField DataField="telefono" headertext="telefono" HeaderStyle-HorizontalAlign="center"><ItemStyle Width="75px" HorizontalAlign="right" /></asp:Boundfield>
            <asp:BoundField DataField="ubicazione" headertext="Ritiro" HeaderStyle-HorizontalAlign="left" SortExpression="ubicazione"><ItemStyle Width="180px" HorizontalAlign="left" /></asp:Boundfield>  
            <asp:TemplateField  SortExpression="cognome, nome">
            <HeaderTemplate>
                <asp:LinkButton ID="LinkButtonPrenotante" runat="server" Text="Prenotante" CommandName="Sort" CommandArgument="cognome, nome" Width="150px" HorizontalAlign="left" Wrap="true" linkcolor="white" linkvisitedcolor="white" DisabledLinkColor="white" LinkBehavior="white" style="color: white;"></asp:LinkButton>
            </HeaderTemplate>  
            <ItemTemplate>
                <asp:Label ID="Cognome" runat="server" Text='<%# Bind("cognome") %>'></asp:Label>
                <asp:Label ID="Nome" runat="server" Text='<%# Bind("nome") %>'></asp:Label>
            </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="id" visible="false"/>
            <asp:BoundField DataField="user_ek" visible="false"><ItemStyle Width="1px" Wrap="false" /></asp:Boundfield>
            <asp:Boundfield DataField="colore" headertext="Colore" HeaderStyle-HorizontalAlign="center"><ItemStyle Width="0px" HorizontalAlign="center" Wrap="false"/></asp:Boundfield> 
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
        </div>
    </asp:Panel>
</div>
<div>
    <asp:Panel ID="pSeiSicuro" runat="server" Visible="true" style="float: left; margin: 0px auto;">
   
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
            <asp:TableCell runat="server">Prenotante:</asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"><asp:Label ID="lPrenotante" runat="server"></asp:Label></asp:TableCell>
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
        <asp:TableRow runat="server" HorizontalAlign="Left" Visible="false">
            <asp:TableCell runat="server" Width="10px"></asp:TableCell>
            <asp:TableCell runat="server">viaggiatori oltre al conducente:</asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"><asp:Label ID="lPasseggeri" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" HorizontalAlign="Left" ID="TableRow1" Visible="true">
            <asp:TableCell runat="server" Width="10px"></asp:TableCell>
            <asp:TableCell runat="server">numero e targa:</asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"><asp:Label ID="lNumero" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" HorizontalAlign="Left" ID="trVeicolo" Visible="false">
            <asp:TableCell runat="server" Width="10px"></asp:TableCell>
            <asp:TableCell runat="server">autoveicolo:</asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"><asp:Label ID="lVeicolo" runat="server"></asp:Label></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" HorizontalAlign="Left" ID="TableRow2" Visible="true">
            <asp:TableCell runat="server" Width="10px"></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" HorizontalAlign="Left" ID="TableRow3" Visible="true">
            <asp:TableCell runat="server" Width="10px"></asp:TableCell>
            <asp:TableCell runat="server" ColumnSpan="2" HorizontalAlign="Center"><asp:Button ID="cbcConferma" runat="server" Text="Conferma richiesta di cancellazione!" Visible="true" OnClick="cbcCancella_Click" /></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server" HorizontalAlign="Center"><asp:Button ID="cbHome" runat="server" Text="Ritorna a risultati" Visible="true" OnClick="cbhome_Click" /></asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    </asp:Panel>
</div>
 
</asp:Content>

