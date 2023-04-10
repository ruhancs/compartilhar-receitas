using Bogus;
using MeuLivroReceitas.Comunication.Request;
using MeuLivroReceitas.Domain.Entities;
using UtilsTests.Encrypt;

namespace UtilsTests.Entities;

public class UserBuilder
{
    //criar um usuario com dados aleatorios
    public static (Usuario user, string password) Construct(int passwordLength = 10)
    {
        string password = string.Empty;

        //Faker vem de Bogus
        var generateUser =  new Faker<Usuario>()
            .RuleFor(c => c.Id, _ => 1)//gerar um nome
            .RuleFor(c => c.Name, f => f.Person.FullName)//gerar um nome
            .RuleFor(c => c.Email, f => f.Internet.Email())//gerar um email
            .RuleFor(c => c.Password, f =>
            {
                password = f.Internet.Password();

                return EncryptPasswordBuilder.Intance().Cryptography(password);
            })//gerar um senha cryptografada
            .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber("## ! ####-####").Replace("!", $"{f.Random.Int(min: 1, max: 9)}"));//gerar uma password no formato
        
        //retorna usuario e senha
        return (generateUser, password);
    }
}
