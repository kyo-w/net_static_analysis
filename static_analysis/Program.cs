using System.CommandLine;
using ShellProgressBar;
using static_analysis.help;

public class Program
{
    private static async Task Main(string[] args)
    {
        Init();
        var rootCommand = new RootCommand("静态分析dll工具");
        rootCommand.ConfigGlobalCommand();
        rootCommand.ConfigProcessCommand();
        rootCommand.ConfigPathCommand();
        rootCommand.ConfigPsCommand();
        await rootCommand.InvokeAsync(args);
    }

    private static void Init()
    {
    }
}