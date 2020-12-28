using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using TestAppCarousel.Models;
using TestAppCarousel.Views;
using Xamarin.Forms;

namespace TestAppCarousel.ViewModels
{

    public class AssessmentViewModel : BaseViewModel
    {


        public class StudentListGroup
        {
            public ObservableCollection<StudentAndList> StudentList { get; set; }
            public StudentListGroup(ObservableCollection<StudentAndList> ss)
            {
                StudentList = ss;
            }
        }
        public ObservableCollection<StudentAndList> Students { get; set; }
        public ObservableCollection<StudentListGroup> StudentGroups { get; set; } = new ObservableCollection<StudentListGroup>();
        private ObservableCollection<Student> _students;


        private readonly IPageService _pageService;

        public ICommand MistakeItemPressedCommand { get; set; }
        private StudentAndList _selectedStudentAndList;
        private Word _selectedWord;
        private string _partOfWord = "";
        private string _theWord = "";
        public int numOfStudents = 0;
        public int studentsPerPage = 0;
        public string TheWord { get { return _theWord; } set { SetValue(ref _theWord, value); } }
        public AssessmentViewModel(int numOfStudents, int studentsPerPage, List<Chapter> chapters, PageService _pageService)
        {
            this.studentsPerPage = studentsPerPage;
            this.numOfStudents = numOfStudents;
            this._pageService = _pageService;
            _students = new ObservableCollection<Student>();
            for (int x = 1; x <= numOfStudents; x++)
            {
                _students.Add(new Student { Name = "student " + x });
            }
            Students = new ObservableCollection<StudentAndList>();

            for (int s = 0; s < _students.Count; s++)
            {

                Students.Add(new StudentAndList(_students[s], chapters[s]));
            }
            int i = 0;
            while (i < _students.Count)
            {
                ObservableCollection<StudentAndList> temp = new ObservableCollection<StudentAndList>(Students.ToList().GetRange(i, Math.Min(studentsPerPage, Students.Count - i)));
                StudentListGroup sg = new StudentListGroup(temp);
                StudentGroups.Add(sg);
                i += studentsPerPage;
            }
            MistakeItemPressedCommand = new Command<ToolbarItem>((e) => MistakeItemPressedMethod(e));
            MessagingCenter.Subscribe<AssessmentPage, Tuple<StudentAndList, Word, string>>(this, "MostRecentView", (sender, arg) =>
            {
                _selectedStudentAndList = arg.Item1;
                _selectedWord = arg.Item2;
                _partOfWord = arg.Item3;
                TheWord = _partOfWord;
            });
        }

        public void MistakeItemPressedMethod(ToolbarItem o)
        {
            if (o.Text.Equals("Undo"))
            {
                MessagingCenter.Send<AssessmentViewModel>(this, "UndoDrawing");
            }
            else
            {
                _pageService.DisplayAlert("Undo Mistake " + _selectedStudentAndList.student.Name, "mitake: " + o.Text + ",  in pada: " + _selectedWord.WordText + ", partofword " + _partOfWord, "ok", "cancel");
            }
            MessagingCenter.Send<AssessmentViewModel>(this, "MistakeRegistered");
        }
    }
}