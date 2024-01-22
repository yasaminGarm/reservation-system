using MongoDB.Bson;

namespace Order.API.Data
{
	public class Table
	{
		public ObjectId Id { get; set; }
		public string Number { get; set; }
		public string Capacity { get; set; }
		public string Description { get; set; }
		public Order Order { get; set; }
	}
}
