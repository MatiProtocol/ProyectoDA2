using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using NonDomainAbstractions;

namespace Services;

[ExcludeFromCodeCoverage]
public class StrategyLoader : IDisposable
{
    private const string StrategiesFolder = "Plugins";
    private readonly Dictionary<string, IPunctuationStrategy> _loadedStrategies;
    private readonly FileSystemWatcher _pluginWatcher;
    private readonly IServiceProvider _serviceProvider;

    public StrategyLoader(Dictionary<string, IPunctuationStrategy> loadedStrategies, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        if(!Directory.Exists(StrategiesFolder))
        {
            Directory.CreateDirectory(StrategiesFolder);
        }

        _loadedStrategies = loadedStrategies;
        LoadAllPlugins();

        _pluginWatcher = new FileSystemWatcher(StrategiesFolder, "*.dll")
        {
            EnableRaisingEvents = true
        };

        _pluginWatcher.Created += (_, e) => LoadStrategy(e.FullPath);
        _pluginWatcher.Deleted += (_, e) => RemovePlugin(e.FullPath);
    }

    private void LoadAllPlugins()
    {
        var files = Directory.GetFiles(StrategiesFolder, "*.dll");

        foreach(var file in files)
        {
            LoadStrategy(file);
        }
    }

    private void LoadStrategy(string filePath)
    {
        try
        {
            var assembly = Assembly.LoadFrom(filePath);
            var types = assembly.GetTypes()
                                .Where(t => typeof(IPunctuationStrategy)
                                    .IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false });

            foreach(var type in types)
            {
                var strategy = CreateInstanceWithDependencies(type, _serviceProvider);
                _loadedStrategies[strategy.GetType().Name] = strategy;
            }
        }
        catch(Exception)
        {
            throw new FileLoadException("Error was loading the plugin");
        }
    }

    private static IPunctuationStrategy CreateInstanceWithDependencies(Type type, IServiceProvider serviceProvider)
    {
        var constructor = type.GetConstructors()
            .FirstOrDefault();

        var parameters = constructor!.GetParameters()
            .Select(p => serviceProvider.GetService(p.ParameterType))
            .ToArray();

        var instance = Activator.CreateInstance(type, parameters) as IPunctuationStrategy;

        return instance!;
    }

    private void RemovePlugin(string filePath)
    {
        var toRemove = _loadedStrategies
            .Where(x => x.Value.GetType().Assembly.Location == filePath)
            .Select(x => x.Key)
            .ToList();

        foreach(var key in toRemove)
        {
            _loadedStrategies.Remove(key);
        }
    }

    public void Dispose()
    {
        _pluginWatcher.Dispose();
    }
}
