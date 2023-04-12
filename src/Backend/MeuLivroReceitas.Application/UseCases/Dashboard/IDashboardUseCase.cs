using MeuLivroReceitas.Comunication.Request;
using MeuLivroReceitas.Comunication.Response;

namespace MeuLivroReceitas.Application.UseCases.Dashboard;

public interface IDashboardUseCase
{
    Task<ResponseDashboardJson> Execute(RequestDashboardJson req);
}
