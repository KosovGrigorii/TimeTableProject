using System;
using System.Collections.Generic;

namespace UserInterface.Models
{
    public class FilterUI
    {
        public string Name { get; set; }
        public int? DaysCount { get; set; }
        public List<int> Days { get; set; }
    }
}