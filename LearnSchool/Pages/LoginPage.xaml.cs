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

namespace LearnSchool.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        private void Entrance_Click(object sender, RoutedEventArgs e)
        {
            User user = DBConnect.db.User.Local.FirstOrDefault(x => x.Login == LoginTb.Text.Trim() && x.Password == PasswordTb.Text.Trim());

            if (user == null)
            {
                MessageBox.Show("Стенись, че то не то");
                return;
            }

            MainWindow.User = user; // авторизованный пользователь

            MainWindow.Instance.MyFrame.Navigate(new ListServecesPage()); // главная страничка
        }

        private void Registration_Click(object sender, RoutedEventArgs e) => MainWindow.Instance.MyFrame.Navigate(new RegistPage());
    }
}
