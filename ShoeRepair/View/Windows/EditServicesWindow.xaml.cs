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
    /// Логика взаимодействия для EditServicesWindow.xaml
    /// </summary>
    public partial class EditServicesWindow : Window
    {
        private Services newservice;
        public EditServicesWindow(Services services)
        {
            InitializeComponent();
            newservice = services;
            NameTb.Text = newservice.Name;
            DescriptionTb.Text = newservice.Description;
            PreisTb.Text = string.Format("{0:F2}", newservice.Price);
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            newservice.Name = NameTb.Text;
            newservice.Description = DescriptionTb.Text;

            if (decimal.TryParse(PreisTb.Text, out decimal price))
            {
                newservice.Price = price; 
            }
            else
            {
                MessageBoxHelper.Error("Введите корректную цену.");
                return;
            }

            App.Ent.SaveChanges();
            MessageBoxHelper.Information("Изменения сохранены!");
            this.Close();
        }
    }
}
