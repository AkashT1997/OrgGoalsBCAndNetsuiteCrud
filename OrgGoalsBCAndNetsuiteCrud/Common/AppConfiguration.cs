using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace OrgGoalsBCAndNetsuiteCrud.Common
{
    public static class AppConfiguration
    {
        //public static string HostEnvironment = string.Empty;

        public static string MongoDBConnectionString = string.Empty;
        public static string MongoDatabaseName = string.Empty;

        public static string BusinessCentralClientId = string.Empty;
        public static string BusinessCentralClientSecret = string.Empty;
        public static string BusinessCentralRedirectUri = string.Empty;

        static AppConfiguration()
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

            var currentDirectory = "/home/site/wwwroot";
            bool isLocal = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID"));
            if (isLocal)
            {
                currentDirectory = Directory.GetCurrentDirectory();
            }
            var path = Path.Combine(currentDirectory, "appsettings.json");

            configurationBuilder.AddJsonFile(path, false);

            IConfigurationRoot root = configurationBuilder.Build();
            //HostEnvironment = root.GetSection("HostEnvironment").Value;

            MongoDBConnectionString = root.GetSection("MongoDB").GetSection("MongoDBConnectionString").Value;
            MongoDatabaseName = root.GetSection("MongoDB").GetSection("MongoDatabaseName").Value;
            BusinessCentralClientId = root.GetSection("BCData").GetSection("BusinessCentralClientId").Value;
            BusinessCentralClientSecret = root.GetSection("BCData").GetSection("BusinessCentralClientSecret").Value;
            BusinessCentralRedirectUri = root.GetSection("BCData").GetSection("BusinessCentralRedirectUri").Value;
        }
    }
}
