using SpeakMate.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Infrastructure.Services
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            if (context.HttpContext.User.Identity?.IsAuthenticated!=true) return;

            var memberId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var dbcontex= context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();

            await dbcontex.Members.
                Where(m=>m.Id == memberId).
                ExecuteUpdateAsync(setters=>setters.SetProperty(x=>x.LastActive,DateTime.UtcNow));

        }
    }
}
