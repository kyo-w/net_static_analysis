using System.CommandLine;
using ShellProgressBar;
using static_analysis.help;

public class Program
{
    static async Task Main(string[] args)
    {
        var rootCommand = new RootCommand("静态分析dll工具");
        rootCommand.ConfigGlobalCommand();
        rootCommand.ConfigProcessCommand();
        rootCommand.ConfigPathCommand();
        rootCommand.ConfigPsCommand();
        await rootCommand.InvokeAsync(args);
    }
}