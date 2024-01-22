using MongoDB.Bson;

namespace Order.API.Data
{
	public class OrderItem
	{
		public ObjectId Id { get; set; }
		public MenuItem MenuItem { get; set; }
		public string Qty { get; set; }
		public string Notes { get; set; }
		public string MenuIteamStatus { get; set; }
	}
}
