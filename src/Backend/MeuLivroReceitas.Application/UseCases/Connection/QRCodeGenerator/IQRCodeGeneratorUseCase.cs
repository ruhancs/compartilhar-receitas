namespace MeuLivroReceitas.Application.UseCases.Connection.QRCodeGenerator;

public interface IQRCodeGeneratorUseCase
{
    Task<string> Execute();
}
