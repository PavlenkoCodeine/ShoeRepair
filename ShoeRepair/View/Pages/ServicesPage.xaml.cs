using ShoeRepair.AppData;
using ShoeRepair.Model;
using ShoeRepair.View.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShoeRepair.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage : Page
    {
        public ServicesPage()
        {
            InitializeComponent();
            ServicesLv.ItemsSource = App.Ent.Services.ToList();
        }
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ServicesLv.SelectedItem is Services selectedService)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить услугу '{selectedService.Name}'?",
                                              "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        foreach (var appointment in App.Ent.ServicesAppointment
                            .Where(sa => sa.Id_Services == selectedService.Id)
                            .ToList())
                        {
                            App.Ent.ServicesAppointment.Remove(appointment);
                        }
                        App.Ent.Services.Remove(selectedService);
                        App.Ent.SaveChanges();

                        MessageBoxHelper.Information("Услуга успешно удалена.");
                        ServicesLv.ItemsSource = App.Ent.Services.ToList();
                    }
                    catch (Exception ex)
                    {
                        MessageBoxHelper.Error($"Ошибка при удалении услуги: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBoxHelper.Information("Пожалуйста, выберите услугу для удаления.");
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var addServiceWindow = new AddServiceWindow();
            if (addServiceWindow.ShowDialog() == true)
            {
                ServicesLv.ItemsSource = App.Ent.Services.ToList();
            }
        }

        private void EditMasterBtn_Click_1(object sender, RoutedEventArgs e)
        {
            if (ServicesLv.SelectedItem == null)
            {
                MessageBoxHelper.Error("Выберите мастера для редактирования.");
                return;
            }

            Services selectedServices = ServicesLv.SelectedItem as Services;
            EditServicesWindow editWindow = new EditServicesWindow(selectedServices);
            editWindow.ShowDialog();
            ServicesLv.ItemsSource = App.Ent.Services.ToList();
        }
    }
}
