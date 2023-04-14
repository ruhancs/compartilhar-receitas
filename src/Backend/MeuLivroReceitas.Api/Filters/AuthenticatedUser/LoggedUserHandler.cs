using MeuLivroReceitas.Application.Services.Token;
using MeuLivroReceitas.Domain.Entities;
using MeuLivroReceitas.Domain.Repositories.User;
using MeuLivroReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MeuLivroReceitas.Api.Filters.AuthenticatedUser;

//LoggedUserRequirements criado na mesma pasta
public class LoggedUserHandler : AuthorizationHandler<LoggedUserRequirements>
{
    //utilizado para acessar a sessao e recuperar o usuario
    private readonly IHttpContextAccessor _httpcontextAccessor;
    private readonly TokenController _tokenController;
    private readonly IUserReadOnlyRepository _repository;

    public LoggedUserHandler(
        IHttpContextAccessor httpContextAccessor,
        TokenController tokenController,
        IUserReadOnlyRepository repository
        )
    {
        _httpcontextAccessor = httpContextAccessor;
        _tokenController = tokenController;
        _repository = repository;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, LoggedUserRequirements requirement)
    {
        try
        {
            //_httpcontextAccessor representa a request recebida
            // no header em Authorization estara o token
            var authorization = _httpcontextAccessor
                .HttpContext
                .Request.Headers["Authorization"]
                .ToString();
            
            //se o token da requisicao estiver vazio
            if (string.IsNullOrWhiteSpace(authorization))
            {
                context.Fail();
                return;
            }

            //pegar somente o token e excluir o bearer
            //pula o tamanho da palavra bearer e pega o resto ate o final
            //trim para remover os espaços
            var token = authorization["Bearer".Length..].Trim();

            var userEmail = _tokenController.GetEmail(token);

            //user na consulta foi utilizado AsNoTracking assim nao podendo modificalo
            //somente o get funciona
            var user = await _repository.GetEmail(userEmail);

            if (user is null)
            {
                context.Fail();
                return;
            }

            context.Succeed(requirement);
        }
        catch
        {
            context.Fail();
        }
        
    }

}
