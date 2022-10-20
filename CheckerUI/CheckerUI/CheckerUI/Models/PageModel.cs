using Xamarin.Forms;

namespace CheckerUI.Models
{
    public class PageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BackgroundImageURL { get; set; }

        public Command NextPageCommand { get; set; }
    }
}
