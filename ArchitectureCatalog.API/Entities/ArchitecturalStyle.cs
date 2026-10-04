namespace ArchitectureCatalog.API.Entities
{
    public class ArchitecturalStyle
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public int StartYear { get; set; }
        public int EndYear { get; set; }

        public ICollection<Building> Buildings { get; set; } = new List<Building>();
    }
}