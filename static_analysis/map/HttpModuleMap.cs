using AsmResolver.DotNet;

namespace static_analysis.map;

public class HttpModuleMap : BaseMap
{
    public HttpModuleMap(string moduleName, string className) 
    {
        ModuleName = moduleName;
        ClassName = className;
    }

    public static void RegistryRecord(string moduleName, TypeDefinition typeDefinition)
    {
        MapManager.HttpModuleMaps.Add(new HttpModuleMap(moduleName, typeDefinition.FullName));
    }

    public override string ToString()
    {
        return $"HttpModule: ModuleName={ModuleName}, ClassName={ClassName}";
    }
}