using AutoMapper;
using BeanSceneWebApp.Areas.Administration.Controllers;
using BeanSceneWebApp.Controllers;
using BeanSceneWebApp.Data;
using BeanSceneWebApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanScene.WebApp.Test
{
    //public class ReservationControllerTest
    //{
    //    private readonly ApplicationDbContext _context;
    //    private readonly ReservationController _reservationController;

    //    private readonly PersonService _personService;
    //    //private readonly RoleManager<IdentityRole> _roleManager;
    //    //private readonly UserManager<IdentityUser> _userManager;
    //    private readonly IMapper _mapper;
    //    public ReservationControllerTest(ApplicationDbContext context, PersonService personService, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IMapper mapper)
    //    {
    //        _context = context;
    //        _personService = personService;
    //        _reservationController = new ReservationController(context, personService,null, null, mapper);
    //        _mapper = mapper;
    //        seedDatabase();
    //    }
    //    private void seedDatabase()
    //    {
    //        var ReservationOrigion = new ReservationOrigion()
    //        {
    //            Name = "Via Phone",
    //        };
    //        var status = new Status()
    //        {
    //            Name = "Confiremed",
    //        };
    //        var reservation = new Reservation()
    //        {
    //            NumberOfGuests= 1,
    //            Note="near window",
                
    //        };
    //        _context.Reservations.Add(reservation);
    //        _context.SaveChanges();



    //    }
    //    [Fact]
    //    public async Task Get_All_Reservations()
    //    {

    //        // Act
    //        var reservations = await _reservationController.Index(0,null,1);

    //        // Assert
    //        Assert.NotNull(reservations);
    //    }


    //    [Fact]
    //    public async Task Create_Reservation()
    //    {
    //        var reservation = new BeanSceneWebApp.Models.Reservation.Create()
    //        {
    //            FirstName="yasmin",
    //            LastName="Gmr",
    //            Phone="98655789",
    //            SelectedStartDate= DateTime.Now,
    //            NumberOfGuests=7,
    //        };

    //        // Act
    //        var result = await _reservationController.Create(reservation);

    //        // Assert
    //        Assert.NotNull(result);
    //    }




    //}
}
