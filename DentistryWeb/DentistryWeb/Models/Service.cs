using System;
using System.Collections.Generic;

namespace DentistryWeb.Models
{
    public partial class Service
    {
       

        public int IdService { get; set; }
        public string NameService { get; set; } = null!;
        public int PriceService { get; set; }

        
    }
}
