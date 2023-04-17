using System.Drawing;

namespace MeuLivroReceitas.Application.UseCases.Connection.GenerateQRCode;

public interface IQRCodeGeneratorUseCase
{
    Task<(Bitmap qrCode, string userId)> Execute();
}
