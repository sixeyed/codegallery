<%@ Page Language="C#" MasterPageFile="~/TriathlonResults.Master" AutoEventWireup="true" CodeBehind="Race.aspx.cs" Inherits="TriathlonResults.Web.Race" Title="TriathlonResults - Race" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:DetailsView ID="DetailsView1" runat="server" DataSourceID="RaceName"
            Height="50px" Width="125px">
        </asp:DetailsView>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Athlete Id"
            DataSourceID="AthleteResults">
            <Columns>
                <asp:BoundField DataField="Athlete Id" HeaderText="Athlete Id" ReadOnly="True" SortExpression="Athlete Id" />
                <asp:BoundField DataField="Athlete" HeaderText="Athlete" SortExpression="Athlete" />
                <asp:BoundField DataField="Sector" HeaderText="Sector" SortExpression="Sector" />
                <asp:BoundField DataField="Time /minutes" HeaderText="Time /minutes" ReadOnly="True"
                    SortExpression="Time /minutes" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="RaceName" runat="server" ConnectionString="<%$ ConnectionStrings:TriathlonResultsConnectionString %>"
            SelectCommand="SELECT [RaceName] FROM [Races] WHERE ([RaceId] = @RaceId)">
            <SelectParameters>
                <asp:QueryStringParameter Name="RaceId" QueryStringField="RaceId" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="AthleteResults" runat="server" ConnectionString="<%$ ConnectionStrings:TriathlonResultsConnectionString %>"
            SelectCommand="select a.AthleteId as 'Athlete Id', a.AthleteName as 'Athlete', s.SectorName as 'Sector', st.Duration/60 as 'Time /minutes' from SectorTimes st join Sectors s 	on s.sectorId = st.sectorId join Athletes a 	on a.athleteId = st.athleteId WHERE ([RaceId] = @RaceId) ORDER BY a.athleteId, s.sectorId">
            <SelectParameters>
                <asp:QueryStringParameter Name="RaceId" QueryStringField="RaceId" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>    
    </div>
</asp:Content>
