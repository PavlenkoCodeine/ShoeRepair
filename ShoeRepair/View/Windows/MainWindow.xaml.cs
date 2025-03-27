using ShoeRepair.AppData;
using ShoeRepair.Model;
using ShoeRepair.View.Pages;
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

namespace ShoeRepair
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Profile> profile = App.Ent.Profile.ToList();
        public MainWindow()
        {
            InitializeComponent();
            FrameHelper.mainFrame = MainFrame;

            if (App.currentUser.Profile.RoleID == 1)
            {
                FrameHelper.mainFrame.Navigate(new OrdersPage());
                OrderBtn.Visibility = Visibility.Visible;
                ClientBtn.Visibility = Visibility.Visible;
                MasterBtn.Visibility = Visibility.Visible;
                ServiseBtn.Visibility = Visibility.Visible;
                ChatBtn.Visibility = Visibility.Visible;
                ServiceUserBtn.Visibility = Visibility.Hidden;
                OrdersUserBtn.Visibility = Visibility.Hidden;
            }
            else if (App.currentUser.Profile.RoleID == 3)
            {
                FrameHelper.mainFrame.Navigate(new ServiceUserPage());
                OrderBtn.Visibility = Visibility.Hidden;
                ClientBtn.Visibility = Visibility.Hidden;
                MasterBtn.Visibility = Visibility.Hidden;
                ServiseBtn.Visibility = Visibility.Hidden;
                ChatBtn.Visibility = Visibility.Visible;
                ServiceUserBtn.Visibility = Visibility.Visible;
                OrdersUserBtn.Visibility = Visibility.Visible;
            }

        }
        private void OrderBtn_Click(object sender, RoutedEventArgs e)
        {
            FrameHelper.mainFrame.Navigate(new OrdersPage()); 
        }

        private void ClientBtn_Click(object sender, RoutedEventArgs e)
        {
            FrameHelper.mainFrame.Navigate(new ClientsPage()); 
        }

        private void MasterBtn_Click(object sender, RoutedEventArgs e)
        {
            FrameHelper.mainFrame.Navigate(new MastersPage());
        }

        private void ServiseBtn_Click(object sender, RoutedEventArgs e)
        {
            FrameHelper.mainFrame.Navigate(new ServicesPage());
        }

        private void ChatBtn_Click(object sender, RoutedEventArgs e)
        {
            FrameHelper.mainFrame.Navigate(new SupportChatPage());
        }

        private void EntBtn_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow authorizationWindow = new AuthorizationWindow();
            authorizationWindow.Show();
            Close();
        }

        private void ServiceUserBtn_Click(object sender, RoutedEventArgs e)
        {
            FrameHelper.mainFrame.Navigate(new ServiceUserPage());
        }

        private void OrdersUserBtn_Click(object sender, RoutedEventArgs e)
        {
            FrameHelper.mainFrame.Navigate(new ProfilePage());
            
        }
    }
}
