<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WcfPersonalizationSample.Web._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:WebPartManager ID="WebPartManager1" runat="server">
    </asp:WebPartManager>
    <div>
    
    </div>
    <asp:WebPartZone ID="WebPartZone1" runat="server">
    </asp:WebPartZone>
    <asp:CatalogZone ID="CatalogZone1" runat="server">
        <ZoneTemplate>
            <asp:PageCatalogPart ID="PageCatalogPart1" runat="server" />
        </ZoneTemplate>
    </asp:CatalogZone>
    <asp:EditorZone ID="EditorZone1" runat="server">
        <ZoneTemplate>
            <asp:PropertyGridEditorPart ID="PropertyGridEditorPart1" runat="server" />
        </ZoneTemplate>
    </asp:EditorZone>
    </form>
</body>
</html>
