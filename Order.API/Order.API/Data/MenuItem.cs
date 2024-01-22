

using MongoDB.Bson;

namespace Order.API.Data
{
	public class MenuItem
	{
		public ObjectId Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Price { get; set; }
		public string Category { get; set; }
		public string[] Ingredients { get; set; }
		public string[] Dietary { get; set; }

	}
}
