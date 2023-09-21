using AsmResolver.DotNet;

namespace static_analysis.map;

public class ApiControllerMap : BaseMap
{

    public ApiControllerMap(string moduleName, string className)
    {
        ModuleName = moduleName;
        ClassName = className;
    }
    
    public override string ToString()
    {
        return $"ApiController: ModuleName={ModuleName}, ClassName={ClassName}";
    }

    public static void RegistryRecord(string moduleName, TypeDefinition typeDefinition)
    {
        MapManager.ApiControllerMaps.Add(new ApiControllerMap(moduleName, typeDefinition.FullName));
    }
}