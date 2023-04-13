using MeuLivroReceitas.Comunication.Enum;

namespace MeuLivroReceitas.Comunication.Request;

public class RequestRegisterRecipeJson
{
    public RequestRegisterRecipeJson()
    {
        Ingredients = new();
    }

    public string Title { get; set; }
    //CategoryComunication é a mesma coisa de enum Category de Domain
    //criado para nao ter comunicacao entre Domain e Communication
    public CategoryComunication Category { get; set; }
    public string MethodPreparation { get; set; }
    public int PreparationTime { get; set; }
    public List<RequesteRegisterIngredientJson> Ingredients { get; set; }
}
