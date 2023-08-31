using System.Collections;
using System.Text.Json;
using static_analysis.map;

namespace static_analysis.output;

public class JsonHandler : IOutputHandler
{
    public void Handle(string outputDir)
    {
        string controllerFileName = Path.Combine(outputDir, "controller.json");
        string httpHandlerFileName = Path.Combine(outputDir, "httpHandler.json");
        string httpModuleFileName = Path.Combine(outputDir, "httpModule.json");
        string apiControllerFileName = Path.Combine(outputDir, "apiController.json");
        Write(controllerFileName, MapManager.ControllerMaps);
        Write(httpHandlerFileName, MapManager.HttpHandlerMaps);
        Write(httpModuleFileName, MapManager.HttpModuleMaps);
        Write(apiControllerFileName, MapManager.ApiControllerMaps);
    }

    private void Write(string fileName, object maps)
    {
        using (var controllerStream = new StreamWriter(fileName))
        {
            var serialize = JsonSerializer.Serialize(maps);
            controllerStream.WriteLine(serialize);
        }
    }
}