using MeuLivroReceitas.Domain.Entities;
using MeuLivroReceitas.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroReceitas.InfraStructure.AccessRpository;

//recebe as interface para implementar os metodos de leitura,escrita e update
//adicionar dependencia em bootstraped em AddRepositories
internal class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository, IUpdateOnlyRepository
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

    //pegar usuario pelo id para atualizalo
    //em application updatePasswordUsecase
    //interface criada em IUpdateOnlyRepository
    public async Task<Usuario> GetById(long id)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Usuario> GetEmail(string email)
    {
        return await _context.Usuarios.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.Equals(email));
    }

    public async Task<Usuario> Login(string email, string password)
    {
        //se encontrar devolve o user se nao encontrar null
        return await _context.Usuarios.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.Equals(email) && u.Password.Equals(password));
    }

    public void Update(Usuario user)
    {
        _context.Usuarios.Update(user);
    }


}
