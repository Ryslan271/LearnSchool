using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

        public ObservableCollection<ServicePhoto> servicePhotos; // все фотографии из ServicePhoto
        
        private int numberListSliderPage = 0; // номер транички для слайдера фото

        #endregion

        #region Dependency Propertys

        public Service EditService
        {
            get { return (Service)GetValue(MyPropertyEditService); }
            set { SetValue(MyPropertyEditService, value); }
        }

        public static readonly DependencyProperty MyPropertyEditService =
            DependencyProperty.Register("EditService", typeof(Service), typeof(EditServicePage));

        public ObservableCollection<ServicePhoto> ServicePhotes
            {
                get { return (ObservableCollection<ServicePhoto>)GetValue(ServicePhotesProperty); }
                set { SetValue(ServicePhotesProperty, value); }
            }

            public static readonly DependencyProperty ServicePhotesProperty =
                DependencyProperty.Register("ServicePhotes", typeof(ObservableCollection<ServicePhoto>), typeof(EditServicePage));

        #endregion

        public EditServicePage(Service service)
        {
            EditService = service;
            servicePhotos = DBConnect.db.ServicePhoto.Local;

            InitializeComponent();
            UpdateSliderList(); // обновление слайдера
        }


        #region Методы

        #region Обновление слайдера картинок
        private void UpdateSliderList() 
            => ServicePhotes = new ObservableCollection<ServicePhoto> (servicePhotos.Skip(3 * numberListSliderPage).Take(3));
        #endregion

        #region Окно проверки уверенности
        private bool Ask() => MessageBox.Show("Вы действительно\nхотите сохранить изменения?", "Уведомление",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
        #endregion

        #endregion

        #region Обработчики

        #region Сохранение изменений
        private void SaveEditService_Click(object sender, RoutedEventArgs e)
        {
            if (title.Text.Trim() == "" || cost.Text.Trim() == "" || durationInSeconds.Text.Trim() == ""
                || description.Text.Trim() == "" || discount.Text.Trim() == "" 
                || Convert.ToInt32(durationInSeconds.Text) <= 14400)
            {
                MessageBox.Show("Че то опять не так, либо:\n" +
                                "* Поля пустые, заполни и потом сохраняй, ЧЕБУПЕЛЬКА\n" +
                                "* Ты че наделал? Длительность не может быть больше 4 часов");
                return;
            }

            if (ListServecesPage.Instance.Services.Cast<object>().Select(s => s as Service).Any(x => x.Title.Trim() == Title.Trim()))
            {
                MessageBox.Show("Такое название уже занято,\nвыбирай другое и втянись");
                return;
            }

            if (DBConnect.db.ChangeTracker.HasChanges() == false || Ask() == false)
                return;

            DBConnect.db.SaveChanges();
        }
        #endregion

        #region Добавление картинки
        private void AddImagePage_Click(object sender, RoutedEventArgs e)
        {
            ServicePhoto servicePhoto = new ServicePhoto();
            OpenFileDialog openFile = new OpenFileDialog()
            {
                Filter = "*.png|*.png|*.jpeg|*.jpeg|*.jpg|*.jpg",
            };
            if (openFile.ShowDialog().GetValueOrDefault())
            {
                servicePhoto.PhotoPath = File.ReadAllBytes(openFile.FileName);
                servicePhoto.ServiceID = EditService.ID;

                ServicePhotes.Add(servicePhoto);
                servicePhotos.Add(servicePhoto);
                
                EditService.MainImagePath = servicePhoto.PhotoPath;

                DBConnect.db.SaveChanges();

                UpdateSliderList();
            }
        }
        #endregion

        #region Кнопки управление слайдером картинок
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
        #endregion

        #region Заменя основного изображения на выбранное
        private void ButtonClickEditPhotoService(object sender, RoutedEventArgs e)
        {
            if (ListSliderPhoto.SelectedItem == null)
                return;

            ServicePhoto photo = ListSliderPhoto.SelectedItem as ServicePhoto;

            ServicePhoto servicePhoto = new ServicePhoto
                                        {
                                            ServiceID = EditService.ID,
                                            PhotoPath = EditService.MainImagePath
                                        };

            EditService.MainImagePath = photo.PhotoPath; 

            ServicePhotes.Remove(photo);
            servicePhotos.Remove(photo);

            if (servicePhoto.PhotoPath != null)
            {
                ServicePhotes.Add(servicePhoto);
                servicePhotos.Add(servicePhoto);
            }

            DBConnect.db.SaveChanges();

            ListServecesPage.Instance.Services.Refresh();
        }
        #endregion

        #endregion

    }
}
