using ShoeRepair.AppData;
using ShoeRepair.Model;
using ShoeRepair.View.Windows;
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
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();
            ServiceLv.ItemsSource = null;
            AppointmentsLv.ItemsSource = App.Ent.Appointments.ToList();
            AppointmentsLv.ItemsSource = App.Ent.Appointments
                 .Where(a => a.Id_Client == App.currentUser.Id)
                   .Include("User")
                   .Include("Status")
                   .Include("ServicesAppointment.Services")
                   .ToList();
        }

        private void AppointmentsLv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AppointmentsLv.SelectedItem is Appointments selectedAppointments)
            {
                var appointments = App.Ent.Appointments
                    .Where(h => h.Id_Client == App.currentUser.Id)
                    .ToList();
                ServiceLv.ItemsSource = appointments
                    .Where(a => App.Ent.User
                        .Any(u => u.Id == a.Id_Master && u.Profile.RoleID == 2))
                    .Select(a => new
                    {
                        a.Id,
                        a.AppointmentDateTime,
                        Status = a.Status.Name,
                        a.Price
                    })
                    .ToList();
                var services = App.Ent.ServicesAppointment
                        .Where(sp => sp.Id_Appointment == selectedAppointments.Id)
                        .Select(sp => new
                        {
                            sp.Services.Name,
                            sp.Quantity,
                            sp.Services.Description,
                            sp.Price,
                            sp.Services.Photo
                        }).ToList();
                ServiceLv.ItemsSource = services;
            }
        }
        private void FeedBeckBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AppointmentsLv.SelectedItem == null)
            {
                MessageBoxHelper.Error("Выберите мастера для редактирования.");
                return;
            }
            Appointments selectedAppointments = AppointmentsLv.SelectedItem as Appointments;
            AddFeedbackWindow addFeedbackWindow = new AddFeedbackWindow(selectedAppointments);
            addFeedbackWindow.ShowDialog();
            AppointmentsLv.ItemsSource = App.Ent.Appointments
               .Where(a => a.Id_Client == App.currentUser.Id)
                 .Include("User")
                 .Include("Status")
                 .Include("ServicesAppointment.Services")
                 .ToList();
        }
    }
}
