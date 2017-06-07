using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel.DataAnnotations;

using TemsParser.CustomAttributes;
using TemsParser.Behaviors;
using TemsParser.Processing;
using TemsParser.Common;
using TemsParser.IO;


public enum MessageTypes
{
    OK,
    Error,
    Warning
}

namespace TemsParser.ViewModels
{
    public class MessageViewModel : ViewModelBase
    {
        public MessageViewModel(string title, string message, MessageTypes messageType, bool isExportable)
        {
            Title = title;
            Message = message;
            MessageType = messageType;
            IsExportable = isExportable;

            CancelCommand = new Command(ex => Close());

            SaveCommand = new AsyncCommand(
                async ex =>
                {
                    var text = String.Format("*******{0}*******\n\n{1}", Title, Message);
                    await FileDialog.SaveTxtAsync(text);
                },
                ce => IsExportable);

            Application.Current.Dispatcher.Invoke(() => this.Show(this));
        }

        public ICommand SaveCommand { get; private set; }

        public string Message { get; private set; }

        public bool IsExportable { get; private set; }

        public MessageTypes MessageType { get; private set; }

    }
}
