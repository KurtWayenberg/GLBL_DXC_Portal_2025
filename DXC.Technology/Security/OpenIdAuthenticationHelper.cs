using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Secrets;
using DXC.Technology.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Threading.Tasks;

namespace DXC.Technology.Security
{
    public class OpenIdConnectHelper
    {
        public static string GetUserRedirectForAuthenticationCodeFetchUrl(string state)
        {
            var oiash = DXC.Technology.Configuration.OpenIdAuthenticationOptionsManager.Current.GetOpenIdAuthenticationOptions();
            string scope = oiash.Scope;
            string nonce = "NotUsed";
            string redirectUrl = oiash.AuthorizationEndpoint
                                    + DXC.Technology.Utilities.String.FormatAsQuerystring("?client_id={0}&redirect_uri={1}&response_type=code&scope={2}&state={3}&nonce={4}", oiash.ClientId, oiash.CallbackEndpoint, scope, state, nonce);
            return redirectUrl;
        }

    }


    public class ServiceAuthenticationToken
    {
        public string access_token { get; set; }
        public string scope { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }

        internal DateTime ValidUntil { get; set; }
    }


    public class CirAuthenticationTokenHelper
    {
        private static Dictionary<string, ServiceAuthenticationToken> sServiceAuthenticationTokenCache = new Dictionary<string, ServiceAuthenticationToken>();
        public static ServiceAuthenticationToken GetServiceAuthenticationTokenFromCacheOrCreateNew(string service)
        {
            //if (sServiceAuthenticationTokenCache.ContainsKey(service))
            //{
            //    if (sServiceAuthenticationTokenCache[service].ValidUntil > DateTime.Now.AddSeconds(15))
            //    {
            //        return sServiceAuthenticationTokenCache[service];
            //    }
            //    sServiceAuthenticationTokenCache.Remove(service);
            //}
            ServiceAuthenticationToken token = GetServiceAuthenticationToken(service);
            token.ValidUntil = DateTime.Now.AddSeconds(15);
            //sServiceAuthenticationTokenCache.Add(service, token);
            return token;

        }
        public static ServiceAuthenticationToken GetServiceAuthenticationToken(string service)
        {
            var oiash = DXC.Technology.Configuration.OpenIdAuthenticationOptionsSetsManager.Current.GetOpenIdAuthenticationOptionsSets().Sets.FirstOrDefault(p => p.Purpose == service);

            string scopes = oiash.Scope;
            string token = GenerateJwtToken(service, oiash.ClientId);
            var dict = new Dictionary<string, string>();
            dict.Add("grant_type", "client_credentials");
            dict.Add("scope", scopes);
            dict.Add("client_assertion_type", "urn:ietf:params:oauth:client-assertion-type:jwt-bearer");
            dict.Add("client_assertion", token);
            DXC.Technology.Configuration.LoggingManager.Current.Log(
                   Microsoft.Extensions.Logging.LogLevel.Information,
                   new Microsoft.Extensions.Logging.EventId(60001, "Auth Endpoint"),
                   oiash.AuthorizationEndpoint);
            DXC.Technology.Configuration.LoggingManager.Current.Log(
                   Microsoft.Extensions.Logging.LogLevel.Information,
                   new Microsoft.Extensions.Logging.EventId(60002, "TOKEN"),
                   token);
            using (var client = new HttpClient())
            {
                var req = new HttpRequestMessage(HttpMethod.Post, oiash.AuthorizationEndpoint) { Content = new FormUrlEncodedContent(dict) };
                var res = client.SendAsync(req).GetAwaiter().GetResult();
                StreamReader reader = new StreamReader(res.Content.ReadAsStream());
                string jsonResponse = reader.ReadToEnd();
                DXC.Technology.Configuration.LoggingManager.Current.Log(
                       Microsoft.Extensions.Logging.LogLevel.Information,
                       new Microsoft.Extensions.Logging.EventId(60003, "RETRIEVED FROM OAUTH"),
                       jsonResponse);
                try
                {
                    var result = JsonConvert.DeserializeObject<ServiceAuthenticationToken>(jsonResponse);
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error parsing:" + jsonResponse + " -> " + ex.Message);
                }
            }
        }

        public static string GenerateJwtToken(string service, string code)
        {
            var oiash = DXC.Technology.Configuration.OpenIdAuthenticationOptionsSetsManager.Current.GetOpenIdAuthenticationOptionsSets().Sets.FirstOrDefault(p => p.Purpose == service);
            //var oiash = DXC.Technology.Configuration.OpenIdAuthenticationOptionsSetsManager.Current.GetOpenIdAuthenticationOptionsSets().Sets[1];
            return GenerateJwtToken(oiash, code);
        }

