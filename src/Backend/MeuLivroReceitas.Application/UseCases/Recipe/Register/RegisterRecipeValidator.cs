using FluentValidation;
using FluentValidation.Results;
using MeuLivroReceitas.Comunication.Request;
using MeuLivroReceitas.Exceptions;

namespace MeuLivroReceitas.Application.UseCases.Recipe.Register
{
    //validacoes do request para validar receitas
    public class RegisterRecipeValidator : AbstractValidator<RequestRegisterRecipeJson>
    {
        public RegisterRecipeValidator()
        {
            RuleFor(c => c.Title).NotEmpty().WithMessage(ResourceMessageError.EMPTY_USER);
            RuleFor(c => c.Category).IsInEnum().WithMessage(ResourceMessageError.EMAIL_EMPTY);
            RuleFor(c => c.MethodPreparation).NotEmpty().WithMessage(ResourceMessageError.EMAIL_EMPTY);
            RuleFor(c => c.Ingredients).NotEmpty().WithMessage(ResourceMessageError.PHONE_EMPTY);

            //validacao para cada elemento dentro da lista de ingredientes
            RuleForEach(c => c.Ingredients).ChildRules(Ingredient =>
            {
                Ingredient.RuleFor(i => i.Product).NotEmpty();
                Ingredient.RuleFor(i => i.Quantity).NotEmpty();
            });

            //nao criar um ingrediente que ja existe
            RuleFor(c => c.Ingredients).Custom((ingredients,context) =>
            {
                //dentro da lista de productsDistint tera somente strings que nao sao iguais
                var productsDistint = ingredients.Select(i => i.Product).Distinct();
                //se o tamanho de productsDistint
                // for diferent do tamanho da lista de ingredients
                //significa que na lista contem produtos repetidos
                if (productsDistint.Count() != ingredients.Count())
                {
                    //adiciona falha ao context
                    context.AddFailure(new ValidationFailure("Ingredients", ""));
                }
            })
        }
    }
}
