using Bogus;
using MeuLivroReceitas.Comunication.Request;

namespace UtilsTests.Requests;

public class RequestRegisterUserBuilder
{
    public static RequestRegisterUserJson Construct(int passwordLength = 10)
    {
        //Faker vem de Bogus
        return new Faker<RequestRegisterUserJson>()
            .RuleFor(c => c.Name, f => f.Person.FullName)//gerar um nome
            .RuleFor(c => c.Email, f => f.Internet.Email())//gerar um email
            .RuleFor(c => c.Password, f => f.Internet.Password(passwordLength))//gerar um senha
            .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber("## ! ####-####").Replace("!", $"{f.Random.Int(min:1, max:9)}"));//gerar uma password no formato
            
    }
}
