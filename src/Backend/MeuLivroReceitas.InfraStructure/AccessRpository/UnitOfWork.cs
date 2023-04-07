using MeuLivroReceitas.Domain.Repositories;

namespace MeuLivroReceitas.InfraStructure.AccessRpository;

//sealed indica que nenhuma classe pode herdar
public sealed class UnitOfWork : IDisposable, IUnitOfWork
{
    private readonly Context _context;

    //para verificar se ja foi liberado memoria
    private bool _diposed;

    public UnitOfWork(Context context)
    {
        _context = context;
    }

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }
    public void Dispose()
    {
        Dispose(true);
    }

    //para liberar memoria nao usada
    public void Dispose(bool disposing)
    {
        if (!_diposed && disposing)
        {
            //liberaçao de memoria
            _context.Dispose();
        }

        _diposed = true;
    }
}
