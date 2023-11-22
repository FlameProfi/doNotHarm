using DoNotHarm.Pages;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace DoNotHarm
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Image LoginUserIMGLink { get; set; }
        private DispatcherTimer _timerSession;
        private DispatcherTimer _timerUnban;
        private int _shoutDownSession;
        private int _shoutDownUnban;

        public MainWindow()
        {
            InitializeComponent();
            LoginUserIMGLink = PhotoUserImage;
            ExitBTN.Visibility = Visibility.Hidden;
        }

        public void TimerSessionReset()
        {
            _shoutDownSession = 120;
            //_shoutDownSession = TimeSpan.FromMinutes(2).Seconds;
            _timerSession = new DispatcherTimer();
            _timerSession.Interval = TimeSpan.FromSeconds(1);
            _timerSession.Tick += TimerSession_Tick;
            _timerSession.Start();
        }

        public void TimerUnbanReset()
        {
            _shoutDownUnban = 60;
            _timerUnban = new DispatcherTimer();
            _timerUnban.Interval = TimeSpan.FromSeconds(1);
            _timerUnban.Tick += TimerUnban_Tick;
            _timerUnban.Start();
        }
        private void TimerSession_Tick(object sender, EventArgs e)
        {
            _shoutDownSession--;
            TitleTB.Text = _shoutDownSession.ToString();
            if (_shoutDownSession == 60)
                MessageBox.Show("Блокировка через минуту");
            else if (_shoutDownSession == 0)
            {
                PhotoUserImage = null;
                App.MainFrame.Navigate(new LoginPage());
                this.IsEnabled = false;
                TimerUnbanReset();
            }
        }


        private void TimerUnban_Tick(object sender, EventArgs e)
        {
            _shoutDownUnban--;
            TitleTB.Text = _shoutDownUnban.ToString();
            if (_shoutDownUnban <= 0)
                this.IsEnabled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            App.MainFrame = this.MainFrame;
            App.MainFrame.Navigate(new LoginPage());
        }

        private void ExitBTN_Click(object sender, RoutedEventArgs e)
        {
            PhotoUserImage.Source = null;
            App.MainFrame.Navigate(new Pages.LoginPage());
        }

        private void MainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            ExitBTN.Visibility = e.Content is LoginPage
                ? Visibility.Collapsed
                : Visibility.Visible;
            Title = $"Не навреди - {(e.Content as Page).Title}";
            TitleTB.Text = Title;
        }
    }
}
