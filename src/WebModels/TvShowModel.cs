using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebModels
{
    public class TvShowModel
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<PersonModel> Cast { get; set; }
    }
}