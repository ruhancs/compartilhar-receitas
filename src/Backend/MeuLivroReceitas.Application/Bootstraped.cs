using MeuLivroReceitas.Application.Services.AuthenticatedUser;
using MeuLivroReceitas.Application.Services.AuthUser;
using MeuLivroReceitas.Application.Services.Cryptography;
using MeuLivroReceitas.Application.Services.Token;
using MeuLivroReceitas.Application.UseCases.Connection.QRCodeGenerator;
using MeuLivroReceitas.Application.UseCases.Dashboard;
using MeuLivroReceitas.Application.UseCases.Login.DoLogin;
using MeuLivroReceitas.Application.UseCases.Recipe;
using MeuLivroReceitas.Application.UseCases.Recipe.Delete;
using MeuLivroReceitas.Application.UseCases.Recipe.GetById;
using MeuLivroReceitas.Application.UseCases.Recipe.Update;
using MeuLivroReceitas.Application.UseCases.User.GetPerfil;
using MeuLivroReceitas.Application.UseCases.User.Register;
using MeuLivroReceitas.Application.UseCases.User.Update;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeuLivroReceitas.Application;

public static class Bootstraped
{
    public static void AddApplication(this IServiceCollection service, IConfiguration config)
    {
        AddPasswordKey(service, config);

        AddHashId(service, config);

        AddTokenParams(service, config);

        addUseCase(service);

        addUserAuthenticated(service);

    }

    private static void AddPasswordKey(IServiceCollection service, IConfiguration config)
    {
        //pega dentro de app.settings.Development.json o PasswordKey
        var section = config.GetRequiredSection("Config:password:PasswordKey");

        service.AddScoped(option => new EncryptPassword(section.Value));
    }
    private static void AddTokenParams(IServiceCollection service, IConfiguration config)
    {
        //pega dentro de app.settings.Development.json o PasswordKey
        var sectionTokenKey = config.GetRequiredSection("Config:Jwt:TokenKey");
        var sectionExpireToken = config.GetRequiredSection("Config:Jwt:ExpireTokenInHours");

        service.AddScoped(option => new TokenController(int.Parse(sectionExpireToken.Value),sectionTokenKey.Value));
    }

    private static void addUseCase(IServiceCollection service)
    {
        service.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>()
            .AddScoped<ILoginUseCase, LoginUseCase>()
            .AddScoped<IUpdatePasswordUseCase, UpdatePasswordUseCase>()
            .AddScoped<IRegisterRecipeUseCase, RegisterRecipeUseCase>()
            .AddScoped<IDashboardUseCase, DashboardUseCase>()
            .AddScoped<IGetRecipeByIdUseCase, GetRecipeByIdUseCase>()
            .AddScoped<IUpdateRecipeUseCase, UpdateRecipeUseCase>()
            .AddScoped<IDeleteRecipeUseCase, DeleteRecipeUseCase>()
            .AddScoped<IGetPerfilUseCase, GetPerfilUseCase>()
            .AddScoped<IQRCodeGeneratorUseCase, QRCodeGeneratorUseCase>();
    }

    private static void addUserAuthenticated(IServiceCollection service)
    {
        service.AddScoped<IAuthenticatedUser, AuthenticatedUser>();
    }
    
    private static void AddHashId(IServiceCollection service, IConfiguration config)
    {
        var salt = config.GetRequiredSection("Config:HashId:Salt");

        service.AddHashids(setup =>
        {
            setup.Salt = salt.Value;
            setup.MinHashLength = 3;
        });
    }
}
