using Xamarin.Forms;
[assembly: Xamarin.Forms.Dependency(typeof(MIJSWinter1.iOS.PlatformNameProvider))]
namespace MIJSWinter1.iOS
{
    public class PlatformNameProvider :PlatformNameProvider
    {
        public string GetName()
        {
            return "iOS";
        }
    }
}
