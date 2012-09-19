using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace Sixeyed.Scratchpad.Identity.Web
{
    public partial class ClaimInspector : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var wrappedToken = XDocument.Parse(HttpContext.Current.Request.Form[1]);
            var binaryToken = wrappedToken.Root.Descendants("{http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd}BinarySecurityToken").First();
            var tokenBytes = Convert.FromBase64String(binaryToken.Value);
            var token = Encoding.UTF8.GetString(tokenBytes);
            var tokenType = wrappedToken.Root.Descendants("{http://schemas.xmlsoap.org/ws/2005/02/trust}TokenType").First().Value;

            if (tokenType == TokenType.Swt)
            {
                ParseSWT(token);
            }
            else if (tokenType == TokenType.Jwt)
            {
                ParseJWT(token);
            }
            else
            {
                Response.Write("<pre>Unknown token type: " + tokenType + "</pre><br/>");
            }
        }

        private void ParseJWT(string token)
        {
            Response.Write("<h2>JWT token contents:</h2>");
            var tokenParts = token.Split('.');
            var encodedPayload = tokenParts[1]; 
            var decodedPayload = Base64Decode(encodedPayload);
            foreach (var part in decodedPayload.Split(','))
            {
                Response.Write("<pre>" + part + "</pre><br/>");
            }
        }

        private void ParseSWT(string token)
        {
            Response.Write("<h2>SWT token contents:</h2>");
            var decoded = HttpUtility.UrlDecode(token);
            foreach (var part in decoded.Split('&'))
            {
                Response.Write("<pre>" + part + "</pre><br/>");
            }
        }

        private struct TokenType
        {
            public const string Jwt = "urn:ietf:params:oauth:token-type:jwt";
            public const string Swt = "http://schemas.xmlsoap.org/ws/2009/11/swt-token-profile-1.0";
        }

        #region No-padding Base64 decoding - see http://self-issued.info/docs/draft-ietf-oauth-json-web-token.html#anchor7
        
        private static char Base64PadCharacter = '=';
        private static char Base64Character62 = '+';
        private static char Base64Character63 = '/';
        private static char Base64UrlCharacter62 = '-';
        private static char Base64UrlCharacter63 = '_';

        private static byte[] DecodeBytes(string arg)
        {
            var s = new StringBuilder(arg);
            s.Replace(Base64UrlCharacter62, Base64Character62);
            s.Replace(Base64UrlCharacter63, Base64Character63);

            int pad = s.Length % 4;
            s.Append(Base64PadCharacter, (pad == 0) ? 0 : 4 - pad);

            return Convert.FromBase64String(s.ToString());
        }

        private static string Base64Decode(string arg)
        {
            return Encoding.UTF8.GetString(DecodeBytes(arg));
        }

        #endregion
    }
}