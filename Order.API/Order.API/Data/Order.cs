using MongoDB.Bson;
using System.Runtime.Serialization;

namespace Order.API.Data
{
	public class Order
	{
		
		public ObjectId Id { get; set; }
		//public string OrderId { get; set; }
		public DateTime OrderDateTime { get; set; }
		public string TableNumber { get; set; }
		public string OrderStatus { get; set; }
		public string OrderNote { get; set; }
		public List<OrderItem> OrderItems { get; set; }
		public double Subtotal { get; set; }
	}
}
