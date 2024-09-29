using StackExchange.Redis;
using System.Text.Json;

namespace API.Cache;

public class RedisCacheService(IConnectionMultiplexer connectionMultiplexer) : ICacheService
{
    public async Task<T?> TryGetAsync<T>(string key)
    {
        var db = connectionMultiplexer.GetDatabase();
        var value = await db.StringGetAsync(key);
        return !value.IsNull ? JsonSerializer.Deserialize<T>(value.ToString()) : default;
    }

    public async Task UpsertAsync<T>(string key, T value)
    {
        var db = connectionMultiplexer.GetDatabase();
        await db.StringSetAsync(key, JsonSerializer.Serialize(value));
    }
}
