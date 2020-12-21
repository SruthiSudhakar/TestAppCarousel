using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using TestAppCarousel.Models;
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
        private int studentsPerPage = 4;

        
        private readonly IPageService _pageService;
        public ICommand MistakeItemPressedCommand { get; set; }
        private StudentAndList _selectedStudentAndList;
        public bool _mistakeShlokaSelected = false;
        public bool MistakeShlokaSelected
        {
            get { return _mistakeShlokaSelected; }
            set { SetValue(ref _mistakeShlokaSelected, value); }
        }

        public AssessmentViewModel(int numOfStudents, int studentsPerPage, PageService _pageService)
        {
            this.studentsPerPage = studentsPerPage;
            this._pageService = _pageService;
            _students = new ObservableCollection<Student>();
            for (int x = 1; x <= numOfStudents; x++)
            {
                _students.Add(new Student { Name = "student " + x });
            }
            Students = new ObservableCollection<StudentAndList>();

            foreach (Student s in _students)
            {

                Students.Add(new StudentAndList(s, ParseAndCreateChapterList.praseJson(@"C:\GitaAppData\XamrinGitaApps\GitaAssesmentApp\TestAppCarousel\TestAppCarousel\TestAppCarousel\Resources\meta_plain_chapter_00.txt")));
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
            MessagingCenter.Subscribe<StudentAndList, StudentAndList>(this, "SelectMistakeType", async (sender, arg) =>
            {
                if (MistakeShlokaSelected)
                {
                    _selectedStudentAndList._selectedWord.HilightedWord = Color.White;
                }
                _selectedStudentAndList = arg;
                MistakeShlokaSelected = true;
            });
        }

        public void MistakeItemPressedMethod(ToolbarItem o)
        {
            if (MistakeShlokaSelected)
            {
                if (o.Name.Equals("Undo"))
                {
                    _pageService.DisplayAlert("Undo Mistake " + _selectedStudentAndList.student.Name, "mitake: " + o.Name + ",  in pada: " + _selectedStudentAndList._selectedWord.WordText, "ok", "cancel");
                    _selectedStudentAndList._selectedWord.HilightedWord = Color.White;
                }
                else
                {

                    _pageService.DisplayAlert("Mistake Marked For " + _selectedStudentAndList.student.Name, "mitake: " + o.Name + ",  in pada: " + _selectedStudentAndList._selectedWord.WordText, "ok", "cancel");
                    _selectedStudentAndList._selectedWord.HilightedWord = Color.Orange;
                }
                MistakeShlokaSelected = false;
            }
        }
    }
}