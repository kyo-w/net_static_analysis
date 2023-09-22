using AsmResolver.DotNet;

namespace static_analysis.map;

public class ControllerMap : BaseMap
{
    private ControllerMap(string moduleName, string className)
    {
        ModuleName = moduleName;
        ClassName = className;
    }

    
    
    public override string ToString()
    {
        return $"Controller: ModuleName={ModuleName}, ClassName={ClassName}";
    }

    public static void RegistryRecord(string moduleName, TypeDefinition typeDefinition)
    {
        MapManager.ControllerMaps.Add(new ControllerMap(moduleName, typeDefinition.FullName));
    }
}