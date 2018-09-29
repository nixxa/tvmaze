using System;

namespace Models
{
    public class Person
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }

        public override int GetHashCode() => Id.GetHashCode() * 17 + Name?.GetHashCode() ?? 0;

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Person other = obj as Person;
            if (other == null) return false;
            return Id  == other.Id;
        }
    }
}