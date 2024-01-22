using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeanSceneWebApp.Data;
using BeanSceneWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Identity;
using AutoMapper.Execution;
using System.Configuration;
using NuGet.Protocol;
using Microsoft.AspNetCore.Http.Metadata;
using BeanSceneWebApp.Models.Reservation;
using AutoMapper;

namespace BeanSceneWebApp.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly PersonService _personService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public ReservationController(ApplicationDbContext context, PersonService personService, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager,IMapper mapper)
        {
            _context = context;
            _personService = personService;
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper= mapper;
        }

        //GET: Reservation
        //[Authorize(Roles = "Administration,Employee,Member")]

        public async Task<IActionResult> Index(int personId, string personSearch, int statusIdSearch)

        {
            //inner join reservation and perosn and sitting and status
            
            var reservations = _context.Reservations.Include(r => r.Origion).Include(r => r.Sitting).Include(r => r.Person).Include(r => r.Status);
            
            if (!String.IsNullOrEmpty(personSearch))
            {
                //when user searched person
                reservations = reservations.Where(s => s.Person.FirstName!.Contains(personSearch) || s.Person.LastName!.Contains(personSearch)).Include(r => r.Origion).Include(r => r.Sitting).Include(r => r.Person).Include(r => r.Status);
                //it filters the reservation where first name or last name of that person contains person search value.
                //The ! operator is used to suppress nullable reference warnings for the FirstName and LastName properties.
                //This means that if FirstName or LastName could be nullable, the condition will still work correctly without generating warnings about possible null values.
            }


            //when the status serached clicked
            if (statusIdSearch != 0)
            {
                reservations = reservations.Where(s => s.StatusId == statusIdSearch).Include(r => r.Origion).Include(r => r.Sitting).Include(r => r.Person).Include(r => r.Status);
            }

            if (User.IsInRole("Administration") || User.IsInRole("Employee"))
            {


            }
            else
            {
                if (personId == 0)
                {
                    //if personId is null as the value is int 0 means null,the code determine the users identity and find the person record in db base on email,if found personid updated

                    var email = User?.Identity != null ? User?.Identity.Name : string.Empty;
                    var person = _context.People.FirstOrDefault(p => p.Email == email);
                    personId = person.Id;

                }
                reservations = reservations.Where(r => r.PersonId == personId).Include(r => r.Origion).Include(r => r.Sitting).Include(r => r.Person).Include(r => r.Status);


            }

            var reservationView = new Models.Reservation.Index()
            {
                Reservations = await reservations.ToListAsync(),
                PersonSearch = personSearch,
                StatusIdSearch = statusIdSearch
            };

            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name");
            ViewData["ReservationOrigionId"] = new SelectList(_context.ReservationOrigions, "Id", "Name");

            return View(reservationView);

        }



        [HttpPost] //method annotation

        public ActionResult GetSittingsByBookingDate(string bookingDate, string? reservationId)
        {
            // string ? reservationId:creting/updating reservationseri/creating reservationId=null/updating reservationId has value
            //it will call in crete form and edit form 
            //when call in create form it will shows as drop down list
            //when it calls in edit it will shows as selected sitting in the reservation:isSelected in update
            //just get not post
           

            var reservation = new Reservation();
            if (reservationId != null)
            {
                //means updating
                reservation = _context.Reservations.FirstOrDefault(r => r.Id == int.Parse(reservationId));
            } 
            
            //object contains display sitting prop list from create reservation model
            //list drop sitting down list in reservation
            List<DisplaySitting> sittingsToDisplay = new List<DisplaySitting>();
            //jason convert c# data type to javascript datatype
            if (bookingDate is null) return Json(sittingsToDisplay);
            //return empty list

            
            //if the booking date that user choose is equal to start time and the sitting is not closed
            var sittings = _context.Sittings.Where(s => s.StartDateTime.Date == DateTime.Parse(bookingDate) && s.Closed==false).ToList();

            //when book date is choosen

            foreach (var sitting in sittings)
            {
                //will break down the sitting base on the duration increment and will show in the drop down list in the sitting
                var incrementEndTime = sitting.StartDateTime;
                while (true)
                {
                    
                    var startTime = incrementEndTime;
                    incrementEndTime = startTime.AddMinutes(sitting.IncrementDuration);

                    if (incrementEndTime.AddMinutes(sitting.IncrementDuration) > sitting.EndDateTime)
                    {
                        incrementEndTime = sitting.EndDateTime;
                    }


                    var reservationsForThisSLice =  _context.Reservations.Where(r => r.Start == startTime && r.End == incrementEndTime && r.StatusId != 5).ToList();

                    //how many booking for this slice is/sum of this reservation slice
                    int totalBookingNumbersForThisSlice = reservationsForThisSLice.Sum(x => Convert.ToInt32(x.NumberOfGuests));

                   
                    //Only add to display when there is enough capacity and also start time is in the future
                    if(totalBookingNumbersForThisSlice < sitting.Capacity && startTime > DateTime.Now.AddMinutes(sitting.Notice))
                    {
                        sittingsToDisplay.Add(new DisplaySitting()
                        {
                            Id = sitting.Id,
                            Name = sitting.Name,
                            Capacity = sitting.Capacity - totalBookingNumbersForThisSlice,
                            startTime = startTime,
                            endTime = incrementEndTime,
                            weekDay = startTime.DayOfWeek.ToString(),
                            isSelected = reservation != null && reservation.Start == startTime && reservation.End == incrementEndTime && reservation.SittingId == sitting.Id

                        });
                    }

                     

                    if(incrementEndTime >= sitting.EndDateTime)
                    {
                        break;
                    }
                }
            }

           return Json(sittingsToDisplay);

          

        }


        //// GET: Reservation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Person)
                .Include(r => r.Sitting)
                .Include(r => r.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservation/Create
        //for guest

        public IActionResult Create()
        {
            //if is member create
            var email = User?.Identity != null ? User?.Identity.Name : string.Empty;
            var person = _context.People.FirstOrDefault(p => p.Email == email);
            if (person != null && User.IsInRole("Member"))
            {
                //if the person loged in as member and wants to make a reservation
                //auto fill the information
                var m = new Models.Reservation.Create
                {
                    Email = person.Email,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Phone = person.Phone,
                    Sittings = new SelectList(_context.Sittings, "Id", "Name")
                };
                // select list is used for drop down list 
                return View(m);
            }
            else
            {
                ViewData["ReservationOrigionId"] = new SelectList(_context.ReservationOrigions, "Id", "Name");
                var m = new Models.Reservation.Create
                {
                    Sittings = new SelectList(_context.Sittings, "Id", "Name")
                };
                return View(m);
            }


        }

        // GET: Reservation/Create
        [Authorize(Roles = "Administration,Employee,Member")]
        public async Task<IActionResult> AuthorizedCreate()
        {
           

            var m = new Models.Reservation.Create
            {

                Sittings = new SelectList(_context.Sittings, "Id", "Name")
            };
           
            return View(m);
        }

        // POST: Reservation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.Reservation.Create m)
        {
            
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var user = await _userManager.FindByEmailAsync(email: m.Email);



                if (user != null && !User.Identity.IsAuthenticated)
                {
                    // Email exists, redirect to login with a message
                    TempData["errorMessage"] = "This email already exists. Please log in.";
                    return RedirectToAction(nameof(AuthorizedCreate)); // Adjust the action and controller names accordingly
                }

                
                transaction.CreateSavepoint("BeforeSavingReservation");

                int sittingId = int.Parse(m.SelectedSittingId);
                var sitting = _context.Sittings.FirstOrDefault(s => s.Id == sittingId);


                var reservationsForThisSLice = _context.Reservations.Where(r => r.Start == m.SelectedStartDate && r.End == m.SelectedEndDate).ToList();

                int totalBookingNumbersForThisSlice = reservationsForThisSLice.Sum(x => Convert.ToInt32(x.NumberOfGuests));
                //calculating the initial capacity - the total num of reservation 
                if (sitting.Capacity - totalBookingNumbersForThisSlice < m.NumberOfGuests)

                    //if (totalBookingNumbersForThisSlice > m.NumberOfGuests)
                {
                    m.errorMessage = "This sitting doesn't have enough capacity";
                    ViewData["ReservationOrigionId"] = new SelectList(_context.ReservationOrigions, "Id", "Name");
             
                    m.Sittings = new SelectList(_context.Sittings, "Id", "Name");
                    return View(m);
                }
                else
                {
                    //var person = new Person { Email = m.Email, FirstName = m.FirstName, LastName = m.LastName };
                    var person = await _personService.FindOrCreateAsync(m.Email, m.FirstName, m.LastName, m.Phone);


                    var status = new Status
                    {
                        Name = "Pending",
                        Id = 1
                    };

                    //Either auto mapper or below code to move data from View Model to Database model
                    var reservation = new Reservation
                    {
                       
                        NumberOfGuests = m.NumberOfGuests,
                        PersonId = person.Id,
                        SittingId = Int32.Parse(m.SelectedSittingId),
                        Start = m.SelectedStartDate,
                        End = m.SelectedEndDate,
                        StatusId = 1,
                       
                        ReservationOrigionId = m.ReservationOrigionId,
                        
                    };

                    if(!User.IsInRole("Administration")&& !User.IsInRole("Employee"))
                    {
                        reservation.ReservationOrigionId = 1;
                    }

                    _context.Reservations.Add(reservation);


                  

                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return RedirectToAction(nameof(Index), new { personId = person.Id });
                }
                m.Sittings = new SelectList(_context.Sittings, "Id", "Name");
                return View(m);
            }
            catch (Exception ex)
            {
                transaction.RollbackToSavepoint("BeforeSavingReservation");
                throw ex;
            }
            





        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            var reservation = _context.Reservations
                .Include(r=>r.Origion)
                .Include(r => r.ReservationTables)
                .Include(r=>r.Status)
                .FirstOrDefault(r => r.Id == id);

            var originalStatusId = reservation.StatusId;
          

            var reservationTable = _context.ReservationTables
                .Include(rt => rt.table)
                .Where(rt => rt.ReservationId == reservation.Id).ToList();

            var person = _context.People
                .Where(p => p.Id == reservation.PersonId)
                .Select(p => new
                {
                    p.Id,
                    p.FirstName,
                    p.LastName,
                    p.Email,
                    p.Phone
                }).FirstOrDefault();

            ViewBag.ReservationStatuses = new SelectList(_context.Statuses, "Id", "Name");
            ViewBag.ReservationOrigions = new SelectList(_context.ReservationOrigions, "Id", "Name");


            if (id == null)
            {
                return NotFound();
            }

            var editModel = new Models.Reservation.Edit
            {
                Id = reservation.Id,
                
                NumberOfGuests = reservation.NumberOfGuests,
               
                SittingId = reservation.SittingId,
                SelectedStartDate = reservation.Start,
                SelectedEndDate = reservation.End,
                SelectedSittingId = reservation.SittingId.ToString(),
              
                PersonId = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Email = person.Email,
                Phone = person.Phone,
                ReservationStatusId = reservation.StatusId,
                Note = reservation.Note,
                ReservationTableName = string.Join(", ", reservationTable.Select(rt => rt.table.Name)),
                ReservationOriginId=reservation.ReservationOrigionId,
                //OriginId = reservation.ReservationOrigionId



            };

            ViewData["ReservationOrigins"] = new SelectList(_context.ReservationOrigions, "Id", "Name");

            ViewData["ReservationStatuses"] = new SelectList(_context.Statuses, "Id", "Name");

            return View(editModel);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Models.Reservation.Edit editModel)
        {
            
            var reservation = _context.Reservations.FirstOrDefault(r => r.Id == id);

            //reservation.ReservationOrigionId =1 ;
            var person = _context.People.FirstOrDefault(p => p.Id == editModel.PersonId);

            var sitting = _context.Sittings.FirstOrDefault(s=>s.Id == reservation.SittingId);
            
            if (id != reservation.Id)
            {
                return NotFound();
            }

            person.FirstName = editModel.FirstName;
            person.LastName = editModel.LastName;

            _context.People.Update(person);

          
            reservation.NumberOfGuests = editModel.NumberOfGuests;
          
            reservation.SittingId = int.Parse(editModel.SelectedSittingId);
            reservation.Start = editModel.SelectedStartDate;
            reservation.End = editModel.SelectedEndDate;
       
            reservation.StatusId = editModel.ReservationStatusId;
            //reservation.ReservationOrigionId = editModel.ReservationOriginalId;
            reservation.ReservationOrigionId = editModel.ReservationOriginId;
            reservation.Note = editModel.Note;
            var originalStatusId = editModel.ReservationOriginalId;



            if (editModel.ReservationStatusId == 4 )
            {
                //when the status changed to completed
                int newSittingCapacity;
                newSittingCapacity = reservation.NumberOfGuests + sitting.Capacity;
                sitting.Capacity = newSittingCapacity;

                var reservationForCancellation = new Reservation
                {
                    Id = reservation.Id //
                };

                var result = await CancelAssignedTable(reservationForCancellation);
            }

            if (editModel.ReservationStatusId != 4 && originalStatusId ==4 )
            {
                int newSittingCapacity;
                newSittingCapacity = sitting.Capacity - reservation.NumberOfGuests ;
                sitting.Capacity = newSittingCapacity;
            }
            // Update other properties as needed

            // Save changes to the database context
            try
            {
                _context.Update(reservation);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                editModel.errorMessage = "Another process is updating this reservation. Please try again later.";

                return RedirectToAction("Index", "Reservation", new { id = editModel.Id });
            }
            
             return RedirectToAction("Index", "Reservation"); // Redirect to a page after successful edit
            

            
        }


        // GET: Reservation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Person)
                .Include(r => r.Sitting)
                .Include(r => r.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            var currentSittingCapacity = reservation.Sitting.Capacity;

            if (reservation == null)
            {
                return NotFound();
            }
            

            return View(reservation);
        }

        //// POST: Reservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reservations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reservations'  is null.");
            }
            var reservation = await _context.Reservations
                .Include(r => r.Person)
                .Include(r => r.Sitting)
                .Include(r => r.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            //var currentSittingCapacity = 
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return (_context.Reservations?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        [HttpGet, Route("/Reservation/Tables/{id:int}")]
        public async Task<IActionResult> Tables(int id)
        {
            var selectedReservation = await _context.Reservations.Include(r => r.Sitting).FirstOrDefaultAsync(r => r.Id == id);


            int sittingId = selectedReservation.SittingId;

            int reservationId = selectedReservation.Id;

            //Taken by other reservationId
            var query = from rt in _context.Tables
                        join rt2 in _context.ReservationTables on rt.Id equals rt2.TableId
                        join otherReservation in _context.Reservations on rt2.ReservationId equals otherReservation.Id
                        where
                        otherReservation.SittingId == sittingId &&
                        otherReservation.Id != reservationId &&
                        otherReservation.Start == selectedReservation.Start &&
                        otherReservation.End == selectedReservation.End &&
                        otherReservation.StatusId != 5
                        select new Models.Reservation.Tables()
                        {
                            Id = rt.Id,
                            Name = rt.Name,
                            Occupied = 0

                        };

            var occupiedTables = await query.ToListAsync();
            //Taken by current reservation Id
            var query1 = from rt in _context.Tables
                         join rt2 in _context.ReservationTables on rt.Id equals rt2.TableId
                         join r in _context.Reservations on rt2.ReservationId equals r.Id
                         where r.SittingId == sittingId && r.Id == reservationId
                         && r.StatusId != 5  //Exclude Canceled reservation when counting occupied tables
                         select new Models.Reservation.Tables()
                         {
                             Id = rt.Id,
                             Name = rt.Name,
                             Occupied = 1
                         };
            var currentReservationTable = await query1.ToListAsync();


            var query2 = from rt in _context.Tables
                         where !_context.ReservationTables.Any(rt2 => rt2.TableId == rt.Id &&
                         _context.Reservations.Any(r => r.Id == rt2.ReservationId &&
                         r.SittingId == sittingId))
                         select new Models.Reservation.Tables()
                         {
                             Id = rt.Id,
                             Name = rt.Name,
                             Occupied = 2

                         };
            var unOccupiedTables = await query2.ToListAsync();

            var combinedTables = occupiedTables
                .Concat(currentReservationTable)
                .Concat(unOccupiedTables)
                .OrderBy(table => table.Id)
                .ToList();
            /// start from here , 
            var data = new Models.Reservation.AddTable()
            {
                ReservationId = reservationId,
                SittingId = sittingId,
                Tables = combinedTables
            };
            return View(data);

   

        }

        [HttpPost, Route("/Reservation/SaveTableSelection")]
        //[Route("/Reservation/Tables/{id:int}")]\

        public async Task<IActionResult> SaveTableSelection(List<int> tableIds, int reservationId,int sittingId, List<int> selectedTables)
        {
            //Get the current table id
            var query = from rt in _context.Tables
                        join rt2 in _context.ReservationTables on rt.Id equals rt2.TableId
                        join r in _context.Reservations on rt2.ReservationId equals r.Id
                        where r.SittingId == sittingId && r.Id == reservationId
                        select rt2.Id;

            //Remove all previousTables
            List<int> previousTableIds = await query.ToListAsync();

            foreach (int tableId in previousTableIds)
            {
                var tableToBeDeleted = await _context.ReservationTables.FindAsync(tableId);
                if (tableToBeDeleted != null)
                {
                    _context.ReservationTables.Remove(tableToBeDeleted);
                    await _context.SaveChangesAsync();
                }
            }
            //Add all SelectedTables

            for (int i = 0; i < selectedTables.Count; i++)
            {
                var tableToBeAdded = new ReservationTable()
                {
                    ReservationId = reservationId,
                    TableId = selectedTables[i]
                };
                _context.ReservationTables.Add(tableToBeAdded);
                await _context.SaveChangesAsync();
            }
            var reservation = _context.Reservations.FirstOrDefault(r => r.Id == reservationId);
            if (reservation != null && selectedTables.Count() != 0)
            {
                reservation.ReservationTablesId = true;
                reservation.Status = _context.Statuses.FirstOrDefault(st => st.Id == 2);
                _context.SaveChanges(); // Save the updated Reservation entity
            }

            //return RedirectToAction("AssignTableConfirmation");
            

            return RedirectToAction("AssignTableConfirmation", "Reservation");
        }
        [HttpGet]

        public async Task<IActionResult> CancelAssignedTable(Reservation reservation)
        {
            // Retrieve the reservation tables associated with the specified reservationId
            var reservationTables = await _context.ReservationTables
                .Where(rt => rt.ReservationId == reservation.Id)
                .ToListAsync();

            var dbReservation = _context.Reservations.FirstOrDefault(r => r.Id == reservation.Id);
            if (dbReservation != null )
            {
                dbReservation.ReservationTablesId = true;
                dbReservation.Status = _context.Statuses.FirstOrDefault(st => st.Id == 1);
                _context.SaveChanges(); // Save the updated Reservation entity
            }

            // Pass the list of reservation tables to the view
            return View(reservationTables);
        }


        [HttpPost]
        public async Task<IActionResult> CancelAssignedTable(List<ReservationTable> reservationTables, Reservation reservationId)
        {
            if (reservationTables == null || reservationTables.Count == 0)
            {
                return BadRequest("Invalid request data");
            }
            // int reservationId = reservationTables[0].ReservationId;

            var reservation = _context.Reservations.FirstOrDefault(r => r.Id == reservationId.Id);

            if (reservationTables != null)
            {
                reservation.ReservationTablesId = false;
            }
            // Delete all the reservation tables passed in the request
            _context.ReservationTables.RemoveRange(reservationTables);

            await _context.SaveChangesAsync();

            // Redirect to some appropriate action or view after deletion
            return RedirectToAction("Index", "Reservation");
        }

        public IActionResult AssignTableConfirmation()
        {
            return View();
        }


        public IActionResult CreateReservationConfirmation()
        {
            return View();
        }




    }
}
