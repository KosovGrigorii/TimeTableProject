using Microsoft.AspNetCore.Mvc.Rendering;

namespace UserInterface.Models
{
    public class FilterChoose
    {
        public SelectList Categories => new SelectList(new string[]
        {
            "Working days amount",
            "Choose working days in week"
        });
        public string FilterId { get; init; }
        public string UserId { get; init; }
    }
}