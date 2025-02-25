using System.Text.Json;

// ReSharper disable once CheckNamespace
// We want to add the extensions to the default named by Microsoft
// So it's easier to use :)
namespace Microsoft.Extensions.Caching.Distributed;

public static class DistributedCacheJsonExtensions
{
    /// <summary>
    /// Asynchronously sets a json string in the specified cache with the specified key. 
    /// </summary>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to store the data in.</param>
    /// <param name="value">The data to store in the cache.</param>
    /// <param name="cancel">Optional. A CancellationToken to cancel the operation.</param>
    /// <typeparam name="T">Object Type.</typeparam>
    public static Task SetJsonAsync<T>(this IDistributedCache cache, string key, T value, CancellationToken cancel = default)
        => SetJsonAsync(cache, key, value, new DistributedCacheEntryOptions(), cancel);
    
    /// <summary>
    /// Asynchronously sets a json string in the specified cache with the specified key. 
    /// </summary>
    /// <param name="cache">The cache in which to store the data.</param>
    /// <param name="key">The key to store the data in.</param>
    /// <param name="value">The data to store in the cache.</param>
    /// <param name="options">The cache options for the entry.</param>
    /// <param name="cancel">Optional. A CancellationToken to cancel the operation.</param>
    /// <typeparam name="T">Object Type.</typeparam>
    public static async Task SetJsonAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options, CancellationToken cancel = default)
    {
        var jsonString = JsonSerializer.Serialize(value);
        await cache.SetStringAsync(key, jsonString, options, cancel);
    }

    /// <summary>
    /// Asynchronously gets a json object from the specified cache with the specified key. 
    /// </summary>
    /// <param name="cache">the cache in which to store the data.</param>
    /// <param name="key">The key to get the stored data for.</param>
    /// <param name="cancel">Optional. A CancellationToken to cancel the operation.</param>
    /// <typeparam name="T">Object Type.</typeparam>
    /// <returns>A task that gets the json object value from the stored cache key.</returns>
    public static async Task<T?> GetJsonAsync<T>(this IDistributedCache cache, string key, CancellationToken cancel = default)
    {
        var jsonString = await cache.GetStringAsync(key, cancel);
        if (jsonString is null) return default;

        var jsonObject = JsonSerializer.Deserialize<T>(jsonString);
        return jsonObject;
    }
}