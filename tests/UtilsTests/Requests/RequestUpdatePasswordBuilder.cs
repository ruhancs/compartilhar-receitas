using Bogus;
using MeuLivroReceitas.Application.UseCases.User.Register;
using MeuLivroReceitas.Application.UseCases.User.Update;
using MeuLivroReceitas.Comunication.Request;

namespace UtilsTests.Requests;

public class RequestUpdatePasswordBuilder
{
    public static RequestUpdatePasswordJson Construct(int passwordLenght=10 )
    {
        return new Faker<RequestUpdatePasswordJson>()
            .RuleFor(c => c.Password, f => f.Internet.Password(10))
            .RuleFor(c => c.NewPassword, f => f.Internet.Password(passwordLenght));

    }


}
