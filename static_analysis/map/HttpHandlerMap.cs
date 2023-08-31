namespace static_analysis.map;

public class HttpHandlerMap
{
    public string ModuleName { set; get; }
    public string ClassName { set; get; }

    public HttpHandlerMap(string moduleName, string className)
    {
        ModuleName = moduleName;
        ClassName = className;
    }

    public override string ToString()
    {
        return $"HttpHandler: ModuleName={ModuleName}, ClassName={ClassName}";
    }
    public string ToCsvString()
    {
        return $"{ModuleName}, {ClassName}";
    }
}