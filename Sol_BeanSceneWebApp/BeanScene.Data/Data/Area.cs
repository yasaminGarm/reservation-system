using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BeanSceneWebApp.Data
{
    public class Area
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int ResturantId { get; set; } = new();
        public List<Table> Tables { get; set; } = new();
    }
}
