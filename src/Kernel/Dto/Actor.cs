using System;

namespace Kernel.Dto
{
    public class Actor
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
    }
}