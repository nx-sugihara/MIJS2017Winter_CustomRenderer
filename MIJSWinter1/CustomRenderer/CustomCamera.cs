using System;
using Xamarin.Forms;
namespace MIJSWinter1
{
    public class CustomCamera : View
    {
        public static readonly BindableProperty FrameRateProperty = BindableProperty.Create("FrameRate",
                                                                                       typeof(string), 
                                                                                       typeof(CustomCamera));
        
        public static readonly BindableProperty ShutterProperty = BindableProperty.Create("Shutter",
                                                                                       typeof(bool),
                                                                                       typeof(CustomCamera), false);

        public string FrameRate
        {
            get { return (string)this.GetValue(FrameRateProperty); }
            set { this.SetValue(FrameRateProperty, value); }
        }

        public bool Shutter
        {
            get { return (bool)this.GetValue(ShutterProperty); }
            set { this.SetValue(ShutterProperty, value); }
        }

        public Action<ImageSource> onShutter;
    }
}