using System;
using System.ComponentModel;
using CheckerWaitersApp.Enums;
using CheckerWaitersApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckerWaitersApp.Views.CreateOrderView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewOrderCardView : ContentView
    {
        private CreateOrderViewModel m_vm = new CreateOrderViewModel();
        public NewOrderCardView()
        {
            InitializeComponent();
        }
        public void SetViewModel(CreateOrderViewModel i_ViewModel)
        {
            m_vm = i_ViewModel;
            BindingContext = m_vm;
        }

        
        private eOrderType convertPickerToOrderType()
        {
            if (TypePicker.SelectedItem != null)
            {
                switch (TypePicker.SelectedItem.ToString()[0])
                {
                    case 'A':
                    {
                        return eOrderType.AllTogether;
                    }
                    case 'B':
                    {
                        return eOrderType.ByLevels;
                    }
                    default:
                    {
                        return eOrderType.Unknown;
                    }
                }
            }
            else return eOrderType.Unknown;
        }
        private void ClearButton_OnClicked(object sender, EventArgs e)
        {
            m_vm.ClearOrderCollection();
        }

        private void ClearAllOrderCollection()
        {
            m_vm.ClearOrderCollection();
        }

        private void TypePicker_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (TypePicker.SelectedItem != null)
            {
                TypeLabel.Text = TypePicker.SelectedItem.ToString();
                m_vm.PickedOrderType = convertPickerToOrderType();
            }
            else
            {
                TypeLabel.Text = "UnKnown";
            }
        }

        private void TableEntry_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TableNumLabel.Text = TableEntry.Text;
        }
    }
}