using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DoNotHarm.Pages
{
    /// <summary>
    /// Логика взаимодействия для WastePage.xaml
    /// </summary>
    public partial class WastePage : Page
    {
        public WastePage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var orders = App.Entities.Orders.ToList();

            OrdersListViev.ItemsSource = orders;
        }

        private void AcceptBTN_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                StackPanel stackPanel = button.Parent as StackPanel;

                var order = (Order)stackPanel.DataContext;

                var asd = 0;

                var orderWindow = new OrderWindow(order);

                orderWindow.ShowDialog();
            }
        }
    }
}
