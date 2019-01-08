<%--
/* 
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

<%@ Page Title="Atrezzi operatore" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="anagrafica.aspx.cs" Inherits="anagrafica"  %>


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

    .menu { width: 1024px; margin: 0px auto; background-color: lightsalmon; border-radius: 6px; box-sizing:border-box; padding:3px; z-index: -22; height: 28px;}
    .col1 {
        width: 10px;
    }
    .col3 {
        width: 130px;
        text-align: left;
    }
    .colinput {
        width: 150px;
        text-align:left;
    }
    .boxleft {
        width: 220px;
        float: left;
    }
    .menusx {
        border: 4px solid; 
        width: 333px; height: auto; 
        border-color: green; 
        background-color: lightgreen; 
        text-align: left; 
        border-radius: 6px; 
        padding: 10px; 
        box-sizing: border-box;
        float: left;
    }
</style>
<br />
<div class="hrbianca"></div>
<div style="height: 22px; text-align: center; font-size: 22px;">ANAGRAFICA UTENTI</div><br /><div class="hrbianca"></div>
<div class="menu">
    <div style="width: 890px; float: left; height:26px; text-align: left; ">
        <asp:Label ID="LBenvenuto" runat="server" Text=" "></asp:Label>
    </div>    
    <asp:Button ID="bUscita" runat="server" Text="Uscita" Width="90px" OnClick="bUscita_Click" Style="float: left; "/>
    <a href="menu.aspx?l=anagrafica"><img src="img/home.png" /></a>
</div>
<hr class="hrbianca" />    
<div class="hrbianca"></div><br />
<asp:Panel runat="server" DefaultButton ="cbCercaa" Style="clear: both; margin: 0px auto;">
<div style="background-color:cornsilk; border-width: 6px; border-radius: 4px; box-sizing: border-box;  margin: 0px auto;">
    <div style="margin: 0px auto; text-align: center;">
            <table border="0" style="border: medium solid #1F5DB2; border-radius: 6px;  margin: 0px auto;" >
            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td class="col3"></td>
                <td class="auto-style1"></td>
                <td class="col1"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td class="col3" colspan="3" style="background-color: #F4AF4C; font-size: medium; font-weight: bold; color: #000000; padding: 3px;">Inserisci i dati di ricerca<br /></td>
                <td class="col1">&nbsp;</td>
            </tr>
            <tr>
                <td class="col1">&nbsp;</td>
                <td class="col1">&nbsp;</td>
                <td class="col3">Username</td>
                <td class="auto-style1">&nbsp;</td>                
                <td class="colinput">
                    <asp:TextBox ID="tNikname" runat="server" Width="241px"></asp:TextBox>
                </td>
                <td class="col1">&nbsp;</td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td class="col3">Nome</td>
                <td class="auto-style1">&nbsp;</td>                
                <td class="colinput">
                    <asp:TextBox ID="tNome" runat="server" Width="241px" ></asp:TextBox>
                </td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1">&nbsp;</td>
                <td class="col1">&nbsp;</td>
                <td class="col3">Cognome</td>
                <td class="auto-style1">&nbsp;</td>                
                <td class="colinput">
                    <asp:TextBox ID="tCognome" runat="server" Width="241px" ></asp:TextBox>
                <td class="col1">&nbsp;</td>
            </tr>
            <tr>
                <td class="col1">&nbsp;</td>
                <td class="col1">&nbsp;</td>
                <td class="col3">Matricola</td>
                <td class="auto-style1">&nbsp;</td>
                <td class="colinput">
                    <asp:TextBox ID="tMatricola" runat="server" Width="241px"></asp:TextBox>
                </td>
                <td class="col1">&nbsp;</td>
            </tr>
             <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td class="col3"></td>
                <td class="auto-style1"></td>
                <td class="col1"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1">&nbsp;</td>
                <td class="col1">&nbsp;</td>
                <td class="col3"><asp:Label ID="Label1" runat="server">Solo abilitato</asp:Label></td>
                <td class="col1">&nbsp;</td>
                <td class="colimput" style="margin: 0px auto; text-align: left;"><asp:CheckBox  ID="cbAbilitato" runat="server" TextAlign="Left"/></td>
                <td class="col1">&nbsp;</td>
            </tr>
            <tr>
                <td class="col1">&nbsp;</td>
                <td class="col1">&nbsp;</td>
                <td class="col3"><asp:Label ID="lPotere" runat="server">Potere</asp:Label></td>
                <td class="col1">&nbsp;</td>
                <td class="colimput" style="margin: 0px auto; text-align: left;"><asp:DropDownList ID="ddlPotere" runat="server" width="204"></asp:DropDownList></td>
                <td class="col1">&nbsp;</td>
            </tr>
            <tr>
                <td class="col1">&nbsp;</td>
                <td class="col1">&nbsp;</td>
                <td class="col3">&nbsp;</td>
                <td class="auto-style1">&nbsp;</td>
                <td class="col1">&nbsp;</td>
                <td class="col1">&nbsp;</td>
            </tr>
            <tr>
                <td class="col1">&nbsp;</td>
                <td class="col1">&nbsp;</td>
                <td class="col3" colspan="3" style="text-align: center;">
                    <asp:Button ID="cbCercaa" runat="server" Text="Cerca" Align="left" BackColor="#FFB94F" Width="150px" EnableTheming="True"  Height="30px" OnClick="cbCerca_Click"   UseSubmitBehavior="False"/>                    
                    <asp:Button ID="cbreset" runat="server" Text="Nuova ricerca" Align="center" BackColor="#FFB94F" Width="150px" EnableTheming="True"  Height="30px" OnClick="cbReset_Click"   UseSubmitBehavior="False" Visible ="false"/>                    
                </td>
                <td class="col1">&nbsp;</td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td class="col3"></td>
                <td class="auto-style1"></td>
                <td class="col1"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1">&nbsp;</td>
                <td class="col1">&nbsp;</td>
                <td class="col3" colspan="3" style="text-align: center;">
                    <asp:Button ID="cbSetPwd" runat="server" Text="Set Password" Align="center" BackColor="#FFB94F" Width="150px" EnableTheming="True" OnClick="cbSetPwd_Click"   UseSubmitBehavior="False"  Style="float: left;"/>
                    <asp:TextBox ID="tpassword" runat="server" Width="200px" ></asp:TextBox>
                </td>
                <td class="col1">&nbsp;</td>
            </tr>


            <tr>
                <td class="col1"></td>
                <td class="col1"></td>
                <td class="col3"></td>
                <td class="auto-style1"></td>
                <td class="col1"></td>
                <td class="col1"></td>
            </tr>
        </table>
    </div>

    <div style="clear: both; margin: 0px auto;">
    <asp:Panel ID="pElenco" runat="server" Visible="true" style="float: left; margin: 0px auto;">
        <div style="margin: 0px auto; padding-top: 7px">
        <asp:GridView ID="gElenco" runat="server"  AutoPostBack="true" AutoGenerateColumns="false" Visible="false"
            DataKeyNames="id" PageSize="999" ShowHeaderWhenEmpty="True" HorizontalAlign="Center"
            style="text-align: left; float: left; " CellPadding="2" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="gElenco_SelectedIndexChanged" Font-Size="Small" >
            <Columns>
            <asp:CommandField ButtonType="Button" SelectText="Seleziona" ShowSelectButton="True"><ItemStyle Width="90px" /></asp:CommandField>
            <asp:BoundField DataField="Nikname" headertext="User" HeaderStyle-HorizontalAlign="left" Visible="true"><ItemStyle  Width="60px" HorizontalAlign="left" /></asp:Boundfield>
            <asp:BoundField DataField="Cognome" headertext="Cognome" HeaderStyle-HorizontalAlign="left" Visible="true"><ItemStyle Width="100px" HorizontalAlign="left" /></asp:Boundfield>
            <asp:BoundField DataField="Nome" headertext="Nome" HeaderStyle-HorizontalAlign="left" Visible="true"><ItemStyle Width="90px" HorizontalAlign="left" /></asp:Boundfield>
            <asp:BoundField DataField="matricola" headertext="matr." HeaderStyle-HorizontalAlign="right"><ItemStyle Width="50px" HorizontalAlign="right" /></asp:Boundfield>
            <asp:Boundfield DataField="struttura_cod" headertext="cod. str." ><ItemStyle Width="50px" HorizontalAlign="left" /></asp:Boundfield>
            <asp:Boundfield DataField="struttura" headertext="struttura" ><ItemStyle Width="310px" HorizontalAlign="left" /></asp:Boundfield>
            <asp:BoundField DataField="mail" headertext="e-mail" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="160px" HorizontalAlign="left" /></asp:Boundfield>
            <asp:BoundField DataField="telefono" headertext="tel." HeaderStyle-HorizontalAlign="right"><ItemStyle Width="120px" HorizontalAlign="right" /></asp:Boundfield>
            <asp:BoundField DataField="id" visible="false"/>
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

<hr class="hrbianca" style="height:16px;" />
<p class="centra-text clearfix">
<asp:TextBox ID="sStato" runat="server" style="border-width: 1px; border-radius: 4px; Border-Color: blue; width: 1024px; box-sizing: border-box;"></asp:TextBox>
</p>

</div>
</asp:Panel>
</asp:Content>


