namespace API.Cache;

public interface ICacheService
{
    Task<T?> TryGetAsync<T>(string key);

    Task UpsertAsync<T>(string key, T value);
}
