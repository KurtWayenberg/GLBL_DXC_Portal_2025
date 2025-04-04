using DXC.Technology.Configuration;
using DXC.Technology.Security;
using DXC.Technology.Translations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DXC.Technology.Caching
{
    public class UserContextService
    {
        public UserContextService()
        {
        }
        public string UserName { get; set; }
        public Dictionary<string, object> Items = new Dictionary<string, object>();
        public Dictionary<string, object> Session = new Dictionary<string, object>();
        public string GetSessionValue(string key)
        {
            if (Session.TryGetValue(key, out object value))
                return (Convert.ToString(value));
            return null;
        }
        public void SetSessionValue(string key, string value)
        {
            if (Session.TryGetValue(key, out object storedvalue))
                Session[key] = value;
            else
                Session.Add(key, value);
        }
        public bool IsAuthenticated()
        {
            return (!UserName.Equals(DXC.Technology.Security.SecurityManager.ServiceAccount));
        }
    }


    public static class ContextHelper
    {
        public static AsyncLocal<UserContextService> ThreadContext = new();
        public static AsyncLocal<string> ThreadContextUserName = new();

        public static string ApplicationLanguage
        {
            get
            {
                return GetUserLanguage() ?? "en";
            }
            set
            {
                //if (!string.IsNullOrEmpty(value))
                //    SetUserLanguage(value);
            }
        }
        public static bool IsThreadContextAuthenticated()
        {
            var isInital = string.IsNullOrEmpty(ThreadContextUserName.Value) || DXC.Technology.Security.SecurityManager.ServiceAccount.Equals(ThreadContextUserName.Value);
            return !isInital;
        }
        public static bool HasContext()
        {
            return !string.IsNullOrEmpty(GetUserName());
        }
        public static string GetUserName()
        {
            if (ThreadContext.Value != null)
                return ThreadContext.Value.GetSessionValue("__UserName");
            return null;
        }
        public static void SetUserName(string userName)
        {
            if (ThreadContext.Value != null)
                ThreadContext.Value.SetSessionValue("__UserName", userName);
        }

        public static string GetApiUserName()
        {
            if (ThreadContext.Value != null)
                return ThreadContext.Value.GetSessionValue("__ApiUserName");
            return null;
        }
        public static void SetApiUserName(string ApiUserName)
        {
            if (ThreadContext.Value != null)
                ThreadContext.Value.SetSessionValue("__ApiUserName", ApiUserName);
        }



        public static string GetUserIp()
        {
            if (ThreadContext.Value != null)
                return ThreadContext.Value.GetSessionValue("__UserIp");
            return null;
        }
        public static void SetUserIp(string userIp)
        {
            if (ThreadContext.Value != null)
                ThreadContext.Value.SetSessionValue("__UserIp", userIp);
        }

        public static string GetUserAuthToken()
        {
            if (ThreadContext.Value != null)
                return ThreadContext.Value.GetSessionValue("__UserAuthToken");
            return null;
        }
        public static void SetUserAuthToken(string UserAuthToken)
        {
            if (ThreadContext.Value != null)
                ThreadContext.Value.SetSessionValue("__UserAuthToken", UserAuthToken);
        }

        public static string GetSecurityToken()
        {
            if (ThreadContext.Value != null)
                return ThreadContext.Value.GetSessionValue("__SecurityToken");
            return null;
        }
        public static void SetSecurityToken(string SecurityToken)
        {
            if (ThreadContext.Value != null)
                ThreadContext.Value.SetSessionValue("__SecurityToken", SecurityToken);
        }

        public static string GetServiceGuid()
        {
            if (ThreadContext.Value != null)
                return ThreadContext.Value.GetSessionValue("__ServiceGuid");
            return null;
        }
        public static void SetServiceGuid(string ServiceGuid)
        {
            if (ThreadContext.Value != null)
                ThreadContext.Value.SetSessionValue("__ServiceGuid", ServiceGuid);
        }

        public static CultureInfo GetCultureInfo()
        {
            string lLanguage = GetUserLanguage();

            if (string.IsNullOrEmpty(lLanguage))
                lLanguage = "en";

            //if (ThreadContext.Value != null && !string.IsNullOrEmpty(ThreadContext.Value.GetSessionValue("__CultureInfo") ))
            //return new CultureInfo( ThreadContext.Value.GetSessionValue("__CultureInfo"));
            return new CultureInfo(lLanguage);
        }
        //public static void SetCultureInfo(CultureInfo CultureInfo)
        //{
        //    if (ThreadContext.Value != null)
        //        ThreadContext.Value.SetSessionValue("__CultureInfo", CultureInfo.Name);
        //}

        public static string GetAscendingType()
        {
            if (ThreadContext.Value != null)
                return ThreadContext.Value.GetSessionValue("__AscendingType");
            return null;
        }
        public static void SetAscendingType(string AscendingType)
        {
            if (ThreadContext.Value != null)
                ThreadContext.Value.SetSessionValue("__AscendingType", AscendingType);
        }
        public static string GetFarmUser()
        {
            if (ThreadContext.Value != null)
                return ThreadContext.Value.GetSessionValue("__FarmUser");
            return null;
        }
        public static void SetFarmUser(string FarmUser)
        {
            if (ThreadContext.Value != null)
                ThreadContext.Value.SetSessionValue("__FarmUser", FarmUser);
        }
        public static string GetDefaultFacility()
        {
            if (ThreadContext.Value != null)
                return ThreadContext.Value.GetSessionValue("__DefaultFacility");
            return null;
        }
        public static void SetDefaultFacility(string DefaultFacility)
        {
            if (ThreadContext.Value != null)
                ThreadContext.Value.SetSessionValue("__DefaultFacility", DefaultFacility);
        }
        public static string GetAuthSecurityToken()
        {
            if (ThreadContext.Value != null)
                return ThreadContext.Value.GetSessionValue("__AuthSecurityToken");
            return null;
        }
        public static void SetAuthSecurityToken(string AuthSecurityToken)
        {
            if (ThreadContext.Value != null)
                ThreadContext.Value.SetSessionValue("__AuthSecurityToken", AuthSecurityToken);
        }

        public static string GetSortingField()
        {
            if (ThreadContext.Value != null)
                return ThreadContext.Value.GetSessionValue("__SortingField");
            return null;
        }
        public static void SetSortingField(string SortingField)
        {
            if (ThreadContext.Value != null)
                ThreadContext.Value.SetSessionValue("__SortingField", SortingField);
        }
        public static string GetImpersonatedUserName()
        {
            if (ThreadContext.Value != null)
                return ThreadContext.Value.GetSessionValue("__ImpersonatedUserName");
            return null;
        }
        public static void SetImpersonatedUserName(string ImpersonatedUserName)
        {
            if (ThreadContext.Value != null)
                ThreadContext.Value.SetSessionValue("__ImpersonatedUserName", ImpersonatedUserName);
        }

        //public static string GetUserLanguage()
        //{
        //    if (ThreadContext.Value != null && !string.IsNullOrEmpty(ThreadContext.Value.GetSessionValue("__UserLanguage")))
        //        return ThreadContext.Value.GetSessionValue("__UserLanguage");
        //    return "en";
        //}

        public static string GetUserLanguage()
        {
            string lLanguage = string.Empty;

            if (ThreadContext.Value != null)
                lLanguage = ThreadContext.Value.GetSessionValue("__UserLanguage");

            if (string.IsNullOrEmpty(lLanguage))
                lLanguage = "en";

            return lLanguage;
            //return "en";
        }

        public static void SetUserLanguage(string UserLanguage)
        {
            if (ThreadContext.Value != null)
                ThreadContext.Value.SetSessionValue("__UserLanguage", UserLanguage);
        }

        public static string GetCounterInfo()
        {
            if (ThreadContext.Value != null)
                return ThreadContext.Value.GetSessionValue("__CounterInfo");
            return null;
        }
        public static void SetCounterInfo(string CounterInfo)
        {
            if (ThreadContext.Value != null)
                ThreadContext.Value.SetSessionValue("__CounterInfo", CounterInfo);
        }


        public static string GetLocalizedApplicationRootURL()
        {
            string root = DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().ApplicationRootUrl;
            return string.Concat(root, GetCultureInfo().TwoLetterISOLanguageName ?? "en", "/");
        }

        private static HttpContext _httpContext => new HttpContextAccessor().HttpContext;

        public static ITranslationProvider GetTranslationProvider()
        {
            return _httpContext.RequestServices.GetRequiredService<ITranslationProvider>();
        }

        //    public static bool HasContext()
        //    {
        //        return _httpContext != null;
        //    }
        //    public static string GetUserIp()
        //    {
        //        object userIp = "";
        //        if (_httpContext.Items.TryGetValue("__UserIp", out userIp))
        //            return Convert.ToString(userIp);
        //        else
        //        {
        //            userIp = _httpContext.Connection.RemoteIpAddress.ToString();
        //            _httpContext.Items.Add("__UserIp", userIp);
        //        }
        //        return Convert.ToString(userIp);
        //    }
        //    public static bool IsAuthenticated()
        //    {
        //        return (!GetUserName().Equals(DXC.Technology.Security.SecurityManager.ServiceAccount));
        //    }
        //    public static string GetClaimRole()
        //    {
        //        return _httpContext.User?.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Role)?.Value;
        //    }
        //    public static string GetUserName()
        //    {
        //        string userName = _httpContext?.User?.Claims?.FirstOrDefault(p => p.Type == ClaimTypes.Name)?.Value;
        //        if (string.IsNullOrEmpty(userName))
        //        {
        //            userName = RetrieveStringFromSession("__UserName");
        //            if (string.IsNullOrEmpty(userName))
        //                userName = DXC.Technology.Security.SecurityManager.ServiceAccount;
        //        }
        //        return userName;
        //    }

        //    //public static string GetUserAuthToken()
        //    //{
        //    //    string UserAuthToken = RetrieveStringFromSession("__Token");
        //    //    return UserAuthToken;
        //    //}

        //    //public static string GetUserLanguage()
        //    //{
        //    //    string UserLanguage = RetrieveStringFromSession("__Language");
        //    //    return UserLanguage;
        //    //}

        //    //public static string GetFarmUser()
        //    //{
        //    //    string FarmUser = RetrieveStringFromSession("__FarmUser");
        //    //    return FarmUser;
        //    //}

        //    //public static string GetDefaultFacilty()
        //    //{
        //    //    string DefaultFacilty = RetrieveStringFromSession("__DefaultFacilty");
        //    //    return DefaultFacilty;
        //    //}

        //    public static bool HasPlayedVideo(int id)
        //    {
        //        if (DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().ApplicationEnvironment == "DEV") return true;
        //        var helpvideos = GetLoggedOnUserSecurityManager().GetUserSetting("HelpVideos");
        //        return helpvideos == null || helpvideos.Contains(id.ToString());
        //    }

        //    public static string GetImpersonatedUserName()
        //    {
        //        var impersonatedUserName = RetrieveStringFromSession("__ImpersonatedUserName");
        //        return (!string.IsNullOrEmpty(impersonatedUserName)) ? impersonatedUserName : GetUserName();
        //    }


        //    public static long GetPartyId()
        //    {
        //        var sm = GetSecurityManager();
        //        if (sm == null) return 0;
        //        var partyAsString = sm.GetUserSetting("DefaultOrganization");
        //        if (string.IsNullOrEmpty(partyAsString)) return 0;

        //        return Convert.ToInt64(partyAsString);
        //    }

        //    public static string GetPartyCode()
        //    {
        //        var sm = GetSecurityManager();
        //        if (sm == null) return "";
        //        return sm.GetUserSetting("DefaultParty");

        //    }
        //    public static void SetUserName(string userName)
        //    {
        //        StoreStringInSession("__UserName", userName);
        //        RefreshSecurityManager();
        //    }
        //    //public static void SetAuthSecurityToken(string token)
        //    //{
        //    //    StoreStringInSession("__Token", token);
        //    //}

        //    //public static void SetUserLanguage(string language)
        //    //{
        //    //    StoreStringInSession("__Language", language);
        //    //}

        //    //public static void SetFarmUser(string FarmUser)
        //    //{
        //    //    StoreStringInSession("__FarmUser", FarmUser);
        //    //}

        //    //public static void SetDefaultFacilty(string DefaultFacilty)
        //    //{
        //    //    StoreStringInSession("__DefaultFacilty", DefaultFacilty);
        //    //}

        //    //public static void SetAscendingType(string AscendingType)
        //    //{
        //    //    StoreStringInSession("__AscendingType", AscendingType);
        //    //}

        //    //public static void SetSortingField(string SortingField)
        //    //{
        //    //    StoreStringInSession("__SortingField", SortingField);
        //    //}

        //    public static void SetImpersonatedUserName(string impersonatedUserName)
        //    {
        //        StoreStringInSession("__ImpersonatedUserName", impersonatedUserName);
        //        RefreshSecurityManager();
        //    }

        //    public static string GetServiceGuid()
        //    {

        //        if (_httpContext.Items != null && _httpContext.Items.TryGetValue("__ServiceGuid", out var serviceguid))
        //        {
        //            _httpContext.Items.Add("__ServiceGuid", System.Guid.NewGuid().ToByteArray());
        //        }
        //        return Convert.ToString(_httpContext.Items["__ServiceGuid"]);
        //    }
        //    public static string GetApiUserName()
        //    {

        //        if (!_httpContext.Items.TryGetValue("__ApiUserName", out var apiusername))
        //        {
        //            _httpContext.Items.Add("__ApiUserName", GetUserName());
        //        }
        //        return Convert.ToString(_httpContext.Items["__ApiUserName"]);
        //    }

        //    public static void SetApiUserName(string apiUserName)
        //    {

        //        if (_httpContext.Items.TryGetValue("__ApiUserName", out var apiUserNameOld))
        //        {
        //            _httpContext.Items.Remove("__ApiUserName");
        //        }

        //        _httpContext.Items.Add("__ApiUserName", apiUserName);
        //    }
        //    public static System.Globalization.CultureInfo GetCultureInfo()
        //    {
        //        return new System.Globalization.CultureInfo("nl-BE");
        //    }
        //    public static System.Globalization.CultureInfo GetCultureInfo(string languageFromRoute)
        //    {
        //        if (_httpContext.Features.Get<Microsoft.AspNetCore.Http.Features.ISessionFeature>()?.Session != null)
        //        {
        //            var userCI = RetrieveCultureInfoFromSession();
        //            if (string.IsNullOrEmpty(languageFromRoute) || userCI.TwoLetterISOLanguageName.ToLower().Equals(languageFromRoute))
        //                return userCI;
        //        }

        //        System.Globalization.CultureInfo newCultureInfo = new System.Globalization.CultureInfo("nl-BE");
        //        switch (languageFromRoute)
        //        {
        //            case "nl":
        //                newCultureInfo = new System.Globalization.CultureInfo("nl-BE");
        //                break;
        //            case "fr":
        //                newCultureInfo = new System.Globalization.CultureInfo("fr-BE");
        //                break;
        //            case "en":
        //                newCultureInfo = new System.Globalization.CultureInfo("en-US");
        //                break;
        //            case "az":
        //                newCultureInfo = new System.Globalization.CultureInfo("az-AZ");
        //                break;
        //        }

        //        if (!_httpContext.Items.TryGetValue("__CultureInfo", out var cultureInfoOld))
        //        {
        //            _httpContext.Items.Add("__CultureInfo", newCultureInfo);
        //        }
        //        else
        //        {
        //            string cultureInfoOldString = cultureInfoOld.ToString();
        //            if (!cultureInfoOldString.Equals(newCultureInfo))
        //            {
        //                _httpContext.Items.Remove("__CultureInfo");
        //                _httpContext.Items.Add("__CultureInfo", newCultureInfo);
        //                if (_httpContext.Features.Get<Microsoft.AspNetCore.Http.Features.ISessionFeature>()?.Session != null)
        //                {
        //                    StoreCultureInfoInSession(newCultureInfo);
        //                }
        //            }
        //            else
        //            {
        //                return newCultureInfo;
        //            }
        //        }
        //        return (System.Globalization.CultureInfo)(_httpContext.Items["__CultureInfo"]);
        //    }

        //    public static void SetCultureInfo(System.Globalization.CultureInfo cultureInfo)
        //    {

        //        if (_httpContext.Items.TryGetValue("__CultureInfo", out var cultureInfoold))
        //        {
        //            _httpContext.Items.Remove("__CultureInfo");
        //        }

        //        _httpContext.Items.Add("__CultureInfo", cultureInfo);
        //    }


        //    public static DXC.Technology.PerformanceCounters.EndToEndPerformanceRecord GetEndToEndPerformanceRecord()
        //    {
        //        if (!_httpContext.Items.TryGetValue("__EndToEndPerformanceRecord", out var endToEndPerformanceRecord))
        //        {
        //            _httpContext.Items.Add("__EndToEndPerformanceRecord", new DXC.Technology.PerformanceCounters.EndToEndPerformanceRecord());
        //        }
        //        return (DXC.Technology.PerformanceCounters.EndToEndPerformanceRecord)_httpContext.Items["__EndToEndPerformanceRecord"];
        //    }

        //    public static void SetEndToEndPerformanceRecord(DXC.Technology.PerformanceCounters.EndToEndPerformanceRecord endToEndPerformanceRecord)
        //    {

        //        if (_httpContext.Items.TryGetValue("__EndToEndPerformanceRecord", out var twoletterisolanguagenameold))
        //        {
        //            _httpContext.Items.Remove("__EndToEndPerformanceRecord");
        //        }

        //        _httpContext.Items.Add("__EndToEndPerformanceRecord", endToEndPerformanceRecord);
        //    }



        //    public static DXC.Technology.PerformanceCounters.BasicEndToEndPerformanceRecord GetBasicEndToEndPerformanceRecord()
        //    {
        //        if (!_httpContext.Items.TryGetValue("__BasicEndToEndPerformanceRecord", out var BasicEndToEndPerformanceRecord))
        //        {
        //            _httpContext.Items.Add("__BasicEndToEndPerformanceRecord", new DXC.Technology.PerformanceCounters.BasicEndToEndPerformanceRecord());
        //        }
        //        return (DXC.Technology.PerformanceCounters.BasicEndToEndPerformanceRecord)_httpContext.Items["__BasicEndToEndPerformanceRecord"];
        //    }

        //    public static void SetBasicEndToEndPerformanceRecord(DXC.Technology.PerformanceCounters.BasicEndToEndPerformanceRecord BasicEndToEndPerformanceRecord)
        //    {

        //        if (_httpContext.Items.TryGetValue("__BasicEndToEndPerformanceRecord", out var twoletterisolanguagenameold))
        //        {
        //            _httpContext.Items.Remove("__BasicEndToEndPerformanceRecord");
        //        }

        //        _httpContext.Items.Add("__BasicEndToEndPerformanceRecord", BasicEndToEndPerformanceRecord);
        //    }


        //    public static DXC.Technology.Security.AuthenticationInformation GetAuthenticationInformation()
        //    {
        //        if (!_httpContext.Items.TryGetValue("__AUTHINFO__", out var oldAuthenticationInformation))
        //        {
        //            _httpContext.Items.Add("__AUTHINFO__", new DXC.Technology.Security.AuthenticationInformation());
        //        }
        //        return (DXC.Technology.Security.AuthenticationInformation)_httpContext.Items["__AUTHINFO__"];
        //    }

        //    public static void SetAuthenticationInformation(DXC.Technology.Security.AuthenticationInformation authenticationInformation)
        //    {

        //        if (_httpContext.Items.TryGetValue("__AUTHINFO__", out var oldAuthenticationInformation))
        //        {
        //            _httpContext.Items.Remove("__AUTHINFO__");
        //        }

        //        _httpContext.Items.Add("__AUTHINFO__", authenticationInformation);
        //    }


        //    public static NameValueCollection GetQueryArgumentsAsNameValueCollection()
        //    {
        //        NameValueCollection result = new NameValueCollection();
        //        foreach (var kv in _httpContext.Request.Query)
        //        {
        //            result.Add(kv.Key, kv.Value);
        //        }
        //        return result;
        //    }

        //    public static string GetController()
        //    {
        //        return _httpContext.Request.RouteValues["controller"].ToString();
        //    }

        //    public static string GetLocalizedApplicationRootURL()
        //    {
        //        string root = DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().ApplicationRootUrl;
        //        return string.Concat(root, GetCultureInfo("").TwoLetterISOLanguageName, "/");
        //    }

        //    public static string GetSecurityToken()
        //    {

        //        if (_httpContext.Items.TryGetValue("__SecurityToken", out var securitytoken))
        //        {
        //            return "";
        //        }
        //        return Convert.ToString(_httpContext.Items["__SecurityToken"]);
        //    }

        //    public static void SetSecurityToken(string securityToken)
        //    {

        //        if (_httpContext.Items.TryGetValue("__SecurityToken", out var apiusername))
        //        {
        //            _httpContext.Items.Remove("__SecurityToken");
        //        }

        //        _httpContext.Items.Add("__SecurityToken", securityToken);
        //    }
        //    public static string GetTraceId()
        //    {
        //        return _httpContext.TraceIdentifier;
        //    }

        //    public static void StoreInSession<T>(long id, T entity)
        //        => StoreInSession<T>(id.ToString(), entity);

        //    public static void StoreInSession<T>(string id, T entity)
        //    {
        //        string key = string.Format("{0}|{1}", typeof(T).Name, id);
        //        string serializedEntity = DXC.Technology.Objects.SerializationHelper.ObjectSerializeToJson(entity);
        //        StoreStringInSession(key, serializedEntity);
        //    }

        //    public static void RemoveFromSession(string id)
        //        => RemoveFromSession<string>(id);
        //    public static void RemoveFromSession<T>(string id)
        //    {
        //        string key = string.Format("{0}|{1}", typeof(T).Name, id);
        //        // string serializedEntity = DXC.Technology.Objects.SerializationHelper.ObjectSerializeToJson(entity);
        //        if (!DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().SessionLess)
        //        {
        //            try
        //            {
        //                _httpContext.Session.Remove(key);
        //            }
        //            catch
        //            {
        //                //Ignore
        //            }
        //        }
        //    }

        //    public static T RetrieveFromSession<T>(long id)
        //        => RetrieveFromSession<T>(id.ToString());

        //    public static T RetrieveFromSession<T>(string id)
        //    {
        //        string key = string.Format("{0}|{1}", typeof(T).Name, id);
        //        string value = RetrieveStringFromSession(key);
        //        if (string.IsNullOrEmpty(value)) return default(T);

        //        return DXC.Technology.Objects.SerializationHelper.JsonDeserializeToObject<T>(value);
        //    }

        //    public static bool ExistsInSession<T>(string id)
        //    {
        //        string key = string.Format("{0}|{1}", typeof(T).Name, id);
        //        if (!DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().SessionLess)
        //        {
        //            return _httpContext.Session.Keys.Contains(key);
        //        }

        //        return false;
        //    }

        //    public static void StoreStringInSession(string key, string information)
        //    {
        //        if (!DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().SessionLess)
        //        {
        //            try
        //            {
        //                if (information != null)
        //                    _httpContext.Session.Set(key, System.Text.UTF8Encoding.UTF8.GetBytes(information));
        //                else
        //                    _httpContext.Session.Remove(key);

        //            }
        //            catch
        //            {
        //                //Ignore
        //            }
        //        }
        //    }
        //    public static string RetrieveStringFromSession(string key)
        //    {
        //        if (!DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().SessionLess)
        //        {
        //            try
        //            {
        //                if (_httpContext.Session.TryGetValue(key, out var informationAsBytes))
        //                    return System.Text.UTF8Encoding.UTF8.GetString(informationAsBytes);
        //            }
        //            catch
        //            {
        //                return null;
        //            }
        //        }

        //        return default(string);

        //    }
        //    public static void StoreCultureInfoInSession(System.Globalization.CultureInfo info)
        //    {
        //        if (!DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().SessionLess)
        //        {
        //            try
        //            {
        //                _httpContext.Session.SetString("Culture", info.TwoLetterISOLanguageName.ToString());
        //            }
        //            catch
        //            {
        //                //Ignore
        //            }
        //        }
        //    }
        //    public static System.Globalization.CultureInfo RetrieveCultureInfoFromSession()
        //    {
        //        var ciName = RetrieveStringFromSession("Culture");
        //        if (string.IsNullOrEmpty(ciName))
        //        {
        //            var ci = new System.Globalization.CultureInfo("nl-be");
        //            ciName = ci.Name;
        //            //StoreCultureInfoInSession(ci);
        //        }
        //        return new System.Globalization.CultureInfo(ciName);
        //    }

        //    public static string GetContextInfo()
        //    {
        //        System.IO.StringWriter lswContextInfo = new System.IO.StringWriter();
        //        var userName = _httpContext.User?.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name)?.Value;
        //        lswContextInfo.WriteLine("User: " + userName);
        //        if (!DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().SessionLess)
        //        {
        //            if (!_httpContext.Session.TryGetValue("FIRST", out var whocares))
        //            {
        //                _httpContext.Session.Set("FIRST", System.Text.UTF8Encoding.UTF8.GetBytes(DateTime.Now.ToString()));
        //            }
        //            _httpContext.Session.TryGetValue("FIRST", out whocares);

        //            lswContextInfo.WriteLine("Session Id: " + _httpContext.Session.Id);
        //            lswContextInfo.WriteLine("Session First: " + System.Text.UTF8Encoding.UTF8.GetString(whocares));
        //        }
        //        if (!_httpContext.Items.TryGetValue("FIRST", out var whocares2))
        //        {
        //            _httpContext.Items.Add("FIRST", System.Text.UTF8Encoding.UTF8.GetBytes(DateTime.Now.ToString()));
        //        }
        //        _httpContext.Items.TryGetValue("FIRST", out whocares2);
        //        lswContextInfo.WriteLine("Items First: " + System.Text.UTF8Encoding.UTF8.GetString((System.Byte[])whocares2));

        //        return lswContextInfo.ToString();
        //    }

        //    public static TechnologyBasicContextInfo GetTechnologyBasicContextInfo()
        //    {
        //        TechnologyBasicContextInfo tbci = (TechnologyBasicContextInfo)_httpContext.Items["TechnologyBasicContextInfo"];
        //        if (tbci == null)
        //        {
        //            tbci = new TechnologyBasicContextInfo();
        //            _httpContext.Items.Add("TechnologyBasicContextInfo", tbci);
        //        }
        //        return tbci;
        //    }

        //    public static IEmailSender GetEmailSender()
        //    {
        //        return _httpContext.RequestServices.GetRequiredService<IEmailSender>();

        //    }
        //    public static LoggingMessageInspector GetLoggingMessageInspector()
        //    {
        //        return _httpContext.RequestServices.GetRequiredService<LoggingMessageInspector>();

        //    }
        //    public static UserProfileMessageClientInspector GetUserProfileMessageClientInspector()
        //    {
        //        return _httpContext.RequestServices.GetRequiredService<UserProfileMessageClientInspector>();

        //    }
        //    private static DXC.Technology.Security.ISecurityManagerFactory sSecurityManagerFactory = null;
        //    public static DXC.Technology.Security.ISecurityManagerFactory SecurityManagerFactory
        //    {
        //        get
        //        {
        //            return sSecurityManagerFactory;
        //        }
        //        set
        //        {
        //            sSecurityManagerFactory = value;
        //        }
        //    }
        //    public static void RefreshSecurityManager()
        //    {
        //        string lockstring = (!string.IsNullOrEmpty(GetUserName())) ? GetUserName() : "--ANY--";
        //        lock (lockstring)
        //        {
        //            if (_httpContext.Items.ContainsKey("__SecurityManager"))
        //                _httpContext.Items.Remove("__SecurityManager");
        //            if (_httpContext.Items.ContainsKey("__SecurityManagerForLoggedOnUser"))
        //                _httpContext.Items.Remove("__SecurityManagerForLoggedOnUser");
        //            if (_httpContext.Items.ContainsKey("__InformationOwner"))
        //                _httpContext.Items.Remove("__InformationOwner");
        //        }
        //    }

        //    public static DXC.Technology.Security.ISecurityManager GetSecurityManager()
        //    {

        //        if (_httpContext.Items.TryGetValue("__SecurityManager", out var securityManager))
        //        {
        //            return (DXC.Technology.Security.ISecurityManager)securityManager;
        //        }
        //        else
        //        {
        //            if (!string.IsNullOrEmpty(GetImpersonatedUserName()))
        //            {
        //                if (SecurityManagerFactory == null)
        //                    throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("TechnologyServiceContextInfo sSecurity Manager Factory");

        //                lock (DXC.Technology.Objects.KeyedStringLocks.GetKeyedLock(GetImpersonatedUserName()))
        //                {
        //                    securityManager = SecurityManagerFactory.GetSecurityManager(GetImpersonatedUserName());
        //                    _httpContext.Items.Add("__SecurityManager", securityManager);
        //                }
        //            }
        //            return (DXC.Technology.Security.ISecurityManager)securityManager;
        //        }
        //    }
        //    public static DXC.Technology.Security.ISecurityManager GetLoggedOnUserSecurityManager()
        //    {

        //        if (_httpContext.Items.TryGetValue("__SecurityManagerForLoggedOnUser", out var securityManager))
        //        {
        //            return (DXC.Technology.Security.ISecurityManager)securityManager;
        //        }
        //        else
        //        {
        //            if (!string.IsNullOrEmpty(GetUserName()))
        //            {
        //                if (SecurityManagerFactory == null)
        //                    throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("TechnologyServiceContextInfo sSecurity Manager Factory");

        //                lock (DXC.Technology.Objects.KeyedStringLocks.GetKeyedLock(GetUserName()))
        //                {
        //                    securityManager = SecurityManagerFactory.GetSecurityManager(GetUserName());
        //                    _httpContext.Items.Add("__SecurityManagerForLoggedOnUser", securityManager);
        //                }
        //            }
        //            return (DXC.Technology.Security.ISecurityManager)securityManager;
        //        }
        //    }
        //    public static DXC.Technology.Security.InformationOwner GetInformationOwner()
        //    {

        //        if (_httpContext.Items.TryGetValue("__InformationOwner", out var informationOwner))
        //        {
        //            return (DXC.Technology.Security.InformationOwner)informationOwner;
        //        }
        //        else
        //        {
        //            if (!string.IsNullOrEmpty(GetImpersonatedUserName()))
        //            {
        //                if (SecurityManagerFactory == null)
        //                    throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("TechnologyServiceContextInfo sSecurity Manager Factory");

        //                lock (DXC.Technology.Objects.KeyedStringLocks.GetKeyedLock(GetImpersonatedUserName()))
        //                {
        //                    informationOwner = SecurityManagerFactory.GetInformationOwner(GetImpersonatedUserName());
        //                    if (informationOwner != null)
        //                        _httpContext.Items.Add("__InformationOwner", informationOwner);
        //                }
        //            }
        //            return (DXC.Technology.Security.InformationOwner)informationOwner;
        //        }
        //    }

        //    public static DXC.Technology.Security.InformationOwner GetLoggedOnInformationOwner()
        //    {

        //        if (_httpContext.Items.TryGetValue("__LoggedOnInformationOwner", out var informationOwner))
        //        {
        //            return (DXC.Technology.Security.InformationOwner)informationOwner;
        //        }
        //        else
        //        {
        //            if (!string.IsNullOrEmpty(GetUserName()))
        //            {
        //                if (SecurityManagerFactory == null)
        //                    throw new DXC.Technology.Exceptions.NamedExceptions.MandatoryParameterNotSpecifiedException("TechnologyServiceContextInfo sSecurity Manager Factory");

        //                lock (DXC.Technology.Objects.KeyedStringLocks.GetKeyedLock(GetUserName()))
        //                {
        //                    informationOwner = SecurityManagerFactory.GetInformationOwner(GetUserName());
        //                    _httpContext.Items.Add("__LoggedOnInformationOwner", informationOwner);
        //                }
        //            }
        //            return (DXC.Technology.Security.InformationOwner)informationOwner;
        //        }
        //    }

        //    public static string GetAffinity()
        //    {
        //        if (!DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().SessionLess)
        //        {
        //            if (!_httpContext.Session.TryGetValue("__Affinity", out var affinity))
        //            {
        //                var securityManager = SecurityManagerFactory.GetSecurityManager(GetUserName());
        //                string useraffinity = securityManager.GetUserSetting("Affinity") ?? DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().DefaultAffinity;
        //                StoreStringInSession("__Affinity", useraffinity);
        //            }
        //            return RetrieveStringFromSession("__Affinity");
        //        }
        //        else
        //        {
        //            return DXC.Technology.Configuration.ApplicationOptionsManager.Current.GetApplicationOptions().DefaultAffinity;
        //        }
        //    }

        //    public static void SetAffinity(string affinity)
        //    {
        //        StoreStringInSession("__Affinity", affinity);
        //    }


        //    public static Microsoft.AspNetCore.Routing.RouteValueDictionary GetRouteData()
        //    {
        //        return _httpContext.Request.RouteValues;
        //    }
    }

    public class NavigationFavorite
    {
        public string ScreenCode { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
    }
}
