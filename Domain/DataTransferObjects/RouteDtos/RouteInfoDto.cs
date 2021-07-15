using System;
using System.Collections.Generic;

namespace Domain.DataTransferObjects.RouteDtos
{
    public class RouteInfoDto
    {
        public string Id { get; set; }
        public float TotalPrice { get; set; }
        public long TotalDistance { get; set; }
        public TimeSpan TotalTime { get; set; }
        public DateTime TravelStart { get; set; }
        public DateTime TravelEnd { get; set; }
        public List<string> Companies { get; set; }

    }
}
