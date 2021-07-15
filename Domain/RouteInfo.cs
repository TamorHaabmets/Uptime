namespace Domain
{
    public class RouteInfo
    {
        public string Id { get; set; }
        public From From { get; set; }
        public To To { get; set; }
        public long Distance { get; set; }
        public Leg Leg { get; set; }
    }

}
