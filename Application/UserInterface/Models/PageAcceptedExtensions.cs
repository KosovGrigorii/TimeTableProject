using System.Collections.Generic;

namespace UserInterface.Models
{
    public class PageAcceptedExtensions
    {
        public string Extensions { get; }

        public PageAcceptedExtensions(IEnumerable<string> extensions)
        {
            Extensions = '.' + string.Join(", .", extensions);
        } 
    }
}