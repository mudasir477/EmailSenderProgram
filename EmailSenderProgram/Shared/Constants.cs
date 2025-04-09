namespace EmailSenderProgram.Shared
{
    public static class Constants
    {
        public static class ConfigurationKeys
        {
            public static readonly string FromEmail = "FromEmail";
            public static readonly string IsDebugMode = "IsDebugMode";
            public static readonly string EmailHost = "EmailHost";
            public static readonly string Port = "Port";
            public static readonly string UserName = "UserName";
            public static readonly string Password = "Password";
            public static readonly string TemplatesFolderName = "TemplatesFolderName";
            public static readonly string LogFilePath = "LogFilePath";
            public static readonly string RollingInterval = "RollingInterval";
        }
    }
}
