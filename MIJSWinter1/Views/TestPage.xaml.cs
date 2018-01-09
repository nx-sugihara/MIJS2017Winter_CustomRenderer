using System;

using Xamarin.Forms;

namespace MIJSWinter1
{
    public partial class TestPage : ContentPage
    {
        public Item Item { get; set; }

        public TestPage()
        {
            InitializeComponent();

            Item = new Item
            {
                Text = "Item name",
                Description = "This is an item description."
            };



            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    this.OSLabel.Text = "iOS";
                    break;
                case Device.Android:
                    this.OSLabel.Text = "Android";
                    break;
            }

            BindingContext = this;
        }

        private void OnClicked(object sender, EventArgs e)
        {
         //   this.TestLabel.Text = "Changed";
        }

        private void Handle_Clicked(object sender, EventArgs e)
        {
            this.OSLabel.Text = DateTime.Now.ToString();
        }
    }
}
