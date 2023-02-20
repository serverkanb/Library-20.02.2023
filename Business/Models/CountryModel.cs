﻿

using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class CountryModel : RecordBase
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
