using System;
using Xamarin.Forms;
namespace MIJSWinter1
{
    public class CustomCamera : View
    {
        public static readonly BindableProperty FrameRateProperty = BindableProperty.Create("FrameRate",
                                                                                       typeof(string), 
                                                                                       typeof(CustomCamera));
        public string FrameRate
        {
            get { return (string)this.GetValue(FrameRateProperty); }
            set { this.SetValue(FrameRateProperty, value); }
        }
    }
}