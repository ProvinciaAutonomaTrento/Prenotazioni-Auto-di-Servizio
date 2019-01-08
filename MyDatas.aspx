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

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MyDatas.aspx.cs" Inherits="MyDatas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
    .lungo {
        position: relative;
        height: 109px;       
    
    }
    .sottoadx {
        position: absolute;
        right: 0px;
        top: 15px;
        max-width: 100%;               
        background-repeat: repeat-x;
        z-index: -2;
    }
    .dentroadx {
        position: absolute;
        top: 6px;
        right: 2px;
        width: 171px;
        height: 78px;
        border: none;
    }
        .auto-style2 {
            width: 6px;
        }
        .auto-style3 {
            height: 23px;
        }
        .auto-style4 {
            width: 6px;
            height: 23px;
        }
        .auto-style5 {
            width: 355px;
        }
        .auto-style6 {
            height: 23px;
            width: 355px;
        }
        .auto-style10 {
            width: 254px;
        }
        .auto-style12 {
            height: 23px;
            width: 254px;
        }
        .auto-style13 {
            width: 254px;
            height: 26px;
        }
        .auto-style14 {
            width: 168px;
        }
        .auto-style15 {
            height: 23px;
            width: 168px;
        }
        .auto-style16 {
            height: 26px;
            width: 168px;
        }
        .auto-testata {
            max-width: 100%;
            height: 121px;
            border: none;
            z-index: -1;
        }
    </style>
