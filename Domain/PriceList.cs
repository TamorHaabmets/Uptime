using System;
using System.Collections.Generic;

namespace Domain
{
    public class PriceList
    {
        public string Id { get; set; }
        public DateTime ValidUntil { get; set; }
        public List<Leg> Legs { get; set; }
    }
}
