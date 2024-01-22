namespace BeanSceneWebApp.Data
{
    public class Table
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public Area Area { get; set; }
        public int AreaId { get; set; } 
        public List<ReservationTable> ReservationTables { get; set; }
        public bool  Availability{ get; set; }
    }
}
