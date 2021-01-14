using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FargoWebApp.Models
{
    public class FargoData
    {
        #region Data types
        public static HttpClient CLIENT = new HttpClient();
        public static List<Country> Countries = new List<Country>();
        public static List<City> Cities = new List<City>();
        public static City city = new City();
        #endregion

        #region Mothods
        /// <summary>
        /// Метод получение список страны
        /// </summary>
        /// <returns>Список страны</returns>
        public static List<Country> GetCountries()
        {  
            CLIENT.BaseAddress = new Uri(Config.Url1+Config._countryListUrl);
            CLIENT.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            CLIENT.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", Config.Token));
            var response = CLIENT.GetAsync(Config.Url1 + Config._countryListUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var countryList = Misc.JsonParse<CountryList>(result);
                Countries = countryList.data.OrderBy(p => p.id).ToList();
            }
            CLIENT.Dispose();
            return Countries;
        }
        /// <summary>
        /// Метод получение всех городов
        /// </summary>
        /// <returns>Список городов</returns>
        public static List<City> GetCities()
        {  
            CLIENT.BaseAddress = new Uri(Config.Url1 + Config._cityListUrl);
            CLIENT.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            CLIENT.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", Config.Token));
            var response = CLIENT.GetAsync(Config.Url1 + Config._cityListUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var cityList = Misc.JsonParse<CityList>(result);
                Cities = cityList.data.OrderBy(p => p.id).ToList();
            }
            CLIENT.Dispose();
            return Cities;
        }
        /// <summary>
        /// Метод поиска города
        /// </summary>
        /// <param name="cityName">Название города</param>
        /// <returns>Город</returns>
        public static City GetCityByName(string cityName)
        {  
            CLIENT.BaseAddress = new Uri(Config.Url1 + Config._getCityByName);
            CLIENT.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            CLIENT.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", Config.Token));
            var response = CLIENT.GetAsync(Config.Url1 + Config._getCityByName).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                city = Misc.JsonParse<CityByName>(result).data; 
            }
            CLIENT.Dispose();
            return city;
        }

        public static async Task<string> GetPrices1Async()
        {
            var baseAddress = new Uri("https://stagingapi.shipox.com/");

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {  
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Bearer "+Config.Token);

                using (var response = await httpClient.GetAsync("api/v1/service_types"))
                { 
                    return  response.Content.ReadAsStringAsync().ToString();
                }
            } 
        }
        #endregion
    }

    #region Classes
    public class Country
    {
        public int marketplace_id { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string currency { get; set; }
        public string bank_account_type { get; set; }
        public string sort_name { get; set; }
        public string sort_order { get; set; }
    }
    public class CountryList
    {
        public List<Country> data { get; set; }
    }
    public class City
    {
        public int id { get; set; }
        public string name { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }
    public class CityList
    {
        public List<City> data { get; set; }
    }
    public class CityByName
    {
        public City data { get; set; }
    }
    #endregion
}