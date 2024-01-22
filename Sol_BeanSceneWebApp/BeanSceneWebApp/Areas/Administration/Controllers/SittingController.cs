using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeanSceneWebApp.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using BeanSceneWebApp.Areas.Administration.Models.Report;
using Microsoft.IdentityModel.Tokens;
using BeanSceneWebApp.Services;

namespace BeanSceneWebApp.Areas.Administration.Controllers
{
    public class SittingController : AdministrationBaseController
    {
        private readonly IMapper _mapper;
        private readonly SittingServices _SittingServices;

        public SittingController(ApplicationDbContext context, IMapper mapper, SittingServices sittingServices) : base(context)
        {
            _mapper = mapper;
            _SittingServices = sittingServices;
        }

        // GET: Administration/Sitting
        public async Task<IActionResult> Index()
        {
            var sittings = _context.Sittings.Include(s => s.Resturant).Include(s => s.SittingType);
          
        
            return View(await sittings.ToListAsync());
          
        }

        public async Task<IActionResult> SittingCalendar()
        {
           

            ViewData["ResturantId"] = new SelectList(_context.Resturants, "Id", "Name");

            ViewData["SittingTypeId"] = new SelectList(_context.SittingTypes, "Id", "Name");

            var m = new Administration.Models.Sitting.Create();

            

            return View("SittingsCalendar", m);
            
        }

        [HttpGet]
        public async Task<IActionResult> GetSittings()
        {
            var sittings = await _context.Sittings.ToListAsync();
            

            return Json(sittings);
          
        }
        // GET: Administration/Sitting/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sittings == null)
            {
                return NotFound();
            }

            var sitting = await _context.Sittings
                .Include(s => s.Resturant)
         
                .Include(s => s.SittingType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sitting == null)
            {
                return NotFound();
            }

            return View(sitting);
        }

        // GET: Administration/Sitting/Create
        public IActionResult Create()
        {
            ViewData["ResturantId"] = new SelectList(_context.Resturants, "Id", "Name");
      
            ViewData["SittingTypeId"] = new SelectList(_context.SittingTypes, "Id", "Name");

            var m = new Administration.Models.Sitting.Create();
            
            return View(m);

         
        }

        // POST: Administration/Sitting/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BeanSceneWebApp.Areas.Administration.Models.Sitting.Create sittingUI)
        {
            if (sittingUI.StartDateTime> sittingUI.EndDateTime)
            {

                ViewData["ResturantId"] = new SelectList(_context.Resturants, "Id", "Name");

                ViewData["SittingTypeId"] = new SelectList(_context.SittingTypes, "Id", "Name");
                sittingUI.errorMessage = "Please enter valid date ,date time start can not be after end time!";
                return View(sittingUI);

            }

            if(sittingUI.Repeat == "NEVER")
            {
                var sitting = _mapper.Map<Data.Sitting>(sittingUI);
                _context.Sittings.Add(sitting);
            } 
            else if(sittingUI.Repeat == "DAILY")
            {
                var serieId = Guid.NewGuid();
                for(int i = 0; i < sittingUI.Occurance; i++)
                {
                    var newSitting = new Data.Sitting()
                    {
                        Name= sittingUI.Name,
                        Capacity= sittingUI.Capacity,
                        SittingTypeId= sittingUI.SittingTypeId,
                        ResturantId= sittingUI.ResturantId,
                        IncrementDuration= sittingUI.IncrementDuration,
                        Notice = sittingUI.Notice,
                        StartDateTime = sittingUI.StartDateTime.AddDays(i),
                        EndDateTime = sittingUI.EndDateTime.AddDays(i),
                        SeriesId = serieId,
                        Repeat = sittingUI.Repeat,
                     

                    };

                    _context.Sittings.Add(newSitting);
                }
            } 

            

            else if (sittingUI.Repeat == "WEEKLY")
            {

                var serieId = Guid.NewGuid();

                for (int i = 0; i < sittingUI.Occurance*7; i++)
                {
                    var newSitting = new Data.Sitting()
                    {
                        Name = sittingUI.Name,
                        Capacity = sittingUI.Capacity,
                        SittingTypeId = sittingUI.SittingTypeId,
                        ResturantId = sittingUI.ResturantId,
                        IncrementDuration = sittingUI.IncrementDuration,
                        Notice = sittingUI.Notice,
                        StartDateTime = sittingUI.StartDateTime.AddDays(i),
                        EndDateTime = sittingUI.EndDateTime.AddDays(i),
                        SeriesId = serieId,
                        Repeat = sittingUI.Repeat,
                        Monday = sittingUI.Monday,
                        Tuesday = sittingUI.Tuesday,
                        Wednesday = sittingUI.Wednesday,
                        Thursday = sittingUI.Thursday,
                        Friday = sittingUI.Friday,
                        Saturday = sittingUI.Saturday,
                        Sunday = sittingUI.Sunday,

                    };

                  

                    if (newSitting.StartDateTime.DayOfWeek == DayOfWeek.Monday && sittingUI.Monday == true)
                    {

                        _context.Add(newSitting);

                    }
                    else if (newSitting.StartDateTime.DayOfWeek == DayOfWeek.Tuesday && sittingUI.Tuesday == true)
                    {

                        _context.Add(newSitting);
                    }

                    else if (newSitting.StartDateTime.DayOfWeek == DayOfWeek.Wednesday && sittingUI.Wednesday == true)
                    {

                        _context.Add(newSitting);
                    }
                    else if (newSitting.StartDateTime.DayOfWeek == DayOfWeek.Thursday && sittingUI.Thursday == true)
                    {

                        _context.Add(newSitting);
                    }
                    else if (newSitting.StartDateTime.DayOfWeek == DayOfWeek.Friday && sittingUI.Friday == true)
                    {

                        _context.Add(newSitting);
                    }
                    else if (newSitting.StartDateTime.DayOfWeek == DayOfWeek.Saturday && sittingUI.Saturday == true)
                    {

                        _context.Add(newSitting);
                    }
                    else if (newSitting.StartDateTime.DayOfWeek == DayOfWeek.Sunday && sittingUI.Sunday == true)
                    {

                        _context.Add(newSitting);
                    }

                }

            }


            await _context.SaveChangesAsync();
        

            return RedirectToAction(nameof(Index));
          
        }

