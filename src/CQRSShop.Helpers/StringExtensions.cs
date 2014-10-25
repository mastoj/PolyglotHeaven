using System;
using System.Configuration;

namespace CQRSShop.Helpers
{
    public static class StringExtensions
    {
        public static TType GetConfigSetting<TType>(this string key, Func<string, TType> convertFunc, Func<TType> defaultValueFunc)
        {
            var valueString = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(valueString))
            {
                return defaultValueFunc();
            }
            return convertFunc(valueString);
        }
    }
}