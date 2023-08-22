using System;
using System.Collections.Generic;

namespace lb4.Models
{
    public partial class Exemption
    {
        public Exemption()
        {
            Student = new HashSet<Student>();
        }

        public short ExemptionId { get; set; }
        public string Description { get; set; }
        public short Discount { get; set; }

        public virtual ICollection<Student> Student { get; set; }
    }
}
