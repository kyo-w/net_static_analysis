using AsmResolver.DotNet;

namespace static_analysis.map;

public interface IConvertMap
{
    public void RegistryRecord(string moduleName, TypeDefinition typeDefinition);
}