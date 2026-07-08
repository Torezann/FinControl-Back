using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace FinControl.API.Extensions;

public static class ControllerBaseExtensions
{
    public static Guid GetUserId(this ControllerBase controller)
    {
        var value = controller.User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? controller.User.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.Parse(value!);
    }
}
