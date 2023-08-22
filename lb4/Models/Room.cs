using System;
using System.Collections.Generic;

namespace lb4.Models
{
    public partial class Room
    {
        public Room()
        {
            Student = new HashSet<Student>();
        }

        public short RoomId { get; set; }
        public short? HostelId { get; set; }
        public short RoomNumber { get; set; }
        public decimal Price { get; set; }
        public short NumberOfBeds { get; set; }
        public short Flat { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        public virtual Hostel Hostel { get; set; }
        public virtual ICollection<Student> Student { get; set; }
    }
}
