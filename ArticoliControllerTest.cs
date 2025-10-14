using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using ArticoliWebService.Controllers;
using ArticoliWebService.Dtos;
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
            var controller = new ArticoliController(new ArticoliRepository(dbContext));

            var response = await controller.GetArticoliByCode(codeArt) as ObjectResult;
            var value = response.Value as ArticoliDto;

            dbContext.Dispose();

            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal(codeArt, value.CodArt);
        }

        [Fact]
        public async Task TestErrrArticoliByCode()
        {
            string codeArt = "00000150A";

            var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new ArticoliController(new ArticoliRepository(dbContext));

            var response = await controller.GetArticoliByCode(codeArt) as ObjectResult;
            var value = response.Value as ArticoliDto;

            dbContext.Dispose();

            Assert.Equal(404, response.StatusCode);
            Assert.Null(value);
            Assert.Equal("Non è stato trovato l'articolo con il codice 00000150A", response.Value);
        }

        [Fact]
        public async Task TestSelArticoliByDescrizione()
        {
            string descrizione = "ACQUA ROCCHETTA";

            var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new ArticoliController(new ArticoliRepository(dbContext));

            var response = await controller.GetArticoliByDesc(descrizione) as ObjectResult;
            var value = response.Value as List<ArticoliDto>;

            dbContext.Dispose();

            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(value);
            Assert.Equal(3, value.Count);
            Assert.Equal("002001201", value.FirstOrDefault().CodArt);
        }

        [Fact]
        public async Task TestErrSelArticoliByDescrizione()
        {
            string descrizione = "Pippo";

            var dbContext = DbContextMocker.alphaShopDbContext();
            var controller = new ArticoliController(new ArticoliRepository(dbContext));

            var response = await controller.GetArticoliByDesc(descrizione) as ObjectResult;
            var value = response.Value as List<ArticoliDto>;

            dbContext.Dispose();

            Assert.Equal(404, response.StatusCode);
            Assert.Null(value);
            Console.WriteLine(response.Value);
            Assert.Equal("Non è stato trovato alcun articolo con la descrizione Pippo", response.Value);
        }
    }
}