using System;

using Xamarin.Forms;

namespace MIJSWinter1
{
    public partial class TestPage : ContentPage
    {
        public Item Item { get; set; }

        private string[] buttonName = { "撮影する", "もう一回" };

        TestViewModel m_viewModel;
        public TestPage()
        {
            InitializeComponent();

            Item = new Item
            {
                Text = "Item name",
                Description = "This is an item description."
            };


            /*
             //OSごとの実装　実験
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    this.OSLabel.Text = "iOS";
                    break;
                case Device.Android:
                    this.OSLabel.Text = "Android";
                    break;
            }
            */
            this.ShutterButton.Text = buttonName[0];

            BindingContext = m_viewModel = new TestViewModel();

        }

        private void OnClicked(object sender, EventArgs e)
        {
            //   this.TestLabel.Text = "Changed";

            if(this.ShutterButton.Text == buttonName[0])//撮影
            {
                if (!this.Camera.Shutter)
                {
                    this.Camera.Shutter = true;
                    this.image.Source = null;

                    this.Camera.onShutter = (a_img) => {
                        //                    this.m_viewModel.m_img = a_img;
                        this.image.Source = a_img;

                        this.ShutterButton.Text = buttonName[1];
                        this.image.IsVisible = true;
                        this.Camera.IsVisible = false;
                    };
                }
            }else if(this.ShutterButton.Text == buttonName[1])//もう一回
            {
                this.ShutterButton.Text = buttonName[0];
                this.Camera.IsVisible = true;
                this.image.IsVisible = false;
            }
        }

        private void Handle_Clicked(object sender, EventArgs e)
        {
        }
    }
}
