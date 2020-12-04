using System.Configuration;

namespace EBusCopyApp.Utilities
{
    public class Constants
    {
        public static string SourcePathKeys = ConfigurationManager.AppSettings["SourcePathKeys"];
        public static string RefreshTimer = ConfigurationManager.AppSettings["RefreshTimer"];

        public static string GetConfiguration(string configKey) { return ConfigurationManager.AppSettings[configKey]; }
    }
}
