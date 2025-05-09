using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

public class CacheService
{
	private readonly IDistributedCache _cache;

	public CacheService(IDistributedCache cache)
	{
		_cache = cache;
	}

	public async Task SetCacheAsync(string key, string value, TimeSpan expiration)
	{
		var options = new DistributedCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = expiration
		};

		await _cache.SetStringAsync(key, value, options);
	}

	public async Task<string?> GetCacheAsync(string key)
	{
		return await _cache.GetStringAsync(key);
	}
}
