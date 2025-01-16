using ShoeRepair.AppData;
using ShoeRepair.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для ServiceUserPage.xaml
    /// </summary>
    public partial class ServiceUserPage : Page
    {
        private ObservableCollection<ServicesAppointment> serviceProjects = new ObservableCollection<ServicesAppointment>();
        List<Categories> categories = App.Ent.Categories.ToList();
        List<Services> services = App.Ent.Services.ToList();
        public ServiceUserPage()
        {
            InitializeComponent();
            ServicesLv.ItemsSource = App.Ent.Services.ToList();
            ServicesDg.ItemsSource = serviceProjects;
            serviceProjects.CollectionChanged += (s, e) => UpdateTotalCost();
            UpdateTotalCost();
            categories.Insert(0, new Categories() { Name = "Все категории" });
            FilterCb.ItemsSource = App.Ent.Categories.ToList();
            FilterCb.ItemsSource = categories;
            FilterCb.DisplayMemberPath = "Name";
            FilterCb.SelectedValuePath = "Id";
            FilterCb.SelectedIndex = 0;
        }
        private void UpdateTotalCost()
        {
            decimal total = serviceProjects.Sum(sp => sp.Price * sp.Quantity);
            TotalCostTb.Text = total.ToString("C");
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Random random = new Random();
                var users = App.Ent.User.Where(u => u.Profile.RoleID == 2).AsQueryable();
                var usersList = users.ToList();
                int randomIndex = random.Next(usersList.Count);
                var randomUser = usersList[randomIndex];
                if (usersList.Count == 0)
                {
                    MessageBoxHelper.Error("Нет доступных мастеров.");
                    return;
                }
                Appointments newAppointments = new Appointments
                {
                    Id_Client = App.currentUser.Id,
                    Id_Master = randomUser.Id,
                    AppointmentDateTime = DateTime.Now,
                    Id_Status = 1,
                    Price = serviceProjects.Sum(sp => sp.Price * sp.Quantity),
                    ServicesAppointment = serviceProjects.Select(sp => new ServicesAppointment
                    {
                        Id_Services = sp.Services.Id,
                        Quantity = sp.Quantity,
                        Price = sp.Price
                    }).ToList()
                };
                App.Ent.Appointments.Add(newAppointments);
                App.Ent.SaveChanges();
                MessageBoxHelper.Information("Заявка успешно отправлена. Ожидайте приглашения в мастерскую.");
                ServicesDg.ItemsSource = null;
                NameTbl.Text = "";
                DescriptionTbl.Text = "";
                PriceTbl.Text = "";
                QuanityTbl.Text = "";
                TotalCostTb.Text = "";
            }
            catch (Exception ex)
            {
                MessageBoxHelper.Error("Ошибка");
            }
        }

        private void AddKorBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ServicesLv.SelectedItem is Services selectedService)
            {
                if (int.TryParse(QuanityTbl.Text, out int quantity) && quantity > 0)
                {
                    serviceProjects.Add(new ServicesAppointment
                    {
                        Services = selectedService,
                        Quantity = quantity,
                        Price = selectedService.Price
                    });
                    UpdateTotalCost();
                    QuanityTbl.Text = string.Empty;
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите корректное количество.", "Некорректное количество", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBoxHelper.Warning("Пожалуйста, выберите услугу.");
            }
        }

        private void ServicesLv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ServiceDetailsGrid.DataContext = ServicesLv.SelectedItem as Services;
        }

        private void FilterCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FilterCb.SelectedValue is int selectedCategoryId)
            {
                if (selectedCategoryId == 0)
                {
                    ServicesLv.ItemsSource = services;
                }
                else
                {
                    ServicesLv.ItemsSource = services.Where(s => s.Id_Categories == selectedCategoryId).ToList();
                }
            }
        }
    }
}
