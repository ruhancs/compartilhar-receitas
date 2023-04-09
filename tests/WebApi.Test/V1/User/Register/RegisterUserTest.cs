using System.Net;
using System.Text.Json;
using FluentAssertions;
using UtilsTests.Requests;
using Xunit;

namespace WebApi.Test.V1.User.Register;

public class RegisterUserTest : ControllerBase
{
    //requisicao em controller user
    private const string METHOD = "User";

    //base(factory) passa factory para o ControllerBase
    public RegisterUserTest(MeuLivroReceitasWebApplicationFactory<Program> factory) : base(factory)
    {    
    }

    [Fact]
    public async Task ValidateSuccess()
    {
        var request = RequestRegisterUserBuilder.Construct();

        var response = await PostRequest(METHOD, request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        //transforma a resposta em json
        var responseData = await JsonDocument.ParseAsync(responseBody);

        //verificar se na resposta contem o token
        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }
}