        // GET: Administration/Sitting/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sittings == null)
            {
                return NotFound();
            }

            var sitting = await _context.Sittings.FindAsync(id);

         

            if (sitting == null)
            {
                return NotFound();
            }
            ViewData["ResturantId"] = new SelectList(_context.Resturants, "Id", "Name", sitting.ResturantId);
       
            ViewData["SittingTypeId"] = new SelectList(_context.SittingTypes, "Id", "Name", sitting.SittingTypeId);

            var sittingToEdit = _mapper.Map<BeanSceneWebApp.Areas.Administration.Models.Sitting.Edit>(sitting);

            return View(sittingToEdit);




        }

        // POST: Administration/Sitting/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BeanSceneWebApp.Areas.Administration.Models.Sitting.Edit sitting)
        {

            if (id != sitting.Id)
            {
                return NotFound();
            }

          


            var dbSitting = _context.Sittings.FirstOrDefault(x => x.Id == id);
            //if there is any difference or if user changed the start date time the we will have the difference in min and we will apply this delta for whole series

            var startTimeDifference = (sitting.StartDateTime - dbSitting.StartDateTime).Minutes;
            var endTimeDiffernce = (sitting.EndDateTime - dbSitting.EndDateTime).Minutes;


            //if user click the update series(bool/true/ not mapped in db)
            if (sitting.IsUpdateSerie)
            {
                 var allSeriesSittings = _context.Sittings.Where(s => s.SeriesId == sitting.SeriesId).Include(s => s.Reservations).ToList();
                //if there is any reservation assign to that sitting
                var hasReservation = allSeriesSittings.Any(s => s.Reservations.Count > 0);
                //show the error message
                if (hasReservation)
                {
                    ViewData["ResturantId"] = new SelectList(_context.Resturants, "Id", "Name", sitting.ResturantId);

                    ViewData["SittingTypeId"] = new SelectList(_context.SittingTypes, "Id", "Name", sitting.SittingTypeId);

                    sitting.errorMessage = "Cannot Edit. There is a reservation assigned to one of the sitting in this serie.";
                    return View(sitting);
                }

                foreach ( var seri in allSeriesSittings) 
                {
                    seri.Name = sitting.Name;
                    seri.Capacity = sitting.Capacity;
                    seri.StartDateTime = seri.StartDateTime.AddMinutes(startTimeDifference);
                    seri.EndDateTime = seri.EndDateTime.AddMinutes(endTimeDiffernce);
                    seri.IncrementDuration = sitting.IncrementDuration;
                    seri.ResturantId = sitting.ResturantId;
                    seri.SittingTypeId = sitting.SittingTypeId;
                    seri.Notice = sitting.Notice;

                    _context.Sittings.Update(seri);
                    

                }
                _context.SaveChanges();

            }
            else
            {
                //when the user just wants to update the single sitting not whole.update series=false
                var reservation = _context.Reservations.Any(r => r.SittingId == sitting.Id);
                //if there is any reservation can not edit
                if(reservation)
                {
                    ViewData["ResturantId"] = new SelectList(_context.Resturants, "Id", "Name", sitting.ResturantId);

                    ViewData["SittingTypeId"] = new SelectList(_context.SittingTypes, "Id", "Name", sitting.SittingTypeId);

                    sitting.errorMessage = "Cannot Edit. There is a reservation assigned to this sitting.";
                    return View(sitting);
                }
                dbSitting.Name = sitting.Name;
                dbSitting.Capacity = sitting.Capacity;
                dbSitting.StartDateTime = sitting.StartDateTime;
                dbSitting.EndDateTime = sitting.EndDateTime;
                dbSitting.IncrementDuration = sitting.IncrementDuration;
                dbSitting.ResturantId = sitting.ResturantId;
                dbSitting.SittingTypeId = sitting.SittingTypeId;
                dbSitting.Notice = sitting.Notice;
                dbSitting.SeriesId = null;

                _context.Sittings.Update(dbSitting);
                _context.SaveChanges();
            }

            //id sittings==id parameter the one which clicked
           
            return RedirectToAction(nameof(Index));




        }

        // GET: Administration/Sitting/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sittings == null)
            {
                return NotFound();
            }

            var sitting = await _context.Sittings
                .Include(s => s.Resturant)
               
                .Include(s => s.SittingType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sitting == null)
            {
                return NotFound();
            }

            return View(sitting);
        }

        // POST: Administration/Sitting/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sittings == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Sittings'  is null.");
            }
            var sitting = _context.Sittings.Where(s => s.Id == id).Include(s => s.Reservations).FirstOrDefault();
            if (!sitting.Reservations.IsNullOrEmpty())
            {
                //if sitting is not null or if there is any reservation
                TempData["alertMessage"] = "Cannot delete. There are sittings with reservation link to this schedule";
                return RedirectToAction(nameof(Delete));
            }
           
            if (sitting != null)
            {
                _context.Sittings.Remove(sitting);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SittingExists(int id)
        {
            return (_context.Sittings?.Any(e => e.Id == id)).GetValueOrDefault();
        }


    }
}
