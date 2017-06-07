using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel;
using System.Windows;
using System.IO;
using System.Threading.Tasks;

using TemsParser.Models.Settings;
using TemsParser.Models.Config;
using TemsParser.Behaviors;
using TemsParser.Extentions;
using TemsParser.CustomAttributes;
using TemsParser.Common;
using TemsParser.IO;
using System.Collections.ObjectModel;
using TemsParser.Resources;
using TemsParser.ViewModels.Config;
using TemsParser.Messages;
using System.Threading;
using System.Diagnostics;

namespace TemsParser.ViewModels
{
    public class ExitViewModel : ViewModelBase, IValidationItem
    {
        public event EventHandler Finished;

        public ExitViewModel(ConfigModel config, SettingsModel settings)
        {
            Task.Run(() =>
            {
                var timer = new Stopwatch();
                timer.Start();

                try
                {
                    FileWriter.SerializeConfig(config);
                }
                catch (Exception e)
                {
                    Alarms.ShowError("Ошибка сохранения файла конфигурации", e.ToString());
                }

                try
                {
                    FileWriter.SerializeSettings(settings);
                }
                catch (Exception e)
                {
                    Alarms.ShowError("Ошибка сохранения файла настроек", e.ToString());
                }

                timer.Stop();

                if (timer.ElapsedMilliseconds < 500)
                {
                    long timeToSleep = 500 - timer.ElapsedMilliseconds;
                    Thread.Sleep(TimeSpan.FromMilliseconds(timeToSleep));
                }

                var handler = Finished;

                if (handler != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        handler(this, new EventArgs());
                    });
                }
            });
        }
    }
}