        /// <summary>
        /// Generate JWT Token after successful login.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public static string GenerateJwtToken(string code)
        {
            System.Diagnostics.Trace.WriteLine("Getting Authentication Token", "Info");
            OpenIdAuthenticationOptions oiash = DXC.Technology.Configuration.OpenIdAuthenticationOptionsManager.Current.GetOpenIdAuthenticationOptions();
            return GenerateJwtToken(oiash, code);
        }

        /// <summary>
        /// Generate JWT Token after successful login.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public static string GenerateJwtToken(OpenIdAuthenticationOptions oiash, string code)
        {
            X509Certificate2 cert = null;
            if (!string.IsNullOrEmpty(oiash.CertificateKeyVaultUrl) && !string.IsNullOrEmpty(oiash.CertificateName))
            {
                //string kid = "21E7FDFB8A3A0D73C8AE5809F76A399205657557";
                string keyvaultUrl = oiash.CertificateKeyVaultUrl;
                string name = oiash.CertificateName;
                cert = CertificateHelper.GetCertificateFromKeyVault(keyvaultUrl, name);

                if (!cert.HasPrivateKey)
                {
                    throw new Exception("Certificate has no private key or private key is not exportable");
                }
            }
            else if (!string.IsNullOrEmpty(oiash.CertificateThumbPrint))
            {
                //string kid = "21E7FDFB8A3A0D73C8AE5809F76A399205657557";
                cert = CertificateHelper.GetCertificate(oiash.CertificateThumbPrint);
            }
            else
            {
                throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("Certificate kid or url/name");
            }

            //var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {          
                Issuer = oiash.TokenIssuer,
                Subject = new ClaimsIdentity(new[] { new Claim("sub", oiash.ClientId), new Claim("jti", System.Guid.NewGuid().ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                NotBefore = DateTime.UtcNow.AddMinutes(-1),
                Audience = oiash.TokenAudience,
                SigningCredentials = new SigningCredentials(new X509SecurityKey(cert, oiash.CertificateKid), SecurityAlgorithms.RsaSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            string tokenAsString = tokenHandler.WriteToken(token);


            DXC.Technology.Configuration.LoggingManager.Current.Log(
                Microsoft.Extensions.Logging.LogLevel.Information,
                new Microsoft.Extensions.Logging.EventId(90001, "Stage 10"),
                "Created Token: " + tokenAsString);

            return tokenAsString;
        }

        public static string GetAuthToken(string code)
        {
            System.Diagnostics.Trace.WriteLine("Getting Authentication Token", "Info");
            DXC.Technology.Configuration.LoggingManager.Current.Log(
                Microsoft.Extensions.Logging.LogLevel.Information,
                new Microsoft.Extensions.Logging.EventId(90001, "Stage 1"),
                "Getting Authentication Token");

            var oiash = DXC.Technology.Configuration.OpenIdAuthenticationOptionsManager.Current.GetOpenIdAuthenticationOptions();

            string jwtToken = GenerateJwtToken(code);

            DXC.Technology.Configuration.LoggingManager.Current.Log(
                Microsoft.Extensions.Logging.LogLevel.Information,
                new Microsoft.Extensions.Logging.EventId(90001, "Stage 2"),
                "Request Token: " + jwtToken);

            //string requesttokenbodytemplate = "grant_type=client_credentials&scope=msg_statuses_v1_P%20msg_mailbox_v1_P%20msg_msg_v1_P&client_assertion_type=urn%3Aietf%3Aparams%3Aoauth%3Aclient-assertion-type%3Ajwt-bearer&client_assertion={0}";
            //string requesttokenbody = string.Format(requesttokenbodytemplate, jwtToken);
            string requesttokenbodytemplate = "grant_type={0}&code={1}&client_assertion_type={2}&client_assertion={3}&redirect_uri={4}";
            string requesttokenbody = DXC.Technology.Utilities.String.FormatAsQuerystring(requesttokenbodytemplate,
                                            "authorization_code",
                                            code,
                                            "urn:ietf:params:oauth:client-assertion-type:jwt-bearer",
                                            JwtUrlHelper.ConvertToUrlFormat(jwtToken),
                                            oiash.CallbackEndpoint
                                            );

            string url = oiash.TokenEndpoint;

            DXC.Technology.Configuration.LoggingManager.Current.Log(
                Microsoft.Extensions.Logging.LogLevel.Information,
                new Microsoft.Extensions.Logging.EventId(90001, "Stage 2.1"),
                "url: " + url);

            DXC.Technology.Configuration.LoggingManager.Current.Log(
                Microsoft.Extensions.Logging.LogLevel.Information,
                new Microsoft.Extensions.Logging.EventId(90001, "Stage 2.2"),
                "Request Token Body: " + requesttokenbody);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            System.Diagnostics.Trace.WriteLine("Url: " + url, "Info");
            System.Diagnostics.Trace.WriteLine("Body: " + requesttokenbody, "Info");
            string jsonResultString = "";
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string connectivityTestSuccessfull = "'Vlaams Toegangsbeheer' niet opgehaald";
                using (var w2 = new WebClient())
                {
                    if (!string.IsNullOrEmpty(oiash.ProxyServer))
                    {
                        WebProxy wp = new WebProxy(oiash.ProxyServer);
                        w2.Proxy = wp;
                    }
                    string jsonResultString2 = w2.DownloadString("https://authenticatie-ti.vlaanderen.be/docs/");
                    bool success = jsonResultString2.Contains("Vlaams Toegangsbeheer");
                    if (success)
                        connectivityTestSuccessfull = "'Vlaams Toegangsbeheer'  gevonden in string!";
                    else
                        connectivityTestSuccessfull = "'Vlaams Toegangsbeheer' NIET gevonden in string!" + jsonResultString2;
                }

                DXC.Technology.Configuration.LoggingManager.Current.Log(
                    Microsoft.Extensions.Logging.LogLevel.Information,
                    new Microsoft.Extensions.Logging.EventId(90001, "Stage 2.3"),
                    connectivityTestSuccessfull);

                using (var w = new WebClient())
                {
                    if (!string.IsNullOrEmpty(oiash.ProxyServer))
                    {
                        DXC.Technology.Configuration.LoggingManager.Current.Log(
                            Microsoft.Extensions.Logging.LogLevel.Information,
                            new Microsoft.Extensions.Logging.EventId(90001, "Stage 2.4"),
                            "using proxy:" + oiash.ProxyServer);

                        WebProxy wp = new WebProxy(oiash.ProxyServer);
                        w.Proxy = wp;
                    }
                    else
                    {
                        DXC.Technology.Configuration.LoggingManager.Current.Log(
                            Microsoft.Extensions.Logging.LogLevel.Information,
                            new Microsoft.Extensions.Logging.EventId(90001, "Stage 2.4"),
                            "no proxy used");

                    }
                    w.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
                    jsonResultString = w.UploadString(url, "POST", requesttokenbody.ToString());

                    DXC.Technology.Configuration.LoggingManager.Current.Log(
                        Microsoft.Extensions.Logging.LogLevel.Information,
                        new Microsoft.Extensions.Logging.EventId(90001, "Stage 2.5"),
                        "jsonResultString: " + jsonResultString);
                }
            }
            catch (Exception ex)
            {
                DXC.Technology.Configuration.LoggingManager.Current.Log(
                    Microsoft.Extensions.Logging.LogLevel.Information,
                    new Microsoft.Extensions.Logging.EventId(90001, "Stage 2.6"),
                    "Token exchange failed exception: " + ex.Message);

                DXC.Technology.Exceptions.ExceptionHelper.Publish(new DXC.Technology.Exceptions.NamedExceptions.ActionFailedException("Get Authentication Token"));
                DXC.Technology.Exceptions.ExceptionHelper.Publish(ex);
                throw;
            }
            //Request Token Result String:
            // {"access_token":"l_NJa-5BavRGgWIS8myk0DoXW7KkqitGNDtnVttcx6c",
            //   "expires_in":3598,
            //   "id_token":"eyJhbGciOiJSUzI1NiIsImtpZCI6IkpwdGc3aktSNm1LMFZZQWFPMkh2aElrc3N2MVJ3SFYtQk5TWUFSVDh4Q3cifQ.eyJhdF9oYXNoIjoidlE5Z1pBdGRLT2dIaDNLcHFyMlFSdyIsImF1ZCI6ImE4ZDNiZTJmLTA5MDAtNGJmNS1iODE1LTdiNzlmZDQ5ZTUyZiIsImF6cCI6ImE4ZDNiZTJmLTA5MDAtNGJmNS1iODE1LTdiNzlmZDQ5ZTUyZiIsImNvdCI6InZvIiwiZXhwIjoxNjQ3MDI1MjMwLCJmYW1pbHlfbmFtZSI6IldheWVuYmVyZyIsImdpdmVuX25hbWUiOiJLdXJ0IiwiaWF0IjoxNjQ3MDIxNjMwLCJpc3MiOiJodHRwczovL2F1dGhlbnRpY2F0aWUtdGkudmxhYW5kZXJlbi5iZS9vcCIsImtpZCI6IkpwdGc3aktSNm1LMFZZQWFPMkh2aElrc3N2MVJ3SFYtQk5TWUFSVDh4Q3ciLCJub25jZSI6IndoYXRldmVyIiwic3ViIjoiZjZjNDA0YTE4ZjA0MDg3MzgyYzk5NTk3ZTIyZmVmMjM0NzhmYTk0OCIsInZvX2RvZWxncm9lcGNvZGUiOiJHSUQiLCJ2b19lbWFpbCI6Imt1cnQud2F5ZW5iZXJnQGR4Yy5jb206MU0yQiIsInZvX2lkIjoiNTNjMzg5ZjctY2Y0Mi00YmJlLTk1YjYtMjBjMjZkMDViYzNkIn0.y3CW0eYsYS9qLweiQDn4n_Qykyf3cTv98sW1yJH-NRS4-hJvxyZEsBs3hDzqIBBS-vVm6HWeOCQr_htnik5R16wibyredwIQOE4o-lIHeHcTWdGhXxbA2x64ktY5NBV5sNu9rHG0r9WykYKnDfP4_0phPuu4E8LTjfFJ8K76yYocLnOHQrt5I9J7akU5ERoX86A4zivk5mlBfTzvThovjnJnoX_goFIRjcjnLbl4BCPq7ZIiwn_O3cQA8mCtQd37Kdx71b062OQ17MsuUzzxvOHOJ0xIslTW0dsQQQNmaZhzeQk82Kc9OCKaZjdWHUXeZrRqqGQOyLnp_o3tUHH7mg",
            //   "scope":"profile vo",
            //   "token_type":"Bearer"}

            DXC.Technology.Configuration.LoggingManager.Current.Log(
                Microsoft.Extensions.Logging.LogLevel.Information,
                new Microsoft.Extensions.Logging.EventId(90001, "Stage 2.3"),
               "Request Token Result String: " + jsonResultString);

            System.Diagnostics.Trace.WriteLine("Request Token Result String: " + jsonResultString, "Info");
            dynamic authTokenResponse = DXC.Technology.Objects.SerializationHelper.JsonDeserializeToObject(jsonResultString);
            string token = authTokenResponse.access_token;
            System.Diagnostics.Trace.WriteLine("Token: " + token, "Info");
            string id_token = authTokenResponse.id_token;
            System.Diagnostics.Trace.WriteLine("Id_Token: " + id_token, "Info");
            return id_token;
        }
        public static AuthenticatieVlaanderenClaim VerifyAndGetAuthenticatieVlaanderenClaim(string jwt)
        {
            //De Client moet de signature van het ID-token valideren(cfr.JWS - validatie) waarbij het algoritme gebruikt wordt dat in de JWT als Header Parameter meegestuurd wordt.De Client dient de keys te gebruiken die door de Issuer worden aangeboden(cfr.de keys).De validatie van de TLS-sessie om het Token Endpoint te gebruiken mag volgens de standaard ook gebruikt worden ter validatie van de Issuer(ipv de signature van het ID-token)
            //   A JWT token has three sections:
            //   - Header: JSON format which is encoded in Base64
            //   - Claims: JSON format which is encoded in Base64.
            //   - Signature: Created and signed based on Header and Claims which is encoded in Base64.
            string[] jwtSections = jwt.Split('.');
            byte[] jwt_headersection = Convert.FromBase64String(JwtUrlHelper.ConvertFromUrlFormat(jwtSections[0]));
            byte[] jwt_claimsection = Convert.FromBase64String(JwtUrlHelper.ConvertFromUrlFormat(jwtSections[1]));
            byte[] jwt_signaturesection = Convert.FromBase64String(JwtUrlHelper.ConvertFromUrlFormat(jwtSections[2]));
            System.Diagnostics.Trace.WriteLine("jwt_headersection: " + System.Text.UTF8Encoding.UTF8.GetString(jwt_headersection), "Info");
            System.Diagnostics.Trace.WriteLine("jwt_claimsection: " + System.Text.UTF8Encoding.UTF8.GetString(jwt_claimsection), "Info");
            System.Diagnostics.Trace.WriteLine("jwt_signaturesection: " + System.Text.UTF8Encoding.UTF8.GetString(jwt_signaturesection), "Info");

            DXC.Technology.Configuration.LoggingManager.Current.Log(
                Microsoft.Extensions.Logging.LogLevel.Information,
                new Microsoft.Extensions.Logging.EventId(90001, "Stage 15"),
                "jwt_claimsection: " + System.Text.UTF8Encoding.UTF8.GetString(jwt_claimsection));

            jwt_claimsection = System.Text.UTF8Encoding.UTF8.GetBytes(System.Text.UTF8Encoding.UTF8.GetString(jwt_claimsection).Replace("vmsw_cir _rol_3d", "vmsw_cir_rol_3d"));

            DXC.Technology.Configuration.LoggingManager.Current.Log(
                 Microsoft.Extensions.Logging.LogLevel.Information,
                 new Microsoft.Extensions.Logging.EventId(90001, "Stage 15b"),
                 "jwt_claimsection: " + System.Text.UTF8Encoding.UTF8.GetString(jwt_claimsection));


            JwtHeader jwt_header = null;
            AuthenticatieVlaanderenClaim jwt_claim = null;

            using (var ms = new MemoryStream(jwt_headersection))
            {
                // Deserialization from JSON  
                System.Runtime.Serialization.Json.DataContractJsonSerializer deserializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(JwtHeader));
                jwt_header = (JwtHeader)deserializer.ReadObject(ms);
            }
            using (var ms = new MemoryStream(jwt_claimsection))
            {
                // Deserialization from JSON  
                System.Runtime.Serialization.Json.DataContractJsonSerializer deserializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(AuthenticatieVlaanderenClaim));
                jwt_claim = (AuthenticatieVlaanderenClaim)deserializer.ReadObject(ms);
            }


            System.Diagnostics.Trace.WriteLine("-Objects created and not null: " + (jwt_header != null).ToString() + (jwt_claim != null).ToString(), "Info");

            var oiash = DXC.Technology.Configuration.OpenIdAuthenticationOptionsManager.Current.GetOpenIdAuthenticationOptions();

            //de Client valideert dat de “iss"-claim (issuer) wel degelijk die van de OpenID Connect Provider is: zie de uitleg omtrent de Discovery URL voor de concrete waarde voor de ACM IDP
            if (!jwt_claim.iss.Equals(oiash.TokenAudience))
                throw new DXC.Technology.Exceptions.NamedExceptions.ActionDeniedException("Invalid Issuer", jwt_claim.iss);

            //de Client dient te valideren dat zijn eigen ClientID vermeld staat in de “aud"-claim (audience): deze claim kan een array zijn met daarin meerdere waarden
            if (!jwt_claim.aud.Equals(oiash.TokenIssuer))
                throw new DXC.Technology.Exceptions.NamedExceptions.ActionDeniedException("Invalid Token Audience", jwt_claim.aud);

            //de huidige tijd mag zeker niet na de expiry time liggen, dat vermeld wordt in de “exp"-claim (expiry time)
            DateTimeOffset currentOffset = new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero);
            var currentUnixTime = Convert.ToInt32(currentOffset.ToUnixTimeSeconds());
            if (jwt_claim.exp < currentUnixTime)
                throw new DXC.Technology.Exceptions.NamedExceptions.ActionDeniedException("Expired Token ", "");

            ////het verschil tussen de “iat"-claim (issued at) en de huidige tijd mag niet te groot zijn (het mag dus niet te lang geleden zijn dat het ID-token geïssued werd): de Client mag bepalen welke waarde aanvaardbaar is
            //DateTimeOffset maxOffset = new DateTimeOffset(DateTime.UtcNow.AddDays(2), TimeSpan.Zero);
            //var maxUnixTime = Convert.ToInt32(currentOffset.ToUnixTimeSeconds());
            //if (jwt_claim.exp > maxUnixTime)
            //    throw new DXC.SystemLib.Exceptions.NamedExceptions.ActionDeniedException("Suspicious Token ", "Token valid too far in the future");

            string vo_id = jwt_claim.vo_id;
            string rrn = jwt_claim.rrn;
            //indien er bij het Authenticatie Request een nonce-waarde meegestuurd werd, dan MOET er in het ID-token een “nonce"-claim aanwezig zijn met exact dezelfde waarde als in het Authenticatie Request
            //als er een acr-claim gevraagd werd, dan moet de Client valideren dat de waarde van de “acr"-claim geldig is
            return jwt_claim;
        }
    }

