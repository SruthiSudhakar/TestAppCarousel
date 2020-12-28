using System.Collections.Generic;
using TestAppCarousel.Models;
using TestAppCarousel.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestAppCarousel.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public List<int> NumberOfStudents { get; set; } = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        public List<int> StudentsPerPage { get; set; } = new List<int> { 1, 2, 3, 4, 5 };
        public List<Chapter> chapters = new List<Chapter>();
        public HomePage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        async private void NumberOfStudents_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            SessionConfiguration.numberOfStudentsInSession = (int)(e.SelectedItem);
            int initialNumItems = chapters.Count;
            for (int i = 0; i < SessionConfiguration.numberOfStudentsInSession - initialNumItems; i ++)
            {
                chapters.Add(ParseAndCreateChapterList.praseJson(@"C:\GitaAppData\XamrinGitaApps\GitaAssesmentApp\TestAppCarousel\TestAppCarousel\TestAppCarousel\Resources\meta_plain_chapter_00.txt"));
            }
            SessionConfiguration.chapters = chapters;
        }

        private void StudentsPerPage_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            SessionConfiguration.numberOfStudentsPerPage = (int)(e.SelectedItem);
        }
    }
}