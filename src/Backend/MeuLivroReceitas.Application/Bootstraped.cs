using MeuLivroReceitas.Application.Services.Cryptography;
using MeuLivroReceitas.Application.Services.Token;
using MeuLivroReceitas.Application.UseCases.Login.DoLogin;
using MeuLivroReceitas.Application.UseCases.User.Register;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeuLivroReceitas.Application;

public static class Bootstraped
{
    public static void AddApplication(this IServiceCollection service, IConfiguration config)
    {
        AddPasswordKey(service, config);

        AddTokenParams(service, config);

        addUseCase(service);

    }

    private static void AddPasswordKey(IServiceCollection service, IConfiguration config)
    {
        //pega dentro de app.settings.Development.json o PasswordKey
        var section = config.GetRequiredSection("Config:PasswordKey");

        service.AddScoped(option => new EncryptPassword(section.Value));
    }
    private static void AddTokenParams(IServiceCollection service, IConfiguration config)
    {
        //pega dentro de app.settings.Development.json o PasswordKey
        var sectionTokenKey = config.GetRequiredSection("Config:TokenKey");
        var sectionExpireToken = config.GetRequiredSection("Config:ExpireToken");

        service.AddScoped(option => new TokenController(int.Parse(sectionExpireToken.Value),sectionTokenKey.Value));
    }

    private static void addUseCase(IServiceCollection service)
    {
        service.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>()
            .AddScoped<ILoginUseCase, LoginUseCase>();
    }
}
