using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArticoliWebService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ArticoliWebService.Test
{
    public class DbContextMocker
    {
        public static AlphaShopDbContext alphaShopDbContext()
        {
            // 1. Stringa di connessione per i test
            var connectionString = "Data Source=localhost;Initial Catalog=AlphaShop;TrustServerCertificate=True;User ID=ApiClient;Password=123_Stella!";

            // 2. Creazione della IConfiguration mock con la stringa di connessione.
            // **ATTENZIONE:** Sostituisci "AlphaShopDb" con il nome esatto della chiave
            // usata nel tuo file appsettings.json e nel tuo metodo OnConfiguring.
            var configData = new Dictionary<string, string>
            {
                {"ConnectionStrings:DefaultConnection", connectionString}
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(configData) // Carica i dati nel Configuration Builder
                .Build();

            // 3. Configurazione del DbContextOptions.
            // Poiché OnConfiguring userà la IConfiguration, qui puoi usare opzioni vuote 
            // o usare le opzioni in memoria, che sono ideali per i test.
            var options = new DbContextOptionsBuilder<AlphaShopDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            // 4. Istanziazione del DbContext, passando le opzioni e la IConfiguration mock.
            var dbContext = new AlphaShopDbContext(options, config);

            return dbContext;
        }



    }
}