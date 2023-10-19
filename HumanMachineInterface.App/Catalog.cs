using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HumanMachineInterface.App
{
    public class Catalog
    {
        public DirectoryInfo _currentDirectory;
        private string _path;

        public Catalog()
        {
            _path = "D:\\";
            _currentDirectory = new DirectoryInfo(_path);
        }

        public string MyPath => _path;

        public List<CatalogItemInfo> GetAllItems()
        {
            var result = new List<CatalogItemInfo>();
            foreach (var item in _currentDirectory.GetDirectories())
                result.Add(new CatalogItemInfo(item.Name, CatalogItemType.Directory));

            foreach (var item in _currentDirectory.GetFiles())
                result.Add(new CatalogItemInfo(item.Name, CatalogItemType.File));

            return result;
        }

        public (bool, string) ChangeDirectory(string path, bool isAbsolute)
        {
            if (isAbsolute)
            {
                _currentDirectory = new DirectoryInfo(path);
                if (_currentDirectory.Exists)
                {
                    _path = path;
                    return (true, _currentDirectory.FullName);
                }
                else
                {
                    _currentDirectory = new DirectoryInfo(_path);
                    return (false, "Каталог по данному пути не существует");
                }
            }
            else
            {
                var catalogs = path.Split("\\");
                var tempPath = _path;
                foreach (var catalog in catalogs)
                {
                    _currentDirectory = new DirectoryInfo(Path.Combine(tempPath, catalog));
                    if (_currentDirectory.Exists)
                    {
                        tempPath = _currentDirectory.FullName;
                    }
                    else
                    {
                        _currentDirectory = new DirectoryInfo(_path);
                        return (false, "Каталог по данному пути не существует");
                    }
                }

                _path = tempPath;
                return (true, _currentDirectory.FullName);
            }
        }

        public (bool, string) ChangeDirectoryToBack()
        {
            var _parentDirectory = _currentDirectory.Parent;

            if (_parentDirectory is not null && _parentDirectory.Exists)
            {
                _path = _parentDirectory.FullName;
                _currentDirectory = _parentDirectory;
                return (true, _currentDirectory.FullName);
            }
            else
            {
                return (false, "У каталога нет родительского каталога");
            }
        }

        public (bool, string) MoveFiles(string newPath, List<string> files)
        {
            var filesInfo = new List<FileInfo>();
            var currentFiles = _currentDirectory.GetFiles().Select(f => f.Name).ToList();

            //Проверяем, существуют ли файлы, введеные юзером в этой папке
            //Если хотя бы один файл не существует, говорим об этом
            foreach (var name in files)
            {
                if (!currentFiles.Contains(name))
                    return (false, $"Файл {name} не содержится в текущей директории");
            }

            //Добавляем файлы, которые нужно переместить в список
            foreach (var file in _currentDirectory.GetFiles())
            {
                if (files.Contains(file.Name))
                    filesInfo.Add(file);
            }

            //Проверяем существует ли новая директория
            var newDirectory = new DirectoryInfo(newPath);
            if (!newDirectory.Exists)
                return (false, "Новая директория не существует");

            //Перемещаем файлы
            foreach (var item in filesInfo)
            {
                item.MoveTo(Path.Combine(newPath.ToString(), Path.GetFileName(item.ToString())));
            }
                

            return (true, _currentDirectory.FullName);
        }
    }
}
