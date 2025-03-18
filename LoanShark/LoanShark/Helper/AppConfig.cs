using System;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LoanShark.Helper
{
    public static class AppConfig
    {
        public static IConfiguration? configuration { get; private set; }

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

            configuration = builder.Build();
        }

        // Helper method to get values
        // !! Use throught the app like this: AppConfig.GetConnectionString("MyLocalDb");
        // This will get the connection string from the appsettings.json file in the project
        public static string? GetConnectionString(string name)
        {
            return configuration[$"ConnectionStrings:{name}"];
        }
    }
}
