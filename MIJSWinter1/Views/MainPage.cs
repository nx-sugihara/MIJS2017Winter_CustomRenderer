using System;

using Xamarin.Forms;

namespace MIJSWinter1
{
    public class MainPage : TabbedPage
    {
        public MainPage()
        {
            Page testPage = null;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    testPage = new NavigationPage(new TestPage())
                    {
                        Title = "Test"
                    };
                    break;
                default:
                    testPage = new TestPage()
                    {
                        Title = "Test"
                    };

                    break;
            }

            if (testPage != null)
            {
                Children.Add(testPage);
            }
            Title = Children[0].Title;
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            Title = CurrentPage?.Title ?? string.Empty;
        }
    }
}
