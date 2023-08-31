using System.CommandLine;
using System.Diagnostics;
using Microsoft.Diagnostics.Runtime;
using static_analysis.utils;

namespace static_analysis.help;

public static class ProcessHelp
{
    public static void ConfigProcessCommand(this RootCommand rootCommand)
    {
        var command = new Command("process", "进程分析");
        var pidOption = new Option<int>(
            name: "--pid",
            description: ".net进程"){IsRequired = true};
        command.AddOption(pidOption);
        command.SetHandler(ProcessHandler, pidOption, 
            GlobalHelp.PassSystemDllOption,
            GlobalHelp.SingleAssemblyOption,
            GlobalHelp.OutputTypeOption,
            GlobalHelp.OutputDirOption);
        rootCommand.Add(command);
    }

    private static void ProcessHandler(int pid,
        bool passSystemDllOption,
        bool singleAssemblyOption,
        string outputTypeOption,
        string outputDirOption)
    {
        if (FileUtils.CreateOutputDir(outputDirOption))
        {
            var process = Process.GetProcessById(pid);
            var attachToProcess = DataTarget.AttachToProcess(process.Id, false);
            var clrInfo = attachToProcess.ClrVersions.First();
            DllAnalysts.AnalystsEveryDll(clrInfo.CreateRuntime().EnumerateModules(), passSystemDllOption, singleAssemblyOption);
            MapManager.Output( outputDirOption, outputTypeOption);   
        }
        else
        {
            Console.WriteLine($"输出目录无法创建: {outputDirOption}");
        }
    }
}