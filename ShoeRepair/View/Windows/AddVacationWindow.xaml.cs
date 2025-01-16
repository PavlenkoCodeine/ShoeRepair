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
    /// Логика взаимодействия для AddVacationWindow.xaml
    /// </summary>
    public partial class AddVacationWindow : Window
    {
        public Vacations Vacation { get; private set; }
        public AddVacationWindow()
        {
            InitializeComponent();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (StartDateDp.SelectedDate.HasValue && EndDateDp.SelectedDate.HasValue)
            {
                Vacation = new Vacations
                {
                    StartDate = StartDateDp.SelectedDate.Value,
                    EndDate = EndDateDp.SelectedDate.Value
                };
                DialogResult = true;
            }
            else
            {
                MessageBoxHelper.Error("Пожалуйста, заполните все поля.");
            }
        }
    }
}
