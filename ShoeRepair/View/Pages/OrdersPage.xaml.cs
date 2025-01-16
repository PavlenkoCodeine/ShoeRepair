using ShoeRepair.AppData;
using ShoeRepair.Model;
using ShoeRepair.View.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
using static System.Net.Mime.MediaTypeNames;

namespace ShoeRepair.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {

        List<Status> status = App.Ent.Status.ToList();
        private List<Appointments> appointments = App.Ent.Appointments.ToList();
        public OrdersPage()
        {
            InitializeComponent();
            LoadOrders();
            ServicesLv.ItemsSource = null;
            OrdersLb.ItemsSource = App.Ent.Appointments.ToList();
            status.Insert(0, new Status() { Name = "Все статусы" });
            StatusFilterCb.ItemsSource = status;
            StatusFilterCb.DisplayMemberPath = "Name";
            StatusFilterCb.SelectedValuePath = "Id";
            StatusFilterCb.SelectedIndex = 0;
        }
        private void LoadOrders()
        {
            OrdersLb.ItemsSource = App.Ent.Appointments
                    .Include("User")
                    .Include("Status")
                    .Include("Feedback")
                    .Include("ServicesAppointment.Services")
                    .ToList();
        }
        private void StatusFilterCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var query = App.Ent.Appointments.AsQueryable();
            if (StatusFilterCb.SelectedValue != null && (int)StatusFilterCb.SelectedValue != 0)
            {
                int selectedStatusId = (int)StatusFilterCb.SelectedValue;
                query = query.Where(p => p.Id_Status == selectedStatusId);
            }
            OrdersLb.ItemsSource = query.ToList();
        }

        private void StartDp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            OrdersLb.ItemsSource = appointments.Where(a => a.AppointmentDateTime == StartDp.SelectedDate);
        }

        private void ResetFiltersBtn_Click(object sender, RoutedEventArgs e)
        {
            StatusFilterCb.SelectedIndex = -1;
            StartDp.SelectedDate = null;
            OrdersLb.ItemsSource = appointments;
        }

        private void DeleteOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersLb.SelectedItem == null)
            {
                MessageBoxHelper.Information("Выберите заявку для удаления.");
                return;
            }
            Appointments selectedAppointments = OrdersLb.SelectedItem as Appointments;
            var result = MessageBox.Show($"Вы уверены, что хотите удалить заявку?",
                                         "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            try
            {
                foreach (var serviceAppointment in App.Ent.ServicesAppointment.Where(sp => sp.Id_Appointment == selectedAppointments.Id).ToList())
                {
                    App.Ent.ServicesAppointment.Remove(serviceAppointment);
                }
                App.Ent.SaveChanges();
                App.Ent.Appointments.Remove(selectedAppointments);
                App.Ent.SaveChanges();
                MessageBoxHelper.Information("Проект и все связанные с ним услуги успешно удалены.");
                OrdersLb.ItemsSource = App.Ent.Appointments.ToList();
            }
            catch (Exception ex)
            {
                MessageBoxHelper.Error($"Ошибка при удалении проекта: {ex.Message}");
            }
        }
        private void OrdersLb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AppointmentDetailsGrid.DataContext = OrdersLb.SelectedItem as Appointments;

            AppointmentStatusCmb.ItemsSource = App.Ent.Status.ToList();
            AppointmentStatusCmb.DisplayMemberPath = "Name";
            AppointmentStatusCmb.SelectedValuePath = "Id";
            AppointmentStatusCmb.SelectedIndex = OrdersLb.SelectedItem as Appointments != null ? (OrdersLb.SelectedItem as Appointments).Id_Status - 1 : -1;

            if (OrdersLb.SelectedItem is Appointments selectedAppointments)
            {
                AppointmentDetailsGrid.DataContext = selectedAppointments;
                var services = App.Ent.ServicesAppointment
                        .Where(sp => sp.Id_Appointment == selectedAppointments.Id)
                        .Select(sp => new
                        {
                            sp.Services.Name,
                            sp.Quantity,
                            sp.Price,
                            sp.Services.Photo
                        }).ToList();
                ServicesLv.ItemsSource = services;
                decimal totalCost = services.Sum(s => s.Price * s.Quantity);
                TotalProjectCostTbl.Text = totalCost.ToString("C");
            }
        }

        private void AppointmentStatusBtn_Click(object sender, RoutedEventArgs e)
        {
            (OrdersLb.SelectedItem as Appointments).Id_Status = Convert.ToInt32(AppointmentStatusCmb.SelectedValue);
            App.Ent.SaveChanges();
            OrdersLb.ItemsSource = appointments = App.Ent.Appointments.ToList();
            MessageBoxHelper.Information("Данные заявки изменены!");
        }
    }
}
