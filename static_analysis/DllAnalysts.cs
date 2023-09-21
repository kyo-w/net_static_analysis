using AsmResolver.DotNet;
using Microsoft.Diagnostics.Runtime;
using ShellProgressBar;
using static_analysis.map;
using static_analysis.utils;

namespace static_analysis;

public class DllAnalysts
{
    private static readonly ProgressBarOptions ProgressBarOptions = new ProgressBarOptions
    {
        ForegroundColor = ConsoleColor.Yellow,
        ForegroundColorDone = ConsoleColor.DarkGreen,
        BackgroundColor = ConsoleColor.DarkGray,
        BackgroundCharacter = '#'
    };

    private static readonly Dictionary<string, List<TypeDefinition>> CacheTypeDefinitions =
        new Dictionary<string, List<TypeDefinition>>();

    private static int _allTypeSize = 0;

    public static void AnalystsEveryDll(IEnumerable<ClrModule> clrModules, bool passSystemDllOption,
        bool singleAssemblyOption)
    {
        var processAllDll = clrModules.Select(elem => elem.Name).ToList();
        Console.WriteLine($"加载分析模块: 总计{processAllDll.Count}个DLL");
        var fileLists =
            FileUtils.FilterPassSystemOption(processAllDll.ToArray(), passSystemDllOption, singleAssemblyOption);
        AnalystsEveryClassType(ProgressLoaderModule(fileLists));
    }

    public static void AnalystsEveryDll(string[] paths)
    {
        Console.WriteLine($"加载分析模块: 总计{paths.Length}个DLL");
        AnalystsEveryClassType(ProgressLoaderModule(paths));
    }

    private static void AnalystsEveryClassType(List<ModuleDefinition> moduleDefinitions)
    {
        foreach (var oneOfDll in moduleDefinitions)
        {
            string moduleName = oneOfDll.Name;
            var typeDefinitions = oneOfDll.GetAllTypes().ToList();
            _allTypeSize += typeDefinitions.Count;
            var complete = CacheTypeDefinitions.TryAdd(moduleName, typeDefinitions);
            if (!complete)
            {
                continue;
            }
            using (var bar = new ProgressBar(typeDefinitions.Count, $"分析模块: {oneOfDll.Name}", ProgressBarOptions))
            {
                foreach (var typeDefinition in typeDefinitions)
                {
                    if (typeDefinition.InheritsFrom(NamespaceConstant.TYPE_CONTROLLER) ||
                        typeDefinition.InheritsFrom(NamespaceConstant.TYPE_CONTROLLER_BASE))
                    {
                        ControllerMap.RegistryRecord(moduleName, typeDefinition);
                    }
                    else if (typeDefinition.InheritsFrom(NamespaceConstant.TYPE_API_CONTROLLER))
                    {
                        ApiControllerMap.RegistryRecord(moduleName, typeDefinition);
                    }
                    else if (typeDefinition.Implements(NamespaceConstant.TYPE_HTTPMODULE))
                    {
                        HttpModuleMap.RegistryRecord(moduleName, typeDefinition);
                    }
                    else if (typeDefinition.Implements(NamespaceConstant.TYPE_HTTPHANDLER))
                    {
                        HttpHandlerMap.RegistryRecord(moduleName, typeDefinition);
                    }else if (typeDefinition.InheritsFrom(NamespaceConstant.TYPE_PS))
                    {
                        PsMaps.RegistryRecord(moduleName, typeDefinition);
                    }
                    bar.Tick();
                }
            }
        }
    }

    private static List<ModuleDefinition> ProgressLoaderModule(string[] allModule)
    {
        Console.Clear();
        using var bar = new ProgressBar(allModule.Length, "分析DLL模块", ProgressBarOptions);
        var assemblies = new List<ModuleDefinition>();
        var analysis = 0;
        foreach (var fileName in allModule)
        {
            analysis++;

            try
            {
                assemblies.Add(AsmResolver.DotNet.ModuleDefinition.FromFile(fileName));
                bar.Tick($"{allModule.Length}/{analysis} 加载: {fileName}模块");
            }
            catch (Exception)
            {
                bar.Tick($"{allModule.Length}/{analysis} 加载失败: {fileName}模块跳过");
            }
        }

        return assemblies;
    }

    public static List<BaseMap> FindClassByClassName(string className)
    {
        var customClassTypes = new List<BaseMap>();
        using var bar = new ProgressBar(_allTypeSize, "分析已缓存的数据", ProgressBarOptions);
        for (var i = 0; i < CacheTypeDefinitions.Count; i++)
        {
            KeyValuePair<string, List<TypeDefinition>> kv = CacheTypeDefinitions.ElementAt(i);
            string moduleName = kv.Key;
            foreach (var typeDefinition in kv.Value)
            {
                if (typeDefinition.InheritsFrom(className))
                {
                    var customClassType = new BaseMap
                    {
                        ClassName = typeDefinition.FullName,
                        ModuleName = moduleName
                    };
                    customClassTypes.Add(customClassType);
                }
                bar.Tick();
            }
        }

        return customClassTypes;
    }

    public static List<BaseMap> FindClassByInterfaceName(string interfaceName)
    {
        var customClassTypes = new List<BaseMap>();
        using var bar = new ProgressBar(_allTypeSize, "分析已缓存的数据", ProgressBarOptions);
        for (var i = 0; i < CacheTypeDefinitions.Count; i++)
        {
            KeyValuePair<string, List<TypeDefinition>> kv = CacheTypeDefinitions.ElementAt(i);
            var moduleName = kv.Key;
            foreach (var typeDefinition in kv.Value)
            {
                if (typeDefinition.Implements(interfaceName))
                {
                    var customClassType = new BaseMap
                    {
                        ClassName = typeDefinition.FullName,
                        ModuleName = moduleName
                    };
                    customClassTypes.Add(customClassType);
                }
                bar.Tick();
            }
        }

        return customClassTypes;
    }
}