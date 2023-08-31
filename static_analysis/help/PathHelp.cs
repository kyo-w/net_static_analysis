using System.CommandLine;
using static_analysis.utils;

namespace static_analysis.help;

public static class PathHelp
{
    public static void ConfigPathCommand(this RootCommand rootCommand)
    {
        var command = new Command("path", "本地路径分析");
        var fileOption = new Option<string>(
            name: "--path",
            description: "目标工程的web目录") { IsRequired = true };
        var iterOption = new Option<bool>(
            name: "--iter",
            description: "遍历目录所有dll", getDefaultValue: () => true);
        command.AddOption(fileOption);
        command.SetHandler(PathHandler, fileOption, iterOption,
            GlobalHelp.PassSystemDllOption, GlobalHelp.SingleAssemblyOption
            , GlobalHelp.OutputDirOption,
            GlobalHelp.OutputTypeOption);
        rootCommand.Add(command);
    }

    private static void PathHandler(string fileOption,
        bool iterOption,
        bool passSystemOption,
        bool singleAssemblyOption,
        string outputDirOption,
        string outputTypeOption)
    {
        if (FileUtils.CreateOutputDir(outputDirOption))
        {
            var files = fileOption.Split(";");
            DllAnalysts.AnalystsEveryDll(FileUtils.GetAllDllFileByDirName(files, iterOption, passSystemOption,
                singleAssemblyOption));
            MapManager.Output(outputDirOption, outputTypeOption);
        }
        else
        {
            Console.WriteLine($"输出目录无法创建: {outputDirOption}");
        }
    }
}