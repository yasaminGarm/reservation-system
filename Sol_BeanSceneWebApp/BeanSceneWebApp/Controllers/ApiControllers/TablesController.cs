using BeanSceneWebApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public TablesController(ApplicationDbContext context) {
            _context = context;
        }
        // GET: api/<TablesController>
        [HttpGet]
        public async Task <IEnumerable <Table>> Get()
        {
            var Tables =  await _context.Tables.ToListAsync();

            return Tables;
        }

        // GET api/<TablesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        
    }
}
