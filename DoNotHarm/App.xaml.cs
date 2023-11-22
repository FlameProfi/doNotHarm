using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DoNotHarm
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Frame MainFrame { get; set; }
        public static DoNotHarmEntities Entities { get; set; }
        public static User CurrentUser { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                Entities = new DoNotHarmEntities();
                Entities.Database.Connection.Open();
                Entities.Users.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка подключения к бд",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }
            base.OnStartup(e);
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString(), "Ошибка!",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
