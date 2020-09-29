using Diary.Commands;
using Diary.Properties;
using System.Windows;
using System.Windows.Input;

namespace Diary.ViewModels
{
    public class DbSettingsViewModel : ViewModelBase
    {
        public DbSettingsViewModel()
        {
            CloseCommand = new RelayCommand(Close);
            ConfirmCommand = new RelayCommand(Confirm);
        }

        

        public ICommand CloseCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }

        public string DbServerAddress
        {
            get
            {
                return Settings.Default.DbServerAddress;
            }
            set
            {
                Settings.Default.DbServerAddress = value;
            }
        }

        public string DbServerName
        {
            get
            {
                return Settings.Default.DbServerName;
            }
            set
            {
                Settings.Default.DbServerName = value;
            }
        }

        public string DbName
        {
            get
            {
                return Settings.Default.DbName;
            }
            set
            {
                Settings.Default.DbName = value;
            }
        }

        public string DbUser
        {
            get
            {
                return Settings.Default.DbUser;
            }
            set
            {
                Settings.Default.DbUser = value;
            }
        }

        public string DbPassword
        {
            get
            {
                return Settings.Default.DbPassword;
            }
            set
            {
                Settings.Default.DbPassword = value;
            }
        }

        private void Confirm(object obj)
        {
            
            Settings.Default.Save();
            CloseWindow(obj as Window);
        }

        private void Close(object obj)
        
        {
            CloseWindow(obj as Window);
            System.Environment.Exit(0);
        }

        private void CloseWindow(Window window)
        {
            window.Close();
        }
    }
}

