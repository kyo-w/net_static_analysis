using static_analysis.map;
using static_analysis.output;

namespace static_analysis;

public class MapManager
{
    public static readonly List<ControllerMap> ControllerMaps = new List<ControllerMap>();
    public static readonly List<ApiControllerMap> ApiControllerMaps = new List<ApiControllerMap>();
    public static readonly List<HttpModuleMap> HttpModuleMaps = new List<HttpModuleMap>();
    public static readonly List<HttpHandlerMap> HttpHandlerMaps = new List<HttpHandlerMap>();
    public static readonly List<PsMaps> PsMaps = new List<PsMaps>();

    public static void Output(
        string outputDirOption,
        string outputTypeOption)
    {
        var outputTypeHandler = GetOutputTypeHandler(outputTypeOption);
        outputTypeHandler.Handle(outputDirOption);
    }

    private static IOutputHandler GetOutputTypeHandler(string outputTypeOption)
    {
        return outputTypeOption switch
        {
            "console" => new ConsoleHandler(),
            "csv" => new CsvHandler(),
            "json" => new JsonHandler(),
            _ => new CsvHandler()
        };
    }
}