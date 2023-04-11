using System.Globalization;

namespace MeuLivroReceitas.Api.Middleware;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IList<string> _suposupportedLanguage = new List<string> 
    {
        "pt",
        "en"
    };
    public CultureMiddleware(RequestDelegate next)
    {

        _next = next;

    }

    //HttpContentpara ter acesso ao que esta chegando a api
    public async Task Invoke(HttpContext context)
    {
        var culture = new CultureInfo("pt");

        //verifica se no header contem Accept-Language
        //que é enviado pelo frontend
        if (context.Request.Headers.ContainsKey("Accept-Language"))
        {
            var language = context.Request.Headers["Accept-Language"].ToString(); ;

            //se contem a linguagem recebida em  _suposupportedLanguage
            if (_suposupportedLanguage.Any(c => c.Equals(language)))
            {
                culture = new CultureInfo (language);
            }

        }

        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        await _next(context);
    }
}
