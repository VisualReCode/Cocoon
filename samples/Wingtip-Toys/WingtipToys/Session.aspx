<%@ Page Language="C#" CodeBehind="Session.aspx.cs" Inherits="WingtipToys.Session" MasterPageFile="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    Session text - Set from the querystring : 
    <asp:Label runat="server" ID="sessionTextLabel"></asp:Label>
    
    <hr/>
    Pulled from the session : 
    <asp:Label runat="server" ID="pullFromSession"></asp:Label>
    
</asp:Content>