using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace FargoWebApp.Models
{
    public class Config
    {

        #region Static datas
        public static readonly bool DevMode;
        public static readonly string Token;
        public static readonly string Url1;
        public static readonly string Url2; 
        public static readonly string _authenticateUrl;
        public static readonly string _countryListUrl;
        public static readonly string _cityListUrl;
        public static readonly string _cityItemUrl; 
        public static readonly string _packageMeuListUrl;
        public static readonly string _priceListUrl;
        public static readonly string _priceListUrlV2;
        public static readonly string _createOrderUrl; 
        public static readonly string _createOrderV2Url;
        public static readonly string _marketplaceUrl;
        public static readonly string _getOrderDetailsUrl;
        public static readonly string _getCityByName; 
        public static readonly string _getLocationByAddress;
        public static readonly string _getAirwaybill;
        public static readonly string _updateOrderStatus;
        public static readonly string _getServiceTypes;
        public static HttpClient Client = new HttpClient();
        #endregion 

        static Config()
        {
            DevMode = Misc.SAFE_ToBoolean(GetAppSetting(nameof(DevMode)), true);
            if (DevMode)
                Url1 = Url2 = GetAppSetting("test_url");
            else
            {
                Url1 = GetAppSetting("live_url_1");
                Url2 = GetAppSetting("live_url_2");
            }
            _authenticateUrl = GetAppSetting(nameof(_authenticateUrl));
            _countryListUrl = GetAppSetting(nameof(_countryListUrl));
            _cityListUrl = GetAppSetting(nameof(_cityListUrl));
            _cityItemUrl = GetAppSetting(nameof(_cityItemUrl));

            _packageMeuListUrl = GetAppSetting(nameof(_packageMeuListUrl));
            _priceListUrl = GetAppSetting(nameof(_priceListUrl));
            _priceListUrlV2 = GetAppSetting(nameof(_priceListUrlV2));
            _createOrderUrl = GetAppSetting(nameof(_createOrderUrl));

            _getLocationByAddress = GetAppSetting(nameof(_getLocationByAddress));
            _getAirwaybill = GetAppSetting(nameof(_getAirwaybill));
            _updateOrderStatus = GetAppSetting(nameof(_updateOrderStatus));
            _getServiceTypes = GetAppSetting(nameof(_getServiceTypes));

            Token =Authorize();
        }

        private static string GetAppSetting(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            string value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(value))
                throw new Exception("Не найден параметр \"" + key + "\" в файле конфигурации!");
            return value;
        }

        private static string Authorize()
        {
            long stop = 0;
            Auth auth = new Auth
            {
                username = "zhambylbeisenaly@gmail.com",
                password = "geekJ5244",
                remember_me = "true"
            };
            Client.BaseAddress = new Uri(Url1+ _authenticateUrl);
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var json = Misc.JsonFrom(auth);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response =Client.PostAsync(Url1+ _authenticateUrl, data);
            while (!response.IsCompleted) { stop++; if (stop == 4450111555) { break; } }
            if (response.IsCompleted && response.Result.IsSuccessStatusCode)
            { 
                AuthResponse AuthResponse = Misc.JsonParse<AuthResponse>(response.Result.Content.ReadAsStringAsync().Result);
                Client.Dispose();
                return AuthResponse.data.id_token;
            }
            Client.Dispose(); 
            throw new UnauthorizedAccessException("Не правильный пароль или логин! Stopped:"+stop); 
        }
 
    }
    #region Classes
    public class Auth
    {
        public string username { get; set; }
        public string password { get; set; }
        public string remember_me { get; set; }
    }
    public class AuthResponse
    {
        public data data { get; set; }
        public string status { get; set; }

    }
    public class data
    {
        public string id_token { get; set; }
        public user user { get; set; }
    }
    public class user
    {
        public string version { get; set; }
        public string login { get; set; }
        public string user_id { get; set; }
        public string marketplace_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string activated { get; set; }
        public string languages { get; set; }
        public string phone { get; set; }
        public string full_name { get; set; }
        public string valid_phone { get; set; }
    }
    #endregion
}