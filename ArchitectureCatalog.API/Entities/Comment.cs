namespace ArchitectureCatalog.API.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public int BuildingId { get; set; }

        public Building Building { get; set; } = null!;
    }
}