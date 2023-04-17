using System.Drawing;
using HashidsNet;
using MeuLivroReceitas.Application.Services.AuthenticatedUser;
using MeuLivroReceitas.Domain.Entities;
using MeuLivroReceitas.Domain.Repositories;
using MeuLivroReceitas.Domain.Repositories.Code;
using QRCoder;

namespace MeuLivroReceitas.Application.UseCases.Connection.GenerateQRCode;

//para utilizala adicionar em Bootstraped em addUseCase
//para injecao de dependencia
public class QRCodeGeneratorUseCase : IQRCodeGeneratorUseCase
{
    private readonly ICodeWriteOnlyRepository _repository;
    private readonly IAuthenticatedUser _user;
    private readonly IUnitOfWork _unitOfWork;
    private IHashids _hashId;

    public QRCodeGeneratorUseCase(
        ICodeWriteOnlyRepository repository,
        IAuthenticatedUser user,
        IUnitOfWork unitOfWork,
        IHashids hashId
        )
    {
        _repository = repository;
        _user = user;
        _unitOfWork = unitOfWork;
        _hashId = hashId;
    }

    //utilizado em websockets AddConnection
    //para gerar qrcode e enviar
    public async Task<(Bitmap qrCode, string userId)> Execute()
    {
        //GetUser implementado em Application services authenticatedUser
        //pega o usuario utilizando o token do header
        var user = await _user.GetUser();

        //gerar o codigo
        var code = new Codes
        {
            Code = Guid.NewGuid().ToString(),
            UserId = user.Id
        };

        await _repository.Register(code);

        await _unitOfWork.Commit();

        // para gerar o qrcode utilizar o plugin
        //que recebe string e devolve o qrcode
        //retorna o codigo e userid
        return (generateQRCodeImage(code.Code), _hashId.EncodeLong(user.Id));
    }

    private static Bitmap generateQRCodeImage(string code)
    {
        var qrCodeGenerator = new QRCodeGenerator();

        var qrCodeData = qrCodeGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);

        var qrCode = new QRCode(qrCodeData);

        return qrCode.GetGraphic(5, Color.Black, Color.Transparent, true);

        //salvar como array de bytes
       // using var stream = new MemoryStream();
        //bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        //return stream.ToArray();
    }
}
