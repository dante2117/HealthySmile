﻿using System;
using System.Collections.Generic;

namespace DentistryWeb.Models
{
    public partial class PriceAppointment
    {
        public DateTime Дата { get; set; }
        public string НомерЗаписи { get; set; } = null!;
        public int Цена { get; set; }
    }
}
