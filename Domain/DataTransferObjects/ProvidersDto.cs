using System;
using System.Collections.Generic;

namespace Domain.DataTransferObjects
{
    public class ProvidersDto
    {
        public List<ProvidersDto> Children { get; set; }
        public ProvidersDto Parent { get; set; }
        public Provider Provider { get; set; }
        public float TotalPrice { get; set; }
        public long TotalDistance { get; set; }
        public TimeSpan TotalTime { get; set; }
        public DateTime LastFlightEnd { get; set; }
        public List<string> Companies { get; set; }
    }
}