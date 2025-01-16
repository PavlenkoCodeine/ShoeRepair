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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShoeRepair.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для SupportChatPage.xaml
    /// </summary>
    public partial class SupportChatPage : Page
    {
        Chats selectedChat;
        public SupportChatPage()
        {
            InitializeComponent();
            ChatsLb.ItemsSource = App.Ent.Chats.ToList();
        }
        private void ChatsLb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedChat = ChatsLb.SelectedItem as Chats;
            ChatGrid.DataContext = selectedChat;
            MessageLb.ItemsSource = App.Ent.Messages.Where(c => c.Id_Chat == selectedChat.Id).ToList();
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(MessageTb.Text))
            {
                MessageBoxHelper.Error("Введите сообщение!");
            }
            else
            {
                try
                {
                    var currentUser = App.currentUser;
                    if (currentUser == null)
                    {
                        MessageBoxHelper.Error("Пользователь не авторизован!");
                        return;
                    }
                    var selectedChat = ChatsLb.SelectedItem as Chats;
                    Messages chatSession = new Messages()
                    {
                        Id_Chat = selectedChat.Id,
                        Id_Sender = currentUser.Id,
                        MessageText = MessageTb.Text,
                        SendDate = DateTime.Now
                    };
                    App.Ent.Messages.Add(chatSession);
                    App.Ent.SaveChanges();

                    MessageLb.ItemsSource = App.Ent.Messages
                        .Where(c => c.Id_Chat == selectedChat.Id)
                        .ToList();
                    MessageTb.Clear();
                }
                catch
                {
                    MessageBoxHelper.Information("Ошибка при добавлении в БД. Попробуйте позже.");
                }
            }
        }
    }
}
