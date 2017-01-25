using MainViewApplication.Presenter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MainViewApplication
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            FirstViewPresenter presenter = new FirstViewPresenter();
            Current.MainWindow = new Window();
            Current.MainWindow.Content = presenter.GetView();
            Current.MainWindow.Show();
        }
    }

}
