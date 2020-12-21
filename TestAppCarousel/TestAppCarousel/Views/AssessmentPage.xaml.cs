using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAppCarousel.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestAppCarousel.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssessmentPage : ContentPage
    {
        public AssessmentPage()
        {
            BindingContext = new AssessmentViewModel(6, 3, new PageService());
            InitializeComponent();
        }
    }
}