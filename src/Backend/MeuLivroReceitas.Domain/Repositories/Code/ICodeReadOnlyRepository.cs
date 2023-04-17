using MeuLivroReceitas.Domain.Entities;

namespace MeuLivroReceitas.Domain.Repositories.Code;

public interface ICodeReadOnlyRepository
{
    //recebe string de conexao
    //entidade do codigo contem userId e code
    Task<Domain.Entities.Codes> GetEntityCode(string code); 
}
