using BeanSceneWebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BeanSceneWebApp.Services
{
    public class SittingServices
    {
        private readonly ApplicationDbContext _context;
        public SittingServices(ApplicationDbContext context) {
            _context = context;
        }

        public async Task GenerateSitting(Sitting sitting)
        {
           
        }
    }
}
