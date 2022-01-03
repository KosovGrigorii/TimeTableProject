using System;
using System.Collections.Generic;

namespace UserInterface
{
    public class FilterUI
    {
        public string Name { get; init; }
        public int? DaysCount { get; init; }
        public List<int> Days { get; init; }
    }
}