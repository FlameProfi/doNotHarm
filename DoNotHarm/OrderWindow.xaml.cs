using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DoNotHarm
{
    public partial class OrderWindow : Window
    {
        private Order _order;

        private List<User> Users = new List<User>();
        private List<Service> Services = new List<Service>();
        public OrderWindow(Order order)
        {
            InitializeComponent();
            _order = order;
            this.Loaded += OrderWindow_Loaded;
        }

        private void OrderWindow_Loaded(object sender, RoutedEventArgs e)
        {
            NameCase.Content = "Кейс №" + _order.Id;

            Users = _order.OrderUsers.Select(x => x.User).ToList();
            Services = _order.OrderServices.Select(x => x.Service).ToList();

            BarcodeTextBox.Text = _order.Barcode;

            RefreshUserOrder();
            RefreshServicesOrder();
        }



        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            var selectUserWindow = new Windows.SelectUserWindow();
            selectUserWindow.Closed += SelectUserWindow_Closed;
            selectUserWindow.ShowDialog();
        }


        private void SelectUserWindow_Closed(object sender, System.EventArgs e)
        {
            var selectUserWindow = (Windows.SelectUserWindow)sender;

            if (selectUserWindow.SelectUserId <= 0) return;

            Users.Add(App.Entities.Users.First(x => x.Id == selectUserWindow.SelectUserId));

            RefreshUserOrder();
        }

        private void RefreshUserOrder()
        {
            this.UsersStackPanel.Children.Clear();

            foreach (var user in Users)
            {
                var name = user.FirstName + " " + user.LastName;

                var stackPanel = new StackPanel()
                {
                    Orientation = Orientation.Horizontal
                };

                var textBox = new TextBox()
                {
                    IsReadOnly = true,
                    Text = name,
                };

                stackPanel.Children.Add(textBox);

                var btnRemove = new Button()
                {
                    Content = "-"
                };

                btnRemove.CommandParameter = user.Id;
                btnRemove.Click += BtnRemoveUser_Click;

                stackPanel.Children.Add(btnRemove);

                this.UsersStackPanel.Children.Add(stackPanel);
            }
        }

        private void BtnRemoveUser_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var userId = (int)btn.CommandParameter;

            var user = Users.First(x => x.Id == userId);
            Users.Remove(user);

            RefreshUserOrder();
        }

        private void SaveAndClose()
        {
            var orderUsers = App.Entities.OrderUsers
                .Where(x => x.OrderId == _order.Id)
                .Include(x => x.User)
                .Include(x => x.Order)
                .ToList();

            App.Entities.OrderUsers.RemoveRange(orderUsers);
            App.Entities.SaveChanges();

            foreach (var user in Users)
            {
                _order.OrderUsers.Add(new OrderUser
                {
                    OrderId = _order.Id,
                    UserId = user.Id,
                });
            }

            App.Entities.SaveChanges();

            var orderServices = App.Entities.OrderServices
                .Where(x => x.OrderId == _order.Id)
                .Include(x => x.Service)
                .Include(x => x.Order)
                .ToList();

            App.Entities.OrderServices.RemoveRange(orderServices);
            App.Entities.SaveChanges();

            foreach (var service in Services)
            {
                _order.OrderServices.Add(new  OrderService
                {
                    OrderId = _order.Id,
                    SerciceId = service.Id,
                });
            }

            App.Entities.SaveChanges();


            var orderDB = App.Entities.Orders.Find(_order.Id);

            orderDB.Barcode = BarcodeTextBox.Text;

            App.Entities.Orders.AddOrUpdate(orderDB);

            App.Entities.SaveChanges();

            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveAndClose();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                SaveAndClose();
            }
        }

        private void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var selectServiceWindow = new Windows.SelectServiceWindow();
            selectServiceWindow.Closed += SelectServiceWindow_Closed;
            selectServiceWindow.ShowDialog();
        }

        private void SelectServiceWindow_Closed(object sender, System.EventArgs e)
        {
            var selectServiceWindow = (Windows.SelectServiceWindow)sender;

            if (selectServiceWindow.SelectServiceId <= 0) return;

            Services.Add(App.Entities.Services.First(x => x.Id == selectServiceWindow.SelectServiceId));

            RefreshServicesOrder();
        }

        private void RefreshServicesOrder()
        {
            this.ServicesStackPanel.Children.Clear();

            foreach (var service in Services)
            {
                var name = service.Name;

                var stackPanel = new StackPanel()
                {
                    Orientation = Orientation.Horizontal
                };

                var textBox = new TextBox()
                {
                    IsReadOnly = true,
                    Text = name,
                };

                stackPanel.Children.Add(textBox);

                var btnRemove = new Button()
                {
                    Content = "-"
                };

                btnRemove.CommandParameter = service.Id;
                btnRemove.Click += BtnRemoveService_Click;

                stackPanel.Children.Add(btnRemove);

                this.ServicesStackPanel.Children.Add(stackPanel);
            }
        }

        private void BtnRemoveService_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var serviceId = (int)btn.CommandParameter;

            var service = Services.First(x => x.Id == serviceId);
            Services.Remove(service);

            RefreshServicesOrder();
        }
    }
}
