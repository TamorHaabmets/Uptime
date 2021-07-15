using System.Collections.Generic;

namespace Domain.DataTransferObjects.ReservationDtos
{
    public class BaseReservationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> TravelRoute { get; set; }
        public List<string> Companies { get; set; }
        public float TotalPrice { get; set; }
    }
}