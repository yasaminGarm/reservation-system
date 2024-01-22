using BeanSceneWebApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeanSceneWebApp.Areas.Administration.Models.Sitting
{
    public class Create
    {
        public int Id { get; set; }


        [Display(Name = "Name")]
        [Required(ErrorMessage = "Sitting Name is Required, minimum length of 2 and maximum length of 20"), MinLength(2), MaxLength(20)]
        [RegularExpression(@"^[A-Za-z-']*$")]
        public string Name { get; set; }

        [Range(1, 300)]
        [Required(ErrorMessage = "Please enter the capacity of sitting")]
        public int Capacity { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        //[Required(ErrorMessage = "Please enter the starting Date time")]
        //public DateTime StartDateTime { get; set; }  = DateTime.Today;
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy h:mm}")]
        public DateTime StartDateTime { get; set; } = DateTime.Today;

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        //[Required(ErrorMessage = "Please enter the ending Date time")]
        //public DateTime EndDateTime { get; set; } = DateTime.Today;

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy h:mm}")]
        public DateTime EndDateTime { get; set; } = DateTime.Today;

        [Range(15, 120)]
        [Required(ErrorMessage = "Please enter the capacity of sitting")]
        public int IncrementDuration { get; set; }


        [Display(Name = "Resturant")]
        public int ResturantId { get; set; }
        public SelectList? Resturants { get; set; }


        [Display(Name = "SittingType")]
        public int SittingTypeId { get; set; }
        public SelectList? SittingTypes { get; set; }

      
        public bool Closed { get; set; }

        public string Repeat { get; set; }
        public int Notice { get; set; }

        public Boolean IsUpdateSerie { get; set; }


        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }

        public int Occurance { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy 0:t}")]
        public DateTime? OccuranceEndTime { get; set; } = DateTime.Today;

        public string errorMessage { get; set; }


    }
}
