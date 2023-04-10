using MeuLivroReceitas.Domain.Repositories.User;
using Moq;

namespace UtilsTests.Repositories;

//mock para simular UserReadOnlyRepository que contem a funcao existUserEmail que verifica se o email ja foi cadastrado
//UserReadOnlyRepositoryBuilder precisa ser passado como parametro em RegisterUseCaseTest
public class UserReadOnlyRepositoryBuilder
{
    private static UserReadOnlyRepositoryBuilder _instance;
    private readonly Mock<IUserReadOnlyRepository> _repository;

    private UserReadOnlyRepositoryBuilder()
    {
        if (_repository == null)
        {
            _repository = new Mock<IUserReadOnlyRepository>();
        }
    }

    public static UserReadOnlyRepositoryBuilder Instancia()
    {
        _instance = new UserReadOnlyRepositoryBuilder();
        return _instance;
    }

    public UserReadOnlyRepositoryBuilder ExistUserEmail(string email)
    {
        if (!string.IsNullOrEmpty(email))
        {
            //quando utilizar ExistUserEmail(email) retorna true
            _repository.Setup(i => i.ExistUserEmail(email)).ReturnsAsync(true);

            //cada execuçao criara uma nova instancia
            return this;
        } 
        else
        {
            _repository.Setup(i => i.ExistUserEmail(email)).ReturnsAsync(false);

            return this;
        }
    }

    public IUserReadOnlyRepository Construct()
    {
        return _repository.Object;
    }
}
