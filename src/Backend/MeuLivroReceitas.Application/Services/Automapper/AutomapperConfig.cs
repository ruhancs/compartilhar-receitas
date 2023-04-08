using AutoMapper;
using MeuLivroReceitas.Comunication.Request;
using MeuLivroReceitas.Domain.Entities;

namespace MeuLivroReceitas.Application.Services.Automapper;

public class AutomapperConfig : Profile
{
    public AutomapperConfig() 
    {
        //mapear RequestRegisterUserJson para Usuario
        //campos devem ser valores iguais
        CreateMap<RequestRegisterUserJson, Usuario>()
            .ForMember(destiny => destiny.Password, config => config.Ignore());//ignora pasword no destino Usuario para encriptografar
    }
}
