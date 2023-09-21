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
            description: ".net进程") { IsRequired = true };
        var shellOption = new Option<bool>(
            name: "--shell",
            description: "交互式shell(extends/interface/help)",
            getDefaultValue: () => false);
        command.AddOption(pidOption);
        command.AddOption(shellOption);
        command.SetHandler(ProcessHandler, pidOption,
            GlobalHelp.PassSystemDllOption,
            GlobalHelp.SingleAssemblyOption,
            GlobalHelp.OutputTypeOption,
            GlobalHelp.OutputDirOption,
            shellOption);
        rootCommand.Add(command);
    }

    private static void ProcessHandler(int pid,
        bool passSystemDllOption,
        bool singleAssemblyOption,
        string outputTypeOption,
        string outputDirOption, bool shellOption)
    {
        if (FileUtils.CreateOutputDir(outputDirOption))
        {
            var process = Process.GetProcessById(pid);
            var attachToProcess = DataTarget.AttachToProcess(process.Id, false);
            var clrInfo = attachToProcess.ClrVersions.First();
            DllAnalysts.AnalystsEveryDll(clrInfo.CreateRuntime().EnumerateModules(), passSystemDllOption,
                singleAssemblyOption);
            if (shellOption)
            {
                new ShellHandler().Run();
            }
            else
            {
                MapManager.Output(outputDirOption, outputTypeOption);
            }
        }
        else
        {
            Console.WriteLine($"输出目录无法创建: {outputDirOption}");
        }
    }
}