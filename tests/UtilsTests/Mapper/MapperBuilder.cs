using AutoMapper;
using MeuLivroReceitas.Application.Services.Automapper;

namespace UtilsTests.Mapper;

//mock para simular Mapper que transforma IRequestUserJson em User
//Mapper precisa ser passado como parametro em RegisterUseCaseTest
public class MapperBuilder
{
    public static IMapper Instance()
    {
        var config = new MapperConfiguration(cfg =>
        {
            //AutomapperConfig criado em Application
            cfg.AddProfile<AutomapperConfig>();
        });

        return config.CreateMapper();
    }
}
