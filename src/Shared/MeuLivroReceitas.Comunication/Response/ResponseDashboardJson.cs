namespace MeuLivroReceitas.Comunication.Response;

// cria lista de ResponseRecipeDashboardJson
//para utilizar como resposta no DashboardUseCase
public class ResponseDashboardJson
{
    public List<ResponseRecipeDashboardJson> Recipes { get; set; }
}
