 <%--/*
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
 */--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="schedagruppo.aspx.cs" Inherits="schedagruppo" %>


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
        margin: 0 auto;
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
        padding: 5px; Width: 1024px; margin: auto; background-color: lightsalmon; height: 6px;
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
        width: 1366px;
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
    .menu_box {
        padding: 5px;
        background-color: salmon;
        width: 1008px;
        height: 25px;
        border-radius: 4px;
        margin: 0px auto;
        text-align: center;
    }
    .tb {
        border-width: 1px; border-radius: 4px; Border-Color: blue;  box-sizing: border-box; text-align: left; padding: 1px; margin: 2px; float:left;
    }
        .nascondi {
            display: none;
        }
</style>
<br />
<div class="hrbianca"></div>
<div class="centra-text" style="font-size: 22px; height: 22px;">SCHEDA COMPOSIZIONE AUTO DI FLOTTA</div><br /><div class="hrbianca"></div>
<div class="hrbianca"></div>
<div class="container" style="background-color:white; border-radius: 4px; height:auto;">
    <div class="menu_box">
    <div style="clear:both;  width: 200px;text-align: left; float: left;">Seleziona etichetta flotta</div>
    <div style="width: 370px; text-align: left; float: left;">
        <asp:DropDownList ID="ddl" runat="server" AutoPostBack="true" Width="350px" OnSelectedIndexChanged="ddlGruppo_SelectedIndexChanged" style="float: left;" Font-Size="Medium"></asp:DropDownList>
    </div>
    <div style="text-align: left; width: 380px; float: left"><pre> </pre></div>
    
    <a href="menu.aspx?l=gruppo" style="float: left;"><img src="img/home.png" alt="home page"/></a>
    </div>
    
    <div style="clear: both;"> </div>
    <hr class="hrbianca" style="height:6px;" />
    <p class="centra-text clearfix">
    <asp:Label ID="sStato" runat="server" style="border-width: 1px; border-radius: 4px; Border-Color: blue; width: 1200px; box-sizing: border-box;"></asp:Label>
    </p>   
    <div > 
        <asp:Label ID="lGruppo" runat="server" Width="660px" Text="Veicoli nella flotta" style="float: left; background-color: salmon;  border-top-right-radius: 6px; border-top-left-radius: 6px;"></asp:Label>
        <div style="width: 14px; height:auto; float: left"> </div>
        <asp:Label ID="LEscluse" runat="server" Width="660px" Text="Veicoli non ancora associati alla flotta: " style="clear:both;background-color: salmon; border-top-right-radius: 6px; border-top-left-radius: 6px; "></asp:Label>
        <asp:GridView ID="gwAuto" runat="server" AllowSorting="True" Width="660px" AutoPostBack="true" Visible="true"
            DataKeyNames="ida" PageSize="999" ShowHeaderWhenEmpty="True" AutoGenerateColumns="false"
            style="text-align: left; float:left; border: 3px blue; border-radius: 3px; height: auto;" CellPadding="2" ForeColor="#333333" GridLines="None" 
            OnSelectedIndexChanged="gwAuto_SelectedIndexChanged" 
            OnSorting = "gwAuto_Sorting"
            Font-Size="Small">
        <Columns>
            <asp:CommandField ButtonType="Button" SelectText="Togli" ShowSelectButton="True" ShowCancelButton="False" headertext="Comando"><ItemStyle Width="50px" HorizontalAlign="center" Wrap="false"/></asp:CommandField>            
            <asp:Boundfield DataField="numero" headertext="Num." HeaderStyle-HorizontalAlign="center" sortexpression="numero">
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            <ItemStyle Width="35px" HorizontalAlign="center" Wrap="false"/></asp:Boundfield>
            
            <asp:Boundfield DataField="targa" headertext="Targa" HeaderStyle-HorizontalAlign="center" sortexpression="targa">
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            <ItemStyle Width="50px" HorizontalAlign="center" Wrap="false"/></asp:Boundfield>

            <asp:Boundfield DataField="marca" headertext="Marca" HeaderStyle-HorizontalAlign="left" sortexpression="marca">
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
            <ItemStyle Width="70px" HorizontalAlign="left" Wrap="true"/></asp:Boundfield> 
            
            <asp:Boundfield DataField="modello" headertext="Modello" HeaderStyle-HorizontalAlign="left" sortexpression="modello">
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
            <ItemStyle Width="220px" HorizontalAlign="left" Wrap="true"/></asp:Boundfield> 

            <asp:TemplateField  SortExpression="Ubicazione">
            <HeaderTemplate>
                <asp:LinkButton ID="Ubicazione" runat="server" Text="Ubicazione" CommandName="Sort" CommandArgument="comune, ubicazione, via, civico" Width="150px" HorizontalAlign="left" wrap="true" linkcolor="white" linkvisitedcolor="white" DisabledLinkColor="white" LinkBehavior="white"></asp:LinkButton>
            </HeaderTemplate>  
            <ItemTemplate>
                <asp:Label ID="Comune" runat="server" Text='<%# Bind("Comune") %>'></asp:Label>
                <asp:Label ID="Ubicazione" runat="server" Text='<%# Bind("Ubicazione") %>'></asp:Label>
                <asp:Label ID="Via" runat="server" Text='<%# Bind("Via") %>'></asp:Label>                
            </ItemTemplate>
            </asp:TemplateField>
            <asp:Boundfield DataField="ida" headertext="ida" itemstyle-cssClass="nascondi" HeaderStyle-CssClass="nascondi">
                <ItemStyle Width="0px"/>
            </asp:Boundfield> 

        </Columns>
        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="Green" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BorderStyle="Solid" BackColor="#506C8C" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" /> 
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>

        <div style="width: 20px; height:auto; color:chocolate; float: left"><span><pre>   </pre></span></div>

        <asp:GridView ID="gwEscluse" runat="server" AllowSorting="true" Width="660px" visible ="true" AutoPostBack="true"  
            DataKeyNames="ida" PageSize="999" ShowHeaderWhenEmpty="True" AutoGenerateColumns="false"
            style="text-align: left; border: 3px blue; border-radius: 3px; height: auto;" CellPadding="2" ForeColor="#333333" GridLines="None" Font-Size="Small"
            OnSelectedIndexChanged="gwEscluse_SelectedIndexChanged" 
            OnSorting = "gwAuto_Sorting" >
        <Columns>
            <asp:CommandField ButtonType="Button" SelectText="Aggiungi" ShowSelectButton="True" headertext="Comando"><ItemStyle Width="50px" HorizontalAlign="center" Wrap="false"/></asp:CommandField>
             <asp:Boundfield DataField="numero" headertext="Num." HeaderStyle-HorizontalAlign="center" sortexpression="numero">
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            <ItemStyle Width="35px" HorizontalAlign="center" Wrap="false"/></asp:Boundfield>
            
            <asp:Boundfield DataField="targa" headertext="Targa" HeaderStyle-HorizontalAlign="center" sortexpression="targa">
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            <ItemStyle Width="50px" HorizontalAlign="center" Wrap="false"/></asp:Boundfield>

            <asp:Boundfield DataField="marca" headertext="Marca" HeaderStyle-HorizontalAlign="left" sortexpression="marca">
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
            <ItemStyle Width="70px" HorizontalAlign="left" Wrap="true"/></asp:Boundfield> 
            
            <asp:Boundfield DataField="modello" headertext="Modello" HeaderStyle-HorizontalAlign="left" sortexpression="modello">
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
            <ItemStyle Width="220px" HorizontalAlign="left" Wrap="true"/></asp:Boundfield> 

            <asp:TemplateField  SortExpression="Ubicazione">
            <HeaderTemplate>
                <asp:LinkButton ID="Ubicazione" runat="server" Text="Ubicazione" CommandName="Sort" CommandArgument="comune, ubicazione, via, civico" Width="150px" HorizontalAlign="left" wrap="true" linkcolor="white" linkvisitedcolor="white" DisabledLinkColor="white" LinkBehavior="white"></asp:LinkButton>
            </HeaderTemplate>  
            <ItemTemplate>
                <asp:Label ID="Comune" runat="server" Text='<%# Bind("Comune") %>'></asp:Label>
                <asp:Label ID="Ubicazione" runat="server" Text='<%# Bind("Ubicazione") %>'></asp:Label>
                <asp:Label ID="Via" runat="server" Text='<%# Bind("Via") %>'></asp:Label>                
            </ItemTemplate>
            </asp:TemplateField>
            <asp:Boundfield DataField="ida" headertext="ida" itemstyle-cssClass="nascondi" HeaderStyle-CssClass="nascondi">
                <ItemStyle Width="0px"/>
            </asp:Boundfield>         </Columns>
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

    <div style="clear: both; width: 1024px;  margin: 0px auto; ">
    </div>
</div>
</asp:Content>

