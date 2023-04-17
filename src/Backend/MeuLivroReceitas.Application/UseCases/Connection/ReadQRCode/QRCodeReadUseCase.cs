using AutoMapper;
using HashidsNet;
using MeuLivroReceitas.Application.Services.AuthenticatedUser;
using MeuLivroReceitas.Comunication.Response;
using MeuLivroReceitas.Domain.Repositories.Code;
using MeuLivroReceitas.Domain.Repositories.Connection;
using MeuLivroReceitas.Exceptions.ExceptionsBase;

namespace MeuLivroReceitas.Application.UseCases.Connection.ReadQRCode
{
    //para utilizala adicionar em Bootstraped em addUseCase
    //para injecao de dependencia

    //usado em AddConnection em QRCodeRead
    public class QRCodeReadUseCase : IQRCodeReadUseCase
    {
        //criado em automapperConfig para tranformar id
        //de long para hash
        private readonly IHashids _hashId;

        //verificar se os usuarios estao conectados
        private readonly IConnectionReadOnlyRepository _connectionRepository;
        private readonly ICodeReadOnlyRepository _repository;
        private readonly IAuthenticatedUser _user;

        public QRCodeReadUseCase(
            ICodeReadOnlyRepository repository,
            IAuthenticatedUser user,
            IConnectionReadOnlyRepository connectionRepository,
            IHashids hashId
            )
        {
            _repository = repository;
            _user = user;
            _connectionRepository = connectionRepository;
            hashId = _hashId;
        }
        
        //em automapperConfig transformar id para strig
        public async Task<(ResponseConnectedUserJson userToConnect, string ownerQRCodeId)> Execute(string connectionCode)
        {
            //GetUser implementado em Application services authenticatedUser
            //pega o usuario utilizando o token do header
            var user = await _user.GetUser();

            //pegar a entidade Codes com o codigo de conexao
            //code contem o usuario que gerou e o codigo
            var code = await _repository.GetEntityCode( connectionCode );

            await Validate(code, user);

            var userToConnect = new ResponseConnectedUserJson
            {
                Id = _hashId.EncodeLong(user.Id),
                Name = user.Name
            };

            return (userToConnect, _hashId.EncodeLong(code.UserId));
        }

        //validar o usuario que esta lendo o qrcode eo qrcode
        private async Task Validate(
            Domain.Entities.Codes code,
            Domain.Entities.Usuario user
            )
        {
            //se o codigo for nulo
            if(code is null)
            {
                throw new ExceptionBase("");
            }

            //se o usuario que gerou o qrcode
            //tentar ler o qrcode dele
            if(code.UserId == user.Id)
            {
                throw new ExceptionBase("");
            }

            //existe conexao entre o usuario que gerou o codigo eo que esta lendo
            var existConnection = await _connectionRepository.ConnectionExist(code.UserId, user.Id);
        
            if( existConnection )
            {
                throw new ExceptionBase("");
            }
        }
    }
}
