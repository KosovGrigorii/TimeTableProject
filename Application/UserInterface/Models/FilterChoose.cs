using Microsoft.AspNetCore.Mvc.Rendering;

namespace UserInterface.Models
{
    public class FilterChoose
    {
        public SelectList Categories { get; init; }
        public string FilterId { get; init; }
        public string UserId { get; init; }
    }
}