using Core.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Services;
public class ResponseCacheService : IResponseCacheService
{
    private readonly IDatabase _database;

    public ResponseCacheService(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
    {
        if (response is null)
        {
            return;
        }

        var option = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        var serializeResponse = JsonSerializer.Serialize(response, option);

        await _database.StringSetAsync(cacheKey, serializeResponse, timeToLive);
    }

    public async Task<string> GetCacheResponseAsync(string cacheKey)
    {
        var response = await _database.StringGetAsync(cacheKey);

        if (response.IsNullOrEmpty)
        {
            return null;
        }

        return response;
    }
}
