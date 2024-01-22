using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Order.API.Data;
using static MongoDB.Driver.WriteConcern;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Order.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private const string CONN_STRING = "mongodb://localhost:27017";
		private readonly MongoClient _client;

		private readonly IMongoCollection<Order.API.Data.Order> _orderCollection;


		public OrderController()
		{
			_client = new MongoClient(CONN_STRING);
			_orderCollection = _client
				.GetDatabase("OrderApiDb")
				.GetCollection<Order.API.Data.Order>("orders");
		}

		// GET: api/<OrderController>
		//[HttpGet]
		//public Order.API.Data.Order GetOrder()
		//{
		//	return new string[] { "value1", "value2" };
		//}

		// GET api/<OrderController>/5
		[HttpGet("{id}")]
		public Order.API.Data.Order GetOrder([FromRoute] string id)
		{
			var filter = Builders<Order.API.Data.Order>.Filter.Eq(c => c.Id, new ObjectId(id));

			var result = _orderCollection.Find(filter).FirstOrDefault();
			return result;
		}

		// POST api/<OrderController>
		[HttpPost]
		public void PostOrder([FromBody] Order.API.Data.Order order)
		{
			_orderCollection.InsertOne(order);
		}

		// PUT api/<OrderController>/5
		[HttpPut("{id}")]
		public void PutOrder(string id, [FromBody] Order.API.Data.Order order)
		{
			var orderFilter = Builders<Order.API.Data.Order>.Filter.Eq(o => o.Id, new ObjectId(id));

			Order.API.Data.Order orderDb = _orderCollection.Find(orderFilter).FirstOrDefault();


			var update = Builders<Order.API.Data.Order>.Update
				.Set(order => order.OrderItems, order.OrderItems);

			_orderCollection.UpdateOne(orderFilter, update);
		}

		// DELETE api/<OrderController>/5
		[HttpDelete("{id}")]
		public void DeleteOrder(string id)
		{
			var filter = GenerateProductIdFilter(id);

			_orderCollection.DeleteOne(filter);
		}

		private static FilterDefinition<Order.API.Data.Order> GenerateProductIdFilter(string id)
		{
			return  Builders<Order.API.Data.Order>.Filter.Eq(o => o.Id, new ObjectId(id));
		}
	}
}
