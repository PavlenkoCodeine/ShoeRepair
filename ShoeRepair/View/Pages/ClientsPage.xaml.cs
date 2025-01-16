using ShoeRepair.AppData;
using ShoeRepair.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShoeRepair.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        public ClientsPage()
        {
            InitializeComponent();
            UserLv.ItemsSource = App.Ent.User.ToList();
        }

        private void SaveChangesBtn_Click(object sender, RoutedEventArgs e)
        {
            App.Ent.SaveChanges();
            MessageBoxHelper.Information("Информация успешно изменена!");
        }

        private void UserLv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UserLv.SelectedItem is User selectedUser)
            {
                UserDetailsGrid.DataContext = selectedUser;

                var appointments = App.Ent.Appointments
                    .Where(h => h.Id_Client == selectedUser.Id)
                    .ToList();
                AppLv.ItemsSource = appointments
                    .Where(a => App.Ent.User
                        .Any(u => u.Id == a.Id_Master && u.Profile.RoleID == 2))
                    .Select(a => new
                    {
                        a.Id,
                        MasterName = App.Ent.User.FirstOrDefault(u => u.Id == a.Id_Master).FullName, 
                        a.AppointmentDateTime,
                        Status = a.Status.Name,
                        FeedbackMark = a.Feedback.Mark,
                        a.Price
                    })
                    .ToList();
            }
        }
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (UserLv.SelectedItem is User selectedUser)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить пользователя '{selectedUser.FullName}'?",
                                              "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result != MessageBoxResult.Yes)
                {
                    return;
                }
                try
                {
                    var appointments = App.Ent.Appointments.Where(a => a.Id_Client == selectedUser.Id).ToList();
                    foreach (var appointment in appointments)
                    {
                        App.Ent.Appointments.Remove(appointment);
                    }
                    var chats = App.Ent.Chats.Where(c => c.Id_User == selectedUser.Id).ToList();
                    foreach (var chat in chats)
                    {
                        App.Ent.Chats.Remove(chat);
                    }
                    var profile = App.Ent.Profile.FirstOrDefault(p => p.Id == selectedUser.ProfileID);
                    if (profile != null)
                    {
                        App.Ent.Profile.Remove(profile);
                    }
                    App.Ent.User.Remove(selectedUser);
                    App.Ent.SaveChanges();

                    MessageBoxHelper.Information("Пользователь и все связанные с ним данные успешно удалены.");
                    UserLv.ItemsSource = App.Ent.User.ToList();
                    UserDetailsGrid.DataContext = null;
                }
                catch (Exception ex)
                {
                    MessageBoxHelper.Error($"Ошибка при удалении пользователя: {ex.Message}");
                }
            }
            else
            {
                MessageBoxHelper.Information("Пожалуйста, выберите пользователя для удаления.");
            }

        }
        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            var query = App.Ent.User.AsQueryable();
            if (!string.IsNullOrWhiteSpace(SearchTb.Text))
            {
                string searchText = SearchTb.Text.ToLower();
                query = query.Where(p => p.FullName.ToLower().Contains(searchText));
            }
            UserLv.ItemsSource = query.ToList();
        }
    }
}
