<%@ Page Language="C#" MasterPageFile="~/TriathlonResults.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="TriathlonResults.Web.Home" Title="TriathlonResults - Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td>Races:</td>
        </tr>
        <tr>
            <td>
            <div>
            
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="TriathlonResults">
            <Columns>
                <asp:HyperLinkField DataTextField="RaceName" DataNavigateUrlFields="RaceId" DataNavigateUrlFormatString="Race.aspx?RaceId={0}" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="TriathlonResults" runat="server" ConnectionString="<%$ ConnectionStrings:TriathlonResultsConnectionString %>"
            SelectCommand="SELECT [RaceName], [RaceId] FROM [Races] ORDER BY [RaceName]"></asp:SqlDataSource>
    
    </div>
            </td>            
        </tr>
    </table>
</asp:Content>
