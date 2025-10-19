using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArticoliWebService.Profiles;
using AutoMapper;

namespace ArticoliWebService.Test
{
    public class MapperMocker
    {
        public static IMapper GetMapper()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ArticoliProfile());
            });

            var mapper = mockMapper.CreateMapper();
            return mapper;
        }

    }
}