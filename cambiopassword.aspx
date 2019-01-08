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


<%@ Page Title="Cambio password" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="cambiopassword.aspx.cs" Inherits="cambiopasswrd" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

    <style type="text/css">
        .centro {
            align-content="center";
        }
        .auto-style1 {
            width: 10px;
        }
        .auto-style3 {
            width: 186px;
        }
        .auto-style5 {
            width: 181px;
        }
        .auto-style7 {
            width: 677px;
        }
        .auto-style8 {
            width: 263px;
        }
        .auto-style9 {
            height: 23px;
        }
        .auto-style10 {
            width: 10px;
            height: 23px;
        }
        .auto-style11 {
            width: 186px;
            height: 23px;
        }
        .auto-style12 {
            width: 181px;
            height: 23px;
        }
        .auto-style13 {
            width: 263px;
            height: 23px;
        }
    </style>
 
    <br />
<div class="hrbianca"></div>
<div style="height: 22px; text-align: center; font-size: 22px;">CAMBIO PASSWORD</div>
    <br /><div class="hrbianca"></div>

    <div>
       <table align="center" border="0" style="border: medium solid #1F5DB2;" class="auto-style7">
            <tr>
                <td class="auto-style31"></td>
                <td class="auto-style1"></td>
                <td class="auto-style3"></td>
                <td class="auto-style5"></td>
                <td class="auto-style8"></td>
                <td class="auto-style23"></td>
            </tr>
            <tr>
                <td class="auto-style32"></td>
                <td class="auto-style1"></td>
                <td class="auto-style3">Cambio password</td>
                <td class="auto-style5"></td>
                <td class="auto-style8"></td>
                <td class="auto-style24">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style1">&nbsp;</td>
                <td class="auto-style3">&nbsp;</td>
                <td class="auto-style5">&nbsp;</td>
                <td class="auto-style8">&nbsp;</td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style31"></td>
                <td class="auto-style1"></td>
                <td class="auto-style3">Vecchia password</td>
                <td class="auto-style5">
                    <asp:TextBox ID="tVecchia" runat="server" Width="160px" TextMode="Password"></asp:TextBox>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Asterisvp" runat="server" BorderStyle="None" Width="11px" Enabled="false"></asp:Label>
                    <asp:Label ID="lpwd" runat="server" Enabled="False" ></asp:Label>
                </td>
                <td class="auto-style23"></td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style1">&nbsp;</td>
                <td class="auto-style3"></td>
                <td class="auto-style5"></td>                
                <td class="auto-style8">&nbsp;</td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style1">&nbsp;</td>
                <td class="auto-style3">Nuova password</td>
                <td class="auto-style5">
                    <asp:TextBox ID="tNuovaPwd" runat="server" Width="160px" TextMode="Password"></asp:TextBox>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Asterisconp" runat="server" BorderStyle="None" Width="11px" Enabled="false"></asp:Label>
                    <asp:Label ID="lnuova" runat="server" Enabled="False"></asp:Label>
                </td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style1">&nbsp;</td>
                <td class="auto-style3">Conferma password</td>
                <td class="auto-style5">
                    <asp:TextBox ID="tNuovaPwd2" runat="server" Width="160px" TextMode="Password"></asp:TextBox>
                </td>
                <td class="auto-style8">
                    <asp:Label ID="Asterisconp2" runat="server" BorderStyle="None" Width="11px" Enabled="false"></asp:Label>
                    <asp:Label ID="lconferma" runat="server" Enabled="False"></asp:Label>
                </td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style1">&nbsp;</td>
                <td class="auto-style3" ></td>
                <td class="auto-style5">
                    &nbsp;</td>
                <td class="auto-style8">&nbsp;</td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style1">&nbsp;</td>
                <td class="auto-style3">
                    &nbsp;</td>
                <td class="auto-style5">
                    <asp:TextBox ID="Asteriscopwd" runat="server" BorderStyle="None" Width="11px"></asp:TextBox>
                </td>
                <td class="auto-style8">&nbsp;</td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style9"></td>
                <td class="auto-style10"></td>
                <td class="auto-style11"></td>
                <td class="auto-style12"></td>
                <td class="auto-style13"></td>
                <td class="auto-style9"></td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style1">&nbsp;</td>
                <td class="auto-style3">
                    <asp:Button ID="cbConferma" runat="server" Text="Conferma" Align="center" BackColor="#FFB94F" Width="186px" EnableTheming="True" OnClick="cbRegistrati_Click"/>
                </td>
                <td class="centro">          
                    &nbsp;</td>
                <td class="auto-style8">          
              <asp:Button ID="Login" runat="server" Text="Logout" Font-Size="X-Small" OnClick="Logout_Click" CssClass="centro" />
                </td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style31"></td>
                <td class="auto-style1"></td>
                <td class="auto-style3"></td>
                <td class="auto-style5"></td>
                <td class="auto-style8"></td>
                <td class="auto-style23"></td>
            </tr>
        </table>
    </div>
    <br />
    <div style="text-align: center; margin: 0px auto;">
        <asp:TextBox ID="tStato" runat="server" BorderStyle="None" Width="1200px" Align="Center" style="text-align: center;"></asp:TextBox>
    </div>
    <p style="text-align:center;">
        <asp:HyperLink ID="hlHome" runat="server" NavigateUrl="~/Default.aspx" Target="_self">Home</asp:HyperLink>
    </p> 
</asp:Content>
