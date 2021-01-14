using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace FargoWebApp.Models
{
    public class Misc
    {
        public static readonly CultureInfo CultRU = new CultureInfo("ru-RU");
        public static readonly JsonSerializerSettings JsonParseSettings = new JsonSerializerSettings() { Culture = CultRU, DateFormatHandling = DateFormatHandling.IsoDateFormat, FloatParseHandling = FloatParseHandling.Decimal };

        //======================================================================
        #region Безопасное преобразование типов:
        //======================================================================
        public static bool SAFE_ToBoolean(string value, bool defaultValue)
        {
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            value = value.ToLower();
            return (value == "true" || value == "1" || value == "yes");
        }

        public static decimal SAFE_ToDecimal(string value, decimal defaultValue)
        {
            if (string.IsNullOrWhiteSpace(value)) return defaultValue;

            value = value.Replace(" ", "").Replace(',', '.');
            if (decimal.TryParse(value, out decimal result) || decimal.TryParse(value.Replace('.', ','), out result))
                return result;
            return defaultValue;
        }

        public static int SAFE_ToInt32(string value, int defaultValue)
        {
            if (string.IsNullOrWhiteSpace(value)) return defaultValue;

            if (int.TryParse(value, out int result))
                return result;
            return defaultValue;
        }

        public static long SAFE_ToInt64(string value, long defaultValue)
        {
            if (string.IsNullOrWhiteSpace(value)) return defaultValue;

            if (long.TryParse(value, out long result))
                return result;
            return defaultValue;
        }

        public static DateTime SAFE_ToDateTime(string value, DateTime defaultValue)
        {
            if (string.IsNullOrWhiteSpace(value)) return defaultValue;

            DateTime result;
            try { result = DateTime.ParseExact(value, "dd.MM.yyyy", CultRU); }
            catch
            {
                if (!DateTime.TryParse(value, out result))
                    return defaultValue;
            }
            return result;
        }

        public static DateTime SAFE_ToDateTime(string value, string format, DateTime defaultValue)
        {
            try { return DateTime.ParseExact(value, format, CultRU); }
            catch { return SAFE_ToDateTime(value, defaultValue); }
        }

        public static string SAFE_ReaderString(IDataReader reader, string fieldName)
        {
            int colIndex = reader.GetOrdinal(fieldName);
            if (reader.IsDBNull(colIndex))
                return null;
            return reader.GetString(colIndex);
        }

        public static DateTime SAFE_ReaderDateTime(IDataReader reader, string fieldName, DateTime defaultValue)
        {
            int colIndex = reader.GetOrdinal(fieldName);
            if (reader.IsDBNull(colIndex))
                return defaultValue;
            return reader.GetDateTime(colIndex);
        }

        public static decimal SAFE_ReaderDecimal(IDataReader reader, string fieldName, decimal defaultValue)
        {
            int colIndex = reader.GetOrdinal(fieldName);
            if (reader.IsDBNull(colIndex))
                return defaultValue;
            return reader.GetDecimal(colIndex);
        }

        public static bool SAFE_ReaderBoolean(IDataReader reader, string fieldName, bool defaultValue)
        {
            int colIndex = reader.GetOrdinal(fieldName);
            if (reader.IsDBNull(colIndex))
                return defaultValue;
            return (Convert.ToInt32(reader.GetValue(colIndex)) == 1);
        }

        public static int SAFE_ReaderInt32(IDataReader reader, string fieldName, int defaultValue)
        {
            int colIndex = reader.GetOrdinal(fieldName);
            if (reader.IsDBNull(colIndex))
                return defaultValue;
            return reader.GetInt32(colIndex);
        }

        public static long SAFE_ReaderInt64(IDataReader reader, string fieldName, long defaultValue)
        {
            int colIndex = reader.GetOrdinal(fieldName);
            if (reader.IsDBNull(colIndex))
                return defaultValue;
            return reader.GetInt64(colIndex);
        }
        public static Guid SAFE_ReaderGuid(IDataReader reader, string fieldName)
        {
            int colIndex = reader.GetOrdinal(fieldName);

            if (reader.IsDBNull(colIndex))
                return Guid.Empty;
            return reader.GetGuid(colIndex);
        }
        public static byte[] SAFE_ReaderByteArray(IDataReader reader, string fieldName)
        {
            int colIndex = reader.GetOrdinal(fieldName);

            if (reader.IsDBNull(colIndex))
                return null;
            return (byte[])reader.GetValue(colIndex);
        }
        //======================================================================
        #endregion
        //======================================================================

        public static bool IsEmailValid(string email)
        {
            try { return (new System.Net.Mail.MailAddress(email).Address == email); }
            catch { return false; }
        }
        public static T JsonParse<T>(string json)
        {
            if (!string.IsNullOrWhiteSpace(json))
                return JsonConvert.DeserializeObject<T>(json, JsonParseSettings);
            return default(T);
        } 
        public static string JsonFrom(object obj)
        {
            if (obj == null) return null;
            return JsonConvert.SerializeObject(obj, JsonParseSettings);
        }
        public static string UrlCombine(string baseUrl, string relUrl)
        {
            return new Uri(new Uri(baseUrl), relUrl).ToString();
        }  
    }
}