<%--
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

<%@ Page Title="Gestione flotta provinciale" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .bradius { border-radius: 4px; box-sizing: border-box; }    
    </style>
    <div class="jumbotron">     
     <div style="text-align: center; margin: 0px auto;">
         <br />
         <br />
        <div>
        <table border="0" style="border: medium solid #1F5DB2; border-radius: 4px; box-sizing: border-box; margin: 0px auto;">
            <tr>
                <td class="auto-style35"></td>
                <td class="auto-style36"></td>
                <td class="auto-style48" colspan="2"></td>
                <td class="auto-style1"></td>
                <td class="auto-style38"></td>
                <td class="auto-style39"></td>
            </tr>
            <tr>
                <td class="auto-style40">&nbsp;</td>
                <td class="auto-style41">&nbsp;</td>
                <td class="auto-style50" colspan="2">&nbsp;</td>
                <td class="auto-style1">&nbsp;</td>
                <td class="auto-style43">&nbsp;</td>
                <td class="auto-style44">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style35"></td>
                <td class="auto-style36"></td>
                <td class="auto-style48" colspan="2">Username</td>
                <td class="auto-style1">
                    <asp:TextBox ID="nikname" runat="server" Width="193px"></asp:TextBox>
                </td>
                <td class="auto-style38">
                   
                    <asp:Label ID="lAsterisconn" runat="server" visible="false"></asp:Label>
                   
                </td>
                <td class="auto-style39"></td>
            </tr>
            <tr>
                <td class="auto-style40">&nbsp;</td>
                <td class="auto-style41">&nbsp;</td>
                <td class="auto-style50" colspan="2">
                    (matricola)</td>
                <td class="auto-style1">
                   
                    &nbsp;</td>
                <td class="auto-style43">&nbsp;</td>
                <td class="auto-style44">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style40">&nbsp;</td>
                <td class="auto-style41">&nbsp;</td>
                <td class="auto-style50" colspan="2">&nbsp;</td>
                <td class="auto-style1">&nbsp;</td>
                <td class="auto-style43">&nbsp;</td>
                <td class="auto-style44">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style40">&nbsp;</td>
                <td class="auto-style41">&nbsp;</td>
                <td class="auto-style47" >Password</td>
                <td class="auto-style2" >&nbsp;</td>
                <td class="auto-style1">
                    <asp:TextBox ID="password" runat="server" Width="193px" TextMode="Password"></asp:TextBox>
                </td>
                <td class="auto-style43">

                    <asp:Label ID="lAsteriscopwd" runat="server" Visible ="false"></asp:Label>

                </td>
                <td class="auto-style44">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style40">&nbsp;</td>
                <td class="auto-style41">&nbsp;</td>
                <td class="auto-style50" colspan="2">
                    &nbsp;</td>
                <td class="auto-style1">

                    &nbsp;</td>
                <td class="auto-style43">&nbsp;</td>
                <td class="auto-style44">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style40">&nbsp;</td>
                <td class="auto-style41">&nbsp;</td>
                <td class="auto-style50" colspan="2">&nbsp;</td>
                <td class="auto-style1">&nbsp;</td>
                <td class="auto-style43">&nbsp;</td>
                <td class="auto-style44">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style40">&nbsp;</td>
                <td class="auto-style41">&nbsp;</td>
                <td class="auto-style50" colspan="2">
                    &nbsp;</td>
                <td class="auto-style1">
                    <asp:Button ID="cbAccedi" runat="server" Text="Accedi" Align="center" BackColor="#FFB94F" Width="202px" EnableTheming="True" OnClick="cbAccedi_Click" Height="30px" class="bradius"/>
                </td>
                <td >                 
                </td>
                <td class="auto-style44">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style40">&nbsp;</td>
                <td class="auto-style41">&nbsp;</td>
                <td class="auto-style50" colspan="2">&nbsp;</td>
                <td class="auto-style1">&nbsp;</td>
                <td class="auto-style43">&nbsp;</td>
                <td class="auto-style44">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style34"></td>
                <td class="auto-style12"></td>
                <td class="auto-style13" colspan="2"></td>
                <td class="auto-style1"></td>
                <td class="auto-style11"></td>
                <td class="auto-style26"></td>
            </tr>
            <tr>
                <td class="auto-style40">&nbsp;</td>
                <td class="auto-style41">&nbsp;</td>
                <td class="auto-style50" colspan="2">&nbsp;</td>
                <td >
                    <asp:Button ID="cbRegistrati" runat="server" Text="Registrati" Width="156px" border="2px" OnClick="cbRegistrati_Click" onmouseover="omiServizio(this);" onmouseout="omoServizio(this);" style="text-align: center;" />
                 </td>
                <td class="centra">
                    <a href = "data/20181204_Registrazione.pdf" target="_blank">Istruzioni</a>
                </td>
                <td class="auto-style44">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style40">&nbsp;</td>
                <td class="auto-style41">&nbsp;</td>
                <td>                    
                </td>
                <td class="auto-style2">&nbsp;</td>
                <td align="center">
                    <asp:Button ID="cbpwddimenticata" runat="server" class="centra" Text="Password dimenticata"  Width="156px" border="2px" OnClick="cbpwddimenticata_Click1" Height="26px"/>
                </td>
                <td class="auto-style44">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style35"></td>
                <td class="auto-style36"></td>
                <td class="auto-style48" colspan="2"></td>
                <td class="auto-style1"></td>
                <td class="auto-style38"></td>
                <td class="auto-style39"></td>
            </tr>
        </table>
    </div>   
    </div>
<div style="height: 100px"></div>
<p class="clb" style="margin: 0px auto; width: 1024px;">
<asp:TextBox ID="sStato" runat="server" style="margin: 0px auto; text-align: center; border-width: 1px; border-radius: 3px; Border-Color: blue; width: 1024px; box-sizing: border-box;" ></asp:TextBox>
</p>
   <p style="clear: both; text-align: center; height: 35px;">&nbsp;Provincia Autonoma di Trento - SERV. SICUREZZA E GESTIONI COMUNI - GESTIONE AUTO DI SERVIZIO tel. 0461-496415 -&nbsp; ver. 1.7.6<br /><span style="font-size: smaller;">Sistema ottimizzato per l&#39;uso con browser: <font color="blue">google chrome e firefox</font></span></p>
</div>
<div style="margin: 0px auto; text-align: center;">
<div id="idHelp" style="margin: 0px auto; border-color: darkblue; border:2px; border-width: 2px; width: 1200px; height: 86px; padding: 4px; box-sizing:padding-box; border-radius: 4px;"></div>     
<script type="text/javascript">
function omiServizio(x) {
    var s = document.getElementById("idHelp");
    s.style.backgroundColor = "white";
    switch (x.id)
    {
        //case "ctl00_MainContent_ddlServizio": s.innerHTML = "Consulenza: si chiede la consulenza, la gara sarà bandita dal richiedente; Stazione appaltante: la gara sarà bandita da APAC per conto dell'ente rihiedente."; break;
        case "MainContent_cbRegistrati": s.innerHTML = "ATTENZIONE: i dipendenti provinciali, che non hanno già ricevuto le credenziali e intendono usufrire del servizio di prenotazione, possono chiamare il numero 0461-496415. Gli operatori potranno attivare l'account pre-registrato in tempi rapidi."; s.style.backgroundColor = "lightblue"; break;
    }           
}
function omoServizio(x) {
    var s = document.getElementById("idHelp");
    s.innerHTML = ""; s.style.backgroundColor = "white";
}
</script>
</div>
</asp:Content>
