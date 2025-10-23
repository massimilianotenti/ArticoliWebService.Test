using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using ArticoliWebService.Controllers;
using ArticoliWebService.Dtos;
using ArticoliWebService.Models;
using ArticoliWebService.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace ArticoliWebService.Test
{
    public class ArticoliControllerTest
    {
        [Fact]
        public async Task TestArticoliByCode()
        {
            string codeArt = "000001501";

            var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new ArticoliController(new ArticoliRepository(dbContext), MapperMocker.GetMapper());

            var response = await controller.GetArticoliByCode(codeArt) as ObjectResult;
            var value = response.Value as ArticoliDto;

            dbContext.Dispose();

            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal(codeArt, value.CodArt);
        }

        [Fact]
        public async Task TestErrArticoliByCode()
        {
            string codeArt = "00000150A";

            var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new ArticoliController(new ArticoliRepository(dbContext), MapperMocker.GetMapper());

            var response = await controller.GetArticoliByCode(codeArt) as ObjectResult;
            var value = response.Value as ErrMsg;

            dbContext.Dispose();

            Assert.Equal(404, response.StatusCode);
            Console.WriteLine(response.Value);
            Console.WriteLine(value.message);
            Assert.Equal("Non è stato trovato l'articolo con il codice 00000150A", value.message);
        }

        // È un attributo del framework di test xUnit. Indica che questo metodo è un test case che 
        // deve essere eseguito automaticamente dal test runner.
        [Fact]
        public async Task TestSelArticoliByDescrizione()
        {
            string descrizione = "ACQUA ROCCHETTA";

            // using var: È una pratica eccellente. Garantisce che, al termine del metodo di test, 
            // la connessione al database (dbContext) venga chiusa e le risorse rilasciate correttamente, 
            // anche in caso di errore.
            using var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new ArticoliController(new ArticoliRepository(dbContext), MapperMocker.GetMapper());

            var response = await controller.GetArticoliByDesc(descrizione, null);
            // 1) Controlla che la risposta sia un HTTP 200 OK con un corpo (OkObjectResult). 
            //    Se non lo è (es. un 404 Not Found), il test fallisce. 
            // 2) Converte l'oggetto in OkObjectResult e lo assegna a okResult
            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            // okResult.Value contiene il corpo della risposta (il payload JSON).
            // Assert.IsAssignableFrom<...> verifica che il corpo della risposta sia una collezione di oggetti ArticoliDto
            var articoli = Assert.IsAssignableFrom<IEnumerable<ArticoliDto>>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(3, articoli.Count());
            // Verifichiamo una proprietà specifica del primo articolo restituito per assicurarci che i dati siano 
            // corretti. L'uso dell'operatore ?. (null-conditional) è una best practice per evitare una 
            // NullReferenceException se la lista fosse inaspettatamente vuota.
            Assert.Equal("002001201", articoli.FirstOrDefault()?.CodArt);
        }

        [Fact]
        public async Task TestSelArticoliByDescrizioneCat()
        {
            string descrizione = "ACQUA ROCCHETTA";
            string cat = "1";

            using var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new ArticoliController(new ArticoliRepository(dbContext), MapperMocker.GetMapper());

            var response = await controller.GetArticoliByDesc(descrizione, cat);
            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            var articoli = Assert.IsAssignableFrom<IEnumerable<ArticoliDto>>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(3, articoli.Count());
            Assert.Equal("002001201", articoli.FirstOrDefault()?.CodArt);
        }

        [Fact]
        public async Task TestErrSelArticoliByDescrizione()
        {
            string descrizione = "pippo";            

            using var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new ArticoliController(new ArticoliRepository(dbContext), MapperMocker.GetMapper());

            var response = await controller.GetArticoliByDesc(descrizione, null);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(response.Result);   
            var errMsg = Assert.IsType<ErrMsg>(notFoundResult.Value);                     

            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal($"Non è stato trovato alcun articolo con la descrizione {descrizione}", errMsg.message, ignoreCase: true);
        }


        [Fact]
        public async Task TestErrSelArticoliByDescrizioneCat()
        {
            string descrizione = "ACQUA ROCCHETTA";
            string cat = "2";

            using var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new ArticoliController(new ArticoliRepository(dbContext), MapperMocker.GetMapper());

            var response = await controller.GetArticoliByDesc(descrizione, cat);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(response.Result);
            var errMsg = Assert.IsType<ErrMsg>(notFoundResult.Value);

            Assert.Equal(404, notFoundResult.StatusCode);
            Console.WriteLine(errMsg.message);
            Assert.Equal($"Non è stato trovato alcun articolo con la descrizione {descrizione} e categoria {cat}", errMsg.message, ignoreCase: true);
        }

        [Fact]
        public async Task TestSelIva()
        {
            using var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new IvaController(new ArticoliRepository(dbContext), MapperMocker.GetMapper());

            var response = await controller.SelIva();
            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            var iva = Assert.IsAssignableFrom<IEnumerable<IvaDto>>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.True(iva.Any());
        }

        [Fact]
        public async Task TestSelFarm()
        {
            using var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new FarmAssortController(new ArticoliRepository(dbContext), MapperMocker.GetMapper());

            var response = await controller.SelFamAssort();
            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            var farm = Assert.IsAssignableFrom<IEnumerable<FamAssortDto>>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.True(farm.Any());
        }
        
        

    }
}