

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DoNotHarm.Pages
{
    /// <summary>
    /// Логика взаимодействия для MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        private bool _sortAscendingForHistorisLogin;
        public MenuPage()
        {
            InitializeComponent();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var historyLogin = new DoNotHarm.HistoryLogin
            {
                DateTime = DateTime.Now,
                IpAddress = "255.255.255.255",
                UserId = App.CurrentUser.Id,
                StatusEnterId = 1
            };

            App.Entities.HistoryLogins.AddOrUpdate(historyLogin);
            App.Entities.SaveChanges();

            string secondName = App.CurrentUser.LastName;
            if (DateTime.Now.Hour >= 6 && DateTime.Now.Hour < 12)
            {
                HelloTB.Text = $"Доброе утро {secondName}";
            }
            else if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 16)
            {
                HelloTB.Text = $"Добрый день {secondName}";
            }
            else if (DateTime.Now.Hour >= 16 && DateTime.Now.Hour < 22)
            {
                HelloTB.Text = $"Добрый вечер {secondName}";
            }
            else
            {
                HelloTB.Text = $"Доброй ночи {secondName}";
            }
            var mainWindow = App.Current.MainWindow as MainWindow;
            mainWindow.TimerSessionReset();

            switch (App.CurrentUser.UserType.Id)
            {
                case 1: LoadPageAdmin(); break;
                case 2: LoadPageLaborant(); break;
                case 3: LoadPageAccountant(); break;
                case 4: LoadPageLaborantoryTester(); break;
            }
        }

        private void LoadPageLaborantoryTester()
        {
            throw new NotImplementedException();
        }

        private void LoadPageAccountant()
        {
            throw new NotImplementedException();
        }

        private void LoadPageLaborant()
        {
            MainContainerForAdmin.Visibility = System.Windows.Visibility.Collapsed;

            var userLaborant = App.CurrentUser;
            MainContainerForLaborant.Visibility = System.Windows.Visibility.Visible;


            var ms = new MemoryStream(App.CurrentUser.UserType.Image);
            var bitmapImage = new BitmapImage();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = ms;
            bitmapImage.EndInit();
            PhotoLabarantImage.Source = bitmapImage;

            InformationAboutUserTB.Text = "Имя:" + userLaborant.FirstName +
                "\n\nФамилия:" + userLaborant.LastName +
                "\n\nРоль:" + userLaborant.UserType.Name;
        }

        #region FakeAdminPage
        private void LoadPageAdmin()
        {
            MainContainerForAdmin.Visibility = System.Windows.Visibility.Visible;
            HistoryListView.Visibility = System.Windows.Visibility.Visible;
        }

        //private void ApplyButton_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    UploadHistory();
        //}

        //private void UploadHistory()
        //{
            
        //    var historyLogins = new List<DoNotHarm.HistoryLogin>();

        //    var usersQueryable = App.Entities.HistoryLogins.AsQueryable();

        //    if (!string.IsNullOrWhiteSpace(loginUser))
        //    {
        //        usersQueryable = usersQueryable
        //                            .Where(x => x.User.Login.Contains(loginUser));
        //    }

        //    historyLogins = usersQueryable.ToList();

        //    UpdateUsersForUserDataGrid(historyLogins);
        //}

        //private void UpdateUsersForUserDataGrid(List<HistoryLogin> historyLogins)
        //{
        //    HistoryDataGrid.ItemsSource = historyLogins;
        //    //HistoryListView.ItemsSource = historyLogins;
        //}

        //private void SortByDateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    var historyLogins = HistoryDataGrid.ItemsSource as List<DoNotHarm.HistoryLogin>;

        //    if (_sortAscendingForHistorisLogin)
        //    {
        //        historyLogins = historyLogins
        //            .OrderBy(x => x.DateTime)
        //            .ToList();
        //    }
        //    else
        //    {
        //        historyLogins = historyLogins
        //            .OrderByDescending(x => x.DateTime)
        //            .ToList();
        //    }

        //    _sortAscendingForHistorisLogin = !_sortAscendingForHistorisLogin;

        //    HistoryDataGrid.ItemsSource = historyLogins;
        //}

        #endregion

        private void AcceptWasteBTN_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.MainFrame.Navigate(new WastePage());
        }

        private void HideAllContainerForAdmin()
        {
            HistoryListView.Visibility = System.Windows.Visibility.Collapsed;
            UsersContainer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ShowUsersMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadingUserContainer();
        }

        private void LoadingUserContainer()
        {
            HideAllContainerForAdmin();
            UsersContainer.Visibility = System.Windows.Visibility.Visible;

            UploadUsers();
        }

        private void ShowHistoryMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadingHistoryContainer();
        }

        private void LoadingHistoryContainer()
        {
            HideAllContainerForAdmin();
            HistoryListView.Visibility = System.Windows.Visibility.Visible;

            UploadHistory();
            //UploadHistory();
        }

        private void EditButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem == null) return;

            var editUserWindow = new Windows.EditUserWindow((User)UsersDataGrid.SelectedItem);
            editUserWindow.ShowDialog();
            UploadUsers();

        }


        private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var editUserWindow = new Windows.EditUserWindow();
            editUserWindow.ShowDialog();
            UploadUsers();

        }

        private void UploadHistory()
        {
            var users = App.Entities.Users.ToList();

            HistoryListView.ItemsSource = users;
        }

        private void UploadUsers()
        {
            UsersDataGrid.ItemsSource = new ObservableCollection<User>(App.Entities.Users.ToList());
        }
    }
}
