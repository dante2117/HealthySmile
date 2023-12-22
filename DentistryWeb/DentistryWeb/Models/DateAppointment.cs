using System;
using System.Collections.Generic;

namespace DentistryWeb.Models
{
    public partial class DateAppointment
    {
        

        public int IdDateAppointment { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public bool Free { get; set; }

       
    }
}
