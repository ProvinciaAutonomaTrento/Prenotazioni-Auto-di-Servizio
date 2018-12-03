<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="RegisterExternalLogin.aspx.cs" Inherits="Account_RegisterExternalLogin" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h3>Esegui registrazione con account <%: ProviderName %> personale</h3>

    <asp:PlaceHolder runat="server">
        <div class="form-horizontal">
            <h4>Form di associazione</h4>
            <hr />
            <asp:ValidationSummary runat="server" ShowModelStateErrors="true" CssClass="text-danger" />
            <p class="text-info">
                L'utente ha eseguito l'autenticazione con <strong><%: ProviderName %></strong>. Immettere di seguito un nome utente per il sito corrente
                e fare clic sul pulsante Accedi.
            </p>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="userName" CssClass="col-md-2 control-label">Nome utente</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="userName" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="userName"
                        Display="Dynamic" CssClass="text-danger" ErrorMessage="Il nome utente è obbligatorio" />
                    <asp:ModelErrorMessage runat="server" ModelStateKey="UserName" CssClass="text-danger" />
                </div>
            </div>

            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:Button runat="server" Text="Accedi" CssClass="btn btn-default" OnClick="LogIn_Click" />
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
</asp:Content>
