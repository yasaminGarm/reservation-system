using BeanSceneWebApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BeanSceneWebApp.Models.Reservation
{
    public class Create
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is Required, minimum length of 2 and maximum length of 20"), MinLength(2), MaxLength(20)]
        [RegularExpression(@"^[A-Za-z-']*$")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is Required, minimum length of 2 and maximum length of 20"), MinLength(2), MaxLength(20)]
        [RegularExpression(@"^[A-Za-z-']*$")]
        public string LastName { get; set; }
        
        public string Email { get; set; }
        public string? Phone { get; set; }

        [Display(Name = "Sitting")]
        public string SittingId { get; set; }
        public  SelectList? Sittings { get; set; }

        public string SelectedSittingId { get; set; }

        public DateTime SelectedStartDate { get; set; }
        public DateTime SelectedEndDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime BookingDate { get; set; } = DateTime.Today;


        //public int DurationInMinutes { get; set; }
        public int NumberOfGuests { get; set; }
        public string ? Note { get; set; }
        public string errorMessage { get; set; }
      
        public int ReservationOrigionId { get; set; }
        public SelectList? ReservationOrigins { get; set; }

    }

    public class DisplaySitting
    {
        //sitting prop to display in reservation
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string weekDay { get; set; }
        public int Capacity { get; set; }
        public bool isSelected { get; set; }
    }
}
