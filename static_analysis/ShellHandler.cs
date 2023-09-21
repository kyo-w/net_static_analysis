
namespace static_analysis;

public class ShellHandler
{
    public void Run()
    {
        while (true)
        {
            var quitCommand = ReadKeyboard();
            if (quitCommand)
            {
                break;
            }
        }
    }

    private bool ReadKeyboard()
    {
        bool quitFlag = false;
        Console.Clear();
        Console.Write(">> ");
        var command = Console.ReadLine();
        var subcommand = command.Split(" ");
        if (!string.IsNullOrEmpty(subcommand[0]))
        {
            switch (subcommand[0])
            {
                case "extend":
                    HandlerExtend(subcommand[1]);
                    break;
                case "interface":
                    HandlerInterface(subcommand[1]);
                    break;
                case "quit":
                    quitFlag = true;
                    break;
                case "help":
                    HandlerHelper();
                    break;
                default:
                    UnknownCommand();
                    break;
            }
        }

        return quitFlag;
    }

    private static void HandlerHelper()
    {
        Console.WriteLine("extend <className> :\t Print all subclasses of this class\n" +
                      "interface <interface> :\t Print all implementations of this interface\n" +
                      "quit :\t Exit program");
    }

    private static void UnknownCommand()
    {
        Console.WriteLine("Unknown command");
    }

    private static void HandlerInterface(string interfaceName)
    {
        var findClassByInterfaceName = DllAnalysts.FindClassByInterfaceName(interfaceName);
        findClassByInterfaceName.ForEach(Console.WriteLine);
    }

    private static void HandlerExtend(string className)
    {
        var findByClassName = DllAnalysts.FindClassByClassName(className);
        findByClassName.ForEach(Console.WriteLine);
    }
}