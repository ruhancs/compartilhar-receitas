using AutoMapper;
using MeuLivroReceitas.Application.Services.AuthUser;
using MeuLivroReceitas.Comunication.Response;

namespace MeuLivroReceitas.Application.UseCases.User.GetPerfil;

public class GetPerfilUseCase : IGetPerfilUseCase
{

    //Automaper para transformar 
    //RequestRegisterUserJson em Usuario
    //registrar o mapeamento em application em AutomapperConfig
    // em EntityResponse
    private readonly IMapper _mapper;
    private readonly IAuthenticatedUser _authenticatedUser;

    public GetPerfilUseCase(
        IMapper mapper,
        IAuthenticatedUser authenticatedUser
        )
    {
        _mapper = mapper;
        _authenticatedUser = authenticatedUser;
    }

    public async Task<ResponsePerfilUserJson> Execute()
    {
        //GetUser implementado em Application services authenticatedUser
        //pega o usuario utilizando o token do header
        var user = await _authenticatedUser.GetUser();

        //transforma user em ResponsePerfilUserJson
        return _mapper.Map<ResponsePerfilUserJson>(user);
    }
}
