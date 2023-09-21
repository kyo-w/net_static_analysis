namespace static_analysis.map;

public class CustomClassType
{
    public string ClassName { set; get; }
    public string ModuleName { set; get; }
    public override string ToString()
    {
        return "Assembly:\t " + ModuleName +", ClassName:\t " + ClassName;
    }
}