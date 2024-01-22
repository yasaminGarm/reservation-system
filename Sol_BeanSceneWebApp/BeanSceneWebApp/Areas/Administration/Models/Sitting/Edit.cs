
using BeanSceneWebApp.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeanSceneWebApp.Areas.Administration.Models.Sitting
{
    public class Edit
    {


        public int Id { get; set; }
        public string Name { get; set; }

        [Range(1, 300)]
        [Required(ErrorMessage = "Please enter the capacity of sitting")]
        public int Capacity { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy h:mm tt}")]
        [Required(ErrorMessage = "Please enter the starting Date time")]
        public DateTime StartDateTime { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy h:mm tt}")]
        [Required(ErrorMessage = "Please enter the ending Date time")]
        public DateTime EndDateTime { get; set; }

        [Range(15, 120)]
        [Required(ErrorMessage = "Please a duration to break down the series for this sitting")]
        public int IncrementDuration { get; set; }

        public int ResturantId { get; set; } = new();
        public Resturant Resturant { get; set; }

        public Boolean Closed { get; set; }

        public int SittingTypeId { get; set; }
        public SittingType SittingType { get; set; }

   

        //public int SittingScheduleId { get; set; }
        //public SittingSchedule SittingSchedule { get; set; }

        public Guid? SeriesId { get; set; }
        public string Repeat { get; set; }
        public int Notice { get; set; }

        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }

        public Boolean IsUpdateSerie { get; set; }

        public string errorMessage { get; set; }
    }
}
