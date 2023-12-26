using System.Globalization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;

public class TxtFileStringLocalizer : IStringLocalizer
{
    private readonly IMemoryCache _cache;
    private readonly string _cacheKey;
    private readonly string _resourcesPath;

    public TxtFileStringLocalizer(string resourcesPath,IMemoryCache cache = null)
    {
        _cache = cache;
        _resourcesPath = resourcesPath;
        _cacheKey = $"TxtFileStringLocalizer_{resourcesPath}";
    }

    public LocalizedString this[string name] => this[name, null];

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var culture = CultureInfo.CurrentUICulture;
            var key = $"{culture.Name}|{name}";
            var resources = _cache.GetOrCreate(_cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(9999);
                return LoadResources();
            });
            if (resources.TryGetValue(key, out var value))
            {
                return new LocalizedString(name, value);
            }

            return new LocalizedString(name, name, true);
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        var resources = _cache.GetOrCreate(_cacheKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
            return LoadResources();
        });
        return resources.Select(r =>
        {
            var keyValue = r.Key.Split('|', 2);
            return new LocalizedString(keyValue[1], r.Value, false, keyValue[0]);
        });
    }

    public IStringLocalizer WithCulture(CultureInfo culture)
    {
        return new TxtFileStringLocalizer(_resourcesPath,_cache);
    }

    private Dictionary<string, string> LoadResources()
    {
        var resources = new Dictionary<string, string>();

        var resourceFiles = Directory.EnumerateFiles(_resourcesPath, "resources.*.txt");
        foreach (var resourceFile in resourceFiles)
        {
            var culture = CultureInfo.GetCultureInfo(Path.GetFileNameWithoutExtension(resourceFile).Split('.').Last());
            var lines = File.ReadAllLines(resourceFile);
            foreach (var line in lines)
            {
                var keyValue = line.Split('=', 2);
                if (keyValue.Length == 2)
                {
                    var key = $"{culture.Name}|{keyValue[0].Trim()}";
                    var value = keyValue[1].Trim();
                    resources[key] = value;
                }
            }
        }

        return resources;
    }
}
