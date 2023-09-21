using System.Collections;
using static_analysis.map;

namespace static_analysis.output;

public class CsvHandler : IOutputHandler
{
    public void Handle(string outputDir)
    {
        string controllerFileName = Path.Combine(outputDir, "controller.csv");
        string httpHandlerFileName = Path.Combine(outputDir, "httpHandler.csv");
        string httpModuleFileName = Path.Combine(outputDir, "httpModule.csv");
        string apiControllerFileName = Path.Combine(outputDir, "apiController.csv");
        string psFileName = Path.Combine(outputDir, "ps.csv");
        Write(controllerFileName, MapManager.ControllerMaps, BaseMap.GetCsvHeader);
        Write(httpHandlerFileName, MapManager.HttpHandlerMaps, BaseMap.GetCsvHeader);
        Write(httpModuleFileName, MapManager.HttpModuleMaps, BaseMap.GetCsvHeader);
        Write(apiControllerFileName, MapManager.ApiControllerMaps, BaseMap.GetCsvHeader);
        Write(psFileName, MapManager.PsMaps, PsMaps.GetCsvHeader);
        Console.WriteLine($"输出目录: {outputDir}");
    }

    private void Write(string fileName, object maps, Func<string> getCsvHeader)
    {
        using var controllerStream = new StreamWriter(fileName);
        foreach (var mapElem in (IEnumerable)maps)
        {
            controllerStream.WriteLine(getCsvHeader.Invoke());
            controllerStream.WriteLine(((BaseMap)mapElem).ToCsvString());
        }
    }
}