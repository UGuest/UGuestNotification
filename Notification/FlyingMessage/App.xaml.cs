using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using ILuffy.IOP;
using ILuffy.IOP.Ioc;
using ILuffy.IOP.Logger.Impl;
using ILuffy.UGuest.Notification.View;
using ILuffy.UGuest.Notification.ViewModel;

namespace ILuffy.UGuest.Notification
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            // This code is used to test the app when using other cultures.
            //
            //System.Threading.Thread.CurrentThread.CurrentCulture =
            //    System.Threading.Thread.CurrentThread.CurrentUICulture =
            //        new System.Globalization.CultureInfo("it-IT");


            // Ensure the current culture passed into bindings is the OS culture.
            // By default, WPF uses en-US as the culture, regardless of the system settings.
            //
            FrameworkElement.LanguageProperty.OverrideMetadata(
              typeof(FrameworkElement),
              new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LoggerUtility.Logger = new Log4NetImpl();

            new IocContainerInstaller().InstallFromConfiguration();

            var mainWindow = new MainWindow();
            var mainWindowVM = new MainWindowViewModel();

            // When the ViewModel asks to be closed, 
            // close the window.
            EventHandler handler = null;
            handler = delegate
            {
                mainWindowVM.RequestClose -= handler;
                mainWindow.Close();
            };
            mainWindowVM.RequestClose += handler;

            mainWindow.DataContext = mainWindowVM;

            mainWindow.Show();
        }
    }
}
