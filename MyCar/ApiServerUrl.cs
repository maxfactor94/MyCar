using System;

namespace MyCar
{
    /// <summary>
    /// Normalizes the configured API base address and prevents clear-text HTTP connections.
    /// </summary>
    internal static class ApiServerUrl
    {
        public static string Normalize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            string url = value.Trim().TrimEnd('/');

            if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                url = "https://" + url.Substring("http://".Length);
            }
            else if (!url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                if (url.Contains("://"))
                    throw new UriFormatException("Адрес API должен использовать HTTPS.");

                url = "https://" + url;
            }

            return url.TrimEnd('/');
        }
    }
}