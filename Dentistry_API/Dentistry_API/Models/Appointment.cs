using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dentistry_API.Models
{

    public partial class Appointment
    {

        public int? IdAppointment { get; set; }
        public string Number { get; set; } = null!;
        public string Complaint { get; set; } = null!;
        public string Treatment { get; set; } = null!;
        public int EmployeeId { get; set; }
        public int PatientId { get; set; }
        public int ServiceId { get; set; }
        public int DateAppointmentId { get; set; }

       
    }
}
