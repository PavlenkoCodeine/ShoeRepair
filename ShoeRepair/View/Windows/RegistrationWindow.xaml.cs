using ShoeRepair.AppData;
using ShoeRepair.Model;
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
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void RegistrationBtn_Click(object sender, RoutedEventArgs e)
        {
            RegUser();
        }
        Profile regUser = new Profile();
        private void RegUser()
        {
            if (App.Ent.Role.Any(r => r.Id == 3))
            {
                regUser.Login = LoginTb.Text;
                regUser.Password = PasswordPb.Password;
                if (string.IsNullOrWhiteSpace(regUser.Login) || string.IsNullOrWhiteSpace(regUser.Password))
                {
                    MessageBoxHelper.Error("Заполните логин и пароль!");
                }
                else
                {
                    if (App.Ent.Profile.Any(p => p.Login == regUser.Login))
                        MessageBoxHelper.Error("Такой пользователь уже существует!");
                    else
                    {
                        try
                        {
                            Profile profile = new Profile()
                            {
                                Login = LoginTb.Text,
                                Password = PasswordPb.Password,
                                Photo = "https://thispersondoesnotexist.com/",
                                RoleID = 1
                            };

                            App.Ent.Profile.Add(profile);

                            User users = new User()
                            {
                                FullName = FullNameTb.Text,
                                DateOfBirth = DateTime.Parse(DateOfBirthTb.Text).Date,
                                Phone = long.Parse(PhoneTb.Text),
                                Email = EmailTb.Text,
                                ProfileID = profile.Id
                            };
                            App.Ent.User.Add(users);
                            App.Ent.SaveChanges();
                            MessageBoxHelper.Information("Пользователь зарегистрирован!");

                            AuthorizationWindow authorizationWindow = new AuthorizationWindow();
                            authorizationWindow.Show();
                            Close();
                        }
                        catch
                        {
                            MessageBoxHelper.Error("Ошибка БД. Попробуйте позже!");
                        }
                    }
                }
            }
        }
        private void AutorizationHl_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow authorizationWindow = new AuthorizationWindow();
            authorizationWindow.Show();
            Close();
        }
    }
}
