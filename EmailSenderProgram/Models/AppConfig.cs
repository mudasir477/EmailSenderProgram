using EmailSenderProgram.Shared;
using System;
using System.Configuration;

namespace EmailSenderProgram.Models
{
    public class AppConfig
    {
        public string FromEmail { get; set; }
        public bool IsDebugMode { get; set; }
        public string EmailHost { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TemplatesFolderName { get; set; }

        public string LogFilePath { get; set; }
        public string RollingInterval { get; set; }

        private static readonly Lazy<AppConfig> lazyInstance =
            new Lazy<AppConfig>(() => new AppConfig());

        private AppConfig()
        {

            FromEmail = ConfigurationManager.AppSettings[Constants.ConfigurationKeys.FromEmail];
            IsDebugMode = bool.Parse(ConfigurationManager.AppSettings[Constants.ConfigurationKeys.IsDebugMode]);
            EmailHost = ConfigurationManager.AppSettings[Constants.ConfigurationKeys.EmailHost];
            Port = int.Parse(ConfigurationManager.AppSettings[Constants.ConfigurationKeys.Port]);
            UserName = ConfigurationManager.AppSettings[Constants.ConfigurationKeys.UserName];
            Password = ConfigurationManager.AppSettings[Constants.ConfigurationKeys.Password];
            TemplatesFolderName = ConfigurationManager.AppSettings[Constants.ConfigurationKeys.TemplatesFolderName];
            LogFilePath = ConfigurationManager.AppSettings[Constants.ConfigurationKeys.LogFilePath];
            RollingInterval = ConfigurationManager.AppSettings[Constants.ConfigurationKeys.RollingInterval];
        }

        public static AppConfig GetInstance()
        {
            return lazyInstance.Value;
        }
    }
}
