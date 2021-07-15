using System;
using System.Collections.Generic;

namespace Domain
{
    public class Reservation
    {
        public string Id{ get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Destination> TravelRoute { get; set; }
        public List<Company> Companies { get; set; }
        public float TotalPrice { get; set; }
        public long TotalTravelTime { get; set; }

    }
}
