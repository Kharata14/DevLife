using DevLife.Application.Features.MeetingEscapes.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DevLife.Application.Features.MeetingEscapes.Commands;
public class SaveFavoriteExcuseCommandHandler : IRequestHandler<SaveFavoriteExcuseCommand, Unit>
{
    private readonly IDistributedCache _cache;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SaveFavoriteExcuseCommandHandler(IDistributedCache cache, IHttpContextAccessor httpContextAccessor)
    {
        _cache = cache;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Unit> Handle(SaveFavoriteExcuseCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException();
        }

        var cacheKey = $"favorites:excuses:{userId}";

        var favoritesJson = await _cache.GetStringAsync(cacheKey, cancellationToken);
        var favorites = string.IsNullOrEmpty(favoritesJson)
            ? new List<ExcuseDto>()
            : JsonSerializer.Deserialize<List<ExcuseDto>>(favoritesJson);

        if (!favorites.Any(f => f.Text == request.Excuse.Text))
        {
            favorites.Add(request.Excuse);
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(favorites), cancellationToken);
        }

        return Unit.Value;
    }
}
