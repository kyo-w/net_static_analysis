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
        Write(controllerFileName, MapManager.ControllerMaps, (elem, outputStream) =>
        {
            outputStream.WriteLine(((ControllerMap)elem).ToCsvString());
        });
        Write(httpHandlerFileName, MapManager.HttpHandlerMaps, (elem, outputStream) =>
        {
            outputStream.WriteLine(((HttpHandlerMap)elem).ToCsvString());
        });
        Write(httpModuleFileName, MapManager.HttpModuleMaps, (elem, outputStream) =>
        {
            outputStream.WriteLine(((HttpModuleMap)elem).ToCsvString());
        });
        Write(apiControllerFileName, MapManager.ApiControllerMaps, (elem, outputStream) =>
        {
            outputStream.WriteLine(((ApiControllerMap)elem).ToCsvString());
        });
        Console.WriteLine($"输出目录: {outputDir}");
    }

    private void Write(string fileName, object maps,  Action<object, StreamWriter> func)
    {
        using (var controllerStream = new StreamWriter(fileName))
        {
            controllerStream.WriteLine("Assembly, ClassName");
            foreach (var mapElem in (IEnumerable)maps)
            {
                func(mapElem, controllerStream);
            }
        }
    }
}