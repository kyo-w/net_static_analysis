using System.CommandLine;

namespace static_analysis.help;

public static class GlobalHelp
{
    public static Option<bool> PassSystemDllOption;

    public static Option<bool> SingleAssemblyOption;
    public static Option<string> OutputTypeOption;
    public static Option<string> OutputDirOption;
    public static void ConfigGlobalCommand(this RootCommand rootCommand)
    {
        PassSystemDllOption = new Option<bool>(
            name: "--pass-system",
            description: "过滤System相关的程序集",
            getDefaultValue: () => true);
        SingleAssemblyOption = new Option<bool>(
            name: "--single-assembly", 
            description: "去除重名的dll文件，分析第一个获取的dll文件"
            ,getDefaultValue: () => true);
        OutputTypeOption = new Option<string>(
            name: "--output-type",
            description: "输出格式[csv/json/console]",
            getDefaultValue: ()=> "csv");
        OutputDirOption = new Option<string>(
            name: "--output-dir",
            description: "输出位置必须为目录(默认当前位置下的output)",
            getDefaultValue:()=> Path.Combine(Directory.GetCurrentDirectory(), "output"));
        rootCommand.AddGlobalOption(PassSystemDllOption);
        rootCommand.AddGlobalOption(SingleAssemblyOption);
        rootCommand.AddGlobalOption(OutputTypeOption);
        rootCommand.AddGlobalOption(OutputDirOption);
    }

}