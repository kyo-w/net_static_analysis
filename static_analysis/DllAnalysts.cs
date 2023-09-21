using AsmResolver.DotNet;
using Microsoft.Diagnostics.Runtime;
using ShellProgressBar;
using static_analysis.map;
using static_analysis.utils;

namespace static_analysis;

public class DllAnalysts
{
    private static readonly ProgressBarOptions _progressBarOptions = new ProgressBarOptions
    {
        ForegroundColor = ConsoleColor.Yellow,
        ForegroundColorDone = ConsoleColor.DarkGreen,
        BackgroundColor = ConsoleColor.DarkGray,
        BackgroundCharacter = '#'
    };

    private static readonly Dictionary<string, List<TypeDefinition>> cacheTypeDefinitions =
        new Dictionary<string, List<TypeDefinition>>();

    private static int allTypeSize = 0;

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
            allTypeSize += typeDefinitions.Count;
            var complete = cacheTypeDefinitions.TryAdd(moduleName, typeDefinitions);
            if (!complete)
            {
                continue;
            }
            using (var bar = new ProgressBar(typeDefinitions.Count, $"分析模块: {oneOfDll.Name}", _progressBarOptions))
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
        using var bar = new ProgressBar(allModule.Length, "分析DLL模块", _progressBarOptions);
        var assemblies = new List<ModuleDefinition>();
        int analysis = 0;
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

    public static List<CustomClassType> FindClassByClassName(string className)
    {
        var customClassTypes = new List<CustomClassType>();
        using (var bar = new ProgressBar(allTypeSize, "分析已缓存的数据", _progressBarOptions))
        {
            for (int i = 0; i < cacheTypeDefinitions.Count; i++)
            {
                KeyValuePair<string, List<TypeDefinition>> kv = cacheTypeDefinitions.ElementAt(i);
                string moduleName = kv.Key;
                foreach (var typeDefinition in kv.Value)
                {
                    if (typeDefinition.InheritsFrom(className))
                    {
                        var customClassType = new CustomClassType();
                        customClassType.ClassName = typeDefinition.FullName;
                        customClassType.ModuleName = moduleName;
                        customClassTypes.Add(customClassType);
                    }
                    bar.Tick();
                }
            }
        }
        return customClassTypes;
    }

    public static List<CustomClassType> FindClassByInterfaceName(string interfaceName)
    {
        var customClassTypes = new List<CustomClassType>();
        using (var bar = new ProgressBar(allTypeSize, "分析已缓存的数据", _progressBarOptions))
        {
            for (int i = 0; i < cacheTypeDefinitions.Count; i++)
            {
                KeyValuePair<string, List<TypeDefinition>> kv = cacheTypeDefinitions.ElementAt(i);
                string moduleName = kv.Key;
                foreach (var typeDefinition in kv.Value)
                {
                    if (typeDefinition.Implements(interfaceName))
                    {
                        var customClassType = new CustomClassType();
                        customClassType.ClassName = typeDefinition.FullName;
                        customClassType.ModuleName = moduleName;
                        customClassTypes.Add(customClassType);
                    }
                    bar.Tick();
                }
            }
        }
        return customClassTypes;
    }
}