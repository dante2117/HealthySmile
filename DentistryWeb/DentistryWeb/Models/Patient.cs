using System;
using System.Collections.Generic;

namespace DentistryWeb.Models
{
    public partial class Patient
    {
        

        public int IdPatient { get; set; }
        public string SurnamePatient { get; set; } = null!;
        public string FirstNamePatient { get; set; } = null!;
        public string MiddlePatient { get; set; } = null!;
        public string EmailPatient { get; set; } = null!;
        public string PasswordPatient { get; set; } = null!;
        public string Sex { get; set; } = null!;
        public int RoleId { get; set; }
        public string Age { get; set; } = null!;

        
    }
}
