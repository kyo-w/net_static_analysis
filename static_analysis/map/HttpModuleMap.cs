namespace static_analysis.map;

public class HttpModuleMap
{
    public string ModuleName { set; get; }
    public string ClassName { set; get; }

    public HttpModuleMap(string moduleName, string className)
    {
        ModuleName = moduleName;
        ClassName = className;
    }

    public override string ToString()
    {
        return $"HttpModule: ModuleName={ModuleName}, ClassName={ClassName}";
    }
    public string ToCsvString()
    {
        return $"{ModuleName}, {ClassName}";
    }
}