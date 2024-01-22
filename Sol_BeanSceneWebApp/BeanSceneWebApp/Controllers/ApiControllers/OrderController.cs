using BeanSceneWebApp.Controllers.ApiControllers;
using BeanSceneWebApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using Order.API.Data;
using System.Configuration;
using static MongoDB.Driver.WriteConcern;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Order.API.Controllers
{
    
    [Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
        private readonly IConfiguration _config;
		private  string ConnectionString; /*= "mongodb://localhost:27017";*/
		private readonly MongoClient _client;

		private readonly IMongoCollection<Order.API.Data.Order> _orderCollection;

		private readonly ApplicationDbContext _context;

		public OrderController(ApplicationDbContext context, IConfiguration configuration)
		{
            _config = configuration;
            ConnectionString = _config["ConnectionStrings:MongoDbConnection"];
            _client = new MongoClient(ConnectionString);
			_orderCollection = _client
				.GetDatabase(_config["OrderApiDb:DbName"])
				.GetCollection<Order.API.Data.Order>("orders");

			_context = context;
		}

		// GET: api/<OrderController>
		//[HttpGet]
		//public Order.API.Data.Order GetOrder()
		//{
		//	return new string[] { "value1", "value2" };
		//}

		// GET api/<OrderController>/5
		[HttpGet("{tableNumber}")]
		public Order.API.Data.Order GetOrder(string tableNumber)
		{
			var orderFilter = Builders<Order.API.Data.Order>.Filter.Eq(o => o.TableNumber, tableNumber);


			var result = _orderCollection.Find(orderFilter).FirstOrDefault() ?? new Order.API.Data.Order();

			return result;
		}

		// POST api/<OrderController>
		[HttpPost("{tableNumber}")]
		public async Task<bool> PostOrder(string tableNumber, [FromBody] Product p)
		{
			var menuItem = new Order.API.Data.MenuItem()
			{
				Id = Guid.NewGuid(),
				Name = p.Name,
				Price = p.Price,
				Description = p.Description,
				Category = p.Category.Name
			};

			var orderItem = new Order.API.Data.OrderItem()
			{
				Id = Guid.NewGuid(),
				MenuIteamStatus = "pending",
				MenuItem = menuItem,
				Notes = "",
				Qty = 1
			};

			var orderItems = new List<Order.API.Data.OrderItem>();

			orderItems.Add(orderItem);

			var orderFilter = Builders<Order.API.Data.Order>.Filter.Eq(o => o.TableNumber, tableNumber);

			var orderDb = await _orderCollection.Find(orderFilter).FirstOrDefaultAsync();



			if (orderDb is null)
			{
				var order = new Order.API.Data.Order()
				{
					OrderDateTime = DateTime.Now,
					OrderNote = "",
					OrderStatus = "Pending",
					TableNumber = tableNumber,
					Subtotal = 0,
					OrderItems = orderItems
				};

				_orderCollection.InsertOne(order);

			}
			else
			{

				var existingOrderItem = orderDb.OrderItems.Find(i => i.MenuItem.Name == menuItem.Name);

				if (existingOrderItem != null)
				{
					var newOrderItems = new List<Order.API.Data.OrderItem>();
					foreach (var oi in orderDb.OrderItems)
					{
						if (oi.MenuItem.Name == menuItem.Name)
						{
							oi.Qty = oi.Qty + 1;
						}
						newOrderItems.Add(oi);

					}


					var update = Builders<Order.API.Data.Order>.Update
					.Set(order => order.OrderItems, newOrderItems);

					_orderCollection.UpdateOne(orderFilter, update);
				}
				else
				{
					var update = Builders<Order.API.Data.Order>.Update
					.Set(order => order.OrderItems, orderDb.OrderItems.Concat(orderItems));
					//two list stick

					_orderCollection.UpdateOne(orderFilter, update);
				}



			}

            //var reservation = _context.Reservations.Where(r => r.ReservationTables.Any(t => t.table.Name == tableNumber)
            //                                                   && r.Start > DateTime.Now && r.End < DateTime.Now
            //                                                   ).FirstOrDefault();
            //var SeatedStatus = _context.Statuses.Where(s => s.Name == "Seated").FirstOrDefault();
            //reservation.Status = SeatedStatus;

            //_context.Reservations.Update(reservation);

            //_context.SaveChanges();

            return true;

		}

		// PUT api/<OrderController>/5
		[HttpPut("{tableId}")]
		public void PutOrder(string tableId, [FromBody] List<Order.API.Data.OrderItem> orderItems)
		{
			var orderFilter = Builders<Order.API.Data.Order>.Filter.Eq(o => o.TableNumber, tableId);

			Order.API.Data.Order orderDb = _orderCollection.Find(orderFilter).FirstOrDefault();


			var update = Builders<Order.API.Data.Order>.Update
				.Set(order => order.OrderItems, orderItems);

			_orderCollection.UpdateOne(orderFilter, update);
		}

		// DELETE api/<OrderController>/5
		[HttpDelete("orderItem/{tableId}/{orderItemId}")]
		public void DeleteOrder(string tableId, string orderItemId)
		{
			var orderFilter = Builders<Order.API.Data.Order>.Filter.Eq(o => o.TableNumber, tableId);
			Order.API.Data.Order orderDb = _orderCollection.Find(orderFilter).FirstOrDefault();
			var newOrderItems = orderDb.OrderItems.Where(orderItem => orderItem.Id.ToString() != orderItemId);
			var update = Builders<Order.API.Data.Order>.Update
			.Set(order => order.OrderItems, newOrderItems);

			_orderCollection.UpdateOne(orderFilter, update);
		}

		[HttpPut("updateReservationStatus/{reservationId}/{status}")]
		public async Task<bool> updateReservationStatus(int reservationId, string status)
		{
            var reservation = _context.Reservations.Where(r => r.Id == reservationId).FirstOrDefault();
            var CompletedStatus = _context.Statuses.Where(s => s.Name == status).FirstOrDefault();
            if (reservation != null)
			{
                try
				{
                    reservation.Status = CompletedStatus;

                    _context.Reservations.Update(reservation);

                    _context.SaveChanges();
                } catch (DbUpdateConcurrencyException ex)
				{
					throw ex;
				}
				

                return true;

			}
			else
			{
				return false;
			}
		
			
			//changing status to completed on press completed or seated button
		}

        [HttpGet("matchingReservation/{tableId}")]
        public async Task<ReservationModel> GetMatchingReservation(int tableId)
        {
			var tableReservations = await _context.ReservationTables.Where(rt => rt.TableId == tableId).Include(rt => rt.reservation).ToListAsync();

			var dbReservation = tableReservations.Where(rt => rt.reservation.Start < DateTime.Now && rt.reservation.End > DateTime.Now).Select(rt => rt.reservation).FirstOrDefault();

			if (dbReservation == null)
			{
				return new ReservationModel()
				{
					Id = 0,
					FirstName = "Not Recorded",
					LastName = "Not Recorded",
					Email = ""
				};
			}


            var reservation = _context.Reservations.Include(r => r.Person).Where(r => r.Id == dbReservation.Id).FirstOrDefault();

			return new ReservationModel()
			{
				Id = reservation.Id,
				FirstName = reservation.Person.FirstName,
				LastName = reservation.Person.LastName,
				Email = reservation.Person.Email
			};
			//returnig data from api to UI/taking order for x ?
        }
    }
}
