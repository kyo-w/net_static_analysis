using AsmResolver.DotNet;

namespace static_analysis.map;

public class HttpHandlerMap : BaseMap
{
    private HttpHandlerMap(string moduleName, string className)
    {
        ModuleName = moduleName;
        ClassName = className;
    }

    public override string ToString()
    {
        return $"HttpHandler: ModuleName={ModuleName}, ClassName={ClassName}";
    }

    public static void RegistryRecord(string moduleName, TypeDefinition typeDefinition)
    {
        MapManager.HttpHandlerMaps.Add(new HttpHandlerMap(moduleName, typeDefinition.FullName));
    }
}