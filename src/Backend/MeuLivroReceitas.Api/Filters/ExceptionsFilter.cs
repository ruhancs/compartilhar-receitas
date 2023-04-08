using System.Net;
using MeuLivroReceitas.Comunication.Response;
using MeuLivroReceitas.Exceptions;
using MeuLivroReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MeuLivroReceitas.Api.Filters;

//em qulaquer lugar de der erro essa funçao e chamada
//ira tratar o erro
//registrar o filtro em program para pegar todos erros
public class ExceptionsFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if(context.Exception is ExceptionBase)
        {
            HandleError(context);
        }
        else
        {

        }
    }

    private void HandleError(ExceptionContext context)
    {
        if(context.Exception is ValidationErrors)
        {
            HandleValidationException(context);
        }
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var validationErrorException = context.Exception as ValidationErrors;
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Result = new ObjectResult(new ResponseError(validationErrorException.MessagesErro));

    }

    private void UnknownError(ExceptionContext context)
    {
        //status 500
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //Result e o corpo da resposta
        context.Result = new ObjectResult(new ResponseError(ResourceMessageError.UNKNOWN_ERROR));
    }
}
