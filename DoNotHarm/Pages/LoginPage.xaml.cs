using System;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DoNotHarm.Pages
{
    public partial class LoginPage : Page
    {
        private bool eyeFlag;
        private bool _isShowCaptcha;
        private string _textCaptcha;
        private string _userTextCaptcha;

        public LoginPage()
        {
            InitializeComponent();
        }

        private void EyeBTN_Click(object sender, RoutedEventArgs e)
        {
            if (eyeFlag == false)
            {
                PasswordPB.Visibility = Visibility.Hidden;
                PasswordTB.Visibility = Visibility.Visible;
                PasswordTB.Text = PasswordPB.Password;
                EyeBTN.Content = "🐸";
                eyeFlag = true;
                return;
            }
            PasswordPB.Visibility = Visibility.Visible;
            PasswordTB.Visibility = Visibility.Hidden;
            PasswordPB.Password = PasswordTB.Text;
            EyeBTN.Content = "👁";
            eyeFlag = false;
        }

        private void EnterBTN_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginTB.Text))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            if (eyeFlag)
            {
                if (string.IsNullOrWhiteSpace(PasswordTB.Text))
                {
                    MessageBox.Show("Заполните все поля");
                    return;
                }
            }
            if (!eyeFlag)
            {
                if (string.IsNullOrWhiteSpace(PasswordPB.Password))
                {
                    MessageBox.Show("Заполните все поля");
                    return;
                }
            }

            if (_isShowCaptcha)
            {
                MessageBox.Show("Не введена капча");
                return;
            }



            if (eyeFlag)
            {
                if (findUser(LoginTB.Text, PasswordTB.Text) is null)
                {
                    MessageBox.Show("Неверный логин или пароль");
                    WrongLogin();
                    ShowCaptcha();
                    return;
                }
                App.CurrentUser = findUser(LoginTB.Text, PasswordTB.Text);
            }
            if (!eyeFlag)
            {
                if (findUser(LoginTB.Text, PasswordPB.Password) is null)
                {
                    MessageBox.Show("Неверный логин или пароль");
                    WrongLogin();
                    ShowCaptcha();
                    return;
                }
                App.CurrentUser = findUser(LoginTB.Text, PasswordPB.Password);
            }

            //MainWindow.LoginUserIMGLink. = App.CurrentUser.UserType.Image
            var ms = new MemoryStream(App.CurrentUser.UserType.Image);

            var bitmapImage = new BitmapImage();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = ms;
            bitmapImage.EndInit();
            MainWindow.LoginUserIMGLink.Source = bitmapImage;


            App.MainFrame.Navigate(new MenuPage()
            {
                Title = $"Меню {App.CurrentUser.UserType.Name}"
            });
        }

        private void WrongLogin()
        {
            var user = App.Entities.Users.FirstOrDefault(x => x.Login == LoginTB.Text);

            if(user == null)
            {
                return;
            }

            var historyLogin = new DoNotHarm.HistoryLogin
            {
                DateTime = DateTime.Now,
                IpAddress = "255.255.255.255",
                UserId = user.Id,
                StatusEnterId = 2
            };

            App.Entities.HistoryLogins.AddOrUpdate(historyLogin);
            App.Entities.SaveChanges();
        }

        private void ShowCaptcha()
        {
            CaptchaTextBox.Text = "";
            _isShowCaptcha = true;
            GenerationCaptcha();
            ContainerCaptcha.Visibility = Visibility.Visible;
        }
        private void HideCaptcha()
        {
            _isShowCaptcha= false;
            ContainerCaptcha.Visibility = Visibility.Collapsed;
        }

        private User findUser(string login, string password)
        {
            return App.Entities.Users
                .FirstOrDefault(x => x.Login == login &&
                    x.Password == password);

        }

       

        private void GenerationCaptcha()
        {
            _textCaptcha = "";

            string symbols = "1234567890абвгд";
            var rnd = new Random();

            for (int i = 0; i < 4; i++)
            {
                // Получаем рандомный символ из списка symbols
                var index = rnd.Next(0, symbols.Length);
                _textCaptcha += symbols[index];
            }

            CapchaLetterOne.Content = _textCaptcha[0];
            CapchaLetterTwo.Content = _textCaptcha[1];
            CapchaLetterThree.Content = _textCaptcha[2];
            CapchaLetterFoure.Content = _textCaptcha[3];
        }

        private void CaptchaTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = e.Source as TextBox;
            _userTextCaptcha = textBox.Text;
        }

        private void CaptchaButton_Click(object sender, RoutedEventArgs e)
        {
            if(_userTextCaptcha == _textCaptcha)
            {
                HideCaptcha();
            }
            else
            {
                MessageBox.Show("Не верно введена капча");
                ShowCaptcha();
            }
        }
    }
}
