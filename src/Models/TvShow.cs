using System;
using System.Collections.Generic;

namespace Models
{
    public class TvShow
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Person> Casts { get; set; }

        public TvShow()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        public override int GetHashCode() => Id.GetHashCode() * 17 + Name?.GetHashCode() ?? 0;

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            TvShow other = obj as TvShow;
            if (other == null) return false;
            return Id  == other.Id;
        }
    }
}