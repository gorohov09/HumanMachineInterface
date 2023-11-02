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
            var isConsoleMenu = true;

            if (args.Length != 0)
            {
                if (args[0] == "console")
                    isConsole = true;
                if (args[0] == "console_menu")
                    isConsoleMenu = true;
            }

            if (isConsole)
            {
                // Command line given, display console
                AllocConsole();
                ConsoleMain();
            }
            else if (isConsoleMenu)
            {
                // Command line given, display console
                AllocConsole();
                ConsoleMenuMain();
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
                            Console.WriteLine(result.Item2);
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
                            Console.WriteLine(result.Item2);
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
                            Console.WriteLine(result.Item2);
                        }
                    }
                }

                Console.Write(service.MyPath + "> ");
            }
        }

        private static void ConsoleMenuMain()
        {
            var service = new Catalog();

            while (true)
            {
                Console.WriteLine($"Каталог: {service.MyPath}\n");

                Console.WriteLine("1. Вывод содержимого текущего каталога");
                Console.WriteLine("2. Смена каталога");
                Console.WriteLine("3. Вернуться к родительскому каталогу");
                Console.WriteLine("4. Переместить группу файлов");
                Console.WriteLine("5. Завершение работы\n");

                Console.Write("Введите номер команды: ");

                var choice = Console.ReadLine();

                if (choice == "1")
                {
                    var listItem = service.GetAllItems();
                    Console.WriteLine();

                    if (listItem.Count == 0)
                    {
                        Console.WriteLine("Текущая директория пустая\n");
                        continue;
                    }
                    foreach (var item in listItem)
                    {
                        Console.Write(item.Type == CatalogItemType.File ? "f " : "d ");
                        Console.WriteLine(item.Name);
                    }
                }
                else if (choice == "2")
                {
                    Console.Write("Введите абсолютный адрес нового каталога: ");
                    var directoryName = Console.ReadLine();

                    var result = service.ChangeDirectory(directoryName, true);

                    if (!result.Item1)
                    {
                        Console.WriteLine($"(!)Ошибка: {result.Item2}\n");
                    }
                }
                else if (choice == "3")
                {
                    var result = service.ChangeDirectoryToBack();

                    if (!result.Item1)
                    {
                        Console.WriteLine($"(!)Ошибка: {result.Item2}\n");
                    }
                }
                else if (choice == "4")
                {
                    var filesDirectory = service.GetAllItems().Where(i => i.Type == CatalogItemType.File).ToList();

                    if (filesDirectory.Count == 0)
                    {
                        Console.WriteLine("В текущей директории отсутствуют файлы\n");
                        continue;
                    }

                    int i = 1;
                    foreach (var file in filesDirectory)
                    {
                        Console.WriteLine($"{i} - {file.Name}");
                        i++;
                    }
                    Console.WriteLine();

                    Console.Write("Введите номера файлов через пробел, которые хотите переместить: ");
                    var str = Console.ReadLine();

                    if (string.IsNullOrEmpty(str))
                    {
                        Console.WriteLine("Нужно выбрать файлы\n");
                        continue;
                    }

                    var filesId = str.Split(' ').Select(x => int.Parse(x)).ToList();
                    var files = new List<string>();
                    foreach (var index in filesId)
                    {
                        files.Add(filesDirectory[index - 1].Name);
                    }

                    Console.Write("Введите абсолютный адрес нового каталога: ");
                    var directoryName = Console.ReadLine();

                    var result = service.MoveFiles(directoryName, files);

                    if (!result.Item1)
                    {
                        Console.WriteLine($"(!)Ошибка: {result.Item2}\n");
                    }
                }
                else if (choice == "5")
                {
                    return;
                }
            }
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
    }
}