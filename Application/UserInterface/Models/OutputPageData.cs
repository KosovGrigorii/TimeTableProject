using Microsoft.AspNetCore.Mvc.Rendering;

namespace UserInterface.Models
{
    public class OutputPageData
    {
        public SelectList OutputExtensions { get; init; }
        public string UserID { get; init; }
    }
}