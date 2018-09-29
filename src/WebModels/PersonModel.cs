using System;
using System.ComponentModel.DataAnnotations;
using Kernel;
using Newtonsoft.Json;

namespace WebModels
{
    public class PersonModel
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [JsonConverter(typeof(DateConverter))]
        public DateTime Birthday { get; set; }
    }
}