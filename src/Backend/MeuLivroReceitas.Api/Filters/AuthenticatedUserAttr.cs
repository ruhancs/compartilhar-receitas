using MeuLivroReceitas.Application.Services.Token;
using MeuLivroReceitas.Comunication.Response;
using MeuLivroReceitas.Domain.Repositories.User;
using MeuLivroReceitas.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace MeuLivroReceitas.Api.Filters;

//verificar se o usuario esta logado
//registrar em program que quer utilizar como filtro
public class AuthenticatedUserAttr : AuthorizeAttribute ,IAsyncAuthorizationFilter
{
    private readonly TokenController _tokenController;
    private readonly IUserReadOnlyRepository _repository;

    public AuthenticatedUserAttr(
        TokenController tokenController,
        IUserReadOnlyRepository repository
        )
    {
        _tokenController = tokenController;
        _repository = repository;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            //pega o token da request
            var token = TokenInRequest(context);
            //recebe o token e retira o email
            var email = _tokenController.GetEmail(token);

            //verificar no db se o email existe
            var user = await _repository.GetEmail(email);
        
            if(user is null)
            {
                throw new System.Exception();
            }

        }
        catch (SecurityTokenExpiredException)
        {
            TokenExpired(context);
        }
        catch
        {
            UserUnAuthorized(context);
        }

    }

    private string TokenInRequest(AuthorizationFilterContext context)
    {
        //context representa a request recebida
        // no header em Authorization estara o token
        var authorization = context
            .HttpContext
            .Request.Headers["Authorization"]
            .ToString();

        if (string.IsNullOrWhiteSpace(authorization))
        {
            //em caso de exception e tratada no catch
            throw new System.Exception();
        }

        //pegar somente o token e excluir o bearer
        //pula o tamanho da palavra bearer e pega o resto ate o final
        //trim para remover os espaços
        return authorization["Bearer".Length..].Trim();
    }

    private void TokenExpired(AuthorizationFilterContext context)
    {
        context.Result = new UnauthorizedObjectResult(new ResponseError(ResourceMessageError.EXPIRED_TOKEN));
    }   
    private void UserUnAuthorized(AuthorizationFilterContext context)
    {
        context.Result = new UnauthorizedObjectResult(new ResponseError(ResourceMessageError.UNAUTHORIZED_USER));
    }
}
