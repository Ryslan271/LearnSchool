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

namespace LearnSchool
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Свойства
        public static MainWindow Instance { get; set; } // Маин окно
        public static User User { get; set; } // авторизованный пользователь
        #endregion

        public MainWindow()
        {
            //_ = DBConnect.db.User.Local;

            InitializeComponent();
            Instance = this;
            MyFrame.Navigate(new Pages.LoginPage()); // открытие окна входа
        }

        #region Обработчики

        #region Кнопка назад
        private void BackBtn_Click(object sender, RoutedEventArgs e) 
        {
            if (MyFrame.CanGoBack == false) 
            {
                User = null;
                return;
            }

            MyFrame.GoBack();
        }
        #endregion

        #region Выход в меню регистрации
        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            User = null;
            MyFrame.Navigate(new Pages.LoginPage());
        }
        #endregion

        #endregion
    }
}
