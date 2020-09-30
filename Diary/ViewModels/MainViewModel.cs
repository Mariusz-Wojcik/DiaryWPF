using Diary.Commands;
using Diary.Models.Domains;
using Diary.Models.Wrappers;
using Diary.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Data;

namespace Diary.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private Repository _repository = new Repository();

        public MainViewModel()
        {
            IsServerConnected();
            App.SplashScreen.Close(new TimeSpan(0, 0, 1));
            AddStudentsCommand = new RelayCommand(AddEditStudent);
            EditStudentsCommand = new RelayCommand(AddEditStudent, CanEditDeleteStudent);
            DeleteStudentsCommand = new AsyncRelayCommand(DeleteStudent, CanEditDeleteStudent);
            RefreshStudentsCommand = new RelayCommand(RefereshStudents);
            RefreshDiary();

            InitGroups();
        }




        public ICommand AddStudentsCommand { get; set; }
        public ICommand EditStudentsCommand { get; set; }
        public ICommand DeleteStudentsCommand { get; set; }
        public ICommand RefreshStudentsCommand { get; set; }


        private StudentWrapper _selectedStudent;

        public StudentWrapper SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                _selectedStudent = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<StudentWrapper> _students;

        public ObservableCollection<StudentWrapper> Students
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
            set
            {
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

        private bool CanEditDeleteStudent(object obj)
        {
            return SelectedStudent != null;
        }

        private async Task DeleteStudent(object obj)
        {
            var metroWindow = Application.Current.MainWindow as MetroWindow;
            var dialog = await metroWindow.ShowMessageAsync("Usuwanie ucznia", $"Czy na pewno chcesz usunąć ucznia " +
                $"{SelectedStudent.FirstName} {SelectedStudent.LastName} ?", MessageDialogStyle.AffirmativeAndNegative);
            if (dialog != MessageDialogResult.Affirmative)
                    return;

            _repository.DeleteStudent(SelectedStudent.Id);

            RefreshDiary();
        }

        private void AddEditStudent(object obj)
        {
            var addEditStudentWindow = new AddEditStudentView(obj as StudentWrapper);
            addEditStudentWindow.Closed += AddEditStudentWindow_Closed;
            addEditStudentWindow.ShowDialog();
        }

        private void AddEditStudentWindow_Closed(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        private void RefereshStudents(object obj)
        {
            RefreshDiary();
        }

        private void InitGroups()
        {
            var groups = _repository.GetGroups();
            groups.Insert(0, new Models.Domains.Group { Id = 0, Name = "Wszystkie" });

            Groups = new ObservableCollection<Group>(groups);

            SelectedGroupId = 0;
        }

        private void RefreshDiary()
        {
            Students = new ObservableCollection<StudentWrapper>(_repository.GetStudents(SelectedGroupId));
        }


        private static bool IsServerConnected()
        {

            using (var context = new ApplicationDbContext())
            {
                try
                {
                    context.Database.Connection.Open();
                    if (context.Database.Connection.State == ConnectionState.Open)
                    { }
                    return true;

                }
                catch (SqlException)
                {
                    var dialog = MessageBox.Show("Nie można się połaczyć z bazą danych, czy chcesz wprowadzić dane do serwera bazy danych?", "Błąd serwera", MessageBoxButton.YesNo);
                    if (dialog == MessageBoxResult.Yes)
                        {
                        var dbSettingsWindow = new DbSettingsView();
                        dbSettingsWindow.ShowDialog();
                        IsServerConnected();
                    }
                    else
                    {
                        System.Environment.Exit(0);
                    }
                    return false;
                }
            }
        }

    }
}

