namespace static_analysis.output;

public class ConsoleHandler : IOutputHandler
{
    public void Handle(string outputDir)
    {
        foreach (var map in MapManager.ControllerMaps)
        {
            Console.WriteLine(map);
        }
        foreach (var map in MapManager.ApiControllerMaps)
        {
            Console.WriteLine(map);
        }
        foreach (var map in MapManager.HttpModuleMaps)
        {
            Console.WriteLine(map);
        }
        foreach (var map in MapManager.HttpHandlerMaps)
        {
            Console.WriteLine(map);
        }
    }
}