using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BeanSceneWebApp.Models.Reservation
{
    public class Edit
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }

        public int ReservationStatusId { get; set; }
        public int ReservationOriginalId { get; set; }
        [Display(Name = "Sitting")]
        public int SittingId { get; set; }
        public SelectList? Sittings { get; set; }

        public string SelectedSittingId { get; set; }

        public DateTime SelectedStartDate { get; set; }
        public DateTime SelectedEndDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime BookingDate { get; set; } = DateTime.Today;

        public SelectList? ReservationStatuses { get; set; }

        public SelectList? ReservationOrigins { get; set; }

        public int ReservationOriginId { get; set; }
     
        public int NumberOfGuests { get; set; }
        public string? Note { get; set; }

        public string ReservationTableName { get; set; }
        public string errorMessage { get; set; }


    }
}
