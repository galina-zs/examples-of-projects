using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models.ViewModels
{
    public class PatientEditModel
    {
        [Required]
        [MaxLength(200)]
        public String Name { get; set; }

        [Required]
        [MaxLength(200)]
        public String Address { get; set; }
        public DateTime Birthday { get; set; }
        [Required]
        [MaxLength(200)]
        public String Gender { get; set; }
    }
}
