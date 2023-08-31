namespace static_analysis.map;

public class ApiControllerMap
{
    public string ModuleName { set; get; }
    public string ClassName { get; set; }

    public ApiControllerMap(string moduleName, string className)
    {
        ModuleName = moduleName;
        ClassName = className;
    }

    public override string ToString()
    {
        return $"ApiController: ModuleName={ModuleName}, ClassName={ClassName}";
    }

    public string ToCsvString()
    {
        return $"{ModuleName}, {ClassName}";
    }
}