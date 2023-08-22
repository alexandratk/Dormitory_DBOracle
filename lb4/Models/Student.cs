using System;
using System.Collections.Generic;

namespace lb4.Models
{
    public partial class Student
    {
        public short StudentId { get; set; }
        public short? RoomId { get; set; }
        public short? ExemptionId { get; set; }
        public string StudentName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PassportId { get; set; }
        public DateTime CheckInDate { get; set; }

        public virtual Exemption Exemption { get; set; }
        public virtual Room Room { get; set; }
    }
}
