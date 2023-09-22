using AsmResolver.DotNet;

namespace static_analysis.map;

public class BaseMap 
{
    
    public string ModuleName { set; get; }
    public string ClassName { get; set; }

    public static string GetCsvHeader()
    {
        return "Assembly, ClassName";
    }
    
    public virtual string ToCsvString()
    {
        return $"{ModuleName}, {ClassName}";
    }
}