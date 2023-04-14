using Microsoft.AspNetCore.Authorization;

namespace MeuLivroReceitas.Api.Filters.AuthenticatedUser;

public class LoggedUserRequirements: IAuthorizationRequirement
{
}
