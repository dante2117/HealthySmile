using System;
using System.Collections.Generic;

namespace DentistryWeb.Models
{
    public partial class Employee
    {
       
        public int IdEmployee { get; set; }
        public string SurnameEmployee { get; set; } = null!;
        public string FirstNameEmployee { get; set; } = null!;
        public string MiddleEmployee { get; set; } = null!;
        public string EmailEmployee { get; set; } = null!;
        public string PasswordEmployee { get; set; } = null!;
        public int Experience { get; set; }
        public int RoleId { get; set; }
        public int ClinicId { get; set; }

       
    }
}
