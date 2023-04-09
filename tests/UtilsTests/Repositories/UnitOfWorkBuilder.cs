using MeuLivroReceitas.Domain.Repositories;
using Moq;

namespace UtilsTests.Repositories;

//mock para simular UnitOfWorkBuilder que contem a funcao Commit para salvar no db e Dispose para liberar memoria
//UnitOfWorkBuilder precisa ser passado como parametro em RegisterUseCaseTest
public class UnitOfWorkBuilder
{
    private static UnitOfWorkBuilder _instance;
    private readonly Mock<IUnitOfWork> _repository;

    private UnitOfWorkBuilder()
    {
        if (_repository == null)
        {
            _repository = new Mock<IUnitOfWork>();
        }
    }

    public static UnitOfWorkBuilder Instance()
    {
        _instance = new UnitOfWorkBuilder();
        return _instance;
    }

    public IUnitOfWork Construct()
    {
        return _repository.Object;
    }
}
