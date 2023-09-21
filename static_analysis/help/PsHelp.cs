using System.CommandLine;
using System.Diagnostics;
using System.Management;

namespace static_analysis.help;

public static class PsHelp
{
    public static void ConfigPsCommand(this RootCommand rootCommand)
    {
        var command = new Command("ps", "获取当前.net进程信息");
        var nameOption = new Option<bool>(
            name: "--name",
            description:"打印程序名称(默认只打印PID/命令行)", getDefaultValue: ()=>false);
        var isOption = new Option<bool>(
            name: "--iis",
            description: "只打印IIS(w3p3进程)", getDefaultValue:()=> false);
        command.AddOption(nameOption);
        command.AddOption(isOption);
        command.SetHandler(HandlerPs, nameOption, isOption);
        rootCommand.Add(command);
    }

    private static void HandlerPs(bool nameOption, bool iisOption)
    {
        var processes = Process.GetProcesses();
        foreach (var process in processes)
        {
            if (!IsIis(process, iisOption))
            {
                continue;
            }
            var isShape = false;
            try
            {
                var processModuleCollection = process.Modules;
                if (processModuleCollection.Cast<ProcessModule>().Any(processModule => processModule.FileName != null && processModule.FileName.Contains("clr.dll")))
                {
                    isShape = true;
                }
            }
            catch (Exception)
            {
                // ignored
            }

            if (!isShape) continue;
            var commandLineArgs = process.GetCommandLine();
            PrintConsole(nameOption, process, commandLineArgs);
        }
    }

    private static bool IsIis(Process process, bool iisOption)
    {
        return process.ProcessName == "w3wp" || !iisOption;
    }

    private static void PrintConsole(bool nameOption, Process process, string commandLineArgs)
    {
        Console.WriteLine(nameOption
            ? $"PID: {process.Id}, Name: {process.ProcessName}, CommandLineArgs: {commandLineArgs}"
            : $"PID: {process.Id}, CommandLineArgs: {commandLineArgs}");
    }
}

internal static class ProcessExtensions
{
    public static string GetCommandLine(this Process? process)
    {
        if (process is null || process.Id < 1)
        {
            return "";
        }

        if (!OperatingSystem.IsWindows())
        {
            throw new PlatformNotSupportedException("WMI is only supported on Windows.");
        }

        string query =
            $@"SELECT CommandLine
           FROM Win32_Process
           WHERE ProcessId = {process.Id}";

        using var searcher = new ManagementObjectSearcher(query);
        using var collection = searcher.Get();
        var managementObject = collection.OfType<ManagementObject>().FirstOrDefault();

        return managementObject != null ? (string)managementObject["CommandLine"] : "";
    }
}