</head>
<body>
    <div class="centra"><img class="auto-testata" src="img/gassx300.png" alt="Logo Gestione flotta sx"/>
    <div class="dentroadx">
            <img class="auto-testata" src="img/gasdx300.png" alt="Logo Gestione flotta dx"/>                    
    </div>           
    </div>

    <form id="formmyapac" runat="server">
    <div>
    <div><br />
        <div style="text-align: center; margin: 0px auto;">
            <asp:HyperLink ID="hltophome" runat="server" NavigateUrl="~/Default.aspx" Target="_self" Align="Center">Home</asp:HyperLink>    
        </div><br />
        <table align="center" border="0" style="border: medium solid #1F5DB2; border-radius: 6px; padding: 10px">
            <tr>
                <td class="auto-style31"></td>
                <td class="auto-style2"></td>
                <td class="auto-style5"></td>
                <td class="auto-style10"></td>
                <td class="auto-style14"></td>
                <td class="auto-style23"></td>
            </tr>
            <tr>
                <td class="auto-style32"></td>
                <td class="auto-style2"></td>
                <td class="auto-style5" colspan="3" style="background-color: #F4AF4C; font-size: medium; font-weight: bold; color: #000000; border-radius: 3px; padding: 2px; box-sizing:padding-box; text-align: center;">I miei dati</td>
                <td class="auto-style24">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style5"></td>
                <td class="auto-style13"></td>
                <td class="auto-style14">&nbsp;</td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style5">Username (PRxxxxx)</td>
                <td class="auto-style13">
                    <asp:TextBox ID="tNikname" runat="server" Width="241px"></asp:TextBox>
                </td>
                <td class="auto-style14">&nbsp;</td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style31"></td>
                <td class="auto-style2"></td>
                <td class="auto-style5">Nome</td>
                <td class="auto-style10">
                    <asp:TextBox ID="tNome" runat="server" Width="241px"></asp:TextBox>
                </td>
                <td class="auto-style14"></td>
                <td class="auto-style23"></td>
            </tr>
            <tr>
                <td class="auto-style31">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style5">
                    Cognome</td>
                <td class="auto-style13">
                    <asp:TextBox ID="tCognome" runat="server" Width="241px"></asp:TextBox>
                <td class="auto-style14">&nbsp;</td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style5">E-mail</td>
                <td class="auto-style13">
                    <asp:TextBox ID="tMail1" runat="server" Width="241px"></asp:TextBox>
                </td>
                <td class="auto-style14">&nbsp;</td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style5">Matricola</td>
                <td class="auto-style13">
                    <asp:TextBox ID="tMatricola" runat="server" Width="241px" ></asp:TextBox>
                </td>
                <td class="auto-style14">&nbsp;</td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style5" >Ente di appartenenza / struttura</td>
                <td class="auto-style13">
                    <asp:DropDownList ID="ddlStruttura" runat="server" Width="249px" Height="16px" Visible="true"></asp:DropDownList>
                    <asp:TextBox ID="tEnte" runat="server" Width="241px" visible="false"></asp:TextBox>
                </td>
                <td class="auto-style14">non in elenco ?<asp:CheckBox ID="cbnoninelenco" runat="server" OnCheckedChanged="cbnoninelenco_CheckedChanged" autopostback="true"/></td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td></td>
                <td class="auto-style2"></td>
                <td class="auto-style5">Scadenza permesso di guida (patente)</td>
                <td class="auto-style10">
                    <asp:TextBox ID="tScadenza" runat="server" Width="241px" ></asp:TextBox>
                </td>
                <td class="auto-style14"></td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td class="auto-style2"></td>
                <td class="auto-style5">
                    Indirizzo (sede di lavoro)</td>
                <td class="auto-style10">
                    <asp:TextBox ID="tIndirizzo" runat="server" Width="241px" ></asp:TextBox>
                </td>
                <td class="auto-style14"></td>
                <td></td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style5">
                    Civico</td>
                <td class="auto-style13">
                    <asp:TextBox ID="tCivico" runat="server" Width="241px" ></asp:TextBox>
                </td>
                <td class="auto-style14">&nbsp;</td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style5">
                    CAP</td>
                <td class="auto-style13">
                    <asp:TextBox ID="tCap" runat="server" Width="241px" ></asp:TextBox>
                </td>
                <td class="auto-style14">&nbsp;</td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style5">Città</td>
                <td class="auto-style13">
                    <asp:TextBox ID="tCitta" runat="server" Width="241px" ></asp:TextBox>
                </td>
                <td class="auto-style14">&nbsp;</td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style5">Telefono</td>
                <td class="auto-style13">
                    <asp:TextBox ID="tTelefono" runat="server" Width="241px" ></asp:TextBox>
                </td>
                <td class="auto-style14">&nbsp;</td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style5">E-mail (ripeti email)</td>
                <td class="auto-style13">
                    <asp:TextBox ID="tMail2" runat="server" Width="241px" ></asp:TextBox>
                </td>
                <td class="auto-style14">&nbsp;</td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style3"></td>
                <td class="auto-style4"></td>
                <td class="auto-style6">Do il consenso al trattamento dei dati personali al solo fine della lavorazione della pratica</td>
                <td class="auto-style12">
                    <asp:CheckBox ID="cbConsenso" runat="server" />
                </td>
                <td class="auto-style15"></td>
                <td class="auto-style3"></td>
            </tr>
            <tr>
                <td class="auto-style34"></td>
                <td class="auto-style2"></td>
                <td class="auto-style5">
                </td>
                <td class="auto-style10"></td>
                <td class="auto-style16"></td>
                <td class="auto-style26"></td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style5" colspan="3" style="text-align: center;">
                    <asp:Button ID="cbRegistrati" runat="server" Text="Chiedi registrazione" Align="center" BackColor="#FFB94F" Width="250px" EnableTheming="True" OnClick="cbRegistrati_Click" Height="30px"/>
                </td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style31"></td>
                <td class="auto-style2"></td>
                <td class="auto-style5"></td>
                <td class="auto-style10"></td>
                <td class="auto-style14"></td>
                <td class="auto-style23"></td>
            </tr>
        </table>
    
    </div>
    <p>
        <asp:Panel ID="pPat" runat="server" Visible="false" Style="margin: 0px auto; text-align: center; color: red;">
            ATTENZIONE: i dipendenti provinciali sono già stati &#39;pre-registrati&#39;. Si prega quindi di contattare direttamente il call center per l&#39;abilitazione del proprio account, al numero <strong>0461-496415</strong>. Grazie.
        </asp:Panel>
    </p>    
    <div style="text-align: center; margin: 0px auto;">
         <asp:TextBox ID="tStato" runat="server" Border="1" Width="1200px" Align="Center" Style="text-align: center;"></asp:TextBox>
    </div>
    <div style="text-align: center; margin: 0px auto;">
        <asp:HyperLink ID="hlHome" runat="server" NavigateUrl="~/Default.aspx" Target="_self" Align="Center">Home</asp:HyperLink>    
    </div>
    </form>
</body>
</html>
