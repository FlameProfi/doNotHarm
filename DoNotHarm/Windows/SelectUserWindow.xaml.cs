using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DoNotHarm.Windows
{
    /// <summary>
    /// Логика взаимодействия для SelectUserWindow.xaml
    /// </summary>
    public partial class SelectUserWindow : Window
    {
        public int SelectUserId { get; set; }

        public SelectUserWindow()
        {
            InitializeComponent();
            Loaded += SelectUserWindow_Loaded;
        }

        private void SelectUserWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UsersComboBox.DisplayMemberPath = "FirstName";
            UsersComboBox.SelectedValuePath = "Id";
            UsersComboBox.ItemsSource = new ObservableCollection<User>(App.Entities.Users.ToList());

            if (UsersComboBox.SelectedItem == null) return;

            SelectUserId = int.Parse(UsersComboBox.SelectedItem.ToString());
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectUserId <= 0) return;
            Close();
        }



        private void UsersComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var user = (User)UsersComboBox.SelectedItem;

            SelectUserId = user.Id;
        }
    }
}
