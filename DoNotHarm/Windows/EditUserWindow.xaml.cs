using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;

namespace DoNotHarm.Windows
{
    public partial class EditUserWindow : Window
    {
        private User _user;

        public EditUserWindow(User user = null)
        {
            _user = user ?? new User();

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FirstNameTextBox.Text = _user.FirstName;
            LastNameTextBox.Text = _user.LastName;

            var userTypes = App.Entities.UserTypes.ToList();

            TypeUserComboBox.ItemsSource = userTypes;

            if (_user.TypeId > 0)
            {
                TypeUserComboBox.SelectedIndex = userTypes.IndexOf(userTypes.First(x => x.Id == _user.TypeId));
            }

            IPTextBox.Text = _user.Ip;
            LoginTextBox.Text = _user.Login;
            PasswordPasswordBox.Text = _user.Password;
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _user.FirstName = FirstNameTextBox.Text;
            _user.LastName = LastNameTextBox.Text;
            _user.Ip = IPTextBox.Text;
            _user.Login = LoginTextBox.Text;
            _user.Password = PasswordPasswordBox.Text;
            _user.TypeId = ((UserType)TypeUserComboBox.SelectedItem).Id;

            if(_user.Services == null || _user.Services.Length == 0)
            {
                _user.Services = "";
            }
            

            App.Entities.Users.AddOrUpdate(_user);
            App.Entities.SaveChanges();

            Close();
        }
    }
}
