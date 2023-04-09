using MeuLivroReceitas.Application.Services.Cryptography;
using MeuLivroReceitas.Application.Services.Token;
using MeuLivroReceitas.Comunication.Request;
using MeuLivroReceitas.Comunication.Response;
using MeuLivroReceitas.Domain.Repositories;
using MeuLivroReceitas.Exceptions.ExceptionsBase;

namespace MeuLivroReceitas.Application.UseCases.Login.DoLogin;

//injecao de dependencia feita no bootstrap
public class LoginUseCase : ILoginUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnly;
    private readonly EncryptPassword _encryptPassword;
    private readonly TokenController _tokenController;
    public LoginUseCase(
        IUserReadOnlyRepository userReadOnly,
        EncryptPassword encryptPassword,
        TokenController tokenController
        )
    {
        _userReadOnly = userReadOnly;
        _encryptPassword = encryptPassword;
        _tokenController = tokenController;
    }
    public async Task<ResponseLoginJson> Execute(RequestLoginJson req)
    {
        var passwordEncrypted = _encryptPassword.Cryptography(req.Password);
        
        var user = await _userReadOnly.Login(req.Email, passwordEncrypted);

        if(user == null)
        {
            throw new LoginInvalidException();
        }

        return new ResponseLoginJson
        {
            Name = user.Name,
            Token = _tokenController.generateToken(user.Email)
        };
        
    }
}
