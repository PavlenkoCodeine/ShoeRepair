using ShoeRepair.AppData;
using ShoeRepair.Model;
using ShoeRepair.View.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace ShoeRepair.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для MastersPage.xaml
    /// </summary>
    public partial class MastersPage : Page
    {
        private ObservableCollection<User> _users = new ObservableCollection<User>();
        private User _selectedUser;
        private int _activeOrdersCount;
        public int ActiveOrdersCount
        {
            get { return _activeOrdersCount; }
            set
            {
                _activeOrdersCount = value;
                OnPropertyChanged(nameof(ActiveOrdersCount));
            }
        }
        public MastersPage()
        {
            InitializeComponent();
            DataContext = this;
            MastersLv.ItemsSource = App.Ent.User.Where(a => a.Profile.RoleID == 2).ToList();
            LoadMasters();
        }
        private void LoadMasters()
        {
            var masters = App.Ent.User
                .Where(u => u.Profile.RoleID == 2)
                .ToList() 
                .Select(u => new User
                {
                    FullName = u.FullName,
                    DateOfBirth = u.DateOfBirth,
                    Email = u.Email
                })
                .ToList();
        }
        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            var query = App.Ent.User.Where(u => u.Profile.RoleID == 2).AsQueryable();
            if (!string.IsNullOrWhiteSpace(SearchTb.Text))
            {
                string searchText = SearchTb.Text.ToLower();
                query = query.Where(p => p.FullName.ToLower().Contains(searchText));
            }
            MastersLv.ItemsSource = query.ToList();
        }

        private void MastersLv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MasterDetailsGrid.DataContext = MastersLv.SelectedItem as User;
            if (MastersLv.SelectedItem is User selectedMaster)
            {
                _selectedUser = selectedMaster;
                MasterDetailsGrid.DataContext = selectedMaster;
                var feedbacks = App.Ent.Feedback
                    .Select(f => new
                    {
                        f.Id,
                        f.Comments,
                        f.Mark
                    })
                    .ToList();
                ReviewsLv.ItemsSource = feedbacks;
                LoadVacations();
                LoadSickLeaves();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MastersLv.SelectedItem == null)
            {
                MessageBoxHelper.Error("Выберите мастера для удаления.");
                return;
            }
            User selectedMaster = MastersLv.SelectedItem as User;
            var result = MessageBox.Show($"Вы уверены, что хотите удалить мастера {selectedMaster.FullName}?",
                                         "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            try
            {
                var profile = selectedMaster.Profile;

                if (profile != null)
                {
                    App.Ent.Profile.Remove(profile);
                }
                App.Ent.User.Remove(selectedMaster);
                App.Ent.SaveChanges();
                App.Ent.SaveChanges();
                MastersLv.ItemsSource = App.Ent.User.Where(a => a.Profile.RoleID == 2).ToList();
                LoadMasters();
                MessageBoxHelper.Information("Мастер успешно удален!");
            }
            catch (Exception ex)
            {
                MessageBoxHelper.Error($"Ошибка при удалении мастера: {ex.Message}");
            }
        }
        private void EditMasterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MastersLv.SelectedItem == null)
            {
                MessageBoxHelper.Error("Выберите мастера для редактирования.");
                return;
            }
            User selectedMaster = MastersLv.SelectedItem as User;
            EditMasterWindow editWindow = new EditMasterWindow(selectedMaster);
            editWindow.ShowDialog();
            LoadMasters();

        }

        private void AddMasterBtn_Click(object sender, RoutedEventArgs e)
        {
            AddMasterWindow addMasterWindow = new AddMasterWindow();
            if (addMasterWindow.ShowDialog() == true)
            {
                MastersLv.ItemsSource = App.Ent.User.Where(a => a.Profile.RoleID == 2).ToList();
            }
        }
        private void AddMasterSickLeave(int masterId, int sickLeaveId)
        {
            MasterLeaves masterSickLeave = new MasterLeaves
            {
                    Id_Master = masterId,
                    Id_Vacation = null,
                    Id_SickLeave = sickLeaveId
            };
            App.Ent.MasterLeaves.Add(masterSickLeave);
            App.Ent.SaveChanges();
        }
        private void AddMasterVacation(int masterId, int vacationId)
        {
            MasterLeaves masterVacation = new MasterLeaves
            {
                    Id_Master = masterId,
                    Id_Vacation = vacationId,
                    Id_SickLeave = null
            };
            App.Ent.MasterLeaves.Add(masterVacation);
            App.Ent.SaveChanges();
        }
        private void LoadVacations()
        {
            var masterId = _selectedUser.Id;
            var vacations = App.Ent.MasterLeaves
                .Where(ml => ml.Id_Master == masterId && ml.Id_Vacation.HasValue)
                .Select(ml => ml.Vacations)
                .ToList();
            if (vacations.Any())
            {
                VacationsLv.ItemsSource = vacations;
            }
            else
            {
                VacationsLv.ItemsSource = null;
            }
        }
        private void AddVacationBtn_Click(object sender, RoutedEventArgs e)
        {
            var vacationWindow = new AddVacationWindow();
            if (vacationWindow.ShowDialog() == true)
            {
                App.Ent.Vacations.Add(vacationWindow.Vacation);
                App.Ent.SaveChanges();
                AddMasterVacation(_selectedUser.Id, vacationWindow.Vacation.Id);

                LoadVacations();
            }
        }
        private void LoadSickLeaves()
        {
            var masterId = _selectedUser.Id;
            var sickLeaves = App.Ent.MasterLeaves
                .Where(ml => ml.Id_Master == masterId && ml.Id_SickLeave.HasValue)
                .Select(ml => ml.SickLeaves)
                .ToList();
            if (sickLeaves.Any())
            {
                SickLeavesLv.ItemsSource = sickLeaves;
            }
            else
            {
                SickLeavesLv.ItemsSource = null;
            }
        }
        private void AddSickLeaveBtn_Click(object sender, RoutedEventArgs e)
        {
            var sickLeaveWindow = new AddSickLeaveWindow();
            if (sickLeaveWindow.ShowDialog() == true)
            {
                App.Ent.SickLeaves.Add(sickLeaveWindow.SickLeave);
                App.Ent.SaveChanges();
                AddMasterSickLeave(_selectedUser.Id, sickLeaveWindow.SickLeave.Id);
                LoadSickLeaves();
            }
        }
    }
}
