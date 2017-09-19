using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServerExternalAuth.Helpers
{
    public static class QueryBuilder
    {
        public static string FacebookUserInfoQuery(List<string> fields, string token)
        {
            return "?fields=" + String.Join(",", fields) + "&access_token=" + token;
        }

        public static string GetQuery(Dictionary<string, string> values, ProviderType provider)
        {
            switch (provider)
            {
                case ProviderType.Facebook:

                    try
                    {
                        var fields = values["fields"];
                        var access_token = values["access_token"];
                        return $"?fields={fields}&access_token={access_token}";
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                case ProviderType.Twitter:
                    try
                    {
                        var token = values["tokenString"];
                        var userInfoEndpoint = values["endpoint"];

                        var tokenString = token.Split('&').ToDictionary(x => x.Split('=')[0], x => x.Split('=')[1]);
                        if (tokenString.Count < 4) return null;

                        var oauth_consumer_key = tokenString["oauth_consumer_key"];
                        var consumerSecret = tokenString["oauth_consumer_secret"];
                        var oauth_token_secret = tokenString["oauth_token_secret"];
                        var oauth_token = tokenString["oauth_token"];
                        var oauth_version = "1.0";
                        var oauth_signature_method = "HMAC-SHA1";
                        string oauth_nonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));

                        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                        string oauth_timestamp = Convert.ToInt64(ts.TotalSeconds).ToString();

                        SortedDictionary<string, string> sd = new SortedDictionary<string, string>();

                        sd.Add("oauth_version", oauth_version);
                        sd.Add("oauth_consumer_key", oauth_consumer_key);
                        sd.Add("oauth_nonce", oauth_nonce);
                        sd.Add("oauth_signature_method", oauth_signature_method);
                        sd.Add("oauth_timestamp", oauth_timestamp);
                        sd.Add("oauth_token", oauth_token);

                        //GS - Build the signature string
                        string baseString = String.Empty;
                        baseString += "GET" + "&";
                        baseString += Uri.EscapeDataString(userInfoEndpoint) + "&";
                        foreach (KeyValuePair<string, string> entry in sd)
                        {
                            baseString += Uri.EscapeDataString(entry.Key + "=" + entry.Value + "&");
                        }

                        baseString = baseString.Substring(0, baseString.Length - 3);

                        string signingKey = Uri.EscapeDataString(consumerSecret) + "&" + Uri.EscapeDataString(oauth_token_secret);

                        HMACSHA1 hasher = new HMACSHA1(new ASCIIEncoding().GetBytes(signingKey));

                        string signatureString = Convert.ToBase64String(hasher.ComputeHash(new ASCIIEncoding().GetBytes(baseString)));

                        //prepare the request
                        string authorizationHeaderParams = String.Empty;
                        authorizationHeaderParams += "OAuth ";
                        authorizationHeaderParams += "oauth_nonce=" + "\"" +
                            Uri.EscapeDataString(oauth_nonce) + "\",";

                        authorizationHeaderParams +=
                            "oauth_signature_method=" + "\"" +
                            Uri.EscapeDataString(oauth_signature_method) +
                            "\",";

                        authorizationHeaderParams += "oauth_timestamp=" + "\"" +
                            Uri.EscapeDataString(oauth_timestamp) + "\",";

                        authorizationHeaderParams += "oauth_consumer_key="
                            + "\"" + Uri.EscapeDataString(
                            oauth_consumer_key) + "\",";

                        authorizationHeaderParams += "oauth_token=" + "\"" +
                            Uri.EscapeDataString(oauth_token) + "\",";

                        authorizationHeaderParams += "oauth_signature=" + "\""
                            + Uri.EscapeDataString(signatureString) + "\",";

                        authorizationHeaderParams += "oauth_version=" + "\"" +
                            Uri.EscapeDataString(oauth_version) + "\"";

                        return authorizationHeaderParams;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                case ProviderType.Google:

                    var google_access_token = values["token"];
                    return $"?access_token={google_access_token}";

                default:
                    return null;
            }
        }
    }
}
