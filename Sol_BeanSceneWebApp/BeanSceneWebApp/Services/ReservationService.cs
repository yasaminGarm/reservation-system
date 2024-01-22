using BeanSceneWebApp.Data;

namespace BeanSceneWebApp.Services
{
    public class ReservationService
    {
        private readonly ApplicationDbContext _context;
        public ReservationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void UpdateReservationStatus(int reservationId, Status reservationStatus)
        {
            var dbReservation = _context.Reservations.Where(r => r.Id == reservationId).FirstOrDefault();
            
            dbReservation.Status = reservationStatus;
            
            _context.Reservations.Update(dbReservation);

            _context.SaveChanges();
        }
    }
}
