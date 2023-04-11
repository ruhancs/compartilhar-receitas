using MeuLivroReceitas.Comunication.Enum;
using MeuLivroReceitas.Comunication.Request;

namespace MeuLivroReceitas.Comunication.Response;

public class ResponseRecipeJson
{
    //SEM NECESSIDADE DE CRIAR melhor criar id como tipo guid
    //id para enviar ao frontend sera encryptografado o numero sequencial
    //para seguranca
    //pode se utilizar ao criar as tabela no Id o tipo guid ao invez de long
    //funcao AddHashId criada em bootstraped application

    public string id { get; set; }
    public string Title { get; set; }
    //CategoryComunication é a mesma coisa de enum Category de Domain
    //criado para nao ter comunicacao entre Domain e Communication
    public CategoryComunication Category { get; set; }
    public string MethodPreparation { get; set; }
    public List<ResponseIngredientJson> Ingredients { get; set; }
}
