using MeuLivroReceitas.Domain.Entities;
using MeuLivroReceitas.Domain.Repositories.Connection;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroReceitas.InfraStructure.AccessRpository.Repository;

public class ConnectionRepository: IConnectionReadOnlyRepository,IConnectionWriteOnlyRepository
{
    //adicionar em Infrastructure em Context
    //public DbSet<Recipe> Receitas { get; set; }
    // para utilizar a Recipe no contexto
    private readonly Context _context;
    public ConnectionRepository(Context context)
    {
        _context = context;
    }

    public async Task<bool> ConnectionExist(long userIdA, long userIdB)
    {
        return await _context.Connection.AnyAsync(
            c => c.UserId == userIdA && c.UserConnectedId == userIdB
            );
    }

    public async Task<List<Usuario>> GetUserConnetions(long userId)
    {
        return await _context.Connection.AsNoTracking()
            //para pegar a Entidade Usuario com o join
            .Include(c => c.UserConnected)
            .Where(c => c.UserId == userId)
            //selecionar apenas os usuarios
            .Select(c => c.UserConnected)
            .ToListAsync();
    }

    public async Task Register(Conexao connection)
    {
        await _context.Connection.AddAsync(connection);
    }

    public async Task Remove(long userId, long userIdToDisconnect)
    {
        var connections = await _context.Connection
            .Where(c => (c.UserId == userId && c.UserConnectedId == userIdToDisconnect)
            || 
            (c.UserId == userIdToDisconnect && c.UserConnectedId == userId))
            .ToListAsync();

        _context.Connection.RemoveRange(connections);
    }
}
