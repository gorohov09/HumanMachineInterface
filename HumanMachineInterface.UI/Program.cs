using HumanMachineInterface.App;
using System;
using System.Linq;

namespace HumanMachineInterface.UI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var catalog = new Catalog();


            Console.Write(catalog.MyPath + "> ");

            while (true)
            {
                var choice = Console.ReadLine().Trim();
                
                if (choice.StartsWith("pcd"))
                {
                    var listItem = catalog.GetAllItems();
                    Console.WriteLine();
                    foreach ( var item in listItem )
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

                        var result = catalog.ChangeDirectory(directoryName, isAbsolute);

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
                        var result = catalog.ChangeDirectoryToBack();

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
                        var result = catalog.MoveFiles(directoryName, files);

                        if (!result.Item1)
                        {
                            Console.WriteLine("Операция невыполнена");
                        }
                    }
                }

                Console.Write(catalog.MyPath + "> ");
            }
        }
    }
}
