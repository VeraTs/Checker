using System;
using System.Threading.Tasks;
using CheckerWaitersApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerWaitersApp.Views.OrdersViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderCardView : ContentView
    {
        public OrderCardView()
        {
            InitializeComponent();
        }

        
        private async void ImageButton_OnClicked(object sender, EventArgs e)
        {
            FramePartial.BackgroundColor = Color.Gray;
            await Task.Delay(20);
            FramePartial.BackgroundColor = Color.Transparent;
        }

        private async void CloseOrderImageButton_OnClicked(object sender, EventArgs e)
        {
            frameFull.BackgroundColor = Color.Gray;
            await Task.Delay(20);
            frameFull.BackgroundColor = Color.Transparent;
        }
    }
}