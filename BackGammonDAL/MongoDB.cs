using BackGammonDAL;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace BackGammonDAL
{
    public class MongoDB
    {
        public IMongoDatabase Client;

        public MongoDB(string dbName)
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"BackGammonServicesApi\bin\" }, StringSplitOptions.None)[0] + "BackGammonDAL";
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .Build();
            string? connectionString = config.GetConnectionString("DefaultConnection");
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            var client = new MongoClient(settings);
            Client = client.GetDatabase(dbName);
        }
    }
}



