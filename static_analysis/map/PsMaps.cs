﻿using AsmResolver.DotNet;

namespace static_analysis.map;

public class PsMaps : BaseMap
{
    public string VerbName { set; get; }
    public string NounName { set; get; }
    public PsMaps(string moduleName, string className, string verbName, string nounName)
    {
        ModuleName = moduleName;
        ClassName = className;
        VerbName = verbName;
        NounName = nounName;
    }

    public override string ToString()
    {
        return $"PowerShell: ModuleName={ModuleName}, ClassName={ClassName}, Command={VerbName}-{NounName}";
    }
    
    public override string ToCsvString()
    {
        return $"{ModuleName}, {ClassName}, {VerbName}-{NounName}";
    }

    public new static string GetCsvHeader()
    {
        return "Assembly, ClassName, Command";
    }
    
    
    public static void RegistryRecord(string moduleName, TypeDefinition typeDefinition)
    {
        var cmdletAttribute = typeDefinition.CustomAttributes.Single(elem =>
        {
            if (elem.Constructor != null && elem.Constructor.FullName.Contains("Cmdlet"))
            {
                return true;
            }

            return false;
        });
        var verbName = cmdletAttribute.Signature?.FixedArguments[0].Element?.ToString();
        var nounName = cmdletAttribute.Signature?.FixedArguments[1].Element?.ToString();

        if (verbName != null)
            if (nounName != null)
                MapManager.PsMaps.Add(new PsMaps(moduleName, typeDefinition.FullName, verbName, nounName));
    }
}