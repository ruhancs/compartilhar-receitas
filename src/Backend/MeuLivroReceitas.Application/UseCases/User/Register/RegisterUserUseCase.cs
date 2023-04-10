using AutoMapper;
using MeuLivroReceitas.Application.Services.Cryptography;
using MeuLivroReceitas.Application.Services.Token;
using MeuLivroReceitas.Comunication.Request;
using MeuLivroReceitas.Comunication.Response;
using MeuLivroReceitas.Domain.Entities;
using MeuLivroReceitas.Domain.Repositories;
using MeuLivroReceitas.Domain.Repositories.User;
using MeuLivroReceitas.Exceptions;
using MeuLivroReceitas.Exceptions.ExceptionsBase;

namespace MeuLivroReceitas.Application.UseCases.User.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnly;
    private readonly IUserWriteOnlyRepository _repository;

    //Automaper para transformar 
    //RequestRegisterUserJson em Usuario
    //registrar o mapeamento em application em AutomapperConfig
    private readonly IMapper _mapper;

    //IUnitOfWork contem o commit para salvar no db
    private readonly IUnitOfWork _unitOfWork;

    private readonly EncryptPassword _encryptPassword;

    private readonly TokenController _tokenController;

    public RegisterUserUseCase(
        IUserWriteOnlyRepository repository, 
        IMapper mapper, 
        IUnitOfWork unitOfWork,
        EncryptPassword encryptPassword,
        TokenController tokenController,
        IUserReadOnlyRepository userReadOnly
        )
    {
        _userReadOnly = userReadOnly;
        _mapper = mapper;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _encryptPassword = encryptPassword;
        _tokenController = tokenController;

    }

    public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson req)
    {
        //validar request
        await Validate(req);


        //converte req para tipo Usuario
        var entity = _mapper.Map<Usuario>(req);
        //encriptografar senha
        entity.Password = _encryptPassword.Cryptography(req.Password);

        //salvar no db
        await _repository.Add(entity);
        await _unitOfWork.Commit();

        var token = _tokenController.generateToken(entity.Email);

        return new ResponseRegisterUserJson
        {
            Token = token,
        };
    }

    private async Task Validate(RequestRegisterUserJson req)
    {
        var validator = new RegisterUserValidator();
        var result = validator.Validate(req);

        //verificar se ja existe o email cadastrado
        var existEmail = await _userReadOnly.ExistUserEmail(req.Email);

        if (existEmail)
        {
            result.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ResourceMessageError.EMAIL_ALREADY_EXIST));
        }

        if (!result.IsValid) 
        {
            var messageError = result.Errors.Select(err => err.ErrorMessage).ToList();
            throw new ValidationErrors(messageError);
        }
    }
}
