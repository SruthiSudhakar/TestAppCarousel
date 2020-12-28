using System;
using System.Collections.Generic;
using TestAppCarousel.ViewModels;
using TestAppCarousel.Views;
using Xamarin.Forms;

namespace TestAppCarousel
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }
        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);
        }
        private void RegisterRoutes()
        {
            Routing.RegisterRoute("Home", typeof(HomePage));
            Routing.RegisterRoute("AsessmentPage", typeof(AssessmentPage));
        }
    }
}
