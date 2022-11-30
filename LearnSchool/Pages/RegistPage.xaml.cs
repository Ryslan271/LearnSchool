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
    /// Логика взаимодействия для RegistPage.xaml
    /// </summary>
    public partial class RegistPage : Page
    {
        public RegistPage()
        {
            InitializeComponent();
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            if (LoginTb.Text.Length == 0 || PasswordTb.Text.Length == 0)
            {
                MessageBox.Show("Заполни поля, ЧЕБУПЕЛЬКА");
                return;
            }

            if (DBConnect.db.User.Local.Any(x => x.Login == LoginTb.Text.Trim()))
            {
                MessageBox.Show("Такой пользователь уже есть, Чепух");
                return;
            }

            DBConnect.db.User.Local.Add(new User 
                {
                   Login = LoginTb.Text.Trim(),
                   Password = PasswordTb.Text.Trim(),
                   RoleId = 2
                }
            );

            MainWindow.Instance.MyFrame.Navigate(new ListServecesPage()); // главная страничка
        }
    }
}
