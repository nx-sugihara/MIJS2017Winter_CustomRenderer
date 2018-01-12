using System;
using Xamarin.Forms;

namespace MIJSWinter1
{
    public class TestViewModel : BaseViewModel
    {
        public ImageSource m_img { get; set; }
        public TestViewModel(ImageSource img = null)
        {
            m_img = img;
        }
    }
}
