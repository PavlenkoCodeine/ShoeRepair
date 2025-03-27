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
    /// Логика взаимодействия для AddFeedbackWindow.xaml
    /// </summary>
    public partial class AddFeedbackWindow : Window
    {
        private Appointments _selectedAppointment;
        public AddFeedbackWindow(Appointments selectedAppointment)
        {
            InitializeComponent();
            _selectedAppointment = selectedAppointment;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MarkTb.Text) || !int.TryParse(MarkTb.Text, out int mark) || mark < 1 || mark > 5)
            {
                MessageBoxHelper.Error("Введите корректную оценку (от 1 до 5).");
                return;
            }
            Feedback newfeedback = new Feedback
            {
                Comments = CommentsTb.Text,
                date = DateDp.SelectedDate.Value,
                Mark = mark
            };
            try
            {
                App.Ent.Feedback.Add(newfeedback);
                App.Ent.SaveChanges();
                MessageBoxHelper.Information("Отзыв успешно добавлен!");
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBoxHelper.Error($"Ошибка при добавлении : {ex.Message}");
            }

        }
    }
}
