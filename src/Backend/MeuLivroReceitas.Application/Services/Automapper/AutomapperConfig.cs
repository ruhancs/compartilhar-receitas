using AutoMapper;
using HashidsNet;
using MeuLivroReceitas.Comunication.Request;
using MeuLivroReceitas.Comunication.Response;
using MeuLivroReceitas.Domain.Entities;

namespace MeuLivroReceitas.Application.Services.Automapper;

//automapper habilitado em program
public class AutomapperConfig : Profile
{
    //IHashids para tranformar o id de long para hash
    //melhor seria criar a tabela com o tipo de id guid
    private readonly IHashids _hashId;
    public AutomapperConfig(IHashids hashId) 
    {
        _hashId = hashId;
        RequestEntity();
        EntityResponse();
    }

    private void RequestEntity()
    {
        //transforma RequestRegisterUserJson em Usuario
        //mapear RequestRegisterUserJson para Usuario
        //campos devem ser valores iguais
        CreateMap<RequestRegisterUserJson, Usuario>()
            .ForMember(destiny => destiny.Password, config => config.Ignore());//ignora pasword no destino Usuario para encriptografar

        //transforma RequestRegisterRecipeJson em Recipe
        CreateMap<RequestRegisterRecipeJson, Recipe>();

        //transforma RequesteRegisterIngredientJson em Ingredient
        CreateMap<RequesteRegisterIngredientJson, Ingredient>();
    }

    private void EntityResponse()
    {
        //transformar Receita para ResponseRecipeJson
        CreateMap<Recipe, ResponseRecipeJson>()
        //transformar o id da resposta de long para hash
            .ForMember(destiny => destiny.id, config => config.MapFrom(origem => _hashId.EncodeLong(origem.Id)));


        CreateMap<Ingredient, ResponseIngredientJson>()
            //transformar o id da resposta de long para hash
            .ForMember(destiny => destiny.id, config => config.MapFrom(origem => _hashId.EncodeLong(origem.Id)));
    }
}
