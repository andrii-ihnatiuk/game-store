using GameStore.Data;
using GameStore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace GameStore.API.Middlewares;

public class HeaderAdderMiddleware
{
    private readonly RequestDelegate _next;

    public HeaderAdderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, GameStoreDbContext dbContext, IMemoryCache memoryCache)
    {
        string? gamesCount = await memoryCache.GetOrCreateAsync("total-games", async factory =>
        {
            factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
            return (await dbContext.Set<Game>().CountAsync()).ToString();
        });

        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Add("x-total-numbers-of-games", gamesCount);
            return Task.CompletedTask;
        });
        await _next(context);
    }
}