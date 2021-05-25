using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models
{
    public class WardStaff
    {
        public Int32 Id { get; set; }

        public Int32 HospitalId { get; set; }

        public Hospital Hospital { get; set; }
        public Int32 WardId { get; set; }

        public Ward Ward { get; set; }

        [Required]
        [MaxLength(200)]
        public String Name { get; set; }

        [Required]
        [MaxLength(200)]
        public String Position { get; set; }
    }
}
