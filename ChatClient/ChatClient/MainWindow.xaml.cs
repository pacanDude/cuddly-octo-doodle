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
using ChatClient.ServiceReference1;
using System.Windows.Threading;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(2000);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            string[] lst = client.GetUsersList();

            if (lst != null)
            {
                usersListBox.ItemsSource = lst;
                usersListBox.Items.Refresh();
            }
        }

        ChatServiceClient client = new ChatServiceClient();
        int sessionId = 0;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool result = client.RegisterUser(loginTextBox.Text, passwordTextBox.Password, "admin@ya.ru", "A new user!");
            if (result == true)
                MessageBox.Show("The user was successfully registered!");
            else
                MessageBox.Show("Something was wrong...");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            sessionId = client.LogIn(loginTextBox.Text, passwordTextBox.Password);
            if (sessionId != 0)
                MessageBox.Show("The user was successfully logged in!");
            else
                MessageBox.Show("Incorrect login or password!");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (sessionId != 0)
                client.LogOut(sessionId);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (sessionId != 0)
                client.LogOut(sessionId);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //if(usersListBox.SelectedValue != null)
            {
                //string loginTo = (string)usersListBox.SelectedValue;

                // Послать ссобщение пользователю по его логину
                client.SendMessage(sessionId, userLoginTextBox.Text, messageTextBox.Text);
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            // Получить список сообщений текущего клиента
            var messages = client.GetMessagesList(sessionId);
            messagesDataGrid.ItemsSource = messages.ToList();
            messagesDataGrid.Columns[0].Visibility = Visibility.Hidden;
        }
    }
}
