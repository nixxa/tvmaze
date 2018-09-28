using System;
using System.ComponentModel.DataAnnotations;

namespace WebModels
{
    public class PersonModel
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
    }
}