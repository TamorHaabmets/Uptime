using System.Collections.Generic;

namespace Domain
{
    public class Company
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Reservation> Reservations { get; set; }
    }

}
