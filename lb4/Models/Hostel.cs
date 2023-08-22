using System;
using System.Collections.Generic;

namespace lb4.Models
{
    public partial class Hostel
    {
        public Hostel()
        {
            Room = new HashSet<Room>();
        }

        public short HostelId { get; set; }
        public short HostelNumber { get; set; }
        public string University { get; set; }
        public string Manager { get; set; }

        public virtual ICollection<Room> Room { get; set; }
    }
}
