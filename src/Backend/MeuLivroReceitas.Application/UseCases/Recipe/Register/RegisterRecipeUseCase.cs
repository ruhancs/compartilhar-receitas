using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MeuLivroReceitas.Application.Services.AuthUser;
using MeuLivroReceitas.Application.UseCases.Recipe.Register;
using MeuLivroReceitas.Comunication.Request;
using MeuLivroReceitas.Comunication.Response;
using MeuLivroReceitas.Domain.Repositories;
using MeuLivroReceitas.Domain.Repositories.Recipe;
using MeuLivroReceitas.Exceptions.ExceptionsBase;

namespace MeuLivroReceitas.Application.UseCases.Recipe
{
    //para utilizala adicionar em Bootstraped em addUseCase
    //para injecao de dependencia
    public class RegisterRecipeUseCase : IRegisterRecipeUseCase
    {
        //para converter a Request em uma entidade de receita para salvar no db
        //e converter a entidade na resposta
        //configurar o _mapper em services automapper
        private IMapper _mapper;
        //para salvar os dados no db com commit
        private IUnitOfWork _unitOfWork;
        private IAuthenticatedUser _authenticatedUser;
        //para salvar a receita com o metodo register
        //interface criada em domai e repositorio em infrastructure
        private IRecipeWriteOnlyRepository _repository;

        public RegisterRecipeUseCase(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IAuthenticatedUser authenticatedUser,
            IRecipeWriteOnlyRepository repository
            )
        {
            //configurar o _mapper em services automapper
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _authenticatedUser = authenticatedUser;
            _repository = repository;
        }

        public async Task<ResponseRecipeJson> Execute(RequestRegisterRecipeJson req)
        {
            Validate(req);

            //GetUser implementado em Application services authenticatedUser
            //pega o usuario utilizando o token do header
            var user = await _authenticatedUser.GetUser();

            //configurar o _mapper em services automapper
            // transforma RequestRegisterRecipeJson em Recipe para criar a receita
            var recipe = _mapper.Map<Domain.Entities.Recipe>(req);
            
            recipe.UserId = user.Id;

            _repository.Register(recipe);

            await _unitOfWork.Commit();

            //transforma recipe em ResponseRecipeJson
            return _mapper.Map<ResponseRecipeJson>(recipe);//retorna a receita criada
        }

        //validacoes do request para validar receitas
        //criadas em RegisterRecipeValidator
        private void Validate(RequestRegisterRecipeJson req) 
        {
            var validator = new RegisterRecipeValidator();
            //resultado das validacoes de req
            var result = validator.Validate(req);

            if(!result.IsValid)
            {
                var errorMessages = result.Errors.Select(c => c.ErrorMessage).ToList();
                throw new ValidationErrors(errorMessages);
            }
        }
    }
}
