using LoadPixelImages.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LoadPixelImages
{
    public partial class App : Application
    {
        public App()
        {
           
            InitializeComponent();

            MainPage = new ImageListPage();
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
