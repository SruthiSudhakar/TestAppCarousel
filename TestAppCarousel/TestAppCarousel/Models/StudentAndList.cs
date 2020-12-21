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
        public Word _selectedWord;
        public ICommand SelectMistakeCommand { get; private set; }
        public StudentAndList(Student s, Chapter cc)
        {
            student = s;
            CurrentChapter = cc;
            SelectMistakeCommand = new Command<Word>((e) => SelectedMistakeCommandMethod(e));
        }
        public void SelectedMistakeCommandMethod(Word word)
        {
            _selectedWord = word;
            MessagingCenter.Send<StudentAndList, StudentAndList>(this, "SelectMistakeType", this);
            word.HilightedWord = Color.Green;
        }

    }
}
