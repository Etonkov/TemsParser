using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel.DataAnnotations;
using TemsParser.Common;
using TemsParser.CustomAttributes;
using TemsParser.Models.Config;
using System.IO;
using System.Windows;
using TemsParser.ViewModels;
using TemsParser.Resources;
using System.Windows.Input;

namespace TemsParser.Models.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public class FilePathViewModel : ViewModelBase, IValidationItem
    {
        private readonly Object LockObj = new Object();
        private bool _isExist;

        public FilePathViewModel(string path)
        {
            Path = path;
            IsExist = true;

            Task.Run(() =>
                {
                    IsExist = File.Exists(path);
                });
        }


        public string Path { get; private set; }

        [EqualsTrue(ErrorMessageResourceType = typeof(ErrorMessages),
            ErrorMessageResourceName = "FileNotFound")]
        public bool IsExist
        {
            get { return _isExist; }
            set
            {
                lock (LockObj)
                {
                    if (_isExist != value)
                    {
                        _isExist = value;
                        OnPropertyChanged();
                        CommandManager.InvalidateRequerySuggested();
                    }
                }
            }
        }


        public override string ToString()
        {
            return Path;
        }
    }
}
