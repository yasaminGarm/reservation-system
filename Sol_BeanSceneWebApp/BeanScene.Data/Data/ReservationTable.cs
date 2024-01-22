using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BeanSceneWebApp.Data
{
    public class ReservationTable
    {


        public int Id { get; set; }

       
        public int? TableId { get; set; }
        public Table table { get; set; }

        public int  ReservationId { get; set; }
        public Reservation reservation { get; set; }

     
    }
}
