using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Логика взаимодействия для ListServecesPage.xaml
    /// </summary>
    public partial class ListServecesPage : Page
    {
        #region Свойства

        static public ListServecesPage Instance { get; set; }
        public ICollectionView Services { get; set; } // все сервисы

        #endregion

        public ListServecesPage()
        {
            Services = new CollectionViewSource { Source = DBConnect.db.Service.Local.Where(x => x.IsDelete != true) }.View;

            InitializeComponent();

            NumberServise.Text = Services.Cast<object>().Count().ToString();

            Instance = this;

            FilterAndSearch(); // метод для фильтрации сервисов
            Sorted(); // метод для сортировки сервисов
        }

        #region Методы

        #region Метод для фильтрации и поиска
        private void FilterAndSearch()
        {
            #region Поиск

            NameDisSearchTb.TextChanged += (s, e) => Services.Refresh();

            Services.Filter += (obj) =>
            {
                var service = obj as Service;

                var search = NameDisSearchTb.Text;

                if (service.Title.Contains(search) || service.Description.Contains(search))
                    return true;

                NumberServise.Text = Services.Cast<object>().Count().ToString();

                return false;
            };
            #endregion

            #region Фильтрация

            FilterServices.SelectionChanged += (s, e) => Services.Refresh();

            Services.Filter += (obj) =>
            {
                var service = obj as Service;

                var tag = (FilterServices.SelectedItem as ComboBoxItem).Tag;

                switch (tag)
                {
                    case "All":
                        NumberServise.Text = Services.Cast<object>().Count().ToString();
                        return true;

                    case "ZeroBeforeFive":
                        if (service.Discount >= 0 && service.Discount <= 5)
                            return true;
                        break;

                    case "FiveBeforeFifteen":
                        if (service.Discount >= 5 && service.Discount <= 15)
                            return true;
                        break;

                    case "FifteenBeforeThirty":
                        if (service.Discount >= 15 && service.Discount <= 30)
                            return true;
                        break;

                    case "ThirtyBeforeSeventy":
                        if (service.Discount >= 30 && service.Discount <= 70)
                            return true;
                        break;

                    case "SeventyBeforeOneHundred":
                        if (service.Discount >= 70 && service.Discount <= 100)
                            return true;
                        break;
                }

                NumberServise.Text = Services.Cast<object>().Count().ToString();

                return false;
            };
            #endregion
        }
        #endregion

        #region Метод сортировки сервисов
        private void Sorted()
        {
            SortCb.SelectionChanged += (s, e) =>
            {
                var tag = (SortCb.SelectedItem as ComboBoxItem).Tag;

                switch (tag)
                {
                    case "Descending":
                        Services.SortDescriptions.Clear();
                        Services.SortDescriptions.Add(new SortDescription()
                        {
                            PropertyName = "Cost",
                            Direction = ListSortDirection.Ascending,
                        });
                        break;

                    case "Ascending":
                        Services.SortDescriptions.Clear();
                        Services.SortDescriptions.Add(new SortDescription()
                        {
                            PropertyName = "Cost",
                            Direction = ListSortDirection.Descending,
                        });
                        break;

                    case "Any":
                        Services.SortDescriptions.Clear();
                        break;
                }
            };
        }
        #endregion

        #region Метод для вывода окна взаимодействия
        private bool Ask() => MessageBox.Show("Вы действительно\nхотите удалить запись?", "Уведомление",
           MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
        #endregion

        #endregion

        #region Обработчики
        private void EditService_Click(object sender, RoutedEventArgs e)
        {
            var selService = (sender as Button).DataContext as Service;
            MainWindow.Instance.MyFrame.Navigate(new EditServicePage(selService));
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Ask() != true)
                return;

            Service service = ServiceList.SelectedItem as Service;
            service.IsDelete = true;
            DBConnect.db.SaveChanges();
            Services.Refresh();
            NumberServise.Text = Services.Cast<object>().Count().ToString();
        }

        private void AddServiceBtn_Click(object sender, RoutedEventArgs e) 
            => MainWindow.Instance.MyFrame.Navigate(new EditServicePage(new Service()));
        #endregion
    }
}
