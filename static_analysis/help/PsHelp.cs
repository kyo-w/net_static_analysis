using System.CommandLine;
using System.Diagnostics;
using System.Management;

namespace static_analysis.help;

public static class PsHelp
{
    public static void ConfigPsCommand(this RootCommand rootCommand)
    {
        var command = new Command("ps", "获取当前.net进程信息");
        var NameOption = new Option<bool>(
            name: "--name",
            description:"打印程序名称(默认只打印PID/命令行)", getDefaultValue: ()=>false);
        var IIsOption = new Option<bool>(
            name: "--iis",
            description: "只打印IIS(w3p3进程)", getDefaultValue:()=> false);
        command.AddOption(NameOption);
        command.AddOption(IIsOption);
        command.SetHandler(HandlerPs, NameOption, IIsOption);
        rootCommand.Add(command);
    }

    private static void HandlerPs(bool nameOption, bool IISOption)
    {
        var processes = Process.GetProcesses();
        foreach (var process in processes)
        {
            if (!isIIS(process, IISOption))
            {
                continue;
            }
            bool isShape = false;
            try
            {
                var processModuleCollection = process.Modules;
                foreach (ProcessModule processModule in processModuleCollection)
                {
                    if (processModule.FileName != null && processModule.FileName.Contains("clr.dll"))
                    {
                        isShape = true;
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }

            if (!isShape) continue;
            string commandLineArgs = process.GetCommandLine();
            printConsole(nameOption, process, commandLineArgs);
        }
    }

    private static bool isIIS(Process process, bool iisOption)
    {
        if (process.ProcessName == "w3wp" || !iisOption)
        {
            return true;
        }
        return false;
    }

    private static void printConsole(bool nameOption, Process process, string commandLineArgs)
    {
        if (nameOption)
        {
            Console.WriteLine($"PID: {process.Id}, Name: {process.ProcessName}, CommandLineArgs: {commandLineArgs}");
        }
        else
        {
            Console.WriteLine($"PID: {process.Id}, CommandLineArgs: {commandLineArgs}");
        }
    }
}

static class ProcessExtensions
{
    public static string GetCommandLine(this Process process)
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

        using (var searcher = new ManagementObjectSearcher(query))
        using (var collection = searcher.Get())
        {
            var managementObject = collection.OfType<ManagementObject>().FirstOrDefault();

            return managementObject != null ? (string)managementObject["CommandLine"] : "";
        }
    }
}