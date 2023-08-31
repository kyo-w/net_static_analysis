namespace static_analysis.map;

public class ControllerMap
{
    public string ModuleName { get; set; }
    public string ClassName { set; get; }

    public ControllerMap(string moduleName, string className)
    {
        ModuleName = moduleName;
        ClassName = className;
    }

    public override string ToString()
    {
        return $"Controller: ModuleName={ModuleName}, ClassName={ClassName}";
    }
    public string ToCsvString()
    {
        return $"{ModuleName}, {ClassName}";
    }
}