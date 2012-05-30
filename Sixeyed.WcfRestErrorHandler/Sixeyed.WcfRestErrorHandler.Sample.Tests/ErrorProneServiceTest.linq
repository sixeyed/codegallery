<Query Kind="Statements">
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.Serialization.dll</Reference>
  <Namespace>System.Net</Namespace>
</Query>

var url = "http://localhost/Sixeyed.WcfRestErrorHandler.Sample/ErrorProneService.svc/lastLogin?userId=xyz";
//var url = "http://localhost/Sixeyed.WcfRestErrorHandler.Sample/ErrorProneService.svc/dbz";

string response = null;
using (var client = new WebClient())
{
	System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate {return true;};
	client.Proxy = System.Net.GlobalProxySelection.GetEmptyWebProxy();		
	url.Dump("Request URL");
	try
	{
		response = client.DownloadString(url);	
		response.Dump("Response");
		client.Dump("Headers");
	}
	catch (WebException webEx)
	{
		var webResponse = (HttpWebResponse)webEx.Response;
		string.Format("Status Code: {0}, Description: {1}", (int)webResponse.StatusCode, webResponse.StatusDescription).Dump("Error");
	}
}
