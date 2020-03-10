using System.Configuration;

namespace EBusCopyApp.Utilities
{
    public class Constants
    {
        public static string InputFilePath = ConfigurationManager.AppSettings["InputFilePath"];
        public static string OutPutFilePath = ConfigurationManager.AppSettings["OutPutFilePath"];
        public static string BackUpFilePath = ConfigurationManager.AppSettings["BackUpFilePath"];
        public static string RefreshTimer = ConfigurationManager.AppSettings["RefreshTimer"];
    }
}
