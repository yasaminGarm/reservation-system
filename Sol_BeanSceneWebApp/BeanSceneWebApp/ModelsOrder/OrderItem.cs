using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Order.API.Data
{
	public class OrderItem
	{
   
        public Guid? Id { get; set; }
        public MenuItem MenuItem { get; set; }
		public int Qty { get; set; }
		public string Notes { get; set; }
		public string MenuIteamStatus { get; set; }
	}
}
