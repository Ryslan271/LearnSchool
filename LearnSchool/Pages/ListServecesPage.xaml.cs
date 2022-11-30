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

        public ICollectionView Services { get; set; } // все сервисы

        #endregion

        public ListServecesPage()
        {
            Services = new CollectionViewSource { Source = DBConnect.db.Service.Local.Where(x => x.IsDelete != true) }.View;

            InitializeComponent();

            FilterAndSearch(); // метод для фильтрации сервисов
            Sorted(); // метод для сортировки сервисов
        }

        #region Методы

            #region Метод для фильтрации и поиска
            private void FilterAndSearch()
            {
                NameDisSearchTb.TextChanged += (s, e) => Services.Refresh();

                Services.Filter += (obj) =>
                {
                    var service = obj as Service;

                    var search = NameDisSearchTb.Text;

                    if (service.Title.Contains(search) || service.Description.Contains(search))
                        return true;

                    return false;
                };
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

        #endregion

        #region Обработчики
        private void EditService_Click(object sender, RoutedEventArgs e)
        {
            var selService = (sender as Button).DataContext as Service;
            MainWindow.Instance.MyFrame.Navigate(new EditServicePage(selService));
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            Service service = ServiceList.SelectedItem as Service;
            service.IsDelete = true;
            DBConnect.db.SaveChanges();
            Services.Refresh();
        }

        private void AddServiceBtn_Click(object sender, RoutedEventArgs e) 
            => MainWindow.Instance.MyFrame.Navigate(new EditServicePage(new Service()));
        #endregion
    }
}
