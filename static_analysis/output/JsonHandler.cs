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
        string psFileName = Path.Combine(outputDir, "ps.json");
        Write(controllerFileName, MapManager.ControllerMaps, typeof(List<ControllerMap>));
        Write(httpHandlerFileName, MapManager.HttpHandlerMaps, typeof(List<HttpHandlerMap>));
        Write(httpModuleFileName, MapManager.HttpModuleMaps, typeof(List<HttpModuleMap>));
        Write(apiControllerFileName, MapManager.ApiControllerMaps, typeof(List<ApiControllerMap>));
        Write(psFileName, MapManager.PsMaps, typeof(List<PsMaps>));
    }

    private void Write(string fileName, object maps, Type type)
    {
        using (var controllerStream = new StreamWriter(fileName))
        {
            var serialize = JsonSerializer.Serialize(maps, type);
            controllerStream.WriteLine(serialize);
        }
    }
}