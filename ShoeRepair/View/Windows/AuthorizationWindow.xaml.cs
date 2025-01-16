using ShoeRepair.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ShoeRepair.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
            LoadUserData();
        }

        private void EntryBtn_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }
        private void Login()
        {
            App.currentUser = App.Ent.User.FirstOrDefault(u => u.Profile.Login == LoginTb.Text 
            && u.Profile.Password == PasswordPb.Password);
            if (App.currentUser != null)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                RememberUserData();
                Close();
            }
            else
            {
                MessageBoxHelper.Error("Введите логин и пароль!");
            }
        }
        void RememberUserData()
        {
            if (RememberMeCb.IsChecked == true)
            {
                Properties.Settings.Default.LoginValue = LoginTb.Text;
                Properties.Settings.Default.PasswordValue = PasswordPb.Password;
            }
            else
            {
                Properties.Settings.Default.LoginValue = string.Empty;
                Properties.Settings.Default.PasswordValue = string.Empty;
            }

            Properties.Settings.Default.Save();
        }

        void LoadUserData()
        {
            if (Properties.Settings.Default.LoginValue != string.Empty)
            {
                LoginTb.Text = Properties.Settings.Default.LoginValue;
                PasswordPb.Password = Properties.Settings.Default.PasswordValue;
            }
        }

        private void RegistrationHl_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            Close();
        }
    }
}
