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

    public static void AnalystsEveryDll(IEnumerable<ClrModule> clrModules, bool passSystemDllOption, bool singleAssemblyOption)
    {
        var processAllDll = clrModules.Select(elem => elem.Name).ToList();
        Console.WriteLine($"加载分析模块: 总计{processAllDll.Count}个DLL");
        var fileLists = FileUtils.FilterPassSystemOption(processAllDll.ToArray(), passSystemDllOption, singleAssemblyOption);
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
            using (var bar = new ProgressBar(typeDefinitions.Count, $"分析模块: {oneOfDll.Name}", _progressBarOptions))
            {
                foreach (var typeDefinition in typeDefinitions)
                {
                    if (typeDefinition.InheritsFrom(NamespaceConstant.TYPE_CONTROLLER) ||
                        typeDefinition.InheritsFrom(NamespaceConstant.TYPE_CONTROLLER_BASE))
                    {
                        MapManager.ControllerMaps.Add(new ControllerMap(moduleName, typeDefinition.FullName));
                    }
                    else if (typeDefinition.InheritsFrom(NamespaceConstant.TYPE_API_CONTROLLER))
                    {
                        MapManager.ApiControllerMaps.Add(new ApiControllerMap(moduleName, typeDefinition.FullName));
                    }
                    else if (typeDefinition.Implements(NamespaceConstant.TYPE_HTTPMODULE))
                    {
                        MapManager.HttpModuleMaps.Add(new HttpModuleMap(moduleName, typeDefinition.FullName));
                    }
                    else if (typeDefinition.Implements(NamespaceConstant.TYPE_HTTPHANDLER))
                    {
                        MapManager.HttpHandlerMaps.Add(new HttpHandlerMap(moduleName, typeDefinition.FullName));
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
}