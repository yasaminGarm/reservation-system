using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeanSceneWebApp.Areas.Administration.Models.Report;
using BeanSceneWebApp.Data;

namespace BeanSceneWebApp.Areas.Administration.Controllers
{

    [Area("Administration")]
    public class StatusReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatusReportController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {

            return View();

        }





        // GET: Administration/StatusReport
        [HttpGet]
        public async Task<IActionResult> GetStatusReport()
        {
            var StatusReport = new List<StatusData>();

            var statuses = _context.Statuses.ToList();

            foreach (var status in statuses)
            {
                var reservationForStatus = await _context.Reservations.Where(r => r.StatusId == status.Id).ToListAsync();

                int TotalNumberOfGuest = 0;
                foreach (var reservation in reservationForStatus)
                {
                    TotalNumberOfGuest = TotalNumberOfGuest + reservation.NumberOfGuests;
                }



                var statusData = new StatusData()
                {
                    StatusName = status.Name,
                       BookingNumber = TotalNumberOfGuest,

                };

                StatusReport.Add(statusData);
            }

            return Json(StatusReport);

        }




    }
}
