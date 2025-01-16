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
    /// Логика взаимодействия для AddSickLeaveWindow.xaml
    /// </summary>
    public partial class AddSickLeaveWindow : Window
    {
        public SickLeaves SickLeave { get; private set; }
        public AddSickLeaveWindow()
        {
            InitializeComponent();
        }
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (StartDateDp.SelectedDate.HasValue && EndDateDp.SelectedDate.HasValue)
            {
                SickLeave = new SickLeaves
                {
                    StartDate = StartDateDp.SelectedDate.Value,
                    EndDate = EndDateDp.SelectedDate.Value,
                    Reason = ReasonTb.Text
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
