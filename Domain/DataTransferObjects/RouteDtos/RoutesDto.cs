using System;
using System.Collections.Generic;

namespace Domain.DataTransferObjects.RouteDtos
{
    public class RoutesDto
    {
        public DateTime ValidUntil { get; set; }
        public List<string> Companies { get; set; }
        public List<string> TravelRoutes { get; set; }
        public List<RouteInfoDto> Providers { get; set; }
    }
}
