using ShoeRepair.AppData;
using ShoeRepair.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
    /// Логика взаимодействия для EditMasterWindow.xaml
    /// </summary>
    public partial class EditMasterWindow : Window
    {
        private User newmaster;
        public EditMasterWindow(User master)
        {
            InitializeComponent();
            newmaster = master;
            FullNameTb.Text = newmaster.FullName;
            LoginTb.Text = newmaster.Profile.Login;
            PasswordTb.Text = newmaster.Profile.Password;
            DateOfBirthDp.SelectedDate = newmaster.DateOfBirth;
            EmailTb.Text = newmaster.Email;
            PhoneTb.Text = newmaster.Phone.ToString();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            newmaster.FullName = FullNameTb.Text;
            newmaster.Profile.Password = PasswordTb.Text;
            newmaster.DateOfBirth = DateOfBirthDp.SelectedDate ?? newmaster.DateOfBirth;
            newmaster.Email = EmailTb.Text;
            if (long.TryParse(PhoneTb.Text, out long phoneNumber))
            {
                newmaster.Phone = phoneNumber;
            }

            App.Ent.SaveChanges();
            MessageBoxHelper.Information("Изменения сохранены!");
            this.Close();
        }
    }
}
