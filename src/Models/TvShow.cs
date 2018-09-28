using System;
using System.Collections.Generic;

namespace Models
{
    public class TvShow
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IEnumerable<Person> Casts { get; set; }

        public TvShow()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}