using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace LoanShark.Helper
{
    public static class AppConfig
    {
        public static IConfiguration? Configuration { get; private set; }

        // Static constructor to load configuration once
        static AppConfig()
        {
            LoadConfiguration();
        }

        private static void LoadConfiguration()
        {
            string jsonFilePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\appsettings.json"));

            var builder = new ConfigurationBuilder()
                .AddJsonFile(jsonFilePath, optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        // Helper method to get values
        // !! Use throught the app like this: AppConfig.GetConnectionString("MyLocalDb");
        // This will get the connection string from the appsettings.json file in the project
        public static string? GetConnectionString(string name)
        {
            return Configuration?[$"ConnectionStrings:{name}"];
        }
    }
}
