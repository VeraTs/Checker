using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerUI.Views.PopupViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupPageNewOrder : Popup
    {
        public PopupPageNewOrder()
        {
            InitializeComponent();
            

        }

        private void newOrderLottie_OnOnFinishedAnimation(object sender, EventArgs e)
        {
            this.Dismiss(null);
        }
    }
}