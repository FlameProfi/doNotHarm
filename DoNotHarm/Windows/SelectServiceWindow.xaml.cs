using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace DoNotHarm.Windows
{
    /// <summary>
    /// Логика взаимодействия для SelectUserWindow.xaml
    /// </summary>
    public partial class SelectServiceWindow : Window
    {
        public int SelectServiceId { get; set; }

        public SelectServiceWindow()
        {
            InitializeComponent();
            Loaded += SelectServiceWindow_Loaded;
        }

        private void SelectServiceWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ServicesComboBox.DisplayMemberPath = "Name";
            ServicesComboBox.SelectedValuePath = "Id";
            ServicesComboBox.ItemsSource = new ObservableCollection<Service>(App.Entities.Services.ToList());

            if (ServicesComboBox.SelectedItem == null) return;

            SelectServiceId = int.Parse(ServicesComboBox.SelectedItem.ToString());
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectServiceId <= 0) return;
            Close();
        }



        private void UsersComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var service = (Service)ServicesComboBox.SelectedItem;

            SelectServiceId = service.Id;
        }
    }
}
