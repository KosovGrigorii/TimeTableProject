using System.Collections.Generic;

namespace UserInterface
{
    public record FilterGetterParameters(IEnumerable<string> Teachers, string ElementId);
}