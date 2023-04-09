using System.Globalization;
using System.Text;
using MeuLivroReceitas.Exceptions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace WebApi.Test.V1;

//MeuLivroReceitasWebApplicationFactory criada recebe o parametro o Program
//criado uma definicao de classe para program
public class ControllerBase : IClassFixture<MeuLivroReceitasWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ControllerBase(MeuLivroReceitasWebApplicationFactory<Program> factory)
    {
        //inicializar o httpclient
        _client = factory.CreateClient();
        //para ResourceMessageError ser na mesma linguagem que a aplicacao 
        ResourceMessageError.Culture = CultureInfo.CurrentCulture;
    }

    protected async Task<HttpResponseMessage> PostRequest(string method, object body)
    {
        //corpo do body tratado para utilizar o postAsync
        var jsonString = JsonConvert.SerializeObject(body);

        return await _client.PostAsync(method, new StringContent(jsonString, Encoding.UTF8, "application/json"));
    }
}
