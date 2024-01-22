using System.ComponentModel.DataAnnotations;

namespace BeanSceneWebApp.Data
{
    public class Reservation
    {


        public int Id { get; set; }

        //[Range (15,180)]
        //public int DurationInMinutes { get; set; }

        [Required(ErrorMessage = "Please enter the number of guests attending")]
        [Range(1,40)]
        public int NumberOfGuests { get; set; }

        public int SittingId { get; set; }
        public Sitting Sitting { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public ReservationOrigion Origion { get; set; } 
        public int ReservationOrigionId { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Required(ErrorMessage = "Please enter the starting date")]
        public DateTime Start { get; set; } = DateTime.Today;



        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime End { get; set; } = DateTime.Today;


        public Person Person { get; set; }
        public int PersonId { get; set; }
        public string ? Note { get; set; }
        public int? AreaId { get; set; }
        public Area? Area { get; set; }
        public List<ReservationTable> ReservationTables { get; set; } = new();
        public bool? ReservationTablesId { get; set; }
        
    }
}
