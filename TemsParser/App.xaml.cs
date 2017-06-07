using System;
using System.Windows;
using System.Threading;
using System.Globalization;

using TemsParser.ViewModels;
using TemsParser.Views;
using TemsParser.Models.Settings;
using TemsParser.Models.Config;
using TemsParser.IO;
using TemsParser.Messages;
using TemsParser.Extentions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TemsParser.Extentions.Model.Config;

namespace TemsParser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex InstanceCheckMutex;

        private static bool InstanceCheck()
        {
            bool isNew;
            InstanceCheckMutex = new Mutex(true, "TemsParser_fdsgsh54ej5j6jfdvvl9i023r", out isNew);
            return isNew;
        }

        private void OnStartup(object sender, StartupEventArgs sea)
        {
            if (!InstanceCheck())
            {
                Alarms.ShowError("Ошибка запуска приложения", "Программа TemsParser уже запущена...");
                Environment.Exit(0);
            }

            // Main view.
            MainView mainView = new MainView();
            MainWindow = mainView;

            // Show SplashScreen.
            var splash = new SplashScreen(@"Icons\Startup.png");
            splash.Show(false);
            //Thread.Sleep(500);

            // Load config.
            ConfigModel configModel;

            try
            {
                configModel = FileReader.DeserialiseConfig();

                ICollection<ValidationResult> validationResults;

                if (configModel.Validate(out validationResults) == false)
                {
                    throw new Exception(String.Join("\n", validationResults));
                }
            }
            catch (Exception e)
            {
                Alarms.ShowError("Ошибка загрузки конфигурации", e.Message);
                configModel = new ConfigModel(true);
            }

            // Load settings.
            SettingsModel settingsModel;

            try
            {
                settingsModel = FileReader.DeserialiseSettings();
            }
            catch (Exception e)
            {
                Alarms.ShowError("Ошибка загрузки настроек", e.Message);
                settingsModel = new SettingsModel();
            }

            // Show main view.
            MainViewModel mainViewModel = new MainViewModel(configModel, settingsModel);
            mainView.DataContext = mainViewModel;
            mainView.Show();

            // Close SplashScreen.
            splash.Close(TimeSpan.FromMilliseconds(100));

            // Save settings when exit from app.
            mainView.Closing += (s, cea) =>
                {
                    var exitView = new ExitView();
                    exitView.Owner = mainView;
                    var exitViewModel = new ExitViewModel(mainViewModel.Config, mainViewModel.Settings);
                    exitView.DataContext = exitViewModel;
                    exitViewModel.Finished += (exitSender, exitEventArgs) => exitView.Close();
                    exitView.ShowInTaskbar = false;
                    exitView.ShowDialog();
                };
        }
    }
}