

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Order.API.Data
{
	public class MenuItem
	{
   
        public Guid? Id { get; set; }
        public string Name { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }
		public string? Category { get; set; }
		public string? Ingredients { get; set; }
		public string? Dietary { get; set; }

	}
}
