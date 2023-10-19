namespace HumanMachineInterface.App
{
    public class CatalogItemInfo
    {
        public string Name { get; }
        public CatalogItemType Type { get; }

        public CatalogItemInfo(string name, CatalogItemType type)
        {
            Name = name;
            Type = type;
        }
    }
}
