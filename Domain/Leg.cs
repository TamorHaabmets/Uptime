using System.Collections.Generic;

namespace Domain
{
    public class Leg
    {
        public string Id { get; set; }
        public RouteInfo RouteInfo { get; set; }
        public List<Provider> Providers { get; set; }
        public PriceList PriceList { get; set; }
    }

}
