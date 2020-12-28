using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TestAppCarousel.ViewModels;
using Xamarin.Forms;

namespace TestAppCarousel.Models
{
    public class StudentAndList
    {
        public Student student { get; set; }
        public Chapter CurrentChapter{ get; set; }
        public StudentAndList(Student s, Chapter cc)
        {
            student = s;
            CurrentChapter = cc;
        }
    }
}
