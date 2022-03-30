using Xamarin.Forms;

namespace CheckerUI.Models
{
    public class OrderButtonModel
    {
        public string m_OrderButtonID { get; set; }
        public Button m_OrderButton { get; set; }

        public OrderButtonModel()
        {
        }
        public OrderButtonModel(int i_ID, Button i_Button)
        {
            m_OrderButtonID = i_ID.ToString();
            m_OrderButton = new Button();
            m_OrderButton = i_Button;
        }
    }
}
