﻿using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Diary
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static SplashScreen SplashScreen;
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var metroWindow = Current.MainWindow as MetroWindow;
            metroWindow.ShowMessageAsync("Nieoczekiwany wyjątek", "Wystapił nieoczekiwany wyjątek: " + Environment.NewLine + e.Exception.Message);

            e.Handled = true;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SplashScreen = new SplashScreen("resources/splashimage.jpg");
            SplashScreen.Show(false);
        }
    }
}
