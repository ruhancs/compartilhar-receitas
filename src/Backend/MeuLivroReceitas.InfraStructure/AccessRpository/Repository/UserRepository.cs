using MeuLivroReceitas.Domain.Entities;
using MeuLivroReceitas.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroReceitas.InfraStructure.AccessRpository;

//recebe as interface para implementar os metodos de leitura e escrita
internal class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
{
    private readonly Context _context;
    public UserRepository(Context context)
    {
        _context = context;
    }

    public async Task Add(Usuario user)
    {
        await _context.Usuarios.AddAsync(user);
    }

    public async Task<bool> ExistUserEmail(string email)
    {
        return await _context.Usuarios.AnyAsync(c => c.Email.Equals(email));
    }

    public async Task<Usuario> Login(string email, string password)
    {
        //se encontrar devolve o user se nao encontrar null
        return await _context.Usuarios.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.Equals(email) && u.Password.Equals(email));
    }
}
