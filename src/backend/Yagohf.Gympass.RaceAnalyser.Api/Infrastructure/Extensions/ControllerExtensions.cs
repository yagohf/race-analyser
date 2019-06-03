using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Yagohf.Gympass.RaceAnalyser.Api.Infrastructure.Extensions
{
    public static class ControllerExtensions
    {
        public static string GetLoggedUser(this Controller controller)
        {
            return controller.HttpContext.User.Claims.Single(c => c.Type.Equals(ClaimTypes.Name)).Value;
        }
    }
}
