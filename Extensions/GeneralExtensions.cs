
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace PostBook.Extensions
{
    public static class GeneralExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }

            return httpContext.User.Claims.Single(c => c.Type == "id").Value;
        }
    }
}