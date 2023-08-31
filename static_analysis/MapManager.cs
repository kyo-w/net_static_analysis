using static_analysis.map;
using static_analysis.output;

namespace static_analysis;

public class MapManager
{
    public static readonly List<ControllerMap> ControllerMaps = new List<ControllerMap>();
    public static readonly List<ApiControllerMap> ApiControllerMaps = new List<ApiControllerMap>();
    public static readonly List<HttpModuleMap> HttpModuleMaps = new List<HttpModuleMap>();
    public static readonly List<HttpHandlerMap> HttpHandlerMaps = new List<HttpHandlerMap>();

    public static void Output(
        string outputDirOption,
        string outputTypeOption)
    {
        var outputTypeHandler = getOutputTypeHandler(outputTypeOption);
        outputTypeHandler.Handle(outputDirOption);
    }

    private static IOutputHandler getOutputTypeHandler(string outputTypeOption)
    {
        switch (outputTypeOption)
        {
            case "console":
                return new ConsoleHandler();
            case "csv":
                return new CsvHandler();
            case "json":
                return new JsonHandler();
            default:
                return new CsvHandler();
        }
    }
}