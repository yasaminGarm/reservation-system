using AutoMapper;
using BeanSceneWebApp.Areas.Administration.Controllers;
using BeanSceneWebApp.Data;
using BeanSceneWebApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;


namespace BeanScene.WebApp.Test
{
    public class SittingControllerTest 
    {
        //private readonly ILogger<SittingController> _logger;
        private readonly SittingServices _sittingServices;
        private readonly IMapper _mapper;

        private readonly ApplicationDbContext _dbcontext;
        private readonly SittingController _sittingController;

        public SittingControllerTest(ApplicationDbContext dbContext, IMapper mapper, SittingServices sittingServices)
        {
            _sittingServices = sittingServices;
            _mapper = mapper;

            _dbcontext = dbContext;
            _sittingController = new SittingController(dbContext, mapper, sittingServices);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var sittingType = new SittingType()
            {
                Name= "Breakfast",    
            };

            var restaurant = new Resturant()
            {
                Name = "BeanScene"
            };

            var sitting = new Sitting()
            {
                Name = "Test",
                IncrementDuration = 60,
                Capacity= 100,
                Friday = true,
                Saturday = true,
                SittingTypeId = 1,
                ResturantId= 1,
                
            };

            _dbcontext.Sittings.Add(sitting);
            _dbcontext.SaveChanges();
        }

        [Fact]
        public async Task Get_All_Sittings()
        {

            // Act
            var sittings = await _sittingController.Index();

            // Assert
            Assert.NotNull(sittings);
        }

        [Fact]
        public async Task Create_Sitting()
        {
            var sittingUI = new BeanSceneWebApp.Areas.Administration.Models.Sitting.Create()
            {
                Name = "sittingTest",
                ResturantId = 1,
                SittingTypeId  = 1,
                StartDateTime = DateTime.Now,
                EndDateTime = DateTime.Now.AddDays(30),
            };

            // Act
            var result = await _sittingController.Create(sittingUI);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Get_Sitting_By_Id()
        {
            

            // Act
            var sitting = await _sittingController.Details(1);

            // Assert
            Assert.True(sitting != null);
        }

        [Fact]
        public async Task delete_sitting()
        {

          
            // Act
            var response = await _sittingController.DeleteConfirmed(1);

            // Assert
            Assert.True(response != null);
            
        }

    }
}