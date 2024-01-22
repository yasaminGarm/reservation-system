using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Order.API.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Order.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MenuItemController : ControllerBase
	{
		private const string CONN_STRING = "mongodb://localhost:27017";
		private readonly MongoClient _client;
		private readonly IMongoCollection<MenuItem> _menuItemCollection;


		public MenuItemController()
		{
			_client = new MongoClient(CONN_STRING);
			_menuItemCollection = _client
				.GetDatabase("OrderApiDb")
				.GetCollection<Order.API.Data.MenuItem>("menuItems");
		}

		// GET: api/<MenuItemController>
		[HttpGet]
		public IEnumerable<MenuItem> GetMenuItems()
		{
			var emptyFilter = Builders<MenuItem>.Filter
				.Empty;

			return _menuItemCollection.Find(emptyFilter).ToList();
		}

		// GET api/<MenuItemController>/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}

		// POST api/<MenuItemController>
		[HttpPost]
		public void PostMenuItem([FromBody] MenuItem menuItem)
		{
			_menuItemCollection.InsertOne(menuItem);
		}

		// PUT api/<MenuItemController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/<MenuItemController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
