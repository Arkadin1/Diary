using Diary.Commands;
using Diary.Models;
using Diary.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Diary.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            AddStudentCommand = new RelayCommand(AddEditStudent);

            EditStudentCommand = new RelayCommand(AddEditStudent, CanEditDeleteStudent);

            DeleteStudentCommand = new AsyncRelayCommand(DeleteStudent, CanEditDeleteStudent);

            RefreshStudentsCommand = new RelayCommand(RefreshStudents);

            RefreshDiary();

            InitGroups();
        }

        


        public ICommand AddStudentCommand { get; set; }

        public ICommand EditStudentCommand { get; set; }

        public ICommand DeleteStudentCommand { get; set; }

        public ICommand RefreshStudentsCommand { get; set; }

        private Student _selectStudent;

        public Student SelectStudent
        {
            get { return _selectStudent; }
            set
            {
                _selectStudent = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Student> _students;

        public ObservableCollection<Student> Students
        {
            get { return _students; }
            set
            {
                _students = value;
                OnPropertyChanged();
            }
        }

        private int _selectedGroupId;

        public int SelectedGroupId
        {
            get { return _selectedGroupId; }
            set { 
                _selectedGroupId = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Group> _group;

        public ObservableCollection<Group> Groups
        {
            get { return _group; }
            set
            {
                _group = value;
                OnPropertyChanged();
            }
        }

      

        private void RefreshStudents(object obj)
        {
            RefreshDiary();
        }

        private bool CanEditDeleteStudent(object obj)
        {
            return SelectStudent != null;
        }

        private async Task DeleteStudent(object obj)
        {
            var metroWindow = Application.Current.MainWindow as MetroWindow;
            var dialog = await metroWindow.ShowMessageAsync("Usuwanie ucznia ", $"Czy na pewno chcesz usunąć ucznia {SelectStudent.FirstName} {SelectStudent.LastName}?", MessageDialogStyle.AffirmativeAndNegative);

            if (dialog != MessageDialogResult.Affirmative)
                return;

            RefreshDiary();

        }

        private void AddEditStudent(object obj)
        {
            var addEditStudentWindow = new AddEditStudentView(obj as Student);
            addEditStudentWindow.Closed += AddEditStudentWindow_Closed;
            addEditStudentWindow.ShowDialog();
        }

        private void AddEditStudentWindow_Closed(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        private void InitGroups()
        {
            Groups = new ObservableCollection<Group>
            {
                new Group {Id = 0, Name = "Wszystkie"},
                new Group {Id = 1, Name = "1A"},
                new Group {Id = 2, Name = "2A"},
            };

            SelectedGroupId = 0;
        }

        private void RefreshDiary()
        {
            Students = new ObservableCollection<Student>
            {
                new Student
                {
                    FirstName = "Kazimierz",
                    LastName = "szpin",
                    Group = new Group { Id = 1 }
                },

                new Student
                {
                    FirstName = "Marek",
                    LastName = "Nowak",
                    Group = new Group { Id = 2 }
                },

                new Student
                {
                    FirstName = "Jan",
                    LastName = "As",
                    Group = new Group{ Id = 3 }
                },
            };
        }
    }
}
