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
    /// Логика взаимодействия для AddServiceWindow.xaml
    /// </summary>
    public partial class AddServiceWindow : Window
    {
        public AddServiceWindow()
        {
            InitializeComponent();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTb.Text;
            string description = DescriptionTb.Text;
            if (decimal.TryParse(PriceTb.Text, out decimal price)) 
            {
                Services newService = new Services
                {
                    Name = name,
                    Description = description,
                    Price = price
                };
                try
                {
                    App.Ent.Services.Add(newService);
                    App.Ent.SaveChanges();
                    MessageBoxHelper.Information("Услуга успешно добавлена!");
                    this.DialogResult = true; 
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBoxHelper.Error($"Ошибка при добавлении услуги: {ex.Message}");
                }
            }
            else
            {
                MessageBoxHelper.Error("Введите корректную цену.");
            }
        }
    }
}
