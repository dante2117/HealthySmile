using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dentistry_API.Models
{
    public partial class History
    {
        public string Жалоба { get; set; } = null!;
        public string Лечение { get; set; } = null!;
        public DateTime Дата { get; set; }
        public string Пациент { get; set; } = null!;
    }
}
