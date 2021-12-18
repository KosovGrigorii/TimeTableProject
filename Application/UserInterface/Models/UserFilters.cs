using System.Collections.Generic;

namespace UserInterface.Models
{
    public class UserFilters
    {
        public IEnumerable<string> Filters { get; init; }
        public string Index { get; init; }
    }
}