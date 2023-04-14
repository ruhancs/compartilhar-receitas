using MeuLivroReceitas.Domain.Entities;
using MeuLivroReceitas.Domain.Repositories.Code;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroReceitas.InfraStructure.AccessRpository.Repository;

//recebe as interface para implementar os metodos de leitura,escrita e update
//adicionar dependencia em bootstraped em AddRepositories
public class CodeRepository : ICodeReadOnlyRepository, ICodeWriteOnlyRepository
{
    //adicionar em Infrastructure em Context
    //public DbSet<Recipe> Receitas { get; set; }
    // para utilizar a Recipe no contexto
    private readonly Context _context;
    public CodeRepository(Context context)
    {
        _context = context;
    }

    public async Task Register(Codes code)
    {
        //evitar que salve muitos codigos para o usuario
        //em caso de perda de conexao
        //apenas um codigo fica salvo
        var codesDb = await _context.Codes.FirstOrDefaultAsync(c => c.UserId == code.UserId);
        if(codesDb is not null)
        {
            codesDb.Code = code.Code;   
            _context.Codes.Update(codesDb);
        }
        else
        {
            await _context.Codes.AddAsync(code);
        }
    }
}
