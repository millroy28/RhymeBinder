using System.Collections.Generic;

namespace RhymeBinder.Models
{
    public class DisplaySimpleUser : SimpleUser
    {
        public List<TimeZone> TimeZones { get; set; }
    }
}
