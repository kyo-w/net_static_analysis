namespace static_analysis.utils;

public class FileUtils
{
    /**
     * 支持分析单文件/目录/多级目录
     */
    public static string[] GetAllDllFileByDirName(string[] dirNameOrFileNames, bool iter, bool passSystemOption, bool singleAssemblyOption)
    {
        List<string> result = new List<string>();
        foreach (var dirNameOrFileName in dirNameOrFileNames)
        {
            var existsDir = Directory.Exists(dirNameOrFileName);
            var existsFile = File.Exists(dirNameOrFileName);
            // 先判断是否为目录
            if (existsDir)
            {
                string[] fileList = null;
                fileList = Directory.GetFiles(dirNameOrFileName, "*.dll", iter ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                result.AddRange(FilterPassSystemOption(fileList, passSystemOption, singleAssemblyOption));
            }
            else
            {
                if (existsFile)
                {
                    result.Add(dirNameOrFileName);
                }
                else
                {
                    Console.WriteLine($"无此文件 {dirNameOrFileName}");
                }
            }
        }

        return result.ToArray();
    }

    public static string[] FilterPassSystemOption(string[] allFileName, bool passSystemOption, bool singleAssemblyOption)
    {
        var cache = new List<string>();
        var result = new List<string>();
        foreach (var fileName in allFileName)
        {
            if (!String.IsNullOrEmpty(fileName))
            {
                var paths = fileName.Replace("\\", "/").Split("/");
                if (passSystemOption && paths[^1].StartsWith("System"))
                {
                    continue;
                }

                if (singleAssemblyOption)
                {
                    if (!cache.Contains(paths[^1]))
                    {
                        cache.Add(paths[^1]);
                        result.Add(fileName);
                    }
                }
                else
                {
                    result.Add(fileName);
                }
            }
        }
        return result.ToArray();
    }

    public static bool CreateOutputDir(string dirname)
    {
        if (!Directory.Exists(dirname))
        {
            try
            {
                Directory.CreateDirectory(dirname);
            }
            catch (Exception)
            {
                return false;
            }
        }

        return true;
    }
}