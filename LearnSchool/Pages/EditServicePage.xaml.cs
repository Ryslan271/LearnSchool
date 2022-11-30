using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для EditServicePage.xaml
    /// </summary>
    public partial class EditServicePage : Page
    {
        #region Свойства
            public Service EditService { get; set; }

            public ObservableCollection<ServicePhoto> servicePhotos; // все фотографии из ServicePhoto
        
            private int numberListSliderPage = 0; // номер транички для слайдера фото

            #region Dependency Propertys
            public ObservableCollection<ServicePhoto> ServicePhotes
            {
                get { return (ObservableCollection<ServicePhoto>)GetValue(ServicePhotesProperty); }
                set { SetValue(ServicePhotesProperty, value); }
            }

            public static readonly DependencyProperty ServicePhotesProperty =
                DependencyProperty.Register("ServicePhotes", typeof(ObservableCollection<ServicePhoto>), typeof(EditServicePage));
            #endregion

        #endregion

        public EditServicePage(Service service)
        {
            EditService = service;
            servicePhotos = DBConnect.db.ServicePhoto.Local;

            InitializeComponent();
            UpdateSliderList();
        }

        private void SaveEditService_Click(object sender, RoutedEventArgs e)
        {
            if (title.Text.Trim() == "" || cost.Text.Trim() == "" || durationInSeconds.Text.Trim() == ""
                || description.Text.Trim() == "" || discount.Text.Trim() == "")
            {
                MessageBox.Show("Поля пустые, заполни и потом сохраняй, ЧЕБУПЕЛЬКА");
                return;
            }

            if (DBConnect.db.ChangeTracker.HasChanges() == false || Ask() == false)
                return;

            DBConnect.db.SaveChanges();
        }

        #region Методы
            
            private void UpdateSliderList() 
                => ServicePhotes = new ObservableCollection<ServicePhoto> (servicePhotos.Skip(3 * numberListSliderPage).Take(3));

        #endregion

        #region Обработчики
            private void AddImagePage_Click(object sender, RoutedEventArgs e)
            {
                
            }

            private void ButtonClickLeftListSlider(object sender, RoutedEventArgs e)
            {
                if (numberListSliderPage - 1 < 0)
                    return;

                numberListSliderPage--;
                UpdateSliderList();
            }

            private void ButtonClickRigthListSlider(object sender, RoutedEventArgs e)
            {
                if (ListSliderPhoto.Items.Count < 3)
                    return;
                numberListSliderPage++;
                UpdateSliderList();
            }

            private void ButtonClickEditPhotoService(object sender, RoutedEventArgs e)
            {
                if (ListSliderPhoto.SelectedItem == null)
                    return;

                ServicePhoto photo = ListSliderPhoto.SelectedItem as ServicePhoto;

                DBConnect.db.ServicePhoto.Add(new ServicePhoto
                {
                    ServiceID = EditService.ID,
                    PhotoPath = EditService.MainImagePath
                });

                EditService.MainImagePath = photo.PhotoPath;
                ServiceImage.DataContext = photo.PhotoPath;
                ServicePhotes.Remove(photo);
                DBConnect.db.SaveChanges();
            }
        #endregion

        private bool Ask() => MessageBox.Show("Вы действительно\nхотите сохранить изменения?", "Уведомление",
            MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
    }
}
