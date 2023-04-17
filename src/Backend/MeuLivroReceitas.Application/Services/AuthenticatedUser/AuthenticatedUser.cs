using MeuLivroReceitas.Application.Services.Token;
using MeuLivroReceitas.Domain.Entities;
using MeuLivroReceitas.Domain.Repositories.User;
using Microsoft.AspNetCore.Http;

namespace MeuLivroReceitas.Application.Services.AuthenticatedUser;

//injetar dependencia em bootstraped de classe e interface
public class AuthenticatedUser : IAuthenticatedUser
{
    private readonly IHttpContextAccessor _httpcontextAccessor;
    private readonly TokenController _tokenController;
    private readonly IUserReadOnlyRepository _repository;

    //para pegar o token recebera com injeçao de dependencia 
    //da classe do .net IHttpContextAccessor
    //em program adicionar builder.Services.AddHttpContextAccessor()
    public AuthenticatedUser(
        IHttpContextAccessor httpContextAccessor, 
        TokenController tokenController,
        IUserReadOnlyRepository repository
        )
    {
        _httpcontextAccessor = httpContextAccessor;
        _tokenController = tokenController;
        _repository = repository;
    }
    public async Task<Usuario> GetUser()
    {
        //_httpContextAccessor representa a request recebida
        // no header em Authorization estara o token
        var authorization = _httpcontextAccessor
            .HttpContext
            .Request.Headers["Authorization"]
            .ToString();

        //pegar somente o token e excluir o bearer
        //pula o tamanho da palavra bearer e pega o resto ate o final
        //trim para remover os espaços
        var token = authorization["Bearer".Length..].Trim();

        var userEmail = _tokenController.GetEmail(token);

        //user na consulta foi utilizado AsNoTracking assim nao podendo modificalo
        //somente o get funciona
        var user = await _repository.GetEmail(userEmail);

        return user;
    }
}
