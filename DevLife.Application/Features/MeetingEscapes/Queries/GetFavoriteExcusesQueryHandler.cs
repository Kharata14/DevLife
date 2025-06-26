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

namespace DevLife.Application.Features.MeetingEscapes.Queries;
public class GetFavoriteExcusesQueryHandler : IRequestHandler<GetFavoriteExcusesQuery, IEnumerable<ExcuseDto>>
{
    private readonly IDistributedCache _cache;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetFavoriteExcusesQueryHandler(IDistributedCache cache, IHttpContextAccessor httpContextAccessor)
    {
        _cache = cache;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IEnumerable<ExcuseDto>> Handle(GetFavoriteExcusesQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Enumerable.Empty<ExcuseDto>();
        }

        var cacheKey = $"favorites:excuses:{userId}";

        var favoritesJson = await _cache.GetStringAsync(cacheKey, cancellationToken);

        if (string.IsNullOrEmpty(favoritesJson))
        {
            return Enumerable.Empty<ExcuseDto>();
        }

        var favorites = JsonSerializer.Deserialize<List<ExcuseDto>>(favoritesJson);
        return favorites;
    }
}