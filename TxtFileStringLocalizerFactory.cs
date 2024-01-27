using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;

public class TxtFileStringLocalizerFactory : IStringLocalizerFactory
{
    private readonly string _resourcesPath;
    private readonly IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    
    public TxtFileStringLocalizerFactory(string resourcesPath)
    {
        _resourcesPath = resourcesPath;
        
    }

    public IStringLocalizer Create(Type resourceSource)
    {
        return new TxtFileStringLocalizer(_resourcesPath,_cache);
    }

    public IStringLocalizer Create2(Type resourceSource,IMemoryCache cache)
    {
        return new TxtFileStringLocalizer(_resourcesPath,cache);
    }

    public IStringLocalizer Create(string baseName, string location)
    {
        return new TxtFileStringLocalizer(_resourcesPath,_cache);
    }
}
