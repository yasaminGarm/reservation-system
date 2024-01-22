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
    public class SittingReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SittingReportController(ApplicationDbContext context)
        {
            _context = context;
        }

   
        public async Task<IActionResult> Index()
        {

            return View();

        }

        // GET: Administration/SittingReport
        [HttpGet]
        public async Task<IActionResult> GetSittingReport()
        {
            var sittingReport = new List<SittingData>();

            var sittings = _context.Sittings.ToList();

            foreach (var sitting in sittings)
            {
                var  reservationForSitting = await _context.Reservations.Where(r => r.SittingId == sitting.Id).ToListAsync();

                int TotalNumberOfGuest = 0;
                foreach (var reservation in reservationForSitting)
                {
                    TotalNumberOfGuest = TotalNumberOfGuest + reservation.NumberOfGuests;
                }
                
                var sittingData = new SittingData() { 
                     SittingName= sitting.Name+" "+sitting.StartDateTime.ToString("yyyy MM dd"),
                     BookingNumber = TotalNumberOfGuest,
                     CapacityOfSitting= sitting.Capacity+ TotalNumberOfGuest,
                     RestCapacity=sitting.Capacity,
                     //SittingDate=sitting.StartDateTime

                };

                sittingReport.Add(sittingData);
            }

            return Json(sittingReport);

        }





    }
}
