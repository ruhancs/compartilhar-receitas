using MeuLivroReceitas.Comunication.Enum;

namespace MeuLivroReceitas.Comunication.Request;

public class RequestDashboardJson
{
    public string TypeOrIngredient { get; set; }
    //podera receber o enum de categorias 
    //se nao receber o parametro pega todas receitas
    public CategoryComunication? Category { get; set; }
}
