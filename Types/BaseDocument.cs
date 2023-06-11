using System;

namespace Organizer.Types
{
    public class BaseDocument
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Enums.Type Type { get; set; }
        public Guid? Signature { get; set; }
        public Enum? State { get; set; }
    }
}
