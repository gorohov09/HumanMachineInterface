using HumanMachineInterface.App;

namespace HumanMachineInterface.UI2
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var isConsole = false;

            if (args.Length != 0)
            {
                if (args[0] == "console")
                    isConsole = true;
            }

            if (isConsole)
            {
                // Command line given, display console
                AllocConsole();
                ConsoleMain();
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                ApplicationConfiguration.Initialize();
                Application.Run(new Form1());
            }
        }

        private static void ConsoleMain()
        {
            var service = new Catalog();


            Console.Write(service.MyPath + "> ");

            while (true)
            {
                var choice = Console.ReadLine().Trim();

                if (choice.StartsWith("pcd"))
                {
                    var listItem = service.GetAllItems();
                    Console.WriteLine();
                    foreach (var item in listItem)
                    {
                        Console.Write(item.Type == CatalogItemType.File ? "f " : "d ");
                        Console.WriteLine(item.Name);
                    }
                }
                else if (choice.StartsWith("cd"))
                {
                    var commands = choice.Split(' ');
                    if (commands.Length != 2 && commands.Length != 3)
                    {
                        Console.WriteLine("Комманда введена неверно");
                    }
                    else
                    {
                        var directoryName = commands[1];
                        bool isAbsolute = false;
                        if (commands.Length == 3)
                            isAbsolute = commands[2] == "-f" ? true : false;

                        var result = service.ChangeDirectory(directoryName, isAbsolute);

                        if (!result.Item1)
                        {
                            Console.WriteLine("Операция невыполнена");
                        }
                    }
                }
                else if (choice.StartsWith("back"))
                {
                    var commands = choice.Split(' ');
                    if (commands.Length != 1)
                    {
                        Console.WriteLine("Комманда введена неверно");
                    }
                    else
                    {
                        var result = service.ChangeDirectoryToBack();

                        if (!result.Item1)
                        {
                            Console.WriteLine("Операция невыполнена");
                        }
                    }
                }
                else if (choice.StartsWith("move"))
                {
                    var commands = choice.Split(' ');
                    if (commands.Length != 3)
                    {
                        Console.WriteLine("Комманда введена неверно");
                    }
                    else
                    {
                        var directoryName = commands[1];
                        var files = commands[2].Split("//").ToList();
                        var result = service.MoveFiles(directoryName, files);

                        if (!result.Item1)
                        {
                            Console.WriteLine("Операция невыполнена");
                        }
                    }
                }

                Console.Write(service.MyPath + "> ");
            }
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
    }
}