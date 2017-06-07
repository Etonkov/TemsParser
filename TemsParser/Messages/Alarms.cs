using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using TemsParser.ViewModels;


namespace TemsParser.Messages
{
    public class Alarms
    {
        private Alarms()
        {

        }


        public static void ShowError(string title, string message, bool isExportable = false)
        {
            new MessageViewModel(title, message, MessageTypes.Error, isExportable);
        }

        public static void ShowInfo(string title, string message)
        {
            new MessageViewModel(title, message, MessageTypes.OK, isExportable: false);
        }

        public static void ShowError(string title, string header, IEnumerable<object> messages, bool isExportable = true)
        {
            ShowError(title, String.Join("\n\n", header, String.Join("\n\n", messages)), isExportable);
        }

        //public static void ShowError(string title, IEnumerable<ValidationResult> messages, bool isExportable = true)
        //{
        //    var stringMassages = messages.Select(o => o.ErrorMessage);

        //    ShowError(title, String.Join("\n\n", stringMassages), isExportable);
        //}

        public static void ShowWarning(string title, string message, bool isExportable = false)
        {
            new MessageViewModel(title, message, MessageTypes.Warning, isExportable);
        }

        public static void ShowWarning(string title,
                                       string header,
                                       IEnumerable<string> messages,
                                       bool isExportable = true)
        {
            ShowWarning(title, String.Join("\n\n", header, String.Join("\n\n", messages)), isExportable);
        }

        public static bool ShowQuestion(string title, string message)
        {
            var mbResult =  MessageBox.Show(message,
                                            title,
                                            MessageBoxButton.OKCancel,
                                            MessageBoxImage.Warning);
            
            if (mbResult == MessageBoxResult.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
