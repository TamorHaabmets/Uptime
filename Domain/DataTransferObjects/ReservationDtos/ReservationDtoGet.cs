using System;

namespace Domain.DataTransferObjects.ReservationDtos
{
    public class ReservationDtoGet : BaseReservationDto
    {
        public string Id { get; set; }
        public TimeSpan TotalTravelTime { get; set; }
    }
}