    public class KeySetManager
    {
        private static DateTime sRefreshDateTime = DateTime.MinValue;
        private static KeySet sKeySet = null;

        private static KeySet GetKeySet()
        {
            if (sKeySet == null || DateTime.Now.Subtract(sRefreshDateTime).TotalHours > 3)
            {
                try
                {
                    var url = "https://authenticatie-ti.vlaanderen.be/op/v1/keys";
                    var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpRequest.Accept = "application/json";

                    var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                    KeySet keySet = null;
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string result = streamReader.ReadToEnd();
                        keySet = JsonConvert.DeserializeObject<KeySet>(result);
                    }
                    sKeySet = keySet;
                }
                catch (Exception)
                {
                    throw; //Todo Log
                }

            }
            return sKeySet;
        }
        public static RsaSecurityKey GetRsaSecurityKey(string kid)
        {

            Key key = GetKeySet().keys.FirstOrDefault(p => p.kid == kid);
            if (key != null)
            {
                var parameters = new System.Security.Cryptography.RSAParameters()
                {
                    Modulus = System.Text.UTF8Encoding.UTF8.GetBytes(key.n),
                    Exponent = System.Text.UTF8Encoding.UTF8.GetBytes(key.e)
                };
                return new RsaSecurityKey(parameters);
            }
            else
            {
                //It is a local key!
                return new RsaSecurityKey(CertificateHelper.GetRSAParameters(kid, false));
            }
        }
    }
    public class KeySet
    {
        public Key[] keys { get; set; }
    }

    public class Key
    {
        public string alg { get; set; }
        public string e { get; set; }
        public string kid { get; set; }
        public string kty { get; set; }
        public string n { get; set; }
        public string use { get; set; }
    }

    //public class JwTHeader
    //{
    //    public string alg { get; set; }
    //    public string kid { get; set; }
    //    public string typ { get; set; }

    //    public string ToJsonString()
    //    {
    //        return JsonConvert.SerializeObject(this);
    //    }
    //    public string ToBase64String()
    //    {
    //        var header = JsonConvert.SerializeObject(this);
    //        var base64header = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(header));
    //        return base64header;
    //    }
    //}

    //public class JwTPayLoad
    //{
    //    public string sub { get; set; }
    //    public string iss { get; set; }
    //    public int iat { get; set; }
    //    public int exp { get; set; }
    //    public string jti { get; set; }
    //    public string aud { get; set; }

    //    public string ToJsonString()
    //    {
    //        return JsonConvert.SerializeObject(this);
    //    }
    //    public string ToBase64String()
    //    {
    //        var header = JsonConvert.SerializeObject(this);
    //        var base64header = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(header));
    //        return base64header;
    //    }
    //}
    public class AuthenticatieVlaanderenClaim
    {
        public string sub { get; set; }
        public string iss { get; set; }
        public int iat { get; set; }
        public int exp { get; set; }
        public string nonce { get; set; }
        public string jti { get; set; }
        public string aud { get; set; }
        public string rrn { get; set; }
        public string family_name { get; set; }
        public string given_name { get; set; }
        public string[] vmsw_cir_rol_3d { get; set; }
        public string vo_id { get; set; }
        public string vo_email { get; set; }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }
        public string ToBase64String()
        {
            var header = JsonConvert.SerializeObject(this);
            var base64header = System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(header));
            return base64header;
        }

        public string UsernameForUser()
        {
            if (string.IsNullOrEmpty(Organization))
                return rrn;
            else
                return string.Concat(Organization.Substring(Organization.Length - 4, 4) + "_" + rrn.Substring(0, 9));
        }

        public string Organization
        {
            get
            {
                if (string.IsNullOrEmpty(vo_id)
                    || (vmsw_cir_rol_3d == null)
                    || (vmsw_cir_rol_3d.Length == 0))
                    return null;
                else
                {
                    return this.vmsw_cir_rol_3d.Select(p => p.Split(':')[1]).Distinct().First();
                }
            }
        }

        public string Department
        {
            get
            {
                return string.IsNullOrEmpty(Organization) ? "CITIZEN" : Organization;
            }
        }
        public string[] RolesForOrganization(string organization)
        {
            if (string.IsNullOrEmpty(vo_id)
                || (vmsw_cir_rol_3d == null)
                || (vmsw_cir_rol_3d.Length == 0))
                return new string[0];
            else
            {
                return this.vmsw_cir_rol_3d.Where(p => p.Contains(organization)).Select(p => p.Split(':')[0].Split('-')[1]).Distinct().ToArray();
            }
        }
    }
    public static class JwtUrlHelper
    {
        public static string ConvertFromUrlFormat(string value)
        {
            //return value;
            string v = value.Replace('-', '+').Replace('_', '/');
            v = (v.Length % 4 == 2) ? v + "==" : ((v.Length % 4 == 3) ? v + "=" : ((v.Length % 4 == 1) ? v + "===" : v));
            return v;
        }
        public static string ConvertToUrlFormat(string value)
        {
            //return value;
            string v = value.Replace('+', '-').Replace('/', '_').Replace("=", "");
            return v;
        }

    }


    public static class CertificateHelper
    {
        public static X509Certificate2 GetCertificateFromKeyVault(string vaultUrl, string certificateName)
        {
            string managedClientId = DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().ManagedIdentityClientId;

            CertificateClient client = (!string.IsNullOrEmpty(managedClientId)) ?
                new Azure.Security.KeyVault.Certificates.CertificateClient(new Uri(vaultUrl), new DefaultAzureCredential(new DefaultAzureCredentialOptions() { ManagedIdentityClientId = managedClientId })) :
                new Azure.Security.KeyVault.Certificates.CertificateClient(new Uri(vaultUrl), new DefaultAzureCredential(new DefaultAzureCredentialOptions()));

            X509Certificate2 certificate = client.DownloadCertificate(certificateName);
            return certificate;
        }

        public static X509Certificate2 GetCertificateFromKeyVaultOldApi(string vaultUrl, string certificateName)
        {
            var certificateClient = new CertificateClient(new Uri(vaultUrl), new DefaultAzureCredential());
            var secretClient = new SecretClient(new Uri(vaultUrl), new DefaultAzureCredential());

            KeyVaultCertificateWithPolicy certificate = certificateClient.GetCertificate(certificateName);

            // Return a certificate with only the public key if the private key is not exportable.
            if (certificate.Policy?.Exportable != true)
            {
                return new X509Certificate2(certificate.Cer);
            }

            // Parse the secret ID and version to retrieve the private key.
            string[] segments = certificate.SecretId.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length != 3)
            {
                throw new InvalidOperationException($"Number of segments is incorrect: {segments.Length}, URI: {certificate.SecretId}");
            }

            string secretName = segments[1];
            string secretVersion = segments[2];

            KeyVaultSecret secret = secretClient.GetSecret(secretName, secretVersion);

            // For PEM, you'll need to extract the base64-encoded message body.
            // .NET 5.0 preview introduces the System.Security.Cryptography.PemEncoding class to make this easier.
            if ("application/x-pkcs12".Equals(secret.Properties.ContentType, StringComparison.InvariantCultureIgnoreCase))
            {
                byte[] pfx = Convert.FromBase64String(secret.Value);
                return new X509Certificate2(pfx);
            }

            throw new NotSupportedException($"Only PKCS#12 is supported. Found Content-Type: {secret.Properties.ContentType}");
        }
        public static X509Certificate2 GetCertificate(string thumprint)
        {
            X509Store store = new X509Store("My", StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

            X509Certificate2 x509 = null;

            X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;

            DXC.Technology.Configuration.LoggingManager.Current.Log(
                Microsoft.Extensions.Logging.LogLevel.Information,
                new Microsoft.Extensions.Logging.EventId(90001, "Stage 3"),
                "Checking crtificates");

            System.Diagnostics.Trace.WriteLine("Checking crtificates ", "Info");
            foreach (X509Certificate2 x509cert in collection)
            {
                DXC.Technology.Configuration.LoggingManager.Current.Log(
                    Microsoft.Extensions.Logging.LogLevel.Information,
                    new Microsoft.Extensions.Logging.EventId(90001, "Stage 4"),
                    "Checking crtificate " + x509cert.Thumbprint);

                if (x509cert.Thumbprint.Equals(thumprint))
                    x509 = x509cert;
            }

            //X509Certificate2Collection tcollection = (X509Certificate2Collection)collection.Find(X509FindType.FindByThumbprint, kid, false);
            //foreach (X509Certificate2 x509cert in tcollection)
            //{
            //    x509 = x509cert;
            //}
            if (x509 == null)
                throw new DXC.Technology.Exceptions.NamedExceptions.ItemNotFoundException("Certificate " + thumprint);

            byte[] rawdata = x509.RawData;
            //Console.WriteLine("Content Type: {0}{1}", X509Certificate2.GetCertContentType(rawdata), Environment.NewLine);
            //Console.WriteLine("Friendly Name: {0}{1}", x509.FriendlyName, Environment.NewLine);
            //Console.WriteLine("Certificate Verified?: {0}{1}", x509.Verify(), Environment.NewLine);
            //Console.WriteLine("Simple Name: {0}{1}", x509.GetNameInfo(X509NameType.SimpleName, true), Environment.NewLine);
            //Console.WriteLine("Signature Algorithm: {0}{1}", x509.SignatureAlgorithm.FriendlyName, Environment.NewLine);
            //Console.WriteLine("Public Key: {0}{1}", x509.PublicKey.Key.ToXmlString(false), Environment.NewLine);
            //Console.WriteLine("PrivateKey Key: {0}{1}", ((RSA)x509.PrivateKey)?.Encrypt(System.Text.ASCIIEncoding.ASCII.GetBytes("Hello"), RSAEncryptionPadding.Pkcs1), Environment.NewLine);
            //Console.WriteLine("Certificate Archived?: {0}{1}", x509.Archived, Environment.NewLine);
            //Console.WriteLine("Length of Raw Data: {0}{1}", x509.RawData.Length, Environment.NewLine);
            return x509;

        }

        public static RSAParameters GetRSAParameters(string kid, bool privateKey)
        {
            X509Certificate2 x509 = GetCertificate(kid);
            return x509.GetRSAPrivateKey().ExportParameters(privateKey);
        }
        public static string HashAndSign(string inputToSign, string kid)
        {
            X509Certificate2 x509 = GetCertificate(kid);
            System.Diagnostics.Trace.WriteLine("Certificate for signing: " + x509.GetPublicKeyString(), "Info");

            byte[] bytesInput = System.Text.Encoding.UTF8.GetBytes(inputToSign);
            byte[] bytes = x509.GetRSAPrivateKey().SignData(bytesInput, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);

            SHA512Managed sha = new SHA512Managed();
            byte[] hashedInput = sha.ComputeHash(bytesInput);
            bytes = x509.GetRSAPrivateKey().SignHash(hashedInput, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);

            string signature = System.Convert.ToBase64String(bytes);
            x509.Reset();
            return signature;
        }
        public static string HashAndSign256(string inputToSign, string kid)
        {
            X509Certificate2 x509 = GetCertificate(kid);
            System.Diagnostics.Trace.WriteLine("Certificate for signing: " + x509.GetPublicKeyString(), "Info");

            byte[] bytesInput = System.Text.Encoding.UTF8.GetBytes(inputToSign);
            byte[] bytes = x509.GetRSAPrivateKey().SignData(bytesInput, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            SHA256Managed sha = new SHA256Managed();
            byte[] hashedInput = sha.ComputeHash(bytesInput);
            bytes = x509.GetRSAPrivateKey().SignHash(hashedInput, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            string signature = System.Convert.ToBase64String(bytes);
            x509.Reset();
            return signature;
        }

        internal static bool VerifySignature(string signedInput, string jwtSignature, string kid)
        {
            X509Certificate2 x509 = GetCertificate(kid);

            System.Diagnostics.Trace.WriteLine("Certificate for signature verification: " + x509.GetPublicKeyString(), "Info");

            byte[] bytesInput = System.Text.Encoding.UTF8.GetBytes(signedInput);
            byte[] bytesSignature = System.Convert.FromBase64String(jwtSignature);
            SHA512Managed sha = new SHA512Managed();
            byte[] hashedInput = sha.ComputeHash(bytesInput);

            bool ok = x509.GetRSAPublicKey().VerifyHash(hashedInput, bytesSignature, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
            if (ok)
                ok = x509.GetRSAPublicKey().VerifyData(bytesInput, bytesSignature, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);

            x509.Reset();
            return ok;
        }


        internal static bool VerifySignature256(string signedInput, string jwtSignature, string kid)
        {
            X509Certificate2 x509 = GetCertificate(kid);

            System.Diagnostics.Trace.WriteLine("Certificate for signature verification: " + x509.GetPublicKeyString(), "Info");

            byte[] bytesInput = System.Text.Encoding.UTF8.GetBytes(signedInput);
            byte[] bytesSignature = System.Convert.FromBase64String(jwtSignature);
            SHA256Managed sha = new SHA256Managed();
            byte[] hashedInput = sha.ComputeHash(bytesInput);

            bool ok = x509.GetRSAPublicKey().VerifyHash(hashedInput, bytesSignature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            if (ok)
                ok = x509.GetRSAPublicKey().VerifyData(bytesInput, bytesSignature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            x509.Reset();
            return ok;
        }
    }

}
